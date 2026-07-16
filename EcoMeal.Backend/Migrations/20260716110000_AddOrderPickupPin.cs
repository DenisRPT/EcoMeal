using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoMeal.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderPickupPin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PickupPin",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickupPin",
                table: "Orders");
        }
    }
}