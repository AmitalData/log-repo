
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CertificateOfOriginRequestServiceReference;
using UnifreightIIG.Common.TheGateway;
using Logitude.Customs.Def.EntityPMs;

namespace Logitude.CustomsMessaging.MessagingServices
{
	public class DCAInGetPC_MSG2280_2281_CertificateOfOriginRequestMessagingService
		: MessagingServiceBase<
		CertificateOfOriginRequestRequestParams,
        INF_MSG_GenericResponseData,
		PC_NG_2280_MSG01_CertificateOfOriginRequest,
		PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback,
		PC_NG_2280_MSG01_CertificateOfOriginRequestRequestService,
		PC_NG_2280_MSG01_CertificateOfOriginRequestResponseService,
        RequestHeader>
    {
        protected override INF_MSG_GenericResponseData GetIIGBLExceptionFromReponseHeader(PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback customsResponse)
        {
            

            return base.GetIIGBLExceptionFromReponseHeader(customsResponse);
        }


        protected override CertificateOfOriginRequestRequestParams CreateDefaultRequestParamsFromCustomsResponse(PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback customsResponse)
        {
			CertificateOfOriginPM entity = new CertificateOfOriginQueryService(CustomContext.GetContext(0)).GetCertificateOfOriginByCounter(
			   customsResponse.CertificateOfOriginRequestFeedback.internalApplication);
			var myRequestParams = new CertificateOfOriginRequestRequestParams()
            {
				LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
			    LoggingEntityId = entity.DeclarationId,
			    LoggingObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.CertificateOfOrigin"),
			    LoggingEntityId2 = entity.Id
		    };
            return myRequestParams;
		}


        protected override PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback CallWS(PC_NG_2280_MSG01_CertificateOfOriginRequest customRequest, CertificateOfOriginRequestRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null; // to check            
            var response = new PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ICertificateOfOriginRequestOperation>()
                    .CertificateOfOriginRequest(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }


        public override string MainInterfaceCode { get { return "2280"; } }
    }
}
