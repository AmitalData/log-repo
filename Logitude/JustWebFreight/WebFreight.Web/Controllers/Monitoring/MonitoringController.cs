using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;


namespace WebFreight.Web.Controllers.Monitoring
{
    public class MonitoringController:ApiController
    {
        #region AdvancedGenericInterfaceMonitoringStatus
        public HttpResponse AdvancedGenericInterfaceMonitoringStatus(object sender, EventArgs e)
        {
            TextWriter textWriter = new StringWriter();
            HttpResponse Response = new HttpResponse(textWriter);

            Response.Clear();
            Response.ContentType = "text/xml";
            Response.Write("<pingdom_http_custom_check>");

            if (AdvancedGenericInterfaceMonitoringStatus())
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

            return Response;
        }



        private bool AdvancedGenericInterfaceMonitoringStatus()
        {
            bool isFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isFailed = CheckIfAnyCommunicationLogsFailed();
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "AdvancedGenericInterfaceMonitoringStatus", "Bug in AdvancedGenericInterface Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isFailed;
        }

        private static bool CheckIfAnyCommunicationLogsFailed()
        {
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(0);
            DateTime twoDaysBefore = todayDateTime.AddDays(-2);
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(0);

            return (from a in commonDataContext.CommunicationLogs
                    where a.CreateDateUTC > twoDaysBefore
                    && a.Subject == "Advanced Generic Interface" && ((a.CommunicationStatusTypeCode == "W"
                    && (DbFunctions.DiffMinutes(a.CreateDateUTC, todayDateTime) > 5) || a.CommunicationStatusTypeCode == "F"))
                    select a).Any();

        }

        //https://test-accounting.amital.co.il/test/api/Monitoring/AdvancedGenericInterfaceMonitoringStatus

        #endregion
    }

}
