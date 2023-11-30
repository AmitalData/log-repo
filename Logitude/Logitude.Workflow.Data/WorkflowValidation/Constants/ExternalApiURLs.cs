namespace Logitude.Workflow.Data.WorkflowValidation.Constants
{
    public static class ExternalApiURLs
    {
        public static string GetWorkFlowVersionActivationValidation(string workflowVersionId) 
        {
            return $"WorkFlowVersion/GetActivationValidation/{workflowVersionId}";
        }
    }
}
