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

        public CheckEssenceLookupPM GetSingleCheckEssenceLookupPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            checkEssenceLookupQuery = new CheckEssenceLookupQueryService(customContext);
            CheckEssenceLookupPM CheckEssenceLookup = checkEssenceLookupQuery.GetSingle(id, false, false);
            return CheckEssenceLookup;
        }

        public CheckEssenceLookupList GetSingleCheckEssenceLookupList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CheckEssenceLookupListQueryService listService = new CheckEssenceLookupListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CheckEssenceLookupList> GetCheckEssenceLookupLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CheckEssenceLookupListQueryService listService = new CheckEssenceLookupListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CheckEssenceLookupList> GetCheckEssenceLookupFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            CheckEssenceLookupListQueryService listService = new CheckEssenceLookupListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCheckEssenceLookupFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CheckEssenceLookupListQueryService queryService = new CheckEssenceLookupListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


    }
}