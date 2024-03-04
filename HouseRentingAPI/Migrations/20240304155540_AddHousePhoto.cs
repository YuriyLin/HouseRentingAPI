using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddHousePhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    PhotoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoURL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.PhotoID);
                });

            migrationBuilder.CreateTable(
                name: "HousesPhoto",
                columns: table => new
                {
                    HousePhotoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HouseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsCoverPhoto = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HousesPhoto", x => x.HousePhotoID);
                    table.ForeignKey(
                        name: "FK_HousesPhoto_Houses_HouseID",
                        column: x => x.HouseID,
                        principalTable: "Houses",
                        principalColumn: "HouseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HousesPhoto_Photos_PhotoID",
                        column: x => x.PhotoID,
                        principalTable: "Photos",
                        principalColumn: "PhotoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HousesPhoto_HouseID",
                table: "HousesPhoto",
                column: "HouseID");

            migrationBuilder.CreateIndex(
                name: "IX_HousesPhoto_PhotoID",
                table: "HousesPhoto",
                column: "PhotoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HousesPhoto");

            migrationBuilder.DropTable(
                name: "Photos");
        }
    }
}
