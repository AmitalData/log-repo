import {Component, OnInit, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentReceivablePM} from '../../../../Shipment/EntityPMs/ShipmentReceivablePM';
import {ConsoleShipmentPM} from '../../../../Shipment/EntityPMs/ConsoleShipmentPM';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {DecimalFormatter} from '../../../../Infrastructure/Utilities/DecimalFormatter';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentTool, ShipmentGenerator, ByPckageType} from '../../../../Shipment/Tools';
import {AppTool, DateTool, ArrayTool, FontTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {MeasurementList} from '../../../../Common/EntityLists/MeasurementList';
import {ChargesTypeList} from '../../../../Common/EntityLists/ChargesTypeList';
import {PackageTypeList} from '../../../../Common/EntityLists/PackageTypeList';
import {UserList} from '../../../../Common/EntityLists/UserList';
import {CurrencyRatesService, LastRate} from '../../../../Common/Services/CurrencyRatesService';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {MeasurementListService} from '../../../../Common/Services/StandardLists/MeasurementListService';
import {ChargesTypeListService} from '../../../../Common/Services/StandardLists/ChargesTypeListService';
import {PackageTypeListService} from '../../../../Common/Services/StandardLists/PackageTypeListService';
import {UserListService} from '../../../../Common/Services/StandardLists/UserListService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ShipmentDomainService} from '../../../../Shipment/Services/ShipmentDomainService';
import {ShipmentPMService} from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {QuotePMService} from '../../../../Quote/Services/StandardPMs/QuotePMService';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    
    templateUrl: './ReceivablesTabComponent.html',
})

export class ReceivablesTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: ShipmentPM = null;
    public OriginShipment: ShipmentPM;
    public ObjectTableName: string = null;
    public DataContext = this;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public ShipmentLevelCode: string = null;
    public ProfitCurrencyId: string = null;
    public ProfitCurrencyCode: string = null;
    public LocalCurrencyId: string = null;
    public LocalCurrencyCode: string = null;
    public ItemsSource: ObservableCollection;
    public IsResourcesReady: boolean = false;
    public IsCustomsInvoiceVisible: boolean = false;
    public IsCustomsCreditVisible: boolean = false;
    public IsCustomsToggleVisible: boolean = false;
    public IsProrateReceivablesVisible: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    private myDomainService: ShipmentDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.OriginShipment = entityArgs.OriginEntity;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.ShipmentLevelCode = this.EntityPM.ShipmentLevelCode;
        this.ProfitCurrencyId = this.EntityPM.ProfitCurrencyId;
        this.ProfitCurrencyCode = this.EntityPM.ProfitCurrencyCode;
        this.LocalCurrencyId = SessionLocator.LocalCurrencyId;
        this.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
        this.ItemsSource = new ObservableCollection([]);
        this.myDomainService = new ShipmentDomainService();
        this.Listen();
        this.Initialize();
        this.SetEditEnabled();
        this.LoadRequiredData();
    }

    private Initialize() {
        if (this.ShipmentLevelCode == "C") {
            this.IsProrateReceivablesVisible = true;
        }

        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }
        
        if (this.ShipmentLevelCode == "D" || this.ShipmentLevelCode == "H") {
            if (ObjectsLocator.CustomsInterfaceSettingPM != null) {
                if (ObjectsLocator.CustomsInterfaceSettingPM.ActivateCustomsManagementInShipments) {
                    this.IsCustomsToggleVisible = true;
                }
            }
        }

        if (this.IsCustomsToggleVisible) {
            if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "CREATENEWCUSTOMSINVOICE")) {
                this.IsCustomsInvoiceVisible = true;
            }

            if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "CREATENEWCUSTOMSCREDIT")) {
                this.IsCustomsCreditVisible = true;
            }
        }
    }

    private SessionEvent: any = null;
    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;  
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "ReceivablesGenerated") {
                    this.BuildItemsSource();
                    this.ComputeShipmentFields();
                }

                else if (s == "OriginShipmentLoaded") {
                    this.OriginShipment = this.entityArgs.OriginEntity;
                }

                else if (s == "StorageReceivableCalculationsChanged") {
                    this.BuildItemsSource();
                    this.ComputeShipmentFields();
                }
            });

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.BuildItemsSource();
                    this.BuildSummaryData();
                    this.BuildProfitData();

                    if (this.SavingRequestCode) {
                        this.ApplySavingCommand();
                    }
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                    this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);

                    this.SetUIProperties();
                    this.BuildItemsSource();
                    this.BuildSummaryData();
                    this.BuildProfitData();
                }
            });

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "SHRE" || tabCode == "JHRE") {
                    this.BuildProfitData();
                    this.CheckUpdateQuantities();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
        AppTool.KillEventEmitter(this.TabSelectedEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    // Load RequiredData
    public BaseQuote: QuotePM = null;
    public AllRates: LastRate[] = [];
    LoadRequiredData() {
        if (this.IsEditingEnabled) {

            this.entityArgs.EditComponent.StartBusyIndicatorLoading();

            if (AppTool.IsNullOrEmpty(this.EntityPM.QuoteId)) {
                this.LoadOtherRequiredData();
            }

            else {
                var myService = new QuotePMService();
                myService.get(this.EntityPM.QuoteId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.BaseQuote = myResponse.Result;
                        this.SetGenerateButtons();
                    }

                    this.LoadOtherRequiredData();
                });
            }
        }
    }
    LoadOtherRequiredData() {
        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
        var myCurrencyRatesService: CurrencyRatesService = new CurrencyRatesService();
        myCurrencyRatesService.getAll(SessionLocator.LocalCurrencyId, todayDate).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllRates = myResponse.Result;
            }

            this.entityArgs.EditComponent.StopBusyIndicator();
        });
    }

    ngOnInit() {
        if (this.EntityPM != null) {
            this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);

            this.entityResourceService.getEntityResourceByTableName("ShipmentReceivable").subscribe((res: any) => {
                this.IsResourcesReady = true;
                this.SetLabels();
                this.SetUIProperties();
                this.BuildItemsSource();
                this.BuildSummaryData();
                this.InitializeProfitArea();
                this.SetGenerateButtons();
            });
        }
    }

    public SelectedRow: ShipmentReceivableItem = null;
    OnRowSelected(itemComponent: ShipmentReceivableItem) {
        this.SelectedRow = itemComponent;
    }
    OnRowLoaded(Row: any) {

        var isExpandaple = false;

        if (this.EntityPM.ShipmentLevelCode == "C") {
            if (Row) {
                var item: ShipmentReceivableItem = Row.rowData;
                if (item) {
                    if (item.EntityPM.ChildShipmentReceivables.length > 0) {
                        if (this.ProrateReceivables) {
                            isExpandaple = true;
                        }
                    }
                }

                Row.SetExpandaple(isExpandaple);
            }
        }
    }

    // Set Labels
    public AmountLocalColumnHeader: string;
    SetLabels() {
        this.AmountLocalColumnHeader = TextCodeTranslator.Translate('Shipment.O.Receivables.AmountLocal').replace('%LocalCurrencyCode', SessionLocator.TenantPM.CurrencyCode);
    }

    // SetUIProperties
    public IsEditingEnabled: boolean = true;
    SetEditEnabled() {
        var isEditingEnabled: boolean = true;

        if (this.EntityPM) {
            if (this.EntityPM.IsCancelled) {
                isEditingEnabled = false;
            }

            else if (this.EntityPM.IsAccountingClosed) {
                isEditingEnabled = false;
            }

            if (isEditingEnabled) {
                if (this.EntityPM.IsOperationalClosed) {
                    isEditingEnabled = false;

                    if (FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Action.AllowReceivables")) {
                        isEditingEnabled = true;
                    }
                }
            }
        }

        this.IsEditingEnabled = isEditingEnabled;
    }
    SetUIProperties() {
        this.SetEditEnabled();
    }

    // ItemsSource
    public InvoiceColumnWidth: number = 100;
    BuildItemsSource() {

        var invoiceColumnWidth = 100;
        var itemsCollection: ShipmentReceivableItem[] = [];

        this.EntityPM.ShipmentReceivables.filter(f => f.ChargesGroupCode == "FRT").sort((a, b) => { return a.ViewOrder - b.ViewOrder }).forEach((item) => {
            var itemComponent = new ShipmentReceivableItem(item, this);

            if (!AppTool.IsNullOrEmpty(item.ARInvoiceId)) {
                var textWidth = AppTool.GetTextWidth(itemComponent.InvoiceNumber + itemComponent.InvoiceStatus, 11) + 10;
                if (textWidth > invoiceColumnWidth) {
                    invoiceColumnWidth = textWidth;
                }
            }

            itemsCollection.push(itemComponent);
        })

        this.EntityPM.ShipmentReceivables.filter(f => f.ChargesGroupCode != "FRT").sort((a, b) => { return a.ViewOrder - b.ViewOrder }).forEach((item) => {
            var itemComponent = new ShipmentReceivableItem(item, this);

            if (!AppTool.IsNullOrEmpty(item.ARInvoiceId)) {
                var textWidth = AppTool.GetTextWidth(itemComponent.InvoiceNumber + itemComponent.InvoiceStatus, 11) + 10;
                if (textWidth > invoiceColumnWidth) {
                    invoiceColumnWidth = textWidth;
                }
            }

            itemsCollection.push(itemComponent);
        })

        this.SetGenerateButtons();
        this.InvoiceColumnWidth = invoiceColumnWidth;
        this.ItemsSource.InsertCollection(itemsCollection);
    }

    // Summary
    public OpenReceivablesCount: number = 0;
    public InvoicesList: any[] = [];
    public CreditNotesList: any[] = [];
    BuildSummaryData() {
        this.OpenReceivablesCount = this.EntityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "OAMT" || f.ShipmentReceivableLineStatusCode == "DRFT").length;
        this.InvoicesList = this.EntityPM.ShipmentARInvoices.filter(f => f.InvoiceTypeCode == "IN" || f.InvoiceTypeCode == "MN" || f.InvoiceTypeCode == "CI");
        this.CreditNotesList = this.EntityPM.ShipmentARInvoices.filter(f => f.InvoiceTypeCode == "CD" || f.InvoiceTypeCode == "CC");
    }

    // Profit
    public IsProfitAreaVisible: boolean = false;
    public IsCurrencyFilterVisible: boolean = false;
    public IsProfitRateVisible: boolean = false;
    public IsByLocalCurrency: boolean = false;
    public SelectedCurrencyCode: string = null;
    public ProfitRate: string = "N/A";
    public Profit: number = null;
    public Defference: number = null;
    public ProfitInSelectedCurrencyText: string = "N/A";
    public PayablesInSelectedCurrencyText: string = "N/A";
    public ReceivablesInSelectedCurrencyText: string = "N/A";
    public DefferenceInSelectedCurrencyText: string = "";
    InitializeProfitArea() {
        if (FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Profit")) {
            this.IsProfitAreaVisible = true;
        }

        this.IsCurrencyFilterVisible = SessionLocator.LocalCurrencyId == this.EntityPM.ProfitCurrencyId ? false : true;
        this.SelectedCurrencyCode = this.EntityPM.ProfitCurrencyCode;

        this.BuildProfitData();
    }
    GetPayablesInSelectedCurrency() {

        var myResult: number = 0;

        if (this.IsByLocalCurrency) {
            if (this.EntityPM.OpenPayablesInLocalCurrency != null) {
                myResult += this.EntityPM.OpenPayablesInLocalCurrency;
            }

            if (this.EntityPM.AccountedPayablesInLocalCurrency != null) {
                myResult += this.EntityPM.AccountedPayablesInLocalCurrency;
            }
        }

        else {
            if (this.EntityPM.OpenPayablesInProfitCurrency != null) {
                myResult += this.EntityPM.OpenPayablesInProfitCurrency;
            }

            if (this.EntityPM.AccountedPayablesInProfitCurrency != null) {
                myResult += this.EntityPM.AccountedPayablesInProfitCurrency;
            }
        }

        if (myResult == null) {
            myResult = 0;
        }

        return myResult;
    }
    GetReceivablesInSelectedCurrency() {

        var myResult: number = 0;

        if (this.IsByLocalCurrency) {
            if (this.EntityPM.OpenReceivablesInLocalCurrency != null) {
                myResult += this.EntityPM.OpenReceivablesInLocalCurrency;
            }

            if (this.EntityPM.AccountedReceivablesInLocalCurrency != null) {
                myResult += this.EntityPM.AccountedReceivablesInLocalCurrency;
            }
        }

        else {
            if (this.EntityPM.OpenReceivablesInProfitCurrency != null) {
                myResult += this.EntityPM.OpenReceivablesInProfitCurrency;
            }

            if (this.EntityPM.AccountedReceivablesInProfitCurrency != null) {
                myResult += this.EntityPM.AccountedReceivablesInProfitCurrency;
            }
        }

        if (myResult == null) {
            myResult = 0;
        }

        return myResult;
    }
    GetProfitInSelectedCurrency() {

        var myResult: number = 0;

        if (this.IsByLocalCurrency) {
            myResult = this.EntityPM.ProfitInLocalCurrency;
        }

        else {
            myResult = this.EntityPM.ProfitInProfitCurrency;
        }

        if (myResult == null) {
            myResult = 0;
        }

        return myResult;
    }
    GetEstimateProfitInSelectedCurrency() {

        var myResult: number = 0;

        if (this.IsByLocalCurrency) {
            myResult = this.EntityPM.EstimateProfitInLocalCurrency;
        }

        else {
            myResult = this.EntityPM.EstimateProfitInProfitCurrency;
        }

        if (myResult == 0) {
            myResult = null;
        }

        return myResult;
    }
    OnSelectCurrency(myCurrencyCode: string) {
        this.SelectedCurrencyCode = myCurrencyCode;

        if (myCurrencyCode == this.LocalCurrencyCode) {
            this.IsByLocalCurrency = true;
        }

        else {
            this.IsByLocalCurrency = false;

        }

        this.BuildProfitData();
    }
    BuildProfitData() {

        var myProfitInSelectedCurrencyText = "N/A";
        var myPayablesInSelectedCurrencyText = "N/A";
        var myReceivablesInSelectedCurrencyText = "N/A";
        var myProfitRate = "N/A";
        var myDefferenceInSelectedCurrency = 0;
        var myDefferenceInSelectedCurrencyText = "";

        // Payables
        var CountOfPAY: number = this.EntityPM.ShipmentPayables.length;
        if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
            CountOfPAY = this.EntityPM.ConnectedShipmentsPayablesCount;
        }

        // Receivables
        var CountOfREC: number = this.EntityPM.ShipmentReceivables.length;
        if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {

            if (this.EntityPM.ProrateReceivables) {
                CountOfREC = this.EntityPM.ConnectedShipmentsReceivablesCount;
            }

            else {
                CountOfREC += this.EntityPM.ConnectedShipmentsReceivablesCount;
            }
        }

        if (CountOfPAY > 0) {
            myPayablesInSelectedCurrencyText = DecimalFormatter.format(this.GetPayablesInSelectedCurrency(), 2);
        }

        if (CountOfREC > 0) {
            myReceivablesInSelectedCurrencyText = DecimalFormatter.format(this.GetReceivablesInSelectedCurrency(), 2);
        }

        if (CountOfPAY > 0 || CountOfREC > 0) {
            myProfitInSelectedCurrencyText = DecimalFormatter.format(this.GetProfitInSelectedCurrency(), 2);
        }

        if (this.EntityPM.ProfitExchangeRate != null) {
            myProfitRate = DecimalFormatter.format(this.EntityPM.ProfitExchangeRate, 5);
        }

        this.ProfitInSelectedCurrencyText = myProfitInSelectedCurrencyText;
        this.PayablesInSelectedCurrencyText = myPayablesInSelectedCurrencyText;
        this.ReceivablesInSelectedCurrencyText = myReceivablesInSelectedCurrencyText;
        this.ProfitRate = myProfitRate;       

        var myProfitInSelectedCurrency = this.GetProfitInSelectedCurrency();
        this.estimateProfitInSelectedCurrency = this.GetEstimateProfitInSelectedCurrency();

        if (myProfitInSelectedCurrency != null && this.EstimateProfitInSelectedCurrency != null) {
            myDefferenceInSelectedCurrency = myProfitInSelectedCurrency - this.EstimateProfitInSelectedCurrency;
            myDefferenceInSelectedCurrencyText = DecimalFormatter.format(myDefferenceInSelectedCurrency, 2);
        }

        this.DefferenceInSelectedCurrencyText = myDefferenceInSelectedCurrencyText;

        var isProfitRateVisible = false;
        if (!this.IsByLocalCurrency) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.ProfitCurrencyId) && this.EntityPM.ProfitCurrencyId != SessionLocator.LocalCurrencyId) {
                isProfitRateVisible = true;
            }
        }

        this.IsProfitRateVisible = isProfitRateVisible
        this.Profit = myProfitInSelectedCurrency;
        this.Defference = myDefferenceInSelectedCurrency;
    }
    UpdateCurrencyRateClicked() {

        var loadingDate = DateTool.GetCurrentDateAsUtc();

        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 350;
        logWindow.Title = TextCodeTranslator.Translate("Shipment.O.Receivables.UpdateCurrencyRate");
        logWindow.WindowArgs = { CurrencyId: this.EntityPM.ProfitCurrencyId, CurrencyCode: this.EntityPM.ProfitCurrencyCode, Rate: this.EntityPM.ProfitExchangeRate, Date: loadingDate };
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.AllRates = comp.RatesList;
                    this.ProfitExchangeRate = AppTool.Round(comp.Rate, 5);
                }
            });
        });
        logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
    }

    OnProfitExchangeRateChanged() {

        var hasChanges: boolean = false;

        this.EntityPM.ShipmentReceivables.forEach(item => {
            if (AppTool.IsNullOrEmpty(item.ARInvoiceId) && AppTool.IsNullOrEmpty(item.ShipmentReceivableParentId)) {

                hasChanges = true;

                item.ProfitCurrencyExchangeRate = this.ProfitExchangeRate;

                if (item.CurrencyId == this.ProfitCurrencyId) {
                    item.AmountInProfitCurrency = item.TotalAmount;
                }

                else {
                    var profitAmount = item.TotalAmountLocal / item.ProfitCurrencyExchangeRate;
                    item.AmountInProfitCurrency = AppTool.Round(profitAmount, 2);
                }
            }
        });

        this.EntityPM.ShipmentPayables.forEach(item => {
            if (AppTool.IsNullOrEmpty(item.ShipmentPayableParentId)) {
                if (item.ShipmentPayableLineStatusCode == "EMPT" || item.ShipmentPayableLineStatusCode == "OAMT") {

                    hasChanges = true;

                    item.ProfitCurrencyExchangeRate = this.ProfitExchangeRate;

                    if (item.CurrencyId == this.ProfitCurrencyId) {
                        item.ExpectedAmountInProfitCurrency = item.ExpectedAmount;
                    }

                    else {
                        var profitAmount = item.ExpectedAmountLocal / item.ProfitCurrencyExchangeRate;
                        item.ExpectedAmountInProfitCurrency = AppTool.Round(profitAmount, 2);
                    }

                    // Other Amounts
                    if (item.ShipmentPayableLineStatusCode == "EMPT" || item.ShipmentPayableLineStatusCode == "OAMT") {
                        item.CorrectionAmount = 0;
                        item.AccountedAmount = 0;
                        item.AccountedAmountInLocalCurrency = 0;
                        item.AccountedAmountInProfitCurrency = 0;

                        if (item.OpenAmount != item.ExpectedAmount) {
                            item.OpenAmount = item.ExpectedAmount;
                            item.OpenAmountInLocalCurrency = item.OpenAmount * item.Rate;
                            item.OpenAmountInProfitCurrency = item.OpenAmountInLocalCurrency / item.ProfitCurrencyExchangeRate;

                            var expe = item.ExpectedAmount == null ? 0 : item.ExpectedAmount;
                            var acct = item.AccountedAmount == null ? 0 : item.AccountedAmount;
                            var open = item.OpenAmount == null ? 0 : item.OpenAmount;
                            var correction = expe - acct - open;

                            item.CorrectionAmount = AppTool.Round(correction, 2);
                            item.CorrectionByUserId = SessionLocator.LoggedUserId;
                            item.CorrectionDate = DateTool.GetCurrentDateAsUtc();

                            if (!AppTool.IsNullOrEmpty(item.CorrectionByUserId)) {
                                ShipmentTool.SetPayableLineStatus(item);
                            }
                        }

                        else {
                            item.OpenAmountInLocalCurrency = item.OpenAmount * item.Rate;
                            item.OpenAmountInProfitCurrency = item.OpenAmountInLocalCurrency / item.ProfitCurrencyExchangeRate;
                        }
                    }
                }
            }
        });

        if (hasChanges) {
            this.ComputeShipmentFields();
            this.BuildItemsSource();
        }
    }

    ShowProfitClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 750;
        logWindow.ShowCloseButton = true;
        logWindow.Title = TextCodeTranslator.Translate("Shipment.S.Profit.ProfitDetails");
        logWindow.WindowArgs = { ShipmentPM: this.EntityPM, IsByLocalCurrency: this.IsByLocalCurrency, IsCurrencyFilterVisible: this.IsCurrencyFilterVisible };
        logWindow.Show('./ShipmentModules/ShipmentTabs/Components/Windows/Profit/ProfitComponent');
    }

    private estimateProfitInSelectedCurrency: number = null;
    get EstimateProfitInSelectedCurrency() { return this.estimateProfitInSelectedCurrency; }
    set EstimateProfitInSelectedCurrency(value: number) {
        if (this.estimateProfitInSelectedCurrency != value) {
            this.estimateProfitInSelectedCurrency = value;

            var rate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);

            if (this.IsByLocalCurrency) {
                this.EntityPM.EstimateProfitInLocalCurrency = AppTool.Round(value, 2);
                this.EntityPM.EstimateProfitInProfitCurrency = AppTool.Round(value / rate, 2);
            }

            else {
                this.EntityPM.EstimateProfitInProfitCurrency = AppTool.Round(value, 2);
                this.EntityPM.EstimateProfitInLocalCurrency = AppTool.Round(value * rate, 2);
            }

            this.BuildProfitData();
        }
    }

    get ProfitExchangeRate() { return this.EntityPM.ProfitExchangeRate; }
    set ProfitExchangeRate(value: number) {
        if (this.EntityPM.ProfitExchangeRate != value) {
            this.EntityPM.ProfitExchangeRate = AppTool.Round(value, 5);
            this.OnProfitExchangeRateChanged();
        }
    }

    // Generate
    public IsGenerateButtonsVisible: boolean = false;
    public IsGenerateFromQuoteChargesEnabled: boolean = false;
    public IsNoChargesTextVisibil: boolean = false;
    SetGenerateButtons() {
        var isGenerateButtonsVisible = false;

        if (this.EntityPM.ShipmentLevelCode == "C") {
            isGenerateButtonsVisible = false;
        }

        else if (this.EntityPM.ShipmentReceivables.length > 0) {
            isGenerateButtonsVisible = false;
        }

        else {
            isGenerateButtonsVisible = true;
        }

        var isNoChargesTextVisibil = false;
        var isGenerateFromQuoteChargesEnabled = false;
        if (isGenerateButtonsVisible) {
            if (this.IsEditingEnabled) {
                if (this.BaseQuote != null) {

                    if (this.BaseQuote.QuoteCharges.length > 0) {
                        isGenerateFromQuoteChargesEnabled = true;
                    }

                    else {
                        isNoChargesTextVisibil = true;
                    }
                }
            }
        }

        this.IsNoChargesTextVisibil = isNoChargesTextVisibil;
        this.IsGenerateButtonsVisible = isGenerateButtonsVisible;
        this.IsGenerateFromQuoteChargesEnabled = isGenerateFromQuoteChargesEnabled;
    }
    GenerateClicked(myCommandCode: string) {
        var isConfirming: boolean = false;

        switch (myCommandCode) {
            case "ATDS": { isConfirming = true; break; }
            case "QTRC": { isConfirming = true; break; }
            case "QTRP": { isConfirming = true; break; }            
        }

        if (isConfirming && this.EntityPM.ShipmentPackages.length == 0) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator.Translate("Shipment.M.ThereAreNoPackages"));
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.StartGenerating(myCommandCode);
                }
            });
        }

        else {
            this.StartGenerating(myCommandCode);
        }
    }
    StartGenerating(myCommandCode: string) {
        switch (myCommandCode) {
            case "ATDS": {
                // AutoDisplay                
                var Generator = new ShipmentGenerator(this.EntityPM, this.AllRates);
                Generator.GenerateReceivablesAutoDisplay();
                this.OnEntityDataGenerated();
                break;
            }

            case "QTRC": {
                // FromQuoteReceivablesOnly
                var Generator = new ShipmentGenerator(this.EntityPM, this.AllRates);
                Generator.GenerateReceivablesFromQuote(this.BaseQuote);
                this.OnEntityDataGenerated();
                break;
            }

            case "QTRP": {
                // FromQuoteReceivablesAndPayables
                var Generator = new ShipmentGenerator(this.EntityPM, this.AllRates);
                Generator.GeneratePayablesFromQuote(this.BaseQuote);
                Generator.GenerateReceivablesFromQuote(this.BaseQuote);
                this.OnEntityDataGenerated();
                this.CurrentSession.FireEvent("PayablesGenerated");
                break;
            }

            case "PAYB": {
                // FromPayables
                var logWindow = new LogitudeWindow();
                logWindow.IsFillScreen = true;
                logWindow.Title = TextCodeTranslator.Translate("Shipment.O.Receivables.GenerateReceivablesFromPayables");
                logWindow.WindowArgs = { EntityPM: this.EntityPM, QuotePM: this.BaseQuote }
                logWindow.Show('./ShipmentModules/ShipmentTabs/Components/Windows/Payables/PayablesComponent');                
                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.OnEntityDataGenerated();
                    }
                });
         
                break;
            }

            case "QTLS": {
                // FromQuoteList
                var logWindow = new LogitudeWindow();
                logWindow.IsFillScreen_115 = true;
                logWindow.Title = TextCodeTranslator.Translate("Shipment.O.Receivables.GenerateFromQuotesList")
                logWindow.WindowArgs = { EntityPM: this.EntityPM, AllRates: this.AllRates };
                logWindow.Show('./ShipmentModules/ShipmentTabs/Components/Windows/Quotes/QuotesComponent');
                logWindow.ComponentLoaded.subscribe(comp => {
                    logWindow.WindowClosed.subscribe(s => {
                        if (s) {
                            this.BaseQuote = comp.BaseQuote;
                            this.OnEntityDataGenerated();
                            this.CurrentSession.FireEvent("PayablesGenerated");
                        }
                    });
                });
                break;
            }

            case "ORGN": {
                // FromOriginShipment
                if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginShipmentId)) {
                    if (this.OriginShipment) {
                        var Generator = new ShipmentGenerator(this.EntityPM, this.AllRates);
                        Generator.GenerateReceivablesFromOriginShipment(this.OriginShipment);
                        this.OnEntityDataGenerated();

                        this.ItemsSource.Collection.forEach((item: ShipmentReceivableItem) => {
                            item.Rate = this.GetCurrencyRate(item.CurrencyId);
                            item.SetQuantity();
                            item.ComputeTotalAmount();
                        });
                    }

                    else {
                        this.entityArgs.EditComponent.StartBusyIndicatorLoading();

                        var myService = new ShipmentPMService();
                        myService.get(this.EntityPM.OriginShipmentId).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                this.OriginShipment = myResponse.Result;
                                this.entityArgs.OriginEntity = myResponse.Result;
                                this.CurrentSession.FireEvent("OriginShipmentLoaded");

                                var Generator = new ShipmentGenerator(this.EntityPM, this.AllRates);
                                Generator.GenerateReceivablesFromOriginShipment(this.OriginShipment);
                                this.OnEntityDataGenerated();

                                this.ItemsSource.Collection.forEach((item: ShipmentReceivableItem) => {
                                    item.Rate = this.GetCurrencyRate(item.CurrencyId);
                                    item.SetQuantity();
                                    item.ComputeTotalAmount();
                                });
                            }

                            this.entityArgs.EditComponent.StopBusyIndicator();
                        });
                    }
                }

                break;
            }

        }
    }
    OnEntityDataGenerated() {
        var rate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
        this.EntityPM.EstimateProfitInProfitCurrency = AppTool.Round(this.EntityPM.EstimateProfitInLocalCurrency / rate, 2);
        this.estimateProfitInSelectedCurrency = this.GetEstimateProfitInSelectedCurrency();

        this.BuildItemsSource();
        this.ComputeShipmentFields();
    }
    GetCurrencyRate(currencyId: string) {
        var myResult: number;

        if (currencyId == SessionLocator.TenantPM.CurrencyId) {
            myResult = 1;
        }

        else {
            var myLastRate: LastRate = this.AllRates.filter(d=> d.ForeignCurrencyId == currencyId)[0];
            if (myLastRate != null) {
                myResult = myLastRate.Rate;
            }
        }

        return myResult;
    }

    // Commands
    AddReceivable() {
        var newItem = new ShipmentReceivablePM(null);
        newItem.Tenant = SessionLocator.Tenant;
        newItem.ShipmentId = this.EntityPM.Id;
        newItem.ShipmentNumber = this.EntityPM.ShipmentNumber;
        newItem.CreateDate = DateTool.GetCurrentDateAsUtc();
        newItem.UpdateDate = DateTool.GetCurrentDateAsUtc();
        newItem.CreatedByUserId = SessionLocator.LoggedUserId;        
        newItem.UpdateByUserId = SessionLocator.LoggedUserId;
        newItem.ShipmentReceivableLineStatusCode = "EMPT";
        newItem.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);

        var itemComponent = new ShipmentReceivableItem(newItem, this, true);
        this.RunAddEditReceivable(itemComponent, TextCodeTranslator.Translate("Shipment.O.Receivables.AddReceivable"));
    };
    EditReceivable(itemComponent: ShipmentReceivableItem) {
        this.RunAddEditReceivable(itemComponent, TextCodeTranslator.Translate("Shipment.O.Receivables.EditReceivable"));
    }
    RunAddEditReceivable(itemComponent: ShipmentReceivableItem, windowTitle: string) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./ShipmentModules/ShipmentTabs/Components/Receivables/AddEditReceivableComponent');
    }
    DeleteItem(itemComponent: ShipmentReceivableItem) {
        if (itemComponent.IsEditingEnabled) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator.Translate("Shipment.M.DeleteThisReceivable"));
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.EntityPM.RemoveReceivable(itemComponent.EntityPM);
                    this.BuildItemsSource();

                    if (itemComponent.ChargesGroupCode == "FRT") {
                        this.OnFreightAmountChanged();
                    }

                    this.ComputeShipmentFields();
                }
            });
        }
    }
    ComputeShipmentFields() {
        if (this.EntityPM != null) {
            ShipmentTool.ComputeTotals(this.EntityPM);
        }

        this.BuildSummaryData();
        this.BuildProfitData();
    }
    OnFreightAmountChanged() {
        this.ItemsSource.Collection.filter(f => f.ChargesGroupCode != "FRT" && f.MeasurementCode == "PRFR").forEach(item => {
            if (item.IsLineAttachted == false) {
                item.SetQuantity();
            }
        });
    }

    // Invoice
    CreateInvoiceClicked(type: string) {
        if (!this.SavingRequested) {
            this.SavingRequested = true;
            this.SavingRequestCode = "NewInvoice";
            this.SavingRequestParam = type;

            this.EntityPM.ShipmentReceivables.forEach(item => {
                if (AppTool.IsNullOrEmpty(item.ARInvoiceId) && AppTool.IsNullOrEmpty(item.ARInvoiceLineId)) {
                    if (AppTool.IsNullOrEmpty(item.ShipmentReceivableLineStatusCode) || item.ShipmentReceivableLineStatusCode == "EMPT") {
                        if (!AppTool.IsNullOrZero(item.Quantity) && !AppTool.IsNullOrZero(item.UnitPrice)) {
                            item.ShipmentReceivableLineStatusCode = "OAMT";
                        }
                    }
                }
            });

            var availableAmount: number = 0;
            var availableAmountText: string = null;

            switch (type) {
                case "IN":
                case "CI":
                    {
                        availableAmountText = TextCodeTranslator.Translate("Shipment.M.NoOpenedAmounts");

                        if (SessionLocator.AccountingSettingPM.AllowMinusInvoicelines) {
                            availableAmount = this.EntityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "OAMT" && f.ARInvoiceId == null && f.ARInvoiceLineId == null).length;
                        }

                        else {
                            availableAmount = this.EntityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "OAMT" && f.ARInvoiceId == null && f.ARInvoiceLineId == null
                                && (f.UnitPrice > 0 || (f.ChargesTypeCode == "ISTOR" && f.MeasurementCode == "STFE" && f.TotalAmount > 0))).length;
                        }

                        if (availableAmount == 0) {
                            var messageWindow = new MessageWindow();
                            messageWindow.Height = 150;
                            messageWindow.Show(availableAmountText);
                            this.StopSavingFlags();
                        }

                        else {
                            this.SaveChanges();
                        }

                        break;
                    }

                case "CD":
                case "CC":
                    {
                        availableAmountText = TextCodeTranslator.Translate("Shipment.M.NoOpenedMinusAmounts");

                        if (SessionLocator.AccountingSettingPM.AllowPositiveAmountsInTheCreditNote) {
                            availableAmount = this.EntityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "OAMT" && f.ARInvoiceId == null && f.ARInvoiceLineId == null).length;
                        }

                        else {
                            availableAmount = this.EntityPM.ShipmentReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "OAMT" && f.ARInvoiceId == null && f.ARInvoiceLineId == null
                                && (f.UnitPrice < 0 || (f.ChargesTypeCode == "ISTOR" && f.MeasurementCode == "STFE" && f.TotalAmount < 0))).length;
                        }

                        if (availableAmount == 0) {
                            var messageWindow = new MessageWindow();
                            messageWindow.Height = 150;
                            messageWindow.Show(availableAmountText);
                            this.StopSavingFlags();
                        }

                        else {
                            this.SaveChanges();
                        }

                        break;
                    }

                case "MN": {
                    this.SaveChanges();
                    break;
                }

                default: {
                    this.StopSavingFlags();
                    break;
                }
            }
        }
    }
    ViewInvoiceClicked(invoiceId: string) {
        if (!AppTool.IsNullOrEmpty(invoiceId)) {
            if (!this.SavingRequested) {
                this.SavingRequested = true;
                this.SavingRequestCode = "ViewInvoice";
                this.SavingRequestParam = invoiceId;
                this.SaveChanges();
            }
        }
    }
    ShowQuoteClicked() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.QuoteId)) {
            if (!this.SavingRequested) {
                this.SavingRequested = true;
                this.SavingRequestCode = "ViewQuote";
                this.SavingRequestParam = this.EntityPM.QuoteId;
                this.SaveChanges();
            }
        }
    }

    private SavingRequested: boolean = false;
    private SavingRequestCode: string = null;
    private SavingRequestParam: string = null;
    SaveChanges() {
        if (this.entityArgs.EditComponent) {
            this.entityArgs.EditComponent.SaveChanges();
        }
    }
    StopSavingFlags() {
        this.SavingRequested = false;
        this.SavingRequestCode = null;
        this.SavingRequestParam = null;
    }
    ApplySavingCommand() {
        switch (this.SavingRequestCode) {
            case "NewInvoice": {
                this.RunNewInvoice();
                break;
            }

            case "ViewInvoice": {
                this.RunViewInvoice();
                break;
            }

            case "ViewQuote": {
                this.RunViewQuote();
                break;
            }
        }

        this.StopSavingFlags();
    }

    RunViewInvoice() {
        if (this.SavingRequestParam) {
            var entityId = this.SavingRequestParam;

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: 'ARInvoice', BackButtonLabel: this.ObjectTableName + ": " + this.EntityPM.ShipmentNumber });

                    let isEditComponentSaved = false;

                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        if (isEditComponentSaved) {
                            this.entityArgs.EditComponent.ReloadEntityPM();
                        }

                        else if (cmpRef.instance.IsReloadNeeded) {
                            this.entityArgs.EditComponent.ReloadEntityPM();
                        }

                        else if (cmpRef.instance.NeedRefresh) {
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
    }
    RunViewQuote() {
        if (this.SavingRequestParam) {
            var entityId = this.SavingRequestParam;

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: 'Quote', BackButtonLabel: this.ObjectTableName + ": " + this.EntityPM.ShipmentNumber });

                    cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                        if (isSaveSuccess) {
                            this.BaseQuote = cmpRef.instance.EntityPM;
                        }
                    });

                    cmpRef.instance.SaveAndCloseCompleted.subscribe((isSaveSuccess: boolean) => {
                        if (isSaveSuccess) {
                            this.BaseQuote = cmpRef.instance.EntityPM;
                        }
                    });
                });
        }
    }   
    RunNewInvoice() {
        var invoiceType = this.SavingRequestParam;

        switch (this.SavingRequestParam) {
            case "MN": {

                this.entityArgs.EditComponent.StartBusyIndicatorLoading();

                this.myDomainService.GetMasterReceivables(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {

                    this.entityArgs.EditComponent.StopBusyIndicator();

                    if (!myResponse.HasError) {

                        if (myResponse.Result.length == 0) {
                            var messageWindow = new MessageWindow();
                            messageWindow.Height = 150;
                            messageWindow.Show(TextCodeTranslator.Translate("Shipment.M.NoOpenedAmounts"));
                        }

                        else {
                            this.RunNewInvoiceWindow(myResponse.Result, invoiceType);
                        }
                    }
                });

                break;
            }

            default: {
                this.RunNewInvoiceWindow(this.EntityPM.ShipmentReceivables, invoiceType);
                break;
            }
        }
    }
    RunNewInvoiceWindow(myReceivables: ShipmentReceivablePM[], invoiceType:string) {
        
        var windowTitle: string = null;
        switch (invoiceType) {
            case "IN": { windowTitle = TextCodeTranslator.Translate("Shipment.O.Receivables.NewInvoice"); break; }
            case "CI": { windowTitle = TextCodeTranslator.Translate("Shipment.O.Receivables.NewCustomsInvoice"); break; }
            case "CC": { windowTitle = TextCodeTranslator.Translate("Shipment.O.Receivables.NewCustomsCredit"); break; }
            case "CD": { windowTitle = TextCodeTranslator.Translate("Shipment.O.Receivables.NewCreditInvoice"); break; }
            case "MN": { windowTitle = TextCodeTranslator.Translate("Shipment.O.Receivables.NewManifestInvoice"); break; }
        }

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.WindowArgs = { Shipment: this.EntityPM, InvoiceTypeCode: invoiceType, EntityLevelCode: this.EntityPM.ShipmentLevelCode, EntityTableName: this.ObjectTableName, EntityReceivables: myReceivables };

        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ EntityPM: comp.EntityPM, ObjectTableName: 'ARInvoice', BackButtonLabel: this.ObjectTableName + ": " + this.EntityPM.ShipmentNumber });

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
            });
        });

      if (FeatureLocator.HasFeaturePermession("ARInvoice", "Intercompany") || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            logitudeWindow.Height = 540;
        }

        logitudeWindow.Show('./InvoiceModules/ARInvoice/Components/NewEntity/NewARInvoiceComponent');
    }

    get ProrateReceivables() { return this.EntityPM.ProrateReceivables; }
    set ProrateReceivables(value: boolean) {
        if (this.EntityPM.ProrateReceivables != value) {
            this.EntityPM.ProrateReceivables = value;

            this.BuildItemsSource();
            this.ComputeShipmentFields();
        }
    }

    // Update Quantities
    public UpdateQuantitiesMessage: string;
    public UpdateQuantitiesMessageWidth: number = 0;
    public IsUpdateQuantitiesVisible: boolean = false;
    CheckUpdateQuantities() {
        var updateMessage = null;

        var activeLines: ShipmentReceivablePM[] = [];
        activeLines = this.EntityPM.ShipmentReceivables;
        activeLines = activeLines.filter(d => d.ShipmentReceivableParentId == null);
        activeLines = activeLines.filter(d => d.ARInvoiceId == null);
        activeLines = activeLines.filter(d => d.UnitPrice != null);

        if (activeLines.length > 0) {

            var isDifferentOrders: boolean = false;
            var isDifferentPRVL: boolean = false;
            var isDifferentPRFR: boolean = false;

            activeLines.forEach(item => {
                switch (item.MeasurementCode) {
                    case "PFCL": {
                        if (item.Quantity != this.EntityPM.PercentForeignChargesLocal) {
                            isDifferentOrders = true;
                        }
                        break;
                    }

                    case "SCGW": {
                        if (item.Quantity != this.EntityPM.GrossWeightPerStorageDays) {
                            isDifferentOrders = true;
                        }
                        break;
                    }

                    case "CWKG": {
                        if (item.Quantity != this.EntityPM.ChargeableWeightInKG) {
                            isDifferentOrders = true;
                        }
                        break;
                    }
                    case "GWKG": {
                        if (item.Quantity != this.EntityPM.GrossWeightInKG) {
                            isDifferentOrders = true;
                        }
                        break;
                    }

                    case "VCBM": {

                        if (item.Quantity != this.EntityPM.VolumeInCBM) {
                            isDifferentOrders = true;
                        }

                        break;
                    }

                    case "GRWT": {

                        if (item.Quantity != this.EntityPM.GrossWeight) {
                            isDifferentOrders = true;
                        }

                        break;
                    }

                    case "GWTN": {

                        if (item.Quantity != this.EntityPM.GrossWeightPerTon) {
                            isDifferentOrders = true;
                        }

                        break;
                    }

                    case "QTY": {

                        if (this.IsLCLEntity) {
                            if (item.Quantity != this.EntityPM.NumberOfPackages) {
                                isDifferentOrders = true;
                            }
                        }

                        else {
                            if (item.Quantity != this.EntityPM.NumberOfContainers) {
                                isDifferentOrders = true;
                            }
                        }

                        break;
                    }

                    case "CHWT": {

                        if (item.Quantity != this.EntityPM.ChargeableWeight) {
                            isDifferentOrders = true;
                        }

                        break;
                    }

                    case "VOLU": {

                        if (item.Quantity != this.EntityPM.Volume) {
                            isDifferentOrders = true;
                        }

                        break;
                    }

                    case "BTEU": {

                        if (item.Quantity != this.EntityPM.TEU) {
                            isDifferentOrders = true;
                        }

                        break;
                    }

                    case "PRVL": {

                        if (item.Quantity != this.EntityPM.ValueOfGoods) {
                            isDifferentPRVL = true;
                        }

                        break;
                    }

                    case "PRFR": {

                        if (this.EntityPM.ShipmentReceivables.filter(f => f.ChargesGroupCode == "FRT").length > 0) {

                            var FRT_Quantity = ArrayTool.Sum(this.EntityPM.ShipmentReceivables.filter(d => d.ChargesGroupCode == "FRT" && AppTool.IsNullOrEmpty(d.ShipmentReceivableParentId)), "TotalAmount");

                            if (item.Quantity != FRT_Quantity) {
                                isDifferentPRVL = true;
                            }

                            if (this.EntityPM.ShipmentReceivables.filter(f => f.MeasurementCode == "PRFR" && f.Quantity != FRT_Quantity).length > 0) {
                                isDifferentPRFR = true;
                            }
                        }

                        break;
                    }
                }
            });

            if (isDifferentOrders) {
                updateMessage = "You have updated the packages details, apply the new values?";
            }

            else if (isDifferentPRVL) {
                updateMessage = "You have updated the value of goods, apply the new values?";
            }

            else if (isDifferentPRFR) {
                updateMessage = "You have updated the value of freight charge, apply the new values?";
            }
        }

        this.UpdateQuantitiesMessage = updateMessage;
        this.UpdateQuantitiesMessageWidth = AppTool.GetTextWidth(updateMessage, 11);
        this.IsUpdateQuantitiesVisible = AppTool.IsNullOrEmpty(updateMessage) ? false : true;
    }
    UpdateQuantitiesClicked() {

        var activeLines: ShipmentReceivableItem[] = [];
        activeLines = this.ItemsSource.Collection;
        activeLines = activeLines.filter(d => d.EntityPM.ShipmentReceivableParentId == null);
        activeLines = activeLines.filter(d => d.EntityPM.ARInvoiceId == null);

        activeLines.forEach((item: ShipmentReceivableItem) => {
            item.SetQuantity();
        });

        this.CheckUpdateQuantities();
    }
}
export class ShipmentReceivableItem extends BaseComponent {
    public IsMinFromQuoteIconVisible: boolean = false; // fix angular 9

    public EntityPM: ShipmentReceivablePM;
    public ShipmentPM: ShipmentPM;
    public ObjectTableName: string = "ShipmentReceivable";
    public DataContext = this;
    public LocalCurrencyCode: string = null;
    public ProfitCurrencyCode: string = null;
    public ByContainersItemsSource: ShipmentReceivablePM[] = [];
    public InsideItemsSource: InsideReceivableViewModel[] = [];
    public IsNewEntity: boolean = false;
    constructor(entity: ShipmentReceivablePM, public fatherComponent: ReceivablesTabComponent, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.ShipmentPM = fatherComponent.EntityPM;
        this.IsNewEntity = isNew;
        this.LocalCurrencyCode = fatherComponent.LocalCurrencyCode;
        this.ProfitCurrencyCode = fatherComponent.ProfitCurrencyCode;
        this.SetUIProperties();
        this.SetLineCells();
        this.SetLineSummary();
        this.SetInvoiceData();
        this.BuildInsideReceivables();
    }

    // SetUIProperties
    public IsRateEnabled: boolean = false;
    public IsQuantityEnabled: boolean = false;
    public IsUnitPriceEnabled: boolean = false;
    public IsTotalAmountEnabled: boolean = false;
    public IsEditingEnabled: boolean = false;
    public IsProfitAmountVisible: boolean = false;
    public IsLineAttachted: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    SetUIProperties() {
        this.IsEditExchangeRateVisible = this.fatherComponent.IsEditExchangeRateVisible;

        var isLineAttachted = false;
        var isEditingEnabled = this.fatherComponent.IsEditingEnabled;

        if (this.EntityPM.ShipmentReceivableParentId != null) {
            isLineAttachted = true;
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ARInvoiceId)) {
            isLineAttachted = true;
        }

        if (isEditingEnabled) {
            if (isLineAttachted) {
                isEditingEnabled = false;
            }
        }

        var isRateEnabled = false;
        var isChargeEnabled = false;
        var isQuantityEnabled = false;
        var isUnitPriceEnabled = false;
        var isTotalAmountEnabled = false;
        var isExchangeRateFixedEnabled = false;
        var isCurrencyEnabled = false;

        if (isEditingEnabled) {
            if (this.IsNewEntity && this.MeasurementCode != "STFE") {
                isChargeEnabled = true;
            }

            if (FeatureLocator.HasFeaturePermession("Shipment", "ShipmentEditExchangeRate")) {
                if (this.CurrencyId != null && this.MeasurementCode != "STFE") {
                    if (this.CurrencyId != SessionLocator.LocalCurrencyId) {
                        isRateEnabled = true;
                    }
                }
            }

            if (this.MeasurementCode != "STFE") {
                isCurrencyEnabled = true;
                isExchangeRateFixedEnabled = true;

                if (!this.EntityPM.IsChargeBySteps) {
                    isUnitPriceEnabled = true;
                }

                if (this.MeasurementCode == "FIXD") {
                    isQuantityEnabled = false;

                    if (!this.EntityPM.IsChargeBySteps) {
                        isTotalAmountEnabled = true;
                    }
                }

                else {
                    isQuantityEnabled = true;
                    isTotalAmountEnabled = false;
                }
            }
        }

        this.IsRateEnabled = isRateEnabled;
        this.IsQuantityEnabled = isQuantityEnabled;
        this.IsUnitPriceEnabled = isUnitPriceEnabled;
        this.IsTotalAmountEnabled = isTotalAmountEnabled;
        this.IsEditingEnabled = isEditingEnabled;
        this.IsLineAttachted = isLineAttachted;

        this.UIProperties.SetEnabled("TotalAmount", this.ObjectTableName, isTotalAmountEnabled);
        this.UIProperties.SetEnabled("TotalAmountLocal", this.ObjectTableName, false);

        this.UIProperties.SetEnabled("Rate", this.ObjectTableName, isRateEnabled);
        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, isChargeEnabled);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, isQuantityEnabled);
        this.UIProperties.SetEnabled("UnitPrice", this.ObjectTableName, isUnitPriceEnabled);
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, isCurrencyEnabled);
        this.UIProperties.SetEnabled("MeasurementId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PrepaidCollectId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("IsExchangeRateFixed", this.ObjectTableName, isExchangeRateFixedEnabled);
        this.SetUIProperties_AmountProfit();
    }
    SetUIProperties_AmountProfit() {
        var isFieldVisible = false;

        if (!AppTool.IsNullOrEmpty(this.CurrencyId)) {
            if (this.ShipmentPM.ProfitCurrencyId != this.CurrencyId && this.ShipmentPM.ProfitCurrencyId != SessionLocator.LocalCurrencyId) {
                isFieldVisible = true;
            }
        }

        this.IsProfitAmountVisible = isFieldVisible;
        this.UIProperties.SetEnabled("AmountInProfitCurrency", this.ObjectTableName, false);
        this.UIProperties.SetVisibility("AmountInProfitCurrency", this.ObjectTableName, isFieldVisible);
    }

    // Line Cells
    public SatusTypeToolTip: string = null;
    public MinMaxFromQuoteToolTip: string = "";
    public IsMinMaxFromQuoteIconVisible: boolean = false;
    public IsFixedAmountIconVisible: boolean = false;
    SetLineCells() {
        this.SetSatusTypeToolTip();
        this.SetMinMaxFromQuoteIconVisibility();
        this.SetFixedAmountIconVisibility();
    }
    SetSatusTypeToolTip() {
        var myResult: string;

        switch (this.EntityPM.ShipmentReceivableLineStatusCode) {
            case "APPD": { myResult = TextCodeTranslator.Translate("Shipment.O.Receivables.Approved"); break; }
            case "OAMT": { myResult = TextCodeTranslator.Translate("Shipment.O.Receivables.OpenAmount"); break; }
            case "ACCT": { myResult = TextCodeTranslator.Translate("Shipment.O.Receivables.Accounted"); break; }
            case "DRFT": { myResult = TextCodeTranslator.Translate("Shipment.O.Receivables.Draft"); break; }
            default: { break; }
        }

        this.SatusTypeToolTip = myResult;
    }
    SetMinMaxFromQuoteIconVisibility() {
        var isVisible = false;
        var iTitle: string = null;

        var iAmount: number = null;
        if (this.EntityPM.Quantity != null && this.EntityPM.UnitPrice != null) {
            iAmount = AppTool.Round(this.EntityPM.Quantity * this.EntityPM.UnitPrice, 2);
        }

        if (iAmount != null) {
            if (this.QuoteSaleMinAmount != null) {
                if (iAmount < this.QuoteSaleMinAmount) {
                    iAmount = this.QuoteSaleMinAmount;
                    isVisible = true;
                    iTitle = TextCodeTranslator.Translate("Shipment.M.Receivables.AmountdueQuoteMinimum");
                }
            }

            if (this.QuoteSaleMaxAmount != null) {
                if (iAmount > this.QuoteSaleMaxAmount) {
                    iAmount = this.QuoteSaleMaxAmount;
                    isVisible = true;
                    iTitle = TextCodeTranslator.Translate("Shipment.M.Receivables.AmountdueQuoteMaximum");
                }
            }
        }

        this.MinMaxFromQuoteToolTip = iTitle;
        this.IsMinMaxFromQuoteIconVisible = isVisible;
    }
    SetFixedAmountIconVisibility() {
        this.IsFixedAmountIconVisible = this.EntityPM.IsFixedPrice && this.EntityPM.IsFromQuote ? true : false;
    }

    // Properties
    get ChargesTypeName() { return this.EntityPM.ChargesTypeName; }
    get ChargesGroupCode() { return this.EntityPM.ChargesGroupCode; }

    get ChargesTypeId() { return this.EntityPM.ChargesTypeId; }
    set ChargesTypeId(value: string) {
        if (this.EntityPM.ChargesTypeId != value) {
            this.EntityPM.ChargesTypeId = value;
            this.SetLineSummary();

            this.fatherComponent.ItemsSource.Collection.filter(f => f.ChargesTypeId == value).forEach(item => {
                item.SetLineSummary();
            });

            if (value == null) {
                this.OnChargesTypeChanged(null);
            }

            else {
                var myService: ChargesTypeListService = new ChargesTypeListService();

                myService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: ChargesTypeList = myResponse.Result;

                        if (list) {
                            this.OnChargesTypeChanged(list);
                        }

                        else {
                            myService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    list = myResponse2.Result;
                                    this.OnChargesTypeChanged(list);
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    OnChargesTypeChanged(list: ChargesTypeList) {
        if (list) {
            this.EntityPM.ChargesTypeCode = list.Code;
            this.EntityPM.ChargesTypeName = list.EnglishName;
            this.EntityPM.ChargesGroupCode = list.ChargesGroupCode;
            this.EntityPM.DueTypeCode = list.DueTypeCode;
            this.EntityPM.DueTypeName = list.DueTypeName;
            this.VatTypeId = list.VatTypeId;
            this.EntityPM.IATACodeId = list.IATACodeId;
            //this.EntityPM.IsBackToBack = list.IsBackToBack;
            this.EntityPM.IsExpense = list.IsExpense;

            this.SetPrepaidCollectId();

            if (!AppTool.IsNullOrEmpty(list.ReceivablesDefaultCurrencyId)) {
                this.CurrencyId = list.ReceivablesDefaultCurrencyId;
            }
            else {
                if (list.ChargesGroupCode == "FRT" || list.ChargesGroupCode == "SCH") {
                    this.CurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
                }

                else {
                    this.CurrencyId = SessionLocator.TenantPM.OtherChargesCurrencyId;
                }
            }

            if (this.fatherComponent.IsLCLEntity) {
                this.MeasurementId = list.MeasurementId;
            }

            else {
                this.MeasurementId = list.ContainerMeasurementId != null ? list.ContainerMeasurementId : list.MeasurementId;
            }
        }

        else {
            this.EntityPM.ChargesTypeCode = null;
            this.EntityPM.ChargesTypeName = null;
            this.EntityPM.ChargesGroupCode = null;
            this.EntityPM.DueTypeCode = null;
            this.EntityPM.DueTypeName = null;
            this.MeasurementId = null;
            this.PrepaidCollectId = null;
            this.CurrencyId = null;
            this.VatTypeId = null;
            this.EntityPM.IATACodeId = null;
            //this.EntityPM.IsBackToBack = false;
        }
    }

    get VatTypeId() { return this.EntityPM.VatTypeId; }
    set VatTypeId(value: string) {
        if (this.EntityPM.VatTypeId != value) {
            this.EntityPM.VatTypeId = value;
        }
    }

    get CurrencyId() { return this.EntityPM.CurrencyId; }
    set CurrencyId(newValue: string) {
        if (this.EntityPM.CurrencyId != newValue) {
            this.EntityPM.CurrencyId = newValue;
            this.SetUIProperties();

            if (newValue == null) {
                this.Rate = null;
                this.CurrencyCode = null;
            }

            else {

                if (SessionLocator.LocalCurrencyId == newValue) {
                    this.Rate = 1;
                    this.CurrencyCode = SessionLocator.LocalCurrencyCode;
                }

                else {
                    var myService = new CurrencyListService();
                    myService.getSingleFromCache(newValue).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var list: CurrencyList = myResponse.Result;
                            if (list != null) {
                                this.CurrencyCode = list.Code;
                            }
                        }
                    });
                }

                var todayDate: Date = DateTool.GetCurrentDateAsUtc();
                var myCurrencyRatesService = new CurrencyRatesService();
                myCurrencyRatesService.getAll(SessionLocator.LocalCurrencyId, todayDate).subscribe((myResponse2: ServiceResponse) => {
                    if (!myResponse2.HasError) {
                        this.fatherComponent.AllRates = myResponse2.Result;
                        this.SetLineRate();
                    }
                });
            }
        }
    }

    get CurrencyCode() { return this.EntityPM.CurrencyCode; }
    set CurrencyCode(value: string) {
        if (this.EntityPM.CurrencyCode != value) {
            this.EntityPM.CurrencyCode = value;
            this.ApplyByContainersFields();
            this.UpdateInsideItemsSource_Currency();
        }
    }
    SetLineRate() {
        if (AppTool.IsNullOrEmpty(this.CurrencyId)) {
            this.Rate = null;
        }

        else if (this.CurrencyId == SessionLocator.LocalCurrencyId) {
            this.Rate = 1;
        }

        else {
            var lastRate: LastRate = this.fatherComponent.AllRates.filter(d => d.ForeignCurrencyId == this.CurrencyId)[0];
            if (lastRate != null) {
                this.Rate = lastRate.Rate;
                this.RateDate = lastRate.ValueDate;
            }
        }
    }

    get MeasurementId() { return this.EntityPM.MeasurementId; }
    set MeasurementId(newValue: string) {
        if (this.EntityPM.MeasurementId != newValue) {
            this.EntityPM.MeasurementId = newValue;

            this.IsByContainerType = false;
            this.ByContainersItemsSource = [];

            if (newValue == null) {
                this.Quantity = null;
                this.EntityPM.MeasurementCode = null;
                this.EntityPM.MeasurementShortName = null;

                this.SetUIProperties();
                this.UpdateInsideItemsSource_Measurement();
            }

            else {
                var myService: MeasurementListService = new MeasurementListService();
                myService.getSingleFromCache(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: MeasurementList = myResponse.Result;
                        if (list != null) {
                            this.EntityPM.MeasurementCode = list.Code;
                            this.EntityPM.MeasurementShortName = list.ShortName;

                            switch (list.Code) {
                                case "GRWT": { this.Quantity = this.ShipmentPM.GrossWeight; break; }
                                case "CHWT": { this.Quantity = this.ShipmentPM.ChargeableWeight; break; }
                                case "VOLU": { this.Quantity = this.ShipmentPM.Volume; break; }
                                case "BTEU": { this.Quantity = this.ShipmentPM.TEU; break; }
                                case "FIXD": { this.Quantity = 1; break; }
                                case "GWTN": { this.Quantity = this.ShipmentPM.GrossWeightPerTon; break; }
                                case "QTY": { this.Quantity = this.fatherComponent.IsLCLEntity ? this.ShipmentPM.NumberOfPackages : this.ShipmentPM.NumberOfContainers; break; }
                                case "CWKG": { this.Quantity = this.ShipmentPM.ChargeableWeightInKG; break; }
                                case "GWKG": { this.Quantity = this.ShipmentPM.GrossWeightInKG; break; }
                                case "VCBM": { this.Quantity = this.ShipmentPM.VolumeInCBM; break; }
                                case "PRVL": {
                                    this.Quantity = this.ShipmentPM.ValueOfGoods;
                                    this.CurrencyId = this.ShipmentPM.ValueOfGoodsCurrencyId;
                                    break;
                                }

                                case "PRFR": {
                                    this.Quantity = ArrayTool.Sum(this.ShipmentPM.ShipmentReceivables.filter(d => d.ChargesGroupCode == "FRT" && AppTool.IsNullOrEmpty(d.ShipmentReceivableParentId)), "TotalAmount");
                                    this.CurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
                                    break;
                                }

                                case "BCNT": {
                                    this.IsByContainerType = true;
                                    this.BuildByContainersItemsSource();
                                    break;
                                }

                                case "SCGW": {
                                    this.Quantity = this.ShipmentPM.GrossWeightPerStorageDays;
                                    break;
                                }

                                case "PFCL": {
                                    this.Quantity = this.ShipmentPM.PercentForeignChargesLocal;
                                    break;
                                }

                                default: {
                                    var myGrouped: ByPckageType[] = ShipmentTool.GetByPckageTypeGrouped(this.ShipmentPM);
                                    var itemGrouped = myGrouped.filter(f => f.MeasurementId == this.MeasurementId)[0];
                                    if (itemGrouped != null) {
                                        this.Quantity = itemGrouped.Quantity;
                                    }

                                    break;
                                }
                            }

                            this.SetUIProperties();
                            this.UpdateInsideItemsSource_Measurement();
                        }
                    }
                });
            }
        }
    }
    get MeasurementCode() { return this.EntityPM.MeasurementCode; }
    get MeasurementShortName() { return this.EntityPM.MeasurementShortName; }

    // BCNT | ByContainers
    public IsByContainerType: boolean = false;
    ApplyByContainersFields() {
        if (this.ByContainersItemsSource) {
            this.ByContainersItemsSource.forEach(item => {
                item.PrepaidCollectId = this.PrepaidCollectId;
                item.CurrencyId = this.CurrencyId;
                item.CurrencyCode = this.CurrencyCode;
                item.Rate = this.Rate;
            });
        }
    }
    BuildByContainersItemsSource() {

        var byContainersItemsSource: ShipmentReceivablePM[] = [];

        var myGrouped: ByPckageType[] = ShipmentTool.GetByPckageTypeGrouped(this.ShipmentPM);

        myGrouped.forEach(item => {
            var newEntity = new ShipmentReceivablePM(null);
            newEntity.ShipmentId = this.ShipmentPM.Id;
            newEntity.ShipmentNumber = this.ShipmentPM.ShipmentNumber;
            newEntity.Tenant = this.ShipmentPM.Tenant;
            newEntity.ShipmentReceivableLineStatusCode = "EMPT";
            newEntity.ChargesTypeId = this.EntityPM.ChargesTypeId;
            newEntity.ChargesTypeCode = this.EntityPM.ChargesTypeCode;
            newEntity.ChargesTypeName = this.EntityPM.ChargesTypeName;
            newEntity.ChargesGroupCode = this.EntityPM.ChargesGroupCode;
            newEntity.DueTypeCode = this.EntityPM.DueTypeCode;
            newEntity.DueTypeName = this.EntityPM.DueTypeName;
            newEntity.IATACodeId = this.EntityPM.IATACodeId;
            newEntity.VatTypeId = this.EntityPM.VatTypeId;
            newEntity.PrepaidCollectId = this.EntityPM.PrepaidCollectId;
            newEntity.CurrencyId = this.EntityPM.CurrencyId;
            newEntity.CurrencyCode = this.EntityPM.CurrencyCode;
            newEntity.Rate = this.EntityPM.Rate;
            newEntity.ProfitCurrencyExchangeRate = this.EntityPM.ProfitCurrencyExchangeRate;
            newEntity.CreateDate = DateTool.GetCurrentDateAsUtc();
            newEntity.CreatedByUserId = SessionLocator.LoggedUserId;
            newEntity.UpdateDate = DateTool.GetCurrentDateAsUtc();
            newEntity.UpdateByUserId = SessionLocator.LoggedUserId;
            newEntity.Quantity = item.Quantity;
            newEntity.MeasurementId = item.MeasurementId;
            newEntity.MeasurementCode = item.MeasurementCode;
            newEntity.MeasurementShortName = item.MeasurementShortName;

            var exsistingEntity = byContainersItemsSource.filter(f => f.ChargesTypeId == newEntity.ChargesTypeId && f.MeasurementId == newEntity.MeasurementId)[0];
            if (exsistingEntity == null) {
                byContainersItemsSource.push(newEntity);
            }

            else {
                //var acctEntity = byContainersItemsSource.filter(f => f.ChargesTypeId == newEntity.ChargesTypeId && f.MeasurementId == newEntity.MeasurementId && (f.ShipmentReceivableLineStatusCode == "ACCT" || f.ShipmentReceivableLineStatusCode == "DRFT"))[0];
                //var openEntity = byContainersItemsSource.filter(f => f.ChargesTypeId == newEntity.ChargesTypeId && f.MeasurementId == newEntity.MeasurementId && (f.ShipmentReceivableLineStatusCode == "EMPT" || f.ShipmentReceivableLineStatusCode == "OAMT"))[0];
                var acctEntity = byContainersItemsSource.filter(f => f.ChargesTypeId == newEntity.ChargesTypeId && f.MeasurementId == newEntity.MeasurementId && f.ShipmentReceivableLineStatusCode == "ACCT")[0];
                var openEntity = byContainersItemsSource.filter(f => f.ChargesTypeId == newEntity.ChargesTypeId && f.MeasurementId == newEntity.MeasurementId && f.ShipmentReceivableLineStatusCode != "ACCT")[0];

                if (acctEntity == null) {
                    openEntity.Quantity = newEntity.Quantity;
                }

                else if (newEntity.Quantity > acctEntity.Quantity) {
                    if (openEntity == null) {
                        byContainersItemsSource.push(newEntity);
                    }

                    else {
                        openEntity.Quantity = newEntity.Quantity;
                    }
                }
            }
        });

        this.ByContainersItemsSource = byContainersItemsSource;
    }

    SetPrepaidCollectId() {
        if (AppTool.IsNullOrEmpty(this.PrepaidCollectId)) {
            if (this.EntityPM.ChargesGroupCode == "FRT") {
                this.PrepaidCollectId = this.ShipmentPM.FreightPrepaidCollectId;
            }

            else if (this.EntityPM.ChargesGroupCode != "FRT") {
                this.PrepaidCollectId = this.ShipmentPM.OtherPrepaidCollectId;
            }
        }
    }
    get PrepaidCollectId() { return this.EntityPM.PrepaidCollectId; }
    set PrepaidCollectId(newValue: string) {
        if (this.EntityPM.PrepaidCollectId != newValue) {
            this.EntityPM.PrepaidCollectId = newValue;
            this.ApplyByContainersFields();
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newVaule: string) {
        if (this.EntityPM.Notes != newVaule) {
            this.EntityPM.Notes = newVaule;
        }
    }

    // Amounts
    public RelativeRateDate: string = null;
    private rateDate: Date = null;
    get RateDate() { return this.rateDate; }
    set RateDate(value: Date) {
        if (this.rateDate != value) {
            this.rateDate = value;
            this.RelativeRateDate = DateTool.GetRelativeRateDate(DateTool.GetCurrentDateAsUtc(), value, "ago");
        }
    }

    get Rate() { return this.EntityPM.Rate; }
    set Rate(newVaule: number) {
        if (this.EntityPM.Rate != newVaule) {
            this.EntityPM.Rate = AppTool.Round(newVaule, 5);
            this.ComputeTotalAmountLocal();
            this.ApplyByContainersFields();
            this.UpdateInsideItemsSource_Rate();
        }
    }

    get IsExchangeRateFixed() { return this.EntityPM.IsExchangeRateFixed; }
    set IsExchangeRateFixed(newVaule: boolean) {
        if (this.EntityPM.IsExchangeRateFixed != newVaule) {
            this.EntityPM.IsExchangeRateFixed = newVaule;
        }
    }

    get Quantity() { return this.EntityPM.Quantity; }
    set Quantity(newVaule: number) {
        if (this.EntityPM.Quantity != newVaule) {
            this.EntityPM.Quantity = AppTool.Round(newVaule, 3);

            if (this.EntityPM.IsChargeBySteps) {
                ShipmentTool.SetReceivableUnitPriceBySteps(this.EntityPM, this.fatherComponent.BaseQuote);
            }

            this.ComputeTotalAmount();
        }
    }

    get UnitPrice() { return this.EntityPM.UnitPrice; }
    set UnitPrice(newVaule: number) {
        if (this.EntityPM.UnitPrice != newVaule) {
            this.EntityPM.UnitPrice = AppTool.Round(newVaule, 3);
            this.ComputeTotalAmount();
        }
    }

    get QuoteSaleMinAmount() { return this.EntityPM.QuoteSaleMinAmount; }
    set QuoteSaleMinAmount(value: number) {
        if (this.EntityPM.QuoteSaleMinAmount != value) {
            this.EntityPM.QuoteSaleMinAmount = AppTool.Round(value, 2);
        }
    }

    get QuoteSaleMaxAmount() { return this.EntityPM.QuoteSaleMaxAmount; }
    set QuoteSaleMaxAmount(value: number) {
        if (this.EntityPM.QuoteSaleMaxAmount != value) {
            this.EntityPM.QuoteSaleMaxAmount = AppTool.Round(value, 2);
        }
    }

    get TotalAmount() { return this.EntityPM.TotalAmount; }
    set TotalAmount(ivalue: number) {

        var value = ivalue;

        if (value) {
            if (this.QuoteSaleMinAmount != null) {
                if (value < this.QuoteSaleMinAmount) {
                    value = this.QuoteSaleMinAmount;
                }
            }

            if (this.QuoteSaleMaxAmount != null) {
                if (value > this.QuoteSaleMaxAmount) {
                    value = this.QuoteSaleMaxAmount;
                }
            }
        }

        if (this.EntityPM.TotalAmount != value) {

            this.EntityPM.TotalAmount = AppTool.Round(value, 2);

            this.SetLineStatus();
            this.ComputeUnitPrice(value);
            this.ComputeTotalAmountLocal();
            this.SetMinMaxFromQuoteIconVisibility();
            this.OnLineAmountChanged();
        }
    }

    get TotalAmountLocal() { return this.EntityPM.TotalAmountLocal; }
    set TotalAmountLocal(newVaule: number) {
        if (this.EntityPM.TotalAmountLocal != newVaule) {
            this.EntityPM.TotalAmountLocal = AppTool.Round(newVaule, 2);
            this.SetLineSummary();

            this.fatherComponent.ItemsSource.Collection.filter(f => f.ChargesTypeId == this.ChargesTypeId).forEach(item => {
                item.SetLineSummary();
            });

            this.ComputeInsideReceivablesData();
        }
    }

    get AmountInProfitCurrency() { return this.EntityPM.AmountInProfitCurrency; }
    set AmountInProfitCurrency(newVaule: number) {
        if (this.EntityPM.AmountInProfitCurrency != newVaule) {
            this.EntityPM.AmountInProfitCurrency = AppTool.Round(newVaule, 2);
        }
    }

    get ProfitCurrencyExchangeRate() { return this.EntityPM.ProfitCurrencyExchangeRate; }
    set ProfitCurrencyExchangeRate(newVaule: number) {
        if (this.EntityPM.ProfitCurrencyExchangeRate != newVaule) {
            this.EntityPM.ProfitCurrencyExchangeRate = AppTool.Round(newVaule, 5);
            this.ComputeTotalAmountInProfitCurrency();
        }
    }

    SetLineStatus() {
        ShipmentTool.SetReceivableLineStatus(this.EntityPM);
    }
    ComputeUnitPrice(myTotalAmount: number) {
        if (!this.EntityPM.IsFixedPrice) {

            var myResult: number = null;

            if (this.EntityPM.IsChargeBySteps) {

            }

            else {
                if (myTotalAmount != null && this.EntityPM.Quantity != null) {
                    if (this.EntityPM.Quantity == 0) {
                        myResult = 0;
                    }

                    else {
                        myResult = myTotalAmount / this.EntityPM.Quantity;
                    }
                }

                this.EntityPM.UnitPrice = AppTool.Round(myResult, 3);
            }
        }
    }
    ComputeTotalAmount() {

        this.SetLineStatus();

        if (!this.EntityPM.IsFixedPrice) {

            var iAmount: number = null;

            if (this.EntityPM.Quantity != null && this.EntityPM.UnitPrice != null) {
                if (this.MeasurementCode == "PRVL" || this.MeasurementCode == "PRFR") {
                    var price = this.EntityPM.UnitPrice / 100;
                    iAmount = this.EntityPM.Quantity * price;
                }

                else {
                    iAmount = this.EntityPM.Quantity * this.EntityPM.UnitPrice;
                }
            }

            /* MinMax */
            if (iAmount != null) {
                if (this.QuoteSaleMinAmount != null) {
                    if (iAmount < this.QuoteSaleMinAmount) {
                        iAmount = this.QuoteSaleMinAmount;
                    }
                }

                if (this.QuoteSaleMaxAmount != null) {
                    if (iAmount > this.QuoteSaleMaxAmount) {
                        iAmount = this.QuoteSaleMaxAmount;
                    }
                }
            }

            this.EntityPM.TotalAmount = AppTool.Round(iAmount, 2);
            this.ComputeTotalAmountLocal();
            this.SetMinMaxFromQuoteIconVisibility();
            this.OnLineAmountChanged();
        }
    }
    ComputeTotalAmountLocal() {

        var myResult: number = null;

        if (this.EntityPM.TotalAmount != null && this.EntityPM.Rate != null) {
            myResult = AppTool.Round(this.EntityPM.TotalAmount * this.EntityPM.Rate, 2);
        }

        this.TotalAmountLocal = myResult;
        this.ComputeTotalAmountInProfitCurrency();
        this.ComputeInsideReceivablesData();
    }
    ComputeTotalAmountInProfitCurrency() {

        if (this.EntityPM.CurrencyId == this.ShipmentPM.ProfitCurrencyId) {
            this.AmountInProfitCurrency = this.EntityPM.TotalAmount;
        }

        else {
            this.AmountInProfitCurrency = (this.TotalAmountLocal / this.EntityPM.ProfitCurrencyExchangeRate);
        }

        if (this.fatherComponent != null) {
            this.fatherComponent.ComputeShipmentFields();
        }
    }
    UpdateCurrencyRateClicked() {

        var loadingDate = DateTool.GetCurrentDateAsUtc();

        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 350;
        logWindow.Title = "Update Currency Rate";
        logWindow.WindowArgs = { CurrencyId: this.EntityPM.CurrencyId, CurrencyCode: this.EntityPM.CurrencyCode, Rate: this.EntityPM.Rate, Date: loadingDate };
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.fatherComponent.AllRates = comp.RatesList;
                    this.Rate = AppTool.Round(comp.Rate, 5);
                    this.RateDate = comp.RateDate;
                }
            });
        });
        logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
    }

    // Line Summary
    private myUserListService: UserListService = null;
    get CreateDate() { return this.EntityPM.CreateDate; }
    get UpdateDate() { return this.EntityPM.UpdateDate; }
    public CreatedByUserName: string = null;
    public UpdatedByUserName: string = null;
    public ReceivableSummary: number = null;
    public PayableSummary: number = null;
    public ProfitSummary: number = null;
    SetLineSummary() {

        if (this.myUserListService == null) {
            this.myUserListService = new UserListService();

            this.myUserListService.getSingleFromCache(this.EntityPM.CreatedByUserId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: UserList = myResponse.Result;
                    if (list) {
                        this.CreatedByUserName = list.EnglishName;
                    }

                    else {
                        this.myUserListService.getSingle(this.EntityPM.CreatedByUserId).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                var list: UserList = myResponse.Result;
                                if (list) {
                                    this.CreatedByUserName = list.EnglishName;
                                }
                            }
                        });
                    }
                }
            });

            if (this.EntityPM.UpdateByUserId == this.EntityPM.CreatedByUserId) {
                this.UpdatedByUserName = this.CreatedByUserName;
            }

            else {
                this.myUserListService.getSingleFromCache(this.EntityPM.UpdateByUserId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: UserList = myResponse.Result;
                        if (list) {
                            this.UpdatedByUserName = list.EnglishName;
                        }

                        else {
                            this.myUserListService.getSingle(this.EntityPM.UpdateByUserId).subscribe((myResponse: ServiceResponse) => {
                                if (!myResponse.HasError) {
                                    var list: UserList = myResponse.Result;
                                    if (list) {
                                        this.UpdatedByUserName = list.EnglishName;
                                    }
                                }
                            });
                        }
                    }
                });
            }
        }

        var myReceivableSummary = 0;
        var myPayableSummary = 0;
        var myProfitSummary = 0;

        if (this.ShipmentPM.ShipmentReceivables.length > 0) {
            myReceivableSummary = ArrayTool.Sum(this.ShipmentPM.ShipmentReceivables.filter(f => f.ChargesTypeId == this.ChargesTypeId), "TotalAmountLocal");
        }

        if (!ArrayTool.Contains(this.ShipmentPM.ShipmentReceivables, this.EntityPM)) {
            if (!AppTool.IsNullOrEmpty(this.TotalAmountLocal)) {
                myReceivableSummary += this.TotalAmountLocal;
            }
        }

        if (this.ShipmentPM.ShipmentPayables.length > 0) {
            var openAmount = ArrayTool.Sum(this.ShipmentPM.ShipmentPayables.filter(f => f.ChargesTypeId == this.ChargesTypeId), "OpenAmountInLocalCurrency");
            var acctAmount = ArrayTool.Sum(this.ShipmentPM.ShipmentPayables.filter(f => f.ChargesTypeId == this.ChargesTypeId), "AccountedAmountInLocalCurrency");
            myPayableSummary = openAmount + acctAmount;
        }

        this.ReceivableSummary = myReceivableSummary;
        this.PayableSummary = myPayableSummary;
        this.ProfitSummary = this.ReceivableSummary - this.PayableSummary;
    }

    // Invoice
    public InvoiceNumber: string = null;
    public InvoiceStatus: string = null;
    get ARInvoiceId() { return this.EntityPM.ARInvoiceId; }
    SetInvoiceData() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.ARInvoiceId)) {
            var shipmentARInvoicePM = this.ShipmentPM.ShipmentARInvoices.filter(d => d.Id === this.EntityPM.ARInvoiceId)[0];
            if (shipmentARInvoicePM != null) {
                this.InvoiceNumber = shipmentARInvoicePM.InvoiceNumber;
                this.InvoiceStatus = shipmentARInvoicePM.StatusName;
            }
        }
    }

    // Child Receivables
    public SumOfQuantity: number;
    public SumOfAmount: number;
    public SumOfAmountLocal: number;
    BuildInsideReceivables() {
        this.InsideItemsSource = [];

        this.EntityPM.ChildShipmentReceivables.forEach(item => {
            var myConsoleShipmentPM: ConsoleShipmentPM = this.ShipmentPM.ShipmentConsoleShipments.filter(f => f.Id == item.ShipmentId)[0];
            var insideReceivable = new InsideReceivableViewModel(item, myConsoleShipmentPM, this.ShipmentPM.ProfitCurrencyId)
            this.InsideItemsSource.push(insideReceivable);
        });

        this.ComputeInsideReceivablesTotals();
    }
    UpdateInsideItemsSource_Rate() {
        if (this.ShipmentPM.ShipmentLevelCode == "C") {
            this.InsideItemsSource.forEach(insidePayable => {
                insidePayable.Rate = this.Rate;
            });
        }
    }
    UpdateInsideItemsSource_Currency() {
        if (this.ShipmentPM.ShipmentLevelCode == "C") {
            this.InsideItemsSource.forEach(insidePayable => {
                insidePayable.CurrencyId = this.CurrencyId;
                insidePayable.CurrencyCode = this.CurrencyCode;
            });
        }
    }
    UpdateInsideItemsSource_Measurement() {
        if (this.ShipmentPM.ShipmentLevelCode == "C") {
            this.InsideItemsSource.forEach(insidePayable => {
                insidePayable.MeasurementId = this.MeasurementId;
                insidePayable.MeasurementCode = this.MeasurementCode;
                insidePayable.MeasurementShortName = this.MeasurementShortName;
                insidePayable.OnMeasurementChanged();
            });
        }
    }
    ComputeInsideReceivablesData() {
        if (this.ShipmentPM.ShipmentLevelCode == "C") {
            var myService = new PackageTypeListService();
            myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var AllPackageTypes: PackageTypeList[] = myResponse.Result;
                    if (AllPackageTypes == null) {
                        AllPackageTypes = [];
                    }

                    var _QuantityTotal = null;
                    var _Ratio = null;
                    var quantity = null;
                    var unitPrice = null;

                    this.InsideItemsSource.forEach(item => {
                        switch (this.MeasurementCode) {
                            case "VCBM": {
                                _QuantityTotal = ArrayTool.Sum(this.InsideItemsSource, "VolumeInCBM");
                                _Ratio = _QuantityTotal == 0 ? 0 : this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * this.UnitPrice;
                                quantity = item.VolumeInCBM;
                                break;
                            }

                            case "VOLU": {
                                _QuantityTotal = ArrayTool.Sum(this.InsideItemsSource, "Volume");
                                _Ratio = _QuantityTotal == 0 ? 0 : this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * this.UnitPrice;
                                quantity = item.Volume;
                                break;
                            }

                            case "GRWT": {
                                _QuantityTotal = ArrayTool.Sum(this.InsideItemsSource, "GrossWeight");
                                _Ratio = _QuantityTotal == 0 ? 0 : this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * this.UnitPrice;
                                quantity = item.GrossWeight;
                                break;
                            }

                            case "CWKG": {
                                _QuantityTotal = ArrayTool.Sum(this.InsideItemsSource, "ChargeableWeightInKG");
                                _Ratio = _QuantityTotal == 0 ? 0 : this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * this.UnitPrice;
                                quantity = item.ChargeableWeightInKG;
                                break;
                            }
                            case "GWKG": {
                                _QuantityTotal = ArrayTool.Sum(this.InsideItemsSource, "GrossWeightInKG");
                                _Ratio = _QuantityTotal == 0 ? 0 : this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * this.UnitPrice;
                                quantity = item.GrossWeightInKG;
                                break;
                            }

                            case "GWTN": {
                                _QuantityTotal = ArrayTool.Sum(this.InsideItemsSource, "GrossWeightPerTon");
                                _Ratio = _QuantityTotal == 0 ? 0 : this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * this.UnitPrice;
                                quantity = item.GrossWeightPerTon;
                                break;
                            }

                            case "QTY": {
                                if (this.fatherComponent.IsLCLEntity) {
                                    _QuantityTotal = ArrayTool.Sum(this.InsideItemsSource, "NumberOfPackages");
                                    _Ratio = _QuantityTotal == 0 ? 0 : this.Quantity / _QuantityTotal;
                                    unitPrice = _Ratio * this.UnitPrice;
                                    quantity = item.NumberOfPackages;
                                }

                                else {
                                    _QuantityTotal = ArrayTool.Sum(this.InsideItemsSource, "NumberOfContainers");
                                    _Ratio = _QuantityTotal == 0 ? 0 : this.Quantity / _QuantityTotal;
                                    unitPrice = _Ratio * this.UnitPrice;
                                    quantity = item.NumberOfContainers;
                                }

                                break;
                            }

                            case "CHWT": {
                                _QuantityTotal = ArrayTool.Sum(this.InsideItemsSource, "ChargeableWeight");
                                _Ratio = _QuantityTotal == 0 ? 0 : this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * this.UnitPrice;
                                quantity = item.ChargeableWeight;
                                break;
                            }

                            case "BTEU": {
                                _QuantityTotal = ArrayTool.Sum(this.InsideItemsSource, "TEU");
                                _Ratio = _QuantityTotal == 0 ? 0 : this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * this.UnitPrice;
                                quantity = item.TEU;
                                break;
                            }

                            case "FIXD": {
                                _Ratio = this.InsideItemsSource.length == 0 ? 0 : this.Quantity / this.InsideItemsSource.length;
                                unitPrice = _Ratio * this.UnitPrice;
                                quantity = 1;
                                break;
                            }

                            case "PRVL": {
                                unitPrice = this.UnitPrice;
                                quantity = item.ValueOfGoods;
                                break;
                            }

                            case "PRFR": {
                                unitPrice = this.UnitPrice;
                                quantity = item.FreightPayablesAmount;
                                break;
                            }

                            case "SCGW": {
                                unitPrice = this.UnitPrice;
                                quantity = item.GrossWeightPerStorageDays;
                                break;
                            }

                            case "PFCL": {
                                unitPrice = this.UnitPrice;
                                quantity = item.PercentForeignChargesLocal;
                                break;
                            }

                            default: {
                                if (this.fatherComponent.IsFCLEntity) {
                                    var list: PackageTypeList = AllPackageTypes.filter(f => f.MeasurementId == this.MeasurementId)[0];
                                    if (list) {
                                        var houseRecord: ConsoleShipmentPM = item.ShipmentPM;
                                        if (houseRecord) {
                                            var fclData = houseRecord.FCLDataList.filter(f => f.Id == list.Id)[0];
                                            if (fclData) {
                                                quantity = fclData.Quantity;
                                            }
                                        }

                                        unitPrice = this.UnitPrice;
                                    }
                                }

                                else {
                                    // Groupage by Chargeable
                                    _QuantityTotal = ArrayTool.Sum(this.InsideItemsSource, "ChargeableWeight");
                                    _Ratio = _QuantityTotal == 0 ? 0 : this.Quantity / _QuantityTotal;
                                    unitPrice = _Ratio * this.UnitPrice;
                                    quantity = item.ChargeableWeight;
                                }

                                break;
                            }
                        }

                        if (AppTool.IsNullOrEmpty(unitPrice)) {
                            unitPrice = 0;
                        }

                        if (AppTool.IsNullOrEmpty(quantity)) {
                            quantity = 0;
                        }

                        item.EntityPM.UnitPrice = AppTool.Round(unitPrice, 3);
                        item.EntityPM.Quantity = AppTool.Round(quantity, 3);

                        var totalAmount = quantity * unitPrice;

                        if (this.MeasurementCode == "PRVL" || this.MeasurementCode == "PRFR") {
                            totalAmount = quantity * unitPrice / 100;
                        }

                        var totalAmountLocal = totalAmount * this.Rate;
                        var totalAmountInProfitCurrency = totalAmountLocal / this.ProfitCurrencyExchangeRate;

                        item.EntityPM.TotalAmount = AppTool.Round(totalAmount, 3);
                        item.EntityPM.TotalAmountLocal = AppTool.Round(totalAmountLocal, 3);
                        item.EntityPM.AmountInProfitCurrency = AppTool.Round(totalAmountInProfitCurrency, 3);
                    });

                    this.ComputeInsideReceivablesTotals();
                }
            });
        }
    }
    ComputeInsideReceivablesTotals() {
        this.SumOfQuantity = ArrayTool.Sum(this.InsideItemsSource, "Quantity");
        this.SumOfAmount = ArrayTool.Sum(this.InsideItemsSource, "TotalAmount");
        this.SumOfAmountLocal = ArrayTool.Sum(this.InsideItemsSource, "TotalAmountLocal");
        this.SetExpectedAmountCell();
    }

    public ExpectedAmountColor: string = null;
    SetExpectedAmountCell() {

        var myColor = FontTool.Black;

        if (this.InsideItemsSource) {
            var sumOfAmount = ArrayTool.Sum(this.InsideItemsSource, "TotalAmount");
            var sumOfAmount = AppTool.Round(sumOfAmount, 2);

            if ((sumOfAmount < 0 || sumOfAmount > 0) && !AppTool.IsNullOrEmpty(this.TotalAmount) && sumOfAmount != this.TotalAmount) {
                myColor = FontTool.Red;
            }

            else if (this.EntityPM.IsChargeBySteps) {
                myColor = FontTool.Gray;
            }

            else if (this.EntityPM.ShipmentReceivableLineStatusCode == "ACCT" || this.EntityPM.ShipmentReceivableLineStatusCode == "DRFT") {
                myColor = FontTool.Gray;
            }

            else if (!AppTool.IsNullOrEmpty(this.EntityPM.ShipmentReceivableParentId)) {
                myColor = FontTool.Gray;
            }
        }

        this.ExpectedAmountColor = myColor;
    }

    SetQuantity() {
        var result = null;

        switch (this.EntityPM.MeasurementCode) {
            case "GRWT": { result = this.ShipmentPM.GrossWeight; break; }
            case "CHWT": { result = this.ShipmentPM.ChargeableWeight; break; }
            case "VOLU": { result = this.ShipmentPM.Volume; break; }
            case "BTEU": { result = this.ShipmentPM.TEU; break; }
            case "FIXD": { result = 1; break; }
            case "PRVL": { result = this.ShipmentPM.ValueOfGoods; break; }
            case "PRFR": { result = ArrayTool.Sum(this.ShipmentPM.ShipmentReceivables.filter(d => d.ChargesGroupCode == "FRT" && AppTool.IsNullOrEmpty(d.ShipmentReceivableParentId)), "TotalAmount"); break; }
            case "GWTN": { result = this.ShipmentPM.GrossWeightPerTon; break; }
            case "QTY": { result = this.fatherComponent.IsLCLEntity ? this.ShipmentPM.NumberOfPackages : this.ShipmentPM.NumberOfContainers; break; }
            case "CWKG": { result = this.ShipmentPM.ChargeableWeightInKG; break; }
            case "GWKG": { result = this.ShipmentPM.GrossWeightInKG; break; }
            case "VCBM": { result = this.ShipmentPM.VolumeInCBM; break; }
            case "BCNT": {
                break;
            }
            case "SCGW": {
                result = this.ShipmentPM.GrossWeightPerStorageDays; break;
            }

            case "PFCL": {
                result = this.ShipmentPM.PercentForeignChargesLocal; break;
            }

            default: {
                if (!AppTool.IsNullOrEmpty(this.MeasurementId)) {
                    var allBCNTGrouped: ByPckageType[] = ShipmentTool.GetByPckageTypeGrouped(this.ShipmentPM);

                    var itemGrouped = allBCNTGrouped.filter(f => f.MeasurementId == this.MeasurementId)[0];
                    if (itemGrouped != null) {
                        result = itemGrouped.Quantity;
                    }
                }

                break;
            }
        }

        this.Quantity = result;
    }
    OnLineAmountChanged() {
        if (this.ChargesGroupCode == "FRT") {
            this.fatherComponent.OnFreightAmountChanged();
        }
    }

    StoragePricingClicked() {
        var entityResourceService: EntityResourceService = new EntityResourceService();
        entityResourceService.getEntityResourceByTableName("ShipmentStoragePricing").subscribe((res1: any) => {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = "Storage Pricing";
            logitudeWindow.WindowArgs = { EntityPM: this.ShipmentPM, ObjectTableName: this.fatherComponent.ObjectTableName };
            logitudeWindow.Show("./ShipmentModules/ShipmentRouting/Components/Routings/WarehouseStoragePricingComponent");
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s.indexOf('+') > -1) {
                    this.UpdateAmount();
                    this.UpdateCurrency();
                }

                else {
                    if (s == "PricesChanged") {
                        this.UpdateAmount();
                    }

                    else if (s == "CurrencyChanged") {
                        this.UpdateCurrency();
                    }
                }

                this.UpdateReceivable();
                this.SetUIProperties_AmountProfit();
            });
        });
    }
    private ComputeReceivableAmount(): number {
        var storageDays: number;
        if (DateTool.GetDateFromDate(this.ShipmentPM.WarehouseLegActualReleaseDate) >= DateTool.GetDateFromDate(this.ShipmentPM.WarehouseLegActualEntryDate)) {
            storageDays = DateTool.GetDaysBetweenDates(this.ShipmentPM.WarehouseLegActualEntryDate, this.ShipmentPM.WarehouseLegActualReleaseDate);
        }

        var myResult: number = ShipmentTool.ComputeImportStorageReceivableAmount(storageDays, this.ShipmentPM);

        return myResult;
    }
    private UpdateAmount() {
        var amount: number = this.ComputeReceivableAmount();
        this.EntityPM.TotalAmount = amount;
    }
    private UpdateCurrency() {
        this.EntityPM.CurrencyId = this.ShipmentPM.ChargeStorageCurrencyId;
        this.EntityPM.CurrencyCode = this.ShipmentPM.ChargeStorageCurrencyCode;

        if (SessionLocator.LocalCurrencyId == this.EntityPM.CurrencyId) {
            this.EntityPM.Rate = 1;
        }
        else {
            var lastRate: LastRate = this.fatherComponent.AllRates.filter(d => d.ForeignCurrencyId == this.EntityPM.CurrencyId)[0];
            if (lastRate != null) {
                this.EntityPM.Rate = lastRate.Rate;
            }
        }

        if (this.ShipmentPM.ProfitCurrencyId == SessionLocator.TenantPM.CurrencyId) {
            this.EntityPM.ProfitCurrencyExchangeRate = 1;
        }

        else {
            var myLastRate: LastRate = this.fatherComponent.AllRates.filter(d => d.ForeignCurrencyId == this.ShipmentPM.ProfitCurrencyId)[0];
            if (myLastRate != null) {
                this.EntityPM.ProfitCurrencyExchangeRate = myLastRate.Rate;
            }
        }
    }
    private UpdateReceivable() {
        this.EntityPM.TotalAmountLocal = AppTool.Round(this.EntityPM.TotalAmount * this.EntityPM.Rate, 2);

        if (this.EntityPM.CurrencyId == this.ShipmentPM.ProfitCurrencyId) {
            this.EntityPM.AmountInProfitCurrency = this.EntityPM.TotalAmount;
        }

        else {
            this.EntityPM.AmountInProfitCurrency = (this.EntityPM.TotalAmountLocal / this.EntityPM.ProfitCurrencyExchangeRate);
        }
    }
}
export class InsideReceivableViewModel {
    public EntityPM: ShipmentReceivablePM;
    public ShipmentPM: ConsoleShipmentPM;
    public ProfitCurrencyId: string;
    constructor(entityPM: ShipmentReceivablePM, myConsoleShipmentPM: ConsoleShipmentPM, profitCurrencyId: string) {
        this.EntityPM = entityPM;
        this.ShipmentPM = myConsoleShipmentPM;
        this.ProfitCurrencyId = profitCurrencyId;
        this.SetSatusTypeToolTip();
    }

    public SatusTypeToolTip: string = null;
    SetSatusTypeToolTip() {
        var myResult: string;

        switch (this.EntityPM.ShipmentReceivableLineStatusCode) {
            case "APPD": { myResult = TextCodeTranslator.Translate("Shipment.O.Receivables.Approved"); break; }
            case "OAMT": { myResult = TextCodeTranslator.Translate("Shipment.O.Receivables.OpenAmount"); break; }
            case "ACCT": { myResult = TextCodeTranslator.Translate("Shipment.O.Receivables.Accounted"); break; }
            case "DRFT": { myResult = TextCodeTranslator.Translate("Shipment.O.Receivables.Draft"); break; }
            default: { break; }
        }

        this.SatusTypeToolTip = myResult;
    }

    // Shipment Properties
    get TEU() { return this.ShipmentPM.TEU; }
    get Volume() { return this.ShipmentPM.Volume; }
    get GrossWeight() { return this.ShipmentPM.GrossWeight; }
    get ChargeableWeight() { return this.ShipmentPM.ChargeableWeight; }
    get GrossWeightPerTon() { return this.ShipmentPM.GrossWeightPerTon; }
    get ValueOfGoods() { return this.ShipmentPM.ValueOfGoods; }
    get FreightPayablesAmount() { return this.ShipmentPM.FreightPayablesAmount; }
    get FreightReceivablesAmount() { return this.ShipmentPM.FreightReceivablesAmount; }
    get NumberOfPackages() { return this.ShipmentPM.NumberOfPackages; }
    get NumberOfContainers() { return this.ShipmentPM.NumberOfContainers; }
    get ChargeableWeightInKG() { return this.ShipmentPM.ChargeableWeightInKG; }
    get GrossWeightInKG() { return this.ShipmentPM.GrossWeightInKG; }
    get VolumeInCBM() { return this.ShipmentPM.VolumeInCBM; }
    get GrossWeightPerStorageDays() { return this.ShipmentPM.GrossWeightPerStorageDays; }
    get PercentForeignChargesLocal() { return this.ShipmentPM.PercentForeignChargesLocal; }

    // Receivable Properties
    get ShipmentId() { return this.EntityPM.ShipmentId; }
    get ShipmentNumber() { return this.EntityPM.ShipmentNumber; }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newVaule: string) {
        if (this.EntityPM.Notes != newVaule) {
            this.EntityPM.Notes = newVaule;
        }
    }

    get MeasurementId() { return this.EntityPM.MeasurementId; }
    set MeasurementId(value: string) {
        if (this.EntityPM.MeasurementId != value) {
            this.EntityPM.MeasurementId = value;
        }
    }

    get MeasurementCode() { return this.EntityPM.MeasurementCode; }
    set MeasurementCode(value: string) {
        if (this.EntityPM.MeasurementCode != value) {
            this.EntityPM.MeasurementCode = value;
        }
    }

    get MeasurementShortName() { return this.EntityPM.MeasurementShortName; }
    set MeasurementShortName(value: string) {
        if (this.EntityPM.MeasurementShortName != value) {
            this.EntityPM.MeasurementShortName = value;
        }
    }

    get CurrencyId() { return this.EntityPM.CurrencyId; }
    set CurrencyId(value: string) {
        if (this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;
        }
    }

    get CurrencyCode() { return this.EntityPM.CurrencyCode; }
    set CurrencyCode(value: string) {
        if (this.EntityPM.CurrencyCode != value) {
            this.EntityPM.CurrencyCode = value;
        }
    }

    get Rate() { return this.EntityPM.Rate; }
    set Rate(value: number) {
        if (this.EntityPM.Rate != value) {
            this.EntityPM.Rate = AppTool.Round(value, 5);
            this.ComputeTotalAmountLocal();
        }
    }

    OnMeasurementChanged() {
        var myQuantity = null;

        switch (this.MeasurementCode) {

            case "SCGW": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.GrossWeightPerStorageDays;
                }

                break;
            }

            case "PFCL": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.PercentForeignChargesLocal;
                }
                break;
            }

            case "CWKG": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.ChargeableWeightInKG;
                }

                break;
            }

            case "GWKG": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.GrossWeightInKG;
                }

                break;
            }

            case "VCBM": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.VolumeInCBM;
                }

                break;
            }

            case "GRWT": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.GrossWeight;
                }

                break;
            }

            case "GWTN": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.GrossWeightPerTon;
                }

                break;
            }

            case "QTY": {
                if (this.ShipmentPM) {
                    if (this.ShipmentPM.IsLCL) {
                        myQuantity = this.ShipmentPM.NumberOfPackages;
                    }

                    else {
                        myQuantity = this.ShipmentPM.NumberOfContainers;
                    }
                }

                break;
            }

            case "CHWT": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.ChargeableWeight;
                }

                break;
            }

            case "VOLU": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.Volume;
                }

                break;
            }

            case "BTEU": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.TEU;
                }

                break;
            }

            case "PRVL": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.ValueOfGoods;
                }

                break;
            }

            case "PRFR": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.FreightReceivablesAmount;
                }

                break;
            }

            default: {
                myQuantity = 1;
                break;
            }
        }

        this.Quantity = myQuantity;
    }

    get Quantity() { return this.EntityPM.Quantity; }
    set Quantity(value: number) {
        if (this.EntityPM.Quantity != value) {
            this.EntityPM.Quantity = AppTool.Round(value, 2);
            this.ComputeTotalAmount();
        }
    }

    get UnitPrice() { return this.EntityPM.UnitPrice; }
    set UnitPrice(value: number) {
        if (this.EntityPM.UnitPrice != value) {
            this.EntityPM.UnitPrice = AppTool.Round(value, 3);
            this.ComputeTotalAmount();
        }
    }

    get TotalAmount() { return this.EntityPM.TotalAmount; }
    set TotalAmount(value: number) {
        if (this.EntityPM.TotalAmount != value) {
            this.EntityPM.TotalAmount = AppTool.Round(value, 2);
            this.ComputeUnitPrice();
            this.ComputeTotalAmountLocal();
        }
    }

    get TotalAmountLocal() { return this.EntityPM.TotalAmountLocal; }
    set TotalAmountLocal(value: number) {
        if (this.EntityPM.TotalAmountLocal != value) {
            this.EntityPM.TotalAmountLocal = AppTool.Round(value, 2);
            this.ComputeTotalAmountInProfitCurrency();
        }
    }

    SetLineStatus() {
        ShipmentTool.SetReceivableLineStatus(this.EntityPM);
        this.SetSatusTypeToolTip();
    }
    ComputeUnitPrice() {
        if (!this.EntityPM.IsChargeBySteps) {

            var myResult: number = null;

            if (this.EntityPM.IsChargeBySteps) {

            }

            else {
                if (this.EntityPM.TotalAmount != null && this.EntityPM.Quantity != null) {
                    if (this.EntityPM.Quantity == 0) {
                        myResult = 0;
                    }

                    else {
                        myResult = this.EntityPM.TotalAmount / this.EntityPM.Quantity;
                    }
                }

                this.EntityPM.UnitPrice = AppTool.Round(myResult, 3);
            }
        }
    }
    ComputeTotalAmount() {

        var myResult: number = null;

        this.SetLineStatus();

        if (this.Quantity != null && this.UnitPrice != null) {
            myResult = this.Quantity * this.UnitPrice;

            if (this.MeasurementCode == "PRVL" || this.MeasurementCode == "PRFR") {
                myResult = this.Quantity * this.UnitPrice / 100;
            }
        }

        this.EntityPM.TotalAmount = AppTool.Round(myResult, 2);
        this.ComputeTotalAmountLocal();
    }
    ComputeTotalAmountLocal() {

        var myResult: number = null;

        if (this.EntityPM.TotalAmount != null && this.EntityPM.Rate != null) {
            myResult = AppTool.Round(this.EntityPM.TotalAmount * this.EntityPM.Rate, 2);
        }

        this.TotalAmountLocal = myResult;
        this.ComputeTotalAmountInProfitCurrency();
    }
    ComputeTotalAmountInProfitCurrency() {

        if (this.EntityPM.CurrencyId == this.ProfitCurrencyId) {
            this.EntityPM.AmountInProfitCurrency = this.EntityPM.TotalAmount;
        }

        else {
            this.EntityPM.AmountInProfitCurrency = (this.TotalAmountLocal / this.EntityPM.ProfitCurrencyExchangeRate);
        }

        this.ComputeOtherAmounts();
    }
    ComputeOtherAmounts() {

    } 
}
