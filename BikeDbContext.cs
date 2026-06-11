using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CarMagazine2025_42.Data.Models;
using CarMagazine2025_42.Data.Models.Cart;
using CarMagazine2025_42.Data.Models.Order;

namespace CarMagazine2025_42.Data.Context
{
    public class BikeDbContext : IdentityDbContext<ApplicationUser>
    {
        public BikeDbContext(DbContextOptions<BikeDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<ShopCartItem> ShopCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка точности для всех полей Price (деньги)
            modelBuilder.Entity<Bike>().Property(b => b.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ShopCartItem>().Property(s => s.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderDetail>().Property(od => od.Price).HasColumnType("decimal(18,2)");

            // Явное указание связи (Fluent API)
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Bike)
                .WithMany()
                .HasForeignKey(od => od.BikeId);

            // Начальные данные (Seed Data)
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Горные велосипеды", Description = "Для пересеченной местности." },
                new Category { CategoryId = 2, CategoryName = "Шоссейные", Description = "Для скорости на асфальте." },
                new Category { CategoryId = 3, CategoryName = "Городские", Description = "Для комфортных поездок." }
            );
        }
    }
}