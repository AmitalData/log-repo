using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Counters;
using Newtonsoft.Json.Linq;
using System;
using UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    public class Fake_DF_NG_8237_NotValidExportDeclerationAmendmentReplyResponseService
    {
        GenericRequestParams genericRequestParams;
        public Fake_DF_NG_8237_NotValidExportDeclerationAmendmentReplyResponseService(GenericRequestParams requestParams)  {
            genericRequestParams = requestParams;
        }
    public ResponseHeader CallWS(AmendmentRequestParams requestParams, out DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg response)
        {
            dynamic data = JObject.Parse(genericRequestParams.TestCase.Param1);

            string Error = data.Error;
            ResponseHeader responseHeader = new ResponseHeader();
            response = new DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg();
            response.ResponseContentHeader = new ResponseContentHeader();

            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParams.Tenant);
            DeclarationPM _dec = declarationQueryService.GetSingle(requestParams.AppicationId, false, false);

            response.Response = new Response();
            response.Response.IssueDateTime = DateTime.Now.ToString();
            response.Response.FunctionCode = new ResponseFunctionCodeType() { Value = "1" };
            response.Response.FunctionalReferenceID = new ResponseFunctionalReferenceIDType
            {
                Value = string.IsNullOrEmpty(_dec.AmendmentRequestNumber) ? CodeCounter.GetNumber("AmendmentRequestNumber", _dec.Tenant).ToString() : _dec.AmendmentRequestNumber
            };

            ResponseAdditionalInformation[] AdditionalInformation = new ResponseAdditionalInformation[3];
            AdditionalInformation[0] = new ResponseAdditionalInformation();
            AdditionalInformation[0].StatementTypeCode = new AdditionalInformationStatementTypeCodeType() { Value = "29" };
            AdditionalInformation[0].Content = new AdditionalDocumentTypeTextType() { Value = "t29" };
            AdditionalInformation[1] = new ResponseAdditionalInformation();
            AdditionalInformation[1].StatementTypeCode = new AdditionalInformationStatementTypeCodeType() { Value = "27" };
            AdditionalInformation[1].Content = new AdditionalDocumentTypeTextType() { Value = "t27" };
            AdditionalInformation[2] = new ResponseAdditionalInformation();
            AdditionalInformation[2].StatementTypeCode = new AdditionalInformationStatementTypeCodeType() { Value = "32" };
            AdditionalInformation[2].Content = new AdditionalDocumentTypeTextType() { Value = "4" };
            response.Response.AdditionalInformation = AdditionalInformation;


            //amendment
            response.Response.Amendment = new ResponseAmendment[1]; // reason to change?


            //status
            response.Response.Status = new ResponseStatus[1];
            response.Response.Status[0] = new ResponseStatus();
            response.Response.Status[0].EffectiveDateTime = DateTime.Now.ToString();
            response.Response.Status[0].NameCode = new StatusNameCodeType() { Value = "5" };

            response.Response.Declaration = new Declaration();
            response.Response.Declaration.ID = new DeclarationIdentificationIDType() { Value = _dec.DeclarationNumber };
            response.Response.Declaration.IssueDateTime = DateTime.Now.ToString();
            response.Response.Declaration.DMExtensions = new DeclarationDMExtensions();
            response.Response.Declaration.DMExtensions.VersionID = new DeclarationVersionIDType() { Value = _dec.VersionId };
            response.Response.Declaration.DMExtensions.AgentFileReferenceID = new AgentFileReferenceIDType() { Value = _dec.CustomFileNo };

            response.ResponseContentHeader.TransmitionDateTime = DateTime.Now;
            response.ResponseContentHeader.Remark = "";
        
            response.ResponseContentHeader.ApplicationID = 0;

            responseHeader.CorrelationId = Guid.NewGuid().ToString();
            responseHeader.ExternalId = Guid.NewGuid().ToString();
            responseHeader.Status = "Success";
            responseHeader.ErrorDescription = "";
            responseHeader.ErrorCode = "None";
            return responseHeader;

        }
  
    }
}
