export class FeaturePM {
    public Id: string;
    public Tenant: number;
    public Code: string;
    public ObjectTableId: string;
    public NameTextCodeId: string;
    public FeatureTypeCode: string;
    public Packagable: boolean;
    public IsBusinessUnitEnabled: boolean;
    public IsOld: boolean;
    public IsCoreFeature: boolean;
    public FeatureUniqeCode: string;

    public NameTextCodeCode: string;

    // Dummy
    public ObjectTableName: string;
    public ParentRoleId: string;
    public RoleTenant: number;
    public IsCustomRole: boolean;
    public IsCustomRoleFeature: boolean;
    public PackageCode: string;
    public TranslatedName: string;
    public Exists: boolean;

    private roleId: string;
    public get RoleId() { return this.roleId; }
    public set RoleId(newValue: string) { if (this.roleId != newValue) { this.roleId = newValue; this.MarkAsDirty(); } }

    private accessLevelCode: string;
    public get AccessLevelCode() { return this.accessLevelCode; }
    public set AccessLevelCode(newValue: string) { if (this.accessLevelCode != newValue) { this.accessLevelCode = newValue; this.MarkAsDirty(); } }

    private isAdded: boolean;
    public get IsAdded() { return this.isAdded; }
    public set IsAdded(newValue: boolean) { if (this.isAdded != newValue) { this.isAdded = newValue; this.MarkAsDirty(); } }

    private isRemoved: boolean;
    public get IsRemoved() { return this.isRemoved; }
    public set IsRemoved(newValue: boolean) { if (this.isRemoved != newValue) { this.isRemoved = newValue; this.MarkAsDirty(); } }

    public IsDirty: boolean;
    public OldEntityPM: FeaturePM;
    MarkAsDirty() {
        this.IsDirty = true;
    }
}
