using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using UnifreightIIG.Common.MessageLib.Claim;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CLAIM_2300_MissingDocumentRequestResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, CLAIM_MSG7_MissingDocumentRequest, GenericRequestParams>
    {
        public override void Update(CLAIM_MSG7_MissingDocumentRequest customResponse, GenericRequestParams requestParams)
        {
            //Analyze Message 2300 - Claim Required Document (DCA)
            ICustomContext myDbContext = CustomContext.GetContext(requestParams.Tenant);
            var claimsRelatedEntityQueryService = new ClaimsRelatedEntityQueryService(myDbContext);
            var myCustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(myDbContext);
            var myCustomsDocumentPointerUpdateService = new CustomsDocumentPointerUpdateService(myDbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(myDbContext);
            var myCustomsDocumentsTicketUpdateService = new CustomsDocumentsTicketUpdateService(myDbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var claimQueryService = new ClaimQueryService(myDbContext);

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "בקשת מסמכים לתביעה " + customResponse.MissingDocumentRequest.TPGIdentifier.fileNumber;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Claim");
            this.MyRequestSheetParam.RequestDescription = "בקשת מסמכים לתביעה " + customResponse.MissingDocumentRequest.TPGIdentifier.fileNumber;

            ClaimsRelatedEntityPM claimsRelatedEntityPM = claimsRelatedEntityQueryService.GetRelatedEntityByTapagNumber(customResponse.MissingDocumentRequest.TPGIdentifier.fileNumber.ToString(), customResponse.MissingDocumentRequest.TPGIdentifier.numeral, requestParams.Tenant);
            if (claimsRelatedEntityPM == null)
            {
                LogMessagingUtil.Instance.AppendLine("Can not find claim file " + customResponse.MissingDocumentRequest.TPGIdentifier.fileNumber + "-" + customResponse.MissingDocumentRequest.TPGIdentifier.numeral);
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "לא נמצא תיק תביעה " + customResponse.MissingDocumentRequest.TPGIdentifier.fileNumber + "-" + customResponse.MissingDocumentRequest.TPGIdentifier.numeral;              
                return;
            }

            this.MyResponseData.ApplicationID = claimsRelatedEntityPM.ClaimId;

            this.MyRequestSheetParam.EntityId1 = claimsRelatedEntityPM.ClaimId;
            if (claimsRelatedEntityPM.ClaimEntityTypeCode == "1055" && !string.IsNullOrWhiteSpace(claimsRelatedEntityPM.ExternalClaimNumber))
            {
                this.MyRequestSheetParam.CustomFileNo = claimsRelatedEntityPM.ExternalClaimNumber;
            }

            foreach (var customsDocumentItem in customResponse.MissingDocumentList)
            {
                CustomsDocumentPointerPM customsDocumentPointerPM = new CustomsDocumentPointerPM();
                CustomsDocumentsTicketPM customsDocumentsTicketPM = new CustomsDocumentsTicketPM();

                List<CustomsDocumentPointerPM> customsDocumentPointerList = myCustomsDocumentPointerQueryService.GetCustomsDocumentPointerPMsByRequiredDocID(customsDocumentItem.documentID.ToString(), requestParams.Tenant);
                if (customsDocumentPointerList == null || customsDocumentPointerList.Count == 0)
                {
                    customsDocumentsTicketPM.ChangeSetOp = ChangeSetOperation.Insert;
                    customsDocumentPointerPM.ChangeSetOp = ChangeSetOperation.Insert;
                }
                else
                {
                    customsDocumentPointerPM = myCustomsDocumentPointerQueryService.GetSingle(customsDocumentPointerList.FirstOrDefault().Id, true, false);
                    customsDocumentPointerPM.ChangeSetOp = ChangeSetOperation.Update;
                    customsDocumentsTicketPM = myCustomsDocumentsTicketQueryService.GetSingle(customsDocumentPointerPM.CustomsDocumentsTicketId, true, false);
                    customsDocumentsTicketPM.ChangeSetOp = ChangeSetOperation.Update;

                    if (!string.IsNullOrWhiteSpace(customsDocumentsTicketPM.DocumentsFilingId))
                    {
                        this.MyResponseData.HasException = true;
                        this.MyResponseData.UserMessage = "קיימת כבר בקשה לדרישה למסמך מספר " + customsDocumentItem.documentID.ToString() + " ולדרישה זו כבר קושר מסמך " + "\n" + "לא בוצע ניתוח למסר זה";
                        LogMessagingUtil.Instance.AppendLine("קיימת כבר בקשה לדרישה למסמך מספר " + customsDocumentItem.documentID.ToString() + " ולדרישה זו כבר קושר מסמך ");
                        continue;
                    }
                }

                // Create Documents Ticket 
                customsDocumentsTicketPM.Tenant = requestParams.Tenant;
                //customsDocumentsTicketPM.DocumentsFilingId = myDocumentId;
                customsDocumentsTicketPM.DocumentTypeCode = customsDocumentItem.documentCode.ToString();
                customsDocumentsTicketPM.Remarks = customsDocumentItem.remark;
                customsDocumentsTicketPM.RequestedCustomsDocId = customsDocumentItem.documentID.ToString();
                myCustomsDocumentsTicketUpdateService.Update(customsDocumentsTicketPM, true);

                // Create Document Pointer (every document pointer has a ticket)
                customsDocumentPointerPM.Tenant = requestParams.Tenant;
                customsDocumentPointerPM.CustomsDocumentsTicketId = customsDocumentsTicketPM.Id;
                customsDocumentPointerPM.ParentEntityCode = "Claim";
                customsDocumentPointerPM.ParentEntityId = claimsRelatedEntityPM.ClaimId;
                customsDocumentPointerPM.Child1EntityCode = "ClaimsRelatedEntity";
                customsDocumentPointerPM.Child1EntityId = claimsRelatedEntityPM.EntityCounterKey.ToString();
                customsDocumentPointerPM.Child2EntityCode = "";
                customsDocumentPointerPM.Child2EntityId = "";

                customsDocumentPointerPM.DocumentStatusCode = "3";
                customsDocumentPointerPM.DocumentRemarks = customsDocumentsTicketPM.Remarks;
                customsDocumentPointerPM.DocumentTypeCode = customsDocumentsTicketPM.DocumentTypeCode;

                string customOfficeName = GetCustomOfficeName(customResponse.MissingDocumentRequest.customOfficeNumber, claimsRelatedEntityPM.Tenant);
                string unitName = GetUnitName(customResponse.MissingDocumentRequest.unitCode, claimsRelatedEntityPM.Tenant);
                ClaimPM claimPM = claimQueryService.GetSingle(claimsRelatedEntityPM.ClaimId, false, true);

                string description = "תיק תביעה- " + customResponse.MissingDocumentRequest.TPGIdentifier.fileNumber.ToString() + " - " + customResponse.MissingDocumentRequest.TPGIdentifier.numeral + "\n"
                            + "תיק מוביל- " + customResponse.MissingDocumentRequest.leadingFileNumber + "\n"
                            + "תחנת מכס- " + customOfficeName + "\n"
                            + "יחידה מקצועית- " + unitName;

                EventContextTagModel myInsertEventContextTagModel = new EventContextTagModel();
                myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.CLAIM_2300_MissingDocumentRequestResponseService;
                myInsertEventContextTagModel.MyNotificationPM = new NotificationPM();
                myInsertEventContextTagModel.MyNotificationPM.NotificationDefinitionCode = "2300N";
                myInsertEventContextTagModel.MyNotificationPM.Description = description;
                myInsertEventContextTagModel.MyNotificationPM.EntityId = claimsRelatedEntityPM.ClaimId;
                myInsertEventContextTagModel.MyNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Claim");
                myInsertEventContextTagModel.MyNotificationPM.Reference1Number = claimsRelatedEntityPM.ExternalClaimNumber;
                myInsertEventContextTagModel.MyNotificationPM.CustomerId = claimPM.CustomerId;
                myInsertEventContextTagModel.MyNotificationPM.AssigneToId = claimPM.ReferantId;
                
                customsDocumentPointerPM.CustomsRequestsSheetId = requestParams.CustomsRequestsSheetId;
                customsDocumentPointerPM.CurrentContextTag = myInsertEventContextTagModel;
                customsDocumentPointerPM.CustomsDocId = customsDocumentItem.documentID.ToString();

                myCustomsDocumentPointerUpdateService.Update(customsDocumentPointerPM, true);
            }
        }

        public override INF_MSG_GenericResponseData GetResponse(CLAIM_MSG7_MissingDocumentRequest customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private string GetCustomOfficeName(int customOfficeNumber, int tenant)
        {
            string description = null;
            if (customOfficeNumber > 0)
            {
                CustomsHouseTypeQueryService customsHouseTypeQueryService = new CustomsHouseTypeQueryService(tenant);
                CustomsHouseTypePM customsHouseTypePM = customsHouseTypeQueryService.GetSingle(customOfficeNumber.ToString(), false, true);
                if (customsHouseTypePM != null)
                {
                    description = customsHouseTypePM.LocalName;
                }
            }

            return description;
        }

        private string GetUnitName(int unitCode, int tenant)
        {
            string unitName = null;
            if (unitCode > 0)
            {
                OrganizationUnitTypeQueryService organizationUnitTypeQueryService = new OrganizationUnitTypeQueryService(tenant);
                OrganizationUnitTypePM organizationUnitTypePM = organizationUnitTypeQueryService.GetSingle(unitCode.ToString(), false, true);
                if (organizationUnitTypePM != null)
                {
                    unitName = organizationUnitTypePM.LocalName;
                }
            }
            return unitName;
        }
    }
}
