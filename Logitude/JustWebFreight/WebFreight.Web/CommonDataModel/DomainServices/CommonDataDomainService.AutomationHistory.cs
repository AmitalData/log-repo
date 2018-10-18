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
using Logitude.Server.Tools;
using Simplog.Data.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {

        public List<AutomationHistoryPM> GetAutomationHistoryesByAutomationId(string automationId, int tenant)
        {
            
          AutomationHistoryQuery automationHistoryQuery = new AutomationHistoryQuery(tenant);
          List<AutomationHistoryPM>  myResult = automationHistoryQuery.GetAutomationHistoryPMsByAutomationId(automationId, tenant);

  
          return myResult;




        }

        public AutomationHistoryPM GetSingleAutomationHistory(string automationId, int version, int tenant)
        {
            // SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("AutomationHistory", "READ", tenant);
            AutomationHistoryQuery automationHistoryQuery = new AutomationHistoryQuery(tenant);
            return automationHistoryQuery.GetSinglePM(automationId, version, tenant);
        }


        public AutomationHistoryList GetSingleAutomationHistoryList(string automationId, int version, int tenant)
        {
            // SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("AutomationHistory", "READ", tenant);

            AutomationHistoryRepository automationHistoryRepository = new AutomationHistoryRepository(tenant);
            AutomationHistoryQuery AutomationHistoryQuery = new AutomationHistoryQuery(automationHistoryRepository);
            AutomationHistoryList AutomationHistoryList = null;
            AutomationHistory AutomationHistory = automationHistoryRepository.GetSingleAutomationHistory(version, automationId, tenant);

            if (AutomationHistory != null)
            {
                List<AutomationHistory> singleEntityList = new List<AutomationHistory>();
                singleEntityList.Add(AutomationHistory);

                IQueryable<AutomationHistory> iQueryable = singleEntityList.AsQueryable();
                IQueryable<AutomationHistoryList> iQueryableEntityList = AutomationHistoryQuery.GetIQueryableEntityList(iQueryable);
                AutomationHistoryList = iQueryableEntityList.FirstOrDefault();
            }
            return AutomationHistoryList;
        }

        public void InsertAutomationHistory(AutomationHistoryPM AutomationHistory)
        {
            // SecurityUtility.CheckContactFeature("AutomationHistory", "NEW", AutomationHistory.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(AutomationHistory.Tenant);
            }

            AutomationHistoryService service = new AutomationHistoryService(objectContext, AutomationHistory.Tenant);
            service.Create(AutomationHistory);

            TableLastUpdateClass.UpdateTableHistory(AutomationHistory.Tenant, "AutomationHistory");
        }

        public void UpdateAutomationHistory(AutomationHistoryPM currentAutomationHistory)
        {
            //  SecurityUtility.CheckContactFeature("AutomationHistory", "UPDATE", currentAutomationHistory.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentAutomationHistory.Tenant);
            }
            string entityName = "AutomationHistory" + currentAutomationHistory.Version + currentAutomationHistory.AutomationsId + currentAutomationHistory.Tenant;
            string entityPmName = "AutomationHistoryPM" + currentAutomationHistory.Version+ currentAutomationHistory.AutomationsId + currentAutomationHistory.Tenant;

            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            AutomationHistoryService service = new AutomationHistoryService(objectContext, currentAutomationHistory.Tenant);
            service.Update(currentAutomationHistory);
            TableLastUpdateClass.UpdateTableHistory(currentAutomationHistory.Tenant, "AutomationHistory");

        }

        public void DeleteAutomationHistory(AutomationHistoryPM AutomationHistory)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(AutomationHistory.Tenant);
            }
            AutomationHistoryRepository automationHistoryRepository = new AutomationHistoryRepository(objectContext);
            AutomationHistory entity = automationHistoryRepository.GetSingleAutomationHistory(AutomationHistory.Version, AutomationHistory.AutomationsId, AutomationHistory.Tenant);
            automationHistoryRepository.Remove(entity);
        }
    }
}