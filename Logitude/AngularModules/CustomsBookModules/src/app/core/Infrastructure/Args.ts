export class ListComponentArgs {
	public QueryCode!: string;
	public ObjectTableName!: string;
	public DisplayTitle!: string;
	public BackButtonTitle!: string;
	public NewButtonLabel!: string;
	public Perspective!: string;
	public Filters: any;
	public IsReadOnlyList!: boolean;
	public IsBackToCurrentListView!: boolean;
	public HideBackButton!: boolean;
	public MethodName!: string;
	public ShowViews: boolean = true;
	public IgnoreSelectedPerspective: boolean = false;
	public SelectedDirection: string = 'All';
	public SelectedTransportMode: string = 'All';
	public SuppressOnRowSelected: boolean = false;
	public SuppressOnRowSelectedField: boolean = false;
	public IsTasksMenuClicked!: boolean;
	public BIReportFolderId!: string;
	public QuerySection!: string;
}

export class NewEntityArgs {
	public Perspective!: string;
	public DefaultValues!: string;
	public QueryNameTextCode!: string;
	public ShowContactPart: boolean = false;
}
export class UserArgs {
	public BackButtonText!: string;
}

export class UserLicenseArgs {
	public AllUserLicenses!: any[];
	public AllPackages!: any[];
	public ActiveNotAdditionalUsersCount!: number;
	public SearchField!: string;
}
