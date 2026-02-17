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

namespace WebFreight.Web.Monitoring
{
    public partial class ContactUnseenMobile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


        

            Response.Clear();
            Response.ContentType = "text/xml";
            //Response.Write("<?xml version='1.0' encoding='UTF-8'?>");
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
                using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())
                {

                    string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment("contactunseenentityqueue");
                    long messagecount = 0;
                    if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailqueueName))
                    {

                        QueueDescription queueDescription = new QueueDescription(emailqueueName);
                        queueDescription.MaxSizeInMegabytes = 5120;
                        queueDescription.MaxDeliveryCount = 99999;
                        queueDescription.LockDuration = new TimeSpan(0, 2, 0);
                        long messagescount = StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription).MessageCount;

                    }
                    else
                    {

                        messagecount = StorageAcountDetails.NameSpaceManager.GetQueue(emailqueueName).MessageCount;
                        QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailqueueName, ReceiveMode.PeekLock);

                    }


                    if (messagecount > 50)
                    {
                        isFailed = true;
                    }

                    else isFailed = false;
                }
            }

            catch (Exception errorInfo)
            {
                ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "ContactUnseenMobile", "Bug in ContactUnseenMobile Method ", "");
            }
            
            return isFailed;
            
        }


    }
}