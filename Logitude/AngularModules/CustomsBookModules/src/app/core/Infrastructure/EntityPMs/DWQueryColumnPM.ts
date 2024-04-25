
export class DWQueryColumnPM {

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


    private indexOrder: number;
    public get IndexOrder() { return this.indexOrder; }
    public set IndexOrder(newValue: number) { this.indexOrder = newValue; }


    private columnWidth: number;
    public get ColumnWidth() { return this.columnWidth; }
    public set ColumnWidth(newValue: number) { this.columnWidth = newValue; }


    private userId: string;
    public get UserId() { return this.userId; }
    public set UserId(newValue: string) { this.userId = newValue; }




}