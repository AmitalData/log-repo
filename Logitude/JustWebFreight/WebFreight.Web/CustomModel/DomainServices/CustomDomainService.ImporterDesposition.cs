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

        public ImporterDespositionPM GetSingleImporterDespositionPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            importerDespositionQuery = new ImporterDespositionQueryService(customContext);
            ImporterDespositionPM ImporterDesposition = importerDespositionQuery.GetSingle(id, true, false);
            return ImporterDesposition;
        }

        public ImporterDespositionList GetSingleImporterDespositionList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ImporterDesposition", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ImporterDespositionListQueryService listService = new ImporterDespositionListQueryService(customContext);
            return listService.GetSingle(id);
        }

   
        public List<ImporterDespositionList> GetImporterDespositionLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ImporterDesposition", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ImporterDespositionListQueryService listService = new ImporterDespositionListQueryService(customContext);
            return listService.GetList(tenant);
        
        }


        public List<ImporterDespositionList> GetImporterDespositionFilters(byte[] xmlFilters, int tenant)
        {


            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ImporterDesposition", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            ImporterDespositionListQueryService listService = new ImporterDespositionListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

     

        public int GetImporterDespositionFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ImporterDesposition", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ImporterDespositionListQueryService queryService = new ImporterDespositionListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertImporterDesposition(ImporterDespositionPM entityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.ImporterDesposition", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            ImporterDespositionUpdateService service = new ImporterDespositionUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
           
            service.Update(entityPm, true);



        }

        public void UpdateImporterDesposition(ImporterDespositionPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.ImporterDesposition", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            ImporterDespositionUpdateService service = new ImporterDespositionUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            service.Update(currententityPm, true);

        }


     

    }
}