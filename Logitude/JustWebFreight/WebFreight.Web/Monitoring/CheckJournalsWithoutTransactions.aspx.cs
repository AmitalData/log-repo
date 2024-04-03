using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.SystemLogs;
using System;
using System.Web;

namespace WebFreight.Web.Monitoring
{
    public partial class CheckJournalsWithoutTransactions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
            Response.Write("<pingdom_http_custom_check>");
            if (AnyFailedJournal())
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
        private bool AnyFailedJournal()
        {
            bool isFaild = false;
            JournalQueryService journalQueryService = new JournalQueryService(0);
            try
            {

                isFaild = journalQueryService.GetJournalsWithoutTransactionsForToday();
            }
            catch (Exception errorInfo)
            {
                ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "CheckJournalsWithoutTransactions", "Bug in AnyFailedJournal Method...", null);
            }
            return isFaild;
        }
    }
}