using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace HouseRentingAPI.Data
{
    public class HouseRentingDbContext : DbContext
    {
        public HouseRentingDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Landlord> Landlords { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<HouseFacility> HouseFacilities { get; set; }
        public DbSet<HouseOtherAttribute> HouseOtherAttributes { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<OtherAttribute> OtherAttributes { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<ComparisonList> ComparisonLists { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<HousePhoto> HousesPhoto { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User to Favorite (one-to-many)
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserID);

            // House to Favorite (One-to-Many)
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.House)
                .WithMany(h => h.Favorites)
                .HasForeignKey(f => f.HouseID);

            // User to Comment (One-to-Many) 
            modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId);

            // House to Comment (One-to-Many)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.House)
                .WithMany(h => h.Comments)
                .HasForeignKey(c => c.HouseId);

            // HouseFacility to House (many-to-one)
            modelBuilder.Entity<HouseFacility>()
                .HasOne(hf => hf.House)
                .WithMany(h => h.HouseFacilities)
                .HasForeignKey(hf => hf.HouseID);

            // HouseFacility to Facility (many-to-one)
            modelBuilder.Entity<HouseFacility>()
                .HasOne(hf => hf.Facility)
                .WithMany(f => f.HouseFacilities)
                .HasForeignKey(hf => hf.FacilityID);

            // ComparisonList to User (many-to-one)
            modelBuilder.Entity<ComparisonList>()
                .HasOne(cl => cl.User)
                .WithMany(u => u.ComparisonLists)
                .HasForeignKey(cl => cl.UserID);

            // ComparisonList to House (many-to-one)
            modelBuilder.Entity<ComparisonList>()
                .HasOne(cl => cl.House)
                .WithMany(h => h.ComparisonLists)
                .HasForeignKey(cl => cl.HouseID);

            // House to PropertyType (many-to-one)
            modelBuilder.Entity<House>()
                .HasOne(h => h.PropertyType)
                .WithMany(pt => pt.Houses)
                .HasForeignKey(h => h.PropertyTypeID);

            // House to Landlord (one-to-many)
            modelBuilder.Entity<House>()
                .HasOne(h => h.Landlord)
                .WithMany(l => l.Houses)
                .HasForeignKey(h => h.LandlordID);

            // House to HouseOtherAttribute (One-to-Many)
            modelBuilder.Entity<HouseOtherAttribute>()
                .HasOne(hoa => hoa.House)
                .WithMany(h => h.HouseOtherAttributes)
                .HasForeignKey(hoa => hoa.HouseID);

            // HouseOtherAttribute has composite primary key
            modelBuilder.Entity<HouseOtherAttribute>()
                .HasKey(hf => new { hf.HouseID, hf.AttributeID });

            // HouseFacility has composite primary key
            modelBuilder.Entity<HouseFacility>()
                .HasKey(hf => new { hf.HouseID, hf.FacilityID });

            // Favorite has composite primary key
            modelBuilder.Entity<Favorite>()
                .HasKey(f => new { f.HouseID, f.UserID });

            // HousePhoto to House (many-to-one)
            modelBuilder.Entity<HousePhoto>()
                .HasOne(hp => hp.House)
                .WithMany(h => h.HousePhotos)
                .HasForeignKey(hp => hp.HouseID);

            // HousePhoto to Photo (many-to-one)
            modelBuilder.Entity<HousePhoto>()
                .HasOne(hp => hp.Photo)
                .WithMany()
                .HasForeignKey(hp => hp.PhotoID);
        }
    }
}
