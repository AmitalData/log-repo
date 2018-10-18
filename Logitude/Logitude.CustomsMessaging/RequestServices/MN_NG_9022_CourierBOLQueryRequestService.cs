
                                                                        //Yuval Chalup 29.10.2015 TASK-16002
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CourierBOLQueryServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class MN_NG_9022_CourierBOLQueryRequestService
        : RequestServiceBase<MN_NG_9022_CourierBOLQuery_Message, CourierBOLQueryRequestParams>
    {
        public override MN_NG_9022_CourierBOLQuery_Message GetRequest(CourierBOLQueryRequestParams requestParams)
        {
            var myMN_NG_9022_CourierBOLQuery_Message = new MN_NG_9022_CourierBOLQuery_Message();
            myMN_NG_9022_CourierBOLQuery_Message.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            myMN_NG_9022_CourierBOLQuery_Message.CourierBillOfLading = new MN_NG_9022_CourierBOLQuery_MessageCourierBillOfLading();

            myMN_NG_9022_CourierBOLQuery_Message.CourierBillOfLading.CourierBillOfLadingNumber = requestParams.CourierBOL;
            myMN_NG_9022_CourierBOLQuery_Message.CourierBillOfLading.CourierNumber = requestParams.CourierVAT;


            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "שאילתא לשטרי מטען בלדר";
            if (!string.IsNullOrEmpty(requestParams.DeclarationId))
            {
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId1 = requestParams.DeclarationId;
                this.MyRequestSheetParam.CustomFileNo = requestParams.CustomFileNo;
            }

            return myMN_NG_9022_CourierBOLQuery_Message;
        }
    }
}
