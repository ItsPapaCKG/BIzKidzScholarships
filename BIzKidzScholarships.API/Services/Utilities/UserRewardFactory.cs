using BizKidzScholarships.Data.Contexts;
using BizKidzScholarships.Data.Entities;

namespace BizKidzScholarships.API.Services.Utilities
{
    public class UserRewardFactory
    {
        public Guid UserId { get; set; }
        private BizKidzDbContext db;
        public UserRewardFactory(Guid userId, BizKidzDbContext context)
        {
            db = context;
            UserId = userId;
        }

        public UserPointsReward New(int taskId, Guid userId)
        {
            var task = db.Tasks.FirstOrDefault(t => t.Id == taskId);
            var attemptNumber = db.Submissions.Select(t => t.AttemptNumber).OrderByDescending(t => t).FirstOrDefault() + 1;

            if (task == null)
            {
                throw new Exception($"TaskId {taskId} does not exist. Could not create reward for user.");
            }

            UserPointsReward reward = new UserPointsReward()
            {
                AttemptNumber = attemptNumber,
                TaskId = taskId,
                UserId = userId,
                Points = task.Reward,
                IsNew = true,
                Created = DateTimeOffset.UtcNow,
                Updated = DateTimeOffset.UtcNow
            };

            return reward;
        }
    }
}
