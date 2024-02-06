﻿// <auto-generated />
using System;
using HouseRentingAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HouseRentingAPI.Migrations
{
    [DbContext(typeof(HouseRentingDbContext))]
    [Migration("20240206070307_AddNewColumn")]
    partial class AddNewColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HouseRentingAPI.Data.ComparisonList", b =>
                {
                    b.Property<Guid>("ComparisonID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HouseID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ComparisonID");

                    b.HasIndex("HouseID");

                    b.HasIndex("UserID");

                    b.ToTable("ComparisonLists");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.Facility", b =>
                {
                    b.Property<Guid>("FacilityID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FacilityName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FacilityID");

                    b.ToTable("Facilities");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.Favorite", b =>
                {
                    b.Property<Guid>("FavoriteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HouseID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FavoriteID");

                    b.HasIndex("HouseID");

                    b.HasIndex("UserID");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.House", b =>
                {
                    b.Property<Guid>("HouseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Distance")
                        .HasColumnType("int");

                    b.Property<string>("HouseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("LandlordID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<Guid>("PropertyTypeID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("HouseID");

                    b.HasIndex("LandlordID");

                    b.HasIndex("PropertyTypeID");

                    b.ToTable("Houses");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.HouseFacility", b =>
                {
                    b.Property<Guid>("HouseID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FacilityID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("HouseID", "FacilityID");

                    b.HasIndex("FacilityID");

                    b.ToTable("HouseFacilities");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.HouseOtherAttribute", b =>
                {
                    b.Property<Guid>("HouseID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AttributeID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("HouseID", "AttributeID");

                    b.HasIndex("AttributeID");

                    b.ToTable("HouseOtherAttributes");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.Landlord", b =>
                {
                    b.Property<Guid>("LandlordID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Landlordname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LineID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LandlordID");

                    b.ToTable("Landlords");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.OtherAttribute", b =>
                {
                    b.Property<Guid>("AttributeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AttributeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AttributeID");

                    b.ToTable("OtherAttributes");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.PropertyType", b =>
                {
                    b.Property<Guid>("TypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TypeID");

                    b.ToTable("PropertyTypes");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNum")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StuId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.ComparisonList", b =>
                {
                    b.HasOne("HouseRentingAPI.Data.House", "House")
                        .WithMany("ComparisonLists")
                        .HasForeignKey("HouseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HouseRentingAPI.Data.User", "User")
                        .WithMany("ComparisonLists")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("House");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.Favorite", b =>
                {
                    b.HasOne("HouseRentingAPI.Data.House", "House")
                        .WithMany("Favorites")
                        .HasForeignKey("HouseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HouseRentingAPI.Data.User", "User")
                        .WithMany("Favorites")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("House");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.House", b =>
                {
                    b.HasOne("HouseRentingAPI.Data.Landlord", "Landlord")
                        .WithMany("Houses")
                        .HasForeignKey("LandlordID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HouseRentingAPI.Data.PropertyType", "PropertyType")
                        .WithMany("Houses")
                        .HasForeignKey("PropertyTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Landlord");

                    b.Navigation("PropertyType");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.HouseFacility", b =>
                {
                    b.HasOne("HouseRentingAPI.Data.Facility", "Facility")
                        .WithMany("HouseFacilities")
                        .HasForeignKey("FacilityID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HouseRentingAPI.Data.House", "House")
                        .WithMany("HouseFacilities")
                        .HasForeignKey("HouseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Facility");

                    b.Navigation("House");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.HouseOtherAttribute", b =>
                {
                    b.HasOne("HouseRentingAPI.Data.OtherAttribute", "OtherAttribute")
                        .WithMany("HouseOtherAttributes")
                        .HasForeignKey("AttributeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HouseRentingAPI.Data.House", "House")
                        .WithMany("HouseOtherAttributes")
                        .HasForeignKey("HouseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("House");

                    b.Navigation("OtherAttribute");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.Facility", b =>
                {
                    b.Navigation("HouseFacilities");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.House", b =>
                {
                    b.Navigation("ComparisonLists");

                    b.Navigation("Favorites");

                    b.Navigation("HouseFacilities");

                    b.Navigation("HouseOtherAttributes");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.Landlord", b =>
                {
                    b.Navigation("Houses");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.OtherAttribute", b =>
                {
                    b.Navigation("HouseOtherAttributes");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.PropertyType", b =>
                {
                    b.Navigation("Houses");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.User", b =>
                {
                    b.Navigation("ComparisonLists");

                    b.Navigation("Favorites");
                });
#pragma warning restore 612, 618
        }
    }
}
