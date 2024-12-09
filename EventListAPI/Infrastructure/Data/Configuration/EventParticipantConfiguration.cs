using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class EventParticipantConfiguration : IEntityTypeConfiguration<EventParticipant>
    {
        public void Configure(EntityTypeBuilder<EventParticipant> builder)
        {
            builder
                .HasKey(e => e.Id);

            builder
                .Property(e => e.RegistrationDate)
                .IsRequired();

            builder.HasOne(ep => ep.Event)
                   .WithMany(e => e.EventParticipants)
                   .HasForeignKey(ep => ep.EventId);

            builder.HasOne(ep => ep.Participant)
                   .WithMany(p => p.EventParticipants)
                   .HasForeignKey(ep => ep.ParticipantId);
        }
    }
}
