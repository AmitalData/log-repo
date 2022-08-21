using Logitude.Server.Tools.TreeFilterQuery.Expression;
using Logitude.Server.Tools.TreeFilterQuery.Interpreter;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.TreeFilterQuery
{
   public class TreeFilterQueryService
    {

        private QueryFilterItem queryFilterItem;
        public TreeFilterQueryService(QueryTreeFilterContext context)
        {
            queryFilterItem = new QueryTreeFilterInterpreter().Run(context);
        }


        public IQueryable<T> Apply<T>(IQueryable<T> queryable)
        {
            if (queryFilterItem == null) return queryable;
            queryable = queryable.Where(queryFilterItem.GetTreeExpression<T>());
            return queryable;
        }

    }
}
