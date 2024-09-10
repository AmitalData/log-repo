using System;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Server.Infrastructure.Azure
{
    public class StorageAcountDetails
    {

        //private static CloudStorageAccount storageAccount;
        private static CloudStorageAccount storageaccount = null;
        public static CloudStorageAccount StorageAccount
        {
            get
            {
                if (storageaccount == null)
                {
                    //CloudStorageAccount storageaccount = null;
                    if (LogitudeSettings.StorageType.ToLower() == "azureemulator")
                    {
                        storageaccount = new CloudStorageAccount(new StorageCredentials(LogitudeSettings.StorageAccountName, LogitudeSettings.StorageAccountKey),
    new Uri(@"http://127.0.0.1:10000/" + LogitudeSettings.StorageAccountName + "/"),
    new Uri(@"http://127.0.0.1:10001/" + LogitudeSettings.StorageAccountName + "/"),
    new Uri(@"http://127.0.0.1:10002/" + LogitudeSettings.StorageAccountName + "/"), null);
                    }
                    else
                    {

                        string dProtocol = @"http://";
                        if (LogitudeSettings.IsCostomsDeploy || LogitudeSettings.StorageAccountName.Equals("amitalexporttest", StringComparison.OrdinalIgnoreCase))
                        {
                            dProtocol = @"httpS://";
                        }



                        storageaccount = new CloudStorageAccount(new StorageCredentials(LogitudeSettings.StorageAccountName, LogitudeSettings.StorageAccountKey),
    new Uri(dProtocol + LogitudeSettings.StorageAccountName + ".blob.core.windows.net/"),
    new Uri(dProtocol + LogitudeSettings.StorageAccountName + ".queue.core.windows.net/"),
    new Uri(dProtocol + LogitudeSettings.StorageAccountName + ".table.core.windows.net/"), null);

                    }
                   

                    return storageaccount;
                }
                return storageaccount;
            }

        }
        private static CloudBlobClient blobClient;

        public static CloudBlobClient BlobClient
        {
            get
            {
                if (StorageAccount != null)
                {
                    blobClient = StorageAccount.CreateCloudBlobClient();
                }
                return blobClient;
            }
        }

        private static CloudQueueClient queueClient;
        public static CloudQueueClient QueueClient
        {
            get
            {
                if (StorageAccount != null)
                {
                    queueClient = StorageAccount.CreateCloudQueueClient();
                }
                return queueClient;
            }
        }

        private static CloudTableClient tableClient;
        public static CloudTableClient TableClient
        {
            get
            {
                if (StorageAccount != null)
                {
                    tableClient = StorageAccount.CreateCloudTableClient();
                }
                return tableClient;
            }
        }



        private static NamespaceManager nameSpaceManager;

        public static NamespaceManager NameSpaceManager
        {
            get
            {
                if (StorageAccount != null)
                {
                    if (nameSpaceManager == null)
                        nameSpaceManager = NamespaceManager.CreateFromConnectionString(GetSettingByName());
                }
                return nameSpaceManager;
            }

        }


        private static string dataCacheTopicName;

        public static string DataCacheTopicName
        {
            get
            {
                if (SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Development))
                {
                    dataCacheTopicName = Environment.MachineName;
                }
                else if (SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Simplog))
                {
                    dataCacheTopicName = "production";
                }
                else
                {
                    dataCacheTopicName = "test";
                }
                return dataCacheTopicName;
            }

        }

        private static string signalRHubTopicName;

        public static string SignalRHubTopicName
        {
            get
            {
                signalRHubTopicName = "SignalRHub";
                return signalRHubTopicName;
            }

        }
        //   private static  NamespaceManager CreateNamespaceManager()
        //{
        //    // Create the namespace manager which gives you access to
        //    // management operations
        //    Uri uri=null;
        //    TokenProvider tP=null;
        //    switch (WebFreightEntryPoint.DeploymentStage)
        //    {
        //        default:
        //            uri = ServiceBusEnvironment.CreateServiceUri("sb", "logitudetest1", String.Empty);
        //            tP = TokenProvider.CreateSharedSecretTokenProvider("owner", "5iKNFIINnT+5u3Zj5SFkaRou/0QYxx7OWzZL/Wlh7us=");
        //            break;
        //    }


        //    return new NamespaceManager(uri, tP);
        //}


        public static QueueClient CreateServiceBusQueueClient(string QueueName)
        {
            //var messagingFactory = MessagingFactory.Create(NameSpaceManager.Address,NameSpaceManager.Settings.TokenProvider);

            return Microsoft.ServiceBus.Messaging.QueueClient.CreateFromConnectionString(GetSettingByName(), QueueName);
        }

        public static QueueClient CreateServiceBusQueueClient(string QueueName, ReceiveMode receivemode)
        {
            //var messagingFactory = MessagingFactory.Create(NameSpaceManager.Address,NameSpaceManager.Settings.TokenProvider);

            return Microsoft.ServiceBus.Messaging.QueueClient.CreateFromConnectionString(GetSettingByName(), QueueName, receivemode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenant">Current Tenant that request the method</param>
        /// <param name="Type">Determine type of container : available values : Tenant</param>
        /// <returns></returns>
        public static CloudBlobContainer GetCurrentContainer(int tenant)
        {

            string containername = "tenant" + tenant.ToString();
            CloudBlobContainer blobContainer = BlobClient.GetContainerReference(containername);
            if (!LogitudeSettings.IsCostomsDeploy)
            {
                blobContainer.CreateIfNotExists();
            }

            return blobContainer;
        }


        public static CloudBlobContainer GetCurrentContainer(string containername)
        {


            CloudBlobContainer blobContainer = BlobClient.GetContainerReference(containername);

            {
                blobContainer.CreateIfNotExists();
            }

            return blobContainer;
        }

        public enum ContainersTypes
        {
            Tenant,
        }

        public enum Locations
        {
            DocsIn,
            DocsOut,
            Logos,
            Dlls,
            Others,
            TenantBackup,
        }

        public static string GetBlobNameByLocation(string blobname, string location)
        {
            string blobName = "";

            switch (location)
            {
                case "quotetemplatesectionfiles":
                    {
                        blobName = "quotetemplatesectionfiles/" + blobname;
                        break;
                    }

                case "docsin":
                    {
                        blobName = "docsin/" + blobname;
                        break;
                    }

                case "docsout":
                    {
                        blobName = "docsout/" + blobname;
                        break;
                    }

                case "logos":
                    {
                        blobName = "logos/" + blobname;
                        break;
                    }

                case "dlls":
                    {
                        blobName = "dlls/" + blobname;
                        break;
                    }

                case "tenantbackup":
                    {
                        blobName = "tenantbackup/" + blobname;
                        break;
                    }

                case "images":
                    {
                        blobName = "images/" + blobname;
                        break;
                    }

                case "champ":
                    {
                        blobName = "champ/" + blobname;
                        break;
                    }

                case "reports":
                    {
                        blobName = "reports/" + blobname;
                        break;
                    }

                case "charts":
                    {
                        blobName = "charts/" + blobname;
                        break;
                    }

                case "how-to":
                    {
                        blobName = blobname;
                        break;
                    }

                case "tariff":
                    {
                        blobName = "tariff/" + blobname;
                        //blobName = blobname;
                        break;
                    }
                case "multiprint":
                    {
                        blobName = "multiprint/" + blobname;
                        //blobName = blobname;
                        break;
                    }
                case "termsOfUse":
                    {
                        blobName = "termsOfUse/" + blobname;
                        break;
                    }
                case "others":
                default:
                    blobName = "others/" + blobname;
                    break;

            }

            return blobName;

        }

        public static string GetSettingByName()
        {
            
            if (SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Cloud))
                return "Endpoint=sb://sb-amitalcloud-prod-il-01.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=srzCxGuZnOAjNkceq6mP1UGxDI6S2USz4+ASbI5qRXE=";
          
            if (SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Logbox))
               return "Endpoint=sb://logboxwe1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=1ICW1EzCyGOnpj8nkC3wklYdM/4WFwCZJPAiWTNJFvs=";

            if(SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Development))
                return "Endpoint=sb://logitudetest2.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Uo7BHCCC7xAQIs1gO27hmruaGpFvoXDhwqATqVsH6PY=";



            return string.Empty;
        }

    }
}