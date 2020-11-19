using Microsoft.Practices.Unity;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace Simplog.Server.Infrastructure.Helpers
{
    public class GenericSort
    {
        public IQueryable<T> GetSorterQuery<T,N>(QueryOperations queryOperations, IQueryable<T> querableData)
        {
            string keyName = GetObjectTableKeyName(queryOperations);
            SortParams<T, N> sortParams = new SortParams<T, N>();
            sortParams.QuerableData = querableData;
            sortParams.SortDirection = queryOperations.SortDirectin;
            sortParams.FirstSortExpression= GetSortExpression<T, N>(queryOperations.SortByColumnName);
            
            if (!string.IsNullOrEmpty(keyName))
            {
                sortParams.SecondarySortByField = keyName;
                sortParams.SecondSortExpression = GetSortExpression<T, string>(keyName);
                return GetSortedQueryWithSecondarySort<T, N>(sortParams);
            }
            else
            {
                return GetSortedQuery<T, N>(sortParams);
            }
           
        }
        private IQueryable<T> GetSortedQuery<T, N>(SortParams<T, N> sortParams)
        {
            if (sortParams.SortDirection.ToLower() == "ascending")
            {
                return sortParams.QuerableData.AsQueryable<T>().OrderBy<T, N>(sortParams.FirstSortExpression);
            }
            else
            {
                return sortParams.QuerableData.AsQueryable<T>().OrderByDescending<T, N>(sortParams.FirstSortExpression);
            }
        }
        private IQueryable<T> GetSortedQueryWithSecondarySort<T,N>(SortParams<T,N> sortParams)
        {
            if(sortParams.SortDirection.ToLower()== "ascending")
            {
                return sortParams.QuerableData.AsQueryable<T>().OrderBy<T, N>(sortParams.FirstSortExpression).ThenBy<T,string>(sortParams.SecondSortExpression);
            }
            else
            {
                return sortParams.QuerableData.AsQueryable<T>().OrderByDescending<T, N>(sortParams.FirstSortExpression).ThenByDescending(sortParams.SecondSortExpression);
            }
        }

        private Expression<Func<T,N>> GetSortExpression<T,N>(string sortByFieldName)
        {
            var param = Expression.Parameter(typeof(T), "item");
            var sortExpression = Expression.Lambda<Func<T, N>>
               (Expression.Convert(Expression.Property(param, sortByFieldName), typeof(N)), param);
            return sortExpression;
        }

        private string GetObjectTableKeyName(QueryOperations queryOperations)
        {
            string keyName = null;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                IObjectTablePropertyGetter objectTablePropertyGetter = InjectionContainer.Container.Resolve(typeof(IObjectTablePropertyGetter), "ObjectTablePropertyGetter", new ParameterOverride("", 1)) as IObjectTablePropertyGetter;
                keyName = objectTablePropertyGetter.GetKeyPropertyPath(queryOperations.ObjectTableName, 0);
                scope.Complete();
            }
            return keyName;
        }

        
    }

    public class SortParams<T,N>
    {
        public string SecondarySortByField { get; set; }
        public Expression<Func<T,N>> FirstSortExpression { get; set; }
        public Expression<Func<T, string>> SecondSortExpression { get; set; }
        public IQueryable<T> QuerableData { get; set; }
        public string SortDirection { get; set; }
    }
}