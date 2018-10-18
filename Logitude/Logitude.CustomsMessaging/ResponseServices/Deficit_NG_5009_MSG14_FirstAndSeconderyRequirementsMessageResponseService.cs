
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnifreightIIG.Common.MessageLib.Deficit;

namespace Logitude.CustomsMessaging.MessageAnalyzer
{
    public class Deficit_NG_5009_MSG14_FirstAndSeconderyRequirementsMessageResponseService :
        ResponseServiceBase<
        INF_MSG_GenericResponseData,
        DE_NG_5009_MSG14_FirstAndSeconderyRequirementsMessage, 
        GenericRequestParams>
    {
        public override void Update(DE_NG_5009_MSG14_FirstAndSeconderyRequirementsMessage customResponse,GenericRequestParams requestParams)
        {
            try
            {
                ICustomContext customContext = CustomContext.GetContext(requestParams.Tenant);
                var paymentOrderQueryService = new PaymentOrderQueryService(customContext);
                PaymentOrderUpdateService paymentOrderUpdateService = new PaymentOrderUpdateService(customContext as IContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);
                var myDeclarationQueryService = new DeclarationQueryService(requestParams.Tenant);

                string deficitFileList = "";
                string requirementType = "";
                string deficitFileListNotification = "";

                switch (customResponse.FirstAndSeconderyRequirements.requirementType.ToString())
                {
                    case "9":
                    case "09":
                        requirementType = "חוב בהתראה";
                        break;
                    case "10":
                        requirementType = "חוב בהרשאה";
                        break;
                    default:
                        break;
                }

                //Get all deficit files' numbers
                if (customResponse.DeficitFile != null)
                {
                    foreach (var deficitFile in customResponse.DeficitFile)
                    {
                        if (!string.IsNullOrWhiteSpace(deficitFile.ToString()))
                        {
                            if (!string.IsNullOrWhiteSpace(deficitFile.TapagIdentifier.ToString()))
                            {
                                if (!string.IsNullOrWhiteSpace(deficitFile.TapagIdentifier.fileNumber.ToString()))
                                {
                                    deficitFileList = deficitFileList + ", " + deficitFile.TapagIdentifier.fileNumber;
                                    deficitFileListNotification = deficitFileListNotification + ", " + deficitFile.TapagIdentifier.fileNumber + " " + deficitFile.TapagIdentifier.numeral;
                                }
                                else
                                {
                                    deficitFileList = deficitFile.TapagIdentifier.fileNumber;
                                    deficitFileListNotification = deficitFile.TapagIdentifier.fileNumber + " " + +deficitFile.TapagIdentifier.numeral;
                                }
                            }
                        }
                    }
                }

                string description = "התראת מכס לגבי תיק גרעון " + customResponse.FirstAndSeconderyRequirements.leadingFileNumber + "\n" +
                        "הוראת תשלום " + customResponse.FirstAndSeconderyRequirements.paymentOrderID + "\n" +
                        "מספרי תיקי תפג " + deficitFileListNotification;
                
                //Get Payment Order
                var PaymentOrderId = paymentOrderQueryService.GetIdByPaymentNumber(customResponse.FirstAndSeconderyRequirements.paymentOrderID.ToString(), requestParams.Tenant);
                var myDeclarationId = myDeclarationQueryService.GetIdByDeclarationNumber(customResponse.DeficitFile.FirstOrDefault().TapagIdentifier.fileNumber, requestParams.Tenant);
                if (string.IsNullOrWhiteSpace(PaymentOrderId))
                {
                    string paymentNumber = customResponse.FirstAndSeconderyRequirements.paymentOrderID.ToString();
                    LogMessagingUtil.Instance.AppendLine("No Payment Order found for customResponse.FirstAndSeconderyRequirements.paymentOrderID '" + customResponse.FirstAndSeconderyRequirements.paymentOrderID.ToString() + "'");
                    this.MyResponseData = new INF_MSG_GenericResponseData();
                    this.MyResponseData.Succeeded = false;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = "הוראת תשלום " + paymentNumber + " לא נמצאה";

                    this.MyRequestSheetParam = new RequestSheetParam();
                    this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");
                    DeclarationPM myDeclarationPM = null;
                    if (!string.IsNullOrWhiteSpace(myDeclarationId))
                    {
                        this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                        this.MyRequestSheetParam.EntityId2 = myDeclarationId;
                        myDeclarationPM = myDeclarationQueryService.GetSingle(myDeclarationId,false,false);
                    }
                    this.MyRequestSheetParam.RequestDescription = "הודעה על " + requirementType + " - " + paymentNumber;
                    DoUpdateNotification("5009N", myDeclarationPM, requestParams.Tenant, paymentNumber, description, "A");
                    return;
                }
                PaymentOrderPM paymentOrderPM = new PaymentOrderPM();
                paymentOrderPM = paymentOrderQueryService.GetSingle(PaymentOrderId, true, false);
                if (paymentOrderPM == null)
                {
                    LogMessagingUtil.Instance.AppendLine("No Payment Order found for id '" + PaymentOrderId + "'");
                    this.MyResponseData = new INF_MSG_GenericResponseData();
                    this.MyResponseData.Succeeded = false;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = "הוראת תשלום " + customResponse.FirstAndSeconderyRequirements.paymentOrderID + " לא נמצאה";

                    this.MyRequestSheetParam = new RequestSheetParam();
                    this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");
                    if (!string.IsNullOrWhiteSpace(myDeclarationId))
                    {
                        this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                        this.MyRequestSheetParam.EntityId2 = myDeclarationId;
                    }
                    this.MyRequestSheetParam.RequestDescription = "הודעה על " + requirementType + " - " + customResponse.FirstAndSeconderyRequirements.paymentOrderID;
                    return;
                }

                string remarks = "הודעה בגין התראה ראשונה ללקוח" + "\n" +
                                    "סוג הדרישה: " + requirementType + "\n" +
                                    ", תוקף הוראת תשלום: " + customResponse.FirstAndSeconderyRequirements.validityDateTo + "\n" +
                                    ", תיק גרעון מוביל: " + customResponse.FirstAndSeconderyRequirements.leadingFileNumber + "\n" +
                                    ", רשימת תיקי גרעון: " + deficitFileList + "\n" +
                                    ", הערות: " + customResponse.FirstAndSeconderyRequirements.note;

                var myUpdateEventContextTagModel = new EventContextTagModel()
                {
                    CallProccessID = EventContextTagModel.ProccessEnum.Deficit_NG_5009_MSG14_FirstAndSeconderyRequirementsMessageResponseService,
                    EventCode = "DFN",
                    EventRemarks = remarks,
                    FUStatusRemarks = remarks,
                    MyNotificationPM = new NotificationPM() { Description = description },
                };
                paymentOrderPM.CurrentContextTag = myUpdateEventContextTagModel;
                paymentOrderPM.ChangeSetOp = ChangeSetOperation.Update;
                LogMessagingUtil.Instance.AppendLine("FirstAndSeconderyRequirements.leadingFileNumber:" + customResponse.FirstAndSeconderyRequirements.leadingFileNumber.ToString() + " Update");

                paymentOrderPM.Tenant = requestParams.Tenant;
                paymentOrderUpdateService.Update(paymentOrderPM, true);

                this.MyResponseData = new INF_MSG_GenericResponseData() 
                { 
                    ApplicationID = paymentOrderPM.Id, 
                    Succeeded = true, 
                    HasException = false,
                    UserMessage = "הודעה על " + requirementType + " - " + paymentOrderPM.PaymentNumber,
                };

                this.MyRequestSheetParam = new RequestSheetParam()
                {
                    EntityId1 = paymentOrderPM.Id,
                    ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder"),
                    RequestDescription = "הודעה על " + requirementType + " - " + paymentOrderPM.PaymentNumber,
                };
                if (!string.IsNullOrWhiteSpace(myDeclarationId))
                    {
                        this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                        this.MyRequestSheetParam.EntityId2 = myDeclarationId;
                    }
            }
            catch (System.Exception ee)
            {
                ///MyResponseData = new INF_MSG_GenericResponseData() { HasException = true, ExceptionMessage = ee.ToString() };
                throw;
            }

        }

        public override INF_MSG_GenericResponseData GetResponse(DE_NG_5009_MSG14_FirstAndSeconderyRequirementsMessage customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private void DoUpdateNotification(string notificationDefinitionCode, DeclarationPM connectedDeclarationPM, int tenant, string reference2Number, string description, string typeCode)
        {
            LogMessagingUtil.Instance.AppendLine("New Message To Agent Request Notification");

            ICustomContext dbContext = CustomContext.GetContext(tenant);
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = tenant;
            newNotificationPM.NotificationDefinitionCode = notificationDefinitionCode;
            newNotificationPM.AssigneToNotificationTypeCode = typeCode;
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = description;
            newNotificationPM.Reference2Number = reference2Number;
            newNotificationPM.DueDate = DateTime.Now;

            string referentUserId = null;
            if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.Id))
            {
                newNotificationPM.EntityId = connectedDeclarationPM.Id;
                newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                newNotificationPM.DepartmentId = connectedDeclarationPM.DepartmentId;
                newNotificationPM.DeclarationOfficeCode = connectedDeclarationPM.DeclarationOfficeCode;
                newNotificationPM.Reference1Number = connectedDeclarationPM.CustomFileNo;
                referentUserId = connectedDeclarationPM.ReferentUserId;
                if (!string.IsNullOrWhiteSpace(connectedDeclarationPM.DeclarationOfficeCode))
                {
                    newNotificationPM.IsHandledByCustomOffice = false;
                }
            }
            if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.CustomerId)) newNotificationPM.CustomerId = connectedDeclarationPM.CustomerId; // moran 20.6.16 - Task 20789

            newNotificationPM.AssigneToId =
               NotificationBase.
               CalcAssigneToId(newNotificationPM.Tenant, null, referentUserId, notificationDefinitionCode, "");

            notificationUpdateService.Update(newNotificationPM, true);
        }
    }
}
#if mergepilot20141214
#endif