using System;
using System.Linq;
using System.Text;
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
        private static string objectTableName = nameof(AdditionalCurrencyRate);
        public static void Trace(AdditionalCurrencyRatePM entityPM, AdditionalCurrencyRate poco, bool isNewEntity)
        {
            ContactPM contactPM = LoggedContactResolver.GetLoggedContact(poco.Tenant);

            StringBuilder eventNotesBuilder = new StringBuilder();

            if (entityPM.Name != poco.Name)
            {
                eventNotesBuilder.Append(PrepareEventNote(TranslateTextsClass.Translate("AdditionalCurrencyRate.F.Name", 0, true), poco.Name, entityPM.Name));
            }
            if (entityPM.RateCoefficient != poco.RateCoefficient)
            {
                eventNotesBuilder.Append(PrepareEventNote(TranslateTextsClass.Translate("AdditionalCurrencyRate.F.RateCoefficient", 0, true), poco.RateCoefficient.ToString(), entityPM.RateCoefficient.ToString()));
            }

            EventTracerArgs eventTracerArgs = new EventTracerArgs()
            {
                EntityId = entityPM.Id,
                Tenant = entityPM.Tenant,
                UserId = contactPM?.Id,
                ObjectTableName = objectTableName,
                IsAddedManually = false,
                Notes = eventNotesBuilder.ToString(),
                EventTypeCode = isNewEntity ? "CREV" : "UPEV",
            };
            EventTracer.CreateTraceEvent(eventTracerArgs);
        }

        private static string PrepareEventNote(string fieldLabel, string oldValue, string newValue)
            => $"{fieldLabel}: {TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0, true)} {oldValue} {TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0, true) + newValue + Environment.NewLine}";

    }
}
