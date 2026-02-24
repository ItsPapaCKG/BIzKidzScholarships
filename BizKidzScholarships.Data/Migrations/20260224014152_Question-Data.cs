using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BizKidzScholarships.Data.Migrations
{
    /// <inheritdoc />
    public partial class QuestionData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "QuizOptions",
                columns: new[] { "OptionId", "Created", "OptionKey", "OptionValue", "Updated" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2026, 2, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "A", "Kidz Bidz", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 2, new DateTimeOffset(new DateTime(2026, 2, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "B", "Microsoft", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 3, new DateTimeOffset(new DateTime(2026, 2, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "C", "Y.E.A.R.S Foundation", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4, new DateTimeOffset(new DateTime(2026, 2, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "D", "Google", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 5, new DateTimeOffset(new DateTime(2026, 2, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "A", "The name of a band", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 6, new DateTimeOffset(new DateTime(2026, 2, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "B", "The cost of performing services in your business", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 7, new DateTimeOffset(new DateTime(2026, 2, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "C", "Scholarships won by Biz Kidz", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 8, new DateTimeOffset(new DateTime(2026, 2, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "D", "The net income gained after expenses are deducted", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 9, new DateTimeOffset(new DateTime(2026, 2, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "A", "Answer A", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 10, new DateTimeOffset(new DateTime(2026, 2, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "B", "Answer B", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 11, new DateTimeOffset(new DateTime(2026, 2, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "C", "Answer C", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 12, new DateTimeOffset(new DateTime(2026, 2, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "D", "Answer D", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "QuizQuestions",
                columns: new[] { "QuestionId", "Created", "Multi", "Prompt", "PromptImageKey", "Updated" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2026, 2, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "What is the name of the non-profit organization that makes Biz Kidz possible?", "https://bizkidz-task-bucket.s3.us-east-2.amazonaws.com/static/biz-kidz-usa-logo.avif", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 2, new DateTimeOffset(new DateTime(2026, 2, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "What is profit?", "https://bizkidz-task-bucket.s3.us-east-2.amazonaws.com/static/entrepreneurshipguide.png", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 3, new DateTimeOffset(new DateTime(2026, 2, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "Select Answer D.", "https://bizkidz-task-bucket.s3.us-east-2.amazonaws.com/static/years-logo.avif", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

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

            migrationBuilder.InsertData(
                table: "QuestionOptions",
                columns: new[] { "OptionId", "QuestionId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 2 },
                    { 6, 2 },
                    { 7, 2 },
                    { 8, 2 },
                    { 9, 3 },
                    { 10, 3 },
                    { 11, 3 },
                    { 12, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "QuestionOptions",
                keyColumns: new[] { "OptionId", "QuestionId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "QuestionOptions",
                keyColumns: new[] { "OptionId", "QuestionId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "QuestionOptions",
                keyColumns: new[] { "OptionId", "QuestionId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "QuestionOptions",
                keyColumns: new[] { "OptionId", "QuestionId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "QuestionOptions",
                keyColumns: new[] { "OptionId", "QuestionId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "QuestionOptions",
                keyColumns: new[] { "OptionId", "QuestionId" },
                keyValues: new object[] { 6, 2 });

            migrationBuilder.DeleteData(
                table: "QuestionOptions",
                keyColumns: new[] { "OptionId", "QuestionId" },
                keyValues: new object[] { 7, 2 });

            migrationBuilder.DeleteData(
                table: "QuestionOptions",
                keyColumns: new[] { "OptionId", "QuestionId" },
                keyValues: new object[] { 8, 2 });

            migrationBuilder.DeleteData(
                table: "QuestionOptions",
                keyColumns: new[] { "OptionId", "QuestionId" },
                keyValues: new object[] { 9, 3 });

            migrationBuilder.DeleteData(
                table: "QuestionOptions",
                keyColumns: new[] { "OptionId", "QuestionId" },
                keyValues: new object[] { 10, 3 });

            migrationBuilder.DeleteData(
                table: "QuestionOptions",
                keyColumns: new[] { "OptionId", "QuestionId" },
                keyValues: new object[] { 11, 3 });

            migrationBuilder.DeleteData(
                table: "QuestionOptions",
                keyColumns: new[] { "OptionId", "QuestionId" },
                keyValues: new object[] { 12, 3 });

            migrationBuilder.DeleteData(
                table: "QuizOptions",
                keyColumn: "OptionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "QuizOptions",
                keyColumn: "OptionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "QuizOptions",
                keyColumn: "OptionId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "QuizOptions",
                keyColumn: "OptionId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "QuizOptions",
                keyColumn: "OptionId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "QuizOptions",
                keyColumn: "OptionId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "QuizOptions",
                keyColumn: "OptionId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "QuizOptions",
                keyColumn: "OptionId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "QuizOptions",
                keyColumn: "OptionId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "QuizOptions",
                keyColumn: "OptionId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "QuizOptions",
                keyColumn: "OptionId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "QuizOptions",
                keyColumn: "OptionId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "QuizQuestions",
                keyColumn: "QuestionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "QuizQuestions",
                keyColumn: "QuestionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "QuizQuestions",
                keyColumn: "QuestionId",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 23, 1, 58, 37, 159, DateTimeKind.Unspecified).AddTicks(9412), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 23, 1, 58, 37, 159, DateTimeKind.Unspecified).AddTicks(9420), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 23, 1, 58, 37, 159, DateTimeKind.Unspecified).AddTicks(9423), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 23, 1, 58, 37, 159, DateTimeKind.Unspecified).AddTicks(9424), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 23, 1, 58, 37, 159, DateTimeKind.Unspecified).AddTicks(9426), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 23, 1, 58, 37, 159, DateTimeKind.Unspecified).AddTicks(9427), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
