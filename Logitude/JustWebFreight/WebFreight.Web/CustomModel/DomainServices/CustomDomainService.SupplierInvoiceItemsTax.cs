using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public SupplierInvoiceItemsTaxPM GetSingleSupplierInvoiceItemsTaxPM(string declarationId, int InvoiceCounterKey , int LineNumber , string TaxTypeCode, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            supplierInvoiceItemsTaxQuery = new SupplierInvoiceItemsTaxQueryService(customContext);
            SupplierInvoiceItemsTaxPM SupplierInvoiceItemsTax = supplierInvoiceItemsTaxQuery.GetSingle(declarationId,InvoiceCounterKey,LineNumber,TaxTypeCode, false, false);
            return SupplierInvoiceItemsTax;
        }

        public SupplierInvoiceItemsTaxList GetSingleSupplierInvoiceItemsTaxList(string declarationId, int InvoiceCounterKey, int LineNumber, string TaxTypeCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
   

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            SupplierInvoiceItemsTaxListQueryService listService = new SupplierInvoiceItemsTaxListQueryService(customContext);
            return listService.GetSingle(declarationId, InvoiceCounterKey, LineNumber, TaxTypeCode);
        }

        public List<SupplierInvoiceItemsTaxList> GetSupplierInvoiceItemsTaxLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Customs.SupplierInvoiceItemsTax", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            SupplierInvoiceItemsTaxListQueryService listService = new SupplierInvoiceItemsTaxListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<SupplierInvoiceItemsTaxList> GetSupplierInvoiceItemsTaxsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.SupplierInvoiceItemsTax", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            SupplierInvoiceItemsTaxListQueryService listService = new SupplierInvoiceItemsTaxListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetSupplierInvoiceItemsTaxFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Customs.SupplierInvoiceItemsTax", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            SupplierInvoiceItemsTaxListQueryService queryService = new SupplierInvoiceItemsTaxListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertSupplierInvoiceItemsTax(SupplierInvoiceItemsTaxPM entityPm)
        {
         //   SecurityUtility.CheckContactFeature("Customs.SupplierInvoiceItemsTax", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            SupplierInvoiceItemsTaxUpdateService service = new SupplierInvoiceItemsTaxUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            service.Update(entityPm, true);

        }

        public void UpdateSupplierInvoiceItemsTax(SupplierInvoiceItemsTaxPM currententityPm)
        {
         //   SecurityUtility.CheckContactFeature("Customs.SupplierInvoiceItemsTax", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            SupplierInvoiceItemsTaxUpdateService service = new SupplierInvoiceItemsTaxUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            service.Update(currententityPm, true);

        }
    }
}