using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.InterestService;
using Logitude.Accounting.Data.EntityLists;
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
        public InterestDataProvider LoadDataProvider(string entityId, int tenant)
        {
            InterestDataProvider InterestReportDP = new InterestDataProvider();
            InterestReportQueryService InterestReportQuery = new InterestReportQueryService(tenant);
            InterestReportPM InteerstReportPM = InterestReportQuery.GetSingle(entityId, true, false);
            InterestReportService interestReportService = new InterestReportService();
            List<InterestTransactionList> interestTransactionLists = interestReportService.GetAllInterestTransactionByDate(entityId, null, tenant, null).interestTransactionLists;

            List<InterestReportLinesByDateProvider> InterestReportLines = InteerstReportPM.InterestReportLinesByDates.Select(d => new InterestReportLinesByDateProvider
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


            InterestReportDP.OpenBalance = InteerstReportPM.OpenBalance;
            InterestReportDP.CustomerName = InteerstReportPM.CustomerName;
            InterestReportDP.InterestCalculationDate = InteerstReportPM.InterestCalculationDate;
            InterestReportDP.InvoiceNumber = InteerstReportPM.ARInvoiceNumber;
            InterestReportDP.InterestReportLinesByDateList = InterestReportLines;
            InterestReportDP.TotalAmount = InteerstReportPM.TotalAmount;
            InterestReportDP.CreditAllotmentPercentage = InteerstReportPM.CreditAllotmentPercentage;
            InterestReportDP.CalCreditAllotmentCommission = InteerstReportPM.CalCreditAllotmentCommission;
            InterestReportDP.AllotmentCommession = InteerstReportPM.CalCreditAllotmentCommission;
            InterestReportDP.AllotmentCalculation= SetAllotmentCalculationEquation(InterestReportDP, InteerstReportPM);

            return InterestReportDP;
        }

         private static string SetAllotmentCalculationEquation(InterestDataProvider InterestReportDP, InterestReportPM InteerstReportPM)
        {
            if (InteerstReportPM.CreditAllotmentPercentage != null)
            {
                return string.Concat(InteerstReportPM.GLAccountInterestCreditLimit, " * ", '(', InteerstReportPM.CreditAllotmentPercentage, " / 100)");
            }
            return null;
        }
    }
}
