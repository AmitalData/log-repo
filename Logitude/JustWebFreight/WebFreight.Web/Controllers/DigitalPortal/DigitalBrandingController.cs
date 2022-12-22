using Logitude.SystemLogs;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalBrandingController : ApiController
    {
        [HttpGet]
        [Route("DigitalBranding/GetBrandingDataByDomain")]
        public HttpResponseMessage GetBrandingDataByDomain(string domain)
        {
            try
            {
                var brandingHelper = new BrandingHelper();

                BrandingData brandingData = brandingHelper.GetBrandingDataByDomain(domain);

                return Request.CreateResponse(HttpStatusCode.OK, brandingData);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", $"GetBrandingDataByDomain : {domain}", "GetBrandingDataByDomain : GetBrandingDataByDomain", null);

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalBranding/GetBrandingTenantNumberByDomain")]
        public HttpResponseMessage GetBrandingTenantNumberByDomain(string domain)
        {
            try
            {
                var brandingHelper = new BrandingHelper();

                var tenant = brandingHelper.GetBrandingDataByDomain(domain)?.Tenant;

                return Request.CreateResponse(HttpStatusCode.OK, tenant);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", $"GetBrandingDataByDomain : {domain}", "GetBrandingDataByDomain : GetBrandingDataByDomain", null);

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}