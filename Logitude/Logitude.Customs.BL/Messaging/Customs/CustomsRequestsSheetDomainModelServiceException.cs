using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Server.Tools.Models;
using Logitude.SystemLogs;
using System.Web;
using Logitude.Server.Tools.Models;

namespace Logitude.Customs.BL.Messaging.Customs
{
    public class CustomsRequestsSheetDomainModelServiceException :System.Exception 
    {
        public enum WhereEnum
        {
            CustomsRequestsSheetServiceException, //WritingRequestsSheet 
            MessageServiceException,
            BLException,
            ReuestSheet,
            SameRequestInProgress,
            NoAvailableSignServer,
            RequestCancelled,
            AggregateDCAAnalyzerIsMust,
            AggregateDCAAnalyzerLockIt,
            DcaMessageNotBelongOurEnvironment

        }
        public enum What2DoEnum
        {
            Default,
            RetryQueue,
            StopQueue,
            NewQueueCreated,
            CancelRequest,
            StopQueueAddLog,
            //StopQueueSentResponseOnDCA,

        }
        public WhereEnum Where { get; private set; }
        public What2DoEnum What2Do { get; private set; }
        public string CustomsRequestsSheetId { get; set; }
        public Boolean SuppressExceptionTostring  { get; set; }

        public CustomsRequestsSheetDomainModelServiceException(WhereEnum whereEnum, What2DoEnum what2Do, string message, Exception innerException)
            : base(message, innerException)
        {
            Where = whereEnum;
            What2Do = what2Do;
            var trace = (Where == WhereEnum.CustomsRequestsSheetServiceException); //true;
            if (trace)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                    
                }
                System.Exception ex = this;
                if (innerException != null)
                {
                    innerException.ChangeExceptionMessage("CustomsRequestsSheetDomainModelServiceException:");
                    ex = innerException;
                }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "system", "CustomsRequestsSheetService:whereEnum=" + whereEnum.ToString() + ":What2Do=" + What2Do + ":" + message, ip);
            }

        }


        public int Tenant { get; set; }
    }
}
