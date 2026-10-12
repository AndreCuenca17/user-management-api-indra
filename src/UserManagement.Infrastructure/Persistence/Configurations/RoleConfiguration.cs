using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        builder.HasKey(role => role.Id);
        builder.Property(role => role.Name).HasMaxLength(50).IsRequired();
        builder.HasIndex(role => role.Name).IsUnique();

        builder.HasData(
            new { Id = SeedData.AdministratorRoleId, Name = "Administrador" },
            new { Id = SeedData.ConsultaRoleId, Name = "Consulta" });

        builder.HasMany(role => role.Permissions)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "role_permissions",
                right => right.HasOne<Permission>().WithMany().HasForeignKey("PermissionId"),
                left => left.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                join =>
                {
                    join.HasKey("RoleId", "PermissionId");
                    join.HasData(
                        new { RoleId = SeedData.AdministratorRoleId, PermissionId = SeedData.UsersReadPermissionId },
                        new { RoleId = SeedData.AdministratorRoleId, PermissionId = SeedData.UsersUpdatePermissionId },
                        new { RoleId = SeedData.ConsultaRoleId, PermissionId = SeedData.UsersReadPermissionId });
                });
    }
}
