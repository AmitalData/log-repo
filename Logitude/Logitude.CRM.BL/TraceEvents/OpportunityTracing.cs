using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.CRM.BL.TraceEvents
{
    public class OpportunityTracing
    {
        public static void Trace(OpportunityPM entityPM, Opportunity entityPoco, bool isNewEntity)
        {
            //EventTracer.CreateTraceEvent();
            //WebFreightDomainService domainService = new WebFreightDomainService();
            //ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);
        }
    }
}
