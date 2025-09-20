using AutoMapper;
using BizKidzScholarships.API.Services.Base;
using BizKidzScholarships.Data.Contexts;
using BizKidzScholarships.Data.dto;
using BizKidzScholarships.Data.Entities;
using BizKidzScholarships.Data.NetworkedModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;

namespace BizKidzScholarships.API.Services
{
    public class UserDataService : BaseCRUDService, IUserDataService
    {
        //private ICurrentUser _user;
        //private BizKidzDbContext _context;
        //private IMapper _mapper;

        protected Guid userId => _user.Id;

        public UserDataService(ICurrentUser user, IMapper mapper, BizKidzDbContext context) : base(user, mapper, context)
        {
            //_user = user;
            //_context = context;
            //_mapper = mapper;
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

            var ent = _mapper.Map<UserProfile>(profile);

            ent.UserId = userId;

            using (var t = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.Profiles.AddAsync(ent);

                    await _context.SaveChangesAsync();

                    await t.CommitAsync();
                }
                catch (Exception ex)
                {
                    t.Rollback();

                    var error = ex.Message;

                    response.Succeeded = false;
                    response.Errors.Add(error);
                    return response;
                }
            }

            return response;
        }

        public async Task<List<DashboardTaskDTO>> GetUserTasks(Guid userId)
        {
            //var tasks = await _context.UserTasks.Where(t => t.UserId == userId).ToListAsync();

            var tasksQueryable = from userTask in _context.UserTasks
                                 join t in _context.Tasks on userTask.TaskId equals t.Id
                                 where t.TaskEnabled && userTask.UserId == userId
                                 select new DashboardTaskDTO { TaskTitle = t.TaskTitle, Reward = t.Reward, Status = userTask.Status, TaskId = t.Id, TaskDescription = t.TaskDescription, TaskImageKey = t.TaskImageKey };

            var tasks = await tasksQueryable.ToListAsync();

            if (tasks is null)
                return [];

            return tasks;
        }

        public async Task<bool> UpdateUserProfile(UserProfileDTO profile)
        {
            var existing = _context.Profiles.Any(ut => ut.UserId == userId);

            if (!existing) return false;

            var ent = _mapper.Map<UserProfile>(profile);

            var result = await SafeUpdateAsync(ent);

            return result;
        }
    }
}
