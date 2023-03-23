export class TaskField {
    public field: string;
    public fieldCode: string;
    public isRequired: boolean;

    constructor() {
        this.field = null;
        this.fieldCode = null;
        this.isRequired = false;
    }
}