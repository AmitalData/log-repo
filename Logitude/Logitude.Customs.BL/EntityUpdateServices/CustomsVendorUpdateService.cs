using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Customs.BL.Models;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.CustomsMessaging.Common.RequestParams;
using System.Data;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomsVendorUpdateService : EntityUpdateService<CustomsVendor, CustomsVendorPM, EntityPM>
    {
        protected override void OnCreating(CustomsVendorPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.CustomsVendor", entityPM.Tenant);
            //entityPM.Id = entityPM.Id;
        }

        protected override void OnUpdating(CustomsVendorPM entityPM)
        {
            //CustomsSettingQueryService settingsQuery = new CustomsSettingQueryService(entityPM.Tenant);
        
            UpdateUnifreight(entityPM);                      
            ConnectToCustomsRequestsSheet(entityPM);
            UpdateNotification(entityPM);
        }

        private void ConnectToCustomsRequestsSheet(CustomsVendorPM dirtyEntityPM)
        {
            if (string.IsNullOrWhiteSpace(dirtyEntityPM.CustomsRequestsSheetId))
            {
                return;
            }
            var entityId = dirtyEntityPM.Id;
            var tableName = "Customs.CustomsVendor";
            var objectTableId = ObjectTableRepository.GetObjectTableByName(tableName);
            string customsRequestsSheetId = dirtyEntityPM.CustomsRequestsSheetId;

            int tenant = dirtyEntityPM.Tenant;
            
            //CustomsRequestsSheetService<RequestParamsBase> customsRequestsSheetService = null;
            //CustomsRequestsSheetService<RequestParamsBase>.Seed(customsRequestsSheetId, dirtyEntityPM.Tenant, null, out customsRequestsSheetService);
            //customsRequestsSheetService.PostUpdateConnectedEntitys(objectTableId, entityId,true,true);
            CustomsRequestsSheetPostUpdateService.PostUpdateConnectedEntitys(dirtyEntityPM.Tenant, dirtyEntityPM.CustomsRequestsSheetId,
                objectTableId, entityId, true, true);

        }
        protected override void UpdateComposition(CustomsVendorPM entityPM)
        {
            VendorCommunicationUpdateService vendorCommunicationUpdateService = new VendorCommunicationUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            vendorCommunicationUpdateService.UpdateMulti(entityPM.VendorCommunications, entityPM.DeletedVendorCommunications, entityPM, false);

            base.UpdateComposition(entityPM);
        }
     
        private void UpdateNotification(CustomsVendorPM dirtyEntityPM)
        {
            string loggingUserId = AuthenticationUtil.ResolveUserId(dirtyEntityPM.Tenant);

            string notificationDefinitionCode = "";
            var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                    case EventContextTagModel.ProccessEnum.VE_3681_VendorErrorOrCriticalChangeResponseServiceUpdate:
                        notificationDefinitionCode = "3681I";
                        break;
                    case EventContextTagModel.ProccessEnum.VE_3681_VendorErrorOrCriticalChangeResponseServiceDefect:
                        notificationDefinitionCode = "3681C";
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(notificationDefinitionCode))
            {
                DoUpdateNotification(dirtyEntityPM, loggingUserId, notificationDefinitionCode);
            }
        }


        private void DoUpdateNotification(CustomsVendorPM dirtyEntityPM, string loggingUserId, string notificationDefinitionCode)
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
                case "3681I":
                    newNotificationPM.NotificationDefinitionCode = "3681I";
                    desc = "ספק " + dirtyEntityPM.VendorNumber + " " + dirtyEntityPM.VendorName + " עודכן " + "\n" + "סוג תנועה : " + dirtyEntityPM.TransactionTypeID;
                    if (!string.IsNullOrWhiteSpace(dirtyEntityPM.TransactionTypeID))
                    {
                        VendorTransactionTypeQueryService vendorTransactionTypeQueryService = new VendorTransactionTypeQueryService(dirtyEntityPM.Tenant);
                        VendorTransactionTypePM vendorTransactionType = vendorTransactionTypeQueryService.GetSingle(dirtyEntityPM.TransactionTypeID, false, true);
                        desc = desc + " (" + vendorTransactionType.LocalName + ")";
                    }
                    if (dirtyEntityPM.DeadlineToFix != DateTime.MinValue)
                    {
                        desc = desc + "\n" + "תאריך לסיום תיקון: " + dirtyEntityPM.DeadlineToFix.Date.ToString("dd/MM/yyyy");
                    }
                    desc = desc + "\n" + "הערות: " + dirtyEntityPM.NotesForCustomsAgent;
                    newNotificationPM.AssigneToNotificationTypeCode = "I";
                    newNotificationPM.DueDate = DateTime.Now;
                    break;
                case "3681C":
                    newNotificationPM.NotificationDefinitionCode = "3681C";
                    desc = "ספק " + dirtyEntityPM.VendorNumber + " " + dirtyEntityPM.VendorName + "\n" + "סטטוס שגוי " + "\n" + "סוג תנועה : " + dirtyEntityPM.TransactionTypeID;
                    if (!string.IsNullOrWhiteSpace(dirtyEntityPM.TransactionTypeID))
                    {
                        VendorTransactionTypeQueryService vendorTransactionTypeQueryService = new VendorTransactionTypeQueryService(dirtyEntityPM.Tenant);
                        VendorTransactionTypePM vendorTransactionType = vendorTransactionTypeQueryService.GetSingle(dirtyEntityPM.TransactionTypeID, false, true);
                        desc = desc + " (" + vendorTransactionType.LocalName + ")";
                    }
                    if (dirtyEntityPM.DeadlineToFix != DateTime.MinValue)
                    {
                        desc = desc + "\n" + "תאריך לסיום תיקון: " + dirtyEntityPM.DeadlineToFix.Date.ToString("dd/MM/yyyy");
                        newNotificationPM.DueDate = dirtyEntityPM.DeadlineToFix;
                    }
                    desc = desc + "\n" + "הערות: " + dirtyEntityPM.NotesForCustomsAgent;
                    newNotificationPM.AssigneToNotificationTypeCode = "A";
                    if (dirtyEntityPM.SubstituteVendorID != null)
                    {
                        newNotificationPM.Reference1Number = dirtyEntityPM.SubstituteVendorID.ToString();
                    }
                    break;
            }

            newNotificationPM.EntityId = dirtyEntityPM.Id;
            newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CustomsVendor");
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = desc;
            newNotificationPM.ResponseNotes = dirtyEntityPM.NotesForCustomsAgent;

            //Eitan H 13916 -->
            string oldassigneId = null;
            string referentUserId = null;
            string customerId = null;
            var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;

            if (eventContextTagModel.MyNotificationPM != null)
            {
                customerId = eventContextTagModel.MyNotificationPM.CustomerId; // moran 13.9.16 - Bug 22397 change from MyNotificationPM.AssigneToId to MyNotificationPM.CustomerId
                referentUserId = eventContextTagModel.MyNotificationPM.Reference1Number;
            }
            if (customerId != null) newNotificationPM.CustomerId = customerId; // moran 20.6.16 - Task 20789

            newNotificationPM.AssigneToId = NotificationBase.CalcAssigneToId(newNotificationPM.Tenant, customerId, referentUserId, notificationDefinitionCode, oldassigneId);
            //<-- Eitan H 13916

            NotificationBase.CloseAllRelatedNotification(dbContext, newNotificationPM, "3681");
            notificationUpdateService.Update(newNotificationPM, true);

        }


        protected override void CheckConcurrency(CustomsVendorPM entityPM, CustomsVendor entityPOCO)
        {
            //if (!entityPM.ConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID) && !entityPM.NewConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID))
            //if (entityPM.ConcurrencyGUID != entityPOCO.ConcurrencyGUID && entityPM.NewConcurrencyGUID != entityPOCO.ConcurrencyGUID)
            //{
            //    string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
            //    throw new OptimisticConcurrencyException(msg);
            //}

        }
    }
}
