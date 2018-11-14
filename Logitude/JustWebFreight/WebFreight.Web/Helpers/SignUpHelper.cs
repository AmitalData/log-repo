using Microsoft.WindowsAzure.Storage.Queue;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class SignUpHelper
    {
        public void SendMessageToQueue(byte[] msg)
        {
            var storageaccount = StorageAcountDetails.StorageAccount;
            var queueclient = storageaccount.CreateCloudQueueClient();

            var queue = queueclient.GetQueueReference("signupqueue");
            queue.CreateIfNotExists();

            var message = new CloudQueueMessage(msg);
            queue.AddMessage(message);

        }
    }
}