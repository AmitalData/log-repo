using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.InfrastructureModel.EntityQueries; 
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.BL.InfrastructureModel.APIDataContract.ApiV1
{
	public partial class EventTypeDetailsQueryService
	{
		protected int Tenant { get; set; }
		protected string ObjectTableName { get; set; }

		public EventTypeDetailsQueryService(int tenant, string objectTableName)
        {
			this.Tenant = tenant;
			this.ObjectTableName = objectTableName;
			context = WebFreightContext.GetContext(tenant);
			query = new EventTypeQuery(tenant);

		}
		public EventsTypes GetEventTypeByObjectTable(bool connectedToStatus)
		{
			try
            {
                Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable objectTable = GetObjectTableId(this.ObjectTableName);

                IQueryable<EventTypePM> Events = GetEventTypes(connectedToStatus, objectTable);
                EventsTypes EventTypeList = BuildEventTypes(objectTable, Events);

                return EventTypeList;
            }
            catch (Exception ex)
			{

				throw ex;
			}


		}

        private EventsTypes BuildEventTypes(Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable objectTable, IQueryable<EventTypePM> Events)
        {
            EventsTypes EventTypeList = new EventsTypes();
            EventTypeList.EntityObjectTable = objectTable.Name;
            GetEventTypeDetails(Tenant, Events, EventTypeList);
            return EventTypeList;
        }

        private void GetEventTypeDetails(int Tenant, IQueryable<EventTypePM> Events, EventsTypes EventTypeList)
        {
            EventTypeList.Events = new List<EventTypeDetails>();
            foreach (var EventType in Events)
            {
                EventTypeList.Events.Add(EventTypeDetailsDataMapping(EventType, Tenant));
            }
        }

        private IQueryable<EventTypePM> GetEventTypes(bool connectedToStatus, Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable objectTable)
        {
            IQueryable<EventTypePM> Events = GetEventTypeWithStauses(objectTable.Id, this.Tenant, connectedToStatus);

            if (Events == null)
                throw new ApplicationException("EventType with Object Table Name " + this.ObjectTableName + " doesn't exist");
            return Events;
        }

        private static Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable GetObjectTableId(string objectTableName)
        {
            var objectTabelRepository = new ObjectTableRepository(0);
            var objectTable = objectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
            if (objectTable == null)
                throw new ApplicationException("Object Table Name " + objectTableName + " doesn't exist");
            return objectTable;
        }

        private IQueryable<EventTypePM> GetEventTypeWithStauses(string objectTableId, int tenant, bool connectedToStatus)
        {
			IQueryable<EventTypePM> EventsList = null;

			if (connectedToStatus)
			{
				EventsList = query.GetEventTypesByObjectTableConnectedToStatus(objectTableId, tenant);
			}
			else
			{
				EventsList = query.GetEventTypesByObjectTable(objectTableId, tenant);
			}

			return EventsList;
		}

        private List<EventTypeDetails> GetMappetEventTypes(IQueryable<EventTypePM> eventType)
		{
			List < EventTypeDetails > EventTypeList = new List<EventTypeDetails>();

			foreach (var eventItem in eventType)
            {
				var EventTypeDetails = new EventTypeDetails()
				{
					Id = eventItem.Id,
					EventTypeCode = eventItem.Code,
					EventTypeName = eventItem.EnglishName,
					EventTypeStatusEntity = GetEntityStatus(eventItem.EntityStatusId, eventItem.Tenant),
					EventTypeCustomerView = eventItem.IsCustomerView,
					EventTypeAgentView = eventItem.IsAgentView,

				};

                EventTypeList.Add(EventTypeDetails);
            }


			return EventTypeList;
 
		}


        private EntityStatus GetEntityStatus(string entityStatusId, int tenant)
        {
			EntityStatus entityStatus = new EntityStatus();
			if (entityStatusId != null)
			{
				EntityStatusQueryService EntityStatusService0 = new EntityStatusQueryService(tenant);
				entityStatus  = EntityStatusService0.GetEntityStatusById(entityStatusId, tenant);

			}
			return entityStatus;
		}
		 
	}
}

 