declare var makeAmBarChart;
declare var window: any;
import { Component, Output, EventEmitter, OnInit, AfterViewInit } from '@angular/core';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { InfraSettings } from '../../../../Infrastructure/Utilities/InfraSettings';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CurrencyRatesService, LastRate} from "../../../../Common/Services/CurrencyRatesService";

// Services
import { GLAccountExtendedListService } from '../../../Services/ExtendedLists/GLAccountExtendedListService';
import { GLAccountTotalByMonthListService } from '../../../Services/StandardLists/GLAccountTotalByMonthListService';
import { FullAccountingSettingListService } from '../../../Services/StandardLists/FullAccountingSettingListService';

// Lists
import { GLAccountList } from '../../../EntityLists/GLAccountList';
import { FullAccountingSettingList } from '../../../EntityLists/FullAccountingSettingList';
import { GLAccountTotalByMonthList } from '../../../EntityLists/GLAccountTotalByMonthList';

import { GLAccountSummary } from '../../../DataContracts/AccountingSummery';
import { AgingReportParameters } from '../../../DataContracts/AgingReportParameters';
import { PeriodM } from '../../../DataContracts/PeriodM';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { ModulesService } from '../../../Services/ModulesService';
import { GLAccountSecurityLevelService } from 'Accounting/Utilities/GLAccountSecurityLevelService';
import { ARPaymentPM } from 'Invoice/EntityPMs/ARPaymentPM';
import { ARInvoicePM } from 'Invoice/EntityPMs/ARInvoicePM';
import {CurrencyList} from "../../../../Common/EntityLists/CurrencyList";
import {CurrencyListService} from "../../../../Common/Services/StandardLists/CurrencyListService";
import {InvoiceTool} from "../../../../Invoice/Tools";
import { PartnersDomainService } from 'Common/Services/PartnersDomainService';


@Component({

    templateUrl: './ReceivablePageComponent.html',
})

export class ReceivablePageComponent {
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private _GLAccountExtendedListService: GLAccountExtendedListService = new GLAccountExtendedListService();
    private _GLAccountTotalByMonthListService: GLAccountTotalByMonthListService = new GLAccountTotalByMonthListService();
    private _FullAccountingSettingListService: FullAccountingSettingListService = new FullAccountingSettingListService();
    private myCurrencyListService: CurrencyListService = new CurrencyListService();
    private LastRatesList: LastRate[] = [];
    glAccountSummary: GLAccountSummary = new GLAccountSummary();

    //#region Queries + Counts
    collectorsGLAVisibility: boolean = false;
    debetorsGLAVisibility: boolean = false;
    activeCustomersGLAVisibility: boolean = false;
    inactiveCustomersGlaVisibility: boolean = false;
    CLIENTGLACCOUNTSGlaVisibility: boolean = false;

    //Counts
    public ARInvoicesDraftsCount: string;
    public ARInvoicesUnpaidCount: string;
    public ARInvoicesOpenConstituentCount: string;
    public ARPaymentsDraftsCount: string;
    public ARPaymentsOpenedCount: string;
    public ARGeneralInvoiceDraftCount: string;
    //#endregion
    IsNewCreditNoteVisibile: boolean = false;
    RecentGLAccountsCount: number = 0;

    public isRTL: boolean = false;
    isReady: boolean = false;
    IsNewARInvoiceEnabled: boolean = false;

    chartId: string = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.chartId = "Receivable_" + this.CurrentSession.GetChartId();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this.LoadResources();

        this.IsNewARInvoiceEnabled = FeatureLocator.HasFeaturePermession("ARInvoice", "NEW");


        //this.LoadAllScreenData();


        //this.SelectedFilter = "Last 6 Months";
        this.SelectedFilter = this.FiltersList[1];
        this.PopulateDeptorsFilterData();
    }
    LoadResources() {
        this._entityResourceService.getEntityResourceByTableName("ARPayment").subscribe((response: any) =>
        {
            this._entityResourceService.getEntityResourceByTableName("ARInvoice").subscribe((response: any) =>
            {
                this._entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) =>
                {
                    this._entityResourceService.getEntityResourceByTableName("LedgerTransaction").subscribe((response: any) =>
                    {
                        this._entityResourceService.getEntityResourceByTableName("Reconciliation").subscribe((response: any) =>
                        {
                            this._entityResourceService.getEntityResourceByTableName("ExternalReconciliation").subscribe((response: any) =>
                            {
                                this._entityResourceService.getEntityResourceByTableName("AccountingNote").subscribe((response: any) =>
                                {
                                    this._entityResourceService.getEntityResourceByTableName("InterestTransaction").subscribe((response: any) =>
                                    {
                                        this._entityResourceService.getEntityResourceByTableName("InterestReport").subscribe((response: any) =>
                                        {
                                            this.isReady = true;
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    }
    InitComponent() {
        this.LoadAllScreenData();
        this.SetQueriesVisibility();
    }

    //#region Customers
    RunNewCustomerWizard() {
        //var windowTitle = "New Customer";

        //var logWindow = new LogitudeWindow();
        //logWindow.Width = 700;
        //logWindow.Height = 500;
        //logWindow.Title = windowTitle;
        ////logWindow.WindowArgs = windowArgs;
        //logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        //logWindow.Show('./Accounting/Components/NewEntity/NewCashBookComponent');
    }
    ViewCustomerQuery(myQueryCode: string) {
        if (myQueryCode != null) {

            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "allcustomerquery";
            var filters = new ApiQueryFilters();
            switch (myQueryCode) {
                // MyCustomersAsCollectors
                // DebetorsCustomers
                // ActiveCustomersGLAccounts
                // InactiveCustomersGLAccount
                case "MyCustomersAsCollectors":
                {
                    displayTitle = "My Customers (As Collectors)";
                    displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.Collectors");

                    break;
                }

                case "DebetorsCustomers":
                {
                    displayTitle = "Debtors Customers";
                    displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.debetors");

                    break;
                }
                case "ActiveCustomersGLAccounts":
                {
                    displayTitle = "Active Customers";
                    displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.ActiveCustomers");

                    break;
                }
                case "InactiveCustomersGLAccount":
                {
                    displayTitle = "Inactive Customers";
                    displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.InactiveCustomers");

                    break;
                }
                case "All Customers":
                {
                    displayTitle = "All Customers";
                    displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.AllCustomers");

                    break;
                }



                default: { break; }
            }

            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "GLAccount";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate("Accounting.General.O.Receivables");
            listArgs.Perspective = "GLAccountRecievable";
            listArgs.IgnoreSelectedPerspective = true;
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

    //#region ARPayments
    NewARPaymentMethod() {
        var FinalText = TextCodeTranslator.Translate("ARPayment.O.New");
        if (SessionLocator.TenantPM.AccountingActivated)
        {
            var entity = new ARPaymentPM();
            entity.IsFullAccounting = true;
            entity.BranchId = SessionLocator.LoggedUserPM?.BranchId;

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: "", EntityPM: entity, ObjectTableName: 'ARPayment' });
                });
        }
        else
        {

            // var FinalText = this.getAutoNewName();
            var logWindow = new LogitudeWindow();
            logWindow.Title = FinalText;
            logWindow.Width = 900;
            logWindow.Height = 570;
            logWindow.Show("./InvoiceModules/ARPayment/Components/NewEntity/NewARPaymentComponent");
            logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
            //SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            //    .then(cmpRef => {
            //        cmpRef.instance.ComponentRef = cmpRef;
            //        cmpRef.instance.Run({ EntityId: "", EntityPM: new ARPaymentPM(), ObjectTableName: 'ARPayment' });
            //    });
        }
    }   
    filterAgrs: ApiQueryFilters;
    private getAutoNewName() {
        var GeneralText = TextCodeTranslator.Translate("General.O.NewEntity");
        var ChangedText = GeneralText.split('%')[0];
        var NewText = TextCodeTranslator.TranslateTable("ARPayment");
        var showlocal = !SessionLocator.LoggedUserPM.DontShowLocal;
        var FinalText = showlocal ? (NewText + " " + ChangedText) : (ChangedText + " " + NewText);
        return FinalText;
    }

    ViewInvoiceQuery(args: string) {
        if (args != null) {

            var backButtonTitle = "Accounting";
            var objectTableName = args.split(':')[0];
            var queryCode = args.split(':')[1];
            var displayTitle = queryCode;
            this.filterAgrs = new ApiQueryFilters();

            var ObjectTable = window.ObjectTables.filter(x => x.Name === objectTableName)[0];
            var query = window.Queries.filter(q => q.ObjectTableId == ObjectTable.Id && q.Code == queryCode)[0];

            if (window.PreDefinedFilters.filter(d => d.queryCode == query.Code) != null) {
                var predefinedFilters = window.PreDefinedFilters.filter(d => d.queryCode == query.Code);

                predefinedFilters.forEach((filter, key) => {
                    var filterOperator = (!AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                    var value1 = filter.PredefinedValue;
                    var value2 = filter.PredefinedValue2;
                    if (value2 != null) {
                        filterOperator = "Between";
                    }
                    this.filterAgrs.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, filter.IsCustomFilter, filter.DisplayInList, false, filter.DataTypeCode);
                });
            }

            this.filterAgrs.ObjectTableName = query.ObjectTableName;

            var listArgs = new ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate("Accounting.General.O.Receivables");
            //listArgs.DisplayTitle = displayTitle;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        this.CurrentSession.AddMenuReference(cmpRef);
                    });
            });
        }
    }
    //#endregion

    //#region General ARInvoice

    private GetCurrenciesExchangeRateByValueDate(entity: ARInvoicePM){
        var myCurrencyRatesService = new CurrencyRatesService();
        var loadingDate = DateTool.GetCurrentDateAsUtc();

        entity.ProfitCurrencyId = SessionLocator.TenantPM.ProfitCurrencyId;

        myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator.LocalCurrencyId, loadingDate).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.LastRatesList = myResponse.Result;
                this.InitializeProfitCurrency(entity);
            }

            else {
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    private InitializeProfitCurrency(entityPM: ARInvoicePM) {

        if (AppTool.IsNullOrEmpty(entityPM.ProfitCurrencyId)) {
            entityPM.ProfitCurrencyId = SessionLocator.TenantPM.ProfitCurrencyId;
        }

        this.myCurrencyListService.getSingleFromCache(entityPM.ProfitCurrencyId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: CurrencyList = myResponse.Result;
                if (list != null) {
                    entityPM.ProfitCurrencyCode = list.Code;
                }
            }
        });

        entityPM.ProfitCurrencyExchangeRate = this.GetCurrencyRate(entityPM.ProfitCurrencyId);
        this.OpenEditComponent(entityPM);
    }


    OpenEditComponent(entity: ARInvoicePM)
    {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {

                var todayDate = DateTool.GetCurrentDateAsUtc();

                entity.IsGeneralInvoice = true;
                entity.IssuedByUserId = SessionLocator.LoggedUserId;
                entity.CreatedByUserId = SessionLocator.LoggedUserId;
                entity.UpdatedByUserId = SessionLocator.LoggedUserId;
                entity.CreateDate = todayDate;
                entity.UpdateDate = todayDate;
                entity.InvoiceDate = todayDate;
                entity.LocalCurrencyId = SessionLocator.LocalCurrencyId;

                entity.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
                entity.Tenant = SessionLocator.TenantPM.Id;
                entity.ProfitCurrencyExchangeRate = this.GetCurrencyRate(entity.ProfitCurrencyId);
                entity.PaymentTermId = SessionLocator.TenantPM.PaymentTermId;
                entity.InvoiceCurrencyId = SessionLocator.TenantPM.CurrencyId;
                entity.InvoiceCurrencyExchangeRate = this.GetCurrencyRate(entity.InvoiceCurrencyId);
                InvoiceTool.ComputeARInvoiceDueDate(entity);

                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityPM: entity, ObjectTableName: 'ARInvoice' });
            });
    }

    public NewGeneralARInvoice(type: string) {

        if (SessionLocator.TenantPM.AccountingActivated ) {
            var entity = new ARInvoicePM();
            entity.ARInvoiceTypeCode = type;
            entity.BranchId = SessionLocator.LoggedUserPM?.BranchId;
            entity.PrintNotes = TextCodeTranslator.Translate("ARInvoice.O.Invoice");
            this.GetCurrenciesExchangeRateByValueDate(entity);
            return;
        }
      


        //var str = TextCodeTranslator.Translate("General.O.NewEntity");
        //str = str.replace("%Entity", "General Invoice");
        var str_NewGeneralInvoice = TextCodeTranslator.Translate("Accounting.General.O.NewGeneralInvoice");
        var str_NewCreditNote = TextCodeTranslator.Translate("Accounting.General.O.NewCreditNote");

        var windowTitle = (type == 'IN' ? str_NewGeneralInvoice : str_NewCreditNote);
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { InvoiceTypeCode: type };
        logWindow.Title = windowTitle;
        logWindow.Width = 550;
        logWindow.Height = 500;

        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ EntityPM: comp.EntityPM, ObjectTableName: 'ARInvoice' });
                            cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                        });
                }
            });
        });
        //logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show("./InvoiceModules/ARInvoice/Components/NewEntity/NewGeneralARInvoiceComponent");
    }

    GetCurrencyRate(currencyId: string) {
        var myResult: number = null;

        if (!AppTool.IsNullOrEmpty(currencyId)) {
            if (currencyId == SessionLocator.TenantPM.CurrencyId) {
                myResult = 1;
            }

            else {
                var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == currencyId)[0];
                if (lastRate != null) {
                    myResult = lastRate.Rate;
                }
            }
        }

        return myResult;
    }
    //#endregion

    RefreshButtonClicked() {
        this.LoadAllScreenData();
    }

    LoadAllScreenData() {
        this.LoadDefaultValues();
        this.LoadRecentGLAccounts();
        this.LoadQueriesCounts();
        this.LoadChartData();
        this.LoadDebtors();
    }

    SetQueriesVisibility() {
        this.collectorsGLAVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "collectorsGLA") ? true : false;
        this.debetorsGLAVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "debetorsGLA") ? true : false;
        this.activeCustomersGLAVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "activeCustomersGLA") ? true : false;
        this.inactiveCustomersGlaVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "inactiveCustomersGla") ? true : false;
        this.CLIENTGLACCOUNTSGlaVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "CLIENTGLACCOUNTS") ? true : false;
        this.IsNewCreditNoteVisibile = FeatureLocator.HasFeaturePermession("ARInvoice", "NEWCREDITNOTE") ? true : false;
    }

    public RecentGLAccountsList: GLAccountList[];
    LoadRecentGLAccounts() {
        this.RecentGLAccountsList = [];
        this.RecentGLAccountsCount = 0;

        this._GLAccountExtendedListService.GetRecentGLAccounts("2").subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult = myResponse.Result;

                    this.RecentGLAccountsList = myResult;
                    this.RecentGLAccountsCount = myResult.length;
                }
            }
        });
    }

    LoadQueriesCounts() {
        this._GLAccountExtendedListService.GetGLAccountsSummary().subscribe((myResult:GLAccountSummary) => {
            if (myResult != null) {
                this.glAccountSummary.CollectorsCount = myResult.CollectorsCount > 1000 ? "1000+" : myResult.CollectorsCount.toString();
                this.glAccountSummary.DebitorsCount = myResult.DebitorsCount > 1000 ? "1000+" : myResult.DebitorsCount.toString();
            }
        });

    }

    EditGLAccount(entity: any) {
        if (entity != null) {

            GLAccountSecurityLevelService.CheckLevel(entity.Id).then(hasAccess =>
            {
                if (hasAccess)
                    this.OpenGLAccountEditWindow(entity);
                else
                    GLAccountSecurityLevelService.ShowSecurityBockingMessage();
            });
        }
    }

    private OpenGLAccountEditWindow(entity: any)
    {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef =>
            {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'GLAccount', BackButtonLabel: TextCodeTranslator.Translate("Accounting.General.O.Receivables") });
                cmpRef.instance.BackCompleted.subscribe(($event: any) =>
                {
                    this.RefreshButtonClicked();
                });
            });
    }

    Abs(number: number) {
        return number < 0 ? number * -1 : number;
    }

    accSettings: FullAccountingSettingList;
    LoadDefaultValues() {

        // Full Accounting Settings
        this._FullAccountingSettingListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var res = myResponse.Result;
                if (res != null && res.length > 0) {
                    var list: FullAccountingSettingList[];
                    list = res;
                    this.accSettings = list[0]; // because there is only one record for each tenant
                    console.log("FullAccountingSettingList: ", list);
                    this.LoadChartData();
                }
            }
        });

    }

    //#region Chart Code

    GLAccounts: GLAccountList[];

    public barChartLabels: string[] = [];
    public barChartData: any[] = [{
        data: [], label: '', scaleShowVerticalLines: false,
    }];


    LoadChartData() {
        if (AppTool.IsNullOrEmpty(this.accSettings)) return;
        var numberOfmonthsbackwards = 6;
        switch (this.SelectedFilter.EnglishName) {
            case 'Last 3 Month': {
                numberOfmonthsbackwards = 3;
                break;
            }
            case 'Last 6 Month': {
                numberOfmonthsbackwards = 6;
                break;
            }
            case 'Last 9 Month': {
                numberOfmonthsbackwards = 9;
                break;
            }

        }

        var args = new AgingReportParameters();

        args.Tenant = SessionLocator.Tenant,
            args.AgingForDate = new Date();
        args.NumberOfmonthsbackwards = numberOfmonthsbackwards == null ? 3 : numberOfmonthsbackwards;
        args.VendorCustomerId = AppTool.IsNullOrEmpty(this.accSettings) ? "" : this.accSettings.CustomerControlAccountId;
        //args.Category1Id = "";
        //args.Category2Id = "";
        //args.Category3Id = "";
        //args.Category4Id = "";
        //args.Category5Id = "";
        //args.CollectorId = SessionLocator.LoggedUserId;
        //args.SalesmanId = "";
        args.IsCustomer = false;
        args.ForceUseMonthMethod = true;


        this._GLAccountExtendedListService.GetAgingReport(args).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var res = myResponse.Result;
                if (res != null && res.length > 0) {
                    var periods: PeriodM[];
                    periods = res;
                    console.log("res: ", periods);
                    this.LoadChart(periods);
                }
            }
        });

    }

    OrganizeData(data: GLAccountList[]) {

        //var oData: any[];
        //var takeNumber = 5; // Number of columns to show in the graph , execlude "Others" column

        //if (!AppTool.IsNullOrEmpty(data)) {

        //    if (data.length > takeNumber) {

        //         1-Take first section
        //        oData = data.slice(0, takeNumber);

        //         2-Calculate "Others" Column
        //        var othersColumn = new GLAccountList();
        //        othersColumn.TotalAmount = 0;
        //        othersColumn.GLAccountTypeCode = "-1"; // manual entry "Others"
        //        for (var i = takeNumber; i < data.length; i++) {
        //            var amount = data[i].TotalAmount;
        //            amount = this.ConvertToLocal(amount, data[i].CurrencyId);
        //            othersColumn.TotalAmount += ((AppTool.IsNullOrEmpty(amount)) ? 0 : amount);
        //        }
        //        oData.push(othersColumn);
        //    } else {
        //        return data
        //    }
        //}

        // 3-Return data
        //return oData;
    }

    LoadChart(data: PeriodM[]) {

        //#region Graph metadata
        var max = 0;
        var DataProvider = [];
        var i = 0;
        var index = 0;
        this.barChartData[0].data = [];
        this.barChartLabels = [];
        this.barChartData = [{
            data: [], label: '', scaleShowVerticalLines: false,
        }];

        //Graph Properties
        var Graphs = Graphs = [{
            "balloonText": "Amount: <br>[[value]]",
            "fillAlphas": 1,
            "id": "AmGraph-1" + i,
            "title": "Aging",
            "type": "column",
            "valueField": "dataCol",
            "fillColors": ["#BADFE8", "#7AC2D4", "#73BFD2", "#7AC2D4", "#BADFE8",],
            "gradientOrientation": "horizontal",
            "borderAlpha": 0,
            "lineColor": "#fff",
            "fixedColumnWidth": 70,


        }]
        //#endregion

        data.forEach(element => {
            //if (element.Total <= 0) return;
            this.barChartData[0].label = "Amount";

            // Amount
            var value = element.Total;// + (Math.floor((Math.random() * 2500) + 1));
            this.barChartData[0].data[i] = value.toString();

            // Labels
            var label = element.PeriodName.replace("b4", !SessionLocator.LoggedUserPM.DontShowLocal ? "עד" : "Before"); // replace 'b4' with 'Before'
            // var label = element.PeriodName.replace("b4", "Before"); // replace 'b4' with 'Before'
            label = label.startsWith("Before") ? label.replace("/20", "/") : label; // minimize year in 'Before' Column
            this.barChartLabels[i] = label;

            // Data
            DataProvider[i] = { "category": this.barChartLabels[i], "dataCol": this.barChartData[0].data[i] };

            if (value > max)
                max = value;


            i++;
        });

        makeAmBarChart(this.chartId, Graphs, DataProvider, max);

    }

    //#endregion

    //#region Filters Code

    monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    // aging chart
    //public FiltersList: string[] = [ 'Last 3 Month',
    //                                'Last 6 Month',
    //                                'Last 9 Month'];


    public FiltersList: any[] =
        [
            { EnglishName: 'Last 3 Month', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LastXMonth").replace("#number", "שלושה") },
            { EnglishName: 'Last 6 Month', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LastXMonth").replace("#number", "שישה") },
            { EnglishName: 'Last 9 Month', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LastXMonth").replace("#number", "תשעה") }
        ];


    private selectedFilter: any;
    get SelectedFilter() { return this.selectedFilter; }
    set SelectedFilter(value: any) {
        if (this.selectedFilter != value) {
            this.selectedFilter = value;
            this.LoadChartData();
        }
    }

    // Deptors
    public DeptorsFiltersList = [];
    private selectedDeptorsFilter: any;
    get SelectedDeptorsFilter() { return this.selectedDeptorsFilter; }
    set SelectedDeptorsFilter(value: any) {
        if (this.selectedDeptorsFilter != value) {
            this.selectedDeptorsFilter = value;
            this.LoadDebtors();
        }
    }
    PopulateDeptorsFilterData() {

        //this.DeptorsFiltersList = ['Accounting Balance', 'Balance Due'];

        this.DeptorsFiltersList =
            [{ EnglishName: 'Accounting Balance', LocalName: TextCodeTranslator.Translate("Accounting.General.O.AccountingBalance") },
                { EnglishName: 'Balance Due', LocalName: TextCodeTranslator.Translate("Accounting.General.O.BalanceDue") }];

        this.SelectedDeptorsFilter = this.DeptorsFiltersList[1];
    }


    //#endregion

    //#region Top 10 Deptors Graph
    TopDebtorsList: GLAccountList[];

    LoadDebtors() {

        //// Get month number from name
        //if (this.SelectedDeptorsFilter == undefined) {
        //    var monthNumber = new Date().getMonth() + 1;
        //} else {
        //    var op = this.SelectedDeptorsFilter.split(' ');
        //    var monthName = op[0];
        //    var monthNumber = this.monthNames.indexOf(monthName) + 1;
        //}

        var monthNumber = new Date().getMonth() + 1;
        var year = new Date().getFullYear();

        if (AppTool.IsNullOrEmpty(this.SelectedDeptorsFilter)) {
            //this.SelectedDeptorsFilter = "Balance Due";
            this.SelectedDeptorsFilter = this.DeptorsFiltersList[1];

        }

        if (this.SelectedDeptorsFilter) {
            this._GLAccountExtendedListService.GetTopDeptors(this.SelectedDeptorsFilter.EnglishName, '2').subscribe((myResponse: ServiceResponse) => { //2-Client
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var myResult: GLAccountList[] = myResponse.Result;

                        this.TopDebtorsList = myResult;

                        console.log("DEPTORS: ", myResult);
                    }
                }
            });
        }
    }

    DeptorsClick(entity: GLAccountList) {
        //if (entity != null) {
        //    var windowArgs: any = {};
        //    windowArgs.accountId = entity.AccountId;
        //    windowArgs.cardId = entity.CardId;
        //    windowArgs.entity = entity;

        //    var windowTitle = "Aging For Customer";
        //    var logWindow = new LogitudeWindow();
        //    logWindow.Width = 550;
        //    logWindow.Height = 400;
        //    logWindow.WindowArgs = windowArgs;
        //    logWindow.Title = windowTitle;
        //    logWindow.ShowCloseButton = true;
        //    logWindow.Show("./Accounting/Components/Others/Aging4CustomerChartWindowComponent");
        //}
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

    getLocalCurr() {
        return SessionLocator.TenantPM.CurrencySign;
    }
}
