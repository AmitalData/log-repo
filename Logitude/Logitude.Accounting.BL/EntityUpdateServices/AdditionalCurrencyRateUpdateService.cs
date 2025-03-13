using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.BL.Validators;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class AdditionalCurrencyRateUpdateService : EntityUpdateService<AdditionalCurrencyRate, AdditionalCurrencyRatePM, EntityPM>
    {
        private string objectTableName = "AdditionalCurrencyRate";

        protected override void Trace(AdditionalCurrencyRatePM entityPM, AdditionalCurrencyRate entityPOCO, string changesXml)
        {
            ContactPM contactPM = LoggedContactResolver.GetLoggedContact(entityPOCO.Tenant);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert || entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                string eventNotes = string.Empty;

                if (entityPM.Name != EntityPOCO.Name)
                {
                    eventNotes = eventNotes + PrepareEventNote(TranslateTextsClass.Translate("AdditionalCurrencyRate.F.Name", 0, true), EntityPOCO.Name, entityPM.Name);
                }
                if (entityPM.Rate != EntityPOCO.Rate)
                {
                    eventNotes = eventNotes + PrepareEventNote(TranslateTextsClass.Translate("AdditionalCurrencyRate.F.Rate", 0, true), EntityPOCO.Rate.ToString(), entityPM.Rate.ToString());
                }

                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = contactPM.Id,
                    ObjectTableName = objectTableName,
                    IsAddedManually = false,
                    Notes = eventNotes,
                    EventTypeCode = entityPM.ChangeSetOp == ChangeSetOperation.Insert? "CREV": "UPEV",
                };
                EventTracer.CreateTraceEvent(eventTracerArgs);
            }
            base.Trace(entityPM, entityPOCO, changesXml);
        }

        protected override void Validate(AdditionalCurrencyRatePM entityPM)
        {
            ValidationResult result = AdditionalCurrencyRateValidator.IsRateValid(entityPM);
            if (result != null)
            {
                throw new ApplicationException(result.ErrorMessage);
            }
            base.Validate(entityPM);
        }

        private string PrepareEventNote(string fieldLabel, string oldValue, string newValue)
            => fieldLabel + ": " + TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0, true) + oldValue + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0, true) + newValue + Environment.NewLine;
    }
}
