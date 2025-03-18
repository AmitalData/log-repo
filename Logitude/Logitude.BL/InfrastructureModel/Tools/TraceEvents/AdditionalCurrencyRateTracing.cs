using System;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Logitude.BL.InfrastructureModel.Tools.TraceEvents
{
    public class AdditionalCurrencyRateTracing
    {
        private static string objectTableName = "AdditionalCurrencyRate";
        public static void Trace(AdditionalCurrencyRatePM entityPM, AdditionalCurrencyRate poco, bool isNewEntity)
        {
            // protected override void Trace(AdditionalCurrencyRatePM entityPM, AdditionalCurrencyRate entityPOCO, string changesXml)
            ContactPM contactPM = LoggedContactResolver.GetLoggedContact(poco.Tenant);

            string eventNotes = string.Empty;

            if (entityPM.Name != poco.Name)
            {
                eventNotes = eventNotes + PrepareEventNote(TranslateTextsClass.Translate("AdditionalCurrencyRate.F.Name", 0, true), poco.Name, entityPM.Name);
            }
            if (entityPM.RateCoefficient != poco.RateCoefficient)
            {
                eventNotes = eventNotes + PrepareEventNote(TranslateTextsClass.Translate("AdditionalCurrencyRate.F.RateCoefficient", 0, true), poco.RateCoefficient.ToString(), entityPM.RateCoefficient.ToString());
            }

            EventTracerArgs eventTracerArgs = new EventTracerArgs()
            {
                EntityId = entityPM.Id,
                Tenant = entityPM.Tenant,
                UserId = contactPM.Id,
                ObjectTableName = objectTableName,
                IsAddedManually = false,
                Notes = eventNotes,
                EventTypeCode = isNewEntity ? "CREV" : "UPEV",
            };
            EventTracer.CreateTraceEvent(eventTracerArgs);
        }

        private static string PrepareEventNote(string fieldLabel, string oldValue, string newValue)
            => fieldLabel + ": " + TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0, true) + oldValue + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0, true) + newValue + Environment.NewLine;

    }
}
