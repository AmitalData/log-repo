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


        public DebtNotificationTypePM GetSingleDebtNotificationTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            debtNotificationTypeQuery = new DebtNotificationTypeQueryService(customContext);
            DebtNotificationTypePM DebtNotificationType = debtNotificationTypeQuery.GetSingle(id, false, false);
            return DebtNotificationType;
        }

        public DebtNotificationTypeList GetSingleDebtNotificationTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);


            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DebtNotificationTypeListQueryService listService = new DebtNotificationTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<DebtNotificationTypeList> GetDebtNotificationTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            DebtNotificationTypeListQueryService listService = new DebtNotificationTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DebtNotificationTypeList> GetDebtNotificationTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);


            customContext = CustomContext.GetContext(tenant);
            DebtNotificationTypeListQueryService listService = new DebtNotificationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetDebtNotificationTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            DebtNotificationTypeListQueryService queryService = new DebtNotificationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


    }
}