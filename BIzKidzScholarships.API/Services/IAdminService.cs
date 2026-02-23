using BizKidzScholarships.Data.dto;
using BizKidzScholarships.Data.Entities;
using BizKidzScholarships.Data.Models.SaveModels;

namespace BizKidzScholarships.API.Services
{
    public interface IAdminService
    {
        Task<List<T>> GetAsListAsync<T>(
            Func<IQueryable<T>, IQueryable<T>>? queryShaper = null
        );

        Task<List<UserResult>> AdminUserList();

        Task<List<UserActivity>> GetActivities();

        Task<List<TaskItem>> GetTasks();

        Task<ResponseModel> SaveTask(TaskSaveModel task);

        Task<ResponseModel> AddRewardAdjustment();
    }
}
