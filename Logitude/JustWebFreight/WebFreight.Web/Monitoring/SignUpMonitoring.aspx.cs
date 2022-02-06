using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage.Queue;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

using System.Web.UI;
using System.Web.UI.WebControls;
using Logitude.SystemLogs;
using Microsoft.WindowsAzure.Storage;

namespace WebFreight.Web.Monitoring
{
    public partial class SignUpMonitoring : System.Web.UI.Page
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
            bool isFailed = false;

            try
            {
                CloudStorageAccount storageAccount = StorageAcountDetails.StorageAccount;
                CloudQueueClient queueclient = storageAccount.CreateCloudQueueClient();
                CloudQueue queue = queueclient.GetQueueReference("signupqueue");
                queue.CreateIfNotExists();
                return queue.ApproximateMessageCount > 50;
            }

            catch (Exception errorInfo)
            {
                ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "SignUpMonitoring", "Bug in SignUpMonitoring Method ", "");
            }

            return isFailed;

        }

        private static long GetMessageQueueCount(string emailqueueName)
        {
            long messagecount;
            QueueDescription queueDescription = new QueueDescription(emailqueueName);
            queueDescription.MaxSizeInMegabytes = 5120;
            queueDescription.MaxDeliveryCount = 99999;
            queueDescription.LockDuration = new TimeSpan(0, 2, 0);
            messagecount = StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription).MessageCount;
            return messagecount;
        }
    }
}