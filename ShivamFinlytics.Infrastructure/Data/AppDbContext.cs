using Microsoft.EntityFrameworkCore;
using ShivamFinlytics.Domain.Entities;
using ShivamFinlytics.Domain.Enums;
using BCrypt.Net;

namespace ShivamFinlytics.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    #region DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<AnalystInsight> AnalystInsights { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<ActivityLog>()
            .HasOne(a => a.User)
            .WithMany(u => u.ActivityLogs)
            .HasForeignKey(a => a.UserId);

        modelBuilder.Entity<AnalystInsight>(entity =>
        {
            entity.HasOne(i => i.User)
                  .WithMany()
                  .HasForeignKey(i => i.CreatedBy)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(i => i.Category)
                  .WithMany()
                  .HasForeignKey(i => i.CategoryId)
                  .IsRequired(false);

            entity.HasOne(i => i.Transaction)
                  .WithMany()
                  .HasForeignKey(i => i.TranctionId) // Matches your 'TranctionId' spelling
                  .IsRequired(false);
        });


        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = (int)RoleType.Admin, Name = "Admin" },
            new Role { RoleId = (int)RoleType.Analyst, Name = "Analyst" },
            new Role { RoleId = (int)RoleType.Viewer, Name = "Viewer" }
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = 1,
                Name = "System Admin",
                Email = "admin@shivam.com",
                PasswordHash = "$2a$11$ea/3uh6HfEPF3ZDD4c6eAOQLkiMTJaV5f28lhtcIdMtX1Yb9w//De",
                RoleId = (int)RoleType.Admin,
                IsActive = true,
                CreatedAt = new DateTime(2026, 04, 01)
            }
        );

        modelBuilder.Entity<Category>().HasData(
        new Category 
        { 
            CategoryId = 1, 
            Name = "Salary", 
            Type = TransactionType.Income 
        },
        new Category 
        { 
            CategoryId = 2, 
            Name = "Food & Groceries", 
            Type = TransactionType.Expense 
        },
        new Category 
        { 
            CategoryId = 3, 
            Name = "Investments", 
            Type = TransactionType.Income 
        }
    );

    modelBuilder.Entity<Transaction>().HasData(
        new Transaction 
        { 
            TransactionId = 1, 
            UserId = 1, 
            CategoryId = 1,
            Amount = 5000.00m, 
            Type = TransactionType.Income, 
            Date = new DateTime(2026, 04, 03), 
            Note = "Monthly Salary Deposit",
            CreatedAt = new DateTime(2026, 04, 03, 10, 0, 0)
        },
        new Transaction 
        { 
            TransactionId = 2, 
            UserId = 1, 
            CategoryId = 2,
            Amount = 45.50m, 
            Type = TransactionType.Expense, 
            Date = new DateTime(2026, 04, 03), 
            Note = "Lunch at Cafe",
            CreatedAt = new DateTime(2026, 04, 03, 13, 15, 0)
        },
        new Transaction 
        { 
            TransactionId = 3, 
            UserId = 1, 
            CategoryId = 2, 
            Amount = 120.00m, 
            Type = TransactionType.Expense, 
            Date = new DateTime(2026, 04, 04), 
            Note = "Weekly Groceries",
            CreatedAt = new DateTime(2026, 04, 04, 09, 30, 0)
        }
    );
    }
}