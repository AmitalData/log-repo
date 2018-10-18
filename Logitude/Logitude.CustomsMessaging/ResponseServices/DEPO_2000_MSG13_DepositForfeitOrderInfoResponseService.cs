
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
    public class DEPO_2000_MSG13_DepositForfeitOrderInfoResponseService : ResponseServiceBase
        <INF_MSG_GenericResponseData, DEPO_NG_2000_MSG13_DepositForfeitOrderInfo, GenericRequestParams>
    {
        public override void Update(DEPO_NG_2000_MSG13_DepositForfeitOrderInfo customResponse, GenericRequestParams requestParams)
        {
            //Analyze Message 2000 - Deposit Forfeit Order Info (DCA) 
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            TapagConnectionTableQueryService tapagConnectionTableQueryService = new TapagConnectionTableQueryService(dbContext);
            var depositQueryService = new DepositQueryService(dbContext);
            var depositUpdateService = new DepositUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            DepositPM myDepositPM = new DepositPM();
            string depositArray = null;

            foreach (var depositFileItem in customResponse.DepositFileForfeitlist)
            {
                string fileNumber = depositFileItem.DepositFileIdentifier.fileNumber;
                TapagConnectionTablePM tapagConnectionTablePM = tapagConnectionTableQueryService.GetTapagConnectionByFileAndNumeral(fileNumber, depositFileItem.DepositFileIdentifier.numeral, requestParams.Tenant);
                if (tapagConnectionTablePM == null || (tapagConnectionTablePM != null && String.IsNullOrWhiteSpace(tapagConnectionTablePM.TapagId)))
                {
                    this.MyResponseData = new INF_MSG_GenericResponseData()
                    {
                        HasException = true,
                        Succeeded = true,
                        UserMessage = "לא נמצא תיק תפג מקושר " + " FileNumber: " + fileNumber + " Numeral: " + depositFileItem.DepositFileIdentifier.numeral,
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
                    myDepositPM.PaymentOrderNumber = customResponse.DepositForfeitOrderDetails.forfeitPaymentOrderNumber;
                    myDepositPM.Remarks = depositFileItem.remarks;
                    myDepositPM.CustomsAmount = depositFileItem.Amount;
                    myDepositPM.ConnectedDeclarationId = tapagConnectionTablePM.DeclarationId;

                    //Create Event "DFO" & Notification - Deposit Forfeit
                    EventContextTagModel myInsertEventContextTagModel = new EventContextTagModel()
                    {
                        CallProccessID = EventContextTagModel.ProccessEnum.DEPO_2000_DepositForfeitOrderInfoResponseServiceForfeit,
                        EventCode = "DFO",
                        EventRemarks = "Deposit Forfiet",
                    };
                    myDepositPM.CurrentContextTag = myInsertEventContextTagModel;

                    depositUpdateService.Update(myDepositPM, true);
                    if (!string.IsNullOrWhiteSpace(depositArray))
                    {
                        depositArray = string.Concat(depositArray, ", ", fileNumber);
                    }

                    depositArray = string.Concat(depositArray, fileNumber);
                }
            }
            
            this.MyResponseData = new INF_MSG_GenericResponseData()
            {
                ApplicationID = myDepositPM.Id,
                Succeeded = true,
                UserMessage = "יידוע בדבר ביצוע חילוט פיקדון " + depositArray,
            };

            this.MyRequestSheetParam = new RequestSheetParam()
            {
                EntityId1 = myDepositPM.Id,
                ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Deposit"),
                EntityId2 = myDepositPM.ConnectedDeclarationId,
                ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                RequestDescription = "יידוע בדבר ביצוע חילוט פיקדון " + depositArray,
            };
        }

        public override INF_MSG_GenericResponseData GetResponse(DEPO_NG_2000_MSG13_DepositForfeitOrderInfo customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private void DoUpdateNotification(DEPO_NG_2000_MSG13_DepositForfeitOrderInfo customResponse, int tenant)
        {
            LogMessagingUtil.Instance.AppendLine("New Message To Agent Request Notification (DepositForfeitOrderInfoResponseService)");

            ICustomContext dbContext = CustomContext.GetContext(tenant);
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            DEPO_NG_2000_MSG13_DepositForfeitOrderInfoDepositFileForfeitlist depositFileItem = customResponse.DepositFileForfeitlist.FirstOrDefault();

            string description = "חילוט פיקדון ";
            if (depositFileItem != null)
            {
                if (depositFileItem.DepositFileIdentifier != null &&
                    !string.IsNullOrEmpty(depositFileItem.DepositFileIdentifier.fileNumber) && depositFileItem.DepositFileIdentifier.numeral > 0)
                {
                    description = description + depositFileItem.DepositFileIdentifier.fileNumber + "/" + depositFileItem.DepositFileIdentifier.numeral;
                }

                if (depositFileItem.Amount > 0)
                {
                    description = description + "\n" + "סכום- " + depositFileItem.Amount.ToString();
                }
                if (!string.IsNullOrEmpty(depositFileItem.remarks))
                {
                    description = description + "\n" + "הערה- " + depositFileItem.remarks;
                }
            }

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = tenant;
            newNotificationPM.NotificationDefinitionCode = "2000N";
            newNotificationPM.AssigneToNotificationTypeCode = "I";
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = description;
            newNotificationPM.DueDate = DateTime.Now;
            if (customResponse.DepositForfeitOrderDetails != null && customResponse.DepositForfeitOrderDetails.forfeitPaymentOrderNumber > 0)
            {
                newNotificationPM.Reference2Number = customResponse.DepositForfeitOrderDetails.forfeitPaymentOrderNumber.ToString();
            }

            string referentUserId = null;
            newNotificationPM.AssigneToId = NotificationBase.CalcAssigneToId(newNotificationPM.Tenant, null, referentUserId, "2000N", "");

            notificationUpdateService.Update(newNotificationPM, true);
        }
    }
}
