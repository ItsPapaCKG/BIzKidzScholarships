using BizKidzScholarships.Data.dto;
using BizKidzScholarships.Data.Entities;
using BizKidzScholarships.Data.Models;

namespace BizKidzScholarships.API.Services
{
    public interface IUserDataService
    {
        int GetUserAge(DateTimeOffset birthday);

        // Public Get User Profile
        Task<UserProfileDTO?> GetUserProfile(Guid userId);

        // Public Get User Assigned Tasks w/ Title/Description/Points bound
        Task<List<DashboardTaskDTO>> GetUserTasks(Guid userId);

        // Get User Points and Entries Total
        UserPointsView? GetUserPoints(Guid userId);

        Task<ResponseModel> SetUserProfile(Guid userId, UserProfileDTO profile, bool isRegister = false);

        Task<ResponseModel> SetUserProfile(Guid userId, UpdateUserProfileDTO profile, bool isRegister = false);

        Task<ResponseModel> StartUploadHandshake(PresignedRequestModel req);

        Task<ResponseModel> UploadConfirmation(UploadHandshakeConfirmationModel confirmation);

        Task<ResponseModel> AssignTask(int taskid, Guid userid);

        Task<ResponseModel> SetGlobalTasksForUser(Guid userId);

        Task<List<ResponseModel>> SetGlobalTasksForUsers(List<Guid> userIds);

        Task<List<ResponseModel>> SetGlobalTasksAllUsers();

        Task<ResponseModel> RegisterUserProfile(Guid userId, RegisterDTO registration);

        Task<QuizQuestionViewModel[]> GetQuiz(int taskId);
    }
}
