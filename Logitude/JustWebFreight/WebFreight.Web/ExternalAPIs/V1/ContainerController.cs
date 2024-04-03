using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class ContainerController : ApiController
    {
        public HttpResponseMessage GetSingleContainer(string containerNumber, string shipmentNumber)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                SecurityUtility.AuthenticateAccessibleAPI("Container", authToken.Tenant);

                ContainerDataQueryService Service = new ContainerDataQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                ContainerData Result = Service.GetContainerDataByContainerNumberAndShipmentNumber(containerNumber, shipmentNumber, tenant);
                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }

            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Put(ContainerData entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.AuthenticateAccessibleAPI("Container", authToken.Tenant);

                    if (FeatureToggleHelper.HasFeatureToggle("API", authToken.Tenant))
                    {
                        ContainerData returnedContainer = null;
                        string computingPartnerCode = "";
                        if (!string.IsNullOrEmpty(entity.ComputingPartnerCode))
                        {
                            computingPartnerCode = entity.ComputingPartnerCode;
                        }

                        IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(authToken.Tenant);
                        ContainerDataQueryService mappingService = new ContainerDataQueryService(authToken.Tenant);

                        ContainerPM containerPM = mappingService.ContainerDataCustomDataMapping(entity, authToken.Tenant, computingPartnerCode, true);

                        if (containerPM != null)
                        {
                            containerPM.IsUpdatedFromAPI = true;
                            ContainerService service = new ContainerService(shipmentsContext, authToken.Tenant);
                            service.Update(containerPM);

                            shipmentsContext = ShipmentsContext.GetContext(authToken.Tenant);
                            mappingService = new ContainerDataQueryService(authToken.Tenant);
                            returnedContainer = mappingService.GetContainerDataById(containerPM.Id, authToken.Tenant, null);
                            APIHelper.AddCommunicationLog("D", entity, returnedContainer, "Container", containerPM.Id, "Container API", authToken.Tenant);
                        }

                        return Request.CreateResponse(HttpStatusCode.OK, returnedContainer);
                    }

                    else
                    {
                        throw new ApplicationException("Update is not allowed");
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Container", null, "Container API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Container", null, "Container API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
    }
}