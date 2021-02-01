using EvoPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
            pdfConverter.PdfDocumentOptions.LeftMargin = 10;
            pdfConverter.PdfDocumentOptions.RightMargin = 10;
            HtmlToPdfElement headerHtml = new HtmlToPdfElement(0, 0, 0, 0, GetEvoPdfHtmlHeader(), null, 2040, 0);
            pdfConverter.PdfHeaderOptions.AddElement(headerHtml);
            pdfConverter.PdfHeaderOptions.HeaderHeight = 100;

            HtmlToPdfElement footerHtml = new HtmlToPdfElement(0, 0, 0, 0, GetEvoPdfHtmlFooter(), null, 2040, 0);
            pdfConverter.PdfFooterOptions.AddElement(footerHtml);
            pdfConverter.PdfFooterOptions.FooterHeight = 20;

            var footerTextElement = new TextElement(0, 20, "page &p; of &P;  ", new Font(new System.Drawing.FontFamily("Times New Roman"), 7, GraphicsUnit.Point));
            footerTextElement.TextAlign = HorizontalTextAlign.Right;
            pdfConverter.PdfFooterOptions.AddElement(footerTextElement);



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


        private string GetEvoPdfHtmlHeader()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table border='1' cellpadding='1' cellspacing='1' style='height: 150px; width: 100 % '>" );
            stringBuilder.Append("<tbody>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width: 30 % '><img  src='https://ckeditor.com/apps/ckfinder/userfiles/files/123984258_682208545772232_645619898285396818_n(1).jpg' style='height:150px; width:290px' /></td>");
            stringBuilder.Append("<td style='text-align:center;vertical-align:top; width:30 % '><div style='margin - top:50px; '><strong>InVentory Report</strong></div> </td>");
            stringBuilder.Append("<td style='text-align:center;vertical-align:top; width:30 % '><div style='margin - top:50px; '><strong>22 Jub 2020 Report</strong></div> </td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</tbody>");
            stringBuilder.Append("</table>");

            return stringBuilder.ToString();
        }
        private string GetEvoPdfHtmlFooter()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<div>Print by abed </div>");
           

            return stringBuilder.ToString();
        }
        



        private string GetBIReportTableStyle()
        {
            return " <style> table {font-family: arial, sans-serif;border-collapse: collapse;width: 100%;} td, th {border: 1px solid #dddddd;text-align: left;padding: 8px;} tr:nth-child(even) { background-color: #dddddd;}</style>";
        }

    }
}