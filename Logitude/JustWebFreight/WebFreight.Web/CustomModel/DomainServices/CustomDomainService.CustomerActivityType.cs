using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public CustomerActivityTypePM GetSingleCustomerActivityTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customerActivityTypeQueryService = new CustomerActivityTypeQueryService(customContext);
            CustomerActivityTypePM CustomerActivityType = customerActivityTypeQueryService.GetSingle(id, false, false);
            return CustomerActivityType;
        }

        public CustomerActivityTypeList GetSingleCustomerActivityTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomerActivityTypeListQueryService listService = new CustomerActivityTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomerActivityTypeList> GetCustomerActivityTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomerActivityTypeListQueryService listService = new CustomerActivityTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomerActivityTypeList> GetCustomerActivityTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomerActivityTypeListQueryService listService = new CustomerActivityTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomerActivityTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomerActivityTypeListQueryService queryService = new CustomerActivityTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


       
    }
}