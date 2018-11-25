using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityPOCOs;
//using Logitude.BL.CommonDataModel.EntityPMs;
//using Logitude.BL.CommonDataModel.EntityQueries;
//using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class BankAccountUpdateService : EntityUpdateService<BankAccount, BankAccountPM, EntityPM>
    {
        protected override void OnCreating(BankAccountPM entityPM, EntityPM entityParentPM)
        {
            BankAccountOnCreatingService bankAccountOnCreatingService = new BankAccountOnCreatingService(MainContext as IAccountingContext);
            bankAccountOnCreatingService.OnCreating(entityPM);
        }

        protected override void OnUpdating(BankAccountPM entityPM, BankAccount entityPOCO)
        {
            BankAccountOnUpdatingService bankAccountOnUpdatingUpdateService = new BankAccountOnUpdatingService(MainContext as IAccountingContext);
            bankAccountOnUpdatingUpdateService.OnUpdating(entityPM, entityPOCO);
            base.OnUpdating(entityPM, entityPOCO);
        }

        protected override void Trace(BankAccountPM entityPM, BankAccount entityPOCO, string changesXml)
        {
            ContactPM loggedContact = GetLoggedContact(entityPM.Tenant);
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
                EventTracer.CreateTraceEvent(eventTracerArgs);
            }
            else if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                //create trace event with updated type.
                string myEventNotes = "";

                if (entityPM.LocalName != entityPOCO.LocalName && (!String.IsNullOrEmpty(entityPM.LocalName) || !String.IsNullOrEmpty(entityPOCO.LocalName)))
                {
                    //myEventNotes = "Previous Local Name: " + (entityPOCO.LocalName == null ? "" : entityPOCO.LocalName);
                    //myEventNotes += " Local Name Changed" + TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.LocalName 
                    //                              + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.LocalName + ". ";
                    myEventNotes += GetOldNewEventNote("Local Name", entityPOCO.LocalName, EntityPM.LocalName);
                    
                }
                if (entityPM.EnglishName != entityPOCO.EnglishName && (!String.IsNullOrEmpty(entityPM.EnglishName) || !String.IsNullOrEmpty(entityPOCO.EnglishName)))
                {
                    //myEventNotes += " English Name Changed" + TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.EnglishName
                    //                              + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.EnglishName + ". ";
                    myEventNotes += GetOldNewEventNote("English Name", entityPOCO.EnglishName, EntityPM.EnglishName);

                }

                if (entityPM.BranchNumber != entityPOCO.BranchNumber && (!String.IsNullOrEmpty(entityPM.BranchNumber) || !String.IsNullOrEmpty(entityPOCO.BranchNumber)))
                {
                    myEventNotes += GetOldNewEventNote("Branch Number", entityPOCO.BranchNumber, EntityPM.BranchNumber);

                }

                if (entityPM.AccountNumber != entityPOCO.AccountNumber && (!String.IsNullOrEmpty(entityPM.AccountNumber) || !String.IsNullOrEmpty(entityPOCO.AccountNumber)))
                {
                    myEventNotes += GetOldNewEventNote("Account Number", entityPOCO.AccountNumber, EntityPM.AccountNumber);

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
                        EventTracer.CreateTraceEvent(eventTracerArgs0);
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
                        EventTracer.CreateTraceEvent(eventTracerArgs1);
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
                EventTracer.CreateTraceEvent(eventTracerArgs);
            }
            base.Trace(entityPM, entityPOCO, changesXml);
        }

        protected override void Validate(BankAccountPM entityPM)
        {
            BankAccountValidateService validateService = new BankAccountValidateService(MainContext as IAccountingContext);
            validateService.Validate(entityPM);
            base.Validate(entityPM);
        }

        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }

        private static ContactPM GetLoggedContact(int tenant)
        {

            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedContact = new ContactQuery(tenant).GetContactByEmailOnly(
                //SecurityUtility.GetAuthenticatedUser()
                AuthenticationUtil.ResolveUserIdentityName(tenant)
                , tenant);
            if (loggedContact == null)
            {
                loggedContact = new ContactQuery(tenant).GetContactByEmailOnly("system@tenant" + tenant + ".com", tenant);
            }
            loggedContact = loggedContact ?? new Logitude.BL.CommonDataModel.EntityPMs.ContactPM() { DontShowLocal = true };
            return loggedContact;
        }

        private static string GetOldNewEventNote(string fieldName, string oldValue, string newValue)
        {
            string oldLabel = TranslateTextsClass.Translate("Accounting.General.O.OldValue",0);
            string newLabel = TranslateTextsClass.Translate("Accounting.General.O.NewValue",0);
            string note = string.Format("{0} {1} {2} {3} {4}{5}",fieldName, oldLabel,oldValue??"", newLabel,newValue ?? "",Environment.NewLine);
            return note;
        }
    }

}
