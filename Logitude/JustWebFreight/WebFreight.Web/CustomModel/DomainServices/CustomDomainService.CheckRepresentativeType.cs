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
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {


        public CheckRepresentativeTypePM GetSingleCheckRepresentativeTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
           checkRepresentativeTypeQuery = new CheckRepresentativeTypeQueryService(customContext);
           CheckRepresentativeTypePM CheckRepresentativeType = checkRepresentativeTypeQuery.GetSingle(id, false, false);
            return CheckRepresentativeType;
        }

        public CheckRepresentativeTypeList GetSingleCheckRepresentativeTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Customs.CheckRepresentativeType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
           CheckRepresentativeTypeListQueryService listService = new CheckRepresentativeTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CheckRepresentativeTypeList> GetCheckRepresentativeTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Customs.CheckRepresentativeType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
           CheckRepresentativeTypeListQueryService listService = new CheckRepresentativeTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CheckRepresentativeTypeList> GetCheckRepresentativeTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.CheckRepresentativeType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
           CheckRepresentativeTypeListQueryService listService = new CheckRepresentativeTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCheckRepresentativeTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.CheckRepresentativeType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
           CheckRepresentativeTypeListQueryService queryService = new CheckRepresentativeTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}