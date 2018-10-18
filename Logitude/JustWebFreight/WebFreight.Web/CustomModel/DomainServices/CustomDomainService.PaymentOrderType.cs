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

        public PaymentOrderTypePM GetSinglePaymentOrderTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            paymentOrderTypeQueryService = new PaymentOrderTypeQueryService(customContext);
            PaymentOrderTypePM PaymentOrderType = paymentOrderTypeQueryService.GetSingle(id, false, false);
            return PaymentOrderType;
        }

        public PaymentOrderTypeList GetSinglePaymentOrderTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            PaymentOrderTypeListQueryService listService = new PaymentOrderTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<PaymentOrderTypeList> GetPaymentOrderTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentOrderTypeListQueryService listService = new PaymentOrderTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<PaymentOrderTypeList> GetPaymentOrderTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            PaymentOrderTypeListQueryService listService = new PaymentOrderTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetPaymentOrderTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentOrderTypeListQueryService queryService = new PaymentOrderTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }



    }
}