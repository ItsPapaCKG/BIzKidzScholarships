using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BizKidzScholarships.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(""" DROP VIEW "AdminUserList"; """);
            migrationBuilder.Sql("""
                CREATE OR REPLACE VIEW "AdminUserList" AS
            SELECT
                u."Id" AS "UserId",
                p."FirstName" || ' ' || p."LastName" AS "Name", 
                p."ChildFullName",
                p."Email",
                p."UserType",
                COALESCE(up."Points", 0) AS "Points",
                COALESCE(up."Points", 0) / 
                    COALESCE((SELECT "Value"::int 
                              FROM "Configuration" 
                              WHERE "Id" = 'EntriesCost'), 100) AS "Entries",
                p."Created",
                p."Updated"
            FROM "AspNetUsers" u
            INNER JOIN "Profiles" p ON p."UserId" = u."Id"
            LEFT JOIN (
                SELECT "UserId", SUM("Points") AS "Points"
                FROM "UserPoints"
                GROUP BY "UserId"
            ) up ON u."Id" = up."UserId";
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(""" DROP VIEW "AdminUserList"; """);
        }
    }
}
