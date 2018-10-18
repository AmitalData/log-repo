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

        public CustomerIdentifyTypePM GetSingleCustomerIdentifyTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customerIdentifyTypeQuery = new CustomerIdentifyTypeQueryService(customContext);
            CustomerIdentifyTypePM CustomerIdentifyType = customerIdentifyTypeQuery.GetSingle(code, false, false);
            return CustomerIdentifyType;
        }

        public CustomerIdentifyTypeList GetSingleCustomerIdentifyTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomerIdentifyType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomerIdentifyTypeListQueryService listService = new CustomerIdentifyTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<CustomerIdentifyTypeList> GetCustomerIdentifyTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomerIdentifyType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomerIdentifyTypeListQueryService listService = new CustomerIdentifyTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomerIdentifyTypeList> GetCustomerIdentifyTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomerIdentifyType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomerIdentifyTypeListQueryService listService = new CustomerIdentifyTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomerIdentifyTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomerIdentifyType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomerIdentifyTypeListQueryService queryService = new CustomerIdentifyTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}