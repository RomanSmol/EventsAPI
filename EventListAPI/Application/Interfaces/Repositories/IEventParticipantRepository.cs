using Domain.Models;

namespace Domain.Interfaces
{
    public interface IEventParticipantRepository
    {
        Task AddAsync(EventParticipant eventParticipant);
        Task<EventParticipant?> GetByIdAsync(int id);
        Task<IEnumerable<EventParticipant>> GetAllAsync();
        Task<EventParticipant> GetByEventAndParticipantIdAsync(int eventId, int participantId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(int eventId);
        Task RemoveAsync(EventParticipant eventParticipant); 
        Task UpdateAsync(EventParticipant updatedEventParticipant);
        Task DeleteAsync(EventParticipant eventParticipant, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int eventId, int participantId);
    }
}
