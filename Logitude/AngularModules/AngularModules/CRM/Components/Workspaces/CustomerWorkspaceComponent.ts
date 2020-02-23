import {Component, Output, EventEmitter} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CodeNameClass} from '../../../Infrastructure/DataContracts/CodeNameClass';
import {LastFilterClass} from '../../../Infrastructure/Utilities/LastFilterClass';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool, DateFormats} from '../../../Infrastructure/Tools';
import {ListComponentArgs} from '../../../Infrastructure/Args';
import {PartnersDomainService, CRMSummary, CompareDataClass} from '../../../Common/Services/PartnersDomainService';
import {BusinessUnitListService} from '../../../Common/Services/StandardLists/BusinessUnitListService';
import {UserListService} from '../../../Common/Services/StandardLists/UserListService';
import {BusinessUnitList} from '../../../Common/EntityLists/BusinessUnitList';
import {UserList} from '../../../Common/EntityLists/UserList';
import {CustomerList} from '../../../Common/EntityLists/CustomerList';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';

@Component({
    moduleId: module.id,
    templateUrl: './CustomerWorkspaceComponent.html',
})

export class CustomerWorkspaceComponent extends BaseComponent {
    public SearchBoxWatermark: string = "Search...";
    public DataContext = this;
    public QuickSearchItemsCount: number = 0;
    public QuickSearchItems: CustomerList[] = [];
    @Output() ReloadUserQueries = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.SearchBoxWatermark = TextCodeTranslator.Translate("Card.F.SearchFields");
        this.InitializeServices();
        this.LoadNonFilteredQueries();
        this.BuildDecreasedShipmentsFilters();
        this.InitializeFilters();
    }

    private filterName_RecordsType: string = "RecordsType";
    private filterName_CreatedByType: string = "CreatedByType";
    private filterName_Owner: string = "Owner";
    private filterName_BusinessUnit: string = "BusinessUnit";
    private filterControlNameSpace: string = "Logitude.CRM.Views.CRMPages.CustomersPageControl";
    private filterName_ViewDecreasedDataType: string = "ViewDecreasedDataType";
    private filterName_ViewDecreasedTimeRange: string = "ViewDecreasedTimeRange";

    private myUserListService: UserListService;
    private myPartnersDomainService: PartnersDomainService;
    private myBusinessUnitListService: BusinessUnitListService;
    private InitializeServices() {
        this.myUserListService = new UserListService();
        this.myPartnersDomainService = new PartnersDomainService;
        this.myBusinessUnitListService = new BusinessUnitListService();
    }

    private searchText: string = null;
    get SearchText() { return this.searchText; }
    set SearchText(value: string) {
        if (this.searchText != value) {
            this.searchText = value;
        }
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

    // Decreased Shipment Filters
    public DecreasedShipmentsDataTypeList: CodeNameClass[] = [];
    public DecreasedShipmentsTimeRangeList: CodeNameClass[] = [];
    BuildDecreasedShipmentsFilters() {

        // DataType
        this.DecreasedShipmentsDataTypeList = [];
        this.DecreasedShipmentsDataTypeList.push(new CodeNameClass("S", "Shipments"));
        this.DecreasedShipmentsDataTypeList.push(new CodeNameClass("T", "TEU"));
        this.DecreasedShipmentsDataTypeList.push(new CodeNameClass("R", "Revenue"));
        this.DecreasedShipmentsDataTypeList.push(new CodeNameClass("C", "Chargeable Weight"));

        // TimeRange
        var last1MonthDateTime = new Date(new Date().setMonth(new Date().getMonth() - 1));
        var last2MonthDateTime = new Date(new Date().setMonth(new Date().getMonth() - 2));
        var date1Formats: DateFormats = DateTool.GetDateFormats(last1MonthDateTime);
        var date2Formats: DateFormats = DateTool.GetDateFormats(last2MonthDateTime);
        var last1MonthLabel: string = date1Formats.MonthName + " " + date1Formats.DateParts.Year;
        var last2MonthLabel: string = date2Formats.MonthName + " " + date2Formats.DateParts.Year;

        this.DecreasedShipmentsTimeRangeList = [];
        this.DecreasedShipmentsTimeRangeList.push(new CodeNameClass("LM", last1MonthLabel + " vs. " + last2MonthLabel));
        this.DecreasedShipmentsTimeRangeList.push(new CodeNameClass("AV.03", last1MonthLabel + " vs. Average of last 3 months"));
        this.DecreasedShipmentsTimeRangeList.push(new CodeNameClass("AV.12", last1MonthLabel + " vs. Average of last 12 months"));

        var defaultDateTypeFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_ViewDecreasedDataType);
        if (AppTool.IsNullOrEmpty(defaultDateTypeFilterCode)) {
            defaultDateTypeFilterCode = "S";
        }

        var defaultTimeRangeFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_ViewDecreasedTimeRange);
        if (AppTool.IsNullOrEmpty(defaultTimeRangeFilterCode)) {
            defaultTimeRangeFilterCode = "LM";
        }

        this.decreasedShipmentsDataTypeSelectedItem = this.DecreasedShipmentsDataTypeList.filter(d => d.Code == defaultDateTypeFilterCode)[0];
        this.decreasedShipmentsTimeRangeSelectedItem = this.DecreasedShipmentsTimeRangeList.filter(d => d.Code == defaultTimeRangeFilterCode)[0];
    }

    private decreasedShipmentsDataTypeSelectedItem: CodeNameClass;
    get DecreasedShipmentsDataTypeSelectedItem() { return this.decreasedShipmentsDataTypeSelectedItem; }
    set DecreasedShipmentsDataTypeSelectedItem(value: CodeNameClass) {
        if (this.decreasedShipmentsDataTypeSelectedItem != value) {
            this.decreasedShipmentsDataTypeSelectedItem = value;
           
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_ViewDecreasedDataType, (value == null ? null : value.Code));
            this.LoadDecreasedShipmentsData();
        }
    }

    private decreasedShipmentsTimeRangeSelectedItem: CodeNameClass;
    get DecreasedShipmentsTimeRangeSelectedItem() { return this.decreasedShipmentsTimeRangeSelectedItem; }
    set DecreasedShipmentsTimeRangeSelectedItem(value: CodeNameClass) {
        if (this.decreasedShipmentsTimeRangeSelectedItem != value) {
            this.decreasedShipmentsTimeRangeSelectedItem = value;

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_ViewDecreasedTimeRange, (value == null ? null : value.Code));
            this.LoadDecreasedShipmentsData();
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
        this.LoadQueriesCounts();
        this.LoadDecreasedShipmentsData();
    }
    LoadNonFilteredQueries() {
        this.ReloadUsersQuery();
        this.LoadRecentCustomers();
    }

    //Load Data Counts
    public Customers_My: number;
    public Customers_AccontManager: number;
    public Customers_Waiting: number;
    public Customers_Potential: number;
    public Customers_Active: number;
    public Customers_Inactive: number;
    private LoadQueriesCounts() {
        this.myPartnersDomainService.GetCustomersCounts(this.OwnerId, this.BusinessUnitId, this.RecordsTypeFilterCode).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                var myData: CRMSummary = myResponse.Result;
                if (myData != null) {
                    this.Customers_My = myData.MyOpenDataCount;
                    this.Customers_AccontManager = myData.MyOpenAsAccountManagerDataCount;
                    this.Customers_Waiting = myData.Customers_Waiting;
                    this.Customers_Potential = myData.Customers_Potential;
                    this.Customers_Active = myData.Customers_Active;
                    this.Customers_Inactive = myData.Customers_Inactive;
                }
            }
        });
    }

    // Load Recent Data
    public RecentCustomersCount: number = 0;
    public RecentCustomersList: RecentCustomerItem[];
    private LoadRecentCustomers() {

        this.myPartnersDomainService.GetRecentCustomers("all", "all").subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: CustomerList[] = myResponse.Result;

                this.RecentCustomersList = [];
                this.RecentCustomersCount = 0;

                list.forEach((item) => {
                    var itemViewModel: RecentCustomerItem = new RecentCustomerItem(item);
                    this.RecentCustomersList.push(itemViewModel);
                });

                this.RecentCustomersCount = this.RecentCustomersList.length;
            }
        });
    }

    // Decreased Shipments
    public DecreasedQuantityWidth: number = 60;
    public ShipmentsDataList: ShipmentDataItem[];
    private LoadDecreasedShipmentsData() {

        var dateTypeCode: string = "S";
        if (this.DecreasedShipmentsDataTypeSelectedItem != null) {
            dateTypeCode = this.DecreasedShipmentsDataTypeSelectedItem.Code;
        }

        var timeRange: string = "LM";
        if (this.DecreasedShipmentsTimeRangeSelectedItem != null) {
            timeRange = this.DecreasedShipmentsTimeRangeSelectedItem.Code;
        }

        var myStartDateTime = DateTool.GetDateByMonth(-1);

        this.myPartnersDomainService.GetCustomersDecreasedShipments(dateTypeCode, myStartDateTime, timeRange, this.OwnerId, this.BusinessUnitId, this.RecordsTypeFilterCode).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                var myData: CompareDataClass[] = myResponse.Result;
                this.BuildDecreasedShipmentsList(myData);
            }
        });
    }
    private BuildDecreasedShipmentsList(myList: CompareDataClass[]) {

        this.ShipmentsDataList = [];
        this.DecreasedQuantityWidth = 60;

        if (myList != null) {
            var data: ShipmentDataItem[] = [];

            myList.forEach((item) => {            
                data.push(new ShipmentDataItem(item, this.DecreasedShipmentsDataTypeSelectedItem.Code));
            });

            data.filter(d => d.IsDecreased).sort((a, b) => { return a.QuantityValue - b.QuantityValue }).forEach((item) => {
                if (this.ShipmentsDataList.length < 20) {
                    var width = AppTool.GetTextWidth(item.Quantity + "") + 10;
                    if (width > this.DecreasedQuantityWidth) {
                        this.DecreasedQuantityWidth = width;
                    }

                    this.ShipmentsDataList.push(item);
                }
            });
        }
    }

    NewCustomerClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "New Potential Customer";
        logWindow.Width = 990;
        logWindow.Height = 600;
        logWindow.Show("./CommonModules/CommonPartners/Components/NewEntity/NewPotentialCustomerComponent");

        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.LoadAllScreenData();
            }
        });
    }
    ViewCustomerQuery(code: string) {
        if (code != null) {
            var objectTableName = "Customer";
            var queryCode = null;
            var displayTitle = "";
            var backButtonTitle = "CRM";
            var isBusinessUnitFilterOn: boolean = true;

            switch (code) {
                case "My":
                    {
                        queryCode = "Customer.MyCustomers";
                        displayTitle = "My Customers (as Salesman)";
                        //isBusinessUnitFilterOn = false;
                        break;
                    }

                case "All":
                    {
                        queryCode = "Customers";
                        displayTitle = "Customers";
                        //isBusinessUnitFilterOn = false;
                        break;
                    }

                case "AccontManager":
                    {
                        queryCode = "Customer.Q.MyCustomersAccMngr";
                        displayTitle = "My Customers (as Account Manager)";
                        //isBusinessUnitFilterOn = false;
                        break;
                    }

                case "ReadyCustomers":
                    {
                        queryCode = "Customer.ReadyCustomers";
                        displayTitle = "Waiting for Activation";
                        break;
                    }

                case "PotentialCustomers":
                    {
                        queryCode = "Customer.PotentialCustomers";
                        displayTitle = "Potential Customers";
                        break;
                    }

                case "ActiveCustomers":
                    {
                        queryCode = "Customer.ActiveCustomers";
                        displayTitle = "Active Customers";
                        break;
                    }

                case "InactiveCustomers":
                    {
                        queryCode = "Customer.InactiveCustomers";
                        displayTitle = "Inactive Customers";
                        break;
                    }

                default: { break; }
            }

            var filterAgrs: ApiQueryFilters = new ApiQueryFilters();

            if (isBusinessUnitFilterOn) {
                //var myOwnerId = null;
                //var myBusinessUnitId = null;

                //if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
                //    myOwnerId = this.OwnerId;
                //}

                //if (!AppTool.IsNullOrEmpty(this.BusinessUnitId)) {
                //    myBusinessUnitId = this.BusinessUnitId;
                //}

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
                        filterAgrs.addAdditionalFilter("SalesmanBusinessUnitId", this.BusinessUnitId, null, null, "Equals", false, true, false, "string");
                    }
                }
            }

            //filterAgrs.Filter1Name = "SalesmanUserId";
            //filterAgrs.Filter1Value = myOwnerId;
            //filterAgrs.Filter1Operator = "Equals";

            //filterAgrs.Filter2Name = "SalesmanBusinessUnitId";
            //filterAgrs.Filter2Value = myBusinessUnitId;
            //filterAgrs.Filter2Operator = "Equals";

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
    EditCustomer(entityId: string) {                
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: 'Customer', BackButtonLabel: "Customers" });
                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    this.LoadAllScreenData();
                });
            });        
    }
    EditCustomerList(entity: CustomerList) {
        if (entity.IsBlockedQuickSearch) {
            var windowTitle = "View Customer";
            var windowArgs: any = {};
            windowArgs.CustomerList = entity;
            var logWindow = new LogitudeWindow();
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./CRMModules/CRMOthers/Components/BlockedCustomer/BlockedCustomerComponent');
        }

        else {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Customer', BackButtonLabel: "Customers" });
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                        this.LoadAllScreenData();
                    });
                });
        }
    }
}

export class RecentCustomerItem {
    public entityList: CustomerList;
    constructor(entityList: CustomerList) {
        this.entityList = entityList;

        this.ComputeRank();
    }

    get Id() { return this.entityList.Id; }
    get LastActivityTypeName() { return this.entityList.LastActivityTypeName; }
    get LastActivityDate() { return this.entityList.LastActivityDate; }
    get EnglishName() { return this.entityList.EnglishName; }
    get CustomerStatusCode() { return this.entityList.CustomerStatusCode; }
    get CustomerStatusName() { return this.entityList.CustomerStatusName; }
    get SalesmanUserEnglishName() { return this.entityList.SalesmanUserEnglishName; }
    get AccountManagerUserEnglishName() { return this.entityList.AccountManagerUserEnglishName; }

    public RankName: string;
    public RankSource1: string;
    public RankSource2: string;
    public RankSource3: string;
    private ComputeRank() {
        this.RankName = this.entityList.RankName;

        if (this.RankName != null) {
            switch (this.RankName.toLowerCase()) {
                case "silver": {
                    //this.RankCode = "1";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarGray.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                    break;
                }

                case "gold": {
                    //this.RankCode = "2";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarOrange.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                    break;
                }

                case "platinum": {
                    //this.RankCode = "3";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarOrange.png";
                    this.RankSource3 = "./Images/Icons/StarOrange.png";
                    break;
                }

                default: {
                    //this.RankCode = "0";
                    this.RankSource1 = "./Images/Icons/StarGray.png";
                    this.RankSource2 = "./Images/Icons/StarGray.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                }
            }
        }
    }
}
export class ShipmentDataItem {
    private entity: CompareDataClass;
    public filterCode: string;
    private CustomerId: string;
    public Id: string;
    constructor(entity: CompareDataClass, filterCode: string) {
        this.entity = entity;
        this.filterCode = filterCode;
        this.CustomerId = entity.Id;
        this.Id = entity.Id;

        this.ComputeRank();
        this.ComputeValues();
    }

    get CustomerName() { return this.entity.EntityName; }

    public Value1: number = 0;
    public Value2: number = 0;
    public IsDecreased: boolean = false;
    public Quantity: string = "";
    public QuantityValue: number = 0;
    public Percent: string = "";
    public PercentValue: number = 0;
    public PercentForeground: string = "#282E30";

    private ComputeValues() {
        var value1: number = 0;
        var value2: number = 0;
        var isDecreased: boolean = false;
        var quantity: string = "";
        var quantityValue: number = 0;
        var percent: string = "";
        var percentValue: number = 0;
        var percentForeground: string = "#282E30";

        switch (this.filterCode) {
            case "T": {
                value1 = AppTool.Round(this.entity.TEU_Old, 2);
                value2 = AppTool.Round(this.entity.TEU_New, 2);
                break;
            }

            case "R": {
                value1 = AppTool.Round(this.entity.Revenue_Old, 2);
                value2 = AppTool.Round(this.entity.Revenue_New, 2);
                break;
            }

            case "S": {
                value1 = this.entity.NumberOfShipments_Old;
                value2 = this.entity.NumberOfShipments_New;
                break;
            }

            case "C": {
                value1 = AppTool.Round(this.entity.ChargeableWeight_Old, 2);
                value2 = AppTool.Round(this.entity.ChargeableWeight_New, 2);
                break;
            }
        }

        quantityValue = value2 - value1;
        quantity = AppTool.Round(quantityValue, 2).toString();

        if (value1 != 0 || value2 != 0) {
            if (value1 == 0) {
                percentValue = 100;
                percent = "100 %";
            }

            else if (value2 == 0) {
                percentValue = 100;
                percent = "-100 %";
            }

            else {
                var def: number = value2 - value1;
                var rat: number = def / value1 * 100;

                if (rat != null) {
                    percentValue = rat;
                    percent = AppTool.Round(rat, 2) + " %";
                }
            }
        }

        var value = Math.abs(percentValue);

        if (value >= 80) {
            percentForeground = "#FF0000";
        }

        else if (value >= 40 && percentValue < 80) {
            percentForeground = "#FF5F0F";
        }

        else if (value < 40) {
            percentForeground = "#E6A000";
        }

        if (value2 < value1) {
            isDecreased = true;
        }

        this.Value1 = value1;
        this.Value2 = value2;
        this.IsDecreased = isDecreased;
        this.Quantity = quantity;
        this.QuantityValue = quantityValue;
        this.Percent = percent;
        this.PercentValue = percentValue;
        this.PercentForeground = percentForeground;
    }

    public RankName: string;
    public RankSource1: string;
    public RankSource2: string;
    public RankSource3: string;
    private ComputeRank() {
        this.RankName = this.entity.RankName;

        if (this.RankName != null) {
            switch (this.RankName.toLowerCase()) {
                case "silver": {
                    //this.RankCode = "1";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarGray.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                    break;
                }

                case "gold": {
                    //this.RankCode = "2";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarOrange.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                    break;
                }

                case "platinum": {
                    //this.RankCode = "3";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarOrange.png";
                    this.RankSource3 = "./Images/Icons/StarOrange.png";
                    break;
                }

                default: {
                    //this.RankCode = "0";
                    this.RankSource1 = "./Images/Icons/StarGray.png";
                    this.RankSource2 = "./Images/Icons/StarGray.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                }
            }
        }
    }
}
