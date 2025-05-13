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

    public DbSet<User> Users { get; set; }

    public DbSet<Admin> Admins { get; set; }

    public DbSet<Author> Authors { get; set; } 

    public DbSet<Genre> Genres { get; set; } 

    public DbSet<Publisher> Publishers { get; set; }
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

            entity.Property(m => m.MembershipStatus)
                .IsRequired()
                .HasConversion<int>(); // Stores enum as int (0=Active, 1=Inactive, 2=Suspended)

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


    }


    


}