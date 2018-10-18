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
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public GuaranteeCustomerActivityPM GetSingleGuaranteeCustomerActivityPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            guaranteeCustomerActivityQuery = new GuaranteeCustomerActivityQueryService(customContext);
            GuaranteeCustomerActivityPM GuaranteeCustomerActivity = guaranteeCustomerActivityQuery.GetSingle(id, false, false);
            return GuaranteeCustomerActivity;
        }

        public GuaranteeCustomerActivityList GetSingleGuaranteeCustomerActivityList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.GuaranteeCustomerActivity", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            GuaranteeCustomerActivityListQueryService listService = new GuaranteeCustomerActivityListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<GuaranteeCustomerActivityList> GetGuaranteeCustomerActivityLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.GuaranteeCustomerActivity", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            GuaranteeCustomerActivityListQueryService listService = new GuaranteeCustomerActivityListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<GuaranteeCustomerActivityList> GetGuaranteeCustomerActivityFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.GuaranteeCustomerActivity", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            GuaranteeCustomerActivityListQueryService listService = new GuaranteeCustomerActivityListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetGuaranteeCustomerActivityFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //      SecurityUtility.CheckContactFeature("Customs.GuaranteeCustomerActivity", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            GuaranteeCustomerActivityListQueryService queryService = new GuaranteeCustomerActivityListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}