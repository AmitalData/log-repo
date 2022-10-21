export class SetValue {
    public field: string;
    public fieldCode: string;
    public type: string;
    public lookupType: string;
    public operator: string;
    public value: string;
    public fieldChangedToggle: boolean;

    constructor() {
        this.field = null;
        this.fieldCode = null;
        this.type = null;
        this.lookupType = null;
        this.operator = null;
        this.value = null;
        this.fieldChangedToggle = false;
    }
}