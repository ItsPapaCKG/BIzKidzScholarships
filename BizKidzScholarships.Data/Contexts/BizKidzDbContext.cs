using BizKidzScholarships.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text.RegularExpressions;
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

        public DbSet<UserResult> UserResults { get; set; }

        public DbSet<QuizQuestion> QuizQuestions { get; set; }

        public DbSet<QuestionOption> QuestionOptions { get; set; }

        public DbSet<QuizOption> QuizOptions { get; set; }

        public DbSet<TaskQuestion> TaskQuestions { get; set; }

        public DbSet<SiteDocument> SiteDocuments { get; set; }

        public DbSet<UserConsent> UserConsents { get; set; }
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

            //migrationBuilder.Sql("""
            //                            CREATE OR REPLACE VIEW "AdminUserList" AS
            //SELECT

            //    "FirstName" || ' ' || "LastName" AS "Name", 
            //    p."ChildFullName",
            //    p."Email",
            //    p."UserType",
            //    SUM(up."Points") AS "Points", 
            //    SUM(up."Points") / COALESCE(c."Value"::int, 100) AS "Entries",
            //    p."Created",
            //    p."Updated"
            //FROM "AspNetUsers" u
            //INNER JOIN "UserPoints" up ON u."Id" = up."UserId"
            //INNER JOIN "Profiles" p ON p."UserId" = u."Id"
            //LEFT JOIN "Configuration" c ON c."Id" = 'EntriesCost'
            //GROUP BY p."FirstName", p."LastName", c."Value"
            //""");

            //migrationBuilder.Sql(""" DROP VIEW "UserActivityView"; """);
            //migrationBuilder.Sql(""" DROP VIEW "AdminUserList"; """);

            modelBuilder.Entity<UserResult>(ur =>
            {
                ur.HasNoKey();

                ur.ToView("AdminUserList");
            });

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
                    },
                    new ConfigurationItem() {
                        Id = "S3BucketName",
                        Value = "bizkidz-task-bucket"
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
                    Created = new DateTimeOffset(2026, 2, 28, 0, 0, 0, TimeSpan.Zero),
                    Updated = new DateTimeOffset(2026, 2, 28, 0, 0, 0, TimeSpan.Zero),
                    TaskType = Enums.TaskType.FileUpload,
                    TaskImageKey = "https://bizkidz-task-bucket.s3.us-east-2.amazonaws.com/static/bizkidzexpo.jpg"
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
                    Created = new DateTimeOffset(2026, 2, 28, 0, 0, 0, TimeSpan.Zero),
                    Updated = new DateTimeOffset(2026, 2, 28, 0, 0, 0, TimeSpan.Zero),
                    TaskType = Enums.TaskType.VideoUpload,
                    TaskImageKey = "https://bizkidz-task-bucket.s3.us-east-2.amazonaws.com/static/pitchcontest.jpg"
                },
                new TaskItem
                {
                    Id = 3,
                    TaskTitle = "Complete the Biz Kidz Launch Kit",
                    TaskPromptTitle = "Pass the Quiz for the Biz Kidz Launch Kit",
                    TaskPromptSubtitle = "Once you have filled out your Biz Kidz Launch Kit, take this quiz to test your knowledge on all you have learned about starting and running a business!",
                    TaskDescription = "Take the quiz and show what you've learned from the Launch Kit.",
                    TaskEnabled = false,
                    TaskNameInternal = "Quiz Completion Task",
                    Reward = 50,
                    Created = new DateTimeOffset(2026, 2, 28, 0, 0, 0, TimeSpan.Zero),
                    Updated = new DateTimeOffset(2026, 2, 28, 0, 0, 0, TimeSpan.Zero),
                    TaskType = Enums.TaskType.Quiz,
                    TaskImageKey = "https://bizkidz-task-bucket.s3.us-east-2.amazonaws.com/static/entrepreneurshipguide.png"
                },
                new TaskItem
                {
                    Id = 4,
                    TaskTitle = "Register an Account",
                    TaskPromptTitle = "If you're 13 years old or older, create an account with the Biz Kidz Scholarships app.",
                    TaskPromptSubtitle = "Register a Biz Kidz Scholarships account.",
                    TaskDescription = "Once registered, you will be prompted in the future to upload photos and complete certain task for a chance to win scholarships.",
                    TaskEnabled = false,
                    TaskNameInternal = "Quiz Completion Task",
                    Reward = 0,
                    Created = new DateTimeOffset(2026, 2, 28, 0, 0, 0, TimeSpan.Zero),
                    Updated = new DateTimeOffset(2026, 2, 28, 0, 0, 0, TimeSpan.Zero),
                    TaskType = Enums.TaskType.Quiz,
                    TaskImageKey = "https://bizkidz-task-bucket.s3.us-east-2.amazonaws.com/static/bizkidzexpo.jpg"
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
                .OnDelete(DeleteBehavior.SetNull);

                s.Property(s => s.SubmissionData).HasColumnType("jsonb");
            });

            modelBuilder.Entity<ActionRequest>(ar =>
            {
                ar.HasKey(a => a.RequestId);

                ar.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId);
            });

            modelBuilder.Entity<QuizQuestion>(qq =>
            {
                qq.ToTable("QuizQuestions");
                qq.HasKey(q => q.QuestionId);
                qq.HasIndex(q => q.Prompt);
            });

            modelBuilder.Entity<QuizQuestion>().HasData([
                    new QuizQuestion() {
                        QuestionId = 1,
                        Prompt = "What is the name of the non-profit organization that makes Biz Kidz possible?",
                        PromptImageKey = "https://bizkidz-task-bucket.s3.us-east-2.amazonaws.com/static/biz-kidz-usa-logo.avif",
                        Created = new DateTimeOffset(2026,2,20,8,0,0,TimeSpan.Zero)
                    },
                    new QuizQuestion() {
                        QuestionId = 2,
                        Prompt = "What is profit?",
                        PromptImageKey = "https://bizkidz-task-bucket.s3.us-east-2.amazonaws.com/static/entrepreneurshipguide.png",
                        Created = new DateTimeOffset(2026,2,20,8,0,0,TimeSpan.Zero)
                    },
                    new QuizQuestion() {
                        QuestionId = 3,
                        Prompt = "Select Answer D.",
                        PromptImageKey = "https://bizkidz-task-bucket.s3.us-east-2.amazonaws.com/static/years-logo.avif",
                        Created = new DateTimeOffset(2026,2,20,8,0,0,TimeSpan.Zero)
                    }
                ]);

            modelBuilder.Entity<QuizOption>(qo =>
            {
                qo.ToTable("QuizOptions");
                qo.HasKey(q => q.OptionId);
                qo.HasIndex(q => q.OptionValue);
            });

            modelBuilder.Entity<QuizOption>().HasData([
                    new QuizOption() {
                        OptionId = 1,
                        OptionKey = "A",
                        OptionValue = "Kidz Bidz",
                        Created = new DateTimeOffset(2026,2,20,8,0,0,TimeSpan.Zero)
                    },
                    new QuizOption() {
                        OptionId = 2,
                        OptionKey = "B",
                        OptionValue = "Microsoft",
                        Created = new DateTimeOffset(2026,2,20,8,0,0,TimeSpan.Zero)
                    },
                    new QuizOption() {
                        OptionId = 3,
                        OptionKey = "C",
                        OptionValue = "Y.E.A.R.S Foundation",
                        Created = new DateTimeOffset(2026,2,20,8,0,0,TimeSpan.Zero)
                    },
                    new QuizOption() {
                        OptionId = 4,
                        OptionKey = "D",
                        OptionValue = "Google",
                        Created = new DateTimeOffset(2026,2,20,8,0,0,TimeSpan.Zero)
                    },
                    new QuizOption() {
                        OptionId = 5,
                        OptionKey = "A",
                        OptionValue = "The name of a band",
                        Created = new DateTimeOffset(2026,2,20,8,0,0,TimeSpan.Zero)
                    },
                    new QuizOption() {
                        OptionId = 6,
                        OptionKey = "B",
                        OptionValue = "The cost of performing services in your business",
                        Created = new DateTimeOffset(2026,2,20,8,0,0,TimeSpan.Zero)
                    },
                    new QuizOption() {
                        OptionId = 7,
                        OptionKey = "C",
                        OptionValue = "Scholarships won by Biz Kidz",
                        Created = new DateTimeOffset(2026,2,20,8,0,0,TimeSpan.Zero)
                    },
                    new QuizOption() {
                        OptionId = 8,
                        OptionKey = "D",
                        OptionValue = "The net income gained after expenses are deducted",
                        Created = new DateTimeOffset(2026,2,20,8,0,0,TimeSpan.Zero)
                    },
                    new QuizOption() {
                        OptionId = 9,
                        OptionKey = "A",
                        OptionValue = "Answer A",
                        Created = new DateTimeOffset(2026,2,20,8,0,0,TimeSpan.Zero)
                    },
                    new QuizOption() {
                        OptionId = 10,
                        OptionKey = "B",
                        OptionValue = "Answer B",
                        Created = new DateTimeOffset(2026,2,20,8,0,0,TimeSpan.Zero)
                    },
                    new QuizOption() {
                        OptionId = 11,
                        OptionKey = "C",
                        OptionValue = "Answer C",
                        Created = new DateTimeOffset(2026,2,20,8,0,0,TimeSpan.Zero)
                    },
                    new QuizOption() {
                        OptionId = 12,
                        OptionKey = "D",
                        OptionValue = "Answer D",
                        Created = new DateTimeOffset(2026,2,20,8,0,0,TimeSpan.Zero)
                    },

                ]);

            modelBuilder.Entity<QuestionOption>(quo =>
            {
                quo.ToTable("QuestionOptions");
                quo.HasKey(q => new { q.QuestionId, q.OptionId });

                quo.HasOne<QuizQuestion>()
                .WithMany()
                .HasForeignKey(f => f.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

                quo.HasOne<QuizOption>()
                .WithMany()
                .HasForeignKey(f => f.OptionId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<QuestionOption>().HasData([
                    new QuestionOption() {
                        OptionId = 1,
                        QuestionId = 1
                    },
                    new QuestionOption() {
                        OptionId = 2,
                        QuestionId = 1
                    },
                    new QuestionOption() {
                        OptionId = 3,
                        QuestionId = 1
                    },
                    new QuestionOption() {
                        OptionId = 4,
                        QuestionId = 1
                    },
                    new QuestionOption() {
                        OptionId = 5,
                        QuestionId = 2
                    },
                    new QuestionOption() {
                        OptionId = 6,
                        QuestionId = 2
                    },
                    new QuestionOption() {
                        OptionId = 7,
                        QuestionId = 2
                    },
                    new QuestionOption() {
                        OptionId = 8,
                        QuestionId = 2
                    },
                    new QuestionOption() {
                        OptionId = 9,
                        QuestionId = 3
                    },
                    new QuestionOption() {
                        OptionId = 10,
                        QuestionId = 3
                    },
                    new QuestionOption() {
                        OptionId = 11,
                        QuestionId = 3
                    },
                    new QuestionOption() {
                        OptionId = 12,
                        QuestionId = 3
                    },
                ]);

            modelBuilder.Entity<TaskQuestion>(tq =>
            {
                tq.ToTable("TaskQuestions");

                tq.HasKey(tq => new { tq.TaskId, tq.QuestionId });

                tq.HasOne<TaskItem>()
                .WithMany()
                .HasForeignKey(f => f.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

                tq.HasOne<QuizQuestion>()
                .WithMany()
                .HasForeignKey(f => f.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SiteDocument>(sd =>
            {
                sd.HasKey(s => s.Id);

                sd.HasIndex(s => s.Name);
            });

            modelBuilder.Entity<SiteDocument>().HasData([
                    new SiteDocument() {
                        Id = 1,
                        Type = ConsentType.PrivacyPolicy,
                        ContentsHTML = """
                        <div style="max-width: 1800px; margin: 40px auto; font-family: Arial, sans-serif; line-height: 1.6; color: #333;">

                          <h1 style="font-size: 28px; margin-bottom: 10px;">Privacy Policy</h1>
                          <p style="font-size: 14px; color: #666;">Last Updated: 2/28/2026</p>

                          <p>
                            We respect your privacy and are committed to protecting your information.
                          </p>

                          <h2 style="margin-top: 30px; font-size: 20px;">1. Information We Collect</h2>

                          <h3 style="margin-top: 20px; font-size: 16px;">Personal Information</h3>
                          <ul style="padding-left: 20px;">
                            <li>First Name</li>
                            <li>Last Name</li>
                            <li>Email Address</li>
                            <li>Date of Birth</li>
                            <li>Child’s Name (if provided)</li>
                            <li>Profile Picture</li>
                            <li>Business Name</li>
                            <li>Phone Number</li>
                          </ul>

                          <h3 style="margin-top: 20px; font-size: 16px;">Technical & Legal Information</h3>
                          <ul style="padding-left: 20px;">
                            <li>IP Address</li>
                            <li>User Agent (browser/device information)</li>
                          </ul>

                          <h3 style="margin-top: 20px; font-size: 16px;">Contest & App Submissions</h3>
                          <ul style="padding-left: 20px;">
                            <li>Files uploaded within the app</li>
                            <li>Answers submitted to questions or prompts</li>
                          </ul>

                          <h2 style="margin-top: 30px; font-size: 20px;">2. How We Use Your Information</h2>
                          <p>We use your information to:</p>
                          <ul style="padding-left: 20px;">
                            <li>Create and manage user accounts</li>
                            <li>Verify eligibility for contests or prizes</li>
                            <li>Communicate important account updates</li>
                            <li>Operate and improve the platform</li>
                            <li>Maintain security and prevent abuse</li>
                          </ul>

                          <p><strong>We do not sell your personal information.</strong></p>

                          <h2 style="margin-top: 30px; font-size: 20px;">3. Data Storage & Security</h2>
                          <p>
                            Your information is stored securely on a private server. We implement reasonable administrative and technical safeguards to protect your data from unauthorized access or misuse.
                          </p>

                          <h2 style="margin-top: 30px; font-size: 20px;">4. Children’s Information</h2>
                          <p>
                            If information about a child is provided, it is used only for account functionality, contest participation, or prize eligibility. Parents or guardians may request deletion of child-related data at any time.
                          </p>

                          <h2 style="margin-top: 30px; font-size: 20px;">5. Data Retention</h2>
                          <p>
                            We retain information only as long as necessary to maintain your account, fulfill contest or prize obligations, and comply with legal requirements. You may request deletion of your account at any time.
                          </p>

                          <h2 style="margin-top: 30px; font-size: 20px;">6. Your Rights</h2>
                          <p>You may:</p>
                          <ul style="padding-left: 20px;">
                            <li>Request access to your information</li>
                            <li>Request correction of inaccurate information</li>
                            <li>Request deletion of your account and associated data</li>
                          </ul>

                          <p>
                            To make a request, contact us at:<br>
                            <strong>[Insert Contact Email]</strong>
                          </p>

                          <h2 style="margin-top: 30px; font-size: 20px;">7. Updates to This Policy</h2>
                          <p>
                            We may update this Privacy Policy from time to time. Updates will be posted within the application.
                          </p>

                        </div>
                        """,
                        Created = new DateTimeOffset(2026, 3, 1, 0, 0, 0, new TimeSpan()),
                        Version = "1.0",
                        Name = "Privacy Policy TEST"
                    },
                    new SiteDocument() {
                        Id = 2,
                        Type = ConsentType.TermsOfService,
                        ContentsHTML = """
                        <div style="max-width: 800px; margin: 40px auto; font-family: Arial, sans-serif; line-height: 1.6; color: #333;">

                          <h1 style="font-size: 28px; margin-bottom: 10px;">Terms of Service</h1>
                          <p style="font-size: 14px; color: #666;">Last Updated: 2/28/2026</p>

                          <p>
                            By accessing or using this application, you agree to the following Terms of Service.
                          </p>

                          <h2 style="margin-top: 30px; font-size: 20px;">1. Age Requirements</h2>
                          <p>
                            This application may only be used by individuals age 13 or older. 
                            Individuals under the age of 13 may not create or own an account.
                          </p>
                          <p>
                            Parents or legal guardians may create and manage accounts on behalf of minors.
                          </p>

                          <h2 style="margin-top: 30px; font-size: 20px;">2. Registration & Prizes</h2>
                          <p>
                            Registration or participation in the app does not guarantee any prizes, awards, or winnings.
                          </p>
                          <p>
                            The application is used solely for recording entries, submissions, and points. 
                            All official drawings, selections, and contests are conducted outside of the application and in real life.
                          </p>
                          <p>
                            The application itself does not distribute, ship, or deliver prizes.
                          </p>

                          <h2 style="margin-top: 30px; font-size: 20px;">3. Acceptable Use</h2>
                          <p>You agree not to:</p>
                          <ul style="padding-left: 20px;">
                            <li>Use the application for unlawful purposes</li>
                            <li>Attempt to gain unauthorized access to accounts or systems</li>
                            <li>Abuse, exploit, or manipulate the point or contest system</li>
                            <li>Upload harmful, malicious, or inappropriate content</li>
                          </ul>

                          <h2 style="margin-top: 30px; font-size: 20px;">4. API & Technical Access</h2>
                          <p>
                            Unauthorized access, use, or copying of the application's API, backend systems, or technical infrastructure is strictly prohibited.
                          </p>

                          <h2 style="margin-top: 30px; font-size: 20px;">5. Intellectual Property</h2>
                          <p>
                            This application, including its design, features, content, and underlying systems, is the property of the <strong>Y.E.A.R.S Foundation</strong>.
                          </p>
                          <p>
                            You may not copy, reproduce, distribute, reverse engineer, or create derivative works of this application without written permission.
                          </p>

                          <h2 style="margin-top: 30px; font-size: 20px;">6. Account Termination</h2>
                          <p>
                            We reserve the right to suspend or terminate accounts that violate these Terms or misuse the application.
                          </p>

                          <h2 style="margin-top: 30px; font-size: 20px;">7. Limitation of Liability</h2>
                          <p>
                            The application is provided "as is" without warranties of any kind. 
                            We are not responsible for technical errors, interruptions, data loss, or disputes related to contest outcomes.
                          </p>

                          <h2 style="margin-top: 30px; font-size: 20px;">8. Changes to These Terms</h2>
                          <p>
                            We may update these Terms from time to time. Continued use of the application constitutes acceptance of any changes.
                          </p>

                        </div>
                        """,
                        Created = new DateTimeOffset(2026, 3, 1, 0, 0, 0, new TimeSpan()),
                        Version = "1.0",
                        Name = "Terms of Service TEST"
                    },
                    new SiteDocument() {
                        Id = 3,
                        Type = ConsentType.MediaConsent,
                        ContentsHTML = """
                        <div>

                            This is a test media consent form. You agree to it, whatever it is.

                        </div>
                        """,
                        Created = new DateTimeOffset(2026, 3, 1, 0, 0, 0, new TimeSpan()),
                        Version = "0.1",
                        Name = "Media Consent TEST"
                    },
                ]);

            modelBuilder.Entity<UserConsent>(uc =>
            {
                uc.HasKey(uc => uc.Id);

                uc.HasOne<IdentityUser<Guid>>()
                .WithMany()
                .HasForeignKey(uc => uc.UserId);

                uc.HasOne(uc => uc.Document)
                .WithMany()
                .HasForeignKey(uc => uc.DocumentId);

                uc.Property(uc => uc.IsGranted).HasDefaultValue(false);
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
