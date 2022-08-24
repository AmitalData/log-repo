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
        private QueryTreeFilterContext queryTreeFilterContext;
        public TreeFilterQueryService(QueryTreeFilterContext queryTreeFilterContext)
        {
            this.queryTreeFilterContext = queryTreeFilterContext;
        }

        public IQueryable<T> Apply<T>(IQueryable<T> queryable)
        {
            if (string.IsNullOrEmpty(queryTreeFilterContext.AdditionalTreeFilter)) return queryable;
            QueryFilterItem queryFilterItem = new QueryTreeFilterInterpreter().Run(queryTreeFilterContext);
            if (IsQueryFilterEmtpy(queryFilterItem)) return queryable;
            return queryable.Where(queryFilterItem.GetTreeExpression<T>());
        }

        public bool IsQueryFilterEmtpy(QueryFilterItem queryFilterItem)
        {
            return (queryFilterItem == null || queryFilterItem.QueryFilterItems == null || queryFilterItem.QueryFilterItems.Count() == 0);

        }
    }
}