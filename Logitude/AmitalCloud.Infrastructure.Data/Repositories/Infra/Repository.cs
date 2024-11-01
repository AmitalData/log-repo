using AmitalCloud.Infrastructure.Domain.Enums;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class Repository< TEntity> : IRepository< TEntity>, IAsyncRepository<TEntity>, IDisposable
        where TEntity : class
     //   where TContext : class, IContext

    {
        private readonly IContext _dbContext;
        private readonly IDbSet<TEntity> _dbSet;
        private string _errorMessage = string.Empty;
        private bool _isDisposed;

        public Repository(IUnitOfWork unitOfWork) : this(unitOfWork.Context)
        {
        }
        public Repository(IContext dbContext)
        {
            _isDisposed = false;
            _dbContext = dbContext;
            _dbSet = (dbContext).Set<TEntity>();
        }
        #region ASync Methods
        public async Task<TEntity> GetSingleAsync<TKeyType>(IEntityKeyFields<TEntity, TKeyType> entityKeys) => await _dbSet.Where(entityKeys.Predicate).FirstOrDefaultAsync();
        public async Task<List<TEntity>> GetAllAsync(int tenant) => await GetQuery(tenant).ToListAsync();
        public async Task<List<TEntity>> GetMultiAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();
        public async Task<List<TEntity>> GetAllAsync(int tenant, bool fromCache = true)
        {
            if (fromCache)
            {
                Task<List<TEntity>> task = new Task<List<TEntity>>(() => GetFromCache(tenant));
                task.Start();
                return await task;
            }
            else
            {
                return await GetAllAsync(tenant);
            }
        }
        public async Task<List<TEntity>> GetAllAsync<TKey>(int tenant, Expression<Func<TEntity, TKey>> orderBy, OrderByDirection orderByDirection = OrderByDirection.Ascending) => await ApplyOrderedBy<TKey>(orderBy, orderByDirection, GetQuery(tenant)).ToListAsync();
        public async Task<List<TEntity>> GetMultiAsync<TKeyType>(IEntityKeyFields<TEntity, TKeyType> entityKeys) => await GetMultiAsync(entityKeys.Predicate);
        public async Task<List<TEntity>> GetMultiAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, int skip, int take) => await _dbSet.Where(predicate).OrderBy(orderBy).Skip(skip).Take(take).ToListAsync();
        public async Task<IEnumerable<TEntity>> GetMultiAsync<TKey>(ISpecification<TEntity, TKey> spec) => await GetQuery(spec).ToListAsync();

        public async Task SubmitChangesAsync() => await _dbContext.SaveChangesAsync();


        #endregion ASync Methods 

        #region Sync Methods 
        public void Insert(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("Empty Entity");
                }
                if (_dbContext == null || _isDisposed)
                {
                    throw new Exception("Disposed");
                }
                _dbSet.Add(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleUnitOfWorkException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }

        }
        public void Delete(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("Entity");
                }
                if (_dbContext == null || _isDisposed)
                {
                    throw new Exception("Disposed");
                }
                _dbSet.Remove(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleUnitOfWorkException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }
        }
        public void Update(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("Entity");
                }
                if (_dbContext == null || _isDisposed)
                {
                    throw new Exception("Disposed");
                }
                _dbSet.Attach(entity);
                _dbContext.SetAsModified(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleUnitOfWorkException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }
        }
        public List<TEntity> GetAll(int tenant) => GetQuery(tenant).ToList();
        private IQueryable<TEntity> GetQuery(int tenant)
        {
            var type = typeof(TEntity);
            var query = _dbSet.AsQueryable();
            if (type.GetProperty("Tenant") != null)
            {
                Expression<Func<TEntity, bool>> predicate = x => (int)type.GetProperty("Tenant").GetValue(x) == tenant;
                query = query.Where(predicate);
            }
            return query;
        }
        public List<TEntity> GetAll(int tenant, bool fromCache = false)
        {
            if (fromCache)
            {
                return GetFromCache( tenant);
            }
            else
            {
                return GetAll(tenant);
            }
        }
        public List<TEntity> GetAll<TKey>(int tenant, Expression<Func<TEntity, TKey>> orderBy, OrderByDirection orderByDirection = OrderByDirection.Ascending) => ApplyOrderedBy<TKey>(orderBy, orderByDirection, GetQuery(tenant)).ToList();
        public List<TEntity> GetMulti<TKeyType>(IEntityKeyFields<TEntity, TKeyType> entityKeys) => GetMulti(entityKeys.Predicate);
        public List<TEntity> GetMulti(Expression<Func<TEntity, bool>> predicate) => _dbSet.Where(predicate).ToList();
        public IEnumerable<TEntity> GetMulti<TKey>(ISpecification<TEntity, TKey> spec) => GetQuery(spec).AsEnumerable();
        public TEntity GetSingle<TKeyType>(IEntityKeyFields<TEntity, TKeyType> entityKeys) => GetMulti(entityKeys.Predicate).FirstOrDefault();
        public List<TEntity> GetMulti<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, int skip, int take) => _dbSet.Where(predicate).OrderBy(orderBy).Skip(skip).Take(take).ToList();
        public void Dispose()
        {
            if (_dbContext != null)
                _dbContext.Dispose();
            _isDisposed = true;
        }
        #endregion Sync Methods
        private void HandleUnitOfWorkException(DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    _errorMessage = _errorMessage + $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage} {Environment.NewLine}";
                }
            }
        }
        private IQueryable<TEntity> GetQuery<TKey>(ISpecification<TEntity, TKey> spec)
        {
            var query = spec.Includes.Aggregate(_dbSet.AsQueryable(),
                    (current, include) => current.Include(include));
            query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
            if (spec.OrderBy != null)
            {
                query = ApplyOrderedBy<TKey>(spec.OrderBy, spec.OrderByDirection, query);
                if (spec.Skip > 0) query = query.Skip(spec.Skip);
                if (spec.Take > 0) query = query.Take(spec.Take);
            }
            return query.Where(spec.Criteria);
        }
        private IQueryable<TEntity> ApplyOrderedBy<TKey>(Expression<Func<TEntity, TKey>> orderBy, OrderByDirection orderByDirection, IQueryable<TEntity> query)
        {
            if (orderBy == null) return query;
            if (orderByDirection == OrderByDirection.Ascending)
            {
                query = query.OrderBy(orderBy);
            }
            else
            {
                query = query.OrderByDescending(orderBy);
            }
            return query;
        }
        private List<TEntity> GetFromCache(int tenant)
        {
            List<TEntity> entity = CacheManager.CacheWrapper.Get<TEntity>(tenant);
            if (entity == null)
            {
                entity = GetAll(tenant);
                CacheManager.CacheWrapper.Insert<TEntity>(tenant, entity);
            }
             return entity;
        }
    }
}