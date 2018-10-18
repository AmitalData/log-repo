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
using Logitude.Customs.Data.EntityKeys;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public  partial class DeclarationDomainService
    {
        public DeclarationTaxPM GetSingleDeclarationTaxPM(string declarationId, string taxCode, int tenant )
        {
            customContext = CustomContext.GetContext(tenant);
            DeclarationTaxQueryService declarationTaxQuery = new DeclarationTaxQueryService(customContext);
            DeclarationTaxPM DeclarationTax = declarationTaxQuery.GetSingle(declarationId, taxCode, false, false);
            return DeclarationTax;
        }

        public DeclarationTaxList GetSingleDeclarationTaxList(string declarationId, string taxCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
      //      SecurityUtility.CheckContactFeature("Customs.DeclarationTax", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DeclarationTaxListQueryService listService = new DeclarationTaxListQueryService(customContext);
            return listService.GetSingle(declarationId, taxCode);
        }

        public List<DeclarationTaxList> GetDeclarationTaxLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.DeclarationTax", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DeclarationTaxListQueryService listService = new DeclarationTaxListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DeclarationTaxList> GetDeclarationTaxFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.DeclarationTax", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            DeclarationTaxListQueryService listService = new DeclarationTaxListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetDeclarationTaxFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.DeclarationTax", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DeclarationTaxListQueryService queryService = new DeclarationTaxListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertDeclarationTax(DeclarationTaxPM entityPm)
        {
          //  SecurityUtility.CheckContactFeature("Customs.DeclarationTax", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            DeclarationTaxUpdateService service = new DeclarationTaxUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            service.Update(entityPm, true);

        }

        public void UpdateDeclarationTax(DeclarationTaxPM currententityPm)
        {
         //   SecurityUtility.CheckContactFeature("Customs.DeclarationTax", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            DeclarationTaxUpdateService service = new DeclarationTaxUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            service.Update(currententityPm, true);

        }

        public void UpdateDeclarationTaxList(DeclarationTaxList list)
        {

        }

        public List<DeclarationTaxPM> GetDeclarationTaxesByDeclarationId(string declarationId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            DeclarationTaxQueryService declarationTaxQuery = new DeclarationTaxQueryService(customContext);
            
            List<DeclarationTaxPM> DeclarationTaxes = declarationTaxQuery.GetDeclarationTaxesForDeclarationId(declarationId,tenant);
            return DeclarationTaxes;
        }

        public List<SupplierInvoiceItemsTaxPM> GetItemsTaxesForDeclaration(string declarationId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            SupplierInvoiceItemsTaxQueryService supplierInvoiceItemsTaxQuery = new SupplierInvoiceItemsTaxQueryService(customContext);

            List<SupplierInvoiceItemsTaxPM> taxes = supplierInvoiceItemsTaxQuery.GetDeclarationTaxesForDeclarationId(declarationId, tenant);
            return taxes;
        }

        //public List<SupplierInvoiceItemsTaxesModPM> GetItemTaxesModifications(string declarationId, int tenant)
        //{
        //     customContext = CustomContext.GetContext(tenant);
        //     SupplierInvoiceItemsTaxesModQueryService queryService = new SupplierInvoiceItemsTaxesModQueryService(customContext);
        //     List<SupplierInvoiceItemsTaxesModPM> modifications = queryService.GetSupplierInvoiceItemTaxModificationsForDeclaration(declarationId, tenant);
        //     return modifications;
        //}

    }
}