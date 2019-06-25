using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.DataContracts;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Export;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.DataProviders;
using WebFreight.Web.Helpers;
using WebFreight.Web.ReportsWebServices;
using WebFreight.Web.ReportsWebServices.LogitudeReports;
using WebFreight.Web.Security;
using WebFreight.Web.ShipmentPackageModel;
using WebFreight.Web.TaxesApprovalModel;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class ReportController : ApiController
    {

        public HttpResponseMessage GetReportListsByGroupId(string groupId, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                List<ReportList> result = new List<ReportList>();
                ReportRepository reportRepository = new ReportRepository(tenant);
                ReportQuery reportQuery = new ReportQuery(reportRepository);
                List<ReportList> reportLists = reportQuery.GetReportListsByGroupIdAndTenant(groupId, tenant).Where(d => d.Code == "CUPA" || !string.IsNullOrEmpty(d.DefaultTemplateId)).OrderBy(d => d.Name).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, reportLists);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        
        public HttpResponseMessage PutReportData(ReportFliter reportFliter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                if(authToken.Tenant!= reportFliter.tenant)
                {
                    throw new Exception("Sorry you’re not authenticated to view this report");
                }


                if (!string.IsNullOrEmpty(reportFliter.UserId))
                {
                    UserQuery userQuery = new UserQuery(authToken.Tenant);
                    bool isExist = userQuery.CheckIfUserExistInTenant(reportFliter.UserId, authToken.Tenant);
                    if (!isExist)
                    {
                        throw new Exception("Sorry you’re not authenticated to view this report");
                    }
                }


                ICommonDataContext context = CommonDataContext.GetContext(reportFliter.tenant);
                CommonDataDomainService commonService = new CommonDataDomainService();
                Tenant currentTenant = context.Tenants.Where(t => t.Id == reportFliter.tenant).FirstOrDefault();
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                string datetimeformat = @"dd\/MM\/yyyy";

                if (!string.IsNullOrEmpty(currentTenant.DateTimeFormat))
                {
                    datetimeformat = currentTenant.DateTimeFormat;
                }

                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = datetimeformat;
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern = "HH:mm";

                string urlImage = "";
                CustomerPotentialActualDataProvider myData = null;


                if (string.IsNullOrEmpty(reportFliter.ReportKey) || reportFliter.ProcessType == "GenerateReport")
                {

                    if (reportFliter.ReportCode == "CUPA" || !reportFliter.ReportsRunUsingWR)
                    {
                        ReportHelper reportHelper = new ReportHelper();
                        byte[] filters = reportHelper.GetReportFilters(reportFliter.QueryFilterItemLists);

                        LogitudeReportsWebService logitudeReportsWebService = new LogitudeReportsWebService();

                        byte[] dataProvider = reportHelper.BuildReportDataProvider(reportFliter, filters);

                   
                        if (dataProvider == null)
                        {
                            throw new Exception("Data Provider is missing");
                        }

                        else
                        {
                            if (reportFliter.ReportCode == "CUPA")
                            {
                                MemoryStream memorystream = new MemoryStream(dataProvider);
                                XmlSerializer serializer = new XmlSerializer(typeof(CustomerPotentialActualDataProvider));
                                myData = (CustomerPotentialActualDataProvider)serializer.Deserialize(memorystream);
                            }

                            else
                            {
                                ReportsTemplatesWebService reportsTemplatesWebService = new ReportsTemplatesWebService();
                                ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(reportFliter.tenant);
                                string reportDocumentId = reportsTemplatesVersionRepository.GetReportDocumentIdByReportTemplateId(reportFliter.DefaultTemplateId, reportFliter.tenant);
                                byte[] template = reportsTemplatesWebService.GetReportTemplate(reportDocumentId, reportFliter.tenant, false);

                                if (template == null)
                                {
                                    throw new Exception("Report Template is missing");
                                }

                                else
                                {
                                    urlImage = reportHelper.GetReportStimulsoftViewer(dataProvider, template,  reportFliter);
                                }
                            }
                        }
                    }
                    else
                    {
                        return BuildReportData(reportFliter);
                    }

                }
                else
                {
                    urlImage = GetReportAsImageFromStorage(reportFliter);
                }

                if (reportFliter.ReportCode == "CUPA")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, myData);
                }

                else
                {
                    if (string.IsNullOrEmpty(urlImage))
                    {
                        throw new Exception("Can't find file (" + reportFliter.ReportKey +  "@" +reportFliter.ReportName+ ")");
                    }

                    List<EditableFieldPosition> editableFieldPositionList = new List<EditableFieldPosition>();
                    editableFieldPositionList.Add(new EditableFieldPosition() {NumberOfRequest = reportFliter.NumberOfRequests,  FieldName =  "Image", FieldValue = urlImage, PageCount = reportFliter.PageCount, ReportKey = reportFliter.ReportKey });
                    return Request.CreateResponse(HttpStatusCode.OK, editableFieldPositionList);
                }
            }

            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetPrepareSendReport(string type , string fileName , int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                
                string extension = "";
                MemoryStream memoryStream = new MemoryStream();
                Document document = null;

                StiReport stiReport = new StiReport();
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = fileName,
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

                if (result != null)
                {
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

                    if (memoryStream != null)
                    {
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

                        fileInfo = new BlobFileInfo()
                        {
                            FileName = document.Id,
                            FolderName = "others",
                            Extension = document.Extension,
                            Tenant = tenant,
                            FileSize = document.FileSize,

                        };

                        storageservice.Write(ByteData, fileInfo);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, document);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
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


        private string GetReportAsImageFromStorage(ReportFliter reportFliter)
        {

            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = GetNewBlobFileInfo(reportFliter.ReportKey + "@" + reportFliter.ReportName, reportFliter.tenant);
            byte[] result = storageservice.Read(fileInfo);
           string url = "";
           if (result != null)
           {
               StiReport stiReport = new StiReport();
               stiReport.LoadDocument(result);

                ReportHelper reportHelper = new ReportHelper();
                url = reportHelper.ExportStimulaImage(stiReport, reportFliter);

           }
          
          return url;
        }


        private HttpResponseMessage BuildReportData(ReportFliter reportFliter)
        {
            try
            {
                
                ReportExecutionLogRepository reportExecutionLogRepository = new ReportExecutionLogRepository(reportFliter.tenant);
                reportFliter.ReportKey = Guid.NewGuid().ToString();

                ReportExecutionLog reportExecutionLog = new ReportExecutionLog()
                {
                    Id = reportFliter.ReportKey,
                    CreateDate = DateTime.Now,
                    CreatedByUserId = reportFliter.UserId,
                    ReportFilterXML = LogitudeXmlSerializer.SerializeObjectToXmlString(reportFliter) ,
                    Tenant = reportFliter.tenant,
                    StatusCode = "W",
                    ReportId = reportFliter.ReportId,
                    ReportTemplateId = reportFliter.DefaultTemplateId,
                  
                };

                reportExecutionLogRepository.Add(reportExecutionLog);
                reportExecutionLogRepository.SubmitChanges();
                
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("ReportExecutionLogQueue", reportExecutionLog.Tenant);
                queueservice.Send(new Dictionary<string, string>() { { "ReportExecutionLogId", reportExecutionLog.Id }, { "Tenant", reportExecutionLog.Tenant.ToString() } }, null, null, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, reportFliter);
           
            }
           catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetCheckIfStimualReportIsBuilt(string reportKey, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ReportExecutionLogRepository reportExecutionLogRepository = new ReportExecutionLogRepository(tenant);
                ReportExecutionLog reportExecutionLog = reportExecutionLogRepository.GetSingleReportExecutionLog(reportKey, tenant);
                ReportBuildResult result = new ReportBuildResult();
                if (reportExecutionLog != null)
                {
                    result.StatusCode = reportExecutionLog.StatusCode;
                    if (result.StatusCode == "F")
                    {
                        result.HasError = true;
                        result.ExceptionMessage = reportExecutionLog.ExceptionMessage;
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
        

    }



    public class ReportBuildResult
    {
        public string ExceptionMessage { get; set; }
        public bool HasError { get; set; }
        public string StatusCode { get; set; }
    }
}