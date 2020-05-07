using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity, TKey>
    {
        Task AddAsync(TEntity obj);

        Task AddRangeAsync(List<TEntity> objs);

        Task<List<TEntity>> GetAllAsync();

        Task<List<TEntity>> GetPageAsync(int pageNumber, int pageSize);

        Task<TEntity> GetAsync(TKey Id);

        void Delete(TEntity obj);

        void DeleteRange(List<TEntity> objs);

        Task<int> SaveAsync();

        void Update(TEntity entity);

        int Count();
    }
}
