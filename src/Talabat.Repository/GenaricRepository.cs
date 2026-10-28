using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Products;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Spicifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenaricRepository<T> : IGenaricRepositoriy<T> where T : BaseEntity
    {
        private readonly StoreDbContext _dbContext;

        public GenaricRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync() // Ask ClR for Creating Object from dbContext
        {
          
            ///if (typeof(T) == typeof(Product))
            ///    return (IEnumerable<T>) await _dbContext.Products.Include(P => P.Brand).Include(P => P.Category).ToListAsync();
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            ///if(typeof(T) == typeof(Product))
            ///  return await _dbContext.Set<Product>().Where(p => p.Id == id).Include(p => p.Brand).Include(p => p.Category).FirstOrDefaultAsync() as T;
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T?> GetByIdWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<int> GetCountAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
        {
            return SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        public async Task AddAsync(T entity)
          => await _dbContext.AddAsync(entity);

        public void Update(T entity)
          =>  _dbContext.Update(entity);

        public void Delete(T entity)
         =>  _dbContext.Remove(entity);

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

      
    }
}
