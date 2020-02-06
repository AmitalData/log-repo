declare var window: any;
import {Component, OnInit, ViewChild, ViewContainerRef, EventEmitter, Output} from '@angular/core';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ListComponentArgs} from '../../../Infrastructure/Args';
import {InvoiceDomainService, DebtorsClass} from '../../../Invoice/Services/InvoiceDomainService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {AppTool} from '../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ARPaymentPM} from '../../EntityPMs/ARPaymentPM';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {CurrencyListService} from '../../../Common/Services/StandardLists/CurrencyListService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {DashBoardFilters} from '../../../Infrastructure/DataContracts/Dashboard/DashboardFilters';
import {LastFilter} from '../../../Infrastructure/Utilities/LastFilter';
import {ChartsService} from '../../../Infrastructure/Services/ChartsService';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

declare var makeAmBarChart: any;
@Component({
    selector: 'OperationsComponent',
    moduleId: module.id,
    templateUrl: './AccountReceivablesComponent.html',
})

export class AccountReceivablesComponent implements OnInit {
    public TenantPM: TenantPM;
    public DebtorsObsList: DebtorsClass[] = [];
    FilterList: DashBoardFilters[] = [];
    SelectedItem: any;
    MoneyInLocalCurrency: string = "(" + SessionLocator.LocalCurrencyCode + ")";
    private myChartsService: ChartsService;
    public myViewsQueryVisibility = false;
    public consolidationButtonVisibility = false;
    public ReceivablesChartId: string = "ReceivablesChartId";
    public ReceivablesChartMoneyInOutId: string = "ReceivablesChartMoneyInOutId";
    public ItemsSource: ObservableCollection;
    public isRTL: boolean = false;
    public ARInvoicesSATFailedVisibility: boolean = false;
    public ARPaymentsSATFailedVisibility: boolean = false;
    public ARInvoiceErrorInTransferVisibility: boolean = false;
    public ARPaymentErrorInTransferVisibility: boolean = false;

    @Output() ReloadUserQueries = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService) {
        if (ObjectsLocator.GlobalSetting) {
            this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }
        this.TenantPM = SessionLocator.TenantPM;
        this.myViewsQueryVisibility = FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES") ? true : false;
        this.consolidationButtonVisibility = FeatureLocator.HasFeaturePermession("ARInvoice", "Consolidation.Constituent") ? true : false;
        this.ReceivablesChartId += this.CurrentSession.GetChartId();
        this.ReceivablesChartMoneyInOutId += this.CurrentSession.GetChartId();
        this.ItemsSource = new ObservableCollection([]);
        this.myChartsService = new ChartsService();
    }

    InitComponent() {
      this.LoadAllScreenData();
      
      this.ARInvoicesSATFailedVisibility = (FeatureLocator.HasFeaturePermession("ARInvoice", "SATFAILEDINVOICES") && SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") ? true : false;
      this.ARPaymentsSATFailedVisibility = (FeatureLocator.HasFeaturePermession("ARPayment", "SATFAILEDPAYMENTS") && SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") ? true : false;
      this.ARInvoiceErrorInTransferVisibility = (FeatureLocator.HasFeaturePermession("ARInvoice", "ErrorInTransferInvoices")) ? true : false;
      this.ARPaymentErrorInTransferVisibility = (FeatureLocator.HasFeaturePermession("ARPayment", "ErrorInTransfer")) ? true : false;

    }

    FillFilters() {
        var list = LastFilter.myList();
        this.FilterList.push(new DashBoardFilters(list[0].lastTitle, 0 + ""));
        this.FilterList.push(new DashBoardFilters(list[1].lastTitle, 1 + ""));
        this.FilterList.push(new DashBoardFilters(list[2].lastTitle, 2 + ""));
        this.FilterList.push(new DashBoardFilters(list[3].lastTitle, 3 + ""));

        this.SelectedItem = this.FilterList[1];
        this.FilterSelectedChange(this.SelectedItem);        
    }

    FilterSelectedChange(item) {
        var days;
        this.SelectedItem = item;
        if (item.Index == 0) days = -7;
        else if (item.Index == 1) days = -30;
        else if (item.Index == 2) days = -90;
        else if (item.Index == 3) days = -365;
        this.LoadBarQueries(0, days, item.Index, 1);
    }

    LoadBarQueries(months: number, days: number, index: number, currency: number) {
        this.myChartsService.GetMoneyStatusForTenant(null,months, days, this.TenantPM.Id, index, currency).subscribe(myResult => {
            this.FillBarsMoney(myResult);
        });
    }

    LoadAllScreenData() {
       // this.LoadCurrencyCodeLocal();
        //this.LoadCurrencyCodeProfit();
        this.LoadQueriesCounts();
        this.LoadGridDataAR();
        this.LoadChartDataAR();
    }

    LoadCurrencyCodeLocal() {
        var myService: CurrencyListService = new CurrencyListService();
        myService.getSingleFromCache(SessionLocator.TenantPM.CurrencyId).subscribe((myResult: ServiceResponse) => {
            if (myResult) {
                this.CurrencyCodeLocal = myResult.Result.Code;
            }
        });
    }

    LoadCurrencyCodeProfit() {
        var myService: CurrencyListService = new CurrencyListService();
        myService.getSingleFromCache(SessionLocator.TenantPM.ProfitCurrencyId).subscribe((myResult: ServiceResponse) => {
            if (myResult) {
                this.CurrencyCodeProfit = myResult.Result.Code;
            }
        });
    }

    ngOnInit() {
        this.FillFilters(); 
    }

    public CurrencyCodeLocal: string = SessionLocator.LocalCurrencyCode;
    public CurrencyCodeProfit: string = SessionLocator.TenantPM.ProfitCurrencyCode; 

    private selectedCurrencyIndex_ARGrid: number = 1;
    get SelectedCurrencyIndex_ARGrid() {
        return this.selectedCurrencyIndex_ARGrid;
    }
    set SelectedCurrencyIndex_ARGrid(value: number) {
        if (this.selectedCurrencyIndex_ARGrid != value) {
            this.selectedCurrencyIndex_ARGrid = value;
            this.LoadGridDataAR();
        }
    }

    SetSelectedCurrencyARGridIndex(args: number) {
       this.SelectedCurrencyIndex_ARGrid = args;
    }

    LoadGridDataAR() {
        this.ItemsSource.Clear();
        if (this.invoiceDomainService == null) {
            this.invoiceDomainService = new InvoiceDomainService();
        }

        this.invoiceDomainService.GetDebrotExposureForGridControl(this.SelectedCurrencyIndex_ARGrid).subscribe((myResult: ServiceResponse) => {
            if (myResult) {

                if (!myResult.HasError) {
                    this.DebtorsObsList = [];
                    this.DebtorsObsList = myResult.Result;
                    this.ItemsSource.InsertCollection(this.DebtorsObsList);
                }
            }
        });
    }

    private selectedCurrencyIndex_ARChart: number = 1;
    get SelectedCurrencyIndex_ARChart() {
        return this.selectedCurrencyIndex_ARChart;
    }
    set SelectedCurrencyIndex_ARChart(value: number) {
        if (this.selectedCurrencyIndex_ARChart != value) {
            this.selectedCurrencyIndex_ARChart = value;
            this.LoadChartDataAR();
        }
    }

    SetSelectedCurrencyARChartIndex(args: number) {
        this.SelectedCurrencyIndex_ARChart = args;
    }

    barChartColors: any[] = [
        {
            backgroundColor: "rgb(23, 130, 184)",
        },
    ]

    LoadChartDataAR() {
        if (this.invoiceDomainService == null) {
            this.invoiceDomainService = new InvoiceDomainService();
        }
        this.invoiceDomainService.GetAgingReportARInvioceData(this.SelectedCurrencyIndex_ARChart, null).subscribe((myResult: ServiceResponse) => {
            this.BarData = myResult.Result;
            this.FillBars();
        });
    }
    public barChartLabels: string[] = [];
    public barChartData: any[] = [{ data: [], label: '' }];
    public BarData: any;

    FillBars() {
        var i = 0;
        var index = 0;
        this.barChartData[0].data = [];
        this.barChartLabels = [];

        var Graphs: any = [{
            "balloonText": "[[value]]",
            "fillAlphas": 0.8,
            "type": "column",
            "valueField": "ammount",
            "fillColors": ["#487E9F", "#c8d8e2"],     
            "lineAlpha": 0,
       
        }];

        var dataProvider: Array<any> = [];
        var max = 0;
        
        this.BarData.forEach(element => {
            if (element.Amount > max)
                max = element.Amount;
            dataProvider.push({ category: element.DateRange, ammount: element.Amount.toFixed(2)});            
        });
        var poisition = this.isRTL == true ? "right" : "left";
        makeAmBarChart(this.ReceivablesChartId, Graphs, dataProvider, max, null, null, null, null, poisition );  
    }

    FillBarsMoney(MoneyInBarData) {
        var i = 0;
        var index = 0;
        var Graphs = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        var  barChartData: any[] = [{ data: [], label: '' }, { data: [], label: '' }];

        barChartData[0].data = [];
        barChartData[1].data = []
       var  barChartLabels = [];
        var max = 0;
        MoneyInBarData.forEach(element => {
            if (i == 0) {
                Graphs = [{
                    "balloonText": "[[value]]",
                    "fillAlphas": 1,
                    "id": "AmGraph-1" + i,
                    "title": "Invoices",
                    "type": "column",
                    "valueField": "col1",
                    "fillColors": ["#d29127", "#dfb267"],
                    "gradientOrientation": "horizontal",
                    "borderAlpha": 0,
                    "lineAlpha": 0,

                },
                    {
                        "balloonText": "[[value]]",
                        "fillAlphas": 1,
                        "id": "AmGraph-2" + i,
                        "title": "Payments",
                        "type": "column",
                        "valueField": "col2",
                        "fillColors": ["#1d758e", "#2186a3"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "lineAlpha": 0,

                    }

                ];
            }
            if (element.DataType == 'Invoices') {
                barChartData[0].label = element.DataType;
                barChartData[0].data[i] = element.TotalAmount;
            }
            else if (element.DataType == 'Payments') {
                barChartData[1].label = element.DataType;
                barChartData[1].data[i] = element.TotalAmount;
            }
            if (!barChartLabels.includes(element.DateRange)) {
                barChartLabels[i] = element.DateRange;
                i++;
            }

            if (element.TotalAmount > max)
                max = element.TotalAmount;

        });
        var i = 0;
        barChartLabels.forEach(item => {
            DataProvider[i] = { "category": barChartLabels[i], "col1": barChartData[0].data[i], "col2": barChartData[1].data[i] };
            i++;
        });
        var poisition = this.isRTL == true ? "right" : "left";
        makeAmBarChart(this.ReceivablesChartMoneyInOutId, Graphs, DataProvider, max, null, null, null, null, poisition);      
    }


    //Props
    get ConsolidationButtonVisibility() {
        return this.consolidationButtonVisibility;
    }    

    get MyViewsQueryVisibility() {
        return this.myViewsQueryVisibility;
    }
    // Query Commands
    filterAgrs: ApiQueryFilters;
    ViewInvoiceQuery(args: string) {
        if (args != null) {

            var backButtonTitle = TextCodeTranslator.Translate("General.MH.Accounting");
            var objectTableName = args.split(':')[0];
            var queryCode = args.split(':')[1];
            this.filterAgrs = new ApiQueryFilters();

            var ObjectTable = window.ObjectTables.filter(x => x.Name === objectTableName)[0];
            //var query = window.Queries.filter(q => q.ObjectTableId == ObjectTable.Id && q.Code == queryCode)[0];

            //if (window.PreDefinedFilters.filter(d => d.QueryId == query.Id) != null) {
            //    var predefinedFilters = window.PreDefinedFilters.filter(d => d.QueryId == query.Id);

            //    predefinedFilters.forEach((filter, key) => {
            //        var filterOperator = (!AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
            //        var value1 = filter.PredefinedValues
            //        var value2 = filter.PredefinedValue2;
            //        if (value2 != null) {
            //            filterOperator = "Between";
            //        }
            //        this.filterAgrs.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, filter.IsCustomFilter, filter.DisplayInList, false, filter.DataTypeCode);
            //    });
            //}

           // this.filterAgrs.ObjectTableName = query.ObjectTableName;

            var listArgs = new ListComponentArgs();
            //listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.BackButtonTitle = backButtonTitle;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
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

    //Counts
    public ARInvoicesDraftsCount: string;
    public ARInvoicesUnpaidCount: string;
    public ARInvoicesOpenConstituentCount: string;
    public ARPaymentsDraftsCount: string;
    public ARPaymentsOpenedCount: string;
    public ARInvoicesSATFailedCount: string;
    public ARPaymentsSATFailedCount: string;
    public ARInvoiceErrorInTransferCount: string
    public ARPaymentErrorInTransferCount: string;
    private invoiceDomainService: InvoiceDomainService;
    LoadQueriesCounts() {
        if (this.invoiceDomainService == null) {
            this.invoiceDomainService = new InvoiceDomainService();
        }

        this.invoiceDomainService.GetAccountingReceivablesSummary().subscribe(myResult => {
            if (myResult != null) {
                this.ARInvoicesDraftsCount = myResult.ARInvoicesDraftsCount > 1000 ? "1000+" : myResult.ARInvoicesDraftsCount.toString();
                this.ARInvoicesUnpaidCount = myResult.ARInvoicesUnpaidCount > 1000 ? "1000+" : myResult.ARInvoicesUnpaidCount.toString();
                this.ARInvoicesOpenConstituentCount = myResult.ARInvoicesOpenConstituentCount > 1000 ? "1000+" : myResult.ARInvoicesOpenConstituentCount.toString();
                this.ARPaymentsDraftsCount = myResult.ARPaymentsDraftsCount > 1000 ? "1000+" : myResult.ARPaymentsDraftsCount.toString();
                this.ARPaymentsOpenedCount = myResult.ARPaymentsOpenedCount > 1000 ? "1000+" : myResult.ARPaymentsOpenedCount.toString();
              this.ARInvoicesSATFailedCount = myResult.ARPaymentsOpenedCount > 1000 ? "1000+" : myResult.ARInvoicesSATFailedCount.toString();
                this.ARPaymentsSATFailedCount = myResult.ARPaymentsOpenedCount > 1000 ? "1000+" : myResult.ARPaymentsSATFailedCount.toString();
                this.ARInvoiceErrorInTransferCount = myResult.ARInvoicesFailedCount > 1000 ? "1000+" : myResult.ARInvoicesFailedCount.toString();
                this.ARPaymentErrorInTransferCount = myResult.ARPaymentFailedCount > 1000 ? "1000+" : myResult.ARPaymentFailedCount.toString();

            }
        });
    }

    // Edit Customer
    LoadEditCustomer(entity: DebtorsClass) {
        var objectTableName = null;
        var editedPartnerId = entity.DebtorId;

        switch (entity.DebtorType) {
            case "AG":
            case "CO":
            case "FL":
                {
                    objectTableName = "Agent";
                    break;
                }

            case "WH":
            case "CC":
                {
                    objectTableName = "Warehouse";
                    break;
                }

            case "CS":
            case "PO":
                {
                    objectTableName = "Customer";
                    break;
                }

            case "AL": { objectTableName = "Airline"; break; }
            case "CG": { objectTableName = "CustomAgent"; break; }
            case "SG": { objectTableName = "ShippingAgent"; break; }
            case "SL": { objectTableName = "ShippingLine"; break; }
            case "TR": { objectTableName = "Trucker"; break; }
            case "VD": { objectTableName = "Vendor"; break; }

        }

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: editedPartnerId, ObjectTableName: objectTableName });
            });
    }

    // Commands
    public NewEntityClicked(args: string) {
        if (args != null) {
            switch (args) {
                case "ARPayment":
                    {
                        this.NewARPaymentMethod();
                        break;
                    }

                case "ARInvoiceConsolidation:IN":
                    {
                        this.NewConsolidationMethod("IN");
                        break;
                    }

                case "ARInvoiceConsolidation:CD":
                    {
                        this.NewConsolidationMethod("CD");
                        break;
                    }
            }
        }
    }

    NewARPaymentMethod() {
        var message = "";
        if (!FeatureLocator.HasEntityPermessions("ARPayment", "NEW", true)) {
            return;
        }

        var str = TextCodeTranslator.Translate("General.O.NewEntity");
        str = str.replace("%Entity", TextCodeTranslator.TranslateTable("ARPayment"));

        var logWindow = new LogitudeWindow();
        logWindow.Title = str;
        logWindow.Width = 900;
        logWindow.Height = 570;
        logWindow.Show("./InvoiceModules/ARPayment/Components/NewEntity/NewARPaymentComponent");

        //SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
        //    .then(cmpRef => {
        //        cmpRef.instance.ComponentRef = cmpRef;
        //        cmpRef.instance.Run({ EntityId: "", EntityPM: new ARPaymentPM(), ObjectTableName: 'ARPayment' });
        //    });

    }

    NewConsolidationMethod(typeCode: string) {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = typeCode;
        logWindow.Title = TextCodeTranslator.Translate("ARInvoice.S.NewConsolidation");
        logWindow.Show("./InvoiceModules/ARInvoice/Components/NewEntity/NewConsolidationComponent");

        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ EntityPM: comp.EntityPM, ObjectTableName: 'ARInvoice', BackButtonLabel: TextCodeTranslator.Translate("General.MH.Accounting") });

                            let isEditComponentSaved = false;
                            cmpRef.instance.BackCompleted.subscribe(bk => {
                                if (isEditComponentSaved) {
                                    this.LoadAllScreenData();
                                }
                            });

                            cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                                if (isSaveSuccess) {
                                    isEditComponentSaved = true;
                                }
                            });
                        });
                }
            });
        });
    }


    // My Views 
    onUserQueriesBackComplete(event) {
        this.LoadAllScreenData();
    }

    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }
}
