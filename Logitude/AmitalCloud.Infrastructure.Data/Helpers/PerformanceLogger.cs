using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;

namespace AmitalCloud.Infrastructure.Data.Helpers
{
    public class PerformanceLogger
    {

        private static ConcurrentDictionary<string, DateTime> logTimes = new ConcurrentDictionary<string, DateTime>();

        private static DbConnection GetGlobalDBConnection()
        {
            var ConfigConnectionString = ConfigurationManager.ConnectionStrings["Globalstr"].ConnectionString;
            return DatabaseInitializer.GetConnection(ConfigConnectionString);
        }

        public static string LogCurrentTime()
        {
            string key = Guid.NewGuid().ToString();
            logTimes.TryAdd(key, DateTime.Now);

            return key;
        }


        private static DateTime GetLogTime(string key)
        {
            DateTime date;
            if (logTimes.TryGetValue(key, out date))
            {
                return date;
            }
            else
            {
                return DateTime.Now;
            }
            //return logTimes.Where(s => s.Key.Contains(key)).FirstOrDefault().Value;
        }

        private static void RemoveLogTime(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (logTimes.Keys.Contains(key))
                {
                    DateTime date;
                    logTimes.TryRemove(key, out date);
                }
            }
        }

        public static void AddServerExecutionTimeHeader(string logKey)
        {
            if (!string.IsNullOrEmpty(logKey))
            {
                DateTime callTime = PerformanceLogger.GetLogTime(logKey);
                DateTime completionTime = DateTime.Now;
                int executionTime = (int)((completionTime.Ticks - callTime.Ticks) / TimeSpan.TicksPerMillisecond);
                HttpContext.Current.Response.Headers.Add("Access-Control-Expose-Headers", "ServerExecutionTime, X-Custom");
                HttpContext.Current.Response.Headers.Add("ServerExecutionTime", executionTime.ToString());

                PerformanceLogger.RemoveLogTime(logKey);
            }

        }

        public static void AddPerformanceLogsList(List<PerformanceLog> logsList)
        {
            var GlobalDBConnection = GetGlobalDBConnection();

            try
            {
                string ip = GetClientIPAddress(); ;
                
                if (logsList != null && logsList.Count > 0)
                {
                    foreach (var entity in logsList)
                    {  
                        string query = "INSERT INTO PerformanceLogs " +
                                        "(Id, LogDateTimeGMT, LogDateTimeLocal, Email, ModelName, MethodName,MonitoringService,ExecutionTime,UserIP,MethodParameters,Tenant,ServerTime) " +
                                        "VALUES (@Id, @LogDateTimeGMT, @LogDateTimeLocal, @Email, @ModelName, @MethodName, @MonitoringService, @ExecutionTime, @UserIP, @MethodParameters, @Tenant,@ServerTime) ";

                        using (SqlConnection cn = new SqlConnection(GlobalDBConnection.ConnectionString))
                        {
                            SqlCommand cmd = new SqlCommand(query, cn);
                            cmd.Parameters.Add("@Id", SqlDbType.VarChar, 100).Value = Guid.NewGuid().ToString();
                            cmd.Parameters.Add("@LogDateTimeGMT", SqlDbType.DateTime).Value = DateTime.UtcNow;
                            cmd.Parameters.Add("@LogDateTimeLocal", SqlDbType.DateTime).Value = entity.LogDateTimeLocal;
                            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = entity.Email;
                            cmd.Parameters.Add("@ModelName", SqlDbType.VarChar,100).Value = entity.ModelName;
                            cmd.Parameters.Add("@MethodName", SqlDbType.VarChar,100).Value = entity.MethodName;
                            cmd.Parameters.Add("@MonitoringService", SqlDbType.VarChar,100).Value = entity.MonitoringService;
                            cmd.Parameters.Add("@ExecutionTime", SqlDbType.Int).Value = entity.ExecutionTime;
                            cmd.Parameters.Add("@UserIP", SqlDbType.VarChar, 50).Value = ip;
                            cmd.Parameters.Add("@MethodParameters", SqlDbType.VarChar, 200).Value = entity.MethodParameters;
                            cmd.Parameters.Add("@Tenant", SqlDbType.Int).Value = entity.Tenant;
                            cmd.Parameters.Add("@ServerTime", SqlDbType.Int).Value = entity.ServerTime;

                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 5;
                            cn.Open();
                            var output = cmd.ExecuteNonQuery();
                            cn.Close();
                        }
                    } 
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "PerformanceLogger.AddPerformanceLogsList", null, null); 
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
