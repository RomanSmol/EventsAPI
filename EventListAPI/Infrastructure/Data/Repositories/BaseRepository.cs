using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repositories;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _entities;

        protected BaseRepository(AppDbContext context)
        {
            _context = context;
            _entities = _context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetByPredicateAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _entities.Where(predicate).ToListAsync(cancellationToken);
        }
    }
}
