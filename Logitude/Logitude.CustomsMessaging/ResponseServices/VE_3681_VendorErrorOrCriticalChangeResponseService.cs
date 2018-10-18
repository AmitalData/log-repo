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
using UnifreightIIG.Common.MessageLib.Vendor;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class VE_3681_VendorErrorOrCriticalChangeResponseService : ResponseServiceBase
        <INF_MSG_GenericResponseData, VE_MSG012_VendorErrorOrCriticalChangeMessage, GenericRequestParams>
    {
        public override void Update(VE_MSG012_VendorErrorOrCriticalChangeMessage customResponse, GenericRequestParams requestParams)
        {
            //Analyze message 3681 - Vendor Error Or Critical Change Message
            ICustomContext context = CustomContext.GetContext(requestParams.Tenant);
            CustomsVendorQueryService vendorQueryService = new CustomsVendorQueryService(context);
            CustomsVendorUpdateService vendorUpdateService = new CustomsVendorUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);

            string id = vendorQueryService.GetIdByVendorNumber(customResponse.VendorErrorOrCriticalChangeMessage.vendorID.ToString(), requestParams.Tenant);
            CustomsVendorPM vendorPM = vendorQueryService.GetSingle(id, true, false);
            if (vendorPM == null)
            {
                LogMessagingUtil.Instance.AppendLine("Can't find vendor: \nVendor Number:" + customResponse.VendorErrorOrCriticalChangeMessage.vendorID);
                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.Succeeded = false;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "הגיע ליקוי לספק " + customResponse.VendorErrorOrCriticalChangeMessage.vendorID + " שלא קיים בטבלת הספקים";

                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.RequestDescription = "ליקוי /שינוי ספק " + customResponse.VendorErrorOrCriticalChangeMessage.vendorID;

                string notificationDefinitionCode = "";
                string assigneToNotificationTypeCode = "";
                switch (customResponse.VendorErrorOrCriticalChangeMessage.transactionTypeID)
                {
                    case 1: 
                    case 2:
                    case 5:
                        notificationDefinitionCode = "3681I";
                        assigneToNotificationTypeCode = "I";
                        break;
                    case 3:
                    case 4:
                    case 6:
                    case 7:
                        notificationDefinitionCode = "3681C";
                        assigneToNotificationTypeCode = "A";
                        break;
                }
                DoUpdateNotification(notificationDefinitionCode, requestParams.Tenant, customResponse.VendorErrorOrCriticalChangeMessage.vendorID.ToString(), this.MyResponseData.UserMessage, assigneToNotificationTypeCode);
                return;
            }

            vendorPM.ChangeSetOp = ChangeSetOperation.Update;
            vendorPM.TransactionTypeID = customResponse.VendorErrorOrCriticalChangeMessage.transactionTypeID.ToString();
            if (customResponse.VendorErrorOrCriticalChangeMessage.deadlineToFixSpecified == true)
            {
                vendorPM.DeadlineToFix = (DateTime)customResponse.VendorErrorOrCriticalChangeMessage.deadlineToFix;
            }
            vendorPM.NotesForCustomsAgent = customResponse.VendorErrorOrCriticalChangeMessage.notesForCustomsAgent;
            vendorPM.SubstituteVendorID = customResponse.VendorErrorOrCriticalChangeMessage.substituteVendorID;

            Logitude.Customs.BL.Models.EventContextTagModel.ProccessEnum callProccessID = new EventContextTagModel.ProccessEnum();
            switch(customResponse.VendorErrorOrCriticalChangeMessage.transactionTypeID)
            {
                case 1: // Create new
                    vendorPM.StatusCode = "1";
                    callProccessID = EventContextTagModel.ProccessEnum.VE_3681_VendorErrorOrCriticalChangeResponseServiceUpdate;
                    break;
                case 2: // Correct
                    if(vendorPM.StatusCode == "2")
                    {
                        vendorPM.StatusCode = "1";
                    }
                    callProccessID = EventContextTagModel.ProccessEnum.VE_3681_VendorErrorOrCriticalChangeResponseServiceUpdate;
                    break;
                case 3: // Incorrect
                    callProccessID = EventContextTagModel.ProccessEnum.VE_3681_VendorErrorOrCriticalChangeResponseServiceDefect;
                    break;
                case 4: // Period of time passed
                    if(vendorPM.StatusCode == "1")
                    {
                        vendorPM.StatusCode = "2";
                    }
                    vendorPM.InActive = true;
                    callProccessID = EventContextTagModel.ProccessEnum.VE_3681_VendorErrorOrCriticalChangeResponseServiceDefect;
                    break;
                case 5: // Update
                    callProccessID = EventContextTagModel.ProccessEnum.VE_3681_VendorErrorOrCriticalChangeResponseServiceUpdate;
                    break;
                case 6: // Cancellation
                    if(vendorPM.StatusCode == "1" | vendorPM.StatusCode == "2") 
                    {
                        vendorPM.StatusCode = "3";
                    }
                    vendorPM.InActive = true;
                    callProccessID = EventContextTagModel.ProccessEnum.VE_3681_VendorErrorOrCriticalChangeResponseServiceDefect;
                    break;
                case 7: // Replace with another
                    if(vendorPM.StatusCode == "1" | vendorPM.StatusCode == "2") 
                    {
                        vendorPM.StatusCode = "7";
                    }
                    vendorPM.InActive = true;
                    callProccessID = EventContextTagModel.ProccessEnum.VE_3681_VendorErrorOrCriticalChangeResponseServiceDefect;
                    break;
            }

            var myInsertEventContextTagModel = new EventContextTagModel() 
            {
                CallProccessID = callProccessID,
                //EventCode = "CUS",
                //EventRemarks = "Vendor Update"
            };
            vendorPM.CurrentContextTag = myInsertEventContextTagModel;

            this.MyResponseData = new INF_MSG_GenericResponseData()
            {
                ApplicationID = id,
                Succeeded = true,
                HasException = false,
            };

            this.MyRequestSheetParam = new RequestSheetParam()
            {
                EntityId1 = id,
                ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.CustomsVendor"),
                RequestDescription = "ליקוי /שינוי ספק " + customResponse.VendorErrorOrCriticalChangeMessage.vendorID,
            };

            vendorUpdateService.Update(vendorPM, true);
        }

        public override INF_MSG_GenericResponseData GetResponse(VE_MSG012_VendorErrorOrCriticalChangeMessage customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private void DoUpdateNotification(string notificationDefinitionCode, int tenant, string responseToMessage, string description, string typeCode)
        {
            LogMessagingUtil.Instance.AppendLine("New Message To Agent Request Notification");

            ICustomContext dbContext = CustomContext.GetContext(tenant);
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = tenant;
            newNotificationPM.NotificationDefinitionCode = notificationDefinitionCode;
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = description;
            newNotificationPM.Reference2Number = responseToMessage;
            newNotificationPM.DueDate = DateTime.Now;
            newNotificationPM.AssigneToNotificationTypeCode = typeCode;

            newNotificationPM.AssigneToId =
               NotificationBase.
               CalcAssigneToId(newNotificationPM.Tenant, null, null, notificationDefinitionCode, "");

            notificationUpdateService.Update(newNotificationPM, true);
        }
    }
}
