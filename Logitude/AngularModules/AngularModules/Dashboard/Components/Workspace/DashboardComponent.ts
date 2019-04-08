import {Component, OnInit, ElementRef, ComponentFactoryResolver, ComponentRef, OnDestroy, ViewEncapsulation} from '@angular/core'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {DashBoardFilters} from '../../../Infrastructure/DataContracts/Dashboard/DashboardFilters';
import {DashboardDomainService} from '../../Services/DashboardDomainService';
import {DirectionTransportFilter} from '../../../Infrastructure/DataContracts/Dashboard/DirectionTransportFilter';
import {DashBoardClass} from '../../../Infrastructure/DataContracts/Dashboard/DashBoardClass';
import {GroupByClass} from '../../../Infrastructure/DataContracts/Dashboard/GroupByClass';
import {FunctionsCRM} from '../../../Infrastructure/DataContracts/Dashboard/FunctionsCRM';
import {List} from '../../../Infrastructure/DataContracts/Dashboard/List';
import {DailySpotlightClass} from '../../../Infrastructure/DataContracts/Dashboard/DailySpotlightClass';
import {ListComponentArgs} from '../../../Infrastructure/Args';
import {FormatTool, AppTool, DateTool} from '../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {LastFilter} from '../../../Infrastructure/Utilities/LastFilter';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LastFilterClass} from '../../../Infrastructure/Utilities/LastFilterClass';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';

declare var makeAMLineChart, makeAmBarChart, makePieChart;

@Component({
    selector: 'DashBoard',
    moduleId: module.id,
    templateUrl: './DashBoardComponent.html',
    encapsulation: ViewEncapsulation.None,
})

export class DashboardComponent extends BaseComponent implements OnInit {
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private dashboarddomainservice: DashboardDomainService;
    public TenantPM: TenantPM;
    public DataContext: DashboardComponent = this;
    SelectedItemActivityShow: DashBoardFilters;
    SelectedItem: any;
    public NoShipmentsInActivityStatus: boolean = false;
    SelectedItemActivity: DashBoardFilters;
    public ActivityStatusDashboardId: string = "ActivityStatusDashboardId_";
    public MoneyInDashboardId: string = "MoneyInDashboardId_";
    public TopFiveDashboardId: string = "TopFiveDashboardId_";
    public TopFiveDashboardLegendId: string;
    public ProfitCurrencyCode: string = SessionLocator.TenantPM.ProfitCurrencyCode;
    public LocalCurrencyCode: string = SessionLocator.TenantPM.AccountingCurrencyCode;
    public isNotMoreDetails: boolean = true;
    public BarData: any;
    public barChartLabels: string[] = [];
    public barChartData: any[] = [{ data: [], label: '' }, { data: [], label: ''  }];
    public SelectedCurrency: string="1";
    public LineData: any=[];
    public lineChartLabels: string[] = [];
    public lineChartData: any[] = [{ data: [], label: '' }];
    public AmLineChartData: any = [];
    public MoneyInLabel: string = "";
    public dailySpotLightClass: DailySpotlightClass; 
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public componentfactoryResolver: ComponentFactoryResolver) {
        super();
        this.TenantPM = InfraSettings.TenantPM;
        this.dailySpotLightClass = new DailySpotlightClass();
        this.dashboarddomainservice = new DashboardDomainService();
        this.ActivityStatusDashboardId = this.ActivityStatusDashboardId + this.CurrentSession.GetChartId();
        this.MoneyInDashboardId = this.ActivityStatusDashboardId + this.CurrentSession.GetChartId();
        this.TopFiveDashboardId = this.TopFiveDashboardId + this.CurrentSession.GetChartId();
        this.TopFiveDashboardLegendId = "TopFiveDashboardLegendId_" + this.CurrentSession.GetNewId("TopFiveDashboardLegendId");
    }

    ngOnInit() {
        this.FillScreen();
    }

    ngOnDestroy() {
        if (this.ActivityStatusPage != null) {
            this.ActivityStatusPage.destroy();
        }
    }

    private FillScreen() {   
        this.FillFilters();        
        this.LoadSpotlightQueries();
        this.LoadPieQueries();
    }


    
    public Shipments_Today_Status: boolean = true;
    public Shipments_Yesterday_Status: boolean = true;
    public Shipments_LastWeek_Status: boolean = true;

    public ARInvoices_Today_Status: boolean = true;
    public ARInvoices_Yesterday_Status: boolean = true;
    public ARInvoices_LastWeek_Status: boolean = true;

    public Quotes_Today_Status: boolean = true;
    public Quotes_Yesterday_Status: boolean = true;
    public Quotes_LastWeek_Status: boolean = true;

    public Customers_Today_Status: boolean = true;
    public Customers_Yesterday_Status: boolean = true;
    public Customers_LastWeek_Status: boolean = true;
    private filterName_DateType: string = "DateType";
    private filterName_TimeRange: string = "TimeRange";
    private filterName_TimeRangeMoney: string = "TimeRangeMoney";
    private filterControlNameSpace: string = "Workspace.Dashboard";

    LoadSpotlightQueries() {
        this.dashboarddomainservice.GetDashboardSpotlightCounts(this.TenantPM.Id).subscribe(myResult => {
            this.dailySpotLightClass = myResult;
            if (this.dailySpotLightClass.Shipments_Today != 0)
                this.Shipments_Today_Status = false;

            if (this.dailySpotLightClass.ARInvoices_Today != 0)
                this.ARInvoices_Today_Status = false;

            if (this.dailySpotLightClass.Quotes_Today != 0)
                this.Quotes_Today_Status = false;

            if (this.dailySpotLightClass.Customers_Today != 0)
                this.Customers_Today_Status = false;

            if (this.dailySpotLightClass.Shipments_Yesterday != 0)
                this.Shipments_Yesterday_Status = false;

            if (this.dailySpotLightClass.ARInvoices_Yesterday != 0)
                this.ARInvoices_Yesterday_Status = false;

            if (this.dailySpotLightClass.Quotes_Yesterday != 0)
                this.Quotes_Yesterday_Status = false;

            if (this.dailySpotLightClass.Customers_Yesterday != 0)
                this.Customers_Yesterday_Status = false;

            if (this.dailySpotLightClass.Shipments_LastWeek != 0)
                this.Shipments_LastWeek_Status = false;

            if (this.dailySpotLightClass.ARInvoices_LastWeek != 0)
                this.ARInvoices_LastWeek_Status = false;

            if (this.dailySpotLightClass.Quotes_LastWeek != 0)
                this.Quotes_LastWeek_Status = false;

            if (this.dailySpotLightClass.Customers_LastWeek != 0)
                this.Customers_LastWeek_Status = false;
        });
    }

    public PieData: any;
    public pieChartLabels: string[] = [];
    public pieChartData: number[] = [];
    private CurrentTop10DebtorsChart: any;
    LoadPieQueries() {
        this.dashboarddomainservice.GetDebrotExposure(this.TenantPM.Id, parseInt(this.SelectedCurrency)).subscribe(myResult => {
            this.PieData = myResult;
            this.FillPie();
        });
    }
    FillPie() {
        var fullData = [];
        this.pieChartLabels = [];
        this.pieChartData = [];

        this.PieData.forEach(element => {
            var amount = AppTool.Round(element.Amount, 3);
            fullData.push({ label: element.DebtorName, data: amount })
            this.pieChartLabels.push(element.DebtorName);
            this.pieChartData.push(element.Amount);
        });

        if (this.CurrentTop10DebtorsChart != null) {
            this.CurrentTop10DebtorsChart.clear();
            this.CurrentTop10DebtorsChart = null;
        }

        this.CurrentTop10DebtorsChart = makePieChart(this.TopFiveDashboardId, fullData, false, true, this.TopFiveDashboardLegendId);
    }
    public TimeRangeFilterList: DashBoardFilters[];
    public TimeRangeFilterList2: DashBoardFilters[];

    
    private activityFromDate: Date;
    public get ActivityFromDate() { return this.activityFromDate; }
    public set ActivityFromDate(value: Date) {
        if (value != this.activityFromDate) {
            this.activityFromDate = value;
            this.SelectedTimeRangeItem = this.TimeRangeFilterList[4];
            LastFilterClass.UpdateFilter(this.filterControlNameSpace,"ActivityFromDate", (value == null ? null : ServiceHelper.GetDateString(value)));

          //  this.LoadLineQueries();

        }
    }

    private activityToDate: Date;
    public get ActivityToDate() { return this.activityToDate; }
    public set ActivityToDate(value: Date) {
        if (value != this.activityToDate) {
            this.activityToDate = value;
            this.SelectedTimeRangeItem = this.TimeRangeFilterList[4];
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityToDate", (value == null ? null : ServiceHelper.GetDateString(value)));

           // this.LoadLineQueries();
        }
    }



    private moneyFromDate: Date;
    public get MoneyFromDate() { return this.moneyFromDate; }
    public set MoneyFromDate(value: Date) {
        if (value != this.moneyFromDate) {
            this.moneyFromDate = value;
            this.SelectedTimeRangeItem2 = this.TimeRangeFilterList2[4];
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityFromDateMoney", (value == null ? null : ServiceHelper.GetDateString(value)));

        }
    }

    private moneyToDate: Date;
    public get MoneyToDate() { return this.moneyToDate; }
    public set MoneyToDate(value: Date) {
        if (value != this.moneyToDate) {
            this.moneyToDate = value;
            this.SelectedTimeRangeItem2 = this.TimeRangeFilterList2[4];
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityToDateMoney", (value == null ? null : ServiceHelper.GetDateString(value)));

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

          //  this.LoadLineQueries();
        }
        this.LoadLineQueries();
    }


    private selectedTimeRangeItem2: DashBoardFilters;
    get SelectedTimeRangeItem2() { return this.selectedTimeRangeItem2; }
    set SelectedTimeRangeItem2(value: DashBoardFilters) {
        if (this.selectedTimeRangeItem2 != value) {
            this.selectedTimeRangeItem2 = value;
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_TimeRangeMoney, (value == null ? null : value.Index));
            if (value.Index == "-1") {
                LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityFromDateMoney", (value == null ? null : ServiceHelper.GetDateString(this.MoneyFromDate)));
                LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityToDateMoney", (value == null ? null : ServiceHelper.GetDateString(this.MoneyToDate)));
            }
        }
        this.LoadBarQueries();

    }

    private FilterSelectedShow() {
        this.FillLineQueries();
    }


    private selectedShowItem: DashBoardFilters;
    get SelectedShowItem() { return this.selectedShowItem; }
    set SelectedShowItem(value: DashBoardFilters) {
        if (this.selectedShowItem != value) {
            this.selectedShowItem = value;

            this.FilterSelectedShow();
        }
    }

    LoadBarQueries() {
        if (this.SelectedTimeRangeItem2.Index == "-1") {
            if (this.MoneyFromDate != null && this.MoneyToDate != null) {
                this.dashboarddomainservice.GetMoneyStatusForTenantCustom("CreateDate",  this.MoneyToDate, this.MoneyFromDate).subscribe(myResult => {
                    this.BarData = myResult;
                    if (this.BarData.length != 0) {
                        var list: GroupByClass[] = [];
                        this.BarData.forEach(item => {
                            var newItem: GroupByClass = new GroupByClass();
                            newItem.XField = item.DateRange;
                            newItem.DataType = item.DataType;
                            newItem.YField = item.TotalAmount;
                            list.push(newItem);                            
                        });
                        this.BarData = list;
                        this.FillBars();
                    }
                });

            }
        }
        else {
            var days = this.ComputeDays("money");
                this.dashboarddomainservice.GetMoneyStatusForTenant("CreateDate", 0, days, this.TenantPM.Id, +this.SelectedTimeRangeItem2.Index, 1).subscribe(myResult => {
                    this.BarData = myResult;
                    var groupedData: GroupByClass[] = [];
                    var barData2 = [];
                    this.BarData.sort((a, b) => { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1 });
                    this.BarData.forEach(item => {
                        var existsedItem = groupedData.filter(f => f.DateRange == item.DateRange && f.DataType == item.DataType)[0];
                        if (existsedItem == null) {
                            existsedItem = new GroupByClass();
                            existsedItem.XField = item.DateRange;
                            existsedItem.DataType = item.DataType;
                            if (item.TotalAmount == null)
                                existsedItem.YField = 0;
                            else
                                existsedItem.YField = parseInt(item.TotalAmount + "");

                            existsedItem.dateRange = item.DateRange;

                            groupedData.push(existsedItem);
                        }

                        else {

                            if (item.TotalAmount == null)
                                existsedItem.YField += 0;
                            else
                                existsedItem.YField += parseInt(item.TotalAmount + "");

                        }
                    });
                    this.BarData = groupedData;
                    this.FillBars();
                });

            }
        
    }

    filterAgrs: ApiQueryFilters;
    DailySpotLightClick(Code) {

      var  myQueryCode: string = "";
       var displayName: string = "";
       var myTableName: string = "";
       this.filterAgrs = new ApiQueryFilters();

        switch (Code) {
            case "QT_TD":
                {
                    myTableName = "Quote";
                    myQueryCode = "All Quotes";
                    displayName = "Today Quotes";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Quotes Zoom");
                    this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, null, null, "Equals", true, false, true,"string");
                    break;
                }

            case "QT_YS":
                {
                    myTableName = "Quote";
                    myQueryCode = "All Quotes";
                    displayName = "Yesterday Quotes";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Quotes Zoom");
                    this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, null, null, "Equals", true, false, true, "string");
                    break;
                }

            case "QT_LW":
                {
                    myTableName = "Quote";
                    myQueryCode = "All Quotes";
                    displayName = "Last Week Quotes";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Quotes Zoom");
                    this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, null, null, "Equals", true, false, true, "string");
                    break;
                }

            case "SH_TD":
                {
                    myTableName = "Shipment";
                    myQueryCode = "Shipments";
                    displayName = "Today Shipments";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Shipments Zoom");
                    this.filterAgrs.addAdditionalFilter("IsCancelled", false, "", "", "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("ShipmentLevelCode", "C", "", "", "NotEqual", false, false, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false,"string");
                    break;
                }
            case "SH_YS":
                {
                    myTableName = "Shipment";
                    myQueryCode = "Shipments";
                    displayName = "Yesterday Shipments";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Shipments Zoom");
                    this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("ShipmentLevelCode", "C", "", "", "NotEqual", false, false, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "string");

                    break;
                }
            case "SH_LW":
                {
                    myTableName = "Shipment";
                    myQueryCode = "Shipments";
                    displayName = "Last Week Shipments";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Shipments Zoom");
                    this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("ShipmentLevelCode", "C", "", "", "NotEqual", false, false, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "string");
                    break;
                }

            case "AR_TD":
                {
                    myTableName = "ARInvoice";
                    myQueryCode = "All Invoices";
                    displayName = "Today Invoices";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Invoices Zoom");
                    this.filterAgrs.addAdditionalFilter("StatusCode", "LL", "VD", null, "NotEqual", false, false, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "string");
                    
                    break;
                }
            case "AR_YS":
                {
                    myTableName = "ARInvoice";
                    myQueryCode = "All Invoices";
                    displayName = "Yesterday Invoices";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Invoices Zoom");
                    this.filterAgrs.addAdditionalFilter("StatusCode", "LL", "VD", "", "NotEqual", false, false, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "string");

                    break;
                }
            case "AR_LW":
                {
                    myTableName = "ARInvoice";
                    myQueryCode = "All Invoices";
                    displayName = "Last Week Invoices";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Invoices Zoom");
                    this.filterAgrs.addAdditionalFilter("StatusCode", "LL", "VD", "", "NotEqual", false, false, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "string");
                    break;
                }

            case "CS_TD":
                {
                    myTableName = "Customer";
                    myQueryCode = "Customers";
                    displayName = "Today Customers";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Customers Zoom");
                    this.filterAgrs.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("CustomerStatusCode", "ACT", null, null, "Equals", false, true, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "string");
                    
                    break;
                }

            case "CS_YS":
                {
                    myTableName = "Customer";
                    myQueryCode = "Customers";
                    displayName = "Yesterday Customers";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Customers Zoom");

                    this.filterAgrs.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("CustomerStatusCode", "ACT", null, null, "Equals", false, true, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "string");
                        break;
                }

            case "CS_LW":
                {
                    myTableName = "Customer";
                    myQueryCode = "Customers";
                    displayName = "Last Week Customers";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Customers Zoom");

                    this.filterAgrs.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("CustomerStatusCode", "ACT", null, null, "Equals", false, true, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "string");
                       break;
                }
        }
        
        var listArgs = new ListComponentArgs();
        listArgs.Filters = this.filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = displayName;
        listArgs.BackButtonTitle = "Dashboard";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadData());
                });
        });
    }
    LoadData() {
        this.LoadPieQueries();
        this.LoadSpotlightQueries();
        this.LoadLineQueries();
        this.LoadBarQueries();

    }
    
    private flagEmpty: boolean = false;
    LoadLineQueries() {     
            if (this.SelectedTimeRangeItem.Index == "-1") {
                if (this.ActivityFromDate != null && this.ActivityToDate != null) {
                    this.dashboarddomainservice.GetActivityStatusByType(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate, this.TenantPM.Id + "",null,null).subscribe(myResult => {
                        this.LineData = myResult;
                        this.FillLineQueries();
                    });
                }
                else {
                    this.FillLineQueries();
                }
        }
        else {
                var days = this.ComputeDays("activity");
            this.dashboarddomainservice.GetActivityStatus(this.SelectedDateTypeItem.Index, 0, days, this.TenantPM.Id).subscribe(myResult => {
                this.LineData = myResult;
                this.FillLineQueries();
            });
        } 
    }    

    private ComputeDays(FilterAction:string) {
        var days;
        var Todate: Date = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject;
        if (FilterAction == "activity") {
            this.activityFromDate = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject;
            if (this.SelectedTimeRangeItem.Index == "0") {
                days = -7;
                Todate.setDate(Todate.getDate() - 6);
                this.activityToDate = Todate;
            }

            else if (this.SelectedTimeRangeItem.Index == "1") {
                days = -30;
                Todate.setMonth(Todate.getMonth()-1);
                this.activityToDate = Todate;
            }

            else if (this.SelectedTimeRangeItem.Index == "2") {
                days = -90;
                Todate.setMonth(Todate.getMonth()-3);
                this.activityToDate = Todate;
            }

            else if (this.SelectedTimeRangeItem.Index == "3") {
                days = -365;
                Todate.setMonth(Todate.getMonth() - 12);
                this.activityToDate = Todate;
            }

           

           
        }       
        else {
            this.moneyFromDate = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject;
            if (this.SelectedTimeRangeItem2.Index == "0") {
                days = -7;
                Todate.setDate(Todate.getDate() - 6);
                this.moneyToDate = Todate;
            }

            else if (this.SelectedTimeRangeItem2.Index == "1") {
                days = -30;
                Todate.setMonth(Todate.getMonth() - 1);
                this.moneyToDate = Todate;
            }

            else if (this.SelectedTimeRangeItem2.Index == "2") {
                days = -90;
                Todate.setMonth(Todate.getMonth() - 3);
                this.moneyToDate = Todate;
            }

            else if (this.SelectedTimeRangeItem2.Index == "3") {
                days = -365;
                Todate.setMonth(Todate.getMonth() - 12);
                this.moneyToDate = Todate;
            }

            
        }

        return days;
    }

   

    
    FillLineQueries() {
        var showIndex: number = parseInt(this.SelectedShowItem.Index);
        var timeIndex: number = parseInt(this.SelectedTimeRangeItem.Index);

        var byMonthData: List<DashBoardClass> = this.LineData;
        var dtf: DirectionTransportFilter = this.GetCurrentDirectionTransmodeFilterItem();
        var directionFilteredList: List<DashBoardClass> = FunctionsCRM.getDirectionFilteredList(dtf, byMonthData);
        if (timeIndex == -1) {
            var diff = DateTool.GetDaysBetweenDates(this.ActivityFromDate, this.ActivityToDate)-1;
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
            this.FillLine(monthQuartersList);
        }
        else {
            var FilteredList: List<GroupByClass> = new List<GroupByClass>();
            this.LineData.items != null ? this.LineData.items.forEach((item: DashBoardClass) => {
                var obj: GroupByClass = new GroupByClass();
                obj.XField = item.DateRange;
                switch (parseInt(this.SelectedShowItem.Index)) {
                    case 0:
                        {
                            obj.YField = item.Total != null ? item.Total:0;
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
              

            this.FillLine(FilteredList);
        }
        this.flagEmpty = true;
        this.lineChartData[0].data.forEach(p => {
            if (p != "0")
                this.flagEmpty = false;            
        });
        if (!this.flagEmpty) {
            makeAMLineChart(this.ActivityStatusDashboardId, this.AmLineChartData);
            this.NoShipmentsInActivityStatus = false;

        }
        else {
            try {
                var elm = document.getElementById(this.ActivityStatusDashboardId);
                elm.innerHTML = "";
            }
            catch (exce) { }
            this.NoShipmentsInActivityStatus = true;
        }

    }
    
     GetCurrentDirectionTransmodeFilterItem()
        {
         var transmodeId: string = "";
         var directionId: string = "";
         var x = 10;
         switch (x) {
             case 0:
                 {
                     directionId = "";
                     break;
                 }

             case 1:
                 {
                     directionId = "E";
                     break;
                 }

             case 2:
                 {
                     directionId = "I";
                     break;
                 }

             case 3:
                 {
                     directionId = "R";
                     break;
                 }

             case 4:
                 {
                     directionId = "D";
                     break;
                 }
             case 5:
                 {
                     directionId = "C";
                     break;
                 }
         }

         switch (x) {
             case 0:
                 {
                     transmodeId = "";
                     break;
                 }
             case 1:
                 {
                     transmodeId = "A";
                     break;
                 }
             case 2:
                 {
                     transmodeId = "O";
                     break;
                 }

             case 3:
                 {
                     transmodeId = "I";
                     break;
                 }
         }

         var filterItem: DirectionTransportFilter = new DirectionTransportFilter("", directionId, transmodeId);
         return filterItem;
     

        }  
     
     FillLine(data: List<GroupByClass>) {
         this.AmLineChartData = [];
         var index = 0;
         this.lineChartData = [{

             scales: {
                 xAxes: [{
                     gridThickness: 0,
                 }]
             },

             xAxes: {
                 gridThickness: 0,
             },
             offsetGridLines: false
             ,
             scaleShowVerticalLines: false,

             data: [], label: 'Total', tension: 0, scaleShowHorizontalLines: false, scaleStepWidth: 0
         }];
         this.lineChartLabels = [];
         data.getAll().forEach(element => {
             var myResult;
             var myStringNumber = AppTool.Round(element.YField, 2) + "";
             var myStringNumber1 = myStringNumber.split('.')[0];
             var myStringNumber2 = myStringNumber.split('.')[1];

             myResult = myStringNumber1.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
             if (!AppTool.IsNullOrEmpty(myStringNumber2)) {
                 myResult += "." + myStringNumber2;
             }

             this.lineChartData[0].data[index] = AppTool.Round(element.YField, 2) + "";
             this.lineChartLabels.push(element.XField);
             index++;

             this.AmLineChartData.push({
                 date: element.XField,
                 visits: AppTool.Round(element.YField, 2) + ""
             });
         });
     }

     FillBars() {
         var i = 0;
         var index = 0;
         var Graphs = [];
         var DataProvider = [];
         var objectArray = [];
         var maximum = 0;
         this.barChartData[0].data = [];
         this.barChartData[1].data = []
         this.barChartLabels = [];

         var max = 0;
         var even = 0;
         this.BarData.forEach(element => {
             if (i == 0) {
                 Graphs = [{
                     "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                     "fillAlphas": 1,
                     "id": "AmGraph-1" + i,

                     "title": "Invoices",
                     "type": "column",
                     "valueField": "col1",
                     "fillColors": ["#d29127", "#dfb267"],
                     "lineAlpha": 0,

                     "gradientOrientation": "horizontal",
                     "borderAlpha": 0,
                 },
                     {
                         "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                         "fillAlphas": 1,
                         "id": "AmGraph-2" + i,
                         "title": "Payments",
                         "type": "column",
                         "lineAlpha": 0,

                         "valueField": "col2",
                         "fillColors": ["#1d758e", "#2186a3"],
                         "gradientOrientation": "horizontal",
                         "borderAlpha": 0,
                     }
                 ];
             }

             if (element.DataType == 'Invoices') {
                 this.barChartData[0].label = element.DataType;
                 this.barChartData[0].data[i] = element.YField;
             }
             else if (element.DataType == 'Payments') {
                 this.barChartData[1].label = element.DataType;
                 this.barChartData[1].data[i] = element.YField;
             }
             if (!this.barChartLabels.includes(element.XField)) {
                 this.barChartLabels[i] = element.XField;

             }
             even++;
             if (even % 2 == 0)
                 i++;

             if (element.YField > max)
                 max = element.YField;

         });

         var i = 0;
         this.barChartLabels.forEach(item => {
             DataProvider[i] = { "category": this.barChartLabels[i], "col1": this.barChartData[0].data[i], "col2": this.barChartData[1].data[i] };
             i++;
         });

         makeAmBarChart(this.MoneyInDashboardId, Graphs, DataProvider, max);
     } 
   
    change(cmpRef: ComponentRef<any>) {
        cmpRef.destroy();
        this.isNotMoreDetails = true;
        this.FillScreen();
    }

    public ActivityStatusPage: any;

    OpenDashBoard() {
        this.isNotMoreDetails = false;
        SessionLocator.DynamicLoader.Load("./Dashboard/Components/Workspace/ActivityStatusDetailsComponent", this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.logoff.subscribe(($event) => this.change(cmpRef))
                        this.ActivityStatusPage = cmpRef;
                    });
    }
        
    FillFilters() {
        this.FillTimeRangeFilterList();
        this.FillShowFilterList();
        this.FillOtherElements();
        this.FillDateTypeFilterList();
        this.LoadData();
    }

    private FillOtherElements() {
        this.MoneyInLabel = SessionLocator.TenantPM.AccountingCurrencyCode;

    }
    public DateTypeFilterList: DashBoardFilters[];

    private FillDateTypeFilterList() {
        this.DateTypeFilterList = [];

        this.DateTypeFilterList.push(new DashBoardFilters("Create Date", "CreateDate"));
        this.DateTypeFilterList.push(new DashBoardFilters("Operational Date", "OperationalDate"));

        var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_DateType);
        if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "CreateDate";
        }
        this.selectedDateTypeItem = this.DateTypeFilterList.filter(d => d.Index == defaultFilterCode)[0];  

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





        if (this.selectedTimeRangeItem2.Index == "-1") {
            var moneyFromDate = LastFilterClass.GetFilterValue(this.filterControlNameSpace, "ActivityFromDateMoney");

            if (!AppTool.IsNullOrEmpty(moneyFromDate)) {
                var moneyDate: Date = new Date();
                var moneyFromDateString =moneyFromDate.split(':');
                moneyDate.setFullYear(moneyFromDateString[0], moneyFromDateString[1] - 1, moneyFromDateString[2]);
                this.moneyFromDate = DateTool.GetDateParts(moneyDate).DateObject;
            }

            var moneyToDate = LastFilterClass.GetFilterValue(this.filterControlNameSpace, "ActivityToDateMoney");
            if (!AppTool.IsNullOrEmpty(moneyToDate)) {
                var moneyDate: Date = new Date();
                var moneyToDateString = moneyToDate.split(':');
                moneyDate.setFullYear(moneyToDateString[0], moneyToDateString[1] - 1, moneyToDateString[2]);
                this.moneyToDate = DateTool.GetDateParts(moneyDate).DateObject;
            }


        }



    }

    private selectedDateTypeItem: DashBoardFilters;
    get SelectedDateTypeItem() { return this.selectedDateTypeItem; }
    set SelectedDateTypeItem(value: DashBoardFilters) {
        if (this.selectedDateTypeItem != value) {
            this.selectedDateTypeItem = value;
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_DateType, (value == null ? null : value.Index));
           

            this.LoadLineQueries();
        }
    }

  

    private FillTimeRangeFilterList() {
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

        var list2 = LastFilter.ActivitymyList();
        this.TimeRangeFilterList2 = [];

        this.TimeRangeFilterList2.push(new DashBoardFilters(list2[0].lastTitle, "0"));
        this.TimeRangeFilterList2.push(new DashBoardFilters(list2[1].lastTitle, "1"));
        this.TimeRangeFilterList2.push(new DashBoardFilters(list2[2].lastTitle, "2"));
        this.TimeRangeFilterList2.push(new DashBoardFilters(list2[3].lastTitle, "3"));
        this.TimeRangeFilterList2.push(new DashBoardFilters(list2[4].lastTitle, "-1"));

        var defaultFilterCode: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_TimeRangeMoney);
        if (AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "0";
        }
        this.selectedTimeRangeItem2 = this.TimeRangeFilterList2.filter(d => d.Index == defaultFilterCode)[0];
    }



    public ShowFilterList: DashBoardFilters[];

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


   
  

    ChangeCurrency(code:string) {

        if (code == this.LocalCurrencyCode)
            code = "1";
        else
            code = "2";

        if (code != this.SelectedCurrency) {
            this.SelectedCurrency = code;
            this.LoadPieQueries();
        }        
    }
}
