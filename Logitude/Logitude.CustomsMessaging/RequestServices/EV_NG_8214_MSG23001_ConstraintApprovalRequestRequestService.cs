using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ConstraintApprovalRequestServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class EV_NG_8214_MSG23001_ConstraintApprovalRequestRequestService
        : RequestServiceBase<EV_NG_8214_MSG23001_ConstraintApprovalRequest, ConstraintApprovalRequestParams>
    {
        public override EV_NG_8214_MSG23001_ConstraintApprovalRequest GetRequest(ConstraintApprovalRequestParams requestParams) 
        {
            //Build request 8214 - Message Request for constraints approval
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var declarationQueryService = new DeclarationQueryService(dbContext);
            var myEV_NG_8214_MSG23001_ConstraintApprovalRequest = new EV_NG_8214_MSG23001_ConstraintApprovalRequest();

            var declarationConstraintPM = declarationQueryService.GetSingle(requestParams.DeclarationId, false, false);
            if (declarationConstraintPM == null)
            {
                throw new System.Exception("ConstraintApprovalRequest: \nDeclarationId:" + requestParams.DeclarationId + " is missing");
            }

            myEV_NG_8214_MSG23001_ConstraintApprovalRequest.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            myEV_NG_8214_MSG23001_ConstraintApprovalRequest.ConstraintApprovalRequest = new EV_NG_8214_MSG23001_ConstraintApprovalRequestConstraintApprovalRequest();
            myEV_NG_8214_MSG23001_ConstraintApprovalRequest.ConstraintApprovalRequest.LeadDocumentIDNUM = declarationConstraintPM.DeclarationNumber;

            var myAgentReason = new EV_NG_8214_MSG23001_ConstraintApprovalRequestConstraintApprovalRequestAgentReason();
            int constraintNumber;
            int.TryParse(requestParams.ConstraintNumber, out constraintNumber);
            
            myAgentReason.constraintId = constraintNumber;
            myAgentReason.agentReasonDescription = requestParams.AgentExplanation;

            myEV_NG_8214_MSG23001_ConstraintApprovalRequest.ConstraintApprovalRequest.AgentReason = new EV_NG_8214_MSG23001_ConstraintApprovalRequestConstraintApprovalRequestAgentReason[1];
            myEV_NG_8214_MSG23001_ConstraintApprovalRequest.ConstraintApprovalRequest.AgentReason[0] = myAgentReason;

            //TODO - handel AddAGlobalScannedAttachmentToEntity - add document (not mandatory)
            //TODO - change requestParams - to check if geting a list or handeling every time with only one constraint??? i changed requestParams - getting a list!!!

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "בקשה לאישור אילוץ " + constraintNumber;
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.EntityId1 = requestParams.DeclarationId;
            this.MyRequestSheetParam.CustomFileNo = declarationConstraintPM.CustomFileNo;

            return myEV_NG_8214_MSG23001_ConstraintApprovalRequest;
        }
    }
}
