using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.EntityChanges.AutomationResult;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.Server.Tools.EntityChanges
{
    public class MainEntityChangeService
    {


        public List<EntityChangeAutomation> EntityChangesAutomationsFailedList { get; set; }
        public List<EntityChangeAutomation> EntityChangesAutomationsSsucceedList { get; set; }
        public bool IsDelayAutomation { get; set; }
        public List<c> Changefields = new List<c>();
        public bool IsChangeSLA { get; set; }
        private List<IAutomationResultService> AutomationResultLists {get;set;}
        private EntityChangeArgs entityChangeArgs { get; set; }
        private DateTime startDate {get;set;}
        private AutomationObjectTableClass automationObjectTable { get; set; }
        private AutomationObjectTableClass otherAutomationObjectTable { get; set; }
        private List<ObjectField> automationsObjectFieldLists { get; set; }
        private GeneralEntityChangeService generalEntityChangeService { get; set; }
        private AutomationObjectFieldService automationObjectFieldService { get; set; }
        private EntityChangeRepository entityChangeRepository { get; set; }
        public MainEntityChangeService(EntityChangeArgs entityChangeArgs)
        {
            this.entityChangeArgs = entityChangeArgs;
            startDate = entityChangeArgs.StartDate != null ? (DateTime)entityChangeArgs.StartDate : DateTime.Now;
           
            entityChangeRepository = new EntityChangeRepository(entityChangeArgs.Tenant);
            automationObjectFieldService = new AutomationObjectFieldService(entityChangeArgs);
            generalEntityChangeService = new GeneralEntityChangeService();

            this.EntityChangesAutomationsFailedList = new List<EntityChangeAutomation>();
            this.EntityChangesAutomationsSsucceedList = new List<EntityChangeAutomation>();
            this.automationObjectTable = generalEntityChangeService.GetAutomationObjectTableByName(entityChangeArgs.ObjectTableName , entityChangeArgs.Tenant);
            this.otherAutomationObjectTable = generalEntityChangeService.GetAutomationObjectTableByName(entityChangeArgs.OtherObjectTableName , entityChangeArgs.Tenant);

            AutomationResultLists = GetAutomationResultLists();
        }


        private List<IAutomationResultService> GetAutomationResultLists()
        {
            var result = new List<IAutomationResultService>();
            result.Add(new AutomationEmailResultService());
            result.Add(new AutomationFollowUpResultService());
            result.Add(new AutomationSetValueResultService());
            result.Add(new AutomationSLAResultService());
            result.Add(new AutomationQueuedTaskResultService());
            result.Add(new AutomationSendInterfaceResultService());

            

            return result;
        }

        public void AddEntityChange()
        {
           var entityChange = CreateEntityChange(automationObjectTable.OriginalId);

            var isHaveAutomation = false;
            if (!string.IsNullOrEmpty(automationObjectTable.AutomationLastUpdate) || (otherAutomationObjectTable != null && !string.IsNullOrEmpty(otherAutomationObjectTable.AutomationLastUpdate)))
            {
                var automationLists = GetAutomationLists();
                if (automationLists.Count() > 0)
                {
                    isHaveAutomation = true;
                    BuildAutomationsObjectFieldLists(automationLists);
                    var resolverAutomationObjectFieldService = new ResolverAutomationObjectFieldService(entityChangeArgs);
                    List<Field> automationFieldLists = resolverAutomationObjectFieldService.GetAutomationObjectFieldValueLists(automationsObjectFieldLists, automationObjectTable);
                    List<Field> externalEntityAutomationConditionFieldLists = resolverAutomationObjectFieldService.GetExternalEntityAutomationConditionFieldLists(automationsObjectFieldLists, automationFieldLists, entityChangeArgs.Tenant);
                    if (externalEntityAutomationConditionFieldLists.Count() > 0) automationFieldLists = automationFieldLists.Concat(externalEntityAutomationConditionFieldLists).ToList();

                    AutomationConditionFields automationConditionFields = automationObjectFieldService.GetAutomationConditionFields(automationFieldLists, automationObjectTable, otherAutomationObjectTable);
                    entityChange.AutomationConditionFieldsXml = LogitudeXmlSerializer.SerializeObjectToXmlString(automationConditionFields);

                    AutomationResultArgs automationResultArgs = new AutomationResultArgs() { EntityPM = entityChangeArgs.EntityPM, EntityChange = entityChange, AutomationLists = automationLists, AutomationFieldLists = automationFieldLists, AutomationObjectTable = automationObjectTable, OtherAutomationObjectTable = otherAutomationObjectTable, EntityChangeArgs = entityChangeArgs, MainEntityChangeService = this };
                    foreach (IAutomationResultService service in AutomationResultLists)
                    {
                        service.Run(automationResultArgs);
                    }

                    UpdateEntityAutomationResulltXml(entityChange);

                    entityChange.ChangesAutomationFieldsXml = GetChangesAutomationFieldsXml(entityChange);

                    if (!IsDelayAutomation && automationLists.Where(d => d.ResultCode == "EMAIL").ToList().Count == 0)
                    {
                        entityChange.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
                    }
                }
            }

            SaveEntityChange(entityChange, isHaveAutomation);
        }

        private string GetChangesAutomationFieldsXml(EntityChange entityChange)
        {
            var result = string.Empty;
            if (Changefields.Count() > 0)
            {
                r rFields = new r();
                rFields.cs = Changefields;
                result = rFields.cs.Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(rFields) : "";
            }
            return result;
        }

        private void UpdateEntityAutomationResulltXml(EntityChange entityChange)
        {
            entityChange.SetAutomationFailedXml = EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "Set Fields Value").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "Set Fields Value").ToList()) : "";
            entityChange.SetSLAAutomationFailedXml = EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "Set SLA Value").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "Set SLA Value").ToList()) : "";
            entityChange.FollowUpAutomationFailedXml = EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "F/U Creation" || d.ResultCode == "Docs Out F/U Creation" || d.ResultCode == "Docs In F/U Creation").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "F/U Creation" || d.ResultCode == "Docs Out F/U Creation" || d.ResultCode == "Docs In F/U Creation").ToList()) : "";
            entityChange.QueuedTaskAutomationFailedXml = EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "QUEUE").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "QUEUE").ToList()) : "";
            entityChange.SendInterfaceAutomationFailedXml = EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "SENDINTERFACE").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "SENDINTERFACE").ToList()) : "";


            entityChange.SetAutomationSsucceedXml = EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "Set Fields Value").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "Set Fields Value").ToList()) : "";
            entityChange.SetSLAAutomationSsucceedXml = EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "Set SLA Value").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "Set SLA Value").ToList()) : "";
            entityChange.FollowUpAutomationSsucceedXml = EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "F/U Creation" || d.ResultCode == "Docs Out F/U Creation" || d.ResultCode == "Docs In F/U Creation").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "F/U Creation" || d.ResultCode == "Docs Out F/U Creation" || d.ResultCode == "Docs In F/U Creation").ToList()) : "";
            entityChange.QueuedTaskAutomationSsucceedXml = EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "QUEUE").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "QUEUE").ToList()) : "";
            entityChange.SendInterfaceAutomationSsucceedXml = EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "SENDINTERFACE").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "SENDINTERFACE").ToList()) : "";

        }

        private void AddEntityChangeQueue(string entityChangeId,  int tenant)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("entitychangequeue", tenant);
            queueservice.Send(new Dictionary<string, string>() { { "EntityChangeId", entityChangeId }, { "Tenant", tenant.ToString() }, { "Type", entityChangeArgs.ProcessType }, { "IsDelayAutomation", IsDelayAutomation.ToString().ToLower() } }, tenant, null, null, null, null);
        }




        private void BuildAutomationsObjectFieldLists(List<Automation> automations)
        {
            automationsObjectFieldLists = automationObjectFieldService.GetAutomationObjectFieldLists(automations, automationObjectTable);
            if (otherAutomationObjectTable != null)
            {
                var otherAutomationsObjectFieldLists = automationObjectFieldService.GetAutomationObjectFieldLists(automations, otherAutomationObjectTable);
                foreach (ObjectField item in otherAutomationsObjectFieldLists)
                {
                    if (!automationsObjectFieldLists.Contains(item)) automationsObjectFieldLists.Add(item);
                }
            }
        }

        private void SaveEntityChange(EntityChange entityChange, bool isHaveAutomation)
        {
            if (isHaveAutomation || (!isHaveAutomation && automationObjectTable.Name != "Shipment" && automationObjectTable.Name != "Master"))
            {
                entityChange.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChangeArgs.Tenant);
                entityChange.ExecutionTime = (int)((DateTime.Now.Ticks - startDate.Ticks) / TimeSpan.TicksPerMillisecond);
                entityChangeRepository.SubmitChanges();
            }
        }


        private List<Automation> GetAutomationLists()
        {
            AutomationRepository automationRepository = new AutomationRepository(entityChangeArgs.Tenant);
            var automations = automationRepository.GetAutomationsByObjectTableId(automationObjectTable.Id, entityChangeArgs.Tenant, automationObjectTable.AutomationLastUpdate).Where(d => d.Type == entityChangeArgs.ProcessType).OrderBy(d => d.Order).ToList();

            if (otherAutomationObjectTable != null && !string.IsNullOrEmpty(otherAutomationObjectTable.AutomationLastUpdate))
            {
                var otherAutomations = automationRepository.GetAutomationsByObjectTableId(otherAutomationObjectTable.Id, entityChangeArgs.Tenant, otherAutomationObjectTable.AutomationLastUpdate).Where(d => d.Type == entityChangeArgs.ProcessType).OrderBy(d => d.Order).ToList();
                automations = automations.Concat(otherAutomations).ToList();
            }
            return automations;
        }

        private EntityChange CreateEntityChange(string objectTableId)
        {
          var entityChange =  new EntityChange()
            {
                Id = IdCounter.GetNumber("EntityChange", entityChangeArgs.Tenant),
                Tenant = entityChangeArgs.Tenant,
                EntityId = entityChangeArgs.EntityId,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(entityChangeArgs.Tenant),
                CreateByUserId = generalEntityChangeService.GetLoggedContactId(entityChangeArgs.Tenant),
                ObjectTableId = objectTableId,
                ChangesFieldsXml = entityChangeArgs.EntityChangeFieldXml,
                CheckStartDate = TenantServerConfigration.GetCurrentDateTime(entityChangeArgs.Tenant),
            };

            entityChangeRepository.Add(entityChange);
            return entityChange;
        }




    }

    public class EntityChangeArgs
    {
        public Object EntityPM { get; set; }
        public Object OldEntityPM { get; set; }
        public string ProcessType { get; set; }
        public string EntityChangeFieldXml { get; set; }
        public string ObjectTableName { get; set; }
        public DateTime? StartDate { get; set; }
        public string EntityId { get; set; }
        public int Tenant { get; set; }
        public string OtherObjectTableName { get; set; }
        public Object ExternalEntity { get; set; }
    }


    public class AutomationResultArgs
    {
        public EntityChange EntityChange { get; set; }
        public AutomationObjectTableClass AutomationObjectTable { get; set; }
        public AutomationObjectTableClass OtherAutomationObjectTable { get; set; }
        public EntityChangeArgs EntityChangeArgs { get; set; }
        public List<Automation> AutomationLists { get; set; }
        public List<Field> AutomationFieldLists { get; set; }
        public MainEntityChangeService MainEntityChangeService { get; set; }
        public Object EntityPM { get; set; }



    }

    public class AutomationObjectTableClass
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string OriginalId { get; set; }
        public string OriginalName { get; set; }

        public DateTime? AutomationLastUpdateDate { get; set; }

        public string AutomationLastUpdate { get; set; }

    }
}
