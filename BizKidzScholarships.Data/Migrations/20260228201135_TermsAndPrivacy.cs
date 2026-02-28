using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BizKidzScholarships.Data.Migrations
{
    /// <inheritdoc />
    public partial class TermsAndPrivacy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SiteDocuments",
                keyColumn: "Id",
                keyValue: 1,
                column: "ContentsHTML",
                value: "<div style=\"max-width: 800px; margin: 40px auto; font-family: Arial, sans-serif; line-height: 1.6; color: #333;\">\r\n\r\n  <h1 style=\"font-size: 28px; margin-bottom: 10px;\">Privacy Policy</h1>\r\n  <p style=\"font-size: 14px; color: #666;\">Last Updated: [Insert Date]</p>\r\n\r\n  <p>\r\n    We respect your privacy and are committed to protecting your information.\r\n  </p>\r\n\r\n  <h2 style=\"margin-top: 30px; font-size: 20px;\">1. Information We Collect</h2>\r\n\r\n  <h3 style=\"margin-top: 20px; font-size: 16px;\">Personal Information</h3>\r\n  <ul style=\"padding-left: 20px;\">\r\n    <li>First Name</li>\r\n    <li>Last Name</li>\r\n    <li>Email Address</li>\r\n    <li>Date of Birth</li>\r\n    <li>Child’s Name (if provided)</li>\r\n    <li>Profile Picture</li>\r\n    <li>Business Name</li>\r\n    <li>Phone Number</li>\r\n  </ul>\r\n\r\n  <h3 style=\"margin-top: 20px; font-size: 16px;\">Technical & Legal Information</h3>\r\n  <ul style=\"padding-left: 20px;\">\r\n    <li>IP Address</li>\r\n    <li>User Agent (browser/device information)</li>\r\n  </ul>\r\n\r\n  <h3 style=\"margin-top: 20px; font-size: 16px;\">Contest & App Submissions</h3>\r\n  <ul style=\"padding-left: 20px;\">\r\n    <li>Files uploaded within the app</li>\r\n    <li>Answers submitted to questions or prompts</li>\r\n  </ul>\r\n\r\n  <h2 style=\"margin-top: 30px; font-size: 20px;\">2. How We Use Your Information</h2>\r\n  <p>We use your information to:</p>\r\n  <ul style=\"padding-left: 20px;\">\r\n    <li>Create and manage user accounts</li>\r\n    <li>Verify eligibility for contests or prizes</li>\r\n    <li>Communicate important account updates</li>\r\n    <li>Operate and improve the platform</li>\r\n    <li>Maintain security and prevent abuse</li>\r\n  </ul>\r\n\r\n  <p><strong>We do not sell your personal information.</strong></p>\r\n\r\n  <h2 style=\"margin-top: 30px; font-size: 20px;\">3. Data Storage & Security</h2>\r\n  <p>\r\n    Your information is stored securely on a private server. We implement reasonable administrative and technical safeguards to protect your data from unauthorized access or misuse.\r\n  </p>\r\n\r\n  <h2 style=\"margin-top: 30px; font-size: 20px;\">4. Children’s Information</h2>\r\n  <p>\r\n    If information about a child is provided, it is used only for account functionality, contest participation, or prize eligibility. Parents or guardians may request deletion of child-related data at any time.\r\n  </p>\r\n\r\n  <h2 style=\"margin-top: 30px; font-size: 20px;\">5. Data Retention</h2>\r\n  <p>\r\n    We retain information only as long as necessary to maintain your account, fulfill contest or prize obligations, and comply with legal requirements. You may request deletion of your account at any time.\r\n  </p>\r\n\r\n  <h2 style=\"margin-top: 30px; font-size: 20px;\">6. Your Rights</h2>\r\n  <p>You may:</p>\r\n  <ul style=\"padding-left: 20px;\">\r\n    <li>Request access to your information</li>\r\n    <li>Request correction of inaccurate information</li>\r\n    <li>Request deletion of your account and associated data</li>\r\n  </ul>\r\n\r\n  <p>\r\n    To make a request, contact us at:<br>\r\n    <strong>[Insert Contact Email]</strong>\r\n  </p>\r\n\r\n  <h2 style=\"margin-top: 30px; font-size: 20px;\">7. Updates to This Policy</h2>\r\n  <p>\r\n    We may update this Privacy Policy from time to time. Updates will be posted within the application.\r\n  </p>\r\n\r\n</div>");

            migrationBuilder.UpdateData(
                table: "SiteDocuments",
                keyColumn: "Id",
                keyValue: 2,
                column: "ContentsHTML",
                value: "<div style=\"max-width: 800px; margin: 40px auto; font-family: Arial, sans-serif; line-height: 1.6; color: #333;\">\r\n\r\n  <h1 style=\"font-size: 28px; margin-bottom: 10px;\">Terms of Service</h1>\r\n  <p style=\"font-size: 14px; color: #666;\">Last Updated: [Insert Date]</p>\r\n\r\n  <p>\r\n    By accessing or using this application, you agree to the following Terms of Service.\r\n  </p>\r\n\r\n  <h2 style=\"margin-top: 30px; font-size: 20px;\">1. Age Requirements</h2>\r\n  <p>\r\n    This application may only be used by individuals age 13 or older. \r\n    Individuals under the age of 13 may not create or own an account.\r\n  </p>\r\n  <p>\r\n    Parents or legal guardians may create and manage accounts on behalf of minors.\r\n  </p>\r\n\r\n  <h2 style=\"margin-top: 30px; font-size: 20px;\">2. Registration & Prizes</h2>\r\n  <p>\r\n    Registration or participation in the app does not guarantee any prizes, awards, or winnings.\r\n  </p>\r\n  <p>\r\n    The application is used solely for recording entries, submissions, and points. \r\n    All official drawings, selections, and contests are conducted outside of the application and in real life.\r\n  </p>\r\n  <p>\r\n    The application itself does not distribute, ship, or deliver prizes.\r\n  </p>\r\n\r\n  <h2 style=\"margin-top: 30px; font-size: 20px;\">3. Acceptable Use</h2>\r\n  <p>You agree not to:</p>\r\n  <ul style=\"padding-left: 20px;\">\r\n    <li>Use the application for unlawful purposes</li>\r\n    <li>Attempt to gain unauthorized access to accounts or systems</li>\r\n    <li>Abuse, exploit, or manipulate the point or contest system</li>\r\n    <li>Upload harmful, malicious, or inappropriate content</li>\r\n  </ul>\r\n\r\n  <h2 style=\"margin-top: 30px; font-size: 20px;\">4. API & Technical Access</h2>\r\n  <p>\r\n    Unauthorized access, use, or copying of the application's API, backend systems, or technical infrastructure is strictly prohibited.\r\n  </p>\r\n\r\n  <h2 style=\"margin-top: 30px; font-size: 20px;\">5. Intellectual Property</h2>\r\n  <p>\r\n    This application, including its design, features, content, and underlying systems, is the property of the <strong>Y.E.A.R.S Foundation</strong>.\r\n  </p>\r\n  <p>\r\n    You may not copy, reproduce, distribute, reverse engineer, or create derivative works of this application without written permission.\r\n  </p>\r\n\r\n  <h2 style=\"margin-top: 30px; font-size: 20px;\">6. Account Termination</h2>\r\n  <p>\r\n    We reserve the right to suspend or terminate accounts that violate these Terms or misuse the application.\r\n  </p>\r\n\r\n  <h2 style=\"margin-top: 30px; font-size: 20px;\">7. Limitation of Liability</h2>\r\n  <p>\r\n    The application is provided \"as is\" without warranties of any kind. \r\n    We are not responsible for technical errors, interruptions, data loss, or disputes related to contest outcomes.\r\n  </p>\r\n\r\n  <h2 style=\"margin-top: 30px; font-size: 20px;\">8. Changes to These Terms</h2>\r\n  <p>\r\n    We may update these Terms from time to time. Continued use of the application constitutes acceptance of any changes.\r\n  </p>\r\n\r\n</div>");

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3,
                column: "TaskEnabled",
                value: false);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 4,
                column: "TaskTitle",
                value: "Register an Account");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SiteDocuments",
                keyColumn: "Id",
                keyValue: 1,
                column: "ContentsHTML",
                value: "<div>\r\n\r\n    This is a test privacy policy. You agree to it, whatever it is.\r\n\r\n</div>");

            migrationBuilder.UpdateData(
                table: "SiteDocuments",
                keyColumn: "Id",
                keyValue: 2,
                column: "ContentsHTML",
                value: "<div>\r\n\r\n    This is a terms of service. You agree to it, whatever it is.\r\n\r\n</div>");

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3,
                column: "TaskEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 4,
                column: "TaskTitle",
                value: "Register an account with Biz Kidz Scholarships");
        }
    }
}
