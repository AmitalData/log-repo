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
using WebFreight.Web.Helpers;
 namespace WebFreight.Web.AccountingModel.Reports.Interest
{
    public class InterestPrintService
    {
        public void /*InterestDataProvider*/ LoadDataProvider(string entityId, int tenant)
        {
            InterestDataProvider InterestReportDP = new InterestDataProvider();
            InterestReportQueryService InterestReportQuery = new InterestReportQueryService(tenant);
            InterestReportPM InteerstPM = InterestReportQuery.GetSingle(entityId, true, false);
            InterestReportDP.OpenBalance = InteerstPM.OpenBalance;
            //InterestReportDP.CustomerName = InteerstPM.cu;
            InterestReportDP.InterestCalculationDate = InteerstPM.InterestCalculationDate;
            InterestReportDP.InvoiceNumber = InteerstPM.ARInvoiceNumber;

            //     public decimal? OpenBalance { get; set; }
            //public string CustomerName { get; set; }
            //public string InvoiceNumber { get; set; }
            //public DateTime? InterestCalculationDate { get; set; }
            //public List<InterestReportLinesByDateProvider> InterestReportLinesByDateList { get; set; }

            //MementoDP.MementoSingleList = new MementoListProvider
            //{
            //    CreateDate = mementoPM.CreateDate,
            //    OwnerName = mementoPM.OwnerName,
            //    TypeName = mementoPM.TypeName,
            //    SeverityName = mementoPM.SeverityName,
            //    CreatedBy = mementoPM.CreatedByUserName,
            //    Subject = mementoPM.Subject
            //};



            //List<MementoLineListProvider> lines = mementoPM.MementoLines.Select(d => new MementoLineListProvider
            //{
            //    CreateDate = d.CreateDate,
            //    Contact = d.ContactName,
            //    Description = d.Description,
            //    Status = d.MementoLineStatusName
            //}).ToList();

            //MementoDP.MementoLineList = lines;

            //return MementoDP;
        }

    }
}
