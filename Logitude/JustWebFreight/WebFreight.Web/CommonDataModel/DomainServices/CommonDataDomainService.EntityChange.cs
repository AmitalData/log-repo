using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using System.Transactions;
using WebFreight.Web.TopicQueues;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Simplog.Data.Helpers;
using Logitude.Server.Tools;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public EntityChangeAutomation GetEntityChangesAutomation(int tenant)
        {
            return null;
        }
        
        public ChangeField GetChangeField(int tenant)
        {
            return null;
        }

        public EntityChangeAutomationsSummary GetEntityChangeAutomationsSummaryByEntityChangeId(string entitychangeId, string objectTableName, int tenant)
        {
            EntityChangeAutomationHelper entityChangeAutomationHelper = new EntityChangeAutomationHelper();
            EntityChangeAutomationsSummary entityChangeAutomationsSummary = entityChangeAutomationHelper.GetEntityChangeAutomationsSummary(entitychangeId, objectTableName, tenant);

            return entityChangeAutomationsSummary;
        }

       

        //public EntityChangeAutomationsSummary GetAutomationsByEntityChangeId(string entitychangeId, string objectTableName, int tenant)
        //{
        //    EntityChangeQuery EntityChangeQuery = new EntityChangeQuery(tenant);
        //    EntityChangePM entityChangePM = EntityChangeQuery.GetSinglePM(entitychangeId, tenant);

        //    List<ObjectField> objectFieldLists = ObjectFieldRepository.GetObjectFieldsByObjectTableName(objectTableName, tenant);
        //    CustomFieldResolver customFieldResolver = new CustomFieldResolver();
        //    EntityChangeAutomationsSummary entityChangeAutomationsSummary = new EntityChangeAutomationsSummary();

        //    List<EntityChangeAutomation> entityChangeAutomation = new List<EntityChangeAutomation>();
        //    EntityChangeAutomationHelper entityChangeAutomationHelper = new EntityChangeAutomationHelper();
        //    if (entityChangePM != null)
        //    {
        //        List<EntityChangeAutomation> list = entityChangeAutomationHelper.CreateEntityChangeAutomation(entityChangePM, "Email");
        //        foreach (EntityChangeAutomation item in list)
        //        {
        //            entityChangeAutomation.Add(item);
        //        }

        //        entityChangeAutomation = entityChangeAutomation.OrderByDescending(d => d.CreateDate).ToList();
        //        entityChangeAutomationsSummary.Id = entityChangePM.Id;
        //    }

        //    entityChangeAutomationsSummary.EntityChangeAutomationList = entityChangeAutomation;

        //    return entityChangeAutomationsSummary;
        //}

        
        
        public List<EntityChangePM> GetEntityChangeListByEntityIdAndObjectTable(string entityId, string objectTableId, int tenant)
        {
            EntityChangeQuery entityChangeQuery = new EntityChangeQuery(tenant);
            List<EntityChangePM> entityChangePMLists = entityChangeQuery.GetEntityChangePMsByEntityIdAndObjectTable(entityId, objectTableId, tenant);
            return entityChangePMLists;
        }
        
        public EntityChangePM GetSingleEntityChange(string Id, int tenant)
        {
            // SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("EntityChange", "READ", tenant);
            EntityChangeQuery EntityChangeQuery = new EntityChangeQuery(tenant);
            return EntityChangeQuery.GetSinglePM(Id, tenant);
        }
        
        public void InsertEntityChange(EntityChangePM EntityChange)
        {
            // SecurityUtility.CheckContactFeature("EntityChange", "NEW", EntityChange.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(EntityChange.Tenant);
            }
            EntityChangeService service = new EntityChangeService(objectContext, EntityChange.Tenant);
            service.Create(EntityChange);

            TableLastUpdateClass.UpdateTableHistory(EntityChange.Tenant, "EntityChange");
        }

        public void UpdateEntityChange(EntityChangePM currentEntityChange)
        {
            //  SecurityUtility.CheckContactFeature("EntityChange", "UPDATE", currentEntityChange.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntityChange.Tenant);
            }
       
     
            EntityChangeService service = new EntityChangeService(objectContext, currentEntityChange.Tenant);
            service.Update(currentEntityChange);

            TableLastUpdateClass.UpdateTableHistory(currentEntityChange.Tenant, "EntityChange");

        }

        public void DeleteEntityChange(EntityChangePM EntityChange)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(EntityChange.Tenant);
            }
            EntityChangeRepository EntityChangeRepository = new EntityChangeRepository(objectContext);
            EntityChange entity = EntityChangeRepository.GetSingleEntityChange(EntityChange.Id, EntityChange.Tenant);
            EntityChangeRepository.Remove(entity);
        }
    }
}