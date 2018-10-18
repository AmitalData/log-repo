 
export class ScreenPM {
      
    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue;}
       
	 
    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; }

    private numberOfRows: number;
    public get NumberOfRows() { return this.numberOfRows; }
    public set NumberOfRows(newValue: number) { this.numberOfRows = newValue; }

    private numberOfColumns: number;
    public get NumberOfColumns() { return this.numberOfColumns; }
    public set NumberOfColumns(newValue: number) { this.numberOfColumns = newValue; }

    private userTenant: number;
    public get UserTenant() { return this.userTenant; }
    public set UserTenant(newValue: number) { this.userTenant = newValue; }

    private name: string;
    public get Name() { return this.name; }
    public set Name(newValue: string) { this.name = newValue; }
	 
    private code: string;
    public get Code() { return this.code; }
    public set Code(newValue: string) { this.code = newValue; }
       
	 
    private objectTableId: string;
    public get ObjectTableId() { return this.objectTableId; }
    public set ObjectTableId(newValue: string) { this.objectTableId = newValue; }
       

    private objectTableName: string;
    public get ObjectTableName() { return this.objectTableName; }
    public set ObjectTableName(newValue: string) { this.objectTableName = newValue; }


    private isReadOnly: boolean;
    public get IsReadOnly() { return this.isReadOnly; }
    public set IsReadOnly(newValue: boolean) { this.isReadOnly = newValue; }
       
}