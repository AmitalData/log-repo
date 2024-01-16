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
    
    public class CertificateOfOriginController : ApiController
    {

        // POST api/<controller>    

        public HttpResponseMessage PostCertificateOfOriginRequest(CertificateOfOriginRequestRequestParams requestParams)
        {
            try
            {
				INF_MSG_GenericResponseData responseData = null;

                var service = new DCAInGetPC_MSG2280_2281_CertificateOfOriginRequestMessagingService();
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