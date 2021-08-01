
import {Component, Output, EventEmitter} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {LastFilterClass} from '../../../Infrastructure/Utilities/LastFilterClass';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CodeNameClass} from '../../../Infrastructure/DataContracts/CodeNameClass';
import {AppTool, DateTool, FormatTool} from '../../../Infrastructure/Tools';
import {ListComponentArgs} from '../../../Infrastructure/Args';
import {BusinessUnitListService} from '../../../Common/Services/StandardLists/BusinessUnitListService';
import {UserListService} from '../../../Common/Services/StandardLists/UserListService';
import {BusinessUnitList} from '../../../Common/EntityLists/BusinessUnitList';
import {UserList} from '../../../Common/EntityLists/UserList';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {CRMDomainService, CRMSummary} from '../../Services/CRMDomainService';
import {ActivityList} from '../../EntityLists/ActivityList';
import {UpcomingActivityItem} from '../../Components/Workspaces/UpcomingActivityItem';
import {ActivityInputArgs} from '../../Args';
import {ChartingDataClass} from '../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';
import {CRMTool} from '../../Tools';
declare var makeAmBarChart, BarClick, ResetItem: any;

@Component({
    
    templateUrl: './ActivityWorkspaceComponent.html',
})

export class ActivityWorkspaceComponent extends BaseComponent {
    @Output() ReloadUserQueries = new EventEmitter();
    public DataContext = this;
    public QuickSearchItems: ActivityList[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    public IsActivityRecentListLoaded: boolean = false;
    constructor() {
        super(); 
        this.ChartID = "ChartID_" + this.CurrentSession.GetChartId();
        this.InProgressBookingId = this.InProgressBookingId + this.CurrentSession.GetChartId();
        this.InitializeServices();
        this.SetQueriesVisibility();
        this.LoadNonFilteredQueries();
        this.InitializeFilters();
    }

    private filterName_RecordsType: string = "RecordsType";
    private filterName_CreatedByType: string = "CreatedByType";
    private filterName_Owner: string = "Owner";
    private filterName_BusinessUnit: string = "BusinessUnit";
    private filterControlNameSpace: string = "Logitude.CRM.Views.CRMPages.ActivitiesPageControl";

    private myUserListService: UserListService;
    private myBusinessUnitListService: BusinessUnitListService;
    private myDomainService: CRMDomainService;
    private InitializeServices() {
        this.myDomainService = new CRMDomainService();
        this.myUserListService = new UserListService();
        this.myBusinessUnitListService = new BusinessUnitListService();
    }

    public OwnerId: string = null;
    public BusinessUnitId: string = null;
    public RecordsTypeFilterCode: string = null;
    public CreatedByTypeFilterCode: string = null;
    public BusinessUnitFilterCode: string = null;
    public RecordsTypesFilterList: CodeNameClass[] = [];
    public CreatedByTypesFilterList: CodeNameClass[] = [];
    public BusinessUnitFilterList: CodeNameClass[] = [];
    public BusinessUnitUsersFilterList: CodeNameClass[] = [];
    public IsBusinessUnitUsers: boolean = false;
    InitializeFilters() {

        // Records Types
        this.RecordsTypesFilterList = [];
        this.RecordsTypesFilterList.push(new CodeNameClass("S", "Salesman Records"));
        this.RecordsTypesFilterList.push(new CodeNameClass("C", "Created By Records"));
        this.RecordsTypeFilterCode = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_RecordsType);
        if (AppTool.IsNullOrEmpty(this.RecordsTypeFilterCode)) {
            this.RecordsTypeFilterCode = "S";
        }
        this.selectedRecordsTypeFilter = this.RecordsTypesFilterList.filter(d => d.Code == this.RecordsTypeFilterCode)[0];

        // CreatedBy Types
        this.CreatedByTypesFilterList = [];
        this.CreatedByTypesFilterList.push(new CodeNameClass("M", "Created By Me"));
        this.CreatedByTypesFilterList.push(new CodeNameClass("All", "Created By"));

        // Business Units
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
            }

            this.OnFiltersInitialized();
        });
    }
    OnFiltersInitialized() {
        if (this.RecordsTypeFilterCode == "C") {
            this.CreatedByTypeFilterCode = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_CreatedByType);
            if (AppTool.IsNullOrEmpty(this.CreatedByTypeFilterCode)) {
                this.CreatedByTypeFilterCode = "M";
            }
            this.selectedCreatedByTypeFilter = this.CreatedByTypesFilterList.filter(d => d.Code == this.CreatedByTypeFilterCode)[0];

            if (this.CreatedByTypeFilterCode == "M") {
                this.OwnerId = SessionLocator.LoggedUserId;                
                this.listOfValuesUserId = this.OwnerId;
                this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', false);
            }

            else {
                this.OwnerId = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_Owner);
                this.listOfValuesUserId = this.OwnerId;
                this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', true);
            }

            this.LoadFilteredQueries();
        }

        else {
            this.BusinessUnitFilterCode = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_BusinessUnit);
            if (AppTool.IsNullOrEmpty(this.BusinessUnitFilterCode)) {
                this.BusinessUnitFilterCode = "M";
            }
            this.selectedBusinessUnitFilter = this.BusinessUnitFilterList.filter(d => d.Code == this.BusinessUnitFilterCode)[0];

            switch (this.BusinessUnitFilterCode) {
                case "M": {
                    this.IsBusinessUnitUsers = false;
                    this.BusinessUnitId = SessionLocator.LoggedUserPM.BusinessUnitId;
                    this.OwnerId = SessionLocator.LoggedUserId;
                    this.listOfValuesUserId = this.OwnerId;                    
                    this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', false);
                    this.LoadFilteredQueries();
                    break;
                }

                case "A": {
                    this.IsBusinessUnitUsers = false;
                    this.BusinessUnitId = null;
                    this.OwnerId = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_Owner);
                    this.listOfValuesUserId = this.OwnerId;                    
                    this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', true);
                    this.LoadFilteredQueries();
                    break;
                }

                default: {
                    this.IsBusinessUnitUsers = true;
                    this.BusinessUnitId = this.BusinessUnitFilterCode;

                    var item = new CodeNameClass("A", "All " + this.SelectedBusinessUnitFilter.Name + " Owners");
                    this.BusinessUnitUsersFilterList.push(item);

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
                                    this.BusinessUnitUsersFilterList.push(new CodeNameClass(list.Id, list.EnglishName));
                                });
                            }
                        }

                        var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_Owner);
                        if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
                            defaultFilterCode = null;
                        }

                        this.OwnerId = defaultFilterCode;
                        this.listOfValuesUserId = this.OwnerId;

                        if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
                            this.selectedUserFilter = this.BusinessUnitUsersFilterList.filter(d => d.Code == this.OwnerId)[0];
                        }

                        if (this.selectedUserFilter == null) {
                            this.selectedUserFilter = this.BusinessUnitUsersFilterList[0];
                        }

                        this.LoadFilteredQueries();
                    });

                    break;
                }
            }            
        }
    }

    private selectedRecordsTypeFilter: CodeNameClass;
    get SelectedRecordsTypeFilter() { return this.selectedRecordsTypeFilter; }
    set SelectedRecordsTypeFilter(value: CodeNameClass) {
        if (this.selectedRecordsTypeFilter != value) {
            this.selectedRecordsTypeFilter = value;
            this.IsActivityRecentListLoaded = false;
            this.RecordsTypeFilterCode = value == null ? "S" : value.Code;
            this.BusinessUnitFilterCode = "M";
            this.CreatedByTypeFilterCode = "M";
            this.OwnerId = SessionLocator.LoggedUserId;
            this.BusinessUnitId = this.RecordsTypeFilterCode == "S" ? SessionLocator.LoggedUserPM.BusinessUnitId : null;

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_RecordsType, this.RecordsTypeFilterCode);
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_BusinessUnit, this.BusinessUnitFilterCode);
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_CreatedByType, this.CreatedByTypeFilterCode);
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
            this.OnFiltersInitialized();
        }
    }

    private selectedCreatedByTypeFilter: CodeNameClass;
    get SelectedCreatedByTypeFilter() { return this.selectedCreatedByTypeFilter; }
    set SelectedCreatedByTypeFilter(value: CodeNameClass) {
        if (this.selectedCreatedByTypeFilter != value) {
            this.selectedCreatedByTypeFilter = value;

            this.CreatedByTypeFilterCode = value == null ? "M" : value.Code;
            this.OwnerId = this.CreatedByTypeFilterCode == "M" ? SessionLocator.LoggedUserId : null;
            this.listOfValuesUserId = this.OwnerId;
            this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', this.CreatedByTypeFilterCode == "M" ? false : true);

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_CreatedByType, this.CreatedByTypeFilterCode);
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
            this.LoadFilteredQueries();
        }
    }

    private selectedBusinessUnitFilter: CodeNameClass;
    get SelectedBusinessUnitFilter() { return this.selectedBusinessUnitFilter; }
    set SelectedBusinessUnitFilter(value: CodeNameClass) {
        if (this.selectedBusinessUnitFilter != value) {
            this.selectedBusinessUnitFilter = value;

            this.BusinessUnitFilterCode = value == null ? "M" : value.Code;
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_BusinessUnit, this.BusinessUnitFilterCode);

            switch (this.BusinessUnitFilterCode) {
                case "M": {
                    this.IsBusinessUnitUsers = false;
                    this.BusinessUnitId = SessionLocator.LoggedUserPM.BusinessUnitId;
                    this.OwnerId = SessionLocator.LoggedUserId;
                    this.listOfValuesUserId = this.OwnerId;
                    this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', false);
                    LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                    this.LoadFilteredQueries();
                    break;
                }

                case "A": {
                    this.IsBusinessUnitUsers = false;
                    this.BusinessUnitId = null;
                    this.OwnerId = null;
                    this.listOfValuesUserId = this.OwnerId;
                    this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', true);
                    LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                    this.LoadFilteredQueries();
                    break;
                }

                default: {
                    this.IsBusinessUnitUsers = true;
                    this.BusinessUnitId = this.BusinessUnitFilterCode;
                    this.OwnerId = null;
                    this.listOfValuesUserId = this.OwnerId;

                    this.BusinessUnitUsersFilterList = [];

                    var item = new CodeNameClass("A", "All " + this.SelectedBusinessUnitFilter.Name + " Owners");
                    this.BusinessUnitUsersFilterList.push(item);

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
                                    this.BusinessUnitUsersFilterList.push(new CodeNameClass(list.Id, list.EnglishName));
                                });
                            }
                        }

                        this.selectedUserFilter = this.BusinessUnitUsersFilterList[0];
                        this.LoadFilteredQueries();
                    });

                    break;
                }
            }
        }
    }

    private selectedUserFilter: CodeNameClass;
    get SelectedUserFilter() { return this.selectedUserFilter; }
    set SelectedUserFilter(value: CodeNameClass) {
        if (this.selectedUserFilter != value) {
            this.selectedUserFilter = value;

            var myCode: string = value == null ? "A" : value.Code;
            this.OwnerId = myCode == "A" ? null : myCode;
            this.listOfValuesUserId = this.OwnerId;
            
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
            this.LoadFilteredQueries();
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

    RefreshButtonClicked() {
        this.LoadAllScreenData();
    }
    onUserQueriesBackComplete(event) {
        this.LoadAllScreenData();
    }
    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }

    private selectedActivityFilter: string = "All";
    get SelectedActivityFilter() { return this.selectedActivityFilter; }
    set SelectedActivityFilter(value: string) {
        if (this.selectedActivityFilter != value) {
            this.selectedActivityFilter = value;
            this.LoadAllScreenData();
        }
    }

    LoadAllScreenData() {
        this.LoadFilteredQueries();
        this.LoadNonFilteredQueries();
    }
    LoadFilteredQueries() {
        this.LoadChartData();
        this.LoadDataCounts();
        this.LoadUpcomingEntities();        
    }
    LoadNonFilteredQueries() {
        this.ReloadUsersQuery();
        //this.LoadDataCounts();
    }

    // Queries Features
    public IsQueryVisible_OpenGroup: boolean = false;
    public IsQueryVisible_MyOpen: boolean = false;
    public IsQueryVisible_AllOpen: boolean = false;
    public IsQueryVisible_ClosedGroup: boolean = false;
    public IsQueryVisible_MyClosed: boolean = false;
    public IsQueryVisible_AllClosed: boolean = false;
    public IsQueryVisible_Meetings: boolean = false;
    public IsQueryVisible_OthersGroup: boolean = false;
    public IsQueryVisible_All: boolean = false;
    public IsQueryVisible_Cancelled: boolean = false;
    public IsQueryVisible_MyViewsGroup: boolean = false;
    private SetQueriesVisibility() {
        if (FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.MyOpenActivities")) {
            this.IsQueryVisible_OpenGroup = true;
        }

        else if (FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.AllOpenActivities")) {
            this.IsQueryVisible_OpenGroup = true;
        }

        if (FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.MyOpenActivities")) {
            this.IsQueryVisible_MyOpen = true;
        }

        if (FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.AllOpenActivities")) {
            this.IsQueryVisible_AllOpen = true;
        }

        if (FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.MyClosedActivities")) {
            this.IsQueryVisible_ClosedGroup = true;
        }

        else if (FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.AllClosedActivities")) {
            this.IsQueryVisible_ClosedGroup = true;
        }

        else if (FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.MeetingsSummary")) {
            this.IsQueryVisible_ClosedGroup = true;
        }

        if (FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.MyClosedActivities")) {
            this.IsQueryVisible_MyClosed = true;
        }

        if (FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.AllClosedActivities")) {
            this.IsQueryVisible_AllClosed = true;
        }

        if (FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.MeetingsSummary")) {
            this.IsQueryVisible_Meetings = true;
        }

        if (FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.AllActivities")) {
            this.IsQueryVisible_OthersGroup = true;
        }

        else if (FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.CancelledActivities")) {
            this.IsQueryVisible_OthersGroup = true;
        }

        if (FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.AllActivities")) {
            this.IsQueryVisible_All = true;
        }

        if (FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.CancelledActivities")) {
            this.IsQueryVisible_Cancelled = true;
        }

        if (FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES")) {
            this.IsQueryVisible_MyViewsGroup = true;
        }
    }

    //Load Data Counts
    public MyOpenCount: number;
    public AllOpenCount: number;
    private LoadDataCounts() {
        this.myDomainService.GetActivitiesSummary(this.SelectedActivityFilter, this.OwnerId, this.BusinessUnitId, this.RecordsTypeFilterCode).subscribe((myResult:any) => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {

                var myData: CRMSummary = myResponse.Result;
                if (myData != null) {
                    this.MyOpenCount = myData.MyOpenDataCount;
                    this.AllOpenCount = myData.AllOpenDataCount;
                }
            }
        });
    }

    //Upcoming Activities
    public UpcomingActivitiesCount: number = 0;
    public UpcomingActivitiesList: UpcomingActivityItem[];
    public LoadUpcomingEntities() {
        this.IsActivityRecentListLoaded = false;
        this.myDomainService.GetUpcomigActivities(this.OwnerId, this.BusinessUnitId, this.selectedActivityFilter, this.RecordsTypeFilterCode).subscribe((myResult:any) => {
            if (myResult == null) {
                this.UpcomingActivitiesList = [];
                this.UpcomingActivitiesCount = 0;
            }

            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    var list: ActivityList[] = myResponse.Result;
                    this.FillUpcomingActivitiesList(list);
                }
            }
        });
    }
    private FillUpcomingActivitiesList(myList: ActivityList[]) {
        this.UpcomingActivitiesList = [];
        myList.forEach(item => {
            var itemViewModel: UpcomingActivityItem = new UpcomingActivityItem(item, this, null);
            this.UpcomingActivitiesList.push(itemViewModel);
        });
        this.UpcomingActivitiesCount = this.UpcomingActivitiesList.length;
        //myList.sort((a, b) => {
        //    return (DateTool.GetDateFromDate(a.SortByDate).valueOf() === DateTool.GetDateFromDate(b.SortByDate).valueOf()) ? 0 : (DateTool.GetDateFromDate(a.SortByDate).valueOf() < DateTool.GetDateFromDate(b.SortByDate).valueOf()) ? -1 : 1
        //}).forEach((item) => {
        //    var itemViewModel: UpcomingActivityItem = new UpcomingActivityItem(item, this, null);
        //    this.UpcomingActivitiesList.push(itemViewModel);
        //});
        this.IsActivityRecentListLoaded = true;

    }

    // Chart
    LoadChartData() {
        this.myDomainService.GetActivitiesDashBoard(this.OwnerId, this.BusinessUnitId, this.selectedActivityFilter, this.RecordsTypeFilterCode).subscribe((result:any) => {
            this.InProgressBookingDashboard = result.Result;
            this.FillInProgressBookingDashboardData();
        });
    }
    BarClicking() {
        if (BarClick() != null) {
            this.OnBarClick(BarClick());
            ResetItem();
        }
    }
    OnBarClick(e) {
        var flag = false;
        let item: any;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;
        var Key = e.target.columnIndex;
        var myQueryCode: string = "All Activities";
        var displayName: string = "";
        var myTableName: string = "Activity";
        this.filterAgrs = new ApiQueryFilters();
        if (flag) {
            var typeName = "";
            var typeCode = "";
            switch (Key + "") {
                case "0": { typeName = "Tasks"; typeCode = "TS"; break; }
                case "1": { typeName = "Phone Calls"; typeCode = "CL"; break; }
                case "2": { typeName = "Appointments"; typeCode = "AP"; break; }
            }

            if ((item.category + "").toLowerCase() == "no date") {
                displayName = "No Date " + typeName;
                this.filterAgrs.addAdditionalFilter("ActivityNext7DaysCustomFilter", "no date", null, null, "Equals", true, false, false, "string");
            }

            else if ((item.category + "").toLowerCase() == "old") {
                displayName = "Old " + typeName;
                this.filterAgrs.addAdditionalFilter("ActivityNext7DaysCustomFilter", "old", null, null, "Equals", true, false, false, "string");
            }

            else {
                var date: any = this.InProgressBookingYAxisFilterd[Key].DateTime[item.index];
                var myFormats = DateTool.GetDateFormats(DateTool.GetDateParts(date).DateObject);
                myFormats.DateParts.DateObject.setHours(0, 0, 0, 0);
                var dayName: string = myFormats.DayName;
                displayName = dayName + " " + typeName;
                this.filterAgrs.addAdditionalFilter("ActivityNext7DaysCustomFilter", myFormats.DateParts.DateObject, null, null, "Equals", true, false, false, "date");
            }


        }

        this.filterAgrs.addAdditionalFilter("IsOpen", true, null, null, "Equals", false, false, false, "boolean");
        this.filterAgrs.addAdditionalFilter("ActivityTypeCode", typeCode, null, null, "Equals", false, false, false, "String");

        if (this.RecordsTypeFilterCode == "C") {
            this.filterAgrs.addAdditionalFilter("CreatedByUserId", this.InProgressBookingYAxisFilterd[e.target.columnIndex].OwnerIds[item.index], null, null, "Equals", false, false, false, "String");
        }

        else {
            this.filterAgrs.addAdditionalFilter("OwnerId", this.InProgressBookingYAxisFilterd[e.target.columnIndex].OwnerIds[item.index], null, null, "Equals", false, false, false, "String");
            this.filterAgrs.addAdditionalFilter("BusinessUnitId", this.InProgressBookingYAxisFilterd[Key].BusinessUnitId[item.index], null, null, "Equals", false, false, false, "String");
        }

        var listArgs = new ListComponentArgs();
        listArgs.Filters = this.filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = displayName;
        listArgs.BackButtonTitle = "CRM";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                this.CurrentSession.AddMenuReference(cmpRef);
            });
    }
    FillInProgressBookingDashboardData() {
        var index = 0;
        this.InProgressBookingXAxis = [];

        this.InProgressBookingDashboard.sort((a, b) => { return (a.DateTimeProperty === b.DateTimeProperty) ? 0 : (a.DateTimeProperty < b.DateTimeProperty) ? -1 : 1 });
        var StringArr: Array<string> = new Array<string>();
        var j = 0;
        this.InProgressBookingDashboard.forEach(element => {
            if (!StringArr.includes(element.LabelProperty)) {
                StringArr.push(element.LabelProperty);
                this.InProgressBookingYAxis[j] = { data: [], label: null, BindingElement: [], OwnerIds: [], DateTime: [], BusinessUnitId: [] };
                this.InProgressBookingYAxis[j].data = [];
                j++;
            }
        });

        var Graphs = [];
        var index = 0;
        this.InProgressBookingDashboard = this.InProgressBookingDashboard.filter(element => element.StringProperty == "TS" || element.StringProperty == "CL" || element.StringProperty == "AP");
        this.InProgressBookingDashboard.forEach(element => {
            for (var i = 0; i < StringArr.length; i++) {
                if (element.LabelProperty == StringArr[i]) {
                    var k = 0;
                    if (element.StringProperty == "CL")
                        k = 1;
                    else if (element.StringProperty == "AP")
                        k = 2;

                    if (this.InProgressBookingYAxis[i].data.length == 0)
                        this.InProgressBookingYAxis[i].data = new Array(3);

                    this.InProgressBookingYAxis[i].data[k] = element.IntegerProperty;
                    this.InProgressBookingYAxis[i].label = element.StringProperty;
                    this.InProgressBookingYAxis[i].BindingElement[k] = element.StringProperty;
                    this.InProgressBookingYAxis[i].BusinessUnitId[k] = element.BusinessUnitId;
                    this.InProgressBookingYAxis[i].DateTime[k] = element.DateTimeProperty;
                    this.InProgressBookingYAxis[i].OwnerIds[k] = element.OwnerId;
                    if (!this.InProgressBookingXAxis.includes(element.LabelProperty) && element.LabelProperty != null) {
                        if (this.InProgressBookingXAxis[i] == null)
                            this.InProgressBookingXAxis[i] = (element.LabelProperty);

                    }
                }
            }
        });
        this.InProgressBookingYAxisFilterd = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        if (this.InProgressBookingYAxis.length > 0)
            maximum = this.InProgressBookingYAxis[0].data[0];
        this.InProgressBookingYAxis.forEach(element => {
            for (var i = 0; i < element.data.length; i++) {
                if (this.InProgressBookingYAxisFilterd[i] == null) {
                    this.InProgressBookingYAxisFilterd[i] = { data: [], label: null, BindingElement: [], OwnerIds: [], DateTime: [], BusinessUnitId: [] };
                }
                if (element.data[i] > maximum)
                    maximum = element.data[i];
                this.InProgressBookingYAxisFilterd[i].data.push(element.data[i]);
                this.InProgressBookingYAxisFilterd[i].BindingElement.push(element.BindingElement[i]);
                this.InProgressBookingYAxisFilterd[i].OwnerIds.push(element.OwnerIds[i]);
                this.InProgressBookingYAxisFilterd[i].BusinessUnitId.push(element.BusinessUnitId[i]);
                this.InProgressBookingYAxisFilterd[i].DateTime.push(element.DateTime[i]);
                if (index == 0) {
                    Graphs[i] = {
                        "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "lineAlpha": 0,
                        "id": "AmGraph-1" + i,
                        "title": element.BindingElement[i] + "",
                        "type": "column",
                        "valueField": "col" + (i + 1),
                        // "bulletBorderColor": "#FFFFFF",
                        "fillColors": [this.barChartColors[i].backgroundColor1 + "", this.barChartColors[i].backgroundColor2 + ""],
                        //  "fillColors": ["#ff0000", "#00ff00"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "showHandOnHover": true,

                        //   "plotAreaFillColors": ["#ff0000", "#f1783e", "#00ff00"],
                    };
                }
                objectArray[i] = (element.data[i]);

            }
            DataProvider[index] = { "category": this.InProgressBookingXAxis[index], "col1": objectArray[0], "col2": objectArray[1], "col3": objectArray[2] };
            index++;
        });

        var InProgressBookingDashboardFilterd: Array<ChartingDataClass> = new Array<ChartingDataClass>();
        try {
            if (this.InProgressBookingXAxis.length != 0) {

                maximum += 1;
                while (maximum % 5 != 0) {
                    maximum += 1;

                }
                makeAmBarChart(this.InProgressBookingId, Graphs, DataProvider, maximum);
            }
        }
        catch (e) {

        }
    }
    barChartColors: any[] = [
        {

            backgroundColor1: '#DA7B38',
            backgroundColor2: '#ecbd9b',


            borderWidth: 0
        },

        {
            backgroundColor1: '#21782E',
            backgroundColor2: '#90bb96',



            borderWidth: 0,
        },

        {
            backgroundColor1: '#487E9F',
            backgroundColor2: '#c8d8e2',
            borderWidth: 0,
        },
    ]

    //Dashboard Propereties
    public InProgressBookingYAxis: any[] = [];
    public InProgressBookingYAxisFilterd = [];
    public InProgressBookingXAxis: string[] = [];
    public ChartID: string = null;
    public InProgressBookingDashboard: Array<ChartingDataClass>;
    public InProgressBookingId: string = "InProgressBookingId_";
    public filterAgrs: ApiQueryFilters;
    isResizing: boolean = false;
    ChartLeft: number = 0;
    lastDownX: number = 0;
    lastDownY: number = 0;
    OnMyMouseDown($event, arg) {
        this.isResizing = true;
        var grid = document.getElementById(this.ChartID);
        var rec = grid.getBoundingClientRect();
        this.ChartLeft = rec.left;
        this.lastDownY = ($event.clientY - rec.bottom);
        this.lastDownX = ($event.clientX - this.ChartLeft);
    }

    ViewActivityQuery(code: string) {
        if (code != null) {
            var objectTableName = "Activity";
            var queryCode = null;
            var backButtonTitle = "CRM";

            var filterAgrs: ApiQueryFilters = new ApiQueryFilters();

            if (!AppTool.IsNullOrEmpty(this.SelectedActivityFilter)) {
                if (this.SelectedActivityFilter != "All") {
                    filterAgrs.addAdditionalFilter("ActivityTypeCode", this.SelectedActivityFilter, null, null, "Equals", false, false, false, "String");
                }
            }

            if (this.RecordsTypeFilterCode == "C") {
                if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
                    filterAgrs.addAdditionalFilter("CreatedByUserId", this.OwnerId, null, null, "Equals", false, true, false, "string");
                }       
            }

            else {
                if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
                    filterAgrs.addAdditionalFilter("OwnerId", this.OwnerId, null, null, "Equals", false, true, false, "string");
                }

                if (!AppTool.IsNullOrEmpty(this.BusinessUnitId)) {
                    filterAgrs.addAdditionalFilter("BusinessUnitId", this.BusinessUnitId, null, null, "Equals", false, true, false, "string");
                }
            }

            switch (code) {
                case "Open:My":
                    {
                        queryCode = "My Open Activities";
                        break;
                    }

                case "Open:All":
                    {
                        queryCode = "All Open Activities";
                        break;
                    }

                case "Closed:My":
                    {
                        queryCode = "My Closed Activities";
                        break;
                    }

                case "Closed:All":
                    {
                        queryCode = "All Closed Activities";
                        break;
                    }

                case "All":
                    {
                        queryCode = "All Activities";
                        break;
                    }

                case "Meetings":
                    {
                        queryCode = "Meetings Summary";
                        break;
                    }

                case "Cancelled":
                    {
                        queryCode = "Cancelled Activities";
                        break;
                    }

                default: { break; }
            }
            
            var listArgs = new ListComponentArgs();
            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.BackButtonTitle = backButtonTitle;
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                    this.CurrentSession.AddMenuReference(cmpRef);
                });;
        }
    }
    NewActivityClicked(typeCode: string) {
        var windowTitle = "";
        var windowTitleIcon = "";
        var path = "";
        var logWindow = new LogitudeWindow();

        switch (typeCode) {
            case "TS":
                {
                    windowTitle = "New Task";
                    break;
                }

            case "CL": {
                windowTitle = "New Phone Call";
                break;
            }

            case "AP": {
                windowTitle = "New Appointment";
                logWindow.Width = 800;
                logWindow.Height = 600;
                break;
            }
            default: { break; }
        }

        windowTitleIcon = CRMTool.GetActivityImageSrc(typeCode);
        var windowArgs: ActivityInputArgs = new ActivityInputArgs();
        windowArgs.TypeCode = typeCode;
        windowArgs.IsAddCustomerAllowed = true;
                
        logWindow.Title = windowTitle;
        logWindow.TitleIcon = windowTitleIcon;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CRMModules/CRMActivity/Components/NewEntity/NewActivityComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.LoadAllScreenData();
            }
        });
    }        
    EditActivity(entity: any) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Activity', BackButtonLabel: "Activities" });
                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    this.LoadAllScreenData();
                });
            });
    }
    Updated(arg: boolean) {
        if (arg) {
            this.LoadUpcomingEntities();
        }
    }
}
