using Logitude.SystemLogs;
using Microsoft.Web.Administration;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFreight.Web.WebServices;

namespace WebFreight.Web
{
    public partial class HealthProb : System.Web.UI.Page
    {
        string source = "ASP.NET 4.0.30319.0";
        protected void Page_Load(object sender, EventArgs e)
        {

            string log = "Application";
            //if (!EventLog.SourceExists(source))
            //{
            //    EventLog.CreateEventSource(source, log);
            //}
            try
            {

                //EventLog.WriteEntry(source, "This is a call from HealthProb Check : " + LogitudeAppSettings.StartDateTime,EventLogEntryType.Information);

                //EventLog eventLog = new EventLog("Application");
                //eventLog.Source = "Application";
                //eventLog.WriteEntry("HealthProb Check", EventLogEntryType.Information);

                //var yourAppPool = new ServerManager().ApplicationPools["DefaultAppPool"];
                //if (yourAppPool != null)
                //{
                //    while (yourAppPool.State != ObjectState.Started)
                //    {
                //        EventLog.WriteEntry(source, "DefaultAppPool Not Started", EventLogEntryType.Warning);

                //    }
                //}

                if (LogitudeAppSettings.IsRecycled)
                {
                    try
                    {
                        LogitudeAppSettings.IsRecycled = false;
                        EventLog.WriteEntry(source, "the system app bool Recycled, the warming is starting", EventLogEntryType.Warning);
                        CallWarmingScenario();
                        EventLog.WriteEntry(source, "First warming is completed", EventLogEntryType.Information);
                        CallWarmingScenario();
                        EventLog.WriteEntry(source, "Second warming is completed", EventLogEntryType.Information);
                        LogitudeAppSettings.WarmingIsFinished = true;
                        EventLog.WriteEntry(source, "Warming is completed", EventLogEntryType.Information);
                    }
                    catch (Exception ex)
                    {
                       // ExceptionHandler.HandleException(ex, DateTime.Now, 0, "Web Startup Warming", "Web Startup Warming", "HealthProb : PageLoad Method", null);
                        EventLog.WriteEntry(source, $"Warming Scenarios exception {ex.Message}", EventLogEntryType.Information);
                        LogitudeAppSettings.WarmingIsFinished = true;
                    }

                }

                if (!LogitudeAppSettings.WarmingIsFinished)
                {
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Status = "503 ServiceUnavailable";
                    Response.StatusCode = 503;
                    Response.Flush();
                }

            }

            catch (Exception ex)
            {
                string errorMessage = ex.Message + Environment.NewLine;

                if (ex.InnerException != null)
                {

                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                }

                errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;

                EventLog.WriteEntry(source, "HealthProb Check Exception : " + errorMessage, EventLogEntryType.Error);
                // throw ex;
            }

        }

        private void CallWarmingScenario()
        {
            EventLog.WriteEntry(source, "Start Warming Scenarios", EventLogEntryType.Information);
            WarmWebService WarmService = new WarmWebService();
            WarmService.StartWarming();
            EventLog.WriteEntry(source, "Warming Scenarios are Finished", EventLogEntryType.Information);

        }
    }
}