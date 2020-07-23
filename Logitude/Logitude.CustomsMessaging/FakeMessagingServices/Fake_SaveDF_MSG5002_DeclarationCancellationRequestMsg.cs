using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Newtonsoft.Json.Linq;
using System;

using UnifreightIIG.Common.DeclarationCancellationRequestMsgServiceReference;
using Exception = UnifreightIIG.Common.DeclarationCancellationRequestMsgServiceReference.Exception;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    public class Fake_SaveDF_MSG5002_DeclarationCancellationRequestMsg
    {
        GenericRequestParams genericRequestParams;
        public Fake_SaveDF_MSG5002_DeclarationCancellationRequestMsg(GenericRequestParams requestParams)
        {
            genericRequestParams = requestParams;
        }
        public ResponseHeader CallWS(out INF_MSG_Generic response)
        {
            //  UpdateDeclaration();
            //  UpdateStatus("5");
            //  response = fakeRespond;
            // UpdateFakeResponseContentHeader();
            //  AddSign();

            dynamic data = JObject.Parse(genericRequestParams.TestCase.Param1);

            string Error = data.Error;
            ResponseHeader responseHeader = new ResponseHeader();
            response = new INF_MSG_Generic();
            response.ResponseContentHeader = new ResponseContentHeader();
            if (Error == "true")
            {
                response.ResponseContentHeader.Exception = new Exception[1];
                response.ResponseContentHeader.Exception[0] = new Exception();
                response.ResponseContentHeader.Exception[0].ExceptionLevel = 3;
                response.ResponseContentHeader.Exception[0].ExeptionType = 9999;
                response.ResponseContentHeader.Exception[0].ExceptionParms = null;
                response.ResponseContentHeader.Exception[0].ExeptionDescription = "FAKE";
                response.ResponseContentHeader.Exception[0].EnglishDescription = "FAKE";
            }
            else
            {
                response.ResponseContentHeader.Exception = null;
            }
            response.ResponseContentHeader.TransmitionDateTime = DateTime.Now;
            response.ResponseContentHeader.Remark = "";

            response.ResponseContentHeader.ApplicationID = 0;

            responseHeader.CorrelationId = Guid.NewGuid().ToString();
            responseHeader.ExternalId = Guid.NewGuid().ToString();
            responseHeader.Status = "Success";
            responseHeader.ErrorDescription = "";
            responseHeader.ErrorCode = "None";
            //response.
            //   response.DeclarationPaymentDetails=AddPaymentDetails_2754();
            return responseHeader;

        }

    }
}
