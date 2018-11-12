using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MasterBOLQueryServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class MN_NG_9020_MasterBOLQueryRequestService : RequestServiceBase<MN_NG_9020_MasterBOLQuery_Message, MasterBOLQueryRequestParams>
    {
        public override MN_NG_9020_MasterBOLQuery_Message GetRequest(MasterBOLQueryRequestParams requestParams)
        {
            //Build request 9020 - Message Request for Master BOL Query
            MN_NG_9020_MasterBOLQuery_Message myMN_NG_9020_MasterBOLQuery_Message = new MN_NG_9020_MasterBOLQuery_Message();
            myMN_NG_9020_MasterBOLQuery_Message.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            myMN_NG_9020_MasterBOLQuery_Message.Date = requestParams.Date;
            myMN_NG_9020_MasterBOLQuery_Message.MasterBillOfLading = requestParams.MasterBillOfLading;
            if(!String.IsNullOrEmpty(requestParams.InternalIdentifier))//task 43644 21.10.18
            {
                if (requestParams.ExactMatch)//Task 44705
                {
                    myMN_NG_9020_MasterBOLQuery_Message.InternalIdentifier = "=" + requestParams.InternalIdentifier;
                }
                else
                {
                    myMN_NG_9020_MasterBOLQuery_Message.InternalIdentifier = requestParams.InternalIdentifier;
                }
            }
            //myMN_NG_9020_MasterBOLQuery_Message.InternalIdentifier = requestParams.InternalIdentifier;
            myMN_NG_9020_MasterBOLQuery_Message.ReturnAllinternalCargos = requestParams.ReturnAllInernalCargos;
            myMN_NG_9020_MasterBOLQuery_Message.ReturnAllinternalCargosSpecified = requestParams.ReturnAllInernalCargos == true ? true : false;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "שאילתא לשטרי מטען " + requestParams.MasterBillOfLading;
            if (!string.IsNullOrEmpty(requestParams.DeclarationId))
            {
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId1 = requestParams.DeclarationId;
                this.MyRequestSheetParam.CustomFileNo = requestParams.CustomFileNo;
            }

            return myMN_NG_9020_MasterBOLQuery_Message;
        }
    }
}
