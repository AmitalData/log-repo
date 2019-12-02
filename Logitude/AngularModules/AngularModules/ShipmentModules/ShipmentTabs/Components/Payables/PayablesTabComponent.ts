import {Component, OnInit, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPayablePM} from '../../../../Shipment/EntityPMs/ShipmentPayablePM';
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
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ShipmentDomainService} from '../../../../Shipment/Services/ShipmentDomainService';
import {ShipmentPMService} from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {QuotePMService} from '../../../../Quote/Services/StandardPMs/QuotePMService';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
@Component({
    moduleId: module.id,
    templateUrl: './PayablesTabComponent.html',
})

export class PayablesTabComponent implements OnInit, OnDestroy {
    public EntityPM: ShipmentPM;
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
    public myDomainService: ShipmentDomainService;
    public IsPriceCheckVisible:boolean=false;
    public myUserListService: UserListService = null;
    public ComponentRef: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        this.EntityPM = entityArgs.EntityPM;
        this.OriginShipment = entityArgs.OriginEntity;
        this.ObjectTableName = entityArgs.ObjectTableName;
        this.ShipmentLevelCode = this.EntityPM.ShipmentLevelCode;
        this.ProfitCurrencyId = this.EntityPM.ProfitCurrencyId;
        this.ProfitCurrencyCode = this.EntityPM.ProfitCurrencyCode;
        this.LocalCurrencyId = SessionLocator.LocalCurrencyId;
        this.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
        this.ItemsSource = new ObservableCollection([]);
        this.myDomainService = new ShipmentDomainService();
        this.myUserListService = new UserListService();
        this.Listen();
        this.SetEditEnabled();
        this.LoadRequiredData();        
    }

    private SessionEvent: any = null;
    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;  
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "PayablesGenerated") {
                    this.BuildItemsSource();
                    this.ComputeShipmentFields();
                }

                else if (s == "OriginShipmentLoaded") {
                    this.OriginShipment = this.entityArgs.OriginEntity;
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
                if (tabCode == "SHPY" || tabCode == "JHPY") {
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

            this.entityResourceService.getEntityResourceByTableName("ShipmentPayable").subscribe((res: any) => {
                this.entityResourceService.getEntityResourceByTableName("TarrifHeader").subscribe((res2: any) => {
                    this.IsResourcesReady = true;
                    this.SetLabels();
                    this.SetUIProperties();
                    this.BuildItemsSource();
                    this.BuildSummaryData();
                    this.InitializeProfitArea();
                    this.SetGenerateButtons();
                });
            });
        }
    }

    public SelectedRow: ShipmentPayableItem = null;
    OnRowSelected(itemComponent: ShipmentPayableItem) {
        this.SelectedRow = itemComponent;
    }
    OnRowLoaded(Row: any) {

        var isExpandaple = false;

        if (this.EntityPM.ShipmentLevelCode == "C") {
            if (Row) {
                var item: ShipmentPayableItem = Row.rowData;
                if (item) {
                    if (item.EntityPM.ChildShipmentPayables.length > 0) {
                        isExpandaple = true;
                    }
                }

                Row.SetExpandaple(isExpandaple);
            }
        }
    }

    PriceCheck() {
        this.entityResourceService.getEntityResourceByTableName("TariffLine").subscribe((res1: any) => {
            var betweenDate: Date = DateTool.GetCurrentDateAsUtc();
            if (!AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageATD)) {
                betweenDate = this.EntityPM.MainCarriageATD;
            }
            else if (!AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageETD)) {
                betweenDate = this.EntityPM.MainCarriageETD;
            }

            var tariffType = "";
            if (this.EntityPM.TransportModeId == "A") {
                tariffType = "AFC";
            }
            else if (this.EntityPM.ShipmentTypeId == "LCL" || this.EntityPM.ShipmentTypeId == "LCLD") {
                tariffType = "OLC";
            }

            var WindowArgs: any =
            {
                BetweenDate: betweenDate,
                FromPort: this.EntityPM.MainCarriageFromPortId,
                ToPort: this.EntityPM.ToPortId,
                GrossWeight: this.EntityPM.GrossWeight,
                ChargeableWeight: this.EntityPM.ChargeableWeight,
                Volume: this.EntityPM.Volume,
                ChargeableWeightUnit: this.EntityPM.ChargeableWeightUnitCode,
                GrossWeightUnit: this.EntityPM.GrossWeightUnitCode,
                VolumeUnit: this.EntityPM.VolumeUnitCode,
                ShipmentPM: this.EntityPM,
                FatherComponent: this,
                TariffType: tariffType
            };
            var logWindow = new LogitudeWindow();
            logWindow.IsFillScreenHeight = true;
            logWindow.Width = 1200;
            logWindow.Title = "Price Check";
            logWindow.ComponentLoaded.subscribe(cmpRef => {
                this.ComponentRef = cmpRef;

                if (WindowArgs != null) {
                    if (this.ComponentRef['SetWindowArgs']) {
                        this.ComponentRef.SetWindowArgs(WindowArgs);
                    }
                }
            });

            logWindow.Show("./TariffModule/Components/Workspaces/TariffSearchAirFreightPricesComponent");
           
        });      
    }

    // Set Labels
    public ExpectedAmountLocalHeader: string;
    SetLabels() {
        this.ExpectedAmountLocalHeader = TextCodeTranslator.Translate("ShipmentPayable.F.ExpectedAmountLocal").replace("%LocalCurrencyCode", this.LocalCurrencyCode);
    }

    // SetUIProperties
    public IsEditingEnabled: boolean = true;
    public IsDeleteAllPayablesVisible: boolean = false;
    SetEditEnabled() {

        if (FeatureLocator.HasFeaturePermession("Shipment", "DeleteAllPayables")) {
            this.IsDeleteAllPayablesVisible = true;
        }

        if (FeatureLocator.HasFeaturePermession("Shipment", "ShipmentPriceCheck") && (this.EntityPM.TransportModeId == "A" || this.EntityPM.ShipmentTypeId == "LCL" || this.EntityPM.ShipmentTypeId == "LCLD" )) {
            this.IsPriceCheckVisible = true;
        }

        
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

                    if (FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Action.AllowPayables")) {
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
    BuildItemsSource() {

        var itemsCollection: ShipmentPayableItem[] = [];

        this.EntityPM.ShipmentPayables.filter(f => f.ChargesGroupCode == "FRT").sort((a, b) => { return a.ViewOrder - b.ViewOrder }).forEach((item) => {
            itemsCollection.push(new ShipmentPayableItem(item, this));
        })

        this.EntityPM.ShipmentPayables.filter(f => f.ChargesGroupCode != "FRT").sort((a, b) => { return a.ViewOrder - b.ViewOrder }).forEach((item) => {
            itemsCollection.push(new ShipmentPayableItem(item, this));
        })

        this.SetGenerateButtons();
        this.ItemsSource.InsertCollection(itemsCollection);
    }

    // Summary
    public AccrualsPayables: number = 0;
    public AccountedPayables: number = 0;
    public OpenPayables: number = 0;
    public DifferencePayablesText: string = "N/A";
    public DifferencePayablesColor: string;
    public PayableList: any[] = [];

    BuildSummaryData() {

        this.PayableList = this.EntityPM.ShipmentAPInvoices;

        if (this.IsByLocalCurrency) {
            this.AccrualsPayables = ArrayTool.Sum(this.EntityPM.ShipmentPayables, "ExpectedAmountLocal");
            this.AccountedPayables = ArrayTool.Sum(this.EntityPM.ShipmentPayables, "AccountedAmountInLocalCurrency");
            this.OpenPayables = ArrayTool.Sum(this.EntityPM.ShipmentPayables, "OpenAmountInLocalCurrency");

        }

        else {
            this.AccrualsPayables = ArrayTool.Sum(this.EntityPM.ShipmentPayables, "ExpectedAmountInProfitCurrency");
            this.AccountedPayables = ArrayTool.Sum(this.EntityPM.ShipmentPayables, "AccountedAmountInProfitCurrency");
            this.OpenPayables = ArrayTool.Sum(this.EntityPM.ShipmentPayables, "OpenAmountInProfitCurrency");
        }

        var myDifferencePayablesText = "N/A";
        var myDifferencePayablesColor = FontTool.Black;

        if (this.EntityPM.ShipmentPayables.length == 0) {
            myDifferencePayablesText = DecimalFormatter.format(0, 2);
        }

        else {
            if (this.EntityPM.ShipmentPayables.filter(f => f.ShipmentPayableAmountTypeCode == "ACCU").length > 0) {
                var myDifference = this.AccrualsPayables - (this.AccountedPayables + this.OpenPayables);
                if (AppTool.IsNullOrEmpty(myDifference)) {
                    myDifference = 0;
                }

                myDifferencePayablesText = DecimalFormatter.format(myDifference, 2);

                if (myDifference < 0) {
                    var myDifferencePayablesColor = FontTool.Red;
                }

                if (AppTool.IsNullOrEmpty(myDifferencePayablesText)) {
                    myDifferencePayablesText = DecimalFormatter.format(0, 2);
                }
            }
        }

        this.DifferencePayablesText = myDifferencePayablesText;
        this.DifferencePayablesColor = myDifferencePayablesColor;
    }

    // Profit
    public IsProfitAreaVisible: boolean = false;
    public IsCurrencyFilterVisible: boolean = false;
    public IsProfitRateVisible: boolean = false;
    public IsByLocalCurrency: boolean = false;
    public SelectedCurrencyCode: string = null;
    public ProfitRate: string = "N/A";
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

        this.BuildSummaryData();
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
            //this.OnProfitExchangeRateChanged();
        }
    }

    ShowProfitClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 750;
        logWindow.ShowCloseButton = true;
        logWindow.Title = TextCodeTranslator.Translate("Shipment.S.Profit.ProfitDetails")
        logWindow.WindowArgs = { ShipmentPM: this.EntityPM, IsByLocalCurrency: this.IsByLocalCurrency, IsCurrencyFilterVisible: this.IsCurrencyFilterVisible };
        logWindow.Show('./ShipmentModules/ShipmentTabs/Components/Windows/Profit/ProfitComponent');
    }

    // Generate
    public IsGenerateButtonsVisible: boolean = false;  
    public IsGenerateFromQuoteChargesEnabled: boolean = false;
    public IsNoChargesTextVisibil: boolean = false;
    private ChargesTypes: ChargesTypeList[];
    SetGenerateButtons() {
        var isGenerateButtonsVisible = false;

        if (this.EntityPM.ShipmentPayables.length > 0) {
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
        if (this.EntityPM.ShipmentPackages.length == 0) {
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
                Generator.GeneratePayablesAutoDisplay();
                this.OnEntityDataGenerated();
                break;
            }

            case "QTPY": {
                // FromQuotePayablesOnly
                var Generator = new ShipmentGenerator(this.EntityPM, this.AllRates);
                Generator.GeneratePayablesFromQuote(this.BaseQuote);
                this.OnEntityDataGenerated();
                break;
            }

            case "ORGN": {
                // FromOriginShipment
                if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginShipmentId)) {
                    if (this.OriginShipment) {
                        var Generator = new ShipmentGenerator(this.EntityPM, this.AllRates);
                        Generator.GeneratePayablesFromOriginShipment(this.OriginShipment);
                        this.OnEntityDataGenerated();

                        this.ItemsSource.Collection.forEach((item: ShipmentPayableItem) => {
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
                                Generator.GeneratePayablesFromOriginShipment(this.OriginShipment);
                                this.OnEntityDataGenerated();

                                this.ItemsSource.Collection.forEach((item: ShipmentPayableItem) => {
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
    AddPayable() {
        var newItem = new ShipmentPayablePM(null);
        newItem.Tenant = SessionLocator.Tenant;
        newItem.ShipmentId = this.EntityPM.Id;
        newItem.ShipmentNumber = this.EntityPM.ShipmentNumber;
        newItem.CreateDate = DateTool.GetCurrentDateAsUtc();
        newItem.UpdateDate = DateTool.GetCurrentDateAsUtc();
        newItem.CreatedByUserId = SessionLocator.LoggedUserId;
        newItem.UpdateByUserId = SessionLocator.LoggedUserId;
        newItem.ShipmentPayableLineStatusCode = "EMPT";
        newItem.ShipmentPayableAmountTypeCode = "ACCU";
        newItem.ShipmentPayableAmountTypeName = "Accrual";
        newItem.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);

        var itemComponent = new ShipmentPayableItem(newItem, this, true);
        this.RunAddEditPayable(itemComponent, TextCodeTranslator.Translate("Shipment.O.Payables.AddPayable"));
    }
    EditPayable(itemComponent: ShipmentPayableItem) {
        this.RunAddEditPayable(itemComponent, TextCodeTranslator.Translate("Shipment.O.Payables.EditPayable"));
    }
    RunAddEditPayable(itemComponent: ShipmentPayableItem, windowTitle: string) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./ShipmentModules/ShipmentTabs/Components/Payables/AddEditPayableComponent');
    }
    DeleteItem(itemComponent: ShipmentPayableItem) {
        if (itemComponent.IsEditingEnabled) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator.Translate("Shipment.M.DeleteThisPayable"));
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.EntityPM.RemovePayable(itemComponent.EntityPM);
                    this.BuildItemsSource();

                    if (itemComponent.ChargesGroupCode == "FRT") {
                        this.OnFreightAmountChanged();
                    }

                    this.ComputeShipmentFields();
                }
            });
        }
    }

    // Invoice    
    ReceiveInvoiceClicked() {
        if (FeatureLocator.HasEntityPermessions("APInvoice", "NEW", true)) {
            if (!this.SavingRequested) {
                this.SavingRequested = true;
                this.SavingRequestCode = "NewInvoice";

                this.EntityPM.ShipmentPayables.forEach(item => {
                    if (AppTool.IsNullOrEmpty(item.ShipmentPayableLineStatusCode) || item.ShipmentPayableLineStatusCode == "EMPT") {
                        if (!AppTool.IsNullOrZero(item.Quantity) && !AppTool.IsNullOrZero(item.UnitPrice)) {
                            item.ShipmentPayableLineStatusCode = "OAMT";
                        }
                    }
                });

                this.SaveChanges();
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

                    var iFields: any[] = [];
                    iFields.push({ FieldName: "ShipmentConcurrencyGUID", FieldValue: this.EntityPM.ConcurrencyGUID });
                    iFields.push({ FieldName: "ShipmentNewConcurrencyGUID", FieldValue: this.EntityPM.NewConcurrencyGUID });

                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: 'APInvoice', BackButtonLabel: this.ObjectTableName + ": " + this.EntityPM.ShipmentNumber, EntityFields: iFields });

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
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.B.Payables.ReceiveInvoice");
        logitudeWindow.WindowArgs = { ShipmentPM: this.EntityPM };

        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ EntityPM: comp.EntityPM, ObjectTableName: 'APInvoice', BackButtonLabel: this.ObjectTableName + ": " + this.EntityPM.ShipmentNumber });

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

        logitudeWindow.Show('./InvoiceModules/APInvoice/Components/NewEntity/NewAPInvoiceComponent');
    }
    TariffsButtonClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = TextCodeTranslator.Translate("TariffHeader.O.Tariffs")
        logWindow.WindowArgs = this;
        logWindow.Show('./ShipmentModules/ShipmentTabs/Components/Windows/Tariffs/TariffsComponent');
    }
    EditTariffClicked(item: ShipmentPayableItem) {
        if (item != null) {
            var editWindow = new LogitudeWindow();
            editWindow.ShowHeaderButtons = true;
            editWindow.Title = "Price Check";
            editWindow.Height = 770;
            editWindow.Width = 1500;
            editWindow.ShowEditComponent(item.TariffId, "Tariff", item.TariffVersion + "" );
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

    // Update Quantities
    public UpdateQuantitiesMessage: string;
    public UpdateQuantitiesMessageWidth: number = 0;
    public IsUpdateQuantitiesVisible: boolean = false;
    CheckUpdateQuantities() {
        var updateMessage = null;

        var activeLines: ShipmentPayablePM[] = [];
        activeLines = this.EntityPM.ShipmentPayables;
        activeLines = activeLines.filter(d => d.ShipmentPayableParentId == null);
        activeLines = activeLines.filter(d => d.ShipmentPayableAmountTypeCode != "NEXP");
        activeLines = activeLines.filter(d => d.ShipmentPayableLineStatusCode != "ACCT");
        activeLines = activeLines.filter(d => d.ShipmentPayableLineStatusCode != "PACC");
        activeLines = activeLines.filter(d => d.UnitPrice != null);

        if (activeLines.length > 0) {

            var isDifferentOrders: boolean = false;
            var isDifferentPRVL: boolean = false;
            var isDifferentPRFR: boolean = false;

            activeLines.forEach(item => {
                switch (item.MeasurementCode) {
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

                        if (this.EntityPM.ShipmentPayables.filter(f => f.ChargesGroupCode == "FRT").length > 0) {

                            var FRT_Quantity = ArrayTool.Sum(this.EntityPM.ShipmentPayables.filter(d => d.ChargesGroupCode == "FRT" && AppTool.IsNullOrEmpty(d.ShipmentPayableParentId)), "ExpectedAmount");

                            if (item.Quantity != FRT_Quantity) {
                                isDifferentPRVL = true;
                            }

                            if (this.EntityPM.ShipmentPayables.filter(f => f.MeasurementCode == "PRFR" && f.Quantity != FRT_Quantity).length > 0) {
                                isDifferentPRFR = true;
                            }
                        }

                        break;
                    }

                    case "VCBM": {

                        if (item.Quantity != this.EntityPM.VolumeInCBM) {
                            isDifferentOrders = true;
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

        var activeLines: ShipmentPayableItem[] = [];
        activeLines = this.ItemsSource.Collection;
        activeLines = activeLines.filter(d => d.EntityPM.ShipmentPayableParentId == null);
        activeLines = activeLines.filter(d => d.EntityPM.ShipmentPayableAmountTypeCode != "NEXP");
        activeLines = activeLines.filter(d => d.EntityPM.ShipmentPayableLineStatusCode != "ACCT");
        activeLines = activeLines.filter(d => d.EntityPM.ShipmentPayableLineStatusCode != "PACC");

        activeLines.forEach((item: ShipmentPayableItem) => {
            item.SetQuantity();
        });

        this.CheckUpdateQuantities();
    }

    DeleteAllClicked() {
        if (this.IsEditingEnabled) {

            if (this.EntityPM.ShipmentPayables.filter(f => f.ShipmentPayableLineStatusCode == 'PACC' || f.ShipmentPayableLineStatusCode == 'ACCT').length > 0) {
                var messageWindow = new MessageWindow();
                messageWindow.Show("Can't delete all, some lines are connected to invoices");
            }

            else if (this.EntityPM.ShipmentLevelCode == "H" && this.EntityPM.ShipmentPayables.filter(f => f.ShipmentPayableParentId != null).length > 0) {
                var messageWindow = new MessageWindow();
                messageWindow.Show("Can't delete all, some lines are connected to master");
            }

            else {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Show("Please note that deleting will erase all the lines with the amounts inserted");
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.EntityPM.ShipmentPayables = [];
                        this.EntityPM.IsDeletingAllPayables = true;
                        this.EntityPM.IsDirty = true;
                        this.BuildItemsSource();
                        this.ComputeShipmentFields();
                    }
                });
            }
        }
    }
}
export class ShipmentPayableItem extends BaseComponent {
    public EntityPM: ShipmentPayablePM;
    public ShipmentPM: ShipmentPM;
    public ObjectTableName: string = "ShipmentPayable";
    public DataContext = this;
    public LocalCurrencyCode: string = null;
    public ProfitCurrencyCode: string = null;
    public ByContainersItemsSource: ShipmentPayablePM[] = [];
    public InsideItemsSource: InsidePayableViewModel[] = [];
    public IsNewEntity: boolean = false;
    constructor(entity: ShipmentPayablePM, public fatherComponent: PayablesTabComponent, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.ShipmentPM = fatherComponent.EntityPM;
        this.IsNewEntity = isNew;
        this.LocalCurrencyCode = fatherComponent.LocalCurrencyCode;
        this.ProfitCurrencyCode = fatherComponent.ProfitCurrencyCode;
        this.SetUIProperties();
        this.SetLineCells();
        this.SetLineSummary();
        this.BuildInsidePayables();
    }

     // SetUIProperties
    public IsRateEnabled: boolean = false;
    public IsQuantityEnabled: boolean = false;
    public IsUnitPriceEnabled: boolean = false;
    public IsTotalAmountEnabled: boolean = false;
    public IsOpenAmountEnabled: boolean = false;
    public IsEditingEnabled: boolean = false;
    public IsLineAttachted: boolean = false;
    public IsProfitAmountVisible: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    SetUIProperties() {
        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }

        var isLineAttachted = false;
        var isEditingEnabled = this.fatherComponent.IsEditingEnabled;
        var isOpenAmountEnabled = false;

        if (this.EntityPM.ShipmentPayableParentId != null) {
            isLineAttachted = true;
        }

        if (this.EntityPM.ShipmentPayableLineStatusCode == "ACCT" || this.EntityPM.ShipmentPayableLineStatusCode == "PACC") {
            isLineAttachted = true;
        }

        if (isEditingEnabled) {
            isOpenAmountEnabled = true;

            if (this.EntityPM.ShipmentPayableParentId != null) {
                isOpenAmountEnabled = false;
            }

            if (this.EntityPM.ShipmentPayableAmountTypeCode == "NEXP" || this.EntityPM.ShipmentPayableLineStatusCode == "EMPT" || this.EntityPM.ShipmentPayableLineStatusCode == "OAMT") {
                isOpenAmountEnabled = false;
            }            

            //if (AppTool.IsNullOrEmpty(this.TariffId)) {
            //    isOpenAmountEnabled = false;
            //}

            if (isLineAttachted) {
                isEditingEnabled = false;
            }
        }

        var isRateEnabled = false;
        var isChargeEnabled = false;
        var isQuantityEnabled = false;
        var isUnitPriceEnabled = false;
        var isTotalAmountEnabled = false;

        if (isEditingEnabled) {
            if (this.IsNewEntity) {
                isChargeEnabled = true;
            }
            
            if (FeatureLocator.HasFeaturePermession("Shipment", "ShipmentEditExchangeRate")) {
                if (this.CurrencyId != null) {
                    if (this.CurrencyId != SessionLocator.LocalCurrencyId) {
                        isRateEnabled = true;
                    }
                }
            }
            if (AppTool.IsNullOrEmpty(this.TariffId)) {
                isQuantityEnabled = true;
            }
            if (!this.EntityPM.IsChargeBySteps && AppTool.IsNullOrEmpty(this.TariffId)) {
                isUnitPriceEnabled = true;
                isTotalAmountEnabled = true;
            }                       
        }
        
        this.IsRateEnabled = isRateEnabled;
        this.IsQuantityEnabled = isQuantityEnabled;
        this.IsUnitPriceEnabled = isUnitPriceEnabled;
        this.IsTotalAmountEnabled = isTotalAmountEnabled;
        this.IsOpenAmountEnabled = isOpenAmountEnabled;
        this.IsEditingEnabled = isEditingEnabled;
        this.IsLineAttachted = isLineAttachted;
        var isFromTariff = this.EntityPM != null && this.EntityPM.TariffId != null;
      
        this.UIProperties.SetEnabled("ExpectedAmount", this.ObjectTableName, this.IsTotalAmountEnabled || isFromTariff);
        this.UIProperties.SetEnabled("ExpectedAmountLocal", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Rate", this.ObjectTableName, isRateEnabled || isFromTariff);
        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, isChargeEnabled || isFromTariff);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsQuantityEnabled || isFromTariff);
        this.UIProperties.SetEnabled("UnitPrice", this.ObjectTableName, this.IsUnitPriceEnabled || isFromTariff);
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, this.IsEditingEnabled || isFromTariff);
        this.UIProperties.SetEnabled("MeasurementId", this.ObjectTableName, this.IsEditingEnabled || isFromTariff);
        this.UIProperties.SetEnabled("PrepaidCollectId", this.ObjectTableName, this.IsEditingEnabled || isFromTariff);
        this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, this.IsEditingEnabled || isFromTariff);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingEnabled || isFromTariff);

        this.SetUIProperties_AmountProfit();
        this.SetUIProperties_MeasurementId();
    }
    SetUIProperties_AmountProfit() {
        var isFieldVisible = false;

        if (!AppTool.IsNullOrEmpty(this.CurrencyId)) {
            if (this.ShipmentPM.ProfitCurrencyId != this.CurrencyId && this.ShipmentPM.ProfitCurrencyId != SessionLocator.LocalCurrencyId) {
                isFieldVisible = true;
            }
        }

        this.IsProfitAmountVisible = isFieldVisible;
        this.UIProperties.SetEnabled("ExpectedAmountInProfitCurrency", this.ObjectTableName, false);
        this.UIProperties.SetVisibility("ExpectedAmountInProfitCurrency", this.ObjectTableName, isFieldVisible);
    }
    SetUIProperties_MeasurementId() {
        var isRequired = false;

        if (this.EntityPM.ShipmentPayableAmountTypeCode == "ACCU") {
            if (AppTool.IsNullOrEmpty(this.EntityPM.MeasurementId)) {
                isRequired = true;
            }
        }

        this.UIProperties.SetRequired("MeasurementId", this.ObjectTableName, isRequired);
    }

    // Line Cells
    public SatusTypeToolTip: string = null;
    public MinMaxFromQuoteToolTip: string = "";
    public IsMinMaxFromQuoteIconVisible: boolean = false;
    public IsMinFromTarrifIconVisible: boolean = false;
    public IsMaxFromTarrifIconVisible: boolean = false;
    public IsAccountedAmountVisible: boolean = false;
    public IsInvoicesIconVisible: boolean = false;
    SetLineCells() {
        this.SetSatusTypeToolTip();
        this.SetMinMaxFromQuoteIconVisibility();
        this.SetMinFromTarrifIconVisibility();
        this.SetMaxFromTarrifIconVisibility();
        this.SetOpenAmountCell();
        this.SetExpectedAmountCell();
        this.IsAccountedAmountVisible = this.AccountedAmount != null && this.AccountedAmount != 0;
        this.IsInvoicesIconVisible = (this.EntityPM.ShipmentPayableLineStatusCode == "PACC" || this.EntityPM.ShipmentPayableLineStatusCode == "ACCT") ? true : false;
    }
    SetSatusTypeToolTip() {
        var myResult: string;

        switch (this.EntityPM.ShipmentPayableLineStatusCode) {
            case "APPD": { myResult = TextCodeTranslator.Translate("Shipment.O.Payables.Approved"); break; }
            case "NOIN": { myResult = TextCodeTranslator.Translate("Shipment.O.Payables.NoInvoiceNeeded"); break; }
            case "OAMT": { myResult = TextCodeTranslator.Translate("Shipment.O.Payables.OpenAmount"); break; }
            case "ACCT": { myResult = TextCodeTranslator.Translate("Shipment.O.Payables.Accounted"); break; }
            case "PACC": { myResult = TextCodeTranslator.Translate("Shipment.O.Payables.PartiallyAccounted"); break; }
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
            if (this.QuoteCostMinAmount != null) {
                if (iAmount < this.QuoteCostMinAmount) {
                    iAmount = this.QuoteCostMinAmount;
                    isVisible = true;
                    iTitle = TextCodeTranslator.Translate("Shipment.M.Payables.AmountdueQuoteMinimum");
                }
            }

            if (this.QuoteCostMaxAmount != null) {
                if (iAmount > this.QuoteCostMaxAmount) {
                    iAmount = this.QuoteCostMaxAmount;
                    isVisible = true;
                    iTitle = TextCodeTranslator.Translate("Shipment.M.Payables.AmountdueQuoteMaximum");
                }
            }
        }

        this.MinMaxFromQuoteToolTip = iTitle;
        this.IsMinMaxFromQuoteIconVisible = isVisible;
    }
    SetMinFromTarrifIconVisibility() {
        var isVisible = false;

        var culculatedAmount: number = null;
        if (this.EntityPM.Quantity != null && this.EntityPM.UnitPrice != null) {
            culculatedAmount = AppTool.Round(this.EntityPM.Quantity * this.EntityPM.UnitPrice, 2);
        }

        if (culculatedAmount != null) {
            if (this.MinAmount != null) {
                if (culculatedAmount <= this.MinAmount) {
                    isVisible = true;
                }
            }
        }

        this.IsMinFromTarrifIconVisible = isVisible;
    }
    SetMaxFromTarrifIconVisibility() {
        var isVisible = false;

        var culculatedAmount: number = null;
        if (this.EntityPM.Quantity != null && this.EntityPM.UnitPrice != null) {
            culculatedAmount = AppTool.Round(this.EntityPM.Quantity * this.EntityPM.UnitPrice, 2);
        }

        if (culculatedAmount != null) {
            if (this.MaxAmount != null) {
                if (culculatedAmount > this.MaxAmount) {
                    isVisible = true;
                }
            }
        }

        this.IsMaxFromTarrifIconVisible = isVisible;
    }

    public ExpectedAmountColor: string = null;
    SetExpectedAmountCell() {

        var myColor = FontTool.Black;

        if (this.InsideItemsSource) {
            var sumOfAmount = ArrayTool.Sum(this.InsideItemsSource, "ExpectedAmount");
            var sumOfAmount = AppTool.Round(sumOfAmount, 2);

            if ((sumOfAmount < 0 || sumOfAmount > 0) && !AppTool.IsNullOrEmpty(this.ExpectedAmount) && sumOfAmount != this.ExpectedAmount) {            
                myColor = FontTool.Red;
            }

            else if (this.EntityPM.IsChargeBySteps) {
                myColor = FontTool.Gray;
            }

            else if (this.EntityPM.ShipmentPayableLineStatusCode == "ACCT" || this.EntityPM.ShipmentPayableLineStatusCode == "PACC") {
                myColor = FontTool.Gray;
            }

            else if (!AppTool.IsNullOrEmpty(this.EntityPM.ShipmentPayableParentId)) {
                myColor = FontTool.Gray;
            }
        }

        this.ExpectedAmountColor = myColor;
    }

    public IsOpenAmountVisible: boolean = false;
    public OpenAmountColor: string = null;
    SetOpenAmountCell() {

        var isOpenAmountVisible = true;

        if (AppTool.IsNullOrEmpty(this.OpenAmount)) {
            isOpenAmountVisible = false;
        }

        else if (this.OpenAmount == 0) {
            if (AppTool.IsNullOrZero(this.Quantity) || AppTool.IsNullOrZero(this.UnitPrice)) {
                isOpenAmountVisible = false;
            }
        }

        var myOpenAmountColor = FontTool.Black;

        if (this.EntityPM.ShipmentPayableAmountTypeCode == "NEXP") {
            myOpenAmountColor = FontTool.Gray;
        }

        else {
            if (this.OpenAmount < 0) {
                myOpenAmountColor = FontTool.Red;
            }

            else {
                if (this.EntityPM.ShipmentPayableLineStatusCode == "EMPT" || this.EntityPM.ShipmentPayableLineStatusCode == "OAMT") {
                    myOpenAmountColor = FontTool.Gray;
                }
            }
        }

        this.OpenAmountColor = myOpenAmountColor;
        this.IsOpenAmountVisible = isOpenAmountVisible;
        this.GetCorrectionUser();
    }

    get ChargesTypeCode() { return this.EntityPM.ChargesTypeCode; }
    get ChargesTypeName() { return this.EntityPM.ChargesTypeName; }
    get ChargesGroupCode() { return this.EntityPM.ChargesGroupCode; }
    get TariffNumber() { return this.EntityPM.TariffNumber; }
    set TariffNumber(value: string) {
        if (value != this.EntityPM.TariffNumber) {
            this.EntityPM.TariffNumber = value;
        }
    }
    get TariffId() {
        return this.EntityPM.TariffId;
    }
    set TariffId(value: string) {
        if (value != this.EntityPM.TariffId) {
            this.EntityPM.TariffId = value;
        }
    }
    get TariffVersion() { return this.EntityPM.TariffVersion; }
    set TariffVersion(value: number) {
        if (value != this.EntityPM.TariffVersion) {
            this.EntityPM.TariffVersion = value;
        }
    }

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

            this.SetPrepaidCollectId();

            if (!AppTool.IsNullOrEmpty(list.PayablesDefaultCurrencyId)) {
                this.CurrencyId = list.PayablesDefaultCurrencyId;
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
            this.SetUIProperties_MeasurementId();

            if (newValue == null) {
                this.Quantity = null;
                this.EntityPM.MeasurementCode = null;
                this.EntityPM.MeasurementShortName = null;
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
                                case "CWKG": { this.Quantity = this.ShipmentPM.ChargeableWeightInKG; break;}
                                case "GWKG": { this.Quantity = this.ShipmentPM.GrossWeightInKG; break; }
                                case "VCBM": { this.Quantity = this.ShipmentPM.VolumeInCBM; break; }
                                case "PRVL": {
                                    this.Quantity = this.ShipmentPM.ValueOfGoods;
                                    this.CurrencyId = this.ShipmentPM.ValueOfGoodsCurrencyId;
                                    break;
                                }

                                case "PRFR": {
                                    this.Quantity = ArrayTool.Sum(this.ShipmentPM.ShipmentPayables.filter(d => d.ChargesGroupCode == "FRT" && AppTool.IsNullOrEmpty(d.ShipmentPayableParentId)), "ExpectedAmount");
                                    this.CurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
                                    break;
                                }

                                case "BCNT": {
                                    this.IsByContainerType = true;
                                    this.BuildByContainersItemsSource();
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

        var byContainersItemsSource: ShipmentPayablePM[] = [];

        var listGrouped: ByPckageType[] = ShipmentTool.GetByPckageTypeGrouped(this.ShipmentPM);

        listGrouped.forEach(itemGrouped => {
            var newEntity = new ShipmentPayablePM(null);
            newEntity.ShipmentId = this.ShipmentPM.Id;
            newEntity.ShipmentNumber = this.ShipmentPM.ShipmentNumber;
            newEntity.Tenant = this.ShipmentPM.Tenant;
            newEntity.ShipmentPayableLineStatusCode = "EMPT";
            newEntity.ShipmentPayableAmountTypeCode = "ACCU";
            newEntity.ShipmentPayableAmountTypeName = "Accrual";
            newEntity.ChargesTypeId = this.EntityPM.ChargesTypeId;
            newEntity.ChargesTypeCode = this.EntityPM.ChargesTypeCode;
            newEntity.ChargesTypeName = this.EntityPM.ChargesTypeName;
            newEntity.ChargesGroupCode = this.EntityPM.ChargesGroupCode;
            newEntity.DueTypeCode = this.EntityPM.DueTypeCode;
            newEntity.DueTypeName = this.EntityPM.DueTypeName;
            newEntity.IATACodeId = this.EntityPM.IATACodeId;
            newEntity.VatTypeId = this.EntityPM.VatTypeId;
            newEntity.PrepaidCollectId = this.EntityPM.PrepaidCollectId;
            newEntity.VendorId = this.EntityPM.VendorId;
            newEntity.VendorName = this.EntityPM.VendorName;
            newEntity.CurrencyId = this.EntityPM.CurrencyId;
            newEntity.CurrencyCode = this.EntityPM.CurrencyCode;
            newEntity.Rate = this.EntityPM.Rate;
            newEntity.ProfitCurrencyExchangeRate = this.EntityPM.ProfitCurrencyExchangeRate;
            newEntity.CreateDate = DateTool.GetCurrentDateAsUtc();
            newEntity.CreatedByUserId = SessionLocator.LoggedUserId;
            newEntity.UpdateDate = DateTool.GetCurrentDateAsUtc();
            newEntity.UpdateByUserId = SessionLocator.LoggedUserId;
            newEntity.Quantity = itemGrouped.Quantity;
            newEntity.MeasurementId = itemGrouped.MeasurementId;
            newEntity.MeasurementCode = itemGrouped.MeasurementCode;
            newEntity.MeasurementShortName = itemGrouped.MeasurementShortName;

            var exsistingEntity = byContainersItemsSource.filter(f => f.ChargesTypeId == newEntity.ChargesTypeId && f.MeasurementId == newEntity.MeasurementId)[0];
            if (exsistingEntity == null) {
                byContainersItemsSource.push(newEntity);
            }

            else {
                var acctEntity = byContainersItemsSource.filter(f => f.ChargesTypeId == newEntity.ChargesTypeId && f.MeasurementId == newEntity.MeasurementId && (f.ShipmentPayableLineStatusCode == "ACCT" || f.ShipmentPayableLineStatusCode == "PACC"))[0];
                var openEntity = byContainersItemsSource.filter(f => f.ChargesTypeId == newEntity.ChargesTypeId && f.MeasurementId == newEntity.MeasurementId && (f.ShipmentPayableLineStatusCode == "EMPT" || f.ShipmentPayableLineStatusCode == "OAMT"))[0];

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
            //this.ApplyByContainersFields();
        }
    }

    public VendorCardEntity: CardList = null; 
    get VendorName() { return this.EntityPM.VendorName; }
    get VendorId() { return this.EntityPM.VendorId; }
    set VendorId(newValue: string) {
        if (this.EntityPM.VendorId != newValue) {
            this.EntityPM.VendorId = newValue;

            if (newValue == null) {
                this.EntityPM.VendorName = null;
                this.UpdateInsideItemsSource_Vendor();
            }

            else {
                var myService: CardListService = new CardListService();
                myService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.VendorCardEntity = myResponse.Result;
                        if (this.VendorCardEntity != null) {
                            this.EntityPM.VendorName = this.VendorCardEntity.EnglishName;
                            this.UpdateInsideItemsSource_Vendor();
                        }
                    }
                });
            }            
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

    get Quantity() { return this.EntityPM.Quantity; }
    set Quantity(newVaule: number) {
        if (this.EntityPM.Quantity != newVaule) {
            this.EntityPM.Quantity = AppTool.Round(newVaule, 3);

            if (this.EntityPM.IsChargeBySteps) {
                ShipmentTool.SetPayableUnitPriceBySteps(this.EntityPM, this.fatherComponent.BaseQuote);
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

    get ExpectedAmount() { return this.EntityPM.ExpectedAmount; }
    set ExpectedAmount(ivalue: number) {

        var value = ivalue;
        if (value) {

            /* MinMax Tariff */
            if (this.MinAmount != null) {
                if (value < this.MinAmount) {
                    value = this.MinAmount;
                }
            }

            if (this.MaxAmount != null) {
                if (value > this.MaxAmount) {
                    value = this.MaxAmount;
                }
            }

            /* MinMax Quote */
            if (this.QuoteCostMinAmount != null) {
                if (value < this.QuoteCostMinAmount) {
                    value = this.QuoteCostMinAmount;
                }
            }

            if (this.QuoteCostMaxAmount != null) {
                if (value > this.QuoteCostMaxAmount) {
                    value = this.QuoteCostMaxAmount;
                }
            }
        }

        if (this.EntityPM.ExpectedAmount != value) {
            this.EntityPM.ExpectedAmount = AppTool.Round(value, 2);
            this.SetLineStatus();
            this.SetLineCells();
            this.ComputeUnitPrice();
            this.ComputeTotalAmountLocal();
            this.OnLineAmountChanged();
        }
    }

    get ExpectedAmountLocal() { return this.EntityPM.ExpectedAmountLocal; }
    set ExpectedAmountLocal(newVaule: number) {
        if (this.EntityPM.ExpectedAmountLocal != newVaule) {
            this.EntityPM.ExpectedAmountLocal = AppTool.Round(newVaule, 2);
            this.SetLineSummary();

            this.fatherComponent.ItemsSource.Collection.filter(f => f.ChargesTypeId == this.ChargesTypeId).forEach(item => {
                item.SetLineSummary();
            });

            this.ComputeInsidePayablesData();                       
        }
    }

    get ExpectedAmountInProfitCurrency() { return this.EntityPM.ExpectedAmountInProfitCurrency; }
    set ExpectedAmountInProfitCurrency(newVaule: number) {
        if (this.EntityPM.ExpectedAmountInProfitCurrency != newVaule) {
            this.EntityPM.ExpectedAmountInProfitCurrency = AppTool.Round(newVaule, 2);
        }
    }

    get OpenAmount() { return this.EntityPM.OpenAmount; }
    set OpenAmount(newVaule: number) {
        if (this.EntityPM.OpenAmount != newVaule) {
            this.EntityPM.OpenAmount = AppTool.Round(newVaule, 2);
            this.SetOpenAmountCell();

            this.OpenAmountInLocalCurrency = newVaule * this.EntityPM.Rate;
            this.OpenAmountInProfitCurrency = this.OpenAmountInLocalCurrency / this.EntityPM.ProfitCurrencyExchangeRate;

            var expe: number = this.EntityPM.ExpectedAmount == null ? 0 : this.EntityPM.ExpectedAmount;
            var acct: number = this.EntityPM.AccountedAmount == null ? 0 : this.EntityPM.AccountedAmount;
            var open: number = newVaule == null ? 0 : newVaule;
            this.CorrectionAmount = expe - acct - open;

            if (!AppTool.IsNullOrEmpty(this.EntityPM.CorrectionByUserId)) {
                this.SetLineStatus();
            }
        }
    }

    get OpenAmountInLocalCurrency() { return this.EntityPM.OpenAmountInLocalCurrency; }
    set OpenAmountInLocalCurrency(newVaule: number) {
        if (this.EntityPM.OpenAmountInLocalCurrency != newVaule) {
            this.EntityPM.OpenAmountInLocalCurrency = AppTool.Round(newVaule, 2);
        }
    }

    get OpenAmountInProfitCurrency() { return this.EntityPM.OpenAmountInProfitCurrency; }
    set OpenAmountInProfitCurrency(newVaule: number) {
        if (this.EntityPM.OpenAmountInProfitCurrency != newVaule) {
            this.EntityPM.OpenAmountInProfitCurrency = AppTool.Round(newVaule, 2);
        }
    }

    get AccountedAmount() { return this.EntityPM.AccountedAmount; }
    set AccountedAmount(newVaule: number) {
        if (this.EntityPM.AccountedAmount != newVaule) {
            this.EntityPM.AccountedAmount = AppTool.Round(newVaule, 2);
        }
    }

    get CorrectionAmount() { return this.EntityPM.CorrectionAmount; }
    set CorrectionAmount(newVaule: number) {
        if (this.EntityPM.CorrectionAmount != newVaule) {
            this.EntityPM.CorrectionAmount = AppTool.Round(newVaule, 2);

            this.CorrectionByUserId = SessionLocator.LoggedUserId;
            this.CorrectionDate = DateTool.GetCurrentDateAsUtc();

            if (this.fatherComponent != null) {
                this.fatherComponent.ComputeShipmentFields();
            }
        }
    }

    get CorrectionAmountColor() {
        var myResult = FontTool.Black;

        if (this.CorrectionAmount) {
            if (this.CorrectionAmount > 0) {
                myResult = "Orange";
            }
        }

        return myResult;
    }

    get CorrectionByUserId() { return this.EntityPM.CorrectionByUserId; }
    set CorrectionByUserId(value: string) {
        if (this.EntityPM.CorrectionByUserId != value) {
            this.EntityPM.CorrectionByUserId = value;
            this.GetCorrectionUser();
        }
    }

    get CorrectionDate() { return this.EntityPM.CorrectionDate; }
    set CorrectionDate(value: Date) {
        if (this.EntityPM.CorrectionDate != value) {
            this.EntityPM.CorrectionDate = value;            
        }
    }

    get IsCorrectionTooltipVisible() {
        var myResult = false;

        if (this.CorrectionAmount) {
            if (this.CorrectionAmount < 0 || this.CorrectionAmount > 0) {
                myResult = true;
            }
        }

        return myResult;
    }

    public CorrectionByUserName: string;
    GetCorrectionUser() {
        if (AppTool.IsNullOrEmpty(this.CorrectionByUserId)) {
            this.CorrectionByUserName = null;
        }

        else {
            this.fatherComponent.myUserListService.getSingleFromCache(this.CorrectionByUserId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: UserList = myResponse.Result;
                    if (list) {
                        this.CorrectionByUserName = list.EnglishName;
                    }
                }
            });
        }
    }

    get MinAmount() { return this.EntityPM.MinAmount; }
    set MinAmount(newVaule: number) {
        if (this.EntityPM.MinAmount != newVaule) {
            this.EntityPM.MinAmount = AppTool.Round(newVaule, 2);
            this.ComputeTotalAmount();
        }
    }

    get MaxAmount() { return this.EntityPM.MaxAmount; }
    set MaxAmount(newVaule: number) {
        if (this.EntityPM.MaxAmount != newVaule) {
            this.EntityPM.MaxAmount = AppTool.Round(newVaule, 2);
            this.ComputeTotalAmount();
        }
    }

    get QuoteCostMinAmount() { return this.EntityPM.QuoteCostMinAmount; }
    set QuoteCostMinAmount(newVaule: number) {
        if (this.EntityPM.QuoteCostMinAmount != newVaule) {
            this.EntityPM.QuoteCostMinAmount = AppTool.Round(newVaule, 2);
        }
    }

    get QuoteCostMaxAmount() { return this.EntityPM.QuoteCostMaxAmount; }
    set QuoteCostMaxAmount(newVaule: number) {
        if (this.EntityPM.QuoteCostMaxAmount != newVaule) {
            this.EntityPM.QuoteCostMaxAmount = AppTool.Round(newVaule, 2);
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
        ShipmentTool.SetPayableLineStatus(this.EntityPM);
        this.SetSatusTypeToolTip();
    }
    ComputeUnitPrice() {
        if (!this.EntityPM.IsChargeBySteps) {

            var myResult: number = null;

            if (this.EntityPM.IsChargeBySteps) {

            }

            else {
                if (this.EntityPM.ExpectedAmount != null && this.EntityPM.Quantity != null) {
                    if (this.EntityPM.Quantity == 0) {
                        myResult = 0;
                    }

                    else {
                        myResult = this.EntityPM.ExpectedAmount / this.EntityPM.Quantity;
                    }
                }

                this.EntityPM.UnitPrice = AppTool.Round(myResult, 3);
            }
        }
    }
    ComputeTotalAmount() {

        var iAmount: number = null;

        if (this.Quantity != null && this.UnitPrice != null) {
            if (this.MeasurementCode == "PRVL" || this.MeasurementCode == "PRFR") {
                var price = this.EntityPM.UnitPrice / 100;
                iAmount = this.EntityPM.Quantity * price;
            }

            else {
                iAmount = this.Quantity * this.UnitPrice;
            }
        }

        /* MinMax Tariff */
        if (iAmount != null) {
            if (this.MinAmount != null) {
                if (iAmount < this.MinAmount) {
                    iAmount = this.MinAmount;
                }
            }

            if (this.MaxAmount != null) {
                if (iAmount > this.MaxAmount) {
                    iAmount = this.MaxAmount;
                }
            }
        }

        /* MinMax Quote */
        if (iAmount != null) {
            if (this.QuoteCostMinAmount != null) {
                if (iAmount < this.QuoteCostMinAmount) {
                    iAmount = this.QuoteCostMinAmount;
                }
            }

            if (this.QuoteCostMaxAmount != null) {
                if (iAmount > this.QuoteCostMaxAmount) {
                    iAmount = this.QuoteCostMaxAmount;
                }
            }
        }
       
        this.EntityPM.ExpectedAmount = AppTool.Round(iAmount, 2);
        this.ComputeTotalAmountLocal();
        this.SetLineCells();
        this.OnLineAmountChanged();
        this.SetLineStatus();
    }
    ComputeTotalAmountLocal() {

        var myResult: number = null;

        if (this.EntityPM.ExpectedAmount != null && this.EntityPM.Rate != null) {
            myResult = AppTool.Round(this.EntityPM.ExpectedAmount * this.EntityPM.Rate, 2);
        }

        this.ExpectedAmountLocal = myResult;
        this.ComputeTotalAmountInProfitCurrency();
        this.ComputeInsidePayablesData();
    }
    ComputeTotalAmountInProfitCurrency() {

        if (this.EntityPM.CurrencyId == this.ShipmentPM.ProfitCurrencyId) {
            this.ExpectedAmountInProfitCurrency = this.EntityPM.ExpectedAmount;
        }

        else {
            this.ExpectedAmountInProfitCurrency = (this.ExpectedAmountLocal / this.EntityPM.ProfitCurrencyExchangeRate);
        }

        this.ComputeOtherAmounts();

        if (this.fatherComponent != null) {
            this.fatherComponent.ComputeShipmentFields();
        }
    }
    ComputeOtherAmounts() {
        if (this.EntityPM.ShipmentPayableLineStatusCode == "EMPT" || this.EntityPM.ShipmentPayableLineStatusCode == "OAMT") {
            this.EntityPM.CorrectionAmount = 0;
            this.EntityPM.AccountedAmount = 0;
            this.EntityPM.AccountedAmountInLocalCurrency = 0;
            this.EntityPM.AccountedAmountInProfitCurrency = 0;

            this.EntityPM.OpenAmount = this.EntityPM.ExpectedAmount;
            this.EntityPM.OpenAmountInLocalCurrency = this.EntityPM.ExpectedAmountLocal;
            this.EntityPM.OpenAmountInProfitCurrency = this.EntityPM.ExpectedAmountInProfitCurrency;

            this.IsOpenAmountVisible = this.OpenAmount != null && this.OpenAmount != 0;
        }

        if (this.fatherComponent != null) {
            this.fatherComponent.ComputeShipmentFields();
        }

        this.SetLineSummary();
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
    get CreateDate() { return this.EntityPM.CreateDate; }
    get UpdateDate() { return this.EntityPM.UpdateDate; }
    public CreatedByUserName: string = null;
    public UpdatedByUserName: string = null;
    public ReceivableSummary: number = null;
    public PayableSummary: number = null;
    public ProfitSummary: number = null;
    SetLineSummary() {

        if (AppTool.IsNullOrEmpty(this.UpdatedByUserName)) {       
            this.fatherComponent.myUserListService.getSingleFromCache(this.EntityPM.CreatedByUserId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: UserList = myResponse.Result;
                    if (list) {
                        this.CreatedByUserName = list.EnglishName;
                    }

                    else {
                        this.fatherComponent.myUserListService.getSingle(this.EntityPM.CreatedByUserId).subscribe((myResponse: ServiceResponse) => {
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
                this.fatherComponent.myUserListService.getSingleFromCache(this.EntityPM.UpdateByUserId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: UserList = myResponse.Result;
                        if (list) {
                            this.UpdatedByUserName = list.EnglishName;
                        }

                        else {
                            this.fatherComponent.myUserListService.getSingle(this.EntityPM.UpdateByUserId).subscribe((myResponse: ServiceResponse) => {
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

        if (this.ShipmentPM.ShipmentPayables.length > 0) {
            var openAmount = ArrayTool.Sum(this.ShipmentPM.ShipmentPayables.filter(f => f.ChargesTypeId == this.ChargesTypeId), "OpenAmountInLocalCurrency");
            var acctAmount = ArrayTool.Sum(this.ShipmentPM.ShipmentPayables.filter(f => f.ChargesTypeId == this.ChargesTypeId), "AccountedAmountInLocalCurrency");
            myPayableSummary = openAmount + acctAmount;
        }

        if (!ArrayTool.Contains(this.ShipmentPM.ShipmentPayables, this.EntityPM)) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.AccountedAmountInLocalCurrency)) {
                var entityOpenAmount = this.EntityPM.OpenAmountInLocalCurrency == null ? 0 : this.EntityPM.OpenAmountInLocalCurrency;
                var entityAcctAmount = this.EntityPM.AccountedAmountInLocalCurrency == null ? 0 : this.EntityPM.AccountedAmountInLocalCurrency;
                var entityAmount = entityOpenAmount + entityAcctAmount;
                myPayableSummary = myPayableSummary + entityAmount;
            }
        }

        this.ReceivableSummary = myReceivableSummary;
        this.PayableSummary = myPayableSummary;
        this.ProfitSummary = this.ReceivableSummary - this.PayableSummary;
    }

    // Invoice
    public PayableInvoices: any[] = [];
    private isInvoicesListLoaded: boolean = false;
    private isInvoicesTooltipOpened: boolean = false;
    get IsInvoicesTooltipOpened() { return this.isInvoicesTooltipOpened; }
    set IsInvoicesTooltipOpened(value: boolean) {
        if (this.isInvoicesTooltipOpened != value) {
            this.isInvoicesTooltipOpened = value;
            if (value) {
                if (!this.isInvoicesListLoaded) {
                    this.isInvoicesListLoaded = true;
                    
                    this.fatherComponent.myDomainService.GetPayableInvoices(this.EntityPM.Id, this.EntityPM.ShipmentPayableParentId).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {                           
                            this.PayableInvoices = myResponse.Result;                            
                        }
                    });
                }
            }
        }
    }

    // ChildPayables
    public SumOfQuantity: number;
    public SumOfAmount: number;
    public SumOfAmountLocal: number;
    BuildInsidePayables() {
        this.InsideItemsSource = [];

        this.EntityPM.ChildShipmentPayables.forEach(item => {
            var myConsoleShipmentPM: ConsoleShipmentPM = this.ShipmentPM.ShipmentConsoleShipments.filter(f => f.Id == item.ShipmentId)[0];
            var insidePayable = new InsidePayableViewModel(item, myConsoleShipmentPM, this.ShipmentPM.ProfitCurrencyId)
            this.InsideItemsSource.push(insidePayable);
        });

        this.ComputeInsidePayablesTotals();
    }   
    UpdateInsideItemsSource_Rate() {
        if (this.ShipmentPM.ShipmentLevelCode == "C") {
            this.InsideItemsSource.forEach(insidePayable => {
                insidePayable.Rate = this.Rate;
            });
        }
    }
    UpdateInsideItemsSource_Vendor() {
        if (this.ShipmentPM.ShipmentLevelCode == "C") {
            this.InsideItemsSource.forEach(insidePayable => {
                insidePayable.VendorId = this.VendorId;
                insidePayable.VendorName = this.VendorName;
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
    ComputeInsidePayablesData() {
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

                            case "GWTN": {
                                _QuantityTotal = ArrayTool.Sum(this.InsideItemsSource, "GrossWeightPerTon");
                                _Ratio = _QuantityTotal == 0 ? 0 : this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * this.UnitPrice;
                                quantity = item.GrossWeightPerTon;
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

                        var expectedAmount = quantity * unitPrice;

                        if (this.MeasurementCode == "PRVL" || this.MeasurementCode == "PRFR") {
                            expectedAmount = quantity * unitPrice / 100;
                        }

                        var expectedAmountLocal = expectedAmount * this.Rate;
                        var expectedAmountInProfitCurrency = expectedAmountLocal / this.ProfitCurrencyExchangeRate;

                        item.EntityPM.ExpectedAmount = AppTool.Round(expectedAmount, 3);
                        item.EntityPM.ExpectedAmountLocal = AppTool.Round(expectedAmountLocal, 3);
                        item.EntityPM.ExpectedAmountInProfitCurrency = AppTool.Round(expectedAmountInProfitCurrency, 3);

                        item.EntityPM.OpenAmount = item.EntityPM.ExpectedAmount;
                        item.EntityPM.OpenAmountInLocalCurrency = item.EntityPM.ExpectedAmountLocal;
                        item.EntityPM.OpenAmountInProfitCurrency = item.EntityPM.ExpectedAmountInProfitCurrency;

                        item.EntityPM.AccountedAmount = 0;
                        item.EntityPM.AccountedAmountInLocalCurrency = 0;
                        item.EntityPM.AccountedAmountInProfitCurrency = 0;
                    });

                    this.ComputeInsidePayablesTotals();
                }
            });
        } 
    }
    ComputeInsidePayablesTotals() {
        this.SumOfQuantity = ArrayTool.Sum(this.InsideItemsSource, "Quantity");
        this.SumOfAmount = ArrayTool.Sum(this.InsideItemsSource, "ExpectedAmount");
        this.SumOfAmountLocal = ArrayTool.Sum(this.InsideItemsSource, "ExpectedAmountLocal");
        this.SetExpectedAmountCell();
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
            case "PRFR": { result = ArrayTool.Sum(this.ShipmentPM.ShipmentPayables.filter(d => d.ChargesGroupCode == "FRT" && AppTool.IsNullOrEmpty(d.ShipmentPayableParentId)), "ExpectedAmount"); break; }
            case "GWTN": { result = this.ShipmentPM.GrossWeightPerTon; break; }
            case "QTY": { result = this.fatherComponent.IsLCLEntity ? this.ShipmentPM.NumberOfPackages : this.ShipmentPM.NumberOfContainers; break; }
            case "CWKG": { result = this.ShipmentPM.ChargeableWeightInKG; break; }
            case "GWKG": { result = this.ShipmentPM.GrossWeightInKG; break; }
            case "VCBM": { result = this.ShipmentPM.VolumeInCBM; break; }
            case "BCNT": {
                break;
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
}
export class InsidePayableViewModel {
    public EntityPM: ShipmentPayablePM;
    public ShipmentPM: ConsoleShipmentPM;
    public ProfitCurrencyId: string;
    constructor(entityPM: ShipmentPayablePM, myConsoleShipmentPM: ConsoleShipmentPM, profitCurrencyId: string) {
        this.EntityPM = entityPM;
        this.ShipmentPM = myConsoleShipmentPM;
        this.ProfitCurrencyId = profitCurrencyId;
        this.SetSatusTypeToolTip();        
    }

    public SatusTypeToolTip: string = null;
    SetSatusTypeToolTip() {
        var myResult: string;

        switch (this.EntityPM.ShipmentPayableLineStatusCode) {
            case "APPD": { myResult = TextCodeTranslator.Translate("Shipment.O.Payables.Approved"); break; }
            case "NOIN": { myResult = TextCodeTranslator.Translate("Shipment.O.Payables.NoInvoiceNeeded"); break; }
            case "OAMT": { myResult = TextCodeTranslator.Translate("Shipment.O.Payables.OpenAmount"); break; }
            case "ACCT": { myResult = TextCodeTranslator.Translate("Shipment.O.Payables.Accounted"); break; }
            case "PACC": { myResult = TextCodeTranslator.Translate("Shipment.O.Payables.PartiallyAccounted"); break; }
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

    // Payable Properties
    get ShipmentId() { return this.EntityPM.ShipmentId; }
    get ShipmentNumber() { return this.EntityPM.ShipmentNumber; }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newVaule: string) {
        if (this.EntityPM.Notes != newVaule) {
            this.EntityPM.Notes = newVaule;
        }
    }

    get VendorId() { return this.EntityPM.VendorId; }
    set VendorId(value: string) {
        if (this.EntityPM.VendorId != value) {
            this.EntityPM.VendorId = value;
        }
    }

    get VendorName() { return this.EntityPM.VendorName; }
    set VendorName(value: string) {
        if (this.EntityPM.VendorName != value) {
            this.EntityPM.VendorName = value;
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

            case "GWTN": {
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

    get ExpectedAmount() { return this.EntityPM.ExpectedAmount; }
    set ExpectedAmount(value: number) {
        if (this.EntityPM.ExpectedAmount != value) {
            this.EntityPM.ExpectedAmount = AppTool.Round(value, 2);
            this.ComputeUnitPrice();
            this.ComputeTotalAmountLocal();
        }
    }

    get ExpectedAmountLocal() { return this.EntityPM.ExpectedAmountLocal; }
    set ExpectedAmountLocal(value: number) {
        if (this.EntityPM.ExpectedAmountLocal != value) {
            this.EntityPM.ExpectedAmountLocal = AppTool.Round(value, 2);
            this.ComputeTotalAmountInProfitCurrency();
        }
    }

    get MinAmount() { return this.EntityPM.MinAmount; }
    set MinAmount(newVaule: number) {
        if (this.EntityPM.MinAmount != newVaule) {
            this.EntityPM.MinAmount = AppTool.Round(newVaule, 2);
            this.ComputeTotalAmount();
        }
    }

    get MaxAmount() { return this.EntityPM.MaxAmount; }
    set MaxAmount(newVaule: number) {
        if (this.EntityPM.MaxAmount != newVaule) {
            this.EntityPM.MaxAmount = AppTool.Round(newVaule, 2);
            this.ComputeTotalAmount();
        }
    }

    get QuoteCostMinAmount() { return this.EntityPM.QuoteCostMinAmount; }
    set QuoteCostMinAmount(newVaule: number) {
        if (this.EntityPM.QuoteCostMinAmount != newVaule) {
            this.EntityPM.QuoteCostMinAmount = AppTool.Round(newVaule, 2);
        }
    }

    get QuoteCostMaxAmount() { return this.EntityPM.QuoteCostMaxAmount; }
    set QuoteCostMaxAmount(newVaule: number) {
        if (this.EntityPM.QuoteCostMaxAmount != newVaule) {
            this.EntityPM.QuoteCostMaxAmount = AppTool.Round(newVaule, 2);
        }
    }

    SetLineStatus() {
        ShipmentTool.SetPayableLineStatus(this.EntityPM);
        this.SetSatusTypeToolTip();
    }
    ComputeUnitPrice() {
        if (!this.EntityPM.IsChargeBySteps) {

            var myResult: number = null;

            if (this.EntityPM.IsChargeBySteps) {

            }

            else {
                if (this.EntityPM.ExpectedAmount != null && this.EntityPM.Quantity != null) {
                    if (this.EntityPM.Quantity == 0) {
                        myResult = 0;
                    }

                    else {
                        myResult = this.EntityPM.ExpectedAmount / this.EntityPM.Quantity;
                    }
                }

                this.EntityPM.UnitPrice = AppTool.Round(myResult, 3);
            }
        }
    }
    ComputeTotalAmount() {

        var iAmount: number = null;

        this.SetLineStatus();

        if (this.Quantity != null && this.UnitPrice != null) {
            iAmount = this.Quantity * this.UnitPrice;

            if (this.MeasurementCode == "PRVL" || this.MeasurementCode == "PRFR") {
                iAmount = this.Quantity * this.UnitPrice / 100;
            }
        }

        /* MinMax Tariff */
        if (iAmount != null) {
            if (this.MinAmount != null) {
                if (iAmount < this.MinAmount) {
                    iAmount = this.MinAmount;
                }
            }

            if (this.MaxAmount != null) {
                if (iAmount > this.MaxAmount) {
                    iAmount = this.MaxAmount;
                }
            }
        }

        /* MinMax Quote */
        if (iAmount != null) {
            if (this.QuoteCostMinAmount != null) {
                if (iAmount < this.QuoteCostMinAmount) {
                    iAmount = this.QuoteCostMinAmount;
                }
            }

            if (this.QuoteCostMaxAmount != null) {
                if (iAmount > this.QuoteCostMaxAmount) {
                    iAmount = this.QuoteCostMaxAmount;
                }
            }
        }

        this.EntityPM.ExpectedAmount = AppTool.Round(iAmount, 2);
        this.ComputeTotalAmountLocal();
    }
    ComputeTotalAmountLocal() {

        var myResult: number = null;

        if (this.EntityPM.ExpectedAmount != null && this.EntityPM.Rate != null) {
            myResult = AppTool.Round(this.EntityPM.ExpectedAmount * this.EntityPM.Rate, 2);
        }

        this.ExpectedAmountLocal = myResult;
        this.ComputeTotalAmountInProfitCurrency();
    }
    ComputeTotalAmountInProfitCurrency() {

        if (this.EntityPM.CurrencyId == this.ProfitCurrencyId) {
            this.EntityPM.ExpectedAmountInProfitCurrency = this.EntityPM.ExpectedAmount;
        }

        else {
            this.EntityPM.ExpectedAmountInProfitCurrency = (this.ExpectedAmountLocal / this.EntityPM.ProfitCurrencyExchangeRate);
        }

        this.ComputeOtherAmounts();
    }
    ComputeOtherAmounts() {
        if (this.EntityPM.ShipmentPayableLineStatusCode == "EMPT" || this.EntityPM.ShipmentPayableLineStatusCode == "OAMT") {
            this.EntityPM.CorrectionAmount = 0;
            this.EntityPM.AccountedAmount = 0;
            this.EntityPM.AccountedAmountInLocalCurrency = 0;
            this.EntityPM.AccountedAmountInProfitCurrency = 0;

            if (this.EntityPM.OpenAmount != this.EntityPM.ExpectedAmount) {
                this.EntityPM.OpenAmount = this.EntityPM.ExpectedAmount;
            }

            else {
                this.EntityPM.OpenAmountInLocalCurrency = this.EntityPM.OpenAmount * this.EntityPM.Rate;
                this.EntityPM.OpenAmountInProfitCurrency = this.EntityPM.OpenAmountInLocalCurrency / this.EntityPM.ProfitCurrencyExchangeRate;
            }
        }
    }    
}
