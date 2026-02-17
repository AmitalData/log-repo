 
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
   public partial class DeficitRepository:IRepository<Deficit>
   {
        
		public List<Deficit> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public string GetIdByDebtNotificationNumber(string debtNotificationNumber, int tenant)
        {
            return
                (
                from rec in context.Deficits
                where rec.DebtNotificationNumber == debtNotificationNumber && rec.Tenant == tenant
                select rec.Id
                )
                .FirstOrDefault();


        }


        public Deficit GetDeficitByPaymentNumberOrTapagId(string paymentNumber, string tapagId, int tenant)
        {
            Deficit deficit = null;
            if (paymentNumber != null)
            {

                deficit = (from a in context.Deficits.Include("DebtNotificationType")
                        where a.PaymentOrderNumber == paymentNumber && a.Tenant == tenant
                        select a).FirstOrDefault();
            }
           else if (tapagId != null)
            {
                deficit = (from a in context.Deficits.Include("DebtNotificationType")
                        where a.TapagId == tapagId && a.Tenant == tenant
                        select a).FirstOrDefault();
            }


            return deficit;

        }

   }

}
   