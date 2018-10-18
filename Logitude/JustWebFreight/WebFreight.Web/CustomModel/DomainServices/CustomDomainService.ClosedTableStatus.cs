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

        public ClosedTableStatusPM GetSingleClosedTableStatusPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            closedTableStatusQuery = new ClosedTableStatusQueryService(customContext);
            ClosedTableStatusPM ClosedTableStatus = closedTableStatusQuery.GetSingle(code, false, false);
            return ClosedTableStatus;
        }

        public ClosedTableStatusList GetSingleClosedTableStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ClosedTableStatus", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ClosedTableStatusListQueryService listService = new ClosedTableStatusListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ClosedTableStatusList> GetClosedTableStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ClosedTableStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ClosedTableStatusListQueryService listService = new ClosedTableStatusListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ClosedTableStatusList> GetClosedTableStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ClosedTableStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ClosedTableStatusListQueryService listService = new ClosedTableStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetClosedTableStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ClosedTableStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ClosedTableStatusListQueryService queryService = new ClosedTableStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}