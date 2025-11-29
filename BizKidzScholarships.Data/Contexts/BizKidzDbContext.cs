using BizKidzScholarships.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskItem = BizKidzScholarships.Data.Entities.TaskItem;

namespace BizKidzScholarships.Data.Contexts
{
    public class BizKidzDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<UserProfile> Profiles { get; set; }

        public DbSet<UserTask> UserTasks { get; set; }

        public DbSet<UserPointsReward> UserPoints { get; set; }

        public DbSet<ActionRequest> ActionRequests { get; set; }

        public DbSet<TaskSubmission> Submissions { get; set; }

        public BizKidzDbContext(DbContextOptions<BizKidzDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserProfile>(s =>
            {
                s.ToTable("Profiles");
                s.HasKey(up => up.UserId);

                s.HasOne(up => up.User)
                .WithMany()
                .HasForeignKey(up => up.UserId);
            });

            modelBuilder.Entity<UserPointsReward>(upo =>
            {
                upo.HasKey(upo => new { upo.UserId, upo.TaskId, upo.AttemptNumber });

                upo.HasOne(upo => upo.User)
                .WithMany()
                .HasForeignKey(upo => upo.UserId);

                upo.HasOne(upo => upo.Task)
                .WithMany(t => t.Rewards)
                .HasForeignKey(r => r.TaskId);
            });

            modelBuilder.Entity<TaskItem>(b =>
            {
                b.ToTable("Tasks");
                b.HasKey(c => c.Id);
                b.HasIndex(c => c.TaskNameInternal);
            });

            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem
                {
                    Id = 1,
                    TaskTitle = "First Added Task",
                    TaskPromptTitle = "Task Prompt",
                    TaskPromptSubtitle = "",
                    TaskDescription = "Task Description Goes Here!",
                    TaskEnabled = true,
                    TaskNameInternal = "First Task",
                    Reward = 1000,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    TaskType = Enums.TaskType.ImageUpload
                });

            modelBuilder.Entity<UserTask>(t =>
            {
                t.HasOne(ut => ut.User)
                .WithMany()
                .HasForeignKey(ut => ut.UserId);

                t.HasOne(ut => ut.Task)
                .WithMany()
                .HasForeignKey(ut => ut.TaskId);

                t.HasIndex(ut => ut.Status);

            });

            modelBuilder.Entity<TaskSubmission>(s =>
            {
                s.HasKey(ts => new { ts.AttemptNumber, ts.UserId, ts.TaskId });

                s.HasOne(ts => ts.User)
                .WithMany()
                .HasForeignKey(ts => ts.UserId);

                s.HasOne(ts => ts.Task)
                .WithMany(t => t.TaskSubmissions)
                .HasForeignKey(ts => ts.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

                s.Property(s => s.SubmissionData).HasColumnType("jsonb");
            });

            modelBuilder.Entity<ActionRequest>(ar =>
            {
                ar.HasKey(a => a.RequestId);

                ar.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId);
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
