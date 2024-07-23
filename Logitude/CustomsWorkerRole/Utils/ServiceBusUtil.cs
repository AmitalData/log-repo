using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsWorkerRole.Utils
{
    public class ServiceBusUtil
    {
        public static long  CurrentMessageCount(string connectionString, string queueName)
        {
            NamespaceManager nsmgr = Microsoft.ServiceBus.NamespaceManager.CreateFromConnectionString(connectionString);
            QueueDescription queueDescription = nsmgr.GetQueue(queueName);
            long count = queueDescription.MessageCount;
            return count;
        }
        public static void GetAllQs( IEnumerable<QueueDescription> qs)
        {
            foreach (var item in qs)
            {

                var mes = "queueName=" + item.Path + ":MessageCount=" + item.MessageCount.ToString();
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug(mes);
                
            }

        }

        public static string ShowAll()
        {
            if (LogitudeSettings.QueueServiceMode == "db")
            {
                return "LogitudeSettings.QueueServiceMode == db";
            }
            //string emailQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment(Logitude.Server.Tools.Helpers.SBQueueNames.CustomsMessagingSheetBQ.ToString()); //Amitalqueue
            var sb= new StringBuilder(); 
            var dummyQ="ThreadedRoleEntryPoint.GetQueueByEnviroment.dummyQ";
            var start = ThreadedRoleEntryPoint.GetQueueByEnviroment(dummyQ);
            var myNS=start.Replace(dummyQ, ""); 
            foreach (var item in StorageAcountDetails.NameSpaceManager.GetQueues())
            {
                var path=item.Path.ToString();
                if (path.StartsWith(myNS, StringComparison.OrdinalIgnoreCase))
                {
                    sb.AppendLine(item.Path + ":TotalMessageCount=" + item.MessageCount.ToString());
                    if (item.MessageCountDetails.ActiveMessageCount > 0)
                    {
                        sb.AppendLine(item.Path + "::ActiveMessageCount:=" + item.MessageCountDetails.ActiveMessageCount.ToString());
                    }
                    if (item.MessageCountDetails.DeadLetterMessageCount > 0)
                    {
                        sb.AppendLine(item.Path + "::DeadLetterMessageCount:=" + item.MessageCountDetails.DeadLetterMessageCount.ToString());
                    }
                    if (item.MessageCountDetails.ScheduledMessageCount > 0)
                    {
                        sb.AppendLine(item.Path + "::ScheduledMessageCount:=" + item.MessageCountDetails.ScheduledMessageCount.ToString());
                    }
                    
                }
            }
            return sb.ToString(); 
        }



        
    }
}
