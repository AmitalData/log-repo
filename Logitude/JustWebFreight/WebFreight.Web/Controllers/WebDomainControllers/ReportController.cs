using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using static WebFreight.Web.Helpers.ReportHelper;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class ReportController : ApiController
    {
        public HttpResponseMessage GetReportListsByGroupId(string groupId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                List<ReportList> result = new List<ReportList>();
                ReportRepository reportRepository = new ReportRepository(tenant);
                ReportQuery reportQuery = new ReportQuery(reportRepository);
                List<ReportList> reportLists = reportQuery.GetReportListsByGroupIdAndTenant(groupId, tenant).Where(d => d.Code == "CUPA" || !string.IsNullOrEmpty(d.DefaultTemplateId) || !string.IsNullOrEmpty(d.DefaultExcelTemplateId)).OrderBy(d => d.Name).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, reportLists);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetReportByCode(string code)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                List<ReportList> result = new List<ReportList>();
                ReportRepository reportRepository = new ReportRepository(authToken.Tenant);
                ReportQuery reportQuery = new ReportQuery(reportRepository);
                ReportList reportLists = reportQuery.GetReportByCode(code, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, reportLists);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage PutBuildStimulReport(ReportFliter reportFliter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ReportHelper reportHelper = new ReportHelper();
                reportHelper.ReportAuthentication(reportFliter, authToken.Tenant);
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = reportHelper.GetReportDateTimeFormat(reportFliter);
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern = "HH:mm";        
				if (string.IsNullOrEmpty(reportFliter.ReportKey) || reportFliter.ProcessType == "GenerateReport")
                {
                    if (reportFliter.ReportCode == "CUPA")
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, reportHelper.BuildCustomerPotentialActualDataProvider(reportFliter));
                    }

                    else
                    {
                        if (reportFliter.ReportsRunUsingWR)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, reportHelper.BuildReportDataViewWorkerRole(reportFliter));
                        }

                        else
                        {
                            string urlImage = reportHelper.BuildStimulReport(reportFliter);
                            BuildStimulReportResult myResult = GetBuildStimulReportResult(reportFliter, urlImage);

                            return Request.CreateResponse(HttpStatusCode.OK, myResult);
                        }
                    }
                }
                else
                {

					var isAppServiceENV = Environment.GetEnvironmentVariable("IsAppService") == "true";
					bool isAppService = ConfigurationManager.AppSettings["IsAppService"] == "true"; 
                    string urlImage = string.Empty;

					if ((isAppServiceENV || isAppService) && !string.IsNullOrEmpty(LogitudeSettings.LogitudeIISURL))
                    {
                        string URI = LogitudeSettings.LogitudeIISURL.TrimEnd('/') + "/api/Report/" + "GetSpecificPageFromStimulReportAsBase64";

                        using (var client = new HttpClient())
                        {
                            string serializedObject = JsonConvert.SerializeObject(reportFliter);
                            StringContent content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                            var result1 = client.PostAsync(URI, content);

                            result1.Wait();
                            if (result1.Result.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                
								  var jsonData = result1.Result.Content.ReadAsStringAsync().Result;
								var urlResponse = JsonConvert.DeserializeObject<UrlResponse>(jsonData.ToString());
								urlImage = urlResponse.url;
								reportFliter.PageCount = urlResponse.pageCount;
							}

                        }
                    }
                    else
                    {
						urlImage = reportHelper.GetSpecificPageFromStimulReportAsBase64(reportFliter);
					}
						 
                    if (string.IsNullOrEmpty(urlImage)) throw new Exception("Can't find file (" + reportFliter.ReportKey + "@" + reportFliter.ReportName + ")");
                    return Request.CreateResponse(HttpStatusCode.OK, GetBuildStimulReportResult(reportFliter, urlImage));
                }

            }

            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        //   public HttpResponseMessage GetPrepareSendReport(string type, string fileName, int tenant)
        //   {
        //       try
        //       {
        //           string token = HttpContext.Current.Request.Headers["Token"];
        //           AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //           SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
        //           SecurityUtility.AuthenticationOnTenant(tenant);

        //           string extension = "";
        //           MemoryStream memoryStream = new MemoryStream();
        //           Document document = null;

        //           StiReport stiReport = new StiReport();
        //           IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

        //           BlobFileInfo fileInfo = new BlobFileInfo()
        //           {
        //               FileName = (fileName + "mdc"),
        //               FolderName = "others",
        //               Extension = "mdc",
        //               Tenant = tenant,
        //           };

        //           byte[] result = storageservice.Read(fileInfo);

        //           if (!string.IsNullOrEmpty(fileName))
        //           {
        //               string[] Names = fileName.Split('@');
        //               if (Names.Length > 1) fileName = Names[1];
        //           }

        //           if (result != null)
        //           {

        //var isAppServiceENV = Environment.GetEnvironmentVariable("IsAppService") == "true";
        //bool isAppService = ConfigurationManager.AppSettings["IsAppService"] == "true";

        //if ((isAppServiceENV || isAppService) && !string.IsNullOrEmpty(LogitudeSettings.LogitudeIISURL))
        //{
        //	string URI = LogitudeSettings.LogitudeIISURL.TrimEnd('/') + "/api/Report/" + "GetMemoryStreamForExcelOrPdf";

        //	using (var client = new HttpClient())
        //	{
        //                       var memoryStreamForExcelOrPdfRequest = new { Result = result, Type = type };

        //		string serializedObject = JsonConvert.SerializeObject(memoryStreamForExcelOrPdfRequest);
        //		StringContent content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
        //		var result1 = client.PostAsync(URI, content);

        //		result1.Wait();
        //		if (result1.Result.StatusCode == System.Net.HttpStatusCode.OK)
        //		{

        //			var  memoryStream1 = result1.Result.Content.ReadAsAsync(typeof(MemoryStream)).Result;
        //                           memoryStream = (MemoryStream)memoryStream1;

        //		}

        //	}
        //                   extension = type == "Excel" ? "xlsx" : "pdf";
        //}
        //               else
        //               {


        //                   stiReport.LoadDocument(result);

        //	if (type == "Excel")
        //	{

        //		extension = "xlsx";
        //		StiExcel2007ExportSettings setting = new StiExcel2007ExportSettings();
        //		setting.UseOnePageHeaderAndFooter = true;
        //		StiExcel2007ExportService service = new StiExcel2007ExportService();
        //		service.ExportExcel(stiReport, memoryStream, setting);
        //	}

        //	else
        //	{
        //		stiReport.ExportDocument(StiExportFormat.Pdf, memoryStream);
        //		extension = "pdf";
        //	}
        //}
        //               if (memoryStream != null)
        //               {
        //                   byte[] ByteData = memoryStream.ToArray();

        //                   DocumentRepository documentRepository = new DocumentRepository(tenant);
        //                   document = new Document()
        //                   {
        //                       FileName = fileName,
        //                       CreateDate = DateTime.Now,
        //                       Extension = extension,
        //                       FileSize = ByteData.Length,
        //                       Tenant = tenant,
        //                       Id = IdCounter.GetNumber("Document", tenant),
        //                       HasFile = true,
        //                       Folder = "others",
        //                   };

        //                   documentRepository.Add(document);
        //                   documentRepository.SubmitChanges();

        //                   fileInfo = new BlobFileInfo()
        //                   {
        //                       FileName = document.Id,
        //                       FolderName = "others",
        //                       Extension = document.Extension,
        //                       Tenant = tenant,
        //                       FileSize = document.FileSize,

        //                   };

        //                   storageservice.Write(ByteData, fileInfo);
        //               }
        //           }

        //           return Request.CreateResponse(HttpStatusCode.OK, document);
        //       }

        //       catch (Exception ex)
        //       {
        //           return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //       }
        //   }

        public HttpResponseMessage GetPrepareSendReport(string type, string fileName, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                string extension = "";
                MemoryStream memoryStream = new MemoryStream();
                Document document = null;

                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;


                var isAppServiceENV = Environment.GetEnvironmentVariable("IsAppService") == "true";
                bool isAppService = ConfigurationManager.AppSettings["IsAppService"] == "true";

                if ((isAppServiceENV || isAppService) && !string.IsNullOrEmpty(LogitudeSettings.LogitudeIISURL))
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug("step 1 get report for send- this is app service sent to IISWR ");

                    string URI = LogitudeSettings.LogitudeIISURL.TrimEnd('/') + "/api/Report/" + "GetMemoryStreamFromStorageForExcelOrPdf";

                    using (var client = new HttpClient())
                    {
                        var memoryStreamForExcelOrPdfRequest = new { fileName = fileName, Tenant = tenant, Type = type };

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
                        else
                        {
                            throw new Exception("get report for send failed to get file from IISWR");
                        }

                    }

                }
                else
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug("step 1 this is WR not appservice");


                    StiReport stiReport = new StiReport();

                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = (fileName + "mdc"),
                        FolderName = "others",
                        Extension = "mdc",
                        Tenant = tenant,
                    };

                    byte[] result = storageservice.Read(fileInfo);

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        string[] Names = fileName.Split('@');
                        if (Names.Length > 1) fileName = Names[1];
                    }

                    if (result == null || result.Length == 0) throw new Exception(string.Format("get report for send in WR to get file from storage. file name {0}", fileName));

                    stiReport.LoadDocument(result);

                    if (type == "Excel")
                    {

                        extension = "xlsx";
                        StiExcel2007ExportSettings setting = new StiExcel2007ExportSettings();
                        setting.UseOnePageHeaderAndFooter = true;
                        StiExcel2007ExportService service = new StiExcel2007ExportService();
                        service.ExportExcel(stiReport, memoryStream, setting);
                    }

                    else
                    {
                        stiReport.ExportDocument(StiExportFormat.Pdf, memoryStream);
                        extension = "pdf";
                    }
                }


                if (memoryStream != null)
                {
                    extension = type == "Excel" ? "xlsx" : "pdf";

                    byte[] ByteData = memoryStream.ToArray();

                    DocumentRepository documentRepository = new DocumentRepository(tenant);
                    document = new Document()
                    {
                        FileName = fileName,
                        CreateDate = DateTime.Now,
                        Extension = extension,
                        FileSize = ByteData.Length,
                        Tenant = tenant,
                        Id = IdCounter.GetNumber("Document", tenant),
                        HasFile = true,
                        Folder = "others",
                    };

                    documentRepository.Add(document);
                    documentRepository.SubmitChanges();

                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = "others",
                        Extension = document.Extension,
                        Tenant = tenant,
                        FileSize = document.FileSize,

                    };

                    storageservice.Write(ByteData, fileInfo);
                }


                return Request.CreateResponse(HttpStatusCode.OK, document);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
      
       
        public HttpResponseMessage GetCheckIfStimulSoftReportIsBliud(string reportKey, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                ReportExecutionLogRepository reportExecutionLogRepository = new ReportExecutionLogRepository(tenant);
                ReportExecutionLog reportExecutionLog = reportExecutionLogRepository.GetReportExecutionLog(reportKey, tenant);
                ReportBuildResult result = new ReportBuildResult();
                if (reportExecutionLog != null)
                {
                    //  UpdateStatusReportExecutionLog(authToken, reportExecutionLogRepository, reportExecutionLog);

                    result.StatusCode = reportExecutionLog.StatusCode;
                    if (result.StatusCode == "F")
                    {
                        result.HasError = true;
                        result.ExceptionMessage = GetUnderStandableMessageFromMessageException(reportExecutionLog.ExceptionMessage);
                    }

                }
                else
                {
                    result.HasError = true;
                    result.ExceptionMessage = "Report execution log not found";
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCheckIfReportsRunUsingWR()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                SettingRepository settingRepository = new SettingRepository();
                bool result = settingRepository.GetCheckIfReportsRunUsingWR("1");


                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetExcel(string reportKey,string reportName)
        {
            try
            {
				string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                MemoryStream res = new ReportHelper().GetExcel(reportKey, reportName, authToken.Tenant);
                if (res == null || res.Length == 0)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                res.Position = 0;
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(res.ToArray())
                };
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = $"{reportKey}@{reportName}.xlsx" };

                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private static BuildStimulReportResult GetBuildStimulReportResult(ReportFliter reportFliter, string urlImage)
        {
            BuildStimulReportResult resultBuildStimulReportArgs = new BuildStimulReportResult();
            resultBuildStimulReportArgs.StimulImageBase64 = urlImage;
            resultBuildStimulReportArgs.PageCount = reportFliter.PageCount;
            resultBuildStimulReportArgs.ReportKey = reportFliter.ReportKey;
            return resultBuildStimulReportArgs;
        }
        private BlobFileInfo GetNewBlobFileInfo(string fileName, int tenant)
        {
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = fileName,
                FolderName = "others",
                Extension = "mdc",
                Tenant = tenant,

            };
            return fileInfo;
        }
        private string GetUnderStandableMessageFromMessageException(string exceptionMessage)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(exceptionMessage))
            {
                string[] lines = exceptionMessage.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                result = lines[0];
            }
            return result;
        }
        private static void UpdateStatusReportExecutionLog(AuthenticationToken authToken, ReportExecutionLogRepository reportExecutionLogRepository, ReportExecutionLog reportExecutionLog)
        {
            if (reportExecutionLog.StatusCode == "W")
            {
                var nowDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                if (reportExecutionLog.CreateDate.AddMinutes(10) < nowDate)
                {
                    reportExecutionLog.StatusCode = "F";
                    reportExecutionLog.ExceptionMessage = "the request has timed out";
                    reportExecutionLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                    //reportExecutionLogRepository.Update(reportExecutionLog);
                    //reportExecutionLogRepository.SubmitChanges();
                }
            }
        }

        [HttpPost]
		public HttpResponseMessage GetSpecificPageFromStimulReportAsBase64(ReportFliter reportFliter)
		{
			try
			{
				
				ReportHelper reportHelper = new ReportHelper();
				UrlResponse urlResponse =new UrlResponse();
				string url = "";
				string extension = "tiff"; //IsUsingFileStreamAndTiffImage(reportFliter.tenant) ? "tiff" : "mdc";
				IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
				BlobFileInfo fileInfo = reportHelper.GetNewBlobFileInfo((reportFliter.ReportKey + "@" + reportFliter.ReportName + extension), extension, reportFliter.tenant);
				byte[] result = storageservice.Read(fileInfo);
				if (result != null)
				{
					urlResponse.url = reportHelper.GetSpecificPageFromTiffImageAsBase64(reportFliter, ref result);
					urlResponse.pageCount = reportFliter.PageCount;

				}

				storageservice.Dispose();


				return Request.CreateResponse(HttpStatusCode.OK, urlResponse);

			}

			catch (Exception ex)
			{

				return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
			}
		}

		[HttpPost]
		public HttpResponseMessage GetMemoryStreamForExcelOrPdf(MemoryStreamForExcelOrPdfRequest memoryStreamForExcelOrPdfRequest)
		{
			try
			{
				StiReport stiReport = new StiReport();
				MemoryStream memoryStream = new MemoryStream();


				stiReport.LoadDocument(memoryStreamForExcelOrPdfRequest.Result);

				if (memoryStreamForExcelOrPdfRequest.Type == "Excel")
				{

					
					StiExcel2007ExportSettings setting = new StiExcel2007ExportSettings();
					setting.UseOnePageHeaderAndFooter = true;
					StiExcel2007ExportService service = new StiExcel2007ExportService();
					service.ExportExcel(stiReport, memoryStream, setting);
				}

				else
				{
					stiReport.ExportDocument(StiExportFormat.Pdf, memoryStream);
					
				}


				return Request.CreateResponse(HttpStatusCode.OK, memoryStream);

			}

			catch (Exception ex)
			{

				return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
			}
		}

        [HttpPost]
        public HttpResponseMessage GetMemoryStreamFromStorageForExcelOrPdf(MemoryStreamForExcelOrPdfRequest memoryStreamForExcelOrPdfRequest)
        {
            try
            {
                StiReport stiReport = new StiReport();
                MemoryStream memoryStream = new MemoryStream();

                string fileName = memoryStreamForExcelOrPdfRequest.FileName;

                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = (fileName + "mdc"),
                    FolderName = "others",
                    Extension = "mdc",
                    Tenant = memoryStreamForExcelOrPdfRequest.Tenant ,
                };

                byte[] result = storageservice.Read(fileInfo);

                if (!string.IsNullOrEmpty(fileName))
                {
                    string[] Names = fileName.Split('@');
                    if (Names.Length > 1) fileName = Names[1];
                }
                 
                if (result == null || result.Length == 0) throw new Exception(string.Format("Get report to send in IISWR failed to get file from storage. file name {0}", fileName));
              
                stiReport.LoadDocument(result);

                if (memoryStreamForExcelOrPdfRequest.Type == "Excel")
                {

                    StiExcel2007ExportSettings setting = new StiExcel2007ExportSettings();
                    setting.UseOnePageHeaderAndFooter = true;
                    StiExcel2007ExportService service = new StiExcel2007ExportService();
                    service.ExportExcel(stiReport, memoryStream, setting);
                }

                else
                {
                    stiReport.ExportDocument(StiExportFormat.Pdf, memoryStream);

                }


                return Request.CreateResponse(HttpStatusCode.OK, memoryStream);

            }

            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, "Get report for send in IISWR failed to create file");

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
		public HttpResponseMessage GetMemoryStreamForPrintExcelOrPdf(MemoryStreamForPrintExcelOrPdfRequest memoryStreamForPrintExcelOrPdfRequest)
		{
			try
			{
				StiReport stiReport = new StiReport();
				MemoryStream memoryStream = new MemoryStream();

                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("step 2 this appService working om IISWR lenth of bytes {0} ", null, memoryStreamForPrintExcelOrPdfRequest?.Result?.Length);

                stiReport.LoadDocument(memoryStreamForPrintExcelOrPdfRequest.Result);

                if (memoryStreamForPrintExcelOrPdfRequest.Type == "PrintToPDF")
				{

					stiReport.ExportDocument(StiExportFormat.Pdf, memoryStream);

				}
				else if (memoryStreamForPrintExcelOrPdfRequest.Type == "MicrosoftExce" || memoryStreamForPrintExcelOrPdfRequest.Type == "MicrosoftExceAdvanced")
				{

					StiExcel2007ExportSettings setting = new StiExcel2007ExportSettings();
					setting.UseOnePageHeaderAndFooter = memoryStreamForPrintExcelOrPdfRequest.UseOnePageHeaderAndFooter;
					setting.ExportDataOnly = memoryStreamForPrintExcelOrPdfRequest.ExportDataOnly;
					setting.ExportObjectFormatting = memoryStreamForPrintExcelOrPdfRequest.ExportObjectFormatting;
					StiExcel2007ExportService service = new StiExcel2007ExportService();
					service.ExportExcel(stiReport, memoryStream, setting);
					

				}

                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("step 3 StiExcel created file in IISWR ");


                return Request.CreateResponse(HttpStatusCode.OK, memoryStream);

			}

			catch (Exception ex)
			{
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, "step ex IISWR failed to create file");

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
			}
		}

        [HttpPost]
        public HttpResponseMessage GetMemoryStreamFromStorageForPrintExcelOrPdf(MemoryStreamForPrintExcelOrPdfRequest memoryStreamForPrintExcelOrPdfRequest)
        {
            try
            {

                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

                string fileName = memoryStreamForPrintExcelOrPdfRequest.FileName;

                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = fileName + "mdc",
                    FolderName = "others",
                    Extension = "mdc",
                    Tenant = (int)memoryStreamForPrintExcelOrPdfRequest.Tenant,

                };

                byte[] result = storageservice.Read(fileInfo);

                if (result == null || result.Length == 0) throw new Exception(string.Format("Dwonload page faild to get file from storage. file name {0}", fileName));

                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("step 1 this is from appService work on IISWR  get file from storage {0} file size {1}", null, fileName, result?.Length);

                if (!string.IsNullOrEmpty(fileName))
                {
                    string[] Names = fileName.Split('@');
                    if (Names.Count() > 1) fileName = Names[1];

                }

                StiReport stiReport = new StiReport();
                MemoryStream memoryStream = new MemoryStream();

                stiReport.LoadDocument(result);

                if (memoryStreamForPrintExcelOrPdfRequest.Type == "PrintToPDF")
                {

                    stiReport.ExportDocument(StiExportFormat.Pdf, memoryStream);

                }
                else if (memoryStreamForPrintExcelOrPdfRequest.Type == "MicrosoftExce" || memoryStreamForPrintExcelOrPdfRequest.Type == "MicrosoftExceAdvanced")
                {

                    StiExcel2007ExportSettings setting = new StiExcel2007ExportSettings();
                    setting.UseOnePageHeaderAndFooter = memoryStreamForPrintExcelOrPdfRequest.UseOnePageHeaderAndFooter;
                    setting.ExportDataOnly = memoryStreamForPrintExcelOrPdfRequest.ExportDataOnly;
                    setting.ExportObjectFormatting = memoryStreamForPrintExcelOrPdfRequest.ExportObjectFormatting;
                    StiExcel2007ExportService service = new StiExcel2007ExportService();
                    service.ExportExcel(stiReport, memoryStream, setting);

                }

                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("step 3 StiExcel created file in IISWR ");


                return Request.CreateResponse(HttpStatusCode.OK, memoryStream);

            }

            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, "step ex IISWR failed to create file");

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetDataProviderProperties(string code)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ReportHelper reportHelper = new ReportHelper();
                List<ISlvLeaf> dataProviderJson = reportHelper.BuildDataProviderJson(code);

                return Request.CreateResponse(HttpStatusCode.OK, dataProviderJson);



            }

            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetPowerBIReports()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                PowerBIReportHelper reportHelper = new PowerBIReportHelper(authToken.Tenant);
                var results = reportHelper.GetReports();

                var response = new
                {
                    reportHelper.ActiveDirectoryTenantId,
                    Reports = results
                };

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }

	public class ReportBuildResult
    {
        public string ExceptionMessage { get; set; }
        public bool HasError { get; set; }
        public string StatusCode { get; set; }
    }
	public class UrlResponse
	{
		public string url { get; set; }
		public int pageCount { get; set; }
		
	}
	public class MemoryStreamForExcelOrPdfRequest
	{
		public byte[] Result { get; set; }
		public string Type { get; set; }

        public string FileName { get; set; }

        public int Tenant { get; set; }

    }
	public class MemoryStreamForPrintExcelOrPdfRequest
	{
		public byte[] Result { get; set; }
		public string Type { get; set; }
		public bool UseOnePageHeaderAndFooter { get; set; }
		public bool ExportDataOnly { get; set; }
		public bool ExportObjectFormatting { get; set; }

        public string FileName { get; set; }

        public int Tenant { get; set; }

    }
}