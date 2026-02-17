using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
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
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{

    public partial class CustomDomainService
    {
        public PhysicalCheckPM GetSinglePhysicalCheckPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            physicalCheckQuery = new PhysicalCheckQueryService(customContext);
            PhysicalCheckPM PhysicalCheck = physicalCheckQuery.GetSingle(id, false, false);
            return PhysicalCheck;
        }

        public PhysicalCheckList GetSinglePhysicalCheckList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.PhysicalCheck", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            PhysicalCheckListQueryService listService = new PhysicalCheckListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<PhysicalCheckList> GetPhysicalCheckLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.PhysicalCheck", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            PhysicalCheckListQueryService listService = new PhysicalCheckListQueryService(customContext);
            return listService.GetList(tenant);
        }

        public List<PhysicalCheckList> GetPhysicalCheckByDeclarationIdLists(string declarationId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.PhysicalCheck", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            PhysicalCheckQueryService queryService = new PhysicalCheckQueryService(customContext);
            return queryService.GetPhysicalChecksByDeclarationId(declarationId, tenant);
        }


        public List<PhysicalCheckList> GetPhysicalCheckFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.PhysicalCheck", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            PhysicalCheckListQueryService listService = new PhysicalCheckListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetPhysicalCheckFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.PhysicalCheck", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            PhysicalCheckListQueryService queryService = new PhysicalCheckListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertPhysicalCheck(PhysicalCheckPM entityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.PhysicalCheck", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            PhysicalCheckUpdateService service = new PhysicalCheckUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            service.Update(entityPm, true);

        }

        public void UpdatePhysicalCheck(PhysicalCheckPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.PhysicalCheck", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            PhysicalCheckUpdateService service = new PhysicalCheckUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            service.Update(currententityPm, true);

        }

        public void UpdatePhysicalCheckList(PhysicalCheckList list)
        {

        }


       
    }
}