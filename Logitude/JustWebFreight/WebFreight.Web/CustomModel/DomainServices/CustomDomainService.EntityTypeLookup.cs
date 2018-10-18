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

        public EntityTypeLookupPM GetSingleEntityTypeLookupPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            entityTypeLookupQuery = new EntityTypeLookupQueryService(customContext);
            EntityTypeLookupPM EntityTypeLookup = entityTypeLookupQuery.GetSingle(code, false, false);
            return EntityTypeLookup;
        }

        public EntityTypeLookupList GetSingleEntityTypeLookupList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.EntityTypeLookup", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            EntityTypeLookupListQueryService listService = new EntityTypeLookupListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<EntityTypeLookupList> GetEntityTypeLookupLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.EntityTypeLookup", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            EntityTypeLookupListQueryService listService = new EntityTypeLookupListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<EntityTypeLookupList> GetEntityTypeLookupFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.EntityTypeLookup", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            EntityTypeLookupListQueryService listService = new EntityTypeLookupListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetEntityTypeLookupFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //      SecurityUtility.CheckContactFeature("Customs.EntityTypeLookup", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            EntityTypeLookupListQueryService queryService = new EntityTypeLookupListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


    }
}