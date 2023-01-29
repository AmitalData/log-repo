import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";
import { ConditionDisabled } from "Workflow/Types";

export class Condition {
    public id: number;
    public isGroup: boolean;
    public groupOperation: string;
    public field: string;
    public fieldCode: string;
    public type: string;
    public lookupType: string;
    public picklistType: string;
    public value: string;
    public valueCode: string;
    public valueExpression: string;
    public operator: string;
    public conditions: Condition[];
    public disabled: ConditionDisabled;
    public fieldChangedToggle: boolean;
    public fieldUsedFrom: string | null;
    public valueUsedFrom: string | null;

    constructor(isGroup: boolean = false) {
        this.id = null;
        this.isGroup = isGroup;
        this.groupOperation = isGroup ? ConditionOperations.And : null;
        this.field = null;
        this.fieldCode = null;
        this.type = null;
        this.lookupType = null;
        this.picklistType = null;
        this.value = null;
        this.valueCode = null;
        this.valueExpression = null;
        this.operator = ConditionOperators.Equals;
        this.conditions = isGroup ? [] : null;
        this.disabled = null;
        this.fieldChangedToggle = false;
        this.fieldUsedFrom = null;
        this.valueUsedFrom = null;
    }
}