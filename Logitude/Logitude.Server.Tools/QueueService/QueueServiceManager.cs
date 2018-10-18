using Microsoft.Practices.Unity;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.QueueService
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
