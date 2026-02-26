using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BizKidzScholarships.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigS3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Tasks_TaskId",
                table: "Submissions");

            migrationBuilder.AlterColumn<int>(
                name: "TaskId",
                table: "Submissions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.InsertData(
                table: "Configuration",
                columns: new[] { "Id", "Created", "Updated", "Value" },
                values: new object[] { "S3BucketName", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "bizkidz-task-bucket" });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 26, 4, 3, 26, 126, DateTimeKind.Unspecified).AddTicks(2876), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 26, 4, 3, 26, 126, DateTimeKind.Unspecified).AddTicks(2883), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 26, 4, 3, 26, 126, DateTimeKind.Unspecified).AddTicks(2886), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 26, 4, 3, 26, 126, DateTimeKind.Unspecified).AddTicks(2887), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 26, 4, 3, 26, 126, DateTimeKind.Unspecified).AddTicks(2889), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 26, 4, 3, 26, 126, DateTimeKind.Unspecified).AddTicks(2892), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Tasks_TaskId",
                table: "Submissions",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Tasks_TaskId",
                table: "Submissions");

            migrationBuilder.DeleteData(
                table: "Configuration",
                keyColumn: "Id",
                keyValue: "S3BucketName");

            migrationBuilder.AlterColumn<int>(
                name: "TaskId",
                table: "Submissions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 24, 1, 41, 52, 444, DateTimeKind.Unspecified).AddTicks(5032), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 24, 1, 41, 52, 444, DateTimeKind.Unspecified).AddTicks(5038), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 24, 1, 41, 52, 444, DateTimeKind.Unspecified).AddTicks(5041), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 24, 1, 41, 52, 444, DateTimeKind.Unspecified).AddTicks(5042), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 24, 1, 41, 52, 444, DateTimeKind.Unspecified).AddTicks(5044), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 24, 1, 41, 52, 444, DateTimeKind.Unspecified).AddTicks(5044), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Tasks_TaskId",
                table: "Submissions",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
