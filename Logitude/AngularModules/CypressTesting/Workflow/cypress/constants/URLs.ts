export class URLs {
    public static readonly GetWorkflowViews = '**/workflowviews/**';
    public static readonly GetWorkflowFlowBuilder = '**/workflows/getsingle?**';
    public static readonly WorkflowVersionRequest = '**/workflowversions';
    public static readonly WorkflowRequest = '**/workflows';
    public static readonly GetNewWorkflow = '**/EntityResource?objectTableName=PartnerType&tenant=**';
    public static readonly GetObjectFieldViews = '**/objectfieldviews/getbyfilters?**';
    public static readonly GetBackToWorkflowsList = '**/workflowviews/getsingle/**';
    public static readonly GetQueryExportExecution = '**/WebFreightDomain/getquerytoexceldata?**';
    public static readonly Getworkflowinstance = '**/workflowinstanceviews/getbyfilters?**';
    public static readonly GetSingleInstanceActivityList = '**/workflowinstanceextended/getactivities?workflowinstanceid=**';
    public static readonly GetQueryToExcelData = '**/WebFreightDomain/getquerytoexceldata?**';
    public static readonly GetSingleInstanceVariables = '**/workflowinstanceextended/getvariables?workflowinstanceid=**';

}