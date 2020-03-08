using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    
    public class DeclarationRestoreController : ApiController
    {

        public DeclarationRestoreController()
        {

        }

        // POST api/<controller>
        public HttpResponseMessage PostDeclarationRequest(DeclarationRestoreRequestParams requestParams)
        {
            try
            {
                DeclarationRestoreResponseData responseData = null;
                if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
                {
                    responseData = new DeclarationRestoreResponseData();
                    switch (requestParams.TestCase.Code)
                    {
                        case "Send Succeeded":
                            {
                                responseData.HasException = false;
                                responseData.Succeeded = true;
                                responseData.UserMessage = null;

                                break;
                            }
                        case "Send Failed":
                            {
                                responseData.HasException = true;
                                responseData.Succeeded = false;
                                responseData.UserMessage = "this is a test fail exception for send declaration restore!";
                                break;
                            }
                    }
                }
                else
                {

                    // use messageing service
                    var messagingService = new DF_NG_8373_Web05_RetrieveImportDeclarationMessagingService();
                    responseData = messagingService.Send(requestParams);

                }

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostDeclarationStatusRequest(DeclarationStatusRequestParams requestParams)
        {
            try
            {
                DeclarationStatusResponseData responseData = null;


                // use messageing service

                var service = new DF_NG_8250_Web01_DeclarationStatus_RequestMessagingService();
                responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostWarehouseBlockBalanceRequest(ST_8328_Web01_WarehouseBlockBalanceRequestParams requestParams)
        {
            try
            {
                ST_8328_Web01_WarehouseBlockBalanceResponseData responseData = null;

                var service = new ST_8328_Web01_WarehouseBlockBalanceMessagingService();
                responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostPrintRequestRequest(DF_NG_8302_Web03_DeclarationPrintRequestParams requestParams)
        {
            try
            {
                DeclarationPrintResponseData responseData = null;

                var service = new DF_NG_8302_Web03_DeclarationPrintMessagingService();
                responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostExportDeclarationDataRequest(ExportDeclarationDataRequestParams requestParams)
        {
            try
            {
                ExportDeclarationDataResponseData responseData = null;

                var service = new DF_9070_ExportDeclarationDataMessagingService();
                responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostStorageEntranceUnloadingRequest(StorageEntranceUnloadingRequestParams requestParams)
        {
            try
            {
                StorageEntranceUnloadingResponseData responseData = null;
                var service = new ST_MSG05_StorageEntranceUnloadingMassagingService();
                responseData = service.Send(requestParams);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}