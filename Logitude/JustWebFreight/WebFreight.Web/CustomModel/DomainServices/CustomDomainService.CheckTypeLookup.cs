
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public CheckTypeLookupPM GetSingleCheckTypeLookupPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            checkTypeLookupQuery = new CheckTypeLookupQueryService(customContext);
            CheckTypeLookupPM CheckTypeLookup = checkTypeLookupQuery.GetSingle(code, false, false);
            return CheckTypeLookup;
        }

        public CheckTypeLookupList GetSingleCheckTypeLookupList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.CheckTypeLookup", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CheckTypeLookupListQueryService listService = new CheckTypeLookupListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<CheckTypeLookupList> GetCheckTypeLookupLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.CheckTypeLookup", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CheckTypeLookupListQueryService listService = new CheckTypeLookupListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CheckTypeLookupList> GetCheckTypeLookupFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.CheckTypeLookup", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CheckTypeLookupListQueryService listService = new CheckTypeLookupListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCheckTypeLookupFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.CheckTypeLookup", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CheckTypeLookupListQueryService queryService = new CheckTypeLookupListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


    }
}