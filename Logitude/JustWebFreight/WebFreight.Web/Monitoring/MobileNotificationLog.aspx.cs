using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel;
namespace WebFreight.Web.Monitoring
{
    public partial class MobileNotificationLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Response.Clear();
            Response.ContentType = "text/xml";
            Response.Write("<pingdom_http_custom_check>");
            string mytenant = Request.QueryString["Tenant"];
            int? tenant = null;
            if (!string.IsNullOrEmpty(mytenant))
            {
                tenant = Int32.Parse(mytenant);

            }
            if (AnyFailedStatus(tenant))
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

        private bool AnyFailedStatus(int? tenant)
        {

            bool IsFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(0);

                IGlobalContext globalContext = GlobalContext.GetContext();
                MobileNotificationLogRepository myRepository = new MobileNotificationLogRepository(globalContext);

       
                try
                {

                    DateTime twoDaysBefore = todayDateTime.AddDays(-2);

                    var mobilenotificationlog = (from d in myRepository.context.MobileNotificationLogs
                                                 where (d.AndroidStatus == "F" || d.IOSStatus == "F") && (d.CreateDate > twoDaysBefore)
                                                 select d).FirstOrDefault();
                    if (mobilenotificationlog != null) IsFailed = true;
                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "MobileNotificationLog", "Bug in AnyWaitingStatus Method : IsFaild = (from a in MobileNotificationLog ...", null);
                }

            

                scope.Complete();
            }
           
            return IsFailed;


        }
    }
}