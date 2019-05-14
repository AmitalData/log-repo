using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.GlobalModel.Tools.TraceEvents
{
    public class BluesnapContractTracing
    {
        public static void Trace(BluesnapContractPM entityPM, BluesnapContract poco, bool isNewEntity)
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
                        EventTypeCode = "CRBC",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "BluesnapContract",
                    });
                }

                else
                {    
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = 0,
                        EventTypeCode = "UPBC",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "BluesnapContract",
                    });
                }

                scope.Complete();
            }            
        }
    }
}