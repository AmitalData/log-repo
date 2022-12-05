using Logitude.CustomsMessaging.MessagingServices;
using UnifreightIIG.Common.TransshipmenDeclarationAmendmentRequestMsgRequestServiceReference;
using Logitude.Customs.BL.Utils;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class DF_MSG8235_TransshipmentDeclarationAmendmentRequestService : RequestServiceBase<DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg, AmendmentRequestParams>
    {
        private DF_MSG8235_ExportDeclarationAmendmentRequestService exportDeclarationAmentmentRequestService = new DF_MSG8235_ExportDeclarationAmendmentRequestService();

        public override void ManipulateRequestParams(AmendmentRequestParams requestParams) =>
            exportDeclarationAmentmentRequestService.ManipulateRequestParams(requestParams);

        public override void PostGetRequest(DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg customRequest, AmendmentRequestParams requestParams)
        {
            UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference.DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg castCustomRequest =
                 Serializer.CastXML<UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference.DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg, DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg>(customRequest);

            exportDeclarationAmentmentRequestService.PostGetRequest(castCustomRequest, requestParams);
        }

        public override DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg GetRequest(AmendmentRequestParams requestParams)
        {
            UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference.DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg req = exportDeclarationAmentmentRequestService.GetRequest(requestParams);
            DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg castreq = Serializer.CastXML<DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg, UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference.DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg>(req);
            return castreq;

        }
    }
}