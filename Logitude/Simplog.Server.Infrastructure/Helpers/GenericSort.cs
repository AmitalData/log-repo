using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Simplog.Server.Infrastructure.Helpers
{
    public class GenericSort
    {
        public IQueryable<T> GetSorterQuery<T,N>(QueryOperations queryOperations, IQueryable<T> querableData)
        {

            var param = Expression.Parameter(typeof(T), "item");
            
            var sortExpression = Expression.Lambda<Func<T,N>>
                (Expression.Convert(Expression.Property(param, queryOperations.SortByColumnName), typeof(N)), param);

            switch (queryOperations.SortDirectin.ToLower())
            {
                case "ascending":
                    return querableData.AsQueryable<T>().OrderBy<T, N>(sortExpression);
                default:
                    return querableData.AsQueryable<T>().OrderByDescending<T, N>(sortExpression);
            } 

            
        }

        
    }
}