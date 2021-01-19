using EvoPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers.BIReport
{
    public class ExportBIReportPdfService
    {

        public byte[] Run(DataTable dataTable)
        {
            byte[] pdfData = null;
            if (dataTable != null)
            {
                if (!CheckReportDataExecuteQuota(dataTable))
                {
                    pdfData = GetEvoPDFData(dataTable);
                }
                else
                {
                    throw new Exception("The report size is too big to be downloaded in PDF. Try by downloading it to Excel format.");
                }
            }
            return pdfData;
        }

        private byte[] GetEvoPDFData(DataTable dataTable)
        {
            HtmlToPdfConverter pdfConverter = new HtmlToPdfConverter();
            pdfConverter.LicenseKey = "fvDj8eTh8eDg4vHk/+Hx4uD/4OP/6Ojo6A==";
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
            pdfConverter.HtmlViewerWidth = 800;
            pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
            pdfConverter.PdfDocumentOptions.EnhancedGraphicsQuality = true;
            pdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
            pdfConverter.TriggeringMode = TriggeringMode.Auto;
            pdfConverter.NavigationTimeout = 120;
            string htmlBody = GetPdfHtmlBody(dataTable);
            var pdfData = pdfConverter.ConvertHtml(htmlBody, null);
            return pdfData;
        }

        private string GetPdfHtmlBody(DataTable dataTable)
        {
            StringBuilder stringBuilder = new StringBuilder("<!DOCTYPE html> <html> <head>" + GetBIReportTableStyle() + "</head><body>");
            stringBuilder.Append("<table>");
            stringBuilder.Append(GetBIReportHtmlHeaderTable(dataTable));
            stringBuilder.Append(GetBIReportHtmlRowsTable(dataTable));
            stringBuilder.Append("</table>");
            stringBuilder.Append("</body>");
            stringBuilder.Append("</html>");

            return stringBuilder.ToString();
        }

        private string GetBIReportHtmlHeaderTable(DataTable dataTable)
        {
            StringBuilder stringBuilder = new StringBuilder("<thead><tr>");
            foreach (DataColumn column in dataTable.Columns)
            {
                stringBuilder.Append(" <th>" + column.ColumnName + "</th> ");
            }
            stringBuilder.Append("</tr></thead>");

            return stringBuilder.ToString();
        }

        private string GetBIReportHtmlRowsTable(DataTable dataTable)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (DataRow row in dataTable.Rows)
            {
                stringBuilder.Append(" <tr>");

                foreach (DataColumn column in dataTable.Columns)
                {
                    stringBuilder.Append("<td>"+row[column.ColumnName].ToString() + "</td>");
                }

                stringBuilder.Append(" </tr>");

            }

            return stringBuilder.ToString();
        }

        private bool CheckReportDataExecuteQuota(DataTable dataTable)
        {
            bool isExecutedQuota = false;
            int numberOfColumnQuota = dataTable.Columns.Cast<DataColumn>().Where(d => d.ColumnName == "Tenant").Any() ? 16 : 15;
            if (dataTable != null && (dataTable.Columns.Count > numberOfColumnQuota || dataTable.Rows.Count > 40000))
            {
                isExecutedQuota = true;
            }
            return isExecutedQuota;
        }

        private string GetBIReportTableStyle()
        {
            return " <style> table {font-family: arial, sans-serif;border-collapse: collapse;width: 100%;} td, th {border: 1px solid #dddddd;text-align: left;padding: 8px;} tr:nth-child(even) { background-color: #dddddd;}</style>";
        }

    }
}