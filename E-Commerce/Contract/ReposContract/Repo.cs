using E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace E_Commerce.Contract.ReposContract
{
    public class Repo<T> : IRepo<T> where T : class
    {
        private readonly AppDbContext _context;

        private readonly DbSet<T> _entity;
        public Repo(AppDbContext context)
        {
            _context = context;
            _entity = context.Set<T>();
        }
        public void Add(T entity)
        {
            _entity.Add(entity);
        }

        public async void Delete(int id)
        {
            var obj = await GetById(id);
            if (obj != null)
                _entity.Remove(obj);
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _entity.ToListAsync();

        public async Task<T> GetById(int id) => await _entity.FindAsync(id);

        public Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

        }


    }
}