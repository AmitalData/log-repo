export class ExternalParams {
    public Menu: string;
    public Action: string;
    public OneTimePasswordId: string = "";
    public Args: ExternalParamsArg[] = [];
}

export class ExternalParamsArg {
    public FieldName: string;
    public FieldValue: any;    
}