using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public NotificationTypePM GetSingleNotificationTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            notificationTypeQuery = new NotificationTypeQueryService(customContext);
            NotificationTypePM NotificationType = notificationTypeQuery.GetSingle(code, false, false);
            return NotificationType;
        }

        public NotificationTypeList GetSingleNotificationTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.NotificationType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            NotificationTypeListQueryService listService = new NotificationTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<NotificationTypeList> GetNotificationTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.NotificationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            NotificationTypeListQueryService listService = new NotificationTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<NotificationTypeList> GetNotificationTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.NotificationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            NotificationTypeListQueryService listService = new NotificationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetNotificationTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.NotificationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            NotificationTypeListQueryService queryService = new NotificationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}