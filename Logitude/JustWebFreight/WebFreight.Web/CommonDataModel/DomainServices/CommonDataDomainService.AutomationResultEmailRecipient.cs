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

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        //public void UpdateAutomationResultEmailRecipientList(AutomationResultEmailRecipientList currentEntity)
        //{
        //}



        public AutomationResultEmailRecipientPM GetSingleAutomationResultEmailRecipient(string id, int tenant)
        {
             SecurityUtility.AuthenticationOnTenant(tenant);
             SecurityUtility.CheckContactFeature("AutomationResultEmailRecipient", "READ", tenant);
            AutomationResultEmailRecipientQuery automationResultEmailRecipientQuery = new AutomationResultEmailRecipientQuery(tenant);
            return automationResultEmailRecipientQuery.GetSinglePM(id, tenant);
        }

        public AutomationResultEmailRecipientPM GetAutomationResultEmailRecipientById(string id, int tenant)
        {
             SecurityUtility.AuthenticationOnTenant(tenant);
             SecurityUtility.CheckContactFeature("AutomationResultEmailRecipient", "READ", tenant);
            AutomationResultEmailRecipientQuery automationResultEmailRecipientQuery = new AutomationResultEmailRecipientQuery(tenant);
            return automationResultEmailRecipientQuery.GetSinglePM(id, tenant);

        }



    public List<AutomationResultEmailRecipientPM> GetAutomationResultEmailRecipientByAutomationId(string automationId, int tenant)
        {

             SecurityUtility.AuthenticationOnTenant(tenant);
             SecurityUtility.CheckContactFeature("AutomationResultEmailRecipient", "READ", tenant);
            AutomationResultEmailRecipientQuery automationResultEmailRecipientQuery = new AutomationResultEmailRecipientQuery(tenant);
            return automationResultEmailRecipientQuery.GetAutomationResultEmailRecipientPMsByAutomationId(automationId, tenant);

        }

    


        public void InsertAutomationResultEmailRecipient(AutomationResultEmailRecipientPM AutomationResultEmailRecipient)
        {
            // SecurityUtility.CheckContactFeature("AutomationResultEmailRecipient", "NEW", AutomationResultEmailRecipient.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(AutomationResultEmailRecipient.Tenant);
            }
            AutomationResultEmailRecipientService service = new AutomationResultEmailRecipientService(objectContext, AutomationResultEmailRecipient.Tenant);
            service.Create(AutomationResultEmailRecipient);

            TableLastUpdateClass.UpdateTableHistory(AutomationResultEmailRecipient.Tenant, "AutomationResultEmailRecipient");
        }

        public void UpdateAutomationResultEmailRecipient(AutomationResultEmailRecipientPM currentAutomationResultEmailRecipient)
        {
              SecurityUtility.CheckContactFeature("AutomationResultEmailRecipient", "UPDATE", currentAutomationResultEmailRecipient.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentAutomationResultEmailRecipient.Tenant);
            }
            string entityName = "AutomationResultEmailRecipient" + currentAutomationResultEmailRecipient.Id + currentAutomationResultEmailRecipient.Tenant;
            string entityPmName = "AutomationResultEmailRecipientPM" + currentAutomationResultEmailRecipient.Id + currentAutomationResultEmailRecipient.Tenant;

            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            AutomationResultEmailRecipientService service = new AutomationResultEmailRecipientService(objectContext, currentAutomationResultEmailRecipient.Tenant);
            service.Update(currentAutomationResultEmailRecipient);
            TableLastUpdateClass.UpdateTableHistory(currentAutomationResultEmailRecipient.Tenant, "AutomationResultEmailRecipient");

        }

        public void DeleteAutomationResultEmailRecipient(AutomationResultEmailRecipientPM AutomationResultEmailRecipient)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(AutomationResultEmailRecipient.Tenant);
            }
            AutomationResultEmailRecipientRepository automationResultEmailRecipientRepository = new AutomationResultEmailRecipientRepository(objectContext);
            AutomationResultEmailRecipient entity = automationResultEmailRecipientRepository.GetSingleAutomationResultEmailRecipient(AutomationResultEmailRecipient.Id, AutomationResultEmailRecipient.Tenant);
            automationResultEmailRecipientRepository.Remove(entity);
        }
    }
}