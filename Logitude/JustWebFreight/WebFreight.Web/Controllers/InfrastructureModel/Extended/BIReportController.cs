using Logitude.Infrastructure.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class BIReportsExtendedController : ApiController
    {
        public HttpResponseMessage GetReportExist(string name, string folderId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                BIReportQueryService bIReportQueryService = new BIReportQueryService(authToken.Tenant);
                bool reportExist = bIReportQueryService.DoesReportExist(name, folderId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, reportExist);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}