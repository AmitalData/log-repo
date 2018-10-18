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

        public PaymentTypePM GetSinglePaymentTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            paymentTypeQuery = new PaymentTypeQueryService(customContext);
            PaymentTypePM PaymentType = paymentTypeQuery.GetSingle(code, false, false);
            return PaymentType;
        }

        public PaymentTypeList GetSinglePaymentTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.PaymentType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            PaymentTypeListQueryService listService = new PaymentTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<PaymentTypeList> GetPaymentTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.PaymentType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentTypeListQueryService listService = new PaymentTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<PaymentTypeList> GetPaymentTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.PaymentType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentTypeListQueryService listService = new PaymentTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetPaymentTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.PaymentType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentTypeListQueryService queryService = new PaymentTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


    }
}