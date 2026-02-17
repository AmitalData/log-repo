//using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.Enums;
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
    public class Repository<TEntity> : IRepository<TEntity>, IAsyncRepository<TEntity>, IDisposable
        where TEntity : class
        //   where TContext : class, IContext

    {
        private readonly IContext _dbContext;
        private readonly System.Data.Entity.IDbSet<TEntity> _dbSet;
        private string _errorMessage = string.Empty;
        private bool _isDisposed;
        IUnitOfWork _unitOfWork;

        protected System.Data.Entity.IDbSet<TEntity> DbSet => _dbSet;
        protected IContext DbContext => _dbContext;

        public Repository(IUnitOfWork unitOfWork) : this(unitOfWork.Context)
        {
            _unitOfWork = unitOfWork;
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
        public async Task<List<TResult>> GetMultiAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> select) => await _dbSet.Where(predicate).Select(select).ToListAsync();
        public async Task<List<TEntity>> GetMultiAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> select) => await _dbSet.Where(predicate).Select(select).ToListAsync();
        public async Task SubmitChangesAsync()
        {
            if (_unitOfWork == null)
            {
                await SaveAsync();
            }
            else
            {
                throw new Exception("Use UOW.Save()");
            }

        }
        public async Task<List<TEntity>> GetMultiAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> select, Expression<Func<TEntity, TKey>> orderBy, int skip, int take) => await _dbSet.Where(predicate).Select(select).OrderBy(orderBy).Skip(skip).Take(take).ToListAsync();
        public async Task<List<TResult>> GetMultiAsync<TResult, TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> select, Expression<Func<TResult, TKey>> orderBy, int skip, int take) => await _dbSet.Where(predicate).Select(select).OrderBy(orderBy).Skip(skip).Take(take).ToListAsync();
        public async Task<List<TEntity>> GetMultiAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> select, string include, Expression<Func<TEntity, TKey>> orderBy, OrderByDirection orderByDirection = OrderByDirection.Ascending)
            => orderByDirection == OrderByDirection.Ascending ? await _dbSet.Where(predicate).Select(select).OrderBy(orderBy).Include(include).OrderBy(orderBy).ToListAsync()
            : await _dbSet.Where(predicate).Select(select).OrderBy(orderBy).Include(include).OrderByDescending(orderBy).ToListAsync();
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
        protected IQueryable<TEntity> GetQuery(int tenant)
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
                return GetFromCache(tenant);
            }
            else
            {
                return GetAll(tenant);
            }
        }
        public List<TEntity> GetAll<TKey>(int tenant, Expression<Func<TEntity, TKey>> orderBy, OrderByDirection orderByDirection = OrderByDirection.Ascending) => ApplyOrderedBy<TKey>(orderBy, orderByDirection, GetQuery(tenant)).ToList();
        public List<TEntity> GetMulti<TKeyType>(IEntityKeyFields<TEntity, TKeyType> entityKeys) => GetMulti(entityKeys.Predicate);
        public List<TResult> GetMulti<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> select) => _dbSet.Where(predicate).Select(select).ToList();
        public List<TResult> GetMulti<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> select, string include) => _dbSet.Where(predicate).Include(include).Select(select).ToList();
        public List<TEntity> GetMulti(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> select) => _dbSet.Where(predicate).Select(select).ToList();
        public List<TEntity> GetMulti(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> select, string include) => _dbSet.Where(predicate).Include(include).Select(select).ToList();
        public List<TEntity> GetMulti(Expression<Func<TEntity, bool>> predicate) => _dbSet.Where(predicate).ToList();
        public List<TEntity> GetMulti(Expression<Func<TEntity, bool>> predicate, string include) => _dbSet.Where(predicate).Include(include).ToList();
        public IEnumerable<TEntity> GetMulti<TKey>(ISpecification<TEntity, TKey> spec) => GetQuery(spec).AsEnumerable();
        public TEntity GetSingle<TKeyType>(IEntityKeyFields<TEntity, TKeyType> entityKeys) => GetMulti(entityKeys.Predicate).FirstOrDefault();
        public List<TEntity> GetMulti<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, int skip, int take) => _dbSet.Where(predicate).OrderBy(orderBy).Skip(skip).Take(take).ToList();
        public List<TEntity> GetMulti<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> select, Expression<Func<TEntity, TKey>> orderBy, int skip, int take)
            => _dbSet.Where(predicate).Select(select).OrderBy(orderBy).Skip(skip).Take(take).ToList();
        public List<TResult> GetMulti<TResult, TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> select, Expression<Func<TResult, TKey>> orderBy, int skip, int take)
            => _dbSet.Where(predicate).Select(select).OrderBy(orderBy).Skip(skip).Take(take).ToList();
        public List<TEntity> GetMulti<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, OrderByDirection orderByDirection = OrderByDirection.Ascending)
            => (orderByDirection == OrderByDirection.Ascending) ?
            _dbSet.Where(predicate).OrderBy(orderBy).ToList() : _dbSet.Where(predicate).OrderByDescending(orderBy).ToList();
        public List<TEntity> GetMulti<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> select, Expression<Func<TEntity, TKey>> orderBy, OrderByDirection orderByDirection = OrderByDirection.Ascending)
            => (orderByDirection == OrderByDirection.Ascending) ?
            _dbSet.Where(predicate).Select(select).OrderBy(orderBy).ToList() : _dbSet.Where(predicate).Select(select).OrderByDescending(orderBy).ToList();
        public List<TResult> GetMulti<TResult,TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> select, string include, Expression<Func<TResult, TKey>> orderBy, OrderByDirection orderByDirection = OrderByDirection.Ascending)
                    => (orderByDirection == OrderByDirection.Ascending) ?
            _dbSet.Where(predicate).Select(select).OrderBy(orderBy).Include(include).ToList() : _dbSet.Where(predicate).Select(select).OrderByDescending(orderBy).Include(include).ToList();


        public List<TResult> GetMulti<TResult, TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> select, Expression<Func<TResult, TKey>> orderBy, OrderByDirection orderByDirection = OrderByDirection.Ascending)
            => (orderByDirection == OrderByDirection.Ascending) ?
            _dbSet.Where(predicate).Select(select).OrderBy(orderBy).ToList() : _dbSet.Where(predicate).Select(select).OrderByDescending(orderBy).ToList();
        public List<object> GetMulti<TInner, TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TInner, bool>> innerpredicate,
           //, Expression<Func<TResult, TResult>> select,
           Expression<Func<TEntity, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector
            //, Expression<Func<TEntity, TInner, TResult>> resultSelector
            ) where TInner : class
        {
            var res = _dbSet.Where(predicate)
                .Join((_dbContext).Set<TInner>().Where(innerpredicate), outerKeySelector, innerKeySelector, (a, b) => new { a, b })
                .ToList<object>();
            return res;
        }


        public List<TEntity> GetMultiByParent<TEntityParentKeys>(TEntityParentKeys entityKeys)
        {
            throw new NotImplementedException();
        }

        protected List<T> GetListNOWAITWhere<T>(Expression<Func<T, bool>> filter) where T : class
        {
            throw new NotImplementedException();

            //using (var myIDbContextLogger = (dbContext as DbContextBase).CreateLogger())
            //{
            //    List<T> myOut = null;
            //    try
            //    {
            //        var adapter = (System.Data.Entity.Infrastructure.IObjectContextAdapter)dbContext;
            //        var objectContext = adapter.ObjectContext;
            //        myOut = GetListNOWAITWhere<T>(filter);
            //    }
            //    catch (Exception e)
            //    {
            //        e.ChangeExceptionMess(myIDbContextLogger.ToString());
            //        throw;
            //    }
            //    return myOut;
            //}
        }

        //protected List<T> GetListNOWAITWhere<T>( Expression<Func<T, bool>> filter) where T : class
        //{
        //    try
        //    {

        //        var newselectSql = "";
        //        //var query = db.Set<T>().Where(filter);
        //        var query = db.CreateObjectSet<T>().Where(filter) as ObjectQuery;

        //        string selectSql = query.ToTraceString();


        //        //newselectSql = "SELECT 1 MyCount  " + selectSql.Substring(indexOffROM) + " FOR UPDATE NOWAIT ";

        //        if (AmitalCLoudSettings.DatabaseManagementSystem == "oracle")
        //        {
        //            newselectSql = selectSql + " FOR UPDATE NOWAIT ";
        //            //newselectSql = selectSql + " FOR UPDATE WAIT 1 ";
        //        }
        //        else
        //        {
        //            var indexOfWhere = selectSql.LastIndexOf("WHERE ");
        //            var sqlServer = " WITH(NOWAIT) ";
        //            sqlServer = " WITH(UPDLOCK, NOWAIT) ";

        //            newselectSql = selectSql.Insert(indexOfWhere, sqlServer);
        //        }
        //        var parameters = query.Parameters.Select(p => GetDbParameter(p.Name, p.Value)).ToArray();

        //        //db.Database.ExecuteSqlCommand(deleteSql, parameters);
        //        return db.ExecuteStoreQuery<T>(newselectSql, parameters).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ChangeExceptionMess(myIDbContextLogger.ToString());
        //        throw;
        //    }

        //}

        protected bool DeleteWhere(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = _dbSet.Where(predicate);
            foreach (var entity in entities)
            {
                Delete(entity);
            }
            return true;
        }

        //TODO change access modifier to protected after UOW is implemented
        public void SubmitChanges()
        {
            if (_unitOfWork == null)
            {
                Save();
            }
            else
            {
                throw new Exception("Use UOW.Save()");
            }
        }

        public void Dispose()
        {
            if (_dbContext != null)
                _dbContext.Dispose();
            _isDisposed = true;
        }

        #endregion Sync Methods

        #region Private Methods 

        private void Save()
        {
            _dbContext.GetType().GetMethod("SaveChanges").Invoke(_dbContext, null);
        }
        private async Task SaveAsync()
        {
            await (Task)_dbContext.GetType().GetMethod("SaveChangesAsync").Invoke(_dbContext, null);
        }

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
            List<TEntity> entity = Helpers.CacheManager.CacheWrapper.Get<TEntity>(tenant);
            if (entity == null)
            {
                entity = GetAll(tenant);
                Helpers.CacheManager.CacheWrapper.Insert<TEntity>(tenant, entity);
            }
            return entity;
        }
        #endregion  Private Methods 
    }
}