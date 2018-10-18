using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Data.ShipmentsModel;
using Simplog.Server.Infrastructure.Azure;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Microsoft.Practices.Unity;

namespace CommunicationWorkerRole
{
    public class HypredDataWorkerRole : WorkerEntryPoint
    {
        private bool serviceStarted = true;
        private int interval = 1;
        private int systemErrorCount = 0;
        QueueDescription queueDescription;
        QueueClient client;

        public override void Run()
        {

            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        int tenant = 0;
                        var message = client.Receive(new TimeSpan(0, 0, 30));
                        LastActivity = DateTime.UtcNow;
                        if (message != null)
                        {

                            try
                            {
                                string fileId = message.Properties["FileId"].ToString();

                                GetFile(fileId);

                                message.Complete();
                                LogDoneItemInMemory();
                            }
                            catch (Exception ex)
                            {
                                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "WorkerRole", "", null);
                                if (message.Properties.Keys.Contains("FileId"))
                                {
                                    string fileId = message.Properties["FileId"].ToString();
                                    if (fileId != null)
                                    {
                                        message.Abandon();
                                    }
                                    else
                                    {
                                        message.Complete();
                                    }
                                }
                                else
                                {
                                    message.Complete();
                                }


                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null,null);
                        Thread.Sleep(10000);
                    }

                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        IShipmentsContext objectContext;
        private void GetFile(string fileId)
        {

            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = fileId,
                //FolderName = document.Folder,
                //Extension = document.Extension,
               
                ExternalContainerName = "hypreddata",
                HasExternalContainer = true,
            };
            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            byte[] datainByte = storageservice.Read(fileInfo);


                  

           // CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer("hypreddata");
            //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, waitingCommLog.Document.Folder));
           // CloudBlockBlob blobfile = blobContainer.GetBlockBlobReference(fileId);
            // if (blobfile.Exists()) // if file doesn't exist, exception will occured and catch method will increase number of retries!
            //   {
            //using (MemoryStream memstream = new MemoryStream())
            //{

                //blobfile.DownloadToStream(memstream);
               // byte[] datainByte = memstream.ToArray();

                MemoryStream stream = new MemoryStream(datainByte);

                var serializer = new XmlSerializer(typeof(ShipmentPM));
                ShipmentPM entityPM = serializer.Deserialize(stream) as ShipmentPM;

                if (objectContext == null)
                {
                    objectContext = ShipmentsContext.GetContext(entityPM.Tenant);
                }
                ShipmentService service = new ShipmentService(objectContext, entityPM, "islam@fnarsoft.com");
                service.Create();

                storageservice.Delete(fileInfo);
               // blobfile.Delete();
                

            //}
        }


        public override bool OnStart()
        {
            try
            {

                ThreadId = Guid.NewGuid().ToString();
                BatchServiceCode = "HypredData";
                DoneItemsInRange = new Dictionary<DateTime, int>();

                string emailQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment("HypredDataQueue");


                if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailQueueName))
                {
                    queueDescription = new QueueDescription(emailQueueName);
                    queueDescription.MaxSizeInMegabytes = 5120;
                    // queueDescription.DefaultMessageTimeToLive = new TimeSpan(3, 1, 0);

                    StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
                }

                client = StorageAcountDetails.CreateServiceBusQueueClient(emailQueueName);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "hypred worker role start", null,null);
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
    }
}
