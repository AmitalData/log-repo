using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomsDocumentPointerUpdateService
    {
        public const string AddNotifcation = "Logitude.Customs.BL.EntityUpdateServices.CustomsDocumentPointerUpdateService.AddNotifcation";
        protected override void OnCreating(CustomsDocumentPointerPM entityPM, CustomsDocumentsTicketPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.CustomsDocumentPointer", entityPM.Tenant);
            if (entityPM.CustomsDocumentsTicketId == "" || entityPM.CustomsDocumentsTicketId == null)
            {
                entityPM.CustomsDocumentsTicketId = entityParentPM.Id;
            }
        }

        protected override void OnUpdating(CustomsDocumentPointerPM entityPM)
        {

            if (entityPM.ParentEntityId != null && entityPM.ParentEntityCode == "Declaration")
            {
                ICustomContext context = MainContext as CustomContext;
                //--------------- task 33591 mohammad replace update service with rep to avoid concurrency stuff-----------------------//

                //DeclarationQueryService declarationQueryService = new DeclarationQueryService(context);
                //DeclarationPM declaration = declarationQueryService.GetSingle(entityPM.ParentEntityId, false, false);
                //declaration.MarkAsChanged = true;
                //declaration.IsChanged = true;
                //DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(context);
                //declarationUpdateService.Update(declaration, true);

                DeclarationRepository declarationRep = new DeclarationRepository(context);
                Declaration declaration = declarationRep.GetSingle(entityPM.ParentEntityId, entityPM.Tenant);
                //declaration.IsChanged = true;
                //declarationRep.Update(declaration);
                //declarationRep.SubmitChanges();
                declarationRep.SetIsChangedAndSubmitChanges(declaration);

                //-----------------------------------------------------------------------------------------------------------------//
                var setting = CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant);
                if (setting.IsConnectedToUniFreight)
                {
                    UpdateUnifreight(entityPM, declaration);
                }
            }

            //CustomsDocumentsTicketId adjustment 16/10/2016 mohammad
            //if (entityPM.CustomDocumentId == null && EntityPOCO.CustomDocumentId != null) //disconnected.
            //{
            //    ICustomContext context = MainContext as CustomContext;
            //    CustomsDocumentQueryService customsDocumentQueryService = new CustomsDocumentQueryService(context);
            //    CustomsDocumentMetaDataValueQueryService customsDocumentMetaDataValueQueryService = new CustomsDocumentMetaDataValueQueryService(context);
            //    CustomsDocumentPM customDocument = customsDocumentQueryService.GetSingle(EntityPOCO.CustomDocumentId, false, false);
            //    CustomsDocumentUpdateService customsdocumentUpdateService = new CustomsDocumentUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
            //    List<CustomsDocumentMetaDataValuePM> metaDataValues = customsDocumentMetaDataValueQueryService.GetCustomDocumentMetaDataValues(EntityPOCO.CustomDocumentId, entityPM.Tenant);
            //    if (metaDataValues.Count == 0)
            //    {
            //        customDocument.DocumentTypeCode = null;
            //        customDocument.ChangeSetOp = ChangeSetOperation.Update;
            //        customsdocumentUpdateService.Update(customDocument, true);
            //    }
            //}

            UpdateNotification(entityPM);

        }

        protected override void OnUpdating(CustomsDocumentPointerPM entityPM, CustomsDocumentPointer entityPOCO)
        {
            try
            {
                base.OnUpdating(entityPM, entityPOCO);
            }
            finally
            {
                var myLogChangesService = new LogChangesService();
                myLogChangesService.
                    LogIt<CustomsDocumentPointerPM, CustomsDocumentPointer>("20180826HD315750.LogUntilDateyyyyMMdd", entityPM, entityPOCO);
            }
        }

        

        private void UpdateNotification(CustomsDocumentPointerPM dirtyEntityPM)
        {
            string loggingUserId = AuthenticationUtil.ResolveUserId(Tenant);//in case delete it came zero always so i used the Tenant in the entity Update service Mohammad.

            string notificationDefinitionCode = "";
            NotificationPM notificationPM = null;
            var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                    case EventContextTagModel.ProccessEnum.VAL_NG_8227_MSG_520_RequiredDocumentMessageInsert:
                        notificationDefinitionCode = "8227N";
                        break;
                    case EventContextTagModel.ProccessEnum.VAL_NG_8227_MSG_520_RequiredDocumentMessageDelete:
                        notificationDefinitionCode = "8227D";
                        break;
                    case EventContextTagModel.ProccessEnum.CLAIM_2300_MissingDocumentRequestResponseService:
                        notificationDefinitionCode = "2300N";
                        notificationPM = eventContextTagModel.MyNotificationPM;
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(notificationDefinitionCode))
            {
                DeclarationPM connectedDeclarationPM = GetConnectedDeclarationPM(dirtyEntityPM);
                DoUpdateNotification(dirtyEntityPM, connectedDeclarationPM, loggingUserId, notificationDefinitionCode, notificationPM);
            }
        }

        private DeclarationPM GetConnectedDeclarationPM(CustomsDocumentPointerPM dirtyEntityPM)
        {
            var declarationQueryService = new Logitude.Customs.BL.EntityQueryServices.DeclarationQueryService(dirtyEntityPM.Tenant);
            var myDBEntity = declarationQueryService.GetSingle(dirtyEntityPM.ParentEntityId, false, false);
            return myDBEntity ?? new DeclarationPM();
        }

        // moran 16.9.14 - Task 7995 -->
        private void DoUpdateNotification(CustomsDocumentPointerPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId, string notificationDefinitionCode, NotificationPM dirtyEntityNotificationPM)
        {
            ICustomContext dbContext = CustomContext.GetContext(dirtyEntityPM.Tenant);
            this.currentContext = dbContext;
            string desc = "";

            var notificationUpdateService = new NotificationUpdateService(this.currentContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), dirtyEntityPM.Tenant);
            var notificationQueryService = new NotificationQueryService(this.currentContext);

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = dirtyEntityPM.Tenant;

            LogMessagingUtil.Instance.AppendLine("New Customs Document Notification");
            newNotificationPM.NotificationDefinitionCode = notificationDefinitionCode;
            switch (notificationDefinitionCode)
            {
                case "8227N":
                    desc = "תיק " + connectedDeclarationPM.CustomFileNo + " - מסמך נדרש עʺי המכס";
                    break;
                case "8227D":
                    desc = "תיק " + connectedDeclarationPM.CustomFileNo + " - בוטלה דרישת מסמך";
                    break;
                case "2300N":
                    if (dirtyEntityNotificationPM == null)
                    {
                        desc = "תיק תביעה מסמך נדרש לתיק תביעה";
                    }
                    else
                    {
                        desc = dirtyEntityNotificationPM.Description;
                    }
                    break;
            }
            if (!string.IsNullOrWhiteSpace(dirtyEntityPM.DocumentTypeCode))
            {
                CustomDocumentTypeQueryService customDocumentTypeQueryService = new CustomDocumentTypeQueryService(dirtyEntityPM.Tenant);
                CustomDocumentTypePM customDocumentType = customDocumentTypeQueryService.GetSingleCustomDocumentTypeWithTenant(dirtyEntityPM.DocumentTypeCode, dirtyEntityPM.Tenant);
                desc = desc + "\n" + "סוג מסמך-" + customDocumentType.LocalName;
            }
            if (!string.IsNullOrWhiteSpace(dirtyEntityPM.CustomsDocId))
            {
                desc = desc + "\n" + "סימוכין מכס-" + dirtyEntityPM.CustomsDocId;
            }

            // Eitan H 6/6/18 Bug 39871: 8227 Notification display call# 310246 (make same remarks for all uses)-->
            // Now FUStatusRemarks == DocumentRemarks for 8227, so no need to get FUStatusRemarks
            if (notificationDefinitionCode.Substring(0,4)!="8227")
            {
                // moran 27.12.15 - Task 19251 -->
                var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                if (!string.IsNullOrWhiteSpace(eventContextTagModel.FUStatusRemarks))
                {
                    desc = desc + "\n" + eventContextTagModel.FUStatusRemarks;
                }
                // moran 27.12.15 - Task 19251 <--
            }
            if (!string.IsNullOrWhiteSpace(dirtyEntityPM.DocumentRemarks))
            {
                desc = desc + "\n" + "הערות מכס-" + dirtyEntityPM.DocumentRemarks;
            }

            newNotificationPM.EntityId = dirtyEntityPM.Id;
            newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CustomsDocumentPointer");
            newNotificationPM.CreatedByRequestID = dirtyEntityPM.CustomsRequestsSheetId;
            if (string.IsNullOrWhiteSpace(newNotificationPM.CreatedByRequestID))
            {
                newNotificationPM.CreatedByRequestID = Logitude.Customs.Def.Messaging.Customs.RequestSheetContext.Current.GetContextOrDefault().CustomsRequestsSheetId;
            }
            newNotificationPM.Reference2Number = dirtyEntityPM.CustomsDocId;
            var oldNotificationPM = new NotificationPM();
            oldNotificationPM = GetNotification(notificationQueryService, newNotificationPM, notificationDefinitionCode);

            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = desc;

            string oldassigneId = null;
            string customerId = null;
            string referentUserId = null;
            if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.Id))
            {
                newNotificationPM.EntityId = connectedDeclarationPM.Id;
                newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                newNotificationPM.DepartmentId = connectedDeclarationPM.DepartmentId;
                newNotificationPM.DeclarationOfficeCode = connectedDeclarationPM.DeclarationOfficeCode;
                newNotificationPM.Reference1Number = connectedDeclarationPM.CustomFileNo;
                customerId = connectedDeclarationPM.CustomerId;
                referentUserId = connectedDeclarationPM.ReferentUserId;
            }
            else if (dirtyEntityNotificationPM != null)
            {
                newNotificationPM.EntityId = dirtyEntityNotificationPM.EntityId;
                newNotificationPM.ObjectTableId = dirtyEntityNotificationPM.ObjectTableId;
                newNotificationPM.Reference1Number = dirtyEntityNotificationPM.Reference1Number;
                newNotificationPM.CustomerId = dirtyEntityNotificationPM.CustomerId;
                referentUserId = dirtyEntityNotificationPM.AssigneToId;
            }

            if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.CustomerId)) newNotificationPM.CustomerId = connectedDeclarationPM.CustomerId; // moran 20.6.16 - Task 20789

            if (oldNotificationPM != null)
            {
                oldassigneId = oldNotificationPM.AssigneToId;
            }
            newNotificationPM.DueDate = DateTime.Now;
            if (notificationDefinitionCode == "8227N" || notificationDefinitionCode == "2300N")
            {
                newNotificationPM.AssigneToNotificationTypeCode = "A";
            }
            else
            {
                newNotificationPM.AssigneToNotificationTypeCode = "I";
            }
            newNotificationPM.IsHandledByCustomOffice = true;

            newNotificationPM.AssigneToId =
               NotificationBase.
               CalcAssigneToId(newNotificationPM.Tenant, customerId, referentUserId, notificationDefinitionCode, oldassigneId);

            NotificationBase.CloseAllRelatedNotification(this.currentContext, newNotificationPM, "8227");
            notificationUpdateService.Update(newNotificationPM, true);
        }

        private NotificationPM GetNotification(NotificationQueryService notificationQueryService, NotificationPM newNotificationPM, string notificationDefinitionCode)
        {
            var oldNotificationPM = notificationQueryService.GetNotification(newNotificationPM.Tenant, newNotificationPM.ObjectTableId, newNotificationPM.EntityId, notificationDefinitionCode);
            return oldNotificationPM;
        }
        // moran 16.9.14 - Task 7995 <--
    }
}
