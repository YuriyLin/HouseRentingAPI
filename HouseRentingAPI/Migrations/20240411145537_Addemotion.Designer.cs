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
    [Migration("20240411145537_Addemotion")]
    partial class Addemotion
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HouseRentingAPI.Data.Comment", b =>
                {
                    b.Property<Guid>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CommentText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("HouseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("emotionresult")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CommentId");

                    b.HasIndex("HouseId");

                    b.HasIndex("UserId");

                    b.ToTable("Comment");
                });

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
                    b.Property<int>("FacilityID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FacilityID"));

                    b.Property<string>("FacilityName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FacilityID");

                    b.ToTable("Facilities");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.Favorite", b =>
                {
                    b.Property<Guid>("HouseID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("HouseID", "UserID");

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

                    b.Property<int>("PropertyTypeID")
                        .HasColumnType("int");

                    b.Property<string>("SquareFeet")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("HouseID");

                    b.HasIndex("LandlordID");

                    b.HasIndex("PropertyTypeID");

                    b.ToTable("Houses");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.HouseFacility", b =>
                {
                    b.Property<Guid>("HouseID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("FacilityID")
                        .HasColumnType("int");

                    b.HasKey("HouseID", "FacilityID");

                    b.HasIndex("FacilityID");

                    b.ToTable("HouseFacilities");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.HouseOtherAttribute", b =>
                {
                    b.Property<Guid>("HouseID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AttributeID")
                        .HasColumnType("int");

                    b.HasKey("HouseID", "AttributeID");

                    b.HasIndex("AttributeID");

                    b.ToTable("HouseOtherAttributes");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.HousePhoto", b =>
                {
                    b.Property<Guid>("HouseID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PhotoID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsCoverPhoto")
                        .HasColumnType("bit");

                    b.HasKey("HouseID", "PhotoID");

                    b.HasIndex("PhotoID");

                    b.ToTable("HousesPhoto");
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
                    b.Property<int>("AttributeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AttributeID"));

                    b.Property<string>("AttributeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AttributeID");

                    b.ToTable("OtherAttributes");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.Photo", b =>
                {
                    b.Property<Guid>("PhotoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PhotoURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PhotoID");

                    b.ToTable("Photo");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.PropertyType", b =>
                {
                    b.Property<int>("PropertyTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PropertyTypeID"));

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PropertyTypeID");

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

                    b.Property<string>("StudentIdCardPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.Comment", b =>
                {
                    b.HasOne("HouseRentingAPI.Data.House", "House")
                        .WithMany("Comments")
                        .HasForeignKey("HouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HouseRentingAPI.Data.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("House");

                    b.Navigation("User");
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

            modelBuilder.Entity("HouseRentingAPI.Data.HousePhoto", b =>
                {
                    b.HasOne("HouseRentingAPI.Data.House", "House")
                        .WithMany("HousePhotos")
                        .HasForeignKey("HouseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HouseRentingAPI.Data.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("House");

                    b.Navigation("Photo");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.Facility", b =>
                {
                    b.Navigation("HouseFacilities");
                });

            modelBuilder.Entity("HouseRentingAPI.Data.House", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("ComparisonLists");

                    b.Navigation("Favorites");

                    b.Navigation("HouseFacilities");

                    b.Navigation("HouseOtherAttributes");

                    b.Navigation("HousePhotos");
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
                    b.Navigation("Comments");

                    b.Navigation("ComparisonLists");

                    b.Navigation("Favorites");
                });
#pragma warning restore 612, 618
        }
    }
}
