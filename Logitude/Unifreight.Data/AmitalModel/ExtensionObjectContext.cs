#if false


using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel
{
    public static class ExtensionObjectContext
    {
        public static int FirstOrDefaultFUNOWAITWhere<T>(this System.Data.Entity.DbContext dbContext, Expression<Func<T, bool>> filter) where T : class
        {

            var adapter = (System.Data.Entity.Infrastructure.IObjectContextAdapter)dbContext;
            var objectContext = adapter.ObjectContext;
            return objectContext.FirstOrDefaultFUNOWAITWhere<T>(filter);
        }
        public static int FirstOrDefaultFUNOWAITWhere<T>(this System.Data.Entity.Core.Objects.ObjectContext db, Expression<Func<T, bool>> filter) where T : class
        {
            var newselectSql = "";
            //var query = db.Set<T>().Where(filter);
            var query = db.CreateObjectSet<T>().Where(filter) as ObjectQuery;

            string selectSql = query.ToTraceString();
            if (false)
            {
                var indexOfWHERE = selectSql.IndexOf("WHERE");
                var b4 = selectSql.Substring(0, indexOfWHERE);
                var after = selectSql.Substring(indexOfWHERE);
                 newselectSql = b4 + " FOR UPDATE NOWAIT " + after;
            }
            var indexOffROM = selectSql.LastIndexOf("FROM ");
            newselectSql = "SELECT 1 MyCount  " + selectSql.Substring(indexOffROM) + " FOR UPDATE NOWAIT ";

            var parameters = query.Parameters.Select(p => new Devart.Data.Oracle.OracleParameter(p.Name, p.Value)).ToArray();

            //db.Database.ExecuteSqlCommand(deleteSql, parameters);
            return db.ExecuteStoreCommand(newselectSql, parameters);
            
        }
        public static int DeleteWhere<T>(this System.Data.Entity.DbContext dbContext, Expression<Func<T, bool>> filter) where T : class
        {

            var adapter = (System.Data.Entity.Infrastructure.IObjectContextAdapter)dbContext;
            var objectContext = adapter.ObjectContext;
            return objectContext.DeleteWhere<T>(filter);
        }

        public static int DeleteWhere<T>(this ObjectContext db, Expression<Func<T, bool>> filter) where T : class
        {

            //var query = db.Set<T>().Where(filter);
            var query = db.CreateObjectSet<T>().Where(filter) as ObjectQuery;

            string selectSql = query.ToTraceString();
            string deleteSql = "DELETE " + selectSql.Substring(selectSql.IndexOf("FROM"));
            //return db.ExecuteStoreCommand(deleteSql,null);

            //var internalQuery = query.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(field => field.Name == "_internalQuery").Select(field => field.GetValue(query)).First();
            //var objectQuery = internalQuery.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(field => field.Name == "_objectQuery").Select(field => field.GetValue(internalQuery)).First() as ObjectQuery;
            var parameters = query.Parameters.Select(p => new Devart.Data.Oracle.OracleParameter(p.Name, p.Value)).ToArray();

            //db.Database.ExecuteSqlCommand(deleteSql, parameters);
            return db.ExecuteStoreCommand(deleteSql, parameters);
        }

        private static object GetPropertyValue(object o, string Name)
        {
            return o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(x => x.Name == Name).First().GetValue(o, null);
        }
        public static string ToTraceString(this IQueryable query)
        {
            string sql = (query as ObjectQuery).ToTraceString();
            return sql;
            var oquery = (ObjectQuery)GetPropertyValue(GetPropertyValue(query, "InternalQuery"), "ObjectQuery");
            return oquery.ToTraceString();
        }
    }
}
#endif