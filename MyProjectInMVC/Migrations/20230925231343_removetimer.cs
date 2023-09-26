using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProjectInMVC.Migrations
{
    /// <inheritdoc />
    public partial class removetimer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeMessage",
                table: "Chat");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeMessage",
                table: "Chat",
                type: "longtext",
                nullable: true);
        }
    }
}
