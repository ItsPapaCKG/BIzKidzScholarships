using BizKidzScholarships.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
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

        public DbSet<ConfigurationItem> Configuration { get; set; }

        public DbSet<UserActivity> UserActivities { get; set; }

        public BizKidzDbContext(DbContextOptions<BizKidzDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //migrationBuilder.Sql("""
            //CREATE OR REPLACE VIEW "UserActivityView" AS
            //SELECT

            //    P."FirstName" || ' ' || P."LastName" AS "FullName",
            //    T."TaskNameInternal" AS "Task",
            //    T."Reward",
            //    S."Created"
            //FROM

            //    "Submissions" S
            //    INNER JOIN "Profiles" P ON P."UserId" = S."UserId"

            //    INNER JOIN "Tasks" T ON T."Id" = S."TaskId";
            //""");

            //migrationBuilder.Sql(""" DROP VIEW "UserActivityView"; """);

            modelBuilder.Entity<UserActivity>(ua => {
                ua.HasNoKey();

                ua.ToView("UserActivityView");
            });

            modelBuilder.Entity<ConfigurationItem>(ci => {
                ci.HasKey(x => x.Id);
            });

            modelBuilder.Entity<ConfigurationItem>().HasData([
                    new ConfigurationItem() {
                        Id = "DefaultProfilePicture",
                        Value="https://bizkidz-task-bucket.s3.us-east-2.amazonaws.com/static/years-logo.avif"
                    }
                ]);

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
                upo.HasKey(upo => upo.AwardId);

                upo.HasIndex(upo => new { upo.UserId, upo.TaskId, upo.AttemptNumber });

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

            modelBuilder.Entity<TaskItem>().HasData([
                new TaskItem
                {
                    Id = 1,
                    TaskTitle = "Sell your products at a Biz Kidz Market",
                    TaskPromptTitle = "Upload a Photo of you and your team operating your Business at a Biz Kidz Market",
                    TaskPromptSubtitle = "Download and print the Biz Kidz USA Logo, and pose with it at your vendor table at a Biz Kidz Market event. Submit your photo and earn points!",
                    TaskDescription = "Sell your products at any Biz Kidz Market and upload a photo to confirm your attendance.",
                    TaskEnabled = true,
                    TaskNameInternal = "Business Photo Upload Task",
                    Reward = 50,
                    IsGlobalTask = true,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    TaskType = Enums.TaskType.FileUpload
                },
                new TaskItem
                {
                    Id = 2,
                    TaskTitle = "Submit a 90-second Pitch Video",
                    TaskPromptTitle = "Upload Pitch Video",
                    TaskPromptSubtitle = "Showcase your business' brilliance by uploading a 90-second video of you pitching your products.",
                    TaskDescription = "Give your business's elevator pitch in 90 seconds or less.",
                    TaskEnabled = true,
                    TaskNameInternal = "Pitch Video Task",
                    Reward = 50,
                    IsGlobalTask = true,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    TaskType = Enums.TaskType.FileUpload
                },
                new TaskItem
                {
                    Id = 3,
                    TaskTitle = "Complete the Biz Kidz Launch Kit",
                    TaskPromptTitle = "Pass the Quiz for the Biz Kidz Launch Kit",
                    TaskPromptSubtitle = "Once you have filled out your Biz Kidz Launch Kit, take this quiz to test your knowledge on all you have learned about starting and running a business!",
                    TaskDescription = "Take the quiz and show what you've learned from the Launch Kit.",
                    TaskEnabled = true,
                    TaskNameInternal = "Pitch Video Task",
                    Reward = 50,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    TaskType = Enums.TaskType.FileUpload
                },
            ]);

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
                s.HasKey(ts => ts.SubmissionId);

                s.HasIndex(ts => new { ts.UserId, ts.TaskId, ts.AttemptNumber })
                .IsUnique();

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
