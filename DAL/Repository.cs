using AIIcsoftAPI.Models.SMIcsoftDataModels;
using System.Linq.Expressions;

namespace AIIcsoftAPI.DAL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly SMDBIcsoftMainContext _context;
        public Repository(SMDBIcsoftMainContext context)
        {
            _context = context;
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity?> GetAsync(object id)
        {
            return await _context.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
        }

        public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).SingleOrDefaultAsync();
        }
    }
}
