using System;
using System.Collections.Generic;

namespace Logitude.Workflow.Data.WorkflowValidation.Exceptions
{
    public class WorkflowValidationException : Exception
    {
        public List<string> ErrorMessages { set; get; }

        public WorkflowValidationException(List<string> errorMessages)
        {
            ErrorMessages = errorMessages;
        }

        public WorkflowValidationException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
