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
using Logitude.Customs.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;


namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class DeclarationDomainService
    {

        public DeclarationConstraintPM GetSingleDeclarationConstraintPM(string id,string consigmenttNumber, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            DeclarationConstraintQueryService declarationConstraintQuery = new DeclarationConstraintQueryService(customContext);
            DeclarationConstraintPM DeclarationConstraint = declarationConstraintQuery.GetSingle(id,consigmenttNumber, false, false);
            return DeclarationConstraint;
        }

        public DeclarationConstraintList GetSingleDeclarationConstraintList(string id, string consignmentNumber, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.DeclarationConstraint", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DeclarationConstraintListQueryService listService = new DeclarationConstraintListQueryService(customContext);
            return listService.GetSingle(id, consignmentNumber);
        }

        public List<DeclarationConstraintList> GetDeclarationConstraintLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.DeclarationConstraint", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DeclarationConstraintListQueryService listService = new DeclarationConstraintListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DeclarationConstraintList> GetDeclarationConstraintFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.DeclarationConstraint", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            DeclarationConstraintListQueryService listService = new DeclarationConstraintListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetDeclarationConstraintFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.DeclarationConstraint", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DeclarationConstraintListQueryService queryService = new DeclarationConstraintListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertDeclarationConstraint(DeclarationConstraintPM entityPM)
        {
            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPM.Tenant);
            }

            DeclarationConstraintUpdateService service = new DeclarationConstraintUpdateService(customContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            service.Update(entityPM, true);
        }

        public void UpdateDeclarationConstraint(DeclarationConstraintPM entityPM)
        {
            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPM.Tenant);
            }

            DeclarationConstraintUpdateService service = new DeclarationConstraintUpdateService(customContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            service.Update(entityPM, true);
        }

    }
}