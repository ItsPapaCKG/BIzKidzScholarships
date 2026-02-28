using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BizKidzScholarships.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserListDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.Sql(""" DROP VIEW "AdminUserList"; """);

            migrationBuilder.Sql("""
                CREATE OR REPLACE VIEW "AdminUserList" AS
                SELECT

                    "FirstName" || ' ' || "LastName" AS "Name", 
                    p."ChildFullName" AS "ChildFullName",
                    p."Email" AS "Email",
                    p."UserType" AS "UserType",
                    COALESCE(SUM(up."Points"),0) AS "Points", 
                    COALESCE(SUM(up."Points"), 0) / COALESCE(SELECT "Value"::int FROM "Configuration" WHERE "Id" = 'EntriesCost', 100) AS "Entries",
                    p."Created",
                    p."Updated"
                FROM "AspNetUsers" u
                LEFT JOIN "UserPoints" up ON u."Id" = up."UserId"
                INNER JOIN "Profiles" p ON p."UserId" = u."Id"
                GROUP BY u."UserId"
            """);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 28, 4, 24, 8, 449, DateTimeKind.Unspecified).AddTicks(8256), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 28, 4, 24, 8, 449, DateTimeKind.Unspecified).AddTicks(8265), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 28, 4, 24, 8, 449, DateTimeKind.Unspecified).AddTicks(8268), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 28, 4, 24, 8, 449, DateTimeKind.Unspecified).AddTicks(8268), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 28, 4, 24, 8, 449, DateTimeKind.Unspecified).AddTicks(8271), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 28, 4, 24, 8, 449, DateTimeKind.Unspecified).AddTicks(8271), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 28, 4, 24, 8, 449, DateTimeKind.Unspecified).AddTicks(8273), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 28, 4, 24, 8, 449, DateTimeKind.Unspecified).AddTicks(8274), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
