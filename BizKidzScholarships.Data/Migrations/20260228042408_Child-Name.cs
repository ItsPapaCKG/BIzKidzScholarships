using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BizKidzScholarships.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChildName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChildFullName",
                table: "Profiles",
                type: "text",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(""" DROP VIEW "AdminUserList"; """);

            migrationBuilder.DropColumn(
                name: "ChildFullName",
                table: "Profiles");

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 28, 3, 53, 29, 809, DateTimeKind.Unspecified).AddTicks(1680), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 28, 3, 53, 29, 809, DateTimeKind.Unspecified).AddTicks(1691), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 28, 3, 53, 29, 809, DateTimeKind.Unspecified).AddTicks(1694), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 28, 3, 53, 29, 809, DateTimeKind.Unspecified).AddTicks(1695), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 28, 3, 53, 29, 809, DateTimeKind.Unspecified).AddTicks(1697), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 28, 3, 53, 29, 809, DateTimeKind.Unspecified).AddTicks(1698), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 2, 28, 3, 53, 29, 809, DateTimeKind.Unspecified).AddTicks(1700), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 2, 28, 3, 53, 29, 809, DateTimeKind.Unspecified).AddTicks(1703), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
