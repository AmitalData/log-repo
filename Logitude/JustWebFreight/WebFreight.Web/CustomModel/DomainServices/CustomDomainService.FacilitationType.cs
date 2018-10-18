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

        public FacilitationTypePM GetSingleFacilitationTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            facilitationTypeQueryService = new FacilitationTypeQueryService(customContext);
            FacilitationTypePM FacilitationType = facilitationTypeQueryService.GetSingle(code, false, false);
            return FacilitationType;
        }

        public FacilitationTypeList GetSingleFacilitationTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          
            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            FacilitationTypeListQueryService listService = new FacilitationTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<FacilitationTypeList> GetFacilitationTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.FacilitationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            FacilitationTypeListQueryService listService = new FacilitationTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<FacilitationTypeList> GetFacilitationTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.FacilitationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            FacilitationTypeListQueryService listService = new FacilitationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetFacilitationTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.FacilitationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            FacilitationTypeListQueryService queryService = new FacilitationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}