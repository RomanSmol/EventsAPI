using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.
                HasKey(e => e.Id);

            builder.
                Property(e => e.Name).IsRequired()
                .IsRequired()
                .HasMaxLength(100);

            builder.
                Property(e => e.Location)
                .IsRequired()
                .HasMaxLength(200);

            builder.
                Property(e => e.EventDate)
                .IsRequired();

            builder.
                Property(e => e.Category)
                .IsRequired()
                .HasMaxLength(50);

            builder.
                Property(e => e.Description)
                .IsRequired();

            builder.
                Property(e => e.MaxParticipants)
                .IsRequired();

            builder.
                Property(e => e.ImageUrl);

            builder.HasMany(e => e.EventParticipants)
                   .WithOne(ep => ep.Event)
                   .HasForeignKey(ep => ep.EventId);
        }
    }
}
