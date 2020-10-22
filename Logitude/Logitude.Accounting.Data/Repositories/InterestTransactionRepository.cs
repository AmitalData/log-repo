 
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
using Logitude.Accounting.Data.Utilities;
using Microsoft.SqlServer.Server;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class InterestTransactionRepository:IRepository<InterestTransaction>
   {
        
		public List<InterestTransaction> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public bool AlreadyExist(string InterestEntityTypeCode ,string EntityId, int tenant)
        {
            return this.GetAll(tenant)
                .Where(r => r.InterestEntityTypeCode == InterestEntityTypeCode)
                .Where(r => r.EntityId == EntityId)
                .Any();
        }

        public IQueryable<InterestTransaction> GetInterestTransactionsForGlAccountAndInterestValueDate(InterestTransactionGetParameters interestTransactionGetParameters)
        {
            return this.GetAll(interestTransactionGetParameters.Tenant)
                .Where(d =>d.Tenant== interestTransactionGetParameters.Tenant 
                && interestTransactionGetParameters.GLAccountIds.Contains(d.GLAccountId)
                && !d.IsClosed
                && !d.IsCancelled
                && d.InterestValueDate <= interestTransactionGetParameters.InterestCalculationDate
                && d.InterestValueDate >= interestTransactionGetParameters.InterestCalculationStartDate);

        }
 
    }

}
   