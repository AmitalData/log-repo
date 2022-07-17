using System.Collections.Generic;

namespace Logitude.Server.Tools.CToolWorkflows.Models
{
    public class CToolWorkflowMessage
    {
        public object Entity { get; set; }
        public List<PropertyChange> Changes { get; set; }
    }
}
