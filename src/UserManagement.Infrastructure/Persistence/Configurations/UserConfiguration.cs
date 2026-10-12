using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Name).HasMaxLength(100).IsRequired();
        builder.Property(user => user.Email).HasMaxLength(255).IsRequired();
        builder.HasIndex(user => user.Email).IsUnique();
        builder.Property(user => user.PasswordHash).HasMaxLength(100).IsRequired();
        builder.Property(user => user.IsActive).IsRequired();

        builder.HasData(
            new
            {
                Id = SeedData.AdminUserId,
                Name = "Administrador Demo",
                Email = "admin@demo.com",
                PasswordHash = SeedData.AdminPasswordHash,
                IsActive = true
            },
            new
            {
                Id = SeedData.ConsultaUserId,
                Name = "Consulta Demo",
                Email = "consulta@demo.com",
                PasswordHash = SeedData.ConsultaPasswordHash,
                IsActive = true
            },
            new
            {
                Id = SeedData.InactiveUserId,
                Name = "Inactivo Demo",
                Email = "inactivo@demo.com",
                PasswordHash = SeedData.InactivePasswordHash,
                IsActive = false
            });

        builder.HasMany(user => user.Roles)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "user_roles",
                right => right.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                left => left.HasOne<User>().WithMany().HasForeignKey("UserId"),
                join =>
                {
                    join.HasKey("UserId", "RoleId");
                    join.HasData(
                        new { UserId = SeedData.AdminUserId, RoleId = SeedData.AdministratorRoleId },
                        new { UserId = SeedData.ConsultaUserId, RoleId = SeedData.ConsultaRoleId },
                        new { UserId = SeedData.InactiveUserId, RoleId = SeedData.ConsultaRoleId });
                });
    }
}
