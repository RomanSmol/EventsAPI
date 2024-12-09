using Domain.Models;

namespace Domain.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(int id);
        Task<Event?> GetByNameAsync(string name);
        Task<Event> AddAsync(Event newEvent);
        Task UpdateAsync(Event updatedEvent);
        Task DeleteAsync(Event eventToDelete);
        Task<IEnumerable<Event>> GetEventsByCategoryAsync(string category);
        Task<IEnumerable<Event>> GetEventsByLocationAsync(string location);
        Task<IEnumerable<Event>> GetEventsByDateAsync(DateTime date);
        Task<bool> ExistsAsync(int eventId, CancellationToken cancellationToken = default);
    }
}