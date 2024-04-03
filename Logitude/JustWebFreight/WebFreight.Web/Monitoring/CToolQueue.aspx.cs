using Simplog.Data.InfrastructureModel;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Logitude.SystemLogs;
using System.Collections.Generic;

namespace WebFreight.Web.Monitoring
{
    public partial class CToolQueue : Page
    {
        protected readonly string ResponseContentType = "text/xml";
        protected readonly string XmlResponseParentElementName = "pingdom_http_custom_check";
        protected readonly string XmlResponseTimeElementName = "response_time";
        protected readonly string XmlResponseStatusElementName = "status";
        protected readonly string XmlResponseStatusElementSuccessValue = "Ok";
        protected readonly string XmlResponseStatusElementFailedValue = "Fail";

        protected readonly int MinutesBeforeNowForAllowedWaitingStatuses = 5;
        protected readonly int MinimumNumberOfAllowedWaitingStatuses = 30;
        protected readonly int MinimumNumberOfAllowedFailedStatuses = 0;
        protected readonly int WaitingStatusCodeNumber = 0;
        protected readonly int FailedStatusCodeNumber = -1;

        protected readonly IWebFreightContext context = WebFreightContext.GetContext(0);


        protected void Page_Load(object sender, EventArgs e)
        {
            BuildXmlResponse();
        }

        protected void BuildXmlResponse()
        {
            BuildXmlResponseHeader();
            BuildXmlResponseBody();
            BuildXmlResponseFooter();
        }

        protected void BuildXmlResponseHeader()
        {
            Response.Clear();
            Response.ContentType = ResponseContentType;
            Response.Write(BuildXmlElementString(XmlResponseParentElementName, null));
        }

        protected void BuildXmlResponseBody()
        {
            string statusElementValue = XmlResponseStatusElementSuccessValue;
            if (AnyWaitingOrFailedStatus())
            {
                statusElementValue = XmlResponseStatusElementFailedValue;
            }
            Response.Write(BuildXmlElementString(XmlResponseStatusElementName, statusElementValue));
        }

        protected void BuildXmlResponseFooter()
        {
            int responseTimeInMillisecond = HttpContext.Current.Timestamp.Millisecond;
            Response.Write(BuildXmlElementString(XmlResponseTimeElementName, responseTimeInMillisecond.ToString()));
            Response.Write(BuildXmlElementString("/" + XmlResponseParentElementName, null));
            Response.End();
        }

        protected string BuildXmlElementString(string elementName, string elementValue)
        {
            if(elementValue == null)
            {
                return "<" + elementName + ">";
            }
            return "<" + elementName + ">" + elementValue + "</" + elementName + ">";
        }

        protected bool AnyWaitingOrFailedStatus()
        {
            bool anyWaitingStatus = false;
            bool anyFailedStatus = false;

            try
            {
                anyWaitingStatus = AnyWaitingStatus();
                anyFailedStatus = AnyFailedStatus();
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }

            return anyWaitingStatus || anyFailedStatus;
        }

        protected bool AnyWaitingStatus()
        {
            List<string> ctoolQueueDefinitionCodes = GetCToolQueueDefinitionCodes();
            DateTime durationBeforeNowForAllowedWaitingStatuses = DateTime.Now.AddMinutes(0 - MinutesBeforeNowForAllowedWaitingStatuses);

            return (from queueMessages in context.QueueMessages
                    where ctoolQueueDefinitionCodes.Contains(queueMessages.QueueDefinitionCode.ToLower()) &&
                    queueMessages.CreateDateTime > durationBeforeNowForAllowedWaitingStatuses &&
                    queueMessages.Status == WaitingStatusCodeNumber
                    select queueMessages).Count() > MinimumNumberOfAllowedWaitingStatuses;
        }

        protected bool AnyFailedStatus()
        {
            List<string> ctoolQueueDefinitionCodes = GetCToolQueueDefinitionCodes();
            DateTime nowDate = DateTime.Now.Date;

            return (from queueMessages in context.QueueMessages
                    where ctoolQueueDefinitionCodes.Contains(queueMessages.QueueDefinitionCode.ToLower()) &&
                    queueMessages.CreateDateTime >= nowDate &&
                    queueMessages.Status == FailedStatusCodeNumber
                    select queueMessages).Count() > MinimumNumberOfAllowedFailedStatuses;
        }

        protected List<string> GetCToolQueueDefinitionCodes()
        {
            return new List<string>
            {
                "CToolShipmentsCreate".ToLower(),
                "CToolShipmentsUpdate".ToLower(),
                "CToolLookups".ToLower()
            };
        }

        protected void HandleException(Exception exception)
        {
            ExceptionHandler.HandleException(exception, DateTime.Now, 0, "", "CToolQueueMonitor", "Error while run CToolQueue monitor", null);
        }
    }
}