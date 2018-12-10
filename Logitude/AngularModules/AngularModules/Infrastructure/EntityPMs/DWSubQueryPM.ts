
export class DWSubQueryPM {

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; }

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; }

    private dWFactTableCode: string;
    public get DWFactTableCode() { return this.dWFactTableCode; }
    public set DWFactTableCode(newValue: string) { this.dWFactTableCode = newValue; }

    private dWQueryId: string;
    public get DWQueryId() { return this.dWQueryId; }
    public set DWQueryId(newValue: string) { this.dWQueryId = newValue; }

    private sQLString: string;
    public get SQLString() { return this.sQLString; }
    public set SQLString(newValue: string) { this.sQLString = newValue; }

    private filtersXML: string;
    public get FiltersXML() { return this.filtersXML; }
    public set FiltersXML(newValue: string) { this.filtersXML = newValue; }

    private columnsXML: string;
    public get ColumnsXML() { return this.columnsXML; }
    public set ColumnsXML(newValue: string) { this.columnsXML = newValue; }

     

}