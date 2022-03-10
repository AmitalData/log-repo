using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Collections.Generic;
using UnifreightIIG.Common.LogisticActionRequestMessageServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class LG_NG_8410_LogisticActionRequestMessageRequestService : RequestServiceBase<LG_NG_8410_LogisticActionRequestMessage, LogisticActionRequestRequestParams>
    {
        public override void OnRequestFail(LogisticActionRequestRequestParams requestParams)
        {
            base.OnRequestFail(requestParams);
        }

        public override LG_NG_8410_LogisticActionRequestMessage GetRequest(LogisticActionRequestRequestParams p)
        {
            var myMsg = new LG_NG_8410_LogisticActionRequestMessage()
            {
                GeneralDetails = new LG_NG_8410_LogisticActionRequestMessageGeneralDetails()
                {
                    ExporterIdentifierType = p.ExporterIdentifierType,
                    ExporterNumber = p.ExporterNumber,
                    PassportCountry = p.PassportCountry,
                    PassportNumber = p.PassportNumber,
                    RequestType = p.RequestType,
                    RequestReason = p.RequestReason,
                    DeliverySiteID = p.DeliverySiteID,
                    CargoIdentifier =  new UnifreightIIG.Common.LogisticActionRequestMessageServiceReference.cargoIdentifier() 
                    {
                        cargoIdentifierKey1 = p.CargoIdentifierKey1,
                        cargoIdentifierKey2 = p.CargoIdentifierKey2,
                        cargoIdentifierKey3 = p.CargoIdentifierKey3,
                        cargoIdentifierType = p.CargoIdentifierType,
                    },
                    PackingDetails = new LG_NG_8410_LogisticActionRequestMessageGeneralDetailsPackingDetails() 
                    {
                        PackagingTypeCode = p.PackagingTypeCode,
                        Quantity = p.Quantity,
                    }
                },
                Attachments = GetAttachments(p.LogisticActionRequestId, p.Tenant),
            };

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.LogisticActionRequest");
            this.MyRequestSheetParam.EntityId1 = p.LogisticActionRequestId;
            this.MyRequestSheetParam.CustomFileNo = p.CustomsFile;
            this.MyRequestSheetParam.RequestDescription = "בקשת ביטול יצוא";

            return myMsg;
        }


        private Attachment[] GetAttachments(string parentEntityId, int tenant)
        {
            List<Attachment> attachments = new List<Attachment>();

            var customsDocumentQueryService = new CustomsDocumentQueryService(tenant);
            var customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = parentEntityId, ParentEntityCode = "LogisticActionRequest" }, tenant);

            foreach (var customsDocumentPM in customsDocumentPMList)
            {
                if (!string.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId))
                {
                    var attachment = new Attachment();
                    attachment.externalAttachmentID = customsDocumentPM.ExternalAttachmentId;
                    attachment.IsAttachment = "false";
                    //   attachment.keywords = customsDocumentPM.Name;
                    //    attachment.fileName = customsDocumentPM.Name;

                    //  attachment.documentType = customsDocumentPM.DocumentTypeCode;
                    attachments.Add(attachment);
                }
            }
            return attachments.ToArray();

        }
    }
}
