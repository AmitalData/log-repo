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
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public CustomsSettingPM GetSingleCustomsSettingPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsSettingQuery = new CustomsSettingQueryService(customContext);
            CustomsSettingPM CustomsSetting = customsSettingQuery.GetSingle(id, false, false);
            return CustomsSetting;
        }

        public CustomsSettingPM GetFirstCustomsSettingPM(int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsSettingQuery = new CustomsSettingQueryService(customContext);
            CustomsSettingPM CustomsSetting = customsSettingQuery.GetSettingByTenantN(tenant);
            return CustomsSetting;
        }

        public CustomsSettingList GetSingleCustomsSettingList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomsSetting", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsSettingListQueryService listService = new CustomsSettingListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomsSettingList> GetCustomsSettingLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomsSetting", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsSettingListQueryService listService = new CustomsSettingListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsSettingList> GetCustomsSettingFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomsSetting", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsSettingListQueryService listService = new CustomsSettingListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsSettingFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomsSetting", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsSettingListQueryService queryService = new CustomsSettingListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations,tenant);

        }


        public void UpdateCustomsSetting(CustomsSettingPM currententityPm)
        {
            SecurityUtility.CheckContactFeature("Customs.CustomsSetting", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            CustomsSettingUpdateService service = new CustomsSettingUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            string entityKeyString = "GetSettingByTenantN," + currententityPm.Tenant.ToString();
            CacheManager.CacheWrapper.Invalidate(entityKeyString);
            service.Update(currententityPm, true);

        }

        public void InsertCustomsSetting(CustomsSettingPM entityPm)
        {
            SecurityUtility.CheckContactFeature("Customs.CustomsSetting", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            CustomsSettingUpdateService service = new CustomsSettingUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
           
            service.Update(entityPm, true);



        }

    }
}