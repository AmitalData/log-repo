using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.TreeFilterQuery.Iterator
{
   public class QueryTreeFilterIterator
    {
        public List<QueryFilterItem> collection;
        private int position = 0;
        public QueryTreeFilterIterator(List<QueryFilterItem> collection)
        {
            this.collection = collection;
        }



        public void SetCollection(List<QueryFilterItem> collection)
        {
            this.collection = collection;
        }

        public bool Any()
        {
            return collection.Any();
        }



        public QueryFilterItem Next()
        {
            QueryFilterItem treeFilter = collection[position];
            position += 1;
            return treeFilter;
        }

        public bool HasNext()
        {
            if (position >= collection.Count ||
                collection[position] == null)
                return false;
            else
                return true;
        }


    }




}
