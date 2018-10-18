using System.Linq;

using Microsoft.WindowsAzure.ServiceRuntime;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Threading;

namespace Simplog.Server.Infrastructure
{
    public class WebFreightEntryPoint : RoleEntryPoint
    {
        public static bool UsingAzure = false; // when true, Use Azure methods
        public static bool IsComplieEnabled = false;
        public static bool CheckConnectionStrategy = false;
        public static DateTime CheckConnectionStartDate;
        //public static bool IsLogEnabled = true;
       // public static bool InAzureStorage = false; // when true, Azure storage will be used
       // public static bool IsStaging = false; // Use online staging for testing.
        //public static string DeploymentStage = "Dev";//Dev//Test1//LogitudeTest//Simplog//Test1//logitudetest2//amital
        //public static bool UsingAzure = true; // when true, Use Azure methods
        //*****Test Branch******
        //int tt = 1;
        //**********************
        public override void Run()
        {
            //TopicDescription dataCacheTopic;
            //if (!StorageAcountDetails.NameSpaceManager.TopicExists("DataCacheTopic"))
            //{
            //    dataCacheTopic = StorageAcountDetails.NameSpaceManager.CreateTopic("DataCacheTopic");
            //}

            //SubscriptionDescription myAgentSubscription = StorageAcountDetails.NameSpaceManager.CreateSubscription("DataCacheTopic", Environment.MachineName);
            //CacheMessageHandler cacheMessageHandler = new CacheMessageHandler();
            //Thread cacheThread = new Thread(cacheMessageHandler.HandleTopicMessages);
            //cacheThread.Start();
            base.Run();
        }
        public override bool OnStart()
        {
            
           // InAzure = true;

            //Start Azure Diagnostics
           // AzureDiagnostics.InitializeAzureDiagnostics();

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

        /// <summary>
        /// Return the name of queue depend on enviroment(Dev,Prodcution or Test)
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public static string GetQueueByEnviroment(string queueName)
        {
            var savequeueName = queueName;   
            if (LogitudeSettings.DeploymentStage == "Dev")
            {
                queueName = Environment.MachineName + "_" + queueName;
            }
            else if (LogitudeSettings.DeploymentStage == "customs")
            {
                queueName = "customs" + "_" + queueName;
            }
            else if (LogitudeSettings.DeploymentStage == "Simplog" || LogitudeSettings.DeploymentStage == "amitalstorage")
            {
                queueName = "Production" + "_" + queueName;
            }
            else
            {
                queueName = "Test" + "_" + queueName;
            }
            if (LogitudeSettings.IsCostomsDeploy)
            {
                var uri = new Uri(LogitudeSettings.LogitudeURL);
                    var branch = uri.LocalPath.Trim(@"\"[0]).Trim(@"/"[0]);
                    queueName = LogitudeSettings.StorageAccountName + "_Customs" + 
                        //customsDeploymentStage.ToString() + 
                        branch + "_" + savequeueName;
            }
            /*
            var customsDeploymentStage = SettingUtil.GetCustomsDeploymentStage();
            switch (customsDeploymentStage)
            {
                case SettingUtil.CustomsDeploymentStage.Test:
                case SettingUtil.CustomsDeploymentStage.Pilot:

                    var uri = new Uri(LogitudeSettings.LogitudeURL);
                    var branch = uri.LocalPath.Trim(@"\"[0]).Trim(@"/"[0]);
                    queueName = LogitudeSettings.StorageAccountName + "_Customs" + customsDeploymentStage.ToString() + branch + "_" + savequeueName;
                    break;
                case SettingUtil.CustomsDeploymentStage.Production:
                                                         //queueName = "Customs" + customsDeploymentStage.ToString() + "_" + savequeueName;
                    queueName = LogitudeSettings.StorageAccountName + "_Customs" + customsDeploymentStage.ToString() + "_" + savequeueName;
                    break;

                default:
                    break;
            }
             */ 

            return queueName;

        }
    }
}
