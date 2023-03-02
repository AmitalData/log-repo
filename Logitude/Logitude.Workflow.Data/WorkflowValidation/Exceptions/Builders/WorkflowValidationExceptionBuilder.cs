using Logitude.Workflow.Data.WorkflowValidation.Models;

namespace Logitude.Workflow.Data.WorkflowValidation.Exceptions.Builders
{
    public class WorkflowValidationExceptionBuilder
    {
        public static WorkflowValidationResponse BuildException(WorkflowValidationException ex)
        {
            return new WorkflowValidationResponse()
            {
                ErrorType = "WorkflowValidationException",
                ErrorMessages = ex.ErrorMessages
            };
        }
    }
}
