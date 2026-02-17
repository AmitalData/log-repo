declare var window: any;
import {Component, OnInit, ComponentRef, Output, EventEmitter}  from '@angular/core';
import {QueryPM} from '../../../Infrastructure/EntityPMs/QueryPM';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {UserExtendedPMService} from '../../../Common/Services/ExtendedPMs/UserExtendedPMService';
import {UsersWorkspaceRecentItem} from  './ViewModel/UsersWorkspaceRecentItem';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ListComponentArgs, UserArgs} from '../../../Infrastructure/Args';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {UserLicensePM} from '../../../Common/EntityPMs/UserLicensePM';
import {PackageList} from '../../../Common/EntityLists/PackageList';
import {PackageListService} from '../../../Common/Services/StandardLists/PackageListService';
import {AppTool} from '../../../Infrastructure/Tools';
import {UserLicenseArgs} from '../../../Infrastructure/Args';

@Component({
    moduleId: module.id,
    selector: 'UserWorkspace',
    templateUrl: './UserWorkspaceComponent.html',
    providers: [UserExtendedPMService]
})

export class UserWorkspaceComponent implements OnInit {
    public ComponentRef: ComponentRef<UserWorkspaceComponent>;
    filterAgrs: ApiQueryFilters;
    private _entityResourceService: EntityResourceService;
    SearchText: string = "Search names /positions";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _userExtendedPMService: UserExtendedPMService) {
        this._entityResourceService = new EntityResourceService();
    }

    ngOnInit() {

    }

    public BackButtonText = "Maintenance";
    public IsDemoTenant: boolean = false;
    Run(args: UserArgs) {
        if (args) {
            this.BackButtonText = args.BackButtonText;
        }

        if (SessionLocator.Tenant == 65 && !SessionLocator.LoggedUserPM.IsCustomerCare) {
            this.IsDemoTenant = true;
        }

        if (!this.IsDemoTenant) {
            this.InitCounts();
            this.CheckVisibilityProperties();
            this.InitLicensesManagment();
            this.LoadAllData();
        }
    }

    private InitCounts() {
        this.LicensesCount = "0";
        this.AllUsersCount = "0";
        this.ActiveUsersCount = "0";
        this.InactiveUsersCount = "0";
        this.ActiveLicensedCount = "0";
        this.ActiveNotLicensedCount = "0";
    }

    public IsLicensesManagmentSystem: boolean = false;
    public LicensesManagmentsList: LicensesManagementDataItem[];
    private allUserLicenses: UserLicensePM[];
    private InitLicensesManagment() {
        var isLicensesManagmentSystem: boolean = false;

        if (SessionLocator.TenantManagementJS.IsMultiPackage) {
            if (FeatureLocator.HasFeaturePermession("User", "User.Feature.LicensesManagment")) {
                isLicensesManagmentSystem = true;
            }
        }
        
        this.IsLicensesManagmentSystem = isLicensesManagmentSystem;

        if (this.IsLicensesManagmentSystem) {
            this.LoadUserLicenses();
        }
    }

    private LoadUserLicenses() {
        this._userExtendedPMService.GetUserLicenses().subscribe(myResult => {
            if (myResult == null) {
                this.LicensesManagmentsList = [];
            }

            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    this.allUserLicenses = myResponse.Result;

                    this.BuildLicensesManagmentCounts();
                    this.CheckUserLicenseExclamationMark();
                }
            }
        });
    }

    private allPackages: PackageList[];
    private BuildLicensesManagmentCounts() {
        this.LicensesManagmentsList = [];

        var service: PackageListService = new PackageListService();
        service.getAllFromCache().subscribe(result => {
            this.allPackages = result.Result;
            this.FillLicensesManagmentsList();
        });
    }

    private FillLicensesManagmentsList() {
        if (SessionLocator.TenantManagementJS.MainAdditionalPackageApplied) {
            var usersCount: number = this.ActiveNotAdditionalUsersCount;
            var numberOfUsers: number = SessionLocator.TenantManagementJS.NumberOfFreeUsers + SessionLocator.TenantManagementJS.NumberOfUsers;
            var myCountText: string = usersCount + "/" + numberOfUsers;

            var mainItem: LicensesManagementDataItem = new LicensesManagementDataItem();
            mainItem.PackageCode = SessionLocator.TenantManagementJS.PackageCode;
            mainItem.PackageName = SessionLocator.TenantManagementJS.PackageName;
            mainItem.CountText = myCountText;
            this.LicensesManagmentsList.push(mainItem);
        }

        var index: number = 0;
        SessionLocator.TenantManagementJS.TenantManagementLicenses.sort((a, b) => { return (a.PackageCode === b.PackageCode) ? 0 : (a.PackageCode < b.PackageCode) ? -1 : 1 }).forEach(item => {
            index++;

            if (index <= 10) {
                var myPackageName: string = "";
                var myPackage: PackageList = this.allPackages.filter(d => d.Code == item.PackageCode)[0];
                if (myPackage != null) {
                    myPackageName = myPackage.Name;
                }

                var usersCount: number = this.allUserLicenses.filter(d => d.PackageCode == item.PackageCode).length;
                var numberOfUsers: number = (AppTool.IsNullOrZero(item.NumberOfUsers) ? 0 : item.NumberOfUsers) + (AppTool.IsNullOrZero(item.FreeUsers) ? 0 : item.FreeUsers);
                var myCountText: string = usersCount + "/" + numberOfUsers;

                var newItem: LicensesManagementDataItem = new LicensesManagementDataItem();
                newItem.PackageCode = item.PackageCode;
                newItem.PackageName = myPackageName;
                newItem.CountText = myCountText;
                this.LicensesManagmentsList.push(newItem);
            }
        });
    }

    public ShowUserLicenseExclamationMark: boolean;
    private CheckUserLicenseExclamationMark() {
        this.ShowUserLicenseExclamationMark = false;

        if (this.allUserLicenses != null) {
            SessionLocator.TenantManagementJS.TenantManagementLicenses.forEach(item => {
                var usersCount: number = this.allUserLicenses.filter(d => d.PackageCode == item.PackageCode).length;
                var numberOfUsers: number = (AppTool.IsNullOrZero(item.NumberOfUsers) ? 0 : item.NumberOfUsers) + (AppTool.IsNullOrZero(item.FreeUsers) ? 0 : item.FreeUsers);

                if (usersCount < numberOfUsers) {
                    this.ShowUserLicenseExclamationMark = true;
                    //break;
                }
            });
        }
    }

    LicensesManagementClicked() {
        var args: UserLicenseArgs = new UserLicenseArgs();
        args.AllPackages = this.allPackages;
        args.AllUserLicenses = this.allUserLicenses;
        args.ActiveNotAdditionalUsersCount = this.ActiveNotAdditionalUsersCount;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 960;
        logitudeWindow.Height = 570;
        logitudeWindow.Title = "Licenses Management";
        logitudeWindow.WindowArgs = args;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureUser/Components/LicensesManagementComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            this.RefreshButtonClicked();
        });
    }

    IsBackButtonEnabled: boolean;
    private RefreshBackButtonEnabled() {
        if (this.isLoadDataSummaryCompleted && this.isLoadRecentUsersCompleted) {
            this.IsBackButtonEnabled = true;
        }
    }

    private isLoadDataSummaryCompleted: boolean;
    private isLoadRecentUsersCompleted: boolean;
    LoadAllData() {
        this.IsBackButtonEnabled = false;

        this.LoadDataSummary();
        this.LoadRecentUsers();
        this.BuildCustomQueriesList();
    }

    RefreshButtonClicked() {
        this.LoadAllData();        
    }

    public AllUsersCount: string;
    public LicensesCount: string;
    public ActiveLicensedCount: string;
    public ActiveUsersCount: string;
    public InactiveUsersCount: string;
    public ActiveNotLicensedCount: string;
    private ActiveNotAdditionalUsersCount: number;
    LoadDataSummary() {
        this.InitCounts();
        this.isLoadDataSummaryCompleted = false;

        this._userExtendedPMService.GetUsersWorkspaceSummary(SessionInfo.LoggedUserTenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.LicensesCount = myResult.LicensesCount > 1000 ? "1000+" : myResult.LicensesCount.toString();
                    this.AllUsersCount = myResult.AllUsersCount > 1000 ? "1000+" : myResult.AllUsersCount.toString();
                    this.ActiveUsersCount = myResult.ActiveUsersCount > 1000 ? "1000+" : myResult.ActiveUsersCount.toString();
                    this.InactiveUsersCount = myResult.InactiveUsersCount > 1000 ? "1000+" : myResult.InactiveUsersCount.toString();
                    this.ActiveLicensedCount = myResult.ActiveLicensedCount > 1000 ? "1000+" : myResult.ActiveLicensedCount.toString();
                    this.ActiveNotLicensedCount = myResult.ActiveNotLicensedCount > 1000 ? "1000+" : myResult.ActiveNotLicensedCount.toString();
                    this.ActiveNotAdditionalUsersCount = myResult.ActiveNotAdditionalUsersCount;

                    this.isLoadDataSummaryCompleted = true;
                    this.LoadUserLicenses();
                    this.RefreshBackButtonEnabled();
                }
            }
        });
    }

    public RecentUserLists: UsersWorkspaceRecentItem[];
    public SelectedUserViewModel: UsersWorkspaceRecentItem;
    LoadRecentUsers() {
        this.RecentUserLists = [];
        this.isLoadRecentUsersCompleted = false;

        this._userExtendedPMService.GetUsersWorkspaceRecentLogins(SessionLocator.Tenant).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult: any = pmResponse.Result;
                if (myResult) {
                    this.RecentUserLists = myResult;

                    this.RecentUserLists.forEach((item) => {
                        item.IPSiteUri = "http://www.infosniper.net/index.php?ip_address=" + item.IP;
                    });

                    this.SelectedUserViewModel = this.RecentUserLists[0];
                    this.isLoadRecentUsersCompleted = true;
                    this.RefreshBackButtonEnabled();
                }
            }
        });
    }

    @Output() ReloadUserQueries = new EventEmitter();
    BuildCustomQueriesList() {
        this.ReloadUserQueries.emit();
    }

    onUserQueriesBackComplete(event) {
        this.RefreshButtonClicked();
    }

    public AllUsersQueryVisibility: boolean = false;
    public ActiveUsersQueryVisibility: boolean = false;
    public InactiveUsersQueryVisibility: boolean = false;
    public ActiveNotLicensedQueryVisibility: boolean = false;
    public MyViewsQueryVisibility: boolean = false;
    public IsLicensesVisibile: boolean = false;
    private CheckVisibilityProperties() {
        if (FeatureLocator.HasFeaturePermession("User", "User.Query.AllUsers")) {
            this.AllUsersQueryVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("User", "User.Query.ActiveUsers")) {
            this.ActiveUsersQueryVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("User", "User.Query.InactiveUsers")) {
            this.InactiveUsersQueryVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("User", "User.Query.ActiveNotLicensed")) {
            if (SessionLocator.TenantManagementJS.ManageLicencesPerUser) {
                this.ActiveNotLicensedQueryVisibility = true;
            }
        }

        if (FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES")) {
            this.MyViewsQueryVisibility = true;
        }

        if (SessionLocator.TenantManagementJS.ManageLicencesPerUser) {
            this.IsLicensesVisibile = true;
        }
    }

    ViewUserQuery(myCode: string, nameTextCodeCode: string = null) {
        var backButtonTitle = "User Work Space";
        var queryCode = "";
        var displayTitle = "";
        if (myCode != null) {

            if (nameTextCodeCode) {
                queryCode = myCode;
                displayTitle = TextCodeTranslator.Translate(nameTextCodeCode);
            }

            else {
                switch (myCode) {
                    case "ACTV":
                        {
                            queryCode = "ActiveUsers";
                            displayTitle = "Active Users";
                            break;
                        }

                    case "NTLC":
                        {
                            queryCode = "ActiveNotLicensed";
                            displayTitle = "Active Not Licensed";
                            break;
                        }

                    case "INAC":
                        {
                            queryCode = "InactiveUsers";
                            displayTitle = "Inactive Users";
                            break;
                        }

                    case "All":
                        {
                            queryCode = "AllUsers";
                            displayTitle = "All Users";
                            break;
                        }
                }
            }

            var listArgs = new ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = "User";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;

            var objectTablePM = window.ObjectTables.filter((d: any) => d.Name == "User")[0];

            SessionLocator.DynamicLoader.Load("./Infrastructure/Components/ListComponent/ListComponent", this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => this.RefreshButtonClicked());

                });
        }
    }

    BackButtonClicked() {
        if (this.ComponentRef != null) {
            this.ComponentRef.destroy();
            this.ComponentRef = null;
        }
    }

    EditUser(selectedItem: any) {
        if (selectedItem) {
            SessionLocator.DynamicLoader.Load("./Infrastructure/Components/EditComponent/EditComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: selectedItem.Id, ObjectTableName: 'User', BackButtonLabel: 'Users Workspace' });
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => this.RefreshButtonClicked());
                });
        }
    }

    ShowHistory(selectedItem: UsersWorkspaceRecentItem) {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 800;
        logWindow.Height = 500;
        logWindow.Title = "User Login History";
        logWindow.DataContext = selectedItem;
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/UserLoginHistoryComponent');
    }

    SupportManagement() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 300;
        logWindow.Height = 260;
        logWindow.Title = "Support Management";
        logWindow.DataContext = this;
        logWindow.IsShowCloseButton = true;
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/SupportManagementComponent');
    }

    ResetUserPassword() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 250;
        logWindow.Title = "Reset User Password";
        logWindow.DataContext = this;
        logWindow.IsShowCloseButton = true;
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/ResetUserPasswordComponent');
    }

    ViewNewUserWindow() {
        this._entityResourceService.getEntityResourceByTableName("User", 0).subscribe(response => {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 965;
            logWindow.Height = 600;
            logWindow.Title = "New User";
            logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/NewUserComponent');
            logWindow.WindowClosed.subscribe(($event: any) => {
                this.RefreshButtonClicked();
            });
        });
    }
}

export class LicensesManagementDataItem {
    public PackageCode: string;
    public PackageName: string;
    public CountText: string;
}
