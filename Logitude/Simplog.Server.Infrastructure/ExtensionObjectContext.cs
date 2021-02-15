
using System;
using System.Collections.Generic;

using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure;


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
        //if (false)
        //{
        //    var indexOfWHERE = selectSql.IndexOf("WHERE");
        //    var b4 = selectSql.Substring(0, indexOfWHERE);
        //    var after = selectSql.Substring(indexOfWHERE);
        //     newselectSql = b4 + " FOR UPDATE NOWAIT " + after;
        //}
            var indexOffROM = selectSql.LastIndexOf("FROM ");
            newselectSql = "SELECT 1 MyCount  " + selectSql.Substring(indexOffROM) +
            " FOR UPDATE NOWAIT ";
            //" FOR UPDATE WAIT 1 ";

        var parameters = query.Parameters.Select(p => GetDbParameter(p.Name, p.Value)).ToArray();

            //db.Database.ExecuteSqlCommand(deleteSql, parameters);
            return db.ExecuteStoreCommand(newselectSql, parameters);
            
    }

    public static List<T> GetListNOWAITWhere<T>(this System.Data.Entity.DbContext dbContext, Expression<Func<T, bool>> filter) where T : class
    {
        using (var myIDbContextLogger = (dbContext as DbContextBase).CreateLogger())
        {
            List<T> myOut = null;
            try
            {
                var adapter = (System.Data.Entity.Infrastructure.IObjectContextAdapter)dbContext;
                var objectContext = adapter.ObjectContext;
                myOut = objectContext.GetListNOWAITWhere<T>(filter);
        }
            catch (Exception e)
            {
                e.ChangeExceptionMess(myIDbContextLogger.ToString());
                throw;
            }
            return myOut;
        }
    }
    public static List<T> GetListNOWAITWhere<T>(this System.Data.Entity.Core.Objects.ObjectContext db, Expression<Func<T, bool>> filter) where T : class
    {


        var newselectSql = "";
        //var query = db.Set<T>().Where(filter);
        var query = db.CreateObjectSet<T>().Where(filter) as ObjectQuery;

        string selectSql = query.ToTraceString();


        //newselectSql = "SELECT 1 MyCount  " + selectSql.Substring(indexOffROM) + " FOR UPDATE NOWAIT ";

        if (LogitudeSettings.DatabaseManagementSystem == "oracle")
        {
            newselectSql = selectSql + " FOR UPDATE NOWAIT ";
            //newselectSql = selectSql + " FOR UPDATE WAIT 1 ";
        }
        else
        {
            var indexOfWhere = selectSql.LastIndexOf("WHERE ");
            var sqlServer = " WITH(NOWAIT) ";

            newselectSql = selectSql.Insert(indexOfWhere, sqlServer);
        }
        var parameters = query.Parameters.Select(p => GetDbParameter(p.Name, p.Value)).ToArray();

        //db.Database.ExecuteSqlCommand(deleteSql, parameters);
        return db.ExecuteStoreQuery<T>(newselectSql, parameters).ToList();

    }
        public static int DeleteWhere<T>(this System.Data.Entity.DbContext dbContext, Expression<Func<T, bool>> filter) where T : class
        {

            var adapter = (System.Data.Entity.Infrastructure.IObjectContextAdapter)dbContext;
            var objectContext = adapter.ObjectContext;
            var tot = objectContext.DeleteWhere<T>(filter);
            return tot;
        }

        public static int DeleteWhere<T>(this ObjectContext db, Expression<Func<T, bool>> filter) where T : class
        {

            //var query = db.Set<T>().Where(filter);
            var query = db.CreateObjectSet<T>().Where(filter) as ObjectQuery;

            string selectSql = query.ToTraceString();
            string deleteSql = "DELETE " + selectSql.Substring(selectSql.LastIndexOf("FROM"));
            //return db.ExecuteStoreCommand(deleteSql,null);

            //var internalQuery = query.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(field => field.Name == "_internalQuery").Select(field => field.GetValue(query)).First();
            //var objectQuery = internalQuery.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(field => field.Name == "_objectQuery").Select(field => field.GetValue(internalQuery)).First() as ObjectQuery;

        var parameters = query.Parameters.Select(p => GetDbParameter(p.Name, p.Value)).ToArray();
        if (LogitudeSettings.DatabaseManagementSystem != "oracle")
        {
            deleteSql = deleteSql.Replace("AS [Extent1]", "").Replace("[Extent1].", "");
        }
            //db.Database.ExecuteSqlCommand(deleteSql, parameters);
            return db.ExecuteStoreCommand(deleteSql, parameters);
        }
    private static System.Data.Common.DbParameter GetDbParameter(string Name, object Value)
    {
        if (LogitudeSettings.DatabaseManagementSystem == "oracle")
        {
            return new Devart.Data.Oracle.OracleParameter(Name, Value) as System.Data.Common.DbParameter;
        }
        return new System.Data.SqlClient.SqlParameter(Name, Value) as System.Data.Common.DbParameter;

    }
        public static IEnumerable<T> UpdateWhereNotFast<T>(this System.Data.Entity.DbSet<T> Input, Func<T, Boolean> Objects, Action<T> UpdateAction) 
            where T : class
        {
            /*
Context.UpdateWhereNotFast((x=> x.Id == 1), (y)=> {y.Title = "something"});
Context.SaveChanges();
             */
            
            var I = Input.Where(Objects).ToList();
            I.ForEach(UpdateAction);
            return I;
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

