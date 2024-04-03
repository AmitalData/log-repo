
export class ObjectTableTabPM {

    constructor() {
        this.IsDirty = false;
    }

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue;}


    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue;this.MarkAsDirty(); }


    private objectTableId: string;
    public get ObjectTableId() { return this.objectTableId; }
    public set ObjectTableId(newValue: string) { this.objectTableId = newValue;this.MarkAsDirty(); }


    private controlPath: string;
    public get ControlPath() { return this.controlPath; }
    public set ControlPath(newValue: string) { this.controlPath = newValue;this.MarkAsDirty(); }


    private tabNameTextCodeId: string;
    public get TabNameTextCodeId() { return this.tabNameTextCodeId; }
    public set TabNameTextCodeId(newValue: string) { this.tabNameTextCodeId = newValue;this.MarkAsDirty(); }


    private indexOrder: number;
    public get IndexOrder() { return this.indexOrder; }
    public set IndexOrder(newValue: number) { this.indexOrder = newValue;this.MarkAsDirty(); }



    private tabNameTextCodeDefaultText: string;
    public get TabNameTextCodeDefaultText() { return this.tabNameTextCodeDefaultText; }
    public set TabNameTextCodeDefaultText(newValue: string) { this.tabNameTextCodeDefaultText = newValue;this.MarkAsDirty(); }


    private objectTableName: string;
    public get ObjectTableName() { return this.objectTableName; }
    public set ObjectTableName(newValue: string) { this.objectTableName = newValue;this.MarkAsDirty(); }

    private tabNameTextCodeCode: string;
    public get TabNameTextCodeCode() { return this.tabNameTextCodeCode; }
    public set TabNameTextCodeCode(newValue: string) { this.tabNameTextCodeCode = newValue;this.MarkAsDirty(); }

    private code: string;
    public get Code() { return this.code; }
    public set Code(newValue: string) { this.code = newValue;this.MarkAsDirty(); }


    private featureId: string;
    public get FeatureId() { return this.featureId; }
    public set FeatureId(newValue: string) { this.featureId = newValue;this.MarkAsDirty(); }


    private changeset: string;
    public get Changeset() { return this.changeset; }
    public set Changeset(newValue: string) { this.changeset = newValue; this.MarkAsDirty(); }

    private hideTabNameInScreen: boolean;
    public get HideTabNameInScreen() { return this.hideTabNameInScreen; }
    public set HideTabNameInScreen(newValue: boolean) { this.hideTabNameInScreen = newValue; this.MarkAsDirty(); }


    public IsDirty: boolean;
    MarkAsDirty() {
        this.IsDirty = true;

    }



    Type: 'Custom' | 'Predefined';
    Name: string;
    ScreenCode: string;
    ScreenName: string;
    OriginalTabCode: string;

    public IsHidden: boolean;
    public Disabled: boolean;
    public HtmlComponentName: string;
    public HtmlComponentUrl: string;
    public FeatureUniqeCode: string;
}
