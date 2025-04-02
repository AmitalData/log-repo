using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System.Linq;

namespace AmitalCloud.Infrastructure.Web.Helpers.TreeFilterQuery
{
    public class TreeFilterQueryService : ITreeFilterQueryService
    {
        public IQueryable<T> Apply<T>(IQueryable<T> queryable, TreeFilterQueryArgs treeFilterQueryArgs)
        {
            if (string.IsNullOrEmpty(treeFilterQueryArgs.AdditionalTreeFilter) || string.IsNullOrWhiteSpace(treeFilterQueryArgs.AdditionalTreeFilter) || treeFilterQueryArgs.AdditionalTreeFilter == "null" || treeFilterQueryArgs.AdditionalTreeFilter == "undefined") return queryable;
            treeFilterQueryArgs.Type = typeof(T);
            QueryFilterItem queryFilterItem = new QueryTreeFilterInterpreter(treeFilterQueryArgs).Run();
            if (IsQueryFilterEmtpy(queryFilterItem)) return queryable;
            return queryable.Where(queryFilterItem.GetTreeExpression<T>());
        }

        private bool IsQueryFilterEmtpy(QueryFilterItem queryFilterItem)
        {
            return (queryFilterItem == null || queryFilterItem.QueryFilterItems == null || queryFilterItem.QueryFilterItems.Count() == 0);
        }
    }
}