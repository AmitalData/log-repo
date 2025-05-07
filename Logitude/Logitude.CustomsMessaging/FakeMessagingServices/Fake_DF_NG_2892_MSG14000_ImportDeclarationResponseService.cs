using Logitude.CustomsMessaging.Common.RequestParams;
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
        private readonly DF_NG_2892_MSG14000_ImportDeclarationAmendmentRequestMsg request;
        public Fake_DF_NG_2892_MSG14000_ImportDeclarationResponseService(GenericRequestParams requestParams)  {
            genericRequestParams = requestParams;
            DF_MSG2892_ImportDeclarationAmendmentRequestService reqService = new DF_MSG2892_ImportDeclarationAmendmentRequestService();
            request = reqService.GetRequest(requestParams);
        }
    public ResponseHeader CallWS(out DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg response)
        {
            dynamic data = JObject.Parse(genericRequestParams.TestCase.Param1);

            string Error = data.Error;
            string FunctionalReferenceID = data.RequestNo;
            ResponseHeader responseHeader = new ResponseHeader();
                          response = new DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg();
            response.Response = new Response();
            response.Response.FunctionalReferenceID = new ResponseFunctionalReferenceIDType() { Value = FunctionalReferenceID };


            //AdditionalInformation
            ResponseAdditionalInformation[] AdditionalInformation = new ResponseAdditionalInformation[3];
            AdditionalInformation[0] = new ResponseAdditionalInformation();
            AdditionalInformation[0].StatementTypeCode = new AdditionalInformationStatementTypeCodeType() { Value = "29" };
            AdditionalInformation[0].Content = new AdditionalInformationContentTextType() { Value = "t29" };
            AdditionalInformation[1] = new ResponseAdditionalInformation();
            AdditionalInformation[1].StatementTypeCode = new AdditionalInformationStatementTypeCodeType() { Value = "27" };
            AdditionalInformation[1].Content = new AdditionalInformationContentTextType() { Value = "t27" };
            AdditionalInformation[2] = new ResponseAdditionalInformation();
            AdditionalInformation[2].StatementTypeCode = new AdditionalInformationStatementTypeCodeType() { Value = "32" };
            AdditionalInformation[2].Content = new AdditionalInformationContentTextType() { Value = "1" };
            response.Response.AdditionalInformation = AdditionalInformation;

            //status
            response.Response.Status = new ResponseStatus() { EffectiveDateTime = DateTime.Now.ToString() };
            response.Response.Status.NameCode = new StatusNameCodeType() { Value = "5" };

            response.Response.Declaration = request.Response?.Declaration;
           


            response.ResponseContentHeader = new ResponseContentHeader();
            if (Error == "true")
            {
                response.ResponseContentHeader.Exception = new Exception[1];
                response.ResponseContentHeader.Exception[0] = new Exception();
                response.ResponseContentHeader.Exception[0].ExceptionLevel =  3 ;
                response.ResponseContentHeader.Exception[0].ExeptionType =  9999 ;
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
