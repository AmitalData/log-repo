declare var window: any;
import {Component, ViewChildren, QueryList, ViewChild, ViewContainerRef, Output, EventEmitter} from '@angular/core';
import {InvoiceDomainService, CreditorsClass} from '../../../Invoice/Services/InvoiceDomainService';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {ListComponentArgs} from '../../../Infrastructure/Args';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {APPaymentPM} from '../../EntityPMs/APPaymentPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {CurrencyListService} from '../../../Common/Services/StandardLists/CurrencyListService';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {DashBoardFilters} from '../../../Infrastructure/DataContracts/Dashboard/DashboardFilters';
import {LastFilter} from '../../../Infrastructure/Utilities/LastFilter';
import {ChartsService} from '../../../Infrastructure/Services/ChartsService';
declare var makeAmBarChart: any;
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'AccountPayablesComponent',
    moduleId: module.id,
    templateUrl: './AccountPayablesComponent.html',
})

export class AccountPayablesComponent {
    MoneyOutLocalCurrency: string = "(" + SessionLocator.LocalCurrencyCode + ")";

    public TenantPM: TenantPM;
    public CreditorsObsList: CreditorsClass[] = [];
    public myViewsQueryVisibility = false;
    public APInvoiceErrorInTransferVisibility: boolean = false;
    public APPaymentErrorInTransferVisibility: boolean = false;

    public PayablesChartId: string = "PayablesChartId";
    FilterList: DashBoardFilters[] = [];
    SelectedItem: any;
    private myChartsService: ChartsService;
    public PayablesChartMoneyInOutId: string = "PayablesChartMoneyInOutId";
    public ItemsSource: ObservableCollection;
    @Output() ReloadUserQueries = new EventEmitter();
    public isRTL: boolean = false;

    constructor(private _entityResourceService: EntityResourceService) {
        if (ObjectsLocator.GlobalSetting) {
            this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }
        this.TenantPM = SessionLocator.TenantPM;
        this.myViewsQueryVisibility = FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES") ? true : false;
        this.PayablesChartMoneyInOutId += SessionLocator.CurrentSession.GetChartId();
        this.PayablesChartId += SessionLocator.CurrentSession.GetChartId();
        this.myChartsService = new ChartsService();
        this.ItemsSource = new ObservableCollection([]);
        this.FillFilters();
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

    LoadBarQueries(months: number, days: number, index: number, currency: number) {
        this.myChartsService.GetMoneyOutStatusForTenant(months, days, this.TenantPM.Id, index, currency).subscribe(myResult => {
            this.FillBarsMoney(myResult);
        });
    }
    FillBarsMoney(MoneyInBarData) {
        var i = 0;
        var index = 0;
        var Graphs = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        var barChartData: any[] = [{ data: [], label: '' }, { data: [], label: '' }];

        barChartData[0].data = [];
        barChartData[1].data = []
        var barChartLabels = [];
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

        makeAmBarChart(this.PayablesChartMoneyInOutId, Graphs, DataProvider,  max, null, null, null, null, poisition );
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

    InitComponent() {
        this.LoadAllScreenData();
        this.APInvoiceErrorInTransferVisibility = (FeatureLocator.HasFeaturePermession("APInvoice", "ErrorInTransfer")) ? true : false;
        this.APPaymentErrorInTransferVisibility = (FeatureLocator.HasFeaturePermession("APPayment", "ErrorInTransfer")) ? true : false;
    }

    LoadAllScreenData() {
        //this.LoadCurrencyCodeLocal();
       // this.LoadCurrencyCodeProfit();
        this.LoadQueriesCounts();
        this.LoadGridDataAP();
        this.LoadChartDataAP();
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

    // Grid View
    public CurrencyCodeLocal: string = SessionLocator.LocalCurrencyCode;
    public CurrencyCodeProfit: string = SessionLocator.TenantPM.ProfitCurrencyCode; 
   
    private selectedCurrencyIndex_APGrid: number = 1;
    get SelectedCurrencyIndex_APGrid() {
        return this.selectedCurrencyIndex_APGrid;
    }
    set SelectedCurrencyIndex_APGrid(value: number) {
        if (this.selectedCurrencyIndex_APGrid != value) {
            this.selectedCurrencyIndex_APGrid = value;
            this.LoadGridDataAP();
        }
    }

    SetSelectedCurrencyIndex(args: number) {
        this.SelectedCurrencyIndex_APGrid = args;
    }

    LoadGridDataAP() {
        this.ItemsSource.Clear();
        if (this.invoiceDomainService == null) {
            this.invoiceDomainService = new InvoiceDomainService();
        }

        this.invoiceDomainService.GetCreditorExposure(this.SelectedCurrencyIndex_APGrid).subscribe((myResult: ServiceResponse) => {
            if (myResult) {

                if (!myResult.HasError) {
                    this.CreditorsObsList = [];
                    this.CreditorsObsList = myResult.Result;
                    this.ItemsSource.InsertCollection(this.CreditorsObsList);
                }
            }
        });
    }

    // Dashboard
    private selectedCurrencyIndex_APChart: number = 1;
    get SelectedCurrencyIndex_APChart() {
        return this.selectedCurrencyIndex_APChart;
    }
    set SelectedCurrencyIndex_APChart(value: number) {
        if (this.selectedCurrencyIndex_APChart != value) {
            this.selectedCurrencyIndex_APChart = value;
            this.LoadChartDataAP();
        }
    }

    SetSelectedCurrencyAPChartIndex(args: number) {
        this.SelectedCurrencyIndex_APChart = args;
    }

    barChartColors: any[] = [
        {
            backgroundColor: "rgb(23, 130, 184)",
        },
    ]

    LoadChartDataAP() {
        if (this.invoiceDomainService == null) {
            this.invoiceDomainService = new InvoiceDomainService();
        }

        this.invoiceDomainService.GetAgingReportAPInvioceData(this.SelectedCurrencyIndex_APChart).subscribe((myResult: ServiceResponse) => {
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
            dataProvider.push({ category: element.DateRange, ammount: element.Amount });
        });
        var poisition = this.isRTL == true ? "right" : "left";

        makeAmBarChart(this.PayablesChartId, Graphs, dataProvider, max, null, null, null, null, poisition);

    }

    //Counts
    public APInvoicesDraftsCount: string;
    public APInvoicesUnpaidCount: string;
    public APPaymentsDraftsCount: string;
    public APPaymentsOpenedCount: string;

    public APInvoiceErrorInTransferCount: string
    public APPaymentErrorInTransferCount: string;
    private invoiceDomainService: InvoiceDomainService;
    LoadQueriesCounts() {
        if (this.invoiceDomainService == null) {
            this.invoiceDomainService = new InvoiceDomainService();
        }

        this.invoiceDomainService.GetAccountPayablesSummary().subscribe(myResult => {
            if (myResult != null) {
                this.APInvoicesDraftsCount = myResult.APInvoicesDraftsCount > 1000 ? "1000+" : myResult.APInvoicesDraftsCount.toString();
                this.APInvoicesUnpaidCount = myResult.APInvoicesUnpaidCount > 1000 ? "1000+" : myResult.APInvoicesUnpaidCount.toString();
                this.APPaymentsDraftsCount = myResult.APPaymentsDraftsCount > 1000 ? "1000+" : myResult.APPaymentsDraftsCount.toString();
                this.APPaymentsOpenedCount = myResult.APPaymentsOpenedCount > 1000 ? "1000+" : myResult.APPaymentsOpenedCount.toString();
                this.APInvoiceErrorInTransferCount = myResult.ARInvoicesFailedCount > 1000 ? "1000+" : myResult.APInvoicesFailedCount.toString();
                this.APPaymentErrorInTransferCount = myResult.ARPaymentFailedCount > 1000 ? "1000+" : myResult.APPaymentFailedCount.toString();

            }
        });
    }

    // Query Commands
    filterAgrs: ApiQueryFilters;
    ViewInvoiceQuery(args: string) {
        if (args != null) {

            var backButtonTitle = TextCodeTranslator.Translate("General.MH.Accounting");
            var objectTableName = args.split(':')[0];
            var queryCode = args.split(':')[1];
            var displayTitle = queryCode;
            this.filterAgrs = new ApiQueryFilters();
            var ObjectTable = window.ObjectTables.filter(x => x.Name === objectTableName)[0];
            //var query = window.Queries.filter(q => q.ObjectTableId == ObjectTable.Id && q.Code == queryCode)[0];

            //if (window.PreDefinedFilters.filter(d => d.QueryId == query.Id) != null) {
            //    var predefinedFilters = window.PreDefinedFilters.filter(d => d.QueryId == query.Id);

            //    predefinedFilters.forEach((filter, key) => {
            //        var filterOperator = (!AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
            //        var value1 = filter.PredefinedValue;
            //        var value2 = filter.PredefinedValue2;
            //        if (value2 != null) {
            //            filterOperator = "Between";
            //        }
            //        this.filterAgrs.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, filter.IsCustomFilter, filter.DisplayInList, false, filter.DataTypeCode);
            //    });
            //}

            //this.filterAgrs.ObjectTableName = query.ObjectTableName;

            var listArgs = new ListComponentArgs();
            //listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.BackButtonTitle = backButtonTitle;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', SessionLocator.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                        SessionLocator.CurrentSession.AddMenuReference(cmpRef);
                    });
            });
        }
    }

    // Edit Customer
    LoadEditCustomer(entity: CreditorsClass) {
        var objectTableName = null;
        var editedPartnerId = entity.CreditorId;

        switch (entity.CreditorType) {
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

         SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: editedPartnerId,  ObjectTableName: objectTableName });
            });
    }

    // Commands 
    NewEntity() {
        
    }

    public NewEntityClicked(args: string) {
        if (args != null) {
            switch (args) {
                case "APPayment":
                    {
                        this.NewAPPaymentMethod();
                        break;
                    }
                case "APInvoiceMultiple":
                    {
                        this.NewMultipleInvoiceMethod();
                        break;
                    }
            }
        }
    }

    NewAPPaymentMethod() {
        var message = "";
        if (!FeatureLocator.HasEntityPermessions("APPayment", "NEW", true)) {
            return;
        }
        var newApPaymentPM: APPaymentPM = new APPaymentPM();
        newApPaymentPM.StatusCode = "DR";
        newApPaymentPM.StatusName = "Draft";
        newApPaymentPM.Tenant = this.TenantPM.Id;
        newApPaymentPM.IsClosed = false;
        newApPaymentPM.CreatedByUserId = SessionLocator.LoggedUserId;
        newApPaymentPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        newApPaymentPM.CreateDate = DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.BranchId = SessionLocator.LoggedUserPM.BranchId;

        newApPaymentPM.LocalCurrencyId = this.TenantPM.CurrencyId;
        newApPaymentPM.ValueDate = DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.RegisterDate = DateTool.GetCurrentDateAsUtc();

        var backButtonLabel = TextCodeTranslator.Translate("General.MH.Accounting");
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: newApPaymentPM.Id, EntityPM: newApPaymentPM, BackButtonLabel: backButtonLabel, ObjectTableName: 'APPayment'});
            });
    }

    NewMultipleInvoiceMethod() {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { IsMultipleEntities: true };

        logWindow.Title = "Receive Multiple AP Invoice";
        logWindow.Show("./InvoiceModules/APInvoice/Components/NewEntity/NewAPInvoiceComponent");

        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ EntityPM: comp.EntityPM, ObjectTableName: 'APInvoice', BackButtonLabel: TextCodeTranslator.Translate("General.MH.Accounting") });

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
    get MyViewsQueryVisibility() {
        return this.myViewsQueryVisibility;
    }

    onUserQueriesBackComplete(event) {
        this.LoadAllScreenData();
    }

    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }
}
