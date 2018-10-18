using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Customs.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public void UpdateCustomsHouseTypeList(CustomsHouseTypeList customsHouseType)
        {

        }


        public CustomsHouseTypePM GetSingleCustomsHouseTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsHouseTypeQuery = new CustomsHouseTypeQueryService(customContext);
            CustomsHouseTypePM CustomsHouseType = customsHouseTypeQuery.GetHouseTypewithAdditional(code, tenant);

            return CustomsHouseType;

            //else
            //{
            //     CustomsHouseType = customsHouseTypeQuery.GetSingle(code, false, false);
            //     CustomsHouseType.Tenant = tenant;
            //    return CustomsHouseType;

            //}

        }

        public CustomsHouseTypeList GetSingleCustomsHouseTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsHouseType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsHouseTypeListQueryService listService = new CustomsHouseTypeListQueryService(customContext);
            listService.Tenant = tenant;
            return listService.GetSingle(code);
        }

        public CustomsHouseTypePM GetHouseTypewithAdditional(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsHouseType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);

            customsHouseTypeQuery = new CustomsHouseTypeQueryService(customContext);
            CustomsHouseTypePM CustomsHouseType = customsHouseTypeQuery.GetHouseTypewithAdditional(code, tenant);
            return CustomsHouseType;
        }

        public List<CustomsHouseTypeList> GetCustomsHouseTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsHouseType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsHouseTypeListQueryService listService = new CustomsHouseTypeListQueryService(customContext);
            listService.Tenant = tenant;
            return listService.GetList(tenant);
        }


        public List<CustomsHouseTypeList> GetCustomsHouseTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsHouseType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsHouseTypeListQueryService listService = new CustomsHouseTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            listService.Tenant = tenant;
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsHouseTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsHouseType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsHouseTypeListQueryService queryService = new CustomsHouseTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            queryService.Tenant = tenant;
            return queryService.GetListCount(queryOperations);

        }


        public void UpdateCustomsHouseType(CustomsHouseTypePM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.CustomsHouseType", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            CustomsHouseTypeUpdateService service = new CustomsHouseTypeUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            customsHouseTypeQuery = new CustomsHouseTypeQueryService(customContext);
            CustomsHouseTypePM CustomsHouseType = customsHouseTypeQuery.GetHouseTypewithAdditional(currententityPm.Code, currententityPm.Tenant);
            if (CustomsHouseType != null)
            {
                service.Update(currententityPm, true);
            }
            //else
            //{
            //    CustomsHouseTypeAdditionalRepository additionalRep = new CustomsHouseTypeAdditionalRepository(customContext);
            //    additionalRep.Add(new CustomsHouseTypeAdditional() { Id = IdCounter.GetNumber("Customs.CustomsHouseTypeAdditional", currententityPm.Tenant).ToString(), Tenant = currententityPm.Tenant, Code = currententityPm.Code });
            //    additionalRep.SubmitChanges();
            //    service.Update(currententityPm, true);
            //}
          

        }

    }
}