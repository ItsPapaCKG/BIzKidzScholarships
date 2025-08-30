using BizKidzScholarships.Data.dto;
using BizKidzScholarships.Data.Entities;

namespace BIzKidzScholarships.API.Services
{
    public interface IUserDataService
    {

        // Public Get User Profile
        UserProfileDTO GetUserProfile(int userId);

        // Public Get User Assigned Tasks w/ Title/Description/Points bound
        List<DashboardTaskDTO> GetUserTasks(int userId);

        // Get User Points and Entries Total
        UserPointsView GetUserPoints(int userId);

    }
}
