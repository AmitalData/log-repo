import {Component, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ARInvoicePM} from '../../../../Invoice/EntityPMs/ARInvoicePM';
import {ARInvoiceLinePM} from '../../../../Invoice/EntityPMs/ARInvoiceLinePM';
import {ARInvoiceTotalVATPM} from '../../../../Invoice/EntityPMs/ARInvoiceTotalVATPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {DateTool, AppTool, ArrayTool, FontTool} from '../../../../Infrastructure/Tools';
import {CurrencyRatesService, LastRate} from '../../../../Common/Services/CurrencyRatesService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SummaryItem, InvoiceTotalsClass} from '../../../../Invoice/Args';
import {InvoiceTool} from '../../../../Invoice/Tools';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {PaymentTermList} from '../../../../Common/EntityLists/PaymentTermList';
import {VatTypeList} from '../../../../Common/EntityLists/VatTypeList';
import {ChargesTypeList} from '../../../../Common/EntityLists/ChargesTypeList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {PaymentTermListService} from '../../../../Common/Services/StandardLists/PaymentTermListService';
import {VatTypeListService} from '../../../../Common/Services/StandardLists/VatTypeListService';
import {ChargesTypeListService} from '../../../../Common/Services/StandardLists/ChargesTypeListService';
import {CommonDomainService} from '../../../../Common/Services/CommonDomainService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {VatTypePercentagePM} from '../../../../Common/EntityPMs/VatTypePercentagePM';
import {VATTypesGroupPM} from '../../../../Common/EntityPMs/VATTypesGroupPM';
import {NumbersPipe} from '../../../../Infrastructure/Pipes/NumbersPipe';
import {ShipmentDomainService} from '../../../../Shipment/Services/ShipmentDomainService';
import {ShipmentReceivablePM} from '../../../../Shipment/EntityPMs/ShipmentReceivablePM';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {InvoiceDomainService} from '../../../../Invoice/Services/InvoiceDomainService';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { ARInvoiceStockLinePM } from '../../../../Invoice/EntityPMs/ARInvoiceStockLinePM';

@Component({
    
    templateUrl: './ARInvoiceDetailsTabNormal.html',
})

export class ARInvoiceDetailsTabNormal extends BaseComponent implements OnDestroy {
    public EntityPM: ARInvoicePM = null;
    public ObjectTableName = "ARInvoice";
    public DataContext = this;
    public ItemsSource: ARInvoiceLineItem[] = [];
    public ObservableItems: ObservableCollection;
    public LocalCurrencyId: string;
    public LocalCurrencyCode: string;
    public IsManifest: boolean = false;
    public EntityWarningsList: string[] = [];
    public EntityWarning: string = "";
    public IsCustomsInvoice: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public InvoiceNumberFilterList: CodeNameClass[] = [];
    public NumbersPipe: NumbersPipe;

    constructor(private entityArgs: EntityArgs) {
        super();
        this.NumbersPipe = new NumbersPipe();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");          
        this.EntityPM = entityArgs.EntityPM;
        this.IsManifest = this.EntityPM.ARInvoiceTypeCode == "MN" ? true : false;
        this.IsCustomsInvoice = (this.EntityPM.ARInvoiceTypeCode == "CI" || this.EntityPM.ARInvoiceTypeCode == "CC") ? true : false;
        this.ObservableItems = new ObservableCollection([]); 
        this.LocalCurrencyId = SessionLocator.LocalCurrencyId;
        this.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
        this.InitializeServices();
        this.InitializeComponent();        
        this.SetUIProperties();        
        this.BuildScreenData();
        this.ShowFixMe();
        this.Listen();

        this.BuildEntityWarnings();

        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }

        if (SessionLocator.AccountingSettingPM.EnableInvoiceStocksManagement) {
            if (!this.EntityPM.IsConstituentInvoice) {
                this.AllowStockInvoiceNumber = true;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.ARInvoiceStockId)) {
            this.IsInvoiceNumberComboBoxEnabled = false;
        }

        this.BuildInvoiceNumberFilters();
        this.SetRegionalTaxVisibility();
    }

    private SetRegionalTaxValuesForLines() {
        this.ItemsSource.filter(f=>f.VatIsMultiPercentage == false).forEach(item => {
            this.myChargesTypeListService.getSingle(item.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: ChargesTypeList = myResponse.Result;
                    if (list != null) {
                        item.IsRegionalTax = list.ApplyRegionalTax;
                    }
                }
            });
        });
    }

    public IsFixMeButtonVisible: boolean = false;
    private BuildEntityWarnings() {
        this.EntityWarning = "";
        this.EntityWarningsList = [];
        if (!AppTool.IsNullOrEmpty(this.EntityPM.TransmissionError)) {
            this.EntityWarningsList.push(this.EntityPM.TransmissionError);
            this.EntityWarning = this.EntityPM.TransmissionError;
        }
    }

    ShowFixMe() {
        this.IsFixMeButtonVisible = false;

        if (this.EntityPM.Tenant == 570) {
            if (AppTool.IsNullOrZero(this.EntityPM.InvoiceCurrencyExchangeRate) || AppTool.IsNullOrZero(this.EntityPM.ProfitCurrencyExchangeRate)) {
                if (SessionLocator.LoggedUserId == "1-23905" || SessionLocator.LoggedUserId == "1-3840") {
                    this.IsFixMeButtonVisible = true;
                }
            }
        }
    }
    FixMeButtonFlicked() {
        this.UpdateData();
        this.ShowFixMe();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.BuildInvoiceLines();
                    this.BuildEntityWarnings();
                }

                else {
                    if (this.IsgetFromStockAfterSaving) {
                        this.InvoiceNumber = null;
                        this.ARInvoiceStockId = null;
                        this.IsInvoiceNumberComboBoxEnabled = true;
                    }
                }          
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.BuildInvoiceLines();
                    this.BuildEntityWarnings();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    public AllVatTypes: VatTypeList[] = [];
    private myCardListService: CardListService;
    private myCurrencyListService: CurrencyListService;
    private myPaymentTermListService: PaymentTermListService;
    public myVatTypeListService: VatTypeListService;
    public myChargesTypeListService: ChargesTypeListService;
    private myCommonDomainService: CommonDomainService;
    private myInvoiceDomainService: InvoiceDomainService;
    InitializeServices() {
        this.myCardListService = new CardListService();
        this.myCurrencyListService = new CurrencyListService();
        this.myPaymentTermListService = new PaymentTermListService();
        this.myVatTypeListService = new VatTypeListService();
        this.myChargesTypeListService = new ChargesTypeListService();
        this.myCommonDomainService = new CommonDomainService();
        this.myInvoiceDomainService = new InvoiceDomainService();
    }
    InitializeComponent() {
        this.SetGridColumns();
        this.ComputeRelativeRateDate();
        this.BillToDependencyValue1 = InvoiceTool.GetBillToPartnerTypes();
        this.GetAllVatTypes();
    }
    GetAllVatTypes() {
        this.myVatTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllVatTypes = myResponse.Result;
            }
        });
    }

    public LocalAmountHeader: string = null;
    public InvoiceAmountHeader: string = null;
    public IsInvoiceAmountHeaderVisible: boolean = false;
    public VATColumnWidth: number = 100;
    public RateColumnWidth: number = 100;
    SetGridColumns() {
        this.LocalAmountHeader = TextCodeTranslator.Translate("ARInvoiceLine.CH.LocalCurrencyAmountListLable").replace("%LocalCurrencyCode", this.LocalCurrencyCode);

        var invoiceAmountHeader = null;
        var isInvoiceAmountHeaderVisible = false;
        if (!AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            if (this.InvoiceCurrencyId != this.LocalCurrencyId) {
                isInvoiceAmountHeaderVisible = true;
                invoiceAmountHeader = TextCodeTranslator.Translate("ARInvoiceLine.CH.ForiegnCurrencyAmountListLable").replace("%ForiegnCurrencyCode", this.InvoiceCurrencyCode);
            }
        }

        this.InvoiceAmountHeader = invoiceAmountHeader;
        this.IsInvoiceAmountHeaderVisible = isInvoiceAmountHeaderVisible;
    }
    SetGridColumnsWidth() {
        var myRateColumnWidth = 100;
        var myVATColumnWidth = 100;

        this.ItemsSource.forEach(item => {

            // Rate
            var myRate: string = this.NumbersPipe.transform(item.ForiegnExchangeRate, 'N5');
            var myRelativeRateDate = item.RelativeRateDate;

            if (AppTool.IsNullOrEmpty(myRate)) {
                myRate = "";
            }

            if (AppTool.IsNullOrEmpty(myRelativeRateDate)) {
                myRelativeRateDate = "";
            }

            var myRateCellText = myRate + myRelativeRateDate;
            var myRateCellWidth = AppTool.GetTextWidth(myRateCellText, 12) + 10;
            if (myRateCellWidth > myRateColumnWidth) {
                myRateColumnWidth = myRateCellWidth;
            }

            // VAT
            var myVATTextWidth = AppTool.GetTextWidth(item.VatTypeCell, 12) + 10;
            if (item.VatTypeUpdateIsVisible) {
                myVATTextWidth += 20;
            }

            if (myVATTextWidth > myVATColumnWidth) {
                myVATColumnWidth = myVATTextWidth;
            }
        });

        this.VATColumnWidth = myVATColumnWidth;
        this.RateColumnWidth = myRateColumnWidth;
    }

    // SetUIProperties
    public IsEditingEnabled: boolean = false;
    public IsStockEnabled: boolean = false;
    public RateIsEnabled: boolean = false;
    public VatTypeFilterIsEnabled: boolean = false;
    public AllowManualInvoiceNumber: boolean = false;
    public AllowStockInvoiceNumber: boolean = false;
    public PaymentTermDisplayInLOV: boolean = true;
    SetUIProperties() {
        var isEditingEnabled = InvoiceTool.IsEditingARInvoiceEnabled(this.EntityPM);
        var isInvoiceDateEnabled = isEditingEnabled;
        
        if (this.EntityPM.IsAutoCredit && AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            isInvoiceDateEnabled = true;
        }

        if (!AppTool.IsNullOrEmpty(this.BillToAddressId)) {
            this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, isEditingEnabled);
        }
        this.UIProperties.SetEnabled("PartnerId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("PaymentTermId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("VatNumber", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("InvoiceDate", this.ObjectTableName, isInvoiceDateEnabled);
        this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("RegionalTaxId", this.ObjectTableName, isEditingEnabled);

        // Generated General Tab
        if (this.EntityPM != null) {
            this.EntityPM.UIProperties.SetEnabled("UpdateDate", this.ObjectTableName, false);
            this.EntityPM.UIProperties.SetEnabled("UpdatedByUserId", this.ObjectTableName, false);
            this.EntityPM.UIProperties.SetEnabled("Sent", this.ObjectTableName, isEditingEnabled);
            this.EntityPM.UIProperties.SetEnabled("HouseNumber", this.ObjectTableName, isEditingEnabled);
            this.EntityPM.UIProperties.SetEnabled("MasterNumber", this.ObjectTableName, isEditingEnabled);
            this.EntityPM.UIProperties.SetEnabled("CustomerRef", this.ObjectTableName, isEditingEnabled);
            this.EntityPM.UIProperties.SetEnabled("BranchId", this.ObjectTableName, isEditingEnabled);
            this.EntityPM.UIProperties.SetEnabled("SATPaymentMethodCode", this.ObjectTableName, isEditingEnabled);
        }
       
        this.IsEditingEnabled = isEditingEnabled;
        this.SetUIProperties_BillToAddress();
        this.SetUIProperties_VatNumber();
        this.SetUIProperties_ExchangeRate();
        this.SetUIProperties_PrintNotes();        
        this.SetUIProperties_ManuallySet();
        this.SetUIProperties_InvoiceNumber();
        this.SetUIProperties_VatTypeFilter();
        this.SetUIProperties_DueDate();
    }
    SetUIProperties_BillToAddress() {
        if (!AppTool.IsNullOrEmpty(this.BillToAddressId)) {
            var isFieldtEnabled = false;

            if (this.IsEditingEnabled) {
                if (!AppTool.IsNullOrEmpty(this.BillToId)) {
                    isFieldtEnabled = true;
                }
            }

            this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, isFieldtEnabled);
        }
    }
    SetUIProperties_VatNumber() {
        var isFieldRequired = false;

        if (SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAR) {
            if (AppTool.IsNullOrEmpty(this.VatNumber)) {
                isFieldRequired = true;
            }

            this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, isFieldRequired);
        }
    }
    SetUIProperties_ExchangeRate() {
        var isFieldtEnabled = false;

        if (this.IsEditingEnabled) {
            if (FeatureLocator.HasFeaturePermession("ARInvoice", "ARInvoiceEditExchangeRate")) {
                if (this.InvoiceCurrencyId) {
                    if (this.InvoiceCurrencyId != SessionLocator.TenantPM.CurrencyId) {
                        isFieldtEnabled = true;
                    }
                }
            }
        }
        
        this.RateIsEnabled = isFieldtEnabled;
        this.UIProperties.SetEnabled("InvoiceCurrencyExchangeRate", this.ObjectTableName, isFieldtEnabled);
    }
    SetUIProperties_PrintNotes() {
        var isFieldtEnabled = true;

        if (this.EntityPM.StatusCode == "VD") {
            isFieldtEnabled = false;
        }

        else if (this.EntityPM.IsPrinted) {
            isFieldtEnabled = false;
        }

        this.UIProperties.SetEnabled("PrintNotes", this.ObjectTableName, isFieldtEnabled);
    }
    SetUIProperties_ManuallySet() {

        var isFieldtEnabled = this.IsEditingEnabled;
        var isFieldtVisible = false;

        if (this.IsInvoiceNumberManuallySet) {
            isFieldtVisible = true;
        }

        else {
            if (SessionLocator.AccountingSettingPM.AllowManualInvoiceNumber) {
                if (!this.EntityPM.IsConstituentInvoice) {
                    isFieldtVisible = true;
                }
            }
        }

        this.AllowManualInvoiceNumber = isFieldtVisible;
        this.UIProperties.SetEnabled("IsInvoiceNumberManuallySet", this.ObjectTableName, isFieldtEnabled);
        this.UIProperties.SetVisibility("IsInvoiceNumberManuallySet", this.ObjectTableName, isFieldtVisible);
    }
    SetUIProperties_InvoiceNumber() {
        var isFieldEnabled = false;
        this.IsStockEnabled = false;

        if (this.IsEditingEnabled || (this.EntityPM.IsAutoCredit && AppTool.IsNullOrEmpty(this.EntityPM.Id))) {
            this.IsStockEnabled = true;

            if (this.IsInvoiceNumberManuallySet) {
                isFieldEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("InvoiceNumber", this.ObjectTableName, isFieldEnabled);
    }
    SetUIProperties_VatTypeFilter() {
        var isFieldtEnabled = this.IsEditingEnabled;

        if (isFieldtEnabled) {
            if (AppTool.IsNullOrEmpty(this.VatTypeId)) {
                isFieldtEnabled = false;
            }
        }

        this.VatTypeFilterIsEnabled = isFieldtEnabled;
    }
    SetUIProperties_DueDate() {
        var AllowManuallyDueDate: boolean = false;

        if (SessionLocator.AccountingSystemPM) {
            AllowManuallyDueDate = SessionLocator.AccountingSystemPM.AllowManuallyDueDate;
        }

        if (AllowManuallyDueDate) {
            this.PaymentTermDisplayInLOV = null;
        }

        if (!this.IsEditingEnabled) {
            AllowManuallyDueDate = false;
        }

        this.UIProperties.SetEnabled("DueDate", this.ObjectTableName, AllowManuallyDueDate);
    }

    get PartnerId() { return this.EntityPM.PartnerId; }
    set PartnerId(newValue: string) {
        if (this.EntityPM.PartnerId != newValue) {
            this.EntityPM.PartnerId = newValue;
            if (AppTool.IsNullOrEmpty(newValue)) {
                this.BillToId = null;
            }
            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
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

    // Bill To
    public BillToDependencyValue1: string = null;
    get BillToId() { return this.EntityPM.BillToId; }
    set BillToId(newValue: string) {
        if (this.EntityPM.BillToId != newValue) {
            this.EntityPM.BillToId = newValue;
            this.EntityPM.CustomerRef = null;
            this.SetUIProperties_BillToAddress();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.VatNumber = null;
                this.BillToName = null;
                this.BillToAddressId = null;
                this.InvoiceCurrencyId = SessionLocator.AccountingCurrencyId;
                this.PaymentTermId = SessionLocator.TenantPM.PaymentTermId;

                this.EntityPM.BillToIsCreditLimitEnabled = false;
                this.EntityPM.BillToCreditLimitAmount = null;
                this.EntityPM.BillToCreditLimitOpenBalance = null;
                this.EntityPM.BillToCreditLimitWarningPercentage = null;
                this.EntityPM.BillToBlockNewInvoiceCreation = false;
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {

                        var list: CardList = myResponse.Result;
                        if (list != null) {
                            this.VatNumber = list.VatNumber;
                            this.BillToName = list.EnglishName;

                            this.EntityPM.BillToIsCreditLimitEnabled = list.IsCreditLimitEnabled;
                            this.EntityPM.BillToCreditLimitAmount = list.CreditLimitAmount;
                            this.EntityPM.BillToCreditLimitOpenBalance = list.CreditLimitOpenBalance;
                            this.EntityPM.BillToCreditLimitWarningPercentage = list.CreditLimitWarningPercentage;
                            this.EntityPM.BillToBlockNewInvoiceCreation = list.BlockNewInvoiceCreation;
                            this.EntityPM.SalesmanUserId = list.SalesmanUserId;

                            if (!AppTool.IsNullOrEmpty(list.InvoiceCurrencyId)) {
                                this.InvoiceCurrencyId = list.InvoiceCurrencyId;
                            }

                            if (!AppTool.IsNullOrEmpty(list.PaymentTermId)) {
                                this.PaymentTermId = list.PaymentTermId;
                            }

                            if (!AppTool.IsNullOrEmpty(list.VatTypeId)) {
                                this.VatTypeId = list.VatTypeId;
                            }

                            if (!AppTool.IsNullOrEmpty(list.BillingAddressId)) {
                                this.BillToAddressId = list.BillingAddressId;
                            }

                            else if (!AppTool.IsNullOrEmpty(list.MainAddressId)) {
                                this.BillToAddressId = list.MainAddressId;
                            }
                        }
                    }
                });
            }
        }
    }

    get BillToName() { return this.EntityPM.BillToName; }
    set BillToName(newValue: string) {
        if (this.EntityPM.BillToName != newValue) {
            this.EntityPM.BillToName = newValue;
        }
    }

    get BillToAddressId() { return this.EntityPM.BillToAddressId; }
    set BillToAddressId(newValue: string) {
        if (this.EntityPM.BillToAddressId != newValue) {
            this.EntityPM.BillToAddressId = newValue;
        }
    }

    // Currency
    get InvoiceCurrencyId() { return this.EntityPM.InvoiceCurrencyId; }
    set InvoiceCurrencyId(newValue: string) {
        if (this.EntityPM.InvoiceCurrencyId != newValue) {
            this.EntityPM.InvoiceCurrencyId = newValue;
            this.SetUIProperties_ExchangeRate();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.InvoiceCurrencyCode = null;
                this.SetGridColumns();
                this.SetCurrencyRateData();
            }

            else {
                this.myCurrencyListService.getSingleFromCache(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CurrencyList = myResponse.Result;
                        if (list != null) {
                            this.InvoiceCurrencyCode = list.Code;
                            this.SetCurrencyRateData();
                            this.SetGridColumns();
                        }
                    }
                });
            }
        }
    }

    get InvoiceCurrencyCode() { return this.EntityPM.InvoiceCurrencyCode; }
    set InvoiceCurrencyCode(value: string) {
        if (this.EntityPM.InvoiceCurrencyCode != value) {
            this.EntityPM.InvoiceCurrencyCode = value;
        }
    }

    get InvoiceCurrencyExchangeRate() { return this.EntityPM.InvoiceCurrencyExchangeRate; }
    set InvoiceCurrencyExchangeRate(value: number) {
        var setValue: number = AppTool.Round(value, 5);
        if (this.EntityPM.InvoiceCurrencyExchangeRate != setValue) {
            this.EntityPM.InvoiceCurrencyExchangeRate = setValue;

            this.ItemsSource.forEach(item => {
                item.OnInvoiceExchangeRateChanged();
            });

            this.SetGridColumnsWidth();
        }
    }

    get ExchangeRateDate() { return this.EntityPM.ExchangeRateDate; }
    set ExchangeRateDate(value: Date) {
        if (this.EntityPM.ExchangeRateDate != value) {
            this.EntityPM.ExchangeRateDate = value;
            this.ComputeRelativeRateDate();
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
        this.RelativeRateDate = DateTool.GetRelativeRateDate(this.InvoiceDate, this.ExchangeRateDate, "old");
    }

    get ProfitCurrencyId() { return this.EntityPM.ProfitCurrencyId; }
    set ProfitCurrencyId(newValue: string) {
        if (this.EntityPM.ProfitCurrencyId != newValue) {
            this.EntityPM.ProfitCurrencyId = newValue;
        }
    }

    get ProfitCurrencyExchangeRate() { return this.EntityPM.ProfitCurrencyExchangeRate; }
    set ProfitCurrencyExchangeRate(newValue: number) {
        if (this.EntityPM.ProfitCurrencyExchangeRate != newValue) {
            this.EntityPM.ProfitCurrencyExchangeRate = AppTool.Round(newValue, 5);
        }
    }

    get SATPaymentMethodCode() { return this.EntityPM.SATPaymentMethodCode; }
    set SATPaymentMethodCode(newValue: string) {
        if (this.EntityPM.SATPaymentMethodCode != newValue) {
            this.EntityPM.SATPaymentMethodCode = newValue;
        }
    }

    UpdateRateFromLine(line: ARInvoiceLineItem) {
        if (this.InvoiceCurrencyId == line.ForiegnCurrencyId) {

            var myValue = AppTool.Round(line.ForiegnExchangeRate, 5);

            if (this.InvoiceCurrencyExchangeRate != myValue) {
                this.EntityPM.InvoiceCurrencyExchangeRate = myValue;

                this.ItemsSource.forEach(item => {
                    if (item != line) {
                        item.OnInvoiceExchangeRateChanged();
                    }
                });
            }
        }
    }

    // Properties
    get VatNumber() { return this.EntityPM.VatNumber; }
    set VatNumber(newValue: string) {
        if (this.EntityPM.VatNumber != newValue) {
            this.EntityPM.VatNumber = newValue;
            this.SetUIProperties_VatNumber();
        }
    }

    get PaymentTermId() { return this.EntityPM.PaymentTermId; }
    set PaymentTermId(newValue: string) {
        if (this.EntityPM.PaymentTermId != newValue) {
            this.EntityPM.PaymentTermId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.PaymentTermName = null;
            }

            else {
                this.myPaymentTermListService.getSingleFromCache(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PaymentTermList = myResponse.Result;
                        if (list != null) {
                            this.PaymentTermName = list.EnglishName;
                        }
                    }
                });
            }

            InvoiceTool.ComputeARInvoiceDueDate(this.EntityPM);
        }
    }

    get PaymentTermName() { return this.EntityPM.PaymentTermName; }
    set PaymentTermName(newValue: string) {
        if (this.EntityPM.PaymentTermName != newValue) {
            this.EntityPM.PaymentTermName = newValue;
        }
    }

    get InvoiceDate() { return this.EntityPM.InvoiceDate; }
    set InvoiceDate(newValue: Date) {
        if (this.EntityPM.InvoiceDate != newValue) {
            this.EntityPM.InvoiceDate = newValue;

            InvoiceTool.ComputeARInvoiceDueDate(this.EntityPM);
            this.ComputeRelativeRateDate();
            this.UpdateData();
        }
    }

    get DueDate() { return this.EntityPM.DueDate; }
    set DueDate(newValue: Date) {
        if (this.EntityPM.DueDate != newValue) {
            this.EntityPM.DueDate = newValue;
            InvoiceTool.ComputeARInvoicePaymentTerm(this.EntityPM);
        }
    }

    get DraftNumber() { return this.EntityPM.DraftNumber; }
    set DraftNumber(newValue: string) {
        if (this.EntityPM.DraftNumber != newValue) {
            this.EntityPM.DraftNumber = newValue;
        }
    }

    get StatusCode() { return this.EntityPM.StatusCode; }
    set StatusCode(newValue: string) {
        if (this.EntityPM.StatusCode != newValue) {
            this.EntityPM.StatusCode = newValue;
        }
    }

    get PrintNotes() { return this.EntityPM.PrintNotes; }
    set PrintNotes(newValue: string) {
        if (this.EntityPM.PrintNotes != newValue) {
            this.EntityPM.PrintNotes = newValue;
        }
    }

    get ARInvoiceStockId() { return this.EntityPM.ARInvoiceStockId; }
    set ARInvoiceStockId(newValue: string) {
        if (this.EntityPM.ARInvoiceStockId != newValue) {
            this.EntityPM.ARInvoiceStockId = newValue;
        }
    }

    // VAT Type Filter
    private vatTypeId: string;
    get VatTypeId() { return this.vatTypeId; }
    set VatTypeId(newValue: string) {
        if (this.vatTypeId != newValue) {
            this.vatTypeId = newValue;
            this.SetUIProperties_VatTypeFilter();
        }
    }
    VatTypeFilterClicked() {
        if (!AppTool.IsNullOrEmpty(this.VatTypeId)) {
            var vatType = this.VatTypeId;

            var loadingDate = this.EntityPM.InvoiceDate;
            if (loadingDate == null) {
                loadingDate = DateTool.GetCurrentDateAsUtc();
            }

            this.VatTypeId = null;

            this.myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.VatTypePercentagesList = myResponse.Result;

                    this.ItemsSource.forEach(item => {
                        item.EntityPM.VatTypeId = vatType;
                        item.GetVatTypeData();
                    });
                    
                    this.SetGridColumnsWidth();
                    this.ComputeTotals();
                }
            });
        }
    }

    // Filter Invoice lines
    public get PrepaidCollectId() { return this.EntityPM.PrepaidCollectId; }
    public set PrepaidCollectId(value: string) {
        if (this.EntityPM.PrepaidCollectId != value) {
            this.EntityPM.PrepaidCollectId = value;
            this.FilterInvoiceLines();
        }
    }

    public get IsCustomsChargesOnly() { return this.EntityPM.IsCustomsChargesOnly; }
    public set IsCustomsChargesOnly(value: boolean) {
        if (this.EntityPM.IsCustomsChargesOnly != value) {
            this.EntityPM.IsCustomsChargesOnly = value;
            this.FilterInvoiceLines();
        }
    }

    FilterInvoiceLines() {
        this.ItemsSource.forEach(item => {
            var isLineMatched: boolean = true;

            if (this.IsCustomsInvoice) {
                if (this.IsCustomsChargesOnly == true) {
                    if (item.IsCustomsCharge == false) {
                        isLineMatched = false;
                    }
                }
            }

            if (isLineMatched) {
                switch (this.PrepaidCollectId) {
                    case "B":
                        {

                            break;
                        }

                    default: {
                        if (item.PrepaidCollectId != this.PrepaidCollectId) {
                            isLineMatched = false;
                        }

                        break;
                    }
                }
            }

            if (isLineMatched) {
                if (this.EntityPM.InvoiceLines.indexOf(item.EntityPM) == -1) {
                    this.EntityPM.AddARInvoiceLinePM(item.EntityPM);
                }
            }

            else {
                if (this.EntityPM.InvoiceLines.indexOf(item.EntityPM) > -1) {
                    this.EntityPM.RemoveARInvoiceLinePM(item.EntityPM);
                }  
            }

            item.RefreshLine();
        });

        this.ComputeTotals();
    }

    // Load Date 
    private LastRatesList: LastRate[] = [];
    private VatTypePercentagesList: VatTypePercentagePM[] = [];
    private myCurrencyRatesService: CurrencyRatesService;
    LoadData() {

        this.CurrentSession.StartBusyIndicatorLoading();

        if (this.myCurrencyRatesService == null) {
            this.myCurrencyRatesService = new CurrencyRatesService();
        }

        var loadingDate = this.EntityPM.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

        this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator.AccountingCurrencyId, loadingDate).subscribe((myResponse1: ServiceResponse) => {
            if (myResponse1.HasError) {
                this.CurrentSession.StopBusyIndicator();
            }

            else {
                this.LastRatesList = myResponse1.Result;               

                this.myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe((myResponse2: ServiceResponse) => {
                    if (myResponse2.HasError) {
                        this.CurrentSession.StopBusyIndicator();
                    }

                    else {
                        this.VatTypePercentagesList = myResponse2.Result;
                        this.BuildInvoiceLines();

                        this.CurrentSession.StopBusyIndicator();
                        this.CheckNotifyPastDateOnInvoiceEdit();
                    }
                });
            }            
        });
    }
    UpdateData() {
        this.CurrentSession.StartBusyIndicatorLoading();

        if (this.myCurrencyRatesService == null) {
            this.myCurrencyRatesService = new CurrencyRatesService();
        }

        var loadingDate = this.EntityPM.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

        this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator.AccountingCurrencyId, loadingDate).subscribe((myResponse: ServiceResponse) => {

            if (!myResponse.HasError) {
                this.LastRatesList = myResponse.Result;

                this.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
                this.SetCurrencyRateData();

                this.myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe((myResponse2: ServiceResponse) => {
                    if (!myResponse2.HasError) {
                        this.VatTypePercentagesList = myResponse2.Result;
                    }

                    this.ItemsSource.forEach(item => {
                        item.SetVatPercentage(this.GetVatTypePercentage(item.VatTypeId));
                    });

                    this.CurrentSession.StopBusyIndicator();
                });
            }

            else {
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
    SetCurrencyRateData() {
        var myRate: number = null;
        var myRateDate: Date = null;

        if (!AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            if (this.InvoiceCurrencyId == SessionLocator.TenantPM.CurrencyId) {
                myRate = 1;
            }

            else {
                var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == this.InvoiceCurrencyId)[0];
                if (lastRate != null) {
                    myRate = lastRate.Rate;
                    myRateDate = lastRate.ValueDate;
                }
            }
        }

        this.InvoiceCurrencyExchangeRate = myRate;
        this.ExchangeRateDate = myRateDate;
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
    GetCurrencyRateDate(currencyId: string) {
        var myResult: Date = null;

        if (!AppTool.IsNullOrEmpty(currencyId)) {
            if (currencyId == SessionLocator.TenantPM.CurrencyId) {
                myResult = null;
            }

            else {
                var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == currencyId)[0];
                if (lastRate != null) {
                    myResult = lastRate.ValueDate;
                }
            }
        }

        return myResult;
    }
    GetVatTypePercentage(vatTypeId: string) {
        var myResult: number = null;

        var vatTypePercentagePM = this.VatTypePercentagesList.filter(d => d.VatTypeId == vatTypeId)[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
        }

        return myResult;
    }
    UpdateCurrencyRateClicked() {

        var loadingDate = this.EntityPM.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 350;
        logWindow.Title = "Update Currency Rate";
        logWindow.WindowArgs = { CurrencyId: this.InvoiceCurrencyId, CurrencyCode: this.InvoiceCurrencyCode, Rate: this.InvoiceCurrencyExchangeRate, Date: loadingDate };
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.LastRatesList = comp.RatesList;
                    this.InvoiceCurrencyExchangeRate = comp.Rate;
                    this.ExchangeRateDate = comp.RateDate;
                }
            });
        });
        logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
    }
    CheckNotifyPastDateOnInvoiceEdit() {
        if (this.IsEditingEnabled) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                if (SessionLocator.AccountingSettingPM.NotifyPastDateOnInvoiceEdit) {
                    if (DateTool.GetDateParts(this.EntityPM.InvoiceDate).DateTicks != DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateTicks) {
                        var confirmWindow = new ConfirmWindow();

                        var message = TextCodeTranslator.Translate("ARInvoice.M.UpdateInvoiceDate");
                        if (message.indexOf("%Date") > -1) {
                            message = message.replace("%Date", DateTool.GetDateFormats(this.InvoiceDate).ShortDateString);
                        }

                        confirmWindow.Show(message);
                        confirmWindow.WindowClosed.subscribe((event: any) => {
                            if (confirmWindow.Yes) {
                                this.InvoiceDate = DateTool.GetCurrentDateAsUtc();
                            }
                        });
                    }
                }
            }
        }
    }

    BuildScreenData() {
        if (this.IsEditingEnabled) {
            this.LoadData();
        }

        else {
            this.BuildInvoiceLines();
        }
    }
    BuildInvoiceLines() {

        this.ItemsSource = [];
        this.ObservableItems.Clear();

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {

            var tempList: ARInvoiceLinePM[] = [];

            if (this.EntityPM.ARInvoiceTypeCode != "CD" && this.EntityPM.ARInvoiceTypeCode != "CC") {               
                this.EntityPM.InvoiceLines.filter(f => f.UnitPrice < 0).forEach(item => {
                    tempList.push(item);
                    this.EntityPM.RemoveARInvoiceLinePM(item);
                });                
            }

            if (this.EntityPM.PrepaidCollectId != "B") {
                this.EntityPM.InvoiceLines.filter(f => f.PrepaidCollectId != this.EntityPM.PrepaidCollectId).forEach(item => {
                    tempList.push(item);
                    this.EntityPM.RemoveARInvoiceLinePM(item);
                }); 
            }

            if (this.EntityPM.ARInvoiceTypeCode == "CI" || this.EntityPM.ARInvoiceTypeCode == "CC") {
                if (this.EntityPM.IsCustomsChargesOnly) {
                    this.EntityPM.InvoiceLines.filter(f => f.IsCustomsCharge == false).forEach(item => {
                        tempList.push(item);
                        this.EntityPM.RemoveARInvoiceLinePM(item);
                    });
                }
            }

            this.EntityPM.InvoiceLines.forEach(item => {
                this.ItemsSource.push(new ARInvoiceLineItem(item, this));
            });

            tempList.forEach(item => {
                this.ItemsSource.push(new ARInvoiceLineItem(item, this));
            });

            this.ItemsSource.forEach(item => {
                this.ObservableItems.Insert(item);
            });

            this.ComputeTotals();
        }

        else {
            if (this.EntityPM != null && this.EntityPM.InvoiceLines != null) {
                this.EntityPM.InvoiceLines.forEach(line => {
                    this.ItemsSource.push(new ARInvoiceLineItem(line, this));
                });

                this.ItemsSource.forEach(item => {
                    this.ObservableItems.Insert(item);
                });
            }

            if (!this.EntityPM.IsAutoCredit) {
                if (this.IsEditingEnabled) {
                    this.LoadEntityOpenReceivables();
                }
            }

            this.BuildSummary();
        }

        this.SetGridColumnsWidth();

    }
    LoadEntityOpenReceivables() {

        this.CurrentSession.StartBusyIndicatorLoading();

        var myService = new ShipmentDomainService();
        myService.GetInvoiceOpenAmountReceivables(this.EntityPM.ARInvoiceTypeCode, this.EntityPM.MainEntityId).subscribe((myResponse: ServiceResponse) => {

            if (!myResponse.HasError) {
                var allReceivables: ShipmentReceivablePM[] = myResponse.Result.filter(d => d.ShipmentReceivableParentId == null);
                var openReceivables = allReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "OAMT" && !AppTool.IsNullOrEmpty(f.UnitPrice) && !AppTool.IsNullOrEmpty(f.Quantity));

                if (this.EntityPM.ARInvoiceTypeCode == "CD") {
                    if (!SessionLocator.AccountingSettingPM.AllowPositiveAmountsInTheCreditNote) {
                        openReceivables = openReceivables.filter(f => f.UnitPrice < 0 || (f.ChargesTypeCode == "ISTOR" && f.MeasurementCode == "STFE" && f.TotalAmount < 0));
                    }
                }

                else {
                    if (!SessionLocator.AccountingSettingPM.AllowMinusInvoicelines) {
                        openReceivables = openReceivables.filter(f => f.UnitPrice > 0 || f.ChargesTypeCode == "ISTOR" && f.MeasurementCode == "STFE" && f.TotalAmount > 0);
                    }
                }

                if (openReceivables.length > 0) {

                    this.GetAllVatTypes();

                    openReceivables.forEach(receivable => {
                        var invoiceLine: ARInvoiceLinePM = new ARInvoiceLinePM(null);
                        invoiceLine.Tenant = receivable.Tenant;
                        invoiceLine.ChargesTypeId = receivable.ChargesTypeId;
                        invoiceLine.ForiegnCurrencyId = receivable.CurrencyId;
                        invoiceLine.ForiegnCurrencyCode = receivable.CurrencyCode;
                        invoiceLine.ReceivableId = receivable.Id;
                        invoiceLine.MeasurementId = receivable.MeasurementId;
                        invoiceLine.MeasurementCode = receivable.MeasurementCode;
                        invoiceLine.PrepaidCollectId = receivable.PrepaidCollectId;
                        invoiceLine.IsExchangeRateFixed = receivable.IsExchangeRateFixed;
                        invoiceLine.EntityId = receivable.ShipmentId;
                        invoiceLine.EntityReference = receivable.ShipmentNumber;
                        invoiceLine.Quantity = AppTool.Round(receivable.Quantity, 3);
                        invoiceLine.UnitPrice = AppTool.Round(receivable.UnitPrice, 3);
                        invoiceLine.ForiegnCurrencyAmount = AppTool.Round(receivable.TotalAmount, 2);
                        //invoiceLine.IsBackToBack = receivable.IsBackToBack;
                        invoiceLine.IsExpense = receivable.IsExpense;
                        invoiceLine.Notes = receivable.Notes;

                        var itemExchangeRate: number = null;

                        if (invoiceLine.IsExchangeRateFixed) {
                            itemExchangeRate = receivable.Rate;
                        }

                        else if (invoiceLine.ForiegnCurrencyId == this.InvoiceCurrencyId) {
                            itemExchangeRate = this.InvoiceCurrencyExchangeRate;
                        }

                        else {
                            itemExchangeRate = this.GetCurrencyRate(invoiceLine.ForiegnCurrencyId);
                        }

                        var itemExchangeRateRounded: number = AppTool.Round(itemExchangeRate, 5);
                        invoiceLine.ForiegnExchangeRate = itemExchangeRateRounded;
                        invoiceLine.ExchangeRateDate = this.GetCurrencyRateDate(invoiceLine.ForiegnCurrencyId);

                        // Local Amount
                        if (SessionLocator.LocalCurrencyId == invoiceLine.ForiegnCurrencyId) {
                            invoiceLine.LocalCurrencyAmount = invoiceLine.ForiegnCurrencyAmount;
                        }
                        else {
                            invoiceLine.LocalCurrencyAmount = AppTool.Round((invoiceLine.ForiegnCurrencyAmount * invoiceLine.ForiegnExchangeRate), 2);
                        }

                        // Profit Amount
                        if (this.EntityPM.ProfitCurrencyId == invoiceLine.ForiegnCurrencyId) {
                            invoiceLine.ProfitCurrencyAmount = invoiceLine.ForiegnCurrencyAmount;
                        }
                        else if (this.EntityPM.ProfitCurrencyId == SessionLocator.LocalCurrencyId) {
                            invoiceLine.ProfitCurrencyAmount = invoiceLine.LocalCurrencyAmount;
                        }
                        else {
                            invoiceLine.ProfitCurrencyAmount = AppTool.Round((invoiceLine.LocalCurrencyAmount / this.EntityPM.ProfitCurrencyExchangeRate), 2);
                        }

                        // Invoice Amount
                        if (this.EntityPM.InvoiceCurrencyId == invoiceLine.ForiegnCurrencyId) {
                            invoiceLine.InvoiceCurrencyAmount = invoiceLine.ForiegnCurrencyAmount;
                        }
                        else if (this.EntityPM.InvoiceCurrencyId == SessionLocator.LocalCurrencyId) {
                            invoiceLine.InvoiceCurrencyAmount = invoiceLine.LocalCurrencyAmount;
                        }
                        else if (this.EntityPM.InvoiceCurrencyId == this.EntityPM.ProfitCurrencyId) {
                            invoiceLine.InvoiceCurrencyAmount = invoiceLine.ProfitCurrencyAmount;
                        }
                        else {
                            invoiceLine.InvoiceCurrencyAmount = AppTool.Round((invoiceLine.LocalCurrencyAmount / this.EntityPM.InvoiceCurrencyExchangeRate), 2)
                        }

                        // VAT
                        invoiceLine.VatTypeId = receivable.VatTypeId;

                        this.myChargesTypeListService.getSingleFromCache(receivable.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                var list: ChargesTypeList = myResponse.Result;

                                if (list != null) {
                                    invoiceLine.Description = list.EnglishName;
                                    invoiceLine.LocalDescription = list.LocalName;
                                    invoiceLine.IsCustomsCharge = list.IsCustoms;

                                    if (AppTool.IsNullOrEmpty(invoiceLine.VatTypeId)) {
                                        invoiceLine.VatTypeId = list.VatTypeId;
                                    }

                                    if (this.RegionalTaxId) {
                                        if (invoiceLine.VatIsMultiPercentage == false) {
                                            invoiceLine.IsRegionalTax = list.ApplyRegionalTax;
                                        }
                                    }
                                }
                            }
                        });

                        if (!AppTool.IsNullOrEmpty(invoiceLine.VatTypeId)) {
                            var list_VAT: VatTypeList = this.AllVatTypes.filter(f => f.Id == invoiceLine.VatTypeId)[0];
                            if (list_VAT) {
                                invoiceLine.VatTypeName = list_VAT.EnglishName;
                                invoiceLine.VatIsMultiPercentage = list_VAT.IsMultiPercentage;

                                if (!list_VAT.IsMultiPercentage) {
                                    invoiceLine.VatPercentage = this.GetVatTypePercentage(invoiceLine.VatTypeId);
                                }
                            }
                        }

                        var newItemClass = new ARInvoiceLineItem(invoiceLine, this);

                        this.ItemsSource.push(newItemClass);
                        this.ObservableItems.Insert(newItemClass);
                    });
                }
            }

            this.SetGridColumnsWidth();
            this.CurrentSession.StopBusyIndicator();
        });
    }

    // Totals
    get IsCurrencyFilterVisible() {
        var myResult = false;

        if (!AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            if (this.LocalCurrencyId != this.InvoiceCurrencyId) {
                myResult = true;
            }
        }

        return myResult;
    }

    private isTotalInLocalCurrency: boolean = false;
    get IsTotalInLocalCurrency() { return this.isTotalInLocalCurrency; }
    set IsTotalInLocalCurrency(value: boolean) {
        if (this.isTotalInLocalCurrency != value) {
            this.isTotalInLocalCurrency = value;
            this.BuildSummary();
        }
    }

    public SummaryItems: SummaryItem[] = [];
    ComputeTotals() {
        this.BuildTotalVATs();

        this.SubTotalInLocalCurrency = AppTool.Round(ArrayTool.Sum(this.EntityPM.InvoiceLines, "LocalCurrencyAmount"), 2);
        this.SubTotalInInvoiceCurrency = AppTool.Round(ArrayTool.Sum(this.EntityPM.InvoiceLines, "InvoiceCurrencyAmount"), 2);
        this.AmountInLocalCurrency = AppTool.Round(this.EntityPM.SubTotalInLocalCurrency + ArrayTool.Sum(this.EntityPM.TotalVATs, "LocalVATAmount"), 2);
        this.AmountInInvoiceCurrency = AppTool.Round(this.EntityPM.SubTotalInInvoiceCurrency + ArrayTool.Sum(this.EntityPM.TotalVATs, "InvoiceCurrencyVATAmount"), 2);

        if (this.EntityPM.ProfitCurrencyId == SessionLocator.LocalCurrencyId) {
            this.AmountInProfitCurrency = this.EntityPM.AmountInLocalCurrency;
        }

        else if (this.EntityPM.ProfitCurrencyId == this.EntityPM.InvoiceCurrencyId) {
            this.AmountInProfitCurrency = this.EntityPM.AmountInInvoiceCurrency;
        }

        else {
            if (this.EntityPM.ProfitCurrencyExchangeRate != 0) {
                this.AmountInProfitCurrency = AppTool.Round(this.EntityPM.AmountInLocalCurrency / this.EntityPM.ProfitCurrencyExchangeRate, 2);
            }
        }

        this.AmountDue = this.AmountInInvoiceCurrency == null ? 0 : this.AmountInInvoiceCurrency;
        this.AmountDueInLocalCurrency = this.AmountInLocalCurrency == null ? 0 : this.AmountInLocalCurrency;
        this.AmountDueInProfitCurrency = this.AmountInProfitCurrency == null ? 0 : this.AmountInProfitCurrency;

        this.BuildSummary();
    }
    BuildTotalVATs() {
        this.EntityPM.TotalVATs = [];

        var myDataLines: ARInvoiceLinePM[] = this.EntityPM.InvoiceLines.filter(f => f.VatTypeId != null);
        if (myDataLines.length > 0) {

            this.GetAllVatTypes();

            // Build Totals Class
            var group_Source: InvoiceTotalsClass[] = [];
            myDataLines.forEach(item => {
                var lineVatType = this.AllVatTypes.filter(f => f.Id == item.VatTypeId)[0];

                if (lineVatType) {

                    if (AppTool.IsNullOrEmpty(item.LocalCurrencyAmount)) {
                        item.LocalCurrencyAmount = 0;
                    }

                    if (AppTool.IsNullOrEmpty(item.InvoiceCurrencyAmount)) {
                        item.InvoiceCurrencyAmount = 0;
                    }

                    if (AppTool.IsNullOrEmpty(item.ProfitCurrencyAmount)) {
                        item.ProfitCurrencyAmount = 0;
                    }

                    if (!lineVatType.IsMultiPercentage) {
                        var myQroupItem = new InvoiceTotalsClass();
                        myQroupItem.Id = lineVatType.Id;
                        myQroupItem.VatTypeId = lineVatType.Id;
                        myQroupItem.VatTypePercentage = item.VatPercentage;
                        myQroupItem.LocalCurrencyAmount = item.LocalCurrencyAmount;
                        myQroupItem.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
                        myQroupItem.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
                        myQroupItem.ExternalVatCard = SessionLocator.AccountingSettingPM.ReceivableVATCard;
                        myQroupItem.ExternalTAXItemId = lineVatType.ExternalTAXItemId;                        

                        if (item.IsRegionalTax) {

                            myQroupItem.LocalCurrencyAmount = item.LocalCurrencyAmount + item.LocalCurrencyAmount * (this.RegionalTaxPercentage / 100);
                            myQroupItem.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount + item.InvoiceCurrencyAmount * (this.RegionalTaxPercentage / 100);
                            myQroupItem.ProfitCurrencyAmount = item.ProfitCurrencyAmount + item.ProfitCurrencyAmount * (this.RegionalTaxPercentage / 100);

                            var regionalTaxItem = new InvoiceTotalsClass();
                            regionalTaxItem.Id = this.RegionalTaxId;
                            regionalTaxItem.VatTypeId = this.RegionalTaxId;
                            regionalTaxItem.VatTypePercentage = this.RegionalTaxPercentage;
                            regionalTaxItem.LocalCurrencyAmount = item.LocalCurrencyAmount;
                            regionalTaxItem.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
                            regionalTaxItem.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
                            regionalTaxItem.ExternalVatCard = SessionLocator.AccountingSettingPM.ReceivableVATCard;
                            regionalTaxItem.ExternalTAXItemId = lineVatType.ExternalTAXItemId;
                            group_Source.push(regionalTaxItem);
                        }

                        group_Source.push(myQroupItem);
                    }

                    else {
                        var myVatGroups = SessionLocator.AllVatTypesGroups.filter(f => f.GroupVATTypeId == item.VatTypeId);

                        myVatGroups.forEach(itemGroup => {
                            var myQroupItem = new InvoiceTotalsClass();
                            myQroupItem.Id = itemGroup.SingleVATTypeId;
                            myQroupItem.VatTypeId = itemGroup.SingleVATTypeId;
                            myQroupItem.LocalCurrencyAmount = item.LocalCurrencyAmount;
                            myQroupItem.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
                            myQroupItem.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
                            myQroupItem.ExternalVatCard = SessionLocator.AccountingSettingPM.ReceivableVATCard;

                            var vatType = this.AllVatTypes.filter(f => f.Id == itemGroup.SingleVATTypeId)[0];
                            if (vatType) {
                                myQroupItem.ExternalTAXItemId = vatType.ExternalTAXItemId;
                                myQroupItem.VatTypePercentage = this.GetVatTypePercentage(vatType.Id);
                            }

                            group_Source.push(myQroupItem);
                        });
                    }
                }
            });

            // Group Totals Class
            var group_data: InvoiceTotalsClass[] = [];
            group_Source.forEach(item => {
                var record: InvoiceTotalsClass = group_data.filter(f => f.VatTypeId == item.VatTypeId && f.VatTypePercentage == item.VatTypePercentage && f.ExternalVatCard == item.ExternalVatCard && f.ExternalTAXItemId == item.ExternalTAXItemId)[0];
                if (record) {
                    record.LocalCurrencyAmount += item.LocalCurrencyAmount;
                    record.InvoiceCurrencyAmount += item.InvoiceCurrencyAmount;
                    record.ProfitCurrencyAmount += item.ProfitCurrencyAmount;
                }

                else {
                    record = new InvoiceTotalsClass();
                    record.Id = item.Id;
                    record.VatTypeId = item.VatTypeId;
                    record.VatTypePercentage = item.VatTypePercentage;
                    record.ExternalVatCard = item.ExternalVatCard;
                    record.ExternalTAXItemId = item.ExternalTAXItemId;
                    record.LocalCurrencyAmount = item.LocalCurrencyAmount;
                    record.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
                    record.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
                    group_data.push(record);
                }
            });

            // Build Invoice Total VATs
            group_data.forEach(item => {

                var itemVatType = this.AllVatTypes.filter(f => f.Id == item.VatTypeId)[0];

                var itemTotalVAT = new ARInvoiceTotalVATPM(null);
                itemTotalVAT.Tenant = SessionLocator.Tenant;
                itemTotalVAT.ARInvoiceId = this.EntityPM.Id;
                itemTotalVAT.VatTypeId = item.VatTypeId;
                itemTotalVAT.VatTypeName = itemVatType ? itemVatType.EnglishName : "";
                itemTotalVAT.ExternalVATCard = item.ExternalVatCard;
                itemTotalVAT.ExternalTAXItemId = item.ExternalTAXItemId;
                itemTotalVAT.VATPercent = AppTool.Round(item.VatTypePercentage, 3);
                itemTotalVAT.LocalVatableAmount = AppTool.Round(item.LocalCurrencyAmount, 2);
                itemTotalVAT.InvoiceCurrencyVatableAmount = AppTool.Round(item.InvoiceCurrencyAmount, 2);
                itemTotalVAT.ProfitVatableAmount = AppTool.Round(item.ProfitCurrencyAmount, 2);
                itemTotalVAT.LocalVATAmount = AppTool.Round((itemTotalVAT.LocalVatableAmount * itemTotalVAT.VATPercent / 100), 2);
                itemTotalVAT.InvoiceCurrencyVATAmount = AppTool.Round((itemTotalVAT.InvoiceCurrencyVatableAmount * itemTotalVAT.VATPercent / 100), 2);
                itemTotalVAT.ProfitCurrencyVATAmount = AppTool.Round((itemTotalVAT.ProfitVatableAmount * itemTotalVAT.VATPercent / 100), 2);
                itemTotalVAT.VatTypeCell = itemTotalVAT.VatTypeName + " (" + this.NumbersPipe.transform(itemTotalVAT.VATPercent, "N3") + "%)";
                this.EntityPM.AddARInvoiceTotalVATPM(itemTotalVAT);
            });
        }
    }
    BuildSummary() {
        this.SummaryItems = [];
        var selectedCurrencyCode = this.IsTotalInLocalCurrency ? "(" + this.LocalCurrencyCode + ")" : "(" + this.InvoiceCurrencyCode + ")";

        if (this.EntityPM.InvoiceLines.length > 0) {
            var mySummaryItem_Sub = new SummaryItem();
            mySummaryItem_Sub.Label = TextCodeTranslator.Translate("ARInvoice.S.Details.Subtotal");
            mySummaryItem_Sub.Value = this.IsTotalInLocalCurrency ? this.NumbersPipe.transform(this.EntityPM.SubTotalInLocalCurrency, "N2") : this.NumbersPipe.transform(this.EntityPM.SubTotalInInvoiceCurrency, "N2");
            this.SummaryItems.push(mySummaryItem_Sub);

            this.EntityPM.TotalVATs.forEach(item => {
                var myOperatorItem = new SummaryItem();
                myOperatorItem.Value = "+";
                this.SummaryItems.push(myOperatorItem);

                var mySummaryItem = new SummaryItem();
                //mySummaryItem.Label = item.VatTypeCell;
                mySummaryItem.Label = item.VatTypeName + " (" + this.NumbersPipe.transform(item.VATPercent, "N3") + "%)";
                mySummaryItem.Value = this.IsTotalInLocalCurrency ? this.NumbersPipe.transform(item.LocalVATAmount, "N2") : this.NumbersPipe.transform(item.InvoiceCurrencyVATAmount, "N2");
                this.SummaryItems.push(mySummaryItem);
            });

            var myOperatorItem = new SummaryItem();
            myOperatorItem.Value = "=";
            this.SummaryItems.push(myOperatorItem);
        }

        var mySummaryItem_All = new SummaryItem();
        mySummaryItem_All.Label = TextCodeTranslator.Translate("ARInvoice.F.AmountInInvoiceCurrency") + " " + selectedCurrencyCode;
        mySummaryItem_All.Value = this.IsTotalInLocalCurrency ? this.NumbersPipe.transform(this.EntityPM.AmountInLocalCurrency, "N2") : this.NumbersPipe.transform(this.EntityPM.AmountInInvoiceCurrency, "N2");
        this.SummaryItems.push(mySummaryItem_All);
    }

    get SubTotalInLocalCurrency() { return this.EntityPM.SubTotalInLocalCurrency; }
    set SubTotalInLocalCurrency(value: number) {       
        if (this.EntityPM.SubTotalInLocalCurrency != value) {
            this.EntityPM.SubTotalInLocalCurrency = AppTool.Round(value,2);
        }
    }

    get SubTotalInInvoiceCurrency() { return this.EntityPM.SubTotalInInvoiceCurrency; }
    set SubTotalInInvoiceCurrency(value: number) {
        if (this.EntityPM.SubTotalInInvoiceCurrency != value) {
            this.EntityPM.SubTotalInInvoiceCurrency = AppTool.Round(value, 2);
        }
    }

    get AmountInLocalCurrency() { return this.EntityPM.AmountInLocalCurrency; }
    set AmountInLocalCurrency(value: number) {
        if (this.EntityPM.AmountInLocalCurrency != value) {
            this.EntityPM.AmountInLocalCurrency = AppTool.Round(value, 2);
        }
    }

    get AmountInInvoiceCurrency() { return this.EntityPM.AmountInInvoiceCurrency; }
    set AmountInInvoiceCurrency(value: number) {
        if (this.EntityPM.AmountInInvoiceCurrency != value) {
            this.EntityPM.AmountInInvoiceCurrency = AppTool.Round(value, 2);
        }
    }

    get AmountInProfitCurrency() { return this.EntityPM.AmountInProfitCurrency; }
    set AmountInProfitCurrency(value: number) {
        if (this.EntityPM.AmountInProfitCurrency != value) {
            this.EntityPM.AmountInProfitCurrency = AppTool.Round(value, 2);
        }
    }

    get AmountDue() { return this.EntityPM.AmountDue; }
    set AmountDue(value: number) {
        var setValue: number = AppTool.Round(value, 2);
        if (setValue == null) {
            setValue = 0;
        }

        if (this.EntityPM.AmountDue != setValue) {
            this.EntityPM.AmountDue = setValue;
        }
    }

    get AmountDueInLocalCurrency() { return this.EntityPM.AmountDueInLocalCurrency; }
    set AmountDueInLocalCurrency(value: number) {
        var setValue: number = AppTool.Round(value, 2);
        if (setValue == null) {
            setValue = 0;
        }

        if (this.EntityPM.AmountDueInLocalCurrency != setValue) {
            this.EntityPM.AmountDueInLocalCurrency = setValue;
        }
    }

    get AmountDueInProfitCurrency() { return this.EntityPM.AmountDueInProfitCurrency; }
    set AmountDueInProfitCurrency(value: number) {
        var setValue: number = AppTool.Round(value, 2);
        if (setValue == null) {
            setValue = 0;
        }

        if (this.EntityPM.AmountDueInProfitCurrency != setValue) {
            this.EntityPM.AmountDueInProfitCurrency = setValue;
        }
    }

    EditLineClicked(item: ARInvoiceLineItem) {
        if (item != null) {
            var logWindow = new LogitudeWindow();
            logWindow.Title = TextCodeTranslator.Translate("ARInvoiceLine.O.EditInvoiceLine");
            logWindow.DataContext = item;
            logWindow.Show('./InvoiceModules/ARInvoice/Components/EditTabs/AddEditARInvoiceLineComponent');
        }
    }

    // Journal Process
    get JournalNumber() {
        return this.EntityPM.JournalNumber;
    }
    get JournalId() {
        return this.EntityPM.JournalId;
    }
    get IsFullAccounting() {
        var result = false;
        if (SessionLocator.TenantPM.AccountingActivated == true && !AppTool.IsNullOrEmpty(this.JournalNumber)) {
            result = true;
        }
        return result;
    }

    EditJournal() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: this.JournalId,ObjectTableName: 'Journal' });
            });
    }


    //Invoice Number
    get IsInvoiceNumberManuallySet() { return this.EntityPM.IsInvoiceNumberManuallySet; }
    set IsInvoiceNumberManuallySet(value: boolean) {
        if (this.EntityPM.IsInvoiceNumberManuallySet != value) {

            if (!value) {

                this.InvoiceNumber = null;
            }

            else if (this.EntityPM.InvoiceNumber == this.EntityPM.Id) {
                this.InvoiceNumber = null;
            }

            this.EntityPM.IsInvoiceNumberManuallySet = value;
            this.UIProperties.SetEnabled("InvoiceNumber", this.ObjectTableName, value);
        }
    }

    get IsInvoiceNumberFromStock() { return this.EntityPM.IsInvoiceNumberFromStock; }
    set IsInvoiceNumberFromStock(value: boolean) {
        if (this.EntityPM.IsInvoiceNumberFromStock != value) {
            this.EntityPM.IsInvoiceNumberFromStock = value;
        }
    }

    get InvoiceNumber() {
        if (this.IsInvoiceNumberManuallySet || this.IsInvoiceNumberFromStock) {
            return this.EntityPM.InvoiceNumber;
        }

        else if (this.EntityPM.Id == this.EntityPM.InvoiceNumber) {
            return null;
        }

        else {
            return this.EntityPM.InvoiceNumber;
        }
    }
    set InvoiceNumber(value: string) {
        if (this.EntityPM.InvoiceNumber != value) {
            if (this.IsInvoiceNumberManuallySet || (this.IsInvoiceNumberFromStock)) {
                this.EntityPM.InvoiceNumber = value;
            }
        }
    }

    private isInvoiceNumberComboBoxEnabled = true;
    get IsInvoiceNumberComboBoxEnabled() { return this.isInvoiceNumberComboBoxEnabled; }
    set IsInvoiceNumberComboBoxEnabled(value: boolean) {
        if (this.isInvoiceNumberComboBoxEnabled != value) {
            this.isInvoiceNumberComboBoxEnabled = value;
        }
    }

    private selectedInvoiceNumberFilter: CodeNameClass;
    get SelectedInvoiceNumberFilter() { return this.selectedInvoiceNumberFilter; }
    set SelectedInvoiceNumberFilter(value: CodeNameClass) {
        if (this.selectedInvoiceNumberFilter != value) {
            this.selectedInvoiceNumberFilter = value;

            this.EntityPM.InvoiceNumber = null;
            this.EntityPM.IsInvoiceNumberFromStock = false;
            this.EntityPM.IsInvoiceNumberManuallySet = false;

            if (value.Code == "MAS") {
                this.IsInvoiceNumberManuallySet = true;
            }

            else if (value.Code == "STK") {
                this.IsInvoiceNumberFromStock = true;
            }

            this.SetUIProperties_InvoiceNumber();
        }
    }

    IsgetFromStockAfterSaving = false;

    BuildInvoiceNumberFilters() {
        this.InvoiceNumberFilterList = [];
        this.InvoiceNumberFilterList.push(new CodeNameClass("CNR", "Counter"));

        if (SessionLocator.AccountingSettingPM.EnableInvoiceStocksManagement) {
            this.InvoiceNumberFilterList.push(new CodeNameClass("STK", "Stock"));
        }

        if (this.AllowManualInvoiceNumber) {
            this.InvoiceNumberFilterList.push(new CodeNameClass("MAS", "Manually Set"));
        }

        if (this.EntityPM != null && !AppTool.IsNullOrEmpty(this.EntityPM.ARInvoiceStockId)) {
            this.selectedInvoiceNumberFilter = this.InvoiceNumberFilterList.filter(a => a.Code == "STK")[0];
        }

        else if (this.EntityPM != null && this.EntityPM.IsInvoiceNumberFromStock && this.EntityPM.IsAutoCredit) {
            this.selectedInvoiceNumberFilter = this.InvoiceNumberFilterList.filter(a => a.Code == "STK")[0];
        }

        else if (this.EntityPM != null && this.EntityPM.IsInvoiceNumberManuallySet == true) {
            this.selectedInvoiceNumberFilter = this.InvoiceNumberFilterList.filter(a => a.Code == "MAS")[0];
        }

        else {
            this.selectedInvoiceNumberFilter = this.InvoiceNumberFilterList.filter(a => a.Code == "CNR")[0];
        }
    }
    GetInvoiceNumberFromStock() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Select Invoice Number From Stock";
        logWindow.Width = 1200;
        logWindow.Height = 600;      
        logWindow.Show('./InvoiceModules/InvoiceStocks/Components/StockSelection/ARInvoiceStockSelectionComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if ((d != null && d != "cancel")) {
                    if (this.EntityPM.IsAutoCredit) {
                        var myConfirmWindow = new ConfirmWindow();
                        myConfirmWindow.Width = 400;
                        myConfirmWindow.Show(TextCodeTranslator.Translate("ARInvoice.M.ConfirmAutoCredit"));
                        myConfirmWindow.WindowClosed.subscribe(event => {
                            if (myConfirmWindow.Yes) {
                                this.SetStockProperties(s.StockLineSelectedItem, TextCodeTranslator.Translate("ARInvoice.M.CreatingAutoCredit"));
                            }
                        });
                    }

                    else {
                        this.SetStockProperties(s.StockLineSelectedItem);
                    }                    
                }
            });
        });
    }
    ReturnInvoiceNumberToStock() {
        this.ARInvoiceStockId = null;
        this.InvoiceNumber = null;
        this.IsInvoiceNumberComboBoxEnabled = true;
        this.SelectedInvoiceNumberFilter = this.InvoiceNumberFilterList.filter(a => a.Code == "CNR")[0];
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    }

    private SetStockProperties(stockLineSelectedItem: ARInvoiceStockLinePM, msg: string = null) {
        this.IsgetFromStockAfterSaving = true;
        this.InvoiceNumber = stockLineSelectedItem.Number;
        this.ARInvoiceStockId = stockLineSelectedItem.Id;
        this.IsInvoiceNumberComboBoxEnabled = false;
        this.CurrentSession.CurrentEditComponent.SaveChanges(msg);
    }

    // RegionalTaxId
    public IsRegionalTaxVisible: boolean = false;
    private SetRegionalTaxVisibility() {
        var isVisible: boolean = false;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.RegionalTaxId)) {
            isVisible = true;
        }

        else if (FeatureLocator.HasFeaturePermession("General", "REGIONALTAX")) {
            if (SessionLocator.AccountingSettingPM.AllowRegionalTaxManagement) {
                isVisible = true;
            }
        }

        this.IsRegionalTaxVisible = isVisible;
    }

    get RegionalTaxId() { return this.EntityPM.RegionalTaxId; }
    set RegionalTaxId(newValue: string) {

        var oldValue = this.EntityPM.RegionalTaxId;

        if (this.EntityPM.RegionalTaxId != newValue) {
            this.EntityPM.RegionalTaxId = newValue;
            
            if (AppTool.IsNullOrEmpty(newValue)) {
                this.RegionalTaxPercentage = null;
            }

            else {
                this.RegionalTaxPercentage = this.GetVatTypePercentage(newValue);
                this.SetRegionalTaxValuesForLines();
                //if (AppTool.IsNullOrEmpty(oldValue)) {
                //    this.ItemsSource.filter(f => f.IsRegionalTax == false && f.VatIsMultiPercentage == false).forEach(item => {
                //        this.myChargesTypeListService.getSingleFromCache(item.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
                //            if (!myResponse.HasError) {
                //                var list: ChargesTypeList = myResponse.Result;
                //                if (list != null) {
                //                    item.IsRegionalTax = list.ApplyRegionalTax;
                //                }
                //            }
                //        });
                //    });
                //}
            }
        }
    }

    get RegionalTaxPercentage() {

        var output: number = 0;

        if (this.EntityPM.RegionalTaxPercentage) {
            output = this.EntityPM.RegionalTaxPercentage;
        }

        return output;
    }
    set RegionalTaxPercentage(newValue: number) {
        if (this.EntityPM.RegionalTaxPercentage != newValue) {
            this.EntityPM.RegionalTaxPercentage = AppTool.Round(newValue, 2);
            this.ComputeTotals();
        }
    }
}
export class ARInvoiceLineItem extends BaseComponent {
    public EntityPM: ARInvoiceLinePM = null;
    public ObjectTableName = "ARInvoiceLine";
    public DataContext = this;
    public LocalCurrencyId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entityPM: ARInvoiceLinePM, public fatherComponent: ARInvoiceDetailsTabNormal) {
        super();
        this.EntityPM = entityPM;
        this.LocalCurrencyId = fatherComponent.LocalCurrencyId;
        this.ReadIsMatched();
        this.ReadCellBackground();
        this.ReadVatTypeData();
        this.ComputeRelativeRateDate();
        this.SetUIProperties();
    }

    public IsEditingEnabled: boolean = false;
    public IsRateEnabled: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    SetUIProperties() {
        if (FeatureLocator.HasFeaturePermession("ARInvoice", "ARInvoiceEditExchangeRate")) {
            this.IsEditExchangeRateVisible = true;
        }

        var isQuantityEnabled = false;
        var isUnitPriceEnabled = false;

        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;

        if (this.IsEditingEnabled) {
            if (this.MeasurementCode != "STFE") {
                isQuantityEnabled = true;
                isUnitPriceEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("ForiegnCurrencyAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("LocalCurrencyAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("InvoiceCurrencyAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Description", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("LocalDescription", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, isQuantityEnabled);
        this.UIProperties.SetEnabled("UnitPrice", this.ObjectTableName, isUnitPriceEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("IsRegionalTax", this.ObjectTableName, this.IsEditingEnabled);

        this.SetUIProperties_Rate();
        this.SetUIProperties_VAT();
        this.InvoiceCurrencyAmountVisibility();
    }
    SetUIProperties_Rate() {
        var isFieldEnabled = false;

        if (this.IsEditingEnabled) {
            if (FeatureLocator.HasFeaturePermession("ARInvoice", "ARInvoiceEditExchangeRate")) {
                if (this.ForiegnCurrencyId) {
                    if (this.ForiegnCurrencyId != this.LocalCurrencyId) {
                        if (!this.IsExchangeRateFixed) {
                            isFieldEnabled = true;
                        }
                    }
                }
            }
        }
        
        this.IsRateEnabled = isFieldEnabled;
        this.UIProperties.SetEnabled("ForiegnExchangeRate", this.ObjectTableName, isFieldEnabled);
    }
    SetUIProperties_VAT() {
        this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VatPercentage", this.ObjectTableName, this.IsEditingEnabled);

        if (this.VatIsMultiPercentage) {
            this.UIProperties.SetEnabled("VatPercentage", this.ObjectTableName, false);
        }

        var isVatPercentageRequired = false;

        if (AppTool.IsNullOrEmpty(this.VatPercentage)) {
            isVatPercentageRequired = true;

            if (!AppTool.IsNullOrEmpty(this.VatTypeId)) {
                if (this.VatIsMultiPercentage) {
                    isVatPercentageRequired = false;
                }
            }
        }

        this.UIProperties.SetRequired("VatPercentage", this.ObjectTableName, isVatPercentageRequired);
    }


    public IsInvoiceCurrencyAmountVisible: boolean = false;
    private InvoiceCurrencyAmountVisibility() {

        var isVisible: boolean = false;

        if (this.fatherComponent.InvoiceCurrencyId != this.LocalCurrencyId) {
            isVisible = true;
        }


        this.IsInvoiceCurrencyAmountVisible = isVisible;
        this.UIProperties.SetVisibility("InvoiceCurrencyAmount", this.ObjectTableName, isVisible);
    }

    public CellBackground: string;
    public IsMatched: boolean = false;
    public IsMatchedPrepaidCollect: boolean = false;
    RefreshLine() {
        this.ReadIsMatched();
        this.ReadCellBackground();
    }

    ReadIsMatched() {
        var isMatched = false;
        var isMatchedCustomsOnly = true;
        var isMatchedPrepaidCollect = false;

        if (this.fatherComponent.IsCustomsInvoice) {
            if (this.fatherComponent.IsCustomsChargesOnly) {
                if (!this.IsCustomsCharge) {
                    isMatchedCustomsOnly = false;
                }
            }
        }

        if (this.fatherComponent.PrepaidCollectId == "B") {
            isMatchedPrepaidCollect = true;
        }

        else if (this.fatherComponent.PrepaidCollectId == this.PrepaidCollectId) {
            isMatchedPrepaidCollect = true;
        }

        if (isMatchedCustomsOnly && isMatchedPrepaidCollect) {
            isMatched = true;
        }

        this.IsMatched = isMatched;
        this.IsMatchedPrepaidCollect = isMatchedPrepaidCollect;
    }

    ReadCellBackground() {
        var myResult = "transparent";

        if (this.Exists) {
            myResult = "rgba(208, 224, 234, 0.4)";
        }

        this.CellBackground = myResult;
    }

    get Exists() {
        let myResult = false;

        if (this.fatherComponent.EntityPM.InvoiceLines.indexOf(this.EntityPM) > -1) {
            myResult = true;
        }

        return myResult;
    }
    set Exists(newValue: boolean) {

        if (newValue == true) {
            this.fatherComponent.EntityPM.AddARInvoiceLinePM(this.EntityPM);

            this.OnInvoiceExchangeRateChanged();
        }

        else {
            this.fatherComponent.EntityPM.RemoveARInvoiceLinePM(this.EntityPM);
        }

        this.ReadCellBackground();
        this.fatherComponent.ComputeTotals();
    }

    // Properties
    get EntityReference() { return this.EntityPM.EntityReference; }
    set EntityReference(newValue: string) {
        if (this.EntityPM.EntityReference != newValue) {
            this.EntityPM.EntityReference = newValue;
        }
    }

    get ForiegnCurrencyId() { return this.EntityPM.ForiegnCurrencyId; }
    set ForiegnCurrencyId(newValue: string) {
        if (this.EntityPM.ForiegnCurrencyId != newValue) {
            this.EntityPM.ForiegnCurrencyId = newValue;
        }
    }

    get ForiegnCurrencyCode() { return this.EntityPM.ForiegnCurrencyCode; }
    set ForiegnCurrencyCode(newValue: string) {
        if (this.EntityPM.ForiegnCurrencyCode != newValue) {
            this.EntityPM.ForiegnCurrencyCode = newValue;
        }
    }

    get LocalCurrencyCode() { return this.EntityPM.InvoiceLocalCurrencyCode; }
    set LocalCurrencyCode(newValue: string) {
        if (this.EntityPM.InvoiceLocalCurrencyCode != newValue) {
            this.EntityPM.InvoiceLocalCurrencyCode = newValue;
        }
    }

    get InvoiceCurrencyCode() { return this.EntityPM.InvoiceCurrencyCode; }
    set InvoiceCurrencyCode(newValue: string) {
        if (this.EntityPM.InvoiceCurrencyCode != newValue) {
            this.EntityPM.InvoiceCurrencyCode = newValue;
        }
    }

    get PrepaidCollectId() { return this.EntityPM.PrepaidCollectId; }
    set PrepaidCollectId(newValue: string) {
        if (this.EntityPM.PrepaidCollectId != newValue) {
            this.EntityPM.PrepaidCollectId = newValue;
        }
    }

    get MeasurementId() { return this.EntityPM.MeasurementId; }
    set MeasurementId(newValue: string) {
        if (this.EntityPM.MeasurementId != newValue) {
            this.EntityPM.MeasurementId = newValue;
        }
    }

    get MeasurementCode() { return this.EntityPM.MeasurementCode; }
    set MeasurementCode(newValue: string) {
        if (this.EntityPM.MeasurementCode != newValue) {
            this.EntityPM.MeasurementCode = newValue;
        }
    }

    get ChargesTypeId() { return this.EntityPM.ChargesTypeId; }
    set ChargesTypeId(value: string) {
        if (this.EntityPM.ChargesTypeId != value) {
            this.EntityPM.ChargesTypeId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.Description = null;
                this.LocalDescription = null;
                this.IsCustomsCharge = false;
            }

            else {
                this.fatherComponent.myChargesTypeListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: ChargesTypeList = myResponse.Result;
                        if (list != null) {
                            this.Description = list.EnglishName;
                            this.LocalDescription = list.LocalName;
                            this.IsCustomsCharge = list.IsCustoms;
                        }
                    }
                });
            }
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    get LocalDescription() { return this.EntityPM.LocalDescription; }
    set LocalDescription(newValue: string) {
        if (this.EntityPM.LocalDescription != newValue) {
            this.EntityPM.LocalDescription = newValue;
        }
    }

    get IsCustomsCharge() { return this.EntityPM.IsCustomsCharge; }
    set IsCustomsCharge(value: boolean) {
        if (this.EntityPM.IsCustomsCharge != value) {
            this.EntityPM.IsCustomsCharge = value;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newValue: string) {
        if (this.EntityPM.Notes != newValue) {
            this.EntityPM.Notes = newValue;
        }
    }

    get IsExchangeRateFixed() { return this.EntityPM.IsExchangeRateFixed; }
    set IsExchangeRateFixed(newValue: boolean) {
        if (this.EntityPM.IsExchangeRateFixed != newValue) {
            this.EntityPM.IsExchangeRateFixed = newValue;
        }
    }

    UpdateCurrencyRateClicked() {

        var loadingDate = this.fatherComponent.InvoiceDate;        
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 350;
        logWindow.Title = "Update Currency Rate";
        logWindow.WindowArgs = { CurrencyId: this.ForiegnCurrencyId, CurrencyCode: this.ForiegnCurrencyCode, Rate: this.ForiegnExchangeRate, Date: loadingDate };
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.ForiegnExchangeRate = comp.Rate;
                    this.ExchangeRateDate = comp.RateDate;
                }
            });
        });
        logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
    }

    // VAT Type
    get VatTypeId() { return this.EntityPM.VatTypeId; }
    set VatTypeId(newValue: string) {
        if (this.EntityPM.VatTypeId != newValue) {
            this.EntityPM.VatTypeId = newValue;

            this.fatherComponent.ItemsSource.filter(f => f.VatTypeId == newValue).forEach(item => {
                item.GetVatTypeData();
            });
            
            this.GetVatTypeData();
            this.fatherComponent.SetGridColumnsWidth();
        }
    }

    GetVatTypeData() {
        if (AppTool.IsNullOrEmpty(this.VatTypeId)) {
            this.VatTypeName = null;
            this.VatPercentage = null;
            this.VatIsMultiPercentage = false;
            //this.EntityPM.ExternalVATCard = null;
            this.EntityPM.ExternalTAXItemId = null;
            this.ReadVatTypeData();
            this.fatherComponent.ComputeTotals();
            this.SetUIProperties_VAT();
        }

        else {
            this.fatherComponent.myVatTypeListService.getSingleFromCache(this.VatTypeId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: VatTypeList = myResponse.Result;
                    if (list != null) {
                        this.VatTypeName = list.EnglishName;
                        this.VatIsMultiPercentage = list.IsMultiPercentage;
                        //this.EntityPM.ExternalVATCard = list.ExternalVATCard;
                        this.EntityPM.ExternalTAXItemId = list.ExternalTAXItemId;

                        if (list.IsMultiPercentage) {
                            this.VatPercentage = null;
                        }

                        else {
                            this.VatPercentage = this.fatherComponent.GetVatTypePercentage(this.VatTypeId);
                        }

                        this.ReadVatTypeData();
                        this.fatherComponent.ComputeTotals();
                        this.SetUIProperties_VAT();
                    }
                }
            });
        }
    }
    SetVatPercentage(myPercentage: number) {
        this.VatPercentage = myPercentage;
    }

    get VatTypeName() { return this.EntityPM.VatTypeName; }
    set VatTypeName(newValue: string) {
        if (this.EntityPM.VatTypeName != newValue) {
            this.EntityPM.VatTypeName = newValue;
        }
    }

    get VatPercentage() { return this.EntityPM.VatPercentage; }
    set VatPercentage(newValue: number) {
        if (this.EntityPM.VatPercentage != newValue) {
            this.EntityPM.VatPercentage = AppTool.Round(newValue, 3);
            this.ReadVatTypeData();
            this.ReCalculateTotals();
            this.SetUIProperties_VAT();
        }
    }

    get VatIsMultiPercentage() { return this.EntityPM.VatIsMultiPercentage; }
    set VatIsMultiPercentage(value: boolean) {
        if (this.EntityPM.VatIsMultiPercentage != value) {
            this.EntityPM.VatIsMultiPercentage = value;
            this.SetUIProperties_VAT();
        }
    }

    public VatTypeCell: string;
    public VatTypeCellColor: string;
    public VatTypeUpdateIsVisible: boolean = false;
    public VatTypeMultiIconVisible: boolean = false;
    public VatTypesGroups: VATTypesGroupPM[] = [];
    ReadVatTypeData() {
        var myValue: string = null;
        var myColor: string = FontTool.Black;
        var isUpdateVisible = false;
        var isMultiIconVisible = false;
        this.VatTypesGroups = [];

        if (!AppTool.IsNullOrEmpty(this.VatTypeId)) {

            if (this.VatIsMultiPercentage) {
                myValue = this.VatTypeName;
                myColor = FontTool.Black;
                isMultiIconVisible = true;
                this.VatTypesGroups = SessionLocator.AllVatTypesGroups.filter(f => f.GroupVATTypeId == this.VatTypeId);
            }

            else if (this.VatPercentage != null) {
                myValue = this.VatTypeName + " (" + this.fatherComponent.NumbersPipe.transform(this.VatPercentage, "N3") + "%)";
                myColor = FontTool.Black;
            }

            else {
                myValue = TextCodeTranslator.Translate("ARInvoice.S.Details.NoVat");
                myColor = FontTool.Red;
                isUpdateVisible = true;
            }
        }

        this.VatTypeCell = myValue;
        this.VatTypeCellColor = myColor;
        this.VatTypeUpdateIsVisible = isUpdateVisible;
        this.VatTypeMultiIconVisible = isMultiIconVisible;
    }
    UpdateVatPercentageClicked() {
        if (!AppTool.IsNullOrEmpty(this.VatTypeId)) {

            var logWindow = new LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 350;
            logWindow.Title = "Add VAT Type Percentage";
            logWindow.WindowArgs = this.VatTypeId;
            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.fatherComponent.ItemsSource.filter(f => f.VatTypeId == this.VatTypeId).forEach(item => {
                            item.SetVatPercentage(comp.Percentage);
                        });
                    }
                });
            });
            logWindow.Show('./CommonModules/CommonOthers/Components/UpdateVATPercentage/UpdateVATPercentageComponent');
        }
    }

    // Amounts
    get Quantity() { return this.EntityPM.Quantity; }
    set Quantity(newValue: number) {
        if (this.EntityPM.Quantity != newValue) {
            this.EntityPM.Quantity = AppTool.Round(newValue, 3);
            this.ComputeTotal();
        }
    }

    get UnitPrice() { return this.EntityPM.UnitPrice; }
    set UnitPrice(newValue: number) {
        if (this.EntityPM.UnitPrice != newValue) {
            this.EntityPM.UnitPrice = AppTool.Round(newValue, 3);
            this.ComputeTotal();
        }
    }

    get ForiegnCurrencyAmount() { return this.EntityPM.ForiegnCurrencyAmount; }
    set ForiegnCurrencyAmount(newValue: number) {
        if (this.EntityPM.ForiegnCurrencyAmount != newValue) {
            this.EntityPM.ForiegnCurrencyAmount = AppTool.Round(newValue, 2);
        }
    }

    get ForiegnExchangeRate() { return this.EntityPM.ForiegnExchangeRate; }
    set ForiegnExchangeRate(value: number) {
        if (!this.EntityPM.IsExchangeRateFixed) {
            if (this.EntityPM.ForiegnExchangeRate != value) {

                this.EntityPM.ForiegnExchangeRate = AppTool.Round(value, 5);

                if (this.ForiegnCurrencyId == this.fatherComponent.ProfitCurrencyId) {
                    if (this.fatherComponent.ProfitCurrencyExchangeRate != value) {
                        this.fatherComponent.ProfitCurrencyExchangeRate = value;
                    }
                }

                if (this.ForiegnCurrencyId == this.fatherComponent.InvoiceCurrencyId) {
                    this.fatherComponent.UpdateRateFromLine(this);
                    this.ExchangeRateDate = this.fatherComponent.GetCurrencyRateDate(this.ForiegnCurrencyId);
                }

                this.CalculateLocalCurrencyAmount();
            }
        }
    }

    get ExchangeRateDate() { return this.EntityPM.ExchangeRateDate; }
    set ExchangeRateDate(newValue: Date) {
        if (this.EntityPM.ExchangeRateDate != newValue) {
            this.EntityPM.ExchangeRateDate = newValue;
            this.ComputeRelativeRateDate();
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
        this.RelativeRateDate = DateTool.GetRelativeRateDate(this.fatherComponent.InvoiceDate, this.ExchangeRateDate, "old");        
    }

    get LocalCurrencyAmount() { return this.EntityPM.LocalCurrencyAmount; }
    set LocalCurrencyAmount(newValue: number) {
        if (this.EntityPM.LocalCurrencyAmount != newValue) {
            this.EntityPM.LocalCurrencyAmount = AppTool.Round(newValue, 2);
        }
    }

    get ProfitCurrencyAmount() { return this.EntityPM.ProfitCurrencyAmount; }
    set ProfitCurrencyAmount(newValue: number) {
        if (this.EntityPM.ProfitCurrencyAmount != newValue) {
            this.EntityPM.ProfitCurrencyAmount = AppTool.Round(newValue, 2);
        }
    }

    get InvoiceCurrencyAmount() { return this.EntityPM.InvoiceCurrencyAmount; }
    set InvoiceCurrencyAmount(newValue: number) {
        if (this.EntityPM.InvoiceCurrencyAmount != newValue) {
            this.EntityPM.InvoiceCurrencyAmount = AppTool.Round(newValue, 2);
        }
    }

    ReCalculateTotals() {
        if (this.Exists) {
            this.fatherComponent.ComputeTotals();
        }
    }
    ComputeTotal() {
        var foriegnCurrencyAmount: number = null;
        var localCurrencyAmount: number = null;
        var profitCurrencyAmount: number = null;
        var invoiceCurrencyAmount: number = null;

        if (!AppTool.IsNullOrEmpty(this.Quantity) && !AppTool.IsNullOrEmpty(this.UnitPrice)) {

            if (this.MeasurementCode == "PRVL" || this.MeasurementCode == "PRFR") {
                var price = this.UnitPrice / 100;
                foriegnCurrencyAmount = this.Quantity * price;
            }

            else {
                foriegnCurrencyAmount = this.Quantity * this.UnitPrice;
            }

            localCurrencyAmount = foriegnCurrencyAmount * this.ForiegnExchangeRate;
            profitCurrencyAmount = 0;
            invoiceCurrencyAmount = 0;

            if (this.fatherComponent.InvoiceCurrencyId == this.ForiegnCurrencyId) {
                invoiceCurrencyAmount = foriegnCurrencyAmount;
            }

            else if (this.fatherComponent.InvoiceCurrencyExchangeRate > 0) {
                invoiceCurrencyAmount = localCurrencyAmount / this.fatherComponent.InvoiceCurrencyExchangeRate;
            }

            if (this.fatherComponent.ProfitCurrencyId == this.ForiegnCurrencyId) {
                profitCurrencyAmount = foriegnCurrencyAmount;
            }

            else if (this.fatherComponent.ProfitCurrencyExchangeRate > 0) {
                profitCurrencyAmount = localCurrencyAmount / this.fatherComponent.ProfitCurrencyExchangeRate;
            }
        }

        this.ForiegnCurrencyAmount = foriegnCurrencyAmount;
        this.LocalCurrencyAmount = localCurrencyAmount;
        this.ProfitCurrencyAmount = profitCurrencyAmount;
        this.InvoiceCurrencyAmount = invoiceCurrencyAmount;
        this.fatherComponent.ComputeTotals();
    }    
    CalculateLocalCurrencyAmount() {
        var localCurrencyAmount: number = this.ForiegnCurrencyAmount * this.ForiegnExchangeRate;
        var profitCurrencyAmount: number = localCurrencyAmount / this.fatherComponent.ProfitCurrencyExchangeRate;
        var invoiceCurrencyAmount: number = 0;

        if (this.ForiegnCurrencyId == this.fatherComponent.InvoiceCurrencyId) {
            invoiceCurrencyAmount = this.ForiegnCurrencyAmount;
        }

        else if (this.fatherComponent.InvoiceCurrencyExchangeRate > 0) {
            invoiceCurrencyAmount = localCurrencyAmount / this.fatherComponent.InvoiceCurrencyExchangeRate;
        }

        this.LocalCurrencyAmount = localCurrencyAmount;
        this.ProfitCurrencyAmount = profitCurrencyAmount;
        this.InvoiceCurrencyAmount = invoiceCurrencyAmount;
        this.fatherComponent.ComputeTotals();
    }
    OnInvoiceExchangeRateChanged() {
        if (this.ForiegnCurrencyId == this.fatherComponent.InvoiceCurrencyId) {
            this.ForiegnExchangeRate = this.fatherComponent.InvoiceCurrencyExchangeRate;
        }

        else {
            this.ForiegnExchangeRate = this.fatherComponent.GetCurrencyRate(this.ForiegnCurrencyId);
        }

        this.ExchangeRateDate = this.fatherComponent.GetCurrencyRateDate(this.ForiegnCurrencyId);

        this.ComputeRelativeRateDate();
        this.CalculateInvoiceCurrencyAmount();
    }
    CalculateInvoiceCurrencyAmount() {
        if (this.ForiegnCurrencyId == this.fatherComponent.InvoiceCurrencyId) {
            this.InvoiceCurrencyAmount = this.ForiegnCurrencyAmount;
        }

        else {
            if (this.fatherComponent.InvoiceCurrencyExchangeRate > 0) {
                this.InvoiceCurrencyAmount = this.LocalCurrencyAmount / this.fatherComponent.InvoiceCurrencyExchangeRate;
            }

            else {
                this.InvoiceCurrencyAmount = 0;
            }
        }

        this.fatherComponent.ComputeTotals();
    }

    get IsRegionalTax() { return this.EntityPM.IsRegionalTax; }
    set IsRegionalTax(newValue: boolean) {
        if (this.EntityPM.IsRegionalTax != newValue) {
            this.EntityPM.IsRegionalTax = newValue;
            this.ReCalculateTotals();
        }
    }
}
