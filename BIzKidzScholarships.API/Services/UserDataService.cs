using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using AutoMapper;
using BizKidzScholarships.API.Services.Base;
using BizKidzScholarships.API.Services.Utilities;
using BizKidzScholarships.Data.Contexts;
using BizKidzScholarships.Data.dto;
using BizKidzScholarships.Data.Entities;
using BizKidzScholarships.Data.Enums;
using BizKidzScholarships.Data.Models;
using BizKidzScholarships.Data.NetworkedModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;
using TaskStatus = BizKidzScholarships.Data.Enums.TaskStatus;

namespace BizKidzScholarships.API.Services
{
    public class UserDataService : BaseCRUDService, IUserDataService
    {
        //private ICurrentUser _user;
        //private BizKidzDbContext _context;
        //private IMapper _mapper;

        protected Guid userId => _user.Id;
        protected UserRewardFactory rewardFactory { get; set; }

        public UserDataService(ICurrentUser user, IMapper mapper, BizKidzDbContext context, IHttpClientFactory _fac) : base(user, mapper, context, _fac)
        {
            rewardFactory = new UserRewardFactory(userId, context);
        }
        public UserPointsView? GetUserPoints(Guid userId)
        {
            bool noUpdates = _context.UserPoints.All(up => !up.IsNew && up.UserId == userId);
            int totalPoints = _context.UserPoints
                .Where(u => u.UserId == userId)
                .GroupBy(p => p.TaskId)
                .Select(t => t.OrderByDescending(up => up.AttemptNumber).First().Points)
                .Sum();

            int entriesCost;
            var entriesConfigured = int.TryParse(_context.Configuration.FirstOrDefault(c => c.Id == "EntriesCost")?.Value, out entriesCost);
            
            int totalEntries = totalPoints / (entriesConfigured && entriesCost != 0 ? entriesCost : 100); // TODO: Points per entry configuration value

            if (noUpdates)
            {
                var view = new UserPointsView()
                {
                    UserId = userId,
                    Points = totalPoints,
                    Entries = totalEntries,
                    Updated = DateTimeOffset.UtcNow
                };

                return view;
            }

            int oldPoints = _context.UserPoints
                .GroupBy(p => p.TaskId)
                .Select(t => t.Where(up => !up.IsNew).OrderByDescending(up => up.AttemptNumber).First().Points)
                .Sum();
            int oldEntries = oldPoints / 100;

            var newView = new UserPointsView()
            {
                UserId = userId,
                Points = totalPoints,
                PreviousPoints = oldPoints,
                Entries = totalEntries,
                PreviousEntries = oldEntries,
                Updated = DateTimeOffset.UtcNow
            };

            return newView;
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

        public async Task<ResponseModel> RegisterUserProfile(Guid userId, RegisterDTO registration)
        {
            var profile = _mapper.Map<UserProfileDTO>(registration);

            profile.ProfileComplete = false;

            return await SetUserProfile(userId, profile);
        }

        public async Task<ResponseModel> SetUserProfile(Guid userId, UpdateUserProfileDTO profile, bool isRegister = false)
        {
            var pr = _mapper.Map<UserProfileDTO>(profile);
            pr.UserId = userId;

            return await SetUserProfile(userId, pr, isRegister);
        }

        public async Task<ResponseModel> SetUserProfile(Guid userId,UserProfileDTO profile, bool isRegister = false)
        {
            ResponseModel response = new();

            if (string.IsNullOrEmpty(profile.BusinessLogoKey))
            {
                profile.BusinessLogoKey = _context.Configuration.FirstOrDefault(c => c.Id == "DefaultProfilePicture")?.Value;
            }

            if (isRegister && profile.FirstName is null || profile.LastName is null)
            {
                response.Success = false;
                response.Errors = new List<string> { "First and last name are required." };
                return response;
            }

            var exists = _context.Profiles.FirstOrDefault(p => p.UserId == userId);

            string firstName = exists is not null && !isRegister ? exists.FirstName : profile.FirstName!; 
            string lastName = exists is not null && !isRegister ? exists.LastName : profile.LastName!; 

            var ent = _mapper.Map<UserProfile>(profile);
            ent.FirstName = firstName;
            ent.LastName = lastName;

            ent.UserId = userId;

            var t = await _context.Database.BeginTransactionAsync();

            try
            {
                if (exists is null)
                    await _context.Profiles.AddAsync(ent);
                else {
                    exists.FirstName = firstName;
                    exists.LastName = lastName;

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
            

            return response;
        }

        public async Task<bool> UpdateUserProfile(UserProfileDTO profile)
        {
            var existing = _context.Profiles.Any(ut => ut.UserId == userId);

            if (!existing) return false;

            var ent = _mapper.Map<UserProfile>(profile);
            ent.ProfileComplete = true;

            var result = await SafeUpdateAsync(ent);

            return result;
        }

        public async Task<List<DashboardTaskDTO>> GetUserTasks(Guid userId)
        {
            //var tasks = await _context.UserTasks.Where(t => t.UserId == userId).ToListAsync();

            var tasksQueryable = from userTask in _context.UserTasks
                                 join t in _context.Tasks on userTask.TaskId equals t.Id
                                 where t.TaskEnabled && userTask.UserId == userId && userTask.Status != TaskStatus.Disabled && userTask.Status != TaskStatus.Hidden
                                 select new DashboardTaskDTO { TaskTitle = t.TaskTitle, Reward = t.Reward, Status = userTask.Status, TaskId = t.Id, TaskDescription = t.TaskDescription, TaskImageKey = t.TaskImageKey, TaskType = t.TaskType, TaskPromptSubtitle = t.TaskPromptSubtitle, TaskPromptTitle = t.TaskPromptTitle };

            var tasks = await tasksQueryable.ToListAsync();

            if (tasks is null)
                return [];

            return tasks;
        }

        public async Task<List<ResponseModel>> SetGlobalTasksForUsers(List<Guid> userIds)
        {
            var globalQueryable = from task in _context.Tasks
                              where task.TaskEnabled == true && task.IsGlobalTask == true
                              select new DashboardTaskDTO { TaskTitle = task.TaskTitle, Reward = task.Reward, Status = TaskStatus.Open, TaskId = task.Id, TaskDescription = task.TaskDescription, TaskImageKey = task.TaskImageKey, TaskType = task.TaskType };

            var userTasksQueryable = from userTask in _context.UserTasks
                                 select userTask.TaskId;

            var userTasks = await userTasksQueryable.ToListAsync();

            var globalTasks = await globalQueryable.ToListAsync();

            var responseList = new List<ResponseModel>();

            foreach (var userId in userIds)
            {
                var response = await UpdateUserWithGlobalTasks(userId, userTasks, globalTasks);

                responseList.Add(response);
            }

            return responseList;
        }

        public async Task<List<ResponseModel>> SetGlobalTasksAllUsers()
        {
            var userQueryable = _context.Users.Select(u => u.Id);

            var userIds = await userQueryable.ToListAsync();

            return await SetGlobalTasksForUsers(userIds);
        }

        public async Task<ResponseModel> SetGlobalTasksForUser(Guid userId)
        {
            var globalQueryable = from task in _context.Tasks
                                  where task.TaskEnabled == true && task.IsGlobalTask == true
                                  select new DashboardTaskDTO { TaskTitle = task.TaskTitle, Reward = task.Reward, Status = TaskStatus.Open, TaskId = task.Id, TaskDescription = task.TaskDescription, TaskImageKey = task.TaskImageKey, TaskType = task.TaskType };

            var userTasksQueryable = from userTask in _context.UserTasks
                                     where userTask.UserId == userId
                                     select userTask.TaskId;

            var userTasks = await userTasksQueryable.ToListAsync();

            var globalTasks = await globalQueryable.ToListAsync();

            var response = await UpdateUserWithGlobalTasks(userId, userTasks, globalTasks);

            return response;
        }

        private async Task<ResponseModel> UpdateUserWithGlobalTasks(Guid userId, List<int> userTaskIds, List<DashboardTaskDTO> globalTasks)
        {
            foreach (var task in globalTasks)
            {
                if (userTaskIds.Any(t => t == task.TaskId)) { continue; }

                var status = await QueueAssignTask(task.TaskId, userId);

                if (!status.Success)
                {
                    _context.ChangeTracker.Clear();

                    status.Errors.Prepend($"Could not update user {userId} to latest Global Tasks. See errors for details.");

                    return status;
                }
            }

            await _context.SaveChangesAsync();

            return new ResponseModel() { Success = true };
        }

        public async Task<ResponseModel> AssignTask(int taskid, Guid userid)
        {
            try
            {
                var userTask = new UserTask()
                {
                    TaskId = taskid,
                    UserId = userid,
                    Status = TaskStatus.Open
                };

                await _context.UserTasks.AddAsync(userTask);

                await _context.SaveChangesAsync();

            } catch (Exception e)
            {
                return new ResponseModel() { Success = false, Errors = { e.Message } };
            }

            return new ResponseModel() { Success = true };
        }

        private async Task<ResponseModel> QueueAssignTask(int taskid, Guid userid)
        {
            try
            {
                var userTask = new UserTask()
                {
                    TaskId = taskid,
                    UserId = userid,
                    Status = TaskStatus.Open
                };

                await _context.UserTasks.AddAsync(userTask);

            }
            catch (Exception e)
            {
                return new ResponseModel() { Success = false, Errors = { e.Message } };
            }

            return new ResponseModel() { Success = true };
        }

        public async Task<ResponseModel> ValidateUserProfilePicture(ActionRequest request, string imageLink)
        {
            var payloadObject = new { S3Link = imageLink };
            var payload = JsonConvert.SerializeObject(payloadObject);

            request.Payload = payload;
            request.Status = RequestStatus.Success;

            bool success = await SafeUpdateAsync(request);

            return new ResponseModel { Success = success };
        }

        public async Task<ResponseModel> StartUploadHandshake(PresignedRequestModel req)
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
            var payload = new
            {
                S3Link = $"{handshake.PresignedUrlPayload.Url}{handshake.PresignedUrlPayload.Key}",
                TaskId = req.TaskId
            };

            var request = new ActionRequest()
            {
                UserId = _user.Id,
                ActionType = req.ActionType,
                Status = RequestStatus.Pending,
                Created = DateTimeOffset.UtcNow,
                Updated = DateTimeOffset.UtcNow,
                Payload = JsonConvert.SerializeObject(payload),
                Expiration = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            var t = await _context.Database.BeginTransactionAsync();
            
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
                await t.CommitAsync();
            }
            catch (Exception ex)
            {
                await t.RollbackAsync();

                return new ResponseModel() { Success = false, Errors = { ex.Message } };
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
                if (request is null) {                     
                    return new ResponseModel() { Success = false, Errors = { $"Invalid Request Id: {confirmation.RequestId}" } };
                }

                if (request.Status == RequestStatus.Closed)
                {
                    return new ResponseModel() { Success = false, Errors = { $"Request is closed: {confirmation.RequestId}" } };
                }

                if (request.Status == RequestStatus.Success)
                {
                    return new ResponseModel() { Success = false, Errors = { $"Request is already confirmed, awaiting further action: {confirmation.RequestId}" } };
                }

                if (request.Expiration < DateTimeOffset.UtcNow)
                {
                    await SetRequestStatus(request, RequestStatus.Denied);
                    return new ResponseModel() { Success = false, Errors = { $"Expired Request: {confirmation.RequestId}" } };
                }


                bool fileUploaded = false;
                var payload = JsonConvert.DeserializeObject<UploadActionPayload>(request.Payload);

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
                    await SetRequestStatus(request, RequestStatus.Failed);
                    return new ResponseModel() { Success = false, Errors = { $"Request {confirmation.RequestId} failed validation." } };
                }

                // switch to check upload types
                switch (request.ActionType)
                {
                    case ActionType.ProfileImageUpload:
                        if (payload is null)
                        {
                            await SetRequestStatus(request, RequestStatus.Cancelled);
                            throw new Exception($"Payload must be specified for Profile Image Uploads. Cancelling request {request.RequestId}");
                        }

                        // update the profile picture
                        var res = await ValidateUserProfilePicture(request, payload.S3Link);

                        return res;
                    case ActionType.TaskUpload:
                        if (payload is null)
                        {
                            await SetRequestStatus(request, RequestStatus.Cancelled);
                            throw new Exception($"Payload must be specified for Uploads. Cancelling request {request.RequestId}");
                        }

                        // create task submission
                        string submissionPayload = JsonConvert.SerializeObject(new { S3Link = payload.S3Link });
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
            var submission = new TaskSubmission() { AttemptNumber = attemptNumber, SubmissionData = payload, TaskId = taskid, UserId = userid, Created = DateTimeOffset.UtcNow, Updated = DateTimeOffset.UtcNow };

            var t = await _context.Database.BeginTransactionAsync();

                try
                {
                    var userTask = _context.UserTasks.FirstOrDefault(ut => ut.TaskId == taskid && ut.UserId == userid);
                    UserPointsReward reward = rewardFactory.New(taskid);

                    if (userTask is null)
                    {
                        throw new Exception($"No user task found with Task Id {taskid} that belongs to User {userid}.");
                    }

                    userTask.Status = Data.Enums.TaskStatus.Completed;

                    await _context.Submissions.AddAsync(submission);
                    _context.UserTasks.Update(userTask);

                    _context.UserPoints.Add(reward);

                    await _context.SaveChangesAsync();
                    await t.CommitAsync();
                }
                catch (Exception e)
                {
                    await t.RollbackAsync();

                    return new ResponseModel { Success = false, Errors = { $"Could not submit submission for task {taskid} for user {userid}. " + e.Message } };
                }
            

            return new ResponseModel { Success = true };
        }

        private async Task SetRequestStatus(ActionRequest request, RequestStatus status)
        {
            try
            {
                _context.ActionRequests.Update(request);

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<ResponseModel> SubmitDataTask(int taskid, Guid userId, string payload)
        {
            var res = await NewUserSubmission(taskid, userId, payload);

            if (!res.Success)
            {
                return res;
            }

            throw new NotImplementedException();
        }

        private async Task<ResponseModel> ValidateQuizInput()
        {
            throw new NotImplementedException();
        }
    }
}
