import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CodeNameClass} from '../../../Infrastructure/DataContracts/CodeNameClass';
import {LastFilterClass} from '../../../Infrastructure/Utilities/LastFilterClass';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {DateTimeToDatePipe} from '../../../Controls/Pipes/DateTimeToDatePipe';
import {AppTool, DateTool, FontTool} from '../../../Infrastructure/Tools';
import {ListComponentArgs} from '../../../Infrastructure/Args';
import {CRMDomainService, DailySpotlightClass} from '../../Services/CRMDomainService';
import {OpportunityListService} from '../../Services/StandardLists/OpportunityListService';
import {ActivityList} from '../../EntityLists/ActivityList';
import {OpportunityList} from '../../EntityLists/OpportunityList';
import {UpcomingActivityItem} from '../../Components/Workspaces/UpcomingActivityItem';
import {BusinessUnitListService} from '../../../Common/Services/StandardLists/BusinessUnitListService';
import {UserListService} from '../../../Common/Services/StandardLists/UserListService';
import {BusinessUnitList} from '../../../Common/EntityLists/BusinessUnitList';
import {UserList} from '../../../Common/EntityLists/UserList';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ActivityInputArgs} from '../../Args';
import {ActivityPM} from '../../EntityPMs/ActivityPM';
import {CRMWorkspaceComponent} from './CRMWorkspaceComponent';
import {ContactListService} from '../../../Common/Services/StandardLists/ContactListService';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
declare var makeChart, FunnelClick, ResetItemFunnel;

@Component({
    moduleId: module.id,
    templateUrl: './OverviewWorkspaceComponent.html',
})

export class OverviewWorkspaceComponent extends BaseComponent {
    public SalesFunnelId: string = "SalesFunnelId_";
    public DataContext = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService) {
        super();
        this.SalesFunnelId = "SalesFunnel_" + this.CurrentSession.GetNewId("SalesFunnel");
        this.InitializeServices();
        this.LoadNonFilteredQueries();
        this.BuildFilters();
        this.InitializeFilters();
    }

    public FatherComp: CRMWorkspaceComponent;
    InitComponent(father: CRMWorkspaceComponent) {
        this.FatherComp = father;
    }

    private filterName_RecordsType: string = "RecordsType";
    private filterName_CreatedByType: string = "CreatedByType";
    private filterName_Owner: string = "Owner";
    private filterName_BusinessUnit: string = "BusinessUnit";
    private filterName_SalesFunnel: string = "SalesFunnel";
    private filterName_TopOpportunities: string = "TopOpportunities";
    private filterControlNameSpace: string = "Logitude.CRM.Views.CRMPages.OverviewPageControl";

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

        this.RecordsTypeFilterCode = "S";

        // Records Types
        //this.RecordsTypesFilterList = [];
        //this.RecordsTypesFilterList.push(new CodeNameClass("S", "Salesmen Records"));
        //this.RecordsTypesFilterList.push(new CodeNameClass("C", "Created By Records"));
        //this.RecordsTypeFilterCode = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_RecordsType);
        //if (AppTool.IsNullOrEmpty(this.RecordsTypeFilterCode)) {
        //    this.RecordsTypeFilterCode = "S";
        //}
        //this.selectedRecordsTypeFilter = this.RecordsTypesFilterList.filter(d => d.Code == this.RecordsTypeFilterCode)[0];

        // CreatedBy Types
        //this.CreatedByTypesFilterList = [];
        //this.CreatedByTypesFilterList.push(new CodeNameClass("M", "Created By Me"));
        //this.CreatedByTypesFilterList.push(new CodeNameClass("All", "Created By"));

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

    BuildFilters() {
        this.BuildFunnelFilters();
        this.BuildTopOpportunitiesFilters();
    }

    //Funnel Filter
    public FunneFilterList: CodeNameClass[];
    private BuildFunnelFilters() {
        this.FunneFilterList = [];
        this.FunneFilterList.push(new CodeNameClass("CNT", "Count"));
        this.FunneFilterList.push(new CodeNameClass("SHI", TextCodeTranslator.Translate("Opportunity.F.NumberOfShipments")));

        var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_SalesFunnel);
        if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "CNT";
        }

        this.selectedFunnelFilter = this.FunneFilterList.filter(d => d.Code == defaultFilterCode)[0];
    }

    private selectedFunnelFilter: CodeNameClass;
    get SelectedFunnelFilter() { return this.selectedFunnelFilter; }
    set SelectedFunnelFilter(value: CodeNameClass) {
        if (this.selectedFunnelFilter != value) {
            this.selectedFunnelFilter = value;

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_SalesFunnel, (value == null ? null : value.Code));
            this.LoadFunnelData();
        }
    }

    //Top Opportunities Filter
    public TopOpportunitiesFilterList: CodeNameClass[];
    private BuildTopOpportunitiesFilters() {
        this.TopOpportunitiesFilterList = [];
        this.TopOpportunitiesFilterList.push(new CodeNameClass("SHI", TextCodeTranslator.Translate("Opportunity.F.NumberOfShipments")));
        this.TopOpportunitiesFilterList.push(new CodeNameClass("STG", "Stage"));
        this.TopOpportunitiesFilterList.push(new CodeNameClass("RAT", "Rating"));
        this.TopOpportunitiesFilterList.push(new CodeNameClass("EST", "Est. Closing Date"));
        this.TopOpportunitiesFilterList.push(new CodeNameClass("DUE", "Stage Due Date"));

        var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_TopOpportunities);
        if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "STG";
        }

        this.selectedTopOpportunitiesFilter = this.TopOpportunitiesFilterList.filter(d => d.Code == defaultFilterCode)[0];
    }

    private selectedTopOpportunitiesFilter: CodeNameClass;
    get SelectedTopOpportunitiesFilter() { return this.selectedTopOpportunitiesFilter; }
    set SelectedTopOpportunitiesFilter(value: CodeNameClass) {
        if (this.selectedTopOpportunitiesFilter != value) {
            this.selectedTopOpportunitiesFilter = value;

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_TopOpportunities, (value == null ? null : value.Code));
            this.LoadTopOpportunities();
        }
    }

    RefreshButtonClicked() {
        this.LoadAllScreenData();
    }

    LoadAllScreenData() {
        this.LoadFilteredQueries();
        this.LoadNonFilteredQueries();
    }
    LoadFilteredQueries() {
        this.LoadActivitiesSummary();
        this.LoadTopOpportunities();
        this.LoadFunnelData();
        this.LoadSpotLightData();

    }
    LoadNonFilteredQueries() {
        this.RunUpcomingBirthdaysFilter();
    }


    // Upcoming Activities
    public UpcomingActivitiesCount: number = 0;
    public UpcomingActivitiesList: UpcomingActivityItem[];
    public LoadActivitiesSummary() {
        this.myDomainService.GetUpcomigActivities(this.OwnerId, this.BusinessUnitId, null, this.RecordsTypeFilterCode).subscribe(myResult => {
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
            var itemViewModel: UpcomingActivityItem = new UpcomingActivityItem(item, null, this);
            this.UpcomingActivitiesList.push(itemViewModel);
        });
        this.UpcomingActivitiesCount = this.UpcomingActivitiesList.length;
        //if (this.UpcomingActivitiesList == null) {
        //    this.UpcomingActivitiesList = new Array<UpcomingActivityItem>();
        //}

        //else {
        //    this.UpcomingActivitiesList = [];
        //}

        //myList.sort((a, b) => {
        //    return (DateTool.GetDateFromDate(a.SortByDate) === DateTool.GetDateFromDate(b.SortByDate)) ? 0 : (DateTool.GetDateFromDate(a.SortByDate) < DateTool.GetDateFromDate(b.SortByDate)) ? -1 : 1
        //}).forEach((item) => {
        //    var itemViewModel: UpcomingActivityItem = new UpcomingActivityItem(item, null, this);
        //    this.UpcomingActivitiesList.push(itemViewModel);
        //});

        //this.UpcomingActivitiesCount = this.UpcomingActivitiesList.length;
    }
    NewActivityClicked(code: string) {
        var logWindow = new LogitudeWindow();
        var windowTitle = "";
        var windowTitleIcon = "";
        var args = new ActivityInputArgs();
        args.Activity = this.EntityPM;
        args.TypeCode = code.toUpperCase();
        logWindow.WindowArgs = args;
        switch (code.toUpperCase()) {
            case "TS":
                {
                    windowTitle = "New Task";
                    windowTitleIcon = "./Images/Activities/TS.png";
                    break;
                }

            case "CL": {
                windowTitle = "New Phone Call";
                windowTitleIcon = "./Images/Activities/CL.png";
                break;
            }

            case "AP": {
                windowTitle = "New Appointment";
                windowTitleIcon = "./Images/Activities/AP.png";
                logWindow.Width = 800;
                logWindow.Height = 600;
                break;
            }
            default: { break; }
        }
        var windowArgs: ActivityInputArgs = new ActivityInputArgs();
        windowArgs.TypeCode = code;
        windowArgs.IsAddCustomerAllowed = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = windowTitle;
        logWindow.TitleIcon = windowTitleIcon;
        logWindow.Show('./CRMModules/CRMActivity/Components/NewEntity/NewActivityComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s == "ok") {
                this.LoadAllScreenData();
            }
        });
    }
    Updated(arg: boolean) {
        if (arg) {
            this.LoadActivitiesSummary();
        }
    }
    EditActivity(entity: any) {
        if (entity != null) {
            this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Activity', BackButtonLabel: "CRM" });
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                            this.LoadActivitiesSummary();
                        });
                    });
            });
        }
    }

    //Spot Light
    private SpotlightData: DailySpotlightClass;
    private LoadSpotLightData() {
        this.myDomainService.GetCRMDailySpotlightCounts(this.OwnerId, SessionLocator.LoggedUserPM.BusinessUnitId, this.RecordsTypeFilterCode).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                this.SpotlightData = myResponse.Result;

                if (this.SpotlightData != null) {
                    this.SetNumbersProperties();
                    this.SetEnabledProperties();
                }
            }
        });
    }

    public Quotes_TodayIsEnabled: boolean = false;
    public Quotes_YesterdayIsEnabled: boolean = false;
    public Quotes_LastWeekIsEnabled: boolean = false;
    public Potentials_TodayIsEnabled: boolean = false;
    public Potentials_YesterdayIsEnabled: boolean = false;
    public Potentials_LastWeekIsEnabled: boolean = false;
    public Customers_TodayIsEnabled: boolean = false;
    public Customers_YesterdayIsEnabled: boolean = false;
    public Customers_LastWeekIsEnabled: boolean = false;
    public Activities_TodayIsEnabled: boolean = false;
    public Activities_YesterdayIsEnabled: boolean = false;
    public Activities_LastWeekIsEnabled: boolean = false;
    public Opportunities_TodayIsEnabled: boolean = false;
    public Opportunities_YesterdayIsEnabled: boolean = false;
    public Opportunities_LastWeekIsEnabled: boolean = false;
    private SetEnabledProperties() {
        this.Quotes_TodayIsEnabled = this.Quotes_Today != "0";
        this.Quotes_YesterdayIsEnabled = this.Quotes_Yesterday != "0";
        this.Quotes_LastWeekIsEnabled = this.Quotes_LastWeek != "0";

        this.Potentials_TodayIsEnabled = this.Potentials_Today != "0";
        this.Potentials_YesterdayIsEnabled = this.Potentials_Yesterday != "0";
        this.Potentials_LastWeekIsEnabled = this.Potentials_LastWeek != "0";

        this.Customers_TodayIsEnabled = this.Customers_Today != "0";
        this.Customers_YesterdayIsEnabled = this.Customers_Yesterday != "0";
        this.Customers_LastWeekIsEnabled = this.Customers_LastWeek != "0";

        this.Activities_TodayIsEnabled = this.Activities_Today != "0";
        this.Activities_YesterdayIsEnabled = this.Activities_Yesterday != "0";
        this.Activities_LastWeekIsEnabled = this.Activities_LastWeek != "0";

        this.Opportunities_TodayIsEnabled = this.Opportunities_Today != "0";
        this.Opportunities_YesterdayIsEnabled = this.Opportunities_Yesterday != "0";
        this.Opportunities_LastWeekIsEnabled = this.Opportunities_LastWeek != "0";
    }

    public Quotes_Today: string = "0";
    public Quotes_Yesterday: string = "0";
    public Quotes_LastWeek: string = "0";
    public Potentials_Today: string = "0";
    public Potentials_Yesterday: string = "0";
    public Potentials_LastWeek: string = "0";
    public Customers_Today: string = "0";
    public Customers_Yesterday: string = "0";
    public Customers_LastWeek: string = "0";
    public Activities_Today: string = "0";
    public Activities_Yesterday: string = "0";
    public Activities_LastWeek: string = "0";
    public Opportunities_Today: string = "0";
    public Opportunities_Yesterday: string = "0";
    public Opportunities_LastWeek: string = "0";
    private SetNumbersProperties() {
        this.Quotes_Today = this.SpotlightData.Quotes_Today.toString();
        this.Quotes_Yesterday = this.SpotlightData.Quotes_Yesterday.toString();
        this.Quotes_LastWeek = this.SpotlightData.Quotes_LastWeek.toString();

        this.Potentials_Today = this.SpotlightData.PotentialCustomers_Today.toString();
        this.Potentials_Yesterday = this.SpotlightData.PotentialCustomers_Yesterday.toString();
        this.Potentials_LastWeek = this.SpotlightData.PotentialCustomers_LastWeek.toString();

        this.Customers_Today = this.SpotlightData.Customers_Today.toString();
        this.Customers_Yesterday = this.SpotlightData.Customers_Yesterday.toString();
        this.Customers_LastWeek = this.SpotlightData.Customers_LastWeek.toString();

        this.Activities_Today = this.SpotlightData.Activities_Today.toString();
        this.Activities_Yesterday = this.SpotlightData.Activities_Yesterday.toString();
        this.Activities_LastWeek = this.SpotlightData.Activities_LastWeek.toString();

        this.Opportunities_Today = this.SpotlightData.Opportunities_Today.toString();
        this.Opportunities_Yesterday = this.SpotlightData.Opportunities_Yesterday.toString();
        this.Opportunities_LastWeek = this.SpotlightData.Opportunities_LastWeek.toString();
    }

    DailySpotLightClicked(queryCode: string) {
        var myQueryCode: string = "";
        var displayName: string = "";
        var myTableName: string = "";

        var myOwnerId = null;
        if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
            myOwnerId = this.OwnerId;
        }

        var myBusinessUnitId = null;
        if (!AppTool.IsNullOrEmpty(SessionLocator.LoggedUserPM.BusinessUnitId)) {
            myBusinessUnitId = SessionLocator.LoggedUserPM.BusinessUnitId;
        }

        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;

        switch (queryCode) {
            case "QT_TD":
                {
                    myTableName = "Quote";
                    myQueryCode = "All Quotes";
                    displayName = "Today Quotes";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Quotes Zoom");

                    filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");

                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }

                    else {
                        filters.addAdditionalFilter("SalesmanUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }

                    break;
                }

            case "QT_YS":
                {
                    myTableName = "Quote";
                    myQueryCode = "All Quotes";
                    displayName = "Yesterday Quotes";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Quotes Zoom");

                    filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");

                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }

                    else {
                        filters.addAdditionalFilter("SalesmanUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }

                    break;
                }

            case "QT_LW":
                {
                    myTableName = "Quote";
                    myQueryCode = "All Quotes";
                    displayName = "Last Week Quotes";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Quotes Zoom");

                    filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");

                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }

                    else {
                        filters.addAdditionalFilter("SalesmanUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }

                    break;
                }

            case "PO_TD":
                {
                    myTableName = "Customer";
                    myQueryCode = "ShippersAndConsignees";
                    displayName = "Today Potential Customers";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Potential Customers Zoom");

                    filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("CustomerStatusCode", "POT", null, null, "Equals", false, false, false, "string");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");

                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }

                    else {
                        filters.addAdditionalFilter("CustomersBusinessUnitFilter", myOwnerId, myBusinessUnitId, null, "Equals", true, false, false, "string");
                    }

                    break;
                }

            case "PO_YS":
                {
                    myTableName = "Customer";
                    myQueryCode = "ShippersAndConsignees";
                    displayName = "Yesterday Potential Customers";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Potential Customers Zoom");

                    filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("CustomerStatusCode", "POT", null, null, "Equals", false, false, false, "string");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");

                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }

                    else {
                        filters.addAdditionalFilter("CustomersBusinessUnitFilter", myOwnerId, myBusinessUnitId, null, "Equals", true, false, false, "string");
                    }

                    break;
                }

            case "PO_LW":
                {
                    myTableName = "Customer";
                    myQueryCode = "ShippersAndConsignees";
                    displayName = "Last Week Potential Customers";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Potential Customers Zoom");

                    filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("CustomerStatusCode", "POT", null, null, "Equals", false, false, false, "string");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");

                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }

                    else {
                        filters.addAdditionalFilter("CustomersBusinessUnitFilter", myOwnerId, myBusinessUnitId, null, "Equals", true, false, false, "string");
                    }

                    break;
                }

            case "CS_TD":
                {
                    myTableName = "Customer";
                    myQueryCode = "Customers";
                    displayName = "Today Customers";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Customers Zoom");

                    filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("CustomerStatusCode", "ACT", null, null, "Equals", false, false, false, "string");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");

                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }

                    else {
                        filters.addAdditionalFilter("CustomersBusinessUnitFilter", myOwnerId, myBusinessUnitId, null, "Equals", true, false, false, "string");
                    }

                    break;
                }

            case "CS_YS":
                {
                    myTableName = "Customer";
                    myQueryCode = "Customers";
                    displayName = "Yesterday Customers";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Customers Zoom");

                    filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("CustomerStatusCode", "ACT", null, null, "Equals", false, false, false, "string");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");

                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }

                    else {
                        filters.addAdditionalFilter("CustomersBusinessUnitFilter", myOwnerId, myBusinessUnitId, null, "Equals", true, false, false, "string");
                    }

                    break;
                }

            case "CS_LW":
                {
                    myTableName = "Customer";
                    myQueryCode = "Customers";
                    displayName = "Last Week Customers";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Customers Zoom");

                    filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("CustomerStatusCode", "ACT", null, null, "Equals", false, false, false, "string");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");

                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }

                    else {
                        filters.addAdditionalFilter("CustomersBusinessUnitFilter", myOwnerId, myBusinessUnitId, null, "Equals", true, false, false, "string");
                    }

                    break;
                }

            case "AC_TD":
                {
                    myTableName = "Activity";
                    myQueryCode = "All Activities";
                    displayName = "Today Activities";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Activities Zoom");

                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");

                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }

                    else {
                        filters.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }

                    break;
                }

            case "AC_YS":
                {
                    myTableName = "Activity";
                    myQueryCode = "All Activities";
                    displayName = "Yesterday Activities";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Activities Zoom");

                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");

                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }

                    else {
                        filters.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }

                    break;
                }

            case "AC_LW":
                {
                    myTableName = "Activity";
                    myQueryCode = "All Activities";
                    displayName = "Last Week Activities";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Activities Zoom");

                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");

                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }

                    else {
                        filters.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }

                    break;
                }

            case "OP_TD":
                {
                    myTableName = "Opportunity";
                    myQueryCode = "All Opportunities";
                    displayName = "Today Opportunities";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Opportunities Zoom");

                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");

                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }

                    else {
                        filters.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }

                    break;
                }

            case "OP_YS":
                {
                    myTableName = "Opportunity";
                    myQueryCode = "All Opportunities";
                    displayName = "Yesterday Opportunities";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Opportunities Zoom");

                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");

                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }

                    else {
                        filters.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }

                    break;
                }

            case "OP_LW":
                {
                    myTableName = "Opportunity";
                    myQueryCode = "All Opportunities";
                    displayName = "Last Week Opportunities";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Opportunities Zoom");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");

                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }

                    else {
                        filters.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }

                    break;
                }
        }

        if (!AppTool.IsNullOrEmpty(myQueryCode)) {
            var listArgs = new ListComponentArgs();
            listArgs.Filters = filters;
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = myTableName;
            listArgs.DisplayTitle = displayName;
            listArgs.BackButtonTitle = "CRM";
            listArgs.ShowViews = false;
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);

                });
        }
    }

    EditOpportunity(entity: any) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.entityList.Id, ObjectTableName: 'Opportunity', BackButtonLabel: "CRM" });
                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    this.LoadAllScreenData();
                    //this.isWindowOpened = false;
                });
            });
    }
    public FunnelData: any;
    public FunnelDataFilterd = [];

    private LoadFunnelData() {
        this.myDomainService.GetStageFunnelData(this.OwnerId, this.BusinessUnitId, this.SelectedFunnelFilter.Code, this.RecordsTypeFilterCode).subscribe((myResult: any) => {
            this.FunnelData = myResult.Result;
            this.fillFunnelData();
        });
    }
    fillFunnelData() {
        try {
            var labelArr: Array<any> = [];
            var dataArr: Array<any> = [];
            this.FunnelDataFilterd = [];
            var i = 0;
            var sum = 0;

            this.FunnelData.forEach(p => {
                this.FunnelDataFilterd[i] = { title: p.LabelProperty, value: p.DecimalProperty };
                i++;
                sum += p.DecimalProperty;
            });

            makeChart(this.SalesFunnelId, this.FunnelDataFilterd, sum);
        }

        catch (e) { }
    }
    FunnelClick() {
        var item = FunnelClick();
        ResetItemFunnel();

        if (item != null) {
            console.log(item);

            var objectTableName = "Opportunity";
            var queryCode = "All Open Opportunities";
            var displayTitle = this.FunnelData[item.index].LabelProperty + " Opportunities";
            var backButtonTitle = "CRM";

            if (this.FunnelData[item.index].DataTypeCode == "SHI") {
                displayTitle += " (" + this.FunnelData[item.index].DecimalProperty + " Shipments)";
            }

            var filterAgrs: ApiQueryFilters = new ApiQueryFilters();
            var listArgs = new ListComponentArgs();
            var myOwnerId = null;
            var myBusinessUnitId = null;
            var myFilterCode = null;

            if (this.FunnelData[item.index].OwnerId != null && this.FunnelData[item.index].OwnerId != "") {
                myOwnerId = this.FunnelData[item.index].OwnerId;
            }

            if (this.FunnelData[item.index].BusinessUnitId != null && this.FunnelData[item.index].BusinessUnitId != "") {
                myBusinessUnitId = this.FunnelData[item.index].BusinessUnitId;
            }

            if (this.FunnelData[item.index].DataTypeCode != null && this.FunnelData[item.index].DataTypeCode != "") {
                myFilterCode = this.FunnelData[item.index].DataTypeCode;
            }

            filterAgrs.addAdditionalFilter("StageId", this.FunnelData[item.index].GroupedId, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("FunnelFilterCode", myFilterCode, null, null, "Equals", true, false, false, "string");

            if (this.RecordsTypeFilterCode == "C") {
                filterAgrs.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", true, false, false, "Boolean");
            }

            else {
                filterAgrs.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", true, false, false, "Boolean");
                filterAgrs.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", true, false, false, "string");
            }

            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                        this.CurrentSession.AddMenuReference(cmpRef);
                    });
            });
        }
    }


    public TopOpportunitiesList: TopOpportunityItem[];
    private LoadTopOpportunities() {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 10;
        filters.SortDirection = "Descending";

        var myOwnerId = null;
        if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
            myOwnerId = this.OwnerId;
        }

        var myBusinessUnitId = null;
        if (!AppTool.IsNullOrEmpty(this.BusinessUnitId)) {
            myBusinessUnitId = this.BusinessUnitId;
        }

        filters.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "boolean");
        filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");

        if (this.RecordsTypeFilterCode == "C") {
            filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
        }

        else {
            filters.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", false, false, false, "string");
            filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
        }

        switch (this.SelectedTopOpportunitiesFilter.Code) {
            case "STG":
                {
                    filters.SortBy = "StageProbability";
                    break;
                }

            case "SHI":
                {
                    filters.SortBy = "NumberOfShipments";
                    break;
                }

            case "RAT":
                {
                    filters.SortBy = "RatingCode";
                    break;
                }

            case "EST":
                {
                    filters.SortBy = "EstimatedClosingDate";
                    break;
                }

            case "DUE":
                {
                    filters.SortBy = "StageDueDate";
                    break;
                }
        }

        var myService: OpportunityListService = new OpportunityListService();
        myService.getByFilters(filters).subscribe(myResult => {
            if (myResult == null) {
                this.TopOpportunitiesList = [];
            }

            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    var list: OpportunityList[] = myResponse.Result;
                    this.FillTopOpportunitiesList(list);
                }
            }
        });
    }
    private FillTopOpportunitiesList(myList: OpportunityList[]) {
        if (this.TopOpportunitiesList == null) {
            this.TopOpportunitiesList = new Array<TopOpportunityItem>();
        }

        else {
            this.TopOpportunitiesList = [];
        }

        switch (this.SelectedTopOpportunitiesFilter.Code) {
            case "STG":
                {
                    var tempList: OpportunityList[] = myList.sort((a, b) => { return b.RatingIndexOrder - a.RatingIndexOrder });
                    tempList.sort((a, b) => { return b.StageProbability - a.StageProbability }).forEach((item) => {
                        var itemViewModel: TopOpportunityItem = new TopOpportunityItem(item, this.SelectedTopOpportunitiesFilter.Code);
                        this.TopOpportunitiesList.push(itemViewModel);
                    });

                    break;
                }
            case "SHI":
                {
                    myList.sort((a, b) => { return b.NumberOfShipments - a.NumberOfShipments }).forEach((item) => {
                        var itemViewModel: TopOpportunityItem = new TopOpportunityItem(item, this.SelectedTopOpportunitiesFilter.Code);
                        this.TopOpportunitiesList.push(itemViewModel);
                    });

                    break;
                }
            case "RAT":
                {
                    var tempList: OpportunityList[] = myList;//.sort((a, b) => { return b.StageName - a.StageName });
                    tempList.sort((a, b) => { return b.RatingIndexOrder - a.RatingIndexOrder }).forEach((item) => {
                        var itemViewModel: TopOpportunityItem = new TopOpportunityItem(item, this.SelectedTopOpportunitiesFilter.Code);
                        this.TopOpportunitiesList.push(itemViewModel);
                    });

                    break;
                }
            case "EST":
                {
                    //.sort((a, b) => { return b.EstimatedClosingDate.valueOf() - a.EstimatedClosingDate.valueOf() })
                    myList.forEach((item) => {
                        var itemViewModel: TopOpportunityItem = new TopOpportunityItem(item, this.SelectedTopOpportunitiesFilter.Code);
                        this.TopOpportunitiesList.push(itemViewModel);
                    });

                    break;
                }
            case "DUE":
                {
                    //.sort((a, b) => { return b.StageDueDate.valueOf() - a.StageDueDate.valueOf() })
                    myList.forEach((item) => {
                        var itemViewModel: TopOpportunityItem = new TopOpportunityItem(item, this.SelectedTopOpportunitiesFilter.Code);
                        this.TopOpportunitiesList.push(itemViewModel);
                    });

                    break;
                }
        }
    }

    // LoadUpcomingBirthdays
    private RunUpcomingBirthdaysFilter() {
        var defaultFilterCode = LastFilterClass.GetFilterValue("Logitude.CRM.Views.CRMPages.ContactsPageControl", "ViewUpcomingBirthdays");
        if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "W05";
        }
        switch (defaultFilterCode) {
            case "W05":
                {
                    this.LoadUpcomingBirthdays(0, 5);
                    break;
                }
            case "W10":
                {
                    this.LoadUpcomingBirthdays(0, 10);
                    break;
                }
            case "W30":
                {
                    this.LoadUpcomingBirthdays(0, 30);
                    break;
                }
            case "TOD":
                {
                    this.LoadUpcomingBirthdays(0, 0);
                    break;
                }
        }
    }
    private LoadUpcomingBirthdays(start: number, end: number) {
        var filters = new ApiQueryFilters();
        filters.addAdditionalFilter("UpcomingBirthdaysFilter", start, end, null, "Equals", true, true, false, "number");
        filters.PageIndex = 0;
        filters.PageSize = 10;
        filters.GetCount = true;
        var service = new ContactListService();
        service.getByFilters(filters).subscribe(myResult => {
            if (myResult != null) {
                this.FatherComp.UpcomingCount = myResult.Count;
                if (myResult.Count != 0) {
                    this.FatherComp.UpcomingCountVisibility = true;
                }
                else {
                    this.FatherComp.UpcomingCountVisibility = false;
                }
            }
        });
    }
}
export class TopOpportunityItem {
    public entityList: OpportunityList;
    private filterCode: string;
    constructor(entityList: OpportunityList, filterCode: string) {
        this.entityList = entityList;
        this.filterCode = filterCode;
        this.ComputeFilterValue();
    }

    get RatingCode() { return this.entityList.RatingCode; }
    get RatingName() { return this.entityList.RatingName; }
    get Topic() { return this.entityList.Subject; }
    get CustomerName() { return this.entityList.CustomerName; }
    get StageName() { return this.entityList.StageName; }
    get StageAge() { return this.entityList.LastStageDate; }

    public FilterValue: string;
    private ComputeFilterValue() {
        var myResult: string = null;

        switch (this.filterCode) {
            case "CRD":
                {
                    myResult = DateTimeToDatePipe.Pipe(this.entityList.CreateDate);
                    break;
                }

            case "EST":
                {
                    myResult = DateTimeToDatePipe.Pipe(this.entityList.EstimatedClosingDate);
                    break;
                }

            case "DUE":
                {
                    myResult = DateTimeToDatePipe.Pipe(this.entityList.StageDueDate);
                    break;
                }

            case "SHI":
                {
                    myResult = this.entityList.NumberOfShipments + "";
                    break;
                }

            case "RAT":
                {
                    myResult = this.entityList.RatingName;
                    break;
                }
        }

        this.FilterValue = myResult;
    }
}
