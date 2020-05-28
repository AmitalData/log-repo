 
export class ScreenFieldPM {
      
    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue;}
       
	 
    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; }

    private row: number;
    public get Row() { return this.row; }
    public set Row(newValue: number) { this.row = newValue; }

    private column: number;
    public get Column() { return this.column; }
    public set Column(newValue: number) { this.column = newValue; }

    private screenId: string;
    public get ScreenId() { return this.screenId; }
    public set ScreenId(newValue: string) { this.screenId = newValue; }

    private objectFieldId: string;
    public get ObjectFieldId() { return this.objectFieldId; }
    public set ObjectFieldId(newValue: string) { this.objectFieldId = newValue; }
	 
    private objectFieldName: string;
    public get ObjectFieldName() { return this.objectFieldName; }
    public set ObjectFieldName(newValue: string) { this.objectFieldName = newValue; }
       
	 
    private screenCode: string;
    public get ScreenCode() { return this.screenCode; }
    public set ScreenCode(newValue: string) { this.screenCode = newValue; }
       

    private objectFieldObjectTableName: string;
    public get ObjectFieldObjectTableName() { return this.objectFieldObjectTableName; }
    public set ObjectFieldObjectTableName(newValue: string) { this.objectFieldObjectTableName = newValue; }


    private objectFieldCode: string;
    public get ObjectFieldCode() { return this.objectFieldCode; }
    public set ObjectFieldCode(newValue: string) { this.objectFieldCode = newValue; }
     
}