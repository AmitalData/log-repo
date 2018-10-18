using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ConstraintAgentAnswerServiceReference;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;


namespace Logitude.CustomsMessaging.RequestServices
{
    public class EV_NG_8216_MSG23003_ConstraintAgentAnswerRequestService
        : RequestServiceBase<EV_NG_8216_MSG23003_ConstraintAgentAnswer, ConstraintAgentObjectionRequestParams>
    {
        public override EV_NG_8216_MSG23003_ConstraintAgentAnswer GetRequest(ConstraintAgentObjectionRequestParams requestParams)
        {
            //Build 8216 message
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(dbContext);
            var myEV_NG_8216_MSG23003_ConstraintAgentAnswer = new EV_NG_8216_MSG23003_ConstraintAgentAnswer();
            int result;
            string declarationNumber="";

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.EntityId1 = requestParams.DeclarationId;
            this.MyRequestSheetParam.RequestDescription = "ערעור על החלטת עבור אילוץ " + requestParams.ConstraintNumber;

            //Get declaration
            var myDeclarationPM = myDeclarationQueryService.GetSingle(requestParams.DeclarationId, false, false);
            if (myDeclarationPM == null)
            {
                throw new System.Exception("ConstraintApprovalRequest: \nDeclarationId:" + requestParams.DeclarationId + " is missing");
            }

            this.MyRequestSheetParam.CustomFileNo = myDeclarationPM.CustomFileNo;

            if (!int.TryParse(requestParams.ConstraintNumber, out result))
            {
                return null;
            }

            if (!string.IsNullOrEmpty(myDeclarationPM.DeclarationNumber))
            {
                declarationNumber = myDeclarationPM.DeclarationNumber;
            }

            myEV_NG_8216_MSG23003_ConstraintAgentAnswer.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            myEV_NG_8216_MSG23003_ConstraintAgentAnswer.ConstraintAgentAnswer = new EV_NG_8216_MSG23003_ConstraintAgentAnswerConstraintAgentAnswer()
            {
                constraintId = result,
                agentObjection = requestParams.AgentObjection,
                LeadDocumentIDNum = declarationNumber
            };

            return myEV_NG_8216_MSG23003_ConstraintAgentAnswer;
        }
    }
}
