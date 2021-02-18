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
        public IQueryable<QueryType> GetSorterQuery<QueryType, FirstSortType>(QueryOperations queryOperations, IQueryable<QueryType> querableData)
        {
            string keyName = null;
            string keyType = null;
            if (!string.IsNullOrEmpty(queryOperations.ObjectTableName))
            {
                keyName = GetObjectTableKeyName(queryOperations);
                
            }
            SortParams<QueryType, FirstSortType> sortParams = new SortParams<QueryType, FirstSortType>();
            sortParams.QuerableData = querableData;
            sortParams.SortDirection = queryOperations.SortDirectin;
            sortParams.FirstSortExpression= GetFirstSortExpression<QueryType, FirstSortType>(queryOperations.SortByColumnName);
            
            if (!string.IsNullOrEmpty(keyName))
            {
                sortParams.SecondarySortByField = keyName;
                keyType = GetObjectFieldDataType(queryOperations, keyName);
                sortParams.SecondSortExpression = GetSecondarySortExpression<QueryType,FirstSortType>(keyName, keyType);
                return GetSortedQueryWithSecondarySort<QueryType, FirstSortType>(sortParams);
            }
            else
            {
                return GetSortedQuery<QueryType, FirstSortType>(sortParams);
            }
           
        }
        private IQueryable<QueryType> GetSortedQuery<QueryType, FirstSortType>(SortParams<QueryType, FirstSortType> sortParams)
        {
            if (sortParams.SortDirection.ToLower() == "ascending")
            {
                return sortParams.QuerableData.AsQueryable<QueryType>().OrderBy<QueryType, FirstSortType>(sortParams.FirstSortExpression);
            }
            else
            {
                return sortParams.QuerableData.AsQueryable<QueryType>().OrderByDescending<QueryType, FirstSortType>(sortParams.FirstSortExpression);
            }
        }
        private IQueryable<QueryType> GetSortedQueryWithSecondarySort<QueryType, FirstSortType>(SortParams<QueryType, FirstSortType> sortParams)
        {
            if(sortParams.SortDirection.ToLower()== "ascending")
            {
                return sortParams.QuerableData.AsQueryable<QueryType>().OrderBy<QueryType, FirstSortType>(sortParams.FirstSortExpression).ThenBy(sortParams.SecondSortExpression);
            }
            else
            {
                return sortParams.QuerableData.AsQueryable<QueryType>().OrderByDescending<QueryType, FirstSortType>(sortParams.FirstSortExpression).ThenByDescending(sortParams.SecondSortExpression);
            }
        }

        private Expression<Func<QueryType, FirstSortType>> GetFirstSortExpression<QueryType, FirstSortType>(string sortByFieldName)
        {
            var param = Expression.Parameter(typeof(QueryType), "item");
            var sortExpression = Expression.Lambda<Func<QueryType, FirstSortType>>
               (Expression.Convert(Expression.Property(param, sortByFieldName), typeof(FirstSortType)), param);
            return sortExpression;
        }
        private Expression<Func<QueryType, FirstSortType>> GetSecondarySortExpression<QueryType, FirstSortType>(string sortByFieldName,string secondarySortType)
        {
            var param = Expression.Parameter(typeof(QueryType), "item");
            var sortExpression = Expression.Lambda<Func<QueryType,FirstSortType>>
               (Expression.Convert(Expression.Property(param, sortByFieldName), Type.GetType(secondarySortType)), param);
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

        private string GetObjectFieldDataType(QueryOperations queryOperations,string keyName)
        {
            IObjectFieldPropertyGetter objectFieldPropertyGetter = InjectionContainer.Container.Resolve(typeof(IObjectFieldPropertyGetter), "ObjectFieldPropertyGetter", new ParameterOverride("", 1)) as IObjectFieldPropertyGetter;
            var type = objectFieldPropertyGetter.GetObjectFieldType(keyName, queryOperations.ObjectTableName, 0);
            return type;
        }

    }

    public class SortParams<QueryType, FirstSortType>
    {
        public string SecondarySortByField { get; set; }
        public Expression<Func<QueryType, FirstSortType>> FirstSortExpression { get; set; }
        public Expression<Func<QueryType,FirstSortType>> SecondSortExpression { get; set; }
        public IQueryable<QueryType> QuerableData { get; set; }
        public string SortDirection { get; set; }
    }
}