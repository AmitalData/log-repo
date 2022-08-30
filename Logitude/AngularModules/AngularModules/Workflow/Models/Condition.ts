import { ConditionGroupOperations } from "Workflow/Constants/ConditionGroupOperations";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";

export class Condition {
    public isGroup: boolean;
    public groupOperation: string;
    public fieldId: string;
    public field: string;
    public type: string;
    public value: string;
    public operator: string;
    public conditions: Condition[];

    constructor(isGroup: boolean = false) {
        this.isGroup = isGroup;
        this.groupOperation = isGroup ? ConditionGroupOperations.And : null;
        this.fieldId = null;
        this.field = null;
        this.type = null;
        this.value = null;
        this.operator = ConditionOperators.Equals;
        this.conditions = isGroup ? [] : null;
    }
}