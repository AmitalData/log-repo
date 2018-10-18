using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.Helpers;
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
        public CustomsBranchPM GetSingleCustomsBranchPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            CustomsBranchQuery = new CustomsBranchQueryService(customContext);
            CustomsBranchPM CustomsBranch = CustomsBranchQuery.GetSingle(id, false, false);
            return CustomsBranch;
        }

        public CustomsBranchList GetSingleCustomsBranchList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsBranchListQueryService listService = new CustomsBranchListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomsBranchList> GetCustomsBranchLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsBranchListQueryService listService = new CustomsBranchListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsBranchList> GetCustomsBranchFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomsBranchListQueryService listService = new CustomsBranchListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsBranchFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsBranchListQueryService queryService = new CustomsBranchListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}