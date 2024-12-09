using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Events
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int eventId, CancellationToken cancellationToken = default)
        {
            return await _context.Events.AnyAsync(e => e.Id == eventId, cancellationToken);
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            return await _context.Events
                .Include(e => e.EventParticipants)
                .ThenInclude(ep => ep.Participant)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Event?> GetByNameAsync(string name)
        {
            return await _context.Events.FirstOrDefaultAsync(e => e.Name == name);
        }

        public async Task<Event> AddAsync(Event newEvent)
        {
            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();

            return newEvent;
        }

        public async Task UpdateAsync(Event updatedEvent)
        {
            _context.Events.Update(updatedEvent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Event eventToDelete)
        {
            _context.Events.Remove(eventToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByCategoryAsync(string category)
        {
            return await _context.Events
                .Where(e => e.Category == category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByLocationAsync(string location)
        {
            return await _context.Events
                .Where(e => e.Location == location)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByDateAsync(DateTime date)
        {
            return await _context.Events
                .Where(e => e.EventDate.Date == date.Date)
                .ToListAsync();
        }
    }
}
// избавится от логики и перенести её в DTOs/Servicesw