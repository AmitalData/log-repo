
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
using Logitude.Accounting.Data.Enums;
using Logitude.Accounting.Data.DataContract;

namespace Logitude.Accounting.Data.Repositories
{
    public partial class InterestReportRepository : IRepository<InterestReport>
    {

        public List<InterestReport> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public InterestReport GetSingleByCusstomerAndStatudDraft(string CustomerId, int tenant)
        {
            InterestReport interestReport = (from a in context.InterestReports
                                             where a.Tenant == tenant && (a.InterestReportStatusCode == "1" || a.InterestReportStatusCode == "5") && a.CustomerId == CustomerId
                                             select a).FirstOrDefault();
            return interestReport;
        }

        public InterestReport GetSingleByARInvoiceId(string invoiceId, int tenant)
        {
            InterestReport interestReport = (from a in context.InterestReports
                                             where a.Tenant == tenant && a.ARinvoiceId == invoiceId
                                             select a).FirstOrDefault();
            return interestReport;
        }

        public InterestReport GetSingleByCusstomerAndStatudNotCancelledOrFailed(string ReportNumber, string CustomerId, int tenant)
        {
            InterestReport interestReport = (from a in context.InterestReports
                                             where a.Tenant == tenant && a.InterestReportStatusCode != "3" && a.InterestReportStatusCode != "6" && a.CustomerId == CustomerId && a.ReportNumber != ReportNumber
                                             select a).FirstOrDefault();
            return interestReport;
        }
        public InterestReport GetSingleByGraterInterestCalculationDate(string CustomerId, string SelectedReportId, DateTime InterestCalculationDate, int tenant)
        {
            InterestReport interestReport = (from a in context.InterestReports
                                             where a.Tenant == tenant
                                                  && a.CustomerId == CustomerId
                                                  && (
                                                          (
                                                             a.InterestCalculationDate > InterestCalculationDate
                                                             && (a.InterestReportStatusCode == "2" || a.InterestReportStatusCode == "4" || a.InterestReportStatusCode == "1")
                                                          )
                                                          ||
                                                          (
                                                             a.InterestCalculationDate == InterestCalculationDate
                                                             && (a.InterestReportStatusCode == "1")
                                                          )
                                                     )
                                                   && a.Id != SelectedReportId

                                             select a).FirstOrDefault();
            return interestReport;
        }

        public decimal GetClosedBalanceOfLastInvoicedOrClosedWithoutInvoiceInterestReport(int tenant, string glaccountId)
        {
            var acc = (from ga in context.GLAccounts
                       where ga.Tenant == tenant
                       where ga.Id == glaccountId
                       select ga
                        );

            decimal? closedBalance = (from a in context.InterestReports
                                      join ga in acc on a.GLAccountId equals ga.Id
                                      where a.Tenant == tenant
                                     && (a.InterestReportStatusCode == InterestReportStatusCodes.Invoiced
                                     || a.InterestReportStatusCode == InterestReportStatusCodes.ClosedWithoutInvoice)
                                     && a.GLAccountId == glaccountId
                                     && ((a.InterestCalculationDate >= ga.InterestCalculationStartDate) || ga.InterestCalculationStartDate == null)
                                      orderby a.InterestCalculationDate descending
                                      select a.CloseBalance).FirstOrDefault();
            return closedBalance != null ? closedBalance.Value : 0;
        }


        public decimal? GetInterestReportOpenBalance(DateTime inputDate, string glAccountId, int tenant)
        {
            var firstOfMonth = new DateTime(inputDate.Year, inputDate.Month, 1);


            var openBalance = GetLedgerOpenBalance(glAccountId, tenant, firstOfMonth);
            var interestDeltas = GetInterestAdjustments(glAccountId, tenant, firstOfMonth);

            return openBalance - interestDeltas.AfterValueDate - interestDeltas.BeforeValueDateWithoutReport;
        }

        private decimal? GetLedgerOpenBalance(string glAccountId, int tenant, DateTime beforeDate)
        {
            var relevantGLAccountIds =new List<string>();
          
             relevantGLAccountIds = (from c in context.GLAccountCurrencies
                                        join gl in context.GLAccounts on c.GLAccountId equals gl.Id
                                        where c.Tenant == tenant
                                              && c.MainGLAccountId == glAccountId
                                              && gl.ActiveForInterest
                                        select c.GLAccountId).ToList();

            relevantGLAccountIds.Add(glAccountId);

            return context.LedgerTransactions
                        .Where(x => relevantGLAccountIds.Contains(x.AccountId) &&
                                                                   x.AccountingDate < beforeDate && x.Tenant == tenant)
                         .Sum(x => (decimal?)(x.LocalAmountDebit - x.LocalAmountCredit)) ?? decimal.Zero;
        }

        private (decimal? AfterValueDate, decimal? BeforeValueDateWithoutReport) GetInterestAdjustments(string glAccountId, int tenant, DateTime beforeDate)
        {
            var relevantGLAccountIds = new List<string>();
            relevantGLAccountIds = (from c in context.GLAccountCurrencies
                                    join gl in context.GLAccounts on c.GLAccountId equals gl.Id
                                    where c.Tenant == tenant
                                          && c.MainGLAccountId == glAccountId
                                          && gl.ActiveForInterest
                                    select c.GLAccountId).ToList();

            relevantGLAccountIds.Add(glAccountId);
            bool hasInvalidRecords = context.InterestTransactions
               .Any(it => it.GLAccountId == glAccountId &&
               it.Tenant == tenant &&
               it.AccountingDate == null &&
               it.InterestEntityTypeCode != InterestEntities.OpenBalance
               && it.InterestReportId  == null);
              
            if (hasInvalidRecords)
            {
                throw new Exception("Interest report cannot run - there are records with InterestEntityTypeCode different from 4 but without AccountingDate");
            }
            var query = context.InterestTransactions
                .Join(context.GLAccounts,
                    it => new { it.GLAccountId, it.Tenant },
                    ga => new { GLAccountId = ga.Id, ga.Tenant },
                    (it, ga) => new { it, ga })
                .Where(x => relevantGLAccountIds.Contains(x.it.GLAccountId) &&

                            x.it.AccountingDate < beforeDate &&
                            (x.ga == null || x.it.AccountingDate >= x.ga.InterestCalculationStartDate) &&
                            x.it.Tenant == tenant && x.it.InterestEntityTypeCode != InterestEntities.OpenBalance)
                .Select(x => new
                {
                    InterestAfter = (x.it.InterestValueDate >= beforeDate) ? x.it.LocalAmount : 0,
                    InterestBeforeUnreported = (x.it.InterestValueDate < beforeDate && x.it.InterestReportId == null) ? x.it.LocalAmount : 0
                });
            var AfterValueDate = query.Sum(x => (decimal?)x.InterestAfter) ?? 0;
            var BeforeValueDateWithoutReport = query.Sum(x => (decimal?)x.InterestBeforeUnreported) ?? 0;

            return ((AfterValueDate != null ? AfterValueDate : 0), (BeforeValueDateWithoutReport != null ? BeforeValueDateWithoutReport : 0));

        }



        public CloseBalanceInterestReportData GetCloseBalanceCalculationDateAndStatusOfTheLastInterestReport(int tenant, string glaccountId)
        {
            var acc = (from ga in context.GLAccounts
                       where ga.Tenant == tenant
                       where ga.Id == glaccountId
                       select ga
            );

            CloseBalanceInterestReportData result = (from a in context.InterestReports
                                                     join ga in acc on a.GLAccountId equals ga.Id
                                                     where a.Tenant == tenant && a.GLAccountId == glaccountId
                           && ((a.InterestCalculationDate >= ga.InterestCalculationStartDate) || ga.InterestCalculationStartDate == null)
                           && (a.InterestReportStatusCode == InterestReportStatusCodes.Invoiced
                           || a.InterestReportStatusCode == InterestReportStatusCodes.ClosedWithoutInvoice)
                                                     orderby a.InterestCalculationDate descending
                                                     select new CloseBalanceInterestReportData()
                                                     {
                                                         Id = a.Id,
                                                         CloseBalance = a.CloseBalance,
                                                         InterestCalculationDate = a.InterestCalculationDate,
                                                         InterestReportStatusCode = a.InterestReportStatusCode,
                                                     }).FirstOrDefault();

            return result;
        }

        public InterestReport GetDraftInterestReportForCustomer(string customerId, string glAccount, int tenant)
        {
            InterestReport interestReport = (from a in context.InterestReports
                                             where a.Tenant == tenant && a.InterestReportStatusCode == "1"
                                             && a.CustomerId == customerId
                                             && a.GLAccountId == glAccount
                                             select a).FirstOrDefault();
            return interestReport;
        }
        public List<InterestReport> GetInterestReportsForCustomer(string customerId, string glAccount, int tenant)
        {
            var interestReports = (from a in context.InterestReports
                                   where a.Tenant == tenant
                                   && a.CustomerId == customerId
                                   && a.GLAccountId == glAccount
                                   select a).ToList();
            return interestReports;
        }

        public InterestReport GetPreviousInvoicedOrCloseWithoutInvoicedtInterestReportForCustomer(string customerId, int tenant, DateTime CalculationDate)
        {
            InterestReport interestReport = (from a in context.InterestReports
                                             where a.Tenant == tenant
                                             && (a.InterestReportStatusCode == "2" || a.InterestReportStatusCode == "4")
                                             && a.CustomerId == customerId
                                             && a.InterestCalculationDate >= CalculationDate
                                             select a).FirstOrDefault();
            return interestReport;
        }

        public string GetInterestReportStatusCode(string InterestReportId, int tenant)
        {
            string InterestReportStatus = (from a in context.InterestReports
                                           where a.Tenant == tenant && a.Id == InterestReportId
                                           select a.InterestReportStatusCode).FirstOrDefault();
            return InterestReportStatus;
        }
    }

}
