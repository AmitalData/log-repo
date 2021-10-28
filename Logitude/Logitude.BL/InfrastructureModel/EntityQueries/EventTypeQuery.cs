using System;
using System.Linq;
using System.Web;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityLists;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using System.Collections.Generic;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class EventTypeQuery
    {
        EventTypeRepository repository;

        public EventTypeQuery()
        {
            repository = new EventTypeRepository(); 
        }

        public EventTypeQuery(int tenant)
        {
            repository = new EventTypeRepository(tenant);
        }

        public EventTypeQuery(EventTypeRepository eventTypeRepository)
        {
            repository = eventTypeRepository;
        }

        public EventTypePM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "EventTypePM" + id + tenant;
                EventTypePM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.EventType.Include("EntityStatus").Include("EventTypeCategory")
                                              where a.Tenant == tenant
                                              select new EventTypePM()
                                              {
                                                  AddedManually = a.AddedManually,
                                                  Code = a.Code,
                                                  EnglishName = a.EnglishName,
                                                  EntityStatusId = a.EntityStatusId,
                                                  Id = a.Id,
                                                  IsManualEntry = a.IsManualEntry,
                                                  LocalName = a.LocalName,
                                                  ObjectTableId = a.ObjectTableId,
                                                  Tenant = a.Tenant,
                                                  EntityStatusName = a.EntityStatus != null ? a.EntityStatus.Name : null,
                                                  ShortView = a.ShortView,
                                                  ManualActivatedFollowUp = a.ManualActivatedFollowUp,
                                                  FollowUpEnglishName = a.FollowUpEnglishName,
                                                  FollowUpLocalName = a.FollowUpLocalName,
                                                  IsFollowUp = a.IsFollowUp,
                                                  InActive = a.InActive,
                                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                  SearchFields = a.SearchFields,
                                                  AgentRoleId = a.AgentRoleId,
                                                  CustomerRoleId = a.CustomerRoleId,
                                                  EventTypeCategoryCode = a.EventTypeCategory != null ? a.EventTypeCategory.Code : null,
                                                  IsAgentView = a.IsAgentView,
                                                  IsCustomerView = a.IsCustomerView,
                                                  IsSharedLogisticsEnabled = a.IsSharedLogisticsEnabled,
                                                  AllowedInAutomation = a.AllowedInAutomation,
                                                  CustomField = a.CustomField,
                                                  IsStatusNotModified = a.IsStatusNotModified,
                                              });

                        foreach (var s in entitystatuses)
                        {
                            string name = "EventTypePM" + s.Id + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }

                        entity = (EventTypePM)CacheManager.CacheWrapper.Get(entityName);
                    }

                    else
                    {
                        entity = (EventTypePM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }

                else
                {
                    entity = (from a in repository.context.EventType.Include("EntityStatus").Include("EventTypeCategory")
                              where a.Tenant == tenant && a.Id == id
                              select new EventTypePM()
                              {
                                  AddedManually = a.AddedManually,
                                  Code = a.Code,
                                  EnglishName = a.EnglishName,
                                  EntityStatusId = a.EntityStatusId,
                                  Id = a.Id,
                                  IsManualEntry = a.IsManualEntry,
                                  LocalName = a.LocalName,
                                  ObjectTableId = a.ObjectTableId,
                                  Tenant = a.Tenant,
                                  EntityStatusName = a.EntityStatus != null ? a.EntityStatus.Name : null,
                                  ShortView = a.ShortView,
                                  ManualActivatedFollowUp = a.ManualActivatedFollowUp,
                                  FollowUpEnglishName = a.FollowUpEnglishName,
                                  FollowUpLocalName = a.FollowUpLocalName,
                                  IsFollowUp = a.IsFollowUp,
                                  InActive = a.InActive,
                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                  SearchFields = a.SearchFields,
                                  AgentRoleId = a.AgentRoleId,
                                  CustomerRoleId = a.CustomerRoleId,
                                  EventTypeCategoryCode = a.EventTypeCategory != null ? a.EventTypeCategory.Code : null,
                                  IsAgentView = a.IsAgentView,
                                  IsCustomerView = a.IsCustomerView,
                                  IsSharedLogisticsEnabled = a.IsSharedLogisticsEnabled,
                                  AllowedInAutomation = a.AllowedInAutomation,
                                  CustomField = a.CustomField,
                                  IsStatusNotModified = a.IsStatusNotModified,
                              }).FirstOrDefault();
                }

                return entity;
            }
            return null;
        }

        public IQueryable<EventTypePM> GetEventTypePMsByTenant(int tenant)
        {
            IQueryable<EventTypePM> eventTypes = from a in repository.context.EventType.Include("EntityStatus").Include("EventTypeCategory")
                                                 where a.Tenant == tenant
                                                 select new EventTypePM()
                                                 {
                                                     AddedManually = a.AddedManually,
                                                     Code = a.Code,
                                                     EnglishName = a.EnglishName,
                                                     EntityStatusId = a.EntityStatusId,
                                                     Id = a.Id,
                                                     IsManualEntry = a.IsManualEntry,
                                                     LocalName = a.LocalName,
                                                     ObjectTableId = a.ObjectTableId,
                                                     Tenant = a.Tenant,
                                                     EntityStatusName = a.EntityStatus != null ? a.EntityStatus.Name : null,
                                                     ShortView = a.ShortView,
                                                     ManualActivatedFollowUp = a.ManualActivatedFollowUp,
                                                     FollowUpEnglishName = a.FollowUpEnglishName,
                                                     FollowUpLocalName = a.FollowUpLocalName,
                                                     IsFollowUp = a.IsFollowUp,
                                                     InActive = a.InActive,
                                                     ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                     SearchFields = a.SearchFields,
                                                     AgentRoleId = a.AgentRoleId,
                                                     CustomerRoleId = a.CustomerRoleId,
                                                     EventTypeCategoryCode = a.EventTypeCategory != null ? a.EventTypeCategory.Code : null,
                                                     IsAgentView =a.IsAgentView,
                                                     IsCustomerView = a.IsCustomerView,
                                                     IsSharedLogisticsEnabled = a.IsSharedLogisticsEnabled,
                                                     AllowedInAutomation = a.AllowedInAutomation,
                                                     CustomField = a.CustomField,
                                                     IsStatusNotModified = a.IsStatusNotModified,

                                                 };
            return eventTypes;
        }

        // code changed by mohammad and alaa HttpContext.Current.Cache.Get(entityName) == null
        public EventTypePM GetSingleEventTypePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "EventTypePM" + id + tenant;
                EventTypePM entity;
                if (HttpContext.Current != null)
                {                    
                    if (CacheManager.CacheWrapper.Get(entityName)==null)
                    {
                        var entitystatuses = (from a in repository.context.EventType.Include("EntityStatus").Include("EventTypeCategory")
                                              where a.Tenant == tenant
                                              select new EventTypePM()
                                              {
                                                  AddedManually = a.AddedManually,
                                                  Code = a.Code,
                                                  EnglishName = a.EnglishName,
                                                  EntityStatusId = a.EntityStatusId,
                                                  //EventGroupCode = a.EventGroupCode,
                                                  Id = a.Id,
                                                  IsManualEntry = a.IsManualEntry,
                                                  LocalName = a.LocalName,
                                                  ObjectTableId = a.ObjectTableId,
                                                  Tenant = a.Tenant,
                                                  EntityStatusName = a.EntityStatus != null ? a.EntityStatus.Name : null,
                                                  ShortView = a.ShortView,
                                                  ManualActivatedFollowUp = a.ManualActivatedFollowUp,
                                                  FollowUpEnglishName = a.FollowUpEnglishName,
                                                  FollowUpLocalName = a.FollowUpLocalName,
                                                  IsFollowUp = a.IsFollowUp,
                                                  InActive = a.InActive,
                                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                  SearchFields = a.SearchFields,
                                                  AgentRoleId = a.AgentRoleId,
                                                  CustomerRoleId = a.CustomerRoleId,
                                                  EventTypeCategoryCode = a.EventTypeCategory != null ? a.EventTypeCategory.Code : null,
                                                  IsAgentView = a.IsAgentView,
                                                  IsCustomerView = a.IsCustomerView,
                                                  IsSharedLogisticsEnabled = a.IsSharedLogisticsEnabled,
                                                  AllowedInAutomation = a.AllowedInAutomation,
                                                  CustomField = a.CustomField,
                                                  IsStatusNotModified = a.IsStatusNotModified,

                                              });

                        foreach (var s in entitystatuses)
                        {
                            string name = "EventTypePM" + s.Id + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (EventTypePM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (EventTypePM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.EventType.Include("EntityStatus").Include("EventTypeCategory")
                              where a.Tenant == tenant && a.Id == id
                              select new EventTypePM()
                              {
                                  AddedManually = a.AddedManually,
                                  Code = a.Code,
                                  EnglishName = a.EnglishName,
                                  EntityStatusId = a.EntityStatusId,
                                  //EventGroupCode = a.EventGroupCode,
                                  Id = a.Id,
                                  IsManualEntry = a.IsManualEntry,
                                  LocalName = a.LocalName,
                                  ObjectTableId = a.ObjectTableId,
                                  Tenant = a.Tenant,
                                  EntityStatusName = a.EntityStatus != null ? a.EntityStatus.Name : null,
                                  ShortView = a.ShortView,
                                  ManualActivatedFollowUp = a.ManualActivatedFollowUp,
                                  FollowUpEnglishName = a.FollowUpEnglishName,
                                  FollowUpLocalName = a.FollowUpLocalName,
                                  IsFollowUp = a.IsFollowUp,
                                  InActive = a.InActive,
                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                  SearchFields = a.SearchFields,
                                  AgentRoleId = a.AgentRoleId,
                                  CustomerRoleId = a.CustomerRoleId,
                                  EventTypeCategoryCode = a.EventTypeCategory != null ? a.EventTypeCategory.Code : null,
                                  IsAgentView = a.IsAgentView,
                                  IsCustomerView = a.IsCustomerView,
                                  IsSharedLogisticsEnabled = a.IsSharedLogisticsEnabled,
                                  AllowedInAutomation = a.AllowedInAutomation,
                                  CustomField = a.CustomField,
                                  IsStatusNotModified = a.IsStatusNotModified,

                              }).FirstOrDefault();
                }

                return entity;
            }
            return null;
        }

        public EventTypePM GetSingleEventTypePMByCode(string code, int tenant)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "EventTypePM" + code + tenant;
                EventTypePM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.EventType.Include("EntityStatus").Include("EventTypeCategory")
                                              where a.Tenant == tenant
                                              select new EventTypePM()
                                              {
                                                  AddedManually = a.AddedManually,
                                                  Code = a.Code,
                                                  EnglishName = a.EnglishName,
                                                  EntityStatusId = a.EntityStatusId,
                                                  //EventGroupCode = a.EventGroupCode,
                                                  Id = a.Id,
                                                  IsManualEntry = a.IsManualEntry,
                                                  LocalName = a.LocalName,
                                                  ObjectTableId = a.ObjectTableId,
                                                  Tenant = a.Tenant,
                                                  EntityStatusName = a.EntityStatus != null ? a.EntityStatus.Name : null,
                                                  ShortView = a.ShortView,
                                                  ManualActivatedFollowUp = a.ManualActivatedFollowUp,
                                                  FollowUpEnglishName = a.FollowUpEnglishName,
                                                  FollowUpLocalName = a.FollowUpLocalName,
                                                  IsFollowUp = a.IsFollowUp,
                                                  InActive = a.InActive,
                                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                  SearchFields = a.SearchFields,
                                                  AgentRoleId = a.AgentRoleId,
                                                  CustomerRoleId = a.CustomerRoleId,
                                                  EventTypeCategoryCode = a.EventTypeCategory != null ? a.EventTypeCategory.Code : null,
                                                  IsAgentView = a.IsAgentView,
                                                  IsCustomerView = a.IsCustomerView,
                                                  IsSharedLogisticsEnabled = a.IsSharedLogisticsEnabled,
                                                  AllowedInAutomation = a.AllowedInAutomation,
                                                  CustomField = a.CustomField,
                                                  IsStatusNotModified = a.IsStatusNotModified,

                                              });

                        foreach (var s in entitystatuses)
                        {
                            string name = "EventTypePM" + s.Code + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (EventTypePM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (EventTypePM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.EventType.Include("EntityStatus").Include("EventTypeCategory")
                              where a.Tenant == tenant && a.Code == code
                              select new EventTypePM()
                              {
                                  AddedManually = a.AddedManually,
                                  Code = a.Code,
                                  EnglishName = a.EnglishName,
                                  EntityStatusId = a.EntityStatusId,
                                  //EventGroupCode = a.EventGroupCode,
                                  Id = a.Id,
                                  IsManualEntry = a.IsManualEntry,
                                  LocalName = a.LocalName,
                                  ObjectTableId = a.ObjectTableId,
                                  Tenant = a.Tenant,
                                  EntityStatusName = a.EntityStatus != null ? a.EntityStatus.Name : null,
                                  ShortView = a.ShortView,
                                  ManualActivatedFollowUp = a.ManualActivatedFollowUp,
                                  FollowUpEnglishName = a.FollowUpEnglishName,
                                  FollowUpLocalName = a.FollowUpLocalName,
                                  IsFollowUp = a.IsFollowUp,
                                  InActive = a.InActive,
                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                  SearchFields = a.SearchFields,
                                  AgentRoleId = a.AgentRoleId,
                                  CustomerRoleId = a.CustomerRoleId,
                                  EventTypeCategoryCode = a.EventTypeCategory != null ? a.EventTypeCategory.Code : null,
                                  IsAgentView = a.IsAgentView,
                                  IsCustomerView = a.IsCustomerView,
                                  IsSharedLogisticsEnabled = a.IsSharedLogisticsEnabled,
                                  AllowedInAutomation = a.AllowedInAutomation,
                                  CustomField = a.CustomField,
                                  IsStatusNotModified = a.IsStatusNotModified,

                              }).FirstOrDefault();
                }
                return entity;
            }
            return null;
        }
        
        public List<EventTypeList> GetEventTypeIdsByListEventCode(List<string> codeEventList, int tenant , string objectTableId)
        {
            List<EventTypeList> result = (from a in repository.context.EventType
                                          where a.Tenant == tenant && codeEventList.Contains(a.Code) && a.ObjectTableId == objectTableId
                                          select new EventTypeList()
                                               {
                                                   Code = a.Code,
                                                   Id = a.Id,
                                               }).ToList();

            return result;
        }

        public IQueryable<EventTypeList> GetIQueryableEntityList(IQueryable<EventType> iQueryable)
        {
            IQueryable<EventTypeList> result = from eventType in iQueryable.Include("EntityStatus").Include("EventTypeCategory")
                                               select new EventTypeList()
                                               {
                                                   AddedManually = eventType.AddedManually,
                                                   Code = eventType.Code,
                                                   EnglishName = eventType.EnglishName,
                                                   EntityStatusId = eventType.EntityStatusId,
                                                   Id = eventType.Id,
                                                   IsManualEntry = eventType.IsManualEntry,
                                                   LocalName = eventType.LocalName,
                                                   ObjectTableId = eventType.ObjectTableId,
                                                   Tenant = eventType.Tenant,
                                                   SearchFields = eventType.SearchFields,
                                                   ShortView = eventType.ShortView,
                                                   ManualActivatedFollowUp = eventType.ManualActivatedFollowUp,
                                                   FollowUpEnglishName = eventType.FollowUpEnglishName,
                                                   FollowUpLocalName = eventType.FollowUpLocalName,
                                                   IsFollowUp = eventType.IsFollowUp,
                                                   InActive = eventType.InActive,
                                                   EntityStatusName = eventType.EntityStatus != null ? eventType.EntityStatus.Name : null,
                                                   EntityStatusWeight = eventType.EntityStatus != null ? eventType.EntityStatus.StatusWeight : (int?)null,
                                                   EventTypeCategoryCode = eventType.EventTypeCategoryCode,
                                                   IsAgentView = eventType.IsAgentView,
                                                   IsCustomerView = eventType.IsCustomerView,
                                                   IsSharedLogisticsEnabled = eventType.IsSharedLogisticsEnabled,
                                                   AllowedInAutomation = eventType.AllowedInAutomation,
                                                   CustomField = eventType.CustomField,
                                               };
            return result;
        }

        public IQueryable<EventTypePM> GetEventTypesByObjectTable(string objectTableId, int tenant)
        {
            IQueryable<EventTypePM> eventTypes = from a in repository.context.EventType.Include("EntityStatus")
                                                 where a.Tenant == tenant && a.ObjectTableId == objectTableId
                                                 select new EventTypePM()
                                                 {
                                                     Id = a.Id,
                                                     Tenant = a.Tenant,
                                                     Code = a.Code,
                                                     AddedManually = a.AddedManually,
                                                     IsManualEntry = a.IsManualEntry,
                                                     LocalName = a.LocalName,
                                                     EnglishName = a.EnglishName,
                                                     EntityStatusId = a.EntityStatusId,
                                                     ObjectTableId = a.ObjectTableId,
                                                     IsFollowUp = a.IsFollowUp,
                                                     FollowUpEnglishName = a.FollowUpEnglishName,
                                                     FollowUpLocalName = a.FollowUpLocalName,
                                                     ShortView = a.ShortView,
                                                     ManualActivatedFollowUp = a.ManualActivatedFollowUp,
                                                     InActive = a.InActive,
                                                     SearchFields = a.SearchFields,
                                                     CustomerRoleId = a.CustomerRoleId,
                                                     AgentRoleId = a.AgentRoleId,
                                                     EventTypeCategoryCode = a.EventTypeCategoryCode,
                                                     IsCustomerView = a.IsCustomerView,
                                                     IsAgentView = a.IsAgentView,
                                                     IsSharedLogisticsEnabled = a.IsSharedLogisticsEnabled,
                                                     EntityStatusName = a.EntityStatus != null ? a.EntityStatus.Name : null,
                                                     ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                     AllowedInAutomation = a.AllowedInAutomation,
                                                     CustomField = a.CustomField,
                                                     IsStatusNotModified = a.IsStatusNotModified,

                                                 };
            return eventTypes;
        }

        public IQueryable<EventTypePM> GetEventTypesByObjectTableConnectedToStatus(string objectTableId, int tenant)
        { 
            IQueryable<EventTypePM> eventTypes = from a in repository.context.EventType.Include("EntityStatus")
                                                 where a.Tenant == tenant && a.ObjectTableId == objectTableId && !String.IsNullOrEmpty(a.EntityStatusId)
                                                 select new EventTypePM()
                                                 {
                                                     Id = a.Id,
                                                     Tenant = a.Tenant,
                                                     Code = a.Code,
                                                     AddedManually = a.AddedManually,
                                                     IsManualEntry = a.IsManualEntry,
                                                     LocalName = a.LocalName,
                                                     EnglishName = a.EnglishName,
                                                     EntityStatusId = a.EntityStatusId,
                                                     ObjectTableId = a.ObjectTableId,
                                                     IsFollowUp = a.IsFollowUp,
                                                     FollowUpEnglishName = a.FollowUpEnglishName,
                                                     FollowUpLocalName = a.FollowUpLocalName,
                                                     ShortView = a.ShortView,
                                                     ManualActivatedFollowUp = a.ManualActivatedFollowUp,
                                                     InActive = a.InActive,
                                                     SearchFields = a.SearchFields,
                                                     CustomerRoleId = a.CustomerRoleId,
                                                     AgentRoleId = a.AgentRoleId,
                                                     EventTypeCategoryCode = a.EventTypeCategoryCode,
                                                     IsCustomerView = a.IsCustomerView,
                                                     IsAgentView = a.IsAgentView,
                                                     IsSharedLogisticsEnabled = a.IsSharedLogisticsEnabled,
                                                     EntityStatusName = a.EntityStatus != null ? a.EntityStatus.Name : null,
                                                     ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                     AllowedInAutomation = a.AllowedInAutomation,
                                                     CustomField = a.CustomField,
                                                     IsStatusNotModified = a.IsStatusNotModified,

                                                 };
            return eventTypes;
        }
         
    }
}