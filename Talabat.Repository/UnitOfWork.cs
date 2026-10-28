using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;

        private Hashtable _repositories;

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }
        public Task<int> CompleteAsync()
           => _dbContext.SaveChangesAsync();

        public ValueTask DisposeAsync()
           => _dbContext.DisposeAsync();

        public IGenaricRepositoriy<TEntity> Repositoriy<TEntity>() where TEntity : BaseEntity
        {
            var Key = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(Key))
            {
                var repository = new GenaricRepository<TEntity>(_dbContext);

                _repositories.Add(Key, repository);
            }

            return _repositories[Key] as IGenaricRepositoriy<TEntity>;
        }
    }
}
