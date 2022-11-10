export class SetValue {
    public field: string;
    public fieldCode: string;
    public type: string;
    public lookupType: string;
    public picklistType: string;
    public operator: string;
    public value: string;
    public isDisabled: boolean;
    public fieldChangedToggle: boolean;

    constructor() {
        this.field = null;
        this.fieldCode = null;
        this.type = null;
        this.lookupType = null;
        this.picklistType = null;
        this.operator = null;
        this.value = null;
        this.isDisabled = false;
        this.fieldChangedToggle = false;
    }
}