using System.Collections.Generic;

namespace Logitude.Workflow.Data.WorkflowValidation.Models
{
    public class WorkflowValidationResponse
    {
        public string ErrorType { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
