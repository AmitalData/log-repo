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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebFreight.Web.DataContracts;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;

namespace WebFreight.Web.Controllers.Monitoring
{
    public class MonitoringController:ApiController
    {
        public class pingdom_http_custom_check
        {
            public string status { get; set; }
            public int response_time { get; set; }
        }


        #region AdvancedGenericInterfaceMonitoringStatus
        [HttpGet]

        public HttpResponseMessage AdvancedGenericInterfaceMonitoringStatus()
        {

            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = CheckAdvancedGenericInterfaceMonitoringStatus();
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            HttpResponseMessage Response = Request.CreateResponse(HttpStatusCode.OK, pingdomCheck);
            return Response;
        }


        private bool CheckAdvancedGenericInterfaceMonitoringStatus()
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
