using Logitude.ShipmentOrderModule.BL.APIDataContract.ApiV1;
using Logitude.ShipmentOrderModule.BL.EntityUpdateServices;
using Logitude.ShipmentOrderModule.Data;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
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
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers.ShipmentOrderModule;

namespace WebFreight.Web.ExternalAPIs.V1.ShipmentOrderModule
{
    public class ShipmentOrderController : ApiController
    {
        public HttpResponseMessage Get(string orderNumber)
        {
            try
            {
                int tenant = GetTenantFromAuthenticationToken();
                Authentication(tenant);
                ShipmentOrder shipmentOrder = new ShipmentOrderService(tenant).GetByOrderNumber(tenant, orderNumber);
                return Request.CreateResponse(HttpStatusCode.OK, shipmentOrder);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Post(ShipmentOrder entity)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        int tenant = GetTenantFromAuthenticationToken();
                        Authentication(tenant);

                        var entityPM = new ShipmentOrderService(tenant).Create(entity);
               
                        var result = new ShipmentOrderQueryService(tenant).GetShipmentOrderById(entityPM.Id, tenant);

                        APIHelper.AddCommunicationLog("D", entity, result, "ShipmentOrder", entityPM.Id, "ShipmentOrder API", tenant);
                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "ShipmentOrder", null, "ShipmentOrder API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }

            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "ShipmentOrder", null, "ShipmentOrder API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Put(ShipmentOrder entity)
        {


            if (ModelState.IsValid)
            {
                try
                {

                    int tenant = GetTenantFromAuthenticationToken();
                    Authentication(tenant);

                    var entityPM = new ShipmentOrderService(tenant).Update(entity);

                    var result = new ShipmentOrderQueryService(tenant).GetShipmentOrderById(entityPM.Id, tenant);

                    APIHelper.AddCommunicationLog("D", entity, result, "ShipmentOrder", entityPM.Id, "ShipmentOrder API", tenant);
                    return Request.CreateResponse(HttpStatusCode.OK, result);

                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "ShipmentOrder", null, "ShipmentOrder API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "ShipmentOrder", null, "ShipmentOrder API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }


        private int GetTenantFromAuthenticationToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            return authToken.Tenant;
        }


        private static void Authentication(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.AuthenticateAPICall(tenant);

        }

    }
}