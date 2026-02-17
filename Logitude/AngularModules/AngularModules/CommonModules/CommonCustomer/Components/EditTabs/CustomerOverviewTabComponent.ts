import {Component, ChangeDetectorRef, OnInit,AfterViewInit} from '@angular/core';
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
import {ChartsService} from '../../../../Infrastructure/Services/ChartsService';
import {QuoteDomainService} from '../../../../Quote/Services/QuoteDomainService';
import {RankList} from '../../../../Common/EntityLists/RankList';
import {RankListService} from '../../../../Common/Services/StandardLists/RankListService';
import {FormatTool, DateTool} from '../../../../Infrastructure/Tools';
import {LastFilterClass} from '../../../../Infrastructure/Utilities/LastFilterClass';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {AppTool} from '../../../../Infrastructure/Tools';
declare var UploadLogoFile, HideImage, SetImage, ArrayBufferToBase64, makeAMLineChart,window: any;  

@Component({
    moduleId: module.id,
    templateUrl: './CustomerOverviewTabComponent.html',
    providers: [ImageLibraryService]
})

export class CustomerOverviewTabComponent extends BaseComponent implements OnInit {
    public EntityPM: CustomerPM;
    public ObjectTableName: string = "Customer";
    private CD: ChangeDetectorRef;
    public filterAgrs: ApiQueryFilters;
    public LineData: any = [];
    public AmLineChartTest: any = [];
    public RankValue: Array<RankList>=[];
    public NoData: boolean = true; 
    public ActivityStatusOverViewDashboardId: string = "ActivityStatusOverViewDashboardId_";
    public rankListService: RankListService;
    public CurrencyCodeLocal: string = SessionLocator.TenantPM.AccountingCurrencyCode;
    public lineChartLabels: string[] = [];
    public lineChartData: any[] = [{ data: [], label: '' }];
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public DataContext: CustomerOverviewTabComponent = this;
    public TenantPM: TenantPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, public _imageLibraryService: ImageLibraryService) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ImageId = this.EntityPM.ImageDetailId;
        this.EntityId = this.EntityPM.Id;
        this.EntityName = "Customer";
        this.StartWorkingDate = this.EntityPM.StartWorkingDate;
        this.TenantPM = SessionLocator.TenantPM;
        this.ActivityStatusOverViewDashboardId = this.ActivityStatusOverViewDashboardId + this.CurrentSession.GetChartId();

        this.rankListService = new RankListService();
        this.rankListService.getAllFromCache().subscribe(result => {
            this.RankListArr = result.Result;
        });
    }

    ngOnInit() {
        this.LoadQuriesCount();
        this.LoadMoneyQueries();        
        this.RankSource1();
        this.RankSource2();
        this.RankSource3();
        this.FillFilters();
    }

   

    public AllQuotes = 0;
    public AllShipments = 0;
    public OpenQuotes = 0;
    public OpenShipments = 0;
    LoadQuriesCount() {
        var service = new QuoteDomainService();
        service.GetDataCountsForCRM(this.TenantPM.Id, this.EntityPM.Id).subscribe((myResult: any) => {
            this.AllQuotes = myResult.Result.AllQuotes;
            this.AllShipments = myResult.Result.AllShipments;
            this.OpenQuotes = myResult.Result.OpenQuotes;
            this.OpenShipments = myResult.Result.OpenShipments;
        });
    }

    public ARPayments = 0.0;
    public InvoicesDue = 0.0;
    public OpenARInvoices = 0.0;
    public OpenReceivables = 0.0;
    LoadMoneyQueries() {
        var service = new QuoteDomainService();
        service.GetCRMMoneyInformation(this.TenantPM.Id, this.EntityPM.Id).subscribe((myResult: any) => {
            this.ARPayments = myResult.Result.ARPayments;
            this.InvoicesDue = myResult.Result.InvoicesDue;
            this.OpenARInvoices = myResult.Result.OpenARInvoices;
            this.OpenReceivables = myResult.Result.OpenReceivables;
        });
    }

    public RankSourceText1: string = "./Images/Icons/StarGray.png";
    public RankSourceText2: string = "./Images/Icons/StarGray.png";
    public RankSourceText3: string = "./Images/Icons/StarGray.png";
    RankSource1(rank = null) {
        if (rank != null) {
            this.RankSourceText1 = "./Images/Icons/StarOrange.png";
        }
        else {
            var myResult: string = null;
            
            if (this.EntityPM != null) {
                var RankCode = this.EntityPM.RankCode;

                switch (RankCode) {
                    case "1": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarGray.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                        break;
                    }

                    case "2": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                        break;
                    }

                    case "3": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText3 = "./Images/Icons/StarOrange.png";
                        break;
                    }

                    default: {
                        this.RankSourceText1 = "./Images/Icons/StarGray.png";
                        this.RankSourceText2 = "./Images/Icons/StarGray.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                    }
                }
            }
        }
    }
    RankSource2(rank = null) {
        if (rank != null)
            this.RankSourceText2 = "./Images/Icons/StarOrange.png";
        else {
            var myResult: string = "./Images/Icons/StarOrange.png";

            if (this.EntityPM != null) {
                var RankCode = this.EntityPM.RankCode;

                switch (RankCode) {
                    case "1": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarGray.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                        break;
                    }

                    case "2": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                        break;
                    }

                    case "3": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText3 = "./Images/Icons/StarOrange.png";
                        break;
                    }

                    default: {
                        this.RankSourceText1 = "./Images/Icons/StarGray.png";
                        this.RankSourceText2 = "./Images/Icons/StarGray.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                    }
                }
            }
        }
    }
    RankSource3(rank = null) {
        if (rank != null) {
            this.RankSourceText3 = "./Images/Icons/StarOrange.png";
            this.RankSourceText2 = "./Images/Icons/StarOrange.png";
        }
        else {
            var RankCode = this.EntityPM.RankCode;

            switch (RankCode) {
                case "1": {
                    this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText2 = "./Images/Icons/StarGray.png";
                    this.RankSourceText3 = "./Images/Icons/StarGray.png";
                    break;
                }

                case "2": {
                    this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText3 = "./Images/Icons/StarGray.png";
                    break;
                }

                case "3": {
                    this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText3 = "./Images/Icons/StarOrange.png";
                    break;
                }

                default: {
                    this.RankSourceText1 = "./Images/Icons/StarGray.png";
                    this.RankSourceText2 = "./Images/Icons/StarGray.png";
                    this.RankSourceText3 = "./Images/Icons/StarGray.png";
                }
            }
        }
    }

    private filterName_DateType: string = "DateType";
    private filterName_TimeRange: string = "TimeRange";
    private filterControlNameSpace: string = "Components.Partners.EditTabs.Customer.CustomerOverviewTabComponent";
    FillFilters() {      
        this.FillDateTypeFilterList();
        this.FillTimeRangeFilterList(); 
        this.FillShowFilterList();  

        this.LoadLineQueries();
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

    }

    public TimeRangeFilterList: DashBoardFilters[];
    private FillTimeRangeFilterList() {
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

    private selectedDateTypeItem: DashBoardFilters;
    get SelectedDateTypeItem() { return this.selectedDateTypeItem; }
    set SelectedDateTypeItem(value: DashBoardFilters) {
        if (this.selectedDateTypeItem != value) {
            this.selectedDateTypeItem = value;

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_DateType, (value == null ? null : value.Index));

            this.LoadLineQueries();
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
            this.LoadLineQueries();
        }
    }

    private selectedShowItem: DashBoardFilters;
    get SelectedShowItem() { return this.selectedShowItem; }
    set SelectedShowItem(value: DashBoardFilters) {
        if (this.selectedShowItem != value) {
            this.selectedShowItem = value;

            this.FilterSelectedShow();
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

        //else if (this.SelectedTimeRangeItem.Index == "2") {
        //    days = -90;
        //    Todate.setMonth(-3);
        //    this.activityToDate = Todate;
        //}

        else if (this.SelectedTimeRangeItem.Index == "2") {
            days = -1095;
            Todate.setMonth(Todate.getMonth() - 36);
            this.activityToDate = Todate;
        }

      else  if (this.SelectedTimeRangeItem.Index == "-1") {
            this.activityFromDate = null;
            this.activityToDate = null;
        }


     
        return days;
    }
    
    private FilterSelectedShow() {        
        this.FillLineQueries();
    }

    private activityFromDate: Date;
    public get ActivityFromDate() { return this.activityFromDate; }
    public set ActivityFromDate(value: Date) {
        if (value != this.activityFromDate) {
            this.activityFromDate = value;
            this.SelectedTimeRangeItem = this.TimeRangeFilterList[3];
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityFromDate", (value == null ? null : ServiceHelper.GetDateString(value)));

            this.LoadLineQueries();
        }
    }

    private activityToDate: Date;
    public get ActivityToDate() { return this.activityToDate; }
    public set ActivityToDate(value: Date) {
        if (value != this.activityToDate) {
            this.activityToDate = value;
            this.SelectedTimeRangeItem = this.TimeRangeFilterList[3];
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityToDate", (value == null ? null : ServiceHelper.GetDateString(value)));

            this.LoadLineQueries();
        }
    }
    
    LoadLineQueries() {
        var service = new ChartsService();
        if (this.SelectedTimeRangeItem.Index == "-1") {
            if (this.ActivityFromDate != null && this.ActivityToDate != null) {
                service.GetActivityStatusByType(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate, this.TenantPM.Id + "", this.EntityPM.Id).subscribe(myResult => {
                    this.LineData = myResult;
                    this.FillLineQueries();                  
                });
            }            
        }

        else {
            var days = this.ComputeDays();
            service.GetActivityStatus(this.SelectedDateTypeItem.Index, 0, days, this.TenantPM.Id, this.EntityPM.Id).subscribe(myResult => {
                this.LineData = myResult;
                this.FillLineQueries();
            });
        }
    }

    FillLineQueries() {
        var showIndex: number = parseInt(this.SelectedShowItem.Index);
        var timeIndex: number = parseInt(this.SelectedTimeRangeItem.Index);
        
        var byMonthData: List<DashBoardClass> = this.LineData;
        var dtf: DirectionTransportFilter = this.GetCurrentDirectionTransmodeFilterItem();
        var directionFilteredList: List<DashBoardClass> = FunctionsCRM.getDirectionFilteredList(dtf, byMonthData);

        if (timeIndex == -1) {                     

             if (DateTool.GetDaysBetweenDates(this.ActivityFromDate, this.ActivityToDate) <= 90) {
                timeIndex = 1;
            }

            else if (DateTool.GetDaysBetweenDates(this.ActivityFromDate, this.ActivityToDate) <= 365) {
                timeIndex = 2;
            }
            else {
                timeIndex = 3;
            }

        }
        if (this.SelectedTimeRangeItem.Index != "-1") {

            var measurmentFilteredList: List<GroupByClass> = FunctionsCRM.getMeasurmentFilterList(showIndex, directionFilteredList, timeIndex, this.EntityPM.Id);
            var monthQuartersList: List<GroupByClass> = FunctionsCRM.getmonthQuartersList(timeIndex, measurmentFilteredList, this.EntityPM.Id);
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

            this.FillLine(FilteredList);
        }

        this.NoData = true;
        this.lineChartData[0].data.forEach(p => {
            if (p != "0")
                this.NoData = false;


        });
        try {
            var els = document.getElementById(this.ActivityStatusOverViewDashboardId);
            if (!this.NoData) {
                makeAMLineChart(this.ActivityStatusOverViewDashboardId, this.AmLineChartTest, 0.6);
                els.hidden = false;
            }
            else {
                els.hidden = true;
            }

        }
        catch (Ex) { }      
    }

    FillLine(data: List<GroupByClass>) {
        var index = 0;
        this.AmLineChartTest = [];
        this.lineChartData = [{

            scales: {
                xAxes: [{
                    gridThickness: 0,
                }]
            },

            xAxes: {
                gridThickness: 0,
            },

            offsetGridLines: false,
            scaleShowVerticalLines: false,
            data: [], label: 'Total', tension: 0, scaleShowHorizontalLines: false, scaleStepWidth: 0
        }];

        this.lineChartLabels = [];
        data.getAll().forEach(element => {
            this.lineChartData[0].data[index] = element.YField + "";
            this.lineChartLabels.push(element.XField);
            index++;

            this.AmLineChartTest.push({
                date: element.XField,
                visits: element.YField + ""
            });
        });
    }
    
    GetCurrentDirectionTransmodeFilterItem() {
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

    public RankListArr: Array<RankList> = [];    
    public ImageId: string = "";
    public EntityId: string = "";
    public EntityName: string = ""; 
    ImageUploadedCompleted(imageId) {
        this.ImageId = imageId;
        this.EntityPM.ImageDetailId = imageId;        
    }
    
    public get StartWorkingDate() { return this.EntityPM.StartWorkingDate; }
    public set StartWorkingDate(value: Date) {
        if (this.EntityPM.StartWorkingDate !=value)
        this.EntityPM.StartWorkingDate = value;
    }
    
    LogoInput: string = Guid.NewRandomString();
    IsShowMessageComplate: boolean = false;
    IsShowProgressLoading: boolean = false;
    LogoFileHtmlId: string = Guid.NewRandomString();
    DataImage: any;    

    ChangeRank(code) {
        var filteredData = this.RankListArr.filter(a => a.Code === code && a.Tenant == SessionLocator.TenantPM.Id)[0];
        this.EntityPM.RankCode = filteredData.Code;
        this.EntityPM.RankName = filteredData.Name;
        this.EntityPM.RankId = filteredData.Id;

        this.RankSource1();
        this.RankSource2();
        this.RankSource3();
    }

    ComponentRef: any = null;
    public CustomerOverViewTabHide: boolean = false;

    MoreDetails() {
        this.CustomerOverViewTabHide = true;
        SessionLocator.DynamicLoader.Load('./CommonModules/CommonCustomer/Components/EditTabs/CustomerOverviewTabDetailsComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Customer = this.EntityPM;
                cmpRef.instance.BackCompleted.subscribe(($event: any) => this.DetailsTabEvent());
                this.ComponentRef = cmpRef;
            });
    }

    DetailsTabEvent() {
        if (this.ComponentRef != null)
            this.ComponentRef.destroy();

        this.CustomerOverViewTabHide = false;
        this.LoadData();       
    }

    LoadData() {
        this.LoadLineQueries();
        this.LoadMoneyQueries();
        this.LoadQuriesCount();
        this.RankSource1();
        this.RankSource2();
        this.RankSource3();
    }   

    public isWindowOpened: boolean = false;

    BusinessClickNew(code: string) {
        var path = "";
        var windowTitle = "";
        switch (code+"") {

            case "Quote": {
                path = './Quote/ComponentsNewEntity/NewQuoteComponent';
                windowTitle = "New Quote";

                break;
            }
            case "Shipment": {

                path = './Shipment/Components/NewShipment/NewShipmentComponent';
                windowTitle = "New Shipment";

                break;
            }

            default: break;
        }

        this._entityResourceService.getEntityResourceByTableName(code, 0).subscribe(response => {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.Title = windowTitle;
            logWindow.Show(path);

            logWindow.WindowClosed.subscribe(s => {
                this.isWindowOpened = false;

                if (s) {
                    this.LoadQuriesCount();
                }
            });
        });
    }
    BusinessClick(code: string) {
        var myQueryCode: string = "";
        var displayName: string = "";
        var myTableName: string = "";
        this.filterAgrs = new ApiQueryFilters();

        switch (code + "") {

            case "OpenQuotes": {
                myTableName = "Quote";
                myQueryCode = "Open Quotes";
                displayName = "Open Quotes";
                break;
            }

            case "AllQuotes": {
                myTableName = "Quote";
                myQueryCode = "All Quotes";
                displayName = "All Quotes";

                break;
            }


            case "OpenShipments": {
                myTableName = "Shipment";
                myQueryCode = "Shipments";
                displayName = "Open Shipments";

                break;
            }


            case "AllShipments": {
                myTableName = "Shipment";
                myQueryCode = "All Shipments";
                displayName="All Shipments"
                break;
            }

            default: break;
        }

        if (myTableName != "") {

            this.filterAgrs.addAdditionalFilter("CustomerId", this.EntityPM.Id, null, null, "Equals", false, false, false, "String");

            var listArgs = new ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = myTableName;
            listArgs.DisplayTitle = displayName;
            
            listArgs.BackButtonTitle = "Customer";
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, this.EntityPM.Tenant).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadQuriesCount());
                    });
            });
        }
    }
    
    ShowOpenReceivables() {
        var table = window.ObjectTables.filter(d => d.Name === 'Shipment')[0];
        if (table != null) {
            this.filterAgrs = new ApiQueryFilters();
            this.filterAgrs.addAdditionalFilter("CustomerId", this.EntityPM.Id, null, null, "Equals", false, false, false, "String");
            this.filterAgrs.addAdditionalFilter("OpenReceivablesInLocalCurrency", 0.0, null, null, "LargerThan", false, false, false, "number");

            var listArgs = new ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = "Shipments";
            listArgs.ObjectTableName = "Shipment";
            listArgs.DisplayTitle = listArgs.QueryCode;
            listArgs.BackButtonTitle = "Back";
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, SessionLocator.Tenant).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        //  cmpRef.instance.BackCompleted.subscribe(($event: any) => this.Load());
                    });
            });
        }
    }

    ShowARPayments() {
        var table = window.ObjectTables.filter(d => d.Name === 'ARPayment')[0];
        if (table != null) {
            this.filterAgrs = new ApiQueryFilters();
            this.filterAgrs.addAdditionalFilter("BillToId", this.EntityPM.Id, null, null, "Equals", false, false, false, "String");
            this.filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("StatusCode", "AD,PR", null, null, "InList", false, false, false, "string");


            var listArgs = new ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = "All Payments";
            listArgs.ObjectTableName = "ARPayment";
            listArgs.DisplayTitle = listArgs.QueryCode;
            listArgs.BackButtonTitle = "Back";
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, SessionLocator.Tenant).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        //  cmpRef.instance.BackCompleted.subscribe(($event: any) => this.Load());
                    });
            });
        }
    }

    ShowOpenARInvoices() {
        var table = window.ObjectTables.filter(d => d.Name === 'ARInvoice')[0];
        if (table != null) {
            this.filterAgrs = new ApiQueryFilters();
            this.filterAgrs.addAdditionalFilter("BillToId", this.EntityPM.Id, null, null, "Equals", false, false, false, "String");
            this.filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("IsAutoCredit", false, null, null, "Equals", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("StatusCode", "AD,PP,PR", null, null, "InList", false, false, false, "string");

            var listArgs = new ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = "All Invoices";
            listArgs.ObjectTableName = "ARInvoice";
            listArgs.DisplayTitle = listArgs.QueryCode;
            listArgs.BackButtonTitle = "Back";
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, SessionLocator.Tenant).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        //  cmpRef.instance.BackCompleted.subscribe(($event: any) => this.Load());
                    });
            });
        }
    }

    ShowInvoicesDue() {
        var table = window.ObjectTables.filter(d => d.Name === 'ARInvoice')[0];
        if (table != null) {
            this.filterAgrs = new ApiQueryFilters();
            this.filterAgrs.addAdditionalFilter("BillToId", this.EntityPM.Id, null, null, "Equals", false, false, false, "String");
            this.filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("DueDate", DateTool.GetCurrentDateTimeAsUtc(), null, null, "LessThan", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("IsAutoCredit", false, null, null, "Equals", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("StatusCode", "AD,PP,PR", null, null, "InList", false, false, false, "string");

            var listArgs = new ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = "All Invoices";
            listArgs.ObjectTableName = "ARInvoice";
            listArgs.DisplayTitle = listArgs.QueryCode;
            listArgs.BackButtonTitle = "Back";
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, SessionLocator.Tenant).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        //  cmpRef.instance.BackCompleted.subscribe(($event: any) => this.Load());
                    });
            });
        }
    }

    NewARPayment() {
        this._entityResourceService.getEntityResourceByTableName("ARPayment", 0).subscribe(response => {
            var str = TextCodeTranslator.Translate("General.O.NewEntity");
            str = str.replace("%Entity", TextCodeTranslator.TranslateTable("ARPayment"));       

            var logWindow = new LogitudeWindow();
            logWindow.WindowArgs = { CustomerId: this.EntityPM.Id};
            logWindow.Width = 900;
            logWindow.Height = 570;
            logWindow.Title = str;
            logWindow.Show("./InvoiceModules/ARPayment/Components/NewEntity/NewARPaymentComponent");
            
            logWindow.WindowClosed.subscribe(s => {
                this.isWindowOpened = false;

                if (s) {
                    this.LoadQuriesCount();
                }
            });
        });
    }     
}
