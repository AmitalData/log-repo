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
using WebFreight.Web.WebServices;

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
                Uploader uploaderService = new Uploader();
                byte[] filedata = uploaderService.DownloadFile(tenantManagementPM.BackgroundId, "jpg", "images", 0);
                CargoTrackingBrandingData data = new CargoTrackingBrandingData()
                {
                    Tenant = tenant,
                    MainColor = tenantManagementPM.MainColor,
                    SecondaryColor = tenantManagementPM.SecondaryColor,
                    BackgroundId = tenantManagementPM.BackgroundId,
                    
                };
                if (filedata != null)
                {
                    data.BackgroundImg = "data:image/" + "jpg" + ";base64," + Convert.ToBase64String(filedata);
                }
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