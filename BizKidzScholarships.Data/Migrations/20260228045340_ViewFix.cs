using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BizKidzScholarships.Data.Migrations
{
    /// <inheritdoc />
    public partial class ViewFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(""" DROP VIEW "AdminUserList"; """);

            migrationBuilder.Sql("""
                CREATE OR REPLACE VIEW "AdminUserList" AS
                SELECT

                    "FirstName" || ' ' || "LastName" AS "Name", 
                    p."ChildFullName" AS "ChildFullName",
                    p."Email" AS "Email",
                    p."UserType" AS "UserType",
                    COALESCE(SUM(up."Points"),0) AS "Points", 
                    COALESCE(SUM(up."Points"), 0) / COALESCE((SELECT "Value"::int FROM "Configuration" WHERE "Id" = 'EntriesCost'), 100) AS "Entries",
                    p."Created",
                    p."Updated"
                FROM "AspNetUsers" u
                LEFT JOIN "UserPoints" up ON u."Id" = up."UserId"
                INNER JOIN "Profiles" p ON p."UserId" = u."Id"
                GROUP BY p."UserId"
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
