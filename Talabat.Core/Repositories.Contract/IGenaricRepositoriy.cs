using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Spicifications;

namespace Talabat.Core.Repositories.Contract
{
    public interface IGenaricRepositoriy <T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
        Task<T?> GetByIdWithSpecAsync(ISpecifications<T> spec);
        Task<int> GetCountAsync(ISpecifications<T> spec);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
