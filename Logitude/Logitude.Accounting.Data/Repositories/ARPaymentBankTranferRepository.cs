 
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
   public partial class ARPaymentBankTranferRepository:IRepository<ARPaymentBankTranfer>
   {
        
		public List<ARPaymentBankTranfer> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public ARPaymentBankTranfer GetSingleARPaymentBankTranferByPaymentId(string paymentId, int tenant)
        {
            return (from a in context.ARPaymentBankTranfers
                    where a.PaymentId == paymentId && a.Tenant == tenant

                    select a).FirstOrDefault();
        }

        public IQueryable<ARPaymentBankTranfer> GetARPaymentBankTranfersByPaymentId(string paymentId, int tenant)
        {
            return (from a in context.ARPaymentBankTranfers
                    where a.PaymentId == paymentId && a.Tenant == tenant
                    select a);
        }

    }

}
   