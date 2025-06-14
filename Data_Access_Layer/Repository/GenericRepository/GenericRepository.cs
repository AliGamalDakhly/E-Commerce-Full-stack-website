using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Data.Context;
using Data_Access_Layer.Repository.IGenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Repository.GenericRepository
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class
    {
        private ApplicationDbContext _dbContext;
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TEntity AddEntity(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public bool DeleteEntity(TKey? id)
        {
            if (id == null) 
                return false;

            TEntity? entity = _dbContext.Set<TEntity>().Find(id);
            if (entity == null)
                return false;

            _dbContext.Set<TEntity>().Remove(entity);
            _dbContext.SaveChanges();
            return true;
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity>? query = _dbContext.Set<TEntity>();

            foreach(var _include in includes)
            {
                query =  query.Include(_include);
            }

            if(filter != null)
                query =  query.Where(filter);

            return query.ToList();
        }

        public TEntity GetById(TKey? id)
        {
            TEntity? entity = null;
            if (id != null)
                entity = _dbContext.Set<TEntity>().Find(id);

            return entity;
        }

        public TEntity UpdateEntity(TEntity entity)
        {
            if(entity != null)
            {
                _dbContext.Set<TEntity>().Update(entity);
                _dbContext.SaveChanges();
            }

            return entity;
        }
    }
}
