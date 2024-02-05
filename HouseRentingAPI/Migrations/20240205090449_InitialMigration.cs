using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    FacilityID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.FacilityID);
                });

            migrationBuilder.CreateTable(
                name: "Landlords",
                columns: table => new
                {
                    LandlordID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Landlordname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LineID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landlords", x => x.LandlordID);
                });

            migrationBuilder.CreateTable(
                name: "OtherAttributes",
                columns: table => new
                {
                    AttributeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherAttributes", x => x.AttributeID);
                });

            migrationBuilder.CreateTable(
                name: "PropertyTypes",
                columns: table => new
                {
                    TypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyTypes", x => x.TypeID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StuId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Houses",
                columns: table => new
                {
                    HouseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LandlordID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropertyTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Distance = table.Column<int>(type: "int", nullable: false),
                    HouseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Houses", x => x.HouseID);
                    table.ForeignKey(
                        name: "FK_Houses_Landlords_LandlordID",
                        column: x => x.LandlordID,
                        principalTable: "Landlords",
                        principalColumn: "LandlordID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Houses_PropertyTypes_PropertyTypeID",
                        column: x => x.PropertyTypeID,
                        principalTable: "PropertyTypes",
                        principalColumn: "TypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComparisonLists",
                columns: table => new
                {
                    ComparisonID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HouseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComparisonLists", x => x.ComparisonID);
                    table.ForeignKey(
                        name: "FK_ComparisonLists_Houses_HouseID",
                        column: x => x.HouseID,
                        principalTable: "Houses",
                        principalColumn: "HouseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComparisonLists_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    FavoriteID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HouseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.FavoriteID);
                    table.ForeignKey(
                        name: "FK_Favorites_Houses_HouseID",
                        column: x => x.HouseID,
                        principalTable: "Houses",
                        principalColumn: "HouseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HouseFacilities",
                columns: table => new
                {
                    HouseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseFacilities", x => new { x.HouseID, x.FacilityID });
                    table.ForeignKey(
                        name: "FK_HouseFacilities_Facilities_FacilityID",
                        column: x => x.FacilityID,
                        principalTable: "Facilities",
                        principalColumn: "FacilityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouseFacilities_Houses_HouseID",
                        column: x => x.HouseID,
                        principalTable: "Houses",
                        principalColumn: "HouseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HouseOtherAttributes",
                columns: table => new
                {
                    HouseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseOtherAttributes", x => new { x.HouseID, x.AttributeID });
                    table.ForeignKey(
                        name: "FK_HouseOtherAttributes_Houses_HouseID",
                        column: x => x.HouseID,
                        principalTable: "Houses",
                        principalColumn: "HouseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouseOtherAttributes_OtherAttributes_AttributeID",
                        column: x => x.AttributeID,
                        principalTable: "OtherAttributes",
                        principalColumn: "AttributeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComparisonLists_HouseID",
                table: "ComparisonLists",
                column: "HouseID");

            migrationBuilder.CreateIndex(
                name: "IX_ComparisonLists_UserID",
                table: "ComparisonLists",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_HouseID",
                table: "Favorites",
                column: "HouseID");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserID",
                table: "Favorites",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_HouseFacilities_FacilityID",
                table: "HouseFacilities",
                column: "FacilityID");

            migrationBuilder.CreateIndex(
                name: "IX_HouseOtherAttributes_AttributeID",
                table: "HouseOtherAttributes",
                column: "AttributeID");

            migrationBuilder.CreateIndex(
                name: "IX_Houses_LandlordID",
                table: "Houses",
                column: "LandlordID");

            migrationBuilder.CreateIndex(
                name: "IX_Houses_PropertyTypeID",
                table: "Houses",
                column: "PropertyTypeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComparisonLists");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "HouseFacilities");

            migrationBuilder.DropTable(
                name: "HouseOtherAttributes");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Facilities");

            migrationBuilder.DropTable(
                name: "Houses");

            migrationBuilder.DropTable(
                name: "OtherAttributes");

            migrationBuilder.DropTable(
                name: "Landlords");

            migrationBuilder.DropTable(
                name: "PropertyTypes");
        }
    }
}
