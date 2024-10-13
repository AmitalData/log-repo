using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class PaymentOrderUpdateService : EntityUpdateService<PaymentOrder, PaymentOrderPM, EntityPM>
    {
        protected override void OnCreating(PaymentOrderPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.PaymentOrder", entityPM.Tenant);

        }
        private void ValidatePM(PaymentOrderPM entityPM)
        {
            var dic = new Dictionary<object, object>();
            var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            CustomsClassLevelValidator.ValidateClass(entityPM, new System.ComponentModel.DataAnnotations.ValidationContext(entityPM, dic));  
            //var myCustomsClassLevelValidator = new CustomsClassLevelValidator("PaymentOrderPM", entityPM.Tenant);
            //myCustomsClassLevelValidator.IsValid(entityPM,entityPM,
            
            //Validator.TryValidateObject(entityPM, new ValidationContext(entityPM, null, null), errors);
        }

        protected override void OnUpdating(PaymentOrderPM entityPM)
        {
            ValidatePM(entityPM);
            //CustomsSettingQueryService settingsQuery = new CustomsSettingQueryService(entityPM.Tenant);
          
                UpdateUnifreight(entityPM);
         

            UpdateNotification(entityPM); // moran 2.9.14 - Task 6932 


            DeclarationQueryService declarationQuery = new DeclarationQueryService(entityPM.Tenant);

            PaymentOrderConnectionTablePM connection = entityPM.PaymentOrderConnectionTables.Where(d =>  d.ConnectedEntityCode == "D").FirstOrDefault();

           if (connection != null)
           {
                if (entityPM.PaymentOrderConnectionTables.Where(d => d.ConnectedEntityCode == "D").Count() > 1)
               {
                       DeclarationPM declaration = declarationQuery.GetSingle(connection.ConnectedEntityId, false, false);
                       entityPM.CustomFiles = declaration.CustomFileNo + "*";

               }
             
               else
               {
                   DeclarationPM declaration = declarationQuery.GetSingle(connection.ConnectedEntityId, false, false);
                   entityPM.CustomFiles = declaration.CustomFileNo;
               }
           }

            if (entityPM.CustomerChanged)
            {
                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);

                PaymentOrderRepository rep = new PaymentOrderRepository(entityPM.Tenant);
                PaymentOrder paymentOrder = rep.GetSingle(entityPM.Id, entityPM.Tenant);
                if (paymentOrder.CustomerId != entityPM.CustomerId)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs() { EntityId = entityPM.Id, ObjectTableName = "Customs.PaymentOrder", Tenant = entityPM.Tenant, UserId = contact.Id, EventTypeCode = "CNG", Notes = "Old Customer: " + paymentOrder.CustomerId + " New Customer: " + entityPM.CustomerId });
                }
            }

        }

        protected override void UpdateComposition(PaymentOrderPM entityPM)
        {
            PaymentOrderLineUpdateService paymentOrderLineUpdateService = new PaymentOrderLineUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
            paymentOrderLineUpdateService.UpdateMulti(entityPM.PaymentOrderLines, entityPM.DeletedPaymentOrderLines, entityPM, false);

            PaymentOrderMethodUpdateService paymentOrderMethodUpdateService = new PaymentOrderMethodUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
            paymentOrderMethodUpdateService.UpdateMulti(entityPM.PaymentOrderMethods, entityPM.DeletedPaymentOrderMethods, entityPM, false);

            PaymentOrderProtestReasonUpdateService paymentOrderProtestReasonUpdateService = new PaymentOrderProtestReasonUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
            paymentOrderProtestReasonUpdateService.UpdateMulti(entityPM.PaymentOrderProtestReasons, entityPM.DeletedPaymentOrderProtestReasons, entityPM, false);

            PaymentOrderConnectionTableUpdateService paymentOrderConnectionTableUpdateService = new PaymentOrderConnectionTableUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
            paymentOrderConnectionTableUpdateService.UpdateMulti(entityPM.PaymentOrderConnectionTables, entityPM.DeletedPaymentOrderConnectionTables, entityPM, false);

        }

        // moran 2.9.14 - Task 6932 -->
        private void UpdateNotification(PaymentOrderPM dirtyEntityPM)
        {
            /*var contactRep = new ContactRepository(dirtyEntityPM.Tenant);
            var contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.ResolveUserIdentityName(dirtyEntityPM.Tenant), dirtyEntityPM.Tenant);
            string loggingUserId = "";
            loggingUserId = contact.Id;*/
            string loggingUserId = AuthenticationUtil.ResolveUserId(dirtyEntityPM.Tenant);

            DeclarationPM connectedDeclarationPM = GetDBEntity(dirtyEntityPM);
            string notificationDefinitionCode = "";
            var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                    case EventContextTagModel.ProccessEnum.TSH_MSG7_AgentPaymentReplyResponseService:
                        notificationDefinitionCode = "3052P";
                        break;
                    case EventContextTagModel.ProccessEnum.TSH_MSG2_PaymentOrderReplyResponseServiceCancel:
                        notificationDefinitionCode = "3050C";
                        break;
                    case EventContextTagModel.ProccessEnum.TSH_MSG2_PaymentOrderReplyResponseServiceUpdate:
                        notificationDefinitionCode = "3050U";
                        break;
                    case EventContextTagModel.ProccessEnum.TSH_MSG2_PaymentOrderReplyResponseServiceCreate:
                        if (connectedDeclarationPM == null || connectedDeclarationPM.IsCourierDeclaration != true)
                        {
                            notificationDefinitionCode = "3050N";
                        }
                        break;
                        // moran 30.10.14 - Task 8327
                    case EventContextTagModel.ProccessEnum.Deficit_NG_5009_MSG14_FirstAndSeconderyRequirementsMessageResponseService:
                        notificationDefinitionCode = "5009N";
                        break;
                }

            }

            if (!string.IsNullOrWhiteSpace(notificationDefinitionCode))
            {
                DoUpdateNotification(dirtyEntityPM, connectedDeclarationPM, loggingUserId, notificationDefinitionCode);
            }
        }


        private void DoUpdateNotification(PaymentOrderPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId, string notificationDefinitionCode)
        {
            ICustomContext dbContext = CustomContext.GetContext(dirtyEntityPM.Tenant);
            this.currentContext = dbContext;
            string desc = "";
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), dirtyEntityPM.Tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = dirtyEntityPM.Tenant;

            switch (notificationDefinitionCode)
            {
                case "3050N":
                    LogMessagingUtil.Instance.AppendLine("New Payment Order Notification");
                    newNotificationPM.NotificationDefinitionCode = "3050N";
                    newNotificationPM.DueDate = dirtyEntityPM.LastPayDate;
                    newNotificationPM.AssigneToNotificationTypeCode = "A";
                    desc = "נוצרה";
                    break;
                case "3050C":
                    newNotificationPM.NotificationDefinitionCode = "3050C";
                    newNotificationPM.DueDate = DateTime.Now;
                    newNotificationPM.AssigneToNotificationTypeCode = "I";
                    desc = "בוטלה";
                    break;
                case "3050U":
                    newNotificationPM.NotificationDefinitionCode = "3050U";
                    newNotificationPM.DueDate = dirtyEntityPM.LastPayDate;
                    newNotificationPM.AssigneToNotificationTypeCode = "A";
                    desc = "עודכנה";
                    break;
                case "3052P":
                    newNotificationPM.NotificationDefinitionCode = "3052P";
                    newNotificationPM.DueDate = dirtyEntityPM.LastPayDate;
                    newNotificationPM.AssigneToNotificationTypeCode = "I";
                    desc = "שולמה";
                    break;
                // moran 30.10.14 - Task 8327
                case "5009N":
                    newNotificationPM.NotificationDefinitionCode = "5009N";
                    newNotificationPM.DueDate = DateTime.Now;
                    newNotificationPM.AssigneToNotificationTypeCode = "A";
                    var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                    if (eventContextTagModel.MyNotificationPM != null && !string.IsNullOrWhiteSpace(eventContextTagModel.MyNotificationPM.Description))
                    {
                        desc = eventContextTagModel.MyNotificationPM.Description;
                    }
                    else
                    {
                        int posA = eventContextTagModel.EventRemarks.LastIndexOf("תיק גרעון מוביל:");
                        var leadingFile = eventContextTagModel.EventRemarks.Substring(posA);
                        int posB = leadingFile.IndexOf("\n");
                        if (posB > 0)
                        {
                            leadingFile = leadingFile.Substring(0, posB);
                        }
                        desc = "התראת מכס לגבי תיק גרעון " + leadingFile + "\n" + "הוראת תשלום " + dirtyEntityPM.PaymentNumber;
                    }
                    break;
            }

            newNotificationPM.EntityId = dirtyEntityPM.Id;
            newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");

            var oldNotificationPM = new NotificationPM();
            if (notificationDefinitionCode.StartsWith("305")) // moran 30.10.14 - Task 8327 - enter into 'if'
            {
                oldNotificationPM = GetNotification(notificationQueryService, newNotificationPM, "3050N");
            }
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = "הוראת תשלום " + dirtyEntityPM.PaymentNumber + " " + desc;
            newNotificationPM.CreatedByRequestID = dirtyEntityPM.CustomsRequestsSheetId;
            if (string.IsNullOrWhiteSpace(newNotificationPM.CreatedByRequestID))
            {
                newNotificationPM.CreatedByRequestID = Logitude.Customs.Def.Messaging.Customs.RequestSheetContext.Current.GetContextOrDefault().CustomsRequestsSheetId;
            }
            newNotificationPM.Reference2Number = dirtyEntityPM.PaymentNumber;
            string oldassigneId = null;
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

            if (oldNotificationPM != null)
            {
                oldassigneId = oldNotificationPM.AssigneToId;
            }

            newNotificationPM.AssigneToId =
                NotificationBase.
                CalcAssigneToId(dirtyEntityPM.Tenant, dirtyEntityPM.CustomerId, referentUserId, notificationDefinitionCode, oldassigneId);

            NotificationBase.CloseAllRelatedNotification(dbContext, newNotificationPM, "3050");
            notificationUpdateService.Update(newNotificationPM, true);

        }

        private NotificationPM GetNotification(NotificationQueryService notificationQueryService, NotificationPM newNotificationPM, string notificationDefinitionCode)
        {
            var oldNotificationPM = notificationQueryService.GetNotification(newNotificationPM.Tenant, newNotificationPM.ObjectTableId, newNotificationPM.EntityId, notificationDefinitionCode);
            return oldNotificationPM;
        }

        protected override void CheckConcurrency(PaymentOrderPM entityPM, PaymentOrder entityPOCO)
        {
            //if (!entityPM.ConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID) && !entityPM.NewConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID))
            if (entityPM.ConcurrencyGUID != entityPOCO.ConcurrencyGUID && entityPM.NewConcurrencyGUID != entityPOCO.ConcurrencyGUID)
            {
                string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant,true);
                throw new OptimisticConcurrencyException(msg);
            }

        }

        protected override void AfterUpdating(PaymentOrderPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.CustomerId != null)
            {
                CardRepository cardRepository = new CardRepository(entityPM.Tenant);
                Card card = cardRepository.GetSingleCard(entityPM.CustomerId, entityPM.Tenant);
                if (card != null)
                {
                    entityPM.CustomerName = card.EnglishName;
                }
            }
        }
    }
}
