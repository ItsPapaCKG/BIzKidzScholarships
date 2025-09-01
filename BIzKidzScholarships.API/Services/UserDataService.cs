using AutoMapper;
using BizKidzScholarships.Data.Contexts;
using BizKidzScholarships.Data.dto;
using BizKidzScholarships.Data.NetworkedModels;
using Microsoft.EntityFrameworkCore;

namespace BIzKidzScholarships.API.Services
{
    public class UserDataService : IUserDataService
    {
        private ICurrentUser _user;
        private BizKidzDbContext _context;
        private IMapper _mapper;

        public UserDataService(ICurrentUser user, IMapper mapper, BizKidzDbContext context)
        {
            _user = user;
            _context = context;
            _mapper = mapper;
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

        public async Task<List<DashboardTaskDTO>> GetUserTasks(Guid userId)
        {
            var tasks = await _context.UserTasks.Where(ut => ut.UserId == userId).ToListAsync();

            var newList = _mapper.Map<List<DashboardTaskDTO>>(tasks);

            return newList;
        }
    }
}
