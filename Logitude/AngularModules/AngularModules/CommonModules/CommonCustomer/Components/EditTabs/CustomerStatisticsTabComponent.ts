import {Component, ChangeDetectorRef, OnInit, AfterViewInit} from '@angular/core';
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
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {ProductTypeListService} from '../../../../Common/Services/StandardLists/ProductTypeListService';
import {ProductTypeList} from '../../../../Common/EntityLists/ProductTypeList';
import {CustomerProductActualDataPM} from '../../../../Common/EntityPMs/CustomerProductActualDataPM';
import {CustomerProductPM} from '../../../../Common/EntityPMs/CustomerProductPM';
import {LastFilterClass} from '../../../../Infrastructure/Utilities/LastFilterClass';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';

declare var window, UploadLogoFile, HideImage, SetImage, ArrayBufferToBase64, makeAmBarChart, BarClick, ResetItem, makeAMLineChart: any;

@Component({
    moduleId: module.id,
    templateUrl: './CustomerStatisticsTabComponent.html',
})

export class CustomerStatisticsTabComponent extends BaseComponent {

    public EntityPM: CustomerPM;
    public ObjectTableName: string = "Customer";
    private CD: ChangeDetectorRef;
    public filterAgrs: ApiQueryFilters;
    FilterList: DashBoardFilters[] = [];
    SelectedCompareComboBoxItems: any;
    FilterListActivity: DashBoardFilters[] = [];
    public timeRangeSelectedIndex: number = 0;
    public LineData: any = [];
    public AmLineChartTest: any = [];
    public actualDataList: any=[];
    public flagEmpty: boolean = true;
    public ActivityStatusOverViewDashboardId: string = "ActivityStatusOverViewDashboardId_";
    public CurrencyCodeLocal: string = SessionLocator.TenantPM.AccountingCurrencyCode;
    public lineChartLabels: string[] = [];
    public lineChartData: any[] = [{ data: [], label: '' }];
    public CompareComboBoxItems: Array<ComboBoxIemClass> = [];
    public BarData: any;
    public barChartLabels: string[] = [];
    public ActualVsPotentialChart: string = "ActualVsPotential_ID_";
    public legenddiv: string = "ActualVsPotentialLegends_ID_";
    public barChartData: any[] = [{ data: [], label: '' }, { data: [], label: '' }];
    public NewActualVsPotential: Array<any> = [];
    private CurrentSession = SessionLocator.SelectedSession;
    CompareComboBoxItemsChange(item) {
        this.SelectedCompareComboBoxItems = item;
        this.LoadActuals();
    }



    NewARPayment() {        
       
        this._entityResourceService.getEntityResourceByTableName("ARPayment", 0).subscribe(response => {
            var str = TextCodeTranslator.Translate("General.O.NewEntity");
            str = str.replace("%Entity", TextCodeTranslator.TranslateTable("ARPayment"));

            var logWindow = new LogitudeWindow();
            logWindow.Width = 900;
            logWindow.Height = 570;
            logWindow.WindowArgs = { CustomerId: this.EntityPM.Id };
            logWindow.Title = str;
            logWindow.Show("./InvoiceModules/ARPayment/Components/NewEntity/NewARPaymentComponent");

            logWindow.WindowClosed.subscribe(s => {
                this.isWindowOpened = false;
                if (s) {
                    this.LoadMoneyQueries();
                }
            });
        });
    }


    LoadActuals() {
        var myMonth: number = DateTool.GetCurrentDateAsUtc().getMonth()+1;
        var myYear: number = DateTool.GetCurrentDateAsUtc().getFullYear();
        
        if (myMonth == 1) {
            myMonth = 12;
            myYear = myYear - 1;
        }
        else {
            myMonth = myMonth - 1;
        }        
        var partnersdomainService: PartnersDomainService = new PartnersDomainService();
        partnersdomainService.GetCustomerActualData(this.EntityId, myYear, myMonth).subscribe(result => {
            this.actualDataList = result;
            this.BuildCompareChartData();
        });
    }

    ActualVsPotentialChartClick() {
        if (BarClick() != null) {
            this.OnActualVsPotentialClick(BarClick());
            ResetItem();
        }
    }


    OnActualVsPotentialClick(e) {
        if (e.target.columnIndex == 1) {
            var objectTableName = "Shipment";
            var queryCode = "CustomerShipmentActualData";


            var myProductCode = null;
            if (!AppTool.IsNullOrEmpty(this.NewActualVsPotential[e.target.columnIndex].ProductTypeCode[e.item.index])) {
                myProductCode = this.NewActualVsPotential[e.target.columnIndex].ProductTypeCode[e.item.index];
            }

            var actualDate: Date = DateTool.GetDateParts(new Date(this.NewActualVsPotential[e.target.columnIndex].Year[e.item.index], this.NewActualVsPotential[e.target.columnIndex].Month, 1)).DateObject;
            var filterAgrs = new ApiQueryFilters();
            filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
            filterAgrs.addAdditionalFilter("ProductCode", myProductCode, null, null, "Equals", false, false, false, "String");
            filterAgrs.addAdditionalFilter("CustomerId", this.EntityPM.Id, null, null, "Equals", false, false, false, "String");
            filterAgrs.addAdditionalFilter("ActualDataDateYearMonth", this.NewActualVsPotential[e.target.columnIndex].Year[e.item.index], this.NewActualVsPotential[e.target.columnIndex].Month[e.item.index], null, "Equals", true, true, false, "Date");

            var listArgs = new ListComponentArgs();
            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = "Customer Actual Data";
            listArgs.BackButtonTitle = "Back";
            listArgs.ShowViews = false;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, SessionLocator.Tenant).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadQueries());
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        this.CurrentSession.AddMenuReference(cmpRef);
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


    Month_Year() {   
        var s: string ="";
        var myMonth: number = DateTool.GetCurrentDateTimeAsUtc().getUTCMonth() ;
        var myYear: number = DateTool.GetCurrentDateTimeAsUtc().getUTCFullYear();
            if (myMonth == 1) {
                myMonth = 12;
                myYear = myYear - 1;
            }
            else {
                myMonth = myMonth - 1;
            }
            var myFormats = DateTool.GetDateFormats(new Date(myYear, myMonth, 1));
            s += myFormats.MonthNameShort;
            s += "-" + myYear;
            return "Actual (" + s + ")";        
    }

    BuildCompareChartData() {        
        var i = 0;
        var index = 0;
        var Graphs = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        var compareCode: string = "S";
        if (this.SelectedCompareComboBoxItems != null) {
            compareCode = this.SelectedCompareComboBoxItems.Code;
        }

        var productTypeListServce: ProductTypeListService = new ProductTypeListService();
        productTypeListServce.getAllFromCache().subscribe(result => {
            if (result.Result != null) {
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
                var list = result.Result.filter(d => !d.InActive).sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 });;
                this.NewActualVsPotential = [];

                list.forEach(element => {
                    var productItem: CustomerProductPM = this.EntityPM.CustomerProducts.filter(d => d.ProductTypeCode == element.Code)[0];
                    var actualItem: CustomerProductActualDataPM = this.actualDataList.filter(d => d.ProductTypeCode == element.Code)[0];
                    if (this.NewActualVsPotential[1] == null) {
                        this.NewActualVsPotential[1] = { Year: [], label: null, ProductTypeCode: [], Month: [], DateTime: [], BusinessUnitId: [] };
                    }                
                    if (actualItem != null) {
                        this.NewActualVsPotential[1].Year.push(actualItem.Year);
                        this.NewActualVsPotential[1].ProductTypeCode.push(actualItem.ProductTypeCode);
                        this.NewActualVsPotential[1].Month.push(actualItem.Month);
                    }
                    else if (productItem != null) {
                        this.NewActualVsPotential[1].Year.push(null);
                        this.NewActualVsPotential[1].ProductTypeCode.push(productItem.ProductTypeCode);
                        this.NewActualVsPotential[1].Month.push(null);
                    }
                    else {
                        this.NewActualVsPotential[1].Year.push(null);
                        this.NewActualVsPotential[1].ProductTypeCode.push(element.Code);
                        this.NewActualVsPotential[1].Month.push(null);
                    }
                        if (i == 0) {
                            Graphs = [{
                                "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                                "fillAlphas": 1,
                                "id": "AmGraph-1" + i,

                                "title": "Potential",
                                "type": "column",
                                "valueField": "col1",
                                "fillColors": ["#c80d05", "#fb5851"],
                                "lineAlpha": 0,

                                "gradientOrientation": "horizontal",
                                "borderAlpha": 0,
                            },
                                {
                                    "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                                    "fillAlphas": 1,
                                    "id": "AmGraph-2" + i,
                                    "title": this.Month_Year(),
                                    "type": "column",
                                    "lineAlpha": 0,

                                    "valueField": "col2",
                                    "fillColors": ["#0f7816", "#5ed967"],
                                    "gradientOrientation": "horizontal",
                                    "borderAlpha": 0,
                                    "showHandOnHover": true,
                                }

                            ];
                    }
                        var  actualValue = 0;
                       var  productValue = 0;

                        switch (compareCode) {
                            case "S":
                                {
                                    if (actualItem != null) {
                                        actualValue = actualItem.NumberOfShipments;
                                    }

                                    if (productItem != null) {
                                        productValue = productItem.PotentialNumberOfShipments;
                                    }

                                    break;
                                }

                            case "T":
                                {
                                    if (actualItem != null) {
                                        actualValue = actualItem.TEU;
                                    }

                                    if (productItem != null) {
                                        productValue = productItem.PotentialTEU;
                                    }

                                    break;
                                }

                            case "R":
                                {
                                    if (actualItem != null) {
                                        actualValue = actualItem.Revenue;
                                    }

                                    if (productItem != null) {
                                        productValue = productItem.PotentialRevenue;
                                    }

                                    break;
                                }

                            case "C":
                                {
                                    if (actualItem != null) {
                                        actualValue = actualItem.ChargeableWeight;
                                    }

                                    if (productItem != null) {
                                        productValue = productItem.PotentialChargeableWeight;
                                    }

                                    break;
                                }
                        }

                        if (actualValue == null) {
                            actualValue = 0;
                        }

                        if (productValue == null) {
                            productValue = 0;
                        }

                        this.barChartData[0].label = "C";
                        this.barChartData[0].data[i] = productValue ;

                        this.barChartData[1].label = "A";
                        this.barChartData[1].data[i] = actualValue;
                        
                            if (!this.barChartLabels.includes(element.Name)) {
                            this.barChartLabels[i] = element.Name;
                        }
                            i++;

                        if (actualValue > max)
                            max = actualValue;

                        if (productValue > max)
                            max = productValue;
                  
                });

                var i = 0;
                this.barChartLabels.forEach(item => {
                    DataProvider[i] = { "category": this.barChartLabels[i], "col1": this.barChartData[0].data[i], "col2": this.barChartData[1].data[i] };
                    i++;
                });

                if (max < 5)
                    max = 5;
                makeAmBarChart(this.ActualVsPotentialChart, Graphs, DataProvider, max, true, this.legenddiv);        
            }
            });        
    }

    SelectedItem: any;
    LoadQueries() {
        this.LoadActuals();
        this.LoadMoneyQueries();
        this.LoadLineQueries();
    }
    

    public DateTypeFilterList: DashBoardFilters[];
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

    private filterName_DateType: string = "DateType";
    private filterName_TimeRange: string = "TimeRange";
    private filterControlNameSpace: string = "Components.Partners.EditTabs.Customer.CustomerOverviewTabComponent";

    private selectedDateTypeItem: DashBoardFilters;
    get SelectedDateTypeItem() { return this.selectedDateTypeItem; }
    set SelectedDateTypeItem(value: DashBoardFilters) {
        if (this.selectedDateTypeItem != value) {
            this.selectedDateTypeItem = value;

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_DateType, (value == null ? null : value.Index));

            this.LoadLineQueries();
        }
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
    public TimeRangeFilterList: DashBoardFilters[];
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

    FillCompareList() {
        var comboBoxItemClass = new ComboBoxIemClass();
        comboBoxItemClass.GetCompareList = new Array<ComboBoxIemClass>();
        var item: ComboBoxIemClass = new ComboBoxIemClass()
        item.Code = "S";
        item.Name = "Shipments";
        comboBoxItemClass.GetCompareList.push(item);

        item = new ComboBoxIemClass()
        item.Code = "T";
        item.Name = "TEU";
        comboBoxItemClass.GetCompareList.push(item);


        item = new ComboBoxIemClass()
        item.Code = "R";
        item.Name = "Revenue";
        comboBoxItemClass.GetCompareList.push(item);

        item = new ComboBoxIemClass()
        item.Code = "C";
        item.Name = "Chargeable Weight";
        comboBoxItemClass.GetCompareList.push(item);

        this.CompareComboBoxItems = comboBoxItemClass.GetCompareList;
        this.SelectedCompareComboBoxItems = this.CompareComboBoxItems.filter(d => d.Code == "S")[0];
    }

    FillFilters() {
        this.FillDateTypeFilterList();
        this.FillTimeRangeFilterList();
        this.FillShowFilterList();
        this.FillCompareList();
        this.LoadQueries();
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
            offsetGridLines: false
            ,
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


    private selectedShowItem: DashBoardFilters;
    get SelectedShowItem() { return this.selectedShowItem; }
    set SelectedShowItem(value: DashBoardFilters) {
        if (this.selectedShowItem != value) {
            this.selectedShowItem = value;

            this.FilterSelectedShow();
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

        if (this.SelectedTimeRangeItem.Index == "-1") {
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
            this.selectedTimeRangeItem = this.TimeRangeFilterList[3];
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityFromDate", (value == null ? null : ServiceHelper.GetDateString(value)));
            this.LoadLineQueries();
        }
    }

    private activityToDate: Date;
    public get ActivityToDate() { return this.activityToDate; }
    public set ActivityToDate(value: Date) {
        if (value != this.activityToDate) {
            this.activityToDate = value;
            this.selectedTimeRangeItem = this.TimeRangeFilterList[3];
            LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityToDate", (value == null ? null : ServiceHelper.GetDateString(value)));
            this.LoadLineQueries();
        }
    }

    public NoData: boolean = true; 

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

    public RankListArr: Array<RankList> = [];

    public EntityId: string = "";
    public EntityName: string = "";


    private _entityResourceService: EntityResourceService = new EntityResourceService();

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.EntityId = this.EntityPM.Id;
        this.EntityName = "Customer";
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityNotes = this.EntityPM.Notes;
        this.ActivityStatusOverViewDashboardId = this.ActivityStatusOverViewDashboardId + this.CurrentSession.GetChartId();
        this.ActualVsPotentialChart = this.ActualVsPotentialChart + this.CurrentSession.GetChartId();
        this.legenddiv = this.legenddiv + this.CurrentSession.GetChartId();
    }

    IsShowMessageComplate: boolean = false;
    IsShowProgressLoading: boolean = false;
    LogoFileHtmlId: string = Guid.NewRandomString();
    public DataContext: CustomerStatisticsTabComponent = this;

    ngOnInit() {
        this.LoadMoneyQueries();
        this.FillFilters();
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
        this.LoadQueries();
    }



    public isWindowOpened: boolean = false;

    BusinessClickNew(code: string) {

        var path = "";
        var windowTitle = "";
        switch (code + "") {

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
                    
                }
            });

        });



    }

    public TenantPM: TenantPM;

    public ARPayments = 0.0;
    public InvoicesDue = 0.0;
    public OpenARInvoices = 0.0;
    public OpenReceivables = 0.0;

    public EntityNotes: string = "";
  
    LoadMoneyQueries() {
        var service = new QuoteDomainService();
        service.GetCRMMoneyInformation(this.TenantPM.Id, this.EntityPM.Id).subscribe((myResult: any) => {
            this.ARPayments = myResult.Result.ARPayments;
            this.InvoicesDue = myResult.Result.InvoicesDue;
            this.OpenARInvoices = myResult.Result.OpenARInvoices;
            this.OpenReceivables = myResult.Result.OpenReceivables;
        });
    }    
}

export class ComboBoxIemClass {
    private code: string
    public get Code() { return this.code; }
    public set Code(value: string) { this.code = value; }

    private name: string
    public get Name() { return this.name; }
    public set Name(value: string) { this.name = value; }

    public GetCompareList: Array<ComboBoxIemClass>;

    constructor() {       


    }

}
