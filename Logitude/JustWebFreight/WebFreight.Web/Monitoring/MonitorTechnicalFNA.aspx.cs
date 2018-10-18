using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web.Monitoring
{
    public partial class MonitorTechnicalFNA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
            Response.Write("<pingdom_http_custom_check>");

            if (AnyTechnicalFNA())
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

        private bool AnyTechnicalFNA()
        {
            bool myResult = false;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(0);

                IGlobalContext globalContext = GlobalContext.GetContext();
                AnalyzeQueueRepository myRepository = new AnalyzeQueueRepository(globalContext);

                myResult = (from d in myRepository.context.AnalyzeQueues
                            where d.Subject == "Technical FNA"
                            && DbFunctions.TruncateTime(d.CreateDate) == DbFunctions.TruncateTime(todayDateTime)
                            select d).Any();

                scope.Complete();
            }

            return myResult;
        }
    }
}