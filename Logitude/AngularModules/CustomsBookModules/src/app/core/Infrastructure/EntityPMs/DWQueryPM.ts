
export class DWQueryPM {

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; }

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; }

    //private dWObjectTableCode: string;
    //public get DWObjectTableCode() { return this.dWObjectTableCode; }
    //public set DWObjectTableCode(newValue: string) { this.dWObjectTableCode = newValue; }

    private sQLString: string;
    public get SQLString() { return this.sQLString; }
    public set SQLString(newValue: string) { this.sQLString = newValue; }

    private createdByUserId: string;
    public get CreatedByUserId() { return this.createdByUserId; }
    public set CreatedByUserId(newValue: string) { this.createdByUserId = newValue; }

    private updateByUserId: string;
    public get UpdateByUserId() { return this.updateByUserId; }
    public set UpdateByUserId(newValue: string) { this.updateByUserId = newValue; }

    private createdDate: Date;
    public get CreatedDate() { return this.createdDate; }
    public set CreatedDate(newValue: Date) { this.createdDate = newValue; }

    private updatedDate: Date;
    public get UpdatedDate() { return this.updatedDate; }
    public set UpdatedDate(newValue: Date) { this.updatedDate = newValue; }

   
     

}