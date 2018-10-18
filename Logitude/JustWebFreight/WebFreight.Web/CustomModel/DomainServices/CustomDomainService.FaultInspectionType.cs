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
        public FaultInspectionTypePM GetSingleFaultInspectionTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            faultInspectionTypeQuery = new FaultInspectionTypeQueryService(customContext);
            FaultInspectionTypePM FaultInspectionType = faultInspectionTypeQuery.GetSingle(code, false, false);
            return FaultInspectionType;
        }

        public FaultInspectionTypeList GetSingleFaultInspectionTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.FaultInspectionType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            FaultInspectionTypeListQueryService listService = new FaultInspectionTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<FaultInspectionTypeList> GetFaultInspectionTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.FaultInspectionType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            FaultInspectionTypeListQueryService listService = new FaultInspectionTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<FaultInspectionTypeList> GetFaultInspectionTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.FaultInspectionType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            FaultInspectionTypeListQueryService listService = new FaultInspectionTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetFaultInspectionTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.FaultInspectionType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            FaultInspectionTypeListQueryService queryService = new FaultInspectionTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}