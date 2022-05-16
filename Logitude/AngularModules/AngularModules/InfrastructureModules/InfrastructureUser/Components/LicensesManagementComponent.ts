import {Component, Output, EventEmitter, OnDestroy}  from '@angular/core';
import {PackageList} from '../../../Common/EntityLists/PackageList';
import {UserList} from '../../../Common/EntityLists/UserList';
import {UserExtendedListService, UserExtendedList} from '../../../Common/Services/ExtendedLists/UserExtendedListService';
import {UserLicensePM} from '../../../Common/EntityPMs/UserLicensePM';
import {UserLicenseArgs} from '../../../Infrastructure/Args';
import {AppTool, FontTool} from '../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {CommonDomainService, UserLicenseUpdateHelper} from '../../../Common/Services/CommonDomainService';
import {UserExtendedPMService} from '../../../Common/Services/ExtendedPMs/UserExtendedPMService';
import {TenantManagementLicensePM} from '../../../Infrastructure/EntityPMs/TenantManagementLicensePM';

@Component({
    
    templateUrl: './LicensesManagementComponent.html',
})

export class LicensesManagementComponent implements OnDestroy {
  public Items: any[] = [];

    @Output() SearchFieldChangeEvent = new EventEmitter();
    public Columns: any[] = [];
    private dirtyItem: UserLicensePM;
    private CurrentSession = SessionLocator.SelectedSession;
    public HeaderColumnWidth: number = 150;
    constructor() {
        this.Listen();
    }

    @Output() MenuHeaderchangeevent = new EventEmitter();
    private Refresh: boolean = false;
    private ListenEvent: any = null;
    Listen() {
        this.ListenEvent = this.CurrentSession.PseventRowSelectEvent.subscribe((res) => {
            if (res.Name == "Add") {
                this.Add(res.User, res.PackageCode);
            }

            if (res.Name == "Remove") {
                this.Refresh = res.Refresh;
                this.Remove(res.User, res.PackageCode);
            }
        });        
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.ListenEvent);
        this.ListenEvent = null
    }

    public AllUserLicenses: UserLicensePM[];
    public AllPackages: PackageList[];
    private ActiveNotAdditionalUsersCount: number = 0;    
    SetWindowArgs(args: UserLicenseArgs) {
        this.AllPackages = args.AllPackages;
        this.ActiveNotAdditionalUsersCount = args.ActiveNotAdditionalUsersCount;
        this.dirtyItem = null;

        if (!AppTool.IsNullOrEmpty(args.SearchField)) {
            this.SearchFields = args.SearchField;
        }

        this.InitColumns();
        this.LoadUserLicenses();
    }

    private InitColumns() {
        this.Columns = [];

        this.Columns.push({
            FieldName: "EnglishName",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Name',
            ServerSideSortable: true,
            Styles: { width: '200px' },
        });

        this.Columns.push({
            FieldName: "Email",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            Display: 'Email',
            Styles: { width: '200px' },
        });
    }

    DataSource = {
        pageSize: 100,
        rowCount: null,
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    private filterAgrs: ApiQueryFilters;
    private GetRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        if (filters == null) {
            filters = new ApiQueryFilters();
        }

        filters.GetCount = true;
        filters.PageIndex = skip;
        filters.PageSize = 100;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.Tenant = SessionLocator.Tenant;
        filters.GetCount = getCount;

        if (!AppTool.IsNullOrEmpty(this.SearchFields)) {
            filters.Filter1Name = "SearchFields";
            filters.Filter1Value = this.SearchFields;
            filters.Filter1Operator = "Contains";
        }

        this.filterAgrs = filters;
        var service: UserExtendedListService = new UserExtendedListService();
        return new Promise((resolve, reject) => { resolve(service.GetCustomDataByFilters(filters)) });
    }
    
    private LoadUserLicenses() {
        this.DataLoaded = false;

        var userExtendedPMService: UserExtendedPMService = new UserExtendedPMService();
        userExtendedPMService.GetUserLicenses().subscribe((myResult:any) => {
            if (myResult == null) {
                this.LicensesManagmentsList = [];
            }

            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    this.AllUserLicenses = myResponse.Result;

                    this.BuildHeaders();
                    this.BuildAdditionalColumns();
                }
            }
        });
    }

    public DataLoaded: boolean = false;
    public LicensesManagmentsList: LicensesManagementDataItem[];
    private BuildHeaders() {
        this.LicensesManagmentsList = [];

        if (SessionLocator.TenantManagementJS.MainAdditionalPackageApplied) {
            var numberOfUsers: number = SessionLocator.TenantManagementJS.NumberOfFreeUsers + SessionLocator.TenantManagementJS.NumberOfUsers;

            var foreground = FontTool.Black;
            if (this.ActiveNotAdditionalUsersCount > numberOfUsers) {
                foreground = FontTool.Red;
            }

            var mainItem: LicensesManagementDataItem = new LicensesManagementDataItem();
            mainItem.Header = this.ActiveNotAdditionalUsersCount + "/" + numberOfUsers;
            mainItem.Color = foreground;
            mainItem.UsersCount = this.ActiveNotAdditionalUsersCount;
            mainItem.NumberOfUsers = numberOfUsers;
            this.LicensesManagmentsList.push(mainItem);
        }

        var index: number = 0;

        var loop_licenses: TenantManagementLicensePM[] = SessionLocator.TenantManagementJS.TenantManagementLicenses.sort((a, b) => { return (a.PackageCode.toLowerCase() === b.PackageCode.toLowerCase()) ? 0 : (a.PackageCode.toLowerCase() < b.PackageCode.toLowerCase()) ? -1 : 1 });
        loop_licenses.forEach(item => {
            index++;

            if (index <= 10) {
                var usersCount: number = this.AllUserLicenses.filter(d => d.PackageCode == item.PackageCode).length;
                var numberOfUsers: number = (AppTool.IsNullOrZero(item.NumberOfUsers) ? 0 : item.NumberOfUsers) + (AppTool.IsNullOrZero(item.FreeUsers) ? 0 : item.FreeUsers);;

                var foreground = FontTool.Black;
                if (usersCount > numberOfUsers) {
                    foreground = FontTool.Red;
                }

                var newItem: LicensesManagementDataItem = new LicensesManagementDataItem();
                newItem.Header = usersCount + "/" + numberOfUsers;
                newItem.Color = foreground;
                newItem.UsersCount = usersCount;
                newItem.NumberOfUsers = numberOfUsers;
                this.LicensesManagmentsList.push(newItem);
            }
        });
    }

    private BuildAdditionalColumns() {
        if (SessionLocator.TenantManagementJS.MainAdditionalPackageApplied) {
            var mainAdditionalPackageApplied = "true";
            this.Columns.push({
                FieldName: SessionLocator.TenantManagementJS.PackageCode + ",0" + "," + SessionLocator.TenantManagementJS.PackageName + "," + mainAdditionalPackageApplied,
                DataTypeCode: 'Boolean',
                Display: SessionLocator.TenantManagementJS.PackageName,
                IsCustomTemplate: true,
                ServerSideSortable: true,
                Styles: { width: '100px' },
                HtmlListComponentName: 'ColumnCheckBoxComponent',
                HtmlListComponentUrl: './InfrastructureModules/InfrastructureUser/Components/ColumnCheckBoxComponent',
            });
        }

        var index: number = 0;

        var loop_licenses: TenantManagementLicensePM[] = SessionLocator.TenantManagementJS.TenantManagementLicenses.sort((a, b) => { return (a.PackageCode.toLowerCase() === b.PackageCode.toLowerCase()) ? 0 : (a.PackageCode.toLowerCase() < b.PackageCode.toLowerCase()) ? -1 : 1 });
        
        loop_licenses.forEach(item => {
            index++;
            if (index <= 10) {
                var myPackageName: string = "";
                var myPackageCode: string = null;
                var mainAdditionalPackageApplied = "false";
                var list: PackageList = this.AllPackages.filter(d => d.Code == item.PackageCode)[0];
                if (list != null) {
                    myPackageName = list.Name;
                    myPackageCode = list.Code;
                }
                this.Columns.push({
                    FieldName: myPackageCode + "," + index + "," + myPackageName + "," + mainAdditionalPackageApplied,
                    DataTypeCode: 'Boolean',
                    Display: myPackageName,
                    IsCustomTemplate: true,
                    ServerSideSortable: true,
                    Styles: { width: '100px' },
                    HtmlListComponentName: 'ColumnCheckBoxComponent',
                    HtmlListComponentUrl: './InfrastructureModules/InfrastructureUser/Components/ColumnCheckBoxComponent',
                });
            }
        });

        this.DataLoaded = true;
    }

    public SearchFields: string;
    SearchTextChanged(text: string) {
        this.SearchFields = text;
        this.SearchFieldChangeEvent.emit(this.SearchFields);
    }

    public Add(user: UserExtendedList, myPackageCode: string) {
        this.dirtyItem = null;
        var itemPM: UserLicensePM = new UserLicensePM();
        itemPM.UserId = user.Id;
        itemPM.PackageCode = myPackageCode;
        itemPM.Tenant = SessionLocator.Tenant;
        itemPM.Email = user.Email;

        if (this.AllUserLicenses.filter(d => d.UserId == user.Id && d.PackageCode == myPackageCode).length == 0) {
            this.AllUserLicenses.push(itemPM);
            this.dirtyItem = itemPM;    
            this.Save(myPackageCode);          
        }
    }

    public Remove(user: UserList, myPackageCode: string) {
        this.dirtyItem = null;
        var itemPM: UserLicensePM = this.AllUserLicenses.filter(d => d.UserId == user.Id && d.PackageCode == myPackageCode)[0];
        if (itemPM != null) {
            var index = this.AllUserLicenses.indexOf(itemPM);

            if (index > -1) {
                this.AllUserLicenses.splice(index, 1);

                if (!AppTool.IsNullOrEmpty(itemPM.Id)) {
                    this.dirtyItem = itemPM;                    
                }

                this.Save(myPackageCode);
            }
        }        
    }

    public ValidationErrorsList: string[] = [];
    private Save(myPackageCode: string) {
        this.BuildHeaders();

        var errors: string[] = [];

        var userLicenses: UserLicensePM[] = this.AllUserLicenses.filter(d => d.PackageCode == myPackageCode);
        var tenantLicenses: TenantManagementLicensePM = SessionLocator.TenantManagementJS.TenantManagementLicenses.filter(d => d.PackageCode == myPackageCode)[0];

        var usersCount: number = userLicenses.length;
        var numberOfUsers: number = (AppTool.IsNullOrZero(tenantLicenses.NumberOfUsers) ? 0 : tenantLicenses.NumberOfUsers) + (AppTool.IsNullOrZero(tenantLicenses.FreeUsers) ? 0 : tenantLicenses.FreeUsers);;

        if (usersCount > numberOfUsers) {
            errors.push("Some Packages have exceeded the allowed number of users");
        }

        //this.BuildHeaders();
        
        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0 && this.dirtyItem != null) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show("You will need to logout and login again for the changes to take place");

            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.RunSave();
                }

                else {
                    this.InitColumns();
                    this.LoadUserLicenses();
                    this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
                }
            });
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    
    private RunSave() {
        if (this.dirtyItem != null) {
            this.CurrentSession.StartBusyIndicatorSaving();

            var myServiceHelper = new UserLicenseUpdateHelper();
            myServiceHelper.Tenant = SessionLocator.Tenant;
            myServiceHelper.Items.push(this.dirtyItem);

            var generalService: CommonDomainService = new CommonDomainService();
            generalService.UpdateUserLicense(myServiceHelper).subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {
                    this.CurrentSession.StopBusyIndicator();
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    var itemPM: UserLicensePM = this.AllUserLicenses.filter(d => d.UserId == this.dirtyItem.UserId && d.PackageCode == this.dirtyItem.PackageCode)[0];
                    if (itemPM != null) {
                        var index = this.AllUserLicenses.indexOf(itemPM);

                        if (index > -1) {
                            this.AllUserLicenses.filter(d => d.UserId == this.dirtyItem.UserId && d.PackageCode == this.dirtyItem.PackageCode)[0].Id = myResponse.Result.Items[0].Id;
                        }
                    }  

                    if (this.Refresh) {
                        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });                        
                        this.Refresh = false;
                    }
                   
                    this.dirtyItem = null;                    
                    this.BuildHeaders();
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    }
}

export class LicensesManagementDataItem {
    public Header: string;
    public Color: string;
    public UsersCount: number;
    public NumberOfUsers: number;
}
