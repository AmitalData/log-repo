 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class PaymentOrderConnectionTableRepository:IRepository<PaymentOrderConnectionTable>
   {
        
		public List<PaymentOrderConnectionTable> GetMulti(EntityKeyFields entityKeys)
        {
            PaymentOrderKeys keys = entityKeys as PaymentOrderKeys;
            return (from a in context.PaymentOrderConnectionTables
                    where a.PaymentOrderId == keys.Id
                    select a).ToList();
        }

        public List<PaymentOrderConnectionTable> GetPaymentOrderByConnectedEntity(string ConnectedEntityCode, string ConnectedEntityId, int tenant)
        {

            return (from a in context.PaymentOrderConnectionTables
                    where a.ConnectedEntityCode == ConnectedEntityCode && a.ConnectedEntityId == ConnectedEntityId && a.Tenant == tenant
                    select a).ToList();
        }


        public List<string> GetPaymentOrderConnectedDeclarations(string paymentOrderId, int tenant)
        {

          List<string> declarationIds =   (from a in context.PaymentOrderConnectionTables
                    where a.PaymentOrderId == paymentOrderId && a.ConnectedEntityCode == "D" && a.Tenant == tenant
                    select a.ConnectedEntityId).ToList();


          DeclarationRepository declarationRep = new DeclarationRepository(tenant);
          return declarationRep.GetCustomsFileNumbersByDeclaraionIds(declarationIds, tenant);

        }

        public PaymentOrderConnectionTable GetPaymentOrderConnectedToDeclaration(string paymentOrderId, int tenant)
        {
            PaymentOrderConnectionTable item = (from a in context.PaymentOrderConnectionTables
                                                where a.PaymentOrderId == paymentOrderId && a.ConnectedEntityCode == "D" && a.Tenant == tenant
                                                select a).FirstOrDefault();
            if (item != null && item.ConnectedEntityId != null)
            {
                return item;
            }

            else return null;
                
           
           
        }



   }

}
   