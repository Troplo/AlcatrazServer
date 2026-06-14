using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alcatraz.Context.Migrations
{
    /// <inheritdoc />
    public partial class notoriety : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotorietyPoints",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotorietyPoints",
                table: "Users");
        }
    }
}
