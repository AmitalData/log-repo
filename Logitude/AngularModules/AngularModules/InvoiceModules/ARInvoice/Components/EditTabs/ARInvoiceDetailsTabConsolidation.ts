import {Component, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ARInvoicePM} from '../../../../Invoice/EntityPMs/ARInvoicePM';
import {ConstituentPM} from '../../../../Invoice/EntityPMs/ConstituentPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {DateTool, AppTool, FormatTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {CurrencyRatesService, LastRate} from '../../../../Common/Services/CurrencyRatesService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {InvoiceTotalsClass, SummaryItem} from '../../../../Invoice/Args';
import {InvoiceTool} from '../../../../Invoice/Tools';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {AddressList} from '../../../../Common/EntityLists/AddressList';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {PaymentTermList} from '../../../../Common/EntityLists/PaymentTermList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {PaymentTermListService} from '../../../../Common/Services/StandardLists/PaymentTermListService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {NumbersPipe} from '../../../../Infrastructure/Pipes/NumbersPipe';
import {InvoiceDomainService} from '../../../../Invoice/Services/InvoiceDomainService';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ARInvoiceList} from '../../../../Invoice/EntityLists/ARInvoiceList';
import {ARInvoiceListService} from '../../../../Invoice/Services/StandardLists/ARInvoiceListService';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { ARInvoiceStockLinePM } from '../../../../Invoice/EntityPMs/ARInvoiceStockLinePM';

@Component({
    
    templateUrl: './ARInvoiceDetailsTabConsolidation.html',
})

export class ARInvoiceDetailsTabConsolidation extends BaseComponent implements OnDestroy {
    public EntityPM: ARInvoicePM = null;
    public ObjectTableName = "ARInvoice";
    public DataContext = this;
    public ItemsSource: SubInvoiceLine[] = [];
    public LocalCurrencyId: string;
    public LocalCurrencyCode: string;
    public EntityWarningsList: string[] = [];
    public EntityWarning: string = "";
    public IsEditExchangeRateVisible: boolean = false;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public InvoiceNumberFilterList: CodeNameClass[] = [];
    constructor(private entityArgs: EntityArgs) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");    
        this.EntityPM = entityArgs.EntityPM;
        this.LocalCurrencyId = SessionLocator.LocalCurrencyId;
        this.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
        this.InitializeServices();
        this.InitializeComponent();
        this.SetUIProperties();
        this.BuildScreenData();
        this.Listen()

        this.BuildEntityWarnings();

        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }

        if (SessionLocator.AccountingSettingPM.EnableInvoiceStocksManagement) {
            this.AllowStockInvoiceNumber = true;
        }

        if (!AppTool.IsNullOrEmpty(this.ARInvoiceStockId)) {
            this.IsInvoiceNumberComboBoxEnabled = false;
        }

        this.BuildInvoiceNumberFilters();
    }

    private BuildEntityWarnings() {
        this.EntityWarning = "";
        this.EntityWarningsList = [];
        if (!AppTool.IsNullOrEmpty(this.EntityPM.TransmissionError)) {
            this.EntityWarningsList.push(this.EntityPM.TransmissionError);
            this.EntityWarning = this.EntityPM.TransmissionError;
        }
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;  
    private Listen() {
        if (this.entityArgs.EditComponent != null) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.LoadInvoices();
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
                    this.LoadInvoices();
                    this.BuildEntityWarnings();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    private myCardListService: CardListService;
    private myCurrencyListService: CurrencyListService;
    private myPaymentTermListService: PaymentTermListService;
    private myInvoiceDomainService: InvoiceDomainService;
    private myARInvoiceListService: ARInvoiceListService;
    InitializeServices() {
        this.myCardListService = new CardListService();
        this.myCurrencyListService = new CurrencyListService();
        this.myPaymentTermListService = new PaymentTermListService();
        this.myInvoiceDomainService = new InvoiceDomainService();
        this.myARInvoiceListService = new ARInvoiceListService();
    }
    InitializeComponent() {
        this.ComputeRelativeRateDate();
        this.BillToDependencyValue1 = InvoiceTool.GetBillToPartnerTypes();
    }

    // SetUIProperties
    public IsEditingEnabled: boolean = false;
    public RateIsEnabled: boolean = false;
    public AllowManualInvoiceNumber: boolean = false;
    public AllowStockInvoiceNumber: boolean = false;
    public PaymentTermDisplayInLOV: boolean = true;
    SetUIProperties() {
        var isEditingEnabled = InvoiceTool.IsEditingARInvoiceEnabled(this.EntityPM);

        this.UIProperties.SetEnabled("PaymentTermId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("VatNumber", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("InvoiceDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("FromDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ToDate", this.ObjectTableName, isEditingEnabled);

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

           
            //this.EntityPM.UIProperties.SetEnabled("SalesmanUserId", this.ObjectTableName, isEditingEnabled);
        }

        this.IsEditingEnabled = isEditingEnabled;
        this.SetUIProperties_Connected();
        this.SetUIProperties_BillToAddress();
        this.SetUIProperties_VatNumber();
        this.SetUIProperties_ExchangeRate();
        this.SetUIProperties_PrintNotes();
        this.SetUIProperties_ManuallySet();
        this.SetUIProperties_InvoiceNumber();
        this.SetUIProperties_DueDate();
    }
    SetUIProperties_Connected() {
        var isFieldtEnabled = false;

        if (this.IsEditingEnabled) {

            if (this.EntityPM.ConstituentInvoices.length == 0) {
                isFieldtEnabled = true;
            }            
        }

        this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, isFieldtEnabled);
        this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, isFieldtEnabled);
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
        var isFieldEnabled = false;

        if (this.IsEditingEnabled) {
            if (FeatureLocator.HasFeaturePermession("ARInvoice", "ARInvoiceEditExchangeRate")) {
                if (this.InvoiceCurrencyId) {
                    if (this.InvoiceCurrencyId != SessionLocator.TenantPM.CurrencyId) {
                        isFieldEnabled = true;
                    }
                }
            } 
        }
        
        this.RateIsEnabled = isFieldEnabled;
        this.UIProperties.SetEnabled("InvoiceCurrencyExchangeRate", this.ObjectTableName, isFieldEnabled);
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

    // Bill To
    public BillToDependencyValue1: string = null;
    get BillToId() { return this.EntityPM.BillToId; }
    set BillToId(newValue: string) {
        if (this.EntityPM.BillToId != newValue) {
            this.EntityPM.BillToId = newValue;
            this.EntityPM.CustomerRef = null;
            this.SetUIProperties_BillToAddress();
            this.LoadInvoices();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.VatNumber = null;
                this.BillToName = null;
                this.BillToAddressId = null;
                this.InvoiceCurrencyId = SessionLocator.AccountingCurrencyId;
                this.PaymentTermId = SessionLocator.TenantPM.PaymentTermId;
                this.EntityPM.IsBillToAllowConsolidation = false;

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
                            this.EntityPM.IsBillToAllowConsolidation = list.EnableConsolidationInvoices;

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

            this.ItemsSource.forEach(item => {
                item.SetIsMatch();
            });

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.InvoiceCurrencyCode = null;
                this.SetCurrencyRateData();
            }

            else {
                this.myCurrencyListService.getSingleFromCache(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CurrencyList = myResponse.Result;
                        if (list != null) {
                            this.InvoiceCurrencyCode = list.Code;
                            this.SetCurrencyRateData();
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

    // Load Date 
    private LastRatesList: LastRate[] = [];
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
                this.LoadInvoices();

                this.CurrentSession.StopBusyIndicator();

                this.CheckNotifyPastDateOnInvoiceEdit();
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
            }

            this.CurrentSession.StopBusyIndicator();
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

    public SelectedItem: SubInvoiceLine;
    public IsNoDataTextVisible: boolean = false;
    AllConnectedInvoices: ARInvoiceList[] = [];
    BuildScreenData() {
        if (this.IsEditingEnabled) {
            this.LoadData();
        }

        else {
            this.LoadInvoices();
        }
    }
    LoadInvoices() {

        this.ItemsSource = [];
        this.IsNoDataTextVisible = false;

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.LoadOtherInvoices();
        }

        else {
            var filters = new ApiQueryFilters();
            filters.PageIndex = 0;
            filters.PageSize = 1000;

            filters.addAdditionalFilter("BillToId", this.BillToId, null, null, "Equals", false, false, false, "string");
            filters.addAdditionalFilter("StatusCode", "CN", null, null, "Equals", false, true, false, "string");
            filters.addAdditionalFilter("IsConstituentInvoice", true, null, null, "Equals", false, false, false, "Boolean");
            filters.addAdditionalFilter("ConsolidationInvoiceId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");

            this.myARInvoiceListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.AllConnectedInvoices = myResponse.Result;
                    this.AllConnectedInvoices = this.AllConnectedInvoices.sort(function (a, b) { return a.InvoiceNumber == b.InvoiceNumber ? 0 : a.InvoiceNumber < b.InvoiceNumber ? -1 : 1; });
                    this.LoadOtherInvoices();
                }
            });
        }
    }
    LoadOtherInvoices() {
        this.ItemsSource = [];
        this.IsNoDataTextVisible = false;

        this.AllConnectedInvoices.forEach(item => {
            this.ItemsSource.push(new SubInvoiceLine(item, this));
        });

        if (this.IsEditingEnabled) {
            var filters = new ApiQueryFilters();
            filters.PageIndex = 0;
            filters.PageSize = 1000;

            filters.addAdditionalFilter("BillToId", this.BillToId, null, null, "Equals", false, false, false, "string");
            filters.addAdditionalFilter("StatusCode", "NT", null, null, "Equals", false, true, false, "string");
            filters.addAdditionalFilter("IsConstituentInvoice", true, null, null, "Equals", false, false, false, "Boolean");
            filters.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "Boolean");
            filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");

            // Type Filter
            if (this.EntityPM.ARInvoiceTypeCode == "CD") {
                if (SessionLocator.AccountingSettingPM.AllowPositiveAmountsInTheCreditNote) {
                    filters.addAdditionalFilter("ARInvoiceTypeCode", "IN,CD", null, null, "InList", false, true, false, "string");
                }

                else {
                    filters.addAdditionalFilter("ARInvoiceTypeCode", "CD", null, null, "Equals", false, true, false, "string");
                }
            }

            else {
                if (SessionLocator.AccountingSettingPM.AllowMinusInvoicelines) {
                    filters.addAdditionalFilter("ARInvoiceTypeCode", "IN,CD", null, null, "InList", false, true, false, "string");
                }

                else {
                    filters.addAdditionalFilter("ARInvoiceTypeCode", "IN", null, null, "Equals", false, true, false, "string");
                }
            }

            // Date Filter
            var myFilterField:string = this.IsByInvoiceDate ? "InvoiceDate" : "CreateDate";

            if (!AppTool.IsNullOrEmpty(this.FromDate) && !AppTool.IsNullOrEmpty(this.ToDate)) {
                var myToDate: Date = DateTool.GetDateParts(this.ToDate).DateObject;
                myToDate.setUTCHours(23);
                myToDate.setUTCMinutes(59);
                myToDate.setUTCSeconds(59);
                filters.addAdditionalFilter(myFilterField, this.FromDate, myToDate, null, "Between", false, true, false, "date");
            }

            else if (!AppTool.IsNullOrEmpty(this.FromDate)) {
                var myFromDate: Date = DateTool.GetDateParts(this.FromDate).DateObject;
                filters.addAdditionalFilter(myFilterField, this.FromDate, null, null, "GreaterThanOrEqual", false, true, false, "date");
            }

            else if (!AppTool.IsNullOrEmpty(this.ToDate)) {
                var myToDate: Date = DateTool.GetDateParts(this.ToDate).DateObject;
                myToDate.setUTCHours(23);
                myToDate.setUTCMinutes(59);
                myToDate.setUTCSeconds(59);
                filters.addAdditionalFilter(myFilterField, myToDate, null, null, "LessThanOrEqual", false, true, false, "date");
            }

            this.myARInvoiceListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: ARInvoiceList[] = myResponse.Result;

                    list = list.sort(function (a, b) { return a.InvoiceNumber == b.InvoiceNumber ? 0 : a.InvoiceNumber < b.InvoiceNumber ? -1 : 1; });

                    list.filter(f => f.InvoiceCurrencyId == this.InvoiceCurrencyId).forEach(item => {
                        this.ItemsSource.push(new SubInvoiceLine(item, this));
                    });

                    list.filter(f => f.InvoiceCurrencyId != this.InvoiceCurrencyId).forEach(item => {
                        this.ItemsSource.push(new SubInvoiceLine(item, this));
                    });
                }

                if (this.ItemsSource.length == 0) {
                    this.IsNoDataTextVisible = true;
                }

                this.BuildTotalsCollection();
            });
        }

        else {

            if (this.ItemsSource.length == 0) {
                this.IsNoDataTextVisible = true;
            }

            this.BuildTotalsCollection();
        }
    }

    // Filter
    private fromDate: Date;
    get FromDate() { return this.fromDate; }
    set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
            this.LoadOtherInvoices();
        }
    }

    private toDate: Date;
    get ToDate() { return this.toDate; }
    set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
            this.LoadOtherInvoices();
        }
    }

    private isByInvoiceDate: boolean = true;
    get IsByInvoiceDate() { return this.isByInvoiceDate; }
    set IsByInvoiceDate(value: boolean) {
        if (this.isByInvoiceDate != value) {
            this.isByInvoiceDate = value;
        }
    }

    private isByCreateDate: boolean = false;
    get IsByCreateDate() { return this.isByCreateDate; }
    set IsByCreateDate(value: boolean) {
        if (this.isByCreateDate != value) {
            this.isByCreateDate = value;
        }
    }

    SetByDateFilter(myCode: string) {
        this.IsByInvoiceDate = false;
        this.IsByCreateDate = false;

        if (myCode == "IN") {
            this.IsByInvoiceDate = true;
        }

        else {
            this.IsByCreateDate = true;
        }

        this.LoadOtherInvoices();
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
            this.BuildTotalsControl();
        }
    }

    public TotalsList: InvoiceTotalsClass[] = [];
    public SummaryItems: SummaryItem[] = [];
    ComputeTotals() {
        this.BuildTotalsCollection(true);
    }
    BuildTotalsCollection(isComputingTotals: boolean = false) {
        var totalsList: InvoiceTotalsClass[] = [];
        var myDataList = this.ItemsSource.filter(f => f.IsConnected == true);

        var subTotalItem = new InvoiceTotalsClass();
        var vatTotalItem = new InvoiceTotalsClass();
        var allTotalItem = new InvoiceTotalsClass();
        subTotalItem.RowLabel = TextCodeTranslator.Translate("ARInvoice.S.Details.Subtotal");
        vatTotalItem.RowLabel = "VAT";
        allTotalItem.RowLabel = TextCodeTranslator.Translate("ARInvoice.F.AmountInInvoiceCurrency");

        subTotalItem.LocalCurrencyAmount = ArrayTool.Sum(myDataList, "SubTotalInLocalCurrency");
        subTotalItem.InvoiceCurrencyAmount = ArrayTool.Sum(myDataList, "SubTotalInInvoiceCurrency");  
        totalsList.push(subTotalItem);

        vatTotalItem.LocalCurrencyAmount = ArrayTool.Sum(myDataList, "VATAmountInLocalCurrency");
        vatTotalItem.InvoiceCurrencyAmount = ArrayTool.Sum(myDataList, "VATAmountInInvoiceCurrency"); 
        totalsList.push(vatTotalItem);

        allTotalItem.LocalCurrencyAmount = ArrayTool.Sum(myDataList, "AmountInLocalCurrency");
        allTotalItem.InvoiceCurrencyAmount = ArrayTool.Sum(myDataList, "AmountInInvoiceCurrency");
        totalsList.push(allTotalItem);

        if (isComputingTotals) {
            this.SubTotalInLocalCurrency = AppTool.Round(subTotalItem.LocalCurrencyAmount, 2);
            this.SubTotalInInvoiceCurrency = AppTool.Round(subTotalItem.InvoiceCurrencyAmount, 2);
            this.AmountInLocalCurrency = AppTool.Round(allTotalItem.LocalCurrencyAmount, 2);
            this.AmountInInvoiceCurrency = AppTool.Round(allTotalItem.InvoiceCurrencyAmount, 2);

            if (this.ProfitCurrencyId == this.InvoiceCurrencyId) {
                this.AmountInProfitCurrency = this.AmountInInvoiceCurrency;
            }

            else {
                if (AppTool.IsNullOrZero(this.ProfitCurrencyExchangeRate)) {
                    this.AmountInProfitCurrency = 0;
                }

                else {
                    this.AmountInProfitCurrency = AppTool.Round(this.AmountInLocalCurrency / this.ProfitCurrencyExchangeRate, 2);
                }
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
        var selectedCurrencyCode = this.IsTotalInLocalCurrency ? "(" + this.LocalCurrencyCode + ")" : "(" + this.InvoiceCurrencyCode + ")";

        for (var i = 0; i < this.TotalsList.length; i++) {
            var item: InvoiceTotalsClass = this.TotalsList[i];

            var mySummaryItem = new SummaryItem();
            mySummaryItem.Label = item.RowLabel;
            mySummaryItem.Value = this.IsTotalInLocalCurrency ? pipe.transform(item.LocalCurrencyAmount, "N2") : pipe.transform(item.InvoiceCurrencyAmount, "N2");
            this.SummaryItems.push(mySummaryItem);

            if (i + 2 < this.TotalsList.length) {
                var myOperatorItem = new SummaryItem();
                myOperatorItem.Value = "+";
                this.SummaryItems.push(myOperatorItem);
            }

            else if (i + 1 < this.TotalsList.length) {
                var myOperatorItem = new SummaryItem();
                myOperatorItem.Value = "=";
                this.SummaryItems.push(myOperatorItem);
            }

            else if (i + 1 == this.TotalsList.length) {
                mySummaryItem.Label += " " + selectedCurrencyCode;
            }
        }
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

    get ARInvoiceStockId() { return this.EntityPM.ARInvoiceStockId; }
    set ARInvoiceStockId(newValue: string) {
        if (this.EntityPM.ARInvoiceStockId != newValue) {
            this.EntityPM.ARInvoiceStockId = newValue;
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

        if (!AppTool.IsNullOrEmpty(this.EntityPM != null && this.EntityPM.ARInvoiceStockId)) {
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
}
export class SubInvoiceLine {
    public entityList: ARInvoiceList;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(item: ARInvoiceList, private fatherComponent: ARInvoiceDetailsTabConsolidation) {
        this.entityList = item;

        if (this.fatherComponent.InvoiceCurrencyId == item.InvoiceCurrencyId) {
            this.IsMatched = true;
        }

        if (this.fatherComponent.EntityPM.ConstituentInvoices.filter(f => f.ConsolidationInvoiceId == this.fatherComponent.EntityPM.Id && f.Id == this.entityList.Id).length > 0) {
            this.isConnected = true;
        }

        this.SetIsMatch();
    }

    SetIsMatch() {
        this.IsMatched = this.fatherComponent.InvoiceCurrencyId == this.InvoiceCurrencyId ? true : false;
    }

    private isMatched: boolean = false;
    get IsMatched() { return this.isMatched; }
    set IsMatched(value: boolean) {
        if (this.isMatched != value) {
            this.isMatched = value;
        }
    }

    get Id() { return this.entityList.Id; }
    get InvoiceCurrencyId() { return this.entityList.InvoiceCurrencyId; }
    get InvoiceCurrencyCode() { return this.entityList.InvoiceCurrencyCode; }
    get InvoiceNumber() { return this.entityList.InvoiceNumber; }
    get CustomerRef() { return this.entityList.CustomerRef; }
    get MasterNumber() { return this.entityList.MasterNumber; }
    get HouseNumber() { return this.entityList.HouseNumber; }
    get MainEntityReference() { return this.entityList.MainEntityReference; }
    get SubTotalInInvoiceCurrency() { return this.entityList.SubTotalInInvoiceCurrency; }
    get SubTotalInLocalCurrency() { return this.entityList.SubTotalInLocalCurrency; }
    get AmountInInvoiceCurrency() { return this.entityList.AmountInInvoiceCurrency; }
    get AmountInLocalCurrency() { return this.entityList.AmountInLocalCurrency; }
    get AmountInProfitCurrency() { return this.entityList.AmountInProfitCurrency; }
    get AmountDue() { return this.entityList.AmountDue; }
    get AmountDueInLocalCurrency() { return this.entityList.AmountDueInLocalCurrency; }
    get AmountDueInProfitCurrency() { return this.entityList.AmountDueInProfitCurrency; }
    get VATAmountInInvoiceCurrency() {
        var myResult: number = 0;

        if (!AppTool.IsNullOrEmpty(this.AmountInInvoiceCurrency)) {
            myResult += this.AmountInInvoiceCurrency;
        }

        if (!AppTool.IsNullOrEmpty(this.SubTotalInInvoiceCurrency)) {
            myResult -= this.SubTotalInInvoiceCurrency;
        }

        return myResult;
    }
    get VATAmountInLocalCurrency() {
        var myResult: number = 0;

        if (!AppTool.IsNullOrEmpty(this.AmountInLocalCurrency)) {
            myResult += this.AmountInLocalCurrency;
        }

        if (!AppTool.IsNullOrEmpty(this.SubTotalInLocalCurrency)) {
            myResult -= this.SubTotalInLocalCurrency;
        }

        return myResult;
    }

    private isConnected: boolean = false;
    get IsConnected() { return this.isConnected; }
    set IsConnected(value: boolean) {
        if (this.isConnected != value) {
            this.isConnected = value;

            if (value == true) {
                var itemPM: ConstituentPM = new ConstituentPM(null);
                itemPM.Id = this.entityList.Id;
                itemPM.Tenant = this.entityList.Tenant;
                itemPM.ConsolidationInvoiceId = this.fatherComponent.EntityPM.Id;
                this.fatherComponent.EntityPM.AddConstituentPM(itemPM);
            }

            else {
                var itemPM: ConstituentPM = this.fatherComponent.EntityPM.ConstituentInvoices.filter(f => f.ConsolidationInvoiceId == this.fatherComponent.EntityPM.Id && f.Id == this.Id)[0];
                if (itemPM) {
                    this.fatherComponent.EntityPM.RemoveConstituentPM(itemPM);
                }
            }

            this.fatherComponent.ComputeTotals();
            this.fatherComponent.SetUIProperties_Connected();

            if (!AppTool.IsNullOrEmpty(this.fatherComponent.EntityPM.Id)) {
                if (this.CurrentSession.CurrentEditComponent) {
                    this.CurrentSession.CurrentEditComponent.SaveChanges();
                }
            }
        }
    }

    ViewEntityClicked() {
        var myBackButtonLabel = "A/R Invoice";
        if (!AppTool.IsNullOrEmpty(this.fatherComponent.EntityPM.InvoiceNumber)) {
            myBackButtonLabel += ": " + this.fatherComponent.EntityPM.InvoiceNumber;
        }

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: this.Id, ObjectTableName: 'ARInvoice', BackButtonLabel: myBackButtonLabel });

                let isEditComponentSaved = false;

                cmpRef.instance.BackCompleted.subscribe(bk => {
                    if (isEditComponentSaved) {
                        this.fatherComponent.LoadInvoices();
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
}
