using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoMeal.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Business",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Business");
        }
    }
}
