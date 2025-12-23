using AutoMapper;
using BizKidzScholarships.API.Services.Base;
using BizKidzScholarships.API.Services.Utilities;
using BizKidzScholarships.Data.Contexts;
using BizKidzScholarships.Data.Entities;
using BizKidzScholarships.Data.NetworkedModels;
using Microsoft.EntityFrameworkCore;

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

    }
}
