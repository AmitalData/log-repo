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


        public AddressPurposePM GetSingleAddressPurposePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
           addressPurposeQuery = new AddressPurposeQueryService(customContext);
            AddressPurposePM AddressPurpose = addressPurposeQuery.GetSingle(code, false, false);
            return AddressPurpose;
        }

        public AddressPurposeList GetSingleAddressPurposeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.AddressPurpose", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            AddressPurposeListQueryService listService = new AddressPurposeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<AddressPurposeList> GetAddressPurposeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.AddressPurpose", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AddressPurposeListQueryService listService = new AddressPurposeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<AddressPurposeList> GetAddressPurposeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.AddressPurpose", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AddressPurposeListQueryService listService = new AddressPurposeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetAddressPurposeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.AddressPurpose", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AddressPurposeListQueryService queryService = new AddressPurposeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}