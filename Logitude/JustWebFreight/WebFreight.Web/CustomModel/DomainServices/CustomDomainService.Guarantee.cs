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

        public GuaranteePM GetSingleGuaranteePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            guaranteeQuery = new GuaranteeQueryService(customContext);
            GuaranteePM Guarantee = guaranteeQuery.GetSingle(id, true, false);
            return Guarantee;
        }

        public GuaranteeList GetSingleGuaranteeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.Guarantee", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            GuaranteeListQueryService listService = new GuaranteeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public GuaranteePM GetGuaranteeByTapagId(string tapagId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            guaranteeQuery = new GuaranteeQueryService(customContext);
            GuaranteePM Guarantee = guaranteeQuery.GetGuaranteeByTapagId(tapagId, tenant);
            return Guarantee;
        }
      

        public List<GuaranteeList> GetGuaranteeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.Guarantee", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            GuaranteeListQueryService listService = new GuaranteeListQueryService(customContext);
            return listService.GetList(tenant);
            return new List<GuaranteeList>();
        }


        public List<GuaranteeList> GetGuaranteeFilters(byte[] xmlFilters, int tenant)
        {


            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.Guarantee", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            GuaranteeListQueryService listService = new GuaranteeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

     

        public int GetGuaranteeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.Guarantee", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            GuaranteeListQueryService queryService = new GuaranteeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertGuarantee(GuaranteePM entityPm)
        {
            SecurityUtility.CheckContactFeature("Customs.Guarantee", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            GuaranteeUpdateService service = new GuaranteeUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
           
            service.Update(entityPm, true);



        }

        public void UpdateGuarantee(GuaranteePM currententityPm)
        {
            SecurityUtility.CheckContactFeature("Customs.Guarantee", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            GuaranteeUpdateService service = new GuaranteeUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
      
            service.Update(currententityPm, true);

        }

   
        public void UpdateGuaranteeList(GuaranteeList list)
        {

        }
    }
}