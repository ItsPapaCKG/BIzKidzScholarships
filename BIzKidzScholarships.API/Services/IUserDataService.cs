using BizKidzScholarships.Data.dto;
using BizKidzScholarships.Data.Entities;

namespace BIzKidzScholarships.API.Services
{
    public interface IUserDataService
    {

        // Public Get User Profile
        UserProfileDTO? GetUserProfile(Guid userId);

        // Public Get User Assigned Tasks w/ Title/Description/Points bound
        Task<List<DashboardTaskDTO>> GetUserTasks(Guid userId);

        // Get User Points and Entries Total
        UserPointsView? GetUserPoints(Guid userId);

        Task<ResponseModel> RegisterUserProfile(Guid userId, RegisterUserProfileDTO profile);

    }
}
