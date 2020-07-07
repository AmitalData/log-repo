using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.WebServices
{
    public class CargoTrackingController: ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetCargoTrackingBrandingData()
        {
            try
            {
                TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(1);
                TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePM(1);
                CargoTrackingBrandingData data = new CargoTrackingBrandingData { Tenant = 1, MainColor = "#c0c0c0", SecondaryColor = "#FF0000", BackgroundId = "1-30" };
                ServiceResponse response = new ServiceResponse();
                response.Result = data;
                return Request.CreateResponse(HttpStatusCode.OK, response);
                }
            

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}