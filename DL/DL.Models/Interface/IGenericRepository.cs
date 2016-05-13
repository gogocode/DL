using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DL.Models.Interface
{
    public interface IGenericRepository<TEntity>
    {
        TEntity GetById(int id);

        IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetAll();

        void Edit(TEntity entity);

        void Insert(TEntity entity);

        void Delete(TEntity entity);
    }
}
