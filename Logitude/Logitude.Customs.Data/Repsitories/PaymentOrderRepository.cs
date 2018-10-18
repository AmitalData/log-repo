 
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
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class PaymentOrderRepository:IRepository<PaymentOrder>
   {
        
		public List<PaymentOrder> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public string GetIdByPaymentNumber(string paymentNumber, int tenant)
       {
            return 
                (
                from rec in   context.PaymentOrders
                where rec.PaymentNumber == paymentNumber && rec.Tenant == tenant  
                select rec.Id 
                )
                .FirstOrDefault();
           
                   
       }


        public List<PaymentOrder> GetPaymentOrdersByDeclarationId(string declarationId, int tenant)
        {

            return (from a in context.PaymentOrders.Include("CustomerCard").Include("CustomerActivityType").Include("PaymentOrderType").Include("PaymentProcess").Include("PaymentStatus").Include("CustomsHouseType").Include("EntityTypeLookup").Include("Client")
                    where a.FirstEntityID == declarationId && a.Tenant == tenant
                    select a).ToList();
        }

   }

}
   