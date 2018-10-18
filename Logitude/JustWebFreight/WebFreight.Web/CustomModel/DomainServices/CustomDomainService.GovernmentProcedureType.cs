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
using Logitude.Customs.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public GovernmentProcedureTypePM GetSingleGovernmentProcedureTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            governmentProcedureTypeQuery = new GovernmentProcedureTypeQueryService(customContext);
            GovernmentProcedureTypePM GovernmentProcedureType = governmentProcedureTypeQuery.GetSingle(id, false, false);
            return GovernmentProcedureType;
        }

        public GovernmentProcedureTypeList GetSingleGovernmentProcedureTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
       //     SecurityUtility.CheckContactFeature("Customs.GovernmentProcedureType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            GovernmentProcedureTypeListQueryService listService = new GovernmentProcedureTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<GovernmentProcedureTypeList> GetGovernmentProcedureTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.GovernmentProcedureType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            GovernmentProcedureTypeListQueryService listService = new GovernmentProcedureTypeListQueryService(customContext);
            List<GovernmentProcedureTypeList> list = listService.GetList(tenant).Where(d => !d.Code.StartsWith("1")).ToList();

         
            return list;
        }


        public List<GovernmentProcedureTypeList> GetGovernmentProcedureTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
       //     SecurityUtility.CheckContactFeature("Customs.GovernmentProcedureType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            GovernmentProcedureTypeListQueryService listService = new GovernmentProcedureTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
             List<GovernmentProcedureTypeList> list= listService.GetList(queryOperations, tenant);
      
             return list;
        }

        public int GetGovernmentProcedureTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.GovernmentProcedureType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            GovernmentProcedureTypeListQueryService queryService = new GovernmentProcedureTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


        public void UpdateGovernmentProcedureType(GovernmentProcedureTypePM currententityPm)
        {


            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            GovernmentProcedureTypeUpdateService service = new GovernmentProcedureTypeUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            service.Update(currententityPm, true);

        }


        public void UpdateGovernmentProcedureTypeList(GovernmentProcedureTypeList currententityPm)
        {


        }

    }
}