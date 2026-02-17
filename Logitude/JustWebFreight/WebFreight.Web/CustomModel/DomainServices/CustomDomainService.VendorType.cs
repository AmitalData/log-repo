using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public VendorTypePM GetSingleVendorTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            vendorTypeQuery = new VendorTypeQueryService(customContext);
            VendorTypePM VendorType = vendorTypeQuery.GetSingle(code, false, false);
            return VendorType;
        }

        public VendorTypeList GetSingleVendorTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.VendorType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            VendorTypeListQueryService listService = new VendorTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<VendorTypeList> GetVendorTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
       //     SecurityUtility.CheckContactFeature("Customs.VendorType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VendorTypeListQueryService listService = new VendorTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<VendorTypeList> GetVendorTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.VendorType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VendorTypeListQueryService listService = new VendorTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetVendorTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.VendorType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            VendorTypeListQueryService queryService = new VendorTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
      
    }
}