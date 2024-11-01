using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using Microsoft.Practices.Unity;
using System;

namespace AmitalCloud.Infrastructure.Data.Services
{
    public static class QueueServiceManager
    {
        public static IQueueService GetQueueService(string queuename,int tenant = 0)
        {
            IQueueService queueservice = ContainerAccessor.Container.Resolve(typeof(IQueueService), "QueueService", new ParameterOverride("", 1)) as IQueueService;
            queueservice.InitializeQueue(queuename, tenant);

            return queueservice;
        }

        public static string GetQueueByEnviroment(string queueName)
        {
            var savequeueName = queueName;
            if (AmitalCloudSettings.DeploymentStage == "Dev")
            {
                queueName = Environment.MachineName + "_" + queueName;
            }
            else if (AmitalCloudSettings.DeploymentStage == "customs")
            {
                queueName = "customs" + "_" + queueName;
            }
            else if (AmitalCloudSettings.DeploymentStage == "Simplog" || AmitalCloudSettings.DeploymentStage == "amitalstorage")
            {
                queueName = "Production" + "_" + queueName;
            }
            else
            {
                queueName = "Test" + "_" + queueName;
            }
            if (AmitalCloudSettings.IsCostomsDeploy)
            {
                var uri = new Uri(AmitalCloudSettings.LogitudeURL);
                var branch = uri.LocalPath.Trim(@"\"[0]).Trim(@"/"[0]);
                queueName = AmitalCloudSettings.StorageAccountName + "_Customs" +
                    //customsDeploymentStage.ToString() + 
                    branch + "_" + savequeueName;
            }
            /*
            var customsDeploymentStage = SettingUtil.GetCustomsDeploymentStage();
            switch (customsDeploymentStage)
            {
                case SettingUtil.CustomsDeploymentStage.Test:
                case SettingUtil.CustomsDeploymentStage.Pilot:

                    var uri = new Uri(AmitalCloudSettings.LogitudeURL);
                    var branch = uri.LocalPath.Trim(@"\"[0]).Trim(@"/"[0]);
                    queueName = AmitalCloudSettings.StorageAccountName + "_Customs" + customsDeploymentStage.ToString() + branch + "_" + savequeueName;
                    break;
                case SettingUtil.CustomsDeploymentStage.Production:
                                                         //queueName = "Customs" + customsDeploymentStage.ToString() + "_" + savequeueName;
                    queueName = AmitalCloudSettings.StorageAccountName + "_Customs" + customsDeploymentStage.ToString() + "_" + savequeueName;
                    break;

                default:
                    break;
            }
             */

            return queueName;

        }
    }
}
