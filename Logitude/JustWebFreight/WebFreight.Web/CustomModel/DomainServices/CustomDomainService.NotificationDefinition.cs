using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Customs.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public NotificationDefinitionPM GetSingleNotificationDefinitionPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            notificationDefinitionQuery = new NotificationDefinitionQueryService(customContext);
            NotificationDefinitionPM notificationDefinition = notificationDefinitionQuery.GeNotificationDefinitionwithDefinition(code, tenant);

            return notificationDefinition;


        }

        public NotificationDefinitionList GetSingleNotificationDefinitionList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.NotificationDefinition", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            NotificationDefinitionListQueryService listService = new NotificationDefinitionListQueryService(customContext);
            listService.Tenant = tenant;
            return listService.GetSingle(id);
        }



        public List<NotificationDefinitionList> GetNotificationDefinitionLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.NotificationDefinition", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            NotificationDefinitionListQueryService listService = new NotificationDefinitionListQueryService(customContext);
            return listService.GetList(tenant);
            return new List<NotificationDefinitionList>();
        }


        public List<NotificationDefinitionList> GetNotificationDefinitionFilters(byte[] xmlFilters, int tenant)
        {


            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.NotificationDefinition", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            NotificationDefinitionListQueryService listService = new NotificationDefinitionListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            listService.Tenant = tenant;
            return listService.GetList(queryOperations, tenant);

        }

        public int GetNotificationDefinitionFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.NotificationDefinition", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            NotificationDefinitionListQueryService queryService = new NotificationDefinitionListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

        //public void InsertNotificationDefinition(NotificationDefinitionPM entityPm)
        //{
        //    SecurityUtility.CheckContactFeature("Customs.NotificationDefinition", "NEW", entityPm.Tenant);

        //    if (customContext == null)
        //    {
        //        customContext = CustomContext.GetContext(entityPm.Tenant);
        //    }

        //    entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

        //    NotificationDefinitionUpdateService service = new NotificationDefinitionUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);


        //    service.Update(entityPm, true);

        //}

        public void UpdateNotificationDefinition(NotificationDefinitionPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.NotificationDefinition", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            NotificationDefinitionUpdateService service = new NotificationDefinitionUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            service.Update(currententityPm, true);

        }


        public void UpdateNotificationDefinitionList(NotificationDefinitionList list)
        {


        }
    }
}