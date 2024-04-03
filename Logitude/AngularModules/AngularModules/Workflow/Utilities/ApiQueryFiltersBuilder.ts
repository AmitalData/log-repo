import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";

export class ApiQueryFiltersBuilder {

    static getObjectTableFilters(name: string | null, getAll: boolean = false, isCustom: boolean | null = null) {
        let apiQueryFilters = new ApiQueryFilters(getAll);

        if (name) {
            apiQueryFilters.addAdditionalFilter("Name", name, null, null, this.getOperator(name), false, false, false, "Text");
        }

        if (isCustom !== null) {
            apiQueryFilters.addAdditionalFilter("IsCustom", isCustom, null, null, "Equals", false, false, false, "Boolean");
        }

        return apiQueryFilters;
    }

    static getObjectFieldFilters(objectTableId: string | null, dataTypeCode: string | null, lookupTableId: string | null, getAll: boolean = false, isFullCustom: boolean | null = null) {
        let apiQueryFilters = new ApiQueryFilters(getAll);

        if (objectTableId) {
            apiQueryFilters.addAdditionalFilter("ObjectTableId", objectTableId, null, null, this.getOperator(objectTableId), false, false, false, "Text");
        }

        if (dataTypeCode) {
            apiQueryFilters.addAdditionalFilter("DataTypeCode", dataTypeCode, null, null, this.getOperator(dataTypeCode), false, false, false, "Text");
        }

        if (lookupTableId) {
            apiQueryFilters.addAdditionalFilter("LookUpTableId", lookupTableId, null, null, this.getOperator(lookupTableId), false, false, false, "Text");
        }

        if (isFullCustom !== null) {
            apiQueryFilters.addAdditionalFilter("IsFullCustom", isFullCustom, null, null, "Equals", true, false, false, "Boolean");
        }

        apiQueryFilters.addAdditionalFilter("IncludeMetaDataFields", true, null, null, "Equals", true, false, false, "Boolean");

        return apiQueryFilters;
    }

    static getWorkflowInstanceFilters(workflowVersionId: string | null, businessKey: string | null, getAll: boolean = false) {
        let apiQueryFilters = new ApiQueryFilters(getAll);

        if (workflowVersionId) {
            apiQueryFilters.addAdditionalFilter("WorkFlowVersionId", workflowVersionId, null, null, this.getOperator(workflowVersionId), false, false, false, "Text");
        }

        if (businessKey) {
            apiQueryFilters.addAdditionalFilter("BusinessKey", businessKey, null, null, "Contains", false, false, false, "Text");
        }

        return apiQueryFilters;
    }

    static getWorkflowInstanceActivityOrVariableFilters(workflowInstanceId: string | null, getAll: boolean = false) {
        let apiQueryFilters = new ApiQueryFilters(getAll);

        if (workflowInstanceId) {
            apiQueryFilters.addAdditionalFilter("WorkflowInstanceId", workflowInstanceId, null, null, this.getOperator(workflowInstanceId), false, false, false, "Text");
        }

        return apiQueryFilters;
    }

    static getWorkflowVersionFilters(workflowId: string | null, getAll: boolean = false) {
        let apiQueryFilters = new ApiQueryFilters(getAll);

        if (workflowId) {
            apiQueryFilters.addAdditionalFilter("WorkflowId", workflowId, null, null, this.getOperator(workflowId), false, false, false, "Text");
        }

        return apiQueryFilters;
    }

    private static getOperator(value: string) {
        return value.indexOf(",") === -1 ? "Equals" : "InListExact";
    }

}