using KitabhChauta.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using kitabhChauta.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace kitabhChauta.DbContext
{
    public class KitabhChautariDbContext : IdentityDbContext<IdentityUser>
    {
        public KitabhChautariDbContext(DbContextOptions<KitabhChautariDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Book Configuration
            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(b => b.ISBN)
                    .IsRequired()
                    .HasMaxLength(17);

                entity.Property(b => b.Price)
                    .HasColumnType("decimal(18,2)");

                entity.Property(b => b.PublishedDate)
                    .HasConversion(
                        v => v.ToUniversalTime(),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

                entity.HasIndex(b => b.ISBN);
            });

            // Member Configuration
            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(m => m.MemberId);
                entity.Property(m => m.MemberId).ValueGeneratedOnAdd();
                entity.Property(m => m.UserId).IsRequired().HasMaxLength(450);
                entity.Property(m => m.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(m => m.LastName).IsRequired().HasMaxLength(100);
                entity.Property(m => m.Email).IsRequired(false).HasMaxLength(256);
                entity.Property(m => m.DateOfBirth).HasColumnType("date").IsRequired(false);
                entity.Property(m => m.RegistrationDate).HasColumnType("timestamp with time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(m => m.ContactNo).IsRequired().HasMaxLength(20);
                entity.Property(m => m.IsStaff).HasDefaultValue(false);
                entity.HasIndex(m => m.ContactNo).IsUnique();
                entity.HasIndex(m => m.Email).IsUnique().HasFilter("\"Email\" IS NOT NULL");
                entity.HasOne<IdentityUser>()
                    .WithOne()
                    .HasForeignKey<Member>(m => m.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Staff Configuration
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasKey(s => s.StaffId);
                entity.Property(s => s.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(s => s.LastName).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Email).IsRequired().HasMaxLength(256);
                entity.Property(s => s.ContactNo).IsRequired().HasMaxLength(20);
                entity.Property(s => s.Username).IsRequired().HasMaxLength(50);
                entity.HasIndex(s => s.Email).IsUnique();
                entity.HasIndex(s => s.Username).IsUnique();
            });

            // Admin Configuration
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(a => a.AdminId);
                entity.Property(a => a.Name).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Email).IsRequired().HasMaxLength(256);
                entity.Property(a => a.Role).IsRequired().HasMaxLength(50);
                entity.Property(a => a.CreatedAt).HasColumnType("timestamp with time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(a => a.UpdatedAt).HasColumnType("timestamp with time zone").IsRequired(false);
                entity.Property(a => a.IsActive).HasDefaultValue(true);
                entity.HasIndex(a => a.Email).IsUnique();
            });

            // Author Configuration
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.Author_id);
                entity.Property(a => a.Author_Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.HasMany(a => a.Books)
                    .WithOne(b => b.Author)
                    .HasForeignKey(b => b.Author_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Genre Configuration
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(g => g.Genre_id);
                entity.Property(g => g.Genre_Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.HasMany(g => g.Books)
                    .WithOne(b => b.Genre)
                    .HasForeignKey(b => b.Genre_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Publisher Configuration
            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.HasKey(p => p.Publisher_id);
                entity.Property(p => p.Publisher_Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.HasMany(p => p.Books)
                    .WithOne(b => b.Publisher)
                    .HasForeignKey(b => b.Publisher_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Cart Configuration
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasOne(c => c.Member)
                    .WithOne()
                    .HasForeignKey<Cart>(c => c.MemberId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasOne(ci => ci.Cart)
                    .WithMany(c => c.CartItems)
                    .HasForeignKey(ci => ci.CartId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(ci => ci.Book)
                    .WithMany()
                    .HasForeignKey(ci => ci.BookId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Wishlist Configuration
            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.HasOne(w => w.Member)
                    .WithOne()
                    .HasForeignKey<Wishlist>(w => w.MemberId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WishlistItem>(entity =>
            {
                entity.HasOne(wi => wi.Wishlist)
                    .WithMany(w => w.WishlistItems)
                    .HasForeignKey(wi => wi.WishlistId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(wi => wi.Book)
                    .WithMany()
                    .HasForeignKey(wi => wi.BookId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Order Configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne(o => o.Member)
                    .WithMany()
                    .HasForeignKey(o => o.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(oi => oi.Book)
                    .WithMany()
                    .HasForeignKey(oi => oi.BookId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}