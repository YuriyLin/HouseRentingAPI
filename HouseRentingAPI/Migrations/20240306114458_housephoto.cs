using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingAPI.Migrations
{
    /// <inheritdoc />
    public partial class housephoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HousesPhoto_Photos_PhotoID",
                table: "HousesPhoto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HousesPhoto",
                table: "HousesPhoto");

            migrationBuilder.DropIndex(
                name: "IX_HousesPhoto_HouseID",
                table: "HousesPhoto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos",
                table: "Photos");

            migrationBuilder.RenameTable(
                name: "Photos",
                newName: "Photo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HousesPhoto",
                table: "HousesPhoto",
                columns: new[] { "HouseID", "PhotoID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photo",
                table: "Photo",
                column: "PhotoID");

            migrationBuilder.AddForeignKey(
                name: "FK_HousesPhoto_Photo_PhotoID",
                table: "HousesPhoto",
                column: "PhotoID",
                principalTable: "Photo",
                principalColumn: "PhotoID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HousesPhoto_Photo_PhotoID",
                table: "HousesPhoto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HousesPhoto",
                table: "HousesPhoto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photo",
                table: "Photo");

            migrationBuilder.RenameTable(
                name: "Photo",
                newName: "Photos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HousesPhoto",
                table: "HousesPhoto",
                column: "HousePhotoID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photos",
                table: "Photos",
                column: "PhotoID");

            migrationBuilder.CreateIndex(
                name: "IX_HousesPhoto_HouseID",
                table: "HousesPhoto",
                column: "HouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_HousesPhoto_Photos_PhotoID",
                table: "HousesPhoto",
                column: "PhotoID",
                principalTable: "Photos",
                principalColumn: "PhotoID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
