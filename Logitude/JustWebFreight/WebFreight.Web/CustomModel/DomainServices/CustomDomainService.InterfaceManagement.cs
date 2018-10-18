using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public void UpdateInterfaceManagementList(InterfaceManagementList InterfaceManagement)
        {

        }


        public InterfaceManagementPM GetSingleInterfaceManagementPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            interfaceManagementQuery = new InterfaceManagementQueryService(customContext);
            InterfaceManagementPM InterfaceManagement = interfaceManagementQuery.GetSingleInterfaceManagementwithDefinition(code, tenant);
            if (InterfaceManagement != null)
            {
                InterfaceManagement.Tenant = tenant;
            }
            return InterfaceManagement;
        }

        public InterfaceManagementList GetSingleInterfaceManagementList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.InterfaceManagement", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            InterfaceManagementListQueryService listService = new InterfaceManagementListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<InterfaceManagementList> GetInterfaceManagementLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           //SecurityUtility.CheckContactFeature("Customs.InterfaceManagement", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            InterfaceManagementListQueryService listService = new InterfaceManagementListQueryService(customContext);
            return listService.GetList(tenant);
        }

        public List<InterfaceManagementList> GetInterfaceManagementWithDefintion( int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
         //SecurityUtility.CheckContactFeature("Customs.InterfaceManagement", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);

            interfaceManagementQuery = new InterfaceManagementQueryService(customContext);
            List<InterfaceManagementList> interfaceManagments = interfaceManagementQuery.GetInterfaceManagementwithDefinition(tenant);
            return interfaceManagments;


        }

        public List<InterfaceManagementList> GetInterfaceManagementFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.InterfaceManagement", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            InterfaceManagementListQueryService listService = new InterfaceManagementListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetInterfaceManagementFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.InterfaceManagement", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            InterfaceManagementListQueryService queryService = new InterfaceManagementListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


        public void UpdateInterfaceManagement(InterfaceManagementPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.InterfaceManagement", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            InterfaceManagementUpdateService service = new InterfaceManagementUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);

            service.Update(currententityPm, true);
          
           

        }


        public void InsertInterfaceManagement(InterfaceManagementPM entityPm)
        {
         //   SecurityUtility.CheckContactFeature("Customs.InterfaceManagement", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }


            InterfaceManagementUpdateService service = new InterfaceManagementUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
           
            service.Update(entityPm, true);

        }

    }
}