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

        public SpecializationTypePM GetSingleSpecializationTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            specializationTypeQuery = new SpecializationTypeQueryService(customContext);
            SpecializationTypePM SpecializationType = specializationTypeQuery.GetSingle(code, false, false);
            return SpecializationType;
        }

        public SpecializationTypeList GetSingleSpecializationTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.SpecializationType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            SpecializationTypeListQueryService listService = new SpecializationTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<SpecializationTypeList> GetSpecializationTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.SpecializationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            SpecializationTypeListQueryService listService = new SpecializationTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<SpecializationTypeList> GetSpecializationTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.SpecializationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            SpecializationTypeListQueryService listService = new SpecializationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetSpecializationTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.SpecializationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            SpecializationTypeListQueryService queryService = new SpecializationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}