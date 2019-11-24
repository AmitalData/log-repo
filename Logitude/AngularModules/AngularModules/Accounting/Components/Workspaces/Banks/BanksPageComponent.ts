declare var makeAmBarChart, makeBanksBarChart, BarClick;
import {Component, Output, EventEmitter, OnInit, AfterViewInit} from '@angular/core';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {ListComponentArgs} from '../../../../Infrastructure/Args';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {CashBookList} from '../../../EntityLists/CashBookList';
import {CashBookStatusChart} from '../../../DataContracts/CashBookStatusChart';
import {RatesTableList} from '../../../../Infrastructure/EntityLists/RatesTableList';
import {BankDepositList} from '../../../EntityLists/BankDepositList';
import {BankDepositExtendedListService} from '../../../Services/ExtendedLists/BankDepositExtendedListService';
import {CashBookExtendedListService} from '../../../Services/ExtendedLists/CashBookExtendedListService';
import {CashBookListService} from '../../../Services/StandardLists/CashBookListService';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {RatesTableListService} from '../../../../Infrastructure/Services/StandardLists/RatesTableListService';
//import {List} from '../../../../Infrastructure/DataContracts/Dashboard/List';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ReconcileEventManager} from '../../../Utilities/ReconcileEventManager';
import {BankAccountExtendedListService} from '../../../Services/ExtendedLists/BankAccountExtendedListService';
import {BankAccountSummary} from '../../../DataContracts/AccountingSummery';
import {PaymentChequeSummary} from '../../../DataContracts/AccountingSummery';
import {PaymentChequeExtendedListService} from '../../../Services/ExtendedLists/PaymentChequeExtendedListService';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { BankDepositSummary } from '../../../DataContracts/AccountingSummery';
import { CashBookSummary } from '../../../DataContracts/AccountingSummery';


@Component({
    moduleId: module.id,
    templateUrl: './BanksPageComponent.html',
})

export class BanksPageComponent {
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private myBankDepositService: BankDepositExtendedListService = new BankDepositExtendedListService();
    private myCashBookExtendedListService: CashBookExtendedListService = new CashBookExtendedListService();
    private myCashBookListService: CashBookListService = new CashBookListService();
    private currencyListService: CurrencyListService = new CurrencyListService();
    private ratesTableListService: RatesTableListService = new RatesTableListService();
    private _BankAccountExtendedListService: BankAccountExtendedListService = new BankAccountExtendedListService();
    paymentChequeExtendedListService: PaymentChequeExtendedListService = new PaymentChequeExtendedListService();
    public RecentBankDepositCount: number = 0;
    _BankAccountSummary: BankAccountSummary = new BankAccountSummary();
    paymentChequeSummary: PaymentChequeSummary = new PaymentChequeSummary();
    bankDepositSummary: BankDepositSummary = new BankDepositSummary();
    cashBookSummary: CashBookSummary = new CashBookSummary();
    screenHeight: number = 0;
    get ScreenHeight() { return this.screenHeight; }
    set ScreenHeight(value: number) {
        this.screenHeight = value;
    }


    //#region Queries Features
    LoadBankPageMENUVisibility: boolean = false;
    public TodayDepositsVisibility: boolean = false;
    public cashDepositsVisibility: boolean = false;
    public chequeDepositVisibility: boolean = false;
    public AllBankDepositsVisibility: boolean = false;
    //#endregion

    txt_cash: string = TextCodeTranslator.Translate('BankDeposit.Q.cash');
    txt_chequeDeposit: string = TextCodeTranslator.Translate('BankDeposit.Q.chequeDeposit');

    public isRTL: boolean = false;
    chartId: string = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
      this.chartId = "CashBookChart_" + this.CurrentSession.GetChartId();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this._entityResourceService.getEntityResourceByTableName("CashBook").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("BankDeposit").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("ARPayment").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("ARInvoice").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("BankAccount").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("ReconcileExternalPage").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("ReconcileExternalPageLine").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("ExternalReconciliation").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("LedgerTransaction").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("ExternalReconciliationLine").subscribe((response: any) => { });
        this.LoadTenantCurrency();
        this.LoadAllScreenData();
    }
    InitComponent() {
        this.LoadAllScreenData();
        this.ScreenHeight = this.getScreenHeight();
     }

    RefreshButtonClicked() {
        this.LoadAllScreenData();
        this.ScreenHeight = this.getScreenHeight();

    }

    LoadAllScreenData() {
        this.LoadRecentBankDeposits();
        this.SetQueriesVisibility();
        this.LoadTenantCurrency();
        this.LoadChartData();
        this.LoadQueriesCounts();

    }

    SetQueriesVisibility() {
        this.AllBankDepositsVisibility = FeatureLocator.HasFeaturePermession("BankDeposit", "BankDeposit.Q.AllBankDeposits") ? true : false;
        this.cashDepositsVisibility = FeatureLocator.HasFeaturePermession("BankDeposit", "CashBankDeposit") ? true : false;
        this.chequeDepositVisibility = FeatureLocator.HasFeaturePermession("BankDeposit", "ChequeBankDeposit") ? true : false;
        this.TodayDepositsVisibility = FeatureLocator.HasFeaturePermession("BankDeposit", "TodayBankDeposit") ? true : false;
        this.LoadBankPageMENUVisibility = FeatureLocator.HasFeaturePermession("ReconcileExternalPage", "LOADBANKPAGEMENU") ? true : false;
    }

    LoadQueriesCounts() {
        this._BankAccountExtendedListService.GetBankAccountsSummary().subscribe(myResult => {
            if (myResult != null) {
                this._BankAccountSummary.AllBankAccountsCount = myResult.AllBankAccountsCount > 1000 ? "1000+" : myResult.AllBankAccountsCount.toString();
            }
        });

        this.paymentChequeExtendedListService.GetPymentChequesSummary().subscribe(myResult => {
            if (myResult != null) {
                this.paymentChequeSummary.AllPaymenChequesCount = myResult.AllPaymentChequesCount > 1000 ? "1000+" : myResult.AllPaymentChequesCount.toString();
            }
        });

        this.myBankDepositService.GetBankDepositsSummary().subscribe(myResult => {
            if (myResult != null) {
                this.bankDepositSummary.TodaysDepositCount = myResult.TodaysDepositCount > 1000 ? "1000+" : myResult.TodaysDepositCount.toString();
            }
        });

        this.myCashBookExtendedListService.GetCashBookSummary().subscribe(myResult => {
            if (myResult != null) {
                this.cashBookSummary.AllCashbookCount = myResult.AllCashbookCount > 1000 ? "1000+" : myResult.AllCashbookCount.toString();
                this.cashBookSummary.CashCashbookCount = myResult.CashCashbookCount > 1000 ? "1000+" : myResult.CashCashbookCount.toString();

                this.cashBookSummary.ChequeCashbookCount = myResult.ChequeCashbookCount > 1000 ? "1000+" : myResult.ChequeCashbookCount.toString();



            }
        });
    }


    //#region Deposits

    public RecentBankDepositsList: BankDepositList[];
    LoadRecentBankDeposits() {
        this.RecentBankDepositsList = [];
        this.RecentBankDepositCount = 0;

        this.myBankDepositService.GetRecentBankDeposits().subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult = myResponse.Result;

                    this.RecentBankDepositsList = myResult;
                    this.RecentBankDepositCount = myResult.length;
                }
            }
        });
    }

    RunNewDepositWizard() {
        this.CurrentSession.StartBusyIndicatorLoading();
        var windowTitle = "New Deposit";
        var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewDeposit");

        var logWindow = new LogitudeWindow();
        logWindow.Width = 520;
        logWindow.Height = 230;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/NewEntity/NewBankDepositComponent');
        this.CurrentSession.StopBusyIndicator();

    }

    ViewDepositQuery(myQueryCode: string) {
        //chequeDeposit
        //cashDeposits
        //TodayDeposits

        if (myQueryCode != null) {

            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "chequeDeposit";
            var filters = new ApiQueryFilters();
            switch (myQueryCode) {
                case "TodayDeposits":
                    {
                        displayTitle = TextCodeTranslator.Translate("BankDeposit.Q.Today");

                        //var fromDate = new Date();
                        //fromDate.setHours(0, 0, 0, 0);
                        //var toDate = new Date();
                        //toDate.setHours(23, 59, 59, 59);
                        //console.log("Date Filter: ", fromDate, toDate);
                        //filters.addAdditionalFilter("AccountingDate", fromDate, toDate, null, "Between", false, false, false, "number");

                        break;
                    }
                case "cashDeposits":
                    {
                        displayTitle = TextCodeTranslator.Translate("BankDeposit.Q.cash");

                        break;
                    }
                case "chequeDeposit":
                    {
                        displayTitle = TextCodeTranslator.Translate("BankDeposit.Q.chequeDeposit");

                        break;
                    }
                case "AllBankDeposits":
                    {
                        displayTitle = TextCodeTranslator.Translate("Accounting.General.Q.AllDeposits");

                        break;
                    }

                default: { break; }
            }

            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "BankDeposit";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate('Accounting.General.O.Banks');
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

    EditBankDeposit(entity: any) {
        if (entity != null) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({
                        EntityId: entity.Id, ObjectTableName: 'BankDeposit', BackButtonLabel: TextCodeTranslator.Translate('Accounting.General.O.Banks') });
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                        this.RefreshButtonClicked();
                    });
                });
        }
    }


    //#endregion

    //#region Cashbooks
    RunNewCashBookWizard() {
        var windowTitle = "New Cashbook";
        var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewCashbook");
        var logWindow = new LogitudeWindow();
        logWindow.Width = 530;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/NewEntity/NewCashBookComponent');
    }

    ViewCashbookQuery(myQueryCode: string) {
        if (myQueryCode != null) {

            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "CashBooks";
            var filters = new ApiQueryFilters();
            switch (myQueryCode) {
                case "CashbookCash":
                    {
                        displayTitle = "Cash Cashbooks";
                        displayTitle = TextCodeTranslator.Translate("Accounting.General.O.Cash");

                        break;
                    }

                case "CashbookCheque":
                    {
                        displayTitle = "Cheque Cashbooks";
                        displayTitle = TextCodeTranslator.Translate("Accounting.General.O.Cheque");

                        break;
                    }

                case "CashBooks":
                    {
                        displayTitle = "All Cashbooks";
                        displayTitle = TextCodeTranslator.Translate("Accounting.General.O.AllCashbook");

                        break;
                    }

                default: { break; }
            }

            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "CashBook";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate('Accounting.General.O.Banks');
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

    EditBank(entity: any) {
        if (entity != null) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'CashBook', BackButtonLabel: TextCodeTranslator.Translate('Accounting.General.O.Banks') });
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                        this.RefreshButtonClicked();
                    });
                });
        }
    }

    //#endregion

    //#region BankAccounts

    RunNewBankAccountWizard() {
        var windowTitle = "New Bank Accounts";
        var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewBankAccounts");

        var logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 500;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/NewEntity/NewBankAccountComponent');
    }
    LoadBankPagesFromFile() {
        var logWindow = new LogitudeWindow();
        logWindow.IsShowCloseButton = true;
        logWindow.Width = 500;
        logWindow.Height = 400;
        logWindow.Title = TextCodeTranslator.Translate("Accounting.General.O.BankPagesFromFile");
        logWindow.WindowArgs = {};
        logWindow.WindowClosed.subscribe(($event: any) => {

        });
        logWindow.Show('./Accounting/Components/NewEntity/LoadRecoExPageComponent');

    }
    ViewBankAccountQuery(myQueryCode: string) {
        if (myQueryCode != null) {

            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "AllBankAccounts";
            var filters = new ApiQueryFilters();
            switch (myQueryCode) {
                case "AllBankAccounts":
                    {
                        displayTitle = "Bank Accounts";
                        displayTitle = TextCodeTranslator.Translate("Accounting.General.O.AllBankAccounts");

                        break;
                    }

                default: { break; }
            }

            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "BankAccount";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate('Accounting.General.O.Banks');
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
    //#endregion

    //#region payment cheques

    ViewPaymentChequesQuery(myQueryCode: string) {
        if (myQueryCode != null) {

            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "AllPaymentCheques";
            var filters = new ApiQueryFilters();
            switch (myQueryCode) {
                case "AllPaymentCheques":
                    {
                        displayTitle = "Payment Cheques";
                        displayTitle = TextCodeTranslator.Translate("Accounting.General.O.AllPaymentCheques");

                        break;
                    }

                default: { break; }
            }

            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "PaymentCheque";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate('Accounting.General.O.Banks');
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
    NewPaymentCheque() {
        var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewPaymentCheque");

        var logWindow = new LogitudeWindow();
        logWindow.Width = 650;
        logWindow.Height = 500;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/NewEntity/NewPaymentChequeComponent');
    }

    //#endregion

    // Load Tenant Currency
    tenantCurrencyCode: string;
    tenantCurrencyId: string;
    ratesTable: RatesTableList[];

    LoadTenantCurrency() {
        // Tenant currency
        var defaultCurrencyId: string = SessionLocator.TenantPM.CurrencyId;
        this.currencyListService.getSingle(defaultCurrencyId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var currency = myResponse.Result;
                    if (currency != undefined || currency != null) {
                        this.tenantCurrencyCode = currency.Code;
                        this.tenantCurrencyId = currency.Id;

                        // Tenant currency rates
                        var filters = new ApiQueryFilters(true);
                        filters.addAdditionalFilter("BaseCurrencyId", this.tenantCurrencyId, null, null, "Equals", false, false, false, "string");

                        this.ratesTableListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
                            if (myResponse != null) {
                                if (!myResponse.HasError) {
                                    var res = myResponse.Result;
                                    if (res != undefined || res != null)
                                        this.ratesTable = res;
                                    this.LoadChartData();
                                }
                            }
                        });
                    }

                }
            }
        });


    }

    ConvertToLocal(foreignAmount: number, currencyId) {
        if (!AppTool.IsNullOrEmpty(foreignAmount)) {

            // if no currency
            if (AppTool.IsNullOrEmpty(currencyId))
                return foreignAmount;

            // calculation
            if (!AppTool.IsNullOrEmpty(this.ratesTable)) {
                var rateRow = this.ratesTable.find(d => d.ForeignCurrencyId == currencyId);
                if (rateRow && rateRow != null) {
                    return rateRow.Rate * foreignAmount;
                } else {
                    return 0;
                }
            } else {
                return 0;
            }

        } else {
            return 0;
        }
    }

    //#region Bar Chart Code
    CashBooks: CashBookList[];

    public barChartLabels: string[] = [];
    public barChartData: any[] = [{
        cash: [],
        postdated: [],
        label: '',
        scaleShowVerticalLines: false
    }];


    LoadChartData() {
        this.myCashBookExtendedListService.GetCashBookStatusChartData().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var res = myResponse.Result;
                if (res != null && res.length > 0) {
                    console.log(res);
                    this.LoadChart(this.OrganizeData(res));
                }
            }
        });
    }

    OrganizeData(data: CashBookStatusChart[]) { //Calculate Others Column
        //data = data.slice(0, 2); // Draw only first two columns

        var oData: any[];
        var takeNumber = 5; // Number of columns to show in the graph , execlude "Others" column

        if (!AppTool.IsNullOrEmpty(data)) {

            if (data.length > takeNumber) {

                // 1-Take first section
                oData = data.slice(0, takeNumber);

                // 2-Calculate "Others" Column
                var othersColumn = new CashBookList();
                othersColumn.TotalAmount = 0;
                othersColumn.CashBookTypeCode = "-1"; // manual entry "Others"
                for (var i = takeNumber; i < data.length; i++) {
                    var amount = data[i].TotalAmount;
                    amount = this.ConvertToLocal(amount, data[i].CurrencyId);
                    othersColumn.TotalAmount += ((AppTool.IsNullOrEmpty(amount)) ? 0 : amount);
                }
                oData.push(othersColumn);
            } else {
                return data
            }
        }

        // 3-Return data
        return oData;
    }

    LoadChart(data: CashBookStatusChart[]) {

        var useLocal = !SessionLocator.LoggedUserPM.DontShowLocal;

        //#region Graph metadata
        var max = 0;
        var DataProvider = [];
        var i = 0;
        var index = 0;
        this.barChartData[0].data = [];
        this.barChartLabels = [];
        this.barChartData = [{
            cash: [],
            postdated: [],
            label: '',
            scaleShowVerticalLines: false
        }];

        // Graph Properties
        var Graphs = Graphs =
        //#endregion


        data.forEach(element => {
            if (element.TotalAmount <= 0) return;
            this.barChartData[0].label = useLocal ? "סכום" : "Amount";
            console.log("[chart]", useLocal);

            // Amount
            var amount = this.ConvertToLocal(element.TotalAmount, element.CurrencyId)
            var cash = element.CashBookTypeCode == "1" ? element.TotalAmount : element.CashTotal;
            var postdated = element.CashBookTypeCode == "1" ? 0 : element.PostdatedTotal;

            var isCheque = false;
            // Labels
            var name = "";
            if (element.CashBookTypeCode == "1") { // 1-cash
                name = (useLocal ? "מזומן" : "Cash") + " (" + element.CurrencyCode + ")";
            } else if (element.CashBookTypeCode == "2") { // 2-cheque
              name = (useLocal ? "המחאות" :"Cheque") + " (" + element.CurrencyCode + ")";
                isCheque = true;
            } else {
                name = useLocal ? "המחאה" :"Others";
            }

            // Data
            DataProvider[i] = {
                "Name": name,
                "CashCol": cash == null ? 0 : cash.toFixed(2),
                "PostdatedCol": postdated == null ? 0 : postdated.toFixed(2),
                "subtitle": isCheque ? "[[title]] : " : "",
                "entityId": (element.CashBookTypeCode == "-1" ? "otherCol" : element.Id),
                "currency": SessionLocator.TenantPM.CurrencySign,
            };

            i++;
        });
        var poisition = this.isRTL == true ? "right" : "left";

        makeBanksBarChart(this.chartId, DataProvider, useLocal, poisition);

    }

    BarClicking() {
        if (BarClick() != null) {
            this.OnBarClick(BarClick());

        }

    }

    OnBarClick(e) {
        //console.log(e);
        if (!AppTool.IsNullOrEmpty(e)) {
            var itemData = e.item.dataContext;

            var id = itemData["entityId"];

            if (id == "otherCol") { // open all cashbook query
                var listArgs = new ListComponentArgs();
                listArgs.QueryCode = 'CashBooks';
                listArgs.Filters = new ApiQueryFilters();
                listArgs.ObjectTableName = "CashBook";
                listArgs.DisplayTitle = TextCodeTranslator.Translate("Accounting.General.O.AllCashbook");
                listArgs.BackButtonTitle = TextCodeTranslator.Translate('Accounting.General.O.Banks');
                this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run(listArgs);
                            cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                            this.CurrentSession.AddMenuReference(cmpRef);
                        });
                });
            } else {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({
                            EntityId: id,
                            ObjectTableName: 'CashBook',
                            BackButtonLabel: TextCodeTranslator.Translate('Accounting.General.O.Banks')
                        });
                    });
            }

        }
    }



    //#endregion

    getScreenHeight() {
        if (self.innerHeight) {
            return self.innerHeight;
        }

        if (document.documentElement && document.documentElement.clientHeight) {
            return document.documentElement.clientHeight;
        }

        if (document.body) {
            return document.body.clientHeight;
        }
    }
    getScreenWidth() {
        if (self.innerWidth) {
            return self.innerWidth;
        }

        if (document.documentElement && document.documentElement.clientWidth) {
            return document.documentElement.clientWidth;
        }

        if (document.body) {
            return document.body.clientWidth;
        }
    }
}
