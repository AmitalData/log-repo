using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using System.Collections.Generic;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public PaymentOrderStatusPM GetSinglePaymentOrderStatusPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            paymentOrderStatusQueryService = new PaymentOrderStatusQueryService(customContext);
            PaymentOrderStatusPM PaymentOrderStatus = paymentOrderStatusQueryService.GetSingle(id, false, false);
            return PaymentOrderStatus;
        }

        public PaymentOrderStatusList GetSinglePaymentOrderStatusList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            PaymentOrderStatusListQueryService listService = new PaymentOrderStatusListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<PaymentOrderStatusList> GetPaymentOrderStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentOrderStatusListQueryService listService = new PaymentOrderStatusListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<PaymentOrderStatusList> GetPaymentOrderStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            PaymentOrderStatusListQueryService listService = new PaymentOrderStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetPaymentOrderStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentOrderStatusListQueryService queryService = new PaymentOrderStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}