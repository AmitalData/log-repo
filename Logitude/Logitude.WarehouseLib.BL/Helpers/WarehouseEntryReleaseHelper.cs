using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.WarehouseLib.BL.Helpers
{
   public class WarehouseEntryReleaseHelper
    {
        public void AddTraceEvents(List<string> eventCodeList, EventTracerArgs eventTracerArgs)
        {
            if (eventCodeList != null && eventCodeList.Count > 0)
            {
                foreach (string eventCode in eventCodeList)
                {
                    eventTracerArgs.EventTypeCode = eventCode;
                    EventTracer.CreateTraceEvent(eventTracerArgs);
                }
            }

        }
    }
}
