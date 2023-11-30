 
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
   public partial class InterestReportRepository:IRepository<InterestReport>
   {
        
		public List<InterestReport> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public InterestReport GetSingleByCusstomerAndStatudDraft(string CustomerId,int tenant)
        {
            InterestReport interestReport = (from a in context.InterestReports
                                      where a.Tenant == tenant && (a.InterestReportStatusCode == "1" || a.InterestReportStatusCode == "5") && a.CustomerId== CustomerId
                                      select a ).FirstOrDefault();
            return interestReport;
        }

        public InterestReport GetSingleByARInvoiceId(string invoiceId, int tenant)
        {
            InterestReport interestReport = (from a in context.InterestReports
                                             where a.Tenant == tenant && a.ARinvoiceId==invoiceId
                                             select a).FirstOrDefault();
            return interestReport;
        }

        public InterestReport GetSingleByCusstomerAndStatudNotCancelledOrFailed(string ReportNumber, string CustomerId, int tenant)
        {
            InterestReport interestReport = (from a in context.InterestReports
                                             where a.Tenant == tenant &&  a.InterestReportStatusCode != "3" && a.InterestReportStatusCode != "6"  && a.CustomerId == CustomerId && a.ReportNumber != ReportNumber
                                             select a).FirstOrDefault();
            return interestReport;
        }
        public InterestReport GetSingleByGraterInterestCalculationDate(string CustomerId, string SelectedReportId ,DateTime InterestCalculationDate, int tenant)
        {
            InterestReport interestReport = (from a in context.InterestReports
                                             where   a.Tenant == tenant 
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

        public decimal GetClosedBalanceOfLastInvoicedOrClosedWithoutInvoiceInterestReport(int tenant,string glaccountId)
        {
            decimal? closedBalance = (from a in context.InterestReports
                                     where a.Tenant == tenant 
                                     && (a.InterestReportStatusCode == InterestReportStatusCodes.Invoiced
                                     || a.InterestReportStatusCode == InterestReportStatusCodes.ClosedWithoutInvoice) 
                                     && a.GLAccountId==glaccountId
                                     orderby a.InterestCalculationDate descending
                                     select a.CloseBalance).FirstOrDefault();
            return closedBalance != null ? closedBalance.Value : 0;
        }

       public CloseBalanceInterestReportData GetCloseBalanceCalculationDateAndStatusOfTheLastInterestReport(int tenant,string glaccountId)
        {
            CloseBalanceInterestReportData result = (from a in context.InterestReports
                          where a.Tenant == tenant && a.GLAccountId == glaccountId
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
   