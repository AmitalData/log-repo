using Logitude.BL.Security;
using Logitude.CargoTracking.BL.CargoTrackingServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebFreight.Web.Controllers.CargoTrackingModel
{
    public class CargoSessionController : ApiController
    {
        public IHttpActionResult GetSessionTimeOut()
        {
            var tenant = GetAuthinticatedTenant();
            string token = HttpContext.Current.Request.Headers["Token"];
            CargoSessionService cargoSessionService = new CargoSessionService();
            var tokenLifeTime = cargoSessionService.GetSessionTimeOut(tenant, token);
            return Ok(tokenLifeTime);
        }
        private int GetAuthinticatedTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            return authToken.Tenant;
        }
    }
}