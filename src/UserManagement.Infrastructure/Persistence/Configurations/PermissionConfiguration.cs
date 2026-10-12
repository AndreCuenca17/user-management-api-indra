using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        builder.HasKey(permission => permission.Id);
        builder.Property(permission => permission.Name).HasMaxLength(100).IsRequired();
        builder.HasIndex(permission => permission.Name).IsUnique();

        builder.HasData(
            new { Id = SeedData.UsersReadPermissionId, Name = "users.read" },
            new { Id = SeedData.UsersUpdatePermissionId, Name = "users.update" });
    }
}
