using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZenBlog.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categoriess");

            migrationBuilder.RenameColumn(
                name: "CateName",
                table: "Categoriess",
                newName: "CategoriesName");

            migrationBuilder.RenameColumn(
                name: "CateId",
                table: "Categoriess",
                newName: "CategoriesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categoriess",
                table: "Categoriess",
                column: "CategoriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Categoriess",
                table: "Categoriess");

            migrationBuilder.RenameTable(
                name: "Categoriess",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "CategoriesName",
                table: "Category",
                newName: "CateName");

            migrationBuilder.RenameColumn(
                name: "CategoriesId",
                table: "Category",
                newName: "CateId");
        }
    }
}
