using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.CargoTracking;

namespace Logitude.CustomsMessaging.ResponseServices
{// moran 25.1.15 - Task 9967
    public class LP_NG_8400_MSG01_LogisticPermitMessageResponseService :
      ResponseServiceBase<INF_MSG_GenericResponseData, LP_NG_8400_MSG01_LogisticPermitMessage, GenericRequestParams>
    {

        DeclarationPM _MyDeclarationPM;

        public override INF_MSG_GenericResponseData GetResponse(LP_NG_8400_MSG01_LogisticPermitMessage customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
            //return null;
        }

        public override void Update(LP_NG_8400_MSG01_LogisticPermitMessage customResponse, GenericRequestParams requestParams)
        {

            var context = CustomContext.GetContext(requestParams.Tenant);
            var myQueryService = new DeclarationQueryService(context);
            LogMessagingUtil.Instance.AppendLine("Analyze Logistic Permit Message (Declaration Id:" + customResponse.GeneralDetails.declarationID + ")");
            if (string.IsNullOrWhiteSpace(customResponse.GeneralDetails.declarationID))
            {
                if (customResponse.CargoIdentifier != null)
                {
                    this._MyDeclarationPM = myQueryService.GetDeclarationPMByCargoIdentifiers(customResponse.CargoIdentifier.cargoIdentifierType.ToString(), customResponse.CargoIdentifier.cargoIdentifierKey1, customResponse.CargoIdentifier.cargoIdentifierKey2, requestParams.Tenant);
                }
            }
            else
            {
                this._MyDeclarationPM = myQueryService.GetSingle(customResponse.GeneralDetails.declarationID, true, false);
                if (this._MyDeclarationPM == null) // moran 21.10.15 - Task 17106
                {
                    string decId = myQueryService.GetIdByDeclarationNumber(customResponse.GeneralDetails.declarationID, requestParams.Tenant);
                    if(decId != null)this._MyDeclarationPM = myQueryService.GetSingle(decId, true, false);
                }
            }

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyRequestSheetParam = new RequestSheetParam();
            string remarks = "";
            string eventCode = "";
            string notificationRemarks = "";

            //Raise Storage event 
            if (customResponse.GeneralDetails.actionCode == 1)
            {
                eventCode = "LPA";
                remarks = "אושר היתר לוגיסטי";
                notificationRemarks = "התקבל היתר לוגיסטי לתיק ";
            }
            else if (customResponse.GeneralDetails.actionCode == 2)
            {
                eventCode = "LPC";
                remarks = "בוטל היתר לוגיסטי";
                notificationRemarks = "בוטל היתר לוגיסטי לתיק ";
            }

            if (this._MyDeclarationPM != null)
            {
                requestParams.AppicationId = this._MyDeclarationPM.Id;
                LogMessagingUtil.Instance.AppendLine("Declaration No. " + this._MyDeclarationPM.DeclarationNumber);
                var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
                MyResponseData.ApplicationID = _MyDeclarationPM.Id;
                this.MyRequestSheetParam.EntityId1 = _MyDeclarationPM.Id;
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                
                
                if (eventCode != "")
                {
                    RaiseEvent(_MyDeclarationPM, requestParams.LoggingUserId, eventCode, remarks);
                }
                this.MyRequestSheetParam.RequestDescription = remarks + " מספר " + customResponse.CargoIdentifier.LogisticPermitDetails[0].logisticPermitId;
                notificationRemarks = notificationRemarks + _MyDeclarationPM.CustomFileNo;
            }
            else
            {
                string logMess = "Couldn't find Declaration for Logistic Permit Message by ID(" + requestParams.AppicationId + ")";
                if (customResponse.CargoIdentifier != null)
                {
                    logMess = logMess + " and Cargo Identifiers(Type:" + customResponse.CargoIdentifier.cargoIdentifierType.ToString() + ", Manifest No.:" + customResponse.CargoIdentifier.cargoIdentifierKey1 + ", Second Cargo ID:" + customResponse.CargoIdentifier.cargoIdentifierKey2;
                }
                LogMessagingUtil.Instance.AppendLine(logMess);
            }

            string notificationDeclaration = customResponse.GeneralDetails.declarationID;
            string notificationCode = "";
            if (customResponse.GeneralDetails.actionCode == 1)
            {
                notificationCode = "8400A";
            }
            else if (customResponse.GeneralDetails.actionCode == 2)
            {
                notificationCode = "8400C";
            }

            if (notificationCode != "")
            {
                string logisticPermitId = "";
                
                if (customResponse.CargoIdentifier.LogisticPermitDetails.Count() > 0)
                {
                    string containersNumber = "";
                    logisticPermitId = customResponse.CargoIdentifier.LogisticPermitDetails[0].logisticPermitId;
                    foreach (var logisticItem in customResponse.CargoIdentifier.LogisticPermitDetails)
                    {
                        containersNumber = logisticItem.containerNumber + "  " + containersNumber;
                    }
                    notificationRemarks = notificationRemarks + "\n" + "עבור מכולה " + containersNumber;
                }
                UpdateNotification(this._MyDeclarationPM, notificationCode, notificationDeclaration, requestParams.Tenant, notificationRemarks, logisticPermitId);
            }
            
            MyResponseData.Succeeded = true;
             
        }

        private void RaiseEvent(DeclarationPM _MyDeclarationPM, string loggingUserId, string eventCode, string remarks)
        {

            string eventType = "";
            if (eventCode == "LPA")
            {
                eventType = "Approved";
            }
            else if (eventCode == "LPC")
            {
                eventType = "Canceled";
            }

            var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
            {
                Tenant = _MyDeclarationPM.Tenant,
                objectTableName = "Customs.Declaration",
                EventCode = eventCode,
                notes = remarks,
                CommunicationLoggingEntityReference = _MyDeclarationPM.DeclarationNumber,
                EntityId = _MyDeclarationPM.Id,
                UserId = loggingUserId,

                CommunicationSubject = "FU Status " + eventCode + " from logitude (Declaration Logistic Permit " + eventType + ")",
                MyFUStatus = new AmitalEventTracerModel.FUStatus()
                {
                    entname = "CFIFILEM",
                    primary_number = _MyDeclarationPM.CustomFileNo,
                    status = "new",
                    xml_status = "new",
                    status_id = eventCode,
                    status_DateTime = DateTime.Now,
                    //status_place = "FRA",
                    //status_save = "no_fail",
                    comments = remarks,
                }
            };

            LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode =" + eventCode + "  CustomFileNo: " + _MyDeclarationPM.CustomFileNo + "   ");
            AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

        }

        private void UpdateNotification(DeclarationPM DeclarationPM, string notificationDefinitionCode, string notificationDeclaration, int tenant, string msgString, string logisticPermitId)
        {
            string loggingUserId = "";
            var contactRep = new ContactRepository(tenant);
            var contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.ResolveUserIdentityName(tenant), tenant);

            loggingUserId = contact.Id;

            DoUpdateNotification(DeclarationPM, loggingUserId, notificationDefinitionCode, notificationDeclaration, tenant, msgString, logisticPermitId);

        }

        private void DoUpdateNotification(DeclarationPM declarationPM, string loggingUserId, string notificationDefinitionCode, string notificationDeclaration, int tenant, string msgString, string logisticPermitId)
        {
            string type = "I";
            LogMessagingUtil.Instance.AppendLine("New Logistic Permit Notification");

            ICustomContext dbContext = CustomContext.GetContext(tenant);
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = tenant;

            newNotificationPM.NotificationDefinitionCode = notificationDefinitionCode;

            //newNotificationPM.EntityId = notificationDeclaration; //moran 21.10.15 - Task 17106 - commented
            //newNotificationPM.ObjectTableId = ObjectTabelRepository.GetObjectTableByName("Customs.Declaration"); //moran 21.10.15 - Task 17106 - commented

            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = msgString;
            newNotificationPM.Reference1Number = logisticPermitId;

            string customerId = null;
            string referentUserId = null;
            if (declarationPM != null)
            {
                newNotificationPM.EntityId = declarationPM.Id; // moran 21.10.15 - Task 17106
                newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"); // moran 21.10.15 - Task 17106
                newNotificationPM.DepartmentId = declarationPM.DepartmentId;
                newNotificationPM.DeclarationOfficeCode = declarationPM.DeclarationOfficeCode;
                newNotificationPM.Reference1Number = declarationPM.CustomFileNo;
                customerId = declarationPM.CustomerId;
                referentUserId = declarationPM.ReferentUserId;
            }
            if (declarationPM != null && !string.IsNullOrWhiteSpace(declarationPM.CustomerId)) newNotificationPM.CustomerId = declarationPM.CustomerId; // moran 20.6.16 - Task 20789

            newNotificationPM.DueDate = DateTime.Now;
            newNotificationPM.AssigneToNotificationTypeCode = type;

            newNotificationPM.AssigneToId =
               NotificationBase.
               CalcAssigneToId(newNotificationPM.Tenant, customerId, referentUserId, notificationDefinitionCode, "");

            if (declarationPM != null && !string.IsNullOrWhiteSpace(declarationPM.DeclarationOfficeCode))
            {
                newNotificationPM.IsHandledByCustomOffice = true;
            }
            if (notificationDefinitionCode == "8400C")
            {
                NotificationBase.CloseAllRelatedNotification(dbContext, newNotificationPM, "8400A");
            }
            notificationUpdateService.Update(newNotificationPM, true);

        }
        
    }
}
