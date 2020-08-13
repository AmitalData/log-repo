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

namespace WebFreight.Web.Controllers.CargoTrackingModel
{
    public class CargoTrackingBrandingController: ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetCargoTrackingBrandingData(int tenant)
        {
            try
            {
                TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenant);
                TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePM(tenant);
                CargoTrackingBrandingData data = new CargoTrackingBrandingData { Tenant = tenant, MainColor =tenantManagementPM.MainColor , SecondaryColor = tenantManagementPM.SecondaryColor, BackgroundId = tenantManagementPM.BackgroundId };
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