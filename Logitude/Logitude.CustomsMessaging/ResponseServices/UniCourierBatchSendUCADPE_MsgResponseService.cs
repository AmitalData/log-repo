using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniCourierBatchSendUCADPE_MsgResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DCAInUCBUCADPEResponseContentHeader, GenericRequestParams>
    {

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBUCADPEResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCBUCADPEResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            int i = 0;
            ICustomContext customContext = CustomContext.GetContext(customResponse.tenant);

            customResponse.listPending.ToList().ForEach(pendingCode =>
            {
                customResponse.declarationIdsList.ToList().ForEach(declarationId =>
                    UpdateDeclarationPending(customResponse.tenant, declarationId, pendingCode, customResponse.listPendingRemark[i], customContext));

                i++;
            });


            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            //this.MyRequestSheetParam.CustomFileNo = customResponse.CustomFileNo;
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyResponseData.Succeeded = true;
            //this.MyResponseData.ApplicationID = customResponse.Declarationid;            
            this.MyResponseData.UserMessage = "ההצהרות עודכנו";
        }


        public void UpdateDeclarationPending(int tenant, string declarationId, string courierReasonCode, string pendingRemark, ICustomContext customContext)
        {
            DeclarationCourierStatusPM myDeclarationCourierStatusPM = new DeclarationCourierStatusQueryService(tenant).GetSingle(declarationId, true, false);
            DeclarationPendingPM declarationPendingPM = myDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == declarationId && r.CourierPendingReasonCode == courierReasonCode).FirstOrDefault();
            DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(customContext, new Dictionary<string, IContext>(), tenant);

            CourierPendingReasonRepository courierPendingReasonRepositoryRepository = new CourierPendingReasonRepository(tenant);
            bool isActive = courierPendingReasonRepositoryRepository.IsActive(courierReasonCode, myDeclarationCourierStatusPM.Tenant);

            if (!isActive) return;

            if (declarationPendingPM == null)
            {
                declarationPendingPM = new DeclarationPendingPM()
                {
                    ChangeSetOp = ChangeSetOperation.Insert,
                    DeclarationID = declarationId,
                    Tenant = tenant,
                    CourierPendingReasonCode = courierReasonCode,
                    PendingRemarks = pendingRemark,
                    Status = "A",
                };

                myDeclarationCourierStatusPM.DeclarationPendings.Add(declarationPendingPM);
            }
            else
            {
                declarationPendingPM.PendingRemarks = pendingRemark;
                declarationPendingPM.ChangeSetOp = ChangeSetOperation.Update;

                if (declarationPendingPM.Status == "S")
                    declarationPendingPM.Status = "A";
            }

            if (myDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.None)
                myDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;

            declarationCourierStatusUpdateService.Update(myDeclarationCourierStatusPM, true);
        }
    }
}
