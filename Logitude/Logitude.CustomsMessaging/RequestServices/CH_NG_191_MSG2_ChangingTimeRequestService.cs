using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ChangingTimeServiceReference;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class CH_NG_191_MSG2_ChangingTimeRequestService:RequestServiceBase<//CH_NG_191_MSG2_ChangingTimeRequestChangingTimeRequest
        CH_NG_191_MSG2_ChangingTimeRequest, CH_NG_191_MSG2_ChangingTimeRequestParams>
    {
        public override CH_NG_191_MSG2_ChangingTimeRequest GetRequest(CH_NG_191_MSG2_ChangingTimeRequestParams requestParams)
        {
            //Sending message 191- Changing Time Request
            ICustomContext context = CustomContext.GetContext(requestParams.Tenant);
            PhysicalCheckQueryService physicalCheckQueryService = new PhysicalCheckQueryService(context);

            PhysicalCheckPM physicalCheckPM = physicalCheckQueryService.GetSingle(requestParams.PhysicalCheckId,false,false);
            int checkId;
            int checkSiteNumber;
            int queueType;
            int requestType;
            int.TryParse(physicalCheckPM.CheckId, out checkId);
            int.TryParse(physicalCheckPM.CheckSiteCode, out checkSiteNumber);
            int.TryParse(physicalCheckPM.QueueTypeCode, out queueType);
            int.TryParse(requestParams.RequestType, out requestType);

            CH_NG_191_MSG2_ChangingTimeRequestChangingTimeRequest changingTimeRequest = new CH_NG_191_MSG2_ChangingTimeRequestChangingTimeRequest();
            changingTimeRequest.Queuetime = DateTime.Now;
            changingTimeRequest.QueuetimeSpecified = true;
            changingTimeRequest.checkId = checkId;
            changingTimeRequest.checkSiteNumber = checkSiteNumber.ToString();
            changingTimeRequest.dateSearchFrom = requestParams.DateSearchFrom;
            changingTimeRequest.dateSearchTo = requestParams.DateSearchTo;
            changingTimeRequest.dateSearchFromSpecified = requestParams.DateSearchFromSpecified;
            changingTimeRequest.dateSearchToSpecified = requestParams.DateSearchToSpecified;
            changingTimeRequest.queueType = queueType;
            changingTimeRequest.queueTypeSpecified = queueType > 0 ? true : false;
            changingTimeRequest.requestType = requestType;
            changingTimeRequest.Queuetime = requestParams.QueueDate;
            changingTimeRequest.QueuetimeSpecified = requestParams.QueueDateSpecified;
            if (requestParams.RequestType == "3")
            {
                changingTimeRequest.BringQueueForwardIndicator = requestParams.BringQueueForwardIndicator;
                changingTimeRequest.BringQueueForwardIndicatorSpecified = true;

                if (requestParams.BringQueueForwardIndicator)
                {
                    // todo: יש לשלוח את 2 השדות החדשים CH_NG_191_MSG2_ChangingTimeRequestParams

                    // after adding NUGET to db:
                    //changingTimeRequest.RequestToAdvanceAQueue = requestParams.RequestToAdvanceAQueue;
                    //changingTimeRequest.RequestDetails = requestParams.RequestDetails;
                }
            }

            var curChangingTimeRequest = changingTimeRequest;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.PhysicalCheck");
            this.MyRequestSheetParam.EntityId1 = requestParams.PhysicalCheckId;
            if (requestType == 1 || requestType == 2) 
            {
                this.MyRequestSheetParam.RequestDescription = "שינוי מועד בדיקה " + checkId;
            }
            else
            {
                this.MyRequestSheetParam.RequestDescription = "חיפוש תורים לבדיקה " + checkId;
            }
            if (physicalCheckPM.DeclarationId != null)
            {
                var declarationQueryService = new DeclarationQueryService(context);
                string customfileNumber = declarationQueryService.GetCustomFileNoByDeclarationId(physicalCheckPM.DeclarationId, physicalCheckPM.Tenant);
                this.MyRequestSheetParam.CustomFileNo = customfileNumber;
                this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId2 = physicalCheckPM.DeclarationId;
            }

            var curCH_NG_191_MSG2_ChangingTimeRequest = new CH_NG_191_MSG2_ChangingTimeRequest() { ChangingTimeRequest = curChangingTimeRequest };
            return curCH_NG_191_MSG2_ChangingTimeRequest;
            
        }
    }
}
