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

        public InterfaceSendOptionPM GetSingleInterfaceSendOptionPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            interfaceSendOptionQuery = new InterfaceSendOptionQueryService(customContext);
            InterfaceSendOptionPM InterfaceSendOption = interfaceSendOptionQuery.GetSingle(code, false, false);
            return InterfaceSendOption;
        }

        public InterfaceSendOptionList GetSingleInterfaceSendOptionList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            InterfaceSendOptionListQueryService listService = new InterfaceSendOptionListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<InterfaceSendOptionList> GetInterfaceSendOptionLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            InterfaceSendOptionListQueryService listService = new InterfaceSendOptionListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<InterfaceSendOptionList> GetInterfaceSendOptionFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            InterfaceSendOptionListQueryService listService = new InterfaceSendOptionListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetInterfaceSendOptionFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            InterfaceSendOptionListQueryService queryService = new InterfaceSendOptionListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}