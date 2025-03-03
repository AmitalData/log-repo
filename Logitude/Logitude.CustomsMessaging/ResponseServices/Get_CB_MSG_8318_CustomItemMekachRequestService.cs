using Logitude.Customs.BL.BL;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using RtfPipe;
using System;
using UnifreightIIG.Common.CustomItemMekachServiceReference;


namespace Logitude.CustomsMessaging.ResponseServices
{
    public class Get_CB_MSG_8318_CustomItemMekachResponseService : ResponseServiceBase<CustomItemMekachResponseData, CB_NG_8318_CustomItemMekachOut, CustomItemMekachRequestParams>
    {
        public override void Update(CB_NG_8318_CustomItemMekachOut customResponse, CustomItemMekachRequestParams requestParams)
        {

            if (customResponse?.CIMekachOut == null)
            {
                this.MyResponseData = new CustomItemMekachResponseData();
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = false;
                this.MyResponseData.UserMessage = "לא התקבלו פרטי תדפיסי חקיקה";
                return;
            }
            //string rtf = customResponse.CIMekachOut?.;

            //this.MyResponseData = new CustomItemMekachResponseData()
            //{
            //    mekachNumber = customResponse.CIMekachOut.mekachNumber,
            //    changeDescription = customResponse.CIMekachOut.,
            //    validityDate = customResponse.CIMekachOut.validityDate,
            //    attachedMekahFile = customResponse.CIMekachOut.attachedMekah


            //};
                //classificationGuidanceTypeName = customResponse.ClassifGuidanceDetailsOut.GeneraClassifGuidanceDetailsOut.classificationGuidanceTypeName,

            //if (customResponse.CIMekachOut?.atta != null)
            //{
            //    foreach (var item in customResponse.CIMekachOut.ClassifGuidanceAttachedCI)
            //    {
            //        MyResponseData.classifGuidanceAttached.Add(new ClassifGuidanceAttached()
            //        {
            //            fullClassification = item.fullClassification,
            //            attachedCustomsItemID = item.attachedCustomsItemID
            //        });
            //    }
            //}


            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = " פרטי תדפיסי חקיקה התקבלו בהצלחה";


        }

        public override CustomItemMekachResponseData GetResponse(CB_NG_8318_CustomItemMekachOut customResponse, CustomItemMekachRequestParams requestParams)
        {
            return this.MyResponseData;
        }

    }
}
