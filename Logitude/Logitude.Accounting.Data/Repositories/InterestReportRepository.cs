 
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
   public partial class InterestReportRepository:IRepository<InterestReport>
   {
        
		public List<InterestReport> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public InterestReport GetSingleByCusstomerAndStatudDraft(string CustomerId,int tenant)
        {
            InterestReport interestReport = (from a in context.InterestReports
                                      where a.Tenant == tenant && a.InterestReportStatusCode == "1"  &&a.CustomerId== CustomerId
                                      select a ).FirstOrDefault();
            return interestReport;
        }

        public decimal GetClosedBalanceOfLastInvoicedOrClosedWithoutInvoiceInterestReport(int tenant)
        {
            decimal? closedBalance = (from a in context.InterestReports
                                     where a.Tenant == tenant && a.InterestReportStatusCode != "1" && a.InterestReportStatusCode != "3"
                                     orderby a.InterestCalculationDate descending
                                     select a.CloseBalance).FirstOrDefault();
            return closedBalance != null ? closedBalance.Value : 0;
        }

   }

}
   