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
        public CheckQueueTypePM GetSingleCheckQueueTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
           CheckQueueTypeQuery = new CheckQueueTypeQueryService(customContext);
           CheckQueueTypePM CheckQueueType = CheckQueueTypeQuery.GetSingle(id, false, false);
            return CheckQueueType;
        }

        public CheckQueueTypeList GetSingleCheckQueueTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.CheckQueueType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CheckQueueTypeListQueryService listService = new CheckQueueTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CheckQueueTypeList> GetCheckQueueTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Customs.CheckQueueType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CheckQueueTypeListQueryService listService = new CheckQueueTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CheckQueueTypeList> GetCheckQueueTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Customs.CheckQueueType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CheckQueueTypeListQueryService listService = new CheckQueueTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCheckQueueTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.CheckQueueType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CheckQueueTypeListQueryService queryService = new CheckQueueTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}