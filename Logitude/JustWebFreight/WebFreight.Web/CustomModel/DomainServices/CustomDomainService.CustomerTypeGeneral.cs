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

        public CustomerTypeGeneralPM GetSingleCustomerTypeGeneralPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customerTypeGeneralQuery = new CustomerTypeGeneralQueryService(customContext);
            CustomerTypeGeneralPM customerTypeGeneral = customerTypeGeneralQuery.GetSingle(code, false, false);
            return customerTypeGeneral;
        }

        public CustomerTypeGeneralList GetSingleCustomerTypeGeneralList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomerTypeGeneral", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomerTypeGeneralListQueryService listService = new CustomerTypeGeneralListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<CustomerTypeGeneralList> GetCustomerTypeGeneralLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomerTypeGeneral", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomerTypeGeneralListQueryService listService = new CustomerTypeGeneralListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomerTypeGeneralList> GetCustomerTypeGeneralFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomerTypeGeneral", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomerTypeGeneralListQueryService listService = new CustomerTypeGeneralListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomerTypeGeneralFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomerTypeGeneral", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomerTypeGeneralListQueryService queryService = new CustomerTypeGeneralListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}