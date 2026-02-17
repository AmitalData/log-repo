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
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';

declare var makeAmBarChart, BarClick, PieClick, makePieChart, ResetItemPie: any;

@Component({
    moduleId: module.id,
    templateUrl: './CompanyPerformanceComponent.html',
    encapsulation: ViewEncapsulation.None,
})

export class CompanyPerformanceComponent extends BaseComponent {

    private filterName_Owner: string = "Owner";
    private filterName_BusinessUnit: string = "BusinessUnit";
    private filterControlNameSpace: string = "Logitude.CRM.Views.CRMPages.DashboardTabsControls.CompanyPerformanceControl";
    private filterName_CloseDate: string = "CloseDate";
    private crmDomainService: CRMDomainService;
    private myBusinessUnitListService: BusinessUnitListService;
    private myUserListService: UserListService;
    private CurrentOpportunityByWonLostChart: any;
    private CurrentOpportunityByTypeChart: any;
    private CurrentOpportunityByLeadSourceChart: any;
    private CurrentActivityChart: any;
    private CurrentQuoteChart: any;
    private fieldCode: string = "S";
    private InitializeServices() {
        this.myUserListService = new UserListService();
        this.myBusinessUnitListService = new BusinessUnitListService();
        this.crmDomainService = new CRMDomainService();
    }

    public DateFilterList: Array<CodeNameClass> = [];
    public DataContext: CompanyPerformanceComponent = this;
    public OpportunitiesWonLostRatioId: string;
    public OpportunitiesbyLeadSourcetypeId: string;
    public OpportunitiesByTypeId: string;
    public CompletedActivitiesId: string;
    public AcceptedDeclinedQuotes: string;
    public ZoomedChartId: string;
    public legenddivId: string;
    public OpportunitiesWonLostRatioIdExistance: boolean = false;
    public OpportunitiesbyLeadSourcetypeIdExistance: boolean = false;
    public OpportunitiesByTypeIdExistance: boolean = false;
    public CompletedActivitiesIdExistance: boolean = false;
    public AcceptedDeclinedQuotesExistance: boolean = false;

    public OpportunityByWonLostLegendId: string;
    public OpportunityByTypeLegendId: string;
    public OpportunityByLeadSourceLegendId: string;
    public ActivityChartLegendId: string;
    public QuoteLegendId;

    private toDate: Date;
    public get ToDate() { return this.toDate; }
    public set ToDate(value: Date) {
        if (value != this.toDate) {
            this.toDate = value;
            this.SelectedDateFilter = this.DateFilterList[10];
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, "CompanyPerformanceToDate", (value == null ? null : ServiceHelper.GetDateString(value)));
        }
    }

    private fromDate: Date;
    public get FromDate() { return this.fromDate; }
    public set FromDate(value: Date) {
        if (value != this.fromDate) {
            this.fromDate = value;
            this.SelectedDateFilter = this.DateFilterList[10];
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, "CompanyPerformanceFromDate", (value == null ? null : ServiceHelper.GetDateString(value)));
        }
    }



    RefreshButtonClicked() {
        this.LoadFilteredQueries();
    }

    private BuildDateFilters() {
        this.DateFilterList = CRMUtilities.GetClosingDateFilterList();

        var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_CloseDate);
        if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "-30";
        }

        this.selectedDateFilter = this.DateFilterList.filter(d => d.Code == defaultFilterCode)[0];

        if (this.selectedDateFilter.Code == "-1_-1") {
            var ActiviytFromDate = LastFilterClass.GetFilterValue(this.filterControlNameSpace, "CompanyPerformanceFromDate");

            if (!AppTool.IsNullOrEmpty(ActiviytFromDate)) {
                var ActivityDate: Date = new Date();
                var ActivityFromDateString = ActiviytFromDate.split(':');
                ActivityDate.setFullYear(ActivityFromDateString[0], ActivityFromDateString[1] - 1, ActivityFromDateString[2]);
                this.fromDate = DateTool.GetDateParts(ActivityDate).DateObject;
            }

            var ActiviytToDate = LastFilterClass.GetFilterValue(this.filterControlNameSpace, "CompanyPerformanceToDate");
            if (!AppTool.IsNullOrEmpty(ActiviytToDate)) {
                var ActivityDate: Date = new Date();
                var ActivityToDateString = ActiviytToDate.split(':');
                ActivityDate.setFullYear(ActivityToDateString[0], ActivityToDateString[1] - 1, ActivityToDateString[2]);
                this.toDate = DateTool.GetDateParts(ActivityDate).DateObject;
            }


        }
    }
    private InitializeIds() {
        this.OpportunitiesWonLostRatioId = "OpportunitiesWonLostRatioId_" + this.CurrentSession.GetNewId("OpportunitiesWonLostRatioId");
        this.OpportunitiesbyLeadSourcetypeId = "OpportunitiesbyLeadSourcetypeId_" + this.CurrentSession.GetNewId("OpportunitiesbyLeadSourcetypeId");
        this.OpportunitiesByTypeId = "OpportunitiesByTypeId_" + this.CurrentSession.GetNewId("OpportunitiesByTypeId");
        this.CompletedActivitiesId = "CompletedActivitiesId_" + this.CurrentSession.GetNewId("CompletedActivitiesId");
        this.AcceptedDeclinedQuotes = "AcceptedDeclinedQuotes_" + this.CurrentSession.GetNewId("AcceptedDeclinedQuotes");
        this.ZoomedChartId = "ZoomedChartId_" + this.CurrentSession.GetNewId("ZoomedChartId");
        this.legenddivId = "legenddiv_" + this.CurrentSession.GetNewId("legenddiv");
        this.OpportunityByWonLostLegendId = "OpportunityByWonLostLegendId_" + this.CurrentSession.GetNewId("OpportunityByWonLostLegendId");
        this.OpportunityByTypeLegendId = "OpportunityByTypeLegendId_" + this.CurrentSession.GetNewId("OpportunityByTypeLegendId");
        this.OpportunityByLeadSourceLegendId = "OpportunityByLeadSourceLegendId_" + this.CurrentSession.GetNewId("OpportunityByLeadSourceLegendId");
        this.ActivityChartLegendId = "ActivityChartLegendId_" + this.CurrentSession.GetNewId("ActivityChartLegendId");
        this.QuoteLegendId = "QuoteLegendId_" + this.CurrentSession.GetNewId("QuoteLegendId");
    }

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.InitializeIds();
        this.InitializeServices();
        this.BuildBusinessUnitFilter();
        this.BuildDateFilters();
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
    public ZoomedChartVisibility: boolean = false;
    public ViewZoomed(chartCode) {
        switch (chartCode) {
            case "QT":
                {
                    this.ZoomedChartVisibility = true;
                    this.LoadZoomedQuotesData();
                    break;
                }

            case "AC":
                {
                    this.ZoomedChartVisibility = true;
                    this.LoadZoomedActivitiesData();
                    break;
                }

            case "OP":
                {
                    this.ZoomedChartVisibility = true;
                    this.LoadZoomedOpportunitiesData();
                    break;
                }
        }

    }

    LoadZoomedQuotesData() {
        if (this.SelectedDateFilter.Code == "-1_-1") {
            this.crmDomainService.GetQuotesGroupBySalesmanCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, this.fieldCode, true).subscribe(result => {
                this.FillZoomedQueries(result.Result, "Q");
            });
        }
        else {
            this.crmDomainService.GetQuotesGroupBySalesman(this.SelectedDateFilter.Code + "", this.OwnerId, this.BusinessUnitId, this.fieldCode, true).subscribe(result => {
                this.FillZoomedQueries(result.Result, "Q");
            });
        }
    }

    LoadZoomedOpportunitiesData() {
        if (this.SelectedDateFilter.Code == "-1_-1") {
            this.crmDomainService.GetOpportunitiesGroupBySalesmanCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, this.fieldCode, true).subscribe(result => {
                this.FillZoomedQueries(result.Result, "O");
            });
        }
        else {
            this.crmDomainService.GetOpportunitiesGroupBySalesman(this.SelectedDateFilter.Code + "", this.OwnerId, this.BusinessUnitId, this.fieldCode, true).subscribe(result => {
                this.FillZoomedQueries(result.Result, "O");
            });
        }
    }

    LoadZoomedActivitiesData() {
        if (this.SelectedDateFilter.Code == "-1_-1") {
            this.crmDomainService.GetActivitiesGroupBySalesmanCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, this.fieldCode, true).subscribe(result => {
                this.FillZoomedActivitiesQueries(result.Result);
            });
        }
        else {
            this.crmDomainService.GetActivitiesGroupBySalesman(this.SelectedDateFilter.Code + "", this.OwnerId, this.BusinessUnitId, this.fieldCode, true).subscribe(result => {
                this.FillZoomedActivitiesQueries(result.Result);
            });
        }
    }


    FillZoomedActivitiesQueries(List: Array<ChartingDataClass>) {


        var barChartData: any[] = [{ data: [], label: '' }, { data: [], label: '' }, { data: [], label: '' }];

        var i = 0;
        var index = 0;
        var Graphs = [];
        var DataProvider = [];
        var objectArray = [];
        barChartData[0].data = [];
        barChartData[1].data = []
        barChartData[2].data = []

        var barChartLabels = [];
        var max = 0;

        var flag: string = "";
        var Col1Title: string = "Task";
        var Col2Title: string = "Phone Call";
        var Col3Title: string = "Appointment";



        var labelIndex = 0;
        var val1Index = 0;
        var val2Index = 0;
        var val3Index = 0;
        var list = List.sort((a, b) => { return (a.StringProperty === b.StringProperty) ? 0 : (a.StringProperty < b.StringProperty) ? -1 : 1 });;
        list.forEach(element => {
            var Accepted;
            var Declined;

            if (i == 0) {
                Graphs = [{
                    "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                    "fillAlphas": 1,
                    "id": "AmGraph-1" + i,

                    "title": Col1Title,
                    "type": "column",
                    "valueField": "col1",
                    "fillColors": ["#DA7B38", "#ecbd9b"],
                    "lineAlpha": 0,
                    "gradientOrientation": "horizontal",
                    "borderAlpha": 0,

                },
                    {
                        "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "id": "AmGraph-2" + i,
                        "title": Col2Title,
                        "type": "column",
                        "lineAlpha": 0,

                        "valueField": "col2",
                        "fillColors": ["#21782E", "#90bb96"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,

                    },


                    {
                        "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "id": "AmGraph-3" + i,
                        "title": Col3Title,
                        "type": "column",
                        "lineAlpha": 0,

                        "valueField": "col3",
                        "fillColors": ["#487E9F", "#c8d8e2"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,

                    }

                ];
            }
            var val1 = 0;
            var val2 = 0;
            var val3 = 0;


            switch (element.DataTypeCode) {
                case "TS":
                    {
                        val1 = element.IntegerProperty;
                        barChartData[0].label = "TS";
                        barChartData[0].data[val1Index] = val1;
                        val1Index++;
                        break;
                    }

                case "CL":
                    {
                        val2 = element.IntegerProperty;
                        barChartData[0].label = "CL";
                        barChartData[1].data[val2Index] = val2;
                        val2Index++;
                        break;
                    }


                case "AP":
                    {
                        val3 = element.IntegerProperty;
                        barChartData[0].label = "AP";
                        barChartData[2].data[val3Index] = val3;
                        val3Index++;
                        break;
                    }

            }

            if (val1 == null) {
                val1 = 0;
            }

            if (val2 == null) {
                val2 = 0;
            }
            if (val3 == null)
                val3 = 0;


            if (val1 != 0 || val2 != 0 || val3) {





                if (!barChartLabels.includes(element.StringProperty)) {
                    barChartLabels[labelIndex] = element.StringProperty;
                    labelIndex++;
                }
                i++;

                if (val1 > max)
                    max = val1;

                if (val2 > max)
                    max = val2;

                if (val3 > max)
                    max = val3;
            }

        });

        var i = 0;
        barChartLabels.forEach(item => {
            DataProvider[i] = { "category": barChartLabels[i], "col1": barChartData[0].data[i], "col2": barChartData[1].data[i], "col3": barChartData[2].data[i] };
            i++;
        });

        if (max < 5)
            max = 5;
        makeAmBarChart(this.ZoomedChartId, Graphs, DataProvider, max, true, this.legenddivId);



    }
    BackButtonClicked() {
        this.ZoomedChartVisibility = false;
        this.LoadFilteredQueries();
    }

    FillZoomedQueries(List: Array<ChartingDataClass>, Code: string) {

        var barChartData: any[] = [{ data: [], label: '' }, { data: [], label: '' }];

        var i = 0;
        var index = 0;
        var Graphs = [];
        var DataProvider = [];
        var objectArray = [];
        barChartData[0].data = [];
        barChartData[1].data = []
        var barChartLabels = [];
        var max = 0;

        var flag: string;
        var Col1Title: string;
        var Col2Title: string;

        if (Code == "Q") {
            flag = "QTAC";
            Col1Title = "Accepted";
            Col2Title = "Declined";
        }
        else if (Code = "O") {
            flag = "CWN";
            Col1Title = "Won";
            Col2Title = "Lost";
        }

        var labelIndex = 0;
        var WonValueIndex = 0;
        var LostValueIndex = 0;
        var list = List.sort((a, b) => { return (a.StringProperty === b.StringProperty) ? 0 : (a.StringProperty < b.StringProperty) ? -1 : 1 });;
        list.forEach(element => {
            var Accepted;
            var Declined;

            if (i == 0) {
                Graphs = [{
                    "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                    "fillAlphas": 1,
                    "id": "AmGraph-1" + i,

                    "title": Col1Title,
                    "type": "column",
                    "valueField": "col1",
                    "fillColors": ["#0f7816", "#5ed967"],
                    "lineAlpha": 0,
                    "gradientOrientation": "horizontal",
                    "borderAlpha": 0,

                },
                    {
                        "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "id": "AmGraph-2" + i,
                        "title": Col2Title,
                        "type": "column",
                        "lineAlpha": 0,

                        "valueField": "col2",
                        "fillColors": ["#c80d05", "#fb5851"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,

                    }

                ];
            }
            var WonValue = 0;
            var LostValue = 0;


            switch (element.DataTypeCode) {
                case flag:
                    {
                        WonValue = element.IntegerProperty;
                        barChartData[0].label = "W";
                        barChartData[0].data[WonValueIndex] = WonValue;
                        WonValueIndex++;
                        break;
                    }

                default:
                    {
                        LostValue = element.IntegerProperty;
                        barChartData[1].label = "L";
                        barChartData[1].data[LostValueIndex] = LostValue;
                        LostValueIndex++;
                        break;
                    }

            }

            if (WonValue == null) {
                WonValue = 0;
            }

            if (LostValue == null) {
                LostValue = 0;
            }


            if (WonValue != 0 || LostValue != 0) {





                if (!barChartLabels.includes(element.StringProperty)) {
                    barChartLabels[labelIndex] = element.StringProperty;
                    labelIndex++;
                }
                i++;

                if (WonValue > max)
                    max = WonValue;

                if (LostValue > max)
                    max = LostValue;
            }

        });

        var i = 0;
        barChartLabels.forEach(item => {
            DataProvider[i] = { "category": barChartLabels[i], "col1": barChartData[0].data[i], "col2": barChartData[1].data[i] };
            i++;
        });

        if (max < 5)
            max = 5;
        makeAmBarChart(this.ZoomedChartId, Graphs, DataProvider, max, true, this.legenddivId);
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

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_CloseDate, (value == null ? null : value.Code));
            if (value.Code == "-1_-1") {
                LastFilterClass.UpdateFilter(this.filterControlNameSpace, "CompanyPerformanceFromDate", (value == null ? null : ServiceHelper.GetDateString(this.FromDate)));
                LastFilterClass.UpdateFilter(this.filterControlNameSpace, "CompanyPerformanceToDate", (value == null ? null : ServiceHelper.GetDateString(this.ToDate)));
            }
        }
        this.LoadFilteredQueries();



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
    private ComputeDays() {
        var days;
        var Todate: Date = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject;
        var FromDate: Date = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject;
        if (this.SelectedDateFilter.Code == "0") {
            this.toDate = Todate;
            this.fromDate = Todate;
        }

        else if (this.SelectedDateFilter.Code == "-1") {
            FromDate.setDate(Todate.getDate() - 1);
            this.fromDate = FromDate;
            this.toDate = Todate;
        }


        else if (this.SelectedDateFilter.Code == "-7") {
            FromDate.setDate(Todate.getDate() - 6);
            this.fromDate = FromDate;
            this.toDate = Todate;
        }

        else if (this.SelectedDateFilter.Code == "-30") {
            FromDate.setMonth(Todate.getMonth() - 1);
            this.fromDate = FromDate;
            this.toDate = Todate;

        }

        else if (this.SelectedDateFilter.Code == "-90") {
            FromDate.setMonth(Todate.getMonth() - 3);
            this.fromDate = FromDate;
            this.toDate = Todate;
        }
       

        else if (this.SelectedDateFilter.Code == "-365") {
            FromDate.setMonth(Todate.getMonth() - 12);
            this.fromDate = FromDate;
            this.toDate = Todate;
        }


        else if (this.SelectedDateFilter.Code != "-1_-1") {
            FromDate = this.SelectedDateFilter.FromDate;
            this.fromDate = FromDate;
            Todate = this.SelectedDateFilter.ToDate;
            this.toDate = Todate;
        }


         }

    LoadFilteredQueries() {
        if (this.SelectedDateFilter != null) {
            this.ComputeDays();
            this.LoadOpportunitiesByWonLost();
            this.LoadOpportunitiesByType();
            this.LoadOpportunitiesByLeadSource();
            this.LoadActivities();
            this.LoadQuotes();
        }

    }

    ActivityClicking() {
        if (PieClick() != null) {
            this.OnActivityClick(PieClick());
            ResetItemPie();
        }
    }


    QuoteClicking() {
        if (PieClick() != null) {
            this.OnQuoteClick(PieClick());
            ResetItemPie();
        }
    }








    OnOpportunityClick(e, code) {

        var item = null;

        var myQueryCode: string = "All Opportunities";
        var myTableName: string = "Opportunity";
        var filterAgrs: ApiQueryFilters = new ApiQueryFilters();


        if (code == "A") {
            item = this.OpportunityListWonLost[e.index];
            filterAgrs.addAdditionalFilter("StageId", item.GroupedId, null, null, "Equals", false, false, false, "String");
        }
        else if (code == "B") {
            item = this.OpportunityListByType[e.index];
            filterAgrs.addAdditionalFilter("OpportunityTypeId", item.GroupedId, null, null, "Equals", false, false, false, "String");
        }
        else {
            item = this.OpportunitiesListByLeadSource[e.index];
            filterAgrs.addAdditionalFilter("LeadSourceId", item.GroupedId, null, null, "Equals", false, false, false, "String");
        }



        filterAgrs.addAdditionalFilter("OwnerId", item.OwnerId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ChartActualClosingDateFilter", ServiceHelper.GetDateString(this.FromDate), ServiceHelper.GetDateString(this.ToDate), null, "Equals", true, false, false, "String");
        filterAgrs.addAdditionalFilter("BusinessUnitId", item.BusinessUnitId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
        filterAgrs.addAdditionalFilter("IsClosed", true, null, null, "Equals", false, false, false, "boolean");

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

    OnQuoteClick(e) {

        var item = this.QuoteList[e.index];
        var myQueryCode: string = "All Quotes";
        var myTableName: string = "Quote";
        var filterAgrs: ApiQueryFilters = new ApiQueryFilters();



        filterAgrs.addAdditionalFilter("SalesmanUserId", item.OwnerId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("BusinessUnitId", item.BusinessUnitId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("StageId", item.GroupedId, null, null, "Equals", false, false, false, "String");
        if (item.DataTypeCode == "QTAC") {
            filterAgrs.addAdditionalFilter("ChartAcceptedDateFilter", ServiceHelper.GetDateString(this.FromDate), ServiceHelper.GetDateString(this.ToDate), null, "Equals", true, false, false, "String");


        }

        else if (item.DataTypeCode == "QTDC") {
            filterAgrs.addAdditionalFilter("ChartDeclinedDateFilter", ServiceHelper.GetDateString(this.FromDate), ServiceHelper.GetDateString(this.ToDate), null, "Equals", true, false, false, "String");

        }


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


    OnActivityClick(e) {

        var item = this.ActivityList[e.index];

        var myQueryCode: string = "All Activities";
        var myTableName: string = "Activity";
        var filterAgrs: ApiQueryFilters = new ApiQueryFilters();

        var Key = item.DataTypeCode;
        var typeName = "";
        var typeCode = "";

        switch (Key + "") {
            case "TS": { typeName = "Tasks"; typeCode = "TS"; break; }
            case "CL": { typeName = "Phone Calls"; typeCode = "CL"; break; }
            case "AP": { typeName = "Appointments"; typeCode = "AP"; break; }
            case "EO": { typeName = "Emails Out"; typeCode = "EO"; break; }
            case "EI": { typeName = "Emails In"; typeCode = "EI"; break; }
        }

        filterAgrs.addAdditionalFilter("IsOpen", false, null, null, "Equals", false, false, false, "boolean");
        filterAgrs.addAdditionalFilter("ActivityStatusCode", "C", null, null, "Equals", true, false, false, "String");
        filterAgrs.addAdditionalFilter("OwnerId", item.OwnerId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("BusinessUnitId", item.BusinessUnitId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ActivityTypeCode", item.DataTypeCode, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ChartCompleteDateFilter", ServiceHelper.GetDateString(this.FromDate), ServiceHelper.GetDateString(this.ToDate), null, "Equals", true, false, false, "String");

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

    FillQuotes(result) {
        try {
            if (this.CurrentQuoteChart != null) {
                this.CurrentQuoteChart.clear();
                this.CurrentQuoteChart = null;
            }
        }
        catch (er) { }
        if (result.Result.length == 0) {
            this.AcceptedDeclinedQuotesExistance = false;
        }
        else {

            this.FillQuotesList(result.Result);
            this.AcceptedDeclinedQuotesExistance = true;

        }
    }
    LoadQuotes() {
        if (this.SelectedDateFilter.Code == "-1_-1") {
            this.crmDomainService.GetQuotesChartDataCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, "").subscribe(result => {
                this.FillQuotes(result);
            });
        }
        else {
            this.crmDomainService.GetQuotesChartData(this.SelectedDateFilter.Code + "", this.OwnerId, this.BusinessUnitId, "").subscribe(result => {
                this.FillQuotes(result);
            });
        }
    }
    public ActivityList: Array<any> = [];
    public QuoteList: Array<any> = [];

    
    FillQuotesList(List: Array<ChartingDataClass>) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.QuoteList = List;
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
            
            this.CurrentQuoteChart= makePieChart(this.AcceptedDeclinedQuotes, fullData, false, true, this.QuoteLegendId,150);
        }

    }



    FillActivities(result) {
        if (this.CurrentActivityChart != null) {
            this.CurrentActivityChart.clear();
            this.CurrentActivityChart = null;
        }
        if (result.Result.length == 0) {
            this.CompletedActivitiesIdExistance = false;          
        }
        else {
            this.CompletedActivitiesIdExistance = true;
            this.FillActivitiesList(result.Result);
        }
    }
    LoadActivities() {
        if (this.SelectedDateFilter.Code == "-1_-1") {
            this.crmDomainService.GetActivitiesChartDataCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, "").subscribe(result => {
                this.FillActivities(result);
            });
        }
        else {
            this.crmDomainService.GetActivitiesChartData(this.SelectedDateFilter.Code, this.OwnerId, this.BusinessUnitId, "").subscribe(result => {
                this.FillActivities(result);
            });
        }
    }

    FillActivitiesList(List: Array<ChartingDataClass>) {

        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.ActivityList = List;
        List.forEach(element => {
            var label = element.StringProperty;
            label == "Call" ? label = "Phone Call":label=label;
            fullData.push({ label: label, data: element.IntegerProperty })
            pieChartLabels.push(label);
            pieChartData.push(element.IntegerProperty);

        });
        var flagEmpty = true;
        pieChartData.forEach(p => {
            if (p != "0")
                flagEmpty = false;
        });
        if (!flagEmpty) {
           
            this.CurrentActivityChart = makePieChart(this.CompletedActivitiesId, fullData, false, true, this.ActivityChartLegendId, 150);
        }        
    }


    FillOpportunitiesByLeadSource(result) {
        if (this.CurrentOpportunityByLeadSourceChart != null) {
            this.CurrentOpportunityByLeadSourceChart.clear();
            this.CurrentOpportunityByLeadSourceChart = null;
        }
        if (result.Result.length == 0) {
            this.OpportunitiesbyLeadSourcetypeIdExistance = false;
            try {
            }
            catch (er) { }
        }
        else {
            this.OpportunitiesbyLeadSourcetypeIdExistance = true;
            this.FillOpportunitiesListByLeadSource(result.Result);
        }        
    }

    LoadOpportunitiesByLeadSource() {
        if (this.SelectedDateFilter.Code == "-1_-1") {
            this.crmDomainService.GetOpportunitiesChartDataCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, "LS").subscribe(result => {
                this.FillOpportunitiesByLeadSource(result);
            });
        }
        else {
            this.crmDomainService.GetOpportunitiesChartData(this.SelectedDateFilter.Code + "", this.OwnerId, this.BusinessUnitId, "LS").subscribe(result => {
                this.FillOpportunitiesByLeadSource(result);
            });
        }
    }
    public NewActivitiesYAxisFitlerd: Array<any> = [];
    public NewCustomersYAxisFitlerd: Array<any> = [];

    FillOppotuniriesByType(result) {
        try {
            if (this.CurrentOpportunityByTypeChart != null) {
                this.CurrentOpportunityByTypeChart.clear();
                this.CurrentOpportunityByTypeChart = null;
            }
        }
        catch (er) { }

        if (result.Result.length == 0) {

            this.OpportunitiesByTypeIdExistance = false;
        }
        else {

            this.FillOpportunitiesListBySalesman(result.Result);
            this.OpportunitiesByTypeIdExistance = true;
        }        
    }
    LoadOpportunitiesByType() {
        if (this.SelectedDateFilter.Code == "-1_-1") {
            this.crmDomainService.GetOpportunitiesChartDataCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, "T").subscribe(result => {
                this.FillOppotuniriesByType(result);
            });
        }
        else {
            this.crmDomainService.GetOpportunitiesChartData(this.SelectedDateFilter.Code + "", this.OwnerId, this.BusinessUnitId, "T").subscribe(result => {
                this.FillOppotuniriesByType(result);
            });
        }
    }

    FillOpportunitiesByWonLost(result) {
        try {
            if (this.CurrentOpportunityByWonLostChart != null) {
                this.CurrentOpportunityByWonLostChart.clear();
                this.CurrentOpportunityByWonLostChart = null;
            }
        }
        catch (er) { }

        if (result.Result.length == 0) {

            this.OpportunitiesWonLostRatioIdExistance = false;
        }
        else {

            this.FillOpportunitiesListWonLost(result.Result);
            this.OpportunitiesWonLostRatioIdExistance = true;

        }
    }

    LoadOpportunitiesByWonLost() {
        if (this.SelectedDateFilter.Code == "-1_-1") {
            this.crmDomainService.GetOpportunitiesChartDataCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, "WL").subscribe(result => {
                this.FillOpportunitiesByWonLost(result);
            });
        }
        else {
            this.crmDomainService.GetOpportunitiesChartData(this.SelectedDateFilter.Code + "", this.OwnerId, this.BusinessUnitId, "WL").subscribe(result => {
                this.FillOpportunitiesByWonLost(result);
            });
        }
    }

    public OpportunityListWonLost: Array<any> = [];
    public OpportunitiesListByLeadSource: Array<any> = [];
    public OpportunityListByType: Array<any> = [];

    

    FillOpportunitiesListWonLost(List: Array<ChartingDataClass>) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.OpportunityListWonLost = List;
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
           
            this.CurrentOpportunityByWonLostChart=makePieChart(this.OpportunitiesWonLostRatioId, fullData, false, true, this.OpportunityByWonLostLegendId,150);
        }

    }

    OpportunityClicking(code:string) {
        if (PieClick() != null) {
            this.OnOpportunityClick(PieClick(), code);
            ResetItemPie();
        }

    }
    FillOpportunitiesListBySalesman(List: Array<ChartingDataClass>) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.OpportunityListByType = List;
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
           
            this.CurrentOpportunityByTypeChart= makePieChart(this.OpportunitiesByTypeId, fullData, false, true, this.OpportunityByTypeLegendId,150);
        }

    }

    FillOpportunitiesListByLeadSource(List: Array<ChartingDataClass>) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.OpportunitiesListByLeadSource = List;
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
           
            this.CurrentOpportunityByLeadSourceChart=makePieChart(this.OpportunitiesbyLeadSourcetypeId, fullData, false, true, this.OpportunityByLeadSourceLegendId,150);
        }

    }

  
}
