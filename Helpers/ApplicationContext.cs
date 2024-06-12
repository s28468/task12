using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

namespace WebApplication1.Helpers;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            entity.HasKey(u => u.Id).HasName("User_PK");
            entity.HasIndex(u => u.Username).IsUnique();

            entity.Property(u => u.Username).HasMaxLength(50);
            entity.Property(u => u.Password).HasMaxLength(256);

            entity.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .IsRequired();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");
            entity.HasKey(r => r.Id).HasName("Role_PK");
            entity.HasIndex(r => r.Name).IsUnique();

            entity.Property(r => r.Name).HasMaxLength(100);
        });
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "User" },
            new Role { Id = 2, Name = "Admin" }
        );
    }
}