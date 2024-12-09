namespace Domain.Models
{
    public class EventParticipant
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        public int ParticipantId { get; set; }
        public Participant Participant { get; set; } = null!;

        public DateTime RegistrationDate { get; set; }
    }
}