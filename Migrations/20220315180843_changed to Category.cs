using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnet_blog_mvc.Migrations
{
    public partial class changedtoCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Categories",
                table: "Posts",
                newName: "Category");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Posts",
                newName: "Categories");
        }
    }
}
