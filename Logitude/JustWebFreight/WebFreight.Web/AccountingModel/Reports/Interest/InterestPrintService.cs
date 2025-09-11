using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.InterestService;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityMapping;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Enums;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Microsoft.TeamFoundation.Work.WebApi;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
        public InterestReportPM _InterestReportPM;
        private int flatLineCounter = 0;

        public InterestDataProvider LoadDataProvider(string entityId, int tenant)
        {
            InterestDataProvider interestReportDP = InitializeInterestDataProvider(entityId, tenant);
            List<InterestReportLinesByDateProvider> interestReportPeriods = GetInterestReportPeriods(entityId, tenant);
            interestReportDP.FutureInterestTransactions = GetFutureInterestTransactions(tenant);

            decimal? lastTotal = ProcessInterestTransactions(interestReportDP, interestReportPeriods, tenant);

            SetInterestReportMetadata(interestReportDP, interestReportPeriods, lastTotal, tenant);

            return interestReportDP;
        }


        private InterestDataProvider InitializeInterestDataProvider(string entityId, int tenant)
        {
            InterestDataProvider interestReportDP = new InterestDataProvider
            {
                InterestReportFlatLineList = new List<InterestReportFlatLine>()
            };


            InterestReportQueryService interestReportQuery = new InterestReportQueryService(tenant);
            _InterestReportPM = interestReportQuery.GetSingle(entityId, true, false);

            interestReportDP.InterestReportFlatLineList.Add(FirstFlatLine(_InterestReportPM));
            interestReportDP.OpenBalance = _InterestReportPM.OpenBalance;
            return interestReportDP;
        }

        private List<InterestReportLinesByDateProvider> GetInterestReportPeriods(string entityId, int tenant)
        {
            InterestReportService interestReportService = new InterestReportService();
            List<InterestTransactionList> interestTransactionLists = interestReportService.GetAllInterestTransactionByDate(entityId, null, tenant, null).interestTransactionLists;

            HashSet<InterestReportLinesByDateProvider> InterestReportPeriods = _InterestReportPM.InterestReportLinesByDates
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


        private decimal? ProcessInterestTransactions(InterestDataProvider interestReportDP, List<InterestReportLinesByDateProvider> interestReportPeriods, int tenant)
        {
            decimal? lastTotal = 0m;

            if (interestReportPeriods.Any())
            {
                decimal totalLocalAmountSum = interestReportDP.OpenBalance ?? 0m; ;
                foreach (var period in interestReportPeriods)
                {
                    decimal totalLocalInPeriod = 0m;
                    List<InterestReportFlatLine> periodLineList = new List<InterestReportFlatLine>();
                    foreach (var transactionDP in period.InterestTransactionList)
                    {
                        InterestReportFlatLine line = new InterestReportFlatLine();
                        line.LineNo = ++flatLineCounter;
                        line.LineType = InterestPeriodLineTypes.Transaction;
                        line.Date = transactionDP.InterestValueDate.Value.Date;
                        totalLocalInPeriod += transactionDP.LocalAmount;
                        line.LocalAmount = transactionDP.LocalAmount;
                        line.AmountInCurrencyReport = _InterestReportPM.IsForeignCurrency ? transactionDP.ForeignAmount : transactionDP.LocalAmount;
                        line.Reference1 = GetReference1(transactionDP);
                        line.Notes = GetNotes(transactionDP);
                        line.IsOpenBalanceLine = false;

                        periodLineList.Add(line);
                    }
                    if (periodLineList.Any())
                    {
                        // First in a period
                        var firstLineInPeriod = periodLineList[0];
                        firstLineInPeriod.Date = period.FromDate.Value.Date;
                        firstLineInPeriod.NumberOfDays = period.TotalInterestDays;
                        firstLineInPeriod.LineType = InterestPeriodLineTypes.FirstInPeriod;
                        

                        // Last in a period
                        var lastLineInPeriod = periodLineList.Last();
                        lastLineInPeriod.Date = period.FromDate.Value.Date;
                        lastLineInPeriod.Notes = TranslateTextsClass.Translate("Accounting.General.O.TotalInterest", tenant);
                        lastLineInPeriod.LineType = InterestPeriodLineTypes.LastInPeriod;

                        lastLineInPeriod.TotalLocalInPeriod = totalLocalInPeriod;

                        totalLocalAmountSum += totalLocalInPeriod;
                        lastLineInPeriod.TotalToDate = totalLocalAmountSum;
                        lastTotal = lastLineInPeriod.TotalToDate;

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
            return lastTotal;

        }


        private void SetInterestReportMetadata(InterestDataProvider interestReportDP, List<InterestReportLinesByDateProvider> interestReportPeriods,
            decimal? lastTotal, int tenant)
        {
            GetGLAccountDisplayNumber(tenant, interestReportDP, _InterestReportPM);
            GetCurrency(tenant,_InterestReportPM.ReportCurrencyId  ,  interestReportDP);

            interestReportDP.CustomerName = _InterestReportPM.CustomerName;
            interestReportDP.InterestCalculationDate = _InterestReportPM.InterestCalculationDate;
            interestReportDP.InvoiceNumber = _InterestReportPM.ARInvoiceNumber;
            interestReportDP.InterestReportLinesByDateList = interestReportPeriods;
            interestReportDP.TotalAmount = _InterestReportPM.TotalAmount;
            interestReportDP.CreditAllotmentPercentage = _InterestReportPM.CreditAllotmentPercentage;
            interestReportDP.CalCreditAllotmentCommission = _InterestReportPM.CalCreditAllotmentCommission;
            interestReportDP.CalculatedPostponedChequesCommision = _InterestReportPM.CalculatedPostponedChequesCommision ?? 0m;
            interestReportDP.AllotmentCommession = _InterestReportPM.CalCreditAllotmentCommission;
            interestReportDP.AllotmentCalculation = SetAllotmentCalculationEquation(interestReportDP, _InterestReportPM);
            interestReportDP.PostponedChequesCommission = !string.IsNullOrEmpty(_InterestReportPM.GLAccountId) ? GetPostponedChequesCommission(_InterestReportPM.GLAccountId, _InterestReportPM.Tenant) : null;
            interestReportDP.CountPostponedCheques = CalcCountPostponedCheques(interestReportDP.CalculatedPostponedChequesCommision, interestReportDP.PostponedChequesCommission);
            interestReportDP.TotalAmountWithPostponedCheques = _InterestReportPM?.TotalAmount + _InterestReportPM?.CalculatedPostponedChequesCommision;
            interestReportDP.InterestReportFlatLineList.Add(EndFlatLine(_InterestReportPM, lastTotal));

            Reorder(interestReportDP);



        }

        private void Reorder(InterestDataProvider interestReportDP)
        {
            // Find the open balance line
            var openBalanceLine = interestReportDP.InterestReportFlatLineList
                .FirstOrDefault(ln => ln.IsOpenBalanceLine);

            if (openBalanceLine != null)
            {
                DateTime openBalanceLineDate = openBalanceLine.Date;

                // Partition lines into Group A and Others
                var groupA = interestReportDP.InterestReportFlatLineList
                    .Where(ln => !ln.IsOpenBalanceLine && ln.Date.Date < openBalanceLineDate)
                    .ToList();

                var others = interestReportDP.InterestReportFlatLineList
                    .Where(ln => !(!ln.IsOpenBalanceLine && ln.Date.Date < openBalanceLineDate))
                    .ToList();

                // Rebuild list: everything before openBalanceLine (except Group A), then Group A, then openBalanceLine, then rest
                var reordered = new List<InterestReportFlatLine>();

                foreach (var ln in others)
                {
                    if (ln == openBalanceLine)
                    {
                        // insert Group A right before the openBalanceLine
                        reordered.AddRange(groupA);
                        reordered.Add(openBalanceLine);
                    }
                    else
                    {
                        reordered.Add(ln);
                    }


                }

                int count = 1;
                decimal runningTotal = 0m;
                foreach (var line in reordered)
                {
                    if (line.TotalLocalInPeriod.HasValue)
                    {
                        runningTotal += line.TotalLocalInPeriod.Value;
                        line.TotalToDate = runningTotal;
                    }
                    else
                        line.TotalToDate = null;

                    line.LineNo = count++;
                }
                // Replace original list
                interestReportDP.InterestReportFlatLineList = reordered;
            }

        }

        private List<FutureInterestTransactionProvider> GetFutureInterestTransactions(int tenant)
        {
            List<FutureInterestTransactionProvider> futureInterestTransactions = new List<FutureInterestTransactionProvider>();
            IAccountingContext context = AccountingContext.GetContext(tenant);
            InterestTransactionListQueryService interestTransactionQueryService = new InterestTransactionListQueryService(context);
            DateTime reportMonthLastDay = DateTimeStaticExtention.GetLastDayOfMonth(_InterestReportPM.InterestCalculationDate.Date);
            IQueryable<InterestTransactionList> futureQuery = 
                interestTransactionQueryService.GetFutureInterestTransactionsByInterestReportMonth(reportMonthLastDay,
                _InterestReportPM.GLAccountId, tenant);
            if (futureQuery != null)
            {
                List<InterestTransactionList> transactionLists = futureQuery.ToHashSet()
                                                                            .GroupBy(t => t.Id)
                                                                            .Select(g => g.First())
                                                                            .ToList();
                if (transactionLists.Count > 0)
                {
                    foreach (InterestTransactionList transactionList in transactionLists)
                    {
                        FutureInterestTransactionProvider futureInterestTransactionProvider = new FutureInterestTransactionProvider()
                        {
                            AccountEntityCode = transactionList.AccountEntityCode,
                            AccountingEntityCode = transactionList.AccountingEntityCode,
                            AccountingDate = transactionList.AccountingDate ?? DateTime.Now,
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
                            AmountInCurrencyReport = _InterestReportPM.IsForeignCurrency ? transactionList.ForeignAmount : transactionList.LocalAmount,
                            Reference1 = GetReference1FromITList(transactionList),
                        };
                        futureInterestTransactions.Add(futureInterestTransactionProvider);
                    }
                }
            }
            return futureInterestTransactions;
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
               
                rv.TotalToDate = rv.LocalAmount;
                rv.TotalLocalInPeriod = rv.LocalAmount;

                var openline = interestReportPM.InterestReportLinesByDates.FirstOrDefault(l => l.IsOpenBalanceLine == true);
                if (openline != null)
                {
                    rv.NumberOfDays = openline.TotalInterestDays;
                    rv.CalculatedStdInterestAmount = openline.CalculatedStandInterestAmount;
                    rv.StdPercentage = openline.StandardInterestPercentage;
                    rv.TotalStdInterest = openline.StandardInterestAmount;
                    rv.CalculatedExcInterestAmount = openline.CalculatedExcepInterestAmount;
                    rv.ExcPercentage = openline.ExceptionalInterestPercentage;
                    rv.TotalExcInterest = openline.ExceptionalInterestAmount;
                    rv.CalculatedCrdInterestAmount = openline.CalculatedCreditInterestAmount;
                    rv.CrdPercentage = openline.CreditInterestPercentage;
                    rv.TotalCrdInterest = openline.CreditInterestAmount;
                    rv.CalculationDetails = openline.CalculationDetails;
                    rv.Date = openline.FromDate;
                    rv.IsOpenBalanceLine = true;
                }

            }
            return rv;
        }

        private InterestReportFlatLine EndFlatLine(InterestReportPM interestReportPM, decimal? lastTotal)
        {
            InterestReportFlatLine rv = new InterestReportFlatLine();
            rv.LineNo = ++flatLineCounter;
            rv.LineType = InterestPeriodLineTypes.End;
            rv.Date = interestReportPM.InterestReportLinesByDates.Max(l => l.ToDate);
            if (interestReportPM != null)
            {
                rv.Notes = TranslateTextsClass.Translate("Accounting.General.O.ReportTotalInterest", interestReportPM.Tenant);
                rv.AccumulatedForInterest = _InterestReportPM.TotalAmount ?? 0m;
                rv.TotalToDate = lastTotal ?? 0m;
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

        private string GetReference1FromITList(InterestTransactionList interestTransactionList)
        {
            return interestTransactionList.InterestEntityTypeCode == InterestEntityTypes.Journal
                                            ? interestTransactionList.Reference1
                                            : interestTransactionList.Source;
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

        private static void GetGLAccountDisplayNumber(int tenant, InterestDataProvider InterestReportDP, InterestReportPM interestReportPM)
        {
            GLAccountQueryService glAccountQuery = new GLAccountQueryService(tenant);
            GLAccountPM gLAccount = glAccountQuery.GetSinglePM(interestReportPM.GLAccountId, tenant);
            InterestReportDP.GLAccountDisplayNumber = gLAccount.DisplayNumber;
        }

        private static void GetCurrency(int tenant,string currencyId, InterestDataProvider InterestReportDP)
        {
            CurrencyQuery currencyQuery = new CurrencyQuery(tenant);
            CurrencyPM currencyPM = currencyQuery.GetSinglePM(currencyId, tenant);

            InterestReportDP.ReportCurrency = currencyPM?.Code +" "+ currencyPM?.LocalName ;
        }


        private static string SetAllotmentCalculationEquation(InterestDataProvider InterestReportDP, InterestReportPM interestReportPM)
        {
            if (interestReportPM.CreditAllotmentPercentage != null)
            {
                return string.Concat(interestReportPM.GLAccountInterestCreditLimit, " * ", '(', interestReportPM.CreditAllotmentPercentage, " / 100)");
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
