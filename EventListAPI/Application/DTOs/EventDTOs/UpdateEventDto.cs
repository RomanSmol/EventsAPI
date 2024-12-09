using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.EventDTOs
{
    public record UpdateEventDto(int Id, string Name, string Description, DateTime EventDate,
        string Location, string Category, int MaxParticipants, string? ImageUrl)
    {
        public Event MapToEntity(Event existingEvent)
        {
            existingEvent.Name = Name;
            existingEvent.Description = Description;
            existingEvent.EventDate = EventDate;
            existingEvent.Location = Location;
            existingEvent.Category = Category;
            existingEvent.MaxParticipants = MaxParticipants;
            existingEvent.ImageUrl = ImageUrl;
            return existingEvent;
        }
    }
}
