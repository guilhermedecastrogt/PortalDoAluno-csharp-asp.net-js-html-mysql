using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProjectInMVC.Migrations
{
    /// <inheritdoc />
    public partial class HomeworkCategoryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomeworkCategory",
                columns: table => new
                {
                    HomeworkCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HomeworkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeworkCategory", x => x.HomeworkCategoryId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeworkCategory");
        }
    }
}
