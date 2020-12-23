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
using Simplog.Global.Data.GlobalModel;
using System.Data.Entity.Core.Objects;
using Simplog.Data.Helpers;
using Simplog.Data.CommonDataModel.Repositories;


namespace WebFreight.Web.Monitoring
{
    public partial class SignupLeadMonitoringPage : System.Web.UI.Page
    {
 
            protected void Page_Load(object sender, EventArgs e)
            {

                Response.Clear();
                Response.ContentType = "text/xml";
                Response.Write("<pingdom_http_custom_check>");

                if (AnyFailedStatus())
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

            private bool AnyFailedStatus()
            {
                bool isSignupLeadProcessFailed = false;
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                try
                {
                    isSignupLeadProcessFailed = CheckIfSignupLeadProcessFailed();
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "SignupLeadMonitoringPage", "Bug in Signup Lead Processs : globaldbRep.GetFirstNotCompletedLogitudeLead()", null);
                }
                    scope.Complete();
                }
                return isSignupLeadProcessFailed;
            }

        private static bool CheckIfSignupLeadProcessFailed()
        {
            bool isSignupLeadProcessFailed = false;
            IGlobalContext globalContext = GlobalContext.GetContext(0);

            var logitudelead = (from a in globalContext.LogitudeLeads
                                where a.StatusCode == "InProgress" && (a.IsEmailVerified || (a.IsEmailVerified == false && a.IsSentToCustomer == false)) //&& (a.IsEmailVerified == true || a.IsSentToCustomer == false)
                                select a).FirstOrDefault();

            if (logitudelead != null)
            {
                TimeSpan timeSpan = DateTime.Now - logitudelead.CreateDate;
                if (timeSpan.Minutes > 10) isSignupLeadProcessFailed = true;
            }

            return isSignupLeadProcessFailed;
        }

        
    
}
}