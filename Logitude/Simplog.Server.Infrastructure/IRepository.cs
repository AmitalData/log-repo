using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Simplog.Server.Infrastructure
{
    public interface IRepository<TEntity>
    {
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
        List<TEntity> All();
        void SubmitChanges();


        List<TEntity> GetMulti(EntityKeyFields entityKeys);

        TEntity GetSingle(EntityKeyFields entityKeys);

        //why not ?? IQueryable<TEntity> GetAll();
        
    }
}