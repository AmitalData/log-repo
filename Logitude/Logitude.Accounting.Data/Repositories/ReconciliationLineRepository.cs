 
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

using System.Linq.Expressions;
namespace Logitude.Accounting.Data.Repositories
{
   public partial class ReconciliationLineRepository:IRepository<ReconciliationLine>
   {
        
		public List<ReconciliationLine> GetMulti(EntityKeyFields entityKeys)
        {
            ReconciliationKeys reconciliationKeys = entityKeys as ReconciliationKeys;

            return (from a in context.ReconciliationLines
                    where a.ReconciliationId == reconciliationKeys.Id
                    select a).ToList();
        }

        public IQueryable<ReconciliationLine> GetReconciliationLines(int tenant)
        {
            return (from d in context.ReconciliationLines //.Include("PartnerType").Include("PaymentTerm")...
                    where d.Tenant == tenant
                    select d);
        }

        public bool IsReconciledBy(int tenant, List<string> transactionIdList)
        {
            var q=(from d in context.ReconciliationLines //.Include("PartnerType").Include("PaymentTerm")...
                    where d.Tenant == tenant
                    where transactionIdList.Contains(d.TransactionId)
                    select d);
            return q.Any();
        }
   }
  
}
   