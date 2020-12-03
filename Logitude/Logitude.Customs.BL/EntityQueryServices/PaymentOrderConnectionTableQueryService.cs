using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
   public partial class PaymentOrderConnectionTableQueryService
    {
       public List<PaymentOrderConnectionTableList> GetPaymentOrderConnectionTable(string ConnectedEntityCode, string ConnectedEntityId, int tenant)
       {

            DeclarationQueryService declarationQueryService = new DeclarationQueryService(tenant);

            var declarationPMs = declarationQueryService.GetDeclarationAmendmentsById(tenant, ConnectedEntityId);

            List<string> ConnectedEntityIds = new List<string>();
            declarationPMs.ForEach(x => ConnectedEntityIds.Add(x.Id));
            ConnectedEntityIds.Add(ConnectedEntityId);

            System.Collections.Generic.List<PaymentOrderConnectionTable> connection = repository.GetPaymentOrderByConnectedEntity(ConnectedEntityCode,ConnectedEntityIds, tenant);

           List<PaymentOrderConnectionTableList> connections = new List<PaymentOrderConnectionTableList>();
           foreach (PaymentOrderConnectionTable item in connection)
           {
               PaymentOrderConnectionTableList connectionList = new PaymentOrderConnectionTableList()
               {
                   ConnectedEntityCode = item.ConnectedEntityCode,
                   ConnectedEntityId = item.ConnectedEntityId,
                   PaymentOrderId = item.PaymentOrderId,
                   Tenant = item.Tenant,
               };
               connections.Add(connectionList);
           }

           return connections;
       }
    }
}
