using Microsoft.WindowsAzure.Storage.Queue;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class QueueMessagesWebHelper
    {

        public  void BackUpForClientData(int tenant)
        {
            var storageaccount = StorageAcountDetails.StorageAccount;
            var queueclient = storageaccount.CreateCloudQueueClient();
            CloudQueue queue = null;

            queue = queueclient.GetQueueReference("clientdatabackup");
            queue.CreateIfNotExists();

            string msg = "ClientDataBackup" + "," + tenant.ToString();
            var message = new CloudQueueMessage(msg);
            queue.AddMessage(message);
        }
    }
}