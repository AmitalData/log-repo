import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";

export class Condition {
    public id: number;
    public isGroup: boolean;
    public groupOperation: string;
    public field: string;
    public fieldCode: string;
    public type: string;
    public value: string;
    public valueCode: string;
    public valueExpression: string;
    public operator: string;
    public conditions: Condition[];
    public isDisabled: boolean;
    public fieldChangedToggle: boolean;

    constructor(isGroup: boolean = false) {
        this.id = null;
        this.isGroup = isGroup;
        this.groupOperation = isGroup ? ConditionOperations.And : null;
        this.field = null;
        this.fieldCode = null;
        this.type = null;
        this.value = null;
        this.valueCode = null;
        this.valueExpression = null;
        this.operator = ConditionOperators.Equals;
        this.conditions = isGroup ? [] : null;
        this.isDisabled = false;
        this.fieldChangedToggle = false;
    }
}