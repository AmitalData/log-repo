using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityLists;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class AutomationService
    {
        bool isNewEntity;
        private int tenant;
        public Automation Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private AutomationPM entityPm;
        private ICommonDataContext objectContext;
        private AutomationRepository entityRepository;
        private ContactRepository contactRepository;
        private Contact loggedContact;
        public AutomationService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new AutomationRepository(objectContext);

            this.contactRepository = new ContactRepository(objectContext);
            this.GetLoggedContact();

        }

        public void Create(AutomationPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("Automation", tenant).ToString();
            this.entityPm.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPm.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            if (this.loggedContact != null)
            {
                this.entityPm.UpdatedByUserId = this.loggedContact.Id;
                this.entityPm.CreatedByUserId = this.loggedContact.Id;
            }

            this.Poco = new Automation();
            this.Poco.Id = this.entityPm.Id;



            AutomationMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

            SaveAutomationResultEmailRecipientLists();
            SaveAutomationLastUpdate(entityPM.ObjectTableId, entityPM.Tenant);
            SaveAutomationHistory();

            List<string> eventCodeLists = new List<string>(new string[] { "AUCR" });
            AddTraceEvent(eventCodeLists);

     
        }

        private void SaveAutomationResultEmailRecipientLists()
        {
            if (this.entityPm.AutomationResultEmailRecipientLists != null && this.entityPm.AutomationResultEmailRecipientLists.Count() > 0)
            {
                AutomationResultEmailRecipientRepository entityRepository = new AutomationResultEmailRecipientRepository(this.entityPm.Tenant);
                foreach (AutomationResultEmailRecipientPM item in this.entityPm.AutomationResultEmailRecipientLists)
                {
                    AutomationResultEmailRecipient Poco = new AutomationResultEmailRecipient();
                    if (string.IsNullOrEmpty(item.Id))
                    {
                        item.Id = item.Id = IdCounter.GetNumber("AutomationResultEmailRecipient", item.Tenant).ToString();
                        item.AutomationsId = this.entityPm.Id;
                        item.Tenant = this.entityPm.Tenant;
                        MapAutomationResultEmailRecipientEntity(item, Poco, true);
                        entityRepository.Add(Poco);
    
                    }
                    else
                    {
                        MapAutomationResultEmailRecipientEntity(item, Poco, true);
                        entityRepository.Remove(Poco);

                    }
                }

                entityRepository.SubmitChanges();
                TableLastUpdateClass.UpdateTableHistory(this.entityPm.Tenant, "AutomationResultEmailRecipient");
            }
        }

        private static void MapAutomationResultEmailRecipientEntity(AutomationResultEmailRecipientPM entityPM, AutomationResultEmailRecipient entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }


            entityPOCO.AutomationsId = entityPM.AutomationsId;
            entityPOCO.RecipientType = entityPM.RecipientType;
            entityPOCO.RecipientValue = entityPM.RecipientValue;

        }

        private void SaveAutomationHistory()
        {

            AutomationHistoryService automationHistoryService = new AutomationHistoryService(this.ObjectContext, entityPm.Tenant);
            AutomationHistoryPM automationHistoryPM = new AutomationHistoryPM()
            {
                AutomationsId = entityPm.Id,
                Version = entityPm.Version,
                Tenant = entityPm.Tenant,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPm.Tenant),
                AutomationXML = entityPm.AutomationXML,
            };

            automationHistoryService.Create(automationHistoryPM);
        }

        public void SaveAutomationLastUpdate(string objectTableId, int tenant)
        {
            AutomationLastUpdateRepository automationLastUpdateRepository = new AutomationLastUpdateRepository(tenant);
            AutomationLastUpdate automationLastUpdate = automationLastUpdateRepository.GetSingleAutomationLastUpdate(objectTableId, tenant);

            if (automationLastUpdate != null)
            {
                automationLastUpdate.LastUpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                automationLastUpdateRepository.Update(automationLastUpdate);
            }
            else
            {
                automationLastUpdate = new AutomationLastUpdate()
                {
                    Tenant = tenant,
                    HasAutomation = true,
                    ObjectTableId = objectTableId,
                    LastUpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                };

                automationLastUpdateRepository.Add(automationLastUpdate);
            }
            automationLastUpdateRepository.SubmitChanges();
        }

        private void AddTraceEvent(List<string> eventCodeList)
        {
            if (eventCodeList!=null && eventCodeList.Count > 0)
            {
                string userId = this.loggedContact != null ? this.loggedContact.Id : "";

                foreach (string eventCode in eventCodeList)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = eventCode,
                        UserId = userId,
                        EntityId = this.entityPm.Id,
                        ObjectTableName = "Automation",
                    });
                }
            }
    
        }


        private void GetLoggedContact()
        {

            if (HttpContext.Current != null && HttpContext.Current.User!=null && HttpContext.Current.User.Identity!=null && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                string email = HttpContext.Current.User.Identity.Name;
                this.loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
            }
            else
            {
                string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
                this.loggedContact = contactRepository.GetSingleContactByEmail(systemContactEmail, tenant);

            }
        }


        public void Update(AutomationPM entityPM)
        {

            this.isNewEntity = false;
            this.entityPm = entityPM;

            this.entityPm.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            if (this.loggedContact != null)
            {
                this.entityPm.UpdatedByUserId = this.loggedContact.Id;
            }

            this.Poco = entityRepository.GetSingleAutomation(entityPM.Id, entityPm.Tenant);



            bool inactiveFieldChange = false;
  
            if (this.Poco.Inactive != this.entityPm.Inactive) inactiveFieldChange = true;

       
            AutomationMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

            SaveAutomationResultEmailRecipientLists();
            SaveAutomationLastUpdate(entityPM.ObjectTableId, entityPM.Tenant);
            SaveAutomationHistory();


            List<string> eventCodeLists = new List<string>(new string[] { "AUUP" });
            if (inactiveFieldChange)
            {
                if (entityPM.Inactive) eventCodeLists.Add("AUSI");
                else eventCodeLists.Add("AURE");
            }

            AddTraceEvent(eventCodeLists);

        }

    }
}