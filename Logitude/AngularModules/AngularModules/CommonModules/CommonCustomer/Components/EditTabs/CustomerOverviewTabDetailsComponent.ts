import {Component, ChangeDetectorRef, OnInit, AfterViewInit,Output,EventEmitter,ViewEncapsulation} from '@angular/core';
import {CustomerPM} from '../../../../Common/EntityPMs/CustomerPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ImageLibraryService} from '../../../../Common/Services/Others/ImageLibraryService';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ImageParameter} from '../../../../Infrastructure/DataContracts/ImageParameter';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ListComponentArgs} from '../../../../Infrastructure/Args';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {AWBWizardArgs, NewShipmentComponentArgs} from '../../../../Shipment/Args';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {DashBoardFilters} from '../../../../Infrastructure/DataContracts/Dashboard/DashboardFilters';
import {List} from '../../../../Infrastructure/DataContracts/Dashboard/List';
import {DailySpotlightClass} from '../../../../Infrastructure/DataContracts/Dashboard/DailySpotlightClass';
import {FunctionsCRM} from '../../../../Infrastructure/DataContracts/Dashboard/FunctionsCRM';
import {DirectionTransportFilter} from '../../../../Infrastructure/DataContracts/Dashboard/DirectionTransportFilter';
import {DashBoardClass} from '../../../../Infrastructure/DataContracts/Dashboard/DashBoardClass';
import {GroupByClass} from '../../../../Infrastructure/DataContracts/Dashboard/GroupByClass';
import {LastFilter} from '../../../../Infrastructure/Utilities/LastFilter';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {DashboardDomainService} from '../../../../Dashboard/Services/DashboardDomainService';
import {FormatTool, AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {LastFilterClass} from '../../../../Infrastructure/Utilities/LastFilterClass';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';

declare var makeAMLineChart, makeAmBarChart, makePieChart;

@Component({
    selector: 'CustomerOverviewTabDetailsComponent',
    moduleId: module.id,
    templateUrl: './CustomerOverviewTabDetailsComponent.html',
    encapsulation: ViewEncapsulation.None,
})

export class CustomerOverviewTabDetailsComponent extends BaseComponent implements OnInit {
    private CurrentDirectionAndTransportModeChart: any;
    private CurrentCountriesChart: any;
    public flag: boolean = true;
    public CountriesDashboardLegendId: string;
    public DirectionAndtransportModeLegendId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.TenantPM = InfraSettings.TenantPM;
        this.myChartsService = new DashboardDomainService();
        this.ShipmentQuantityByTimeOverViewId = this.ShipmentQuantityByTimeOverViewId + this.CurrentSession.GetChartId();
        this.DirectionAndTransportModeOverViewId = this.DirectionAndTransportModeOverViewId + this.CurrentSession.GetChartId();
        this.CountriesOverviewId = this.CountriesOverviewId + this.CurrentSession.GetChartId();
        this.CountriesDashboardLegendId = "CountriesDashboardLegendId_" + this.CurrentSession.GetNewId("CountriesDashboardLegendId");
        this.DirectionAndtransportModeLegendId = "DirectionAndtransportModeLegendId_" + this.CurrentSession.GetNewId("DirectionAndtransportModeLegendId");
    }
    public ShipmentQuantityByTimeOverViewId: string = "ShipmentQuantityByTimeOverViewId_";
    public CountriesOverviewId: string = "CountriesOverviewId_";
    public DirectionAndTransportModeOverViewId: string = "DirectionAndTransportModeOverViewId_";
    public NoShipmentsQuantityByTime: boolean = false;
    public NoDirectionAndTransportMode: boolean = false;
    public NoCountries: boolean = false;
    private myChartsService: DashboardDomainService;
    public TenantPM: TenantPM;
    FilterList: DashBoardFilters[] = [];
    FilterListActivity: DashBoardFilters[] = [];
    SelectedItem: any;
    public timeRangeSelectedIndex: number = 0;

    SelectedItemActivity: any;

    public DataContext: CustomerOverviewTabDetailsComponent = this;

    public FinalShipmentData: any;
    public topCountries: any = 10;

    public get TopCountries() { return this.topCountries; }
    public set TopCountries(value: any) { this.topCountries = value; }

    private filterName_TimeRange: string = "TimeRange";
    private filterControlNameSpace: string = "Components.Partners.EditTabs.Customer.CustomerOverviewDetailsTabComponent";

    public Customer: any; 
    public includeOthersCountries: any = true;
    public get IncludeOthersCountries() { return this.includeOthersCountries; }
    public set IncludeOthersCountries(value: boolean) {
        this.includeOthersCountries = value;
        this.CommonFiltersCountries();
    }
  
    private selectedTransportFilterCustomers: string = "All"; 
    private filterName_DateType: string = "DateType";
    public FinalDirectionAndTransportData: any;
    public FinalCountriesData: any;
    public selectedDay: number;
    public selectedMonth: number;
    public FinalCustomersData: any;
    public barChartLabels: string[] = [];
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
            this.CommonFiltersCountries();
        }
    }

    private selectedTransportFilterCountries: string = "All";
    get SelectedTransportFilterCountries() { return this.selectedTransportFilterCountries; }
    set SelectedTransportFilterCountries(newValue: string) {
        if (this.selectedTransportFilterCountries != newValue) {
            this.selectedTransportFilterCountries = newValue;
            this.CommonFiltersCountries();
        }

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
                LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityFromDate", (value == null ? null : ServiceHelper.GetDateString(this.ActivityFromDate)));
                LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityToDate", (value == null ? null : ServiceHelper.GetDateString(this.ActivityToDate)));
            }
        }
        this.LoadQuires();

    }

  
    LoadActivityStatus() {
        var service = new DashboardDomainService();

        if (this.SelectedTimeRangeItem.Index == "-1") {
            if (this.ActivityFromDate != null && this.ActivityToDate != null) {
                service.GetActivityStatusByType(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate, this.TenantPM.Id + "", this.SelectedDirectionFilterShipment, this.SelectedTransportFilterShipment, this.Customer.Id).subscribe(myResult => {
                    this.FinalShipmentData = myResult;
                    this.CommonFiltersShipment();
                });            
            }
        }
        else {
            var days = this.ComputeDays();
            var month: number = 0;
            if (days == -1095) {
                month = -36;
                days = 0;
            }

            service.GetActivityStatus(this.SelectedDateTypeItem.Index, month, days, this.TenantPM.Id, this.Customer.Id).subscribe(myResult => {
                this.FinalShipmentData = myResult;
                this.CommonFiltersShipment();
            });
        }
    }


    LoadQuires() {
        var service = new DashboardDomainService(); 
        this.LoadActivityStatus();
        if (this.SelectedTimeRangeItem.Index == "-1") {          
                service.GetShipmentByDirectionAndTransmodeCustom(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate, this.Customer.Id).subscribe(myResult => {
                    this.FinalDirectionAndTransportData = myResult;
                    this.CommonFiltersDirectionAndTransportMode();
                });
                this.CommonFiltersCountries();
            }
        
        else {
            var days = this.ComputeDays();
            var month: number = 0;
            if (days == -1095) {
                month = -36;
                days = 0;
            }         
            service.GetShipmentByDirectionAndTransmode(this.SelectedDateTypeItem.Index, month, days, this.TenantPM.Id, this.Customer.Id).subscribe(myResult => {
                this.FinalDirectionAndTransportData = myResult;
                this.CommonFiltersDirectionAndTransportMode();
            });
            this.CommonFiltersCountries();
        }
        
      
    }




    private ComputeDays() {
        var days;
        var Todate: Date = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject;
        this.activityFromDate = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject;

        if (this.SelectedTimeRangeItem.Index == "0") {
            days = -90;
            Todate.setMonth(Todate.getMonth() - 3);
            this.activityToDate = Todate;
        }

        else if (this.SelectedTimeRangeItem.Index == "1") {
            days = -365;
            Todate.setMonth(Todate.getMonth() - 12);
            this.activityToDate = Todate;
        }

        else if (this.SelectedTimeRangeItem.Index == "2") {
            days = -1095;
            Todate.setMonth(Todate.getMonth() - 36);
            this.activityToDate = Todate;
        }

     

        return days;
    }

    public TimeRangeFilterList: DashBoardFilters[];

    private activityFromDate: Date;
    public get ActivityFromDate() { return this.activityFromDate; }
    public set ActivityFromDate(value: Date) {
        if (value != this.activityFromDate) {
            this.activityFromDate = value;
            this.SelectedTimeRangeItem = this.TimeRangeFilterList[3];
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityFromDate", (value == null ? null : ServiceHelper.GetDateString(value)));

        }
    }

    private activityToDate: Date;
    public get ActivityToDate() { return this.activityToDate; }
    public set ActivityToDate(value: Date) {
        if (value != this.activityToDate) {
            this.activityToDate = value;
            this.SelectedTimeRangeItem = this.TimeRangeFilterList[3];
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityToDate", (value == null ? null : ServiceHelper.GetDateString(value)));

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
   


    CommonFiltersDirectionAndTransportMode() {        
        var byMonthData: List<DashBoardClass> = this.FinalDirectionAndTransportData;
        var measurmentFilteredList: List<GroupByClass> = FunctionsCRM.getMeasurmentFilterListForDirectionAndTransmode(parseInt(this.SelectedShowItem.Index), byMonthData, this.timeRangeSelectedIndex, this.Customer.Id);
        this.FillDirectionPie(measurmentFilteredList);
    }

    CommonFiltersCountries() {

        if (this.SelectedTimeRangeItem.Index == "-1") {
            var dtf: DirectionTransportFilter = this.GetCurrentDirectionTransmodeFilterItemCountries();
            var service = new DashboardDomainService();

            service.GetShipmentsByTop10CountriesDashBoardCustom(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate, parseInt(this.SelectedShowItem.Index), this.TenantPM.Id, this.TopCountries, this.IncludeOthersCountries, this.Customer.Id, dtf.FilterDirectionID, dtf.FilterTransportID).subscribe(myResult => {
                this.FinalCountriesData = myResult;
                var countriesFilterdList: List<GroupByClass> = FunctionsCRM.getCountriesFilterdList(this.FinalCountriesData, parseInt(this.SelectedShowItem.Index), this.TopCountries, this.IncludeOthersCountries);
                this.fillCountriesPie(countriesFilterdList);
            });

        }
        else {


            var days = this.ComputeDays();
            var month: number = 0;
            if (days == -1095) {
                month = -36;
                days = 0;
            }
            var dtf: DirectionTransportFilter = this.GetCurrentDirectionTransmodeFilterItemCountries();
            var service = new DashboardDomainService();
            service.GetShipmentsByTop10CountriesDashBoard(this.SelectedDateTypeItem.Index, month, days, parseInt(this.SelectedShowItem.Index), this.TenantPM.Id, this.TopCountries, this.IncludeOthersCountries, this.Customer.Id, dtf.FilterDirectionID, dtf.FilterTransportID).subscribe(myResult => {
                this.FinalCountriesData = myResult;
                var countriesFilterdList: List<GroupByClass> = FunctionsCRM.getCountriesFilterdList(this.FinalCountriesData, parseInt(this.SelectedShowItem.Index), this.TopCountries, this.IncludeOthersCountries);
                this.fillCountriesPie(countriesFilterdList);
            });
        }
    }

    fillCountriesPie(data: List<GroupByClass>) {

        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        console.log(data);
        data.getAll().forEach(element => {
            if (element.YField != 0) {
                fullData.push({ label: element.XField, data: element.YField })
                pieChartLabels.push(element.XField);
                pieChartData.push(element.YField);
            }
        });

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
            this.CurrentCountriesChart= makePieChart(this.CountriesOverviewId, fullData, false, true, this.CountriesDashboardLegendId);     
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
        data.getAll().forEach(element => {
            if (element.YField != 0) {
                fullData.push({ label: element.XField, data: element.YField })
                pieChartLabels.push(element.XField);
                pieChartData.push(element.YField);
            }
        });        
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
            this.CurrentDirectionAndTransportModeChart= makePieChart(this.DirectionAndTransportModeOverViewId, fullData, false, true, this.DirectionAndtransportModeLegendId);
            this.NoDirectionAndTransportMode = false;
        }
        else {        
            this.NoDirectionAndTransportMode = true;
        }


    }

    CommonFiltersShipment() {
        var showIndex: number = parseInt(this.SelectedShowItem.Index);

        var byMonthData: List<DashBoardClass> = this.FinalShipmentData;
        var dtf: DirectionTransportFilter = this.GetCurrentDirectionTransmodeFilterItemShipment();
        var directionFilteredList: List<DashBoardClass> = FunctionsCRM.getDirectionFilteredList(dtf, byMonthData);

    
        if (this.SelectedTimeRangeItem.Index != "-1") {

            var measurmentFilteredList: List<GroupByClass> = FunctionsCRM.getMeasurmentFilterList(showIndex, directionFilteredList, +this.SelectedTimeRangeItem.Index, this.Customer.Id);
            var monthQuartersList: List<GroupByClass> = FunctionsCRM.getmonthQuartersList(+this.SelectedTimeRangeItem.Index, measurmentFilteredList, this.Customer.Id);
            this.FillBars(monthQuartersList);
        }
        else {
            var FilteredList: List<GroupByClass> = new List<GroupByClass>();
            this.FinalShipmentData.items != null ? this.FinalShipmentData.items.forEach((item: DashBoardClass) => {
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
            }) : null;

            this.FillBars(FilteredList);
        }
        
       
    }



    FilterSelectedChangeShow(item) {

        this.SelectedShowItem = item;
        this.CommonFiltersShipment();

        this.CommonFiltersDirectionAndTransportMode();

        this.CommonFiltersCountries();

    }
    FillBars(data: List<GroupByClass>) {

        var Graphs = Graphs = [{
            "balloonText": "[[value]]",
            "fillAlphas": 1,
            "id": "AmGraph-1" + i,
            "title": "Invoices",
            "type": "column",
            "lineAlpha": 0,
            "valueField": "col1",
            "fillColors": ["#487E9F", "#c8d8e2"],
            "gradientOrientation": "horizontal",
            "borderAlpha": 0,
        }];
        var max = 0;

        var DataProvider = [];

        var i = 0;
        var index = 0;
        this.barChartData[0].data = [];
        this.barChartLabels = [];
        this.barChartData= [{
            data: [], label: '', scaleShowVerticalLines: false,
        }];

        data.getAll().forEach(element => {
            this.barChartData[0].label = "Quantity";

            this.barChartData[0].data[i] = element.YField + "";


            this.barChartLabels[i] = element.XField + "";;
            DataProvider[i] = { "category": this.barChartLabels[i], "col1": this.barChartData[0].data[i] };

            if (element.YField > max)
                max = element.YField;


            i++;
        });

        var barChartColors: any[] = [
            {
                backgroundColor: "rgb(73,165,191)" /*Safari 5.1-6*/

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


            makeAmBarChart(this.ShipmentQuantityByTimeOverViewId, Graphs, DataProvider, max,false,null);

            this.NoShipmentsQuantityByTime = false;

        }

        else {
            try {
                var elm = document.getElementById(this.ShipmentQuantityByTimeOverViewId);
                elm.innerHTML = "";
            }
            catch (exc) { }
            this.NoShipmentsQuantityByTime = true;

        }     
     
    }

 



    FillFilters() {
        this.FillDateTypeFilterList();
        this.FillTimeRangeFilterList();
        this.FillShowFilterList();
        this.LoadQuires();
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
        this.CommonFiltersCountries();
    }


    public DateTypeFilterList: DashBoardFilters[];

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

        var list = LastFilter.MyCRMListDefault();
        this.TimeRangeFilterList.push(new DashBoardFilters(list[0].lastTitle, "0"));
        this.TimeRangeFilterList.push(new DashBoardFilters(list[1].lastTitle, "1"));
        this.TimeRangeFilterList.push(new DashBoardFilters(list[2].lastTitle, "2"));
        this.TimeRangeFilterList.push(new DashBoardFilters(list[3].lastTitle, "-1"));

        var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_TimeRange);
        if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "0";
        }

        this.selectedTimeRangeItem = this.TimeRangeFilterList.filter(d => d.Index == defaultFilterCode)[0];
        if (this.selectedTimeRangeItem == null)
            this.selectedTimeRangeItem = this.TimeRangeFilterList[0];


        if (this.selectedTimeRangeItem.Index == "-1") {
            var ActiviytFromDate = LastFilterClass.GetFilterValue(this.filterControlNameSpace, "ActivityFromDate");

            if (!AppTool.IsNullOrEmpty(ActiviytFromDate)) {
                var ActivityDate: Date = new Date();
                var ActivityFromDateString = ActiviytFromDate.split(':');
                ActivityDate.setFullYear(ActivityFromDateString[0], ActivityFromDateString[1] - 1, ActivityFromDateString[2]);
                this.activityFromDate = DateTool.GetDateParts(ActivityDate).DateObject;
            }

            var ActiviytToDate = LastFilterClass.GetFilterValue(this.filterControlNameSpace, "ActivityToDate");
            if (!AppTool.IsNullOrEmpty(ActiviytToDate)) {
                var ActivityDate: Date = new Date();
                var ActivityToDateString = ActiviytToDate.split(':');
                ActivityDate.setFullYear(ActivityToDateString[0], ActivityToDateString[1] - 1, ActivityToDateString[2]);
                this.activityToDate = DateTool.GetDateParts(ActivityDate).DateObject;
            }


        }
    }

    ngOnInit() {
        this.FillFilters();
    }



  
    @Output()
    public BackCompleted = new EventEmitter();

    BackButtonClicked() {
        this.BackCompleted.emit();

    }

    CountriesTopValueChanged(flag) {
        if (flag)
            this.TopCountries = this.TopCountries + 1;
        else
            this.TopCountries = this.TopCountries - 1;

        if (this.TopCountries < 0)
            this.TopCountries = 0;

        this.CommonFiltersCountries();

    }


    Change() {
        console.log("Fired2");
    }




}
