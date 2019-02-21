import { ReconcileEventManager } from './../../../../Accounting/Utilities/ReconcileEventManager';
import { AccountingEntityHelper } from './../../../../Accounting/Utilities/AccountingEntityHelper';
import { LedgerTransactionPM } from './../../../../Accounting/EntityPMs/LedgerTransactionPM';
import { LedgerTransactionExtendedListService } from './../../../../Accounting/Services/ExtendedLists/LedgerTransactionExtendedListService';
import { RegionList } from './../../../../Common/EntityLists/RegionList';
import { EventEmitter, Output } from '@angular/core';
import { EntityListService } from './../../../../Infrastructure/Services/EntityListService';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ARPaymentPM } from '../../../../Invoice/EntityPMs/ARPaymentPM';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool, DateTool, ArrayTool } from '../../../../Infrastructure/Tools';
import { CurrencyListService } from '../../../../Common/Services/StandardLists/CurrencyListService';
import { CurrencyList } from '../../../../Common/EntityLists/CurrencyList';
import { CardList } from '../../../../Common/EntityLists/CardList';
import { AddressList } from '../../../../Common/EntityLists/AddressList';
import { AddressListService } from '../../../../Common/Services/StandardLists/AddressListService';
import { CardListService } from '../../../../Common/Services/StandardLists/CardListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ARInvoiceListService } from '../../../../Invoice/Services/StandardLists/ARInvoiceListService';
import { ARInvoiceList } from '../../../../Invoice/EntityLists/ARInvoiceList';
import { ARPaymentInvoicePM } from '../../../../Invoice/EntityPMs/ARPaymentInvoicePM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { CurrencyRatesService, LastRate } from '../../../../Common/Services/CurrencyRatesService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { PartnersDomainService } from '../../../../Common/Services/PartnersDomainService';
import { BankAccountPMService } from '../../../../Accounting/Services/StandardPMs/BankAccountPMService';
import { BankAccountPM } from '../../../../Accounting/EntityPMs/BankAccountPM';
import { InvoiceDomainService } from '../../../../Invoice/Services/InvoiceDomainService';
import { CashBookPM } from '../../../../Accounting/EntityPMs/CashBookPM';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { AccountingPaymentMethodList } from '../../../../Invoice/EntityLists/AccountingPaymentMethodList';
import { AccountingPaymentMethodListService } from '../../../../Invoice/Services/StandardLists/AccountingPaymentMethodListService';
import { GLAccountPMService } from '../../../../Accounting/Services/StandardPMs/GLAccountPMService';
import { GLAccountPM } from '../../../../Accounting/EntityPMs/GLAccountPM';
import { LineModel } from '../../../../Accounting/Components/Others/ReconcileComponent';

@Component({
    moduleId: module.id,
    templateUrl: './ARPaymentDetailsFullAccountingTab.html',
    styleUrls: ['./ARPaymentDetailsFullAccountingTab.css']
})

export class ARPaymentDetailsFullAccountingTab extends BaseComponent implements OnInit, OnDestroy {


    public TransactionsList: ObservableCollection;


    public EntityPM: ARPaymentPM;
    public glAccount: GLAccountPM;
    public ObjectTableName = "ARPayment";
    public DataContext = this;
    public ItemsSource: ObservableCollection;
    public FullAccounting: boolean = false;
    public DisplaySATSettings: boolean = false;
    public IsMultiCurrency: boolean = false;
    public TransferStatusVisibilityColumn: boolean = false;
    public EnableNegativeOffsetARPayments: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    get IsNegativeAmountEnabled() { return this.EnableNegativeOffsetARPayments == true && this.AccountingPaymentMethodCode == "FS" ? true : false; }
    public isRTL: boolean = false;
    public ARPaymentChequeStatus = "";
    public ARPaymentChequeStatusColor = "black";

    _LedgerTransactionExtendedListService: LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService();

    constructor(private entityArgs: EntityArgs, private _entityResourceService: EntityResourceService) {
        super();
        console.log("[FULL ACCOUNING ARPayment]");

        if (ObjectsLocator.GlobalSetting) {
            this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }

        this.EntityPM = entityArgs.EntityPM;
        this.FullAccounting = SessionLocator.TenantPM.AccountingActivated;
        this.originalPaymentOpenAmount = this.EntityPM.OpenAmount;
        this.paymentAmountTotal = this.EntityPM.AmountInPaymentCurrency;
        this.TransactionsList = new ObservableCollection([]);


        //#region old
        this.ItemsSource = new ObservableCollection([]);
        this.EnableNegativeOffsetARPayments = ObjectsLocator.AccountingSettingPM.EnableNegativeOffsetARPayments;

        if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "EnableMultiCurrency")) {
            if (ObjectsLocator.AccountingSettingPM.EnableMultiCurrencyARPayments) {
                this.IsMultiCurrency = true;
            }
        }

        if (SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG") {
            this.TransferStatusVisibilityColumn = true;
        }

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            this.DisplaySATSettings = true;
        }

        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }

        this.SetUIProperties();
        this.ComputeRelativeRateDate();
        this.Listen();
        this.CheckARPaymentCashBook();

        if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
            this.LoadCurrencyRates();
        }

        else {
            this.LoadData();
        }

        //#endregion

        this.GetData();


    }

    ngOnInit() {
        this.LoadPaymentMethods();
    }

    //#region abdullah code
    originalPaymentOpenAmount:number;
    paymentAmountTotal:number = 0;
    amount2reconcileTotal:number = 0;;
    IsEntityValid: boolean = true;

    _loading: boolean = false;
    GetData(){
        if(this.EntityPM.GLAccountId){
            console.log(">>> Getting transactions for Account: ", this.EntityPM.GLAccountId);

            this._loading = true;
            this._LedgerTransactionExtendedListService.getTransactionsForARPayment(this.EntityPM.Id,this.EntityPM.GLAccountId).subscribe(myResult => {
                this._loading = false;

                var mm: ServiceResponse = myResult;
                if (!mm.HasError)
                {

                    console.log();

                    var transactions = mm.Result.Result;
                    var tempItemSource: any[] = [];
                    if (transactions != null) {
                        for (var i = 0; i < transactions.length; i++) {
                            var line = new TransactionLineModel(transactions[i], this);
                            // var line = transactions;
                            tempItemSource.push(line);
                        }
                        this.TransactionsList.InsertCollection(tempItemSource);
                    }
                }
                else
                {
                }
            });
        }else{
            console.error("No GLAccount for this payment ", this.EntityPM);

        }
    }

    CalculateTotals(){

        // payment open amount
        var _linesAmount2reco = 0;
        this.TransactionsList.Collection.forEach((line:TransactionLineModel)=>{
            if(line && line.AmountToReconcile >= 0){
                _linesAmount2reco += line.AmountToReconcile;
            }
        });
        this.amount2reconcileTotal = _linesAmount2reco;

        if(_linesAmount2reco <= this.originalPaymentOpenAmount)
            this.EntityPM.OpenAmount = this.originalPaymentOpenAmount - _linesAmount2reco;
        else
            this.EntityPM.OpenAmount = 0;

        //

    }
    OpenSource(id: string, sourceTypeCode: string) {

        var tableName = AccountingEntityHelper.getEntityObjectTableName(sourceTypeCode);;
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: id,
                    ObjectTableName: tableName
                });
            });

    }
    OpenJournal(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal', BackButtonLabel: 'Back' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }
    OpenReco(recoId) {
        if (!AppTool.IsNullOrEmpty(recoId)) {
            // this.showAlert = false;
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: recoId, ObjectTableName: 'Reconciliation' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        // SessionLocator.CurrentSession.CloseCurrentWindow();
                    });
                });

        }
    }
    //#endregion

    //#region old code
    public AllMethods: AccountingPaymentMethodList[] = [];
    LoadPaymentMethods() {
        var myService: AccountingPaymentMethodListService = new AccountingPaymentMethodListService();
        myService.getAll().subscribe((response: ServiceResponse) => {
            if (response != null) {
                this.AllMethods = response.Result;
            }
        });
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.LoadData();
                }

                if (this.RequestedCommandCode) {
                    this.ApplyRequestedCommand();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.LoadData();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    // UIProperties
    public RateIsEnabled: boolean = true;
    get IsScreenEnabled() {
        var result = true;

        if (this.EntityPM != null) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
                result = true;
            }

            else {
                result = false;
            }
        }

        return result;
    }
    SetUIProperties() {
        this.SetUIProperties_Cheque();
        this.SetUIProperties_Invoices();
        this.SetUIProperties_CreditCard();
        this.SetUIProperties_BankTransfer();
        this.GetRateIsEnabled();

        if (!this.IsScreenEnabled) {
            this.UIProperties.SetEnabled("AccountingPaymentMethodId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AmountInPaymentCurrency", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("RegisterDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ValueDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ChequeOrPaymentRef", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Bank", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("BankBranch", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Account", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CreditCardTypeId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("BranchId", this.ObjectTableName, false);
            if (this.FullAccounting) {
                this.UIProperties.SetEnabled("BankAccountId", this.ObjectTableName, false);
            }
        }

        else {
            this.UIProperties.SetEnabled("AccountingPaymentMethodId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("AmountInPaymentCurrency", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("RegisterDate", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ValueDate", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ChequeOrPaymentRef", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("Bank", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("BankBranch", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("Account", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PrintNotes", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("CreditCardTypeId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("BranchId", this.ObjectTableName, true);

            if (AppTool.IsNullOrEmpty(this.EntityPM.BillToId)) {
                this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, false);
            }

            if (this.EntityPM.StatusCode == "VD") {
                this.UIProperties.SetEnabled("PrintNotes", this.ObjectTableName, false);
            }
            if (this.FullAccounting) {
                this.UIProperties.SetEnabled("BankAccountId", this.ObjectTableName, true);
            }
        }
    }

    SetUIProperties_Invoices() {
        if (!this.IsScreenEnabled) {
            this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, true);

            if (this.EntityPM.PaymentInvoices.length > 0) {
                this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, false);
            }
        }

        this.SetUIProperties_ExchangeRate();
    }
    SetUIProperties_ExchangeRate() {
        var isEnabled: boolean = false;

        if (this.IsScreenEnabled) {
            if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "ARPaymentEditExchangeRate")) {
                if (this.EntityPM.PaymentInvoices.length > 0) {
                    isEnabled = false;
                }
                else {
                    isEnabled = true;
                }

                if (this.PaymentCurrencyId == SessionLocator.TenantPM.CurrencyId) {
                    isEnabled = false;
                }
                else {
                    isEnabled = true;
                }
            }
        }

        this.UIProperties.SetEnabled("PaymentCurrencyExchangeRate", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("ExchangeRateDate", this.ObjectTableName, isEnabled);
    }
    SetUIProperties_Cheque() {
        if (this.FullAccounting && this.AccountingPaymentMethodCode == "CH") {
            this.UIProperties.SetRequired("BankBranch", this.ObjectTableName, AppTool.IsNullOrEmpty(this.BankBranch));
            this.UIProperties.SetRequired("Account", this.ObjectTableName, AppTool.IsNullOrEmpty(this.Account));
            var service: InvoiceDomainService = new InvoiceDomainService();
            service.GetStatusOfARPaymentCheques(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null && !myResponse.HasError) {
                    this.ARPaymentChequeStatus = myResponse.Result;
                    if (this.ARPaymentChequeStatus == "בקופה" || this.ARPaymentChequeStatus == "משמרת" || this.ARPaymentChequeStatus == "הופקד- טרם נפרע") {
                        this.ARPaymentChequeStatusColor = "orange";
                    }
                    else if (this.ARPaymentChequeStatus == "הוחזר ללקוח") {
                        this.ARPaymentChequeStatusColor = "red";
                    }
                    else if (this.ARPaymentChequeStatus == "נפרע") {
                        this.ARPaymentChequeStatusColor = "green";
                    }
                }
            });
        }
        else {
            this.UIProperties.SetRequired("BankBranch", this.ObjectTableName, false);
            this.UIProperties.SetRequired("Account", this.ObjectTableName, false);
        }

        this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, false);

        if (!AppTool.IsNullOrEmpty(this.AccountingPaymentMethodCode)) {
            if (this.AccountingPaymentMethodCode == "CH") {
                if (AppTool.IsNullOrEmpty(this.ChequeOrPaymentRef)) {
                    this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, true);
                }
                this.UIProperties.SetVisibility("Bank", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("Account", this.ObjectTableName, true);
            }
        }
    }
    SetUIProperties_CreditCard() {
        this.UIProperties.SetVisibility("CreditCardTypeId", this.ObjectTableName, false);
        this.CreditCardTypeIdVisibility = false;
        this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, false);
        if (this.AccountingPaymentMethodCode == "CC") {

            if (AppTool.IsNullOrEmpty(this.CreditCardTypeId)) {
                this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, true);
            }
            this.UIProperties.SetVisibility("CreditCardTypeId", this.ObjectTableName, true);
            this.UIProperties.SetVisibility("Bank", this.ObjectTableName, true);
            this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, true);
            this.UIProperties.SetVisibility("Account", this.ObjectTableName, true);
            this.CreditCardTypeIdVisibility = true;
        }
    }
    SetUIProperties_BankTransfer() {
        this.UIProperties.SetRequired("BankAccountId", this.ObjectTableName, false);
        if (this.FullAccounting == true && this.AccountingPaymentMethodCode == "BT") {
            if (AppTool.IsNullOrEmpty(this.BankAccountId)) {
                this.UIProperties.SetRequired("BankAccountId", this.ObjectTableName, true);
            }

            this.UIProperties.SetVisibility("BankAccountId", this.ObjectTableName, true);
            if (!this.FullAccounting) {
                this.UIProperties.SetVisibility("Bank", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("Account", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetVisibility("Bank", this.ObjectTableName, false);
                this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, false);
                this.UIProperties.SetVisibility("Account", this.ObjectTableName, false);
            }
            this.BankAccountIdVisibility = true;
        }
        else {
            this.UIProperties.SetVisibility("BankAccountId", this.ObjectTableName, false);
            this.BankAccountIdVisibility = false;
        }
    }
    GetRateIsEnabled() {
        var result = true;
        if (this.EntityPM != null) {
            if (this.EntityPM.PaymentCurrencyId == SessionLocator.TenantPM.CurrencyId || this.EntityPM.PaymentCurrencyId == null || SessionLocator.TenantPM.CurrencyId == null) {
                result = false;
            }
        }

        this.SetUIProperties_ExchangeRate();

        this.RateIsEnabled = result;
    }

    // Load Data
    public IsDataLoaded: boolean = false;
    private searchText: string = "";
    SearchTextKeyUp(args: any) {
        this.searchText = args;
        this.LoadData();
    }

    private ConnectedList: ARInvoiceList[] = [];
    private IsMatchedList: ARInvoiceList[] = [];
    LoadData() {

        this.ItemsSource.Clear();

        if (!AppTool.IsNullOrEmpty(this.BillToId) && this.EntityPM.StatusCode != "VD") {

            if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {

                if (this.EntityPM.PaymentInvoices.length == 1) {
                    this.LoadPaymentInvoices_Created();
                    //var iConnectedItem = new ARInvoiceList();
                    //iConnectedItem.Id = this.EntityPM.PaymentInvoices[0].ARInvoiceId;
                    //iConnectedItem.InvoiceCurrencyId = this.EntityPM.PaymentInvoices[0].ForeignCurrencyId;
                    //iConnectedItem.InvoiceCurrencyExchangeRate = this.EntityPM.PaymentInvoices[0].ExchangeRate;
                    //iConnectedItem.AmountInInvoiceCurrency = this.EntityPM.PaymentInvoices[0].ForeignAmount;
                    //iConnectedItem.AmountInLocalCurrency = this.EntityPM.PaymentInvoices[0].LocalAmount;
                    //iConnectedItem.Id = this.EntityPM.PaymentInvoices[0].ARInvoiceId;
                    //iConnectedItem.MetodoPagoCode = this.EntityPM.PaymentInvoices[0].ARInvoiceMetodoPagoCode;
                    //this.ConnectedList.push(iConnectedItem);
                }

                else {
                    this.LoadPaymentInvoices_IsMatched();
                }
            }

            else {
                this.LoadPaymentInvoices_Connected();
            }
        }

        else {
            this.UpdateSummary();
            this.IsDataLoaded = true;
        }
    }
    LoadPaymentInvoices_Created() {
        var invoiceId = this.EntityPM.PaymentInvoices[0].ARInvoiceId;

        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 1;
        filters.SortBy = "InvoiceDate";
        filters.SortDirection = "Descending";

        filters.addAdditionalFilter("Id", invoiceId, null, null, "Equals", false, false, false, "string");

        var myService = new ARInvoiceListService();
        myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {

                this.ConnectedList = myResponse.Result;

                this.LoadPaymentInvoices_IsMatched();
            }
        });
    }
    LoadPaymentInvoices_Connected() {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 1000;
        filters.SortBy = "InvoiceDate";
        filters.SortDirection = "Descending";

        var searchValue = null;
        if (!AppTool.IsNullOrEmpty(this.searchText)) {
            searchValue = AppTool.IsNullOrEmpty(this.searchText.trim()) ? null : this.searchText;
        }

        filters.addAdditionalFilter("ARPaymentInvoicesSearch", searchValue, null, null, "Contains", true, false, false, "string");
        filters.addAdditionalFilter("ARPaymentInvoicesConnected", this.EntityPM.Id, null, null, "Contains", true, false, false, "string");

        var myService = new ARInvoiceListService();
        myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {

                this.ConnectedList = myResponse.Result;

                if (this.EntityPM.IsClosed) {
                    this.FillBaselist();
                }

                else {
                    this.LoadPaymentInvoices_IsMatched();
                }
            }
        });
    }
    LoadPaymentInvoices_IsMatched() {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 1000;
        filters.SortBy = "InvoiceDate";
        filters.SortDirection = "Descending";

        filters.addAdditionalFilter("BillToId", this.EntityPM.BillToId, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("StatusCode", "DR,AD,PP,PD", null, null, "InList", false, true, false, "string");
        filters.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "Boolean");
        filters.addAdditionalFilter("IsConstituentInvoice", false, null, null, "Equals", false, false, false, "Boolean");

        var searchValue = null;
        if (!AppTool.IsNullOrEmpty(this.searchText)) {
            searchValue = AppTool.IsNullOrEmpty(this.searchText.trim()) ? null : this.searchText;
        }

        filters.addAdditionalFilter("ARPaymentInvoicesSearch", searchValue, null, null, "Contains", true, false, false, "string");

        var myService = new ARInvoiceListService();
        myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.IsMatchedList = myResponse.Result;
            }

            this.FillBaselist();
        });
    }
    FillBaselist() {
        // this.ItemsSource.Clear();

        // var connectedList: ARPaymentInvoiceArgs[] = [];
        // var unConnectedMatchedList: ARPaymentInvoiceArgs[] = [];
        // var unConnectedListNotMatched: ARPaymentInvoiceArgs[] = [];
        // var itemsCollection: ARPaymentInvoiceArgs[] = [];

        // if (this.ConnectedList.length > 0) {
        //     this.ConnectedList.forEach(item => {
        //         if (this.EntityPM.PaymentInvoices.filter(d => d.ARInvoiceId == item.Id)[0]) {

        //             if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
        //                 if (this.EntityPM.AmountInPaymentCurrency > 0) {
        //                     var value = this.EntityPM.AmountInPaymentCurrency;

        //                     if (item.AmountDue > value) {
        //                         item.AmountDue = item.AmountDue - value;
        //                     }

        //                     else {
        //                         item.AmountDue = 0;
        //                     }
        //                 }
        //             }

        //             connectedList.push(new ARPaymentInvoiceArgs(item, this));
        //         }
        //     });

        //     connectedList.sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
        //         itemsCollection.push(item);
        //     });
        // }

        // if (!this.EntityPM.IsClosed) {
        //     this.IsMatchedList.forEach(item => {
        //         if (connectedList.filter(f => f.Id == item.Id).length == 0) {
        //             if (!item.IsClosed) {
        //                 var isCurrencyMatched: boolean = false;

        //                 if (this.PaymentCurrencyId == item.InvoiceCurrencyId) {
        //                     isCurrencyMatched = true;
        //                 }

        //                 else if (this.IsMultiCurrency) {
        //                     if (this.PaymentCurrencyId == SessionLocator.LocalCurrencyId) {
        //                         isCurrencyMatched = true;
        //                     }

        //                     else if (item.InvoiceCurrencyId == SessionLocator.LocalCurrencyId) {
        //                         isCurrencyMatched = true;
        //                     }
        //                 }

        //                 if (isCurrencyMatched == false || item.StatusCode == "DR") {
        //                     unConnectedListNotMatched.push(new ARPaymentInvoiceArgs(item, this));
        //                 }

        //                 else {
        //                     unConnectedMatchedList.push(new ARPaymentInvoiceArgs(item, this));
        //                 }
        //             }
        //         }
        //     });

        //     unConnectedMatchedList.filter(f => f.CurrencyId == this.PaymentCurrencyId).sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
        //         itemsCollection.push(item);
        //     });

        //     unConnectedMatchedList.filter(f => f.CurrencyId != this.PaymentCurrencyId).sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
        //         itemsCollection.push(item);
        //     });

        //     unConnectedListNotMatched.sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
        //         itemsCollection.push(item);
        //     });
        // }

        // this.ItemsSource.InsertCollection(itemsCollection);
        // this.UpdateSummary();
        // this.IsDataLoaded = true;
    }

    // BillTo
    get BillToId() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.BillToId;
    }
    set BillToId(value: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.BillToId != value) {
                this.EntityPM.BillToId = value;

                if (AppTool.IsNullOrEmpty(this.EntityPM.BillToId)) {
                    this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, false);
                }

                else {
                    this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, true);
                }

                this.LoadData();

                if (AppTool.IsNullOrEmpty(this.EntityPM.BillToId)) {
                    this.BillToAddressId = null;
                    this.EntityPM.BillToName = null;
                    this.EntityPM.BillToPartnerTypeId = null;
                    this.PaymentCurrencyId = SessionLocator.TenantPM.CurrencyId;
                }

                else {
                    var myService: CardListService = new CardListService();
                    myService.getSingle(this.EntityPM.BillToId).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var list: CardList = myResponse.Result;
                            if (list) {
                                this.EntityPM.BillToName = list.EnglishName;
                                this.EntityPM.BillToPartnerTypeId = list.PartnerTypeId;

                                if (!AppTool.IsNullOrEmpty(list.InvoiceCurrencyId)) {
                                    this.PaymentCurrencyId = list.InvoiceCurrencyId;
                                }

                                this.LoadAddress();
                                if (this.FullAccounting){
                                    if (!AppTool.IsNullOrEmpty(list.GLAccountId)) {
                                        var myGLAccountPMService = new GLAccountPMService();
                                        myGLAccountPMService.get(list.GLAccountId).subscribe((myResponse: ServiceResponse) => {
                                            if (!myResponse.HasError) {
                                                var glaccount: GLAccountPM = myResponse.Result;
                                                if (glaccount != null && !glaccount.IsMultiCurrency) {
                                                    this.PaymentCurrencyId = glaccount.CurrencyId;
                                                }
                                            }
                                        });
                                    }
                                }
                            }
                        }
                    });
                }

            }
        }
    }

    get BillToAddressId() { return this.EntityPM.BillToAddressId; }
    set BillToAddressId(value: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.BillToAddressId != value) {
                this.EntityPM.BillToAddressId = value;
            }
        }
    }

    LoadAddress() {
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetBillingAddressListByCardId(this.EntityPM.BillToId).subscribe((resp: any) => {
            if (resp != null) {
                var billingAddress = resp;
                if (billingAddress != null) {
                    this.BillToAddressId = billingAddress.Id;
                }
                else {
                    var myService: AddressListService = new AddressListService();
                    myService.getSingle(this.EntityPM.BillToId).subscribe(myResult => {
                        var myResponse: ServiceResponse = myResult;
                        if (!myResponse.HasError) {
                            var billingAddress: AddressList = myResponse.Result;
                            if (billingAddress != null) {
                                this.BillToAddressId = billingAddress.Id;
                            }
                            else {
                                var myService: PartnersDomainService = new PartnersDomainService();
                                myService.GetMainAddressListByCardId(this.EntityPM.BillToId).subscribe((resp: any) => {
                                    if (resp != null) {
                                        var mainAddress = resp;
                                        if (mainAddress != null) {
                                            this.BillToAddressId = mainAddress.Id;
                                        }

                                        else {
                                            this.BillToAddressId = null;
                                        }
                                    }
                                });

                            }
                        }
                    });
                }

            }
        });
    }

    // Currency
    get PaymentCurrencyId() { return this.EntityPM.PaymentCurrencyId; }
    set PaymentCurrencyId(value: string) {
        if (this.EntityPM.PaymentCurrencyId != value) {
            this.EntityPM.PaymentCurrencyId = value;
            this.CheckARPaymentCashBook();
            this.PaymentCurrencyExchangeRate = this.GetCurrencyRate(value);
            this.ExchangeRateDate = this.GetCurrencyRateDate(value);

            if (AppTool.IsNullOrEmpty(value)) {
                this.PaymentCurrencyCode = null;
            }

            else {
                var myService = new CurrencyListService();
                myService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CurrencyList = myResponse.Result;
                        if (list) {
                            this.PaymentCurrencyCode = list.Code;
                        }
                    }
                });
            }

            this.SetUIProperties_ExchangeRate();

            this.ItemsSource.Collection.forEach(item => {
                item.SetUIProperties();
                item.InitExchangeRate();
            });

            if (this.FullAccounting == true && this.AccountingPaymentMethodCode == "BT") {
                this.CheckGLAccountCurrencyId();
            }
        }
    }

    get PaymentCurrencyCode() { return this.EntityPM.PaymentCurrencyCode; }
    set PaymentCurrencyCode(value: string) {
        if (this.EntityPM.PaymentCurrencyCode != value) {
            this.EntityPM.PaymentCurrencyCode = value;
        }
    }

    get PaymentCurrencyExchangeRate() { return this.EntityPM.PaymentCurrencyExchangeRate; }
    set PaymentCurrencyExchangeRate(value: number) {
        if (this.EntityPM != null) {
            if (this.EntityPM.PaymentCurrencyExchangeRate != value) {
                this.EntityPM.PaymentCurrencyExchangeRate = AppTool.Round(value, 5);
                this.GetRateIsEnabled();
                this.ComputeLocalAmount();

                this.ItemsSource.Collection.forEach(item => {
                    item.InitExchangeRate();
                });
            }
        }
    }

    get ExchangeRateDate() { return this.EntityPM.ExchangeRateDate; }
    set ExchangeRateDate(value: Date) {
        if (this.EntityPM != null) {
            if (this.EntityPM.ExchangeRateDate != value) {
                this.EntityPM.ExchangeRateDate = value;
                this.ComputeRelativeRateDate();
            }
        }
    }

    private myRelativeRateDate: string = null;
    get RelativeRateDate() { return this.myRelativeRateDate; }
    set RelativeRateDate(value: string) {
        if (this.myRelativeRateDate != value) {
            this.myRelativeRateDate = value;
        }
    }
    ComputeRelativeRateDate() {
        this.RelativeRateDate = DateTool.GetRelativeRateDate(this.RegisterDate, this.ExchangeRateDate, "old");
    }

    public LastRatesList: LastRate[] = [];
    LoadCurrencyRates() {
        var loadingDate = this.EntityPM.RegisterDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

        var myService: CurrencyRatesService = new CurrencyRatesService();
        myService.GetCurrenciesExchangeRateByValueDate(SessionLocator.TenantPM.CurrencyId, loadingDate).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.LastRatesList = myResponse.Result;
            }

            this.LoadData();
        });
    }
    UpdateCurrencyRates() {
        var loadingDate = this.RegisterDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

        var myService: CurrencyRatesService = new CurrencyRatesService();
        myService.GetCurrenciesExchangeRateByValueDate(SessionLocator.TenantPM.CurrencyId, loadingDate).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.LastRatesList = myResponse.Result;

                this.SetCurrencyRateData();
            }
        });
    }
    SetCurrencyRateData() {
        var myRate: number = null;
        var myRateDate: Date = null;

        if (!AppTool.IsNullOrEmpty(this.PaymentCurrencyId)) {
            if (this.PaymentCurrencyId == SessionLocator.TenantPM.CurrencyId) {
                myRate = 1;
            }

            else {
                var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == this.PaymentCurrencyId)[0];
                if (lastRate != null) {
                    myRate = lastRate.Rate;
                    myRateDate = lastRate.ValueDate;
                }
            }
        }

        this.PaymentCurrencyExchangeRate = myRate;
        this.ExchangeRateDate = myRateDate;
    }
    GetCurrencyRate(currencyId: string): number {
        var result = null;

        if (AppTool.IsNullOrEmpty(currencyId)) {
            result = null;
        }

        else {
            if (currencyId == SessionLocator.TenantPM.CurrencyId) {
                result = 1;
            }

            else {
                if (this.LastRatesList) {
                    var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == currencyId)[0];
                    if (lastRate != null) {
                        result = lastRate.Rate;
                    }
                }
            }
        }

        return result;
    }
    GetCurrencyRateDate(currencyId: string): Date {

        var result = null;

        if (!AppTool.IsNullOrEmpty(currencyId)) {
            if (currencyId == SessionLocator.TenantPM.CurrencyId) {
                result = null;
            }

            else {
                if (this.LastRatesList) {
                    var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == currencyId)[0];
                    if (lastRate != null) {
                        result = lastRate.ValueDate;
                    }
                }
            }
        }

        return result;
    }

    get RegisterDate() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.RegisterDate;
    }
    set RegisterDate(value: Date) {
        if (this.EntityPM != null) {
            if (this.EntityPM.RegisterDate != value) {
                this.EntityPM.RegisterDate = value;
                this.UpdateCurrencyRates();
            }
        }
    }

    get AccountingPaymentMethodId() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.AccountingPaymentMethodId;
    }
    set AccountingPaymentMethodId(value: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.AccountingPaymentMethodId != value) {
                this.EntityPM.AccountingPaymentMethodId = value;

                this.RefreshPaymentMethodFields();
                //this.CheckARPaymentCashBook();
            }
        }
    }

    get AccountingPaymentMethodCode() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.AccountingPaymentMethodCode;
    }
    set AccountingPaymentMethodCode(value: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.AccountingPaymentMethodCode != value) {
                this.EntityPM.AccountingPaymentMethodCode = value;

                this.CheckARPaymentCashBook();
            }
        }
    }

    get SATPaymentMethodCode() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.SATPaymentMethodCode;
    }
    set SATPaymentMethodCode(value: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.SATPaymentMethodCode != value) {
                this.EntityPM.SATPaymentMethodCode = value;

            }
        }
    }

    get BranchId() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.BranchId;
    }
    set BranchId(value: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.BranchId != value) {
                this.EntityPM.BranchId = value;
                this.CheckARPaymentCashBook();
            }
        }
    }

    public CashBookName: string;
    public BranchGLAccountNumber: string;
    public IsCashBookValid: boolean = false;
    public BranchGLAccountId: string;
    CheckARPaymentCashBook() {
        this.IsCashBookValid = false;

        if (this.FullAccounting && (this.AccountingPaymentMethodCode == "CA" || this.AccountingPaymentMethodCode == "CH")) {
            var service: InvoiceDomainService = new InvoiceDomainService();
            service.CheckARPaymentCashBook(this.AccountingPaymentMethodCode, this.PaymentCurrencyId, this.BranchId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null && !myResponse.HasError) {
                    var cashbook: CashBookPM[] = myResponse.Result;
                    if (cashbook != null && cashbook.length > 0) {
                        if (cashbook.length == 1) {
                            var data: CashBookPM = cashbook[0];
                            this.CashBookName = data.EnglishName;
                            this.BranchGLAccountNumber = data.AccountNumber;
                            this.BranchGLAccountId = data.AccountId;
                            this.IsCashBookValid = true;
                            if (AppTool.IsNullOrEmpty(this.EntityPM.CashbookId)) {
                                this.EntityPM.CashbookId = data.Id;
                            }
                            this.UIProperties.SetValidity("BranchId", this.ObjectTableName, true, "");
                        }
                        else {
                            var branchesText = "";
                            cashbook.forEach((item: CashBookPM) => {
                                if (!AppTool.IsNullOrEmpty(item.EnglishName)) {
                                    branchesText += item.EnglishName + ",";
                                }
                            });
                            branchesText = branchesText.replace(/,\s*$/, "");
                            var msg = "There is no cashbook for this branch, Cashbooks for branches: " + branchesText + "  was found, change the branch please";
                            this.UIProperties.SetValidity("BranchId", this.ObjectTableName, false, msg);
                        }
                    }
                    else {
                        var msg = "There is no cashbook that compatible to this ARPayment, create one please";
                        this.UIProperties.SetValidity("BranchId", this.ObjectTableName, false, msg);
                    }
                }
            });
        }
    }

    //Payment Line Properties
    private RefreshPaymentMethodFields() {
        this.Bank = null;
        this.BankBranch = null;
        this.Account = null;
        this.ChequeOrPaymentRef = null;
        this.ValueDate = null;
        this.CreditCardTypeId = null;

        var lists: AccountingPaymentMethodList[] = this.AllMethods.filter(d => d.Id == this.AccountingPaymentMethodId);
        if (lists) {
            var list = lists[0];
            if (list) {
                this.AccountingPaymentMethodCode = list.Code;
            }
        }

        if (this.AccountingPaymentMethodCode == "CA" || this.AccountingPaymentMethodCode == "FS") {
            this.ValueDate = DateTool.GetCurrentDateAsUtc();
        }

        this.SetUIProperties_Cheque();
        this.SetUIProperties_CreditCard();
        this.SetUIProperties_BankTransfer();
    }
    get PaymentMethodDetailsLabel() {
        var result = "";
        if (this.AccountingPaymentMethodCode == "CH") {
            result = TextCodeTranslator.Translate("ARPayment.S.Details.Cheque");
        }

        else if (this.AccountingPaymentMethodCode == "FS") {
            result = "Offsetting";
        }

        else if (this.AccountingPaymentMethodCode == "BT") {
            result = TextCodeTranslator.Translate("ARPayment.S.Details.BankTransfer");
        }
        else if (this.AccountingPaymentMethodCode == "CC") {
            result = TextCodeTranslator.Translate("ARPayment.S.Details.CreditCard");
        }

        else {
            var method: AccountingPaymentMethodList = this.AllMethods.filter(d => d.Code == this.AccountingPaymentMethodCode)[0];
            if (method != null) {
                result = method.Name;
            }
        }

        return result;
    }
    get ChequePaymentRefLabel() {
        var result = TextCodeTranslator.Translate("ARPayment.S.Details.PaymentRef");
        if (this.AccountingPaymentMethodCode == "CH") {
            result = TextCodeTranslator.Translate("ARPayment.S.Details.ChequeRef");
        }

        return result;
    }
    get VisibleIfCash() {
        var result = false;
        if (!AppTool.IsNullOrEmpty(this.AccountingPaymentMethodCode)) {
            if (this.AccountingPaymentMethodCode == "CA" || this.AccountingPaymentMethodCode == "FS") {
                result = true;
            }
        }
        return result;
    }
    get CollapsedIfCash() {
        var result = false;
        if (!AppTool.IsNullOrEmpty(this.AccountingPaymentMethodCode)) {
            if (this.AccountingPaymentMethodCode == "CA" || this.AccountingPaymentMethodCode == "FS") {
                result = false;
            }

            else {
                result = true;
            }
        }
        return result;
    }

    get Bank() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.Bank;
    }
    set Bank(value: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.Bank != value) {
                this.EntityPM.Bank = value;
            }
        }
    }

    get BankBranch() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.BankBranch;
    }
    set BankBranch(value: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.BankBranch != value) {
                this.EntityPM.BankBranch = value;

                if (this.FullAccounting && this.AccountingPaymentMethodCode == "CH") {
                    if (!AppTool.IsNullOrEmpty(value)) {
                        this.UIProperties.SetRequired("BankBranch", this.ObjectTableName, false);
                    }
                    else {
                        this.UIProperties.SetRequired("BankBranch", this.ObjectTableName, true);
                    }
                }
            }
        }
    }

    get Account() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.Account;
    }
    set Account(value: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.Account != value) {
                this.EntityPM.Account = value;
                if (this.FullAccounting && this.AccountingPaymentMethodCode == "CH") {
                    if (!AppTool.IsNullOrEmpty(value)) {
                        this.UIProperties.SetRequired("Account", this.ObjectTableName, false);
                    }
                    else {
                        this.UIProperties.SetRequired("Account", this.ObjectTableName, true);
                    }
                }
            }
        }
    }

    get ValueDate() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.ValueDate;
    }
    set ValueDate(value: Date) {
        if (this.EntityPM != null) {
            if (this.EntityPM.ValueDate != value) {
                this.EntityPM.ValueDate = value;
                //if (value != null) {
                //    this.UIProperties.SetRequired("ValueDate", this.ObjectTableName, false);
                //    this.UIProperties.SetValidity("ValueDate", this.ObjectTableName, false, null);
                //}
                //else {
                //    this.UIProperties.SetRequired("ValueDate", this.ObjectTableName, true);
                //    this.UIProperties.SetValidity("ValueDate", this.ObjectTableName, true, null);
                //}
            }
        }
    }

    get ChequeOrPaymentRef() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.ChequeOrPaymentRef;
    }
    set ChequeOrPaymentRef(value: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.ChequeOrPaymentRef != value) {
                this.EntityPM.ChequeOrPaymentRef = value;
                this.SetUIProperties_Cheque();
            }
        }
    }

    get CreditCardTypeId() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.CreditCardTypeId;
    }
    set CreditCardTypeId(value: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.CreditCardTypeId != value) {
                this.EntityPM.CreditCardTypeId = value;
            }

            if (!AppTool.IsNullOrEmpty(value)) {
                this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, true);
            }
        }
    }

    public CreditCardTypeIdVisibility: boolean = false;
    public BankAccountIdVisibility: boolean = false;

    get BankAccountId() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.BankAccountId;
    }
    set BankAccountId(value: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.BankAccountId != value) {
                this.EntityPM.BankAccountId = value;
                this.CheckGLAccountCurrencyId();

                if (!AppTool.IsNullOrEmpty(value)) {
                    this.UIProperties.SetRequired("BankAccountId", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("BankAccountId", this.ObjectTableName, true);
                }
            }
        }
    }

    bankAccount: BankAccountPM;
    public GLAccountNumber: string = "";
    public IsGLAccountCurrencyDifferent = false;
    private CheckGLAccountCurrencyId() {
        var service: BankAccountPMService = new BankAccountPMService();
        if (!AppTool.IsNullOrEmpty(this.BankAccountId)) {
            service.get(this.BankAccountId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null && !myResponse.HasError) {
                    this.bankAccount = myResponse.Result;
                    if (this.bankAccount != null && this.bankAccount.GLAccountCurrencyId != null && this.bankAccount.GLAccountCurrencyId != "multi") {
                        if (this.bankAccount.GLAccountCurrencyId != this.PaymentCurrencyId) {
                            this.GLAccountNumber = this.bankAccount.GLAccountNumber;
                            this.IsGLAccountCurrencyDifferent = true;
                            var msg = "The currency of the bank account GLAccount (" + this.GLAccountNumber + ") is different from ARPayment curreny";
                            this.UIProperties.SetValidity("BankAccountId", this.ObjectTableName, false, msg );
                        }
                        else {
                            this.IsGLAccountCurrencyDifferent = false;
                            this.UIProperties.SetValidity("BankAccountId", this.ObjectTableName, true, "");
                        }
                    }
                    else {
                        this.IsGLAccountCurrencyDifferent = false;
                        this.UIProperties.SetValidity("BankAccountId", this.ObjectTableName, true, "");
                    }
                }
            });
        }
        else {
            this.IsGLAccountCurrencyDifferent = false;
            this.UIProperties.SetValidity("BankAccountId", this.ObjectTableName, true, "");
        }
    }
    EditGLAccount(arg: string) {
        if (arg == "BA") {
            if (!AppTool.IsNullOrEmpty(this.bankAccount.GLAccountId)) {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: this.bankAccount.GLAccountId, ObjectTableName: 'GLAccount' });
                    });
            }
        }
        else {
            if (!AppTool.IsNullOrEmpty(this.BranchGLAccountId)) {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: this.BranchGLAccountId, ObjectTableName: 'GLAccount' });
                    });
            }
        }
    }

    // Amounts
    get AmountInPaymentCurrency() { return this.EntityPM.AmountInPaymentCurrency; }
    set AmountInPaymentCurrency(value: number) {
        if (this.EntityPM.AmountInPaymentCurrency != value) {
            this.EntityPM.AmountInPaymentCurrency = AppTool.Round(value, 2);
            this.ComputeLocalAmount();
            this.ComputeOpenAmount();
            this.UpdateSummary();

            this.ItemsSource.Collection.forEach(item => {
                item.SetUIProperties();
            });
        }
    }

    get AmountInLocalCurrency() { return this.EntityPM.AmountInLocalCurrency; }
    set AmountInLocalCurrency(value: number) {
        if (this.EntityPM.AmountInLocalCurrency != value) {
            this.EntityPM.AmountInLocalCurrency = AppTool.Round(value, 2);
        }
    }

    get OpenAmount() { return this.EntityPM.OpenAmount == null ? 0 : this.EntityPM.OpenAmount; }
    set OpenAmount(value: number) {
        if (this.EntityPM.OpenAmount != value) {
            this.EntityPM.OpenAmount = AppTool.Round(value, 2);
        }
    }

    get PrintNotes() { return this.EntityPM.PrintNotes; }
    set PrintNotes(value: string) {
        if (this.EntityPM.PrintNotes != value) {
            this.EntityPM.PrintNotes = value;
        }
    }

    ComputeOpenAmount() {
        this.OpenAmount = this.AmountInPaymentCurrency - this.Summary_AmountPaid;
    }
    ComputeLocalAmount() {
        this.AmountInLocalCurrency = this.AmountInPaymentCurrency * this.PaymentCurrencyExchangeRate;
    }

    // Summary
    public Summary_Amount: number = 0;
    public Summary_AmountPaid: number = 0;
    public Summary_AmountPaidColor: string = "#282E30";
    UpdateSummary() {
        var Amount: number = 0;
        var AmountPaid: number = 0;
        var AmountPaidColor: string = "#282E30";

        if (this.AmountInPaymentCurrency) {
            Amount = AppTool.Round(this.AmountInPaymentCurrency, 2);
        }

        if (this.EntityPM.PaymentInvoices.length > 0) {
            AmountPaid = ArrayTool.Sum(this.EntityPM.PaymentInvoices, "PaymentAmount");
            AmountPaid = AppTool.Round(AmountPaid, 2);
        }

        if (this.IsNegativeAmountEnabled == false) {
            if (AmountPaid < 0) {
                AmountPaidColor = "#E53030";
            }
        }

        if (AmountPaid > Amount) {
            AmountPaidColor = "#E53030";
        }

        this.Summary_Amount = Amount;
        this.Summary_AmountPaid = AmountPaid;
        this.Summary_AmountPaidColor = AmountPaidColor;
    }

    ViewEntity(args: any) {
        if (args) {
            this.RequestedCommandCode = "ViewInvoice";
            this.RequestedCommandParam = args.Id;

            this.entityArgs.EditComponent.SaveChanges();
        }
    }
    UpdateCurrencyRateMethod() {
        this._entityResourceService.getEntityResourceByTableName("RatesTable", 0).subscribe((response: any) => {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 350;
            logWindow.Title = "Update Currency Rate";
            logWindow.WindowArgs = { CurrencyId: this.EntityPM.PaymentCurrencyId, CurrencyCode: this.EntityPM.PaymentCurrencyCode, Rate: this.EntityPM.PaymentCurrencyExchangeRate, Date: this.RegisterDate };
            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.PaymentCurrencyExchangeRate = comp.Rate;
                        this.ExchangeRateDate = comp.RateDate;
                    }
                });
            });
            logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
        });
    }
    UpdatePaymentInvoicesErrors() {
        var haserrors = false;

        if (this.ItemsSource.Collection.filter(d => d.InputHasError)[0]) {
            haserrors = true;
        }

        this.EntityPM.HasInvoicesErrors = haserrors;
    }

    private RequestedCommandCode: string = null;
    private RequestedCommandParam: string = null;
    ApplyRequestedCommand() {
        if (this.RequestedCommandCode == "ViewInvoice") {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.RequestedCommandParam, ObjectTableName: 'ARInvoice' });

                    this.RequestedCommandParam = null;

                    let isEditComponentSaved = false;

                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        if (isEditComponentSaved) {
                            this.entityArgs.EditComponent.ReloadEntityPM();
                        }
                    });

                    cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                        if (isSaveSuccess) {
                            isEditComponentSaved = true;
                        }
                    });

                    cmpRef.instance.SaveAndCloseCompleted.subscribe((isSaveSuccess: boolean) => {
                        if (isSaveSuccess) {
                            isEditComponentSaved = true;
                        }
                    });
                });
        }

        this.RequestedCommandCode = null;
    }

    //#endregion
}


export class TextStore{
    static open: string =  TextCodeTranslator.Translate('Accounting.O.ARP.Open');
    static Closed: string =  TextCodeTranslator.Translate('Accounting.O.ARP.Closed');
    static partiallyOpened: string =  TextCodeTranslator.Translate('Accounting.O.ARP.partiallyOpened');

    static ErrorsInSelectedLines: string =  TextCodeTranslator.Translate('Reconciliations.O.ErrorsInSelectedLines');
    static AmountMustBSmaller2OpenAmount: string =  TextCodeTranslator.Translate('Reconciliations.O.AmountMustBSmaller2OpenAmount');


}

export class TransactionLineModel extends BaseComponent {
    public DataContext = this;
    public LedgerTransactionPM: LedgerTransactionPM = null;
    public ObjectTableName = "LedgerTransaction";
    public isRTL: boolean = false;
    isLineValid: boolean = true;


    constructor(
        private ledgerTransaction: LedgerTransactionPM,
        private parent: ARPaymentDetailsFullAccountingTab
    ) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = this.parent.EntityPM;
        this.LedgerTransactionPM = ledgerTransaction;

        this.originalOpenAmount = this.OpenAmount;

        this.CalculateFields();
    }

    CalculateFields(){
        this.IconCode = AccountingEntityHelper.getEntityIcon(this.LedgerTransactionPM.SourceTypeCode);

        this.Status = this.GetStatus();
        this.OriginalAmount = this.CalculateOriginalAmount();
        this.OriginalAmountCurrency = this.CalculatOriginalCurruncy();
    }

    //#region Properties
    public IconCode: string;
    public ReconciliationNumber: string;


    private _OriginalAmount : number;
    public get OriginalAmount() : number {
        return this._OriginalAmount;
    }
    public set OriginalAmount(v : number) {
        this._OriginalAmount = v;
    }


    private _OriginalAmountCurrency : string;
    public get OriginalAmountCurrency() : string {
        return this._OriginalAmountCurrency;
    }
    public set OriginalAmountCurrency(v : string) {
        this._OriginalAmountCurrency = v;
    }

    private _isChecked : boolean;
    public get IsChecked() : boolean {
        if(this.IsReconciled){
            return true;
        }else{
            return this._isChecked;
        }
    }
    public set IsChecked(v : boolean) {
        this._isChecked = v;
    }


    get IsReconciled() { return this.LedgerTransactionPM.IsReconciled; }
    set IsReconciled(value: boolean) {
        if (this.LedgerTransactionPM.IsReconciled != value) {
            this.LedgerTransactionPM.IsReconciled = value;
        }
    }

    originalOpenAmount: number;

    get AmountToReconcile() { return this.LedgerTransactionPM.AmountToReconcile; }
    set AmountToReconcile(value: number) {
        if (this.LedgerTransactionPM.AmountToReconcile != value) {
            this.LedgerTransactionPM.AmountToReconcile = value;

            //set amount
            if (this.AmountToReconcile >= 0 && this.AmountToReconcile <= this.originalOpenAmount){
                this.OpenAmount = this.originalOpenAmount - this.AmountToReconcile;
            }else{
                this.OpenAmount = this.originalOpenAmount;
            }

            //validate line
            // if (this.parent.IsEntityValid) {
                if (this.AmountToReconcile >= 0 && this.AmountToReconcile <= this.originalOpenAmount) {
                    this.UIProperties.SetValidity("AmountToReconcile", this.ObjectTableName, false, TextStore.AmountMustBSmaller2OpenAmount);
                    this.parent.IsEntityValid = true;
                    this.isLineValid = true;

                } else {
                    this.UIProperties.SetValidity("AmountToReconcile", this.ObjectTableName, true, "valid");
                    this.parent.IsEntityValid = false;
                    this.isLineValid = false;
                }
            // }

            //update parent totals
            this.parent.CalculateTotals();
        }

    }

    get OpenAmount() { return this.LedgerTransactionPM.OpenAmount; }
    set OpenAmount(value: number) {
        if (this.LedgerTransactionPM.OpenAmount != value) {
            this.LedgerTransactionPM.OpenAmount = value;
        }
    }

    private _Status : string;
    public get Status() : string {
        return this._Status;
    }
    public set Status(v : string) {
        this._Status = v;
    }

    //#endregion

    //#region Other Properties
    get Id() { return this.LedgerTransactionPM.Id; }
    get Tenant() { return this.LedgerTransactionPM.Tenant; }
    get AccountingDate() { return this.LedgerTransactionPM.AccountingDate; }
    get DocumentDate() { return this.LedgerTransactionPM.DocumentDate; }
    get JournalNumber() { return this.LedgerTransactionPM.JournalNumber; }
    get Source() { return this.LedgerTransactionPM.Source; }
    get SourceType() { return this.LedgerTransactionPM.SourceType; }
    get SourceId() { return this.LedgerTransactionPM.SourceId; }
    get DueDate() { return this.LedgerTransactionPM.DueDate; }
    get LocalAmountCredit() { return this.LedgerTransactionPM.LocalAmountCredit; }
    get LocalAmountDebit() { return this.LedgerTransactionPM.LocalAmountDebit; }
    get ForeignAmountCredit() { return this.LedgerTransactionPM.ForeignAmountCredit; }
    get ForeignAmountDebit() { return this.LedgerTransactionPM.ForeignAmountDebit; }
    get OpenAmountCurrencyCode() { return this.LedgerTransactionPM.OpenAmountCurrencyCode; }
    get OpenAmountCurrencySign() { return this.LedgerTransactionPM.OpenAmountCurrencySign; }
    get CurrencyId() { return this.LedgerTransactionPM.CurrencyId; }
    get Reference1() { return this.LedgerTransactionPM.Reference1; }
    get Reference2() { return this.LedgerTransactionPM.Reference2; }
    get Reference3() { return this.LedgerTransactionPM.Reference3; }
    get Notes() { return this.LedgerTransactionPM.Notes; }
    get IsPartial() { return this.OpenAmount != this.AmountToReconcile; }
    get OpenAmountCurrencyId() { return this.LedgerTransactionPM.OpenAmountCurrencyId; }
    get SourceTypeCode() { return this.LedgerTransactionPM.SourceTypeCode; }
    get SourceNumber() { return this.LedgerTransactionPM.SourceNumber; }
    get GroupNumber() { return this.LedgerTransactionPM.GroupHash; }
    get RecoNumber() { return this.LedgerTransactionPM.RecoNumber; }
    get ReconciliationId() { return this.LedgerTransactionPM.ReconciliationId; }

    //#endregion

    GetStatus() {
        var __s = "";

        if (this.OriginalAmount == this.originalOpenAmount)
            __s = TextStore.open;
        else if (0 == this.originalOpenAmount)
            __s = TextStore.Closed;
        else
            __s = TextStore.partiallyOpened;

        return __s;
    }
    GetStatusColor(){
        var _color = 'black';
        if (this.OriginalAmount == this.originalOpenAmount)
            _color = 'green';
        else if (0 == this.originalOpenAmount)
            _color = 'black';
        else
            _color = 'orange';
        return _color;
    }

    CalculateOriginalAmount() {
        var transaction = this.LedgerTransactionPM;

        if (!AppTool.IsNullOrEmpty(this.parent.EntityPM.GLAccountRecoMethodCode)) {

            if (this.parent.EntityPM.GLAccountRecoMethodCode == "0") { // 0-local currency

                if (transaction['LocalAmountCredit'] == 0) {
                    return transaction['LocalAmountDebit'];
                } else {
                    return -1 * transaction['LocalAmountCredit'];
                }

            } else if (this.parent.EntityPM.GLAccountRecoMethodCode == "1") { // 1-foreign currency

                if (transaction['ForeignAmountCredit'] == 0) {
                    return transaction['ForeignAmountDebit'];
                } else {
                    return -1 * transaction['ForeignAmountCredit'];
                }

            }

        }
    }
    CalculatOriginalCurruncy() {
        //
        // [i] copied from list template
        //

        if (!AppTool.IsNullOrEmpty(this.parent.EntityPM.GLAccountRecoMethodCode)) {
            // this code was copied to reconcile window, if it need change, please chenge it in reconcile window too
            if (this.parent.EntityPM.GLAccountRecoMethodCode == "0") { // 0-local currency

                // local
                return SessionLocator.TenantPM.CurrencySign;

            } else if (this.parent.EntityPM.GLAccountRecoMethodCode == "1") { // 1-foreign currency

                // foreign
                return this.ledgerTransaction.CurrencySign;

            }

        }
    }
}
