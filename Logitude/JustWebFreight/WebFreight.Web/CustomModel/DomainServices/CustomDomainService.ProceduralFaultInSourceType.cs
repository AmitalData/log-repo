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
        public ProceduralFaultInSourceTypePM GetSingleProceduralFaultInputSourceTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            proceduralFaultInputSourceTypeQuery = new ProceduralFaultInSourceTypeQueryService(customContext);
            ProceduralFaultInSourceTypePM ProceduralFaultInputSourceType = proceduralFaultInputSourceTypeQuery.GetSingle(code, false, false);
            return ProceduralFaultInputSourceType;
        }

        public ProceduralFaultInSourceTypeList GetSingleProceduralFaultInputSourceTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProceduralFaultInputSourceType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ProceduralFaultInSourceTypeListQueryService listService = new ProceduralFaultInSourceTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ProceduralFaultInSourceTypeList> GetProceduralFaultInputSourceTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProceduralFaultInputSourceType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProceduralFaultInSourceTypeListQueryService listService = new ProceduralFaultInSourceTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ProceduralFaultInSourceTypeList> GetProceduralFaultInputSourceTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProceduralFaultInputSourceType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProceduralFaultInSourceTypeListQueryService listService = new ProceduralFaultInSourceTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetProceduralFaultInSourceTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProceduralFaultInputSourceType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProceduralFaultInSourceTypeListQueryService queryService = new ProceduralFaultInSourceTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}