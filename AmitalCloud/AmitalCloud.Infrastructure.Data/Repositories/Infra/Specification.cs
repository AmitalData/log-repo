using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AmitalCloud.Infrastructure.Data.BaseClasses;
using AmitalCloud.Infrastructure.Domain.Enums;
using Microsoft.Azure.KeyVault.Core;
namespace AmitalCloud.Infrastructure.Data.Repositories.Infra
{
    public class Specification<T, TKeyType> : BaseSpecification<T, TKeyType>
    {
        public Specification(Expression<Func<T, bool>> criteria) : base(criteria)
        {
        }
        public Specification(Expression<Func<T, bool>> criteria, Expression<Func<T, TKeyType>> orderBy, OrderByDirection direction = OrderByDirection.Ascending) : base(criteria, orderBy, direction)
        {
        }
        public void Include(Expression<Func<T, object>> includeExpression) => AddInclude(includeExpression);
        public void Include(string includeExpression) => AddInclude(includeExpression);

    }
}
