using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools.TreeFilterQuery.Interpreter;
using Logitude.Server.Tools.TreeFilterQuery.Iterator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Logitude.Server.Tools.TreeFilterQuery.Interpreter;

namespace Logitude.Server.Tools.TreeFilterQuery.Expression
{

    public class JavaScriptSerializerExpression: IQueryTreeFilterExpression
    {

        public void Interpret(QueryTreeFilterContext queryTreeFilterContext)
        {

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            queryTreeFilterContext.QueryFilterItem = javaScriptSerializer.Deserialize<QueryFilterItem>(queryTreeFilterContext.AdditionalTreeFilter);

        }





    }


}
