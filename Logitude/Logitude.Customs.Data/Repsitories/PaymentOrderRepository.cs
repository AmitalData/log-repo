 
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
            var payment =
                            (
                            from rec in context.PaymentOrders
                            where rec.PaymentNumber == paymentNumber && rec.Tenant == tenant
                            select rec
                            )//
                .FirstOrDefault();
           
                   
            if(payment!= null)
            {
                return payment.Id;
            }

            return null;

       }


        public List<PaymentOrder> GetPaymentOrdersByDeclarationId(string declarationId, int tenant)
        {

            return (from a in context.PaymentOrders.Include("CustomerCard").Include("CustomerActivityType").Include("PaymentOrderType").Include("PaymentProcess").Include("PaymentStatus").Include("CustomsHouseType").Include("EntityTypeLookup").Include("Client")
                    where a.FirstEntityID == declarationId && a.Tenant == tenant
                    select a).ToList();
        }

        public string GetDecIdOfCustomFileByPaymentNumber(string PaymentNumber, int tenant)
        {

           var CustomFiles =  (from a in context.PaymentOrders
                    where a.PaymentNumber == PaymentNumber && a.Tenant == tenant
                    select a).FirstOrDefault()?.CustomFiles;

            if(CustomFiles != null)
            {
                CustomFiles = CustomFiles.Replace("*", "");
                return (from a in context.Declarations
                    where a.CustomFileNo == CustomFiles && a.Tenant == tenant && a.AmendmentDontDisplayInList == false
                    select a).FirstOrDefault()?.Id;
            }
           return null;
        }

    }

}
   