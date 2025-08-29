using Microsoft.EntityFrameworkCore;
using BizKidzScholarships.Data.Entities;
using TaskItem = BizKidzScholarships.Data.Entities.TaskItem;

namespace BizKidzScholarships.Data.Contexts
{
    public class BizKidzDbContext : DbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<TaskItem> Tasks { get; set; }
        DbSet<UserProfile> Profiles { get; set; }

        DbSet<UserTask> UserTasks { get; set; }

        DbSet<UserPoints> UserPoints { get; set; }

        public BizKidzDbContext(DbContextOptions<BizKidzDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(u =>
            {
                u.Property(p => p.Id)
                .UseIdentityColumn();

                u.HasIndex(p => p.Username);

                u.HasOne(up => up.Profile)
                .WithOne(u => u.User)
                .HasForeignKey<UserProfile>(up => up.UserId);
            });

            modelBuilder.Entity<TaskItem>(b => {
                b.ToTable("Tasks");
                b.HasKey(c => c.Id);
                b.HasIndex(c => c.TaskNameInternal);
            });

            modelBuilder.Entity<UserTask>(t =>
            {
                t.HasOne(ut => ut.User)
                .WithMany(u => u.UserTasks)
                .HasForeignKey(ut => ut.UserId);

                t.HasOne(ut => ut.Task)
                .WithMany()
                .HasForeignKey(ut => ut.TaskId);

                t.HasIndex(ut => ut.Status);

            });

            modelBuilder.Entity<UserProfile>(s => {
                s.ToTable("Profiles");

                s.HasOne(up => up.User)
                .WithOne(p => p.Profile)
                .HasForeignKey<UserProfile>(u => u.UserId);
            });

            modelBuilder.Entity<TaskSubmission>(s => { 
                s.HasKey(ts => new { ts.AttemptNumber, ts.UserId, ts.TaskId });

                s.HasOne(ts => ts.User)
                .WithMany(u => u.TaskSubmissions)
                .HasForeignKey(ts => ts.UserId);

                s.HasOne(ts => ts.Task)
                .WithMany(t => t.TaskSubmissions)
                .HasForeignKey(ts => ts.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
