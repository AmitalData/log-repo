using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.GlobalModel.Tools.TraceEvents
{
    public class HelpResourceTracing
    {
        public static void Trace(HelpResourcePM entityPM, HelpResource poco, bool isNewEntity)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                ContactRepository contactRep = new ContactRepository(0);
                Contact loggedContact = contactRep.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), 0);

                if (loggedContact == null)
                {
                    loggedContact = contactRep.GetSingleContactByEmail("system@tenant0.com", 0);
                }

                if (isNewEntity)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = 0,
                        EventTypeCode = "CREV",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Code,
                        ObjectTableName = "HelpResource",
                    });
                }

                else
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = 0,
                        EventTypeCode = "UPEV",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Code,
                        ObjectTableName = "HelpResource",
                    });
                }

                scope.Complete();
            }
        }
    }
}
