using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Collections.Generic;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel;
using System;
using System.Text;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class TraceEventQuery
    {
        TraceEventRepository repository;

        public TraceEventQuery(int tenant)
        {
            repository = new TraceEventRepository(tenant);
        }

        public TraceEventQuery(TraceEventRepository traceEventRepository)
        {
            repository = traceEventRepository;
        }



        public IQueryable<TraceEventPM> GetTraceEventPMsByEntityIdForMobile(int tenant, string entityId, string objectTableId, ShipmentPM shipmentPM)
        {
     
            ObjectTableRepository objectTableRep = new ObjectTableRepository(tenant);
            string masterId = null;
            string customId = null;
          
                if (shipmentPM != null)
                {
                    if (shipmentPM.ShipmentLevelCode == "H") masterId = shipmentPM.MasterShipmentDataId;
                    if (!string.IsNullOrEmpty(shipmentPM.CustomFileId)) customId = shipmentPM.CustomFileId;
                }

            IQueryable<TraceEventPM> traceEvents = from a in repository.context.TraceEvent.Include("EventType")
                                                   where a.Tenant == tenant && (a.EntityId == entityId || a.EntityId == masterId || (a.EntityId == customId && (a.EventType.Code == "DOK" || a.EventType.Code == "PIOD" || a.EventType.Code == "CCD" || a.EventType.Code == "RSH" || a.EventType.Code == "EXCE" || a.EventType.Code == "TRG"))) && a.ObjectTableId == objectTableId && !a.Deleted
                                                   orderby a.LogDateTime descending
                                                   select new TraceEventPM()
                                                   {
                                                       EntityId = a.EntityId,
                                                       EventDateTime = a.EventDateTime,
                                                       EventTypeId = a.EventTypeId,
                                                       Id = a.Id,
                                                       LogDateTime = a.LogDateTime,
                                                       Notes = a.Notes,
                                                       ObjectTableId = a.ObjectTableId,
                                                       Tenant = a.Tenant,
                                                       UserId = a.UserId,
                                                       Deleted = a.Deleted,
                                                       ShortView = a.EventType.ShortView,
                                                       EventTypeEnglishName = a.EventType.EnglishName,
                                                       EventTypeLocalName = a.EventType.LocalName,
                                                       EventTypeCode = a.EventType.Code,
                                                       IsManualEntry = a.EventType.IsManualEntry,
                                                       //EventTypeCategoryCode = a.EventType.EventTypeCategory != null ? a.EventType.EventTypeCategory.Code : null,
                                                       IsAgentView = a.EventType.IsAgentView,
                                                       IsCustomerView = a.EventType.IsCustomerView,
                                                       ExternalId = a.ExternalId,
                                                       IsAddedManually = a.IsAddedManually,
                                                       CustomerCareUserEmail = a.CustomerCareUserEmail,
                                                       Location = a.Location,
                                                       PartnerName = a.PartnerName,
                                                   };



            return traceEvents;
        }





        public IQueryable<TraceEventPM> GetTraceEventPMsByTenantByEntityId(int tenant, string entityId, string objectTableId)
        {
            ObjectTableRepository objectTableRep = new ObjectTableRepository(tenant);
            bool isShipment = objectTableRep.IsObjectTableShipment(objectTableId);
            string masterId = null;

            if (isShipment)
            {

                ShipmentRepository shipmentRep = new ShipmentRepository(tenant);
                Shipment shipment = shipmentRep.GetSingleShipmentwithOutIncludes(entityId, tenant);
                if (shipment != null)
                {
                    if (shipment.ShipmentLevelCode == "H") masterId = shipment.MasterShipmentDataId;
                }

            }
      
            IQueryable<TraceEventPM> traceEvents = from a in repository.context.TraceEvent.Include("EventType").Include("User.Contact").Include("EventType.EventTypeCategory")
                                                   where a.Tenant == tenant && (a.EntityId == entityId || a.EntityId == masterId) && a.ObjectTableId == objectTableId && !a.Deleted
                                                   orderby a.LogDateTime descending
                                                   select new TraceEventPM()
                                                   {
                                                       EntityId = a.EntityId,
                                                       EventDateTime = a.EventDateTime,
                                                       EventTypeId = a.EventTypeId,
                                                       Id = a.Id,
                                                       LogDateTime = a.LogDateTime,
                                                       Notes = a.Notes,
                                                       ObjectTableId = a.ObjectTableId,
                                                       Tenant = a.Tenant,
                                                       UserId = a.UserId,
                                                       Deleted = a.Deleted,
                                                       ShortView = a.EventType.ShortView,
                                                       EventTypeEnglishName = a.EventType.EnglishName,
                                                       EventTypeLocalName = a.EventType.LocalName,
                                                       EventTypeCode = a.EventType.Code,
                                                       ContactEnglishFirstName = a.User.Contact.EnglishName,
                                                       IsManualEntry = a.EventType.IsManualEntry,
                                                       EventTypeCategoryCode = a.EventType.EventTypeCategory != null ? a.EventType.EventTypeCategory.Code : null,
                                                       IsAgentView = a.EventType.IsAgentView,
                                                       IsCustomerView = a.EventType.IsCustomerView,
                                                       ExternalId = a.ExternalId,
                                                       IsAddedManually = a.IsAddedManually,
                                                       CustomerCareUserEmail = a.CustomerCareUserEmail,
                                                       Location = a.Location,
                                                       PartnerName = a.PartnerName,

                                                   };



            return traceEvents;
        }

        public IQueryable<TraceEventPM> GetTraceEventPMsByTenant(int tenant)
        {
            IQueryable<TraceEventPM> traceEvents = from a in repository.context.TraceEvent.Include("EventType")
                                                   where a.Tenant == tenant
                                                   select new TraceEventPM()
                                                   {
                                                       EntityId = a.EntityId,
                                                       EventDateTime = a.EventDateTime,
                                                       EventTypeId = a.EventTypeId,
                                                       Id = a.Id,
                                                       LogDateTime = a.LogDateTime,
                                                       Notes = a.Notes,
                                                       ObjectTableId = a.ObjectTableId,
                                                       Tenant = a.Tenant,
                                                       UserId = a.UserId,
                                                       Deleted = a.Deleted,
                                                       ShortView = a.EventType.ShortView,
                                                       EventTypeEnglishName = a.EventType.EnglishName,
                                                       EventTypeLocalName = a.EventType.LocalName,
                                                       EventTypeCode = a.EventType.Code,
                                                       IsManualEntry = a.EventType.IsManualEntry,
                                                       EventTypeCategoryCode = a.EventType.EventTypeCategoryCode,
                                                       ExternalId = a.ExternalId,
                                                       IsAddedManually = a.IsAddedManually,
                                                       CustomerCareUserEmail = a.CustomerCareUserEmail,
                                                       Location = a.Location,
                                                       PartnerName = a.PartnerName,
                                                   };
            foreach (TraceEventPM trace in traceEvents)
            {
                Contact contact = ContactRepository.GetSingleContact(trace.UserId, trace.Tenant, true);
                trace.ContactEnglishFirstName = contact.EnglishName;
            }

            return traceEvents;
        }

        public TraceEventPM GetSingleTraceEventPM(string id, int tenant)
        {
            TraceEventPM trace = (from a in repository.context.TraceEvent.Include("EventType")
                                  where a.Id == id
                                  select new TraceEventPM()
                                  {
                                      EntityId = a.EntityId,
                                      EventDateTime = a.EventDateTime,
                                      EventTypeId = a.EventTypeId,
                                      Id = a.Id,
                                      LogDateTime = a.LogDateTime,
                                      Notes = a.Notes,
                                      ObjectTableId = a.ObjectTableId,
                                      Tenant = a.Tenant,
                                      UserId = a.UserId,
                                      Deleted = a.Deleted,
                                      ShortView = a.EventType.ShortView,
                                      EventTypeEnglishName = a.EventType.EnglishName,
                                      EventTypeLocalName = a.EventType.LocalName,
                                      EventTypeCode = a.EventType.Code,
                                      IsManualEntry = a.EventType.IsManualEntry,
                                      EventTypeCategoryCode = a.EventType.EventTypeCategoryCode,
                                      ExternalId = a.ExternalId,
                                      IsAddedManually = a.IsAddedManually,
                                      CustomerCareUserEmail = a.CustomerCareUserEmail,
                                      Location = a.Location,
                                      PartnerName = a.PartnerName,
                                  }).FirstOrDefault();

            Contact contact = ContactRepository.GetSingleContact(trace.UserId, trace.Tenant, true);
            trace.ContactEnglishFirstName = contact.EnglishName;
            return trace;
        }

        public TraceEventPM GetSinglePM(string id, int tenant)
        {
            TraceEventPM trace = (from a in repository.context.TraceEvent.Include("EventType")
                                  where a.Id == id
                                  select new TraceEventPM()
                                  {
                                      EntityId = a.EntityId,
                                      EventDateTime = a.EventDateTime,
                                      EventTypeId = a.EventTypeId,
                                      Id = a.Id,
                                      LogDateTime = a.LogDateTime,
                                      Notes = a.Notes,
                                      ObjectTableId = a.ObjectTableId,
                                      Tenant = a.Tenant,
                                      UserId = a.UserId,
                                      Deleted = a.Deleted,
                                      ShortView = a.EventType.ShortView,
                                      EventTypeEnglishName = a.EventType.EnglishName,
                                      EventTypeLocalName = a.EventType.LocalName,
                                      EventTypeCode = a.EventType.Code,
                                      IsManualEntry = a.EventType.IsManualEntry,
                                      EventTypeCategoryCode = a.EventType.EventTypeCategoryCode,
                                      ExternalId = a.ExternalId,
                                      IsAddedManually = a.IsAddedManually,
                                      CustomerCareUserEmail = a.CustomerCareUserEmail,
                                      Location = a.Location,
                                      PartnerName = a.PartnerName,
                                  }).FirstOrDefault();

            Contact contact = ContactRepository.GetSingleContact(trace.UserId, trace.Tenant, true);
            trace.ContactEnglishFirstName = contact.EnglishName;
            return trace;
        }

        public TraceEventPM CheckIfEntityIsCustomsCleared(int tenant, string entityId,string ObjectTableId)
        {
            IQueryable<TraceEventPM> traceEvents = from a in repository.context.TraceEvent.Include("EventType").Include("User.Contact").Include("EventType.EventTypeCategory")
                                                   where a.Tenant == tenant && a.EntityId == entityId && a.EventType.Code.ToLower() == "ccd" && !a.Deleted
                                                   select new TraceEventPM()
                                                   {
                                                       EntityId = a.EntityId,
                                                       EventDateTime = a.EventDateTime,
                                                       EventTypeId = a.EventTypeId,
                                                       Id = a.Id,
                                                       LogDateTime = a.LogDateTime,
                                                       Notes = a.Notes,
                                                       ObjectTableId = a.ObjectTableId,
                                                       Tenant = a.Tenant,
                                                       UserId = a.UserId,
                                                       Deleted = a.Deleted,
                                                       ShortView = a.EventType.ShortView,
                                                       EventTypeEnglishName = a.EventType.EnglishName,
                                                       EventTypeLocalName = a.EventType.LocalName,
                                                       EventTypeCode = a.EventType.Code,
                                                       ContactEnglishFirstName = a.User.Contact.EnglishName,
                                                       IsManualEntry = a.EventType.IsManualEntry,
                                                       EventTypeCategoryCode = a.EventType.EventTypeCategory != null ? a.EventType.EventTypeCategory.Code : null,
                                                       IsAgentView = a.EventType.IsAgentView,
                                                       IsCustomerView = a.EventType.IsCustomerView,
                                                       ExternalId = a.ExternalId,
                                                       IsAddedManually = a.IsAddedManually,
                                                       CustomerCareUserEmail = a.CustomerCareUserEmail,
                                                       Location = a.Location,
                                                       PartnerName = a.PartnerName,

                                                   };
            return traceEvents.FirstOrDefault();
        }

        public string GetStatusCodeById(int tenant, string entityId, string statusId)
        {
            var temp = (from a in repository.context.EntityStatus
                       where a.Tenant == tenant && a.Id == statusId
                       select a).FirstOrDefault();

            //IQueryable<TraceEventPM> traceEvents = from a in repository.context.TraceEvent.Include("EventType").Include("User.Contact").Include("EventType.EventTypeCategory")
            //                                       where a.Tenant == tenant && a.EntityId == entityId && a.EventType.Code.ToLower() == temp.Code.ToLower() && !a.Deleted
            //                                       select new TraceEventPM()
            //                                       {
            //                                           EntityId = a.EntityId,
            //                                           EventDateTime = a.EventDateTime,
            //                                           EventTypeId = a.EventTypeId,
            //                                           Id = a.Id,
            //                                           LogDateTime = a.LogDateTime,
            //                                           Notes = a.Notes,
            //                                           ObjectTableId = a.ObjectTableId,
            //                                           Tenant = a.Tenant,
            //                                           UserId = a.UserId,
            //                                           Deleted = a.Deleted,
            //                                           ShortView = a.EventType.ShortView,
            //                                           EventTypeEnglishName = a.EventType.EnglishName,
            //                                           EventTypeCode = a.EventType.Code,
            //                                           ContactEnglishFirstName = a.User.Contact.EnglishName,
            //                                           IsManualEntry = a.EventType.IsManualEntry,
            //                                           EventTypeCategoryCode = a.EventType.EventTypeCategory != null ? a.EventType.EventTypeCategory.Code : null,
            //                                           IsAgentView = a.EventType.IsAgentView,
            //                                           IsCustomerView = a.EventType.IsCustomerView,
            //                                           ExternalId = a.ExternalId,
            //                                           IsAddedManually = a.IsAddedManually,
            //                                           CustomerCareUserEmail = a.CustomerCareUserEmail,
            //                                           Location = a.Location, 

            //                                       };
            return (temp != null ? temp.Code : "");
           
        }

        public TraceEventPM getCreatedEvent(int tenant, string entityId)
        {
            IQueryable<TraceEventPM> traceEvents = from a in repository.context.TraceEvent.Include("EventType").Include("User.Contact").Include("EventType.EventTypeCategory")
                                                   where a.Tenant == tenant && a.EntityId == entityId && a.EventType.Code.ToLower() == "opop" && !a.Deleted
                                                   select new TraceEventPM()
                                                   {
                                                       EntityId = a.EntityId,
                                                       EventDateTime = a.EventDateTime,
                                                       EventTypeId = a.EventTypeId,
                                                       Id = a.Id,
                                                       LogDateTime = a.LogDateTime,
                                                       Notes = a.Notes,
                                                       ObjectTableId = a.ObjectTableId,
                                                       Tenant = a.Tenant,
                                                       UserId = a.UserId,
                                                       Deleted = a.Deleted,
                                                       ShortView = a.EventType.ShortView,
                                                       EventTypeEnglishName = a.EventType.EnglishName,
                                                       EventTypeLocalName = a.EventType.LocalName,
                                                       EventTypeCode = a.EventType.Code,
                                                       ContactEnglishFirstName = a.User.Contact.EnglishName,
                                                       IsManualEntry = a.EventType.IsManualEntry,
                                                       EventTypeCategoryCode = a.EventType.EventTypeCategory != null ? a.EventType.EventTypeCategory.Code : null,
                                                       IsAgentView = a.EventType.IsAgentView,
                                                       IsCustomerView = a.EventType.IsCustomerView,
                                                       ExternalId = a.ExternalId,
                                                       IsAddedManually = a.IsAddedManually,
                                                       CustomerCareUserEmail = a.CustomerCareUserEmail,
                                                       Location = a.Location,
                                                       PartnerName = a.PartnerName,

                                                   };
            return traceEvents.FirstOrDefault();
        }



        public IQueryable<TraceEventPM> GetTraceEventPMsByEntityIdsAndObjectTableId( List<string>entityIds,string objectTableId, int tenant)
        {
           // string sql = GetTraceEventSelectSql(entityIds, objectTableId, tenant);

         //  IQueryable<TraceEvent> traceEvent = repository.context.GetActiveDbContext().Database.SqlQuery<TraceEvent>(sql).AsQueryable();

            IQueryable<TraceEventPM> traceEvents = from a in repository.context.TraceEvent.Include("EventType").Include("User.Contact")
                                                   where entityIds.Contains(a.EntityId) && a.Tenant == tenant && a.ObjectTableId == objectTableId
                                                   select new TraceEventPM() 
                                                   {
                                                       EventTypeCode = a.EventType != null? a.EventType.Code:null,
                                                       EventTypeEnglishName = a.EventType != null ?  a.EventType.Code:null,
                                                       EventDateTime = a.EventDateTime,
                                                       LogDateTime = a.LogDateTime,
                                                       EntityId = a.EntityId,
                                                       Notes = a.Notes,
                                                       EventTypeId = a.EventTypeId,
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       UserId = a.UserId,
                                                       IsAddedManually =a.IsAddedManually,
                                                       ContactEnglishFirstName = ((a.User != null && a.User.Contact != null) ? a.User.Contact.EnglishName : null),
                                                   };
            return traceEvents;
        }

        private static string GetTraceEventSelectSql(List<string> entityIds ,string objectTableId, int tenant)
        {
            var values = new StringBuilder();
            values.AppendFormat("{0}", "'" + entityIds[0] + "'");
            for (int i = 1; i < entityIds.Count; i++)
                values.AppendFormat(", {0}", "'" + entityIds[i] + "'");

            var sql = string.Format(
                "SELECT * FROM TraceEvent WHERE tenant = "  + tenant + " and objectTableId = "  + objectTableId  + " and id IN ({0})",
                values);
            return sql;
        }
    }
}
