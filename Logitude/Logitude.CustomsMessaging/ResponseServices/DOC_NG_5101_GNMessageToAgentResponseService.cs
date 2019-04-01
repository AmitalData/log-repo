using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Storage;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using UnifreightIIG.Common.MessageLib.Docs;
using Logitude.Server.Tools.Models;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.CustomsMessaging.MessagingServices;

namespace Logitude.CustomsMessaging.ResponseServices
{  // moran 6.10.14 - Task 8066 -->
    public class DOC_NG_5101_GNMessageToAgentResponseService :
      ResponseServiceBase<INF_MSG_GenericResponseData, DOC_NG_5101_GNMessageToAgent, GenericRequestParams>
    {

        DeclarationPM _MyDeclarationPM;

        public override INF_MSG_GenericResponseData GetResponse(DOC_NG_5101_GNMessageToAgent customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DOC_NG_5101_GNMessageToAgent customResponse, GenericRequestParams requestParams)
        {
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myQueryService = new DeclarationQueryService(context);

            if (customResponse.MessageToAgent == null)
            {
                LogMessagingUtil.Instance.AppendLine("No Message To Agent details in the Response (MessageToAgent = null) ");
                this.MyResponseData = new INF_MSG_GenericResponseData();
                MyResponseData.UserMessage = "No Message To Agent details in the Response (MessageToAgent is null)";
                MyResponseData.Succeeded = true;
                MyResponseData.HasException = true;

                return;
            }

            LogMessagingUtil.Instance.AppendLine("Analyze Message To Agent response" + requestParams.AppicationId);

            string notificationDefinitionCode = "";
            string notificationDeclaration = "";
            string notificationDescription = ""; 
            string notificationStatusCode = ""; 
            string assigneToNotificationTypeCode = "I"; 

            switch (customResponse.MessageToAgent.msgCode)
            {
                case 3:
                case 4:
                case 5:
                    notificationDefinitionCode = "5101N";
                    assigneToNotificationTypeCode = "I";
                    notificationDescription = "הודעה לסוכן"; 
                    if (customResponse.MessageToAgent.RelatedEntity.entityType == 1055)
                    {
                        notificationDeclaration = customResponse.MessageToAgent.RelatedEntity.entityIdKey1;
                        notificationDescription = notificationDescription + " בגין הצהרה מספר " + notificationDeclaration; //eitan h 4/3/15 task 11572
                    }
                    if (customResponse.MessageToAgent.RelatedEntity.entityType == 12234)
                    {
                        notificationDescription = notificationDescription + " בגין בטוחה מספר " + customResponse.MessageToAgent.RelatedEntity.entityIdKey1; 
                    }
                    notificationStatusCode = "VAN";
                    break;
                case 6:
                    notificationDefinitionCode = "";
                    notificationDescription = "הודעה על תצהיר יבואן חדש ";
                    break;
                case 11:
                    /*notificationDefinitionCode = "5101I";
                    assigneToNotificationTypeCode = "I"; */
                    if (customResponse.MessageToAgent.RelatedEntity.entityType == 1163) //Deposition
                    {
                        string importerVAT = null;
                        string customsVendorId = null;
                        string[] msgString = customResponse.MessageToAgent.msgString.Split(new[] { "היבואן: " }, StringSplitOptions.None);
                        if (msgString != null)
                        {
                            string[] message = msgString[1].Split(" ".ToCharArray());
                            importerVAT = message[0];
                        }
                        string[] messageSplit = customResponse.MessageToAgent.msgString.Split(new[] { "הספק: " }, StringSplitOptions.None);
                        if (msgString != null)
                        {
                            string[] message = messageSplit[1].Split(" ".ToCharArray());
                            string customsVendorCode = message[0];
                            customsVendorId = CheckIfCustomsVendorCodeExist(customsVendorCode, requestParams.Tenant);
                        }
                        if(!string.IsNullOrWhiteSpace(customsVendorId))
                        {
                            SendImporterDeclarationRequest(requestParams, importerVAT, customsVendorId);
                        }
                    }
                    else
                    {
                        notificationDefinitionCode = "";
                        notificationDescription = "הודעה על תצהיר תקופתי העומד לפוג ";
                    }
                    break;
                case 7:
                    notificationDefinitionCode = "5101D";
                    notificationDescription = "הצהרה נותבה לתור בקרת מסמכים";
                    notificationStatusCode = "VCI"; // moran 1.8.16 - Task 21654 - change CDC to VCI
                    assigneToNotificationTypeCode = "I"; 
                    break;
                case 8:
                    notificationDefinitionCode = "5101T";
                    notificationDescription = "הצהרה נותבה לתור בחינה";
                    notificationStatusCode = "VCC";
                    assigneToNotificationTypeCode = "I"; 
                    break;
                case 9:
                    notificationDefinitionCode = "5101C";
                    notificationDescription = "הצהרה נותבה לתור רשות";
                    notificationStatusCode = "VCR";
                    assigneToNotificationTypeCode = "I"; 
                    break;
                case 12:
                    notificationDefinitionCode = "5101G";
                    assigneToNotificationTypeCode = "I";
                    notificationDescription = "ביטול אחסנה להצהרה " + customResponse.MessageToAgent.RelatedEntity.entityIdKey1;
                    break;
                case 13: 
                    notificationDefinitionCode = "5101U";
                    assigneToNotificationTypeCode = "I";
                    notificationDescription = "עדכון אחסנה להצהרה " + customResponse.MessageToAgent.RelatedEntity.entityIdKey1;
                    break;
                case 14: 
                    notificationDefinitionCode = "5101S";
                    assigneToNotificationTypeCode = "I";
                    notificationDescription = "נוצרה בקשת אחסנה " + customResponse.MessageToAgent.RelatedEntity.entityIdKey1;
                    break;
                case 16: // moran 17.1.17 - Task 21101
                    notificationDefinitionCode = "5101B";
                    assigneToNotificationTypeCode = "I";
                    notificationDescription = "בדיקה בטחונית להצהרה " + customResponse.MessageToAgent.RelatedEntity.entityIdKey1;
                    notificationStatusCode = "VCB";
                    break;
                case 18:
                    notificationDefinitionCode = "5101P";
                    assigneToNotificationTypeCode = "I";
                    notificationDescription = "אישור פריקה/טעינה למצהר " + customResponse.MessageToAgent.RelatedEntity.entityIdKey1;
                    notificationStatusCode = "VCP";
                    break;
                case 22:
                    notificationDescription = "ממתין ליסמ " + customResponse.MessageToAgent.RelatedEntity.entityIdKey1;
                    notificationStatusCode = "VCS";
                    break;
                case 23:
                    notificationDescription = "ממתין ליסמ ולבקרת מסמכים " + customResponse.MessageToAgent.RelatedEntity.entityIdKey1;
                    notificationStatusCode = "VCD";
                    break;
                case 24:
                    notificationDescription = "ממתין לביטחון " + customResponse.MessageToAgent.RelatedEntity.entityIdKey1;
                    notificationStatusCode = "VCE";
                    break;
                case 25:
                    notificationDescription = "ממתין לביטחון ולבקרת מסמכים " + customResponse.MessageToAgent.RelatedEntity.entityIdKey1;
                    notificationStatusCode = "VCA";
                    break;
                case 26:
                    notificationDescription = "ממתין לביטחון וליסמ " + customResponse.MessageToAgent.RelatedEntity.entityIdKey1;
                    notificationStatusCode = "VCG";
                    break;
                case 27:
                    notificationDescription = "ממתין לבטחון, יסמ ולבקרת מסמכים " + customResponse.MessageToAgent.RelatedEntity.entityIdKey1;
                    notificationStatusCode = "VCT";
                    break;
                default:
                    notificationDefinitionCode = "5101N";
                    assigneToNotificationTypeCode = "I";
                    notificationDescription = "הודעה לסוכן בגין " + customResponse.MessageToAgent.RelatedEntity.entityIdKey1;
                    notificationStatusCode = "VAN";
                    break;
            }

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = notificationDescription;

            if (!string.IsNullOrWhiteSpace(customResponse.MessageToAgent.msgString))
            {
                notificationDescription = notificationDescription + "\n" + customResponse.MessageToAgent.msgString;
            }

            if (customResponse.MessageToAgent.RelatedEntity.entityType == 1055 || customResponse.MessageToAgent.RelatedEntity.entityType == 1015) 
            {
                notificationDeclaration = customResponse.MessageToAgent.RelatedEntity.entityIdKey1;
                LogMessagingUtil.Instance.AppendLine("NotificationDeclaration = " + notificationDeclaration);

                requestParams.AppicationId = myQueryService.GetIdByDeclarationNumber(notificationDeclaration, requestParams.Tenant);
                if (string.IsNullOrWhiteSpace(requestParams.AppicationId))
                {
                    LogMessagingUtil.Instance.AppendLine("Cannot GetIdByDeclarationNumber " + notificationDeclaration);
                    MyResponseData = new INF_MSG_GenericResponseData() { Succeeded = false, HasException = false, UserMessage = "Cannot GetIdByDeclarationNumber " + notificationDeclaration };

                    if (!string.IsNullOrWhiteSpace(notificationDefinitionCode))
                    {
                        LogMessagingUtil.Instance.AppendLine("Start Sending Notification... ");
                        DoUpdateNotification(notificationDefinitionCode, requestParams.Tenant, customResponse.MessageToAgent.responseToMessage.ToString(), notificationDescription, assigneToNotificationTypeCode);
                    }
                    return;
                }

                this._MyDeclarationPM = myQueryService.GetSingle(requestParams.AppicationId, true, false);
                if (this._MyDeclarationPM == null)
                {
                    LogMessagingUtil.Instance.AppendLine("Can not found declaration" + requestParams.AppicationId);
                    MyResponseData = new INF_MSG_GenericResponseData() { Succeeded = false, HasException = false, UserMessage = "Can not found declaration" + requestParams.AppicationId };
                    return;
                }

                if(customResponse.MessageToAgent.msgCode == 14 || customResponse.MessageToAgent.msgCode == 13)
                {
                    var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
                    this._MyDeclarationPM.StorageStatusCode = "3";
                    this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                    myDeclarationUpdateService.Update(this._MyDeclarationPM, true);
                }

                this.MyRequestSheetParam.EntityId1 = requestParams.AppicationId;
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.CustomFileNo = this._MyDeclarationPM.CustomFileNo;

                if (this._MyDeclarationPM.IsCourierDeclaration == true)
                {
                    DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
                    switch (customResponse.MessageToAgent.msgCode)
                    {
                        case 7:
                            this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                            this._MyDeclarationPM.CourierCustomStatusCode = "2";
                            this._MyDeclarationPM.CourierSuspentionReasonCode = customResponse.MessageToAgent.msgCode.ToString();
                            this._MyDeclarationPM.CourierSuspentionCode = "25";
                            declarationUpdateService.Update(this._MyDeclarationPM, true);
                            break;
                        case 8:
                        case 9:
                        case 16:
                            this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                            this._MyDeclarationPM.CourierCustomStatusCode = "2";
                            this._MyDeclarationPM.CourierSuspentionReasonCode = customResponse.MessageToAgent.msgCode.ToString();
                            declarationUpdateService.Update(this._MyDeclarationPM, true);
                            SendDeclarationStatusRequest(this._MyDeclarationPM);
                            break;
                        case 17:
                            notificationDefinitionCode = "5101M";
                            notificationDescription = "התקבל מסר שטר מטען מאסטר מחברת התעופה";
                            notificationStatusCode = "VCM";
                            assigneToNotificationTypeCode = "I";
                            break;
                        case 22:
                            this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                            this._MyDeclarationPM.CourierCustomStatusCode = "2";
                            this._MyDeclarationPM.CourierSuspentionReasonCode = customResponse.MessageToAgent.msgCode.ToString();
                            this._MyDeclarationPM.CourierSuspentionCode = "30";
                            declarationUpdateService.Update(this._MyDeclarationPM, true);
                            break;
                        case 23:
                            this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                            this._MyDeclarationPM.CourierCustomStatusCode = "2";
                            this._MyDeclarationPM.CourierSuspentionReasonCode = customResponse.MessageToAgent.msgCode.ToString();
                            this._MyDeclarationPM.CourierSuspentionCode = "31";
                            declarationUpdateService.Update(this._MyDeclarationPM, true);
                            break;
                        case 24:
                            this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                            this._MyDeclarationPM.CourierCustomStatusCode = "2";
                            this._MyDeclarationPM.CourierSuspentionReasonCode = customResponse.MessageToAgent.msgCode.ToString();
                            this._MyDeclarationPM.CourierSuspentionCode = "32";
                            declarationUpdateService.Update(this._MyDeclarationPM, true);
                            break;
                        case 25:
                            this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                            this._MyDeclarationPM.CourierCustomStatusCode = "2";
                            this._MyDeclarationPM.CourierSuspentionReasonCode = customResponse.MessageToAgent.msgCode.ToString();
                            this._MyDeclarationPM.CourierSuspentionCode = "33";
                            declarationUpdateService.Update(this._MyDeclarationPM, true);
                            break;
                        case 26:
                            this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                            this._MyDeclarationPM.CourierCustomStatusCode = "2";
                            this._MyDeclarationPM.CourierSuspentionReasonCode = customResponse.MessageToAgent.msgCode.ToString();
                            this._MyDeclarationPM.CourierSuspentionCode = "34";
                            declarationUpdateService.Update(this._MyDeclarationPM, true);
                            break;
                        case 27:
                            this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                            this._MyDeclarationPM.CourierCustomStatusCode = "2";
                            this._MyDeclarationPM.CourierSuspentionReasonCode = customResponse.MessageToAgent.msgCode.ToString();
                            this._MyDeclarationPM.CourierSuspentionCode = "35";
                            declarationUpdateService.Update(this._MyDeclarationPM, true);
                            break;
                    }
                }

            }
            else if (customResponse.MessageToAgent.RelatedEntity.entityType == 1053)  // Not For dev Yet!!!
            {
                //this.MyRequestSheetParam.EntityId1 = requestParams.AppicationId;
                //this.MyRequestSheetParam.ObjectTableId1 = ObjectTabelRepository.GetObjectTableByName("Customs.Declaration");
            }
            else if (customResponse.MessageToAgent.RelatedEntity.entityType == 12234)  // Collateral
            {
                LogMessagingUtil.Instance.AppendLine("Notification Collateral= " + customResponse.MessageToAgent.RelatedEntity.entityIdKey1);
                var customsCollateralQueryService = new CustomsCollateralQueryService(context);
                requestParams.AppicationId = customsCollateralQueryService.GetIdByCollateralRequestNumber(customResponse.MessageToAgent.RelatedEntity.entityIdKey1, requestParams.Tenant);
                if (String.IsNullOrWhiteSpace(requestParams.AppicationId))
                {
                    LogMessagingUtil.Instance.AppendLine("Can not found Collateral" + requestParams.AppicationId);
                    MyResponseData = new INF_MSG_GenericResponseData() { Succeeded = false, HasException = false, UserMessage = "Can not found Collateral" + requestParams.AppicationId };
                    return;
                }
                this.MyRequestSheetParam.EntityId1 = requestParams.AppicationId;
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.CustomsCollateral");
            }

            this.MyResponseData = new INF_MSG_GenericResponseData();
            MyResponseData.ApplicationID = requestParams.AppicationId;
            MyResponseData.Succeeded = true;
            MyResponseData.UserMessage = notificationDescription;
            
            if (this._MyDeclarationPM != null && !string.IsNullOrWhiteSpace(notificationStatusCode))
            {
                LogMessagingUtil.Instance.AppendLine("Sent status " + notificationStatusCode + " to UNF");
                RaiseEvent(this._MyDeclarationPM, notificationStatusCode, customResponse.MessageToAgent.msgString.Replace("00:00:00", ""));
            }

            if (!string.IsNullOrWhiteSpace(notificationDefinitionCode))
            {
                LogMessagingUtil.Instance.AppendLine("Start Sending Notification...");
                DoUpdateNotification(notificationDefinitionCode, requestParams.Tenant, customResponse.MessageToAgent.responseToMessage.ToString(), notificationDescription, assigneToNotificationTypeCode);
            }
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

            string customerId = null;
            string referentUserId = null;
            if (this._MyDeclarationPM != null)
            {
                newNotificationPM.EntityId = this._MyDeclarationPM.Id;
                newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                newNotificationPM.Reference1Number = this._MyDeclarationPM.CustomFileNo;
                newNotificationPM.DepartmentId = this._MyDeclarationPM.DepartmentId;
                newNotificationPM.DeclarationOfficeCode = this._MyDeclarationPM.DeclarationOfficeCode;
                customerId = this._MyDeclarationPM.CustomerId;
                referentUserId = this._MyDeclarationPM.ReferentUserId;
            }
            if(MyRequestSheetParam.ObjectTableId1 == ObjectTableRepository.GetObjectTableByName("Customs.CustomsCollateral"))
            {
                newNotificationPM.EntityId = MyRequestSheetParam.EntityId1;
                newNotificationPM.ObjectTableId = MyRequestSheetParam.ObjectTableId1;
            }
            if (this._MyDeclarationPM != null && !string.IsNullOrWhiteSpace(this._MyDeclarationPM.CustomerId)) newNotificationPM.CustomerId = this._MyDeclarationPM.CustomerId; // moran 20.6.16 - Task 20789

            newNotificationPM.AssigneToId =
               NotificationBase.CalcAssigneToId(newNotificationPM.Tenant, customerId, referentUserId, notificationDefinitionCode, "");

            if (this._MyDeclarationPM != null && !string.IsNullOrWhiteSpace(newNotificationPM.DeclarationOfficeCode))
            {
                newNotificationPM.IsHandledByCustomOffice = true;
            }

            notificationUpdateService.Update(newNotificationPM, true);
        }

        private void RaiseEvent(DeclarationPM dirtyDeclarationPM, string code, string remarks)
        {
            try
            {
                string loggingUserId = "";
                loggingUserId = AuthenticationUtil.ResolveUserId(dirtyDeclarationPM.Tenant);

                var eventContextTagModel = dirtyDeclarationPM.CurrentContextTag as EventContextTagModel;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {

                    Tenant = dirtyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = code,
                    notes = remarks,
                    CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                    EntityId = dirtyDeclarationPM.Id,
                    UserId = loggingUserId,
                    CommunicationSubject = "FU Status " + code + " from logitude (Agent Response)",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dirtyDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = code,
                        status_DateTime = DateTime.Now,
                        //status_save = "no_fail",
                        comments = remarks,
                    }
                };

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode = " + code + " CustomFileNo= " + dirtyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (System.Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }



        }

        void SendDeclarationStatusRequest(DeclarationPM myDeclarationPM)
        {
            var mySBQMessage = new SBQMessageService();
            var newSearchDeclarationStatusRequestParams = new DeclarationStatusRequestParams()
            {
                LoggingEnabled = true,
                CustomFileNo = myDeclarationPM.CustomFileNo,
                DeclarationNumber = myDeclarationPM.DeclarationNumber,
                Tenant = myDeclarationPM.Tenant,
                RequestName = "Declaration Status (from Message To Agent Response) " + myDeclarationPM.DeclarationNumber,
                ResponseName = "Declaration Status (from Message To Agent Response) " + myDeclarationPM.DeclarationNumber,
                RequestVIA = SendRequestVIA.WebServiceBatch,
                InterfaceTypeCode = "8250",
                LoggingEntityId = myDeclarationPM.Id,
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                LoggingUserId = AuthenticationUtil.ResolveUserId(myDeclarationPM.Tenant),
            };

            try
            {
                SBQMessageService.CreateSheetSBQMessage<Logitude.CustomsMessaging.Common.RequestParams.DeclarationStatusRequestParams>(newSearchDeclarationStatusRequestParams, false, DateTime.Now.AddMinutes(2));
            }
            catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
            {
                if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("8250 RequestInProgress stop create a new one !! ");
                }
                throw;
            }
        }

        void SendImporterDeclarationRequest(GenericRequestParams requestParams, string importerNumber, string vendorCode)
        {
            DateTime today = DateTime.Today;
            string loggingUserId = AuthenticationUtil.ResolveUserId(requestParams.Tenant);
            var newImporterDeclarationRequestParams = new ImporterDeclarationRequestParams()
            {
                LoggingEnabled = true,
                LoggingUserId = loggingUserId,
                Tenant = requestParams.Tenant,
                RequestName = "Importer Declaration Request",
                ResponseName = "Importer Declaration Request",
                IsByExpireDate = false,
                IsByType = true,
                ImporterNumber = importerNumber,
                Code = vendorCode,
                DeclarationConect = "2",
                FromDate = today.AddDays(-1),
                ToDate = today.AddDays(365),
                RequestVIA = SendRequestVIA.WebServiceBatch
            };

            var service = new VE_8326_ImporterDeclarationMessagingService();
            var responseData = service.Send(newImporterDeclarationRequestParams);
            if (!responseData.Succeeded)
            {
                LogMessagingUtil.Instance.AppendLine("Request Failed " + responseData.CustomsRequestsSheetId + ", Message: " + responseData.UserMessage);
                return;
            }
            LogMessagingUtil.Instance.AppendLine("Request Succeeded " + responseData.CustomsRequestsSheetId);
        }

        string CheckIfCustomsVendorCodeExist(string vendorNumber, int tenant)
        {

            if (string.IsNullOrWhiteSpace(vendorNumber))
            {
                return null;
            }

            ICustomContext customContext = CustomContext.GetContext(tenant);
            CustomsVendorQueryService query = new CustomsVendorQueryService(customContext);
            string vendorId = query.GetIdByVendorNumber(vendorNumber, tenant);
            if(!string.IsNullOrWhiteSpace(vendorId))
            {
                return vendorId;
            }

            return null;
        }
    }
}
