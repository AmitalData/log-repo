using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Resolvers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public class BankAccountTraceEventService :EntityTraceEventService<BankAccount,BankAccountPM>, IBankAccountTraceEventService
    {
        private IAccountingContext _MainContext;
        public BankAccountTraceEventService(IAccountingContext mainContext)
        {
            _MainContext = mainContext;
        }
        public override void Trace(BankAccountPM entityPM, BankAccount entityPOCO, string changesXml)
        {
            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(entityPM.Tenant);//GetLoggedContact(entityPM.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                //create trace event with created type.
                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = loggedContact.Id,
                    ObjectTableName = "BankAccount",
                    IsAddedManually = false,
                    EventTypeCode = "CREV",

                };
                AddEventToList(GetNewTraceEvent(eventTracerArgs));
            }
            else if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                //create trace event with updated type.
                string myEventNotes = "";

                if (entityPM.LocalName != entityPOCO.LocalName && (!string.IsNullOrEmpty(entityPM.LocalName) || !string.IsNullOrEmpty(entityPOCO.LocalName)))
                {
                    //myEventNotes = "Previous Local Name: " + (entityPOCO.LocalName == null ? "" : entityPOCO.LocalName);
                    //myEventNotes += " Local Name Changed" + TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.LocalName 
                    //                              + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.LocalName + ". ";
                    myEventNotes += GetOldNewEventNote("Local Name", entityPOCO.LocalName, entityPM.LocalName);

                }
                if (entityPM.EnglishName != entityPOCO.EnglishName && (!string.IsNullOrEmpty(entityPM.EnglishName) || !string.IsNullOrEmpty(entityPOCO.EnglishName)))
                {
                    //myEventNotes += " English Name Changed" + TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.EnglishName
                    //                              + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.EnglishName + ". ";
                    myEventNotes += GetOldNewEventNote("English Name", entityPOCO.EnglishName, entityPM.EnglishName);

                }

                if (entityPM.BranchNumber != entityPOCO.BranchNumber && (!string.IsNullOrEmpty(entityPM.BranchNumber) || !string.IsNullOrEmpty(entityPOCO.BranchNumber)))
                {
                    myEventNotes += GetOldNewEventNote("Branch Number", entityPOCO.BranchNumber, entityPM.BranchNumber);

                }

                if (entityPM.AccountNumber != entityPOCO.AccountNumber && (!string.IsNullOrEmpty(entityPM.AccountNumber) || !string.IsNullOrEmpty(entityPOCO.AccountNumber)))
                {
                    myEventNotes += GetOldNewEventNote("Account Number", entityPOCO.AccountNumber, entityPM.AccountNumber);

                }
                if (entityPM.Inactive != entityPOCO.Inactive)
                {
                    if (entityPM.Inactive == true)
                    {
                        EventTracerArgs eventTracerArgs0 = new EventTracerArgs()
                        {
                            EntityId = entityPM.Id,
                            Tenant = entityPM.Tenant,
                            UserId = loggedContact.Id,
                            ObjectTableName = "BankAccount",
                            IsAddedManually = false,
                            EventTypeCode = "DCTV",
                            Notes = myEventNotes,

                        };
                        AddEventToList(GetNewTraceEvent(eventTracerArgs0));
                    }
                    else
                    {
                        EventTracerArgs eventTracerArgs1 = new EventTracerArgs()
                        {
                            EntityId = entityPM.Id,
                            Tenant = entityPM.Tenant,
                            UserId = loggedContact.Id,
                            ObjectTableName = "BankAccount",
                            IsAddedManually = false,
                            EventTypeCode = "ACTV",
                            Notes = myEventNotes,

                        };
                        AddEventToList(GetNewTraceEvent(eventTracerArgs1));
                    }

                }
                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = loggedContact.Id,
                    ObjectTableName = "BankAccount",
                    IsAddedManually = false,
                    EventTypeCode = "CUPD",
                    Notes = myEventNotes,

                };
                AddEventToList(GetNewTraceEvent(eventTracerArgs));
            }
            
            base.Trace(entityPM, entityPOCO, changesXml);
        }

        public virtual void InsertTraceEvents()
        {
            foreach (TraceEventResponse response in TraceEventResponses)
            {
                if (response.TraceEvent != null)
                {
                    IWebFreightContext objectContext = WebFreightContext.GetContext(response.TraceEvent.Tenant);
                    TraceEventRepository traceEventRepository = new TraceEventRepository(objectContext);
                    traceEventRepository.Add(response.TraceEvent);
                    traceEventRepository.SubmitChanges();
                    objectContext.SaveChanges();

                    EventType eventType = GetEventType(response.EventTypeCode, response.TraceEvent.Tenant);
                    if (eventType.IsCustomerView)
                    {
                        ContactsUnseenEntitiesHelper.AddUnseenEntityRecord(response.TraceEvent.Id, response.TraceEvent.Tenant);
                    }
                }

            }
        }

        private string GetOldNewEventNote(string fieldName, string oldValue, string newValue)
        {
            string oldLabel = TranslateTextsClassUtilResolver.Translate("Accounting.General.O.OldValue", 0);//TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0);
            string newLabel = TranslateTextsClassUtilResolver.Translate("Accounting.General.O.NewValue", 0);//TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0);
            string note = string.Format("{0} {1} {2} {3} {4}{5}", fieldName, oldLabel, oldValue ?? "", newLabel, newValue ?? "", Environment.NewLine);
            return note;
        }

        public TraceEventResponse GetNewTraceEvent(EventTracerArgs args)
        {
            TraceEventResponse response = new TraceEventResponse();
            if (!string.IsNullOrEmpty(args.EventTypeCode))
            {
                int tenant = args.Tenant;
                
                EventType eventType;
                eventType = GetEventType(args.EventTypeCode, tenant);

                if (eventType == null)
                {
                    //throw new Exception("Event Type is not recognized:" + args.EventTypeCode);
                    response.ErrorsList.Add("Event Type is not recognized:" + args.EventTypeCode);
                    return response;
                }

                else
                {
                    ObjectTable objectTable = GetObjectTable(args.ObjectTableName, args.Tenant);

                    #region User
                    string myUserId = null;
                    string myCustomerCareUserEmail = null;

                    if (!string.IsNullOrEmpty(args.UserId))
                    {
                        myUserId = args.UserId;

                        if (tenant != 0)
                        {
                            myCustomerCareUserEmail = GetSystemUserCaseCustomerCare(out myUserId, args.UserId, args.Tenant);
                        }
                    }
                    #endregion

                    #region Dates
                    if (args.LogDateTime == null)
                    {
                        args.LogDateTime = DateTimeUtilResolver.GetDateCurrentDateTime(tenant);//TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    else if (args.LogDateTime.Value.Year == 1)
                    {
                        args.LogDateTime = DateTimeUtilResolver.GetDateCurrentDateTime(tenant);//TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    if (args.EventDateTime == null)
                    {
                        args.EventDateTime = DateTimeUtilResolver.GetDateCurrentDateTime(tenant);//TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    else if (args.EventDateTime.Value.Year == 1)
                    {
                        args.EventDateTime = DateTimeUtilResolver.GetDateCurrentDateTime(tenant);//TenantServerConfigration.GetCurrentDateTime(tenant);
                    }
                    #endregion

                    #region Status Notes
                    if (args.IsAddedManually)
                    {
                        if (!string.IsNullOrEmpty(args.NewStatusId) && !string.IsNullOrEmpty(args.CurrentStatusId))
                        {
                            EntityStatus newStatus = GetSingleEntityStatus(args.NewStatusId, tenant); //EntityStatusRepository.GetSingleEntityStatus(args.NewStatusId, tenant, true);
                            EntityStatus currentStatus = GetSingleEntityStatus(args.CurrentStatusId, tenant);//EntityStatusRepository.GetSingleEntityStatus(args.CurrentStatusId, tenant, true);

                            Contact user = GetSingleContactPM(myUserId, tenant);//ContactRepository.GetSingleContact(myUserId, tenant, true);
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
                        myNotes = CheckIfOracleDBSubstring(myNotes);
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
                        ExternalId = args.ExternalId,
                        IsAddedManually = args.IsAddedManually,
                        UserId = myUserId,
                        CustomerCareUserEmail = myCustomerCareUserEmail,
                        Notes = myNotes,
                        Location = null,
                    };

                    response.TraceEvent = myTraceEvent;
                    response.EventTypeCode = eventType.Code;
                  
                }
            }
            return response;
        }

        public virtual string CheckIfOracleDBSubstring(string notes)
        {
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                if (notes.Length > 2000)
                {
                    notes = notes.Substring(0, 1999);
                }
            }

            else
            {
                if (notes.Length > 4000)
                {
                    notes = notes.Substring(0, 3999);
                }
            }
            return notes;
        }

        public virtual EventType GetEventType(string eventTypeCode, int tenant)
        {
            IWebFreightContext objectContext = WebFreightContext.GetContext(tenant);
            EventTypeRepository eventTypeRepository = new EventTypeRepository(objectContext);
            EventType eventType = eventTypeRepository.GetSingleEventTypeByCode(eventTypeCode, tenant);
            return eventType;
        }

        public virtual ObjectTable GetObjectTable(string objectTableName,int tenant)
        {
            IWebFreightContext objectContext = WebFreightContext.GetContext(tenant);
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(objectContext);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
            return objectTable;
        }

        public virtual string GetSystemUserCaseCustomerCare(out string myUserId,string userId,int tenant)
        {
            string myCustomerCareUserEmail = "";
            myUserId = userId;
            if (HttpContext.Current != null && HttpContext.Current.User != null)
            {
                string email = HttpContext.Current.User.Identity.Name;
                if (!string.IsNullOrEmpty(email))
                {
                    UserRepository userRepository = new UserRepository(0);
                    User user = userRepository.GetSingleUserByEmail(email, 0, false);
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
            return myCustomerCareUserEmail;
        }

        public virtual Contact GetSingleContactPM(string userId,int tenant)
        {
            return ContactRepository.GetSingleContact(userId, tenant, true);
        }

        public virtual EntityStatus GetSingleEntityStatus(string statusId,int tenant)
        {
            return EntityStatusRepository.GetSingleEntityStatus(statusId, tenant, true);
        }
    }

    public interface IBankAccountTraceEventService
    {
        void Trace(BankAccountPM entityPM, BankAccount entityPOCO, string changesXml);
    }
}
