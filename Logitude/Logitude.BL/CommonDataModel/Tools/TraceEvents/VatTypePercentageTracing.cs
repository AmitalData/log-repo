using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class VatTypePercentageTracing
    {
        public static void Trace(VatTypePercentagePM entityPM, VatTypePercentage entityPOCO, bool isNewEntity)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (isNewEntity)
            {

            }

            else
            {
                if (entityPM.Percentage != entityPOCO.Percentage)
                {
                    string fromPercentage = "Null";
                    string toPercentage = "Null";
                    if (entityPOCO.Percentage != null)
                    {
                        fromPercentage = String.Format("{0:N2}", entityPOCO.Percentage);
                    }

                    if (entityPM.Percentage != null)
                    {
                        toPercentage = String.Format("{0:N2}", entityPM.Percentage);
                    }

                    string eventNotes = "Percentage changed from " + fromPercentage + " to " + toPercentage;

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "VTPD",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.VatTypeId,
                        ObjectTableName = "VatType",
                        Notes = eventNotes,
                    });
                }
            }
        }
    }
}
