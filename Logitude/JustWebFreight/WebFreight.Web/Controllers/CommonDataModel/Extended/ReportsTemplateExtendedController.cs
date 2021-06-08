using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class ReportsTemplateExtendedController : ApiController
    {
        public HttpResponseMessage GetReportsTemplateListsByReportId(string reportId, string reportType = null)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("ReportsTemplate", "READ", authToken.Tenant);


                int tenant = authToken.Tenant;

                ReportsTemplateQuery reportsTemplateQuery = new ReportsTemplateQuery(tenant);
                List<ReportsTemplateList> myResult = reportsTemplateQuery.GetReportsTemplateListsByReportId(reportId, tenant, reportType).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



      

        public HttpResponseMessage GetReportsTemplatePMsByReportId(string reportId , string reportType = null)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                SecurityUtility.CheckContactFeature("ReportsTemplate", "READ", authToken.Tenant);
                int tenant = authToken.Tenant;
                ReportsTemplateQuery reportsTemplateQuery = new ReportsTemplateQuery(tenant);
                List<ReportsTemplatePM> myResult = reportsTemplateQuery.GetReportsTemplatePMsByReportId(reportId, tenant, reportType);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCopyReportsTemplateByReportsTemplateId(string reportsTemplateId, string userId)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("ReportsTemplate", "NEW", authToken.Tenant);

                int tenant = authToken.Tenant;
                ReportsTemplatePM result = null;
                ReportHelper reportHelper = new ReportHelper();
                ReportsTemplate reportsTemplate = reportHelper.CopyReportsTemplate(reportsTemplateId, userId, tenant);
                if (reportsTemplate != null)
                {
                    ReportsTemplateQuery reportsTemplateQuery = new ReportsTemplateQuery(tenant);
                    result = reportsTemplateQuery.GetSinglePM(reportsTemplate.Id, tenant);
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostReportTemplate(ReportsTemplatePM reportsTemplatePM)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;
                SecurityUtility.CheckContactFeature("ReportsTemplate", "NEW", authToken.Tenant);
                if (reportsTemplatePM.TemplateData == null)
                {
                    if (reportsTemplatePM.TemplateType == "R")
                    {
                        StiReport report = new StiReport();
                        reportsTemplatePM.TemplateData = report.SaveToByteArray();
                    }
                    else
                    {
                        reportsTemplatePM.TemplateData = new byte[0];
                    }
                }

                string extension = reportsTemplatePM.TemplateType == "R" ? "mrt" : "html";

                ReportHelper reportHelper = new ReportHelper();
                DocumentFile documentFile = new DocumentFile() { FileName = reportsTemplatePM.Description, FileData = reportsTemplatePM.TemplateData, Extension = extension, Folder = "reports", Tenant = tenant };
                Document newDocument = reportHelper.CreateDocumentAndWriteOnStorage(documentFile);
                 
                ReportsTemplateRepository reportsTemplateRepository = new ReportsTemplateRepository(tenant);
                ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(tenant);
                string reportTemplateId=  reportHelper.AddReportTemplate(reportsTemplatePM.ReportId, reportsTemplatePM.Description, reportsTemplatePM.CreatedByUserId, newDocument.Id, tenant, reportsTemplateRepository, reportsTemplatesVersionRepository, null, reportsTemplatePM.IsSystem, reportsTemplatePM.TemplateType);

                reportsTemplateRepository.SubmitChanges();
                reportsTemplatesVersionRepository.SubmitChanges();
            

                ReportsTemplateQuery reportsTemplateQuery = new ReportsTemplateQuery(tenant);
                ReportsTemplatePM result = reportsTemplateQuery.GetSinglePM(reportTemplateId,tenant);

                if(result.TemplateType == "M")
                {
                    result.TemplateData = reportsTemplatePM.TemplateData;
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetMessageReportsTemplateBodyByReportTemplateIdAndVersion(string reportsTemplateId, int version)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;
                byte[] fileData = null;
                SecurityUtility.CheckContactFeature("ReportsTemplate", "READ", authToken.Tenant);
                ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(tenant);
                string documentId = reportsTemplatesVersionRepository.GetReportDocumentIdByReportTemplateIdAndVersion(reportsTemplateId, version, tenant);
                if (!string.IsNullOrEmpty(documentId))
                {
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = documentId,
                        FolderName = "reports",
                        Extension = "html",
                        Tenant = tenant,

                    };

                  fileData = storageservice.Read(fileInfo);
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, fileData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutSaveReportTemplateMessageBody(ReportsTemplatePM reportsTemplatePM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;
                SecurityUtility.CheckContactFeature("ReportsTemplate", "UPDATE", authToken.Tenant);
                if (reportsTemplatePM.TemplateData != null)
                {

                    ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(tenant);
                    string documentId = reportsTemplatesVersionRepository.GetReportDocumentIdByReportTemplateIdAndVersion(reportsTemplatePM.Id, reportsTemplatePM.CurrentVersion, tenant);
                    if (!string.IsNullOrEmpty(documentId))
                    {
                        IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                        BlobFileInfo fileInfo = new BlobFileInfo()
                        {
                            FileName = documentId,
                            FolderName = "reports",
                            Extension = "html",
                            Tenant = tenant,
                            FileSize = reportsTemplatePM.TemplateData.Length,
                        };

                        storageservice.Write(reportsTemplatePM.TemplateData, fileInfo);


                        reportsTemplatePM.TemplateData = null;
                        ICommonDataContext MyContext = CommonDataContext.GetContext(reportsTemplatePM.Tenant);
                        ReportsTemplateService service = new ReportsTemplateService(MyContext, reportsTemplatePM.Tenant);

                        service.Update(reportsTemplatePM);

                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, reportsTemplatePM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetReportTemplateEditorHtmlData(string reportsTemplateId,  int version , string userId, string subject,  string from = null, string replyTo = null, string cc = null)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("ReportsTemplate", "READ", authToken.Tenant);
                string htmlstring = "";
                int tenant = authToken.Tenant;
                byte[] fileData = null;
                SendHtmlFilter reslutFilter = new SendHtmlFilter();
                ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(tenant);
                string documentId = reportsTemplatesVersionRepository.GetReportDocumentIdByReportTemplateIdAndVersion(reportsTemplateId, version, tenant);

                if (!string.IsNullOrEmpty(documentId))
                {
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = documentId,
                        FolderName = "reports",
                        Extension = "html",
                        Tenant = tenant,

                    };

                    fileData = storageservice.Read(fileInfo);
                    string html = string.Empty;
                    if (fileData != null)
                    {
                        html = System.Text.Encoding.UTF8.GetString(fileData);
                    }


                    HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
                    reslutFilter.Htmlstring = htmlEditorHelper.ResolveSystemDataHtml(html, userId, ref subject, ref from, ref replyTo, ref cc, tenant);

                }
          
                reslutFilter.Subject = subject;
                reslutFilter.From = from;
                reslutFilter.ReplyTo = replyTo;
                reslutFilter.Cc = cc;

                return Request.CreateResponse(HttpStatusCode.OK, reslutFilter);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        

    }
}