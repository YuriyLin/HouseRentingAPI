using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModifyPropetyTypeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Houses_PropertyTypes_PropertyTypeID",
                table: "Houses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyTypes",
                table: "PropertyTypes");

            migrationBuilder.DropColumn(
                name: "TypeID",
                table: "PropertyTypes");

            migrationBuilder.AddColumn<int>(
                name: "PropertyTypeID",
                table: "PropertyTypes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "AttributeID",
                table: "OtherAttributes",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyTypeID",
                table: "Houses",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "SquareFeet",
                table: "Houses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "AttributeID",
                table: "HouseOtherAttributes",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "FacilityID",
                table: "HouseFacilities",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "FacilityID",
                table: "Facilities",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyTypes",
                table: "PropertyTypes",
                column: "PropertyTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_PropertyTypes_PropertyTypeID",
                table: "Houses",
                column: "PropertyTypeID",
                principalTable: "PropertyTypes",
                principalColumn: "PropertyTypeID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Houses_PropertyTypes_PropertyTypeID",
                table: "Houses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyTypes",
                table: "PropertyTypes");

            migrationBuilder.DropColumn(
                name: "PropertyTypeID",
                table: "PropertyTypes");

            migrationBuilder.DropColumn(
                name: "SquareFeet",
                table: "Houses");

            migrationBuilder.AddColumn<Guid>(
                name: "TypeID",
                table: "PropertyTypes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AttributeID",
                table: "OtherAttributes",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<Guid>(
                name: "PropertyTypeID",
                table: "Houses",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "AttributeID",
                table: "HouseOtherAttributes",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "FacilityID",
                table: "HouseFacilities",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "FacilityID",
                table: "Facilities",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyTypes",
                table: "PropertyTypes",
                column: "TypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_PropertyTypes_PropertyTypeID",
                table: "Houses",
                column: "PropertyTypeID",
                principalTable: "PropertyTypes",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
