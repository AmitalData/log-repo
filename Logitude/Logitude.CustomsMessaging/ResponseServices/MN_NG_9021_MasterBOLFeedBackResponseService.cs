using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MasterBOLQueryServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class MN_NG_9021_MasterBOLFeedBackResponseService : ResponseServiceBase<MasterBOLFeedBackResponseData, MN_NG_9021_MasterBOLFeedBack_Message, MasterBOLQueryRequestParams>
    {

        public override void Update(MN_NG_9021_MasterBOLFeedBack_Message customResponse, MasterBOLQueryRequestParams requestParams)
        {
            //Analayze Message 9021- Master BOL FeedBack
            List<InternalCargoResult> internalCargoResultList = new List<InternalCargoResult>();

            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData = new MasterBOLFeedBackResponseData();
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                return;
            }

            foreach (var internalCargoItem in customResponse.InternalCargos)
            {
                InternalCargoResult internalCargoResult = new InternalCargoResult();
                internalCargoResult.CargoIdentifierKey3 = internalCargoItem.cargoIdentifier.cargoIdentifierKey3;
                internalCargoResult.PacakgesQuantity = internalCargoItem.PacakgesQuantity.ToString();
                internalCargoResult.TotalWheight = internalCargoItem.TotalWheight.ToString();
                internalCargoResult.Submitter = internalCargoItem.Submitter; 

                 internalCargoResultList.Add(internalCargoResult);
            }

            this.MyResponseData = new MasterBOLFeedBackResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "ניתוח בוצע בהצלחה";
            this.MyResponseData.InternalCargosList = internalCargoResultList;
            return;

        }

        public override MasterBOLFeedBackResponseData GetResponse(MN_NG_9021_MasterBOLFeedBack_Message customResponse, MasterBOLQueryRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
