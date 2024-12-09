using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories
{
    public class ParticipantRepository(AppDbContext context) : BaseRepository<Participant>(context), IParticipantRepository
    {
        private readonly AppDbContext _context;

        public async Task<IEnumerable<Participant>> GetAllAsync()
        {
            return await _context.Participants
                .Include(p => p.Role)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(int eventId)
        {
            return await _context.EventParticipants
                .Where(ep => ep.EventId == eventId)
                .Select(ep => ep.Participant)
                .ToListAsync();
        }

        public async Task<Participant?> GetByIdAsync(int id)
        {
            return await _context.Participants
                .Include(p => p.Role)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Participant participant, CancellationToken cancellationToken)
        {
            await _context.AddAsync(participant, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Participant participantToDelete)
        {
                _context.Participants.Remove(participantToDelete);
                await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int participantId, CancellationToken cancellationToken = default)
        {
            return await _context.Participants.AnyAsync(p => p.Id == participantId, cancellationToken);
        }
    }
}
