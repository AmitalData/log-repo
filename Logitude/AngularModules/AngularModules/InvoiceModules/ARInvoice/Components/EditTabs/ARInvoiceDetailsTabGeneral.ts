import { ChangeDetectorRef, Component, OnDestroy } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ARInvoicePM } from '../../../../Invoice/EntityPMs/ARInvoicePM';
import { ARInvoiceLinePM } from '../../../../Invoice/EntityPMs/ARInvoiceLinePM';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DateTool, AppTool } from '../../../../Infrastructure/Tools';
import { CurrencyRatesService, LastRate } from '../../../../Common/Services/CurrencyRatesService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { InvoiceTotalsClass, SummaryItem } from '../../../../Invoice/Args';
import { InvoiceTool, InvoicePartnerType } from '../../../../Invoice/Tools';
import { CardList } from '../../../../Common/EntityLists/CardList';
import { AddressList } from '../../../../Common/EntityLists/AddressList';
import { CurrencyList } from '../../../../Common/EntityLists/CurrencyList';
import { PaymentTermList } from '../../../../Common/EntityLists/PaymentTermList';
import { VatTypeList } from '../../../../Common/EntityLists/VatTypeList';
import { ChargesTypeList } from '../../../../Common/EntityLists/ChargesTypeList';
import { CardListService } from '../../../../Common/Services/StandardLists/CardListService';
import { CurrencyListService } from '../../../../Common/Services/StandardLists/CurrencyListService';
import { PaymentTermListService } from '../../../../Common/Services/StandardLists/PaymentTermListService';
import { VatTypeListService } from '../../../../Common/Services/StandardLists/VatTypeListService';
import { ChargesTypeListService } from '../../../../Common/Services/StandardLists/ChargesTypeListService';
import { CommonDomainService } from '../../../../Common/Services/CommonDomainService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { UpdateCurrencyRateComponent } from '../../../../CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent';
import { VatTypePercentagePM } from '../../../../Common/EntityPMs/VatTypePercentagePM';
import { NumbersPipe } from '../../../../Infrastructure/Pipes/NumbersPipe';
import { ShipmentDomainService } from '../../../../Shipment/Services/ShipmentDomainService';
import { ShipmentReceivablePM } from '../../../../Shipment/EntityPMs/ShipmentReceivablePM';
import { CustomerPM } from '../../../../Common/EntityPMs/CustomerPM';
import { GlobalDomainService } from '../../../../Common/Services/GlobalDomainService';
import { JournalExtendedPMService } from '../../../../Accounting/Services/ExtendedPMs/JournalExtendedPMService';
import { JournalPM } from '../../../../Accounting/EntityPMs/JournalPM';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { GLAccountPMService } from '../../../../Accounting/Services/StandardPMs/GLAccountPMService';
import { GLAccountPM } from '../../../../Accounting/EntityPMs/GLAccountPM';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { ARInvoicePMService } from 'Invoice/Services/StandardPMs/ARInvoicePMService';
declare var window: any;

@Component({

    templateUrl: './ARInvoiceDetailsTabGeneral.html',
})

export class ARInvoiceDetailsTabGeneral extends BaseComponent implements OnDestroy {
    public EntityPM: ARInvoicePM = null;
    public ObjectTableName = "ARInvoice";
    public DataContext = this;
    public ItemsSource: ARInvoiceLineItem[] = [];
    public ObservableItems: ObservableCollection;
    public LocalCurrencyCode: string;
    public IsManifest: boolean = false;
    public IsCustomsInvoice: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    public IsDatesFieldEnabledWhileCrediting: boolean = false;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public ShowLocal: boolean = false;
    DisplayFieldsFromList: string;
    DisplayLocalFieldsFromList: string;
    BillToLovSizeForFullAccounting: number;
    IsAccountingActivated: boolean = false;
    public AllowVatTypes: boolean = true;
    public IsUsingVirtuallization: boolean = false;
    public InvoicePartners: InvoicePartnerType[] = [];

    public BillToFilter: ApiQueryFilters;
    public PartnerTypeComboBoxIsDisabled: boolean = true;
    public GLAccountsFilterItems: ApiQueryFilters;

    constructor(private entityArgs: EntityArgs, private cdRef: ChangeDetectorRef) {
   


      super();
        // this.CurrentSession.StartBusyIndicatorLoading();
        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = entityArgs.EntityPM;
        this.InitLOVFilters();
        this.BuildPartnersTypes();
        this.IsManifest = this.EntityPM.ARInvoiceTypeCode == "MN" ? true : false;
        this.IsCustomsInvoice = (this.EntityPM.ARInvoiceTypeCode == "CI" || this.EntityPM.ARInvoiceTypeCode == "CC") ? true : false;
        this.ObservableItems = new ObservableCollection([]);
        this.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
        this.ShowLocal = !SessionLocator.LoggedUserPM.DontShowLocal;

        this.CheckFeatures();
        this.InitializeBillToLov();
        this.InitializeServices();
        this.InitializeComponent();
        this.SetUIProperties();
        this.BuildScreenData();
        this.Listen();

        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }
    }

    get CheckIsFullAccounting() {
        return SessionLocator.TenantPM.AccountingActivated;
    }
    InitLOVFilters() {
        this.GLAccountsFilterItems = new ApiQueryFilters();
        this.GLAccountsFilterItems.addAdditionalFilter("GLAccountId", "null", null, null, "NotEqual", false, false, false, "string");
    }
    BuildPartnersTypes() {
        this.InvoicePartners = InvoiceTool.GetARInvoicePartners(null);
        this.PartnersTypeSelectionMethod(this.InvoicePartners[0]);
    }


    InitLOVBillToFilters() {
        this.BillToFilter = new ApiQueryFilters();
        this.BillToFilter.addAdditionalFilter("ActiveGLAccount", true, null, null, "Equals", true, false, false, "Boolean");
    }

    public SelectedPartnerType: InvoicePartnerType = null;
    public BillToDependencyValue1: string;
    public BillToDependencyValue2: boolean;

    PartnersTypeSelectionMethod(selected: InvoicePartnerType) {
        if (this.SelectedPartnerType != selected) {
            this.SelectedPartnerType = selected;
            
            if (selected) {
                this.EntityPM.BillToPartnerTypeId = selected.PartnerTypeId;
                this.BillToDependencyValue1 = selected.PartnerTypeId;
                this.BillToDependencyValue2 = selected.IsCustomer;
            }

            this.SetUIProperties();
        }
    }

    get BillToPartnerTypeId() { return this.billToPartnerTypeId; }
    set BillToPartnerTypeId(newValue: string) {
        if (this.billToPartnerTypeId != newValue) {
            this.billToPartnerTypeId = newValue;
        }
    }

    private billToPartnerTypeId: string;


    SetIsUsingVirtuallization() {
        var hasGridVirtuallizationToggleFeature = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "EVG")[0]
        if (hasGridVirtuallizationToggleFeature) {
            this.IsUsingVirtuallization = true;
        }
    }

    private InitializeBillToLov() {
        this.InitLOVBillToFilters();
        if (this.IsAccountingActivated) {
            this.DisplayFieldsFromList = "Code,CalculatedEnglishName,CalculatedLocalName,GLAccountDisplayNumber,CountryCode,PartnerTypeName";
            this.DisplayLocalFieldsFromList = "Code,CalculatedLocalName,GLAccountDisplayNumber,CountryCode,PartnerTypeName";
            this.BillToLovSizeForFullAccounting = 550;
        }
    }
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private SessionEvent: any = null;
    myEntityPMService: ARInvoicePMService = new ARInvoicePMService();
    private Listen() {
        if (this.entityArgs.EditComponent != null) {


            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.myEntityPMService.get(this.EntityPM.Id).subscribe(res => {
                        this.CurrentSession.CurrentEditComponent.EntityPM = res.Result;
                        this.EntityPM = res.Result;
                        this.SetUIProperties();
                        this.BuildScreenData();
                    })


                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.BuildScreenData();
                }
            });
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "IsSignedChanged") {
                    this.myEntityPMService.get(this.EntityPM.Id).subscribe(res => {
                        this.CurrentSession.CurrentEditComponent.EntityPM = res.Result;
                        this.EntityPM = res.Result;
                        this.IsSigned = this.EntityPM.IsSigned;
                    })
                }
            });


        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
        AppTool.KillEventEmitter(this.SessionEvent);
    }

    private myCardListService: CardListService;
    public myCurrencyListService: CurrencyListService;
    private myPaymentTermListService: PaymentTermListService;
    public myVatTypeListService: VatTypeListService;
    public myChargesTypeListService: ChargesTypeListService;
    private myCommonDomainService: CommonDomainService;
    private myGLAccountPMService: GLAccountPMService;
    InitializeServices() {
        this.myCardListService = new CardListService();
        this.myCurrencyListService = new CurrencyListService();
        this.myPaymentTermListService = new PaymentTermListService();
        this.myVatTypeListService = new VatTypeListService();
        this.myChargesTypeListService = new ChargesTypeListService();
        this.myCommonDomainService = new CommonDomainService();
        this.myGLAccountPMService = new GLAccountPMService();
    }
    InitializeComponent() {
        this.SetGridColumns();
        this.ComputeRelativeRateDate();
        this.BillToDependencyProperty1 = InvoiceTool.GetBillToPartnerTypes();
    }


    CheckFeatures() {

        var table = window.ObjectTables.filter(d => d.Name === 'ARInvoice')[0];
        var hideVatTypesFeature = FeatureLocator.Features.filter(f => (f.Code == "HideVatTypes") && f.ObjectTableId == table.Id)[0];
        if (hideVatTypesFeature) {
            this.AllowVatTypes = false;
        }
    }

    public LocalAmountHeader: string = null;
    public InvoiceAmountHeader: string = null;
    public IsInvoiceAmountHeaderVisible: boolean = false;
    public VATColumnWidth: number = 100;
    public RateColumnWidth: number = 100;
    SetGridColumns() {
        this.LocalAmountHeader = TextCodeTranslator.Translate("ARInvoiceLine.CH.LocalCurrencyAmountListLable").replace("%LocalCurrencyCode", SessionLocator.LocalCurrencyCode);

        var invoiceAmountHeader = null;
        var isInvoiceAmountHeaderVisible = false;
        if (!AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            if (this.InvoiceCurrencyId != SessionLocator.LocalCurrencyId) {
                isInvoiceAmountHeaderVisible = true;
                invoiceAmountHeader = TextCodeTranslator.Translate("ARInvoiceLine.CH.AmountListLable").replace("%InvoiceCurrencyCode", this.InvoiceCurrencyCode);
            }
        }

        this.InvoiceAmountHeader = invoiceAmountHeader;
        this.IsInvoiceAmountHeaderVisible = isInvoiceAmountHeaderVisible;
    }
    SetGridColumnsWidth() {
        var isRateExists = this.ItemsSource.filter(f => !AppTool.IsNullOrEmpty(f.RelativeRateDate)).length > 0 ? true : false;
        if (isRateExists) {
            this.RateColumnWidth = 120;
        }

        else {
            this.RateColumnWidth = 100;
        }

        var isVATExists = this.ItemsSource.filter(f => f.VatTypeUpdateIsVisible == true).length > 0 ? true : false;
        if (isVATExists) {
            this.VATColumnWidth = 150;
        }

        else {

            var vATColumnWidth = 100;
            this.ItemsSource.forEach(item => {

                if (!AppTool.IsNullOrEmpty(item.VatTypeCell)) {
                    var widthOfLabel = AppTool.GetTextWidth(item.VatTypeCell) + 10;

                    if (widthOfLabel > vATColumnWidth) {
                        vATColumnWidth = widthOfLabel;
                    }
                }
            });

            if (vATColumnWidth > 150) {
                vATColumnWidth = 150;
            }

            this.VATColumnWidth = vATColumnWidth;
        }
    }

    // SetUIProperties
    public IsEditingEnabled: boolean = false;
    public RateIsEnabled: boolean = false;
    public VatTypeFilterIsEnabled: boolean = false;
    public AllowManualInvoiceNumber: boolean = false;
    public PaymentTermDisplayInLOV: boolean = true;
    SetUIProperties() {

        this.IsDatesFieldEnabledWhileCrediting
            = FeatureLocator.HasFeaturePermession("ARInvoice", "DatesFieldEnabledWhileCrediting")
            && this.EntityPM.IsAutoCredit && this.EntityPM.StatusCode == 'AC' && !this.EntityPM.ApprovedDate;

        var isEditingEnabled = InvoiceTool.IsEditingARInvoiceEnabled(this.EntityPM);

        if (!AppTool.IsNullOrEmpty(this.BillToAddressId)) {
            this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, isEditingEnabled);
        }

        this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("PaymentTermId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("BranchId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("VatNumber", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ConfirmationNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ConfirmationNumberStatusName", this.ObjectTableName, false);

        this.UIProperties.SetEnabled("InvoiceDate", this.ObjectTableName, this.IsDatesFieldEnabledWhileCrediting || isEditingEnabled);
        this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, isEditingEnabled);
        this.PartnerTypeComboBoxIsDisabled = !isEditingEnabled;

        // Generated General Tab
        if (this.EntityPM != null) {
            this.EntityPM.UIProperties.SetEnabled("UpdateDate", this.ObjectTableName, false);
            this.EntityPM.UIProperties.SetEnabled("UpdatedByUserId", this.ObjectTableName, false);
            this.EntityPM.UIProperties.SetEnabled("Sent", this.ObjectTableName, isEditingEnabled);
            this.EntityPM.UIProperties.SetEnabled("HouseNumber", this.ObjectTableName, isEditingEnabled);
            this.EntityPM.UIProperties.SetEnabled("MasterNumber", this.ObjectTableName, isEditingEnabled);
            this.EntityPM.UIProperties.SetEnabled("CustomerRef", this.ObjectTableName, isEditingEnabled);
            this.EntityPM.UIProperties.SetEnabled("SATPaymentMethodCode", this.ObjectTableName, isEditingEnabled);
            this.EntityPM.UIProperties.SetEnabled("ConfirmationNumberStatusName", this.ObjectTableName, false);

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

        else if (SessionLocator.AccountingSettingPM.AllowManualInvoiceNumber) {
            isFieldtVisible = true;
        }

        this.AllowManualInvoiceNumber = isFieldtVisible;
        this.UIProperties.SetEnabled("IsInvoiceNumberManuallySet", this.ObjectTableName, isFieldtEnabled);
        this.UIProperties.SetVisibility("IsInvoiceNumberManuallySet", this.ObjectTableName, isFieldtVisible);
    }
    SetUIProperties_InvoiceNumber() {
        var isFieldEnabled = false;

        if (this.IsEditingEnabled) {
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

        this.UIProperties.SetEnabled("DueDate", this.ObjectTableName, this.IsDatesFieldEnabledWhileCrediting || AllowManuallyDueDate);
    }

    // Bill To
    public BillToDependencyProperty1: string = null;
    public cardList: CardList = null;
    public glaccount: GLAccountPM = null;
    get BillToId() { return this.EntityPM.BillToId; }
    set BillToId(newValue: string) {
        if (this.EntityPM.BillToId != newValue) {
            this.EntityPM.BillToId = newValue;
            this.EntityPM.CustomerRef = null;
            this.SetUIProperties_BillToAddress();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.VatNumber = null;
                this.BillToName = null;
                this.BillToLocalName = null;
                this.BillToAddressId = null;
                this.InvoiceCurrencyId = SessionLocator.AccountingCurrencyId;
                this.PaymentTermId = SessionLocator.TenantPM.PaymentTermId;
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.cardList = myResponse.Result;
                        if (this.cardList != null) {
                            this.VatNumber = this.cardList.VatNumber;
                            this.BillToName = this.cardList.EnglishName;
                            this.BillToLocalName = this.cardList.LocalName;
                            this.EntityPM.SalesmanUserId = this.cardList.SalesmanUserId;

                            if (!AppTool.IsNullOrEmpty(this.cardList.GLAccountId)) {
                                this.myGLAccountPMService.get(this.cardList.GLAccountId).subscribe((myResponse: ServiceResponse) => {
                                    if (!myResponse.HasError) {
                                        this.glaccount = myResponse.Result;
                                    }
                                });
                            }

                            if (!AppTool.IsNullOrEmpty(this.cardList.InvoiceCurrencyId)) {
                                this.InvoiceCurrencyId = this.cardList.InvoiceCurrencyId;
                            }

                            if (!AppTool.IsNullOrEmpty(this.cardList.PaymentTermId)) {
                                this.PaymentTermId = this.cardList.PaymentTermId;
                            }

                            if (!AppTool.IsNullOrEmpty(this.cardList.VatTypeId)) {
                                this.VatTypeId = this.cardList.VatTypeId;
                            }

                            if (!AppTool.IsNullOrEmpty(this.cardList.BillingAddressId)) {
                                this.BillToAddressId = this.cardList.BillingAddressId;
                            }

                            else if (!AppTool.IsNullOrEmpty(this.cardList.MainAddressId)) {
                                this.BillToAddressId = this.cardList.MainAddressId;
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

    get BranchId() { return this.EntityPM.BranchId; }
    set BranchId(value: string) {
        if (this.EntityPM.BranchId != value) {
            this.EntityPM.BranchId = value;
        }
    }

    get BillToLocalName() { return this.EntityPM.BillToLocalName; }
    set BillToLocalName(newValue: string) {
        if (this.EntityPM.BillToLocalName != newValue) {
            this.EntityPM.BillToLocalName = newValue;
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

    get IsSigned() { return this.EntityPM.IsSigned; }
    set IsSigned(value: string) {
        if (this.EntityPM.IsSigned != value) {
            this.EntityPM.IsSigned = value;
        }
    }

    private myRelativeRateDate: string = null;
    get RelativeRateDate() { return this.myRelativeRateDate; }
    set RelativeRateDate(value: string) {
        if (this.myRelativeRateDate != value) {
            this.myRelativeRateDate = value;
        }
    }
    get ExchangeRateDate() { return this.EntityPM.ExchangeRateDate; }
    set ExchangeRateDate(value: Date) {
        if (this.EntityPM.ExchangeRateDate != value) {
            this.EntityPM.ExchangeRateDate = value;
            this.ComputeRelativeRateDate();
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

    get ConfirmationNumber() { return this.EntityPM.ConfirmationNumber; }
    set ConfirmationNumber(newValue: string) {
        if (this.EntityPM.ConfirmationNumber != newValue) {
            this.EntityPM.ConfirmationNumber = newValue;
        }
    }
    get ConfirmationNumberStatusName() { return this.EntityPM.ConfirmationNumberStatusName; }
    set ConfirmationNumberStatusName(newValue: string) {
        if (this.EntityPM.ConfirmationNumberStatusName != newValue) {
            this.EntityPM.ConfirmationNumberStatusName = newValue;
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

    get InvoiceNumber() {
        if (this.EntityPM.Id == this.EntityPM.InvoiceNumber) {
            return null;
        }
        else {
            return this.EntityPM.InvoiceNumber;
        }
    }
    set InvoiceNumber(newValue: string) {
        if (this.EntityPM.InvoiceNumber != newValue) {
            this.EntityPM.InvoiceNumber = newValue;
        }
    }

    get StatusCode() { return this.EntityPM.StatusCode; }
    set StatusCode(newValue: string) {
        if (this.EntityPM.StatusCode != newValue) {
            this.EntityPM.StatusCode = newValue;
        }
    }

    get IsInvoiceNumberManuallySet() { return this.EntityPM.IsInvoiceNumberManuallySet; }
    set IsInvoiceNumberManuallySet(newValue: boolean) {
        if (this.EntityPM.IsInvoiceNumberManuallySet != newValue) {
            this.EntityPM.IsInvoiceNumberManuallySet = newValue;

            this.UIProperties.SetEnabled("InvoiceNumber", this.ObjectTableName, newValue);

            if (!newValue) {
                this.InvoiceNumber = null;
            }
        }
    }

    get PrintNotes() { return this.EntityPM.PrintNotes; }
    set PrintNotes(newValue: string) {
        if (this.EntityPM.PrintNotes != newValue) {
            this.EntityPM.PrintNotes = newValue;
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
                        item.VatTypeId = vatType;
                    });

                    this.SetGridColumnsWidth();
                }
            });
        }
    }

    // Prepaid Collect Filter
    get PrepaidCollectId() { return this.EntityPM.PrepaidCollectId; }
    set PrepaidCollectId(newValue: string) {
        if (this.EntityPM.PrepaidCollectId != newValue) {
            this.EntityPM.PrepaidCollectId = newValue;
        }
    }
    OnSelectPrepaidCollect(myPrepaidCollectId: string) {
        if (this.PrepaidCollectId != myPrepaidCollectId) {
            this.PrepaidCollectId = myPrepaidCollectId;

            this.RunPrepaidCollectFilter();
        }
    }
    RunPrepaidCollectFilter() {

        this.ItemsSource.forEach(item => {

            if (this.PrepaidCollectId == "B") {
                if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                    if (this.EntityPM.InvoiceLines.indexOf(item.EntityPM) == -1) {
                        this.EntityPM.AddARInvoiceLinePM(item.EntityPM);
                    }
                }

                else {
                    if (!AppTool.IsNullOrEmpty(item.EntityPM.Id)) {
                        if (this.EntityPM.InvoiceLines.indexOf(item.EntityPM) == -1) {
                            this.EntityPM.AddARInvoiceLinePM(item.EntityPM);
                        }
                    }
                }
            }

            else {
                if (item.PrepaidCollectId == this.PrepaidCollectId) {
                    if (this.EntityPM.InvoiceLines.indexOf(item.EntityPM) == -1) {
                        this.EntityPM.AddARInvoiceLinePM(item.EntityPM);
                    }
                }

                else {
                    if (this.EntityPM.InvoiceLines.indexOf(item.EntityPM) > -1) {
                        this.EntityPM.RemoveARInvoiceLinePM(item.EntityPM);
                    }
                }
            }

            item.RefreshLine();
        });

        this.ComputeTotals();
    }

    // Load Date
    public LastRatesList: LastRate[] = [];
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

        this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator.AccountingCurrencyId, loadingDate).subscribe((myResponse: ServiceResponse) => {

            if (!myResponse.HasError) {
                this.LastRatesList = myResponse.Result;

                this.myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe((myResponse2: ServiceResponse) => {

                    if (!myResponse2.HasError) {
                        this.VatTypePercentagesList = myResponse2.Result;
                    }

                    this.BuildInvoiceLines();

                    this.CurrentSession.StopBusyIndicator();
                });
            }

            else {
                this.CurrentSession.StopBusyIndicator();
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

    BuildScreenData() {
        this.SetIsUsingVirtuallization();
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
            var list: ARInvoiceLinePM[] = this.EntityPM.InvoiceLines;

            list.forEach(line => {
                if (line.UnitPrice < 0) {
                    if (this.EntityPM.ARInvoiceTypeCode != "CD") {
                        if (this.EntityPM.InvoiceLines.indexOf(line) > -1) {
                            tempList.push(line);
                            this.EntityPM.RemoveARInvoiceLinePM(line);
                        }
                    }
                }

                else if (this.EntityPM.PrepaidCollectId != "B") {
                    if (line.PrepaidCollectId != this.EntityPM.PrepaidCollectId) {
                        if (this.EntityPM.InvoiceLines.indexOf(line) > -1) {
                            tempList.push(line);
                            this.EntityPM.RemoveARInvoiceLinePM(line);
                        }
                    }
                }

                if (tempList.indexOf(line) == -1) {
                    this.ItemsSource.push(new ARInvoiceLineItem(line, this, false));
                }
            });

            tempList.forEach(item => {
                this.ItemsSource.push(new ARInvoiceLineItem(item, this, false));
            });

            this.ItemsSource.forEach(item => {
                this.ObservableItems.Insert(item);
            });

            this.ComputeTotals();
        }

        else {
            if (this.EntityPM != null && this.EntityPM.InvoiceLines != null) {
                this.EntityPM.InvoiceLines.forEach(line => {
                    this.ItemsSource.push(new ARInvoiceLineItem(line, this, false));
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

            this.BuildTotalsCollection();
            this.BuildTotalsControl();
        }

        this.SetGridColumnsWidth();
    }
    LoadEntityOpenReceivables() {

        this.CurrentSession.StartBusyIndicatorLoading();

        var myService = new ShipmentDomainService();
        myService.GetInvoiceOpenAmountReceivables(this.EntityPM.ARInvoiceTypeCode, this.EntityPM.MainEntityId).subscribe((myResponse: ServiceResponse) => {

            if (!myResponse.HasError) {
                var allReceivables: ShipmentReceivablePM[] = myResponse.Result;
                var openReceivables = allReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "OAMT" && !AppTool.IsNullOrEmpty(f.UnitPrice) && !AppTool.IsNullOrEmpty(f.Quantity));

                if (this.EntityPM.ARInvoiceTypeCode == "CD") {
                    if (!SessionLocator.AccountingSettingPM.AllowPositiveAmountsInTheCreditNote) {
                        openReceivables = openReceivables.filter(f => f.UnitPrice < 0);
                    }
                }

                else {
                    if (!SessionLocator.AccountingSettingPM.AllowMinusInvoicelines) {
                        openReceivables = openReceivables.filter(f => f.UnitPrice > 0);
                    }
                }

                if (openReceivables.length > 0) {
                    var allVatTypes: VatTypeList[] = [];
                    this.myVatTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            allVatTypes = myResponse.Result;
                        }
                    });

                    openReceivables.forEach(receivable => {
                        var invoiceLine: ARInvoiceLinePM = new ARInvoiceLinePM(null);
                        invoiceLine.Tenant = receivable.Tenant;
                        invoiceLine.ChargesTypeId = receivable.ChargesTypeId;
                        invoiceLine.ForiegnCurrencyId = receivable.CurrencyId;
                        invoiceLine.ForiegnCurrencyCode = receivable.CurrencyCode;
                        invoiceLine.Description = receivable.ChargesTypeName;
                        invoiceLine.ReceivableId = receivable.Id;
                        invoiceLine.VatTypeId = receivable.VatTypeId;
                        invoiceLine.VatPercentage = this.GetVatTypePercentage(receivable.VatTypeId);
                        invoiceLine.MeasurementId = receivable.MeasurementId;
                        invoiceLine.MeasurementCode = receivable.MeasurementCode;
                        invoiceLine.PrepaidCollectId = receivable.PrepaidCollectId;
                        invoiceLine.IsExchangeRateFixed = receivable.IsExchangeRateFixed;
                        invoiceLine.EntityId = receivable.ShipmentId;
                        invoiceLine.EntityReference = receivable.ShipmentNumber;
                        invoiceLine.Quantity = AppTool.Round(receivable.Quantity, 3);
                        invoiceLine.UnitPrice = AppTool.Round(receivable.Quantity, 3);
                        invoiceLine.ForiegnCurrencyAmount = AppTool.Round(receivable.Quantity, 2);
                        invoiceLine.IsExpense = receivable.IsExpense;

                        var itemExchangeRate: number = invoiceLine.IsExchangeRateFixed ? receivable.Rate : this.GetCurrencyRate(invoiceLine.ForiegnCurrencyId);
                        var itemExchangeRateRounded: number = AppTool.Round(itemExchangeRate, 5);
                        invoiceLine.ForiegnExchangeRate = itemExchangeRateRounded;

                        var localAmount: number = invoiceLine.ForiegnCurrencyAmount * invoiceLine.ForiegnExchangeRate;
                        var localAmountRounded: number = localAmount == null ? 0 : AppTool.Round(localAmount, 2);
                        invoiceLine.LocalCurrencyAmount = localAmountRounded;

                        var profitCurrencyAmount: number = invoiceLine.LocalCurrencyAmount / this.ProfitCurrencyExchangeRate;
                        var profitCurrencyAmountRounded: number = profitCurrencyAmount == null ? 0 : AppTool.Round(profitCurrencyAmount, 2);
                        invoiceLine.ProfitCurrencyAmount = profitCurrencyAmountRounded;

                        var invoiceCurrencyAmount: number = invoiceLine.LocalCurrencyAmount / this.InvoiceCurrencyExchangeRate;
                        var invoiceCurrencyAmountRounded: number = invoiceCurrencyAmount == null ? 0 : AppTool.Round(invoiceCurrencyAmount, 2);
                        invoiceLine.InvoiceCurrencyAmount = invoiceCurrencyAmountRounded;

                        this.myChargesTypeListService.getSingleFromCache(receivable.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                var list: ChargesTypeList = myResponse.Result;
                                if (list != null) {
                                    invoiceLine.LocalDescription = list.LocalName;
                                    invoiceLine.VatTypeName = list.VatTypeName;
                                }
                            }
                        });

                        this.ItemsSource.push(new ARInvoiceLineItem(invoiceLine, this, false));
                        this.ObservableItems.Insert(new ARInvoiceLineItem(invoiceLine, this, false));
                    });
                }
            }

            this.SetGridColumnsWidth();
            this.CurrentSession.StopBusyIndicator();
        });
    }

    get IsCurrencyFilterVisible() {
        var myResult = false;

        if (!AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            if (SessionLocator.LocalCurrencyId != this.InvoiceCurrencyId) {
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
            this.BuildTotalsControl();
        }
    }

    public TotalsList: InvoiceTotalsClass[] = [];
    public SummaryItems: SummaryItem[] = [];
    ComputeTotals() {
        this.BuildTotalsCollection(true);
        this.BuildTotalsControl();
    }
    BuildTotalsCollection(isComputingTotals: boolean = false) {

        var totalsList: InvoiceTotalsClass[] = [];
        var invoicelinesList: ARInvoiceLinePM[] = this.EntityPM.InvoiceLines;

        var sumLocalCurrencyAmount = 0;
        var sumInvoiceCurrencyAmount = 0;
        if (this.EntityPM != null && this.EntityPM.InvoiceLines != null) {
            invoicelinesList.forEach(item => {
                if (item.LocalCurrencyAmount != null) {
                    sumLocalCurrencyAmount = sumLocalCurrencyAmount + item.LocalCurrencyAmount;
                }

                if (item.InvoiceCurrencyAmount != null) {
                    sumInvoiceCurrencyAmount = sumInvoiceCurrencyAmount + item.InvoiceCurrencyAmount;
                }
            });
        }

        var subtotal = new InvoiceTotalsClass();
        subtotal.RowLabel = TextCodeTranslator.Translate("ARInvoice.S.Details.Subtotal");
        subtotal.LocalCurrencyAmount = sumLocalCurrencyAmount;
        subtotal.InvoiceCurrencyAmount = sumInvoiceCurrencyAmount;

        var dataGroupList: InvoiceTotalsClass[] = [];
        this.ItemsSource.filter(f => f.VatTypeId != null && f.Exists == true).forEach(item => {
            var dataGroupItem = dataGroupList.filter(d => d.VatTypeCell == item.VatTypeCell)[0];
            if (dataGroupItem == null) {
                dataGroupItem = new InvoiceTotalsClass();
                dataGroupItem.RowLabel = item.VatTypeCell;
                dataGroupItem.VatTypeCell = item.VatTypeCell;
                dataGroupItem.LocalCurrencyAmount = 0;
                dataGroupItem.InvoiceCurrencyAmount = 0;
                dataGroupList.push(dataGroupItem);
            }

            if (item.VatPercentage != null) {
                if (item.LocalCurrencyAmount != null) {
                    dataGroupItem.LocalCurrencyAmount = dataGroupItem.LocalCurrencyAmount + (item.VatPercentage * item.LocalCurrencyAmount / 100);
                }

                if (item.InvoiceCurrencyAmount != null) {
                    dataGroupItem.InvoiceCurrencyAmount = dataGroupItem.InvoiceCurrencyAmount + (item.VatPercentage * item.InvoiceCurrencyAmount / 100);
                }
            }
        });

        if (dataGroupList.length > 0) {
            totalsList.push(subtotal);

            dataGroupList.forEach(item => {
                totalsList.push(item);

                if (item.LocalCurrencyAmount != null) {
                    sumLocalCurrencyAmount = sumLocalCurrencyAmount + item.LocalCurrencyAmount;
                }

                if (item.InvoiceCurrencyAmount != null) {
                    sumInvoiceCurrencyAmount = sumInvoiceCurrencyAmount + item.InvoiceCurrencyAmount;
                }
            });
        }

        if (isComputingTotals) {
            this.SubTotalInLocalCurrency = AppTool.Round(subtotal.LocalCurrencyAmount, 2);
            this.SubTotalInInvoiceCurrency = AppTool.Round(subtotal.InvoiceCurrencyAmount, 2);
            this.AmountInLocalCurrency = AppTool.Round(sumLocalCurrencyAmount, 2);
            this.AmountInInvoiceCurrency = AppTool.Round(sumInvoiceCurrencyAmount, 2);

            if (this.ProfitCurrencyId == this.InvoiceCurrencyId) {
                this.AmountInProfitCurrency = this.AmountInInvoiceCurrency;
            }

            else {
                this.AmountInProfitCurrency = this.AmountInLocalCurrency / this.ProfitCurrencyExchangeRate;
            }

            this.AmountDue = this.AmountInInvoiceCurrency == null ? 0 : this.AmountInInvoiceCurrency;
            this.AmountDueInLocalCurrency = this.AmountInLocalCurrency == null ? 0 : this.AmountInLocalCurrency;
            this.AmountDueInProfitCurrency = this.AmountInProfitCurrency == null ? 0 : this.AmountInProfitCurrency;
        }

        this.TotalsList = totalsList;
        this.BuildTotalsControl();
    }
    BuildTotalsControl() {
        this.SummaryItems = [];
        var pipe = new NumbersPipe();

        for (var i = 0; i < this.TotalsList.length; i++) {
            var item: InvoiceTotalsClass = this.TotalsList[i];

            var mySummaryItem = new SummaryItem();
            mySummaryItem.Label = item.RowLabel;
            mySummaryItem.Value = this.IsTotalInLocalCurrency ? pipe.transform(item.LocalCurrencyAmount, "N2") : pipe.transform(item.InvoiceCurrencyAmount, "N2");
            this.SummaryItems.push(mySummaryItem);

            if (i + 1 < this.TotalsList.length) {
                var myOperatorItem = new SummaryItem();
                myOperatorItem.Value = "+";
                this.SummaryItems.push(myOperatorItem);
            }

            else if (i + 1 == this.TotalsList.length) {
                var myOperatorItem = new SummaryItem();
                myOperatorItem.Value = "=";
                this.SummaryItems.push(myOperatorItem);
            }
        }

        // Total
        var myCurrencyCode = this.IsTotalInLocalCurrency ? "(" + SessionLocator.LocalCurrencyCode + ")" : "(" + this.InvoiceCurrencyCode + ")";

        var myTotalItem = new SummaryItem();
        myTotalItem.Label = TextCodeTranslator.Translate("ARInvoice.F.AmountInInvoiceCurrency");
        myTotalItem.Label += " " + myCurrencyCode;
        myTotalItem.Value = this.IsTotalInLocalCurrency ? pipe.transform(this.EntityPM.AmountInLocalCurrency, "N2") : pipe.transform(this.EntityPM.AmountInInvoiceCurrency, "N2");
        this.SummaryItems.push(myTotalItem);
    }

    get SubTotalInLocalCurrency() { return this.EntityPM.SubTotalInLocalCurrency; }
    set SubTotalInLocalCurrency(value: number) {
        var setValue: number = AppTool.Round(value, 2);
        if (this.EntityPM.SubTotalInLocalCurrency != setValue) {
            this.EntityPM.SubTotalInLocalCurrency = setValue;
        }
    }

    get SubTotalInInvoiceCurrency() { return this.EntityPM.SubTotalInInvoiceCurrency; }
    set SubTotalInInvoiceCurrency(value: number) {
        var setValue: number = AppTool.Round(value, 2);
        if (this.EntityPM.SubTotalInInvoiceCurrency != setValue) {
            this.EntityPM.SubTotalInInvoiceCurrency = setValue;
        }
    }

    get AmountInLocalCurrency() { return this.EntityPM.AmountInLocalCurrency; }
    set AmountInLocalCurrency(value: number) {
        var setValue: number = AppTool.Round(value, 2);
        if (this.EntityPM.AmountInLocalCurrency != setValue) {
            this.EntityPM.AmountInLocalCurrency = setValue;
        }
    }

    get AmountInInvoiceCurrency() { return this.EntityPM.AmountInInvoiceCurrency; }
    set AmountInInvoiceCurrency(value: number) {
        var setValue: number = AppTool.Round(value, 2);
        if (this.EntityPM.AmountInInvoiceCurrency != setValue) {
            this.EntityPM.AmountInInvoiceCurrency = setValue;
        }
    }

    get AmountInProfitCurrency() { return this.EntityPM.AmountInProfitCurrency; }
    set AmountInProfitCurrency(value: number) {
        var setValue: number = AppTool.Round(value, 2);
        if (this.EntityPM.AmountInProfitCurrency != setValue) {
            this.EntityPM.AmountInProfitCurrency = setValue;
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
            logWindow.Show('./InvoiceModules/ARInvoice/Components/EditTabs/AddEditARGeneralInvoiceLineComponent');
        }
    }

    AddARInvoiceLineClicked() {
        var line: ARInvoiceLinePM = new ARInvoiceLinePM(null);
        line.Tenant = SessionLocator.TenantPM.Id;
        line.ARInvoiceId = this.EntityPM.Id;
        line.ForiegnCurrencyId = "";
        line.ForiegnCurrencyCode = "";
        line.ForiegnExchangeRate = this.InvoiceCurrencyExchangeRate;

        line.InvoiceCurrencyCode = this.InvoiceCurrencyCode;
        line.InvoiceCurrencyId = this.InvoiceCurrencyId;
        var logWindow = new LogitudeWindow();
        logWindow.Title = TextCodeTranslator.Translate("ARInvoiceLine.O.EditInvoiceLine");
        var addEditViewModel: ARInvoiceLineItem = new ARInvoiceLineItem(line, this, true);
        logWindow.DataContext = addEditViewModel;
        logWindow.Show('./InvoiceModules/ARInvoice/Components/EditTabs/AddEditARGeneralInvoiceLineComponent');
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
                cmpRef.instance.Run({ EntityId: this.JournalId, ObjectTableName: 'Journal' });
            });
    }
}

export class ARInvoiceLineItem extends BaseComponent {
    public EntityPM: ARInvoiceLinePM = null;
    public ObjectTableName = "ARInvoiceLine";
    public DataContext = this;

    constructor(entityPM: ARInvoiceLinePM, public fatherComponent: ARInvoiceDetailsTabGeneral, public AddNewLineMode) {
        super();
        this.EntityPM = entityPM;
        this.ReadIsMatched();
        this.ReadCellBackground();
        this.ReadVatTypeData();
        this.ComputeRelativeRateDate();
        this.SetUIProperties();

        if (this.EntityPM.ForiegnCurrencyCode != null) {
            this.AmountForiegnLabel = TextCodeTranslator.Translate("ARInvoiceLine.F.ForiegnCurrencyAmount").replace("%ForiegnCurrencyCode", this.EntityPM.ForiegnCurrencyCode);
        } else {
            this.AmountForiegnLabel = TextCodeTranslator.Translate("ARInvoiceLine.F.ForiegnCurrencyAmount").replace("%ForiegnCurrencyCode", "");
        }
    }

    public IsEditingEnabled: boolean = false;
    public IsRateEnabled: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    SetUIProperties() {

        this.IsEditExchangeRateVisible = this.fatherComponent.IsEditExchangeRateVisible;

        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;

        this.UIProperties.SetEnabled("ForiegnCurrencyAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("LocalCurrencyAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("InvoiceCurrencyAmount", this.ObjectTableName, false);

        this.UIProperties.SetVisibility("InvoiceCurrencyAmount", this.ObjectTableName, false);
        if (this.fatherComponent.InvoiceCurrencyId != SessionLocator.LocalCurrencyId) {
            this.UIProperties.SetVisibility("InvoiceCurrencyAmount", this.ObjectTableName, true);
        }

        this.UIProperties.SetEnabled("Description", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("LocalDescription", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("UnitPrice", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VatPercentage", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ForiegnCurrencyId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingEnabled);


        this.SetUIProperties_Rate();
    }
    SetUIProperties_Rate() {
        var isFieldEnabled = false;

        if (this.IsEditingEnabled) {
            if (FeatureLocator.HasFeaturePermession("ARInvoice", "ARInvoiceEditExchangeRate")) {
                if (this.ForiegnCurrencyId != SessionLocator.LocalCurrencyId && !this.IsExchangeRateFixed) {
                    isFieldEnabled = true;
                }
            }
        }

        this.IsRateEnabled = isFieldEnabled;
        this.UIProperties.SetEnabled("ForiegnExchangeRate", this.ObjectTableName, isFieldEnabled);
    }

    public IsMatched: boolean = false;
    public CellBackground: string;
    RefreshLine() {
        this.ReadIsMatched();
        this.ReadCellBackground();
    }

    ReadIsMatched() {
        var isMatched = false;

        if (this.fatherComponent.PrepaidCollectId == "B") {
            isMatched = true;
        }

        else if (this.fatherComponent.PrepaidCollectId == this.PrepaidCollectId) {
            isMatched = true;
        }

        this.IsMatched = isMatched;
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
    public AmountForiegnLabel: string;
    get ForiegnCurrencyId() { return this.EntityPM.ForiegnCurrencyId; }
    set ForiegnCurrencyId(newValue: string) {
        if (this.EntityPM.ForiegnCurrencyId != newValue) {
            this.EntityPM.ForiegnCurrencyId = newValue;
            this.SetCurrencyRateData();
            this.fatherComponent.myCurrencyListService.getSingleFromCache(newValue).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: CurrencyList = myResponse.Result;
                    if (list != null) {
                        this.ForiegnCurrencyCode = list.Code;

                        this.AmountForiegnLabel = TextCodeTranslator.Translate("ARInvoiceLine.F.ForiegnCurrencyAmount").replace("%ForiegnCurrencyCode", this.ForiegnCurrencyCode);
                    }
                }
            });
        }
    }

    SetCurrencyRateData() {
        var myRate: number = null;
        var myRateDate: Date = null;

        if (!AppTool.IsNullOrEmpty(this.ForiegnCurrencyId)) {
            if (this.ForiegnCurrencyId == SessionLocator.TenantPM.CurrencyId) {
                myRate = 1;
            }

            else {
                var lastRate: LastRate = this.fatherComponent.LastRatesList.filter(d => d.ForeignCurrencyId == this.ForiegnCurrencyId)[0];
                if (lastRate != null) {
                    myRate = lastRate.Rate;
                    myRateDate = lastRate.ValueDate;
                }
            }
        }

        this.ForiegnExchangeRate = myRate;
        this.ExchangeRateDate = myRateDate;
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
    get InvoiceCurrencyId() { return this.EntityPM.InvoiceCurrencyId; }
    set InvoiceCurrencyId(newValue: string) {
        if (this.EntityPM.InvoiceCurrencyId != newValue) {
            this.EntityPM.InvoiceCurrencyId = newValue;
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

    public chargesTypeList: ChargesTypeList = null;
    get ChargesTypeId() { return this.EntityPM.ChargesTypeId; }
    set ChargesTypeId(newValue: string) {
        if (this.EntityPM.ChargesTypeId != newValue) {
            this.EntityPM.ChargesTypeId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.Description = null;
                this.LocalDescription = null;
                this.VatTypeId = null;
                this.LineActionCode = null;

            }

            else {
                this.fatherComponent.myChargesTypeListService.getSingleFromCache(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.chargesTypeList = myResponse.Result;
                        if (this.chargesTypeList != null) {
                            this.Description = this.chargesTypeList.EnglishName;
                            this.LocalDescription = this.chargesTypeList.LocalName;
                            this.VatTypeId = this.chargesTypeList.VatTypeId;
                            this.LineActionCode = this.chargesTypeList.IsExpense ? '2' : '1';
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
    get ValueDate() { return this.EntityPM.ValueDate; }
    set ValueDate(newValue: Date) {
        if (this.EntityPM.ValueDate != newValue) {
            this.EntityPM.ValueDate = newValue;
        }
    }
    get LineActionCode() { return this.EntityPM.LineActionCode; }
    set LineActionCode(newValue: string) {
        if (this.EntityPM.LineActionCode != newValue) {
            this.EntityPM.LineActionCode = newValue;
        }
    }
    get ReportedinTaxReport() { return this.EntityPM.ReportedinTaxReport; }
    set ReportedinTaxReport(newValue: string) {
        if (this.EntityPM.ReportedinTaxReport != newValue) {
            this.EntityPM.ReportedinTaxReport = newValue;
        }
    }

    get GLAccountLocalName() { return this.EntityPM.GLAccountLocalName; }
    set GLAccountLocalName(newValue: string) {
        if (this.EntityPM.GLAccountLocalName != newValue) {
            this.EntityPM.GLAccountLocalName = newValue;
        }
    }

    get GLAccountDisplayNumber() { return this.EntityPM.GLAccountDisplayNumber; }
    set GLAccountDisplayNumber(newValue: string) {
        if (this.EntityPM.GLAccountDisplayNumber != newValue) {
            this.EntityPM.GLAccountDisplayNumber = newValue;
        }
    }

    get LocalDescription() { return this.EntityPM.LocalDescription; }
    set LocalDescription(newValue: string) {
        if (this.EntityPM.LocalDescription != newValue) {
            this.EntityPM.LocalDescription = newValue;
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
            this.ReadVatTypeData();
            this.fatherComponent.ComputeTotals();
        }

        else {
            this.fatherComponent.myVatTypeListService.getSingleFromCache(this.VatTypeId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: VatTypeList = myResponse.Result;
                    if (list != null) {
                        this.VatTypeName = list.EnglishName;
                        this.VatPercentage = this.fatherComponent.GetVatTypePercentage(this.VatTypeId);
                        this.ReadVatTypeData();
                        this.fatherComponent.ComputeTotals();
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
        }
    }

    public VatTypeCell: string;
    public VatTypeCellColor: string;
    public VatTypeUpdateIsVisible: boolean = false;
    ReadVatTypeData() {
        var myValue: string = null;
        var myColor: string = "#282E30";
        var isUpdateVisible = false;

        var pipe: NumbersPipe = new NumbersPipe();

        if (!AppTool.IsNullOrEmpty(this.VatTypeId)) {
            if (this.VatPercentage != null) {
                myValue = this.VatTypeName + " (" + pipe.transform(this.VatPercentage, "N3") + "%)";
                myColor = "#282E30";
                isUpdateVisible = false;
            }

            else {
                myValue = TextCodeTranslator.Translate("ARInvoice.S.Details.NoVat");
                myColor = "#E53030";
                isUpdateVisible = true;
            }
        }

        this.VatTypeCell = myValue;
        this.VatTypeCellColor = myColor;
        this.VatTypeUpdateIsVisible = isUpdateVisible;
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
    set ForiegnExchangeRate(newValue: number) {
        if (this.EntityPM.ForiegnExchangeRate != newValue) {
            this.EntityPM.ForiegnExchangeRate = AppTool.Round(newValue, 5);

            if (this.ForiegnCurrencyId == this.fatherComponent.ProfitCurrencyId) {
                if (this.fatherComponent.ProfitCurrencyExchangeRate != newValue) {
                    this.fatherComponent.ProfitCurrencyExchangeRate = newValue;
                }
            }

            if (this.ForiegnCurrencyId == this.fatherComponent.InvoiceCurrencyId) {
                this.fatherComponent.UpdateRateFromLine(this);
                this.ExchangeRateDate = this.fatherComponent.GetCurrencyRateDate(this.ForiegnCurrencyId);
            }

            this.CalculateLocalCurrencyAmount();
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
        var profitCurrencyAmount: number = 0;
        var invoiceCurrencyAmount: number = 0;

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
        if (this.fatherComponent.IsEditingEnabled) {
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
}
