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
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Simplog.Server.Infrastructure;
using System.ServiceModel.DomainServices.Server;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public RequiredGuaranteeTypePM GetSingleRequiredGuaranteeTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            requiredGuaranteeTypeQuery = new RequiredGuaranteeTypeQueryService(customContext);
            RequiredGuaranteeTypePM RequiredGuaranteeType = requiredGuaranteeTypeQuery.GetSingle(id, true, false);
            return RequiredGuaranteeType;
        }

        public RequiredGuaranteeTypeList GetSingleRequiredGuaranteeTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.RequiredGuaranteeType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            RequiredGuaranteeTypeListQueryService listService = new RequiredGuaranteeTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        //public List<RequiredGuaranteeTypeList> GetRequiredGuaranteeTypesForGuarantee(string guaranteeId, int tenant)
        //{
        //    customContext = CustomContext.GetContext(tenant);
        //    requiredGuaranteeTypeQuery = new RequiredGuaranteeTypeQueryService(customContext);
        //    List<RequiredGuaranteeTypeList> RequiredGuaranteeTypes = requiredGuaranteeTypeQuery.GetRequiredGuaranteeTypesForGuarantee(guaranteeId, tenant);
        //    return RequiredGuaranteeTypes;
        //}

        public List<RequiredGuaranteeTypeList> GetRequiredGuaranteeTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.RequiredGuaranteeType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            RequiredGuaranteeTypeListQueryService listService = new RequiredGuaranteeTypeListQueryService(customContext);
            return listService.GetList(tenant);
            return new List<RequiredGuaranteeTypeList>();
        }


        public List<RequiredGuaranteeTypeList> GetRequiredGuaranteeTypeFilters(byte[] xmlFilters, int tenant)
        {


            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.RequiredGuaranteeType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            RequiredGuaranteeTypeListQueryService listService = new RequiredGuaranteeTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }



        public int GetRequiredGuaranteeTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.RequiredGuaranteeType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            RequiredGuaranteeTypeListQueryService queryService = new RequiredGuaranteeTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertRequiredGuaranteeType(RequiredGuaranteeTypePM entityPm)
        {
            //   SecurityUtility.CheckContactFeature("Customs.RequiredGuaranteeType", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            RequiredGuaranteeTypeUpdateService service = new RequiredGuaranteeTypeUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            service.Update(entityPm, true);



        }

        public void UpdateRequiredGuaranteeType(RequiredGuaranteeTypePM currententityPm)
        {
            // SecurityUtility.CheckContactFeature("Customs.RequiredGuaranteeType", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            RequiredGuaranteeTypeUpdateService service = new RequiredGuaranteeTypeUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            service.Update(currententityPm, true);

        }


       
    }
}