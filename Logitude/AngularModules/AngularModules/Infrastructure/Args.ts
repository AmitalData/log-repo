import { AddressList } from '../Common/EntityLists/AddressList';
import { FilterItem } from './DataContracts/ApiQueryFilters';

export class ListComponentArgs {
    public QueryCode: string;
    public ObjectTableName: string;
    public DisplayTitle: string;
    public BackButtonTitle: string;
    public NewButtonLabel: string = null;
    public Perspective: string = null;
    public Filters: any;
    public IsReadOnlyList: boolean;
    public IsBackToCurrentListView: boolean;
    public HideBackButton: boolean;
    public MethodName: string = null;
    public ShowViews: boolean = true;
    public IgnoreSelectedPerspective: boolean = false;
    public SelectedDirection: string = "All";
    public SelectedTransportMode: string = "All";
    public SuppressOnRowSelected: boolean = false;
    public SuppressOnRowSelectedField: boolean = false;
    public IsTasksMenuClicked: boolean;
    public BIReportFolderId: string;
    public QuerySection: string;
    public IsCargoTrackingMenuClicked: boolean = false;
    public DontCheckQueryFeature: boolean = false;
    public IsDigitalPortalMenuClicked: boolean = false;
    public DefaultFilterItems: FilterItem[] | null = null;
}

export class NewEntityArgs {
    public Perspective: string = null;
    public DefaultValues: string = null;
    public QueryNameTextCode: string = null;
    public ShowContactPart: boolean = false;
    public Address: AddressList;
}
export class UserArgs {
    public BackButtonText: string;
}

export class UserLicenseArgs {
    public AllUserLicenses: any[];
    public AllPackages: any[];
    public ActiveNotAdditionalUsersCount: number;
    public SearchField: string;
}
