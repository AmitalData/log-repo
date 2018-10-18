using System.Collections.Generic;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public PaymentProtestTypePM GetSinglePaymentProtestTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            paymentProtestTypeQuery = new PaymentProtestTypeQueryService(customContext);
            PaymentProtestTypePM PaymentProtestType = paymentProtestTypeQuery.GetSingle(id, false, false);
            return PaymentProtestType;
        }

        public PaymentProtestTypeList GetSinglePaymentProtestTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            PaymentProtestTypeListQueryService listService = new PaymentProtestTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<PaymentProtestTypeList> GetPaymentProtestTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentProtestTypeListQueryService listService = new PaymentProtestTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<PaymentProtestTypeList> GetPaymentProtestTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            PaymentProtestTypeListQueryService listService = new PaymentProtestTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetPaymentProtestTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentProtestTypeListQueryService queryService = new PaymentProtestTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}