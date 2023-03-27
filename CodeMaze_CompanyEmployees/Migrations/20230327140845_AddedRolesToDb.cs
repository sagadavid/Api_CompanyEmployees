using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CodeMaze_CompanyEmployees.Migrations
{
    /// <inheritdoc />
    public partial class AddedRolesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "661c2bfe-198b-4283-9c6e-26499a2bcf72", "3c881322-c1b4-42a1-aa25-f4f5cd7c041e", "Administrator", "ADMINISTRATOR" },
                    { "f31cadfc-1ad9-4bdb-9ecf-291869a2ddca", "648f486b-e6dd-4e0b-9f11-c288fec30c9c", "Manager", "MANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "661c2bfe-198b-4283-9c6e-26499a2bcf72");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f31cadfc-1ad9-4bdb-9ecf-291869a2ddca");
        }
    }
}
