using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;
using WebFreight.Web.Validators;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.Helpers;
using Logitude.BL.DataContracts;
using Simplog.Data.CommonDataModel;
namespace WebFreight.Web.ExternalAPIs.V1
{
    public class ShipmentStatusesController : ApiController
    {
        public HttpResponseMessage Get(string housenumber, bool latestStatusOnly)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                if(authToken == null)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Sorry! you are not authorized to read data!");
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                var service = new ShipmentStatusesQueryService(authToken.Tenant);
                var result = service.GetShipmentStatuses(housenumber, latestStatusOnly);
                
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
    }
}