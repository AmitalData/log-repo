using EvoPdf;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers.BIReport
{
    public class ExportBIReportPdfService
    {
        private BIReportXMLData bIReportXMLData = null;
        private DataTable bIReportdataTable = null;
        private int tenant;
        private BIReportHtmlRenderingService bIReportHtmlRenderingService;
        private HtmlToPdfConverter pdfConverter = null;
        public ExportBIReportPdfService(BIReportXMLData bIReportXMLData, DataTable bIReportdataTable, int tenant)
        {
            this.bIReportXMLData = bIReportXMLData;
            this.bIReportdataTable = bIReportdataTable;
            this.tenant = tenant;
            this.bIReportHtmlRenderingService = new BIReportHtmlRenderingService(this.bIReportdataTable);
        }

        public byte[] Run()
        {
            byte[] pdfData = null;

            if (bIReportdataTable != null)
            {
                if (CheckIfAllowExportBIReportToPdfFormat())
                {
                    pdfData = GetEvoPdfData();
                }
                else
                {
                    throw new Exception("The report size is too big to be downloaded in PDF. Try by downloading it to Excel format.");
                }
            }
            return pdfData;
        }

        private byte[] GetEvoPdfData()
        {
            InitializePdfConverter();
            SetEvoPdfHeader();
            SetEvoPdfFooter();
            string htmlBody = bIReportHtmlRenderingService.Render();
            var pdfData = pdfConverter.ConvertHtml(htmlBody, null);
            return pdfData;
        }


        private void InitializePdfConverter()
        {
            pdfConverter = new HtmlToPdfConverter();
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
        }


        private void SetEvoPdfHeader()
        {
            HtmlToPdfElement headerHtml = new HtmlToPdfElement(0, 0, 0, 0, GetEvoPdfHtmlHeader(), null, 2040, 0);
            pdfConverter.PdfHeaderOptions.AddElement(headerHtml);
            pdfConverter.PdfHeaderOptions.HeaderHeight = 105;
            pdfConverter.PdfDocumentOptions.ShowHeader = true;

        }


        private void SetEvoPdfFooter()
        {
            HtmlToPdfElement footerHtml = new HtmlToPdfElement(0, 0, 0, 0, GetEvoPdfHtmlFooter(), null, 2040, 0);
            pdfConverter.PdfFooterOptions.AddElement(footerHtml);
            pdfConverter.PdfFooterOptions.FooterHeight = 40;
            pdfConverter.PdfDocumentOptions.ShowFooter = true;
            SetFooterPageNumber();


        }

        private void SetFooterPageNumber()
        {
            var footerTextElement = new TextElement(0, 20, "page &p; of &P;  ", new Font(new System.Drawing.FontFamily("Times New Roman"), 10, GraphicsUnit.Point));
            footerTextElement.TextAlign = HorizontalTextAlign.Right;
            footerTextElement.LineStyle = new LineStyle(LineDashStyle.Solid);
            pdfConverter.PdfFooterOptions.AddElement(footerTextElement);
        }

        private string GetEvoPdfHtmlHeader()
        {
            string currentDate = GetCurrentDateAsStringFormat();
            string companyLogoBase64 = GetCompanyLogoBase64();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<div style='width:100%;height:350px'>");
            stringBuilder.Append("<table width='100%' style='border-collapse: collapse;width:100%;margin-right:10px;height:350px'>");
            stringBuilder.Append("<tbody><tr>");
            stringBuilder.Append("<td style='width:30%;text-align:left'><img style='width:725px;Height:350px;max-Height:350px' src='data:image/jpg;base64," + companyLogoBase64 + "'/>" + "</td>");
            stringBuilder.Append("<td style='table-layout: auto;vertical-align: center;Width:30%;text-align:center;max-width:30%;word-wrap:break-word'><div style='font-size:40px;'><strong>" + bIReportXMLData.BIReportPM.Name + "</strong></div> </td>");
            stringBuilder.Append("<td style='table-layout: auto;vertical-align: center;Width:40%;text-align:right;max-width:30%;word-wrap:break-word;'><div style='font-size:30px;margin-right:50px;margin-top:-90px'><strong>" + currentDate + "</strong></div> </td>");
            stringBuilder.Append("</tr></tbody>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("</div>");

            return stringBuilder.ToString();
        }

        private string GetEvoPdfHtmlFooter()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<div style='width:100%;vertical-align: center;text-align:center;height:86px;font-size:40px'>Printed by Logitude</div>");

            return stringBuilder.ToString();
        }

        private bool CheckIfAllowExportBIReportToPdfFormat()
        {
            bool isAllow = true;
            int numberOfBiReportColumn = bIReportdataTable.Columns.Cast<DataColumn>().Where(d => d.ColumnName == "Tenant").Any() ? 16 : 15;
            if (bIReportdataTable != null && (bIReportdataTable.Columns.Count > numberOfBiReportColumn || bIReportdataTable.Rows.Count > 40000))
            {
                isAllow = false;
            }
            return isAllow;
        }

        private string GetCurrentDateAsStringFormat()
        {
            TenantRepository tenantRepoitory = new TenantRepository(tenant);
            var curTenant = tenantRepoitory.GetSingleByTenant(tenant);
            string datetimeformat = @"dd\/MM\/yyyy";
            if (!string.IsNullOrEmpty(curTenant.DateTimeFormat)) datetimeformat = curTenant.DateTimeFormat;
            string todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).ToString(curTenant.DateTimeFormat, CultureInfo.CurrentCulture);
            return todayDate;
        }

        private string GetCompanyLogoBase64()
        {
            string result = string.Empty;
            byte[] companyLogoData = new HtmlEditorHelper().GetFileFromServer(("logo" + tenant.ToString()), "jpg", "logos", tenant);
            if (companyLogoData != null)
            {
                char[] companyLogobase64Data = new char[(int)(Math.Ceiling((double)companyLogoData.Length / 3) * 4)];
                Convert.ToBase64CharArray(companyLogoData, 0, companyLogoData.Length, companyLogobase64Data, 0);
                result = new String(companyLogobase64Data);
            }
            return result;
        }



    }
}