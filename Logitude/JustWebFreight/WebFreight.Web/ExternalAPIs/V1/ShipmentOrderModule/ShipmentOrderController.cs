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

namespace WebFreight.Web.ExternalAPIs.V1.ShipmentOrderModule
{
    public class ShipmentOrderController : ApiController
    {
        public HttpResponseMessage GetSingleShipmentOrder(string orderNumber)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ShipmentOrderQueryService shipmentOrderQueryService = new ShipmentOrderQueryService(tenant);

                var result = new ShipmentOrder();
                if (!string.IsNullOrEmpty(orderNumber))
                {
                    result = shipmentOrderQueryService.GetByOrderNumber(orderNumber, tenant);
                }

                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(result);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Post(ShipmentOrder entity)
        {
            ShipmentOrder oldEntity = entity;

            if (ModelState.IsValid)
            {
                try
                {

                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        int tenant = entity.Tenant;
                        SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                        if (entity != null)
                        {
                            oldEntity = LogitudeXmlSerializer.DeserializeObject<ShipmentOrder>(LogitudeXmlSerializer.SerializeObjectToXmlString(entity));
                        }

                        IShipmentOrderContext MyContext = ShipmentOrderContext.GetContext(entity.Tenant);
                        ShipmentOrderQueryService mappingService = new ShipmentOrderQueryService(entity.Tenant);
                        ShipmentOrderPM entityPM = mappingService.ShipmentOrderDataMappingAndValidatin(entity, entity.Tenant);
                        // mappingService.SetInvoiceLinesEntityId(entityPM, entity.Tenant);


                        entityPM.Tenant = entity.Tenant;

                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        ShipmentOrderUpdateService service = new ShipmentOrderUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        service.Update(entityPM, true);

                        // entityPM = mappingService.ShipmentOrderDataMappingAndValidatin(entity, entity.Tenant);
                        APIHelper.AddCommunicationLog("D", oldEntity, entity, "ShipmentOrder", entityPM.Id, "ShipmentOrder API", entity.Tenant);

                        scope.Complete();


                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "ShipmentOrder", null, "ShipmentOrder API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }

            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "ShipmentOrder", null, "ShipmentOrder API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Put(ShipmentOrder entity)
        {
            ShipmentOrder oldEntity = entity;

            if (ModelState.IsValid)
            {
                try
                {
           
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        int tenant = authToken.Tenant;
                        SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                        if (entity != null)
                        {
                            oldEntity = LogitudeXmlSerializer.DeserializeObject<ShipmentOrder>(LogitudeXmlSerializer.SerializeObjectToXmlString(entity));
                        }

                        IShipmentOrderContext MyContext = ShipmentOrderContext.GetContext(tenant);
                        ShipmentOrderQueryService mappingService = new ShipmentOrderQueryService(tenant);
                        ShipmentOrderPM entityPM = mappingService.ShipmentOrderDataMappingAndValidatin(entity, tenant);

                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        ShipmentOrderUpdateService service = new ShipmentOrderUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        service.Update(entityPM, true);

                        APIHelper.AddCommunicationLog("D", oldEntity, entity, "ShipmentOrder", entityPM.Id, "ShipmentOrder API", authToken.Tenant);


                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "ShipmentOrder", null, "ShipmentOrder API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "ShipmentOrder", null, "ShipmentOrder API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

    }
}