using System;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

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
                        storageaccount = new CloudStorageAccount(new StorageCredentials(LogitudeSettings.StorageAccountName, LogitudeSettings.StorageAccountKey),
    new Uri(@"http://" + LogitudeSettings.StorageAccountName + ".blob.core.windows.net/"),
    new Uri(@"http://" + LogitudeSettings.StorageAccountName + ".queue.core.windows.net/"),
    new Uri(@"http://" + LogitudeSettings.StorageAccountName + ".table.core.windows.net/"), null);
                    }
                    //                switch (LogitudeSettings.DeploymentStage)
                    //                {
                    //                    case "Dev":
                    //                        storageaccount = new CloudStorageAccount(new StorageCredentials("devstoreaccount1", "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw=="),
                    //new Uri(@"http://127.0.0.1:10000/devstoreaccount1/"),
                    //new Uri(@"http://127.0.0.1:10001/devstoreaccount1/"),
                    //new Uri(@"http://127.0.0.1:10002/devstoreaccount1/"));
                    //                        break;

                    //                    case "Simplog":
                    //                        storageaccount = new CloudStorageAccount(new StorageCredentials("simplog", "4TBEStcAfMhUqpZOfuJby9OAAj2b2CHnAxAd3xeTeuC1IhjOkanV3iAEv4u6N8IiAGDjK04t0PqsiFCnybk+og=="),
                    //new Uri(@"http://simplog.blob.core.windows.net/"),
                    //new Uri(@"http://simplog.queue.core.windows.net/"),
                    //new Uri(@"http://simplog.table.core.windows.net/"));
                    //                        break;

                    //                    case "Test1":
                    //                        storageaccount = new CloudStorageAccount(new StorageCredentials("simplogtest1", "3TMyh2uVpfmx5u9PqNMUXQ73yjqWADDJiGQzu/tWRnRECRHtQ2kMmTxZr7/Z8LXcY3b8Abus3xczvCp0OO6/rA=="),
                    //new Uri(@"http://simplogtest1.blob.core.windows.net/"),
                    //new Uri(@"http://simplogtest1.queue.core.windows.net/"),
                    //new Uri(@"http://simplogtest1.table.core.windows.net/"));
                    //                        break;


                    //                    case "LogitudeTest":
                    //                        storageaccount = new CloudStorageAccount(new StorageCredentials("logitudetest", "mLXeQ+QjE4BW7Eb3RLcWFKj/PvE6gTIKpwluMjU/TWdFj15Dm76qWhJ/nLz099kRgaqUV2RMIt5B083SXvhhpQ=="),
                    //new Uri(@"http://logitudetest.blob.core.windows.net/"),
                    //new Uri(@"http://logitudetest.queue.core.windows.net/"),
                    //new Uri(@"http://logitudetest.table.core.windows.net/"));
                    //                        break;

                    //                    case "LocalStorage":
                    //                        storageaccount = new CloudStorageAccount(new StorageCredentials("devstoreaccount1", "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw=="),
                    //new Uri(@"http://127.0.0.1:10000/devstoreaccount1/"),
                    //new Uri(@"http://127.0.0.1:10001/devstoreaccount1/"),
                    //new Uri(@"http://127.0.0.1:10002/devstoreaccount1/"));
                    //                        break;

                    //                    case "logitudeeu1":
                    //                        storageaccount = new CloudStorageAccount(new StorageCredentials("logitudeeu1", "Uhk6O+aGeORy1Twv6FMbq1vXj0D691Jwaj+/hDTXzDm8bqvVrs+26zTBahLuTNH/UYTWV+tJouf6EyP9Y7jS/A=="),
                    //new Uri(@"http://logitudeeu1.blob.core.windows.net/"),
                    //new Uri(@"http://logitudeeu1.queue.core.windows.net/"),
                    //new Uri(@"http://logitudeeu1.table.core.windows.net/"));
                    //                        break;

                    //                    case "customs":
                    //                        storageaccount = new CloudStorageAccount(new StorageCredentials("customs", "u9o4h/6RCuGJpzUVc+meEqj8fJLbCHZQ4//rQvHp6Jid5KU/mhRJszSmtpyU+l4xfF3RAPfkYyScRmM8+JUZ7A=="),
                    //new Uri(@"http://customs.blob.core.windows.net/"),
                    //new Uri(@"http://customs.queue.core.windows.net/"),
                    //new Uri(@"http://customs.table.core.windows.net/"));
                    //                        break;

                    //                    case "amital":
                    //                        storageaccount = new CloudStorageAccount(new StorageCredentials("amital", "n8X8wzZBrYbjMWCpNw/JyreZRrjeQ6fqTGCGwuSj5mGSBOfIyAO1SElkMEqK2i7CrDPK4kJajHjxa/ZNxxTtwg=="),
                    //new Uri(@"http://amital.blob.core.windows.net/"),
                    //new Uri(@"http://amital.queue.core.windows.net/"),
                    //new Uri(@"http://amital.table.core.windows.net/"));
                    //                        break;

                    //                    case "amitalcloud":
                    //                        storageaccount = new CloudStorageAccount(new StorageCredentials("amitalcloud", "czvnH30OagkI4VLkkjJiaPp1n6XwMgfy6qurGxwGT7AxfPE9ns+KLOp87lrEQoRKCx6jeJIRx2cTxytUiFK4pA=="),
                    //new Uri(@"http://amitalcloud.blob.core.windows.net/"),
                    //new Uri(@"http://amitalcloud.queue.core.windows.net/"),
                    //new Uri(@"http://amitalcloud.table.core.windows.net/"));
                    //                        break;

                    //                    case "logitudetest2":
                    //                        storageaccount = new CloudStorageAccount(new StorageCredentials("logitudetest2", "6SJt9T1pUn8upIAw5A2J6svg72fW67VQWyEryTCT1GPYw1qm4/dL9GG0PwBOrL3Tv3RmO+az5J8z2TEH2wRItg=="),
                    //new Uri(@"http://logitudetest2.blob.core.windows.net/"),
                    //new Uri(@"http://logitudetest2.queue.core.windows.net/"),
                    //new Uri(@"http://logitudetest2.table.core.windows.net/"));
                    //                        break;

                    //                    case "logitudetest3":
                    //                        storageaccount = new CloudStorageAccount(new StorageCredentials("logitudetest3", "yadxhgB9xB8BtdNThidHCtXcXxMe0447emamP0JNJORajR++sGs2x/UnbwcKQ7p3512lFcggVpSoiJ9bLUHlhA=="),
                    //new Uri(@"http://logitudetest3.blob.core.windows.net/"),
                    //new Uri(@"http://logitudetest3.queue.core.windows.net/"),
                    //new Uri(@"http://logitudetest3.table.core.windows.net/"));
                    //                        break;
                    //                }

                    //storageAccount = storageaccount;

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
                        nameSpaceManager = NamespaceManager.CreateFromConnectionString(GetSettingByName(LogitudeSettings.DeploymentStage));
                }
            return nameSpaceManager;
        }
    
	}


    private static string dataCacheTopicName;

    public static string DataCacheTopicName
    {
        get
        {
            if (LogitudeSettings.DeploymentStage == "Dev")
            {
                dataCacheTopicName = Environment.MachineName;
            }
            else if (LogitudeSettings.DeploymentStage == "Simplog")
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

               return Microsoft.ServiceBus.Messaging.QueueClient.CreateFromConnectionString(GetSettingByName(LogitudeSettings.DeploymentStage), QueueName);
           }

           public static QueueClient CreateServiceBusQueueClient(string QueueName,ReceiveMode receivemode)
           {
               //var messagingFactory = MessagingFactory.Create(NameSpaceManager.Address,NameSpaceManager.Settings.TokenProvider);

               return Microsoft.ServiceBus.Messaging.QueueClient.CreateFromConnectionString(GetSettingByName(LogitudeSettings.DeploymentStage), QueueName, receivemode);
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
            if (!LogitudeSettings.IsCostomsDeploy)
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

        public static string GetBlobNameByLocation(string blobname,string location)
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

                case "Tariff":
                    {
                        blobName = blobname;
                        break;
                    }

                case "others":
                default:
                    blobName = "others/" + blobname;
                    break;

            }

            return blobName;

        }

        public static string GetSettingByName(string enviroment)
        {
            string result = "";

            switch (enviroment)
            {
                case "Simplog" :
                    result = "Endpoint=sb://logitudeeu1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=l4gltH9kzHBZSaedncIYYu5tb6YTS4UZftIGUPYHHFw=";//"Endpoint=sb://logitudeeu1.servicebus.windows.net/;SharedSecretIssuer=owner;SharedSecretValue=t3IPjGhc6MOE5FZfKqKv5R6JNwsC/RPkW5axmdp+RUY=";
                    break;

                case "amitalstorage":
                    result = "Endpoint=sb://amitalwe1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=kYh5EX8few4tSQ2IJUWYz3NXMs+25X8282WwekJROiQ=";
                    break;

                case "Test2":
                    result = "Endpoint=sb://logitudetest2.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Uo7BHCCC7xAQIs1gO27hmruaGpFvoXDhwqATqVsH6PY=";
                    break;

                case "logitudepreproduction":
                    result = "Endpoint=sb://logitudepreproduction.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=I7E9gyzPLLpD5+Qqub6/97l0f8WyckwMWAwsRcfVyqo=";
                    break;

                case "logboxwe1":
                    result = "Endpoint=sb://logboxwe1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=1ICW1EzCyGOnpj8nkC3wklYdM/4WFwCZJPAiWTNJFvs=";
                    break;
                
                //case "Dev":
                //case "Test1":
                default:
                    result = "Endpoint=sb://logitudetest2.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Uo7BHCCC7xAQIs1gO27hmruaGpFvoXDhwqATqVsH6PY=";
                    //result = "Endpoint=sb://logitudetest1.servicebus.windows.net;SharedSecretIssuer=owner;SharedSecretValue=5iKNFIINnT+5u3Zj5SFkaRou/0QYxx7OWzZL/Wlh7us=";
                    break;

            }

            return result;
        }

    }
}