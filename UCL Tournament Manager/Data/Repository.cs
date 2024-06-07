using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UCL_Tournament_Manager.Data
{
    public class Repository : IRepository
    {
        protected TournamentContext _context;

        public Repository(TournamentContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Console.WriteLine("Repository created.");
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Console.WriteLine("AddAsync called.");
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync<T>(int id) where T : class
        {
            Console.WriteLine("GetByIdAsync called.");
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            Console.WriteLine("GetAllAsync called.");
            return await _context.Set<T>().ToListAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Console.WriteLine("UpdateAsync called.");
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync<T>(T entity) where T : class
        {
            ArgumentNullException.ThrowIfNull(entity);

            Console.WriteLine("DeleteAsync called.");
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            Console.WriteLine("SaveChangesAsync called.");
            await _context.SaveChangesAsync();
        }
    }
}
