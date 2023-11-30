using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.BL.EntityQueryServices;
using Logitude.WarehouseLib.Data;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WarehouseModel.Extended
{
    public class WarehouseEntryPackageExtendedController : ApiController
    {
        public HttpResponseMessage GetwarehouseEntryPackagePMListByCustomerIdAndWarehouseId(string customerId , string warehouseId , int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                IWarehouseContext MyContext = WarehouseContext.GetContext(authToken.Tenant);
                WarehouseEntryPackageQueryService warehouseEntryPackageQuery = new WarehouseEntryPackageQueryService(MyContext);
                List<WarehouseEntryPackagePM> warehouseEntryPackagePMLists = warehouseEntryPackageQuery.GetWarehouseEntryPackagePMListsByCustomerIdIdAndWarehouseId(customerId, warehouseId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, warehouseEntryPackagePMLists);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetWarehouseEntryPackagePMListsByShipmentIdAndWarehouseIdAndCustomerId(string shipmentId , string customerId, string warehouseId ,  int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                IWarehouseContext MyContext = WarehouseContext.GetContext(authToken.Tenant);
                WarehouseEntryPackageQueryService warehouseEntryPackageQuery = new WarehouseEntryPackageQueryService(MyContext);
                 List<WarehouseEntryPackagePM> warehouseEntryPackagePMLists = warehouseEntryPackageQuery.GetWarehouseEntryPackagePMListsByShipmentIdAndWarehouseIdAndCustomerId(warehouseId, customerId , shipmentId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, warehouseEntryPackagePMLists);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

    }
}