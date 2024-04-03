import { ExpressionValue, SetValueDisabled } from "Workflow/Types";

export class SetValue {
    public id: number;
    public field: string;
    public fieldCode: string;
    public type: string;
    public lookupType: string;
    public picklistType: string;
    public operator: string;
    public value: string;
    public expressionValue: ExpressionValue;
    public disabled: SetValueDisabled;
    public fieldChangedToggle: boolean;
    public fieldUsedFrom: string | null;
    public valueUsedFrom: string | null;

    constructor() {
        this.id = null;
        this.field = null;
        this.fieldCode = null;
        this.type = null;
        this.lookupType = null;
        this.picklistType = null;
        this.operator = null;
        this.value = null;
        this.expressionValue = null;
        this.disabled = null;
        this.fieldChangedToggle = false;
        this.fieldUsedFrom = null;
        this.valueUsedFrom = null;
    }
}