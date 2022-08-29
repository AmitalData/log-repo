using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.TreeFilterQuery.Interpreter
{

    public interface IQueryTreeFilterExpression
    {
        void Interpret(QueryTreeFilterContext context);
    }


    public class QueryTreeFilterContext
    {
        public QueryFilterItem QueryFilterItem { get; set; }
        public string ParentObjectTableName { get; set; }

        public string ParentEntityId { get; set; }
        public string AdditionalTreeFilter { get; set; }

        public string ObjectTableName { get; set; }
        public object ParentEntity { get; set; }

        public int Tenant { get; set; }

        public bool IsInterpreterFinished { get; set; }
        public Type Type { get; set; }




    }

}
