namespace Domain.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int MaxParticipants { get; set; }
        public string? ImageUrl { get; set; }

        public ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
    }
}