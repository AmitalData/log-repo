using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers.BIReport
{
    public class ExportBIReportService
    {

        public byte[] Run(BIReportXMLData bIReportXMLData, int tenant, bool allowEmptyReport)
        {
            byte[] reportData = null;

            DWQueryBuilderHelper QBHelper = new DWQueryBuilderHelper(tenant);
            SqlCommandDefinition sqlCommandDefinition = QBHelper.GetQuerySQL(bIReportXMLData.DWQueryData);
            DataTable dataTable = QBHelper.GetDWQueryData(sqlCommandDefinition);
            if(!allowEmptyReport && dataTable.Rows.Count == 0)
            {
                return null;
            }
            BIReportsSecurityIntegrationService bIReportsSecurityIntegrationService = new BIReportsSecurityIntegrationService(tenant);
            bIReportsSecurityIntegrationService.CheckBIReportDataSecurity(dataTable);
            if (bIReportXMLData.ExportDataType == "Pdf")
            {
                reportData = new ExportBIReportPdfService(bIReportXMLData, dataTable, tenant).Run();
            }
            else
            {
                reportData = new ExportBIReportExcelService().Run(bIReportXMLData, dataTable, tenant);
            }

            return reportData;

        }

        public string GetBIReportExtensionFile(string exportDataType)
        {
            string result = exportDataType;
            if (exportDataType == "Excel") result = "xlsx";
            return result;

        }

    }
}