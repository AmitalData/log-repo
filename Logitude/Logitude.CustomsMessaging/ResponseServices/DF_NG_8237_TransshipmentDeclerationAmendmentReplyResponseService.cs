using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using UnifreightIIG.Common.TransshipmenDeclarationAmendmentRequestMsgRequestServiceReference;
using Logitude.Customs.BL.Utils;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_8237_TransshipmentDeclerationAmendmentReplyResponseService : ResponseServiceBase<ExportDeclarationAmendmentResponseData, DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg, AmendmentRequestParams>
    {
        public override void Update(DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg customResponse, AmendmentRequestParams requestParams)
        {
            UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference.DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg castCustomResponse =
               Serializer.CastXML<UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference.DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg, DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg>(customResponse);

            var service = new DF_NG_8237_ExportDeclerationAmendmentReplyResponseService();
            service.Update(castCustomResponse, requestParams);

            MyResponseData = service.MyResponseData;
        }

        public override ExportDeclarationAmendmentResponseData GetResponse(DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg customResponse, AmendmentRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
