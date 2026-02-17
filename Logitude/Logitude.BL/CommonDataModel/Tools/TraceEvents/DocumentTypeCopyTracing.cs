using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class DocumentTypeCopyTracing
    {
        public static void Trace(DocumentTypeCopyPM entityPM, DocumentTypeCopy poco, bool isNewEntity)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);
            // commented the tracing here because there is no update or create events for now ----Mohammad
            if (isNewEntity)
            {
                //EventTracer.CreateTraceEvent(new TraceEvent(), "CRDT", entityPM.Tenant, loggedContact.Id, entityPM.Id, null, "DocumentTypeCopy", null, null, false);
            }

            else
            {
                //EventTracer.CreateTraceEvent(new TraceEvent(), "UPDT", entityPM.Tenant, loggedContact.Id, entityPM.Id, null, "DocumentTypeCopy", null, null, false);
            }
        }

    }
}
