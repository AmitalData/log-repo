import { ServiceResponse } from './../../../../Infrastructure/DataContracts/ServiceResponse';
import { PartnerTypeListService } from './../../../../Common/Services/StandardLists/PartnerTypeListService';
import { GLAccountListService } from './../../../../Accounting/Services/StandardLists/GLAccountListService';
import { TenantPM } from './../../../../Common/EntityPMs/TenantPM';
import {Component, AfterViewInit, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {ARPaymentPM} from '../../../../Invoice/EntityPMs/ARPaymentPM';
import {ARInvoicePM} from '../../../../Invoice/EntityPMs/ARInvoicePM';
import {ARPaymentInvoicePM} from '../../../../Invoice/EntityPMs/ARPaymentInvoicePM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {CurrencyRatesService, LastRate} from '../../../../Common/Services/CurrencyRatesService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {AddressListService} from  '../../../../Common/Services/StandardLists/AddressListService';
import {AddressList} from  '../../../../Common/EntityLists/AddressList';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {InvoiceTool} from '../../../../Invoice/Tools';
import {InvoiceDomainService} from '../../../../Invoice/Services/InvoiceDomainService';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {AccountingPaymentMethodList} from '../../../../Invoice/EntityLists/AccountingPaymentMethodList';
import {AccountingPaymentMethodListService} from '../../../../Invoice/Services/StandardLists/AccountingPaymentMethodListService';
import {GLAccountPMService} from '../../../../Accounting/Services/StandardPMs/GLAccountPMService';
import {GLAccountPM} from '../../../../Accounting/EntityPMs/GLAccountPM';
import { GLAccountList } from '../../../../Accounting/EntityLists/GLAccountList';
import { PartnerTypeList } from 'Common/EntityLists/PartnerTypeList';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
declare var window: any;
@Component({

    templateUrl: './NewARPaymentComponent.html',
})

export class NewARPaymentComponent extends BaseComponent implements OnInit {
    public DataContext: NewARPaymentComponent = this;
    public ObjectTableName: string = "ARPayment";
    public newARPaymentPM: ARPaymentPM = new ARPaymentPM();
    public TenantPM: TenantPM;
    public invoicePm: ARInvoicePM = new ARInvoicePM();
    public LastRatesList: LastRate[] = [];
    public AllMethods: AccountingPaymentMethodList[] = [];
    private IsLoadCurrencyList: boolean = false;
    private customerId = null;
    public DisplaySATSettings: boolean = false;
    public DisplayFechaPago: boolean = false;
    public EnableNegativeOffsetARPayments: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    public IsCreatedFromInvoiceSide: boolean = false;
    get IsNegativeAmountEnabled() { return this.EnableNegativeOffsetARPayments == true && this.AccountingPaymentMethodCode == "FS" ? true : false; }
    public isRTL: boolean = false;
    public accountingActivated: boolean;
    private _glaService: GLAccountListService = new GLAccountListService();
    _PartnerTypeListService: PartnerTypeListService = new PartnerTypeListService();
    private CurrentSession = SessionLocator.SelectedSession;
    DisplayFieldsFromList:string;
    DisplayLocalFieldsFromList:string;
    BillToLovSizeForFullAccounting:number;
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    constructor(private _entityResourceService: EntityResourceService) {
        super();
        this.accountingActivated = SessionLocator.TenantPM.AccountingActivated;
        this._entityResourceService.getEntityResourceByTableName("ARPayment", 0).subscribe((response: any) => {});
        this._entityResourceService.getEntityResourceByTableName("ARInvoice", 0).subscribe((response: any) => {});
        this.InitializeBillToLov();
        this.loadPartnerTypesFilter();

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this.TodayDate = DateTool.GetCurrentDateAsUtc();
        this.TenantPM = SessionLocator.TenantPM;

        this.EnableNegativeOffsetARPayments = ObjectsLocator.AccountingSettingPM.EnableNegativeOffsetARPayments;

        if (this.invoicePm == null) {
            this.invoicePm = new ARInvoicePM();
        }


        if(SessionLocator.TenantPM.AccountingActivated)
            this.invoicePm.IsFullAccounting = true;

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            this.DisplaySATSettings = true;


                this.DisplayFechaPago = true;

        }

        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }
    }

    private InitializeBillToLov() {
        if (this.accountingActivated) {
            this.DisplayFieldsFromList = "Code,CalculatedEnglishName,GLAccountDisplayNumber,CityName,CountryCode,PartnerTypeName";
            this.DisplayLocalFieldsFromList = "Code,CalculatedLocalName,GLAccountDisplayNumber,CityName,CountryCode,PartnerTypeName";
            this.BillToLovSizeForFullAccounting = 550;
        }
    }

    private loadPartnerTypesFilter()
    {
        this.checkPartnerTypesFilterFeature();
        if(this.isPartnerTypesFilterEnabled)
            this.getPartnerTypes();
    }

    ngOnInit() {
        this.Initialize();
        this.BuildAdditionalFields();
    }

    // Additional Fields
    private timerToken: any;
    private Retries: number = 0;
    private GeneratedComponent: any;
    private additionalFieldsScreenCode = "NewARPayment";
    public ShowAdditionalFieldsScreen: boolean = false;
    BuildAdditionalFields() {

       var objectTableId = window.ObjectTables.filter((x: any) => x.Name === this.ObjectTableName)[0].Id;
        var myScreen = window.Screens.filter((x: any) => x.ObjectTableId === objectTableId && x.Code.toLowerCase() == this.additionalFieldsScreenCode.toLowerCase())[0];

        if (myScreen != null) {

            var myScreenFields = window.ScreenFields.filter((x: any) => x.ScreenId === myScreen.Id && x.Tenant === SessionInfo.LoggedUserTenant);

            if (myScreenFields.length == 0) {
                myScreenFields = window.ScreenFields.filter((x: any) => x.ScreenId === myScreen.Id);
            }

            if (myScreenFields.length != 0) {
                this.ShowAdditionalFieldsScreen = true;
                this.RunComponent();
            }
        }

    }
    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }
    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {

                this.GeneratedComponent = cmpRef.instance;

                cmpRef.instance.LoadCompleted.subscribe(s => {
                    this.SetUIProperties_GeneratedComponent();
                });

                var screenCode = this.additionalFieldsScreenCode;
                cmpRef.instance.LabelWidth = 110;
                cmpRef.instance.Run(this.newARPaymentPM, this.ObjectTableName, screenCode);
            });
    }
    RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }
    SetUIProperties_GeneratedComponent() {
        if (this.GeneratedComponent) {
           this.GeneratedComponent.SetEnabled(true);
        }
    }

    windowArgs;
    createBankTransferFromReconcileWindow: boolean = false;
    preselectedPaymentMethodCode;
    selectedAmount;
    preSelectedCurrencyId;
    exteranlPageLinesIds;
    presetValueDate;
    presetPaymentReference;
    presetRegisterDate;
    SetWindowArgs(args: any) {
        if (args) {
            this.windowArgs = args;
            this.invoicePm = args['ARInvoice'];
            this.customerId = args['CustomerId'];

            if(args.AccountingPaymentMethodCode){
                this.createBankTransferFromReconcileWindow = true;
                this.preselectedPaymentMethodCode = args.AccountingPaymentMethodCode;
                this.selectedAmount = args.PaymentAmount;
                this.preSelectedCurrencyId = args.Currency;
                this.RegisterDate = new Date();
                this.exteranlPageLinesIds = args.ExteranlPageLinesIds;
                this.presetValueDate = args.ValueDate;
                this.presetPaymentReference = args.PaymentReference;
                this.presetRegisterDate = args.RegisterDate;
            }

        }

        if (this.invoicePm != null) {
            this.IsCreatedFromInvoiceSide = true;
            this.UIProperties.SetEnabled("PartnerId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, false);
            if (!this.accountingActivated) {
                this.UIProperties.SetEnabled("PaymentCurrencyExchangeRate", this.ObjectTableName, false);
            }
        }
    }

    public CurrencyList: CurrencyList[] = [];
    private LoadCurrencyListMethod() {
        var myService: CurrencyListService = new CurrencyListService();
        myService.getAll().subscribe((myResult: ServiceResponse) => {
            if (myResult) {
                this.CurrencyList = myResult.Result;
                this.IsLoadCurrencyList = true;
                this.Initialize();
            }
        });
    }

    public AddressList: AddressList[] = [];
    private LoadAddressListMethod() {
        var myService: AddressListService = new AddressListService();
        myService.getAll().subscribe((myResult: ServiceResponse) => {
            if (myResult) {
                this.AddressList = myResult.Result;
            }
        });
    }

    public CardList: CardList[] = [];
    private LoadCardListMethod() {
        var myService: CardListService = new CardListService();
        myService.getAll().subscribe((myResult: ServiceResponse) => {
            if (myResult) {
                this.CardList = myResult.Result;
            }
        });
    }

    public RateIsEnabled = false;
    SetUIProperties() {
        if (FeatureLocator.HasFeaturePermession("ARPayment", "ARPaymentEditExchangeRate")) {
            if (this.PaymentCurrencyId) {
                if (this.PaymentCurrencyId != this.TenantPM.CurrencyId) {
                    if (this.IsCreatedFromInvoiceSide == false) {
                        this.RateIsEnabled = true;
                    }
                }
            }
        }
        else if (SessionLocator.TenantPM.AccountingActivated) {
            if (this.PaymentCurrencyId) {
                if (this.PaymentCurrencyId != this.TenantPM.CurrencyId) {

                    this.RateIsEnabled = true;
                }
                else {
                    this.RateIsEnabled = false;

                }
            }
        }

        this.UIProperties.SetEnabled("PaymentCurrencyExchangeRate", this.ObjectTableName, this.RateIsEnabled);
        this.SetUIProperties_Payment();
    }

    LoadData() {
        if (this.invoicePm != null && AppTool.IsNullOrEmpty(this.invoicePm.Id)) {

            var loadingDate = this.newARPaymentPM.RegisterDate;
            if (loadingDate == null) {
                loadingDate = DateTool.GetCurrentDateAsUtc();
            }

            var myService: CurrencyRatesService = new CurrencyRatesService();
            myService.GetCurrenciesExchangeRateByValueDate(SessionLocator.TenantPM.CurrencyId, loadingDate).subscribe((resp:ServiceResponse) => {
                if (resp != null) {
                    if (!resp.HasError) {
                        this.LastRatesList = resp.Result;
                        this.SetCurrencyRateData();
                    }
                }
            });
        }

        var myService1: AccountingPaymentMethodListService = new AccountingPaymentMethodListService();
        myService1.getAll().subscribe((response: ServiceResponse) => {
            if (response != null) {
                this.AllMethods = response.Result;
                if(this.preselectedPaymentMethodCode){
                    var preselectedMethod = this.AllMethods.find(m=>m.Code == this.preselectedPaymentMethodCode);
                    this.AccountingPaymentMethodId = preselectedMethod.Id;
                    this.UIProperties.SetEnabled("AccountingPaymentMethodId", this.ObjectTableName, false);

                }
            }
        });
    }

    public IsVisible: boolean = false;
    Initialize() {
        this.FillTipoCadenaPagoList();
        this.CreateARPayment();

        if (this.IsCreatedFromInvoiceSide) {
            this.newARPaymentPM.PartnerId = this.invoicePm.PartnerId;
            this.newARPaymentPM.BillToId = this.invoicePm.BillToId;
            this.newARPaymentPM.BillToName = this.invoicePm.BillToName;
            this.newARPaymentPM.BillToAddressId = this.invoicePm.BillToAddressId;
            this.newARPaymentPM.BillToPartnerTypeId = this.invoicePm.BillToPartnerTypeId;
            this.newARPaymentPM.PaymentCurrencyId = this.invoicePm.InvoiceCurrencyId;
            this.newARPaymentPM.PaymentCurrencyCode = this.invoicePm.InvoiceCurrencyCode;
            this.newARPaymentPM.ExchangeRateDate = this.invoicePm.ExchangeRateDate;
            this.newARPaymentPM.PaymentCurrencyExchangeRate = this.invoicePm.InvoiceCurrencyExchangeRate;

          if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (!AppTool.IsNullOrEmpty(this.invoicePm.MetodoPagoCode)) {
              this.MetodoPagoCode = this.invoicePm.MetodoPagoCode;
              this.UIProperties.SetEnabled("MetodoPagoCode", this.ObjectTableName, false);
            }
          }
        }

        else {
            if (!AppTool.IsNullOrEmpty(this.customerId)) {
                this.PartnerId = this.customerId;
            }

            if(!this.preSelectedCurrencyId)
                this.PaymentCurrencyId = SessionLocator.TenantPM.CurrencyId;
            else
                this.PaymentCurrencyId = this.preSelectedCurrencyId;

            this.newARPaymentPM.PaymentCurrencyExchangeRate = 1;
        }


        if(SessionLocator.TenantPM.AccountingActivated)
            this.newARPaymentPM.IsFullAccounting = true;

        if(this.preselectedPaymentMethodCode)
            this.newARPaymentPM.ForceUsingBankTransferMethod = true;

        this.LoadData();
        this.SetUIProperties();
        this.IsVisible = true;
    }

    public TipoCadenaPagoList: TipoCadenaPagoClass[] = [];
    FillTipoCadenaPagoList() {
        this.TipoCadenaPagoList.push({ Code: null, Name: null });
        this.TipoCadenaPagoList.push({ Code: "01", Name: "SPEI (Electronic Payment System between Banks)" });

    }

    private selectedTipoCadenaPago: TipoCadenaPagoClass = null;
    get SelectedTipoCadenaPago() {

        var tipoCadenaPago: string = null;
        if (!AppTool.IsNullOrEmpty(this.TipoCadenaPago)) {
            tipoCadenaPago = this.TipoCadenaPago.toUpperCase();
        }

        switch (tipoCadenaPago) {

            case "01":
                {
                    this.selectedTipoCadenaPago = this.TipoCadenaPagoList.filter(d => d.Code == tipoCadenaPago)[0];
                    break;
                }

            default:
                {
                    this.selectedTipoCadenaPago = this.TipoCadenaPagoList.filter(d => d.Code == null)[0];
                    break;
                }
        }

        return this.selectedTipoCadenaPago;
    }
    set SelectedTipoCadenaPago(newValue: TipoCadenaPagoClass) {
        if (this.selectedTipoCadenaPago != newValue) {
            this.selectedTipoCadenaPago = newValue;

            if (newValue == null) {
                this.TipoCadenaPago = null;
            }

            else {
                this.TipoCadenaPago = newValue.Code;
            }
        }


        this.ValidateTipoCadenaPagoFields();

    }

    ValidateTipoCadenaPagoFields() {
        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (!AppTool.IsNullOrEmpty(this.TipoCadenaPago) && this.TipoCadenaPago == "01" && this.SATPaymentMethodCode == "03") {
                if (AppTool.IsNullOrEmpty(this.CertPago))
                    this.UIProperties.SetRequired("CertPago", "ARPayment", true);
                else
                    this.UIProperties.SetRequired("CertPago", "ARPayment", false);

                if (AppTool.IsNullOrEmpty(this.CadPago))
                    this.UIProperties.SetRequired("CadPago", "ARPayment", true);
                else
                    this.UIProperties.SetRequired("CadPago", "ARPayment", false);

                if (AppTool.IsNullOrEmpty(this.SelloPago))
                    this.UIProperties.SetRequired("SelloPago", "ARPayment", true);
                else
                    this.UIProperties.SetRequired("SelloPago", "ARPayment", false);
            }
            else {
                this.UIProperties.SetRequired("CertPago", "ARPayment", false);
                this.UIProperties.SetRequired("CadPago", "ARPayment", false);
                this.UIProperties.SetRequired("SelloPago", "ARPayment", false);
            }
        }
    }

    get TipoCadenaPago() {
        if (this.newARPaymentPM != null) {
            return this.newARPaymentPM.TipoCadenaPago;
        }
        else
            return null;
    }
    set TipoCadenaPago(newValue: string) {
        if (this.newARPaymentPM.TipoCadenaPago != newValue) {
            this.newARPaymentPM.TipoCadenaPago = newValue;
            this.ValidateTipoCadenaPagoFields();
        }
    }

    get CadPago() {
        if (this.newARPaymentPM != null) {
            return this.newARPaymentPM.CadPago;
        }
        else
            return null;


    }
    set CadPago(newValue: string) {
        if (this.newARPaymentPM.CadPago != newValue) {
            this.newARPaymentPM.CadPago = newValue;
            this.ValidateTipoCadenaPagoFields();
        }
    }

    get CertPago() {
        if (this.newARPaymentPM != null) {
            return this.newARPaymentPM.CertPago;
        }
        else
            return null;
    }
    set CertPago(newValue: string) {
        if (this.newARPaymentPM.CertPago != newValue) {
            this.newARPaymentPM.CertPago = newValue;
            this.ValidateTipoCadenaPagoFields();
        }
    }
    get SelloPago() {
        if (this.newARPaymentPM != null) {
            return this.newARPaymentPM.SelloPago;
        }
        else
            return null;
    }
    set SelloPago(newValue: string) {
        if (this.newARPaymentPM.SelloPago != newValue) {
            this.newARPaymentPM.SelloPago = newValue;
            this.ValidateTipoCadenaPagoFields();
        }
    }


    get FechaPago() { return this.newARPaymentPM.FechaPago; }
    set FechaPago(value: Date) {
        if (this.newARPaymentPM.FechaPago != value) {
            this.newARPaymentPM.FechaPago = value;

        }
    }


    SetCurrencyRateData() {
        if (this.invoicePm != null && AppTool.IsNullOrEmpty(this.invoicePm.Id)) {
            var rate = null;
            var rateDate = null;

            if (!AppTool.IsNullOrEmpty(this.PaymentCurrencyId)) {
                if (this.PaymentCurrencyId == this.TenantPM.CurrencyId) {
                    rate = 1;
                }

                else {
                    if (this.LastRatesList != null) {
                        var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == this.PaymentCurrencyId)[0];
                        if (lastRate != null) {
                            rate = lastRate.Rate;
                            rateDate = lastRate.ValueDate;
                        }
                    }
                }
            }

            this.PaymentCurrencyExchangeRate = rate;
            this.ExchangeRateDate = rateDate;
        }
    }

    CreateARPayment() {
        this.newARPaymentPM = new ARPaymentPM();
        this.newARPaymentPM.Tenant = this.TenantPM.Id;
        this.newARPaymentPM.StatusCode = "DR";
        this.newARPaymentPM.StatusName = "Draft";
        this.newARPaymentPM.SATTransferStatusCode = "NT";
        this.newARPaymentPM.SATTransferStatusName = "Not Transfered";
        this.newARPaymentPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.newARPaymentPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.newARPaymentPM.CreateDate = DateTool.GetCurrentDateAsUtc();
        this.newARPaymentPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
        this.newARPaymentPM.LocalCurrencyId = SessionLocator.TenantPM.CurrencyId;
        this.newARPaymentPM.RegisterDate = DateTool.GetCurrentDateAsUtc();
        this.newARPaymentPM.AmountInPaymentCurrency = this.selectedAmount;
        this.newARPaymentPM.PaymentCurrencyId = this.preSelectedCurrencyId;
        if(this.presetValueDate){
            this.newARPaymentPM.ValueDate = this.presetValueDate;
            this.newARPaymentPM.PresetValueDate = this.presetValueDate;
        }

        if(this.presetRegisterDate)
            this.newARPaymentPM.RegisterDate = this.presetRegisterDate;

        if(this.presetPaymentReference)
            this.newARPaymentPM.ChequeOrPaymentRef = this.presetPaymentReference;


        if (!SessionLocator.LoggedUserPM.IsCustomerCare) {
            this.newARPaymentPM.BranchId = SessionLocator.LoggedUserPM.BranchId;
        }

        this.EntityPM = this.newARPaymentPM;

        if(this.createBankTransferFromReconcileWindow){
            this.ComputeTotals();
            this.newARPaymentPM.ReconcileExternalPagesIds = this.exteranlPageLinesIds;
        }


    }

    get RegisterDate() { return this.newARPaymentPM.RegisterDate; }
    set RegisterDate(newValue: Date) {
        if (this.newARPaymentPM.RegisterDate != newValue) {
            this.newARPaymentPM.RegisterDate = newValue;
            this.LoadData();
        }
    }
    get PrintNotes() { return this.newARPaymentPM.PrintNotes; }
    set PrintNotes(newValue: string) {
        if (this.newARPaymentPM.PrintNotes != newValue) {
            this.newARPaymentPM.PrintNotes = newValue;
        }
    }
    public TodayDate: Date = new Date();

    get SelectableDateStart() { return this.TodayDate.setFullYear(this.TodayDate.getFullYear() - 100); }
    get SelectableDateEnd() { return this.TodayDate.setFullYear(this.TodayDate.getFullYear() + 100); }
    get SelectablePaymentDateEnd() { return this.TodayDate; }

    get AccountingPaymentMethodId() { return this.newARPaymentPM.AccountingPaymentMethodId; }
    set AccountingPaymentMethodId(newValue: string) {
        if (this.newARPaymentPM.AccountingPaymentMethodId != newValue) {
            this.newARPaymentPM.AccountingPaymentMethodId = newValue;
            this.RefreshPaymentMethodFields();
        }
    }

    get AccountingPaymentMethodCode() { return this.newARPaymentPM.AccountingPaymentMethodCode; }
    set AccountingPaymentMethodCode(newValue: string) {
        if (this.newARPaymentPM.AccountingPaymentMethodCode != newValue) {
            this.newARPaymentPM.AccountingPaymentMethodCode = newValue;
        }
    }

    get SATPaymentMethodCode() {
        if (this.newARPaymentPM == null) {
            return null;
        }
        return this.newARPaymentPM.SATPaymentMethodCode;
    }
    set SATPaymentMethodCode(value: string) {
        if (this.newARPaymentPM != null) {
            if (this.newARPaymentPM.SATPaymentMethodCode != value) {
                this.newARPaymentPM.SATPaymentMethodCode = value;
                this.SetUIProperties_Payment();
            }
        }
    }

    get DebitAccountId() { return this.newARPaymentPM.DebitAccountId; }
    set DebitAccountId(newValue: string) {
        if (this.newARPaymentPM.DebitAccountId != newValue) {
            this.newARPaymentPM.DebitAccountId = newValue;
        }
    }

    get DebitAccountDependencyProperty1() { return "AR,BN"; }

    get PartnerId() { return this.EntityPM.PartnerId; }
    set PartnerId(newValue: string) {
        if (this.EntityPM.PartnerId != newValue) {
            this.EntityPM.PartnerId = newValue;
            if (AppTool.IsNullOrEmpty(newValue)) {
                this.BillToId = null;
            }
            else {
                var myService: CardListService = new CardListService();
                myService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list != null) {
                            this.BillToId = list.BillToId;
                            if (AppTool.IsNullOrEmpty(this.BillToId)) {
                                this.BillToId = newValue;
                            }
                        }
                    }
                });
            }
        }
    }

    // BillTo Properties
    get BillToId() { return this.newARPaymentPM.BillToId; }
    set BillToId(newValue: string) {
        if (this.IsCreatedFromInvoiceSide == false) {
            if (this.newARPaymentPM.BillToId != newValue) {
                this.newARPaymentPM.BillToId = newValue;
                this.GetCardProperties();
            }
        }
    }

    get BillToName() { return this.newARPaymentPM.BillToName; }
    set BillToName(newValue: string) {
        if (this.newARPaymentPM.BillToName != newValue) {
            this.newARPaymentPM.BillToName = newValue;
        }
  }


  get MetodoPagoCode() { return this.newARPaymentPM.MetodoPagoCode; }
  set MetodoPagoCode(newValue: string) {
    if (this.newARPaymentPM) {
      if (this.newARPaymentPM.MetodoPagoCode != newValue) {
        this.newARPaymentPM.MetodoPagoCode = newValue;
        this.SetUIProperties_Payment();
      }
    }
  }

    billtoCard:CardList;
    GetCardProperties() {
        if (AppTool.IsNullOrEmpty(this.newARPaymentPM.BillToId)) {
            this.FillDataFromCardList(new CardList());
        }

        else {
            var myService: CardListService = new CardListService();
            myService.getSingle(this.newARPaymentPM.BillToId).subscribe((resp: ServiceResponse) => {
                if (resp != null) {
                    if (!resp.HasError) {
                        var cardList = resp.Result;
                        if (cardList != null) {
                            this.billtoCard = cardList;
                            this.FillDataFromCardList(cardList);
                        }
                    }
                }
            });
        }
    }

    FillDataFromCardList(list: CardList) {
        if (list == null) {
            this.BillToAddressId = null;
            this.BillToName = null;
            this.AccountingPaymentMethodId = null;
            this.AccountingPaymentMethodCode = null;
            this.SATPaymentMethodCode = null;
            if(!this.preSelectedCurrencyId)
                this.PaymentCurrencyId = SessionLocator.TenantPM.CurrencyId;
            this.newARPaymentPM.BillToPartnerTypeId = null;
        }

        else {
            if (!AppTool.IsNullOrEmpty(list.InvoiceCurrencyId) && !this.preSelectedCurrencyId) {
                this.PaymentCurrencyId = list.InvoiceCurrencyId;
            }
            if (!AppTool.IsNullOrEmpty(list.SATPaymentMethodCode)) {
               // this.ARPaymentMethodCode = list.SATPaymentMethodCode;
            }

            this.newARPaymentPM.BillToPartnerTypeId = list.PartnerTypeId;
            this.BillToName = list.EnglishName;
            this.LoadAddress();

            if (SessionLocator.TenantPM.AccountingActivated == true) {
                if (!AppTool.IsNullOrEmpty(list.GLAccountId)) {
                    var myGLAccountPMService = new GLAccountPMService();
                    myGLAccountPMService.get(list.GLAccountId).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var glaccount: GLAccountPM = myResponse.Result;
                            if (glaccount != null && !glaccount.IsMultiCurrency && !this.preSelectedCurrencyId) {
                                this.PaymentCurrencyId = glaccount.CurrencyId;
                            }
                        }
                    });
                }
            }
            //if (!AppTool.IsNullOrEmpty(list.SATPaymentMethodCode)) {
            //    this.SATPaymentMethodCode = list.SATPaymentMethodCode;
            //}
        }
    }

    LoadAddress() {
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetAddressByCardAndType(this.newARPaymentPM.BillToId, "B").subscribe((resp: any) => {
            var billingAddress = resp;
            if (billingAddress != null) {
                var item = billingAddress;
                this.BillToAddressId = item.Id;
            }

            else {
                this.GetBillingAddress();
            }

        });
    }

    GetBillingAddress() {
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetBillingAddressListByCardId(this.newARPaymentPM.BillToId).subscribe((resp: any) => {
            var billingAddress = resp;
            if (billingAddress != null) {
                this.BillToAddressId = billingAddress.Id;
            }

            else {
                var myService: PartnersDomainService = new PartnersDomainService();
                myService.GetAddressByCardAndType(this.newARPaymentPM.BillToId, "M").subscribe((resp: any) => {
                    if (resp != null) {
                        var mainAddress = resp;
                        if (mainAddress != null) {
                            var item = mainAddress;
                            this.BillToAddressId = item.Id;
                        }
                        else {

                            this.GetMainAddressListByCardId();
                        }
                    }
                });
            }
        });
    }

    GetMainAddressListByCardId() {
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetMainAddressListByCardId(this.newARPaymentPM.BillToId).subscribe((resp: any) => {
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

    get BillToAddressId() { return this.newARPaymentPM.BillToAddressId; }
    set BillToAddressId(newValue: string) {
        if (this.IsCreatedFromInvoiceSide == false) {
            if (this.newARPaymentPM.BillToAddressId != newValue) {
                this.newARPaymentPM.BillToAddressId = newValue;
            }
        }
    }

    // Currency Properties
    get PaymentCurrencyIsEnabled() {
        if (this.invoicePm != null) {
            return AppTool.IsNullOrEmpty(this.invoicePm.Id);
        }
        else
            return false;
    }

    get PaymentCurrencyId() { return this.newARPaymentPM.PaymentCurrencyId; }
    set PaymentCurrencyId(newValue: string) {
        if (this.IsCreatedFromInvoiceSide == false) {
            if (this.newARPaymentPM.PaymentCurrencyId != newValue) {
                this.newARPaymentPM.PaymentCurrencyId = newValue;
                this.SetUIProperties();
                this.SetCurrencyCode();
                this.SetCurrencyRateData();
                this.ComputeTotals();
            }
        }
    }

    get PaymentCurrencyExchangeRate() { return this.newARPaymentPM.PaymentCurrencyExchangeRate; }
    set PaymentCurrencyExchangeRate(newValue: number) {
        if (this.IsCreatedFromInvoiceSide == false) {
            if (this.newARPaymentPM.PaymentCurrencyExchangeRate != newValue) {
                this.newARPaymentPM.PaymentCurrencyExchangeRate = AppTool.Round(newValue, 5);
                this.ComputeTotals();
            }
        }
    }

    get ExchangeRateDate() { return this.newARPaymentPM.ExchangeRateDate; }
    set ExchangeRateDate(newValue: Date) {
        if (this.newARPaymentPM.ExchangeRateDate != newValue) {
            this.newARPaymentPM.ExchangeRateDate = newValue;
        }
    }

    get RelativeRateDate() { return DateTool.GetRelativeRateDate(this.newARPaymentPM.RegisterDate, this.ExchangeRateDate, "old"); }

    SetCurrencyCode() {
        if (!AppTool.IsNullOrEmpty(this.PaymentCurrencyId)) {
            var myService: CurrencyListService = new CurrencyListService();
            myService.getSingleFromCache(this.PaymentCurrencyId).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    if (response != null) {
                        var currency = response.Result;
                        if (currency != null) {
                            this.newARPaymentPM.PaymentCurrencyCode = currency.Code;
                            this.newARPaymentPM.PaymentCurrencySign = currency.Sign;
                        }
                    }
                }
            });
        }
        else {
            var myService: CurrencyListService = new CurrencyListService();
            myService.getSingle(this.PaymentCurrencyId).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    if (response != null) {
                        var list = response.Result;
                        if (list != null && list.length > 0) {
                            this.newARPaymentPM.PaymentCurrencyCode = list.Code;
                            this.newARPaymentPM.PaymentCurrencySign = list.Sign;
                        }
                    }
                }
            });
        }
    }

    // Payment Line Properties
    RefreshPaymentMethodFields() {
        this.newARPaymentPM.Bank = null;
        this.newARPaymentPM.BankBranch = null;
        this.newARPaymentPM.Account = null;
        // this.newARPaymentPM.ChequeOrPaymentRef = null;

        var lists: AccountingPaymentMethodList[] = this.AllMethods.filter(d => d.Id == this.AccountingPaymentMethodId);
        if (lists) {
            var list = lists[0];
            if (list) {
                this.AccountingPaymentMethodCode = list.Code;
            }
        }

        if (this.AccountingPaymentMethodCode == "CA" || this.AccountingPaymentMethodCode == "FS") {
            this.newARPaymentPM.ValueDate = DateTool.GetCurrentDateAsUtc();
        }
    }

    get AmountInPaymentCurrency() { return this.newARPaymentPM.AmountInPaymentCurrency; }
    set AmountInPaymentCurrency(newValue: number) {
        if (this.newARPaymentPM.AmountInPaymentCurrency != newValue) {
            this.newARPaymentPM.AmountInPaymentCurrency = AppTool.Round(newValue,2);
            this.ComputeTotals();
        }
    }

    get AmountInLocalCurrency() { return this.newARPaymentPM.AmountInLocalCurrency; }
    set AmountInLocalCurrency(newValue: number) {
        if (this.newARPaymentPM.AmountInLocalCurrency != newValue) {
            this.newARPaymentPM.AmountInLocalCurrency = AppTool.Round(newValue, 2);
        }
    }

    get OpenAmount() { return this.newARPaymentPM.OpenAmount; }
    set OpenAmount(newValue: number) {
        if (this.newARPaymentPM.OpenAmount != newValue) {
            this.newARPaymentPM.OpenAmount = AppTool.Round(newValue, 2);

        }
    }

    get OpenAmountInLocalCurrency() { return this.EntityPM.OpenAmountInLocalCurrency == null ? 0 : this.EntityPM.OpenAmountInLocalCurrency; }
    set OpenAmountInLocalCurrency(value: number) {
        if (this.EntityPM.OpenAmountInLocalCurrency != value) {
            this.EntityPM.OpenAmountInLocalCurrency = AppTool.Round(value, 2);
        }
    }


    get BankAccountLiteId() { return this.newARPaymentPM.BankAccountLiteId; }
    set BankAccountLiteId(newValue: string) {
        if (this.newARPaymentPM.BankAccountLiteId != newValue) {
            this.newARPaymentPM.BankAccountLiteId = newValue;
        }
    }

    get BranchId() { return this.newARPaymentPM.BranchId; }
    set BranchId(value: string) {
        if (this.newARPaymentPM.BranchId != value) {
            this.newARPaymentPM.BranchId = value;
        }
    }

    ComputeTotals() {
        this.OpenAmount = this.AmountInPaymentCurrency;
        this.OpenAmountInLocalCurrency = this.PaymentCurrencyExchangeRate == null ? 0 : this.OpenAmount * this.PaymentCurrencyExchangeRate;
        this.AmountInLocalCurrency = this.PaymentCurrencyExchangeRate == null ? 0 : this.AmountInPaymentCurrency * this.PaymentCurrencyExchangeRate;
    }

    //Commands
    UpdateCurrencyRateClicked() {
        this._entityResourceService.getEntityResourceByTableName("RatesTable", 0).subscribe((response: any) => {

            var loadingDate = this.newARPaymentPM.RegisterDate;
            if (loadingDate == null) {
                loadingDate = DateTool.GetCurrentDateAsUtc();
            }
            var logWindow = new LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 350;
            logWindow.Title = "Update Currency Rate";
            logWindow.WindowArgs = { CurrencyId: this.newARPaymentPM.PaymentCurrencyId, CurrencyCode: this.newARPaymentPM.PaymentCurrencyCode, Rate: this.newARPaymentPM.PaymentCurrencyExchangeRate, Date: loadingDate };
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

    SetUIProperties_Payment() {
        this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, false);
        this.UIProperties.SetRequired("SATPaymentMethodCode", this.ObjectTableName, false);
        this.UIProperties.SetRequired("MetodoPagoCode", this.ObjectTableName, false);

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (AppTool.IsNullOrEmpty(this.SATPaymentMethodCode)) {
                this.UIProperties.SetRequired("SATPaymentMethodCode", this.ObjectTableName, true);
          }

          if (AppTool.IsNullOrEmpty(this.MetodoPagoCode)) {
              this.UIProperties.SetRequired("MetodoPagoCode", this.ObjectTableName, true);

              this.ValidateTipoCadenaPagoFields();
          }
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    OkButtonClicked() {
        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (AppTool.IsNullOrEmpty(this.newARPaymentPM.BillToId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.F.BillToId")));
        }

        if (AppTool.IsNullOrEmpty(this.newARPaymentPM.BillToAddressId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.F.BillToAddressId")));
        }

        if (AppTool.IsNullOrEmpty(this.newARPaymentPM.PaymentCurrencyId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.F.PaymentCurrencyCode")));
        }

        if (this.RegisterDate == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.F.RegisterDate")));
        }

        else if (DateTool.GetDateParts(this.RegisterDate).DateTicks > DateTool.GetCurrentDateAsUtcForAccountingValidation(SessionLocator.TenantPM.TimeZoneOffset).valueOf()) {
            errors.push(TextCodeTranslator.Translate("ARPayment.M.CantSetFutureDatePayment"));
        }

        if (AppTool.IsNullOrEmpty(this.newARPaymentPM.AccountingPaymentMethodId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.F.ARPaymentMethodCode")));
        }

        if (this.newARPaymentPM.AmountInPaymentCurrency == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.F.AmountInPaymentCurrency")));
        }

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (AppTool.IsNullOrEmpty(this.SATPaymentMethodCode)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate( "ARPayment.F.SATPaymentMethodCode")));
            }

            if (AppTool.IsNullOrEmpty(this.MetodoPagoCode)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.F.MetodoPagoCode")));
          }

          if (this.SATPaymentMethodCode == "99") {
            errors.push("Forma Pago value can't be 'Por Definir'.Please choose another value.");
            }


            if (!AppTool.IsNullOrEmpty(this.TipoCadenaPago) && this.TipoCadenaPago == "01" && this.SATPaymentMethodCode == "03") {
                if (AppTool.IsNullOrEmpty(this.CertPago))
                    errors.push(msg.replace("%FieldName", "Cert Pago"));

                if (AppTool.IsNullOrEmpty(this.CadPago))
                    errors.push(msg.replace("%FieldName", "Cad Pago"));

                if (AppTool.IsNullOrEmpty(this.SelloPago))
                    errors.push(msg.replace("%FieldName", "Sello Pago"));
            }
        }

        if (this.newARPaymentPM.AmountInPaymentCurrency == 0) {
            var isAllowed = false;
            if (this.newARPaymentPM.AccountingPaymentMethodCode != null) {
                if (this.newARPaymentPM.AccountingPaymentMethodCode.toUpperCase() == "FS") {
                    isAllowed = true;
                }
            }

            if (!isAllowed) {
                errors.push(TextCodeTranslator.Translate("ARPayment.M.CantSetZeroAmount"));
            }
        }

        if (this.newARPaymentPM.AmountInPaymentCurrency < 0) {
            if (!this.IsNegativeAmountEnabled) {
                errors.push(TextCodeTranslator.Translate("ARPayment.M.CantSetMinusAmount"));
            }
        }

        if (AppTool.IsNullOrEmpty(this.newARPaymentPM.BranchId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.F.BranchId")));
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            if (this.IsCreatedFromInvoiceSide) {
                if (this.BillToId != this.invoicePm.BillToId) {
                    errors.push("Bill to doesn't match the invoice bill to");
                }

                if (this.PaymentCurrencyId != this.invoicePm.InvoiceCurrencyId) {
                    errors.push("Payment currency doesn't match the invoice currency");
                }
            }
        }

        if (this.ValidationErrorsList.length == 0) {
            // Check Full Accounting
            if (SessionLocator.TenantPM.AccountingActivated == true /*&& (this.newARPaymentPM.ARPaymentMethodCode == "CH" || this.newARPaymentPM.ARPaymentMethodCode == "CA")*/) {
                var invoiceDomainService: InvoiceDomainService = new InvoiceDomainService();
                invoiceDomainService.ValidateARPaymentFullAccounting(this.newARPaymentPM.AccountingPaymentMethodCode, this.newARPaymentPM.PaymentCurrencyId, this.newARPaymentPM.BillToId, this.newARPaymentPM.AccountingPaymentMethodCode, this.newARPaymentPM.RegisterDate, this.newARPaymentPM.BankAccountId).subscribe((response: ServiceResponse) => {
                    if (response != null) {
                        if (!response.HasError) {

                            //var validate = response.Result;
                            //if (validate == null) {
                            //    errors.push("There is no appropriate Cashbook for this payment, You have to create one");
                            //}

                            this.CompleteSubmission(errors);
                        }
                        else {
                            this.ValidationErrorsList = response.ErrorsArray;
                        }
                    }
                });
            }
            else {
                this.CompleteSubmission(errors);
            }
        }
    }

    CompleteSubmission(errors) {
        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            if (this.invoicePm != null && !AppTool.IsNullOrEmpty(this.invoicePm.Id)) {
                this.ConnectARInvoiceToPayment(this.newARPaymentPM, this.invoicePm);
            }

            if (SessionLocator.TenantPM.AccountingActivated) {
                this.fetchGLAccount()
                    .then((res:any) => {
                        this.newARPaymentPM.GLAccountId = res.Id;
                        this.newARPaymentPM.GLAccountRecoMethodCode = res.ReconcileMethodCode;
                        this.newARPaymentPM.GLAccountCurrencyCode = res.CurrencyCode;
                        this.RunEditWindow();
                    }, err => {
                        this.ValidationErrorsList = ['Somthing wrong! no gl account found for this bill to account'];
                        return;
                    })
            } else {
                this.RunEditWindow();
            }
        }
    }
    ConnectARInvoiceToPayment(entityPM: ARPaymentPM, invoicePM: ARInvoicePM) {
        var connectAmount = this.GetSmallestAmount(entityPM, invoicePM);
        var connectAmountLocal = connectAmount * invoicePM.InvoiceCurrencyExchangeRate;

        var record: ARPaymentInvoicePM = new ARPaymentInvoicePM(null);

        record.Tenant = SessionLocator.TenantPM.Id;
        record.ARInvoiceId = invoicePM.Id;
        record.ARPaymentId = entityPM.Id;
        record.ForeignCurrencyId = invoicePM.InvoiceCurrencyId;
        record.ForeignAmount = connectAmount == null ? 0 : connectAmount;
        record.LocalAmount = connectAmountLocal == null ? 0 : connectAmountLocal;
        record.PaymentAmount = record.ForeignAmount;
        record.ExchangeRate = invoicePM.InvoiceCurrencyExchangeRate;
        record.ARInvoiceMetodoPagoCode = invoicePM.MetodoPagoCode;

        entityPM.PaymentInvoices = [];
        entityPM.PaymentInvoices.push(record);

        if (entityPM.OpenAmount > connectAmount) {
            entityPM.OpenAmount = entityPM.OpenAmount - connectAmount;
        }

        else {
            entityPM.OpenAmount = 0;
        }
    }
    GetSmallestAmount(paymentPM: ARPaymentPM, invoicePM: ARInvoicePM): number {
        var invoiceAmount = invoicePM.AmountDue;
        var paymentAmount = paymentPM.OpenAmount == null ? 0 : paymentPM.OpenAmount;
        var smallestAmount = null;

        if (invoiceAmount <= paymentAmount) {
            smallestAmount = invoiceAmount;
        }

        else {
            smallestAmount = paymentAmount;
        }

        return smallestAmount;
    }
    RunEditWindow() {

        this.CurrentSession.CurrentWindow.WindowClosed.subscribe(s => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.newARPaymentPM.Id, EntityPM: this.newARPaymentPM, ObjectTableName: 'ARPayment' });

                    let isEditComponentSaved = false;

                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        if (isEditComponentSaved) {
                            this.CurrentSession.FireEvent("NewARPaymentInvoiceTabCreated");
                            if(this.EntityPM.ForceUsingBankTransferMethod){
                                const paymentNo = cmpRef.instance.EntityPM.PaymentNo;
                                this.CurrentSession.FireEvent({Name: "BankTransferARPaymentCreated", PaymentNumber: paymentNo});
                            }
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
        });

        this.CurrentSession.CloseCurrentWindow();
    }

    fetchGLAccount() {
        return new Promise((resolve, reject) => {

            var _glaId = this.billtoCard.GLAccountId;
            this.CurrentSession.StartBusyIndicatorLoading();
            this._glaService.getSingle(_glaId)
                .subscribe((response:any) => {

                    var res: ServiceResponse = response;
                    if (!res.HasError) {
                        var glaccount = res.Result;

                        resolve(glaccount);
                        this.CurrentSession.StopBusyIndicator();
                    }
                    else {
                        reject();

                        this.ValidationErrorsList = res.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                });

        });
    }

    AllowedPartnerTypesCodes: string[] = ['CS','AG','AC','AL','CG','SG','SL','TR','VD','WH'];
    PartnerTypes: PartnerTypeList[] = [];
    filterByPartnerTypeCode: string;
    isPartnerTypesFilterEnabled: boolean = false;
    getPartnerTypes(){
        this._PartnerTypeListService.getAll().subscribe((res:ServiceResponse)=>
        {
            console.log('[PartnerTypeListService]');
            var partnerTypes: PartnerTypeList[] = res.Result || [];
            this.PartnerTypes = partnerTypes.filter(d => this.AllowedPartnerTypesCodes.indexOf(d.Id) > -1); // filter
            this.SelectedPartnerType = partnerTypes.filter(d => d.Id == 'CS')[0]; // default
        });
    }

    checkPartnerTypesFilterFeature(){
        var arpaymentOT = window.ObjectTables.filter(d => d.Name === "ARPayment")[0];
          this.isPartnerTypesFilterEnabled =  FeatureLocator.Features.filter(f => (f.Code == "NewScreenPartnerTypes") && f.ObjectTableId == arpaymentOT.Id)[0]? true : false;
    }
    private _SelectedPartnerType : PartnerTypeList;
    public get SelectedPartnerType() : PartnerTypeList {
        return this._SelectedPartnerType;
    }
    public set SelectedPartnerType(type : PartnerTypeList) {
        this._SelectedPartnerType = type;
        this.filterByPartnerTypeCode = type.Id;
        this.PartnerId = null;

    }

}

class TipoCadenaPagoClass {
    public Code: string;
    public Name: string;
}
