using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace Simplog.Server.Infrastructure.Helpers
{
    public class MockObjectSet<T> : IDbSet<T> where T : class
    {
        HashSet<T> data;
        IQueryable<T> query;

        public MockObjectSet()
        {
            this.data = new HashSet<T>();
            this.query = this.data.AsQueryable();
        }
        public MockObjectSet(IEnumerable<T> testData)
        {
            if (testData == null)
                throw new ArgumentNullException("testData");

            this.data = new HashSet<T>(testData);
            this.query = this.data.AsQueryable();
        }
        #region IObjectSet<T> Members

        public void AddObject(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            data.Add(entity);
        }

        public void Attach(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            data.Add(entity);
        }

        public void DeleteObject(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            data.Remove(entity);
        }

        public void Detach(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.data.Remove(entity);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        #endregion

        #region IQueryable Members

        public Type ElementType
        {
            get { return query.ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return query.Expression; }
        }

        public IQueryProvider Provider
        {
            get { return query.Provider; }
        }

        #endregion

        public T Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            data.Add(entity);
            return entity;
        }

        T IDbSet<T>.Attach(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            data.Add(entity);
            return entity;
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            throw new NotImplementedException();
        }

        public T Create()
        {
            throw new NotImplementedException();
        }

        public T Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public System.Collections.ObjectModel.ObservableCollection<T> Local
        {
            get { throw new NotImplementedException(); }
        }

        public T Remove(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            data.Remove(entity);
            return entity;

        }
    }
}