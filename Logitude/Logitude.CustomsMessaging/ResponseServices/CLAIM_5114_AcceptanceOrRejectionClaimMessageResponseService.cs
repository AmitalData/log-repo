using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnifreightIIG.Common.MessageLib.Claim.ClaimREJACC;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CLAIM_5114_AcceptanceOrRejectionClaimMessageResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, CLAIM_MSG10_AcceptanceOrRejectionClaimMessage, GenericRequestParams>
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
            var myClaimsRelatedEntityUpdateService = new ClaimsRelatedEntityUpdateService(myDbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var claimQueryService = new ClaimQueryService(myDbContext);
            string userMessage = "אישור/דחיה תביעה";

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = userMessage;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Claim");
            this.MyRequestSheetParam.RequestDescription = userMessage;

            foreach (var claimsRelatedEntityItem in customResponse.ClaimFileList)
            {
                ClaimsRelatedEntityPM claimsRelatedEntityPM = claimsRelatedEntityQueryService.GetRelatedEntityByTapagNumber(claimsRelatedEntityItem.TPGIdentifier.fileNumber.ToString(), claimsRelatedEntityItem.TPGIdentifier.numeral, requestParams.Tenant);
                if (claimsRelatedEntityPM != null)
                {
                    LogMessagingUtil.Instance.AppendLine("Analyze Acceptance Or Rejection Claim Message for Claim Id:" + claimsRelatedEntityPM.ClaimId);
                    ClaimPM claimPM = claimQueryService.GetSingle(claimsRelatedEntityPM.ClaimId, false, true);

                    userMessage = userMessage + " " + claimsRelatedEntityItem.TPGIdentifier.fileNumber;
                    this.MyRequestSheetParam.EntityId1 = claimsRelatedEntityPM.ClaimId;
                    this.MyRequestSheetParam.RequestDescription = userMessage;
                    this.MyResponseData.UserMessage = userMessage;

                    string decisionTypeName = GetDecisionTypeName(customResponse.AcceptanceOrRejectionClaimMessage.decisionCode, requestParams.Tenant);
                    string description = "תיק תביעה: " + claimsRelatedEntityItem.TPGIdentifier.fileNumber.ToString() + "\n"
                        + "מספר רץ: " + claimsRelatedEntityItem.TPGIdentifier.numeral + "\n"
                        + "תיק מוביל: " + customResponse.AcceptanceOrRejectionClaimMessage.leadingFileNumber + "\n"
                        + "החלטה: " + decisionTypeName + "\n"
                        + "סכום שהופקד" + customResponse.AcceptanceOrRejectionClaimMessage.depositingAmount;

                    DoUpdateNotification("5114N", claimPM, requestParams.Tenant, claimsRelatedEntityPM.ExternalClaimNumber, description, "A");

                    claimsRelatedEntityPM.ChangeSetOp = ChangeSetOperation.Update;
                    claimsRelatedEntityPM.DecisionCode = customResponse.AcceptanceOrRejectionClaimMessage.decisionCode.ToString();
                    claimsRelatedEntityPM.DecisionNote = customResponse.AcceptanceOrRejectionClaimMessage.decisionNote;
                    claimsRelatedEntityPM.EilatVatRefoundDecision = customResponse.AcceptanceOrRejectionClaimMessage.eilatVatRefoundDecision;
                    claimsRelatedEntityPM.DepositingAmount = customResponse.AcceptanceOrRejectionClaimMessage.depositingAmount;
                    claimsRelatedEntityPM.RefundAmount = claimsRelatedEntityItem.refundAmount;

                    if (claimsRelatedEntityItem.RefundQuantity != null)
                    {
                        ClaimsRelatedEntitiesRefundPM claimsRelatedEntitiesRefundPM = new ClaimsRelatedEntitiesRefundPM();
                        claimsRelatedEntitiesRefundPM.ChangeSetOp = ChangeSetOperation.Insert;
                        claimsRelatedEntitiesRefundPM.InvoiceNumber = claimsRelatedEntityItem.RefundQuantity.InvoicesequenceNumber;
                        claimsRelatedEntitiesRefundPM.SequenceNumeric = claimsRelatedEntityItem.RefundQuantity.InvoicesequenceNumber;
                        claimsRelatedEntitiesRefundPM.RefundQuntity = claimsRelatedEntityItem.RefundQuantity.refundQuantity;
                        claimsRelatedEntityPM.ClaimsRelatedEntitiesRefunds.Add(claimsRelatedEntitiesRefundPM);
                    }

                    if (customResponse.Seizure != null)
                    {
                        foreach (var seizureItem in customResponse.Seizure)
                        {
                            ClaimsRelatedEntitiesSeizurePM claimsRelatedEntitiesSeizurePM = new ClaimsRelatedEntitiesSeizurePM();
                            claimsRelatedEntitiesSeizurePM.ChangeSetOp = ChangeSetOperation.Insert;
                            claimsRelatedEntitiesSeizurePM.SeizureFactorCode = seizureItem.seizureFactorCode.ToString();
                            claimsRelatedEntitiesSeizurePM.SeizureMethodCode = seizureItem.seizureMethodCode.ToString();
                            claimsRelatedEntitiesSeizurePM.SeizureAmount = seizureItem.seizureAmount;
                            claimsRelatedEntityPM.ClaimsRelatedEntitiesSeizures.Add(claimsRelatedEntitiesSeizurePM);
                        }
                    }

                    myClaimsRelatedEntityUpdateService.Update(claimsRelatedEntityPM,true);
                }
                else
                {
                    LogMessagingUtil.Instance.AppendLine("Can not find Claim by- fileNumber:" + claimsRelatedEntityItem.TPGIdentifier.fileNumber.ToString() + " numeral:" + claimsRelatedEntityItem.TPGIdentifier.numeral);
                }
            }

        }

        private string GetDecisionTypeName(int decisionTypeCode, int tenant)
        {
            string decisionTypeName = null;
            if (decisionTypeCode >= 0)
            {
                DecisionTypeQueryService decisionTypeQueryService = new DecisionTypeQueryService(tenant);
                DecisionTypePM decisionTypePM = decisionTypeQueryService.GetSingle(decisionTypeCode.ToString(), false, true);
                if (decisionTypePM != null)
                {
                    decisionTypeName = decisionTypePM.LocalName;
                }
            }
            return decisionTypeName;
        }

        private void DoUpdateNotification(string notificationDefinitionCode, ClaimPM connectedClaimPM, int tenant, string reference1Number, string description, string assigneToNotificationTypeCode)
        {
            LogMessagingUtil.Instance.AppendLine("New Message To Agent Request Notification- 5114N");

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

            newNotificationPM.AssigneToId = NotificationBase.CalcAssigneToId(newNotificationPM.Tenant, null, referentUserId, notificationDefinitionCode, "");

            notificationUpdateService.Update(newNotificationPM, true);
        }
    }
}
