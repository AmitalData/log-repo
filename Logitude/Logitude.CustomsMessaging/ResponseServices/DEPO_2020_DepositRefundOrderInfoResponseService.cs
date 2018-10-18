
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
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
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Deposit;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DEPO_2020_DepositRefundOrderInfoResponseService : ResponseServiceBase
        <INF_MSG_GenericResponseData, DEPO_NG_2020_MSG12_DepositRefundOrderInfo, GenericRequestParams>
    {
        public override void Update(DEPO_NG_2020_MSG12_DepositRefundOrderInfo customResponse, GenericRequestParams requestParams)
        {
            //Analyze Message 2020 - Deposit Refund (DCA)
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            TapagConnectionTableQueryService tapagConnectionTableQueryService = new TapagConnectionTableQueryService(dbContext);
            var depositQueryService = new DepositQueryService(dbContext);
            var depositUpdateService = new DepositUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            DepositPM myDepositPM = new DepositPM();
            string depositArray = null;
            string fileNumber = "";
            string numeral = "";

            foreach (var depositFileItem in customResponse.DepositFileRefundlist)
            {
                fileNumber = depositFileItem.DepositFileIdentifier.fileNumber;
                numeral = depositFileItem.DepositFileIdentifier.numeral.ToString();
                LogMessagingUtil.Instance.AppendLine("Analyze Message Deposit Refund " + fileNumber);

                TapagConnectionTablePM tapagConnectionTablePM = tapagConnectionTableQueryService.GetTapagConnectionByFileAndNumeral(fileNumber, depositFileItem.DepositFileIdentifier.numeral, requestParams.Tenant);
                if (tapagConnectionTablePM == null || (tapagConnectionTablePM != null && String.IsNullOrWhiteSpace(tapagConnectionTablePM.TapagId)))
                {
                    this.MyResponseData = new INF_MSG_GenericResponseData()
                    {
                        ApplicationID = myDepositPM.Id,
                        Succeeded = true,
                        HasException= true,
                        UserMessage = "לא נמצא תיק תפג מקושר " + " FileNumber: " + fileNumber + " Numeral: " + depositFileItem.DepositFileIdentifier.numeral,
                    };

                    this.MyRequestSheetParam = new RequestSheetParam()
                    {
                        RequestDescription = "מענה לבקשת החזר פיקדון " + fileNumber + "-" + numeral,
                    };
                    DoUpdateNotification(customResponse, requestParams.Tenant);
                    return;
                }

                var depositId = depositQueryService.GetDepositIdByTapagNumber(tapagConnectionTablePM.TapagId, requestParams.Tenant);
                myDepositPM = depositQueryService.GetSingle(depositId, true, false);

                if (myDepositPM != null)
                {
                    myDepositPM.ChangeSetOp = ChangeSetOperation.Update;
                    myDepositPM.CustomsTapagFile = fileNumber;
                    myDepositPM.CustomsNumeral = depositFileItem.DepositFileIdentifier.numeral;
                    if (customResponse.DepositRefundOrderDetails.refundPaymentOrderNumber != null)
                    {
                        myDepositPM.PaymentOrderNumber = (int)customResponse.DepositRefundOrderDetails.refundPaymentOrderNumber;
                    }
                    myDepositPM.Remarks = depositFileItem.remarks;
                    myDepositPM.CustomsAmount = depositFileItem.amount;
                    myDepositPM.ConnectedDeclarationId = tapagConnectionTablePM.DeclarationId;
                    myDepositPM.DecisionCode = customResponse.DepositRefundOrderDetails.decisionCode.ToString();

                    //Create Event "DRE" & Notification - Deposit Refund
                    EventContextTagModel myInsertEventContextTagModel = new EventContextTagModel()
                    {
                        CallProccessID = EventContextTagModel.ProccessEnum.DEPO_2020_DepositRefundOrderInfoResponseServiceRefund,
                        EventCode = "DRE",
                        EventRemarks = "Deposit Refund",
                    };
                    myDepositPM.CurrentContextTag = myInsertEventContextTagModel;

                    depositUpdateService.Update(myDepositPM, true);
                    depositArray = string.Concat(depositArray, ", ", myDepositPM.TapagID);
                }
            }

            this.MyResponseData = new INF_MSG_GenericResponseData()
            {
                ApplicationID = myDepositPM.Id,
                Succeeded = true,
                UserMessage = "מענה לבקשת החזר פיקדון (רשימה) " + depositArray,
            };

            this.MyRequestSheetParam = new RequestSheetParam()
            {
                EntityId1 = myDepositPM.Id,
                ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Deposit"),
                EntityId2 = myDepositPM.ConnectedDeclarationId,
                ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                RequestDescription = "מענה לבקשת החזר פיקדון " + fileNumber + "-" + numeral,
            };
        }

        public override INF_MSG_GenericResponseData GetResponse(DEPO_NG_2020_MSG12_DepositRefundOrderInfo customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private void DoUpdateNotification(DEPO_NG_2020_MSG12_DepositRefundOrderInfo customResponse, int tenant)
        {
            LogMessagingUtil.Instance.AppendLine("New Message To Agent Request Notification (DepositRefundOrderInfoResponseService)");

            ICustomContext dbContext = CustomContext.GetContext(tenant);
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            string description = "החזר פיקדון ";
            if (customResponse.DepositFileRefundlist != null && customResponse.DepositFileRefundlist.Length > 0)
            {
                description = description + customResponse.DepositFileRefundlist.FirstOrDefault().DepositFileIdentifier.fileNumber + "/" +
                    customResponse.DepositFileRefundlist.FirstOrDefault().DepositFileIdentifier.numeral;

                if (customResponse.DepositFileRefundlist.FirstOrDefault().amount > 0)
                {
                    description = description + "\n" + "סכום שהוחזר- " + customResponse.DepositFileRefundlist.FirstOrDefault().amount;
                }
                if (!string.IsNullOrEmpty(customResponse.DepositFileRefundlist.FirstOrDefault().remarks))
                {
                    description = description + "\n" + "הערה- " + customResponse.DepositFileRefundlist.FirstOrDefault().remarks;
                }
            }
            if (customResponse.DepositRefundOrderDetails.decisionCode > 0)
            {
                description = description + "\n" + "קוד החלטה- " + customResponse.DepositRefundOrderDetails.decisionCode;
            }
            
            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = tenant;
            newNotificationPM.NotificationDefinitionCode = "2020N";
            newNotificationPM.AssigneToNotificationTypeCode = "I";
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = description;
            newNotificationPM.DueDate = DateTime.Now;
            if (customResponse.DepositRefundOrderDetails.refundPaymentOrderNumberSpecified == true && customResponse.DepositRefundOrderDetails.refundPaymentOrderNumber != null)
            {
                newNotificationPM.Reference2Number = customResponse.DepositRefundOrderDetails.refundPaymentOrderNumber.ToString();
            }

            string referentUserId = null;
            newNotificationPM.AssigneToId = NotificationBase.CalcAssigneToId(newNotificationPM.Tenant, null, referentUserId, "2020N", "");

            notificationUpdateService.Update(newNotificationPM, true);
        }
    }
}
