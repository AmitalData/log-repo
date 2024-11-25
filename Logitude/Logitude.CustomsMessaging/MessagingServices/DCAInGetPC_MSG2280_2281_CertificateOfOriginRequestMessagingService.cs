
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
using Logitude.Server.Tools.Helpers;
using System;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;

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
			try
			{
				var responseContentHeader = customsResponse.GetResponseContentHeader() as IResponseContentHeader;
				ThrowIIGBLException(_ResponseHeader, responseContentHeader);

			}
			catch (System.ServiceModel.FaultException<UnifreightIIGFault> myUnifreightIIGFault)
			{
				var defaultMessage = "Sending request to IIG Server Failed ";
				var FormattedMessage = UnifreightIIG.Common.Utils.ErrorHandlerUtil.CreateNew().ToFormattedMessage(myUnifreightIIGFault);
				if (String.IsNullOrWhiteSpace(FormattedMessage))
				{
					FormattedMessage = defaultMessage;
				}
				LogMessagingUtil.Instance.AppendLine(FormattedMessage);

				switch (myUnifreightIIGFault.Detail.PlaceFault)
				{

					case UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError:
					case UnifreightIIGFault.PlaceFaultEnum.IIGFatalException:
					case UnifreightIIGFault.PlaceFaultEnum.IIGTechnicalError:
						{
							LogMessagingUtil.Instance.AppendLine("PC_NG_2281_MSG02_CertificateOfOriginRequestFeedback: " + myUnifreightIIGFault.Detail.PlaceFault);
							if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
							{
								//ITZIK+MIRT _ResponseHeader.ErrorDescription = FormattedMessage;
								//ITZIK+MIRT (_ResponseService as DF_NG_2754_MSG10004_ImportDeclarationResponseService)._ResponseHeaderExeption = _ResponseHeader;
							}
						}
						break;
					default:
						return new ExportDeclarationAmendmentResponseData() { UserMessage = FormattedMessage, HasException = true };
				}
			}

			return null;
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


        public override string MainInterfaceCode { get { return "2281"; } }
    }
}
