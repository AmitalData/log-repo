using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFreight.Web.GlobalModelDB;
using System.Transactions;
using WebFreight.Web.Testing;
using System.Data.SqlClient;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using WebFreight.Web.GlobalModel;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.SystemLogs;
using System.Configuration;
using Simplog.Server.Infrastructure;

namespace WebFreight.Web.Monitoring
{
    public partial class DbSize : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
            //Response.Write("<?xml version='1.0' encoding='UTF-8'?>");
            Response.Write("<pingdom_http_custom_check>");

            if (IsDbSizeGood())
            {
                Response.Write("<status>OK</status>");
            }
            else
            {
                Response.Write("<status>Fail</status>");
            }
            int ResponseTime_Millisecond = 0;//HttpContext.Current.Timestamp.Millisecond;
            String ResponseTime = "<response_time>" + ResponseTime_Millisecond + "</response_time>";
            Response.Write(ResponseTime);
            Response.Write("</pingdom_http_custom_check>");
            Response.End();
        }

        private bool IsDbSizeGood()
        {

            
            #region  Check Global DB size


            string Global_strConnString = ConfigurationManager.ConnectionStrings["Globalstr"].ConnectionString;

            if (!CheckDbSize(Global_strConnString))
                return false;

            #endregion

            #region Check Main DBs size
           
            List<GlobalDB> GlobalDatabases;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                GlobalDBRepository globaldbRep = new GlobalDBRepository();

                try
                {
                    GlobalDatabases = globaldbRep.All();
                    scope.Complete();
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "DbSize", "Bug in IsSqlDbUp Method : globaldbRep.All()",null);
                    scope.Complete();
                    return false;
                }
            }

            foreach (GlobalDB db in GlobalDatabases)
            {
                if (!CheckDbSize(db.DBConnection))
                    return false;

            }
			#endregion

			#region Check ErrorLogs DB size
			string ErrorLogs_strConnString = string.Empty;

			if (LogitudeSettings.DatabaseManagementSystem == "oracle")
			{
				ErrorLogs_strConnString = ConfigurationManager.ConnectionStrings["Oracle_SystemLogsStr"].ConnectionString; ;
			}
			else
			{
				ErrorLogs_strConnString = ConfigurationManager.ConnectionStrings["SystemLogsStr"].ConnectionString;
			}

            if (!CheckDbSize(ErrorLogs_strConnString))
                return false;

            #endregion

            return true;
        }

        private bool CheckDbSize(string strConnString)
        {
            try
            {

                SqlConnectionStringBuilder sqlSB;

                sqlSB = new SqlConnectionStringBuilder()
                {
                    InitialCatalog = strConnString.Split(',')[0],
                    UserID = strConnString.Split(',')[1],
                    Password = strConnString.Split(',')[2],
                    DataSource = strConnString.Split(',')[3],
                    IntegratedSecurity = false,
                    MultipleActiveResultSets = true,

                };

                Int64 totalDBSize = 0;
                decimal usedDbSize = 0;

                string totalDbSizeQuery = "SELECT CONVERT(BIGINT,DATABASEPROPERTYEX (" + "'" + sqlSB.InitialCatalog + "'" + ", 'MAXSIZEINBYTES'))/1024/1024";
                using (SqlConnection connection = new SqlConnection(sqlSB.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(totalDbSizeQuery, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            totalDBSize = (Int64)reader[0];
                        }
                    }
                }

                string usedSpaceQuery = "select sum(reserved_page_count)*8.0/1024 from sys.dm_db_partition_stats";
                //string usedSpaceQuery = "select top 1 code from ports";
                using (SqlConnection connection = new SqlConnection(sqlSB.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(usedSpaceQuery, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            usedDbSize = (decimal)reader[0];
                        }
                    }
                }

                if (usedDbSize > (decimal)(0.8 * totalDBSize)) // if used size > (80% of total size)
                {
                    return false;
                }
            }

            catch (Exception errorInfo)
            {
                ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "DbSize",
                    "SELECT SUM(reserved_page_count)*8.0/1024 FROM sys.dm_db_partition_stats;",null);
                return false;
            }

            return true;

        }
    }
}