using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.Helpers;
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
        public PaymentMethodTypePM GetSinglePaymentMethodTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            paymentMethodTypeQuery = new PaymentMethodTypeQueryService(customContext);
            PaymentMethodTypePM PaymentMethodType = paymentMethodTypeQuery.GetSingle(id, false, false);
            return PaymentMethodType;
        }

        public PaymentMethodTypeList GetSinglePaymentMethodTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            PaymentMethodTypeListQueryService listService = new PaymentMethodTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<PaymentMethodTypeList> GetPaymentMethodTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentMethodTypeListQueryService listService = new PaymentMethodTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<PaymentMethodTypeList> GetPaymentMethodTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            PaymentMethodTypeListQueryService listService = new PaymentMethodTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetPaymentMethodTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentMethodTypeListQueryService queryService = new PaymentMethodTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}