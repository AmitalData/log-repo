using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.Data.EntityListQueryServices;
using Logitude.TariffModule.Data.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.TariffModel.DomainServices
{
    public partial class TariffDomainService : LogitudeDomainService
    {
        public List<TariffList> GetTariffFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Tariff", "READ", tenant);

            ITariffModuleContext context = TariffModuleContext.GetContext(tenant);           

            TariffListQueryService listService = new TariffListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<TariffList> listQuery = listService.GetList(queryOperations, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetCustomFieldsValues("Tariff", tenant, listQuery.Cast<object>().ToList());

            return listQuery;
        }

        public int GetTariffFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Tariff", "READ", tenant);

            ITariffModuleContext context = TariffModuleContext.GetContext(tenant);

            TariffListQueryService queryService = new TariffListQueryService(context);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }
    }
}