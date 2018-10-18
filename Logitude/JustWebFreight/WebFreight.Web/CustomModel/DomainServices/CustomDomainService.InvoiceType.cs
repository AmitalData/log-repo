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
        public InvoiceTypePM GetSingleInvoiceTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            invoiceTypeQuery = new InvoiceTypeQueryService(customContext);
            InvoiceTypePM InvoiceType = invoiceTypeQuery.GetSingle(code, false, false);
            return InvoiceType;
        }

        public InvoiceTypeList GetSingleInvoiceTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.InvoiceType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            InvoiceTypeListQueryService listService = new InvoiceTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<InvoiceTypeList> GetInvoiceTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.InvoiceType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            InvoiceTypeListQueryService listService = new InvoiceTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<InvoiceTypeList> GetInvoiceTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.InvoiceType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            InvoiceTypeListQueryService listService = new InvoiceTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetInvoiceTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.InvoiceType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            InvoiceTypeListQueryService queryService = new InvoiceTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

      

   
    }
}