using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class NotificationDefinitionQueryService : EntityQueryService<NotificationDefinition, NotificationDefinitionKeys, NotificationDefinitionPM, object, NotificationDefinitionKeys>

    {

        //public List<NotificationDefinitionList> GetNotificationDefinitionwithDefinition(int tenant)
        //{

        //    NotificationTenantDefinitionRepository definitionRep = new NotificationTenantDefinitionRepository(context);
            
        //    IQueryable<NotificationTenantDefinition> definitions = definitionRep.GetAll(tenant);

        //    List<NotificationDefinitionList> notificationDefinitions = (from a in context.NotificationDefinitions.Include("AssigneeNotificationType")
        //                                                                join d in definitions
        //                                                 on a.Code equals d.Code into xy
        //                                                 from s in xy.DefaultIfEmpty()
        //                                                                select new NotificationDefinitionList()
        //                                                 {
        //                                                     AssigneeNotificationTypeCode = a.AssigneeNotificationTypeCode,
        //                                                     AssigneeNotificationTypeName = a.AssigneeNotificationType != null? a.AssigneeNotificationType.LocalName : null,
        //                                                     Code = a.Code,
        //                                                     DefaultAssigneeId = s.DefaultAssigneeId,
        //                                                     DefaultAssigneeName = s.User!= null? s.User.Contact.LocalName : null,
        //                                                     EnglishName = a.EnglishName,
        //                                                     LocalName = a.LocalName,


        //                                                 }).ToList();


        //    return notificationDefinitions;


        //}

        public NotificationDefinitionPM GeNotificationDefinitionwithDefinition(string code, int tenant)
        {
            NotificationDefinitionPM notificationDefinition = null;
            if (!string.IsNullOrWhiteSpace(code))
{
                NotificationDefinition notification = repository.GetSingle(new NotificationDefinitionKeys() { Code = code });
                NotificationTenantDefinitionRepository definitionRepository = new NotificationTenantDefinitionRepository(context);
                NotificationTenantDefinition definition = definitionRepository.GetSingleNotificationTenantDefinitionByCode(code, tenant);
                notificationDefinition = new NotificationDefinitionPM()
    {
                    Code = notification.Code,
                    EnglishName = notification.EnglishName,
                    LocalName = notification.LocalName,
                    AssigneeNotificationTypeCode = notification.AssigneeNotificationTypeCode,
                    
                    Tenant = tenant
                };
                if (definition != null)
                {
                    notificationDefinition.DefaultAssigneeId = definition.DefaultAssigneeId;
                    notificationDefinition.DefaultAssigneeName = definition.User != null ? definition.User.Contact.LocalName : null;
                  
                }
            }
            return notificationDefinition;
        }



    }
}
