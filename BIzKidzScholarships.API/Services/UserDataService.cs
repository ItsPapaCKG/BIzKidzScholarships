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
using Amazon.SimpleEmail;

using Newtonsoft.Json;
using TaskStatus = BizKidzScholarships.Data.Enums.TaskStatus;
using Amazon.SimpleEmail.Model;
using System.Runtime.InteropServices;

namespace BizKidzScholarships.API.Services
{
    public class UserDataService : BaseCRUDService, IUserDataService
    {
        //private ICurrentUser _user;
        //private BizKidzDbContext _context;
        //private IMapper _mapper;

        protected Guid userId => _user.Id;
        protected UserRewardFactory rewardFactory { get; set; }

        private IHttpContextAccessor _httpContext { get; set; }

        private IAmazonSimpleEmailService _email { get; set; }

        private IConfiguration _config { get; set; }

        public UserDataService(ICurrentUser user, IMapper mapper, BizKidzDbContext context, IHttpClientFactory _fac, IHttpContextAccessor httpContext, IAmazonSimpleEmailService eml, IConfiguration config) : base(user, mapper, context, _fac)
        {
            rewardFactory = new UserRewardFactory(userId, context);
            _httpContext = httpContext;
            _email = eml;
        }

        public int GetUserAge(DateTimeOffset birthday)
        {
            var today = DateTimeOffset.UtcNow.Date;
            var dob = birthday.Date;

            var age = today.Year - dob.Year;

            if (dob > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        public async Task<UserPointsView?> GetUserPoints(Guid userId)
        {

            bool noUpdates = await _context.UserPoints.AllAsync(up => !up.IsNew && up.UserId == userId);
            int totalPoints = _context.UserPoints
                .Where(u => u.UserId == userId)
                .GroupBy(p => p.TaskId)
                .Select(t => t.OrderByDescending(up => up.AttemptNumber).First().Points)
                .Sum();

            int entriesCost = 100;
            var entriesConfigured = int.TryParse((await _context.Configuration.FirstOrDefaultAsync(c => c.Id == "EntriesCost"))?.Value, out entriesCost);
            
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

            int oldPointsTotal = _context.UserPoints
                .Where(p => p.UserId == userId)
                .GroupBy(p => p.TaskId)
                .Select(t => t.Where(up => !up.IsNew).OrderByDescending(up => up.AttemptNumber).First().Points)
                .Sum();

            var allNewPoints = await _context.UserPoints
                .Where(p => p.UserId == userId && p.IsNew)
                .ToArrayAsync();

            if (allNewPoints.Any())
            {
                var t = await _context.Database.BeginTransactionAsync();

                try
                {

                    _context.UserPoints.UpdateRange(allNewPoints);

                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    await t.RollbackAsync();
                    // FUTURE: Logging
                }

                await t.CommitAsync();
            }

            int oldEntries = oldPointsTotal / (entriesConfigured && entriesCost != 0 ? entriesCost : 100);

            var newView = new UserPointsView()
            {
                UserId = userId,
                Points = totalPoints,
                PreviousPoints = oldPointsTotal,
                Entries = totalEntries,
                PreviousEntries = oldEntries,
                Updated = DateTimeOffset.UtcNow
            };

            return newView;
        }

        public async Task<UserProfileDTO?> GetUserProfile(Guid userId)
        {
            var ent = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);

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

            var privacyId = await _context.SiteDocuments.Where(d => d.Type == ConsentType.PrivacyPolicy).OrderByDescending(d => d.Created).Select(d => d.Id).FirstOrDefaultAsync();
            var termsId = await _context.SiteDocuments.Where(d => d.Type == ConsentType.TermsOfService).OrderByDescending(d => d.Created).Select(d => d.Id).FirstOrDefaultAsync();
            var mediaId = await _context.SiteDocuments.Where(d => d.Type == ConsentType.MediaConsent).OrderByDescending(d => d.Created).Select(d => d.Id).FirstOrDefaultAsync();

            var consentList = new List<UserConsentDTO>() {
                new UserConsentDTO()
                {
                    DocumentId = privacyId,
                    ConsentType = ConsentType.PrivacyPolicy,
                    IsGranted = registration.PrivacyConsent
                },
                new UserConsentDTO()
                {
                    DocumentId = termsId,
                    ConsentType = ConsentType.TermsOfService,
                    IsGranted = registration.PrivacyConsent
                },
                new UserConsentDTO()
                {
                    DocumentId = mediaId,
                    ConsentType = ConsentType.TermsOfService,
                    IsGranted = registration.PrivacyConsent
                }
            };

            try
            {
                await AddUserConsent(userId, consentList);
            }
            catch (Exception ex)
            {
                // future: Logging
            }

            await CreditForRegistering(userId);

            return await SetUserProfile(userId, profile);
        }

        private async Task CreditForRegistering(Guid userId)
        {
            var t = await _context.Database.BeginTransactionAsync();

            try
            {
                await NewUserSubmission(4, userId, "");
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await t.RollbackAsync();
            }
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

            if (isRegister)
            {
                ent.Created = DateTimeOffset.UtcNow;
            }

            ent.Updated = DateTimeOffset.UtcNow;

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

            // TODO: Configurable S3 locations
            string folder = req.IsPrivate ? "uploads-private/" : "uploads/";
            string bucketName = (await _context.Configuration.FirstOrDefaultAsync(c => c.Id == GlobalConstants.S3Bucket))?.Value ?? string.Empty;
            // first, get a Presigned URL from Amazon
            try
            {
                using (AmazonS3Client client = new AmazonS3Client(RegionEndpoint.USEast2))
                {
                    string key = folder + Guid.NewGuid() + "." + req.extension;

                    CreatePresignedPostRequest presignedPostRequest = new CreatePresignedPostRequest();
                    presignedPostRequest.BucketName = bucketName;
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

        public async Task<NewHandshakeResponse> CreateNewHandshake(Guid userId, ActionType type)
        {
            var response = new NewHandshakeResponse();

            var request = new ActionRequest()
            {
                UserId = userId,
                ActionType = type,
                Status = RequestStatus.Pending,
                Expiration = DateTimeOffset.UtcNow.AddMinutes(5)
            };

            var t = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.ActionRequests.AddAsync(request);
                await _context.SaveChangesAsync();

                await t.CommitAsync();
            }
            catch (Exception ex)
            {
                await t.RollbackAsync();
                response.Success = false;
                response.Errors.Add(ex.Message);
            }

            response.RequestId = request.RequestId;

            return response;
        }

        public async Task<ResponseModel> NewPasswordReset(string email, string resetLink)
        {
            var response = new ResponseModel() { Success = false };

            try
            {
                var confirmation = await _email.SendEmailAsync(new Amazon.SimpleEmail.Model.SendEmailRequest()
                {
                    Source = "noreply@scholarships.bizkidzusa.org",
                    Destination = new Amazon.SimpleEmail.Model.Destination()
                    {
                        ToAddresses = new List<string>() { email }
                    },
                    Message = new Amazon.SimpleEmail.Model.Message()
                    {
                        Subject = new Content() { Charset="UTF-8", Data="Password Reset | Biz Kidz Scholarships" },
                        Body = new Body()
                        {
                            Html = new Content() { Charset = "UTF-8", Data = $"<p>Reset your email here: <a href='{resetLink}'>Password Reset Link</a></p>" },
                            Text = new Content() { Charset = "UTF-8", Data = $"Reset your email here: {resetLink}" }
                        }
                    }
                });

                response.Success = !string.IsNullOrEmpty(confirmation.MessageId);
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public async Task<ResponseModel> StartPasswordReset(string email)
        {
            //var userId = await _context.Profiles.Where(p => p.Email == email).Select(p => p.UserId).FirstOrDefaultAsync();

            //if (userId == Guid.Empty)
            //{
            //    return new ResponseModel() { Success = false, Errors = { "Invalid users" } };
            //}

            //return await NewPasswordReset(userId, email);

            throw new NotImplementedException();
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
                    var bucket = "bizkidz-task-bucket";

                    using (var client = new AmazonS3Client(RegionEndpoint.USEast2))
                    {
                        try
                        {
                            var uri = new Uri(payload.S3Link);
                            var path = uri.AbsolutePath.TrimStart('/');

                            await client.GetObjectMetadataAsync(bucket, path);
                            fileUploaded = true;
                        } catch
                        {
                            fileUploaded = false;
                        }
                    }
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

            var previous = await _context.Submissions.Where(s => s.TaskId == taskid && s.UserId == userid).OrderByDescending(x => x.AttemptNumber).FirstOrDefaultAsync();

            if (previous != null) {
                attemptNumber = previous.AttemptNumber + 1;
            } else
            {
                attemptNumber = 1;
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

        public async Task<QuizQuestionViewModel[]> GetQuiz(int taskId)
        {
            QuizQuestionViewModel[] quiz = [];

            var taskQuestionIds = await _context.TaskQuestions.Where(tq => tq.TaskId == taskId).Select(tq => tq.QuestionId).ToListAsync();

            foreach (var questionId in taskQuestionIds)
            {
                var qvm = new QuizQuestionViewModel();

                var question = await _context.QuizQuestions.FirstOrDefaultAsync(qq => qq.QuestionId == questionId);

                if (question is null)
                {
                    throw new Exception($"Question Id {questionId} is invalid.");
                }

                // convert the A: AnswerText, B: AnswerText, C: .... etc into a Dictionary<string, string> for use on the frontend
                var optionsQueryable = from questionOption in _context.QuestionOptions
                                       join o in _context.QuizOptions on questionOption.OptionId equals o.OptionId
                                       join q in _context.QuizQuestions on questionOption.QuestionId equals q.QuestionId
                                       where q.QuestionId == questionId
                                       select new { o.OptionKey, o.OptionValue };

                var options = await optionsQueryable.ToDictionaryAsync(a => a.OptionKey, b => b.OptionValue);

                qvm.Multi = question.Multi;
                qvm.QuestionId = questionId;
                qvm.Prompt = question.Prompt;
                qvm.PromptImageKey = question.PromptImageKey;
                qvm.Options = options;

                // add the question to the quiz
                quiz.Append(qvm);
            }

            return quiz;
        }

        public async Task<ResponseModel> AddUserConsent(Guid userId, UserConsentDTO consent)
        {
            var consentEntity = new UserConsent()
            {
                ConsentTimeUtc = DateTime.UtcNow,
                DocumentId = consent.DocumentId,
                IsGranted = consent.IsGranted,
                UserId = userId,
                IPAddress = _httpContext.HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = _httpContext.HttpContext.Request.Headers.UserAgent.ToString(),
                ConsentType = consent.ConsentType
            };

            var t = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.UserConsents.AddAsync(consentEntity);

                await _context.SaveChangesAsync();

                await t.CommitAsync();

                return new ResponseModel() { Success = false, Errors = { "Failed to add user consent" } };
            }
            catch (Exception ex)
            {
                await t.RollbackAsync();

                return new ResponseModel() { Success = false, Errors = { "Failed to add user consent" } };
            }
        }

        public async Task<ResponseModel> AddUserConsent(Guid userId, List<UserConsentDTO> consent)
        {
            var r = new ResponseModel();

            try { 
                foreach (var c in consent)
                {
                    await AddUserConsent(userId, c);
                }
            } catch (Exception ex)
            {
                r.Errors.Add(ex.Message);
                r.Success = false;
            }


            return r;
        }

        public Task<ResponseModel> ResetPasswordRequest(Guid request, string newPassword)
        {
            throw new NotImplementedException();
        }

        public async Task<HTMLContentResponse> GetPrivacy()
        {
            return await GetDocument(ConsentType.PrivacyPolicy);
        }

        public async Task<HTMLContentResponse> GetDocument(ConsentType type)
        {
            var response = new HTMLContentResponse();

            var doc = await _context.SiteDocuments.Where(d => d.Type == type).OrderByDescending(d => d.Created).Select(d => d.ContentsHTML).FirstOrDefaultAsync();

            response.HTML = doc;

            return response;
        }
    }
}
