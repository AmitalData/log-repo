using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
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
            UpdateDeclarationPendings(customResponse);

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            //this.MyRequestSheetParam.CustomFileNo = customResponse.CustomFileNo;
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyResponseData.Succeeded = true;
            //this.MyResponseData.ApplicationID = customResponse.Declarationid;            
            this.MyResponseData.UserMessage = "ההצהרות עודכנו";
        }

        public void UpdateDeclarationPendings(DCAInUCBUCADPEResponseContentHeader customResponse)
        {
            int i = 0;
            AddMultiPendingsRequestParams rp = customResponse.requestParamsData;
            ICustomContext customContext = CustomContext.GetContext(customResponse.tenant);

            List<string> declarationIdsList = rp.checkboxAll ?
               new DeclarationCourierStatusListQueryService(customContext).GetDeclarationCourierStatusListPendingBulk(customResponse.queryOperations, customResponse.tenant).Select(x => x.DeclarationId).ToList() :
                rp.declarationIdsList.ToList();


            if (rp.checkboxAll && rp.allWithoutdeclarationIdsList != null && rp.allWithoutdeclarationIdsList.Count() > 0)
                declarationIdsList.RemoveAll(x => rp.allWithoutdeclarationIdsList.Contains(x));


            rp.listPending.ToList().ForEach(pendingCode =>
            {
                declarationIdsList.ForEach(declarationId =>
                    UpdateDeclarationPending(customResponse.tenant, declarationId, pendingCode, rp.listPendingRemark[i], customContext));

                i++;
            });
        }


        private void UpdateDeclarationPending(int tenant, string declarationId, string courierReasonCode, string pendingRemark, ICustomContext customContext)
        {
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(" UCADPA start update DeclarationId: " + declarationId + " pending code: " + courierReasonCode);
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
