using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;

namespace Logitude.CargoTracking.Data
{
    internal class InterceptorArithabort : IDbCommandInterceptor
    {
        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            command.CommandText = "SET ARITHABORT ON; " + command.CommandText;
        }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext) { }
        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext) { }
        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext) { }
        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext) { }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext) { }
    }
}
