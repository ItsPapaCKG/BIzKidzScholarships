using Amazon;
using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using AutoMapper;
using BizKidzScholarships.API.Services.Base;
using BizKidzScholarships.Data.Contexts;
using BizKidzScholarships.Data.dto;
using BizKidzScholarships.Data.Entities;
using BizKidzScholarships.Data.Enums;
using BizKidzScholarships.Data.Models;
using BizKidzScholarships.Data.NetworkedModels;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace BizKidzScholarships.API.Services
{
    public class UserDataService : BaseCRUDService, IUserDataService
    {
        //private ICurrentUser _user;
        //private BizKidzDbContext _context;
        //private IMapper _mapper;

        protected Guid userId => _user.Id;

        public UserDataService(ICurrentUser user, IMapper mapper, BizKidzDbContext context, IHttpClientFactory _fac) : base(user, mapper, context, _fac)
        {

        }
        public UserPointsView? GetUserPoints(Guid userId)
        {
            var ent = _context.UserPoints.FirstOrDefault(p => p.UserId == userId);

            if (ent == null)
                return null;

            var model = _mapper.Map<UserPointsView>(ent);

            return model;
        }

        public UserProfileDTO? GetUserProfile(Guid userId)
        {
            var ent = _context.Profiles.FirstOrDefault(p => p.UserId == userId);

            if (ent == null)
                return null;

            var model = _mapper.Map<UserProfileDTO>(ent);

            return model;
        }

        // TODO: Include step to upload byte[] image to S3, retrieve the link, and set to profile column LogoKey
        public async Task<ResponseModel> SetUserProfile(Guid userId,RegisterUserProfileDTO profile)
        {
            ResponseModel response = new();

            var exists = _context.Profiles.FirstOrDefault(p => p.UserId == userId);

            var ent = _mapper.Map<UserProfile>(profile);

            ent.UserId = userId;

            using (var t = _context.Database.BeginTransaction())
            {
                try
                {
                    if (exists is null)
                        await _context.Profiles.AddAsync(ent);
                    else {
                        _context.Entry(exists).State = EntityState.Detached;

                        _context.Profiles.Update(ent);
                    }

                    await _context.SaveChangesAsync();

                    await t.CommitAsync();
                }
                catch (Exception ex)
                {
                    t.Rollback();

                    var error = ex.Message;

                    response.Success = false;
                    response.Errors.Add(error);
                    return response;
                }
            }

            return response;
        }

        public async Task<bool> UpdateUserProfile(UserProfileDTO profile)
        {
            var existing = _context.Profiles.Any(ut => ut.UserId == userId);

            if (!existing) return false;

            var ent = _mapper.Map<UserProfile>(profile);

            var result = await SafeUpdateAsync(ent);

            return result;
        }

        public async Task<List<DashboardTaskDTO>> GetUserTasks(Guid userId)
        {
            //var tasks = await _context.UserTasks.Where(t => t.UserId == userId).ToListAsync();

            var tasksQueryable = from userTask in _context.UserTasks
                                 join t in _context.Tasks on userTask.TaskId equals t.Id
                                 where t.TaskEnabled && userTask.UserId == userId
                                 select new DashboardTaskDTO { TaskTitle = t.TaskTitle, Reward = t.Reward, Status = userTask.Status, TaskId = t.Id, TaskDescription = t.TaskDescription, TaskImageKey = t.TaskImageKey, TaskType = t.TaskType };

            var tasks = await tasksQueryable.ToListAsync();

            if (tasks is null)
                return [];

            return tasks;
        }

        public async Task<ResponseModel> UpdateUserProfilePicture(Guid userId, string imageKey)
        {
            var profile = GetUserProfile(userId);

            profile.BusinessLogoKey = imageKey;

            var ent = _mapper.Map<UserProfile>(profile);

            bool success = await SafeUpdateAsync(ent);

            return new ResponseModel { Success = success };
        }

        public ResponseModel StartUploadHandshake(PresignedRequestModel req)
        {
            PresignedHandshakeModel handshake = new PresignedHandshakeModel();

            // first, get a Presigned URL from Amazon
            try
            {
                using (AmazonS3Client client = new AmazonS3Client(RegionEndpoint.USEast2))
                {
                    string key = "uploads/" + Guid.NewGuid() + "." + req.extension;

                    CreatePresignedPostRequest presignedPostRequest = new CreatePresignedPostRequest();
                    presignedPostRequest.BucketName = "bizkidz-task-bucket";
                    presignedPostRequest.Key = key;
                    presignedPostRequest.Expires = DateTime.UtcNow.AddMinutes(10);

                    var response = client.CreatePresignedPost(presignedPostRequest);

                    handshake.PresignedUrlPayload = new PresignedPostURLDataModel() { Url = response.Url, Key = key, Fields = response.Fields };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel() { Success = false, Errors = { ex.Message } };
            }

            // Create the request in the DB to track actions needed after upload is confirmed
            var request = new ActionRequest()
            {
                UserId = _user.Id,
                ActionType = req.ActionType,
                Status = RequestStatus.Pending,
                Created = DateTimeOffset.UtcNow,
                Updated = DateTimeOffset.UtcNow,
                Expiration = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            using (var t = _context.Database.BeginTransaction())
            {
                try
                {
                    // Attach request Entity to DbSet
                    _context.ActionRequests.Add(request);

                    if (request.RequestId == Guid.Empty)
                        throw new Exception("Unable to generate Handshake Request ID.");

                    handshake.RequestId = request.RequestId;

                    // SaveChanges()
                    _context.SaveChanges();

                    // Commit()
                    t.Commit();
                }
                catch (Exception ex)
                {
                    t.Rollback();

                    return new ResponseModel() { Success = false, Errors = { ex.Message } };
                }
                finally
                {
                    t.Dispose();
                }
            }

            // send the handshake required data to the user
            return handshake;
        }

        public async Task<ResponseModel> UploadConfirmation(UploadHandshakeConfirmationModel confirmation)
        {
            try
            {
                var request = _context.ActionRequests.FirstOrDefault(r => r.RequestId == confirmation.RequestId);

                // check that request exists
                if (request is null)
                    return new ResponseModel() { Success = false, Errors = { $"Invalid Request Id: {confirmation.RequestId}" } };

                bool fileUploaded = false;
                var payload = request.Payload as dynamic;

                // try and access the s3 file
                if (payload is not null)
                {
                    var client = _httpClientFactory.CreateClient();
                    var uri = new Uri(payload.S3Link);

                    var response = await client.GetAsync(uri);
                    fileUploaded = response.IsSuccessStatusCode;
                }

                // if not successful, set status of request and return ResponseModel
                if (!fileUploaded && payload is not null)
                {
                    SetRequestStatus(request, RequestStatus.Failed);
                    return new ResponseModel() { Success = false, Errors = { $"Request {confirmation.RequestId} failed validation." } };
                }

                // switch to check upload types
                switch (request.ActionType)
                {
                    case ActionType.ProfileImageUpload:
                        if (payload is null)
                        {
                            SetRequestStatus(request, RequestStatus.Cancelled);
                            throw new Exception($"Payload must be specified for Profile Image Uploads. Cancelling request {request.RequestId}");
                        }

                        // update the profile picture
                        var res = UpdateUserProfilePicture(request.UserId, payload.S3Link);

                        return res;
                    case ActionType.TaskUpload:
                        if (payload is null)
                        {
                            SetRequestStatus(request, RequestStatus.Cancelled);
                            throw new Exception($"Payload must be specified for Profile Image Uploads. Cancelling request {request.RequestId}");
                        }

                        // create task submission
                        dynamic submissionPayload = new { S3Link = payload.S3Link };
                        int taskid = (int)payload.TaskId;
                        Guid userid = request.UserId;

                        return await NewUserSubmission(taskid, userid, submissionPayload);
                    default:
                        throw new Exception($"Could not determine the task type of request {request.RequestId}.");
                }
            }
            catch (Exception e)
            {
                return new ResponseModel() { Success = false, Errors = { e.Message } };
            }
        }

        private async Task<ResponseModel> NewUserSubmission(int taskid, Guid userid, string payload)
        {
            // get previous submissions

            int attemptNumber = 0;

            var previous = _context.Submissions.Where(s => s.TaskId == taskid && s.UserId == userid).OrderByDescending(x => x.AttemptNumber).FirstOrDefault();

            if (previous != null) {
                attemptNumber = previous.AttemptNumber + 1;
            }

            //

            var submission = new TaskSubmission() { AttemptNumber = attemptNumber, SubmissionData = payload, TaskId = taskid, UserId = userid };

            using (var t = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.Submissions.AddAsync(submission);

                    await _context.SaveChangesAsync();
                    await t.CommitAsync();
                }
                catch (Exception e)
                {
                    await t.RollbackAsync();

                    await t.DisposeAsync();

                    throw new Exception($"Could not submit submission for task {taskid} for user {userid}");
                }
            }

            return new ResponseModel { Success = true };
        }

        private async void SetRequestStatus(ActionRequest request, RequestStatus status)
        {
            using (var t = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.ActionRequests.Update(request);

                    await _context.SaveChangesAsync();

                    await t.CommitAsync();
                }
                catch (Exception ex)
                {
                    await t.RollbackAsync();

                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
