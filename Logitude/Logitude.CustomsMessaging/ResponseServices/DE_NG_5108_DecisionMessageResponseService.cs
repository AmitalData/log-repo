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
using UnifreightIIG.Common.MessageLib.Deficit.NG5108;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DE_NG_5108_DecisionMessageResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DE_NG_5108_MSG12_DecisionMessage, GenericRequestParams>
    {
        public override INF_MSG_GenericResponseData GetResponse(DE_NG_5108_MSG12_DecisionMessage customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DE_NG_5108_MSG12_DecisionMessage customResponse, GenericRequestParams requestParams)
        {
            //Analyze Deficit Decision Message - Interface 5108
            ICustomContext myDbContext = CustomContext.GetContext(requestParams.Tenant);
            var deficitQueryService = new DeficitQueryService(myDbContext);
            var myDeficitUpdateService = new DeficitUpdateService(myDbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var claimQueryService = new ClaimQueryService(myDbContext);
            var tapagConnectionTableQueryService = new TapagConnectionTableQueryService(myDbContext);
            string userMessage = "החלטת מכס בגין גרעון";

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = userMessage;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = userMessage;

            foreach (var deficitFileItem in customResponse.DeficitFile)
            {
                TapagConnectionTablePM tapagConnectionTablePM = tapagConnectionTableQueryService.GetTapagConnectionByFileAndNumeral(deficitFileItem.TapagIdentifier.fileNumber, deficitFileItem.TapagIdentifier.numeral, requestParams.Tenant);
                if (tapagConnectionTablePM == null)
                {
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = "לא נמצא תיק תפג " + deficitFileItem.TapagIdentifier.fileNumber;
                    return;
                }
                DeficitPM deficitPM = deficitQueryService.GetDeficitByPaymentOrderNumberOrTapagId(null, tapagConnectionTablePM.TapagId, requestParams.Tenant);
                if (deficitPM != null)
                {
                    LogMessagingUtil.Instance.AppendLine("Analyze Deficit Decision for :" + deficitFileItem.TapagIdentifier.fileNumber);
                    userMessage = userMessage + " " + deficitFileItem.TapagIdentifier.fileNumber;
                    this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Deficit");
                    this.MyRequestSheetParam.EntityId1 = deficitPM.Id;
                    this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                    this.MyRequestSheetParam.EntityId2 = tapagConnectionTablePM.DeclarationId;
                    this.MyRequestSheetParam.RequestDescription = userMessage;
                    this.MyResponseData.UserMessage = userMessage;

                    DeficitDecisionPM deficitDecisionPM = new DeficitDecisionPM();
                    if (deficitPM.DeficitDecisions != null && deficitPM.DeficitDecisions.Count() > 0)
                    {
                        deficitDecisionPM = deficitPM.DeficitDecisions.FirstOrDefault(si => si.DeclarationId == tapagConnectionTablePM.DeclarationId);
                        if (deficitDecisionPM != null)
                        {
                            deficitDecisionPM.ChangeSetOp = ChangeSetOperation.Update;
                        }
                    }
                    else
                    {
                        deficitDecisionPM.ChangeSetOp = ChangeSetOperation.Insert;
                        deficitDecisionPM.DeclarationId = tapagConnectionTablePM.DeclarationId;
                        deficitDecisionPM.DeficitId = deficitPM.Id;
                        deficitDecisionPM.Tenant = deficitPM.Tenant;
                    }

                    deficitDecisionPM.RequestID = customResponse.DecisionMessage.requestID.ToString();
                    deficitDecisionPM.RequestTypeCode = customResponse.DecisionMessage.requestType.ToString();
                    deficitDecisionPM.RequestDate = customResponse.DecisionMessage.requestDate;
                    deficitDecisionPM.ApprovedProfessionCode = customResponse.DecisionMessage.approvedProfession.ToString();
                    deficitDecisionPM.DecisionCode = customResponse.DecisionMessage.decisionCode.ToString();
                    deficitDecisionPM.DecisionNoteForLetter = customResponse.DecisionMessage.decisionNoteForLetter;
                    if (deficitFileItem.DebtBalance != null)
                    {
                        deficitDecisionPM.TotalComponentAmount = deficitFileItem.DebtBalance.totalComponentAmount;
                        deficitDecisionPM.TotalEstimatedAmount = deficitFileItem.DebtBalance.totalEstimatedAmount;
                        deficitDecisionPM.TotalFinancialPenaltyAmount = deficitFileItem.DebtBalance.totalFinancialPenaltyAmount;
                        deficitDecisionPM.TotalInterestAmount = deficitFileItem.DebtBalance.totalInterestAmount;
                        deficitDecisionPM.TotalLinkingAmount = deficitFileItem.DebtBalance.totalLinkingAmount;
                    }

                    if(deficitDecisionPM.ChangeSetOp == ChangeSetOperation.Insert)
                    {
                        deficitPM.DeficitDecisions.Add(deficitDecisionPM);
                    }

                    deficitPM.ChangeSetOp = ChangeSetOperation.Update;
                    myDeficitUpdateService.Update(deficitPM, true);

                    var declarationQueryService = new DeclarationQueryService(requestParams.Tenant);
                    DeclarationPM connectedDeclarationPM = declarationQueryService.GetSingle(deficitDecisionPM.DeclarationId, false, false);

                    this.MyRequestSheetParam.CustomFileNo = connectedDeclarationPM.CustomFileNo;
                    string decisionTypeName = GetDecisionTypeName(customResponse.DecisionMessage.decisionCode.ToString(), requestParams.Tenant);
                    string description = "החלטת מכס בגין גרעון " + connectedDeclarationPM.CustomFileNo + " - " + decisionTypeName;
                    DoUpdateNotification("5108N", deficitPM, requestParams.Tenant, connectedDeclarationPM, description, "A");
                }
            }
        }

        private string GetDecisionTypeName(string decisionTypeCode, int tenant)
        {
            string decisionTypeName = null;
            if (!string.IsNullOrEmpty(decisionTypeCode))
            {
                DecisionTypeQueryService decisionTypeQueryService = new DecisionTypeQueryService(tenant);
                DecisionTypePM decisionTypePM = decisionTypeQueryService.GetSingle(decisionTypeCode, false, true);
                if (decisionTypePM != null)
                {
                    decisionTypeName = decisionTypePM.LocalName;
                }
            }
            return decisionTypeName;
        }

        private void DoUpdateNotification(string notificationDefinitionCode, DeficitPM deficitPM, int tenant, DeclarationPM connectedDeclarationPM, string description, string assigneToNotificationTypeCode)
        {
            LogMessagingUtil.Instance.AppendLine("New Message To Agent Request Notification- 5108N");

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
            newNotificationPM.DueDate = DateTime.Now;
            newNotificationPM.EntityId = deficitPM.Id;
            newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Deficit");
            newNotificationPM.Reference2Number = deficitPM.TapagNumber;

            string customerId = null;
            string referentUserId = null;
            if (connectedDeclarationPM != null)
            {
                newNotificationPM.EntityId = connectedDeclarationPM.Id;
                newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                newNotificationPM.Reference1Number = connectedDeclarationPM.CustomFileNo;
                newNotificationPM.DepartmentId = connectedDeclarationPM.DepartmentId;
                newNotificationPM.DeclarationOfficeCode = connectedDeclarationPM.DeclarationOfficeCode;
                customerId = connectedDeclarationPM.CustomerId;
                referentUserId = connectedDeclarationPM.ReferentUserId;
            }
            if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.CustomerId)) newNotificationPM.CustomerId = connectedDeclarationPM.CustomerId; 
            newNotificationPM.AssigneToId = NotificationBase.CalcAssigneToId(newNotificationPM.Tenant, null, referentUserId, notificationDefinitionCode, "");

            notificationUpdateService.Update(newNotificationPM, true);
        }
    }
}
