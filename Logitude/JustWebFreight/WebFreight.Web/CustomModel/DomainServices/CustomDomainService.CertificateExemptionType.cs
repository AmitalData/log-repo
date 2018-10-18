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
        public CertificateExemptionTypePM GetSingleCertificateExemptionTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            certificateExemptionTypeQuery = new CertificateExemptionTypeQueryService(customContext);
            CertificateExemptionTypePM CertificateExemptionType = certificateExemptionTypeQuery.GetSingle(id, false, false);
            return CertificateExemptionType;
        }

        public CertificateExemptionTypeList GetSingleCertificateExemptionTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.CertificateExemptionType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CertificateExemptionTypeListQueryService listService = new CertificateExemptionTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CertificateExemptionTypeList> GetCertificateExemptionTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.CertificateExemptionType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CertificateExemptionTypeListQueryService listService = new CertificateExemptionTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CertificateExemptionTypeList> GetCertificateExemptionTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.CertificateExemptionType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CertificateExemptionTypeListQueryService listService = new CertificateExemptionTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCertificateExemptionTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.CertificateExemptionType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CertificateExemptionTypeListQueryService queryService = new CertificateExemptionTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}