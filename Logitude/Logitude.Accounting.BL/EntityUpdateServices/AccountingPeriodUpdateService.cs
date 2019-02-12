using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.Web;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
//using Logitude.BL.CommonDataModel.EntityPMs;
//using Logitude.BL.CommonDataModel.EntityQueries;
//using Logitude.BL.Security;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class AccountingPeriodUpdateService : EntityUpdateService<AccountingPeriod, AccountingPeriodPM, EntityPM>
    {
        protected override void OnCreating(AccountingPeriodPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.Id == null || entityPM.Id == "") entityPM.Id = IdCounter.GetNumber("AccountingPeriod", entityPM.Tenant);
           // if (entityPM.AccountTypeCode == null || entityPM.AccountTypeCode == "") entityPM.AccountTypeCode = "1";
        }

        protected override void OnUpdating(AccountingPeriodPM entityPM, AccountingPeriod entityPOCO)
        {
            if(entityPM.ClosedMonth > entityPOCO.ClosedMonth) // Closed Month incremented
            {
                //check
                JournalRepository repo = new JournalRepository(entityPM.Tenant);
                bool exist = repo.CheckIfThereNonTranslatedJournalsByMonth(entityPM.Year, (entityPM.ClosedMonth == null ? 0 : entityPM.ClosedMonth.Value), entityPM.Tenant);

                if (exist)
                {
                    throw new ApplicationException(TextCodesTranslator.TranslateText("AccountingPeriods.O.therearejournalsdidnottranslated", entityPM.Tenant));
                }
            }
        }


        protected override void Trace(AccountingPeriodPM entityPM, AccountingPeriod entityPOCO, string changesXml)
        {
            ContactPM contactPM = GetLoggedContact(entityPOCO.Tenant);
            bool showLocals = true;
            if (contactPM != null)
                showLocals = !contactPM.DontShowLocal;

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                //create trace event with created type.
                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);

                 ContactPM loggedContact = GetLoggedContact(entityPM.Tenant);

                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = contact.Id,
                    ObjectTableName = "AccountingPeriod",
                    IsAddedManually = false,
                    EventTypeCode = "PCR",
                };
                EventTracer.CreateTraceEvent(eventTracerArgs);
            }
            else if (entityPM.ChangeSetOp == ChangeSetOperation.Delete)
            {
                //create trace event with created type.
                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);

                

                // ContactPM loggedContact = LoggedContact(entityPM.Tenant);


                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = contact.Id,
                    ObjectTableName = "AccountingPeriod",
                    IsAddedManually = false,
                    EventTypeCode = "PDL",
                };
                EventTracer.CreateTraceEvent(eventTracerArgs);
            }
            else if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                //create trace event with updated type.
                //if (entityPM.ClosedMonth != entityPOCO.ClosedMonth)
                //{
                //    ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                //    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                //    Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                //    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                //    String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.ClosedMonth.ToString() + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.ClosedMonth.ToString();
                //    EventTracerArgs eventTracerArgs = new EventTracerArgs()
                //    {
                //        EntityId = entityPM.Id,
                //        Tenant = entityPM.Tenant,
                //        UserId = contact.Id,
                //        ObjectTableName = "AccountingPeriod",
                //        IsAddedManually = false,
                //        EventTypeCode = "PUPD",
                //        Notes = notes,
                //    };

                //    EventTracer.CreateTraceEvent(eventTracerArgs);
                //}
                if (entityPM.OpenMonth > entityPOCO.OpenMonth)
                {
                    ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                    Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                    String notes = showLocals ? ("נפתח חודש" + entityPM.OpenMonth) : ("Month " + entityPM.OpenMonth + " was opened");

                    EventTracerArgs eventTracerArgs = new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "AccountingPeriod",
                        IsAddedManually = false,
                        EventTypeCode = "OPEN",
                        Notes = notes,
                    };
                    EventTracer.CreateTraceEvent(eventTracerArgs);
                }
                if (entityPM.OpenMonth < entityPOCO.OpenMonth)
                {
                    ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                    Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                    String notes = showLocals ? ("ביטול פתיחת חודש" + entityPM.OpenMonth) : ("Open Month " + entityPM.OpenMonth + " was canceled");
                    EventTracerArgs eventTracerArgs = new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "AccountingPeriod",
                        IsAddedManually = false,
                        EventTypeCode = "OPCN",
                        Notes = notes,
                    };
                    EventTracer.CreateTraceEvent(eventTracerArgs);
                }
                if (entityPM.ClosedMonth > entityPOCO.ClosedMonth || (entityPM.ClosedMonth > 0 && entityPOCO.ClosedMonth == null))
                {
                    ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                    Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                    String notes = showLocals ? ("נסגר חודש" + entityPM.ClosedMonth) : ("Month " + entityPM.ClosedMonth + " was closed");

                    EventTracerArgs eventTracerArgs = new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "AccountingPeriod",
                        IsAddedManually = false,
                        EventTypeCode = "CLOS",
                        Notes = notes,
                    };
                    EventTracer.CreateTraceEvent(eventTracerArgs);
                }
            }
            base.Trace(entityPM, entityPOCO, changesXml);
        }


        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }




        public virtual ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


        public void CreateDefaultPeriodsForYear(int year, int tenant)
        {
            PeriodTypeRepository typesRepo = new PeriodTypeRepository(tenant);
            List<PeriodType> typesList = typesRepo.All();

            foreach (PeriodType type in typesList)
            {
                AccountingPeriodPM period = new AccountingPeriodPM()
                {
                    Tenant = tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    PeriodTypeCode = type.Code,
                    Year = year,
                    OpenMonth = 1,
                    ClosedMonth = null,
                };
                Update(period, true);
            }
        }

        protected override void Validate(AccountingPeriodPM entityPM)
        {
            ValidationResult result = AccountingPeriodValidator.IsAccountingPeriodValid(entityPM);
            if (result != null)
            {
                throw new ApplicationException(result.ErrorMessage);
            }
            base.Validate(entityPM);
        }

    }
}
