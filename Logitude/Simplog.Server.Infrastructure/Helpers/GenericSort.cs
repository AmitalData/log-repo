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
            if (!string.IsNullOrEmpty(queryOperations.ObjectTableName))
            {
                keyName = GetObjectTableKeyName(queryOperations);
                
            }
            SortParams<QueryType, FirstSortType> sortParams = new SortParams<QueryType, FirstSortType>();
            sortParams.QuerableData = querableData;
            sortParams.SortDirection = queryOperations.SortDirectin;
            sortParams.FirstSortExpression= GetFirstSortExpression<QueryType, FirstSortType>(queryOperations.SortByColumnName);
            return GetSortedQuery(sortParams);
            //if (!string.IsNullOrEmpty(keyName))
            //{
            //    sortParams.SecondarySortByField = keyName;
            //    sortParams.SecondarySortByFieldDataType = GetObjectFieldDataType(queryOperations, keyName);
            //    sortParams = SetSecondarySortParams(sortParams);
            //    return GetSortedQueryWithSecondarySort(sortParams);
            //}
            //else
            //{
            //    return GetSortedQuery(sortParams);
            //}

        }

        private SortParams<QueryType, FirstSortType> SetSecondarySortParams<QueryType, FirstSortType>
            (SortParams<QueryType, FirstSortType> sortParams)
        {
            var keyType = sortParams.SecondarySortByFieldDataType;
            switch (keyType)
            {
                case "System.Int32": 
                    {
                        sortParams.SecondIntegerSortExpression = GetSecondarySortExpression<QueryType, int>(sortParams.SecondarySortByField);
                        break; 
                    }
                case "System.Int64":
                    {
                        sortParams.SecondLongSortExpression = GetSecondarySortExpression<QueryType, long>(sortParams.SecondarySortByField);
                        break;
                    }
                case "System.String":
                    {
                        sortParams.SecondStringSortExpression = GetSecondarySortExpression<QueryType, string>(sortParams.SecondarySortByField);
                        break;
                    }

            }
            return sortParams;
        }

        private Expression<Func<QueryType, SecondaryType>> GetSecondarySortExpression<QueryType, SecondaryType>(string secondarySortByField)
        {
            var param = Expression.Parameter(typeof(QueryType), "item");
            var sortExpression = Expression.Lambda<Func<QueryType, SecondaryType>>
               (Expression.Convert(Expression.Property(param, secondarySortByField), typeof(SecondaryType)), param);
            return sortExpression;
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
            var keyType = sortParams.SecondarySortByFieldDataType;
            switch (keyType)
            {
                case "System.Int32":
                    {
                        return GetSortedQueryWithIntegerSecondarySort(sortParams);
                    }
                case "System.Int64":
                    {
                        return GetSortedQueryWithLongSecondarySort(sortParams);
                    }
                case "System.String":
                    {
                        return GetSortedQueryWithStringSecondarySort(sortParams);
                    }
                default:
                    {
                        throw new NotSupportedException("secondary sort data type is not supported");
                    }
            }
        }

        private Expression<Func<QueryType, FirstSortType>> GetFirstSortExpression<QueryType, FirstSortType>(string sortByFieldName)
        {
            var param = Expression.Parameter(typeof(QueryType), "item");
            var sortExpression = Expression.Lambda<Func<QueryType, FirstSortType>>
               (Expression.Convert(Expression.Property(param, sortByFieldName), typeof(FirstSortType)), param);
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

        private IQueryable<QueryType> GetSortedQueryWithStringSecondarySort<QueryType, FirstSortType>(SortParams<QueryType, FirstSortType> sortParams)
        {
            if (sortParams.SortDirection.ToLower() == "ascending")
            {
                return sortParams.QuerableData.AsQueryable().OrderBy(sortParams.FirstSortExpression).ThenBy(sortParams.SecondStringSortExpression);
            }
            else
            {
                return sortParams.QuerableData.AsQueryable().OrderByDescending(sortParams.FirstSortExpression).ThenByDescending(sortParams.SecondStringSortExpression);
            }
        }
        private IQueryable<QueryType> GetSortedQueryWithIntegerSecondarySort<QueryType, FirstSortType>(SortParams<QueryType, FirstSortType> sortParams)
        {
            if (sortParams.SortDirection.ToLower() == "ascending")
            {
                return sortParams.QuerableData.AsQueryable().OrderBy(sortParams.FirstSortExpression).ThenBy(sortParams.SecondIntegerSortExpression);
            }
            else
            {
                return sortParams.QuerableData.AsQueryable().OrderByDescending(sortParams.FirstSortExpression).ThenByDescending(sortParams.SecondIntegerSortExpression);
            }
        }
        private IQueryable<QueryType> GetSortedQueryWithLongSecondarySort<QueryType, FirstSortType>(SortParams<QueryType, FirstSortType> sortParams)
        {
            if (sortParams.SortDirection.ToLower() == "ascending")
            {
                return sortParams.QuerableData.AsQueryable().OrderBy(sortParams.FirstSortExpression).ThenBy(sortParams.SecondLongSortExpression);
            }
            else
            {
                return sortParams.QuerableData.AsQueryable().OrderByDescending(sortParams.FirstSortExpression).ThenByDescending(sortParams.SecondLongSortExpression);
            }
        }

    }

    public class SortParams<QueryType, FirstSortType>
    {
        public string SecondarySortByField { get; set; }
        public Expression<Func<QueryType, FirstSortType>> FirstSortExpression { get; set; }
        public Expression<Func<QueryType,string>> SecondStringSortExpression { get; set; }
        public Expression<Func<QueryType, int>> SecondIntegerSortExpression { get; set; }
        public Expression<Func<QueryType, long>> SecondLongSortExpression { get; set; }
        public IQueryable<QueryType> QuerableData { get; set; }
        public string SortDirection { get; set; }
        public string SecondarySortByFieldDataType { get; set; }
    }
}