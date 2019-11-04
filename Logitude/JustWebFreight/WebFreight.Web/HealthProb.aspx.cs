using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web
{
    public partial class HealthProb : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string source = "ASP.NET 4.0.30319.0";
            string log = "Application";
            //if (!EventLog.SourceExists(source))
            //{
            //    EventLog.CreateEventSource(source, log);
            //}
            try
            {
                
                EventLog.WriteEntry(source, "This is a call from HealthProb Check",
                    EventLogEntryType.Information);

                //EventLog eventLog = new EventLog("Application");
                //eventLog.Source = "Application";
                //eventLog.WriteEntry("HealthProb Check", EventLogEntryType.Information);

                var yourAppPool = new ServerManager().ApplicationPools["DefaultAppPool"];
                if (yourAppPool != null)
                {
                    while (yourAppPool.State != ObjectState.Started)
                    {
                        EventLog.WriteEntry(source, "DefaultAppPool Not Started", EventLogEntryType.Warning);

                    }
                }


            }
            catch (Exception ex)
            {

                EventLog.WriteEntry(source, "HealthProb Check : " + ex.Message, EventLogEntryType.Error);
               // throw ex;
            }

        }
    }
}