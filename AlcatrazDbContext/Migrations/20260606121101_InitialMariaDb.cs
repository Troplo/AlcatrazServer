using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alcatraz.Context.Migrations
{
    /// <inheritdoc />
    public partial class InitialMariaDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerStatisticBoards",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<uint>(type: "INTEGER", nullable: false),
                    BoardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Rank = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<float>(type: "REAL", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerStatisticBoards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Guid = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerNickName = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    RewardFlags = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerStatisticBoardValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerBoardId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PropertyId = table.Column<int>(type: "INTEGER", nullable: false),
                    ValueJSON = table.Column<string>(type: "TEXT", nullable: true),
                    RankingCriterionIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    SliceScoreJSON = table.Column<string>(type: "TEXT", nullable: true),
                    ScoreLostForNextSliceJSON = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerStatisticBoardValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerStatisticBoardValues_PlayerStatisticBoards_PlayerBoardId",
                        column: x => x.PlayerBoardId,
                        principalTable: "PlayerStatisticBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionTokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false),
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
                name: "UserRelationships",
                columns: table => new
                {
                    User1Id = table.Column<uint>(type: "INTEGER", nullable: false),
                    User2Id = table.Column<uint>(type: "INTEGER", nullable: false),
                    ByRelationShip = table.Column<uint>(type: "INTEGER", nullable: false),
                    Details = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRelationships", x => new { x.User1Id, x.User2Id });
                    table.ForeignKey(
                        name: "FK_UserRelationships_Users_User1Id",
                        column: x => x.User1Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRelationships_Users_User2Id",
                        column: x => x.User2Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoginPins",
                columns: table => new
                {
                    Pin = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    TokenId = table.Column<string>(type: "varchar(36)", nullable: false),
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
                name: "IX_PlayerStatisticBoardValues_PlayerBoardId",
                table: "PlayerStatisticBoardValues",
                column: "PlayerBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionTokens_UserId",
                table: "SessionTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRelationships_User2Id",
                table: "UserRelationships",
                column: "User2Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginPins");

            migrationBuilder.DropTable(
                name: "PlayerStatisticBoardValues");

            migrationBuilder.DropTable(
                name: "UserRelationships");

            migrationBuilder.DropTable(
                name: "SessionTokens");

            migrationBuilder.DropTable(
                name: "PlayerStatisticBoards");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
