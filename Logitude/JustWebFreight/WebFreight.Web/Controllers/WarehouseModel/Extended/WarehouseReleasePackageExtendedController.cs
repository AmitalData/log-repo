using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.BL.EntityQueryServices;
using Logitude.WarehouseLib.Data;
using Logitude.WarehouseLib.Data.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WarehouseModel.Extended
{
    public class WarehouseReleasePackageExtendedController : ApiController
    {
        public HttpResponseMessage GetWarehouseReleasePackagePMListsByWarehouseReleaseId(string warehouseReleaseId, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("WarehouseRelease", "READ", authToken.Tenant);
                IWarehouseContext MyContext = WarehouseContext.GetContext(authToken.Tenant);
                WarehouseReleasePackageQueryService warehouseReleasePackageQuery = new WarehouseReleasePackageQueryService(MyContext);
                List<WarehouseReleasePackagePM> warehouseReleasePackagePMLists = warehouseReleasePackageQuery.GetWarehouseReleasePackagePMListsByWarehouseReleaseId(warehouseReleaseId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, warehouseReleasePackagePMLists);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage GetWarehouseReleasePackagePMThatNotUsedForAnyEntityLists()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("WarehouseRelease", "READ", authToken.Tenant);
                IWarehouseContext MyContext = WarehouseContext.GetContext(authToken.Tenant);
                WarehouseReleasePackageQueryService warehouseReleasePackageQuery = new WarehouseReleasePackageQueryService(MyContext);
                var result = warehouseReleasePackageQuery.GetWarehouseReleasePackagePMLists(authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetCheckIfShipmentHasReleasePackages(string shipmentId, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("WarehouseRelease", "READ", authToken.Tenant);
                IWarehouseContext MyContext = WarehouseContext.GetContext(authToken.Tenant);
                WarehouseReleaseQueryService warehouseReleaseQueryService = new WarehouseReleaseQueryService(authToken.Tenant);
                bool result  = warehouseReleaseQueryService.CheckIfShipmentHasReleasePackage(shipmentId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        


    }
}