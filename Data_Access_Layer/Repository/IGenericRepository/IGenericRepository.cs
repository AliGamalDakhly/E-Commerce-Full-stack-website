using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Repository.IGenericRepository
{
    public interface IGenericRepository<TEntity, TKey>  where TEntity : class
    {

        /// <summary>
        /// This method get All records,
        /// you can add filteration condition,
        /// you can include any navigation property 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null,
            params Expression<Func<TEntity, object>>[] includes);


        /// <summary>
        /// this method returns a record by ID,
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        TEntity GetById(TKey? id);
        TEntity AddEntity(TEntity entity);
        TEntity UpdateEntity(TEntity entity);
        bool DeleteEntity(TKey? id);
    }
}
