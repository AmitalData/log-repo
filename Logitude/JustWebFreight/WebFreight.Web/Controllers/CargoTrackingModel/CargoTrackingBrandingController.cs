using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
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

        [HttpGet]
        public HttpResponseMessage GetCargoTrackingBrandingData(string domain)
        {
            try
            {
                CargoTrackingHelper cargoTrackingHelper = new CargoTrackingHelper();
                CargoTrackingBrandingData brandingData = cargoTrackingHelper.GetCargoTrackingBrandingDataByDomain(domain);
                ServiceResponse response = new ServiceResponse();
                response.Result = brandingData;
                return Request.CreateResponse(HttpStatusCode.OK, response);
                }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCargoTrackingBrandingDataForPrivateSite(string domain)
        {

            try
            {
                CargoTrackingHelper cargoTrackingHelper = new CargoTrackingHelper();
                CargoTrackingBrandingData brandingData = cargoTrackingHelper.GetCargoTrackingBrandingDataByDomain(domain,true);
                ServiceResponse response = new ServiceResponse();
                response.Result = brandingData;
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCargoTrackingBrandingDataForPrivateSite(string domain)
        {

            try
            {
                TenantManagementQuery tenantManagementQuery = new TenantManagementQuery();
                TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePMByDomain(domain);
                ServiceResponse response = new ServiceResponse();
                if (tenantManagementPM != null)
                {
                    CargoTrackingBrandingData data = new CargoTrackingBrandingData()
                    {
                        Tenant = tenantManagementPM.Id,
                        MainColor = tenantManagementPM.MainColor,
                        SecondaryColor = tenantManagementPM.SecondaryColor,
                    };
                    response.Result = data;
                }
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
    }
}