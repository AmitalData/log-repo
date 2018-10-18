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
        public FreightPaymentMethodPM GetSingleFreightPaymentMethodPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            freightPaymentMethodQueryService = new FreightPaymentMethodQueryService(customContext);
            FreightPaymentMethodPM FreightPaymentMethod = freightPaymentMethodQueryService.GetSingle(code, false, false);
            return FreightPaymentMethod;
        }

        public FreightPaymentMethodList GetSingleFreightPaymentMethodList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.FreightPaymentMethod", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            FreightPaymentMethodListQueryService listService = new FreightPaymentMethodListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<FreightPaymentMethodList> GetFreightPaymentMethodLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            FreightPaymentMethodListQueryService listService = new FreightPaymentMethodListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<FreightPaymentMethodList> GetFreightPaymentMethodFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            FreightPaymentMethodListQueryService listService = new FreightPaymentMethodListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetFreightPaymentMethodFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            FreightPaymentMethodListQueryService queryService = new FreightPaymentMethodListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}