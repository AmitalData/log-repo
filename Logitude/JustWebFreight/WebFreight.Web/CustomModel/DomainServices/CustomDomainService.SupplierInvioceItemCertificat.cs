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

        public SupplierInvioceItemCertificatPM GetSingleSupplierInvioceItemCertificatPM(string declarationId, int InvoiceCounterKey, int LineNumber, int itemcertificatecounterkey, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            supplierInvioceItemCertificatQuery = new SupplierInvioceItemCertificatQueryService(customContext);
            SupplierInvioceItemCertificatPM SupplierInvioceItemCertificat = supplierInvioceItemCertificatQuery.GetSingle(declarationId, InvoiceCounterKey, LineNumber, itemcertificatecounterkey, false, false);
            return SupplierInvioceItemCertificat;
        }

        public SupplierInvioceItemCertificatList GetSingleSupplierInvioceItemCertificatList(string declarationId, int InvoiceCounterKey, int LineNumber, int itemcertificatecounterkey, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);


            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            SupplierInvioceItemCertificatListQueryService listService = new SupplierInvioceItemCertificatListQueryService(customContext);
            return listService.GetSingle(declarationId, InvoiceCounterKey, LineNumber, itemcertificatecounterkey);
        }

        public List<SupplierInvioceItemCertificatList> GetSupplierInvioceItemCertificatLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.SupplierInvioceItemCertificat", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            SupplierInvioceItemCertificatListQueryService listService = new SupplierInvioceItemCertificatListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<SupplierInvioceItemCertificatList> GetSupplierInvioceItemCertificatsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.SupplierInvioceItemCertificat", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            SupplierInvioceItemCertificatListQueryService listService = new SupplierInvioceItemCertificatListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetSupplierInvioceItemCertificatFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.SupplierInvioceItemCertificat", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            SupplierInvioceItemCertificatListQueryService queryService = new SupplierInvioceItemCertificatListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        //public void InsertSupplierInvioceItemCertificat(SupplierInvioceItemCertificatPM entityPm)
        //{
        //    //   SecurityUtility.CheckContactFeature("Customs.SupplierInvioceItemCertificat", "NEW", entityPm.Tenant);

        //    if (customContext == null)
        //    {
        //        customContext = CustomContext.GetContext(entityPm.Tenant);
        //    }

        //    SupplierInvioceItemCertificatUpdateService service = new SupplierInvioceItemCertificatUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
        //    entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
        //    service.Update(entityPm, true);

        //}

        //public void UpdateSupplierInvioceItemCertificat(SupplierInvioceItemCertificatPM currententityPm)
        //{
        //    //   SecurityUtility.CheckContactFeature("Customs.SupplierInvioceItemCertificat", "UPDATE", currententityPm.Tenant);

        //    if (customContext == null)
        //    {
        //        customContext = CustomContext.GetContext(currententityPm.Tenant);
        //    }

        //    SupplierInvioceItemCertificatUpdateService service = new SupplierInvioceItemCertificatUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
        //    currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

        //    service.Update(currententityPm, true);

        //}

        public List<SupplierInvioceItemCertificatPM> GetSupplierInvioceItemCertificates(string declarationId, int InvoiceCounterKey, int LineNumber, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            supplierInvioceItemCertificatQuery = new SupplierInvioceItemCertificatQueryService(customContext);

            List<SupplierInvioceItemCertificatPM> certificates = supplierInvioceItemCertificatQuery.GetSupplierInvioceItemCertificatesForSupplierInvoice(declarationId, InvoiceCounterKey, tenant);
            return certificates.Where(d => d.LineNumber == LineNumber).ToList(); ;
        }
    }
}