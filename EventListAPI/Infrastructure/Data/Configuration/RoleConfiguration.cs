using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(role => role.Id);

            builder.Property(role => role.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .HasMany(role => role.Participants)
                .WithOne(user => user.Role)
                .HasForeignKey(user => user.RoleId);
        }
    }
}
