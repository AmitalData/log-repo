using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.ReportsWebServices;

namespace WebFreight.Web.AccountingModel.Reports.TaxDeductionReport
{
    public class TaxDeductionReportPrintService
    {
        public TaxDeductionReportPM taxDeductionReportPM;


        public TaxDeductionReportData LoadDataProvider(string entityId, int tenant)
        {
            IAccountingContext context = AccountingContext.GetContext(tenant);
            TaxDeductionReportQueryService taxDeductionReportQueryService = new TaxDeductionReportQueryService(context);
            taxDeductionReportPM = taxDeductionReportQueryService.GetSingle(entityId, false, false);

            TaxDeductionReportDataProvider deductionReportDataProvider = new TaxDeductionReportDataProvider(taxDeductionReportPM, tenant, null);
            TaxDeductionReportData data = deductionReportDataProvider.GetTaxDeductionReportData();

            if (taxDeductionReportPM.ByMonth)
            {
                if (taxDeductionReportPM.FromMonth.HasValue && taxDeductionReportPM.Month.HasValue)
                {
                    data.TaxYear = $"{taxDeductionReportPM.FromMonth.Value.Month}/{taxDeductionReportPM.TaxYear} - {taxDeductionReportPM.Month.Value.Month}/{taxDeductionReportPM.TaxYear}";
                }
                else if (taxDeductionReportPM.Month.HasValue)
                {
                    data.TaxYear = $"{taxDeductionReportPM.Month.Value.Month}/{taxDeductionReportPM.TaxYear}";
                }
                else
                {
                    data.TaxYear = taxDeductionReportPM.TaxYear.ToString();
                }
            }
            else
            {
                data.TaxYear = taxDeductionReportPM.TaxYear.ToString();
            }


            data.ByVendorList = data.ByVendorList.OrderBy(d => d.VendorLocalName).ToList();

            new TaxDeductionReportService().SaveAndUpdateReportWithLock(context, taxDeductionReportPM, data, tenant, entityId);

            return data;
        }
    }
}