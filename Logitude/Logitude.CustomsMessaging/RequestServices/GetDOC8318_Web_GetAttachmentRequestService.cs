using Logitude.CustomsMessaging.Common.RequestParams;
using UnifreightIIG.Common.CustomItemMekachServiceReference;
using UnifreightIIG.Common.GetAttachmentServiceReference;


namespace Logitude.CustomsMessaging.RequestServices
{
    public class GetDOC8318_Web_GetAttachmentRequestService : RequestServiceBase<DOC_Web_GetAttachment_IN, GetAttachmentRequestParams>
    {
        public override DOC_Web_GetAttachment_IN GetRequest(GetAttachmentRequestParams requestParams)
        {
            DOC_Web_GetAttachment_IN myMsg = new DOC_Web_GetAttachment_IN();
            
            // TODO: fix the params to send to the service:

            //myMsg.CIMekachIn = new UnifreightIIG.Common.GetAttachmentServiceReference.CustomsBookItemHeaderIn()
            //{
            //    customsItemId = requestParams.customsItemId,
            //    customsItemIdSpecified = true,
            //    validToDate = requestParams.validToDate,
            //    languageType = requestParams.languageType,
            //};



            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "תדפיסי חקיקה- קובץ (מקח''ים) - ספר סיווג";
            return myMsg;

        }
    }
}
