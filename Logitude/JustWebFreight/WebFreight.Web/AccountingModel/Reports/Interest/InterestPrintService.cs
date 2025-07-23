using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.InterestService;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Enums;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.AccountingModel.DomainServices;
using WebFreight.Web.App_Code.AngularJS_App_Code.Generated;
using WebFreight.Web.Helpers;
 namespace WebFreight.Web.AccountingModel.Reports.Interest
{
    public class InterestPrintService
    {
        public InterestReportPM _InterestReportPM;
        public InterestDataProvider LoadDataProvider(string entityId, int tenant)
        {
            InterestDataProvider InterestReportDP = new InterestDataProvider();
            InterestReportQueryService InterestReportQuery = new InterestReportQueryService(tenant);
            _InterestReportPM = InterestReportQuery.GetSingle(entityId, true, false);
            InterestReportService interestReportService = new InterestReportService();
            List<InterestTransactionList> interestTransactionLists = interestReportService.GetAllInterestTransactionByDate(entityId, null, tenant, null).interestTransactionLists;

            List<InterestReportLinesByDateProvider> InterestReportLines = _InterestReportPM.InterestReportLinesByDates.Select(d => new InterestReportLinesByDateProvider
            {
                FromDate = d.FromDate,
                ToDate = d.ToDate,
                AccumulatedAmount = d.AccumulatedAmount,
                TotalAmount = d.TotalAmount,
                TotalInterestDays = d.TotalInterestDays,
                StandardInterestPercentage = d.StandardInterestPercentage,
                ExceptionalInterestPercentage = d.ExceptionalInterestPercentage,
                CreditInterestPercentage = d.CreditInterestPercentage,
                CalculationDetails = d.CalculationDetails,
                TotalInterest = d.CalculatedCreditInterestAmount + d.CalculatedExcepInterestAmount + d.CalculatedStandInterestAmount,
                TotalLocalAmount = interestTransactionLists == null ? 0 : interestTransactionLists.Sum(s => s.LocalAmount),
                InterestTransactionList = interestTransactionLists == null ? null : interestTransactionLists.Where(s => s.InterestValueDate.Date == d.FromDate.Date).Select(a =>

                new InterestTransactionProvider
                {
                    EntityType = a.InterestEntityIconCode,
                    EntityNumber = a.InterestEntityNumber,
                    LocalAmount = a.LocalAmount,
                    InterestValueDate = a.InterestValueDate,
                    CurrencyCode = a.CurrencyCode,
                    ForeignAmount = a.ForeignAmount,
                }).ToList(),
                GroupedInterestTransactionList = interestTransactionLists == null ? null : interestTransactionLists.Where(s => s.InterestValueDate.Date == d.FromDate.Date).GroupBy(x => new { x.InterestEntityNumber, x.InterestEntityIconCode, x.CurrencyCode, x.InterestValueDate }).Select(a =>
                   new InterestTransactionProvider
                   {
                       EntityType = a.Key.InterestEntityIconCode,
                       EntityNumber = a.Key.InterestEntityNumber,
                       LocalAmount = a.Sum(x => x.LocalAmount),
                       InterestValueDate = a.Key.InterestValueDate,
                       CurrencyCode = a.Key.CurrencyCode,
                       ForeignAmount = a.Sum(x => x.ForeignAmount),
                   }).ToList(),
            }).ToList();

            List<FutureInterestTransactionProvider> futureInterestTransactions = new List<FutureInterestTransactionProvider>();
            IAccountingContext context = AccountingContext.GetContext(tenant);
            InterestTransactionListQueryService interestTransactionQueryService = new InterestTransactionListQueryService(context);
            DateTime reportMonthLastDay = DateTimeStaticExtention.GetLastDayOfMonth(_InterestReportPM.InterestCalculationDate.Date);
            IQueryable<InterestTransactionList> futureQuery = 
                interestTransactionQueryService.GetFutureInterestTransactionsByInterestReportMonth(reportMonthLastDay, 
                _InterestReportPM.GLAccountId, tenant);
            if (futureQuery != null)
            { 
                List<InterestTransactionList> transactionLists = futureQuery.ToList();
                if (transactionLists.Count > 0) 
                { 
                    foreach (InterestTransactionList transactionList in transactionLists)
                    {
                        FutureInterestTransactionProvider futureInterestTransactionProvider = new FutureInterestTransactionProvider()
                        { 
                            AccountEntityCode = transactionList.AccountEntityCode,
                            AccountingEntityCode = transactionList.AccountingEntityCode,
                            AccountingDate = transactionList.AccountingDate,
                            CreateDateTime = transactionList.CreateDateTime,    
                            CurrencyCode = transactionList.CurrencyCode,
                            ForeignAmount = transactionList.ForeignAmount,
                            EntityType = transactionList.InterestEntityIconCode,
                            EntityNumber = transactionList.InterestEntityNumber,
                            InterestValueDate = transactionList.InterestValueDate,
                            IsCancelled = transactionList.IsCancelled,
                            IsClosed = transactionList.IsClosed,
                            InterestEntityTypeCode = transactionList.InterestEntityTypeCode,
                            InterestEntityType = transactionList.InterestEntityType, 
                            JournalNumber = transactionList.JournalNumber,
                            LocalAmount = transactionList.LocalAmount,
                            Notes = transactionList.Notes,
                            OriginalEntityLineNumber = transactionList.OriginalEntityLineNumber,
                            SearchFields = transactionList.SearchFields,
                            Source = transactionList.Source,
                            SourceType = transactionList.SourceType,
                            SourceTypeCode = transactionList.SourceTypeCode,
                            Tenant = transactionList.Tenant,
                            Reference1 = GetReference1(transactionList),
                        };
                        futureInterestTransactions.Add(futureInterestTransactionProvider);
                    }
                }
            }
            InterestReportDP.FutureInterestTransactions = futureInterestTransactions;

            GetGLAccountDisplayNumber(tenant, InterestReportDP, _InterestReportPM);

            InterestReportDP.OpenBalance = _InterestReportPM.OpenBalance;
            InterestReportDP.CustomerName = _InterestReportPM.CustomerName;
            InterestReportDP.InterestCalculationDate = _InterestReportPM.InterestCalculationDate;
            InterestReportDP.InvoiceNumber = _InterestReportPM.ARInvoiceNumber;
            InterestReportDP.InterestReportLinesByDateList = InterestReportLines;
            InterestReportDP.TotalAmount = _InterestReportPM.TotalAmount;
            InterestReportDP.CreditAllotmentPercentage = _InterestReportPM.CreditAllotmentPercentage;
            InterestReportDP.CalCreditAllotmentCommission = _InterestReportPM.CalCreditAllotmentCommission;
            InterestReportDP.CalculatedPostponedChequesCommision = _InterestReportPM.CalculatedPostponedChequesCommision;
            InterestReportDP.AllotmentCommession = _InterestReportPM.CalCreditAllotmentCommission;
            InterestReportDP.AllotmentCalculation = SetAllotmentCalculationEquation(InterestReportDP, _InterestReportPM);

            return InterestReportDP;
        }

        private string GetReference1(InterestTransactionList interestTransactionList)
        {
            return interestTransactionList.InterestEntityType == InterestEntityTypeCodes.Journal
                                            ? interestTransactionList.Reference1
                                            : interestTransactionList.InterestEntityNumber;
            return interestTransactionList.InterestEntityNumber;

        }

        private static void GetGLAccountDisplayNumber(int tenant, InterestDataProvider InterestReportDP, InterestReportPM _InterestReportPM)
        {
            GLAccountQueryService glAccountQuery = new GLAccountQueryService(tenant);
            GLAccountPM gLAccount = glAccountQuery.GetSinglePM(_InterestReportPM.GLAccountId, tenant);
            InterestReportDP.GLAccountDisplayNumber = gLAccount.DisplayNumber;
        }

        private static string SetAllotmentCalculationEquation(InterestDataProvider InterestReportDP, InterestReportPM _InterestReportPM)
        {
            if (_InterestReportPM.CreditAllotmentPercentage != null)
            {
                return string.Concat(_InterestReportPM.GLAccountInterestCreditLimit, " * ", '(', _InterestReportPM.CreditAllotmentPercentage, " / 100)");
            }
            return null;
        }


    }

    public static class DateTimeStaticExtention
    {
        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }
    }
}
