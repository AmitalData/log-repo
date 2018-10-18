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

        public ProductIdentificationTypePM GetSingleProductIdentificationTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            productIdentificationTypeQuery = new ProductIdentificationTypeQueryService(customContext);
            ProductIdentificationTypePM ProductIdentificationType = productIdentificationTypeQuery.GetSingle(code, false, false);
            return ProductIdentificationType;
        }

        public ProductIdentificationTypeList GetSingleProductIdentificationTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProductIdentificationType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ProductIdentificationTypeListQueryService listService = new ProductIdentificationTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ProductIdentificationTypeList> GetProductIdentificationTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProductIdentificationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProductIdentificationTypeListQueryService listService = new ProductIdentificationTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ProductIdentificationTypeList> GetProductIdentificationTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProductIdentificationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProductIdentificationTypeListQueryService listService = new ProductIdentificationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetProductIdentificationTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProductIdentificationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProductIdentificationTypeListQueryService queryService = new ProductIdentificationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}