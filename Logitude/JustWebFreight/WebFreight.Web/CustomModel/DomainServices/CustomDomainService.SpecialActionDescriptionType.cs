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

        public SpecialActionDescriptionTypePM GetSingleSpecialActionDescriptionTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            specialActionDescriptionTypeQuery = new SpecialActionDescriptionTypeQueryService(customContext);
            SpecialActionDescriptionTypePM SpecialActionDescriptionType = specialActionDescriptionTypeQuery.GetSingle(id, false, false);
            return SpecialActionDescriptionType;
        }

        public SpecialActionDescriptionTypeList GetSingleSpecialActionDescriptionTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.SpecialActionDescriptionType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            SpecialActionDescriptionTypeListQueryService listService = new SpecialActionDescriptionTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<SpecialActionDescriptionTypeList> GetSpecialActionDescriptionTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.SpecialActionDescriptionType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            SpecialActionDescriptionTypeListQueryService listService = new SpecialActionDescriptionTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<SpecialActionDescriptionTypeList> GetSpecialActionDescriptionTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.SpecialActionDescriptionType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            SpecialActionDescriptionTypeListQueryService listService = new SpecialActionDescriptionTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetSpecialActionDescriptionTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.SpecialActionDescriptionType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            SpecialActionDescriptionTypeListQueryService queryService = new SpecialActionDescriptionTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}