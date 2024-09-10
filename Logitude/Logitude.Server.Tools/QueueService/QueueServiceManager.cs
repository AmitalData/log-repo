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
            return Simplog.Server.Infrastructure.WebFreightEntryPoint.GetQueueByEnviroment(queueName);
        }
    }
}
