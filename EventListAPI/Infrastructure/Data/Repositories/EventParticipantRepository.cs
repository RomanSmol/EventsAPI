using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class EventParticipantRepository : IEventParticipantRepository
    {
        private readonly AppDbContext _context;

        public EventParticipantRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(EventParticipant eventParticipant)
        {
            await _context.EventParticipants.AddAsync(eventParticipant);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int eventId, int participantId)
        {
            return await _context.EventParticipants
                .AnyAsync(ep => ep.EventId == eventId && ep.ParticipantId == participantId);
        }

        public async Task<EventParticipant?> GetByIdAsync(int id)
        {
            return await _context.EventParticipants.FindAsync(id);
        }

        public async Task<IEnumerable<EventParticipant>> GetAllAsync()
        {
            return await _context.EventParticipants.ToListAsync();
        }

        public async Task<EventParticipant?> GetByEventAndParticipantIdAsync(int eventId, int participantId, CancellationToken cancellationToken = default)
        {
            return await _context.EventParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.ParticipantId == participantId, cancellationToken);
        }

        public async Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(int eventId)
        {
            return await _context.EventParticipants
                .Where(ep => ep.EventId == eventId)
                .Select(ep => ep.Participant)
                .ToListAsync();
        }

        public async Task DeleteAsync(EventParticipant eventParticipant, CancellationToken cancellationToken = default)
        {
            _context.EventParticipants.Remove(eventParticipant);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(EventParticipant eventParticipant)
        {
            _context.EventParticipants.Remove(eventParticipant);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EventParticipant updatedEventParticipant)
        {
            _context.EventParticipants.Update(updatedEventParticipant);
            await _context.SaveChangesAsync();
        }
    }
}
