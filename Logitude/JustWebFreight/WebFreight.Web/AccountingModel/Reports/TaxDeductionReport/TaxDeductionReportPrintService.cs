using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.ReportsWebServices;

namespace WebFreight.Web.AccountingModel.Reports.TaxDeductionReport
{
    public class TaxDeductionReportPrintService
    {

        //public void BuildPaymentChequeReport(string entityId, int tenant, string documentOutId)
        //{

        //    TaxDeductionReportData TaxDeductionDP = LoadDataProvider(entityId, tenant);


        //    // 2
        //    // Get byte[] of DataProvider
        //    XmlSerializer serializer = new XmlSerializer(typeof(TaxDeductionReportData));
        //    MemoryStream memstream = new MemoryStream();
        //    serializer.Serialize(memstream, TaxDeductionDP);
        //    memstream.Seek(0, SeekOrigin.Begin);
        //    var reader = new StreamReader(memstream);
        //    string content = reader.ReadToEnd();
        //    byte[] bytearray = memstream.ToArray();


        //    // 3
        //    // Get StiObject
        //    StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "TDDP", Name = "TaxDeductionReportData", BusinessObjectValue = TaxDeductionDP };


        //    // 4
        //    //Build report
        //    Byte[] templatedata = null;
        //    StiReport report = new StiReport();
        //    DocumentTypeTemplateRepository documentTypeTemplaterep = new DocumentTypeTemplateRepository(tenant);
        //    DocumentOutRepository documentOutRepository = new DocumentOutRepository(tenant);


        //    DocumentOut documentOut = documentOutRepository.GetSingleDocumentOut(documentOutId, tenant);
        //    DocumentTypeTemplate defaulttemplate = documentTypeTemplaterep.GetSingleDocumentTypeTemplate(documentOut.DocumentTemplateId);

        //    if (defaulttemplate != null)
        //        templatedata = defaulttemplate.TemplateBody;


        //    if (templatedata != null)
        //    {
        //        if (templatedata.Length != 0)
        //        {
        //            ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
        //            report = exportDocumentHelper.LoadandRender(report, templatedata, defaulttemplate, currentBusinessObject, documentTypeTemplaterep, tenant);
        //        }
        //    }



        //}

        public TaxDeductionReportData LoadDataProvider(string entityId, int tenant)
        {
            TaxDeductionReportData TaxDeductionDP = new TaxDeductionReportData();
             TaxDeductionReportQueryService taxDeductionReportQueryService = new TaxDeductionReportQueryService(tenant);
            TaxDeductionReportPM taxDeductionReportPM = taxDeductionReportQueryService.GetSingle(entityId, false, false);
            //TenantQuery tenantQuery = new TenantQuery(tenant);
            //TenantPM tenantPM = TenantQuery.GetSingleTenantPM(tenant, false);
            TaxDeductionReportDataProvider deductionReportDataProvider = new TaxDeductionReportDataProvider(taxDeductionReportPM.TaxYear, tenant);
            TaxDeductionReportData data = deductionReportDataProvider.GetTaxDeductionReportData();
            return TaxDeductionDP;
        }

    }
}