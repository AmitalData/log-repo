using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class InterestReportLineQueryService
    {
        public List<InterestReportLinePM> GetInterestReportLinesForInterestReport(string interestReportId,int tenant)
        {
            InterestReportLineRepository interestReportLineRepository = new InterestReportLineRepository(tenant);
            IQueryable<InterestReportLine> interestReports = interestReportLineRepository.GetInterestReportLinesForInterestReport(interestReportId, tenant);
            List<InterestReportLinePM> interestReportLinePMs = (from a in interestReports
                                                                select new InterestReportLinePM()
                                                                {
                                                                    InterestReportId = a.InterestReportId,
                                                                    InterestTransactionId = a.InterestTransactionId,
                                                                    Tenant = a.Tenant,
                                                                }).ToList();
            return interestReportLinePMs;
        }

        public List<InterestTransactionPM> GetInterestTransactionForReport(string interestReportId, int tenant)
        {
            //InterestReportLineRepository interestReportLineRepository = new InterestReportLineRepository(tenant);

            List<InterestTransactionPM> transactions = (from reportLine in context.InterestReportLines
                                join intrestTransaction in context.InterestTransactions on reportLine.InterestTransactionId equals intrestTransaction.Id
                                where reportLine.InterestReportId == interestReportId && reportLine.Tenant == tenant
                                select new InterestTransactionPM()
                                {
                                    Id = intrestTransaction.Id,
                                    Tenant = intrestTransaction.Tenant,
                                    GLAccountId = intrestTransaction.GLAccountId,
                                    InterestEntityTypeCode = intrestTransaction.InterestEntityTypeCode,
                                    


                                }).ToList();



            return transactions;
        }
    }
}
