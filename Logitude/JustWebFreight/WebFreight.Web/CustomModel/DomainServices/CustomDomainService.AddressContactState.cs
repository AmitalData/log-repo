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

        public AddressContactStatePM GetSingleAddressContactStatePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            addressContactStateQuery = new AddressContactStateQueryService(customContext);
            AddressContactStatePM AddressContactState = addressContactStateQuery.GetSingle(code, false, false);
            return AddressContactState;
        }

        public AddressContactStateList GetSingleAddressContactStateList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.AddressContactState", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            AddressContactStateListQueryService listService = new AddressContactStateListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<AddressContactStateList> GetAddressContactStateLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.AddressContactState", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AddressContactStateListQueryService listService = new AddressContactStateListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<AddressContactStateList> GetAddressContactStateFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.AddressContactState", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AddressContactStateListQueryService listService = new AddressContactStateListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetAddressContactStateFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.AddressContactState", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AddressContactStateListQueryService queryService = new AddressContactStateListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}