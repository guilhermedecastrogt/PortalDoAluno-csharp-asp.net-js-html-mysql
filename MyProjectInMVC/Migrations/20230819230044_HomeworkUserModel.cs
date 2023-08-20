using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProjectInMVC.Migrations
{
    /// <inheritdoc />
    public partial class HomeworkUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeworkCategory");

            migrationBuilder.CreateTable(
                name: "HomeworkUserModel",
                columns: table => new
                {
                    HomeworkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeworkUserModel", x => x.HomeworkId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeworkUserModel");

            migrationBuilder.CreateTable(
                name: "HomeworkCategory",
                columns: table => new
                {
                    HomeworkCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HomeworkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeworkCategory", x => x.HomeworkCategoryId);
                });
        }
    }
}
