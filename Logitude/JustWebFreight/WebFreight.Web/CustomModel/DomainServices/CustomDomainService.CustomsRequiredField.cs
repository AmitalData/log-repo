using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.Helpers;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public void DeleteCustomsRequiredFieldList(CustomsRequiredFieldList entity)
        {
 
        }
        public CustomsRequiredFieldPM GetSingleCustomsRequiredFieldPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsRequiredFieldQuery = new CustomsRequiredFieldQueryService(customContext);
            CustomsRequiredFieldPM CustomsRequiredField = customsRequiredFieldQuery.GetSingle(id, false, false);
            return CustomsRequiredField;
        }

        public CustomsRequiredFieldList GetSingleCustomsRequiredFieldList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsRequiredFieldListQueryService listService = new CustomsRequiredFieldListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomsRequiredFieldList> GetCustomsRequiredFieldLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsRequiredFieldListQueryService listService = new CustomsRequiredFieldListQueryService(customContext);
            return listService.GetList(tenant);
        }

        public List<CustomsRequiredFieldPM> GetCustomsRequiredFieldListsByObjectTable(string objectTableId,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            customsRequiredFieldQuery = new CustomsRequiredFieldQueryService(customContext);
            return customsRequiredFieldQuery.GetCustomRequiredFieldsByObjectTable(objectTableId, tenant);
        }



        public List<CustomsRequiredFieldList> GetCustomsRequiredFieldFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomsRequiredFieldListQueryService listService = new CustomsRequiredFieldListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsRequiredFieldFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsRequiredFieldListQueryService queryService = new CustomsRequiredFieldListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertCustomsRequiredField(CustomsRequiredFieldPM entityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.CustomsRequiredField", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            CustomsRequiredFieldUpdateService service = new CustomsRequiredFieldUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            service.Update(entityPm, true);
            TableLastUpdateClass.UpdateTableHistory(entityPm.Tenant, "Customs.CustomsRequiredField");

        }

        public void UpdateCustomsRequiredField(CustomsRequiredFieldPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.CustomsRequiredField", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            CustomsRequiredFieldUpdateService service = new CustomsRequiredFieldUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            service.Update(currententityPm, true);
            TableLastUpdateClass.UpdateTableHistory(currententityPm.Tenant, "Customs.CustomsRequiredField");

        }

        public void DeleteCustomsRequiredField(CustomsRequiredFieldPM entityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.CustomsRequiredField", "UPDATE", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            CustomsRequiredFieldUpdateService service = new CustomsRequiredFieldUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;

            service.Update(entityPm, true);
            TableLastUpdateClass.UpdateTableHistory(entityPm.Tenant, "Customs.CustomsRequiredField");

        }

        public void UpdateCustomsRequiredFieldList(CustomsRequiredFieldList list)
        {

        }




    }
}