using AutoMapper;
using BizKidzScholarships.Data.Contexts;
using BizKidzScholarships.Data.dto;
using BizKidzScholarships.Data.NetworkedModels;

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
        public UserPointsView GetUserPoints(Guid userId)
        {
            throw new NotImplementedException();
        }

        public UserProfileDTO GetUserProfile(int userId = 0)
        {
            throw new NotImplementedException();
        }

        public List<DashboardTaskDTO> GetUserTasks(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
