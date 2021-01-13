using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.Server.Tools.Counters;
using System;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddEventTypes
    {
        public static void AddEventType(EventTypeDetails eventTypeDetails, EventTypeRepository eventTypeRepository, Dictionary<string, EventType> tenantEventTypes)
        {
            if (tenantEventTypes.Keys.Contains(eventTypeDetails.Code+eventTypeDetails.ObjectTableId))
            {
                EventType eventType = tenantEventTypes[eventTypeDetails.Code+eventTypeDetails.ObjectTableId];
                eventType.EnglishName = eventTypeDetails.EnglishName;
                eventType.AddedManually = eventTypeDetails.AddedManually;
                eventType.EntityStatusId = eventTypeDetails.EntityStatusId;
                eventType.FollowUpEnglishName = eventTypeDetails.FollowUpEnglishName;
                eventType.FollowUpLocalName = eventTypeDetails.FollowUpLocalName;
                eventType.InActive = eventTypeDetails.InActive;
                eventType.IsFollowUp = eventTypeDetails.IsFollowUp;
                eventType.IsManualEntry = eventTypeDetails.IsManualEntry;
                eventType.LocalName = eventTypeDetails.LocalName;
                eventType.ManualActivatedFollowUp = eventTypeDetails.ManualActivatedFollowUp;
                eventType.ObjectTableId = eventTypeDetails.ObjectTableId;
                eventType.ShortView = eventTypeDetails.ShortView;
                eventType.Tenant = eventTypeDetails.Tenant;
                eventType.SearchFields = eventTypeDetails.Code + "," + eventTypeDetails.EnglishName + "," + eventTypeDetails.LocalName;
                eventType.EventTypeCategoryCode = string.IsNullOrEmpty(eventTypeDetails.EventTypeCategoryCode) ? "OPE" : eventTypeDetails.EventTypeCategoryCode;
                eventType.IsCustomerView = eventTypeDetails.IsCustomerView;
                eventType.IsAgentView = eventTypeDetails.IsAgentView;
                eventType.IsSharedLogisticsEnabled = eventTypeDetails.IsSharedLogisticsEnabled;
                eventType.AllowedInAutomation = eventTypeDetails.AllowedInAutomation;
                eventType.UpdateDate = DateTime.Now;
                eventTypeRepository.Update(eventType);
            }

            else
            {
                EventType newEventType = new EventType()
                {
                    Id = IdCounter.GetNumber("EventType", eventTypeDetails.Tenant).ToString(),
                    Tenant = eventTypeDetails.Tenant,
                    ShortView = eventTypeDetails.ShortView,
                    ObjectTableId = eventTypeDetails.ObjectTableId,
                    ManualActivatedFollowUp = eventTypeDetails.ManualActivatedFollowUp,
                    LocalName = eventTypeDetails.LocalName,
                    IsManualEntry = eventTypeDetails.IsManualEntry,
                    IsFollowUp = eventTypeDetails.IsFollowUp,
                    InActive = eventTypeDetails.InActive,
                    FollowUpLocalName = eventTypeDetails.FollowUpLocalName,
                    FollowUpEnglishName = eventTypeDetails.FollowUpEnglishName,
                    EntityStatusId = eventTypeDetails.EntityStatusId,
                    AddedManually = eventTypeDetails.AddedManually,
                    Code = eventTypeDetails.Code,
                    EnglishName = eventTypeDetails.EnglishName,
                    SearchFields= eventTypeDetails.Code + "," + eventTypeDetails.EnglishName + "," + eventTypeDetails.LocalName,
                    EventTypeCategoryCode = string.IsNullOrEmpty(eventTypeDetails.EventTypeCategoryCode) ? "OPE" : eventTypeDetails.EventTypeCategoryCode,
                    IsCustomerView = eventTypeDetails.IsCustomerView,
                    IsAgentView = eventTypeDetails.IsAgentView,
                    IsSharedLogisticsEnabled = eventTypeDetails.IsSharedLogisticsEnabled,
                    AllowedInAutomation = eventTypeDetails.AllowedInAutomation,
                    UpdateDate = DateTime.Now,
            };

                eventTypeRepository.Add(newEventType);
            }

        }
    }
}