using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddcolumnUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvatarNum",
                table: "User",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarNum",
                table: "User");
        }
    }
}
