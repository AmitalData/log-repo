using Logitude.Customs.BL.BL;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using RtfPipe;
using System;
using System.Collections.Generic;
using System.IO;
using UnifreightIIG.Common.CustomItemMekachServiceReference;


namespace Logitude.CustomsMessaging.ResponseServices
{
    public class Get_CB_MSG_8318_CustomItemMekachResponseService : ResponseServiceBase<CustomItemMekachResponseData, CB_NG_8318_CustomItemMekachOut, CustomItemMekachRequestParams>
    {
        public override void Update(CB_NG_8318_CustomItemMekachOut customResponse, CustomItemMekachRequestParams requestParams)
        {
            this.MyResponseData = new CustomItemMekachResponseData();
            if (customResponse?.CIMekachOut == null)
            {
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = false;
                this.MyResponseData.UserMessage = "לא התקבלו פרטי תדפיסי חקיקה";
                return;
            }

            this.MyResponseData = new CustomItemMekachResponseData
            {
                CustomItemMekachDataList = new List<CustomItemMekachData>()
            };
            if (customResponse.CIMekachOut?.Length > 0)
            {
                foreach (var item in customResponse.CIMekachOut)
                {
                    this.MyResponseData.CustomItemMekachDataList.Add(new CustomItemMekachData
                        {
                            mekachNumber = item.mekachNumber,
                            changeDescription = item.changeDescription,
                            validityDate = item.validityDate,
                            attachedMekahFile = item.attachedMekahFile?.attachmentID != null ? item.attachedMekahFile?.attachmentID : ""
                    });
                }
            }
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

