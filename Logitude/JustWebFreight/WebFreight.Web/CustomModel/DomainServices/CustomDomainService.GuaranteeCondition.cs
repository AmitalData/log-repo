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

        public GuaranteeConditionPM GetSingleGuaranteeConditionPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            guaranteeConditionQuery = new GuaranteeConditionQueryService(customContext);
            GuaranteeConditionPM GuaranteeCondition = guaranteeConditionQuery.GetSingle(id, true, false);
            return GuaranteeCondition;
        }

        public GuaranteeConditionList GetSingleGuaranteeConditionList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.GuaranteeCondition", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            GuaranteeConditionListQueryService listService = new GuaranteeConditionListQueryService(customContext);
            return listService.GetSingle(id);
        }

      

        public List<GuaranteeConditionList> GetGuaranteeConditionLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("Customs.GuaranteeCondition", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            GuaranteeConditionListQueryService listService = new GuaranteeConditionListQueryService(customContext);
            return listService.GetList(tenant);
            return new List<GuaranteeConditionList>();
        }


        public List<GuaranteeConditionList> GetGuaranteeConditionFilters(byte[] xmlFilters, int tenant)
        {


            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.GuaranteeCondition", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            GuaranteeConditionListQueryService listService = new GuaranteeConditionListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }



        public int GetGuaranteeConditionFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.GuaranteeCondition", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            GuaranteeConditionListQueryService queryService = new GuaranteeConditionListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertGuaranteeCondition(GuaranteeConditionPM entityPm)
        {
         //   SecurityUtility.CheckContactFeature("Customs.GuaranteeCondition", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            GuaranteeConditionUpdateService service = new GuaranteeConditionUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            service.Update(entityPm, true);



        }

        public void UpdateGuaranteeCondition(GuaranteeConditionPM currententityPm)
        {
           // SecurityUtility.CheckContactFeature("Customs.GuaranteeCondition", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            GuaranteeConditionUpdateService service = new GuaranteeConditionUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            service.Update(currententityPm, true);

        }


        public void UpdateGuaranteeConditionList(GuaranteeConditionList list)
        {

        }

    }
}