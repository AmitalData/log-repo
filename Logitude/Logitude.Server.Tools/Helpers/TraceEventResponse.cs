using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
    public class TraceEventResponse
    {
        public TraceEvent TraceEvent { get; set; }
        public string EventTypeCode { get; set; }
        public List<string> ErrorsList { get; set; }
    }
}
