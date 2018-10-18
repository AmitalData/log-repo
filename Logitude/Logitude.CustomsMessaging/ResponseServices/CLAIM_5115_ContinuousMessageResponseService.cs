using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using UnifreightIIG.Common.MessageLib.Claim;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CLAIM_5115_ContinuousMessageResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, CLAIM_MSG8_ContinuousMessage, GenericRequestParams>
    {
        private ClaimPM _MyClaimPM;
        private int _MyTenant;

        public override void Update(CLAIM_MSG8_ContinuousMessage customResponse, GenericRequestParams requestParams)
        {
            //Analyze Message 5115 - Claim Messages For Agent (DCA)
            this._MyTenant = requestParams.Tenant;
            ICustomContext myDbContext = CustomContext.GetContext(requestParams.Tenant);
            var claimsRelatedEntityQueryService = new ClaimsRelatedEntityQueryService(myDbContext);
            var claimQueryService = new ClaimQueryService(myDbContext);
            var claimUpdateService = new ClaimUpdateService(myDbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            ClaimsRelatedEntityPM claimsRelatedEntityPM = null;

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "הודעות שוטפות לתיק תביעה " + customResponse.TPGIdentifier.FirstOrDefault().fileNumber;

            foreach (var claimItem in customResponse.TPGIdentifier)
            {
                //claimId = claimsRelatedEntityQueryService.GetClaimIdByTapagNumber(claimItem.fileNumber.ToString(), claimItem.numeral, requestParams.Tenant);
                claimsRelatedEntityPM = claimsRelatedEntityQueryService.GetRelatedEntityByTapagNumber(claimItem.fileNumber.ToString(), claimItem.numeral, requestParams.Tenant);
                if (claimsRelatedEntityPM != null)
                {
                    ClaimPM claimPM = claimQueryService.GetSingle(claimsRelatedEntityPM.ClaimId, false, true);
                    string messageTypeName = GetMessageTypeName(customResponse.ContinuousMessage.messageType);
                    string customOfficeName = GetCustomOfficeName(customResponse.ContinuousMessage.customOfficeNumber);
                    string unitName = GetUnitName(customResponse.ContinuousMessage.unitCode);

                    string description = "תיק תפ''ג: " + claimPM.TapagNumber + "\n"
                        + "תיק תביעה: " + claimItem.fileNumber.ToString() + "-" + claimItem.numeral + "\n"
                        + "תיק מוביל: " + customResponse.ContinuousMessage.leadingFileNumber + "\n"
                        + "סוג הודעה: " + messageTypeName + "\n"
                        + "תחנת מכס: " + customOfficeName + "\n"
                        + "יחידה מקצועית: " + unitName + "\n"
                        + "הודעת המכס: " + customResponse.ContinuousMessage.note;

                    DoUpdateNotification("5115N", claimPM, requestParams.Tenant, claimsRelatedEntityPM.ExternalClaimNumber, description, "A");
                }
            }

            this.MyRequestSheetParam = new RequestSheetParam();
            if (claimsRelatedEntityPM != null)
            {
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Claim");
                this.MyRequestSheetParam.EntityId1 = claimsRelatedEntityPM.ClaimId;
                this.MyRequestSheetParam.RequestDescription = "הודעות שוטפות לתיק תביעה " + claimsRelatedEntityPM.TapagNumber;
                if (claimsRelatedEntityPM.ClaimEntityTypeCode == "1055" && !string.IsNullOrWhiteSpace(claimsRelatedEntityPM.ExternalClaimNumber))
                {
                    this.MyRequestSheetParam.CustomFileNo = claimsRelatedEntityPM.ExternalClaimNumber;
                }
            }
        }

        private string GetUnitName(int unitCode)
        {
            string unitName = null;
            if (unitCode > 0)
            {
                OrganizationUnitTypeQueryService organizationUnitTypeQueryService = new OrganizationUnitTypeQueryService(this._MyTenant);
                OrganizationUnitTypePM organizationUnitTypePM = organizationUnitTypeQueryService.GetSingle(unitCode.ToString(), false, true);
                if (organizationUnitTypePM != null)
                {
                    unitName = organizationUnitTypePM.LocalName;
                }
            }
            return unitName;
        }

        private string GetMessageTypeName(int messageType)
        {
            string messageTypeName = null;
            if (messageType >= 0)
            {
                ContinuousMessagesTypeCodeQueryService continuousMessagesTypeCodeQueryService = new ContinuousMessagesTypeCodeQueryService(this._MyTenant);
                ContinuousMessagesTypeCodePM continuousMessagesTypeCodePM = continuousMessagesTypeCodeQueryService.GetSingle(messageType.ToString(), false, true);
                if (continuousMessagesTypeCodePM != null)
                {
                    messageTypeName = continuousMessagesTypeCodePM.LocalName;
                }
            }
            return messageTypeName;
        }

        private string GetCustomOfficeName(int customOfficeNumber)
        {
            string description = null;
            if (customOfficeNumber > 0)
            {
                CustomsHouseTypeQueryService customsHouseTypeQueryService = new CustomsHouseTypeQueryService(this._MyTenant);
                CustomsHouseTypePM customsHouseTypePM = customsHouseTypeQueryService.GetSingle(customOfficeNumber.ToString(), false, true);
                if (customsHouseTypePM != null)
                {
                    description = customsHouseTypePM.LocalName;
                }
            }

            return description;
        }

        public override INF_MSG_GenericResponseData GetResponse(CLAIM_MSG8_ContinuousMessage customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private void DoUpdateNotification(string notificationDefinitionCode, ClaimPM connectedClaimPM, int tenant, string reference1Number, string description, string assigneToNotificationTypeCode)
        {
            LogMessagingUtil.Instance.AppendLine("New Message To Agent Request Notification");

            ICustomContext dbContext = CustomContext.GetContext(tenant);
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = tenant;
            newNotificationPM.NotificationDefinitionCode = notificationDefinitionCode;
            newNotificationPM.AssigneToNotificationTypeCode = assigneToNotificationTypeCode;
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = description;
            newNotificationPM.Reference2Number = connectedClaimPM.TapagNumber;
            newNotificationPM.DueDate = DateTime.Now;
            newNotificationPM.EntityId = connectedClaimPM.Id;
            newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Claim");
            newNotificationPM.Reference1Number = reference1Number;

            string referentUserId = null;
            if (connectedClaimPM != null && !string.IsNullOrWhiteSpace(connectedClaimPM.CustomerId)) newNotificationPM.CustomerId = connectedClaimPM.CustomerId; // moran 20.6.16 - Task 20789

            newNotificationPM.AssigneToId =
               NotificationBase.
               CalcAssigneToId(newNotificationPM.Tenant, null, referentUserId, notificationDefinitionCode, "");

            notificationUpdateService.Update(newNotificationPM, true);
        }
    }
}
