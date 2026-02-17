using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
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

        public AutonomyTypePM GetSingleAutonomyTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            autonomyTypeQuery = new AutonomyTypeQueryService(customContext);
            AutonomyTypePM AutonomyType = autonomyTypeQuery.GetSingle(id,false,false);
            return AutonomyType;
        }

        public AutonomyTypeList GetSingleAutonomyTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Customs.AutonomyType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            AutonomyTypeListQueryService listService = new AutonomyTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<AutonomyTypeList> GetAutonomyTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("Customs.AutonomyType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AutonomyTypeListQueryService listService = new AutonomyTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<AutonomyTypeList> GetAutonomyTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
       //     SecurityUtility.CheckContactFeature("Customs.AutonomyType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            AutonomyTypeListQueryService listService = new AutonomyTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetAutonomyTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
       //     SecurityUtility.CheckContactFeature("Customs.AutonomyType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AutonomyTypeListQueryService queryService = new AutonomyTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}