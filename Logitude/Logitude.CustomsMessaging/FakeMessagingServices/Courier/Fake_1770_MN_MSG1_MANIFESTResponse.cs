using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using UnifreightIIG.Common.MANIFESTRequestServiceReference;
using Exception = UnifreightIIG.Common.ImportDeclarationServiceReference.Exception;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    public class Fake_1770_MN_MSG1_MANIFESTResponse  
    {
        public Fake_1770_MN_MSG1_MANIFESTResponse(MANIFESTRequestRequestParams requestParams) { }
      
        public ResponseHeader CallWS(MANIFESTRequestRequestParams requestParams,out MN_MSG4_SendManifestFeedBack_Message response)
        {
            dynamic data = JObject.Parse(requestParams.TestCase.Param1);

            ResponseHeader responseHeader = new ResponseHeader();
            response = new MN_MSG4_SendManifestFeedBack_Message();
            response.ResponseContentHeader = new ResponseContentHeader();
            response.ResponseContentHeader.ApplicationID = 0;
            response.Response = new Response();

            response.Response.Declaration = new Declaration()
            {
                Submitter= new DeclarationSubmitter() { ID = new SubmitterIdentificationIDType() { Value = "514193408" }  },
                
            };

            if (!string.IsNullOrEmpty(data.manifestCargoStatusCode.ToString()))
            {

                response.Response.Error = new ResponseError[1];
                response.Response.Error[0] = new ResponseError
                {
                    ValidationCode = new ErrorValidationCodeType()
                    {
                        Value = "FAKE",
                        listVersionID = data.manifestCargoStatusCode.ToString(),
                        listName = "FAKE",
                        name= "2772"
                    }
                };

                response.Response.Error[0].Pointer = new ResponseErrorPointer[2];

                response.Response.Error[0].Pointer[0] = new ResponseErrorPointer() { DocumentSectionCode = new PointerDocumentSectionCodeType() { Value = "42A" } };
                response.Response.Error[0].Pointer[1] = new ResponseErrorPointer() { DocumentSectionCode = new PointerDocumentSectionCodeType() { Value = "28A" }, SequenceNumeric = 10 };

            }
                responseHeader.CorrelationId = Guid.NewGuid().ToString();
            responseHeader.ExternalId = Guid.NewGuid().ToString();
            responseHeader.Status = "Success";
            responseHeader.ErrorDescription = "";
            responseHeader.ErrorCode = "None";
            return responseHeader;

        }



    }
}
