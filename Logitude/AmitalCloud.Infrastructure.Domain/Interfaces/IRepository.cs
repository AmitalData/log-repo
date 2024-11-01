using AmitalCloud.Infrastructure.Domain.BaseClasses;
using AmitalCloud.Infrastructure.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.Interfaces
{
    public interface IRepository< TEntity> 
        //where TContext : class , IContext
        where TEntity : class
    {
        void Insert(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        List<TEntity> GetAll(int tenant);
        List<TEntity> GetAll(int tenant, bool fromCache=true);
        List<TEntity> GetAll<TKey>(int tenant, Expression<Func<TEntity, TKey>> orderBy ,OrderByDirection orderByDirection = OrderByDirection.Ascending );
        List<TEntity> GetMulti<TKeyType>(IEntityKeyFields<TEntity, TKeyType> entityKeys);
        List<TEntity> GetMulti(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> GetMulti<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy,int skip,int take);
        IEnumerable<TEntity> GetMulti<TKey>(ISpecification<TEntity, TKey> spec);
        TEntity GetSingle<TKeyType>(IEntityKeyFields<TEntity, TKeyType> entityKeys);
    }

    public interface IAsyncRepository<TEntity> where TEntity : class
    {
        #region Async Methods
        Task<List<TEntity>> GetAllAsync(int tenant);
        Task<List<TEntity>> GetMultiAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetSingleAsync<TKeyType>(IEntityKeyFields<TEntity, TKeyType> entityKeys);
        Task<List<TEntity>> GetAllAsync(int tenant, bool fromCache = true);
        Task<List<TEntity>> GetAllAsync<TKey>(int tenant, Expression<Func<TEntity, TKey>> orderBy, OrderByDirection orderByDirection = OrderByDirection.Ascending);
        Task<List<TEntity>> GetMultiAsync<TKeyType>(IEntityKeyFields<TEntity, TKeyType> entityKeys);
        Task<List<TEntity>> GetMultiAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, int skip, int take);
        Task<IEnumerable<TEntity>> GetMultiAsync<TKey>(ISpecification<TEntity, TKey> spec);
        #endregion Async Methods
    }
}
