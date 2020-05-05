using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoviIDApiCore.Data;
using CoviIDApiCore.Exceptions;
using CoviIDApiCore.Models.Database;
using DbException = CoviIDApiCore.Exceptions.DbException;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.Interfaces.Repositories;

namespace CoviIDApiCore.V1.Repositories
{
    public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : BaseModel<TKey> where TKey : struct
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _entitySet;

        public BaseRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            _entitySet = dbContext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity obj)
        {
            if (obj == default)
                throw new DbException(Messages.CreateNull);

            await _entitySet.AddAsync(obj);
        }

        public async Task AddRangeAsync(List<TEntity> objs)
        {
            if (objs == default)
                throw new DbException(Messages.AddRangeNull);
            else if (objs.Count == 0)
                return;

            await _entitySet.AddRangeAsync(objs);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _entitySet.ToListAsync();
        }

        public async Task<List<TEntity>> GetPageAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new ValidationException(Messages.InvalidPageInfo);

            return await _entitySet
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<TEntity> GetAsync(TKey id)
        {
            return await _entitySet
                .Where(e => EqualityComparer<TKey>.Default.Equals(e.Id, id))
                .SingleOrDefaultAsync();
        }

        public void Delete(TEntity obj)
        {
            if (obj == default)
                throw new DbException(Messages.DeleteNull);

            _entitySet.Remove(obj);
        }

        public void DeleteRange(List<TEntity> objs)
        {
            if (objs.Count == 0)
                return;

            _entitySet.RemoveRange(objs);
        }

        public async Task<int> SaveAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DbException(e, Messages.FailedToSave);
            }
        }

        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }

        public int Count()
        {
            return _entitySet.Count();
        }
    }
}