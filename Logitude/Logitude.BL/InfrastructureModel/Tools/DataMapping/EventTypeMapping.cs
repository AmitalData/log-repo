using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class EventTypeMapping
    {
        public static void MapEntity(EventTypePM eventTypePM, EventType eventType, bool isNewState)
        {
            if (eventTypePM.AddedManually)
            {
                // Ayman
                // Task 44065: Event Types Adjustments
                eventTypePM.IsManualEntry = true;
                eventTypePM.ManualActivatedFollowUp = eventTypePM.IsFollowUp;
            }

            eventType.AddedManually = eventTypePM.AddedManually;
            eventType.Code = eventTypePM.Code;
            eventType.EnglishName = eventTypePM.EnglishName;
            eventType.EntityStatusId = eventTypePM.EntityStatusId;
            eventType.ShortView = eventTypePM.ShortView;
            eventType.IsFollowUp = eventTypePM.IsFollowUp;
            eventType.FollowUpEnglishName = eventTypePM.FollowUpEnglishName;
            eventType.FollowUpLocalName = eventTypePM.FollowUpLocalName;
            eventType.SearchFields = eventTypePM.Code + "," + eventTypePM.EnglishName + "," + eventTypePM.LocalName;
            eventType.IsManualEntry = eventTypePM.IsManualEntry;
            eventType.LocalName = eventTypePM.LocalName;
            eventType.ObjectTableId = eventTypePM.ObjectTableId;
            eventType.Tenant = eventTypePM.Tenant;
            eventType.ManualActivatedFollowUp = eventTypePM.ManualActivatedFollowUp;
            eventType.InActive = eventTypePM.InActive;
            eventType.CustomerRoleId = eventTypePM.CustomerRoleId;
            eventType.AgentRoleId = eventTypePM.AgentRoleId;
            eventType.IsAgentView = eventTypePM.IsAgentView;
            eventType.IsCustomerView = eventTypePM.IsCustomerView;
            eventType.IsSharedLogisticsEnabled = eventTypePM.IsSharedLogisticsEnabled;
            eventType.AllowedInAutomation = eventTypePM.AllowedInAutomation;
            eventType.CustomField = eventTypePM.CustomField;
            eventType.IsStatusNotModified = eventTypePM.IsStatusNotModified;
            eventType.EventTrigger = eventTypePM.EventTrigger;

        }
    }
}