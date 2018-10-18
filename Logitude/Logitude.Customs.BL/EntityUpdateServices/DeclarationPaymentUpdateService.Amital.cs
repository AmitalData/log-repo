using Logitude.Customs.BL.ClosedTable;
using Logitude.Customs.BL.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationPaymentUpdateService //.Amital
    {
        private void UpdateUnifreight(DeclarationPaymentPM entityPM, DeclarationPM declaration)
        {/*
            //if(entityPM.)
            var notificationUpdateService = new NotificationUpdateService(this.currentContext);
            var notificationQueryService = new NotificationQueryService(this.currentContext);
            //var id = notificationQueryService.GetNotificationwithManaement(Tenant); 
           
            //var oldNotificationPM = notificationQueryService.GetSingle(
            
            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            newNotificationPM.Tenant = entityPM.Tenant;
            switch (entityPM.ChangeSetOp)
            {
                case Simplog.Server.Infrastructure.ChangeSetOperation.Insert:
                        newNotificationPM.NotificationDefinitionCode = "3050N";
                        break;
                case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                        newNotificationPM.NotificationDefinitionCode = "3050C";
                        break;
                case Simplog.Server.Infrastructure.ChangeSetOperation.Update:
                        newNotificationPM.NotificationDefinitionCode = "3050U";
                        break;
            }
            if(entityPM.ChangeSetOp ==  Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

            }
            //newNotificationPM.NotificationDefinitionCode = NotificationDefinitionDetails.NotificationEnum.POC.ToString();
            newNotificationPM.EntityId = entityPM.DeclarationId;
            newNotificationPM.ObjectTableId = ObjectTabelRepository.GetObjectTableByName("Customs.DeclarationPayment");
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.ClosedByAssignee = "SYSTEM";
            newNotificationPM.IsClosedByAssignee = true;
            if(declaration != null && !string.IsNullOrWhiteSpace(declaration.ReferentUserId))
            {
                newNotificationPM.AssigneToId = declaration.ReferentUserId;
            }
            else if(string.IsNullOrWhiteSpace(entityPM.CreatedByUserId))
            {
                newNotificationPM.AssigneToId = entityPM.CreatedByUserId;
            }
           // CustomerAccountManagerByProductstable
            if(declaration != null && !string.IsNullOrWhiteSpace(declaration.DeclarationOfficeCode))
            {
                newNotificationPM.IsHandledByCustomOffice = true;
                newNotificationPM.IsClosedBCustomOffice = true;
                newNotificationPM.ClosedByCustomOfficeUserId = "SYSTEM";
            }
            newNotificationPM.UserNotes = "התראה נסגרה אוטומטית בעקבות התראה מספר " + newNotificationPM.Id + ", קוד התראה " + newNotificationPM.NotificationDefinitionCode;

            notificationUpdateService.Update(newNotificationPM, true);
        */}
    }
}
