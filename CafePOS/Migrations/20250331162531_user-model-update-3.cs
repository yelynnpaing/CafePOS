using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafePOS.Migrations
{
    /// <inheritdoc />
    public partial class usermodelupdate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserRole",
                table: "AspNetUsers",
                newName: "NewRole");

            migrationBuilder.AddColumn<string>(
                name: "AvailableRoles",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrentRoles",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableRoles",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CurrentRoles",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "NewRole",
                table: "AspNetUsers",
                newName: "UserRole");
        }
    }
}
