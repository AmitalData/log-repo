using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace LogitudeBatchServices
{
    public  class LogitudeBatchServiceHelper
    {
        private static string serviceName = "";

        public static string ServiceName { get => GetServiceName();  }

        public static string GetCommandLineArgs(Process process)
        {
           
            if (process == null)
            {
                return string.Empty;
            }
      
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
                using (var objects = searcher.Get())
                {
                    var result = objects.Cast<ManagementBaseObject>().SingleOrDefault();
                    return result?["CommandLine"]?.ToString() ?? "";
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string GetServiceName()
        {
            if (serviceName != "")
                return serviceName;
            var processId = Process.GetCurrentProcess().Id;
            var query = $"SELECT * FROM Win32_Service where ProcessId = {processId}";
            var managementObject = new ManagementObjectSearcher(query).Get().Cast<ManagementObject>().FirstOrDefault();

            if (managementObject == null)
            {
                return "BatchServicesManager(HardCoded)";
            }

            var sn = managementObject["Name"].ToString();
            serviceName = sn;
            return sn;
        }

        public static string getWorkerRoleName()
        {
            if (CommunicationWorkerRole.ThreadedRoleEntryPoint.getWorkerRoleName() == "")
                CommunicationWorkerRole.ThreadedRoleEntryPoint.SetWorkerRoleName();
           return CommunicationWorkerRole.ThreadedRoleEntryPoint.getWorkerRoleName();
        }
    }
}
