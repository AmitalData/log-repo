using Logitude.SystemLogs;
using Microsoft.AspNet.SignalR.Messaging;
using Simplog.Data.CommonDataModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web.Monitoring
{
    public partial class CustomerActualData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
            Response.Write("<pingdom_http_custom_check>");
            if (!CheckCustomerActualDataStatus())
            {
                Response.Write("<status>Fail</status>");
            }
            else
            {
                Response.Write("<status>OK</status>");
            }

            int ResponseTime_Millisecond = HttpContext.Current.Timestamp.Millisecond;
            String ResponseTime = "<response_time>" + ResponseTime_Millisecond + "</response_time>";
            Response.Write(ResponseTime);
            Response.Write("</pingdom_http_custom_check>");
            Response.End();
        }

        private bool CheckCustomerActualDataStatus()
        {
            try
            {
                DateTime date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
                DateTime date2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 0, 0);

                if (DateTime.Now >= date1 && DateTime.Now <= date2)
                {
                    CommonDataContext Context = CommonDataContext.GetContextByDBId("0");
                    int AllTenants = Context.Tenants.Count();
                    int UpdatedTenants = 0;
                    SqlConnection sqlConnection1 = new SqlConnection(Context.GetConnection().ConnectionString);
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader reader;

                    cmd.CommandText = "select count(*) from CustomerActualDataHistory where CONVERT(date,startdatetime) = CONVERT(date,getdate()-1)";
                    //cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnection1;

                    sqlConnection1.Open();

                    reader = cmd.ExecuteReader();
                    // Data is accessible through the DataReader object here.

                    try
                    {
                        while (reader.Read())
                        {
                            UpdatedTenants = reader.GetInt32(0);
                        }
                    }
                    finally
                    {
                        // Always call Close when done reading.
                        reader.Close();
                    }

                    sqlConnection1.Close();

                    if (UpdatedTenants == AllTenants)
                    {
                        return true;
                    }
                }
            }
            catch (Exception errorInfo)
            {
                ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "CustomerActualData", "", "Bug in CheckCustomerActualDataStatus Method", null);
            }

            return false;
        }
    }
}