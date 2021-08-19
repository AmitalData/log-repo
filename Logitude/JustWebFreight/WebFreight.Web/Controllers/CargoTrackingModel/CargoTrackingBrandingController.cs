using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.Resolvers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Controllers.CargoTrackingModel
{
    public class CargoTrackingBrandingController: ApiController
    {
 
        public HttpResponseMessage PutGetCargoTrackingBrandingData(CargoTrackingBrandingDataRequest BrandingDataRequest)
        {
            try
            {
                CargoTrackingHelper cargoTrackingHelper = new CargoTrackingHelper();
                CargoTrackingBrandingData brandingData = cargoTrackingHelper.GetCargoTrackingBrandingDataByDomain(BrandingDataRequest);
                ServiceResponse response = new ServiceResponse();
                response.Result = brandingData;
                return Request.CreateResponse(HttpStatusCode.OK, response);
                }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutGetCargoTrackingBrandingDataForPrivateSite(CargoTrackingBrandingDataRequest BrandingDataRequest)
        {

            try
            {
                CargoTrackingHelper cargoTrackingHelper = new CargoTrackingHelper();
                CargoTrackingBrandingData brandingData = cargoTrackingHelper.GetCargoTrackingBrandingDataByDomain(BrandingDataRequest, true);
                ServiceResponse response = new ServiceResponse();
                response.Result = brandingData;
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCargoTrackingBrandingTenantByDomain(string domain)
        {
            try
            {
                TenantManagementQuery tenantManagementQuery = new TenantManagementQuery();
                ServiceResponse response = new ServiceResponse();
                int? tenant = tenantManagementQuery.GetTenantSinglePMByDomain(domain);
                if (tenant != null && tenant != 0)
                {
                    response.Result = tenant;
                }

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetLoggedContact(int tenant)
        {
            try
            {
                var contact = LoggedContactResolver.GetLoggedContact(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, contact);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}