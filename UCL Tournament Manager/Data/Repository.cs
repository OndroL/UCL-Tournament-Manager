using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UCL_Tournament_Manager.Data;

namespace UCL_Tournament_Manager.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly TournamentContext _context;
        private DbSet<T> _entities;

        public Repository(TournamentContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            T entity = await GetByIdAsync(id);
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
