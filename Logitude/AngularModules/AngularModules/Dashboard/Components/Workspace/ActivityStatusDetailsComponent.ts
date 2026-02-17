import {Component, Output, EventEmitter, OnInit, AfterViewInit, ViewEncapsulation} from '@angular/core'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {DashBoardFilters} from '../../../Infrastructure/DataContracts/Dashboard/DashboardFilters';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {DirectionTransportFilter} from '../../../Infrastructure/DataContracts/Dashboard/DirectionTransportFilter';
import {DashBoardClass} from '../../../Infrastructure/DataContracts/Dashboard/DashBoardClass';
import {GroupByClass} from '../../../Infrastructure/DataContracts/Dashboard/GroupByClass';
import {FunctionsCRM} from '../../../Infrastructure/DataContracts/Dashboard/FunctionsCRM';
import {List} from '../../../Infrastructure/DataContracts/Dashboard/List';
import {DailySpotlightClass} from '../../../Infrastructure/DataContracts/Dashboard/DailySpotlightClass';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {TransportsFilter} from '../../../Controls/TransportsFilter';
import {DirectionsFilter} from '../../../Controls/DirectionsFilter';
import {DashboardDomainService} from '../../Services/DashboardDomainService';
import {LastFilter} from '../../../Infrastructure/Utilities/LastFilter';
import {LastFilterClass} from '../../../Infrastructure/Utilities/LastFilterClass';
import {FormatTool, AppTool, DateTool} from '../../../Infrastructure/Tools';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';

declare var  makeAmBarChart, makePieChart;

@Component({
    selector: 'ActivityStatusDetailsComponent',
    moduleId: module.id,
    templateUrl: './ActivityStatusDetailsComponent.html',
    encapsulation: ViewEncapsulation.None,

})

export class ActivityStatusDetailsComponent extends BaseComponent implements OnInit  {

    private CurrentDirectionAndTransportModeChart: any;
    private CurrentCustomersChart: any;
    private CurrentCountriesChart: any;
    public CountriesDashboardLegendId: string;
    public CustomersDashboardLegendId: string;
    public DirectionAndtransportModeLegendId: string;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.TenantPM = InfraSettings.TenantPM;
        this.DirectionAndTransportModeDashboardId = this.DirectionAndTransportModeDashboardId + this.CurrentSession.GetChartId();
        this.ShipmentsQuantityByTimeDashboardId = this.ShipmentsQuantityByTimeDashboardId + this.CurrentSession.GetChartId();
        this.CountriesDashboardId = this.CountriesDashboardId + this.CurrentSession.GetChartId();
        this.CustomersDashboardId = this.CustomersDashboardId + this.CurrentSession.GetChartId();
        this.CountriesDashboardLegendId = "CountriesDashboardLegendId_" + this.CurrentSession.GetNewId("CountriesDashboardLegendId");        
        this.CustomersDashboardLegendId = "CustomersDashboardLegendId_" + this.CurrentSession.GetNewId("CustomersDashboardLegendId");        
        this.DirectionAndtransportModeLegendId = "DirectionAndtransportModeLegendId_" + this.CurrentSession.GetNewId("DirectionAndtransportModeLegendId");
    }


   

    public TenantPM: TenantPM;
    FilterList: DashBoardFilters[] = [];
    FilterListActivity: DashBoardFilters[] = [];
    public ShipmentsQuantityByTimeDashboardId: string = "ShipmentsQuantityByTimeDashboardId_";
    public DirectionAndTransportModeDashboardId: string = "DirectionAndTransportModeDashboardId_";
    public CountriesDashboardId: string = "CountriesDashboardId_";
    public CustomersDashboardId: string = "CustomersDashboardId_";
    public DataContext: ActivityStatusDetailsComponent = this;
    public FinalShipmentData: any;
    public topCustomers: any=10;
    public topCountries: any=10;

    public get TopCountries() { return this.topCountries; }
    public set TopCountries(value: any) { this.topCountries = value; }

    public get TopCustomers() { return this.topCustomers; }
    public set TopCustomers(value: any) {
    this.topCustomers = value;
        
    }

    public includeOthersCountries: any = true;

    public get IncludeOthersCountries() { return this.includeOthersCountries; }
    public set IncludeOthersCountries(value: boolean) {
    this.includeOthersCountries = value;
    this.LoadShipmentsByTop10Countries();
    }
    public includeOthersCustomers: any = false;

    public get IncludeOthersCustomers() { return this.includeOthersCustomers; }
    public set IncludeOthersCustomers(value: any) {
    this.includeOthersCustomers = value;

    this.LoadCustomers();
    }

    public FinalDirectionAndTransportData: any;
    public FinalCountriesData: any;
    public FinalCustomersData: any;
    public barChartLabels: string[] = [];
    public NoShipmentsQuantityByTime: boolean = false;
    public NoDirectionAndTransportMode: boolean = false;
    public NoCountries: boolean = false;
    public NoCustomers: boolean = false;    
    public barChartData: any[] = [{
        data: [], label: '', scaleShowVerticalLines: false,
}];

    private selectedDirectionFilterShipment: string = "All";
    get SelectedDirectionFilterShipment() { return this.selectedDirectionFilterShipment; }
    set SelectedDirectionFilterShipment(newValue: string) {
        if (this.selectedDirectionFilterShipment != newValue) {
            this.selectedDirectionFilterShipment = newValue;
            if (this.SelectedTimeRangeItem.Index == "-1") {
                this.LoadActivityStatus();
            }
            else {
                this.CommonFiltersShipment();
            }
        }
    }
    
    private selectedTransportFilterShipment: string = "All";
    get SelectedTransportFilterShipment() { return this.selectedTransportFilterShipment; }
    set SelectedTransportFilterShipment(newValue: string) {
        if (this.selectedTransportFilterShipment != newValue) {
            this.selectedTransportFilterShipment = newValue;
            if (this.SelectedTimeRangeItem.Index == "-1") {
                this.LoadActivityStatus();
            }
            else {
                this.CommonFiltersShipment();
            }
        }

    }


    private selectedDirectionFilterCountries: string = "All";
    get SelectedDirectionFilterCountries() { return this.selectedDirectionFilterCountries; }
    set SelectedDirectionFilterCountries(newValue: string) {
        if (this.selectedDirectionFilterCountries != newValue) {
            this.selectedDirectionFilterCountries = newValue;
            this.LoadShipmentsByTop10Countries();
        }
    }

    private selectedTransportFilterCountries: string = "All";
    get SelectedTransportFilterCountries() { return this.selectedTransportFilterCountries; }
    set SelectedTransportFilterCountries(newValue: string) {
        if (this.selectedTransportFilterCountries != newValue) {
            this.selectedTransportFilterCountries = newValue;
            this.LoadShipmentsByTop10Countries();
        }

    }


    private selectedDirectionFilterCustomers: string = "All";
    get SelectedDirectionFilterCustomers() { return this.selectedDirectionFilterCustomers; }
    set SelectedDirectionFilterCustomers(newValue: string) {
        if (this.selectedDirectionFilterCustomers != newValue) {
            this.selectedDirectionFilterCustomers = newValue;
            this.LoadCustomers();
        }
    }

    private selectedTransportFilterCustomers: string = "All";
    get SelectedTransportFilterCustomers() { return this.selectedTransportFilterCustomers; }
    set SelectedTransportFilterCustomers(newValue: string) {
        if (this.selectedTransportFilterCustomers != newValue) {
            this.selectedTransportFilterCustomers = newValue;
            this.LoadCustomers();
        }

    } private filterName_DateType: string = "DateType";
    private filterName_TimeRange: string = "TimeRange";
    private filterControlNameSpace: string = "Components.Partners.EditTabs.Customer.CustomerOverviewTabComponent";

    LoadActivityStatus() {
        var service = new DashboardDomainService();


        if (this.SelectedTimeRangeItem.Index == "-1") {
            if (this.ActivityFromDate != null && this.ActivityToDate != null) {
                service.GetActivityStatusByType(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate, this.TenantPM.Id + "", this.SelectedDirectionFilterShipment, this.SelectedTransportFilterShipment).subscribe(myResult => {
                    this.FinalShipmentData = myResult;
                    this.CommonFiltersShipment();
                });
            }
        }
        else {
            var days = this.ComputeDays();
            service.GetActivityStatus(this.SelectedDateTypeItem.Index, 0, days, this.TenantPM.Id).subscribe(myResult => {
                this.FinalShipmentData = myResult;
                this.CommonFiltersShipment();
            });          
        }        
    }

    LoadDirectionAndTransportmode() {
        var service = new DashboardDomainService();
        if (this.SelectedTimeRangeItem.Index == "-1") {
            if (this.ActivityFromDate != null && this.ActivityToDate != null) {
                service.GetShipmentByDirectionAndTransmodeCustom(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate).subscribe(myResult => {
                    this.FinalDirectionAndTransportData = myResult;
                    this.CommonFiltersDirectionAndTransportMode();
                });
              
            }
        }
        else {
            var days = this.ComputeDays();

            service.GetShipmentByDirectionAndTransmode(this.SelectedDateTypeItem.Index, 0, days, this.TenantPM.Id, null).subscribe(myResult => {
                this.FinalDirectionAndTransportData = myResult;
                this.CommonFiltersDirectionAndTransportMode();
            });           
        }
    }

    LoadShipmentsByTop10Countries() {
        var service = new DashboardDomainService();
        if (this.SelectedTimeRangeItem.Index == "-1") {
            if (this.ActivityFromDate != null && this.ActivityToDate != null) {                        
                var dtf: DirectionTransportFilter = this.GetCurrentDirectionTransmodeFilterItemCountries();
                service.GetShipmentsByTop10CountriesDashBoardCustom(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate, parseInt(this.SelectedShowItem.Index), this.TenantPM.Id, this.TopCountries, this.IncludeOthersCountries, "", dtf.FilterDirectionID, dtf.FilterTransportID).subscribe(myResult => {
                    this.FinalCountriesData = myResult;
                    var countriesFilterdList: List<GroupByClass> = FunctionsCRM.getCountriesFilterdList(this.FinalCountriesData, parseInt(this.SelectedShowItem.Index), this.TopCountries, this.IncludeOthersCountries);
                    this.fillCountriesPie(countriesFilterdList);
                });
            }
        }
        else {
            this.CommonFiltersCountries();        
        }
    }


    LoadCustomers() {
        if (this.SelectedTimeRangeItem.Index == "-1") {
            if (this.ActivityFromDate != null && this.ActivityToDate != null) {                              
                var service = new DashboardDomainService();
                service.GetTop10DashBoardCustom(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate, parseInt(this.SelectedShowItem.Index), this.TenantPM.Id, this.TopCustomers, this.IncludeOthersCustomers, this.SelectedDirectionFilterCustomers, this.SelectedTransportFilterCustomers).subscribe(myResult => {
                    this.FinalCustomersData = myResult;
                    this.FillCustomersPie();
                });
            }
        }
        else {
            this.CommonFiltersCustomers();
        }

    }

    LoadQuires() {        
        this.LoadActivityStatus();
        this.LoadDirectionAndTransportmode();
        this.LoadShipmentsByTop10Countries();
        this.LoadCustomers();

      
    }


    CommonFiltersCustomers() {
        var service = new DashboardDomainService();
        var days = this.ComputeDays();
        service.GetTop10DashBoard(this.SelectedDateTypeItem.Index, 0, days, parseInt(this.SelectedShowItem.Index), this.TenantPM.Id, this.TopCustomers, this.IncludeOthersCustomers, this.SelectedDirectionFilterCustomers, this.SelectedTransportFilterCustomers).subscribe(myResult => {
            this.FinalCustomersData = myResult;
            this.FillCustomersPie();
        });
    }

    FillCustomersPie() {
        if (this.FinalCustomersData != null) {
            var dtf: DirectionTransportFilter = this.GetCurrentDirectionTransmodeFilterItemCustomers();
            var directionFilteredList: List<DashBoardClass> = FunctionsCRM.getDirectionFilteredList(dtf, this.FinalCustomersData);
            var measurmentFilteredList: List<GroupByClass> = FunctionsCRM.getCustomersFilterdList(directionFilteredList, parseInt(this.SelectedShowItem.Index));
            var pieChartLabels = [];
            var pieChartData = [];
            var fullData = [];         

            measurmentFilteredList.getAll() != null ? measurmentFilteredList.getAll().forEach(element => {
                if (element.YField != 0) {
                    fullData.push({ label: element.XField, data: element.YField })
                    pieChartLabels.push(element.XField);
                    pieChartData.push(element.YField);
                }
            }):null;



            var flagEmpty = true;
            pieChartData.forEach(p => {
                if (p != "0")
                    flagEmpty = false;
            });
            if (this.CurrentCustomersChart != null) {
                this.CurrentCustomersChart.clear();
                this.CurrentCustomersChart = null;
            }
            if (!flagEmpty) {              
                this.CurrentCustomersChart = makePieChart(this.CustomersDashboardId, fullData, false, true, this.CustomersDashboardLegendId);
                this.NoCustomers = false;

            }

            else {
                try {
                 //   var elm = document.getElementById(this.CustomersDashboardId);
                }
                catch (exc) { }

              //  elm.innerHTML = "";
                this.NoCustomers = true;

            }

        }

    }
    
    GetCurrentDirectionTransmodeFilterItemShipment() {
        var transmodeId: string = "";
        var directionId: string = "";

        if (this.SelectedDirectionFilterShipment == "All")
            directionId = "";
        else
            directionId = this.SelectedDirectionFilterShipment;

        if (this.SelectedTransportFilterShipment == "All")
            transmodeId = "";
        else
            transmodeId = this.SelectedTransportFilterShipment;
        
        var filterItem: DirectionTransportFilter = new DirectionTransportFilter("", directionId, transmodeId);


        return filterItem;


    }
    GetCurrentDirectionTransmodeFilterItemCountries() {
        var transmodeId: string = "";
        var directionId: string = "";

        if (this.SelectedDirectionFilterCountries == "All")
            directionId = "";
        else
            directionId = this.SelectedDirectionFilterCountries;

        if (this.SelectedTransportFilterCountries == "All")
            transmodeId = "";
        else
            transmodeId = this.SelectedTransportFilterCountries;

        var filterItem: DirectionTransportFilter = new DirectionTransportFilter("", directionId, transmodeId);


        return filterItem;


    }
    GetCurrentDirectionTransmodeFilterItemCustomers() {
        var transmodeId: string = "";
        var directionId: string = "";

        if (this.SelectedDirectionFilterCustomers == "All")
            directionId = "";
        else
            directionId = this.SelectedDirectionFilterCustomers;

        if (this.SelectedTransportFilterCustomers == "All")
            transmodeId = "";
        else
            transmodeId = this.SelectedTransportFilterCustomers;

        var filterItem: DirectionTransportFilter = new DirectionTransportFilter("", directionId, transmodeId);


        return filterItem;


    }


    CommonFiltersDirectionAndTransportMode() {
        if (this.SelectedTimeRangeItem.Index != "-1") {
            var byMonthData: List<DashBoardClass> = this.FinalDirectionAndTransportData != null ? this.FinalDirectionAndTransportData : []
          
            var measurmentFilteredList: List<GroupByClass> = FunctionsCRM.getMeasurmentFilterListForDirectionAndTransmode(parseInt(this.SelectedShowItem.Index), byMonthData, parseInt(this.SelectedTimeRangeItem.Index), null);
            this.FillDirectionPie(measurmentFilteredList);
        }
        else {
            var FilteredList: List<GroupByClass> = new List<GroupByClass>();
            this.FinalDirectionAndTransportData != null ? this.FinalDirectionAndTransportData.items.forEach((item: DashBoardClass) => {
                var obj: GroupByClass = new GroupByClass();
                obj.XField = item.DirectionName + "/" + item.TransportModeName;
                switch (parseInt(this.SelectedShowItem.Index)) {
                    case 0:
                        {
                            obj.YField = item.Total != null ? item.Total : 0;

                            break;
                        }

                    case 1:
                        {
                            obj.YField = item.SumChargeableWeight != null ? item.SumChargeableWeight : 0;

                            break;
                        }   


                    case 2:
                        {
                            obj.YField = item.SumGrossWeight != null ? item.SumGrossWeight : 0;

                            break;
                        }

                    case 3:
                        {
                            obj.YField = item.TotalProfitInLocalCurrency != null ? item.TotalProfitInLocalCurrency : 0;

                            break;
                        }


                    case 4:
                        {
                            obj.YField = item.TotalProfitInProfitCurrency != null ? item.TotalProfitInProfitCurrency : 0;

                            break;
                        }

                    case 5:
                        {
                            obj.YField = item.ReceivablesInLocalCurrency != null ? item.ReceivablesInLocalCurrency : 0;

                            break;
                        }

                    case 6:
                        {
                            obj.YField = item.ReceivablesInProfitCurrency != null ? item.ReceivablesInProfitCurrency : 0;
                            break;
                        }


                }
                FilteredList.add(obj);
            }):null;
            this.FillDirectionPie(FilteredList);
        }
               
    }

    CommonFiltersCountries() {
        var days = this.ComputeDays();
        var dtf: DirectionTransportFilter = this.GetCurrentDirectionTransmodeFilterItemCountries();
        var service = new DashboardDomainService();      
        service.GetShipmentsByTop10CountriesDashBoard(this.SelectedDateTypeItem.Index, 0, days, parseInt(this.SelectedShowItem.Index), this.TenantPM.Id, this.TopCountries, this.IncludeOthersCountries, "", dtf.FilterDirectionID, dtf.FilterTransportID).subscribe(myResult => {
            this.FinalCountriesData = myResult;
            var countriesFilterdList: List<GroupByClass> = FunctionsCRM.getCountriesFilterdList(this.FinalCountriesData, parseInt(this.SelectedShowItem.Index), this.TopCountries, this.IncludeOthersCountries);
            this.fillCountriesPie(countriesFilterdList);
        });
    }

    fillCountriesPie(data: List<GroupByClass>) {
        var fullData = [];         
        var pieChartLabels = [];
        var pieChartData = [];

        data.getAll() != null ? data.getAll().forEach(element => {
            if (element.YField != 0) {
                fullData.push({ label: element.XField, data: element.YField })
                pieChartLabels.push(element.XField);
                pieChartData.push(element.YField);
            }
        }):null;




        var flagEmpty = true;
        pieChartData.forEach(p => {
            if (p != "0")
                flagEmpty = false;
        });
        if (this.CurrentCountriesChart != null) {
            this.CurrentCountriesChart.clear();
            this.CurrentCountriesChart = null;
        }
        if (!flagEmpty) {
         
            this.CurrentCountriesChart=makePieChart(this.CountriesDashboardId, fullData, false, true, this.CountriesDashboardLegendId);

            this.NoCountries = false;

        }

        else {        
            this.NoCountries = true;
        }
    }
        
    FillDirectionPie(data: List<GroupByClass>) {

        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];         

        data.getAll() != null ? data.getAll().forEach(element => {
            if (element.YField != 0) {

                fullData.push({ label: element.XField, data: element.YField })

                pieChartLabels.push(element.XField);
                pieChartData.push(element.YField);
            }
        }):null;



        var flagEmpty = true;
        pieChartData.forEach(p => {
            if (p != "0")
                flagEmpty = false;
        });
        if (this.CurrentDirectionAndTransportModeChart != null) {
            this.CurrentDirectionAndTransportModeChart.clear();
            this.CurrentDirectionAndTransportModeChart = null;
        }
        if (!flagEmpty) {          
            this.CurrentDirectionAndTransportModeChart = makePieChart(this.DirectionAndTransportModeDashboardId, fullData, false, true, this.DirectionAndtransportModeLegendId);
            this.NoDirectionAndTransportMode = false;
        }

        else {       
            this.NoDirectionAndTransportMode = true;
        }
    }
    
    CommonFiltersShipment() {

        var showIndex: number = parseInt(this.SelectedShowItem.Index);
        var timeIndex: number = parseInt(this.SelectedTimeRangeItem.Index);

        var byMonthData: List<DashBoardClass> = this.FinalShipmentData != null ? this.FinalShipmentData : new List<DashBoardClass> ();
        var dtf: DirectionTransportFilter = this.GetCurrentDirectionTransmodeFilterItemShipment();
        var directionFilteredList: List<DashBoardClass> = FunctionsCRM.getDirectionFilteredList(dtf, byMonthData);

        if (timeIndex == -1) {
            var diff = DateTool.GetDaysBetweenDates(this.ActivityFromDate, this.ActivityToDate) - 1;
            if (diff <= 7) {
                timeIndex = 0;
            }

            else if (diff <= 30 || (diff <= 32 && this.ActivityFromDate.getDate() == this.ActivityFromDate.getDate())) {
                timeIndex = 1;
            }

            else if (diff <= 90 || (diff <= 93 && this.ActivityFromDate.getDate() == this.ActivityFromDate.getDate())) {
                timeIndex = 2;
            }

            else if (diff <= 365 || (diff <= 365 && this.ActivityFromDate.getDate() == this.ActivityFromDate.getDate())) {
                timeIndex = 3;
            }
            else {
                timeIndex = 4;
            }

        }
        if (this.SelectedTimeRangeItem.Index != "-1") {
            var measurmentFilteredList: List<GroupByClass> = FunctionsCRM.getMeasurmentFilterList(showIndex, directionFilteredList, timeIndex, null);
            var monthQuartersList: List<GroupByClass> = FunctionsCRM.getmonthQuartersList(timeIndex, measurmentFilteredList, null);
            this.FillBars(monthQuartersList);
        }
        else {            
                var FilteredList: List<GroupByClass> = new List<GroupByClass>();
            this.FinalShipmentData != null ? this.FinalShipmentData.items.forEach((item: DashBoardClass) => {
                    var obj: GroupByClass = new GroupByClass();
                    obj.XField = item.DateRange;
                    switch (parseInt(this.SelectedShowItem.Index)) {
                        case 0:
                            {
                                obj.YField = item.Total != null ? item.Total : 0;
                                break;
                            }

                        case 1:
                            {
                                obj.YField = item.SumChargeableWeight != null ? item.SumChargeableWeight : 0;

                                break;
                            }


                        case 2:
                            {
                                obj.YField = item.SumGrossWeight != null ? item.SumGrossWeight : 0;

                                break;
                            }

                        case 3:
                            {
                                obj.YField = item.TotalProfitInLocalCurrency != null ? item.TotalProfitInLocalCurrency : 0;

                                break;
                            }


                        case 4:
                            {
                                obj.YField = item.TotalProfitInProfitCurrency != null ? item.TotalProfitInProfitCurrency : 0;

                                break;
                            }

                        case 5:
                            {
                                obj.YField = item.ReceivablesInLocalCurrency != null ? item.ReceivablesInLocalCurrency : 0;

                                break;
                            }

                        case 6:
                            {
                                obj.YField = item.ReceivablesInProfitCurrency != null ? item.ReceivablesInProfitCurrency : 0;
                                break;
                            }


                    }
                    FilteredList.add(obj);
                }):null;


                this.FillBars(FilteredList);
                                   
        }        
    }
   

    FilterSelectedChangeShow(item) {

        this.SelectedShowItem = item;
        this.CommonFiltersShipment();
        
        this.CommonFiltersDirectionAndTransportMode();

        this.LoadShipmentsByTop10Countries();
        this.LoadCustomers();

    }
    FillBars(data: List<GroupByClass>) {
       
        var i = 0;
        var index = 0;
        var Graphs = Graphs = [{
            "balloonText": "[[value]]",
            "fillAlphas": 1,
            "id": "AmGraph-1" + i,
            "title": "Invoices",
            "type": "column",
            "valueField": "col1",
            "fillColors": ["#487E9F", "#c8d8e2"],
            "gradientOrientation": "horizontal",
            "borderAlpha": 0,
            "lineAlpha": 0,

        }];
        var max = 0;

       
        var DataProvider = [];
        this.barChartData[0].data = [];
        this.barChartLabels = [];
        data.getAll() != null ? data.getAll().forEach(element => {
            this.barChartData[0].label = "Quantity";

            this.barChartData[0].data[i] = element.YField + "";;


            this.barChartLabels[i] = element.XField + "";

            DataProvider[i] = { "category": this.barChartLabels[i], "col1": this.barChartData[0].data[i] };


            if (element.YField > max)
                max = element.YField;
            i++;
        }):null;

       var barChartColors: any[] = [
            {
               backgroundColor:"rgb(73,165,191)" /*Safari 5.1-6*/
               
               ,
               borderColor: "rgba(147,206,222,1)",


                borderWidth: 2
           }

       
          
           
        ]
       var flagEmpty = true;
       this.barChartData[0].data.forEach(p => {
           if (p != "0")
               flagEmpty = false;
       });
       if (!flagEmpty) {


           makeAmBarChart(this.ShipmentsQuantityByTimeDashboardId, Graphs, DataProvider, max);

           this.NoShipmentsQuantityByTime = false;

       }

       else {
           try {
               var elm = document.getElementById(this.ShipmentsQuantityByTimeDashboardId);
               elm.innerHTML = "";
           }
           catch (exc) { }
           this.NoShipmentsQuantityByTime = true;

       }             
    }
    FillFilters() {
        this.FillDateTypeFilterList();
        this.FillShowFilterList();
        this.FillTimeRangeFilterList();
        this.LoadQuires(); 
    }
    ngOnInit() {
        this.FillFilters();               
    }
    public DateTypeFilterList: DashBoardFilters[];
    public TimeRangeFilterList: DashBoardFilters[];
    FillDateTypeFilterList() {
        this.DateTypeFilterList = [];
        this.DateTypeFilterList.push(new DashBoardFilters("Create Date", "CreateDate"));
        this.DateTypeFilterList.push(new DashBoardFilters("Operational Date", "OperationalDate"));
        var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_DateType);
        if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "CreateDate";
        }
        this.selectedDateTypeItem = this.DateTypeFilterList.filter(d => d.Index == defaultFilterCode)[0];
    }
    FillTimeRangeFilterList() {
        this.TimeRangeFilterList = [];
        var list = LastFilter.ActivitymyList();
        this.TimeRangeFilterList.push(new DashBoardFilters(list[0].lastTitle, "0"));
        this.TimeRangeFilterList.push(new DashBoardFilters(list[1].lastTitle, "1"));
        this.TimeRangeFilterList.push(new DashBoardFilters(list[2].lastTitle, "2"));
        this.TimeRangeFilterList.push(new DashBoardFilters(list[3].lastTitle, "3"));
        this.TimeRangeFilterList.push(new DashBoardFilters(list[4].lastTitle, "-1"));

        var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_TimeRange);
        if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "0";
        }
        this.selectedTimeRangeItem = this.TimeRangeFilterList.filter(d => d.Index == defaultFilterCode)[0];

        if (this.selectedTimeRangeItem.Index == "-1") {
            var ActiviytFromDate = LastFilterClass.GetFilterValue(this.filterControlNameSpace, "DetailsActivityFromDate");

            if (!AppTool.IsNullOrEmpty(ActiviytFromDate)) {
                var ActivityDate: Date = new Date();
                var ActivityFromDateString = ActiviytFromDate.split(':');
                ActivityDate.setFullYear(ActivityFromDateString[0], ActivityFromDateString[1] - 1, ActivityFromDateString[2]);
                this.activityFromDate = DateTool.GetDateParts(ActivityDate).DateObject;
            }

            var ActiviytToDate = LastFilterClass.GetFilterValue(this.filterControlNameSpace, "DetailsActivityToDate");
            if (!AppTool.IsNullOrEmpty(ActiviytToDate)) {
                var ActivityDate: Date = new Date();
                var ActivityToDateString = ActiviytToDate.split(':');
                ActivityDate.setFullYear(ActivityToDateString[0], ActivityToDateString[1] - 1, ActivityToDateString[2]);
                this.activityToDate = DateTool.GetDateParts(ActivityDate).DateObject;
            }


        }






    }
    public ShowFilterList: DashBoardFilters[];
    private selectedShowItem: DashBoardFilters;
    get SelectedShowItem() { return this.selectedShowItem; }
    set SelectedShowItem(value: DashBoardFilters) {
        if (this.selectedShowItem != value) {
            this.selectedShowItem = value;

            this.FilterSelectedShow();
        }
    }    
    FilterSelectedShow() {
        this.CommonFiltersShipment();
        this.CommonFiltersDirectionAndTransportMode();
        this.LoadShipmentsByTop10Countries();
        this.LoadCustomers();
    }

    private FillShowFilterList() {
        this.ShowFilterList = [];

        this.ShowFilterList.push(new DashBoardFilters("Shipments", "0"));
        this.ShowFilterList.push(new DashBoardFilters("ChargeWeight", "1"));
        this.ShowFilterList.push(new DashBoardFilters("GrossWeight", "2"));
        this.ShowFilterList.push(new DashBoardFilters("Profit (" + SessionLocator.TenantPM.AccountingCurrencyCode + ")", "3"));
        this.ShowFilterList.push(new DashBoardFilters("Profit (" + SessionLocator.TenantPM.ProfitCurrencyCode + ")", "4"));
        this.ShowFilterList.push(new DashBoardFilters("Receivables (" + SessionLocator.TenantPM.AccountingCurrencyCode + ")", "5"));
        this.ShowFilterList.push(new DashBoardFilters("Receivables (" + SessionLocator.TenantPM.ProfitCurrencyCode + ")", "6"));

        this.selectedShowItem = this.ShowFilterList[0];
    }

    private selectedDateTypeItem: DashBoardFilters;
    get SelectedDateTypeItem() { return this.selectedDateTypeItem; }
    set SelectedDateTypeItem(value: DashBoardFilters) {
        if (this.selectedDateTypeItem != value) {
            this.selectedDateTypeItem = value;

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_DateType, (value == null ? null : value.Index));

            this.LoadQuires();
        }
    }

    private selectedTimeRangeItem: DashBoardFilters;
    get SelectedTimeRangeItem() { return this.selectedTimeRangeItem; }
    set SelectedTimeRangeItem(value: DashBoardFilters) {
        if (this.selectedTimeRangeItem != value) {
            this.selectedTimeRangeItem = value;
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_TimeRange, (value == null ? null : value.Index));
            if (value.Index == "-1") {
                LastFilterClass.UpdateFilter(this.filterControlNameSpace, "DetailsActivityFromDate", (value == null ? null : ServiceHelper.GetDateString(this.ActivityFromDate)));
                LastFilterClass.UpdateFilter(this.filterControlNameSpace, "DetailsActivityToDate", (value == null ? null : ServiceHelper.GetDateString(this.ActivityToDate)));
            }
        }
        this.LoadQuires();

    }


    private activityFromDate: Date;
    public get ActivityFromDate() { return this.activityFromDate; }
    public set ActivityFromDate(value: Date) {
        if (value != this.activityFromDate) {
            this.activityFromDate = value;
            this.SelectedTimeRangeItem = this.TimeRangeFilterList[4];
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, "DetailsActivityFromDate", (value == null ? null : ServiceHelper.GetDateString(value)));
          //  this.LoadQuires();
        }
    }

    private activityToDate: Date;
    public get ActivityToDate() { return this.activityToDate; }
    public set ActivityToDate(value: Date) {
        if (value != this.activityToDate) {
            this.activityToDate = value;
            this.SelectedTimeRangeItem = this.TimeRangeFilterList[4];
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, "DetailsActivityToDate", (value == null ? null : ServiceHelper.GetDateString(value)));
           // this.LoadQuires();
        }
    }


    ComputeDays() {
        var days;
        var Todate: Date = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject;
        this.activityFromDate = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject;
        if (this.SelectedTimeRangeItem.Index == "0") {
            days = -7;
            Todate.setDate(Todate.getDate() - 6);
            this.activityToDate = Todate;
        }

        else if (this.SelectedTimeRangeItem.Index == "1") {
            days = -30;
            Todate.setMonth(Todate.getMonth() - 1);
            this.activityToDate = Todate;
        }

        else if (this.SelectedTimeRangeItem.Index == "2") {
            days = -90;
            Todate.setMonth(Todate.getMonth() - 3);
            this.activityToDate = Todate;
        }

        else if (this.SelectedTimeRangeItem.Index == "3") {
            days = -365;
            Todate.setMonth(Todate.getMonth() - 12);
            this.activityToDate = Todate;
        }
       
        return days;

    }
 
    @Output()
    public logoff = new EventEmitter();

    BackButtonClicked() {
        this.logoff.emit();

    }

    CountriesTopValueChanged(flag) {
        if (flag)
            this.TopCountries = this.TopCountries + 1;
        else
            this.TopCountries = this.TopCountries - 1;

        if (this.TopCountries < 0)
            this.TopCountries = 0;

        this.LoadShipmentsByTop10Countries();
                
    }

    CustomersTopValueChanged(flag){
        if (flag)
            this.TopCustomers = this.TopCustomers + 1;
        else 
            this.TopCustomers = this.TopCustomers - 1;

        if (this.TopCustomers < 0)
            this.TopCustomers = 0;
        
        this.LoadCustomers();

    }
    Change() {
        console.log("Fired2");
    }

 


}
