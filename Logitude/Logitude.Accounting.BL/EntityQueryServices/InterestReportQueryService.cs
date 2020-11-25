using Logitude.Accounting.BL.InterestService.HelperClasses;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class InterestReportQueryService
    {
        public decimal GetClosedBalanceOfLastInvoicedOrClosedWithoutInvoiceInterestReport(int tenant,string glaccountId)
        {
            decimal closedBalance = 0;
            InterestReportRepository interestReportRepository = new InterestReportRepository(tenant);
            closedBalance = interestReportRepository.GetClosedBalanceOfLastInvoicedOrClosedWithoutInvoiceInterestReport(tenant, glaccountId);
            return closedBalance;
        }
        public override void GetComposition(EntityKeyFields entityKeys, InterestReportPM entityPM)
        {
            IAccountingContext context = MainContext as IAccountingContext;
            InterestReportKeys activityKeys = entityKeys as InterestReportKeys;
            InterestReportLinesByDateQueryService queryService = new InterestReportLinesByDateQueryService(context);
            entityPM.InterestReportLinesByDates = queryService.GetMulti(activityKeys, true);
        }

        public bool CheckRecentCustomerReports(int tenant, DateTime interestDate, string customerId)
        {
            return (from a in context.InterestReports
                    where a.Tenant == tenant && a.InterestCalculationDate > interestDate && a.InterestReportStatusCode != "3" && a.CustomerId == customerId
                    select a).Any();
        }

        private static List<string> GetRecentStatuesAreNotAllowed()
        {
            List<string> RecentStatuesAreNotAllowed = new List<string>();
            RecentStatuesAreNotAllowed.Add("3");
            RecentStatuesAreNotAllowed.Add("6");
            RecentStatuesAreNotAllowed.Add("9");
            RecentStatuesAreNotAllowed.Add(null);

            return RecentStatuesAreNotAllowed;
        }


        public bool CheckRecentCustomerReports(DateTime interestDate, InterestReportPM InterestReportPM)
        {
            List<string> RecentStatuesAreNotAllowed = GetRecentStatuesAreNotAllowed();
            return (from a in context.InterestReports
                    where a.Tenant == InterestReportPM.Tenant && a.InterestCalculationDate > interestDate && !RecentStatuesAreNotAllowed.Contains(a.InterestReportStatusCode)  && a.CustomerId == InterestReportPM.CustomerId && a.Id != InterestReportPM.Id
                    select a).Any();
        }

        public List<string> GetInterestReprtsWithInvocies(int tenant, List<string> ARInvoiceIds)
        {
            return (from a in context.InterestReports
                    where a.Tenant == tenant && ARInvoiceIds.Contains(a.ARinvoiceId)
                    select a.Id+","+a.ARinvoiceId).ToList();
        }

        public decimal? GetCreditLimitFromGLAccount(String GLAccountId,int tenant)
        {
            return (from a in context.GLAccounts
                    where a.Tenant == tenant && a.Id == GLAccountId
                    select a.InterestCreditLimit).FirstOrDefault();
        }
        public List<string> GetInterestReportNumbersByIds(List<string> InterestReportIds, int tenant)
        {
            var result = (from a in  context.InterestReports where a.Tenant == tenant   && InterestReportIds.Contains(a.Id) select a.ReportNumber).ToList();

            return result;
        }
        public List<InterestReportPM> GetNotInvoicedInterestReportsByDates(DateTime fromDate, DateTime toDate, int tenant, List<string> ExcludedIds)
        {
            
            return (from a in context.InterestReports
                    where
                     a.Tenant == tenant&& !ExcludedIds.Contains(a.Id) && a.InterestCalculationDate >= fromDate && a.InterestCalculationDate <= toDate && (a.InterestReportStatusCode == "1" || a.InterestReportStatusCode == "8" || a.InterestReportStatusCode == "9")
                    select new InterestReportPM()
                    {

                        Id = a.Id,

                        Tenant = a.Tenant,
                        CustomerId = a.CustomerId,
                        CreateDateTime = a.CreateDateTime,

                        CreatedByUserId = a.CreatedByUserId,

                        UpdateDateTime = a.UpdateDateTime,

                        UpdatedByUserId = a.UpdatedByUserId,

                        GLAccountId = a.GLAccountId,

                        ReportNumber = a.ReportNumber,

                        InterestCalculationDate = a.InterestCalculationDate,

                        TotalAmount = a.TotalAmount,

                        OpenBalance = a.OpenBalance,

                        CloseBalance = a.CloseBalance,

                        ARinvoiceId = a.ARinvoiceId,

                        InvoiceAmount = a.InvoiceAmount,

                        GLAccountInterestCreditLimit = a.GLAccountInterestCreditLimit,

                        InterestReportStatusCode = a.InterestReportStatusCode,
                        CreatedByLocalName = a.CreatedByUser != null ? a.CreatedByUser.Contact.LocalName : null,
                        UpdatedByLocalName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact.LocalName : null,
                        InterestReportStatusName = a.InterestReportStatuse == null ? null : a.InterestReportStatuse.EnglishName,
                        InterestReportStatusLocalName = a.InterestReportStatuse == null ? null : a.InterestReportStatuse.LocalName,
                        GLAccountDisplayNumber = a.GLAccount == null ? null : a.GLAccount.DisplayNumber,
                        ARInvoiceNumber = a.ARInvoice == null ? null : a.ARInvoice.InvoiceNumber,
                        GLAccountLocalName = a.GLAccount == null ? null : a.GLAccount.LocalName

                    }).ToList();


        }


        public IQueryable<InterestReportLinesByDatePM> GetFirstAndLastInterestReportLineByDatesForInterestReports(List<string>  ReportIds)
        {

            IQueryable<InterestReportLinesByDatePM> LinesByDates = (from a in context.InterestReportLinesByDates
                                                                    join itself in context.InterestReportLinesByDates
                                                                    on a.InterestReportId equals itself.InterestReportId
                                                                    where (ReportIds.Contains(a.InterestReportId))
                                                                    select new { a = a, itself = itself }).
                                                                    OrderBy(s => s.a.FromDate).ThenByDescending(s => s.itself.ToDate).
                                                                    GroupBy(x => x.a.InterestReportId).
                                                                    Select(x => new InterestReportLinesByDatePM()
                                                                    {
                                                                      InterestReportId = x.FirstOrDefault().a.InterestReportId,
                                                                      FromDate = x.OrderBy(s=>s.a.FromDate).FirstOrDefault().a.FromDate,
                                                                      ToDate = x.OrderByDescending(s=>s.itself.ToDate).FirstOrDefault().itself.ToDate,
                                                                    });
          

            return LinesByDates;

        }

        public IQueryable<InterestReportPM> GetInterestReportsBySelectedIds(InterestReportArguments interestReportArgs)
        {
            return (from a in context.InterestReports
                    where
                     a.Tenant == interestReportArgs.Tenant && (a.InterestReportStatusCode =="1" || a.InterestReportStatusCode == "9") && (interestReportArgs.FromDate != null ? a.InterestCalculationDate>= interestReportArgs.FromDate: a.InterestCalculationDate!=null) && (interestReportArgs.ToDate != null ? a.InterestCalculationDate <= interestReportArgs.ToDate : a.InterestCalculationDate != null) && (!interestReportArgs.AllSelected ? interestReportArgs.SelectedIds.Contains(a.Id) : !interestReportArgs.ExcludedIds.Contains(a.Id) )&& (a.TotalAmount ==null || (a.TotalAmount == null && a.GLAccount.MinimumInterestInvoiceBilling==null) || a.TotalAmount <= a.GLAccount.MinimumInterestInvoiceBilling)

                    select new InterestReportPM()
                    {

                        Id = a.Id,

                    });

        }

        public  InterestReportPM  GetSinglePMForInterest(string Id, int Tenant)
        {
            return (from a in context.InterestReports
                    where
                     a.Tenant == Tenant &&  a.Id == Id

                    select new InterestReportPM()
                    {

                        Id = a.Id,
                        ReportNumber=a.ReportNumber,

                    }).FirstOrDefault();

        }

        public List<InterestReportPM> GetInterestReportsByIds(List<string> ids, int tenant)
        {
            return (from a in context.InterestReports
                    where
                     a.Tenant == tenant && ids.Contains(a.Id)
                    select new InterestReportPM()
                    {

                        Id = a.Id,

                        Tenant = a.Tenant,

                        CustomerId = a.CustomerId ,

                        CreateDateTime = a.CreateDateTime,

                        CreatedByUserId = a.CreatedByUserId,

                        UpdateDateTime = a.UpdateDateTime,

                        UpdatedByUserId = a.UpdatedByUserId,

                        GLAccountId = a.GLAccountId,

                        ReportNumber = a.ReportNumber,

                        InterestCalculationDate = a.InterestCalculationDate,

                        TotalAmount = a.TotalAmount,

                        OpenBalance = a.OpenBalance,

                        CloseBalance = a.CloseBalance,

                        ARinvoiceId = a.ARinvoiceId,

                        SearchFields = a.SearchFields,

                        InvoiceAmount = a.InvoiceAmount,

                        GLAccountInterestCreditLimit = a.GLAccountInterestCreditLimit,

                        InterestReportStatusCode = a.InterestReportStatusCode,

                        CreatedByLocalName = a.CreatedByUser != null ? a.CreatedByUser.Contact.LocalName : null,

                        UpdatedByLocalName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact.LocalName : null,

                        InterestReportStatusName = a.InterestReportStatuse == null ? null : a.InterestReportStatuse.EnglishName,

                        InterestReportStatusLocalName = a.InterestReportStatuse == null ? null : a.InterestReportStatuse.LocalName,

                        GLAccountDisplayNumber = a.GLAccount == null ? null : a.GLAccount.DisplayNumber,

                        ARInvoiceNumber = a.ARInvoice == null ? null : a.ARInvoice.InvoiceNumber,

                        GLAccountLocalName = a.GLAccount == null ? null : a.GLAccount.LocalName,

                       GLAccountMinimumInterest = a.GLAccount.MinimumInterestInvoiceBilling,

                    }).ToList();
        }
        public InterestReportPM GetDraftInterestReportForCustomer(string customerId, string glaccountId, int tenant)
        {
            InterestReportRepository interestReportRepository = new InterestReportRepository(tenant);
            InterestReport interestReport = interestReportRepository.GetDraftInterestReportForCustomer(customerId, glaccountId, tenant);
            InterestReportPM interestReportPM = this.GetEntityPM(interestReport);
            
            return interestReportPM;
        }

        public InterestReportPM GetPreviousInvoicedOrCloseWithoutInvoicedtInterestReportForCustomer(string customerId, int tenant, DateTime CalculationDate)
        {
            InterestReportRepository interestReportRepository = new InterestReportRepository(tenant);
            InterestReport interestReport = interestReportRepository.GetPreviousInvoicedOrCloseWithoutInvoicedtInterestReportForCustomer(customerId, tenant, CalculationDate);
            InterestReportPM interestReportPM = this.GetEntityPM(interestReport);

            return interestReportPM;
        }

        public string GetInterestReportStatusCode(string InterestReportId,   int tenant)
        {
            InterestReportRepository interestReportRepository = new InterestReportRepository(tenant);
            string InterestReportStatusCode = interestReportRepository.GetInterestReportStatusCode(InterestReportId, tenant);
            return InterestReportStatusCode;

        }

        public dynamic GetCloseBalanceCalculationDateAndStatusOfTheLastInterestReport(int tenant,string glaccountId)
        {
            InterestReportRepository interestReportRepository = new InterestReportRepository(tenant);
            return interestReportRepository.GetCloseBalanceCalculationDateAndStatusOfTheLastInterestReport(tenant, glaccountId);
        }
    }
}
