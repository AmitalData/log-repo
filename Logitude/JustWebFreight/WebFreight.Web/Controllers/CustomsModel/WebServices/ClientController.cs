using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using WebFreight.Web.CustomWebServices;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class ClientController : ApiController
    {

        public HttpResponseMessage PostClientRequest(ClientSearchRequestParams requestParams)
        {
            try
            {
                ClientSearchResponseData responseData = null;


                // use messageing service

                var service = new CL_MSG101_GetCustomerByEntityCustomerIdentificationMassagingService();
                responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostClientSearchByIDRequest(ClientSearchRequestParams requestParams)
        {
            try
            {
                ClientSearchByIDResponseData responseData = null;

                var service = new CL_NG_8343_ClientSearchByIDParamMessagingService();
                responseData = service.Send(requestParams);

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostUpdateDeleteClientAddressContactRequest(AddAddressContactForClient requestParams)
        {
            try
            {
                INF_MSG_GenericResponseData responseData = null;
                if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
                {
                    responseData = new INF_MSG_GenericResponseData();
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
                    var messagingService = new CL_3630_AddUpdateDeleteAddressContactMassagingService();
                    responseData = messagingService.Send(requestParams);

                }

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage CreateClientRequest(CreateClientRequestParams requestParams)
        {
            try
            {
                INF_MSG_GenericResponseData responseData = null;


                // use messageing service

                var service = new CL_MSG100_AddClientMassagingService();
                responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSingleClientPMByCode(string code, bool isIncludeAll = false)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                customContext = CustomContext.GetContext(tenant);
                ClientQueryService clientQuery = new ClientQueryService(customContext);
                ClientPM client = clientQuery.GetClientByCode(code, tenant);
                if (isIncludeAll && client != null)
                {
                    client = clientQuery.GetSingle(client.Id, true, false);
                }

                return Request.CreateResponse(HttpStatusCode.OK, client);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutRecallClientsForCutomsRequest(ImageParameter fileUploadParamerter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string message = "";
                if (fileUploadParamerter != null)
                {
                    ClientWebService clientWebService = new ClientWebService();
                    message = clientWebService.RecallClientsForCutomsRequest(fileUploadParamerter.Key, fileUploadParamerter.Tenant);
                }

                return Request.CreateResponse(HttpStatusCode.OK, message);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}