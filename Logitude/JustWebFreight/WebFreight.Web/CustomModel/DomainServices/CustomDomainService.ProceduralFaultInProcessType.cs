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
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public ProceduralFaultInProcessTypePM GetSingleProceduralFaultInputProcessTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            proceduralFaultInputProcessTypeQuery = new ProceduralFaultInProcessTypeQueryService(customContext);
            ProceduralFaultInProcessTypePM ProceduralFaultInputProcessType = proceduralFaultInputProcessTypeQuery.GetSingle(code, false, false);
            return ProceduralFaultInputProcessType;
        }

        public ProceduralFaultInProcessTypeList GetSingleProceduralFaultInputProcessTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProceduralFaultInputProcessType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ProceduralFaultInProcessTypeListQueryService listService = new ProceduralFaultInProcessTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ProceduralFaultInProcessTypeList> GetProceduralFaultInputProcessTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProceduralFaultInputProcessType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProceduralFaultInProcessTypeListQueryService listService = new ProceduralFaultInProcessTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ProceduralFaultInProcessTypeList> GetProceduralFaultInputProcessTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProceduralFaultInputProcessType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProceduralFaultInProcessTypeListQueryService listService = new ProceduralFaultInProcessTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetProceduralFaultInputProcessTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProceduralFaultInputProcessType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProceduralFaultInProcessTypeListQueryService queryService = new ProceduralFaultInProcessTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}