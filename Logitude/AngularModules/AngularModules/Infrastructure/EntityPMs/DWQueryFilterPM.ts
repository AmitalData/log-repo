
export class DWQueryFilterPM {

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; }


    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; }


    private dWQueryId: string;
    public get DWQueryId() { return this.dWQueryId; }
    public set DWQueryId(newValue: string) { this.dWQueryId = newValue; }


    private dWObjectFieldId: string;
    public get DWObjectFieldId() { return this.dWObjectFieldId; }
    public set DWObjectFieldId(newValue: string) { this.dWObjectFieldId = newValue; }


    private isPredefined: boolean;
    public get IsPredefined() { return this.isPredefined; }
    public set IsPredefined(newValue: boolean) { this.isPredefined = newValue; }


    private predefinedValue: string;
    public get PredefinedValue() { return this.predefinedValue; }
    public set PredefinedValue(newValue: string) { this.predefinedValue = newValue; }


    private predefinedValue2: string;
    public get PredefinedValue2() { return this.predefinedValue2; }
    public set PredefinedValue2(newValue: string) { this.predefinedValue2 = newValue; }


    private operator: string;
    public get Operator() { return this.operator; }
    public set Operator(newValue: string) { this.operator = newValue; }


    private indexOrder: string;
    public get IndexOrder() { return this.indexOrder; }
    public set IndexOrder(newValue: string) { this.indexOrder = newValue; }


    private userId: string;
    public get UserId() { return this.userId; }
    public set UserId(newValue: string) { this.userId = newValue; }

}