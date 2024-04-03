using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using System.Reflection;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure;
using System.Transactions;

namespace Logitude.Server.Tools.Helpers
{
    public class EventTracer
    {
        public static void CreateTraceEvent(EventTracerArgs args)
        {
            if (!string.IsNullOrEmpty(args.EventTypeCode))
            {
                int tenant = args.Tenant;
                IWebFreightContext objectContext = WebFreightContext.GetContext(tenant);

                EventTypeRepository eventTypeRepository = new EventTypeRepository(objectContext);
                EventType eventType = eventTypeRepository.GetSingleEventTypeByCode(args.EventTypeCode, tenant);
                if (eventType == null)
                {
                    throw new Exception("Event Type is not recognized:" + args.EventTypeCode);
                }
                else
                {
                    ObjectTableRepository objectTabelRepository = new ObjectTableRepository(objectContext);
                    ObjectTable objectTable = objectTabelRepository.GetObjectTableByName(args.ObjectTableName, 0, true);
                    ObjectTable childObjectTable = objectTabelRepository.GetObjectTableByName(args.ChildObjectTableName, 0, true);

                    #region User
                    string myUserId = null;
                    string myCustomerCareUserEmail = null;

                    if (!string.IsNullOrEmpty(args.UserId))
                    {
                        myUserId = args.UserId;

                        if (tenant != 0)
                        {
                            if (HttpContext.Current != null && HttpContext.Current.User != null)
                            {
                                string email = HttpContext.Current.User.Identity.Name;
                                if (!string.IsNullOrEmpty(email))
                                {
                                    UserRepository userRepository = new UserRepository(0);
                                    User user = userRepository.GetSingleUserByEmail(email, 0, true);
                                    if (user != null)
                                    {
                                        User systemUser = userRepository.GetSingleUserByEmail("system@tenant" + tenant + ".com", tenant, true);
                                        if (systemUser != null)
                                        {
                                            myUserId = systemUser.Id;
                                        }

                                        myCustomerCareUserEmail = user.Contact.Email;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Dates
                    if (args.LogDateTime == null)
                    {
                        args.LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    else if (args.LogDateTime.Value.Year == 1)
                    {
                        args.LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    if (args.EventDateTime == null)
                    {
                        args.EventDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    else if (args.EventDateTime.Value.Year == 1)
                    {
                        args.EventDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    if (args.EventTypeCode == "CWOP" || args.EventTypeCode == "CLOP" || args.EventTypeCode == "CCOP")
                    {
                        args.LogDateTime = args.EventDateTime;
                    }

                    #endregion

                    #region Status Notes
                    if (args.IsAddedManually)
                    {
                        if (!string.IsNullOrEmpty(args.NewStatusId) && !string.IsNullOrEmpty(args.CurrentStatusId))
                        {
                            EntityStatus newStatus = EntityStatusRepository.GetSingleEntityStatus(args.NewStatusId, tenant, true);
                            EntityStatus currentStatus = EntityStatusRepository.GetSingleEntityStatus(args.CurrentStatusId, tenant, true);

                            Contact user = ContactRepository.GetSingleContact(myUserId, tenant, true);
                            if (newStatus != null)
                            {
                                args.Notes = "Status was changed manually from " + currentStatus.Name + " to " + newStatus.Name + " by " + (user != null ? user.EnglishName : "");
                            }
                        }
                    }
                    #endregion

                    string myNotes = args.Notes;
                    if (myNotes != null)
                    {
                        string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                        if (dbms == "oracle")
                        {
                            if (myNotes.Length > 2000)
                            {
                                myNotes = myNotes.Substring(0, 1999);
                            }
                        }

                        else
                        {
                            if (myNotes.Length > 4000)
                            {
                                myNotes = myNotes.Substring(0, 3999);
                            }
                        }
                    }

                    TraceEvent myTraceEvent = new TraceEvent()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Tenant = tenant,
                        EntityId = args.EntityId,
                        EventTypeId = eventType.Id,
                        ObjectTableId = objectTable.Id,
                        LogDateTime = args.LogDateTime.Value,
                        EventDateTime = args.EventDateTime.Value,
                        ExternalId = args.ExternalId?.Trim(),
                        IsAddedManually = args.IsAddedManually,
                        UserId = myUserId,
                        CustomerCareUserEmail = myCustomerCareUserEmail,
                        Notes = myNotes,
                        Location = null,
                        ChildEntityId = args.ChildEntityId,
                        ChildObjectTableId = childObjectTable?.Id,
                    };

                    if ((Transaction.Current != null && Transaction.Current.IsolationLevel == System.Transactions.IsolationLevel.Snapshot)
                        || Transaction.Current == null)
                    {
                        using (var scope = objectContext.GetSnapshotTransaction())
                        {
                            TraceEventRepository traceEventRepository = new TraceEventRepository(objectContext);
                            traceEventRepository.Add(myTraceEvent);
                            traceEventRepository.SubmitChanges();
                            objectContext.SaveChanges();
                            scope.Commit();
                        }
                    }
                    else
                    {
                        TraceEventRepository traceEventRepository = new TraceEventRepository(objectContext);
                        traceEventRepository.Add(myTraceEvent);
                        traceEventRepository.SubmitChanges();
                        objectContext.SaveChanges();
                    }

                    if (eventType.IsCustomerView)
                    {
                        ContactsUnseenEntitiesHelper.AddUnseenEntityRecord(myTraceEvent.Id, tenant);
                    }

                    if (!string.IsNullOrEmpty(eventType.CustomField) && objectTable.AllowCustomFields)
                    {
                        EventCustomFieldUpdateService.UpdateEventCustomFieldValue(new UpdateEventCustomFieldArgs() { CustomField = eventType.CustomField, EventDateTime = myTraceEvent.EventDateTime, Entity = args.Entity, EntityId = args.EntityId, ObjectTableName = args.ObjectTableName, Tenant = args.Tenant });
                    }

                }

            }
        }

        public static Response CreateTraceEventsList(List<TraceEventParams> traceEventParamsList, int tenant, string objectTableName, string currentStatusId, string param = null)
        {
            Response response = new Response();
            IWebFreightContext objectContext = WebFreightContext.GetContext(tenant);

            EventTypeRepository eventTypesRepository = new EventTypeRepository(objectContext);
            TraceEventRepository traceEventsRepository = new TraceEventRepository(objectContext);
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(objectContext);
            EntityStatusRepository entityStatusRepository = new EntityStatusRepository(objectContext);

            Tenant tenantEntity = TenantRepository.GetSingleTenant(tenant, true);
            ObjectTable objectTable = objectTableRepository.GetObjectTableByName(objectTableName, 0, true);

            List<EventType> eventTypes = eventTypesRepository.GetEventTypesByTenantAndObjectTableId(tenant, objectTable.Id).ToList();
            List<EntityStatus> statusList = entityStatusRepository.GetEntityStatusByTenant(tenant).ToList();

            string newStatusId = null;
            string lastStatusId = currentStatusId;
            foreach (TraceEventParams traceEventParams in traceEventParamsList)
            {
                TraceEvent traceEvent = new TraceEvent();
                traceEvent.Id = Guid.NewGuid().ToString();
                EventType eventType = eventTypes.Where(e => e.Code == traceEventParams.EventTypeCode).FirstOrDefault();

                if (eventType == null)
                {
                    response.HasError = true;
                    response.ErrorMessage = "EventTypeCode:" + traceEventParams.EventTypeCode + " doesn't exist in the database,Upsert this entity before using it," + Environment.NewLine;
                    return response;
                }

                if (traceEventParams.Tenant != 0)
                {
                    UserRepository userRepository = new UserRepository(0);
                    User user = userRepository.GetSingleUser(traceEventParams.UserId, 0, false);
                    if (user != null)
                    {

                        User systemUser = userRepository.GetSingleUserByCodeOrEmail(null, "system@tenant" + traceEventParams.Tenant + ".com", traceEventParams.Tenant, true);
                        if (systemUser != null)
                        {
                            traceEventParams.UserId = systemUser.Id;
                        }
                        traceEvent.CustomerCareUserEmail = user.Contact.Email;
                    }
                }

                traceEvent.UserId = traceEventParams.UserId;
                traceEvent.EntityId = traceEventParams.EntityId;
                if (traceEventParams.LogDate != null)
                {
                    traceEvent.LogDateTime = traceEventParams.LogDate.Value;
                }
                else
                {
                    traceEvent.LogDateTime = TenantServerConfigration.GetCurrentDateTime(traceEventParams.Tenant);
                }

                if (traceEventParams.EventDate != null)
                {
                    traceEvent.EventDateTime = traceEventParams.EventDate.Value;
                }

                if (traceEvent.EventDateTime.Year == 1)
                {
                    traceEvent.EventDateTime = TenantServerConfigration.GetCurrentDateTime(traceEventParams.Tenant);
                }

                traceEvent.Notes = traceEventParams.Notes;
                traceEvent.Tenant = traceEventParams.Tenant;
                traceEvent.ObjectTableId = objectTable.Id;
                traceEvent.EventTypeId = eventType.Id;
                traceEvent.ExternalId = traceEventParams.ExternalId?.Trim();

                if (traceEventParams.Manually)
                {
                    EntityStatus currentStatus = statusList.Where(s => s.Id == traceEventParams.CurrentStatusId).FirstOrDefault();
                    EntityStatus newStatus = statusList.Where(s => s.Id == traceEventParams.NewStatusId).FirstOrDefault();
                    Contact user = ContactRepository.GetSingleContact(traceEventParams.UserId, traceEventParams.Tenant, true);
                    if (newStatus != null)
                        traceEvent.Notes = "Status were changed manually from " + currentStatus.Name + " to " + newStatus.Name + " by " + (user != null ? user.EnglishName : "");
                    traceEvent.IsAddedManually = true;
                }
                traceEventsRepository.Add(traceEvent);
                if (!traceEventParams.Manually)
                {
                    DateTime? statusDate = null;
                    newStatusId = GetNewStatusId(lastStatusId, eventType.EntityStatusId, traceEventParams.Tenant, ref statusDate, statusList);
                }
                else
                {
                    newStatusId = traceEventParams.NewStatusId;
                }

                lastStatusId = newStatusId;


                if (traceEvent != null)
                {
                    if (eventType != null && eventType.IsCustomerView)
                    {
                        ContactsUnseenEntitiesHelper.AddUnseenEntityRecord(traceEvent.Id, traceEventParams.Tenant);
                    }
                }


                // ContactsUnseenEntitiesHelper.AddUnseenEntityRecord(traceEventParams.UserId, traceEventParams.EntityId, objectTable.Id, eventType, traceEventParams.Tenant, param, "");

            }


            objectContext.SaveChanges();
            response.Result = newStatusId;


            return response;
        }

        public static string GetNewStatusId(string currentStatusId, string newStatusId, int tenant, ref DateTime? statusDate, List<EntityStatus> statusList)
        {
            EntityStatus currentStatus = statusList.Where(s => s.Id == currentStatusId).FirstOrDefault();
            EntityStatus newStatus = statusList.Where(s => s.Id == newStatusId).FirstOrDefault();

            if (currentStatus == null)
            {
                statusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                return newStatusId;
            }

            else if (newStatus == null)
            {
                return currentStatusId;
            }

            string id = null;

            if (currentStatus.StatusWeight > newStatus.StatusWeight)
            {
                id = currentStatus.Id;
            }

            else
            {
                statusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                id = newStatus.Id;
            }

            return id;
        }


    }

    public class TraceEventParams
    {
        public string EventTypeCode { get; set; }
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string UserId { get; set; }
        public string Notes { get; set; }
        public string ObjectTableName { get; set; }
        public string CurrentStatusId { get; set; }
        public string NewStatusId { get; set; }
        public bool Manually { get; set; }
        public DateTime? StatusDate { get; set; }
        public DateTime? LogDate { get; set; }
        public DateTime? EventDate { get; set; }
        public string ExternalId { get; set; }
    }


    public class EventTracerArgs
    {
        public int Tenant { get; set; }
        public string Notes { get; set; }
        public string UserId { get; set; }
        public string EntityId { get; set; }
        public string EventTypeCode { get; set; }
        public string ObjectTableName { get; set; }
        public bool IsAddedManually { get; set; }
        public DateTime? LogDateTime { get; set; }
        public DateTime? EventDateTime { get; set; }
        public string ExternalId { get; set; }
        public string NewStatusId { get; set; }
        public string CurrentStatusId { get; set; }
        public object Entity { get; set; }
        public string ChildEntityId { get; set; }
        public string ChildObjectTableName { get; set; }
    }

    public class UpdateEventCustomFieldArgs
    {
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableName { get; set; }
        public object Entity { get; set; }
        public string CustomField { get; set; }
        public DateTime EventDateTime { get; set; }
        public string EventTypeId { get; set; }



    }





}