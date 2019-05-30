/// <reference path="../../../infrastructure/Utilities/featurelocator.ts" />
import {Component, Output, EventEmitter} from '@angular/core';
import {CRMDomainService, CRMSummary} from '../../Services/CRMDomainService';
import {OpportunityList} from '../../EntityLists/OpportunityList';
import {OpportunityListService} from '../../Services/StandardLists/OpportunityListService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CodeNameClass} from '../../../Infrastructure/DataContracts/CodeNameClass';
import {LastFilterClass} from '../../../Infrastructure/Utilities/LastFilterClass';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {DateTimeToDatePipe} from '../../../Controls/Pipes/DateTimeToDatePipe';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {BusinessUnitListService} from '../../../Common/Services/StandardLists/BusinessUnitListService';
import {BusinessUnitList} from '../../../Common/EntityLists/BusinessUnitList';
import {UserListService} from '../../../Common/Services/StandardLists/UserListService';
import {UserList} from '../../../Common/EntityLists/UserList';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ListComponentArgs} from '../../../Infrastructure/Args';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
declare var makeChart, FunnelClick, ResetItem;

@Component({
    moduleId: module.id,
    templateUrl: './OpportunityWorkspaceComponent.html',
})

export class OpportunityWorkspaceComponent extends BaseComponent {
    public SalesFunnelId: string;
    public DataContext = this;
    public QuickSearchItems: OpportunityList[] = [];
    @Output() ReloadUserQueries = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.SalesFunnelId = "SalesFunnel_" + this.CurrentSession.GetNewId("SalesFunnel");
        this.InitializeServices();
        this.SetQueriesVisibility();
        this.LoadNonFilteredQueries();
        this.BuildFilters();
        this.InitializeFilters();
    }

    private filterName_RecordsType: string = "RecordsType";
    private filterName_CreatedByType: string = "CreatedByType";
    private filterName_Owner: string = "Owner";
    private filterName_BusinessUnit: string = "BusinessUnit";
    private filterControlNameSpace: string = "Logitude.CRM.Views.CRMPages.OpportunitiesPageControl";
    private filterName_SalesFunnel: string = "SalesFunnel";
    private filterName_TopOpportunities: string = "TopOpportunities";

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
    public FunneFilterList: CodeNameClass[] = [];
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
    public TopOpportunitiesFilterList: CodeNameClass[] = [];
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
    onUserQueriesBackComplete(event) {
        this.LoadAllScreenData();
    }
    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }

    LoadAllScreenData() {
        this.LoadFilteredQueries();
        this.LoadNonFilteredQueries();
    }
    LoadFilteredQueries() {
        this.LoadFunnelData();
        this.LoadTopOpportunities();
        this.LoadQueriesCounts();
    }
    LoadNonFilteredQueries() {
        this.ReloadUsersQuery();
        //this.LoadQueriesCounts();
        this.LoadRecentOpportunities();
    }

   //Screen Visibilities
    public NoRecentVisibility: boolean = false;
    public NoTopVisibility: boolean = false;

    // Queries Features
    public OpenQueriesVisibility: boolean = false;
    public MyOpenQueryVisibility: boolean = false;
    public AllOpenQueryVisibility: boolean = false;
    public StageOpenQueryVisibility: boolean = false;
    public ClosedQueriesVisibility: boolean = false;
    public MyClosedQueryVisibility: boolean = false;
    public AllClosedQueryVisibility: boolean = false;
    public OtherQueriesVisibility: boolean = false;
    public AllQueryVisibility: boolean = false;
    public CancelledQueryVisibility: boolean = false;
    public MyViewsQueryVisibility: boolean = false;   
    SetQueriesVisibility() {
        if (FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.MyOpenOpportunities")) {
            this.OpenQueriesVisibility = true;
        }

        else if (FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.AllOpenOpportunities")) {
            this.OpenQueriesVisibility = true;
        }

        else if (FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.OpenByStage")) {
            this.OpenQueriesVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.MyOpenOpportunities")) {
            this.MyOpenQueryVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.AllOpenOpportunities")) {
            this.AllOpenQueryVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.OpenByStage")) {
            this.StageOpenQueryVisibility = true;
        }


        if (FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.MyClosedOpportunities")) {
            this.ClosedQueriesVisibility = true;
        }

        else if (FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.AllClosedOpportunities")) {
            this.ClosedQueriesVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.MyClosedOpportunities")) {
            this.MyClosedQueryVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.AllClosedOpportunities")) {
            this.AllClosedQueryVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.AllOpportunities")) {
            this.OtherQueriesVisibility = true;
        }

        else if (FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.CancelledOpportunities")) {
            this.OtherQueriesVisibility = true;
        }


        if (FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.AllOpportunities")) {
            this.AllQueryVisibility = true;
        }


        if (FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.CancelledOpportunities")) {
            this.CancelledQueryVisibility = true;
        }


        if (FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES")) {
            this.MyViewsQueryVisibility = true;
        }



    }

    //Load Data Counts
    public AllOpenCount: number;
    public MyOpenCount: number;
    public OpenByStageCount: number;
    private LoadQueriesCounts() {
        this.myDomainService.GetOpportunitiesSummary(this.OwnerId, this.BusinessUnitId, this.RecordsTypeFilterCode).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                var myData: CRMSummary = myResponse.Result;
                if (myData != null) {
                    this.MyOpenCount = myData.MyOpenDataCount;
                    this.AllOpenCount = myData.AllOpenDataCount;
                    this.OpenByStageCount = myData.OpenByStageCount;
                }
            }
        });
    }

    // Load Recent Data
    public RecentOpportunitiesCount: number = 0;
    public RecentOpportuntiesList: OpportunityList[] = [];
    private LoadRecentOpportunities() {
        this.myDomainService.GetRecentOpportunities(null, null).subscribe(myResult => {
            if (myResult == null) {
                this.RecentOpportuntiesList = [];
                this.RecentOpportunitiesCount = 0;
                this.NoRecentVisibility = true;
            }

            else {
                this.RecentOpportuntiesList = myResult;
                this.RecentOpportunitiesCount = this.RecentOpportuntiesList.length;
                if (this.RecentOpportunitiesCount == 0)
                    this.NoRecentVisibility = true;
                else
                    this.NoRecentVisibility = false;
            }

        });
    }

    // Load Top Data
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

        if (!AppTool.IsNullOrEmpty(this.BusinessUnitId)) {
            myBusinessUnitId = this.BusinessUnitId;

            if (myBusinessUnitId == "all" || myBusinessUnitId == "null") {
                myBusinessUnitId = null;
            }
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

        if (this.TopOpportunitiesList == null) {
            this.TopOpportunitiesList = new Array<TopOpportunityItem>();
        }

        else {
            this.TopOpportunitiesList = [];
        }

        var myService: OpportunityListService = new OpportunityListService();
        myService.getByFilters(filters).subscribe(myResult => {
            if (myResult != null) {

                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {

                    var list: OpportunityList[] = myResponse.Result;

                    this.FillTopOpportunitiesList(list);
                }
            }
        });
    }
    private FillTopOpportunitiesList(myList: OpportunityList[]) {
        this.TopOpportunitiesList = [];
        switch (this.SelectedTopOpportunitiesFilter.Code) {


            case "STG":
                {
                    var tempList: OpportunityList[] = myList.sort((a, b) => { return b.RatingIndexOrder - a.RatingIndexOrder });
                    tempList.sort((a, b) => { return b.StageProbability - a.StageProbability }).forEach((item) => {
                        var itemViewModel: TopOpportunityItem = new TopOpportunityItem(item, this.SelectedTopOpportunitiesFilter);
                        this.TopOpportunitiesList.push(itemViewModel);
                    });

                    break;
                }
            case "SHI":
                {
                    myList.sort((a, b) => { return b.NumberOfShipments - a.NumberOfShipments }).forEach((item) => {
                        var itemViewModel: TopOpportunityItem = new TopOpportunityItem(item, this.SelectedTopOpportunitiesFilter);
                        this.TopOpportunitiesList.push(itemViewModel);
                    });
                    break;
                }
            case "RAT":
                {
                    myList.sort((a, b) => { return (a.StageName === b.StageName) ? 0 : (a.StageName < b.StageName) ? -1 : 1 });
                    myList.sort((a, b) => { return b.RatingIndexOrder - a.RatingIndexOrder }).forEach((item) => {
                        var itemViewModel: TopOpportunityItem = new TopOpportunityItem(item, this.SelectedTopOpportunitiesFilter);
                        this.TopOpportunitiesList.push(itemViewModel);
                    });
                    //this.TopOpportunitiesList.reverse();
                    break;
                }
            case "EST":
                {
                    myList.sort((a, b) => { return b.EstimatedClosingDate!=null?(b.EstimatedClosingDate.valueOf() - a.EstimatedClosingDate.valueOf()):-1 }).forEach((item) => {
                        var itemViewModel: TopOpportunityItem = new TopOpportunityItem(item, this.SelectedTopOpportunitiesFilter);
                        this.TopOpportunitiesList.push(itemViewModel);
                    });

                    break;
                }
            case "DUE":
                {
                    myList.sort((a, b) => { return b.StageDueDate!=null?(b.StageDueDate.valueOf() - a.StageDueDate.valueOf()) :-1}).forEach((item) => {
                        var itemViewModel: TopOpportunityItem = new TopOpportunityItem(item, this.SelectedTopOpportunitiesFilter);
                        this.TopOpportunitiesList.push(itemViewModel);
                    });

                    break;
                }


        }
                        this.NoTopVisibility = this.TopOpportunitiesList.length == 0 ? true : false;

        //this.TopOpportunitiesList.reverse();

    }

    // Load Funnel Data
    public FunnelData: any;
    public FunnelDataFilterd = [];
    LoadFunnelData() {
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
        ResetItem();

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
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        }
    }

    EditOpportunity(entity: any) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Opportunity', BackButtonLabel: "Opportunity" });
                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    this.LoadAllScreenData();
                    //this.isWindowOpened = false;
                });
            });
    }    
    RunOpportunityWizard() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "New Opportunity";
        logWindow.Show('./CRMModules/CRMOpportunity/Components/NewEntity/NewOpportunityComponent');
        
        logWindow.WindowClosed.subscribe(s => {            
            if (s) {
                this.LoadAllScreenData();
            }
        });
    }
    ViewOpportunityQuery(code: string) {
        if (code != null) {
            var objectTableName = "Opportunity";
            var queryCode = null;
            var displayTitle = "";
            var backButtonTitle = "CRM";

            var filterAgrs: ApiQueryFilters = new ApiQueryFilters();

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
                        queryCode = "My Open Opportunities";
                        break;
                    }

                case "Open:All":
                    {
                        queryCode = "All Open Opportunities";
                        break;
                    }

                case "Closed:My":
                    {
                        queryCode = "My Closed Opportunities";
                        break;
                    }

                case "Closed:All":
                    {
                        queryCode = "All Closed Opportunities";
                        break;
                    }

                case "OpenByStage":
                    {
                        queryCode = "Open By Stage";
                        break;
                    }

                case "All":
                    {
                        queryCode = "All Opportunities";
                        break;
                    }

                case "Cancelled":
                    {
                        queryCode = "Cancelled Opportunities";
                        break;
                    }
                default: { break; }

            }
        
            displayTitle = queryCode;

            var listArgs = new ListComponentArgs();
            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = displayTitle;
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
}
export class TopOpportunityItem {
    public entityList: OpportunityList;
    private filter: CodeNameClass;
    constructor(entityList: OpportunityList, filter: CodeNameClass) {
        this.entityList = entityList;
        this.filter = filter;
        this.ComputeFilterValue();
    }

    get CustomerName() { return this.entityList.CustomerName; }
    get Id() { return this.entityList.Id; }
    get LastActivityTypeName() { return this.entityList.LastCompletedActivityTypeName; }
    get StageName() { return this.entityList.StageName; }
    get LastActivityDate() { return this.entityList.LastCompletedActivityDate; }
    get Topic() { return this.entityList.Subject; }
    get StageAge() { return this.entityList.LastStageDate; }
    get CreateDate() { return this.entityList.CreateDate; }
    get RatingCode() { return this.entityList.RatingCode; }
    get RatingName() { return this.entityList.RatingName; }
    get EstimatedClosingDate() { return this.entityList.EstimatedClosingDate; }
    get NumberOfShipments() { return this.entityList.NumberOfShipments; }
    get BudgetAmount() { return this.entityList.ValueField; }
    get OwnerName() { return this.entityList.OwnerName; }


   
    public get StageAgeVisibility() {
        var result = false;
        if (this.StageAge != null) {
            result = true
        }
        return result;

    }

    public FilterValue: string;
    private ComputeFilterValue() {
        var myResult: string = null;

        switch (this.filter.Code) {
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

        return myResult;
    }
}
