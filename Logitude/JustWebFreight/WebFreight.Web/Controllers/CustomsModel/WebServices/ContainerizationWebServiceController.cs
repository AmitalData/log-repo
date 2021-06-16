using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{

    public class ContainerizationWebServiceController : ApiController
    {
        public HttpResponseMessage SendContainerization(GenericRequestParams requestParams)
        {
            try
            {
                GenericRequestParams ContainerizationRequest = new GenericRequestParams()
                {
                    LoggingEnabled = true,
                    Tenant = requestParams.Tenant,
                    RequestName = "המכלה",
                    ResponseName = "המכלה תשובה",
                    LoggingEntityId = requestParams.LoggingEntityId,
                    LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Containerization"),
                    MainInterfaceCode = "2450",
                    InterfaceTypeCode ="2450" , 
                    LoggingEntityReference = requestParams.AppicationId,
                    LoggingUserId = requestParams.LoggingUserId,
                    RequestVIA=requestParams.RequestVIA,
                    ForcePersonalSign=requestParams.ForcePersonalSign,
                };
                ContainerizationRequest.RequestVIA = requestParams.RequestVIA; 
                var myRequestMessagingService = new SaveCC_MSG2450_ContainerizationMessageMessagingService();
                var resData = myRequestMessagingService.Send(ContainerizationRequest);
                return Request.CreateResponse(HttpStatusCode.OK, resData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}