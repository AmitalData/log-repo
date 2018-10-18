using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsWorkerRole.Utils
{
    public class LogUtil
    {
        public static void LogMe(string log, string type, DateTime clientDate, string message , string stackTrace , int tenant, string userId, string userName,string IP)
        {
            AzureLog.SaveLogsInStorage(log, type, clientDate, message, stackTrace, tenant, userId, userName, IP);
            //ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method", null);
        }
    }
}
