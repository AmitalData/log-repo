using Logitude.Server.Tools.TreeFilterQuery.Interpreter;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.TreeFilterQuery.Iterator
{
    public class QueryTreeFilterCollection
    {
        public List<QueryFilterItem> QueryFilterItems = new List<QueryFilterItem>();
        private string filterName = string.Empty;
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
