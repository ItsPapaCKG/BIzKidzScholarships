using BizKidzScholarships.Data.dto;
using BizKidzScholarships.Data.Entities;
using BizKidzScholarships.Data.Models;

namespace BizKidzScholarships.API.Services
{
    public interface IUserDataService
    {

        // Public Get User Profile
        UserProfileDTO? GetUserProfile(Guid userId);

        // Public Get User Assigned Tasks w/ Title/Description/Points bound
        Task<List<DashboardTaskDTO>> GetUserTasks(Guid userId);

        // Get User Points and Entries Total
        UserPointsView? GetUserPoints(Guid userId);

        Task<ResponseModel> SetUserProfile(Guid userId, RegisterUserProfileDTO profile);

        ResponseModel StartUploadHandshake(PresignedRequestModel req);

        Task<ResponseModel> UploadConfirmation(UploadHandshakeConfirmationModel confirmation);

    }
}
