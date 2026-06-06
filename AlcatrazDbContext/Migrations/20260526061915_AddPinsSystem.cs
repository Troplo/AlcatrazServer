using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alcatraz.Context.Migrations
{
    /// <inheritdoc />
    public partial class AddPinsSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionTokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false, maxLength: 36),
                    UserId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoginPins",
                columns: table => new
                {
                    Pin = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    TokenId = table.Column<string>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginPins", x => x.Pin);
                    table.ForeignKey(
                        name: "FK_LoginPins_SessionTokens_TokenId",
                        column: x => x.TokenId,
                        principalTable: "SessionTokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoginPins_TokenId",
                table: "LoginPins",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionTokens_UserId",
                table: "SessionTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginPins");

            migrationBuilder.DropTable(
                name: "SessionTokens");
        }
    }
}
