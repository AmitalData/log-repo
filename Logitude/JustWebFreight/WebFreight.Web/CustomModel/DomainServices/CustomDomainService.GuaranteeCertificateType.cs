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
using System.ServiceModel.DomainServices.Server;
using Logitude.Server.Tools.Helpers;
namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public GuaranteeCertificateTypePM GetSingleGuaranteeCertificateTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            guaranteeCertificateTypeQuery = new GuaranteeCertificateTypeQueryService(customContext);
            GuaranteeCertificateTypePM GuaranteeCertificateType = guaranteeCertificateTypeQuery.GetSingle(id, false, false);
            return GuaranteeCertificateType;
        }

        public GuaranteeCertificateTypeList GetSingleGuaranteeCertificateTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.GuaranteeCertificateType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            GuaranteeCertificateTypeListQueryService listService = new GuaranteeCertificateTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<GuaranteeCertificateTypeList> GetGuaranteeCertificateTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.GuaranteeCertificateType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            GuaranteeCertificateTypeListQueryService listService = new GuaranteeCertificateTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<GuaranteeCertificateTypeList> GetGuaranteeCertificateTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.GuaranteeCertificateType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            GuaranteeCertificateTypeListQueryService listService = new GuaranteeCertificateTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetGuaranteeCertificateTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.GuaranteeCertificateType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            GuaranteeCertificateTypeListQueryService queryService = new GuaranteeCertificateTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}