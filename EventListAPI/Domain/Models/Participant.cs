namespace Domain.Models
{
    public class Participant
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
    }
}