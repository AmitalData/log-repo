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


        public CustomerRoleTypePM GetSingleCustomerRoleTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customerRoleTypeQuery = new CustomerRoleTypeQueryService(customContext);
            CustomerRoleTypePM CustomerRoleType = customerRoleTypeQuery.GetSingle(code, false, false);
            return CustomerRoleType;
        }

        public CustomerRoleTypeList GetSingleCustomerRoleTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
     //       SecurityUtility.CheckContactFeature("Customs.CustomerRoleType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomerRoleTypeListQueryService listService = new CustomerRoleTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<CustomerRoleTypeList> GetCustomerRoleTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
      //      SecurityUtility.CheckContactFeature("Customs.CustomerRoleType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomerRoleTypeListQueryService listService = new CustomerRoleTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomerRoleTypeList> GetCustomerRoleTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
      //      SecurityUtility.CheckContactFeature("Customs.CustomerRoleType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomerRoleTypeListQueryService listService = new CustomerRoleTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomerRoleTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
       //     SecurityUtility.CheckContactFeature("Customs.CustomerRoleType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomerRoleTypeListQueryService queryService = new CustomerRoleTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}