using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.EventDTOs
{
    public record CreateEventDto(string Name, string Description, DateTime EventDate,
        string Location, string Category, int MaxParticipants, string? ImageUrl)
    {
        public Event MapIntoEventEntity()
        {
            return new Event()
            {
                Name = Name,
                Description = Description,
                EventDate = EventDate,
                Location = Location,
                Category = Category,
                MaxParticipants = MaxParticipants,
                ImageUrl = ImageUrl
            };
        }
    }
}
