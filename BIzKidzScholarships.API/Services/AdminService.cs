using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BizKidzScholarships.API.Services.Base;
using BizKidzScholarships.API.Services.Utilities;
using BizKidzScholarships.Data.Contexts;
using BizKidzScholarships.Data.dto;
using BizKidzScholarships.Data.Entities;
using BizKidzScholarships.Data.Models;
using BizKidzScholarships.Data.NetworkedModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BizKidzScholarships.API.Services
{
    public class AdminService : BaseCRUDService
    {
        protected Guid userId => _user.Id;
        protected UserRewardFactory rewardFactory { get; set; }

        public AdminService(ICurrentUser user, IMapper mapper, BizKidzDbContext context, IHttpClientFactory _fac) : base(user, mapper, context, _fac)
        {
            rewardFactory = new UserRewardFactory(userId, context);
        }

        protected async Task<List<T>> GetAsListAsync<T>(
            Func<IQueryable<T>, IQueryable<T>>? queryShaper = null
        ) where T : class
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();

                if (queryShaper is not null)
                {
                    query = queryShaper(query);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<T>();
            }
        }

        public async Task<List<UserResult>> AdminUserList()
        {
            return await GetAsListAsync<UserResult>();
        }

        public async Task<List<UserActivity>> GetActivities()
        {
            return await GetAsListAsync<UserActivity>(q => q.OrderByDescending(a => a.Created).Take(3));
        }

        public async Task<List<TaskItem>> GetTasks()
        {
            return await GetAsListAsync<TaskItem>();
        }

        public async Task<AdminTaskSubmissionsSearchResults> GetAllSubmissions()
        {
            List <AdminTaskSubmissionView> submissions = await _context.Submissions
                    .Join(_context.Profiles, s => s.UserId, p => p.UserId, (s, p) => new AdminTaskSubmissionView() { TaskId = s.TaskId, AttemptNumber = s.AttemptNumber, UserFullName = p.FirstName + " " + p.LastName, SubmissionId = s.SubmissionId, TaskType = s.Task.TaskType, UserId = s.UserId, Created = s.Created })
                    .ToListAsync();

            var result = new AdminTaskSubmissionsSearchResults()
            {
                Results = submissions
            };

            return result;
        }

        public async Task<TaskSearchResponse> GetTasksSearch()
        {
            try
            {
                var list = await GetAsListAsync<TaskItem>();

                var results = new TaskSearchResponse() { Results = list };

                return results;
            }
            catch (Exception ex)
            {
                return new TaskSearchResponse() { Error = ex.Message };
            }
        }

        public async Task<ResponseModel> SaveTask()
        {
            throw new NotImplementedException();
        }

        public async Task<AdminTaskSubmissionsSearchResults> GetSubmissions(int taskId)
        {
            var search = new AdminTaskSubmissionsSearchResults();

            try
            {
                List<AdminTaskSubmissionView> submissions = await _context.Submissions
                    .Where(s => s.TaskId == taskId)
                    .Join(_context.Profiles, s => s.UserId, p => p.UserId, (s, p) => new AdminTaskSubmissionView() { TaskId = s.TaskId, AttemptNumber = s.AttemptNumber, UserFullName = p.FirstName + " " + p.LastName, SubmissionId = s.SubmissionId, TaskType = s.Task.TaskType, UserId = s.UserId, Created = s.Created })
                    .ToListAsync();

                search.Results.AddRange(submissions);

                return search;
            } catch (Exception ex)
            {
                search.Success = false;
                search.Errors.Add(ex.Message);
                return search;
            }
            
        }

        public async Task<ResponseModel> GetSubmissionLink(Guid submissionId)
        {
            var response = new GetSubmissionResult();

            var submission = await _context.Submissions.FirstOrDefaultAsync(s => s.SubmissionId == submissionId);

            if (submission is null)
            {
                response.Success = false;
                response.Errors.Add("No submission found.");
                return response;
            }

            var submissionData = JsonConvert.DeserializeObject<UploadTaskPayloadData>(submission.SubmissionData);

            if (string.IsNullOrEmpty(submissionData.S3Link))
            {
                response.Success = false;
                response.Errors.Add("Error fetching S3 Link for Submission: Invalid S3 link.");
            }

            var key = GetS3Key(submissionData.S3Link);

            var presignedUrlRequest = new GetPreSignedUrlRequest()
            {
                BucketName = Configuration.BucketName,
                Key = key
            };

            var client = new AmazonS3Client(RegionEndpoint.USEast2);
            var awsResponse = await client.GetPreSignedURLAsync(presignedUrlRequest);

            response.S3Link = awsResponse;

            if (string.IsNullOrEmpty(awsResponse))
            {
                response.Success = false;
                response.Errors.Add("AWS Could not retrieve a link to access this data");
            }

            return response;
        }

    }
}
