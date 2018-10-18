using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public CustomsDocumentPointerPM GetSingleCustomsDocumentPointerPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsDocumentPointerQuery = new CustomsDocumentPointerQueryService(customContext);
            CustomsDocumentPointerPM CustomsDocumentPointer = customsDocumentPointerQuery.GetSingle(id, true, false);
            return CustomsDocumentPointer;
        }

        public CustomsDocumentPointerList GetSingleCustomsDocumentPointerList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.CustomsDocumentPointer", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentPointerListQueryService listService = new CustomsDocumentPointerListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomsDocumentPointerList> GetCustomsDocumentPointerLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentPointer", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentPointerListQueryService listService = new CustomsDocumentPointerListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsDocumentPointerList> GetCustomsDocumentPointerFilters(byte[] xmlFilters, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentPointer", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentPointerListQueryService listService = new CustomsDocumentPointerListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsDocumentPointerFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentPointer", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentPointerListQueryService queryService = new CustomsDocumentPointerListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        //public void InsertCustomsDocumentPointer(CustomsDocumentPointerPM entityPm)
        //{
        //    //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentPointer", "NEW", entityPm.Tenant);

        //    if (customContext == null)
        //    {
        //        customContext = CustomContext.GetContext(entityPm.Tenant);
        //    }

        //    CustomsDocumentPointerUpdateService service = new CustomsDocumentPointerUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
        //    entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

        //    service.Update(entityPm, true);

        //}

        //public void UpdateCustomsDocumentPointer(CustomsDocumentPointerPM currententityPm)
        //{
        //    //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentPointer", "UPDATE", currententityPm.Tenant);

        //    if (customContext == null)
        //    {
        //        customContext = CustomContext.GetContext(currententityPm.Tenant);
        //    }

        //    CustomsDocumentPointerUpdateService service = new CustomsDocumentPointerUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
        //    currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

        //    service.Update(currententityPm, true);

        //}
        
        //[Invoke]
        //public void DeleteCustomsDocumentPointer(CustomsDocumentPointerPM entityPm)
        //{
        //    //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentPointer", "UPDATE", entityPm.Tenant);

        //    if (customContext == null)
        //    {
        //        customContext = CustomContext.GetContext(entityPm.Tenant);
        //    }

        //    CustomsDocumentPointerUpdateService service = new CustomsDocumentPointerUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
        //    entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;

        //    service.Update(entityPm, true);

        //}

        

        public void UpdateCustomsDocumentPointerList(CustomsDocumentPointerList list)
        {

        }

        public List<CustomsDocumentPointerPM> GetCustomsDocumentPointersByEntityIdAndChilds(string entityId, string childEntityId1, string childEntityId2, string childEntityId3, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customsDocumentPointerQuery = new CustomsDocumentPointerQueryService(tenant);
            return customsDocumentPointerQuery.GetCustomsDocumentPointerPMsByEntityIdAndChilds(entityId, childEntityId1, childEntityId2, childEntityId3, tenant);
        }
    }
}