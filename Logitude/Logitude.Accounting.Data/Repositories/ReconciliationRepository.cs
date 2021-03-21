 
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
using System.Data.Entity;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class ReconciliationRepository:IRepository<Reconciliation>
   {
        
		public List<Reconciliation> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        
        public IQueryable<Reconciliation> GetClosedReconciliationsByAccount(string gLAccountId, int tenant)
        {
            if (!String.IsNullOrEmpty(gLAccountId))
            {
                return (from record in context.Reconciliations //.Include("PartnerType").Include("PaymentTerm")... 
                        where record.Tenant == tenant && record.AccountId == gLAccountId && !String.IsNullOrEmpty(record.Number)
                        select record);
            }
            else
            {
                return null;
            }
        }

        public Reconciliation GetLastOpenReconciliation(int tenant, string accountId, string cancelledReconciliationId)
        {
            var q= (from record in context.Reconciliations 
                    where record.Tenant == tenant
                    where record.AccountId == accountId
                    where !record.IsCancelled
                    where record.Id != cancelledReconciliationId
                    orderby record.CreateDate descending
                    select record);
            

            var poco= q.FirstOrDefault();
            return poco;
        }
    }

}
   