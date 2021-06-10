using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class TermsofUseController : ApiController
        // private label id
    {
        public HttpResponseMessage GetCheckIfGoToTermUseComponent(int tenant, string userId)
        {
            try
            {
                TermsofUseArgs result = new TermsofUseArgs();

                if (!string.IsNullOrEmpty(userId))
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    TermsofUseQuery termsofUseQuery = new TermsofUseQuery(tenant);

                    TermsofUseSignatureQuery termsofUseSignatureQuery = new TermsofUseSignatureQuery(tenant);

                    TenantPM tenantPM = TenantQuery.GetSingleTenantPM(tenant, false);
                    TermsofUsePM termofuse = new TermsofUsePM();
                    termofuse = termsofUseQuery.GetTermOfUseByPrivateLabel(termofuse.PrivateLabelId);
                    if (!string.IsNullOrEmpty(tenantPM.PrivateLabelId) && termofuse == null)
                    {
                        throw new Exception("You are unable to login without approving the terms of use, please contact your administrator!");
                    }

                    if (termofuse == null) termofuse = termsofUseQuery.GetTermsofUseDefault();

                    if (termofuse == null) result.IsTermOfUse = false;
                    else
                    {

                        result.VersionNumber = termofuse.VersionNumber;
                        result.Id = termofuse.Id;
                        result.VersionDocumentId = termofuse.VersionDocumentId;
                        result.PrivateLabelId = termofuse.PrivateLabelId;

                        TermsofUseSignaturePM termsofUseSignaturePM = termsofUseSignatureQuery.GetByIdAndContactId(termofuse.Id, userId, authToken.Tenant);

                        if (termsofUseSignaturePM != null)
                        {
                            result.IsTermOfUse = false;
                        }
                        else
                        {
                            if (LogitudeSettings.DeploymentStage == "logboxwe1")
                            {
                                if (string.IsNullOrEmpty(tenantPM.PrivateLabelId))
                                {
                                    result.IsTermOfUse = false;
                                }
                                else
                                {
                                    result.IsTermOfUse = true;
                                }
                            }
                            else
                            {
                                result.IsTermOfUse = true;
                            }

                        }
                    }

                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetTenantTermsofUse(string privateLabeldId)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                TermsofUseQuery termsofUseQuery = new TermsofUseQuery(authToken.Tenant);  
                List<TermsofUsePM> TermsofUsePMLists = termsofUseQuery.GetByPrivateLabeldId(privateLabeldId).ToList();


                return Request.CreateResponse(HttpStatusCode.OK, TermsofUsePMLists);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSingle(int Id, int tenant)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                TermsofUseQuery termsofUseQuery = new TermsofUseQuery(tenant);
                TermsofUsePM termsofUsePM = termsofUseQuery.GetSingleById(Id);


                return Request.CreateResponse(HttpStatusCode.OK, termsofUsePM);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage Post(TermsofUsePM termsofUsePM)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                //int tenant = authToken.Tenant;
                int tenant = termsofUsePM.Tenant;
                if (termsofUsePM.FileData == null)
                {
                    termsofUsePM.FileData = new byte[0];

                }

                string extension = "pdf";

                ReportHelper reportHelper = new ReportHelper();
                DocumentFile documentFile = new DocumentFile() { FileName = termsofUsePM.VersionDocumentName, FileData = termsofUsePM.FileData, Extension = extension, Folder = "termsOfUse", Tenant = tenant };
                Document newDocument = reportHelper.CreateDocumentAndWriteOnStorage(documentFile);

                termsofUsePM.VersionDocumentId = newDocument.Id;

                ICommonDataContext MyContext = CommonDataContext.GetContext(termsofUsePM.Tenant);
                TermsofUseService service = new TermsofUseService(MyContext, termsofUsePM.Tenant);
                service.Create(termsofUsePM);

                return Request.CreateResponse(HttpStatusCode.OK, termsofUsePM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}