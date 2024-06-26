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
using System.Net.Http;
using System.Configuration;
using Newtonsoft.Json;

namespace WebFreight.Web.WebPages
{
    public partial class DawnLoadReportPage : System.Web.UI.Page
    {
        public byte[] _DatainByte;

        int? tenant = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            try
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
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = fileName + "mdc",
                        FolderName = "others",
                        Extension = "mdc",
                        Tenant = (int)tenant,

                    };

                    byte[] result = storageservice.Read(fileInfo);

                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug("step 1 get file from storage {0} file size {1}", null, fileName, result?.Length);

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        string[] Names = fileName.Split('@');
                        if (Names.Count() > 1) fileName = Names[1];

                    }


                    if (result != null)
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        string documentName = "";
                        string ShowType = "attachment";
                        string contentType = "";

                        var isAppServiceENV = Environment.GetEnvironmentVariable("IsAppService") == "true";
                        bool isAppService = ConfigurationManager.AppSettings["IsAppService"] == "true";

                        if ((isAppServiceENV || isAppService) && !string.IsNullOrEmpty(LogitudeSettings.LogitudeIISURL))
                        {
                            string URI = LogitudeSettings.LogitudeIISURL.TrimEnd('/') + "/api/Report/" + "GetMemoryStreamForPrintExcelOrPdf";

                            using (var client = new HttpClient())
                            {
                                var memoryStreamForExcelOrPdfRequest = new { Result = result, Type = type, UseOnePageHeaderAndFooter = useOnePageHeaderAndFooter, ExportDataOnly = exportDataOnly, ExportObjectFormatting = exportObjectFormatting };

                                string serializedObject = JsonConvert.SerializeObject(memoryStreamForExcelOrPdfRequest);
                                StringContent content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                                var result1 = client.PostAsync(URI, content);

                                result1.Wait();
                                if (result1.Result.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug("step 2 back from IISWR ");
                                    var memoryStream1 = result1.Result.Content.ReadAsAsync(typeof(MemoryStream)).Result;
                                    memoryStream = (MemoryStream)memoryStream1;

                                }

                            }

                            if (type == "PrintToPDF")
                            {
                                contentType = "application/" + "pdf";
                                documentName = fileName + ".pdf";
                                ShowType = "inline";


                            }
                            else if (type == "MicrosoftExce" || type == "MicrosoftExceAdvanced")
                            {
                                contentType = "application/" + "vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                documentName = fileName + ".xlsx";

                            }

                        }
                        else
                        {
                            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("step 2 this is not appService ");

                            stiReport.LoadDocument(result);

                            if (type == "PrintToPDF")
                            {

                                stiReport.ExportDocument(StiExportFormat.Pdf, memoryStream);
                                contentType = "application/" + "pdf";
                                documentName = fileName + ".pdf";
                                ShowType = "inline";


                            }
                            else if (type == "MicrosoftExce" || type == "MicrosoftExceAdvanced")
                            {

                                StiExcel2007ExportSettings setting = new StiExcel2007ExportSettings();
                                setting.UseOnePageHeaderAndFooter = useOnePageHeaderAndFooter;
                                setting.ExportDataOnly = exportDataOnly;
                                setting.ExportObjectFormatting = exportObjectFormatting;
                                StiExcel2007ExportService service = new StiExcel2007ExportService();
                                service.ExportExcel(stiReport, memoryStream, setting);
                                contentType = "application/" + "vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                documentName = fileName + ".xlsx";

                            }

                            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("step 3 StiExcel created file in ASPX ");
                        }


                        if (memoryStream != null)
                        {
                            _DatainByte = memoryStream.ToArray();

                            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("step 4 send file to client file sise {0}", null, _DatainByte.Length);

                            HttpContext.Current.Response.Clear();
                            HttpContext.Current.Response.AddHeader("Content-Length", _DatainByte.Length.ToString());

                            var browser = HttpContext.Current.Request.Browser;
                            HttpContext.Current.Response.ContentType = contentType;

                            if (browser != null && browser.Browser.Equals("ie", StringComparison.OrdinalIgnoreCase))
                            {

                                HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename*=UTF-8''" + HttpUtility.UrlPathEncode(documentName) + "\"");
                            }
                            else
                            {
                                HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename=\"" + HttpUtility.UrlPathEncode(documentName) + "\"");
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
                    //  throw new ApplicationException(message);
                }

            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, "step ex ASPX page failed to create file");

                Response.Output.Write(ex.Message);
            }

        }



        private bool ToBoolean(string value)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(value)) value = value.ToLower();
            if (value == "true") result = true;

            return result;
        }

    }
}
