using System;


namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class GetAttachmentRequestParams : RequestParamsBase
    {
        public int customsItemId { get; set; }
        public DateTime validToDate { get; set; }
        public int languageType { get; set; }
    }
}
