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
        public PaymentMethodStatusPM GetSinglePaymentMethodStatusPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            paymentMethodStatusQuery = new PaymentMethodStatusQueryService(customContext);
            PaymentMethodStatusPM PaymentMethodStatus = paymentMethodStatusQuery.GetSingle(id, false, false);
            return PaymentMethodStatus;
        }

        public PaymentMethodStatusList GetSinglePaymentMethodStatusList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            PaymentMethodStatusListQueryService listService = new PaymentMethodStatusListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<PaymentMethodStatusList> GetPaymentMethodStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentMethodStatusListQueryService listService = new PaymentMethodStatusListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<PaymentMethodStatusList> GetPaymentMethodStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            PaymentMethodStatusListQueryService listService = new PaymentMethodStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetPaymentMethodStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentMethodStatusListQueryService queryService = new PaymentMethodStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}