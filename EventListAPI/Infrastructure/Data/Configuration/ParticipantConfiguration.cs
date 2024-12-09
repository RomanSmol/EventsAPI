using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(p => p.DateOfBirth)
                .IsRequired();

            builder.Property(p => p.RegistrationDate)
                .IsRequired();

            builder.Property(p => p.Password)
                .IsRequired();


            builder.HasOne(p => p.Role)
                   .WithMany(r => r.Participants)
                   .HasForeignKey(p => p.RoleId);

            builder.HasMany(p => p.EventParticipants)
                   .WithOne(ep => ep.Participant)
                   .HasForeignKey(ep => ep.ParticipantId);
        }
    }
}
