using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafePOS.Migrations
{
    /// <inheritdoc />
    public partial class cafeTablemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CafeTableId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "cafeTables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TableNumber = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cafeTables", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CafeTableId",
                table: "Orders",
                column: "CafeTableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_cafeTables_CafeTableId",
                table: "Orders",
                column: "CafeTableId",
                principalTable: "cafeTables",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_cafeTables_CafeTableId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "cafeTables");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CafeTableId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CafeTableId",
                table: "Orders");
        }
    }
}
