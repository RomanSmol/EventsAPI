using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken);
        Task<Role?> GetByIdAsync(int id);
        Task<Role?> GetByNameAsync(string name);
        Task AddAsync(Role role, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task DeleteAsync(Role role, CancellationToken cancellationToken);
    }
}
