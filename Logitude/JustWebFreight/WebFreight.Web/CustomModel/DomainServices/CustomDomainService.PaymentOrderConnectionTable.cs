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
using Logitude.Customs.Data.Repsitories;
using System.ServiceModel.DomainServices.Server;
using Logitude.Customs.Data.EntityPOCOs;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public PaymentOrderConnectionTablePM GetSinglePaymentOrderConnectionTablePM(string paymentOrderId,string connectedEntityId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            paymentOrderConnectionTableQuery = new PaymentOrderConnectionTableQueryService(customContext);
            PaymentOrderConnectionTablePM PaymentOrderConnectionTable = paymentOrderConnectionTableQuery.GetSingle(paymentOrderId, connectedEntityId, false, false);
            return PaymentOrderConnectionTable;
        }

        public PaymentOrderConnectionTableList GetSinglePaymentOrderConnectionTableList(string paymentOrderId, string connectedEntityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            PaymentOrderConnectionTableListQueryService listService = new PaymentOrderConnectionTableListQueryService(customContext);
            return listService.GetSingle(paymentOrderId,connectedEntityId);
        }


        public List<string> GetAccountingCustomFileNumbers(string paymentOrderId, int tenant)
        {
            paymentOrderConnectionTableRepository = new PaymentOrderConnectionTableRepository(tenant);
            return paymentOrderConnectionTableRepository.GetPaymentOrderConnectedDeclarations(paymentOrderId, tenant);
        }
      

        public List<PaymentOrderConnectionTableList> GetPaymentOrderConnectionTableLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentOrderConnectionTableListQueryService listService = new PaymentOrderConnectionTableListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<PaymentOrderConnectionTableList> GetPaymentOrderConnectionTableFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            PaymentOrderConnectionTableListQueryService listService = new PaymentOrderConnectionTableListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetPaymentOrderConnectionTableFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentOrderConnectionTableListQueryService queryService = new PaymentOrderConnectionTableListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations,tenant);

        }


        [Invoke] 
        public string GetEntityIdForPaymentOrderConnectionTable(string paymentOrderId, int tenant)
        {
            paymentOrderConnectionTableRepository = new PaymentOrderConnectionTableRepository(tenant);
            PaymentOrderConnectionTable connection = paymentOrderConnectionTableRepository.GetPaymentOrderConnectedToDeclaration(paymentOrderId, tenant);
            return connection.ConnectedEntityId;


        }

      

    }
}