using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.InterestService;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Enums;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel.Metadata;
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
        public InterestReportPM InterestReportPM;
        private int flatLineCounter = 0;

        public InterestDataProvider LoadDataProvider(string entityId, int tenant)
        {
            InterestDataProvider interestReportDP = InitializeInterestDataProvider(entityId, tenant);
            List<InterestReportLinesByDateProvider> interestReportPeriods = GetInterestReportPeriods(entityId, tenant);


            ProcessInterestTransactions(interestReportDP, interestReportPeriods, tenant);

            SetInterestReportMetadata(interestReportDP, interestReportPeriods, tenant);

            return interestReportDP;
        }
        private InterestDataProvider InitializeInterestDataProvider(string entityId, int tenant)
        {
            InterestDataProvider interestReportDP = new InterestDataProvider
            {
                InterestReportFlatLineList = new List<InterestReportFlatLine>()
            };


            InterestReportQueryService interestReportQuery = new InterestReportQueryService(tenant);
            InterestReportPM = interestReportQuery.GetSingle(entityId, true, false);

            interestReportDP.InterestReportFlatLineList.Add(FirstFlatLine(InterestReportPM));

            return interestReportDP;
        }

        private List<InterestReportLinesByDateProvider> GetInterestReportPeriods(string entityId, int tenant)
        {
            InterestReportService interestReportService = new InterestReportService();
            List<InterestTransactionList> interestTransactionLists = interestReportService.GetAllInterestTransactionByDate(entityId, null, tenant, null).interestTransactionLists;

            HashSet<InterestReportLinesByDateProvider> InterestReportPeriods = InterestReportPM.InterestReportLinesByDates
                .Where(l => l.IsOpenBalanceLine != true)
                .Select(d => new InterestReportLinesByDateProvider
            {
                FromDate = d.FromDate,
                ToDate = d.ToDate,
                AccumulatedAmount = d.AccumulatedAmount,
                TotalAmount = d.TotalAmount,
                TotalInterestDays = d.TotalInterestDays,

                CalculationDetails = d.CalculationDetails,

                // Percentages
                StandardInterestPercentage = d.StandardInterestPercentage,
                ExceptionalInterestPercentage = d.ExceptionalInterestPercentage,
                CreditInterestPercentage = d.CreditInterestPercentage,

                // Calculated Amounts
                CalculatedStandardInterestAmount = d.CalculatedStandInterestAmount,
                CalculatedExceptionalInterestAmount = d.CalculatedExcepInterestAmount,
                CalculatedCreditInterestAmount = d.CalculatedCreditInterestAmount,

                // Total Amounts
                TotalStandardInterestAmount = d.StandardInterestAmount,
                TotalExceptionalInterestAmount = d.ExceptionalInterestAmount,
                TotalCreditInterestAmount = d.CreditInterestAmount,


                TotalInterest = d.CalculatedCreditInterestAmount + d.CalculatedExcepInterestAmount + d.CalculatedStandInterestAmount,
                TotalLocalAmount = interestTransactionLists.Sum(s => s.LocalAmount),

                InterestTransactionList = interestTransactionLists.Where(s => s.InterestValueDate.Date == d.FromDate.Date)
                .Select(a =>
                new InterestTransactionProvider
                {
                    EntityType = a.InterestEntityIconCode,
                    EntityNumber = a.InterestEntityNumber,
                    LocalAmount = a.LocalAmount,
                    InterestValueDate = a.InterestValueDate,
                    CurrencyCode = a.CurrencyCode,
                    ForeignAmount = a.ForeignAmount,
                    Reference1 = a.Reference1,
                    Notes = a.Notes,
                }).ToList(),

                GroupedInterestTransactionList = interestTransactionLists.Where(s => s.InterestValueDate.Date == d.FromDate.Date)
                .GroupBy(x => new { x.InterestEntityNumber, x.InterestEntityIconCode, x.CurrencyCode, x.InterestValueDate })
                .Select(a =>
                   new InterestTransactionProvider
                   {
                       EntityType = a.Key.InterestEntityIconCode,
                       EntityNumber = a.Key.InterestEntityNumber,
                       LocalAmount = a.Sum(x => x.LocalAmount),
                       InterestValueDate = a.Key.InterestValueDate,
                       CurrencyCode = a.Key.CurrencyCode,
                       ForeignAmount = a.Sum(x => x.ForeignAmount),
                   }).ToList(),
            }).ToHashSet();

            return InterestReportPeriods.OrderBy(s => s.FromDate).ToList<InterestReportLinesByDateProvider>();

        }


        private void ProcessInterestTransactions(InterestDataProvider interestReportDP, List<InterestReportLinesByDateProvider> interestReportPeriods, int tenant)
        {

            if (interestReportPeriods.Any())
            {

                foreach (var period in interestReportPeriods)
                {
                    List<InterestReportFlatLine> periodLineList = new List<InterestReportFlatLine>();
                    foreach (var transactionDP in period.InterestTransactionList)
                    {
                        InterestReportFlatLine line = new InterestReportFlatLine();
                        line.LineNo = ++flatLineCounter;
                        line.LineType = InterestPeriodLineTypes.Transaction;
                        line.Date = transactionDP.InterestValueDate;

                        line.LocalAmount = transactionDP.LocalAmount;
                        line.Reference1 = GetReference1(transactionDP);
                        line.Notes = GetNotes(transactionDP);

                        periodLineList.Add(line);
                    }
                    if (periodLineList.Any())
                    {
                        // First in a period
                        var firstLineInPeriod = periodLineList[0];
                        firstLineInPeriod.Date = period.FromDate;
                        firstLineInPeriod.NumberOfDays = period.TotalInterestDays;
                        firstLineInPeriod.LineType = InterestPeriodLineTypes.FirstInPeriod;

                        // Last in a period
                        var lastLineInPeriod = periodLineList.Last();
                        lastLineInPeriod.Date = period.ToDate;
                        lastLineInPeriod.Notes = TranslateTextsClass.Translate("Accounting.General.O.TotalInterest", tenant);
                        lastLineInPeriod.LineType = InterestPeriodLineTypes.LastInPeriod;
                        lastLineInPeriod.TotalToDate = period.TotalLocalAmount;

                        lastLineInPeriod.CalculatedStdInterestAmount = period.CalculatedStandardInterestAmount;
                        lastLineInPeriod.StdPercentage = period.StandardInterestPercentage;
                        lastLineInPeriod.TotalStdInterest = period.TotalStandardInterestAmount;

                        lastLineInPeriod.CalculatedExcInterestAmount = period.CalculatedExceptionalInterestAmount;
                        lastLineInPeriod.ExcPercentage = period.ExceptionalInterestPercentage;
                        lastLineInPeriod.TotalExcInterest = period.TotalExceptionalInterestAmount;

                        lastLineInPeriod.CalculatedCrdInterestAmount = period.CalculatedCreditInterestAmount;
                        lastLineInPeriod.CrdPercentage = period.CreditInterestPercentage;
                        lastLineInPeriod.TotalCrdInterest = period.TotalCreditInterestAmount;

                        lastLineInPeriod.CalculationDetails = period.CalculationDetails;



                        interestReportDP.InterestReportFlatLineList.AddRange(periodLineList);
                    }
                }

            }


        }


        private void SetInterestReportMetadata(InterestDataProvider interestReportDP, List<InterestReportLinesByDateProvider> interestReportPeriods, int tenant)
        {
            GetGLAccountDisplayNumber(tenant, interestReportDP, InterestReportPM);

            interestReportDP.OpenBalance = InterestReportPM.OpenBalance;
            interestReportDP.CustomerName = InterestReportPM.CustomerName;
            interestReportDP.InterestCalculationDate = InterestReportPM.InterestCalculationDate;
            interestReportDP.InvoiceNumber = InterestReportPM.ARInvoiceNumber;
            interestReportDP.InterestReportLinesByDateList = interestReportPeriods;
            interestReportDP.TotalAmount = InterestReportPM.TotalAmount;
            interestReportDP.CreditAllotmentPercentage = InterestReportPM.CreditAllotmentPercentage;
            interestReportDP.CalCreditAllotmentCommission = InterestReportPM.CalCreditAllotmentCommission;
            interestReportDP.CalculatedPostponedChequesCommision = InterestReportPM.CalculatedPostponedChequesCommision??0m;
            interestReportDP.AllotmentCommession = InterestReportPM.CalCreditAllotmentCommission;
            interestReportDP.AllotmentCalculation = SetAllotmentCalculationEquation(interestReportDP, InterestReportPM);
            interestReportDP.PostponedChequesCommission = !string.IsNullOrEmpty(InterestReportPM.GLAccountId) ? GetPostponedChequesCommission(InterestReportPM.GLAccountId, InterestReportPM.Tenant) : null;
            interestReportDP.CountPostponedCheques = CalcCountPostponedCheques(interestReportDP.CalculatedPostponedChequesCommision, interestReportDP.PostponedChequesCommission);
            interestReportDP.TotalAmountWithPostponedCheques = InterestReportPM?.TotalAmount + InterestReportPM?.CalculatedPostponedChequesCommision;

            interestReportDP.InterestReportFlatLineList.Add(EndFlatLine(InterestReportPM));


        }



        /// Creates and returns the first flat line of the interest report.
        private InterestReportFlatLine FirstFlatLine(InterestReportPM interestReportPM)
        {
            InterestReportFlatLine rv = new InterestReportFlatLine();
            rv.LineNo = ++flatLineCounter;
            rv.LineType = InterestPeriodLineTypes.First;
            if (interestReportPM != null)
            {
                rv.Date = interestReportPM.InterestCalculationDate;
                rv.Notes = TranslateTextsClass.Translate("Accounting.General.O.OpenAmount", interestReportPM.Tenant);
                rv.LocalAmount = interestReportPM.OpenBalance ?? 0m;
            }
            return rv;
        }

        private InterestReportFlatLine EndFlatLine(InterestReportPM interestReportPM)
        {
            InterestReportFlatLine rv = new InterestReportFlatLine();
            rv.LineNo = ++flatLineCounter;
            rv.LineType = InterestPeriodLineTypes.End;
            rv.Date = interestReportPM.InterestReportLinesByDates.Max(l => l.ToDate);
            if (interestReportPM != null)
            {
                rv.Notes = TranslateTextsClass.Translate("Accounting.General.O.ReportTotalInterest", interestReportPM.Tenant);
                rv.AccumulatedForInterest = InterestReportPM.TotalAmount ?? 0m;
            }
            return rv;
        }


        private string GetReference1(InterestTransactionProvider interestTransactionDP)
        {
            string rv = string.Empty;
            

            switch (interestTransactionDP.EntityType)   // InterestEntityIconCode
            {
                case InterestEntityTypeCodes.ARInvoice:
                case InterestEntityTypeCodes.ARPayment:
                case InterestEntityTypeCodes.Adjustments:
                case InterestEntityTypeCodes.InterestReport:
                    rv = interestTransactionDP.EntityNumber;
                    break;
                case InterestEntityTypeCodes.Journal:
                    rv = interestTransactionDP.Reference1;
                    break;
                default:
                    break;
            }
            return rv;

        }

        private string GetNotes(InterestTransactionProvider interestTransactionDP)
        {
            string rv = interestTransactionDP.Notes;


            switch (interestTransactionDP.EntityType)   // InterestEntityIconCode
            {
                case InterestEntityTypeCodes.ARInvoice:
                case InterestEntityTypeCodes.ARPayment:
                case InterestEntityTypeCodes.Adjustments:
                case InterestEntityTypeCodes.InterestReport:

                    break;
                case InterestEntityTypeCodes.Journal:

                    break;
                default:
                    break;
            }
            return rv;

        }

        private static void GetGLAccountDisplayNumber(int tenant, InterestDataProvider InterestReportDP, InterestReportPM InterestReportPM)
        {
            GLAccountQueryService glAccountQuery = new GLAccountQueryService(tenant);
            GLAccountPM gLAccount = glAccountQuery.GetSinglePM(InterestReportPM.GLAccountId, tenant);
            InterestReportDP.GLAccountDisplayNumber = gLAccount.DisplayNumber;
        }

        private static string SetAllotmentCalculationEquation(InterestDataProvider InterestReportDP, InterestReportPM InterestReportPM)
        {
            if (InterestReportPM.CreditAllotmentPercentage != null)
            {
                return string.Concat(InterestReportPM.GLAccountInterestCreditLimit, " * ", '(', InterestReportPM.CreditAllotmentPercentage, " / 100)");
            }
            return null;
        }

        private static decimal? GetPostponedChequesCommission(string glaccountId, int tenant)
        {
            GLAccountQueryService glAccountQuery = new GLAccountQueryService(tenant);
            GLAccountPM gLAccount = glAccountQuery.GetSingle(glaccountId, false, false);
            return gLAccount?.PostponedChequesCommission;
        }
        private static int CalcCountPostponedCheques(decimal? sum, decimal? postponedChequesCommission)
        {
            if (postponedChequesCommission != null &&
                sum != null && postponedChequesCommission != 0)
            {
                return  (int)(sum / postponedChequesCommission);
            }
            else
            {
                return 0;
            }
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
