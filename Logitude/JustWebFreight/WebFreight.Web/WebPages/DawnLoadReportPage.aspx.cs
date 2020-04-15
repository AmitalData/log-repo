using System;
using System.Linq;
using System.Transactions;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.WebServices;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using WebFreight.Web.WcfApi;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Xsl;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel;
using Stimulsoft.Report;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Stimulsoft.Report.Export;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WebPages
{
    public partial class DawnLoadReportPage : System.Web.UI.Page
    {
        public byte[] _DatainByte;

        int? tenant = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            string fileName = Request["fileName"] ?? "";
            string type = Request["type"] ?? "";

            bool useOnePageHeaderAndFooter = ToBoolean(Request["UseOnePageHF"]);
            bool exportDataOnly = ToBoolean(Request["exportDataOnly"]);
            bool exportObjectFormatting = ToBoolean(Request["exportObjectForm"]);
            string token = Request["tempId"] ?? "";
            SecurityDocumentResult securityDocumentResult = SecurityDocumentHelper.ValidationDocumentToken(token);
            bool isValid = securityDocumentResult.IsValid;
            string email = securityDocumentResult.Email;
            string exceptionMessage = securityDocumentResult.ExceptionResult;
            tenant = securityDocumentResult.Tenant;

            if (isValid)
            {
                StiReport stiReport = new StiReport();
                byte[] result = ReadFileFromStorage(fileName, type);
                if (result != null)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    string ShowType = type == "PrintToPDF" ? "inline" : "attachment";
                    string contentType = "application/" + type == "PrintToPDF" ? "pdf" : "vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    string reportName = GetReportName(fileName) + (type == "PrintToPDF" ? ".pdf" : ".xlsx");
                    if (type != "ExcelOnly")
                    {
                        stiReport.LoadDocument(result);
                        if (type == "PrintToPDF") stiReport.ExportDocument(StiExportFormat.Pdf, memoryStream);
                        else if (type == "MicrosoftExce" || type == "MicrosoftExceAdvanced")
                        {
                            new StiExcel2007ExportService().ExportExcel(stiReport, memoryStream, new StiExcel2007ExportSettings() { UseOnePageHeaderAndFooter = useOnePageHeaderAndFooter, ExportDataOnly = exportDataOnly, ExportObjectFormatting = exportObjectFormatting });
                        }
                    }
                    else memoryStream = new MemoryStream(result);

                    if (memoryStream != null)
                    {
                        _DatainByte = memoryStream.ToArray();
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.AddHeader("Content-Length", _DatainByte.Length.ToString());

                        var browser = HttpContext.Current.Request.Browser;
                        HttpContext.Current.Response.ContentType = contentType;

                        if (browser != null && browser.Browser.Equals("ie", StringComparison.OrdinalIgnoreCase))
                        {

                            HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename*=UTF-8''" + HttpUtility.UrlPathEncode(reportName) + "\"");
                        }
                        else
                        {
                            HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename=\"" + HttpUtility.UrlPathEncode(reportName) + "\"");
                        }

                        HttpContext.Current.Response.BinaryWrite(_DatainByte);

                        if (HttpContext.Current.Response.IsClientConnected)
                        {
                            HttpContext.Current.Response.Flush();
                            HttpContext.Current.Response.Close();
                            HttpContext.Current.ApplicationInstance.CompleteRequest();

                        }

                    }

                 

                }

            }
            else
            {
                var message = exceptionMessage;
                if (string.IsNullOrEmpty(exceptionMessage)) message = "Sorry you’re not authenticated to view this document.";
                Response.Output.Write(message);

            }

        }

        private byte[] ReadFileFromStorage(string fileName, string type)
        {
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = fileName,
                FolderName = "others",
                Extension = type == "ExcelOnly" ? "xlsx" : "mdc",
                Tenant = (int)tenant,

            };
            byte[] result = storageservice.Read(fileInfo);
            return result;
        }

        private bool ToBoolean(string value)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(value)) value = value.ToLower();
            if (value == "true") result = true;

            return result;
        }


      public string   GetReportName(string fileName)
        {
            string result = fileName;
            if (!string.IsNullOrEmpty(fileName))
            {
                string[] Names = fileName.Split('@');
                if (Names.Count() > 1) result = Names[1];
            }

            return result;
        }

    }
}
