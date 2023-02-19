
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

    private inactive: boolean;
    public get Inactive() { return this.inactive; }
    public set Inactive(newValue: boolean) { this.inactive = newValue; }


    private type: string;
    public get Type() { return this.type; }
    public set Type(newValue: string) { this.type = newValue; }


    private sortedByFieldCode: string;
    public get SortedByFieldCode() { return this.sortedByFieldCode; }
    public set SortedByFieldCode(newValue: string) { this.sortedByFieldCode = newValue; }


    private sortedType: string;
    public get SortedType() { return this.sortedType; }
    public set SortedType(newValue: string) { this.sortedType = newValue; }

    private relatedScreenCode: string;
    public get RelatedScreenCode() { return this.relatedScreenCode; }
    public set RelatedScreenCode(newValue: string) { this.relatedScreenCode = newValue; }

    public OldEntityPM: ScreenPM;

    private isDirty: boolean;
    public get IsDirty() { return this.isDirty; }
    public set IsDirty(newValue: boolean) { this.isDirty = newValue; }

    private disableMarkAsDirty: boolean;
    public get DisableMarkAsDirty() { return this.disableMarkAsDirty; }
    public set DisableMarkAsDirty(newValue: boolean) { this.disableMarkAsDirty = newValue; }

    private isHeaderScreen: boolean;
    public get IsHeaderScreen() { return this.isHeaderScreen; }
    public set IsHeaderScreen(newValue: boolean) { this.isHeaderScreen = newValue; }

}
