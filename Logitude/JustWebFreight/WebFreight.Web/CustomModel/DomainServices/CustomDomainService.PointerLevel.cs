using System.Collections.Generic;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public PointerLevelPM GetSinglePointerLevelPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            pointerLevelQuery = new PointerLevelQueryService(customContext);
            PointerLevelPM PointerLevel = pointerLevelQuery.GetSingle(id, false, false);
            return PointerLevel;
        }

        public PointerLevelList GetSinglePointerLevelList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            PointerLevelListQueryService listService = new PointerLevelListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<PointerLevelList> GetPointerLevelLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            PointerLevelListQueryService listService = new PointerLevelListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<PointerLevelList> GetPointerLevelFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            PointerLevelListQueryService listService = new PointerLevelListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetPointerLevelFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            PointerLevelListQueryService queryService = new PointerLevelListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}