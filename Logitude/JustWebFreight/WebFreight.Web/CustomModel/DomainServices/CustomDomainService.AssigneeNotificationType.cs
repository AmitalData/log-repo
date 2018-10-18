using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;


namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public AssigneeNotificationTypePM GetSingleAssigneeNotificationTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            assigneeNotificationTypeQuery = new AssigneeNotificationTypeQueryService(customContext);
            AssigneeNotificationTypePM AssigneeNotificationType = assigneeNotificationTypeQuery.GetSingle(id, false, false);
            return AssigneeNotificationType;
        }

        public AssigneeNotificationTypeList GetSingleAssigneeNotificationTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.AssigneeNotificationType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            AssigneeNotificationTypeListQueryService listService = new AssigneeNotificationTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<AssigneeNotificationTypeList> GetAssigneeNotificationTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.AssigneeNotificationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AssigneeNotificationTypeListQueryService listService = new AssigneeNotificationTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<AssigneeNotificationTypeList> GetAssigneeNotificationTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.AssigneeNotificationType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            AssigneeNotificationTypeListQueryService listService = new AssigneeNotificationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetAssigneeNotificationTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.AssigneeNotificationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AssigneeNotificationTypeListQueryService queryService = new AssigneeNotificationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


    }
}