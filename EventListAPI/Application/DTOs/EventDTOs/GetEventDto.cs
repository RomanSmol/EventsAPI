using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.EventDTOs
{
    public record GetEventDto(int Id, string Name, string Description, DateTime EventDate,
        string Location, string Category, int MaxParticipants, string? ImageUrl)
    {
        public static GetEventDto MapIntoDto(Event eventEntity)
        {
            return new GetEventDto(eventEntity.Id, eventEntity.Name, eventEntity.Description,
                eventEntity.EventDate, eventEntity.Location, eventEntity.Category, eventEntity.MaxParticipants, eventEntity.ImageUrl);
        }
    }
}
