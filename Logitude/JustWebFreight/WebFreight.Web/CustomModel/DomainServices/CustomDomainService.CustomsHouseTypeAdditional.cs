using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
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

        public CustomsHouseTypeAdditionalPM GetSingleCustomsHouseTypeAdditionalPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsHouseTypeAdditionalQuery = new CustomsHouseTypeAdditionalQueryService(customContext);
            CustomsHouseTypeAdditionalPM CustomsHouseTypeAdditional = customsHouseTypeAdditionalQuery.GetSingle(id, false, false);
            return CustomsHouseTypeAdditional;
        }

       

        public CustomsHouseTypeAdditionalList GetSingleCustomsHouseTypeAdditionalList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomsHouseTypeAdditional", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsHouseTypeAdditionalListQueryService listService = new CustomsHouseTypeAdditionalListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomsHouseTypeAdditionalList> GetCustomsHouseTypeAdditionalLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomsHouseTypeAdditional", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsHouseTypeAdditionalListQueryService listService = new CustomsHouseTypeAdditionalListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsHouseTypeAdditionalList> GetCustomsHouseTypeAdditionalFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomsHouseTypeAdditional", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsHouseTypeAdditionalListQueryService listService = new CustomsHouseTypeAdditionalListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsHouseTypeAdditionalFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomsHouseTypeAdditional", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsHouseTypeAdditionalListQueryService queryService = new CustomsHouseTypeAdditionalListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }


        public void UpdateCustomsHouseTypeAdditional(CustomsHouseTypeAdditionalPM currententityPm)
        {
            SecurityUtility.CheckContactFeature("Customs.CustomsHouseTypeAdditional", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            CustomsHouseTypeAdditionalUpdateService service = new CustomsHouseTypeAdditionalUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            service.Update(currententityPm, true);

        }


    }
}