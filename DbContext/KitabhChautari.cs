using KitabhChauta.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

public class KitabhChautariDbContext : DbContext
{
    public KitabhChautariDbContext(DbContextOptions<KitabhChautariDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; } = default!;

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

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.BookId);

            entity.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);


            entity.Property(b => b.ISBN)
                .IsRequired()
                .HasMaxLength(13);

            entity.Property(b => b.Price)
                .HasColumnType("decimal(18,2)");

            entity.Property(b => b.PublishedDate)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            // Optional index example
            entity.HasIndex(b => b.ISBN);
        });
        // Member Configuration
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(m => m.MemberId);

            entity.Property(m => m.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(m => m.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(m => m.Email)
                .IsRequired();

            entity.Property(m => m.DateOfBirth)
                .HasColumnType("date");

            entity.Property(m => m.RegistrationDate)
                .IsRequired()
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));


            // Unique index on Email
            entity.HasIndex(m => m.Email)
                .IsUnique();

           
            
        });


        

       
        // Author Configuration
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(a => a.Author_id);

            entity.Property(a => a.Author_Name)
                .IsRequired()
                .HasMaxLength(100);

            // One-to-Many with Book
            entity.HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.Author_id)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete
        });

        // Genre Configuration
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(g => g.Genre_id);

            entity.Property(g => g.Genre_Name)
                .IsRequired()
                .HasMaxLength(100);

            // One-to-Many with Book
            entity.HasMany(g => g.Books)
                .WithOne(b => b.Genre)
                .HasForeignKey(b => b.Genre_id)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete
        });

        // Publisher Configuration
        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(p => p.Publisher_id);

            entity.Property(p => p.Publisher_Name)
                .IsRequired()
                .HasMaxLength(100);

            // One-to-Many with Book
            entity.HasMany(p => p.Books)
                .WithOne(b => b.Publisher)
                .HasForeignKey(b => b.Publisher_id)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete
        });


        // Cart Configuration
        // Configure relationships
        modelBuilder.Entity<Cart>()
            .HasOne(c => c.Member)
            .WithOne()
            .HasForeignKey<Cart>(c => c.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Cart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Book)
            .WithMany()
            .HasForeignKey(ci => ci.BookId)
            .OnDelete(DeleteBehavior.Restrict);


        // Wishlist Configuration
        modelBuilder.Entity<Wishlist>()
           .HasOne(w => w.Member)
           .WithOne()
           .HasForeignKey<Wishlist>(w => w.MemberId)
           .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WishlistItem>()
            .HasOne(wi => wi.Wishlist)
            .WithMany(w => w.WishlistItems)
            .HasForeignKey(wi => wi.WishlistId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WishlistItem>()
            .HasOne(wi => wi.Book)
            .WithMany()
            .HasForeignKey(wi => wi.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        // Order Configuration

        modelBuilder.Entity<Order>()
        .HasOne(o => o.Member)
        .WithMany()
        .HasForeignKey(o => o.MemberId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Book)
            .WithMany()
            .HasForeignKey(oi => oi.BookId)
            .OnDelete(DeleteBehavior.Restrict);
    }





}