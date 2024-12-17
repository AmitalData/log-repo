using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;

namespace Logitude.Customs.BL.NotificationBL
{
    public class NotificationBase
        //<TPM>        where TPM : EntityPM
    {
#if false
        

        EntityPM _EntityPM;
        void JustDoIt(EntityPM entityPM)
        {
            _EntityPM=entityPM;
            CreateNew();
            
            CloseAllRelatedNotification();
        }

        private void CreateNew()
        {
            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = _EntityPM.Tenant;
            newNotificationPM.EntityId = declarationPM.Id;
            newNotificationPM.ObjectTableId = ObjectTabelRepository.GetObjectTableByName("Customs.Declaration");

            LogMessagingUtil.Instance.AppendLine("New Release Notification");
            newNotificationPM.NotificationDefinitionCode = "2740N";

            newNotificationPM.DueDate = DateTime.Now;            
            newNotificationPM.CreateDate = DateTime.Now;

            newNotificationPM.Description = "התרה לתיק עמילות " + declarationPM.CustomFileNo;

            newNotificationPM.DepartmentId = declarationPM.DepartmentId;
            newNotificationPM.DeclarationOfficeCode = declarationPM.DeclarationOfficeCode;


            
            newNotificationPM.AssigneToNotificationTypeCode = "I";
            AssigneTo();

        }
#endif
        public static string CalcAssigneToId(int Tenant, string CustomerId, string ReferentUserId, string notificationDefinitionCode, string oldAssigneToId)
        {

            if (!string.IsNullOrWhiteSpace(oldAssigneToId) && IsValidContact(oldAssigneToId, Tenant))
            {
                return oldAssigneToId;
            }
            else if (!string.IsNullOrWhiteSpace(ReferentUserId) && IsValidContact(ReferentUserId, Tenant))
            {
                return ReferentUserId;
            }
            else if (!string.IsNullOrWhiteSpace(CustomerId))
            {
               /*var clientQueryService = new ClientQueryService(Tenant);
               var client = new ClientPM();
                
                client = clientQueryService.GetSingle(CustomerId, true, false);
                if (client != null)
                {
                    return client.Id;
                }*/
                
                //return CustomerId;
                // moran 13.1.15 - Task 9921 -->
                // Get card Id by VatNumber (cards table) and user by card from CustomerAccountManagerByProduct with code "CC"
                ICommonDataContext CommonContext = CommonDataContext.GetContext(Tenant);
                var cardRepository = new CardRepository(CommonContext);
                Card card = cardRepository.GetSingleCardByVatNumber(CustomerId, Tenant);
                if (card != null && !String.IsNullOrWhiteSpace(card.Id))
                {
                    var customerAccountManagerRepository = new CustomerAccountManagerByProductRepository(CommonContext);
                    CustomerAccountManagerByProduct customerAccountManagerByProduct = customerAccountManagerRepository.GetSingleCustomerAccountManagerByProduct("CC", card.Id, Tenant);
                    if (customerAccountManagerByProduct != null && !String.IsNullOrWhiteSpace(customerAccountManagerByProduct.AccountManagerId) && IsValidContact(customerAccountManagerByProduct.AccountManagerId, Tenant))
                    {
                        return customerAccountManagerByProduct.AccountManagerId;
                    }
                }
                // moran 13.1.15 - Task 9921 <--
            }
            else if (false)
            {

            }
            var notificationTenantDefinitionQueryService = new NotificationTenantDefinitionQueryService(Tenant);
            var notificationTenantDefinitionPM = notificationTenantDefinitionQueryService.GetNotificationTenantDefinition(Tenant, notificationDefinitionCode);
            if (notificationTenantDefinitionPM != null && !string.IsNullOrWhiteSpace(notificationTenantDefinitionPM.DefaultAssigneeId) && IsValidContact(notificationTenantDefinitionPM.DefaultAssigneeId, Tenant))
            {
                return notificationTenantDefinitionPM.DefaultAssigneeId;
            }
            var customsSettingPM = CustomsSettingQueryService.GetSettingByTenant(Tenant);
            if (customsSettingPM != null && !string.IsNullOrWhiteSpace(customsSettingPM.DefaultNotificationAssignee) && IsValidContact(customsSettingPM.DefaultNotificationAssignee, Tenant))
            {
                return customsSettingPM.DefaultNotificationAssignee;
            }
            return null;
        }

        private static bool IsValidContact(string UserId, int Tenant) // Task 18210 - add Active contact check function and calling it in all if's before returning value
        {
            ContactRepository contactRep = new ContactRepository(Tenant);
            Contact contact = contactRep.GetSingleContactByIdAndTenant(UserId, Tenant, true);
            if (contact == null || contact.InActive)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public static void CloseAllRelatedNotification(ICustomContext dbContext, NotificationPM newNotificationPM, string NotificationDefinitionCode)
        {
            //var notificationUpdateService = new NotificationUpdateService(newNotificationPM.Tenant);
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), newNotificationPM.Tenant);
            var notificationQueryService = new NotificationQueryService(newNotificationPM.Tenant);

            /*var openNotificationList = notificationQueryService.GetOpenNotificationList(newNotificationPM.Tenant, newNotificationPM.ObjectTableId, newNotificationPM.EntityId, 
                NotificationDefinitionCode);*/
            var openNotificationList = notificationQueryService.GetOpenNotificationList(newNotificationPM.Tenant, newNotificationPM.ObjectTableId, newNotificationPM.EntityId,
                NotificationDefinitionCode, newNotificationPM.Reference2Number);
            foreach (var item in openNotificationList)
            {
                if (newNotificationPM.Id != item.Id)
                {
                    item.ChangeSetOp = ChangeSetOperation.Update;
                    // moran 30.11.14 - Task 9236 -->
                    //item.IsClosedByAssignee = true;
                    //item.ClosedByAssignee = AuthenticationUtil.ResolveUnifreightUserId(newNotificationPM.Tenant);
                    item.ClosedByAssignee = NotificationBase.CalcAssigneToId(newNotificationPM.Tenant, "", "", NotificationDefinitionCode, newNotificationPM.AssigneToId);
                    if(string.IsNullOrWhiteSpace(item.ClosedByAssignee))
                    {
                        //item.ClosedByAssignee = AuthenticationUtil.ResolveUnifreightUserId(newNotificationPM.Tenant);//Eitan H task 35737
                        item.ClosedByAssignee = AuthenticationUtil.ResolveUserIdOfUnifreightUser(newNotificationPM.Tenant);//Eitan H task 35737
                    }
                    if (!string.IsNullOrWhiteSpace(item.ClosedByAssignee))
                    {
                        item.IsClosedByAssignee = true;
                    }
                    // moran 30.11.14 - Task 9236 <--
                    item.ResponseNotes =  "הודעה נסגרה אוטומטית בעקבות הודעה מספר " + newNotificationPM.Id + ", קוד הודעה " + newNotificationPM.NotificationDefinitionCode;
                    if (newNotificationPM.IsHandledByCustomOffice == true)
                    {
                        item.IsClosedBCustomOffice = true;
                        //item.ClosedByCustomOfficeUserId = AuthenticationUtil.ResolveUnifreightUserId(newNotificationPM.Tenant);
                        item.ClosedByCustomOfficeUserId = AuthenticationUtil.ResolveSystemUserId(newNotificationPM.Tenant); // Mirit 23/05/15 Task 14292
                    }
                    notificationUpdateService.Update(item, true);
                }
            }
}

        public static NotificationPM GetNotification(NotificationPM newNotificationPM, string notificationDefinitionCode)
        {
            var notificationQueryService = new NotificationQueryService(newNotificationPM.Tenant);
            var oldNotificationPM = notificationQueryService.GetNotification(newNotificationPM.Tenant, newNotificationPM.ObjectTableId, newNotificationPM.EntityId, notificationDefinitionCode);
            return oldNotificationPM;
        }
    }
}
