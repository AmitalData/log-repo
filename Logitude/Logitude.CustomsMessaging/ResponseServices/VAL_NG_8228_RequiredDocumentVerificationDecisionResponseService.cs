using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Ransom;
using System.Runtime.CompilerServices;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class VAL_NG_8228_RequiredDocumentVerificationDecisionResponseService : ResponseServiceBase<RequiredDocumentResponseData, VAL_NG_8228_MSG550_RequiredDocumentVerificationDecisionMessage, RequiredDocumentRequestParams>
    {
        public override void Update(VAL_NG_8228_MSG550_RequiredDocumentVerificationDecisionMessage customResponse, RequiredDocumentRequestParams requestParams)
        {
            //Analyze message 8228- Required Document Verification Decision (DCA)
            ICustomContext commonContext = CustomContext.GetContext(requestParams.Tenant);
            var myCustomsDocumentQueryService = new CustomsDocumentQueryService(commonContext);
            var CustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(commonContext);
            var CustomsDocumentsTicketUpdateService = new CustomsDocumentsTicketUpdateService(commonContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var customsDocumentUpdateService = new CustomsDocumentUpdateService(commonContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            string requestDescription = "אימות מסמך נדרש ";

            InitMyResponseData(customResponse, requestParams);

            //Check if Tapag file is already exist
            string documentsFilingId = myCustomsDocumentQueryService.GetDocumentInIdByCustomsDocId(customResponse.GeneralDetails.documentId.ToString(), requestParams.Tenant);
            if (String.IsNullOrWhiteSpace(documentsFilingId))
            {
                LogMessagingUtil.Instance.AppendLine("Can not find document: documentId=" + customResponse.GeneralDetails.documentId);
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "Can not find document: documentId=" + customResponse.GeneralDetails.documentId;

                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.CustomsDocument");
                this.MyRequestSheetParam.EntityId2 = customResponse.GeneralDetails.documentId.ToString();
                this.MyRequestSheetParam.RequestDescription = "אימות מסמך נדרש " + customResponse.GeneralDetails.documentId;
                return;
            }

            List<CustomsDocumentsTicketPM> myCustomsDocumentsTicketPMList = CustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketsByDocumentsFilingId(documentsFilingId, requestParams.Tenant);
            CustomsDocumentsTicketPM myCustomsDocumentsTicketPM = myCustomsDocumentsTicketPMList.FirstOrDefault();
            myCustomsDocumentsTicketPM.ChangeSetOp = ChangeSetOperation.Update;

            string remarks = "";
            if (!String.IsNullOrWhiteSpace(customResponse.GeneralDetails.remarks))
            {
                remarks = "הערות כלליות - " + customResponse.GeneralDetails.remarks;
            }
            if (customResponse.VerificationDecision.rejectVerificationReason == 1)
            {
                remarks = remarks + "\n" + "סיבת דחיה - " + "מסמך אינו בסוג המסמך הנדרש";
            }
            if (customResponse.VerificationDecision.rejectVerificationReason == 2)
            {
                remarks = remarks + "\n" + "סיבת דחיה - " + "מסמך לא קריא";
            }
            if (!String.IsNullOrWhiteSpace(customResponse.VerificationDecision.rejectVerificationRemark))
            {
                remarks = remarks + "\n" + "הערות לדחיה - " + customResponse.VerificationDecision.rejectVerificationRemark;
            }
            if (!String.IsNullOrWhiteSpace(customResponse.Worker.workerName))
            {
                remarks = remarks + "\n" + "שם עובד מכס - " + customResponse.Worker.workerName;
            }
            if (customResponse.VerificationDecision.replacingDocumentIdSpecified == true)
            {
                remarks = remarks + "\n" + "מספר מסמך נדרש מחליף - " + customResponse.VerificationDecision.replacingDocumentId;
            }
            myCustomsDocumentsTicketPM.VerificationRemarks = remarks;
            EventContextTagModel myInsertEventContextTagModel = new EventContextTagModel();
            switch(customResponse.VerificationDecision.verificationDecisionType)
            {
                case 2: // אומת
                    myCustomsDocumentsTicketPM.VerificationStatusTypeCode = "4";
                    myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.VAL_NG_8228_RequiredDocumentVerificationDecisionVerified;
                    myInsertEventContextTagModel.EventCode = "RDA";
                    myInsertEventContextTagModel.EventRemarks = "Required Document Verified By Customs";
                    myInsertEventContextTagModel.FUStatusRemarks = "דרישת מסמך אומתה " + "\n" + "הערות - " + customResponse.GeneralDetails.remarks;
                    this.UpdateRequestedCustomsDocId(customResponse, requestParams,myCustomsDocumentsTicketPM);
                    break;
                case 3: // אומת בנוכחות הלקוח
                    myCustomsDocumentsTicketPM.VerificationStatusTypeCode = "5";
                    myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.VAL_NG_8228_RequiredDocumentVerificationDecisionVerifiedWithClient;
                    myInsertEventContextTagModel.EventCode = "RDA";
                    myInsertEventContextTagModel.EventRemarks = "Required Document Verified By Customs";
                    myInsertEventContextTagModel.FUStatusRemarks = "דרישת מסמך אומתה " + "\n" + "הערות - " + customResponse.GeneralDetails.remarks;
                    this.UpdateRequestedCustomsDocId(customResponse, requestParams, myCustomsDocumentsTicketPM);
                    break;
                case 4: // נדחה
                    myCustomsDocumentsTicketPM.VerificationStatusTypeCode = "6";
                    myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.VAL_NG_8228_RequiredDocumentVerificationDecisionReject;
                    myInsertEventContextTagModel.EventCode = "RDR";
                    myInsertEventContextTagModel.EventRemarks = "Required Document Rejected By Customs";
                    myInsertEventContextTagModel.FUStatusRemarks = remarks;
                    requestDescription = "דחיית מסמך נדרש ";
                    break;
            }
            myCustomsDocumentsTicketPM.CurrentContextTag = myInsertEventContextTagModel;
            CustomsDocumentsTicketUpdateService.Update(myCustomsDocumentsTicketPM, true);
            /*
            if (!String.IsNullOrWhiteSpace(myCustomsDocumentsTicketPM.VerificationStatusTypeCode)) // its not allowed to rest  customDocument.DocumentStatusCode 
            {
                if (myCustomsDocumentsTicketPM.DocumentStatusCode != myCustomsDocumentsTicketPM.VerificationStatusTypeCode)
                {
                    CustomsDocumentPM customDocument = myCustomsDocumentQueryService.GetSingle(myCustomsDocumentsTicketPM.DocumentsFilingId, false, false);
                    customDocument.DocumentStatusCode = myCustomsDocumentsTicketPM.VerificationStatusTypeCode;
                    customDocument.ChangeSetOp = ChangeSetOperation.Update;
                    customsDocumentUpdateService.Update(customDocument, true);
                }
            }*/

            this.MyRequestSheetParam = new RequestSheetParam();
            if (myCustomsDocumentsTicketPM.CustomsDocumentPointers != null)
            {
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId1 = myCustomsDocumentsTicketPM.CustomsDocumentPointers.FirstOrDefault().ParentEntityId;
            }
            this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.CustomsDocument");
            this.MyRequestSheetParam.EntityId2 = documentsFilingId;
            this.MyRequestSheetParam.RequestDescription = requestDescription + customResponse.GeneralDetails.documentId;

            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = requestDescription + customResponse.GeneralDetails.documentId;
            this.MyResponseData.Title = requestDescription;
            this.MyResponseData.VerificationDecisionType = customResponse.VerificationDecision.verificationDecisionType.ToString();
        }

        private void UpdateRequestedCustomsDocId(VAL_NG_8228_MSG550_RequiredDocumentVerificationDecisionMessage customResponse, RequiredDocumentRequestParams requestParams, CustomsDocumentsTicketPM myCustomsDocumentsTicketPM)
        {
            var customContext = CustomContext.GetContext(requestParams.Tenant);
            var CustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(customContext);

            var myDeclarationUpdateService = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myQueryService = new DeclarationQueryService(requestParams.Tenant);
            if (customResponse?.ConnectedEntity.Length > 0 && customResponse?.ConnectedEntity[0].entityIdKey1 != null)
            {
                var customfileno = myQueryService.GetCustomFileNoByDeclarationNumber(customResponse?.ConnectedEntity[0].entityIdKey1, requestParams.Tenant);
                var _MyDeclarationPM = myQueryService.GetAcceptDeclarationAmendmentByCustomsFile(customfileno, requestParams.Tenant);
                if (_MyDeclarationPM != null)
                {
                    var IsThereRequestCustomDoc = CustomsDocumentsTicketQueryService.GetIfThereRequestDocumentDocIdNotVerifiedByDeclarationId(_MyDeclarationPM.Id, myCustomsDocumentsTicketPM.Id);
                    if (!IsThereRequestCustomDoc) {
                        _MyDeclarationPM.RequestedCustomsDocId = 0;
                        _MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                        myDeclarationUpdateService.Update(_MyDeclarationPM, true);
                    }
                }
            }


        }
        private void InitMyResponseData(VAL_NG_8228_MSG550_RequiredDocumentVerificationDecisionMessage customResponse, RequiredDocumentRequestParams requestParams)
        {
            this.MyResponseData = new RequiredDocumentResponseData();
            this.MyResponseData.Succeeded = true;

            this.MyResponseData.DocumentTypeVisibility = "Collapsed";
            this.MyResponseData.ReplacingDocumentIdVisibility = "Visible";

            if (customResponse.VerificationDecision.verificationDecisionType != 4)
            {
                this.MyResponseData.Title = "אימות מסמך נדרש " + customResponse.GeneralDetails.documentId;
            }
            else
            {
                this.MyResponseData.Title = "דחיית מסמך נדרש " + customResponse.GeneralDetails.documentId;
            }
            this.MyResponseData.DocumentNumber = customResponse.GeneralDetails.documentId.ToString();
            this.MyResponseData.DocumentWorkerName = customResponse.Worker.workerName;
            this.MyResponseData.Remarks = customResponse.VerificationDecision.rejectVerificationRemark;
            if (customResponse.VerificationDecision.replacingDocumentIdSpecified == true && customResponse.VerificationDecision.replacingDocumentId > 0)
            {
                this.MyResponseData.ReplacingDocumentId = customResponse.VerificationDecision.replacingDocumentId.ToString();
            }

            this.MyResponseData.DocumentConnectedEntitiesList = new List<DocumentConnectedEntitiesResult>();
            if (customResponse.ConnectedEntity != null)
            {
                foreach (var relatedEntityItem in customResponse.ConnectedEntity)
                {
                    DocumentConnectedEntitiesResult connectedEntity = new DocumentConnectedEntitiesResult();
                    connectedEntity.EntityType = relatedEntityItem.entityType.ToString();
                    connectedEntity.EntityTypeName = GetEntityTypeName(relatedEntityItem.entityType.ToString(), requestParams.Tenant);
                    connectedEntity.EntityNumber = relatedEntityItem.entityIdKey1;
                    this.MyResponseData.DocumentConnectedEntitiesList.Add(connectedEntity);
                }
            }
        }

        public override RequiredDocumentResponseData GetResponse(VAL_NG_8228_MSG550_RequiredDocumentVerificationDecisionMessage customResponse, RequiredDocumentRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private string GetEntityTypeName(string entityTypeCode, int tenant)
        {
            string entityTypeName = "";
            if (!string.IsNullOrWhiteSpace(entityTypeCode))
            {
                EntityTypeLookupQueryService entityTypeLookupQueryService = new EntityTypeLookupQueryService(tenant);
                EntityTypeLookupPM entityTypeLookupPM = entityTypeLookupQueryService.GetSingle(entityTypeCode, false, true);
                if (entityTypeLookupPM != null)
                {
                    entityTypeName = entityTypeLookupPM.LocalName;
                }
            }
            return entityTypeName;
        }
    }
}
