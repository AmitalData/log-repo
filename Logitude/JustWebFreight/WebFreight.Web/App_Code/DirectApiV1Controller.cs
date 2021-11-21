using Logitude.BL.QuoteModel.APIDataContract;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.ShipmentsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code
{
    public class DirectApiV1Controller : ApiController
    {
        public HttpResponseMessage GetSingleDirect(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                HouseQueryService Service = new HouseQueryService(authToken.Tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = Service.GetHouseById(id, authToken.Tenant);
                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                string json = LogitudeXmlSerializer.SerializeObjectToJosnString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage Post(Direct entity)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    //using (TransactionScope scope = TransactionFactory.GetTransaction())
                    // {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                    SecurityUtility.CheckContactFeature("Shipment", "NEW", authToken.Tenant);


                    IShipmentsContext MyContext = ShipmentsContext.GetContext(authToken.Tenant);
                    DirectQueryService mappingService = new DirectQueryService(authToken.Tenant);
                    ShipmentPM entityPM  = mappingService.DirectCustomDataMappingAndValidatin(entity, authToken.Tenant);
                    
                    ShipmentService service = new ShipmentService(MyContext, entityPM, SecurityUtility.GetAuthenticatedUser());
                    service.Create();


                    // scope.Complete();


                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    //}
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }


        public HttpResponseMessage Put(Direct entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //string logKey = PerformanceLogger.LogCurrentTime();
                    //using (TransactionScope scope = TransactionFactory.GetTransaction())
                    //{
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("Shipment", "UPDATE", authToken.Tenant);

                    //string entityName = "Shipment" + entityPM.Id + entityPM.Tenant;
                    //string entityPmName = "ShipmentPM" + entityPM.Id + entityPM.Tenant;
                    //if (CacheManager.CacheWrapper.Get(entityName) != null)
                    // {
                    //     CacheManager.CacheWrapper.Invalidate(entityName);
                    //}
                    //if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                    //{
                    //    CacheManager.CacheWrapper.Invalidate(entityPmName);
                    //}

                    IShipmentsContext MyContext = ShipmentsContext.GetContext(authToken.Tenant);
                    DirectQueryService mappingService = new DirectQueryService(authToken.Tenant);
                    ShipmentPM entityPM = mappingService.DirectDataMappingAndValidatin(entity, authToken.Tenant);

                    ShipmentService service = new ShipmentService(MyContext, entityPM, SecurityUtility.GetAuthenticatedUser());
                    service.Update();
                  


                    //scope.Complete();
                    //PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    //}
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

    }
}
