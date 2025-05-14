using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using kitabhChautari.Models;
using KitabhChautari.Enums;
using Microsoft.AspNetCore.Identity;

namespace kitabhChautari.Data
{
    public class KitabhChautariDbContext : IdentityDbContext<IdentityUser>
    {
        public KitabhChautariDbContext(DbContextOptions<KitabhChautariDbContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Staff> Staffs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Member Configuration
            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(m => m.MemberId);
                entity.Property(m => m.MemberId).ValueGeneratedOnAdd();
                entity.Property(m => m.UserId).IsRequired().HasMaxLength(450);
                entity.Property(m => m.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(m => m.LastName).IsRequired().HasMaxLength(100);
                entity.Property(m => m.Email).IsRequired(false).HasMaxLength(256); // Email is optional
                entity.Property(m => m.DateOfBirth).HasColumnType("date").IsRequired(false);
                entity.Property(m => m.RegistrationDate).HasColumnType("timestamp with time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(m => m.MembershipStatus).IsRequired().HasConversion<int>().HasDefaultValue(MembershipStatus.Active);
                entity.Property(m => m.ContactNo).IsRequired().HasMaxLength(20);
                entity.Property(m => m.IsStaff).HasDefaultValue(false);
                entity.HasIndex(m => m.ContactNo).IsUnique(); // Unique index on ContactNo
                entity.HasIndex(m => m.Email).IsUnique().HasFilter(@"""Email"" IS NOT NULL"); // PostgreSQL syntax for unique non-null emails
                entity.HasOne<IdentityUser>()
                    .WithOne()
                    .HasForeignKey<Member>(m => m.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
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
        }
    }
}