using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools
{
    public abstract class EntityTraceEventService<TEntityPoco,TEntityPM>
    {
        public List<TraceEventResponse> TraceEventResponses;
        public EntityTraceEventService()
        {
            TraceEventResponses = new List<TraceEventResponse>();
        }

        public virtual void Trace(TEntityPM entityPM,TEntityPoco entityPOCO, string changesXml) { }

        public void AddEventToList(TraceEventResponse traceEventResponse)
        {
            this.TraceEventResponses.Add(traceEventResponse);
        }
    }
}
