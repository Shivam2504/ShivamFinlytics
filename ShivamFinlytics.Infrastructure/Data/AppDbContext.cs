using System;
using Microsoft.EntityFrameworkCore;
using ShivamFinlytics.Domain.Entities;

namespace ShivamFinlytics.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<AnalystInsight> AnalystInsights { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<ActivityLog> ActivityLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

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
            // 1. Link to User (The person who created it)
             entity.HasOne(i => i.User)
                    .WithMany() // or .WithMany(u => u.AnalystInsights) if you added that to User entity
                    .HasForeignKey(i => i.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent accidental user deletion

            // 2. Link to Category (Optional)
            entity.HasOne(i => i.Category)
                    .WithMany()
                    .HasForeignKey(i => i.CategoryId)
                    .IsRequired(false);

            // 3. Link to Transaction (Optional)
            entity.HasOne(i => i.Transaction)
            .WithMany()
            .HasForeignKey(i => i.TranctionId) // Keeping your spelling 'TranctionId'
            .IsRequired(false);
        });
    }

}
