using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ContinuousRequestOnClaimFileServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class CLAIM_5005_ContinuousRequestOnClaimFileRequestService : RequestServiceBase<CLAIM_MSG9_ContinuousRequestOnClaimFile, ContinuousRequestOnClaimFileRequestParams>
    {
        private ClaimPM _ClaimPM;
        private ICustomContext _DbContext;

        public override CLAIM_MSG9_ContinuousRequestOnClaimFile GetRequest(ContinuousRequestOnClaimFileRequestParams requestParams)
        {
            var myCLAIM_MSG9_ContinuousRequestOnClaimFile = new CLAIM_MSG9_ContinuousRequestOnClaimFile();
            myCLAIM_MSG9_ContinuousRequestOnClaimFile.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            this._DbContext = CustomContext.GetContext(requestParams.Tenant);
            ClaimQueryService myClaimQueryService = new ClaimQueryService(this._DbContext);
            _ClaimPM = myClaimQueryService.GetSingle(requestParams.AppicationId, true, false);

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Claim");
            this.MyRequestSheetParam.EntityId1 = requestParams.AppicationId;
            this.MyRequestSheetParam.RequestDescription = "בקשה לביטול/ערר תביעה";

            if (_ClaimPM != null && _ClaimPM.ClaimsRelatedEntities != null && _ClaimPM.ClaimsRelatedEntities.Count > 0)
            {
                ClaimsRelatedEntityPM myClaimsRelatedEntityPM = _ClaimPM.ClaimsRelatedEntities.Where(r => r.EntityCounterKey.ToString() == requestParams.ClaimRelatedEntityCounterKey).FirstOrDefault();
                if (myClaimsRelatedEntityPM.ClaimEntityTypeCode == "1055" && !string.IsNullOrWhiteSpace(myClaimsRelatedEntityPM.ExternalClaimNumber))
                {
                    this.MyRequestSheetParam.CustomFileNo = myClaimsRelatedEntityPM.ExternalClaimNumber;
                }
                this.MyRequestSheetParam.RequestDescription = "בקשה לביטול/ערר תביעה " + myClaimsRelatedEntityPM.TapagNumber;

                myCLAIM_MSG9_ContinuousRequestOnClaimFile.ContinuousRequestOnClaimFile = new CLAIM_MSG9_ContinuousRequestOnClaimFileContinuousRequestOnClaimFile();
                int continuousRequestTypeCode;
                int.TryParse(myClaimsRelatedEntityPM.ContinuousRequestTypeCode, out continuousRequestTypeCode);
                myCLAIM_MSG9_ContinuousRequestOnClaimFile.ContinuousRequestOnClaimFile.continuousRequestType = continuousRequestTypeCode;
                myCLAIM_MSG9_ContinuousRequestOnClaimFile.ContinuousRequestOnClaimFile.messageSourceCode = 6;
                myCLAIM_MSG9_ContinuousRequestOnClaimFile.ContinuousRequestOnClaimFile.explanation = myClaimsRelatedEntityPM.Explanation;
                myCLAIM_MSG9_ContinuousRequestOnClaimFile.ContinuousRequestOnClaimFile.TPGIdentifier = new TPGIdentifier();
                myCLAIM_MSG9_ContinuousRequestOnClaimFile.ContinuousRequestOnClaimFile.TPGIdentifier.fileNumber = myClaimsRelatedEntityPM.TapagNumber;
                if (myClaimsRelatedEntityPM.Numeral != null)
                {
                    myCLAIM_MSG9_ContinuousRequestOnClaimFile.ContinuousRequestOnClaimFile.TPGIdentifier.numeral = (int)myClaimsRelatedEntityPM.Numeral;
                }

                myCLAIM_MSG9_ContinuousRequestOnClaimFile.RequestSubmiter = new CLAIM_MSG9_ContinuousRequestOnClaimFileRequestSubmiter();
                if (!string.IsNullOrWhiteSpace(_ClaimPM.ClaimSubmiterNumber))
                {
                    int claimSubmiterID = 0;
                    int.TryParse(_ClaimPM.ClaimSubmiterNumber, out claimSubmiterID);
                    myCLAIM_MSG9_ContinuousRequestOnClaimFile.RequestSubmiter.submiterID = claimSubmiterID;
                }
                myCLAIM_MSG9_ContinuousRequestOnClaimFile.RequestSubmiter.submiterTypeCode = 3;

                myCLAIM_MSG9_ContinuousRequestOnClaimFile.Attachment = GetClaimAttachment(myClaimsRelatedEntityPM.EntityCounterKey);
            }

            return myCLAIM_MSG9_ContinuousRequestOnClaimFile;
        }

        private Attachment[] GetClaimAttachment(int entityCounterKey)
        {
            var claimAttachmentList = new List<Attachment>();
            var customsDocumentQueryService = new CustomsDocumentQueryService(_DbContext);

            //Get Claims Attachments
            var customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = this._ClaimPM.Id, ParentEntityCode = "Claim", Child1EntityCode = "ClaimRelatedEntityCancelOrObjection", Child1EntityId = entityCounterKey.ToString() }, this._ClaimPM.Tenant);
            foreach (CustomsDocumentPM customsDocumentPM in customsDocumentPMList)
            {
                if (!string.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId))
                {
                    var claimAttachment = new Attachment();
                    claimAttachment.documentType = customsDocumentPM.DocumentTypeCode;
                    claimAttachment.fileName = customsDocumentPM.Name;
                    claimAttachment.externalAttachmentID = customsDocumentPM.ExternalAttachmentId;
                    claimAttachment.IsAttachment = false.ToString();
                    //claimAttachment.Remark = customsDocumentPM.DocumentRemarks;
                    //claimAttachment.content = customsDocumentPM.

                    claimAttachmentList.Add(claimAttachment);
                }
            }

            return claimAttachmentList.ToArray();
        }
    }
}
