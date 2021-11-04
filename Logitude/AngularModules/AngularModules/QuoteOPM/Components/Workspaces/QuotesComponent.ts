import {Component, Output, EventEmitter} from '@angular/core';
import {QuoteDomainService, CRMSummary} from '../../Services/QuoteDomainService';
import {QuoteOPList} from '../../EntityLists/QuoteOPList';
import {QuoteOPListService} from '../../Services/StandardLists/QuoteOPListService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {CodeNameClass} from '../../../Infrastructure/DataContracts/CodeNameClass';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LastFilterClass} from '../../../Infrastructure/Utilities/LastFilterClass';
import {DateTimeToDatePipe} from '../../../Controls/Pipes/DateTimeToDatePipe';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {BusinessUnitListService} from '../../../Common/Services/StandardLists/BusinessUnitListService';
import {BusinessUnitList} from '../../../Common/EntityLists/BusinessUnitList';
import {UserListService} from '../../../Common/Services/StandardLists/UserListService';
import {UserList} from '../../../Common/EntityLists/UserList';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {NewQuoteComponentArgs} from '../../Args';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ListComponentArgs} from '../../../Infrastructure/Args';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';


declare var makeChart, FunnelClick, ResetItemFunnel;

@Component({
    
    templateUrl: './QuotesComponent.html',
})

export class QuotesComponent extends BaseComponent {
    public DataContext = this;
    public SalesFunnelId: string = "SalesFunnelId_";
    public IsResourcesReady: boolean = false;
    @Output() ReloadUserQueries = new EventEmitter();
    public QuickSearchItems: QuoteOPList[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService) {
        super();
        this.SalesFunnelId = "SalesFunnel_" + this.CurrentSession.GetNewId("SalesFunnel");

        //this._entityResourceService.getEntityResourceByTableName("Quote", 0).subscribe((response: any) => {
            this._entityResourceService.getEntityResourceByTableName("QuoteOP", 0).subscribe((response: any) => {
                this.IsResourcesReady = true;
                this.InitializeServices();
                this.LoadNonFilteredQueries();
                this.BuildTopQuotesFilters();
                this.InitializeFilters();
            });
        //});
    }

    private filterName_RecordsType: string = "RecordsType";
    private filterName_CreatedByType: string = "CreatedByType";
    private filterName_Owner: string = "Owner";
    private filterName_BusinessUnit: string = "BusinessUnit";
    private filterControlNameSpace: string = "Simplog.QuoteLib.Views.QuotesMainMenu.QuotesMainControl";
    private filterName_TopQuotes: string = "TopQuotes";

    private myUserListService: UserListService;
    private myBusinessUnitListService: BusinessUnitListService;
    private myDomainService: QuoteDomainService;
    private InitializeServices() {
        this.myDomainService = new QuoteDomainService();
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

    //Top Quotes Filter
    public TopQuotesFilterList: CodeNameClass[] = [];
    private BuildTopQuotesFilters() {
        this.TopQuotesFilterList = [];
        this.TopQuotesFilterList.push(new CodeNameClass("EX", "Expiration Date"));
        this.TopQuotesFilterList.push(new CodeNameClass("SD", "Stage Due Date"));
        this.TopQuotesFilterList.push(new CodeNameClass("RT", "Rating"));
        this.TopQuotesFilterList.push(new CodeNameClass("ST", "Stage"));

        var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_TopQuotes);
        if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "EX";
        }

        this.selectedTopQuotesItem = this.TopQuotesFilterList.filter(d => d.Code == defaultFilterCode)[0];
    }

    private selectedTopQuotesItem: CodeNameClass;
    get SelectedTopQuotesItem() { return this.selectedTopQuotesItem; }
    set SelectedTopQuotesItem(value: CodeNameClass) {
        if (this.selectedTopQuotesItem != value) {
            this.selectedTopQuotesItem = value;

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_TopQuotes, (value == null ? null : value.Code));
            this.LoadTopQuotes();
        }
    }

    //Direction and TransportMode filters
    private currentDirectionId: string = "";
    private currentTransportModeId: string = "";

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
        this.LoadQueriesCounts();
        this.LoadTopQuotes();
        this.LoadFunnelData();

    }
    LoadNonFilteredQueries() {
        this.LoadRecentQuotes();
        this.ReloadUsersQuery();
    }

    //Load Data Counts
    public Quotes_Created: number;
    public Quotes_Draft: number;
    public Quotes_Expired: number;
    public Quotes_Accepted: number;
    public Quotes_AcceptedNOShip: number;
    public Quotes_Cancelled: number;
    public Quotes_Sent: number;
    public Quotes_AllFollowups: number;
    public Quotes_MyFollowups: number;
    public Quotes_All: number;
    public Quotes_My: number;
    private LoadQueriesCounts() {
        this.myDomainService.GetQuotesCounts(this.OwnerId, this.BusinessUnitId, this.currentDirectionId, this.currentTransportModeId, this.RecordsTypeFilterCode).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myData: CRMSummary = myResponse.Result;
                    if (myData != null) {
                        this.Quotes_Created = myData.Quotes_Created;
                        this.Quotes_Draft = myData.Quotes_Draft;
                        this.Quotes_Expired = myData.Quotes_Expired;
                        this.Quotes_Accepted = myData.Quotes_Accepted;
                        this.Quotes_AcceptedNOShip = myData.Quotes_AcceptedNOShip;
                        this.Quotes_Cancelled = myData.Quotes_Cancelled;
                        this.Quotes_Sent = myData.Quotes_Sent;
                        this.Quotes_AllFollowups = myData.Quotes_AllFollowups;
                        this.Quotes_MyFollowups = myData.Quotes_MyFollowups;
                        this.Quotes_All = myData.Quotes_All;
                        this.Quotes_My = myData.Quotes_My;
                    }
                }
            }
        });
    }

    // Load Recent Data
    public RecentQuotesList: QuoteOPList[] = [];
    public IsNoDataVisible_RecentQuotes: boolean = false;
    private LoadRecentQuotes() {
        this.RecentQuotesList = [];
        this.IsNoDataVisible_RecentQuotes = false;

        this.myDomainService.GetRecentQuotes("all", "all").subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.RecentQuotesList = myResponse.Result;

                if (this.RecentQuotesList.length == 0) {
                    this.IsNoDataVisible_RecentQuotes = true;
                }
            }
        });
    }

    // Load Top Data
    public TopQuotesList: TopQuoteItem[] = [];
    public IsNoDataVisible_TopQuotes: boolean = false;
    private LoadTopQuotes() {
        this.TopQuotesList = [];
        this.IsNoDataVisible_TopQuotes = false;
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 10;
        filters.SortDirection = "Descending";

        switch (this.SelectedTopQuotesItem.Code) {
            case "EX":
                {
                    filters.SortBy = "ExpirationDate";
                    break;
                }

            case "SD":
                {
                    filters.SortBy = "StageDueDate";
                    break;
                }

            case "RT":
                {
                    filters.SortBy = "RatingIndexOrder";
                    break;
                }

            case "ST":
                {
                    filters.SortBy = "StageMaxDays";
                    break;
                }
        }

        var myOwnerId: string = null;
        var myBusinessUnitId: string = null;
        var myDirectionId: string = null;
        var myTransportModeId: string = null;

        if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
            myOwnerId = this.OwnerId;

            if (myOwnerId == "all" || myOwnerId == "null") {
                myOwnerId = null;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.BusinessUnitId)) {
            myBusinessUnitId = this.BusinessUnitId;

            if (myBusinessUnitId == "all" || myBusinessUnitId == "null") {
                myBusinessUnitId = null;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.currentDirectionId)) {
            myDirectionId = this.currentDirectionId;
        }

        if (!AppTool.IsNullOrEmpty(this.currentTransportModeId)) {
            myTransportModeId = this.currentTransportModeId;
        }

        filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
        filters.addAdditionalFilter("DirectionId", myDirectionId, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("TransportModeId", myTransportModeId, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("TopQuotes", true, null, null, "Equals", true, false, false, "Boolean");

        if (this.RecordsTypeFilterCode == "C") {
            filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
        }

        else {
            filters.addAdditionalFilter("SalesmanUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
            filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
        }

        var myService: QuoteOPListService = new QuoteOPListService();
        myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: QuoteOPList[] = myResponse.Result;
                this.FillTopQuotesList(list);
            }
        });
    }
    private FillTopQuotesList(myList: QuoteOPList[]) {
        switch (this.SelectedTopQuotesItem.Code) {
            case "EX":
                {
                    myList.sort((a, b) => { return a.ExpirationDate.valueOf() - b.ExpirationDate.valueOf() }).forEach((item) => {
                        var itemViewModel: TopQuoteItem = new TopQuoteItem(item, this.SelectedTopQuotesItem);
                        this.TopQuotesList.push(itemViewModel);
                    });
                    
                    break;
                }

            case "SD":
                {
                    myList.sort((a, b) => { return a.StageDueDate.valueOf() - b.StageDueDate.valueOf() }).forEach((item) => {
                        var itemViewModel: TopQuoteItem = new TopQuoteItem(item, this.SelectedTopQuotesItem);
                        this.TopQuotesList.push(itemViewModel);
                    });
                    
                    break;
                }

            case "RT":
                {
                    myList.sort((a, b) => { return b.RatingIndexOrder - a.RatingIndexOrder }).forEach((item) => {
                        var itemViewModel: TopQuoteItem = new TopQuoteItem(item, this.SelectedTopQuotesItem);
                        this.TopQuotesList.push(itemViewModel);
                    });
                    
                    break;
                }

            case "ST":
                {
                    myList.sort((a, b) => { return a.StageMaxDays - b.StageMaxDays }).forEach((item) => {
                        var itemViewModel: TopQuoteItem = new TopQuoteItem(item, this.SelectedTopQuotesItem);
                        this.TopQuotesList.push(itemViewModel);
                    });
                    
                    break;
                }
        }

        if (this.TopQuotesList.length == 0) {
            this.IsNoDataVisible_TopQuotes = true;
        }
    }

    // Load Funnel Data
    public FunnelData: any;
    public FunnelDataFilterd = [];
    LoadFunnelData() {
        this.myDomainService.GetStageFunnelData(this.OwnerId, this.BusinessUnitId, this.RecordsTypeFilterCode).subscribe((myResult: any) => {
                this.FunnelData = myResult;
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
            var els = document.getElementsByTagName('a');
            els[1].remove();
        }

        catch (e) { }
    }    
    FunnelClick() {
        var item=FunnelClick();
        ResetItemFunnel();

        if (item != null) {

            var objectTableName = "QuoteOP";
            var queryCode = "Open Quotes";
            var displayTitle = this.FunnelData[item.index].LabelProperty +" Quotes";
            var backButtonTitle = TextCodeTranslator.Translate("General.MH.Quotes");

            var filterAgrs: ApiQueryFilters = new ApiQueryFilters();
            var listArgs = new ListComponentArgs();
            var myOwnerId=null;
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
            filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", true, false, false, "Boolean");

            if (this.RecordsTypeFilterCode == "C") {
                filterAgrs.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", true, false, false, "string");
            }

            else {
                filterAgrs.addAdditionalFilter("SalesmanUserId", myOwnerId, null, null, "Equals", true, false, false, "string");
                filterAgrs.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", true, false, false, "string");
            }

            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe((response:any) => {
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

    EditQuote(entity: any) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'QuoteOP', BackButtonLabel: TextCodeTranslator.Translate("General.MH.QuoteOPs") });
                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    this.LoadAllScreenData();
                    //this.isWindowOpened = false;
                });
            });
    }    
    RunQuoteWizard(oldWizard: boolean = false) {
        var args = new NewQuoteComponentArgs();
        var logWindow = new LogitudeWindow();
        //logWindow.RTL = false;
        logWindow.Width = oldWizard ? 1200 : 975;
        logWindow.Height = 800;
        logWindow.WindowArgs = args;
        logWindow.Title = TextCodeTranslator.Translate("Quote.S.NewQuote.CreateNewQuote");
        oldWizard ? logWindow.Show('./QuoteOPM/Components/NewEntity/NewQuoteComponentOld') : logWindow.Show('./QuoteOPM/Components/NewEntity/NewQuoteComponent');

        logWindow.WindowClosed.subscribe(s => {            
            if (s) {
                this.LoadAllScreenData();
            }
        });
    }
    ViewQuoteQuery(code: string) {
        if (code != null) {
            var objectTableName = "QuoteOP";
            var queryCode = null;
            var displayTitle = "";
            var backButtonTitle = TextCodeTranslator.Translate("General.MH.Quotes");
            var MethodName: string = null;

            var filterAgrs: ApiQueryFilters = new ApiQueryFilters();
            filterAgrs.SortBy = "OpenDate";
            filterAgrs.SortDirection = "Descending";

            if (this.RecordsTypeFilterCode == "C") {
                if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
                    filterAgrs.addAdditionalFilter("CreatedByUserId", this.OwnerId, null, null, "Equals", false, true, false, "string");
                }
            }

            else {
                if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
                    filterAgrs.addAdditionalFilter("SalesmanUserId", this.OwnerId, null, null, "Equals", false, true, false, "string");
                }

                if (!AppTool.IsNullOrEmpty(this.BusinessUnitId)) {
                    filterAgrs.addAdditionalFilter("BusinessUnitId", this.BusinessUnitId, null, null, "Equals", false, true, false, "string");
                }
            }

            switch (code) {
                case "CRT":
                    {
                        queryCode = "Created Quotes";
                        break;
                    }

                case "DRF":
                    {
                        queryCode = "Draft Quotes";
                        break;
                    }

                case "SNT":
                    {
                        queryCode = "Sent Quotes";
                        break;
                    }

                case "ACW":
                    {
                        queryCode = "Accepted Without Shipments";
                        break;
                    }

                case "ACP":
                    {
                        queryCode = "Accepted Quotes";
                        break;
                    }

                case "CNC":
                    {
                        queryCode = "Cancelled Quotes";
                        break;
                    }

                case "MY":
                    {
                        queryCode = "My Quotes";
                        break;
                    }

                case "EXP":
                    {
                        queryCode = "Expired Quotes";
                        break;
                    }

                case "AllF":
                    {
                        queryCode = "All Follow Ups";
                        MethodName = "QuoteFollowUp";
                        break;
                    }

                case "MYF":
                    {
                        queryCode = "My Follow Ups";
                        MethodName = "QuoteFollowUp";
                        break;
                    }

                case "ALL":
                    {
                        queryCode = "All Quotes";
                        break;
                    }

                default: { break; }
            }

            var listArgs = new ListComponentArgs();
            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.BackButtonTitle = backButtonTitle;
            listArgs.MethodName = MethodName;
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                    this.CurrentSession.AddMenuReference(cmpRef);
                });;
        }
    }

    OnImageError(item: any, field: string) {
        if (item && field) {
            item[field] = "--";
        }
    }
}
export class TopQuoteItem {
    public entityList: QuoteOPList;
    private filter: CodeNameClass;    
    constructor(entityList: QuoteOPList, filter: CodeNameClass) {
        this.entityList = entityList;
        this.filter = filter;
        this.ComputeFilterValue();
    }

    get CustomerName() { return this.entityList.CustomerName; }
    get Salesman() { return this.entityList.Salesman; }
    get QuoteNumber() { return this.entityList.QuoteNumber; }
    get StageName() { return this.entityList.StageName; }
    get DirectionId() { return this.entityList.DirectionId; }
    get DirectionName() { return this.entityList.DirectionName; }
    get TransportModeId() { return this.entityList.TransportModeId; }
    get TransportModeName() { return this.entityList.TransportModeName; }
    get RatingCode() { return this.entityList.RatingCode; }
    get RatingName() { return this.entityList.RatingName; }
    get LastStageDate() { return this.entityList.LastStageDate; }

    public FilterValue: string;
    private ComputeFilterValue() {
        var myResult: string = null;

        switch (this.filter.Code) {
            case "EX":
                {
                    myResult = DateTimeToDatePipe.Pipe(this.entityList.ExpirationDate);
                    break;
                }

            case "SD":
                {
                    myResult = DateTimeToDatePipe.Pipe(this.entityList.StageDueDate);
                    break;
                }

            case "RT":
                {
                    myResult = this.entityList.RatingName;
                    break;
                }

            case "ST":
                {
                    myResult = this.entityList.StageName;
                    break;
                }
        }

        this.FilterValue = myResult;
    }
}
