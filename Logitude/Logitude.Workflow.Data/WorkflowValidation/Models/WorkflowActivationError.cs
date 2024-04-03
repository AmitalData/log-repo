using System.Collections.Generic;

namespace Logitude.Workflow.Data.WorkflowValidation.Models
{
    public class WorkflowActivationError
    {
        public string NodeType { get; set; }
        public string NodeName { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
