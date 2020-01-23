import { Component, ViewChildren, QueryList} from '@angular/core';
import { LocationDirective } from '../../../../Infrastructure/Utilities/LocationDirective';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DashboardWorkspaceComponent } from '../DashboardWorkspaceComponent';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { LastFilterClass } from '../../../../Infrastructure/Utilities/LastFilterClass';
import { DateTool, AppTool } from '../../../../Infrastructure/Tools';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { BusinessUnitListService } from '../../../../Common/Services/StandardLists/BusinessUnitListService';
import { BusinessUnitList } from '../../../../Common/EntityLists/BusinessUnitList';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { UserList } from '../../../../Common/EntityLists/UserList';
import { UserListService } from '../../../../Common/Services/StandardLists/UserListService';
import { CRMUtilities } from '../../../CRMUtilities';

@Component({
    moduleId: module.id,
    templateUrl: './QuoteDashboardComponent.html',
})

export class QuoteDashboardComponent extends BaseComponent {

    private isViewInited = false;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;

    private filterName_CreateDate: string = "CreateDate";
    private filterName_Owner: string = "Owner";
    private filterName_BusinessUnit: string = "BusinessUnit";
    // need to change
    private filterControlNameSpace: string = "Logitude.CRM.Views.CRMPages.DashboardTabsControls.ByCreateDateControl";
    public DateFilterList: Array<CodeNameClass> = [];
    public DataContext: QuoteDashboardComponent = this;
    private myBusinessUnitListService: BusinessUnitListService;
    private myUserListService: UserListService;

    private InitializeServices() {
        this.myBusinessUnitListService = new BusinessUnitListService();
        this.myUserListService = new UserListService();
    }

    constructor() {
        super();
        this.RunComponent();
        this.InitializeServices();
        this.BuildBusinessUnitFilter();
        this.BuildDateFilters();
    }

    RunComponent() {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isViewInited = true;
                this.InitializeComponent();
            }
        }
        else {
            this.RunComponentTimer();
        }
    }

    private InitializeComponent() {
        if (this.isViewInited) {
            this.LoadComponents();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private Wizard: DashboardWorkspaceComponent;
    InitTab(wizard: DashboardWorkspaceComponent) {
        this.Wizard = wizard;
    }

    RefreshTab() {
        
    }

    RefreshButtonClicked() {
        this.LoadFilteredQueries();
        this.LoadComponents();
    }

    LoadFilteredQueries() {
        if (this.SelectedDateFilter != null) {
            var days: number = parseInt(this.SelectedDateFilter.Code);
            this.ComputeDays();
        }

    }
    private BuildDateFilters() {
        this.DateFilterList = CRMUtilities.GetDateFilterList();

        var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_CreateDate);
        if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "-30";
        }

        this.selectedDateFilter = this.DateFilterList.filter(d => d.Code == defaultFilterCode)[0];



        if (this.selectedDateFilter.Code == "-2") {
            var ActiviytFromDate = LastFilterClass.GetFilterValue(this.filterControlNameSpace, "ByCreateFromDate");

            if (!AppTool.IsNullOrEmpty(ActiviytFromDate)) {
                var ActivityDate: Date = new Date();
                var ActivityFromDateString = ActiviytFromDate.split(':');
                ActivityDate.setFullYear(ActivityFromDateString[0], ActivityFromDateString[1] - 1, ActivityFromDateString[2]);
                this.fromDate = DateTool.GetDateParts(ActivityDate).DateObject;
            }

            var ActiviytToDate = LastFilterClass.GetFilterValue(this.filterControlNameSpace, "ByCreateToDate");
            if (!AppTool.IsNullOrEmpty(ActiviytToDate)) {
                var ActivityDate: Date = new Date();
                var ActivityToDateString = ActiviytToDate.split(':');
                ActivityDate.setFullYear(ActivityToDateString[0], ActivityToDateString[1] - 1, ActivityToDateString[2]);
                this.toDate = DateTool.GetDateParts(ActivityDate).DateObject;
            }


        }


    }

    private selectedBusinessUnitFilter: CodeNameClass;
    get SelectedBusinessUnitFilter() { return this.selectedBusinessUnitFilter; }
    set SelectedBusinessUnitFilter(value: CodeNameClass) {
        if (this.selectedBusinessUnitFilter != value) {
            this.selectedBusinessUnitFilter = value;

            this.GetSelectedBusinessUnitId();

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_BusinessUnit, (value == null ? null : value.Code));

            this.BuildUsersFilters(true);

        }
    }

    BuildUsersFilters(isUpdatingFilter: boolean) {
        this.UsersFilterList = [];

        if (this.SelectedBusinessUnitFilter == null) {
            this.selectedUserFilter = null;
            this.GetSelectedOwnerId();

            if (isUpdatingFilter) {
                LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
            }

            this.LoadFilteredQueries();
        }

        else {
            switch (this.SelectedBusinessUnitFilter.Code) {
                case "M":
                    {
                        var item = new CodeNameClass(SessionLocator.LoggedUserId, SessionLocator.LoggedUserPM.EnglishName);
                        this.UsersFilterList.push(item);
                        this.selectedUserFilter = item;
                        this.GetSelectedOwnerId();

                        if (isUpdatingFilter) {
                            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                        }

                        this.LoadFilteredQueries();
                        break;
                    }

                case "A":
                    {
                        // On Screen will be LOV
                        var item = new CodeNameClass("A", "All Owners");
                        this.UsersFilterList.push(item);

                        if (isUpdatingFilter) {
                            this.OwnerId = null;
                            this.listOfValuesUserId = null;
                            this.selectedUserFilter = null;
                            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                        }

                        else {
                            this.selectedUserFilter = item;
                            this.GetSelectedOwnerId();
                        }

                        this.LoadFilteredQueries();
                        break;
                    }

                default:
                    {
                        var item = new CodeNameClass("A", "All " + this.SelectedBusinessUnitFilter.Name + " Owners");
                        this.UsersFilterList.push(item);

                        var filters = new ApiQueryFilters();
                        filters.PageIndex = 0;
                        filters.PageSize = 100;
                        filters.Filter1Name = "BusinessUnitId";
                        filters.Filter1Value = this.BusinessUnitId;
                        filters.Filter1Operator = "Equals";

                        this.myUserListService.getAllFromCache(filters).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                var loadedUsers: UserList[] = myResponse.Result;

                                if (loadedUsers != null) {
                                    loadedUsers.forEach((list) => {
                                        this.UsersFilterList.push(new CodeNameClass(list.Id, list.EnglishName));
                                    });
                                }
                            }

                            if (isUpdatingFilter) {
                                this.OwnerId = null;
                                this.listOfValuesUserId = null;
                                this.selectedUserFilter = item;
                                LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                            }

                            else {
                                var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_Owner);
                                if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
                                    defaultFilterCode = null;
                                }

                                this.OwnerId = defaultFilterCode;
                                this.listOfValuesUserId = this.OwnerId;

                                if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
                                    this.selectedUserFilter = this.UsersFilterList.filter(d => d.Code == this.OwnerId)[0];
                                }

                                if (this.selectedUserFilter == null) {
                                    this.selectedUserFilter = this.UsersFilterList[0];
                                }
                            }

                            this.LoadFilteredQueries();
                        });

                        break;
                    }
            }
        }
    }

    private listOfValuesUserId: string;
    get ListOfValuesUserId() { return this.listOfValuesUserId; }
    set ListOfValuesUserId(value: string) {
        if (this.listOfValuesUserId != value) {

            this.OwnerId = value;
            this.listOfValuesUserId = value;

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
            this.LoadFilteredQueries();
        }
    }

    GetSelectedOwnerId() {
        var myResult = null;
        this.listOfValuesUserId = null;

        if (this.SelectedUserFilter) {
            switch (this.SelectedUserFilter.Code) {
                case "A": {
                    var defaultFilterCode = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_Owner);
                    if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
                        defaultFilterCode = null;
                    }

                    myResult = defaultFilterCode;
                    this.listOfValuesUserId = myResult;
                    break;
                }

                default: {
                    myResult = this.SelectedUserFilter.Code;
                    this.listOfValuesUserId = myResult;
                    break;
                }
            }
        }

        this.OwnerId = myResult;
    }

    private selectedUserFilter: CodeNameClass;
    get SelectedUserFilter() { return this.selectedUserFilter; }
    set SelectedUserFilter(value: CodeNameClass) {
        if (this.selectedUserFilter != value) {
            this.selectedUserFilter = value;

            this.GetSelectedOwnerId();

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);

            this.LoadFilteredQueries();
        }
    }

    GetSelectedBusinessUnitId() {
        var myResult: string = null;

        if (this.SelectedBusinessUnitFilter) {
            switch (this.SelectedBusinessUnitFilter.Code) {
                case "M": {
                    myResult = SessionLocator.LoggedUserPM.BusinessUnitId;
                    break;
                }

                case "A": {
                    myResult = null;
                    break;
                }

                default: {
                    myResult = this.SelectedBusinessUnitFilter.Code;
                    break;
                }
            }
        }

        this.BusinessUnitId = myResult;
    }

    private ComputeDays() {
        var days;
        var Todate: Date = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject;
        var FromDate: Date = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject;
        if (this.SelectedDateFilter.Code == "0") {
            days = 0;
            this.toDate = Todate;
            this.fromDate = Todate;
        }


        else if (this.SelectedDateFilter.Code == "-1") {
            days = -1;
            FromDate.setDate(Todate.getDate() - 1);
            this.fromDate = FromDate;
            this.toDate = FromDate;
        }


        else if (this.SelectedDateFilter.Code == "-7") {
            days = -7;
            FromDate.setDate(Todate.getDate() - 6);
            this.fromDate = FromDate;
            this.toDate = Todate;
        }

        else if (this.SelectedDateFilter.Code == "-30") {
            days = -30;
            FromDate.setMonth(Todate.getMonth() - 1);
            this.fromDate = FromDate;
            this.toDate = Todate;

        }

        else if (this.SelectedDateFilter.Code == "-90") {
            days = -90;
            FromDate.setMonth(Todate.getMonth() - 3);
            this.fromDate = FromDate;
            this.toDate = Todate;
        }

        else if (this.SelectedDateFilter.Code == "-365") {
            days = -365;
            FromDate.setMonth(Todate.getMonth() - 12);
            this.fromDate = FromDate;
            this.toDate = Todate;
        }

        return days;
    }


    private toDate: Date;
    public get ToDate() { return this.toDate; }
    public set ToDate(value: Date) {
        if (value != this.toDate) {
            this.toDate = value;
            this.SelectedDateFilter = this.DateFilterList[6];
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ByCreateToDate", (value == null ? null : ServiceHelper.GetDateString(value)));
        }
    }

    private fromDate: Date;
    public get FromDate() { return this.fromDate; }
    public set FromDate(value: Date) {
        if (value != this.fromDate) {
            this.fromDate = value;
            this.SelectedDateFilter = this.DateFilterList[6];
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ByCreateFromDate", (value == null ? null : ServiceHelper.GetDateString(value)));
        }
    }


    private selectedDateFilter: CodeNameClass;
    public get SelectedDateFilter() { return this.selectedDateFilter; }
    public set SelectedDateFilter(value: CodeNameClass) {

        if (this.selectedDateFilter != value) {
            this.selectedDateFilter = value;

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_CreateDate, (value == null ? null : value.Code));
            if (value.Code == "-2") {
                LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ByCreateFromDate", (value == null ? null : ServiceHelper.GetDateString(this.FromDate)));
                LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ByCreateToDate", (value == null ? null : ServiceHelper.GetDateString(this.ToDate)));
            }
        }
        this.LoadFilteredQueries();
    }

    public OwnerId: string;
    public BusinessUnitId: string;
    public UsersFilterList: CodeNameClass[] = [];
    public BusinessUnitFilterList: CodeNameClass[] = [];
    BuildBusinessUnitFilter() {
        this.myBusinessUnitListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: BusinessUnitList[] = myResponse.Result;

                this.BusinessUnitFilterList = [];
                this.BusinessUnitFilterList.push(new CodeNameClass("M", "My Records"));

                if (list) {
                    list.filter(d => d.Id != SessionLocator.Tenant.toString()).forEach((item) => {
                        this.BusinessUnitFilterList.push(new CodeNameClass(item.Id, item.Name));
                    });
                }

                this.BusinessUnitFilterList.push(new CodeNameClass("A", "All Records"));

                var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_BusinessUnit);
                if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
                    defaultFilterCode = "M";
                }

                this.selectedBusinessUnitFilter = this.BusinessUnitFilterList.filter(d => d.Code == defaultFilterCode)[0];
                this.GetSelectedBusinessUnitId();
                this.BuildUsersFilters(false);
            }
        });
        this.LoadComponents();
    }

    private PageChild_OQS: any = null;
    private PageChild_QOC: any = null;
    private PageChild_QCV: any = null;
    private PageChild_KPI: any = null;
    private PageChild_TFS: any = null;

    LoadComponents() {
        if (this.isViewInited) {

            let OQSLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "OQS")[0];
            let QOCLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "QOC")[0];
            let QCVLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "QCV")[0];
            let KPILocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "KPI")[0];
            let TFSLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "TFS")[0];

            if (OQSLocation != null) {
                if (this.PageChild_OQS == null) {
                    SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/QuoteDashboardTabComponent/OpenQuotesByStageComponent', OQSLocation.viewContainerRef)
                        .then(cmpRef => {
                            this.PageChild_OQS = cmpRef.instance;
                            this.PageChild_OQS.InitTab(this);
                        });
                }

                else {
                    this.PageChild_OQS.RefreshTab();
                }
            }

            if (QOCLocation != null) {
                if (this.PageChild_QOC == null) {
                    SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/QuoteDashboardTabComponent/QuotesByCountryComponent', QOCLocation.viewContainerRef)
                        .then(cmpRef => {
                            this.PageChild_QOC = cmpRef.instance;
                            this.PageChild_QOC.InitTab(this);
                        });
                }

                else {
                    this.PageChild_QOC.RefreshTab();
                }
            }

            if (QCVLocation != null) {
                if (this.PageChild_QCV == null) {
                    SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/QuoteDashboardTabComponent/QuotesConversionComponent', QCVLocation.viewContainerRef)
                        .then(cmpRef => {
                            this.PageChild_QCV = cmpRef.instance;
                            this.PageChild_QCV.InitTab(this);
                        });
                }

                else {
                    this.PageChild_QCV.RefreshTab();
                }
            }
            if (KPILocation != null) {
                if (this.PageChild_KPI == null) {
                    SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/QuoteDashboardTabComponent/SentQuotesKPIComponent', KPILocation.viewContainerRef)
                        .then(cmpRef => {
                            this.PageChild_KPI = cmpRef.instance;
                            this.PageChild_KPI.InitTab(this);
                        });
                }

                else {
                    this.PageChild_KPI.RefreshTab();
                }
            }
            if (TFSLocation != null) {
                if (this.PageChild_TFS == null) {
                    SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/QuoteDashboardTabComponent/TopFiveSalesmanProfitComponent', TFSLocation.viewContainerRef)
                        .then(cmpRef => {
                            this.PageChild_TFS = cmpRef.instance;
                            this.PageChild_TFS.InitTab(this);
                        });
                }

                else {
                    this.PageChild_TFS.RefreshTab();
                }
            }
        }
    }
}
