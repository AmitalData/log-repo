using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class InterestBasesTypeUpdateService
    {
        protected override void OnCreating(InterestBasesTypePM entityPM, EntityPM entityParentPM)
        {

   
        }

        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }

        protected override void OnUpdating(InterestBasesTypePM entityPM, InterestBasesType entityPOCO)
        {
            ContactPM contact = GetLoggedContact(entityPM.Tenant);
            bool showLocals = !contact.DontShowLocal;
            if (entityPM.Code != entityPOCO.Code)
            {
                InterestBasesTypeRepository PeriodRepository = new InterestBasesTypeRepository(entityPM.Tenant);
                InterestBasesType Period = PeriodRepository.GetSingleByCode(entityPM.Code, entityPM.Tenant);
                if (Period != null)
                {
                    throw new Exception(TextCodesTranslator.TranslateText("Accounting.General.O.Abasetypewiththesamecodeexists", entityPM.Tenant, showLocals));
                }
            }
        }

        protected override void UpdateComposition(InterestBasesTypePM entityPM)
        {
            InterestBasesPeriodUpdateService mementoLineUpdateService = new InterestBasesPeriodUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            mementoLineUpdateService.UpdateMulti(entityPM.InterestBasesPeriods, entityPM.DeletedInterestBasesPeriods, entityPM, false);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Update && entityPM.InterestBasesPeriods.Count > 0)
            {

                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                foreach (var line in entityPM.InterestBasesPeriods)
                {
                    if (line.ChangeSetOp == ChangeSetOperation.Insert)
                    {
                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                EntityId = entityPM.Id,
                                Tenant = entityPM.Tenant,
                                UserId = contact.Id,
                                ObjectTableName = "InterestBasesType",
                                IsAddedManually = false,
                                EventTypeCode = "PCEV",
                            });
                    }

                    if (line.ChangeSetOp == ChangeSetOperation.Update)
                    {
                        InterestBasesPeriodRepository PeriodRepository = new InterestBasesPeriodRepository(entityPM.Tenant);
                        InterestBasesPeriod Period = PeriodRepository.GetSingle(line.InterestBaseTypeId,line.LineNumber, entityPM.Tenant);

                        if (Period.InterestBaseStartDate != line.InterestBaseStartDate || Period.InterestRate != line.InterestRate)
                        {
                            String notes = "";
                            if (Period.InterestBaseStartDate != line.InterestBaseStartDate)
                                notes += TranslateTextsClass.Translate("InterestBasesPeriod.F.InterestBaseStartDate", 0) + ", " + TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + Period.InterestBaseStartDate + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + line.InterestBaseStartDate + "\n";

                            if (Period.InterestRate != line.InterestRate)
                                notes += TranslateTextsClass.Translate("InterestBasesPeriod.F.InterestRate", 0) + ", " + TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + Period.InterestRate + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + line.InterestRate + "\n";

                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                EntityId = entityPM.Id,
                                Tenant = entityPM.Tenant,
                                UserId = contact.Id,
                                ObjectTableName = "InterestBasesType",
                                IsAddedManually = false,
                                EventTypeCode = "PUEV",
                                Notes = notes
                            });
                        }
                    }
                }
            }
        }

        protected override void Trace(InterestBasesTypePM entityPM, InterestBasesType entityPOCO, string changesXml)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = contact.Id,
                    ObjectTableName = "InterestBasesType",
                    IsAddedManually = false,
                    EventTypeCode = "CREV",

                });
            }

            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {

                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);

                if (entityPM.LocalName != entityPOCO.LocalName || entityPM.EnglishName != entityPOCO.EnglishName || entityPM.Description != entityPOCO.Description)
                {
                    String notes = "";
                    if (entityPM.LocalName!= entityPOCO.LocalName)
                        notes += TranslateTextsClass.Translate("InterestBasesType.F.LocalName", 0)+", "+TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.LocalName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.LocalName +"\n";

                    if (entityPM.EnglishName != entityPOCO.EnglishName)
                        notes += TranslateTextsClass.Translate("InterestBasesType.F.EnglishName", 0) + ", " + TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.EnglishName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.EnglishName + "\n";

                    if (entityPM.Description != entityPOCO.Description)
                        notes += TranslateTextsClass.Translate("InterestBasesType.F.Description", 0) + ", " + TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.Description + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.Description + "\n";

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                      EntityId = entityPM.Id,
                      Tenant = entityPM.Tenant,
                      UserId = contact.Id,
                      ObjectTableName = "InterestBasesType",
                      IsAddedManually = false,
                      EventTypeCode = "UPEV",
                      Notes = notes
                    });
                }

                if (entityPM.InActive != entityPOCO.InActive)
                {
                    String notes = "Record was set to Inactive";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "InterestBasesType",
                        IsAddedManually = false,
                        EventTypeCode = "INEV",
                        Notes = notes
                    });
                }
            }
        }
     }
}
