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
    public class ExportBIReportToPDFService
    {
        public byte[] Run(DataTable dataTable)
        {
            byte[] pdfData = null;
            if (dataTable != null)
            {
                if (!CheckReportDataExecuteQuota(dataTable))
                {
                    StringBuilder stringBuilder = new StringBuilder("<table>");
                    stringBuilder.Append(GetBIReportHtmlHeader(dataTable));
                    stringBuilder.Append(GetBIReportHtmlRows(dataTable));
                    stringBuilder.Append("</table>");
                    pdfData = ConvertHTMLToEvoPDF(stringBuilder.ToString());
                }
                else
                {
                    throw new Exception("The report size is too big to be downloaded in PDF. Try by downloading it to Excel format.");
                }
            }
            return pdfData;
        }

        private string GetBIReportHtmlHeader(DataTable dataTable)
        {
            StringBuilder stringBuilder = new StringBuilder("<thead><tr>");
            foreach (DataColumn column in dataTable.Columns)
            {
                stringBuilder.Append(" <th>" + column.ColumnName + "</th> ");
            }
            stringBuilder.Append("</tr></thead>");

            return stringBuilder.ToString();
        }

        private string GetBIReportHtmlRows(DataTable dataTable)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (DataRow row in dataTable.Rows)
            {
                stringBuilder.Append(" <tr>");

                foreach (DataColumn column in dataTable.Columns)
                {
                    stringBuilder.Append("<td>"+row[column.ColumnName].ToString() + "</td>");
                }

                stringBuilder.Append(" /<tr>");

            }

            return stringBuilder.ToString();
        }

        private bool CheckReportDataExecuteQuota(DataTable dataTable)
        {
            bool result = false;
            if (dataTable != null && (dataTable.Columns.Count > 15 || dataTable.Rows.Count > 40000))
            {
                result = true;
            }
            return result;
        }

        private byte[] ConvertHTMLToEvoPDF(string htmlString)
        {
            HtmlToPdfConverter pdfConverter = new HtmlToPdfConverter();
            pdfConverter.LicenseKey = "4W9+bn19bn5ue2B+bn1/YH98YHd3d3c=";
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
            pdfConverter.HtmlViewerWidth = 800;
            pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
            pdfConverter.PdfDocumentOptions.EnhancedGraphicsQuality = true;
            pdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
            pdfConverter.TriggeringMode = TriggeringMode.Auto;
            pdfConverter.NavigationTimeout = 120;
            var pdfData = pdfConverter.ConvertHtml(htmlString, null);
            return pdfData;
        }



    }
}