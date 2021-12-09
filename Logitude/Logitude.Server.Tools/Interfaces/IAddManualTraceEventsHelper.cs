using Logitude.Server.Tools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Interfaces
{
    public interface IAddManualTraceEventsHelper
    {
        void Initialize(int tenant);
        NewTraceEventResult Trace(TraceEventsServiceArgs args, string loggedUserEmail);
    }
}
