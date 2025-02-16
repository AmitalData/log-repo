using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.InterestService;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
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
        public InterestReportPM InterestReportPM;
        private int FlatLineCounter_ = 0;
        public InterestDataProvider LoadDataProvider(string entityId, int tenant)
        {
            InterestDataProvider InterestReportDP = new InterestDataProvider();
            InterestReportDP.InterestReportFlatLineList = new List<InterestReportFlatLine>();

            InterestReportQueryService InterestReportQuery = new InterestReportQueryService(tenant);
            InterestReportPM = InterestReportQuery.GetSingle(entityId, true, false);
            InterestReportDP.InterestReportFlatLineList.Add(FirstFlatLine(InterestReportPM));
            InterestReportService interestReportService = new InterestReportService();
            List<InterestTransactionList> interestTransactionLists = interestReportService.GetAllInterestTransactionByDate(entityId, null, tenant, null).interestTransactionLists;

            List<InterestReportLinesByDateProvider> InterestReportLines = InterestReportPM.InterestReportLinesByDates.Select(d => new InterestReportLinesByDateProvider
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

                InterestTransactionList = interestTransactionLists == null ? null : interestTransactionLists.Where(s => s.InterestValueDate.Date == d.FromDate.Date)
                .Select(a =>
                new InterestTransactionProvider
                {
                    EntityType = a.InterestEntityIconCode,
                    EntityNumber = a.InterestEntityNumber,
                    LocalAmount = a.LocalAmount,
                    InterestValueDate = a.InterestValueDate,
                    CurrencyCode = a.CurrencyCode,
                    ForeignAmount = a.ForeignAmount,
                }).ToList(),

                GroupedInterestTransactionList = interestTransactionLists == null ? null : interestTransactionLists.Where(s => s.InterestValueDate.Date == d.FromDate.Date)
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
            }).ToList();

            InterestReportLines = InterestReportLines.OrderBy(s => s.FromDate).ToList();    

            if (InterestReportLines != null && InterestReportLines.Count > 0)
            {

                foreach (var period in InterestReportLines) // For each period
                {
                    List<InterestTransactionList> periodInterestTransactionList = interestTransactionLists == null ? null : interestTransactionLists.Where(s => s.InterestValueDate.Date == period.FromDate.Date)
                    List<InterestReportFlatLine> periodLineList = new List<InterestReportFlatLine>();
                    InterestReportFlatLine firstLine = FirstPeriodFlatLine(InterestReportPM, period, periodInterestTransactionList);


                    periodLineList.Add(firstLine);
                    if (periodLineList.Count > 0)
                    {
                        InterestReportDP.InterestReportFlatLineList.AddRange(periodLineList);
                    }
                }

            }

            GetGLAccountDisplayNumber(tenant, InterestReportDP, InterestReportPM);

            InterestReportDP.OpenBalance = InterestReportPM.OpenBalance;
            InterestReportDP.CustomerName = InterestReportPM.CustomerName;
            InterestReportDP.InterestCalculationDate = InterestReportPM.InterestCalculationDate;
            InterestReportDP.InvoiceNumber = InterestReportPM.ARInvoiceNumber;
            InterestReportDP.InterestReportLinesByDateList = InterestReportLines;
            InterestReportDP.TotalAmount = InterestReportPM.TotalAmount;
            InterestReportDP.CreditAllotmentPercentage = InterestReportPM.CreditAllotmentPercentage;
            InterestReportDP.CalCreditAllotmentCommission = InterestReportPM.CalCreditAllotmentCommission;
            InterestReportDP.CalculatedPostponedChequesCommision = InterestReportPM.CalculatedPostponedChequesCommision;
            InterestReportDP.AllotmentCommession = InterestReportPM.CalCreditAllotmentCommission;
            InterestReportDP.AllotmentCalculation = SetAllotmentCalculationEquation(InterestReportDP, InterestReportPM);
            InterestReportDP.PostponedChequesCommission = !string.IsNullOrEmpty(InterestReportPM.GLAccountId) ? GetPostponedChequesCommission(InterestReportPM.GLAccountId, InterestReportPM.Tenant) : null;
            InterestReportDP.CountPostponedCheques = CalcCountPostponedCheques(InterestReportDP.CalculatedPostponedChequesCommision , InterestReportDP.PostponedChequesCommission);
            InterestReportDP.TotalAmountWithPostponedCheques = InterestReportPM?.TotalAmount + InterestReportPM?.CalculatedPostponedChequesCommision;

            InterestReportDP.InterestReportFlatLineList.Add(EndFlatLine(InterestReportPM));

            return InterestReportDP;
        }



        /// Creates and returns the first flat line of the interest report.
        private InterestReportFlatLine FirstFlatLine(InterestReportPM interestReportPM)
        {
            InterestReportFlatLine rv = new InterestReportFlatLine();
            rv.LineNo = ++FlatLineCounter_;
            rv.LineType = "F"; // First 
            if (interestReportPM != null)
            {
                rv.Date = interestReportPM.InterestCalculationDate;
                rv.Details = TranslateTextsClass.Translate("Accounting.General.O.OpenAmount", interestReportPM.Tenant);
                rv.LocalAmount = interestReportPM.OpenBalance ?? 0m;
            }
            return rv;
        }

        private InterestReportFlatLine EndFlatLine(InterestReportPM interestReportPM)
        {
            InterestReportFlatLine rv = new InterestReportFlatLine();
            rv.LineNo = ++FlatLineCounter_;
            rv.LineType = "E"; // First 
            if (interestReportPM != null)
            {
                rv.Notes = TranslateTextsClass.Translate("Accounting.General.O.ReportTotalInterest", interestReportPM.Tenant);
                rv.AccumulatedForInterest = InterestReportPM.TotalAmount ?? 0m;
            }
            return rv;
        }

        private InterestReportFlatLine FirstPeriodFlatLine(InterestReportPM interestReportPM, InterestReportLinesByDateProvider period, List<InterestTransactionList> periodInterestTransactionList)
        {
            InterestReportFlatLine rv = new InterestReportFlatLine();
            rv.LineNo = ++FlatLineCounter_;
            rv.LineType = "P1"; // First in a period
            rv.Date = period.FromDate;
            rv.NumberOfDays = period.TotalInterestDays;

            FillRef1Notes(period, periodInterestTransactionList, ref rv);
            return rv;
        }

        private static void FillRef1Notes(InterestReportLinesByDateProvider period, List<InterestTransactionList> periodInterestTransactionList, ref InterestReportFlatLine flatLine)
        {
            flatLine.Reference1 = string.Empty;
            flatLine.Notes = string.Empty;
            if (period != null)
            {
                if (period.GroupedInterestTransactionList != null && period.GroupedInterestTransactionList.Count > 0)
                {
                    var firstlist = period.GroupedInterestTransactionList[0];
                    switch (firstlist.EntityType)   // InterestEntityIconCode
                    {
                        case "IN":
                        case "PY":
                        case "AJ":
                        case "IR":
                            flatLine.Reference1 = firstlist.EntityNumber;
                            break;
                        case "JR":
                            flatLine.Reference1 = firstlist.EntityNumber;
                            break;
                        default:
                            break;
                    }
                }
            }
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
}
