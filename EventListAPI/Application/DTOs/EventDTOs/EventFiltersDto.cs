using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.EventDTOs
{
    public class EventFiltersDto
    {
        public DateTime? EventDate { get; set; }
        public string? Location { get; set; }
        public string? Category { get; set; }
    }
}
