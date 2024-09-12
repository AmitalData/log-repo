using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;
using Logitude.Server.Tools.Utils;
using Growl.CoreLibrary;
using Devart.Data.Linq;
using System.Collections;

namespace Logitude.Server.Tools.Helpers
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// For an Entity Framework IQueryable, returns the SQL with inlined Parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>


        public static string ToTraceQuery<T>(this IQueryable<T> query)
        {
            try
            {


                ObjectQuery<T> objectQuery = GetQueryFromQueryable(query);

                var result = objectQuery.ToTraceString();
                foreach (var parameter in objectQuery.Parameters.Reverse().ToArray())
                {
                    var name = "@" + parameter.Name;
                    var value = parameter.Value is null ? "NULL" : "'" + parameter.Value.ToString() + "'";

                    DateTime dt = new DateTime();
                    if (value != null && value.ToString().Length > 10 && DateTime.TryParse(value.Substring(1, 11), out dt))
                    {
                        value = string.Format("cast('{0}' as date)", dt.ToString("yyyy-MM-dd"));
                    }

                    result = result.Replace(name, value);
                }

                return result;
            }

            catch (Exception ex)
            {
                return "error " + ex.Message;

            }
        }

        /// <summary>
        /// For an Entity Framework IQueryable, returns the SQL and Parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static TraceStringValues ToTraceString<T>(this IQueryable<T> query)
        {
            TraceStringValues MySql = new TraceStringValues();
            ObjectQuery<T> objectQuery = GetQueryFromQueryable<T>(query);

            var traceString = new StringBuilder();

            traceString.AppendLine(objectQuery.ToTraceString());
            traceString.AppendLine();
            MySql.TSQL = traceString.ToString();
            MySql.TSQLParams = new List<MyTSqlParam>();
            foreach (var parameter in objectQuery.Parameters)
            {
                var param = new MyTSqlParam();
                param.Name = parameter.Name;
                param.Value = parameter.Value;
                MySql.TSQLParams.Add(param);
                //traceString.AppendLine(parameter.Name + " [" + parameter.ParameterType.FullName + "] = " + parameter.Value);
            }

            return MySql;// traceString.ToString();
        }

        private static System.Data.Entity.Core.Objects.ObjectQuery<T> GetQueryFromQueryable<T>(IQueryable<T> query)
        {
            var internalQueryField = query.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(f => f.Name.Equals("_internalQuery")).FirstOrDefault();
            var internalQuery = internalQueryField.GetValue(query);
            var objectQueryField = internalQuery.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(f => f.Name.Equals("_objectQuery")).FirstOrDefault();
            return objectQueryField.GetValue(internalQuery) as System.Data.Entity.Core.Objects.ObjectQuery<T>;
        }
        
        
        public static System.Collections.IList LogAndGetList<T>(this IQueryable<T> query, string funcOrQueryName)
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug(string.Format("{0} Query \r\n {1} ", funcOrQueryName, query.ToTraceQuery()));
            DateTime start = DateTime.Now;
            
            Type elementType = query.ElementType;

            Type listType = typeof(List<>).MakeGenericType(elementType);
            IList resultList = (IList)Activator.CreateInstance(listType);

            resultList = query.ToList();

            NetCommonHelper.Logger.DevLog.Instance.WriteDebug( string.Format("{0} SUM duration {1} seconds ",funcOrQueryName, (DateTime.Now - start).TotalSeconds));

            return resultList;

            ;
        }

    }

    public class TraceStringValues
    {
        public string TSQL { get; set; }
        public List<MyTSqlParam> TSQLParams { get; set; }
    }

    public class MyTSqlParam
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string Type { get; set; }
    }
}
