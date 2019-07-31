import {Component, ViewChildren, QueryList,ViewEncapsulation} from '@angular/core';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DashboardWorkspaceComponent} from '../DashboardWorkspaceComponent';
import {CodeNameClass} from '../../../../Infrastructure/DataContracts/CodeNameClass';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {BusinessUnitListService} from '../../../../Common/Services/StandardLists/BusinessUnitListService';
import {BusinessUnitList} from '../../../../Common/EntityLists/BusinessUnitList';
import {LastFilterClass} from '../../../../Infrastructure/Utilities/LastFilterClass';
import {AppTool, FormatTool, DateTool} from '../../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {UserList} from '../../../../Common/EntityLists/UserList';
import {UserListService} from '../../../../Common/Services/StandardLists/UserListService';
import {CRMUtilities} from '../../../CRMUtilities';
import {CRMDomainService} from '../../../Services/CRMDomainService';
import {ChartingDataClass} from '../../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';
import {FunctionsCRM} from '../../../../Infrastructure/DataContracts/Dashboard/FunctionsCRM';
import {List} from '../../../../Infrastructure/DataContracts/Dashboard/List';
import {ListComponentArgs} from '../../../../Infrastructure/Args';

declare var makeAmBarChart, BarClick, PieClick, makePieChart, ResetItem, ResetItemPie: any;

@Component({
    moduleId: module.id,
    templateUrl: './ByInProgressComponent.html',
    encapsulation: ViewEncapsulation.None,
})

export class ByInProgressComponent extends BaseComponent {

    private filterName_Owner: string = "Owner";
    private filterName_BusinessUnit: string = "BusinessUnit";
    private filterControlNameSpace: string = "Logitude.CRM.Views.CRMPages.DashboardTabsControls.ByInProgressControl";
    private filterName_CreateDate: string = "CreateDate";
    private crmDomainService: CRMDomainService;
    private myBusinessUnitListService: BusinessUnitListService;
    private myUserListService: UserListService;
    private CurrentOpportunityBySalesmanChart: any;
    private CurrentQuotesBySalesmanChart: any;
    public DataContext: ByInProgressComponent = this;
    private fieldCode: string = "P";
    private InitializeServices() {
        this.myUserListService = new UserListService();
        this.myBusinessUnitListService = new BusinessUnitListService();
        this.crmDomainService = new CRMDomainService();
    }
    public NewCustomersDashboardId: string;
    public NewOpportunitiesDashboardId: string;
    public NewQuotesDashboardId: string;
    public NewActivitiesDashboardId: string;
    public NewActivitiesTDId: string;
    public NewCustomerDashboardListExistance: boolean = false;
    public NewOpportunitiesDashboardIdExistance: boolean = false;
    public NewQuotesDashboardIdExistance: boolean = false;
    public NewActivitiesDashboardIdExistance: boolean = false;
    public NewOpportunityBySalesmanLegendId: string;
    public NewQuotesBySalesmanLegendId: string;
    RefreshButtonClicked() {
        this.LoadFilteredQueries();
    }

    private InitializeIds() {
        this.NewCustomersDashboardId = "NewCustomersDashboardId_" + this.CurrentSession.GetNewId("NewCustomerDashboard");
        this.NewOpportunitiesDashboardId = "NewOpportunitiesDashboardId_" + this.CurrentSession.GetNewId("NewOpportunitiesDashboard");
        this.NewQuotesDashboardId = "NewQuotesDashboardId_" + this.CurrentSession.GetNewId("NewQuotesDashboard");
        this.NewActivitiesDashboardId = "NewActivitiesDashboardId_" + this.CurrentSession.GetNewId("NewActivitiesDashboard");
        this.NewActivitiesTDId = "NewActivitiesTDId_" + this.CurrentSession.GetNewId("NewActivitiesTDId");
        this.NewOpportunityBySalesmanLegendId = "NewOpportunityBySalesmanLegendId__" + this.CurrentSession.GetNewId("NewOpportunityBySalesmanLegendId");
        this.NewQuotesBySalesmanLegendId = "NewQuotesBySalesmanLegendId__" + this.CurrentSession.GetNewId("NewQuotesBySalesmanLegendId");        
    }

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.InitializeIds();
        this.InitializeServices();
        this.BuildBusinessUnitFilter();
    }
    private Wizard: DashboardWorkspaceComponent;
    InitTab(wizard: DashboardWorkspaceComponent) {
        this.Wizard = wizard;
    }

    RefreshTab() {

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


    private selectedDateFilter: CodeNameClass;
    public get SelectedDateFilter() { return this.selectedDateFilter; }
    public set SelectedDateFilter(value: CodeNameClass) {

        if (this.selectedDateFilter != value) {
            this.selectedDateFilter = value;

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_CreateDate, (value == null ? null : value.Code));
            this.LoadFilteredQueries();
        }


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

    LoadFilteredQueries() {
            this.LoadQuotesData(0);
            this.LoadCustomersData(0);
            this.LoadActivitiesData(0);
            this.LoadOpportunitiesData(0);        
    }

    NewActivityClicking() {
        if (BarClick() != null) {
            this.OnNewActivityClick(BarClick());
            ResetItem();
        }
    }

    NewOpportunityClicking() {
        if (PieClick() != null) {
            this.OnNewOpportunityClick(PieClick());
        }
            ResetItemPie();        
    }


    NewQuotesClicking() {
        if (PieClick() != null) {
            this.OnNewQuoteClick(PieClick());
            ResetItemPie();
        }
    }

    NewCustomerClicking() {
        if (BarClick() != null) {
            this.OnNewCustomerClick(BarClick());
            ResetItem();
        }
    }


    OnNewOpportunityClick(e) {
        var item = this.NewOpportunityList[e.index];
        var myQueryCode: string = "All Opportunities";
        var myTableName: string = "Opportunity";
        var filterAgrs: ApiQueryFilters = new ApiQueryFilters();


        filterAgrs.addAdditionalFilter("OwnerId", item.OwnerId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("BusinessUnitId", item.BusinessUnitId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
        filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "boolean");


        var listArgs = new ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = "Opportunities";
        listArgs.BackButtonTitle = "CRM";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadFilteredQueries());
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                this.CurrentSession.AddMenuReference(cmpRef);
            });



    }

    OnNewQuoteClick(e) {

        var item = this.NewQuoteList[e.index];
        var myQueryCode: string = "All Quotes";
        var myTableName: string = "Quote";
        var filterAgrs: ApiQueryFilters = new ApiQueryFilters();



        filterAgrs.addAdditionalFilter("SalesmanUserId", item.OwnerId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "boolean");
        filterAgrs.addAdditionalFilter("BusinessUnitId", item.BusinessUnitId, null, null, "Equals", false, false, false, "String");

        var listArgs = new ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = "Quotes";
        listArgs.BackButtonTitle = "CRM";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadFilteredQueries());
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                this.CurrentSession.AddMenuReference(cmpRef);
            });

    }



    OnNewCustomerClick(e) {
        var flag = false;
        let item: any;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;
        var Key = e.target.columnIndex;
        var myQueryCode: string = "ShippersAndConsignees";
        var myTableName: string = "Customer";
        var filterAgrs: ApiQueryFilters = new ApiQueryFilters();       

        filterAgrs.addAdditionalFilter("SalesmanUserId", this.NewCustomersYAxisFitlerd[e.target.columnIndex].OwnerIds[item.index], null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("SalesmanBusinessUnitId", this.NewCustomersYAxisFitlerd[e.target.columnIndex].BusinessUnitId[item.index], null, null, "Equals", true, false, false, "String");
        filterAgrs.addAdditionalFilter("CustomerStatusCode", "POT", null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("CRMChartFilter", true, null, null, "Equals", true, false, false, "boolean");

        var listArgs = new ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = "Customers";
        listArgs.BackButtonTitle = "CRM";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadFilteredQueries());
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                this.CurrentSession.AddMenuReference(cmpRef);
            });



    }

    OnNewActivityClick(e) {
        var flag = false;
        let item: any;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;
        var Key = e.target.columnIndex;
        var myQueryCode: string = "All Activities";
        var myTableName: string = "Activity";
        var filterAgrs: ApiQueryFilters = new ApiQueryFilters();
        if (flag) {
            var typeName = "";
            var typeCode = "";
            switch (Key + "") {
                case "0": { typeName = "Tasks"; typeCode = "TS"; break; }
                case "1": { typeName = "Phone Calls"; typeCode = "CL"; break; }
                case "2": { typeName = "Appointment"; typeCode = "AP"; break; }
            }
        }

        var myCode = null;
        if (!AppTool.IsNullOrEmpty(this.NewActivitiesYAxisFitlerd[e.target.columnIndex].label))
            myCode = this.NewActivitiesYAxisFitlerd[e.target.columnIndex].label;

        filterAgrs.addAdditionalFilter("OwnerId", this.NewActivitiesYAxisFitlerd[e.target.columnIndex].OwnerIds[item.index], null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("IsOpen", true, null, null, "Equals", false, false, false, "boolean");
        filterAgrs.addAdditionalFilter("ActivityTypeCode", typeCode, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("BusinessUnitId", this.NewActivitiesYAxisFitlerd[e.target.columnIndex].BusinessUnitId[item.index], null, null, "Equals", false, false, false, "String");

        var listArgs = new ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = typeName;
        listArgs.BackButtonTitle = "CRM";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadFilteredQueries());
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                this.CurrentSession.AddMenuReference(cmpRef);
            });

    }


    LoadQuotesData(days: number) {
        this.crmDomainService.GetQuotesGroupBySalesman(days + "", this.OwnerId, this.BusinessUnitId, this.fieldCode, false).subscribe(result => {
            try {
                if (this.CurrentQuotesBySalesmanChart != null) {
                    this.CurrentQuotesBySalesmanChart.clear();
                    this.CurrentQuotesBySalesmanChart = null;
                }
            }
            catch (er) { }
            if (result.Result.length == 0) {
                this.NewQuotesDashboardIdExistance = false;
            }
            else {

                this.FillQuotesList(result.Result);
                this.NewQuotesDashboardIdExistance = true;

            }


        });
    }
    public NewQuoteList: Array<any> = [];

    FillQuotesList(List: Array<ChartingDataClass>) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.NewQuoteList = List;
        List.forEach(element => {
            fullData.push({ label: element.StringProperty, data: element.IntegerProperty })
            pieChartLabels.push(element.StringProperty);
            pieChartData.push(element.IntegerProperty);

        });
        var flagEmpty = true;
        pieChartData.forEach(p => {
            if (p != "0")
                flagEmpty = false;
        });
        if (!flagEmpty) {
            
            this.CurrentQuotesBySalesmanChart=makePieChart(this.NewQuotesDashboardId, fullData, false, true, this.NewQuotesBySalesmanLegendId);
        }

    }




    LoadCustomersData(days: number) {
        this.crmDomainService.GetCustomersGroupBySalesman(days, this.OwnerId, this.BusinessUnitId, this.fieldCode, true).subscribe(result => {
            if (result.Result.length == 0) {
                this.NewCustomerDashboardListExistance = false;
                try {
                    var elm = document.getElementById(this.NewCustomersDashboardId);
                }
                catch (er) { }
                elm.innerHTML = "";

            }
            else {
                this.NewCustomerDashboardListExistance = true;
                this.FillCustomerList(result.Result);
            }

        });
    }

    FillCustomerList(List: Array<ChartingDataClass>) {

        var index = 0;
        var NewCustomerXAxis = [];
        var NewCustomerYAxis = [];
        List.sort((a, b) => { return (a.DateTimeProperty === b.DateTimeProperty) ? 0 : (a.DateTimeProperty < b.DateTimeProperty) ? -1 : 1 });
        var StringArr: Array<string> = new Array<string>();
        var j = 0;
        List.forEach(element => {
            if (!StringArr.includes(element.StringProperty)) {
                StringArr.push(element.StringProperty);
                NewCustomerYAxis[j] = { data: [], label: null, BindingElement: [], OwnerIds: [], DateTime: [], BusinessUnitId: [] };
                NewCustomerYAxis[j].data = [];
                j++;
            }
        });

        var Graphs = [];
        var index = 0;
        List.forEach(element => {
            for (var i = 0; i < StringArr.length; i++) {
                if (element.StringProperty == StringArr[i]) {
                    if (NewCustomerYAxis[i].data.length == 0)
                        NewCustomerYAxis[i].data = new Array(1);

                    NewCustomerYAxis[i].data[0] = element.IntegerProperty;
                    NewCustomerYAxis[i].label = element.Code;
                    NewCustomerYAxis[i].BindingElement[0] = element.StringProperty;
                    NewCustomerYAxis[i].BusinessUnitId[0] = element.BusinessUnitId;
                    NewCustomerYAxis[i].DateTime[0] = element.DateTimeProperty;
                    NewCustomerYAxis[i].OwnerIds[0] = element.OwnerId;
                    if (!NewCustomerXAxis.includes(element.StringProperty) && element.StringProperty != null) {
                        if (NewCustomerXAxis[i] == null)
                            NewCustomerXAxis[i] = (element.StringProperty);

                    }
                }
            }
        });
        this.NewCustomersYAxisFitlerd = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        if (NewCustomerYAxis.length > 0)
            maximum = NewCustomerYAxis[0].data[0];
        if (maximum == null || maximum === undefined)
            maximum = 0;

        NewCustomerYAxis.forEach(element => {
            for (var i = 0; i < element.data.length; i++) {
                if (this.NewCustomersYAxisFitlerd[i] == null) {
                    this.NewCustomersYAxisFitlerd[i] = { data: [], label: null, BindingElement: [], OwnerIds: [], DateTime: [], BusinessUnitId: [] };
                }
                if (element.data[i] > maximum)
                    maximum = element.data[i];
                this.NewCustomersYAxisFitlerd[i].data.push(element.data[i]);
                this.NewCustomersYAxisFitlerd[i].BindingElement.push(element.BindingElement[i]);
                this.NewCustomersYAxisFitlerd[i].OwnerIds.push(element.OwnerIds[i]);
                this.NewCustomersYAxisFitlerd[i].BusinessUnitId.push(element.BusinessUnitId[i]);
                this.NewCustomersYAxisFitlerd[i].DateTime.push(element.DateTime[i]);
                this.NewCustomersYAxisFitlerd[i].label = element.label;

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
                        "fillColors": ["#DA7B38", "#ecbd9b"],
                        //  "fillColors": ["#ff0000", "#00ff00"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "showHandOnHover": true,

                        //   "plotAreaFillColors": ["#ff0000", "#f1783e", "#00ff00"],
                    };
                }
                objectArray[i] = (element.data[i]);

            }
            DataProvider[index] = { "category": NewCustomerXAxis[index], "col1": objectArray[0] };
            index++;
        });

        var InProgressBookingDashboardFilterd: Array<ChartingDataClass> = new Array<ChartingDataClass>();
        try {
            if (NewCustomerXAxis.length != 0) {

                maximum += 1;
                while (maximum % 5 != 0) {
                    maximum += 1;

                }

                makeAmBarChart(this.NewCustomersDashboardId, Graphs, DataProvider, maximum, null, null, 0);
            }

        }
        catch (e) {

        }



    }


    LoadActivitiesData(days: number) {
        this.crmDomainService.GetActivitiesGroupBySalesman(days + "", this.OwnerId, this.BusinessUnitId, this.fieldCode, false).subscribe(result => {
            if (result.Result.length == 0) {
                this.NewActivitiesDashboardIdExistance = false;
                try {
                    var elm = document.getElementById(this.NewActivitiesDashboardId);
                }
                catch (er) { }
                elm.innerHTML = "";

            }
            else {
                this.NewActivitiesDashboardIdExistance = true;
                this.FillActivitiesList(result.Result);
            }

        });
    }
    public NewActivitiesYAxisFitlerd: Array<any> = [];
    public NewCustomersYAxisFitlerd: Array<any> = [];
    FillActivitiesList(List: Array<ChartingDataClass>) {


        var index = 0;
        var NewCustomerXAxis = [];
        var NewCustomerYAxis = [];
        //   List.sort((a, b) => { return (a.DateTimeProperty === b.DateTimeProperty) ? 0 : (a.DateTimeProperty < b.DateTimeProperty) ? -1 : 1 });
        var StringArr: Array<string> = new Array<string>();
        var j = 0;
        List = List.filter(element => element.DataTypeCode == "TS" || element.DataTypeCode == "CL" || element.DataTypeCode == "AP");
        List.forEach(element => {
            if (!StringArr.includes(element.StringProperty)) {
                StringArr.push(element.StringProperty);
                NewCustomerYAxis[j] = { data: [], label: null, BindingElement: [], OwnerIds: [], DateTime: [], BusinessUnitId: [] };
                NewCustomerYAxis[j].data = [];
                j++;
            }
        });

        var Graphs = [];
        var index = 0;
        List.forEach(element => {
            for (var i = 0; i < StringArr.length; i++) {
                if (element.StringProperty == StringArr[i]) {
                    var k = 0;
                    if (element.DataTypeCode == "CL")
                        k = 1;
                    else if (element.DataTypeCode == "AP")
                        k = 2;
                    if (NewCustomerYAxis[i].data.length == 0)
                        NewCustomerYAxis[i].data = new Array(3);

                    NewCustomerYAxis[i].data[k] = element.IntegerProperty;
                    NewCustomerYAxis[i].label = element.Code;
                    NewCustomerYAxis[i].BindingElement[k] = element.DataTypeCode;
                    NewCustomerYAxis[i].BusinessUnitId[k] = element.BusinessUnitId;
                    NewCustomerYAxis[i].DateTime[k] = element.DateTimeProperty;
                    NewCustomerYAxis[i].OwnerIds[k] = element.OwnerId;
                    if (!NewCustomerXAxis.includes(element.StringProperty) && element.StringProperty != null) {
                        if (NewCustomerXAxis[i] == null)
                            NewCustomerXAxis[i] = (element.StringProperty);

                    }
                }
            }
        });

        var barChartColors: any[] = [
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
        this.NewActivitiesYAxisFitlerd = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        if (NewCustomerYAxis.length > 0)
            maximum = NewCustomerYAxis[0].data[0];
        if (maximum == null || maximum === undefined)
            maximum = 0;
        NewCustomerYAxis.forEach(element => {
            for (var i = 0; i < element.data.length; i++) {
                if (this.NewActivitiesYAxisFitlerd[i] == null) {
                    this.NewActivitiesYAxisFitlerd[i] = { data: [], label: null, BindingElement: [], OwnerIds: [], DateTime: [], BusinessUnitId: [] };
                }
                if (element.data[i] > maximum)
                    maximum = element.data[i];
                this.NewActivitiesYAxisFitlerd[i].data.push(element.data[i]);
                this.NewActivitiesYAxisFitlerd[i].BindingElement.push(element.BindingElement[i]);
                this.NewActivitiesYAxisFitlerd[i].OwnerIds.push(element.OwnerIds[i]);
                this.NewActivitiesYAxisFitlerd[i].BusinessUnitId.push(element.BusinessUnitId[i]);
                this.NewActivitiesYAxisFitlerd[i].DateTime.push(element.DateTime[i]);
                this.NewActivitiesYAxisFitlerd[i].label = element.label;
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
                        "fillColors": [barChartColors[i].backgroundColor1 + "", barChartColors[i].backgroundColor2 + ""],
                        //  "fillColors": ["#ff0000", "#00ff00"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "showHandOnHover": true,

                        //   "plotAreaFillColors": ["#ff0000", "#f1783e", "#00ff00"],
                    };
                }
                objectArray[i] = (element.data[i]);

            }
            DataProvider[index] = { "category": NewCustomerXAxis[index], "col1": objectArray[0], "col2": objectArray[1], "col3": objectArray[2] };
            index++;
        });

        var InProgressBookingDashboardFilterd: Array<ChartingDataClass> = new Array<ChartingDataClass>();
        try {
            if (NewCustomerXAxis.length != 0) {

                maximum += 1;
                while (maximum % 5 != 0) {
                    maximum += 1;

                }
                makeAmBarChart(this.NewActivitiesDashboardId, Graphs, DataProvider, maximum, null, null, 0);
            }

        }
        catch (e) {

        }



    }

    LoadOpportunitiesData(days: number) {
        this.crmDomainService.GetOpportunitiesGroupBySalesman(days + "", this.OwnerId, this.BusinessUnitId, this.fieldCode, false).subscribe(result => {
            try {
                if (this.CurrentOpportunityBySalesmanChart != null) {
                    this.CurrentOpportunityBySalesmanChart.clear();
                    this.CurrentOpportunityBySalesmanChart = null;
                }
            }
            catch (er) { }

            if (result.Result.length == 0) {

                this.NewOpportunitiesDashboardIdExistance = false;
            }
            else {

                this.FillOpportunitiesList(result.Result);
                this.NewOpportunitiesDashboardIdExistance = true;

            }
        });
    }

    public NewOpportunityList: Array<any> = [];
    FillOpportunitiesList(List: Array<ChartingDataClass>) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.NewOpportunityList = List;
        List.forEach(element => {
            fullData.push({ label: element.StringProperty, data: element.IntegerProperty })
            pieChartLabels.push(element.StringProperty);
            pieChartData.push(element.IntegerProperty);

        });

        var flagEmpty = true;
        pieChartData.forEach(p => {
            if (p != "0")
                flagEmpty = false;
        });
        if (!flagEmpty) {
           
            this.CurrentOpportunityBySalesmanChart=makePieChart(this.NewOpportunitiesDashboardId, fullData, false, true, this.NewOpportunityBySalesmanLegendId);
        }

    }



}
