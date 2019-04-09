import {Component, OnInit, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ARPaymentPM} from '../../../../Invoice/EntityPMs/ARPaymentPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, DateTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {AddressList} from '../../../../Common/EntityLists/AddressList';
import {AddressListService} from '../../../../Common/Services/StandardLists/AddressListService';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ARInvoiceListService} from '../../../../Invoice/Services/StandardLists/ARInvoiceListService';
import {ARInvoiceList} from '../../../../Invoice/EntityLists/ARInvoiceList';
import {ARPaymentInvoicePM} from '../../../../Invoice/EntityPMs/ARPaymentInvoicePM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {CurrencyRatesService, LastRate} from '../../../../Common/Services/CurrencyRatesService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {BankAccountPMService} from '../../../../Accounting/Services/StandardPMs/BankAccountPMService';
import {BankAccountPM} from '../../../../Accounting/EntityPMs/BankAccountPM';
import {InvoiceDomainService} from '../../../../Invoice/Services/InvoiceDomainService';
import {CashBookPM} from '../../../../Accounting/EntityPMs/CashBookPM';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {GetAccountingSystemWindowArgs} from '../../../../Common/Args';
import {GlobalDomainService} from '../../../../Common/Services/GlobalDomainService';
import {AccountingPaymentMethodList} from '../../../../Invoice/EntityLists/AccountingPaymentMethodList';
import {AccountingPaymentMethodListService} from '../../../../Invoice/Services/StandardLists/AccountingPaymentMethodListService';
import { Invoice } from '../../../../Customs/DataContract/ResponseData/ExportDeclarationDataResponseData';
import {GLAccountPMService} from '../../../../Accounting/Services/StandardPMs/GLAccountPMService';
import {GLAccountPM} from '../../../../Accounting/EntityPMs/GLAccountPM';

@Component({
    moduleId: module.id,
    templateUrl: './ARPaymentDetailsTabComponent.html',
})

export class ARPaymentDetailsTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: ARPaymentPM;
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
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs, private _entityResourceService: EntityResourceService) {
        super();

        if (ObjectsLocator.GlobalSetting) {
            this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }

        this.EntityPM = entityArgs.EntityPM;
        this.FullAccounting = SessionLocator.TenantPM.AccountingActivated;
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

    }

    ngOnInit() {
        this.LoadPaymentMethods();
    }  

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

    public RateIsEnabled: boolean = false;
    SetUIProperties_ExchangeRate() {
        var isEnabled: boolean = false;

        if (this.IsScreenEnabled) {
            if (FeatureLocator.HasFeaturePermession("ARPayment", "ARPaymentEditExchangeRate")) {
                if (this.PaymentCurrencyId) {
                    if (this.PaymentCurrencyId != SessionLocator.TenantPM.CurrencyId) {
                        if (this.EntityPM.PaymentInvoices.length == 0) {
                            isEnabled = true;
                        }
                    }
                }
            }
        }

        this.RateIsEnabled = isEnabled;
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
        this.SetUIProperties_ExchangeRate();
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
        this.ItemsSource.Clear();

        var connectedList: ARPaymentInvoiceArgs[] = [];
        var unConnectedMatchedList: ARPaymentInvoiceArgs[] = [];
        var unConnectedListNotMatched: ARPaymentInvoiceArgs[] = [];
        var itemsCollection: ARPaymentInvoiceArgs[] = [];

        if (this.ConnectedList.length > 0) {
            this.ConnectedList.forEach(item => {
                if (this.EntityPM.PaymentInvoices.filter(d => d.ARInvoiceId == item.Id)[0]) {

                    if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                        if (this.EntityPM.AmountInPaymentCurrency > 0) {
                            var value = this.EntityPM.AmountInPaymentCurrency;

                            if (item.AmountDue > value) {
                                item.AmountDue = item.AmountDue - value;
                            }

                            else {
                                item.AmountDue = 0;
                            }
                        }
                    }

                    connectedList.push(new ARPaymentInvoiceArgs(item, this));
                }
            });

            connectedList.sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
                itemsCollection.push(item);
            });
        }

        if (!this.EntityPM.IsClosed) {
            this.IsMatchedList.forEach(item => {
                if (connectedList.filter(f => f.Id == item.Id).length == 0) {
                    if (!item.IsClosed) {
                        var isCurrencyMatched: boolean = false;

                        if (this.PaymentCurrencyId == item.InvoiceCurrencyId) {
                            isCurrencyMatched = true;
                        }

                        else if (this.IsMultiCurrency) {
                            if (this.PaymentCurrencyId == SessionLocator.LocalCurrencyId) {
                                isCurrencyMatched = true;
                            }

                            else if (item.InvoiceCurrencyId == SessionLocator.LocalCurrencyId) {
                                isCurrencyMatched = true;
                            }
                        }

                        if (isCurrencyMatched == false || item.StatusCode == "DR") {
                            unConnectedListNotMatched.push(new ARPaymentInvoiceArgs(item, this));
                        }

                        else {
                            unConnectedMatchedList.push(new ARPaymentInvoiceArgs(item, this));
                        }
                    }
                }                
            });

            unConnectedMatchedList.filter(f => f.CurrencyId == this.PaymentCurrencyId).sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
                itemsCollection.push(item);
            });

            unConnectedMatchedList.filter(f => f.CurrencyId != this.PaymentCurrencyId).sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
                itemsCollection.push(item);
            });

            unConnectedListNotMatched.sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
                itemsCollection.push(item);
            });
        }

        this.ItemsSource.InsertCollection(itemsCollection);
        this.UpdateSummary();
        this.IsDataLoaded = true;
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
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: this.bankAccount.GLAccountId, ObjectTableName: 'GLAccount' });
                    });
            }
        }
        else {
            if (!AppTool.IsNullOrEmpty(this.BranchGLAccountId)) {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
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

    ViewEntity(args: ARPaymentInvoiceArgs) {
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
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
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
}
export class ARPaymentInvoiceArgs extends BaseComponent {
    public EntityPM: ARPaymentInvoicePM = null;
    public Invoice: ARInvoiceList = null;
    public PaymentPM: ARPaymentPM = null;
    public DataContext: ARPaymentInvoiceArgs = this;
    public ObjectTableName: string = "ARInvoice";
    public SortingValue: number = 0;
    public LocalCurrencyId: string = null;
    public IsMultiCurrency: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(item: ARInvoiceList, private trigger: ARPaymentDetailsTabComponent) {
        super();

        this.Invoice = item;
        this.PaymentPM = this.trigger.EntityPM;
        this.LocalCurrencyId = SessionLocator.LocalCurrencyId;
        this.IsMultiCurrency = this.trigger.IsMultiCurrency;
        
        if (item) {
            if (item.DueDate) {
                this.SortingValue = DateTool.GetDateParts(item.DueDate).DateTicks;
            }
        }

        this.EntityPM = this.PaymentPM.PaymentInvoices.filter(d => d.ARInvoiceId == this.Invoice.Id)[0];
        if (this.EntityPM) {
            this.isConnected = true;
            this.ExchangeRate = this.EntityPM.ExchangeRate;
            this.ConnectedAmount_INV = this.EntityPM.ForeignAmount;
            this.ConnectedAmount_PAY = this.EntityPM.PaymentAmount;
        }

        this.InitProperties();
        this.InitAllAmounts();
        this.InitExchangeRate();
        this.SetUIProperties();   
    }

    public Id: string = null;
    public DueDate: Date = null;
    public Status: string = null;
    public InvoiceNumber: string = null;
    public TransferStatus: string = null;
    public SATTransferStatus: string = null;
    public ShipmentNumber: string = null;
    public CurrencyId: string = null;
    public CurrencyCode: string = null;
    public InvoiceAmount: number = 0;
    public TransferStatusCode: string = null;

    InitProperties() {
        this.Id = this.Invoice.Id;
        this.DueDate = this.Invoice.DueDate;
        this.Status = this.Invoice.StatusName;
        this.InvoiceNumber = this.Invoice.InvoiceNumber;
        this.TransferStatus = this.Invoice.TransferStatusName;
        this.CurrencyId = this.Invoice.InvoiceCurrencyId;
        this.CurrencyCode = this.Invoice.InvoiceCurrencyCode;
        this.InvoiceAmount = this.Invoice.AmountInInvoiceCurrency == null ? 0 : this.Invoice.AmountInInvoiceCurrency;
        this.ShipmentNumber = this.Invoice.IsConsolidationInvoice ? "List" : this.Invoice.MainEntityReference;
        this.SATTransferStatus = this.Invoice.SATInvoiceStatusName;
        this.TransferStatusCode = this.Invoice.TransferStatusCode;

    }

    public ExchangeRate: number = 0;
    public OtherPaymentsAmount: number = 0;
    public InputHasError: boolean;
    InitAllAmounts() {
        var myInvoiceAmount: number = 0;
        var myAmountDue: number = 0;
        var myOtherAmounts: number = 0;

        if (this.Invoice.AmountInInvoiceCurrency != null) {
            myInvoiceAmount = this.Invoice.AmountInInvoiceCurrency;
        }

        if (this.Invoice.AmountDue != null) {
            myAmountDue = this.Invoice.AmountDue;
        }    

        if (this.Invoice.ARInvoiceTypeCode == "CD" || this.Invoice.ARInvoiceTypeCode == "CC") {
            if (myInvoiceAmount > 0) {
                myInvoiceAmount = myInvoiceAmount * -1;
            }

            if (myAmountDue > 0) {
                myAmountDue = myAmountDue * -1;
            }
        }

        myOtherAmounts = myInvoiceAmount - myAmountDue;
        if (this.EntityPM) {
            myOtherAmounts -= this.EntityPM.ForeignAmount;
            this.Invoice.AmountPaid = this.EntityPM.ForeignAmount;
        }

        this.InvoiceAmount = myInvoiceAmount;
        this.AmountDue = myAmountDue;
        this.OtherPaymentsAmount = AppTool.Round(myOtherAmounts, 2);
    }
    InitExchangeRate() {

        var myExchangeRate: number = null;

        if (this.EntityPM) {
            myExchangeRate = this.EntityPM.ExchangeRate;
        }

        else {
            if (this.CurrencyId == SessionLocator.LocalCurrencyId) {
                myExchangeRate = this.PaymentPM.PaymentCurrencyExchangeRate;
            }

            else {
                myExchangeRate = this.Invoice.InvoiceCurrencyExchangeRate;
            }
        }

        this.ExchangeRate = myExchangeRate;
    }

    private isControlEnabled: boolean = false;
    private isCurrencyMatched: boolean = false;
    private isAllowedToConnect: boolean = true;
    public CheckBoxVisibility: boolean = false;
    public NotMatchedVisibility: boolean = false;
    public IsAdvancedButtonVisible: boolean = false;
    public CheckBoxEnabled: boolean = true;
    SetUIProperties() {

        this.UIProperties.SetEnabled("AmountToPay", this.ObjectTableName, true);
        this.isControlEnabled = false;
        this.CheckBoxVisibility = false;
        this.NotMatchedVisibility = false;
        this.IsAdvancedButtonVisible = false;
        this.CheckBoxEnabled = true;

        this.SetUIProperties_CurrencyMatched();
        this.SetUIProperties_AllowedToConnect();

        if (this.isAllowedToConnect) {
            this.isControlEnabled = true;

            if (this.trigger.EntityPM.OpenAmount <= 0) {
                if (this.Invoice.ARInvoiceTypeCode == "IN") {
                    this.isControlEnabled = false;
                }
            }
        }

        if (this.isConnected) {
            this.isControlEnabled = true;
            this.CheckBoxVisibility = true;
            this.NotMatchedVisibility = false;
            if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE" && this.PaymentPM.SATTransferStatusCode != undefined && this.PaymentPM.SATTransferStatusCode != "NT" && this.PaymentPM.SATTransferStatusCode != "TE" &&
                !(this.PaymentPM.SATTransferStatusCode == "TD" && (this.PaymentPM.StatusCode == "DR"))) {
                this.CheckBoxEnabled = false;
            }
        }

        else {
            if (!this.isAllowedToConnect) {
                this.CheckBoxVisibility = false;
                this.NotMatchedVisibility = true;
                this.IsAdvancedButtonVisible = false;
            }

            else
                if (this.trigger.EntityPM.OpenAmount <= 0) {
                    if (this.Invoice.ARInvoiceTypeCode == "CD" || this.Invoice.ARInvoiceTypeCode == "CC") {
                        this.CheckBoxVisibility = true;
                    }

                    if (this.Invoice.ARInvoiceTypeCode == "IN") {
                        this.NotMatchedVisibility = false;
                    }
                }

                else {
                    this.CheckBoxVisibility = true;
                }
        }

        this.SetUIProperties_AmountPaidEnabled();
      this.SetLineColors();

    


    }
    SetUIProperties_CurrencyMatched() {
        this.isCurrencyMatched = false;

        if (this.PaymentPM.PaymentCurrencyId == this.CurrencyId) {
            this.isCurrencyMatched = true;
        }

        else {
            if (this.trigger.IsMultiCurrency) {
                if (this.PaymentPM.PaymentCurrencyId == SessionLocator.LocalCurrencyId) {
                    this.isCurrencyMatched = true;
                    this.IsAdvancedButtonVisible = true;
                }

                else if (this.CurrencyId == SessionLocator.LocalCurrencyId) {
                    this.isCurrencyMatched = true;
                    this.IsAdvancedButtonVisible = true;
                }
            }
        }
    }
  SetUIProperties_AllowedToConnect() {
    this.isAllowedToConnect = true;

    if (this.IsConnected == false) {
      if (this.Invoice.StatusCode == "DR") {
        this.isAllowedToConnect = false;
      }

      if (this.isCurrencyMatched == false) {
        this.isAllowedToConnect = false;
      }

      if (AppTool.IsNullOrZero(this.AmountDue)) {
        this.isAllowedToConnect = false;
      }

      if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
        if ((this.Invoice.MetodoPagoCode != this.PaymentPM.MetodoPagoCode && !AppTool.IsNullOrEmpty(this.PaymentPM.MetodoPagoCode)) || (this.PaymentPM.MetodoPagoCode == "PUE" && this.Invoice.StatusCode != "AD") && this.PaymentPM.OpenAmount >= this.Invoice.AmountPaid) {
          this.isAllowedToConnect = false;
        }
      }
    }
  }
    SetUIProperties_AmountPaidEnabled() {
        var isEnabled: boolean = true;

        if (this.Invoice.StatusCode == "DR") {
            isEnabled = false;
        }

        else if (this.isCurrencyMatched == false) {
            isEnabled = false;
        }

      if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
        if (this.Invoice.MetodoPagoCode == this.PaymentPM.MetodoPagoCode && (this.PaymentPM.MetodoPagoCode == "PUE")) {
          //this.UIProperties.SetEnabled("AmountToPay", this.ObjectTableName, false);
          isEnabled = false;
        }
      }
        this.UIProperties.SetEnabled("AmountPaid", this.ObjectTableName, isEnabled);
    }

    public CellColor: string = "#282E30";
    public CellBackground: string = "transparent";
    public CurrencyCodeColor: string = "transparent";
    public CurrencyCodeBackground: string = "transparent";
    public StatusBackground: string = "transparent";
    SetLineColors() {
        this.CellBackground = "transparent";
        this.CurrencyCodeColor = this.CellColor;
        this.CurrencyCodeBackground = "transparent";
        this.StatusBackground = "transparent";

        if (this.IsConnected) {
            this.CellBackground = "rgba(208, 224, 234, 0.4)";
        }

        else {
            if (!this.isCurrencyMatched) {
                this.CurrencyCodeColor = "rgb(255, 94,0)";
                this.CurrencyCodeBackground = "rgba(255, 171,3, 0.6)";
            }

            if (this.Invoice.StatusCode == "DR") {
                this.StatusBackground = "rgba(255, 171,3, 0.6)";
            }
        }
    }

    private isConnected: boolean = false;
    public get IsConnected() { return this.isConnected; }
    public set IsConnected(value: boolean) {
        if (this.isConnected != value) {
            this.isConnected = value;

            if (value == true) {
                this.GetSmallestAmount();
                this.Connect();
            }

            else {
                var itemPM = this.PaymentPM.PaymentInvoices.filter(d => d.ARInvoiceId == this.Id)[0];
                if (itemPM) {
                    this.PaymentPM.RemoveARPaymentInvoicePM(itemPM);
                    this.Disconnect();                    
                }
            }            
        }
    }

    public ConnectedAmount_INV: number = 0;
    public ConnectedAmount_PAY: number = 0;
    private GetSmallestAmount() {

        var invoiceAmount = this.AmountDue;
        var paymentAmount = this.PaymentPM.OpenAmount;

        var ConnectedAmountOfInvoice: number = 0;
        var ConnectedAmountOfPayment: number = 0;

        if (this.IsMultiCurrency && this.CurrencyId != this.PaymentPM.PaymentCurrencyId) {
            var invoiceAmountInPayment: number = 0;
            var paymentAmountInInvoice: number = 0;

            if (this.CurrencyId == this.LocalCurrencyId) {
                invoiceAmountInPayment = invoiceAmount / this.PaymentPM.PaymentCurrencyExchangeRate;
                paymentAmountInInvoice = paymentAmount * this.PaymentPM.PaymentCurrencyExchangeRate;
            }

            else {
                invoiceAmountInPayment = invoiceAmount * this.Invoice.InvoiceCurrencyExchangeRate;
                paymentAmountInInvoice = paymentAmount / this.Invoice.InvoiceCurrencyExchangeRate;
            }

            if (invoiceAmountInPayment <= paymentAmount) {
                ConnectedAmountOfInvoice = invoiceAmount;
                ConnectedAmountOfPayment = invoiceAmountInPayment;
            }

            else {
                ConnectedAmountOfInvoice = paymentAmountInInvoice;
                ConnectedAmountOfPayment = paymentAmount;
            }
        }

        else {
            if (invoiceAmount <= paymentAmount) {
                ConnectedAmountOfInvoice = invoiceAmount;
                ConnectedAmountOfPayment = invoiceAmount;
            }

            else {
                ConnectedAmountOfInvoice = paymentAmount;
                ConnectedAmountOfPayment = paymentAmount;
            }
        }

        this.ConnectedAmount_INV = ConnectedAmountOfInvoice;
        this.ConnectedAmount_PAY = ConnectedAmountOfPayment;
    }
    public GetConnectedAmount_PAY(invoiceAmount: number) {
        var myResult: number = 0;

        if (!AppTool.IsNullOrZero(invoiceAmount)) {
            if (this.IsMultiCurrency && this.CurrencyId != this.PaymentPM.PaymentCurrencyId) {
                if (this.CurrencyId == this.LocalCurrencyId) {
                    myResult = invoiceAmount / this.ExchangeRate;
                }

                else {
                    myResult = invoiceAmount * this.ExchangeRate;
                }
            }

            else {
                myResult = invoiceAmount;
            }
        }

        return myResult;
    }

    private amountDue: number = 0;
    get AmountDue() { return this.amountDue == null ? 0 : this.amountDue; }
    set AmountDue(value: number) {
        if (this.amountDue != value) {
            this.amountDue = AppTool.Round(value, 2);
        }
    }

    get AmountPaid() { return this.Invoice.AmountPaid == null ? 0 : this.Invoice.AmountPaid; }
    set AmountPaid(value: number) {
        if (this.Invoice.AmountPaid != value || this.InputHasError) {
            this.Invoice.AmountPaid = value;

            var inputErrors: number = 0;
            var inputEntry = value == null ? 0 : AppTool.Round(value, 2);
            var allowedAmount = AppTool.Round(this.InvoiceAmount - this.OtherPaymentsAmount, 2);

            this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, true, "");
            
            if (this.Invoice.ARInvoiceTypeCode == "CD" || this.Invoice.ARInvoiceTypeCode == "CC") {
                if (inputEntry > 0) {
                    var msg = TextCodeTranslator.Translate("ARPayment.M.OnlyMinusValue");
                    this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, false, msg);
                    inputErrors++;
                }

                else if (inputEntry < allowedAmount) {
                    var msg = TextCodeTranslator.Translate("ARPayment.M.AmountPaidNotLess") + " " + allowedAmount;
                    this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, false, msg);
                    inputErrors++;
                }
            }

            else {
                if (inputEntry < 0) {
                    var msg = TextCodeTranslator.Translate("ARPayment.M.CantPayMinusValue");
                    this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, false, msg);
                    inputErrors++;
                }

                else if (inputEntry > allowedAmount) {
                    var msg = TextCodeTranslator.Translate("ARPayment.M.AmountPaidLessOrEqual") + " " + allowedAmount;
                    this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, false, msg);
                    inputErrors++;
                }
            }

            this.InputHasError = (inputErrors > 0);
            this.trigger.UpdatePaymentInvoicesErrors();

            if (this.InputHasError) {
                return;
            }

            else {
                if (!this.IsConnected) {
                    if (inputEntry != 0 && inputEntry != null) {
                        this.ConnectedAmount_INV = inputEntry;
                        this.ConnectedAmount_PAY = this.GetConnectedAmount_PAY(inputEntry);
                        this.Connect();
                    }
                }

                else {
                    if (inputEntry == 0 || inputEntry == null) {
                        this.IsConnected = false;
                    }

                    else {
                    
                        this.Invoice.AmountPaid = inputEntry;
                        this.AmountDue = this.InvoiceAmount - this.OtherPaymentsAmount - inputEntry;

                        var itemPM = this.PaymentPM.PaymentInvoices.filter(d => d.ARInvoiceId == this.Id)[0];
                        if (itemPM) {
                            this.ConnectedAmount_INV = inputEntry;
                            this.ConnectedAmount_PAY = this.GetConnectedAmount_PAY(inputEntry);

                            itemPM.ForeignAmount = this.ConnectedAmount_INV;
                            itemPM.PaymentAmount = this.ConnectedAmount_PAY

                            if (this.CurrencyId == this.LocalCurrencyId) {
                                itemPM.LocalAmount = itemPM.ForeignAmount;
                            }

                            else {
                                itemPM.LocalAmount = AppTool.Round(itemPM.ForeignAmount * itemPM.ExchangeRate, 2);
                            }
                        }

                        this.UpdatePayment();
                    }
                }
            }
        }
    }

    private Connect() {
        this.isConnected = true;

        var itemPM: ARPaymentInvoicePM = new ARPaymentInvoicePM(this.PaymentPM);
        itemPM.Tenant = SessionLocator.TenantPM.Id;
        itemPM.ARInvoiceId = this.Id;
        itemPM.ARPaymentId = this.PaymentPM.Id;
        itemPM.ARInvoiceNumber = this.InvoiceNumber;
        itemPM.ForeignCurrencyId = this.CurrencyId;
        itemPM.ExchangeRate = this.ExchangeRate;
        itemPM.ForeignAmount = this.ConnectedAmount_INV == null ? 0 : AppTool.Round(this.ConnectedAmount_INV, 2);
        itemPM.PaymentAmount = this.ConnectedAmount_PAY == null ? 0 : AppTool.Round(this.ConnectedAmount_PAY, 2);
        itemPM.ARInvoiceMetodoPagoCode = this.Invoice.MetodoPagoCode;
        itemPM.ARInvoiceTransferStatusCode = this.TransferStatusCode;

        if (this.CurrencyId == this.LocalCurrencyId) {
            itemPM.LocalAmount = itemPM.ForeignAmount;
        }

        else {
            itemPM.LocalAmount = AppTool.Round(itemPM.ForeignAmount * itemPM.ExchangeRate, 2);
        }

        this.PaymentPM.AddARPaymentInvoicePM(itemPM);

        var invoiceAmount: number = this.InvoiceAmount;
        var invoiceAmountPaid: number = itemPM.ForeignAmount;
        var invoiceAmountDue: number = invoiceAmount - this.OtherPaymentsAmount - invoiceAmountPaid;

        if (this.Invoice.ARInvoiceTypeCode == "CD" || this.Invoice.ARInvoiceTypeCode == "CC") {
            if (invoiceAmountDue > 0) {
                invoiceAmountDue = invoiceAmountDue * -1;
            }

            if (invoiceAmountPaid > 0) {
                invoiceAmountPaid = invoiceAmountPaid * -1;
            }
      }

      if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
        if (this.Invoice.MetodoPagoCode == this.PaymentPM.MetodoPagoCode && (this.PaymentPM.MetodoPagoCode == "PUE" && this.Invoice.StatusCode == "AD")) {
          this.AmountPaid = invoiceAmountPaid = this.AmountDue;
          invoiceAmountDue = 0;
        }

        
      }

        this.AmountDue = AppTool.Round(invoiceAmountDue, 2);
        this.Invoice.AmountPaid = AppTool.Round(invoiceAmountPaid, 2);
        
        this.SetLineColors();
        this.UpdatePayment();
    }
    private Disconnect() {
        this.AmountDue = AppTool.Round(this.InvoiceAmount - this.OtherPaymentsAmount, 2);
        this.Invoice.AmountPaid = 0;

        this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, true, "");
        this.InputHasError = false;
        this.ConnectedAmount_INV = 0;
        this.ConnectedAmount_PAY = 0;

        this.SetLineColors();
        this.UpdatePayment();
    }

    UpdatePayment() {
        var paymentAmount = this.PaymentPM.AmountInPaymentCurrency == null ? 0 : this.PaymentPM.AmountInPaymentCurrency;

        var allPaidAmounts = 0;
        this.PaymentPM.PaymentInvoices.forEach(item => {
            var itemPaidAmount: number = 0;

            if (item.ForeignCurrencyId == this.PaymentPM.PaymentCurrencyId) {
                itemPaidAmount = item.ForeignAmount;
            }

            else {
                itemPaidAmount = item.LocalAmount / item.ExchangeRate;
            }

            allPaidAmounts += itemPaidAmount;
        });

        var openAmount = paymentAmount - allPaidAmounts;

        this.trigger.EntityPM.OpenAmount = (openAmount == null) ? 0 : AppTool.Round(openAmount, 2);
        this.trigger.UpdatePaymentInvoicesErrors();
        this.trigger.UpdateSummary();
        this.trigger.ComputeOpenAmount();
        this.trigger.SetUIProperties_Invoices();

        this.trigger.ItemsSource.Collection.forEach(item => {
            item.SetUIProperties();
      });

      if (this.PaymentPM.PaymentInvoices.length > 0) {
        this.PaymentPM.UIProperties.SetEnabled("MetodoPagoCode", "ARPayment", false);
      }
      else {
        this.PaymentPM.UIProperties.SetEnabled("MetodoPagoCode", "ARPayment", true);
      }
    }
    AdvancedClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 300;
        logWindow.Height = 185;
        logWindow.Title = "Edit Amount to Pay";
        logWindow.WindowArgs = this;
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.ExchangeRate = comp['ExchangeRate'];

                    var inputEntry_INV = comp['ForeignAmount'];
                    var inputEntry_PAY = comp['PaymentAmount'];

                    this.InputHasError = false;
                    this.trigger.UpdatePaymentInvoicesErrors();

                    if (!this.IsConnected) {
                        if (inputEntry_INV != 0 && inputEntry_INV != null) {
                            this.ConnectedAmount_INV = inputEntry_INV;
                            this.ConnectedAmount_PAY = inputEntry_PAY;
                            this.Connect();
                        }
                    }

                    else {
                        if (inputEntry_INV == 0 || inputEntry_INV == null) {
                            this.IsConnected = false;                            
                        }

                        else {

                            this.ConnectedAmount_PAY = inputEntry_PAY;

                            this.Invoice.AmountPaid = inputEntry_INV;
                            this.AmountDue = this.InvoiceAmount - this.OtherPaymentsAmount - inputEntry_INV;

                            var itemPM = this.PaymentPM.PaymentInvoices.filter(d => d.ARInvoiceId == this.Id)[0];
                            if (itemPM) {
                                itemPM.ForeignAmount = inputEntry_INV;
                                itemPM.PaymentAmount = inputEntry_PAY;
                                itemPM.ExchangeRate = this.ExchangeRate;

                                if (this.CurrencyId == this.LocalCurrencyId) {
                                    itemPM.LocalAmount = itemPM.ForeignAmount;
                                }

                                else {
                                    itemPM.LocalAmount = AppTool.Round(itemPM.ForeignAmount * itemPM.ExchangeRate, 2);
                                }
                            }

                            this.UpdatePayment();
                        }
                    }
                }
            });
        });

        logWindow.Show('./InvoiceModules/ARPayment/Components/EditTabs/EditMultiCurrency');
    }
}
