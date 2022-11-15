using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace CustomsWorkerRole.BL
{
    public class ServerMonitorControlService
    {
        /*const */
        DateTime THEGracePeriod = new DateTime(2023, 2, 1);


        public void StopProccessIfNotExist()
        {
            if (Environment.UserInteractive)
            {
                return;
            }


            var serversNameQueryService = new ServersNameQueryService(0);
            if (!serversNameQueryService.Any())
            {
                if (DateTime.Now < THEGracePeriod)
                {
                    Logger.LogMe($"Please insert {Environment.MachineName} in ServersNames!! -> Grace Period  TILL  {THEGracePeriod} ", false, "ServerMonitorControl");

                }
                else
                {
                    Logger.LogMe($"Please insert ServersNames!! -Grace Period exceeded !! {THEGracePeriod} ", true, "ServerMonitorControl");
                    ExitEnsureLogWrite();

                }
                return;
            }

            var serviceNameList = serversNameQueryService.GetServiceNameListByMachineName(Environment.MachineName);
            serviceNameList = serviceNameList.Select(r => r.ToLower()).ToList();
            if (serviceNameList.Count == 0)
            {
                Logger.LogMe($"ServersName defined But {Environment.MachineName} not exist ", true, "ServerMonitorControl");
                ExitEnsureLogWrite();
                return;
            }
            Logger.LogMe($"{Environment.MachineName} exist in ServersName ", false, "ServerMonitorControl");
            var curServiceName = GetServiceName();
            if (!String.IsNullOrWhiteSpace(  curServiceName ))
            {
                curServiceName = curServiceName.ToLower();
                if (serviceNameList.Contains(curServiceName))
                {
                    Logger.LogMe($"{curServiceName} found in  ServersName", false, "ServerMonitorControl");

                }
                else
                {
                    Logger.LogMe($"{curServiceName} not found ServersName ", false, "ServerMonitorControl");
                }
            }
            else
            {
                Logger.LogMe($"GetServiceName() == null", false, "ServerMonitorControl");
            }
        }

        private static void ExitEnsureLogWrite()
        {
            for (int i = 0; i < 10; i++)
            {
                //Task.Delay(TimeSpan.FromSeconds(1));
                System.Threading.Thread.Sleep(200);
                Debug.WriteLine("Before Exit - try to Write logs");
            }

            Environment.Exit(0);
        }




        protected String GetServiceName()
        {
            try
            {


                // Calling System.ServiceProcess.ServiceBase::ServiceNamea allways returns
                // an empty string,
                // see https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=387024

                // So we have to do some more work to find out our service name, this only works if
                // the process contains a single service, if there are more than one services hosted
                // in the process you will have to do something else

                int processId = System.Diagnostics.Process.GetCurrentProcess().Id;
                String query = "SELECT * FROM Win32_Service where ProcessId = " + processId;

                var searcher = new System.Management.ManagementObjectSearcher(query);

                foreach (System.Management.ManagementObject queryObj in searcher.Get())
                {
                    return queryObj["Name"].ToString();
                }

                //throw new Exception("Can not get the ServiceName");

                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
