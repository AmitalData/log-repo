using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class ReportsDomainController : ApiController
    {
        public HttpResponseMessage GetIQueryableEntityList(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                PartnersDomainService service = new PartnersDomainService();
              IQueryable<ParticipantList>  myResult = service.GetParticipantLists(tenant);
                  return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetBusinessUnitLists(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                CommonDataDomainService service = new CommonDataDomainService();
                IQueryable<BusinessUnitList> myResult = service.GetBusinessUnitLists(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetAdditionalServicesByTenant(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                CommonDataDomainService service = new CommonDataDomainService();
                IQueryable<AdditionalServicePM> myResult = service.GetAdditionalServicesByTenant(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetProductTypesByTenant(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                CommonDataDomainService service = new CommonDataDomainService();
                IQueryable<ProductTypeList> myResult = service.GetProductTypeLists(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetLeadSourceLists(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                CommonDataDomainService service = new CommonDataDomainService();
                IQueryable<LeadSourceList> myResult = service.GetLeadSourceLists(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutUploadStaticFile(ImageParameter fileUploadParamerter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(fileUploadParamerter.Tenant);

                string documentId = "";
                if (fileUploadParamerter!=null && !string.IsNullOrEmpty(fileUploadParamerter.Base64String))
                {
                    byte[] data = Convert.FromBase64String(fileUploadParamerter.Base64String);

                    ICommonDataContext objectContext = CommonDataContext.GetContext(fileUploadParamerter.Tenant);
                    ReportService reportService = new ReportService(objectContext, fileUploadParamerter.Tenant);

                    documentId = reportService.CreateDocumentForReport(fileUploadParamerter.EntityId, fileUploadParamerter.Tenant, data.Length);

                     if (!string.IsNullOrEmpty(documentId))
                     {
                         string fileName = "reports/" + documentId + ".mrt";

                         Uploader service = new Uploader();
                         var myResult = service.UploadStaticFile(data, fileName, fileUploadParamerter.Tenant);
                         if (!myResult) documentId = "";

                     }
                }

                return Request.CreateResponse(HttpStatusCode.OK, documentId);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        





        
        



    }
}