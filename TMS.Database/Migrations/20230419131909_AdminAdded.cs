using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.Database.Migrations
{
    /// <inheritdoc />
    public partial class AdminAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "13f79e11-a23e-4742-a4d6-7470f8105e75", 0, "9712f1d7-45ba-483a-9a7d-36dfd729a752", null, false, false, null, null, null, "AQAAAAEAACcQAAAAEEcFJCWD/LEto/cGSwBYnG5QBf0wg0XWaCIb/3uTFL0x0nqoqdwSfkTMHjADXUCppA==", null, false, "f92f166b-f3cd-456f-ab85-d57de98d4885", false, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "13f79e11-a23e-4742-a4d6-7470f8105e75");
        }
    }
}
