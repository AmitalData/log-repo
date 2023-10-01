import { Component, OnInit, OnDestroy, ChangeDetectorRef}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {APPaymentPM} from '../../../../Invoice/EntityPMs/APPaymentPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, DateTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {APInvoiceListService} from '../../../../Invoice/Services/StandardLists/APInvoiceListService';
import {APInvoiceList} from '../../../../Invoice/EntityLists/APInvoiceList';
import {APPaymentInvoicePM} from '../../../../Invoice/EntityPMs/APPaymentInvoicePM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {CurrencyRatesService, LastRate} from '../../../../Common/Services/CurrencyRatesService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {AccountingPaymentMethodList} from '../../../../Invoice/EntityLists/AccountingPaymentMethodList';
import {AccountingPaymentMethodListService} from '../../../../Invoice/Services/StandardLists/AccountingPaymentMethodListService';
import {GLAccountWithholdingTaxExtendedPMService} from  '../../../../Accounting/Services/Others/GLAccountWithholdingTaxExtendedService';
import {BankAccountPMService} from '../../../../Accounting/Services/StandardPMs/BankAccountPMService';
import {BankAccountPM} from  '../../../../Accounting/EntityPMs/BankAccountPM';
import {FullAccountingSettingPM} from '../../../../Accounting/EntityPMs/FullAccountingSettingPM';
import { FullAccountingSettingPMService } from '../../../../Accounting/Services/StandardPMs/FullAccountingSettingPMService';
import { PaymentChequeExtendedPMService } from '../../../../Accounting/Services/ExtendedPMs/PaymentChequeExtendedPMService';
import { GLAccountListService } from '../../../../Accounting/Services/StandardLists/GLAccountListService';
import { GLAccountList } from '../../../../Accounting/EntityLists/GLAccountList';
import { LedgerTransactionPM } from 'Accounting/EntityPMs/LedgerTransactionPM';
import { LedgerTransactionExtendedListService } from './../../../../Accounting/Services/ExtendedLists/LedgerTransactionExtendedListService';
import { ReconciliationExtendedPMService } from './../../../../Accounting/Services/ExtendedPMs/ReconciliationExtendedPMService';
import { JournalExtendedPMService } from 'Accounting/Services/ExtendedPMs/JournalExtendedPMService';
import { JournalPM } from 'Accounting/EntityPMs/JournalPM';

@Component({
    templateUrl: './APPaymentDetailsTabComponent.html',
})

export class APPaymentDetailsTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: APPaymentPM = null;
    public ObjectTableName = "APPayment";
    public DataContext = this;
    public ItemsSource: ObservableCollection;
    private APInvoiceListService: APInvoiceListService = new APInvoiceListService();
    public EnableNegativeOffsetAPPayments: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    get IsNegativeAmountEnabled() { return this.EnableNegativeOffsetAPPayments == true && this.PaymentMethodCode == "FS" ? true : false; }
    public isRTL: boolean = false;
    public IsFullAccounting: boolean = false;
    public IsNoVendorTax: boolean = false;
    public PaymentChequeActivated: boolean = true;
    public IsMultiCurrency: boolean = false;
    public LocalCurrencyCode = "";
    ReconcileInternalTrans:LedgerTransactionPM[];
    private CurrentSession = SessionLocator.SelectedSession;
    public fullAccountingSettingPMService: FullAccountingSettingPMService = new FullAccountingSettingPMService();
    PaymentChequePMService: PaymentChequeExtendedPMService = new PaymentChequeExtendedPMService();
    _LedgerTransactionExtendedListService: LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService();
	_ReconciliationExtendedPMService: ReconciliationExtendedPMService = new ReconciliationExtendedPMService();
    IsChequeLinkVisibile: boolean = false;
    DisplayFieldsFromList:string;
    DisplayLocalFieldsFromList:string;
    VendorLovSizeForFullAccounting:number;
    _JournalExtendedPMService: JournalExtendedPMService = new JournalExtendedPMService();
    constructor(private entityArgs: EntityArgs, private _entityResourceService: EntityResourceService, private cd: ChangeDetectorRef) {
        super();

        if (ObjectsLocator.GlobalSetting) {
            this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
            this.IsSplitComponentOpened  = this.isRTL;

        }
        this.IsFullAccounting = SessionLocator.TenantPM.AccountingActivated;
        this.InitializeBillToLov()
        this.EntityPM = entityArgs.EntityPM;
        this.ReconcileInternalTrans = this.EntityPM.ReconcileInternalTrans;
        console.log('ayed', entityArgs)
        this.ItemsSource = new ObservableCollection([]);
        this.EnableNegativeOffsetAPPayments = ObjectsLocator.AccountingSettingPM.EnableNegativeOffsetAPPayments;
        this.GetFullAccountingSettings();

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ChequeOrPaymentRef) &&  this.EntityPM.AutomaticPaymentCheque) {
            this.IsChequeLinkVisibile = true;

        }

        if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "EnableMultiCurrency")) {
            if (ObjectsLocator.AccountingSettingPM.EnableMultiCurrencyAPPayments) {
                this.IsMultiCurrency = true;
            }
        }

        this.InitializeServices();
        this.ApplyViewModel();
        this.Listen();

        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }

        this.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
    }

    private InitializeBillToLov() {
        if (this.IsFullAccounting) {
            this.DisplayFieldsFromList = "Code,CalculatedEnglishName,GLAccountDisplayNumber,CityName,CountryCode,PartnerTypeName";
            this.DisplayLocalFieldsFromList = "Code,CalculatedLocalName,GLAccountDisplayNumber,CityName,CountryCode,PartnerTypeName";
            this.VendorLovSizeForFullAccounting = 550;
        }
    }
    public ShowSplitButton: boolean = false;
    IsSplitComponentOpened: boolean;

    token: any;
    SplitTooltip: string = "";
    SplitButtonClicked() {
        this.IsSplitComponentOpened = !this.IsSplitComponentOpened;
        if (!this.IsSplitComponentOpened) {
            this.AutomaticPaymentCheque=!this.isRTL;
            this.PaymentChequeActivated = !this.isRTL;
            this.SplitTooltip = this.isRTL ? TextCodeTranslator.Translate("Accounting.General.O.PaymentChequeAutomaticOption") : "";
            if (AppTool.IsNullOrEmpty(this.ChequeOrPaymentRef)) {
                this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, this.isRTL);
            }
        }
        else {
            this.PaymentChequeActivated = this.isRTL;
            this.AutomaticPaymentCheque = this.isRTL;
            this.SplitTooltip = this.isRTL ? "" : TextCodeTranslator.Translate("Accounting.General.O.PaymentChequeAutomaticOption") ;
            this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, !this.isRTL);
            this.ChequeOrPaymentRef = null;
        }

        this.ShowSplitButton = true;
       // this.cd.detectChanges();


    }
    entityId: string;
    ViewPaymentCheque() {
        if(this.IsFullAccounting){
            this.PaymentChequePMService.GetPaymentChequeByPaymentIdAndChequeNumber(this.EntityPM.ChequeOrPaymentRef, this.EntityPM.Id).subscribe((myResult:ServiceResponse) => {
                var myResponse: ServiceResponse = myResult;
                if (myResponse != null) {

                    var res = myResponse.Result;
                    var entityId = res.Id;
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: "PaymentCheque", BackButtonLabel: "PaymentCheque" });
                            cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                                this.BuildScreenData();
                            });
                        });
                }

            });

        }
    }

    private FullAccountingSetting: FullAccountingSettingPM = new FullAccountingSettingPM();
    public GetFullAccountingSettings() {
        this.fullAccountingSettingPMService.get(SessionLocator.TenantPM.Id.toString()).subscribe((myResult: any) => {
            var myResponse: ServiceResponse = myResult;
            if (myResponse != null) {

                var res = myResponse.Result;
                this.FullAccountingSetting = res;

                if (this.FullAccountingSetting != null) {
                    this.PaymentChequeActivated = this.FullAccountingSetting.IsPaymentChequesActivated && this.IsFullAccounting;
                    this.SetUIProperties_Cheque();
                }
            }
        });
    }

    private SessionEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private BackCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "ExternalAPPaymentChanged") {
                    this.UpdateSummary();
                    this.ComputeOpenAmount();
                }
            });
            let isSaveCompleted = false;
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    isSaveCompleted = isSaveSuccess;
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.RefreshScreen();
                    this.checkLedgerCreated();
                }

                if (this.RequestedCommandCode) {
                    this.ApplyRequestedCommand();
                }
            });

            this.BackCompletedEvent = this.entityArgs.EditComponent.BackCompleted.subscribe((isBackCompleted: boolean) => {
                if (isBackCompleted && isSaveCompleted && this.entityArgs.EditComponent.EntityPM.StatusCode == "AD") {
                    if (this.IsFullAccounting && this.ReconcileInternalTrans && this.ReconcileInternalTrans.length > 0) {
                        const paymentNo = this.entityArgs.EditComponent.EntityPM.PaymentNo;
                        this.CurrentSession.FireEvent({Name: "InternalReconcileAPPaymentCreated", PaymentNumber: paymentNo});
                    }
                }
            });



            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.RefreshScreen();
                }
            });
        }
    }
    ngOnInit() {
        this.LoadPaymentMethods();
        this.LoadCurrencies();
        this.checkLedgerCreated();
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    vendor: CardList;
    VendorChanged(vednor:CardList){
        this.vendor = vednor;
        this.LoadTaxPercentage();

    }

    private IsTaxUpdated = false;
    private LoadTaxPercentage() {
        if (this.IsFullAccounting)
        {
            const nonIsraeliVendor = this.vendor ? this.vendor.CountryCode != "IL" : false;
            if(nonIsraeliVendor) {
                this.TaxDeductionPercentage = 0;
            } else {

                this.GLAccountWithholdingService.GetDeductionPercentage(this.VendorId, this.EntityPM.RegisterDate).subscribe((myResult:any) => {
                    var myResponse: ServiceResponse = myResult;
                    if (!myResponse.HasError) {
                        if (AppTool.IsNullOrEmpty(this.EntityPM.Id) || this.IsTaxUpdated) {
                            this.IsNoVendorTax = myResponse.Result.IsDefault;
                            this.TaxDeductionPercentage = myResponse.Result.Percentage;
                        }
                        else {
                            this.IsNoVendorTax = myResponse.Result.IsDefault;
                        }
                    }
                    else {
                        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                });
            }

        }
    }

    private ApplyViewModel() {
        this.RefreshFields();
        this.SetUIProperties();
        this.BuildScreenData();
    }

    CardListService: CardListService;
    _GLAccountListService: GLAccountListService = new GLAccountListService();
    PartnersDomainService: PartnersDomainService;
    GLAccountWithholdingService: GLAccountWithholdingTaxExtendedPMService;
    BankAccountPMService: BankAccountPMService;
    InitializeServices() {
        this.CardListService = new CardListService();
        this.PartnersDomainService = new PartnersDomainService();
        this.GLAccountWithholdingService = new GLAccountWithholdingTaxExtendedPMService();
        this.BankAccountPMService = new BankAccountPMService();
    }

    //RefreshFields
    private RefreshFields() {
        this.ComputeRelativeRateDate();
        this.GetRateIsEnabled();
    }
    public RateIsEnabled: boolean = false;
    GetRateIsEnabled() {
        this.SetUIProperties_ExchangeRate();
    }

    // SetUIProperties
    private SetUIProperties() {
        this.SetUIProperties_Cheque();
        this.SetUIProperties_Invoices();
        this.SetUIProperties_CreditCard();

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

            if (this.IsFullAccounting) {
                this.UIProperties.SetEnabled("BankAccountId", this.ObjectTableName, false);
            }
        }
        else {

            if (this.EntityPM.IsCreatedFromInvoiceSide) {
                this.UIProperties.SetEnabled("PartnerId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("VendorAddressId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, false);
            }

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
            if (this.IsFullAccounting) {
                this.UIProperties.SetEnabled("BankAccountId", this.ObjectTableName, true);
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.VendorId)) {
                this.UIProperties.SetEnabled("VendorAddressId", this.ObjectTableName, false);
            }

            if (this.EntityPM.StatusCode == "VD") {
                this.UIProperties.SetEnabled("PrintNotes", this.ObjectTableName, false);
            }
        }

        this.SetUIProperties_FullAccounting();
    }

    public SetUIProperties_Invoices() {
        if (!this.IsScreenEnabled) {
            this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VendorAddressId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("VendorAddressId", this.ObjectTableName, true);

            if (this.EntityPM.PaymentInvoices.length > 0) {
                this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("VendorAddressId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, false);
            }
        }

        this.SetUIProperties_ExchangeRate();
    }
    private SetUIProperties_ExchangeRate() {
        var isEnabled: boolean = false;

        if (this.IsScreenEnabled) {
            if (FeatureLocator.HasFeaturePermession("APPayment", "APPaymentEditExchangeRate")) {
                if (this.PaymentCurrencyId) {
                    if (this.PaymentCurrencyId != SessionLocator.TenantPM.CurrencyId) {
                        if (this.EntityPM.PaymentInvoices.length == 0) {
                            if (!this.EntityPM.IsCreatedFromInvoiceSide) {
                                isEnabled = true;
                            }
                        }
                    }
                }
            }
        }

        this.RateIsEnabled = isEnabled;
        this.UIProperties.SetEnabled("PaymentCurrencyExchangeRate", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("ExchangeRateDate", this.ObjectTableName, isEnabled);
    }
    private SetUIProperties_Cheque() {
        this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, false);
        this.UIProperties.SetVisibility("Bank", this.ObjectTableName, false);
        this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, false);
        this.UIProperties.SetVisibility("Account", this.ObjectTableName, false);

        if (!AppTool.IsNullOrEmpty(this.PaymentMethodCode)) {
            if (this.PaymentMethodCode == "CH" && !this.PaymentChequeActivated) {
                if (AppTool.IsNullOrEmpty(this.ChequeOrPaymentRef)) {
                    this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, true);
                }
            }

            if (this.PaymentMethodCode == "CH" || this.PaymentMethodAddedManually) {
                this.UIProperties.SetVisibility("Bank", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("Account", this.ObjectTableName, true);
            }
        }
    }
    private SetUIProperties_CreditCard() {
        this.UIProperties.SetVisibility("CreditCardTypeId", this.ObjectTableName, false);
        this.CreditCardTypeIdVisibility = false;
        this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, false);
        if (this.PaymentMethodCode == "CC") {
            if (AppTool.IsNullOrEmpty(this.CreditCardTypeId)) {
                this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, true);
            }
            this.UIProperties.SetVisibility("CreditCardTypeId", this.ObjectTableName, true);
            this.CreditCardTypeIdVisibility = true;
        }
    }

    // BuildScreenData
    private BuildScreenData() {
        if (this.IsFullAccounting && this.EntityPM.ReconcileInternalTrans && this.ReconcileInternalTrans.length > 0) {
            this.VendorId = this.EntityPM.VendorId;
            this.AmountInPaymentCurrency = this.EntityPM.AmountInPaymentCurrency;
            this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VendorAddressId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AmountInPaymentCurrency", this.ObjectTableName, false);
        }
        if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
            this.LoadCurrencyRates();
        }

        else {
            this.LoadData();
        }
    }

    //Refresh Screen
    private RefreshScreen() {
        this.SetUIProperties();
        this.LoadData();
        this.CollapseSplitButton();
    }
    ChequeNumber: string;
    IsSplitButtonVisibile: boolean = false;
    CollapseSplitButton() {
        if (this.PaymentMethodCode == "CH" && this.AutomaticPaymentCheque) {
            this.IsSplitButtonVisibile = false;

        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ChequeOrPaymentRef) && this.EntityPM.AutomaticPaymentCheque) {
            this.IsChequeLinkVisibile = true;

        }
    }
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
    //LoadCurrencyRates
    public LastRatesList: LastRate[] = [];
    LoadCurrencyRates() {
        var loadingDate = this.RegisterDate;
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

   
    async LoadCurrencyRatesOnTheFly() {
        var loadingDate = this.RegisterDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

        var myService: CurrencyRatesService = new CurrencyRatesService();
        await new Promise(res =>
            myService.GetCurrenciesExchangeRateByValueDate(SessionLocator.TenantPM.CurrencyId, loadingDate).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.LastRatesList = myResponse.Result;
                }
                res();
               // this.LoadData();
            })
        );
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

    async GetCurrencyRate(currencyId: string): Promise<number> {
        var result = null;

        if (AppTool.IsNullOrEmpty(currencyId)) {
            result = null;
        }

        else {
            if (currencyId == SessionLocator.TenantPM.CurrencyId) {
                result = 1;
            }

            else {
                if (this.LastRatesList.length == 0) {
                    await this.LoadCurrencyRatesOnTheFly();
                }

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

    // Load Data
    public IsDataLoaded: boolean = false;
    private searchText: string = "";
    SearchTextKeyUp(args: any) {
        this.searchText = args;
        this.LoadData();
    }

    private createdFromInvoiceLine: APPaymentInvoiceArgs;
    private ConnectedList: APInvoiceList[] = [];
    private IsMatchedList: APInvoiceList[] = [];
    private ReconcileInvoiceList: APInvoiceList[] = [];
    LoadData() {
        this.ItemsSource.Clear();
        if(this.EntityPM.ReconcileInternalTrans.length > 0) {
            var invoiceNumbers="";
            this.EntityPM.ReconcileInternalTrans.forEach(element => {
                invoiceNumbers=invoiceNumbers+element.SourceNumber+",";
              
            });
            invoiceNumbers = invoiceNumbers.substring(0,invoiceNumbers.length-1)
            this.LoadPaymentInvoices(invoiceNumbers);
        }
        if (!AppTool.IsNullOrEmpty(this.EntityPM.VendorId) && this.EntityPM.StatusCode != "VD") {
            if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                this.LoadPaymentInvoices_IsMatched();
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
    LoadPaymentInvoices_Connected() {

        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 1000;
        filters.SortBy = "DueDate";
        filters.SortDirection = "Descending";

        var searchValue = null;
        if (!AppTool.IsNullOrEmpty(this.searchText)) {
            searchValue = AppTool.IsNullOrEmpty(this.searchText.trim()) ? null : this.searchText;
        }

        filters.addAdditionalFilter("APPaymentInvoicesSearch", searchValue, null, null, "Contains", true, false, false, "string");
        filters.addAdditionalFilter("APPaymentInvoicesConnected", this.EntityPM.Id, null, null, "Contains", true, false, false, "string");

        this.APInvoiceListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
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
        filters.SortBy = "DueDate";
        filters.SortDirection = "Descending";

        filters.addAdditionalFilter("VendorId", this.EntityPM.VendorId, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("StatusCode", "WA,AD,PP,PD", null, null, "InList", false, true, false, "string");
        filters.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "Boolean");

        var searchValue = null;
        if (!AppTool.IsNullOrEmpty(this.searchText)) {
            searchValue = AppTool.IsNullOrEmpty(this.searchText.trim()) ? null : this.searchText;
        }

        filters.addAdditionalFilter("APPaymentInvoicesSearch", searchValue, null, null, "Contains", true, false, false, "string");

        this.APInvoiceListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.IsMatchedList = myResponse.Result;
            }

            this.FillBaselist();
        });
    }
    FillBaselist() {
        this.ItemsSource.Clear();
        var connectedList: APPaymentInvoiceArgs[] = [];
        var unConnectedMatchedList: APPaymentInvoiceArgs[] = [];
        var unConnectedListNotMatched: APPaymentInvoiceArgs[] = [];
        var itemsCollection: APPaymentInvoiceArgs[] = [];
        
     
         
            if (this.ReconcileInvoiceList.length > 0) {
                this.ReconcileInvoiceList.forEach(item => {
                  
                        connectedList.push(new APPaymentInvoiceArgs(item, this));
                });
    
                connectedList.sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
                    itemsCollection.push(item);
                });
            }
            
       
        if (this.ConnectedList.length > 0) {
            this.ConnectedList.forEach(item => {
                if (this.EntityPM.PaymentInvoices.filter(d => d.APInvoiceId == item.Id)[0]) {

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

                    connectedList.push(new APPaymentInvoiceArgs(item, this));
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

                        if (isCurrencyMatched == false || item.StatusCode == "WA") {
                            unConnectedListNotMatched.push(new APPaymentInvoiceArgs(item, this));
                        }

                        else {
                            unConnectedMatchedList.push(new APPaymentInvoiceArgs(item, this));
                        }
                    }
                }
            });

            unConnectedMatchedList.filter(f => f.CurrencyId == this.PaymentCurrencyId).sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
                if (this.EntityPM.IsCreatedFromInvoiceSide && !AppTool.IsNullOrEmpty(this.EntityPM.CreatedFromInvoiceId) && item.Invoice.Id == this.EntityPM.CreatedFromInvoiceId) {
                    item.IsConnected = true;
                    this.createdFromInvoiceLine = item;
                }

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
        if(this.IsFullAccounting) {
                // this.CurrentSession.StartBusyIndicatorLoading();
                this.CardListService.getSingle(this.EntityPM.VendorId).subscribe((myResult:any) => {
                    var myResponse: ServiceResponse = myResult;
                    // this.CurrentSession.StopBusyIndicator();
                    if (!myResponse.HasError) {
                        var cardList: CardList = myResponse.Result;
                        if (cardList != null) {
                            this.GLAccountId = cardList.GLAccountId;
                            this.GetTransactionsForAPPayment();
                        }
                    }
                });
        }
    }
    LoadPaymentInvoices(invoiceNumbers) {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 1000;
        filters.SortBy = "DueDate";
        filters.SortDirection = "Descending";

        //filters.addAdditionalFilter("VendorId", this.EntityPM.VendorId, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("InvoiceNumber", invoiceNumbers, null, null, "InList", false, true, false, "string");

      


        this.APInvoiceListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.ReconcileInvoiceList = myResponse.Result; 
                this.FillBaselist();
            }
           
           
        });
    }
    // Vendor Properties
    get VendorId() {        
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.VendorId;
    }
    set VendorId(value: string) {       
        if (!this.EntityPM.IsCreatedFromInvoiceSide) {
            if (this.EntityPM != null) {
                if (this.EntityPM.VendorId != value || (this.IsFullAccounting && this.EntityPM.ReconcileInternalTrans && this.ReconcileInternalTrans.length > 0)) {
                    this.EntityPM.VendorId = value;
                    this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, true);
                    this.PaymentCurrencyId = SessionLocator.TenantPM.CurrencyId;
                    if (AppTool.IsNullOrEmpty(this.EntityPM.VendorId)) {
                        this.UIProperties.SetEnabled("VendorAddressId", this.ObjectTableName, false);
                    }
                    else {
                        this.UIProperties.SetEnabled("VendorAddressId", this.ObjectTableName, true);
                    }
                    this.GetCardProperties();
                    this.LoadData();
                    this.LoadTaxPercentage();
                    this.IsTaxUpdated = true;

                }
            }
        }
    }

    public GLAccountId: string;
    public BillToId: string;
    GetCardProperties() {
        if (AppTool.IsNullOrEmpty(this.EntityPM.VendorId)) {
            this.FillDataFromCardList(null);
            this.EntityPM.VendorPartnerTypeId = null;
        }
        else {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.CardListService.getSingle(this.EntityPM.VendorId).subscribe((myResult:any) => {
                var myResponse: ServiceResponse = myResult;
                this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    var cardList: CardList = myResponse.Result;
                    if (cardList != null) {
                        this.FillDataFromCardList(cardList);
                    }
                }
            });
        }
    }
    private FillDataFromCardList(list: CardList) {
        if (list == null) {
            this.GLAccountId = null;
            this.BillToId =null;
            this.vendorGLAccount = null;
            this.deductionFileNumber = null;
            this.VendorAddressId = null;
            this.PaymentCurrencyId = SessionLocator.TenantPM.CurrencyId;
            this.VendorAddressId = null;
            this.EntityPM.VendorBankAddress = null;
            this.EntityPM.VendorIBANNumber = null;
            this.EntityPM.VendorBankAccountNumber = null;
            this.EntityPM.VendorSwift = null;
            this.EntityPM.VendorBankName = null;
        }
        else {
            this.GLAccountId = list.GLAccountId;
            this.BillToId = list.BillToId;
            this.EntityPM.VendorBankAddress = list.BankAddress;
            this.EntityPM.VendorIBANNumber = list.IBANNumber;
            this.EntityPM.VendorBankAccountNumber = list.AccountNumber;
            this.EntityPM.VendorSwift = list.Swift;
            this.EntityPM.VendorBankName = list.BankName;
            if (!AppTool.IsNullOrEmpty(list.InvoiceCurrencyId)) {
                this.PaymentCurrencyId = list.InvoiceCurrencyId;
            }
            if (SessionLocator.LoggedUserPM.DontShowLocal) {
                this.EntityPM.VendorName = list.EnglishName;
            }
            else {
                this.EntityPM.VendorName = list.LocalName;
            }
            this.EntityPM.VendorPartnerTypeId = list.PartnerTypeId;
            this.LoadAddressAndGeneralTab();


            if(this.IsFullAccounting)
                this.GetConnectedGLAccount();
            else 
            this.GetConnectedBillTo();
            

        }
    }


    GetConnectedBillTo() {
        if (this.BillToId)
        {

            this.CurrentSession.StartBusyIndicatorLoading();
            this.CardListService.getSingle(this.BillToId).subscribe((myResult:any) => {
                var myResponse: ServiceResponse = myResult;
                this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    var cardList: CardList = myResponse.Result;
                    if (!AppTool.IsNullOrEmpty(cardList.InvoiceCurrencyId)) {
                        this.PaymentCurrencyId = cardList.InvoiceCurrencyId;
                        
                    }

                }
            });
        }
    }

    vendorGLAccount: GLAccountList;
    deductionFileNumber: string;
    GetConnectedGLAccount() {
        if (this.GLAccountId)
        {
            this.CurrentSession.StartBusyIndicatorLoading();
            this._GLAccountListService.getSingle(this.GLAccountId).subscribe((myResult:any) => {
                console.log("[_GLAccountListService.getSingle]", myResult);
                this.CurrentSession.StopBusyIndicator();

                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    var gla: GLAccountList = myResponse.Result;
                    this.vendorGLAccount = gla;
                    this.deductionFileNumber = gla ? gla.DeductionFileNumber : null;
                    this.EntityPM.ExcludeFromDeductionReport = gla.ExcludeFromDeductionReport;
                    this.EntityPM.VendorGLAccountId = gla.Id;
                    if (!gla.IsMultiCurrency) {
                        this.PaymentCurrencyId = gla.CurrencyId;
                    }
                }
            });
        }else{
            this.vendorGLAccount = null;
            this.deductionFileNumber = null;
            this.EntityPM.ExcludeFromDeductionReport = false;
              this.EntityPM.VendorGLAccountId = null;
        }
    }
    invoicesLedgerTransactions: any[] = [];
    paymentLedgerTransactions: string[] = [];
    GetTransactionsForAPPayment()
	{
		if (this.GLAccountId) {
			this.invoicesLedgerTransactions = [];
            this.paymentLedgerTransactions = [];
			// this.CurrentSession.StartBusyIndicatorLoading();

            this._LedgerTransactionExtendedListService.GetTransactionsForAPPayment(this.EntityPM.Id, this.GLAccountId, this.EntityPM.PaymentCurrencyId).subscribe((myResult: ServiceResponse) => {
                // this.CurrentSession.StopBusyIndicator();
				var mm: ServiceResponse = myResult;
				if (!mm.HasError) {

					var transactions = mm.Result;
					if (transactions != null) {
						for (var i = 0; i < transactions.length; i++) {
							this.invoicesLedgerTransactions.push(transactions[i]);
						}
					}
                    if(this.invoicesLedgerTransactions && this.invoicesLedgerTransactions.length > 0 && this.invoicesLedgerTransactions[0] && this.invoicesLedgerTransactions[0].Reference3) {

                        this.paymentLedgerTransactions = this.invoicesLedgerTransactions[0].Reference3.split(',');
                    }
                    this.ItemsSource.Collection.forEach(item => {
                        var ledgerTransaction = this.invoicesLedgerTransactions.filter(x=>x.Reference1 == item.InvoiceNumber)[0];
                        if(ledgerTransaction) {
                            item.CheckBoxEnabled = true;
                            item.RecoNumber = ledgerTransaction.RecoNumber;
                            if(ledgerTransaction.RecoNumber != null && ledgerTransaction.RecoNumber.length > 0) {
                                var items = this.paymentLedgerTransactions.filter(value => ledgerTransaction.RecoNumber.split(',').includes(value));
                                if(items.length > 0) {
                                    item.CheckBoxEnabled = false;
                                }
                            }
                        }
                    });
				}
			});

		} else {
			console.error("No GLAccount for this payment ", this.EntityPM);
		}
	}



    OpenReco(recoNumber)
	{
		if (!AppTool.IsNullOrEmpty(recoNumber)) {

			this.CurrentSession.StartBusyIndicatorLoading()

			this._ReconciliationExtendedPMService.getByNumber(recoNumber)
				.subscribe((myResult: ServiceResponse) =>
				{
					this.CurrentSession.StopBusyIndicator()

					var mm: ServiceResponse = myResult;
					if (!mm.HasError) {

						var reco: any = mm.Result;
						var recoId = reco.Id;
						SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
							.then(cmpRef =>
							{
								cmpRef.instance.ComponentRef = cmpRef;
								cmpRef.instance.Run({ EntityId: recoId, ObjectTableName: 'Reconciliation' });
                                cmpRef.instance.BackCompleted.subscribe(bk =>
                                    {
                                        this.GetTransactionsForAPPayment();
                                    });
							});
					}
					else {

					}
				});




		}
	}
    _IsDisplayOnly = false;
	public get IsDisplayOnly(): boolean
	{
		return this._IsDisplayOnly;
	}
	public set IsDisplayOnly(v: boolean)
	{
		this._IsDisplayOnly = v;
	}
    checkLedgerCreated(firstCall: boolean = false)
	{
		if (this.EntityPM.Id && this.IsFullAccounting && (this.EntityPM.StatusCode == 'AD' || this.EntityPM.StatusCode == 'CL')) {

			this.CurrentSession.StartBusyIndicatorLoading();
			this._JournalExtendedPMService.GetByAccountingEntityId(this.EntityPM.Id, '5').subscribe((myResult: ServiceResponse) => // 3- ARPayment
			{
				console.log("_JournalExtendedPMService.GetByAccountingEntityId", myResult);
				this.CurrentSession.StopBusyIndicator();

				var res: ServiceResponse = myResult;
				var createdJournal: JournalPM = res.Result;

				if (createdJournal) {
					if (this.IsDisplayOnly == true && createdJournal.IsLedgerCreated) {
						this.GetTransactionsForAPPayment();
					}
					this.IsDisplayOnly = !createdJournal.IsLedgerCreated;
				}
				else {
					console.log("[Check Ledger] no journal created");
				}
			});

		}

	}
    

    private LoadAddressAndGeneralTab() {
        this.PartnersDomainService.GetBillingOrMainAddressListByCardId(this.EntityPM.VendorId).subscribe((resp: any) => {
            if (resp != null) {
                var address = resp;
                if (address != null) {
                    this.VendorAddressId = address.Id;
                }
                else {
                    this.VendorAddressId = null;
                }
            }
        });
    }

    get VendorAddressId() {
        if (this.EntityPM == null) {
            return null;
        }

        return this.EntityPM.VendorAddressId;
    }
    set VendorAddressId(value: string) {
        if (!this.EntityPM.IsCreatedFromInvoiceSide) {
            if (this.EntityPM != null) {
                if (this.EntityPM.VendorAddressId != value) {
                    this.EntityPM.VendorAddressId = value;
                }
            }
        }
    }

    // Currency Properties
    get PaymentCurrencyId() {
        if (this.EntityPM == null) {
            return null;
        }

        return this.EntityPM.PaymentCurrencyId;
    }
    set PaymentCurrencyId(value: string) {
        this.setPaymentCurrencyId(value)
    }

    async setPaymentCurrencyId(value: string) {
        if (!this.EntityPM.IsCreatedFromInvoiceSide) {
            if (this.EntityPM != null) {
                if (this.EntityPM.PaymentCurrencyId != value) {
                    this.EntityPM.PaymentCurrencyId = value;                    
                    this.PaymentCurrencyExchangeRate = await this.GetCurrencyRate(value);                    
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

                }
            }
        }}

    get PaymentCurrencyExchangeRate() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.PaymentCurrencyExchangeRate;
    }
    set PaymentCurrencyExchangeRate(value: number) {
        if (!this.EntityPM.IsCreatedFromInvoiceSide) {
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
    }

    get ExchangeRateDate() {
        if (this.EntityPM == null) {
            return null;
        }

        return this.EntityPM.PaymentCurrencyExchangeRateDate;
    }
    set ExchangeRateDate(value: Date) {
        if (this.EntityPM != null) {
            if (this.EntityPM.PaymentCurrencyExchangeRateDate != value) {
                this.EntityPM.PaymentCurrencyExchangeRateDate = value;
                this.ComputeRelativeRateDate();
            }
        }
    }

    get AutomaticPaymentCheque() { return this.EntityPM.AutomaticPaymentCheque; }
    set AutomaticPaymentCheque(value: boolean) {
        if (this.EntityPM.AutomaticPaymentCheque != value) {
            this.EntityPM.AutomaticPaymentCheque = value;
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

    // Properties
    get TaxDeductionPercentage() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.TaxDeductionPercentage;
    }
    set TaxDeductionPercentage(value: number) {
        if (this.EntityPM != null) {
            if (this.EntityPM.TaxDeductionPercentage != value) {

            const nonIsraeliVendor = this.vendor ? this.vendor.CountryCode != "IL" : false;
            if(nonIsraeliVendor)
                value = 0;
            this.EntityPM.TaxDeductionPercentage = value;
            this.CalculateTaxDeductionLocalAmount();
            }
        }
    }

    get TaxDeductionLocalAmount() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.TaxDeductionLocalAmount;
    }
    set TaxDeductionLocalAmount(value: number) {
        if (this.EntityPM != null) {
            if (this.EntityPM.TaxDeductionLocalAmount != value) {
                this.EntityPM.TaxDeductionLocalAmount = value;
            }
        }
    }

    private CalculateTaxDeductionLocalAmount() {
        if (this.IsFullAccounting && this.TaxDeductionPercentage != null && this.AmountInLocalCurrency != null) {
            this.TaxDeductionLocalAmount = (this.TaxDeductionPercentage * this.AmountInLocalCurrency)/100;
            this.SetUIProperties_FullAccounting_Tax();
        }
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
                this.IsTaxUpdated = true;
                this.LoadTaxPercentage();
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
            }
        }
    }

    get PaymentMethodCode() {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.PaymentMethodCode;
    }
    set PaymentMethodCode(value: string) {
        if (this.EntityPM != null) {
            if (this.EntityPM.PaymentMethodCode != value) {
                this.EntityPM.PaymentMethodCode = value;
                this.SetAutomaticPaymentCheque(value);
                this.SetUIProperties_FullAccounting(); // for BankAccountId

                this.ItemsSource.Collection.forEach((item: APPaymentInvoiceArgs) => {

                    if (item.IsConnected && item.Invoice.AmountDue == 0 && item.AmountPaid == 0) {
                        if (value != "FS") {
                            item.IsConnected = false;
                        }
                    }

                    item.SetUIProperties();
                });
            }
        }
    }

    SetAutomaticPaymentCheque(value: string){
        if (this.PaymentMethodCode == "CH" && this.PaymentChequeActivated) {
        this.EntityPM.AutomaticPaymentCheque = true;
        this.IsSplitButtonVisibile = true;
    }
    else {
        this.EntityPM.AutomaticPaymentCheque = false;
        this.IsSplitButtonVisibile = false;
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
            }
        }
    }

    //Payment Line Properties
    public PaymentMethodAddedManually: boolean = false;
    private RefreshPaymentMethodFields() {
        this.Bank = null;
        this.BankBranch = null;
        this.Account = null;
        this.ChequeOrPaymentRef = null;
        this.ValueDate = null;
        this.CreditCardTypeId = null;
        if (this.IsFullAccounting) {
            this.BankAccountId = null;
        }
        var lists: AccountingPaymentMethodList[] = this.AllMethods.filter(d => d.Id == this.AccountingPaymentMethodId);
        if (lists) {
            var list = lists[0];
            if (list) {
                this.PaymentMethodCode = list.Code;
                this.PaymentMethodAddedManually = list.AddedManually;
            }
        }

        if (this.PaymentMethodCode == "CA" || this.PaymentMethodCode == "FS") {
            this.ValueDate = DateTool.GetCurrentDateAsUtc();
        }

        this.SetUIProperties_Cheque();
        this.SetUIProperties_CreditCard();

        if (this.IsFullAccounting) {
            this.SetUIProperties_FullAccounting();
        }
    }

    public BankAccountIdVisibility: boolean = false;
    SetUIProperties_FullAccounting() {
        if (this.IsFullAccounting) {
            this.SetUIProperties_FullAccounting_Tax();
            if (this.EntityPM.Id != null) {
                this.ShowSplitButton = false;
                this.PaymentChequeActivated = false;
            }
            if (this.PaymentMethodCode == "CH" || this.PaymentMethodCode == "BT" || this.PaymentMethodCode == "CC") {
                this.BankAccountIdVisibility = true;
            }
            else {
                this.BankAccountIdVisibility = false;
            }

            if (this.PaymentMethodCode == "CH" || this.PaymentMethodCode == "BT" || this.PaymentMethodCode == "CC") {
                this.UIProperties.SetVisibility("Bank", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("Account", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("Bank", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("BankBranch", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("Account", this.ObjectTableName, false);
                this.UIProperties.SetRequired("BankAccountId", this.ObjectTableName, !this.BankAccountId);
            }
             else
             {
                this.UIProperties.SetRequired("BankAccountId", this.ObjectTableName, false);
            }

        }
    }
    SetUIProperties_FullAccounting_Tax() {
        this.UIProperties.SetEnabled("TaxDeductionLocalAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TaxDeductionPercentage", this.ObjectTableName, false);
        this.UIProperties.SetRequired("TaxDeductionLocalAmount", this.ObjectTableName, AppTool.IsNullOrEmpty(this.TaxDeductionLocalAmount));
        this.UIProperties.SetRequired("TaxDeductionPercentage", this.ObjectTableName, AppTool.IsNullOrEmpty(this.TaxDeductionPercentage));
    }

    get PaymentMethodDetailsLabel() {
        var result = "";
        if (this.PaymentMethodCode == "CH") {
            result = TextCodeTranslator.Translate("APPayment.S.Details.Cheque");
        }
        else if (this.PaymentMethodCode == "BT") {
            result = TextCodeTranslator.Translate("APPayment.S.Details.BankTransfer");
        }
        else if (this.PaymentMethodCode == "CC") {
            result = TextCodeTranslator.Translate("APPayment.S.Details.CreditCard");
        }
        else {
            var method: AccountingPaymentMethodList = this.AllMethods.filter(d => d.Code == this.PaymentMethodCode)[0];
            if (method != null) {
                result = method.Name;
            }
        }

        return result;
    }
    get ChequePaymentRefLabel() {
        var result = TextCodeTranslator.Translate("APPayment.S.Details.PaymentRef");
        if (this.PaymentMethodCode == "CH") {
            result = TextCodeTranslator.Translate("APPayment.S.Details.ChequeRef");
        }

        return result;
    }
    get VisibleIfCash() {
        var result = false;
        if (AppTool.IsNullOrEmpty(this.PaymentMethodCode) || this.PaymentMethodCode == "CA" || this.PaymentMethodCode == "FS") {
            result = true;
        }
        return result;
    }
    get CollapsedIfCash() {
        var result = false;
        if (!AppTool.IsNullOrEmpty(this.PaymentMethodCode)) {
            if (this.PaymentMethodCode == "CA" || this.PaymentMethodCode == "FS") {
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
                if (value != null) {
                    this.UIProperties.SetRequired("ValueDate", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("ValueDate", this.ObjectTableName, true);
                }
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
                if (!AppTool.IsNullOrEmpty(value)) {
                    this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, true);
                }
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
        if (AppTool.IsNullOrEmpty(value)) {
            this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, true);
        }
    }

    public CreditCardTypeIdVisibility: boolean = false;

    get PaymentCurrencyCode() { return this.EntityPM.PaymentCurrencyCode; }
    set PaymentCurrencyCode(value: string) {
        if (this.EntityPM.PaymentCurrencyCode != value) {
            this.EntityPM.PaymentCurrencyCode = value;
        }
    }

    public AllCurrencies: CurrencyList[] = [];
    LoadCurrencies() {
        var myService: CurrencyListService = new CurrencyListService();
        myService.getAll().subscribe((response: ServiceResponse) => {
            if (response != null) {
                this.AllCurrencies = response.Result;
            }
        });
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

    get AmountInPaymentCurrency() { return this.EntityPM.AmountInPaymentCurrency; }
    set AmountInPaymentCurrency(value: number) {
        if (this.EntityPM.AmountInPaymentCurrency != value || (this.IsFullAccounting && this.EntityPM.ReconcileInternalTrans && this.ReconcileInternalTrans.length > 0)) {
            this.EntityPM.AmountInPaymentCurrency = AppTool.Round(value, 2);            
            this.ComputeLocalAmount();
            this.UpdateSummary();
            this.ComputeOpenAmount();
            this.UpdateLineCreatedFromInvoice();

            this.ItemsSource.Collection.forEach(item => {
                item.SetUIProperties();
            });

            this.CalculateTaxDeductionLocalAmount();
        }
    }

    get AmountInLocalCurrency() { return this.EntityPM.AmountInLocalCurrency; }
    set AmountInLocalCurrency(value: number) {
        if (this.EntityPM.AmountInLocalCurrency != value) {
            this.EntityPM.AmountInLocalCurrency = AppTool.Round(value, 2);
            this.CalculateTaxDeductionLocalAmount();
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
                this.BankAccountPMService.get(this.EntityPM.BankAccountId).subscribe((res:any) => {
                    if (res) {
                        if (res.Result) {
                            var bank: BankAccountPM = res.Result;
                            this.BankBranch = bank.BranchNumber;
                            this.Account = bank.AccountNumber;
                            this.Bank = bank.BankCode;
                        }
                    }
                });

                this.SetUIProperties_FullAccounting()
            }
        }
    }

    UpdateLineCreatedFromInvoice() {
        if (this.createdFromInvoiceLine) {
            this.createdFromInvoiceLine.UpdateAmountToPay();
        }
    }
    ComputeOpenAmount() {
        this.OpenAmount = this.AmountInPaymentCurrency - this.Summary_AmountPaid - this.Summary_ExternalAmount;
    }
    ComputeLocalAmount() {
        this.AmountInLocalCurrency = this.AmountInPaymentCurrency * this.PaymentCurrencyExchangeRate;
    }

    // Summary
    public Summary_Amount: number = 0;
    public Summary_AmountPaid: number = 0;
    public Summary_ExternalAmount: number = 0;
    public Summary_AmountPaidColor: string = "#282E30";
    public Summary_ExternalAmountColor: string = "#1E4AC4";
    public IsExternalPaymentVisible: boolean = false;
    UpdateSummary() {
        var Amount: number = 0;
        var AmountPaid: number = 0;
        var ExternalAmount: number = 0;
        var AmountPaidColor: string = "#282E30";
        var ExternalAmountColor: string = "#1E4AC4";
        var isExternalPaymentVisible: boolean = false;

        if (this.AmountInPaymentCurrency) {
            Amount = AppTool.Round(this.AmountInPaymentCurrency, 2);
        }

        if (this.EntityPM.PaymentInvoices.length > 0) {
            AmountPaid = ArrayTool.Sum(this.EntityPM.PaymentInvoices, "PaymentAmount");
            AmountPaid = AppTool.Round(AmountPaid, 2);
        }

        if (this.EntityPM.ExternalPaymentAmount) {
            ExternalAmount = AppTool.Round(this.EntityPM.ExternalPaymentAmount, 2);
        }

        if (this.IsNegativeAmountEnabled == false) {
            if (AmountPaid < 0) {
                AmountPaidColor = "#E53030";
            }
        }

        if ((AmountPaid + ExternalAmount) > Amount) {
            AmountPaidColor = "#E53030";
            ExternalAmountColor = "#E53030";
        }

        this.Summary_Amount = Amount;
        this.Summary_AmountPaid = AmountPaid;
        this.Summary_ExternalAmount = ExternalAmount;
        this.Summary_AmountPaidColor = AmountPaidColor;
        this.Summary_ExternalAmountColor = ExternalAmountColor;

        if (!AppTool.IsNullOrZero(this.EntityPM.ExternalPaymentAmount)) {
            isExternalPaymentVisible = true;
        }

        this.IsExternalPaymentVisible = isExternalPaymentVisible;
    }

    EnterExternalPaymentClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { EntityPM: this.EntityPM };
        logWindow.Title = "External Payment";
        logWindow.Width = 650;
        logWindow.Height = 450;
        logWindow.Show('./InvoiceModules/APPayment/Components/Other/ExternalPaymentComponent');
    }

    public ViewEntity(args: APPaymentInvoiceArgs) {
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
            logWindow.Title = TextCodeTranslator.Translate("RatesTable.O.UpdateCurrencyRate");
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
                    cmpRef.instance.Run({ EntityId: this.RequestedCommandParam, ObjectTableName: 'APInvoice' });

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
export class APPaymentInvoiceArgs extends BaseComponent {
    public EntityPM: APPaymentInvoicePM = null;
    public Invoice: APInvoiceList = null;
    public PaymentPM: APPaymentPM = null;
    public DataContext: APPaymentInvoiceArgs = this;
    public ObjectTableName: string = "APInvoice";
    public SortingValue: number = 0;
    public LocalCurrencyId: string = null;
    public IsMultiCurrency: boolean = false;
    constructor(item: APInvoiceList, private trigger: APPaymentDetailsTabComponent) {
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

        this.EntityPM = this.PaymentPM.PaymentInvoices.filter(d => d.APInvoiceId == this.Invoice.Id)[0];
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
    public ShipmentNumber: string = null;
    public CurrencyId: string = null;
    public CurrencyCode: string = null;
    public InvoiceAmount: number = 0;
    public TransferStatusCode: string = null;
    public RecoNumber: string = null;

    InitProperties() {
        this.Id = this.Invoice.Id;
        this.DueDate = this.Invoice.DueDate;
        this.Status = this.Invoice.StatusName;
        this.InvoiceNumber = this.Invoice.InvoiceNumber;
        this.TransferStatus = this.Invoice.TransferStatusName;
        this.CurrencyId = this.Invoice.InvoiceCurrencyId;
        this.CurrencyCode = this.Invoice.InvoiceCurrencyCode;
        this.InvoiceAmount = this.Invoice.AmountInInvoiceCurrency == null ? 0 : this.Invoice.AmountInInvoiceCurrency;
        this.ShipmentNumber = this.Invoice.IsMultipleEntities ? "List" : this.Invoice.MainEntityReference;
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
    public ConnectFeature: boolean = false;
    public DissconectFeature: boolean = false;

    public IsAdvancedButtonVisible: boolean = false;
    public CheckBoxEnabled: boolean = true;
    public NoPermision: string="";
    SetUIProperties() {
        this.isControlEnabled = false;
        this.CheckBoxVisibility = false;
        this.NotMatchedVisibility = false;
        this.IsAdvancedButtonVisible = false;
        //this.CheckBoxEnabled = true;

        this.SetUIProperties_CurrencyMatched();
        this.SetUIProperties_AllowedToConnect();

        if (this.isAllowedToConnect) {
            this.isControlEnabled = true;

            if (this.trigger.EntityPM.OpenAmount <= 0) {
                this.isControlEnabled = false;
            }
        }

        if (this.isConnected) {
            this.isControlEnabled = true;
            this.CheckBoxVisibility = true;
            this.NotMatchedVisibility = false;
        }

        else {
            if (!this.isAllowedToConnect) {
                this.CheckBoxVisibility = false;
                this.NotMatchedVisibility = true;
                this.IsAdvancedButtonVisible = false;
            }

            else
                if (this.trigger.EntityPM.OpenAmount <= 0) {

                    if (this.trigger.EntityPM.OpenAmount == 0 && this.PaymentPM.PaymentMethodCode == "FS") {
                        this.CheckBoxVisibility = true;
                    }

                    else {
                        this.NotMatchedVisibility = false;
                    }
                }

                else {
                    this.CheckBoxVisibility = true;
                }
        }

        this.SetUIProperties_AmountPaidEnabled();
        this.SetLineColors();

        var featureExist: boolean = false;
        if (FeatureLocator.HasFeaturePermession("APPayment", "APPaymentConnectInvoices")) {
            this.ConnectFeature = true;
            featureExist = true;

        }
        else {
            if (this.isConnected) {
                this.CheckBoxVisibility = true;
                this.ConnectFeature = true;

            }
            else {
                this.CheckBoxVisibility = false;
                this.ConnectFeature = false;
                this.UIProperties.SetEnabled("AmountPaid", this.ObjectTableName, false);

            }
        }

        if (FeatureLocator.HasFeaturePermession("APPayment", "APPaymentDissconectInvoices")) {
            this.DissconectFeature = true;

        }
        else {
            if (this.isConnected) {
                this.CheckBoxEnabled = false;
                this.NoPermision = "You have no permission to disconnect invoices";
                this.UIProperties.SetEnabled("AmountPaid", this.ObjectTableName, false);

            }
            else {
                this.CheckBoxEnabled = true;
                this.NoPermision = null;


            }
        }

        if (this.trigger.IsFullAccounting && this.trigger.EntityPM.ReconcileInternalTrans && this.trigger.EntityPM.ReconcileInternalTrans.length > 0) {
            this.CheckBoxEnabled = false;
        }
    }
    SetUIProperties_CurrencyMatched() {
        this.isCurrencyMatched = false;

        if (this.PaymentPM.PaymentCurrencyId) {
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
    }
    SetUIProperties_AllowedToConnect() {

        this.isAllowedToConnect = true;

        if (this.IsConnected == false) {

            if (this.Invoice.StatusCode == "WA" || this.isCurrencyMatched == false) {
                this.isAllowedToConnect = false;
            }

            else if (AppTool.IsNullOrZero(this.AmountDue)) {

                if (this.AmountDue == 0 && this.Invoice.StatusCode == "AD" && this.PaymentPM.PaymentMethodCode == "FS") {
                    this.isAllowedToConnect = true;
                }

                else {
                    this.isAllowedToConnect = false;
                }
            }
        }
    }
    SetUIProperties_AmountPaidEnabled() {
        var isEnabled: boolean = true;

        if (this.Invoice.StatusCode == "WA") {
            isEnabled = false;
        }

        else if (this.isCurrencyMatched == false) {
            isEnabled = false;
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

            if (this.Invoice.StatusCode == "WA") {
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
                var itemPM = this.PaymentPM.PaymentInvoices.filter(d => d.APInvoiceId == this.Id)[0];
                if (itemPM) {
                    this.PaymentPM.RemoveAPPaymentInvoicePM(itemPM);
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

            if (this.InvoiceAmount < 0) {
                if (inputEntry > 0) {
                    var msg = TextCodeTranslator.Translate("APPayment.M.OnlyMinusValue");
                    this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, false, msg);
                    inputErrors++;
                }

                else if (inputEntry < allowedAmount) {
                    var msg = TextCodeTranslator.Translate("APPayment.M.AmountPaidNotLess") + " " + allowedAmount;
                    this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, false, msg);
                    inputErrors++;
                }
            }

            else {
                if (inputEntry < 0) {
                    var msg = TextCodeTranslator.Translate("APPayment.M.CantPayMinusValue");
                    this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, false, msg);
                    inputErrors++;
                }

                else if (inputEntry > allowedAmount) {
                    var msg = TextCodeTranslator.Translate("APPayment.M.AmountPaidLessOrEqual") + " " + allowedAmount;
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
                        if(!(this.RecoNumber && this.RecoNumber != null && this.RecoNumber.length > 0)) {
                            this.IsConnected = false;
                        }
                    }

                    else {

                        this.Invoice.AmountPaid = inputEntry;
                        this.AmountDue = this.InvoiceAmount - this.OtherPaymentsAmount - inputEntry;

                        var itemPM = this.PaymentPM.PaymentInvoices.filter(d => d.APInvoiceId == this.Id)[0];
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

        var itemPM: APPaymentInvoicePM = new APPaymentInvoicePM(this.PaymentPM);
        itemPM.Tenant = SessionLocator.TenantPM.Id;
        itemPM.APInvoiceId = this.Id;
        itemPM.APPaymentId = this.PaymentPM.Id;
        itemPM.APInvoiceNumber = this.InvoiceNumber;
        itemPM.ForeignCurrencyId = this.CurrencyId;
        itemPM.ExchangeRate = this.ExchangeRate;
        itemPM.ForeignAmount = this.ConnectedAmount_INV == null ? 0 : AppTool.Round(this.ConnectedAmount_INV, 2);
        itemPM.PaymentAmount = this.ConnectedAmount_PAY == null ? 0 : AppTool.Round(this.ConnectedAmount_PAY, 2);
        itemPM.APInvoiceTransferStatusCode = this.TransferStatusCode;

        if (this.CurrencyId == this.LocalCurrencyId) {
            itemPM.LocalAmount = itemPM.ForeignAmount;
        }

        else {
            itemPM.LocalAmount = AppTool.Round(itemPM.ForeignAmount * itemPM.ExchangeRate, 2);
        }

        this.PaymentPM.AddAPPaymentInvoicePM(itemPM);

        var invoiceAmount: number = this.InvoiceAmount;
        var invoiceAmountPaid: number = itemPM.ForeignAmount;
        var invoiceAmountDue: number = invoiceAmount - this.OtherPaymentsAmount - invoiceAmountPaid;

        if (invoiceAmount < 0) {
            if (invoiceAmountDue > 0) {
                invoiceAmountDue = invoiceAmountDue * -1;
            }

            if (invoiceAmountPaid > 0) {
                invoiceAmountPaid = invoiceAmountPaid * -1;
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
        this.trigger.UpdatePaymentInvoicesErrors();
        this.trigger.UpdateSummary();
        this.trigger.ComputeOpenAmount();
        this.trigger.SetUIProperties_Invoices();

        this.trigger.ItemsSource.Collection.forEach(item => {
            item.SetUIProperties();
        });
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

                            var itemPM = this.PaymentPM.PaymentInvoices.filter(d => d.APInvoiceId == this.Id)[0];
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

        logWindow.Show('./InvoiceModules/APPayment/Components/EditTabs/APEditMultiCurrency');
    }
    UpdateAmountToPay() {
        this.GetSmallestAmount();

        var invoicePayment: APPaymentInvoicePM = this.PaymentPM.PaymentInvoices.filter(d => d.APInvoiceId == this.Invoice.Id)[0];
        if (invoicePayment) {
            invoicePayment.ForeignAmount = this.ConnectedAmount_INV == null ? 0 : AppTool.Round(this.ConnectedAmount_INV, 2);
            invoicePayment.PaymentAmount = this.ConnectedAmount_PAY == null ? 0 : AppTool.Round(this.ConnectedAmount_PAY, 2);

            if (this.CurrencyId == this.LocalCurrencyId) {
                invoicePayment.LocalAmount = invoicePayment.ForeignAmount;
            }

            else {
                invoicePayment.LocalAmount = AppTool.Round(invoicePayment.ForeignAmount * invoicePayment.ExchangeRate, 2);
            }

            var invoiceAmount: number = this.InvoiceAmount;
            var invoiceAmountPaid: number = invoicePayment.ForeignAmount;
            var invoiceAmountDue: number = invoiceAmount - this.OtherPaymentsAmount - invoiceAmountPaid;

            if (invoiceAmount < 0) {
                if (invoiceAmountDue > 0) {
                    invoiceAmountDue = invoiceAmountDue * -1;
                }

                if (invoiceAmountPaid > 0) {
                    invoiceAmountPaid = invoiceAmountPaid * -1;
                }
            }

            this.AmountDue = AppTool.Round(invoiceAmountDue, 2);
            this.Invoice.AmountPaid = AppTool.Round(invoiceAmountPaid, 2);

            this.SetLineColors();
            this.UpdatePayment();
        }
    }
}
