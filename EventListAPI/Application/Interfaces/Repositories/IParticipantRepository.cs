using Application.Interfaces.Repositories;
using Domain.Models;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IParticipantRepository : IRepository<Participant>
    {
        Task<IEnumerable<Participant>> GetAllAsync();
        Task<Participant?> GetByIdAsync(int id);
        Task AddAsync(Participant participant, CancellationToken cancellationToken = default);
        Task DeleteAsync(Participant participantToDelete);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(int eventId);
        Task<bool> ExistsAsync(int participantId, CancellationToken cancellationToken = default);
    }
}
