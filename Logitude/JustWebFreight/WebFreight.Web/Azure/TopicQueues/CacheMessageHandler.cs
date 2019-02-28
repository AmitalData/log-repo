using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.TopicQueues
{
    public class CacheMessageHandler
    {
       
        SubscriptionClient subscriptionClient;
        public CacheMessageHandler()
        {
            //string[] roleId = RoleEnvironment.CurrentRoleInstance.Id.Split('_');
            string subscribtionName = Environment.MachineName;//roleId[roleId.Length - 1];
            subscriptionClient = Microsoft.ServiceBus.Messaging.SubscriptionClient.CreateFromConnectionString(StorageAcountDetails.GetSettingByName(LogitudeSettings.DeploymentStage), StorageAcountDetails.DataCacheTopicName, subscribtionName);
        }

        public void HandleTopicMessages()
        {
            while (true)
            {
                try
                {
                    var message = subscriptionClient.Receive();
                    if (message != null)
                    {
                        string Key = message.Properties["Key"].ToString();

                        if (Key.Contains("$"))
                        {
                            string[] iKeys = Key.Split('$');

                            foreach(string iKey in iKeys)
                            {
                                CacheManager.CacheWrapper.Remove(iKey);
                            }
                        }

                        else
                        {
                            CacheManager.CacheWrapper.Remove(Key);
                        }

                        message.Complete();
                    }
                }

                catch { }
            }
        }

       
    }
}
