
export class SharedUserQuery {
    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; }
    
    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; }

    private userId: string;
    public get UserId() { return this.userId; }
    public set UserId(newValue: string) { this.userId = newValue; }

    private queryId: string;
    public get QueryId() { return this.queryId; }
    public set QueryId(newValue: string) { this.queryId = newValue; }
}
