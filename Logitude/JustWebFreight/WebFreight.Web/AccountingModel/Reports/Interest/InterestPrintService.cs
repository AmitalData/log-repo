using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.Interfaces;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.AccountingModel.DomainServices;
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
            AccountingDomainService interestTransactionQuery = new AccountingDomainService();
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
                InterestTransactionList = interestTransactionQuery.GetAllInterestTransactionByDate(entityId, d.FromDate, tenant).interestTransactionLists.Select (a =>
                new InterestTransactionProvider
                {
                    EntityType = a.InterestEntityTypeCode,
                    EntityNumber = a.InterestEntityNumber,
                    LocalAmount = a.LocalAmount,
                    InterestValueDate = a.InterestValueDate,
                    CurrencyCode = a.CurrencyCode,
                    ForeignAmount = a.ForeignAmount,
                }).ToList(),
            }).ToList();


            InterestReportDP.OpenBalance = InteerstReportPM.OpenBalance;
            InterestReportDP.CustomerName = InteerstReportPM.CustomerName;
            InterestReportDP.InterestCalculationDate = InteerstReportPM.InterestCalculationDate;
            InterestReportDP.InvoiceNumber = InteerstReportPM.ARInvoiceNumber;
            InterestReportDP.InterestReportLinesByDateList = InterestReportLines;

            return InterestReportDP;
        }

    }
}
