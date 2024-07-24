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
        DateTime THEGracePeriod = new DateTime(2023, 3, 1);
        const string ServerMonitorControl = "ServerMonitorControl";

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
                    NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"Please insert {Environment.MachineName} in ServersNames!! -> Grace Period  TILL  {THEGracePeriod} ");

                }
                else
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError($"SHUTDOWN!!! Please insert ServersNames!! -Grace Period exceeded !! {THEGracePeriod} ");
                    
                    ExitEnsureLogWrite();

                }
                return;
            }

            var serviceNameList = serversNameQueryService.GetServiceNameListByMachineName(Environment.MachineName);
            serviceNameList = serviceNameList.Select(r => r.ToLower()).ToList();
            if (serviceNameList.Count == 0)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"SHUTDOWN!!! ServersName defined But {Environment.MachineName} not exist "+":"+ServerMonitorControl);
                ExitEnsureLogWrite();
                return;
            }
            NetCommonHelper.Logger.DevLog.Instance.WriteError($"{Environment.MachineName} exist in ServersName " + ":" + ServerMonitorControl);
            var curServiceName = GetServiceName();
            if (!String.IsNullOrWhiteSpace(  curServiceName ))
            {
                curServiceName = curServiceName.ToLower();
                if (serviceNameList.Contains(curServiceName))
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"{curServiceName} found in  ServersName" + ":" + ServerMonitorControl);

                }
                else
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"{curServiceName} not found ServersName " + ":" + ServerMonitorControl);

                    NetCommonHelper.Logger.DevLog.Instance.WriteError($"SHUTDOWN!!!restrict!! {curServiceName} not found ServersName " + ":" + ServerMonitorControl);
                    ExitEnsureLogWrite();
                    return;
                }
            }
            else
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"GetServiceName() == null" + ":" + ServerMonitorControl);
            }
        }

        private static void ExitEnsureLogWrite()
        {
            for (int i = 0; i < 10; i++)
            {
                //Task.Delay(TimeSpan.FromSeconds(1));
                System.Threading.Thread.Sleep(200);
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("SHUTDOWN!!! Before Exit - try to Write logs");
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
