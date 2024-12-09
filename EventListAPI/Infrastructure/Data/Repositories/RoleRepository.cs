using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Role role, CancellationToken cancellationToken)
        {
            await _context.Roles.AddAsync(role, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Roles.ToListAsync(cancellationToken);
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }
    }
}
