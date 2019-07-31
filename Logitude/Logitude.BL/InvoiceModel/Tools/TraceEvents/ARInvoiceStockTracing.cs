using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.TraceEvents
{
    public class ARInvoiceStockTracing
    {
        public static void Trace(ARInvoiceStockPM entityPM, ARInvoiceStock poco, bool isNewEntity, string loggedContactId)
        {
            if (isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "ARInvoiceStock",
                });
            }

            else
            {               
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "ARInvoiceStock",
                });
            }

            if(entityPM.Cancelled)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CANC",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "ARInvoiceStock",
                });
            }

            if (entityPM.Reactivated)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "REAC",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "ARInvoiceStock",
                });
            }

            if (entityPM.NumbersAdded)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "INND",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "ARInvoiceStock",
                    Notes = entityPM.EventNotes,
                });
            }

            if (entityPM.NumberRemoved)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "INNR",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "ARInvoiceStock",
                    Notes = entityPM.EventNotes,
                });
            }

            if (entityPM.SeriesRemoved)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "INSR",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "ARInvoiceStock",
                    Notes = entityPM.EventNotes,
                });
            }
        }
    }
}
