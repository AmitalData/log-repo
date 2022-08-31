import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";

export class Condition {
    public isGroup: boolean;
    public groupOperation: string;
    public field: string;
    public fieldCode: string;
    public type: string;
    public value: string;
    public operator: string;
    public conditions: Condition[];
    public fieldChangedToggle: boolean;

    constructor(isGroup: boolean = false) {
        this.isGroup = isGroup;
        this.groupOperation = isGroup ? ConditionOperations.And : null;
        this.field = null;
        this.fieldCode = null;
        this.type = null;
        this.value = null;
        this.operator = ConditionOperators.Equals;
        this.conditions = isGroup ? [] : null;
        this.fieldChangedToggle = false;
    }
}