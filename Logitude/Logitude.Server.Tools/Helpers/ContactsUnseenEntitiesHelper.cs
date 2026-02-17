using Microsoft.ServiceBus.Messaging;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

namespace Logitude.Server.Tools.Helpers
{
    public class ContactsUnseenEntitiesHelper
    {
        public static void AddUnseenEntityRecord(string traceEventId, int tenant)
        {
          Tenant  tenantpm =   TenantRepository.GetSingleTenant(tenant, true);

          if (tenantpm.IsMobileActivated)
         {
             using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())
             {

                 QueueClient client = ServiceBusQueueHelper.CreateContactUnseenEntityQueue(tenant);
                 BrokeredMessage message = new BrokeredMessage();
              
                 message.Properties["Tenant"] = tenant;
                 message.Properties["TenantName"] = tenantpm.Company;
                 message.Properties["TraceEventId"] = traceEventId;
                 message.Properties["SourceEventDate"] = DateTime.UtcNow;
                 client.Send(message);
          

                 scope.Complete();
             }
         }
            //}
        }
    }
}