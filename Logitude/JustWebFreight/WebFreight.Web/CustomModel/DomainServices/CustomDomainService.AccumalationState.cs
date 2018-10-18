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

        public AccumalationStatePM GetSingleAccumalationStatePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            accumalationStateQueryService = new AccumalationStateQueryService(customContext);
            AccumalationStatePM AccumalationState = accumalationStateQueryService.GetSingle(id, false, false);
            return AccumalationState;
        }

        public AccumalationStateList GetSingleAccumalationStateList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            AccumalationStateListQueryService listService = new AccumalationStateListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<AccumalationStateList> GetAccumalationStateLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            AccumalationStateListQueryService listService = new AccumalationStateListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<AccumalationStateList> GetAccumalationStateFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            AccumalationStateListQueryService listService = new AccumalationStateListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetAccumalationStateFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            AccumalationStateListQueryService queryService = new AccumalationStateListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}