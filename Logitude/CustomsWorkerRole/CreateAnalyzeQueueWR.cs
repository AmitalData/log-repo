using Logitude.Server.Tools.Counters;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomsWorkerRole
{
    class CreateAnalyzeQueueWR : WorkerEntryPoint
    {
        QueueDescription queueDescription;
        QueueClient client;
        public  readonly  static  string QueueName;
        static  CreateAnalyzeQueueWR()
        {
            QueueName = "CustomsQueue";
        }
        public override void Run()
        {

            while (true)
            {
                try
                {
                    WorkOnce(); 
                }
                catch (Exception e)
                {

                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "ChampMessageInWR : SaveMessageToAnalyzeQueue Method", null);
                }
                
            }

        }

        private void SaveMessageToAnalyzeQueue(byte[] messageBytes)
        {
            AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            //byte[] messageBytes = Encoding.ASCII.GetBytes(messageData);
            //string mmm = Encoding.ASCII.GetString(messageBytes);
            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                From = "Customs",
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = messageBytes,
                Status = "W",
                Retries = 0,
                ConnectedToEntity = false,
                ConnectedToTenant = false,
                FileSize = messageBytes.Length,

            };
            analyzeQueueReposiory.Add(analyzeQueue);
            analyzeQueueReposiory.SubmitChanges();
        }




        public override bool OnStart()
        {
            try
            {
                string queueName = ThreadedRoleEntryPoint.GetQueueByEnviroment(CreateAnalyzeQueueWR.QueueName);


                if (!StorageAcountDetails.NameSpaceManager.QueueExists(queueName))
                {
                    queueDescription = new QueueDescription(queueName);
                    StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
                }

                client = StorageAcountDetails.CreateServiceBusQueueClient(queueName);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null,null);
            }

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

        public override void WorkOnce()
        {
            if (!General.IsUpdating())
            {
                var message = client.Receive();
                try
                {

                if (message != null)
                {
                    byte[] data;
                    var messageStream = message.GetBody<Stream>();
                    ////  string messageData = Encoding.ASCII.GetString(messageStream.GetBuffer());//message.Properties["MessageXML"].ToString();
                    byte[] buffer = new byte[16 * 1024];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = messageStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        data = ms.ToArray();
                    }
                    if (data != null)
                    {
                        SaveMessageToAnalyzeQueue(data);
                        message.Complete();
                    }
                }

                }
                catch (Exception e)
                {
                    
                    if (message.Properties.Keys.Contains("MessageXML"))
                    {

                        message.Abandon();

                    }
                    else
                    {
                        message.Complete();
                    }
                    throw;
                }
            }

            else
            {
                Thread.Sleep(60000);
            }
        }
    }
}
