using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Claim;

namespace Logitude.CustomsMessaging.ResponseServices
{
    class CLAIM_5114_AcceptanceOrRejectionClaimMessageResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, CLAIM_MSG10_AcceptanceOrRejectionClaimMessage, GenericRequestParams>
    {
        public ClaimPM _MyClaimPM;

        public override INF_MSG_GenericResponseData GetResponse(CLAIM_MSG10_AcceptanceOrRejectionClaimMessage customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(CLAIM_MSG10_AcceptanceOrRejectionClaimMessage customResponse, GenericRequestParams requestParams)
        {
            //Analyze Acceptance Or Rejection Claim Message - interface 5114
            ICustomContext myDbContext = CustomContext.GetContext(requestParams.Tenant);
            var claimsRelatedEntityQueryService = new ClaimsRelatedEntityQueryService(myDbContext);
            var myCustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(myDbContext);
            var myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(myDbContext);
            var claimQueryService = new ClaimQueryService(myDbContext);

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "אישור/דחיה תביעה ";// + customResponse.AcceptanceOrRejectionClaimMessage.TPGIdentifier.fileNumber;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Claim");
            //this.MyRequestSheetParam.RequestDescription = "אישור/דחיה תביעה " + customResponse..TPGIdentifier.fileNumber;

        }
    }
}
