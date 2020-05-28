 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
using Logitude.Accounting.BL.EntityQueryServices;

namespace Logitude.Accounting.BL.EntityUpdateServices
{ 
   public partial class AccountingNoteUpdateService
   {
        protected override void OnCreating(AccountingNotePM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.Id == null || entityPM.Id == "") entityPM.Id = IdCounterWrapperGetNumber(entityPM.Tenant);


            // users:
            ContactPM user = GetLoggedContact(entityPM.Tenant);
            if (user != null)
            {
                entityPM.CreatedByUserId = user.Id;
                entityPM.CreateDate = GetCurrentDateTime(entityPM.Tenant);
            }

        }

        protected override void OnUpdating(AccountingNotePM entityPM, AccountingNote entityPOCO)
        {
            ContactPM user = GetLoggedContact(entityPM.Tenant);
            if (user != null)
            {
                entityPM.UpdatedByUserId = user.Id;
                entityPM.UpdateDate = GetCurrentDateTime(entityPM.Tenant);
            }


        }

        protected override void Trace(AccountingNotePM entityPM, AccountingNote entityPOCO, string changesXml)
        {
            CreateGLAccountEvents(entityPM, entityPOCO);
        }

        private void CreateGLAccountEvents(AccountingNotePM entityPM, AccountingNote entityPOCO)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                EventTracerArgs newNoteEvent = GetNewEvent(entityPM);
                EventTracer.CreateTraceEvent(newNoteEvent);
            }
            else if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                EventTracerArgs updatedEvent = GetUpdateEvent(entityPM, entityPOCO);
                EventTracer.CreateTraceEvent(updatedEvent);
            }
            else if (entityPM.ChangeSetOp == ChangeSetOperation.Delete)
            {
                EventTracerArgs deletedEvent = GetDeletedEvent(entityPM, entityPOCO);
                EventTracer.CreateTraceEvent(deletedEvent);
            }
        }

        private EventTracerArgs GetDeletedEvent(AccountingNotePM entityPM, AccountingNote entityPOCO)
        {
            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(entityPM.Tenant);
            GLAccountPM glaccount = GetGLAccount(entityPM.CardId, entityPM.Tenant);
            
            string notes = entityPOCO.Notes;
            EventTracerArgs newNoteEvent = new EventTracerArgs()
            {
                EventTypeCode = "NTDL",
                EntityId = glaccount.Id,
                Tenant = entityPM.Tenant,
                UserId = loggedContact.Id,
                ObjectTableName = "GLAccount",
                IsAddedManually = false,
                Notes = notes

            };
            return newNoteEvent;
        }

        private EventTracerArgs GetUpdateEvent(AccountingNotePM entityPM, AccountingNote entityPOCO)
        {
            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(entityPM.Tenant);
            GLAccountPM glaccount = GetGLAccount(entityPM.CardId, entityPM.Tenant);

            string OLD_VALUE = TextCodesTranslator.TranslateText("Accounting.General.O.OldValue", 0, LoggedContactResolver.GetLoggedContactShowLocal(entityPM.Tenant));
            string NEW_VALUE = TextCodesTranslator.TranslateText("Accounting.General.O.NewValue", 0, LoggedContactResolver.GetLoggedContactShowLocal(entityPM.Tenant));

            string notes = OLD_VALUE + entityPOCO.Notes + NEW_VALUE + entityPM.Notes;
            EventTracerArgs newNoteEvent = new EventTracerArgs()
            {
                EventTypeCode = "NTUP",
                EntityId = glaccount.Id,
                Tenant = entityPM.Tenant,
                UserId = loggedContact.Id,
                ObjectTableName = "GLAccount",
                IsAddedManually = false,
                Notes = notes

            };
            return newNoteEvent;
        }

        private EventTracerArgs GetNewEvent(AccountingNotePM entityPM)
        {
            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(entityPM.Tenant);
            GLAccountPM glaccount = GetGLAccount(entityPM.CardId, entityPM.Tenant);

            string notes = entityPM.Notes;

            return new EventTracerArgs()
            {
                EventTypeCode = "NTAD",
                EntityId = glaccount.Id,
                Tenant = entityPM.Tenant,
                UserId = loggedContact.Id,
                ObjectTableName = "GLAccount",
                IsAddedManually = false,
                Notes = notes,

            };
        }

        private GLAccountPM GetGLAccount(string cardId, int tenant)
        {
            GLAccountQueryService glaQuery = new GLAccountQueryService(tenant);
            GLAccountPM glaccount = glaQuery.GetGLAccountByCardId(cardId, tenant);
            return glaccount;
        }

        // methods
        public virtual DateTime GetCurrentDateTime(int tenant)
        {
            return TenantServerConfigration.GetCurrentDateTime(tenant);
        }


        public virtual Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }


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



        public virtual string IdCounterWrapperGetNumber(int Tenant)
        {
            return (new IdCounterWrapper()).GetNumber(
                    "AccountingNote", Tenant);
        }

    }
}
	 