using AmitalCloud.Infrastructure.Domain.DataContracts;
using System.Collections.Generic;
using System.Linq;

namespace AmitalCloud.Infrastructure.Web.Helpers.TreeFilterQuery
{
    public class QueryTreeFilterCollection
    {
        public List<QueryFilterItem> QueryFilterItems = new List<QueryFilterItem>();
        public QueryTreeFilterCollection(QueryTreeFilterContext queryTreeFilterContext)
        {
            BuildCollection(queryTreeFilterContext.QueryFilterItem);
        }

        private void BuildCollection(QueryFilterItem queryFilterItem)
        {
            if (queryFilterItem.QueryFilterItems == null || queryFilterItem.QueryFilterItems.Count() == 0)
            {
                QueryFilterItems.Add(queryFilterItem);
                return;
            }

            for (var i = 0; i < queryFilterItem.QueryFilterItems.Count; i++)
            {
                BuildCollection(queryFilterItem.QueryFilterItems[i]);
            }
        }

        public QueryTreeFilterIterator CreateIterator()
        {
            return new QueryTreeFilterIterator(QueryFilterItems);
        }

        public QueryTreeFilterIterator CreateIterator(List<QueryFilterItem> collections)
        {
            return new QueryTreeFilterIterator(collections);
        }
    }
}
