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

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateAutomationConditionList(AutomationList currentEntity)
        {
        }



        public IQueryable<AutomationPM> GetAutomationesByTenant(int tenant)
        {
            AutomationQuery automationQuery = new AutomationQuery(tenant);
            return automationQuery.GetAutomationPMsByTenant(tenant);
        }
        public AutomatedBackup GetAutomatedBackupById(int tenant)
        {

           
            return null;
        }

        public AutomationCondition GetAutomationConditionByASD(int tenant)
        {

           
            return null;
        } 
        

        public List<AutomationPM> GetAutomationesByObjectTableId( string objectTableId ,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Automation", "READ", tenant);
            AutomationQuery automationQuery = new AutomationQuery(tenant);

           List<AutomationPM> myResult  = automationQuery.GetAutomationPMsByObjectTableId(objectTableId, tenant);
     
            return myResult;
        }



        public AutomationPM GetSingleAutomation(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           SecurityUtility.CheckContactFeature("Automation", "READ", tenant);
            AutomationQuery automationQuery = new AutomationQuery(tenant);
            return automationQuery.GetSinglePM(id, tenant);
        }

        public AutomationPM GetAutomationById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Automation", "READ", tenant);
            AutomationQuery automationQuery = new AutomationQuery(tenant);
            return automationQuery.GetSinglePM(id, tenant);

        }

        public AutomationList GetSingleAutomationList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Automation", "READ", tenant);

            AutomationRepository automationRepository = new AutomationRepository(tenant);
            AutomationQuery automationQuery = new AutomationQuery(automationRepository);
            AutomationList AutomationList = null;
            Automation Automation = automationRepository.GetSingleAutomation(id, tenant);

            if (Automation != null)
            {
                List<Automation> singleEntityList = new List<Automation>();
                singleEntityList.Add(Automation);

                IQueryable<Automation> iQueryable = singleEntityList.AsQueryable();
                IQueryable<AutomationList> iQueryableEntityList = automationQuery.GetIQueryableEntityList(iQueryable);
                AutomationList = iQueryableEntityList.FirstOrDefault();
            }
            return AutomationList;
        }

        public void InsertAutomation(AutomationPM entityPM)
        {
          SecurityUtility.CheckContactFeature("Automation", "NEW", entityPM.Tenant);
          AutomationLastUpdateRepository automationLastUpdateRepository = new AutomationLastUpdateRepository(entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }
            AutomationService service = new AutomationService(objectContext, entityPM.Tenant);
            service.Create(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Automation");
        }

        public void UpdateAutomation(AutomationPM entityPM)
        {
            SecurityUtility.CheckContactFeature("Automation", "UPDATE", entityPM.Tenant);

  
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            //if (entityPM.AutomatedDataBackup != null)
            //{
            //    entityPM.AutomationXML = LogitudeXmlSerializer.SerializeObjectToXmlElementString(entityPM.AutomatedDataBackup);
            //}

            AutomationService service = new AutomationService(objectContext, entityPM.Tenant);
            service.Update(entityPM);
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Automation");

        }

      
        public void DeleteAutomation(AutomationPM Automation)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(Automation.Tenant);
            }
            AutomationRepository automationRepository = new AutomationRepository(objectContext);
            Automation entity = automationRepository.GetSingleAutomation(Automation.Id, Automation.Tenant);
            automationRepository.Remove(entity);
        }
    }
}