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

        public PaymentProcessPM GetSinglePaymentProcessPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            paymentProcessQueryService = new PaymentProcessQueryService(customContext);
            PaymentProcessPM PaymentProcess = paymentProcessQueryService.GetSingle(id, false, false);
            return PaymentProcess;
        }

        public PaymentProcessList GetSinglePaymentProcessList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            PaymentProcessListQueryService listService = new PaymentProcessListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<PaymentProcessList> GetPaymentProcessLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentProcessListQueryService listService = new PaymentProcessListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<PaymentProcessList> GetPaymentProcessFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            PaymentProcessListQueryService listService = new PaymentProcessListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetPaymentProcessFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentProcessListQueryService queryService = new PaymentProcessListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }



    }
}