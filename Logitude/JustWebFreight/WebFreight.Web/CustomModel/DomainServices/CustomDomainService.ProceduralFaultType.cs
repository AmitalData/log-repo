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

        public ProceduralFaultTypePM GetSingleProceduralFaultTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            proceduralFaultTypeQuery = new ProceduralFaultTypeQueryService(customContext);
            ProceduralFaultTypePM ProceduralFaultType = proceduralFaultTypeQuery.GetSingle(code, false, false);
            return ProceduralFaultType;
        }

        public ProceduralFaultTypeList GetSingleProceduralFaultTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProceduralFaultType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ProceduralFaultTypeListQueryService listService = new ProceduralFaultTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ProceduralFaultTypeList> GetProceduralFaultTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProceduralFaultType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProceduralFaultTypeListQueryService listService = new ProceduralFaultTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ProceduralFaultTypeList> GetProceduralFaultTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProceduralFaultType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProceduralFaultTypeListQueryService listService = new ProceduralFaultTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetProceduralFaultTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProceduralFaultType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProceduralFaultTypeListQueryService queryService = new ProceduralFaultTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}