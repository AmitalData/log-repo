using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using UnifreightIIG.Common.GetAttachmentServiceReference;


namespace Logitude.CustomsMessaging.ResponseServices
{
    public class GetDOC8318_Web_GetAttachmentResponseService : ResponseServiceBase<AttachmentResponseData, DOC_Web_GetAttachment_OUT, GetAttachmentRequestParams>
    {
        public override void Update(DOC_Web_GetAttachment_OUT customResponse, GetAttachmentRequestParams requestParams)
        {
            this.MyResponseData = new AttachmentResponseData();
            if (customResponse?.Attachment == null || customResponse?.Attachment?.content == null)
            {
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = false;
                this.MyResponseData.UserMessage = "לא התקבל קובץ של פרטי תדפיסי חקיקה";
                return;
            }

            AttachedMekahFileData attachedMekahFileData = new AttachedMekahFileData();
            attachedMekahFileData.fileName = customResponse.Attachment.fileName;
            attachedMekahFileData.content = Convert.ToBase64String(customResponse.Attachment.content);

            this.MyResponseData = new AttachmentResponseData
            {
                AttachedMekahFileData = attachedMekahFileData
            };


            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = " פרטי קובץ תדפיסי חקיקה התקבלו בהצלחה";
        }
        public override AttachmentResponseData GetResponse(DOC_Web_GetAttachment_OUT customResponse, GetAttachmentRequestParams requestParams)
        {
            return this.MyResponseData;
        }

    }
}

