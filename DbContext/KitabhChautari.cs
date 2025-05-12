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

    public DbSet<User> Users { get; set; }

    public DbSet<Admin> Admins { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.BookId);

            entity.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(b => b.Author)
                .IsRequired()
                .HasMaxLength(100);

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

            entity.Property(m => m.MembershipStatus)
                .IsRequired()
                .HasConversion<int>(); // Stores enum as int (0=Active, 1=Inactive, 2=Suspended)

            // Unique index on Email
            entity.HasIndex(m => m.Email)
                .IsUnique();

           
            // One-to-Many with Book
            entity.HasMany(m => m.Books)
                .WithOne(b => b.Member)
                .HasForeignKey(b => b.MemberId)
                .OnDelete(DeleteBehavior.SetNull); // Changed to SetNull for consistency
        });


        //  Admin manages many Books (one-to-many)
        modelBuilder.Entity<Admin>()
            .HasMany(a => a.Books)
            .WithOne(b => b.Admin)
            .HasForeignKey(b => b.AdminId);

        // Staff manages many Books (one-to-many)
        modelBuilder.Entity<Staff>()
            .HasMany(s => s.Books)
            .WithOne(b => b.Staff)
            .HasForeignKey(b => b.StaffId);


    }


    

public DbSet<Member> Member { get; set; } = default!;

public DbSet<Staff> Staff { get; set; } = default!;

public DbSet<User> User { get; set; } = default!;

public DbSet<Admin> Admin { get; set; } = default!;
}