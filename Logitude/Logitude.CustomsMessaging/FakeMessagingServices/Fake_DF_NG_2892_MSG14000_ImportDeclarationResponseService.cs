using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Newtonsoft.Json.Linq;
using System;
using UnifreightIIG.Common.ImportDeclarationAmendmentServiceReference;
using Exception = UnifreightIIG.Common.ImportDeclarationAmendmentServiceReference.Exception;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    public class Fake_DF_NG_2892_MSG14000_ImportDeclarationResponseService  
    {
        GenericRequestParams genericRequestParams;
        public Fake_DF_NG_2892_MSG14000_ImportDeclarationResponseService(GenericRequestParams requestParams)  {
            genericRequestParams = requestParams;
        }
    public INF_MSG_Generic CallWS()
        {
            //  UpdateDeclaration();
            //  UpdateStatus("5");
            //  response = fakeRespond;
            // UpdateFakeResponseContentHeader();
            //  AddSign();

            dynamic data = JObject.Parse(genericRequestParams.TestCase.Param1);

            string Error = data.Error;

            INF_MSG_Generic response = new INF_MSG_Generic();
            response.ResponseContentHeader = new ResponseContentHeader();
            if (Error == "true")
            {
                response.ResponseContentHeader.Exception = new Exception[1];
                response.ResponseContentHeader.Exception[0].ExceptionLevel =  3 ;
                response.ResponseContentHeader.Exception[0].ExeptionType =  9999 ;
                response.ResponseContentHeader.Exception[0].ExceptionParms = null;
                response.ResponseContentHeader.Exception[0].ExeptionDescription = "FAKE";
                response.ResponseContentHeader.Exception[0].EnglishDescription = "FAKE";
            }

            response.ResponseContentHeader.TransmitionDateTime = DateTime.Now;
            response.ResponseContentHeader.Remark = "";
            response.ResponseContentHeader.Exception = null;
            response.ResponseContentHeader.ApplicationID = 0;
            //response.
         //   response.DeclarationPaymentDetails=AddPaymentDetails_2754();
            return response;

        }
  
    }
}
