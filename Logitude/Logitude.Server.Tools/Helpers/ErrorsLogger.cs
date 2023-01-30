using Logitude.CRM.Data.EntityPOCOs;
using Logitude.SystemLogs;
using Logitude.SystemLogs.POCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace Logitude.Server.Tools.Helpers
{
    public class ErrorsLogger
    {

        private static DbConnection GetLogsDBConnection()
        {
            var ConfigConnectionString = ConfigurationManager.ConnectionStrings["SystemLogsStr"].ConnectionString;
            return DatabaseInitializer.GetConnection(ConfigConnectionString);
        }
        public static void AddErrorLog(ErrorLog errorLog)
        { 
            try
            { 
                string strConnString = GetLogsDBConnection().ConnectionString;
                string query = "INSERT INTO ErrorLogs " +
                    "(Id, Tenant, UserName, LogDate, ClientDate, Tier,Exception,StackTrace,SearchFields,IP) " +
                 "VALUES (@Id, @Tenant, @UserName, @LogDate, @ClientDate, @Tier, @Exception, @StackTrace, @SearchFields, @IP) ";

                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cmd.Parameters.Add("@Id", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                    cmd.Parameters.Add("@Tenant", SqlDbType.Int).Value = errorLog.Tenant;
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 50).Value = errorLog.UserName;
                    cmd.Parameters.Add("@LogDate", SqlDbType.DateTime).Value = DateTime.UtcNow;
                    cmd.Parameters.Add("@ClientDate", SqlDbType.DateTime).Value = errorLog.ClientDate;
                    cmd.Parameters.Add("@Tier", SqlDbType.VarChar,50).Value = errorLog.Tier;
                    cmd.Parameters.Add("@Exception", SqlDbType.VarChar,8500).Value = errorLog.Exception; 
                    cmd.Parameters.Add("@StackTrace", SqlDbType.VarChar, 8500).Value = errorLog.StackTrace; 
                    cmd.Parameters.Add("@SearchFields", SqlDbType.VarChar, 8500).Value = errorLog.SearchFields; 
                    cmd.Parameters.Add("@IP", SqlDbType.VarChar, 50).Value = GetClientIPAddress();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 5;
                    cn.Open();
                    var output = cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "ErrorsLogger.AddErrorLog", null, null);
            }
        }

        private static string GetClientIPAddress()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                if (string.IsNullOrEmpty(currentIP))
                {
                    currentIP = HttpContext.Current.Request.UserHostAddress;
                }
                return currentIP;
            }
            return "";
        }
    }

}
