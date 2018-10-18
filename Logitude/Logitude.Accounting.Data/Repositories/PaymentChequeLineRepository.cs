 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class PaymentChequeLineRepository:IRepository<PaymentChequeLine>
   {
        
		public List<PaymentChequeLine> GetMulti(EntityKeyFields entityKeys)
        {

            PaymentChequeKeys paymentChequKeys = entityKeys as PaymentChequeKeys;

            return (from a in context.PaymentChequeLines
                    where a.PaymentChequeId == paymentChequKeys.Id
                    select a).ToList();
        }

        public int GetMaxLineNumber(string paymentChequeId, int tenant)
        {
            int maxLine = (from a in context.PaymentChequeLines
                           where a.PaymentChequeId == paymentChequeId && a.Tenant == tenant
                           select a).Max(d => (int?)d.Line) ?? 0;
            return maxLine;
        }
    }

}
   