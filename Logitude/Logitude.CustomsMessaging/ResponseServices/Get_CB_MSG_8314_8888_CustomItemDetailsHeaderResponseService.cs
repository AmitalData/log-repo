using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityLists;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CustomItemDetailsServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public  class Get_CB_MSG_8314_8888_CustomItemDetailsHeaderResponseService: ResponseServiceBase<CB_NG_8314_8888_CustomsItemDetailsResponseData, CB_NG_8888_CustomsItemOut, CD_NG_8314_Web01_CustomsItemDetailsRequestParams>
    {
        public override CB_NG_8314_8888_CustomsItemDetailsResponseData GetResponse(
            CB_NG_8888_CustomsItemOut customResponse, CD_NG_8314_Web01_CustomsItemDetailsRequestParams requestParams)
        {
            bool succeeded = false;
            bool hasException = false;
            string exceptionMessage = null;

            if (customResponse.CustomsItem != null)
            {
                succeeded = true;
            }
           

            CB_NG_8314_8888_CustomsItemDetailsResponseData responseData = new CB_NG_8314_8888_CustomsItemDetailsResponseData() { Succeeded = succeeded, HasException = hasException, UserMessage = exceptionMessage };
            responseData.CustomsItemList = new List<CustomsItem>();
             foreach (var item in customResponse.CustomsItem)
             {

                responseData.CustomsItemList.Add(new CustomsItem()
                {
                    fullClassification = item.fullClassification,
                    statisticMeasurementUnitCode = item.statisticMeasurementUnitCode,
                    isDiscountCode = item.isDiscountCode,
                    goodsDescription = item.GoodsDescription,
                    customsBookTypeName = item.CustomsBookTypeName

                });


           }
            return responseData;


           
        }
        public override void Update(CB_NG_8888_CustomsItemOut customResponse, CD_NG_8314_Web01_CustomsItemDetailsRequestParams requestParams)
        {
            this.MyResponseData = new CB_NG_8314_8888_CustomsItemDetailsResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;




        }

    }
}
