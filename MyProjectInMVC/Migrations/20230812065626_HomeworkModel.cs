using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProjectInMVC.Migrations
{
    /// <inheritdoc />
    public partial class HomeworkModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Homeworks", // Nome da tabela no banco de dados
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 24, nullable: true),
                    Instructions = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    CategoryId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homeworks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Homeworks_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category", // Nome da tabela de categorias
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Homeworks"
            );
        }
    }
}
