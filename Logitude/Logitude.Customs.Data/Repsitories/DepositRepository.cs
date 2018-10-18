 
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
   public partial class DepositRepository:IRepository<Deposit>
   {
        
		public List<Deposit> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public Deposit GetDepositByPaymentNumberOrTapagId(string paymentNumber, string tapagId, int tenant)
        {
            Deposit deposit = null;
            if (paymentNumber != null)
            {

                deposit = (from a in context.Deposits.Include("EntityTypeLookup").Include("DepositFileType")
                           where a.PaymentNumber == paymentNumber && a.Tenant == tenant
                           select a).FirstOrDefault();
            }
            else if (tapagId != null)
            {
                deposit = (from a in context.Deposits.Include("EntityTypeLookup").Include("DepositFileType")
                           where a.TapagID == tapagId && a.Tenant == tenant
                           select a).FirstOrDefault();
            }


            return deposit;

        }

        public string GetDepositIdByTapagNumber(string tapagId, int tenant)
        {
            return
                (
                from rec in context.Deposits
                where rec.TapagID == tapagId && rec.Tenant == tenant
                select rec.Id
                )
                .FirstOrDefault();
        }


        public object GetDepositIdByPaymentNumber(string paymentNumber, int tenant)
        {
            return
                (
                from rec in context.Deposits
                where rec.PaymentNumber == paymentNumber && rec.Tenant == tenant
                select rec.Id
                )
                .FirstOrDefault();
        }
   }

}
   