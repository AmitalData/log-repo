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

        public ProductNameTypePM GetSingleProductNameTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            productNameTypeQuery = new ProductNameTypeQueryService(customContext);
            ProductNameTypePM ProductNameType = productNameTypeQuery.GetSingle(code, false, false);
            return ProductNameType;
        }

        public ProductNameTypeList GetSingleProductNameTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProductNameType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ProductNameTypeListQueryService listService = new ProductNameTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ProductNameTypeList> GetProductNameTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProductNameType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProductNameTypeListQueryService listService = new ProductNameTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ProductNameTypeList> GetProductNameTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProductNameType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProductNameTypeListQueryService listService = new ProductNameTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetProductNameTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ProductNameType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProductNameTypeListQueryService queryService = new ProductNameTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}