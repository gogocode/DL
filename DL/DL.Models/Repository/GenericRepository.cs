using DL.Models.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DL.Models.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {


        protected DbSet<TEntity> DbSet;

        private DbContext _dbContext;

        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = _dbContext.Set<TEntity>();
        }

        public GenericRepository()
        {
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public TEntity GetById(int id)
        {
            return DbSet.Find(id);
        }

        public IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public void Edit(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            this.SaveChanges();
        }

        public void Insert(TEntity entity)
        {

            DbSet.Add(entity);
            this.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
            this.SaveChanges();
        }

        public void SaveChanges()
        {
            this._dbContext.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._dbContext != null)
                {
                    this._dbContext.Dispose();
                    this._dbContext = null;
                }
            }
        }

    }
}
