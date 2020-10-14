import {Component, OnDestroy} from '@angular/core';
import {QuotePM} from '../../../Quote/EntityPMs/QuotePM';
import {QuoteChargePM} from '../../../Quote/EntityPMs/QuoteChargePM';
import {QuoteTotalVATPM} from '../../../Quote/EntityPMs/QuoteTotalVATPM';
import {QuoteUtilities} from '../../../Quote/Utilities/QuoteUtilities';
import {AppTool, DateTool, FontTool, ArrayTool} from '../../../Infrastructure/Tools';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {CardList} from '../../../Common/EntityLists/CardList';
import {VatTypeList} from '../../../Common/EntityLists/VatTypeList';
import {CurrencyList} from '../../../Common/EntityLists/CurrencyList';
import {ChargesTypeList} from '../../../Common/EntityLists/ChargesTypeList';
import {MeasurementList} from '../../../Common/EntityLists/MeasurementList';
import {CardListService} from '../../../Common/Services/StandardLists/CardListService';
import {VatTypeListService} from '../../../Common/Services/StandardLists/VatTypeListService';
import {CurrencyListService} from '../../../Common/Services/StandardLists/CurrencyListService';
import {ChargesTypeListService} from '../../../Common/Services/StandardLists/ChargesTypeListService';
import {MeasurementListService} from '../../../Common/Services/StandardLists/MeasurementListService';
import {CurrencyRatesService, LastRate} from '../../../Common/Services/CurrencyRatesService';
import {CommonDomainService} from '../../../Common/Services/CommonDomainService';
import {VatTypePercentagePM} from '../../../Common/EntityPMs/VatTypePercentagePM';
import {VATTypesGroupPM} from '../../../Common/EntityPMs/VATTypesGroupPM';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import { DecimalFormatter } from '../../../Infrastructure/Utilities/DecimalFormatter';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { QuoteTool } from '../../../Quote/Tools';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';

@Component({
    selector: 'LCLChargesComponent',    
    templateUrl: './LCLChargesComponent.html',
})

export class LCLChargesComponent extends BaseComponent implements OnDestroy {
    public EntityPM: QuotePM = null;
    public ObjectTableName: string = "Quote";
    public DataContext = this;
    public ItemsSource: ObservableCollection;
    public IsAdhoc: boolean = false;
    public IsRoutingRate: boolean = false;
    public TransportModeId: string;
    public LocalCurrencyId: string;
    public LocalCurrencyCode: string;
    public DisplayTariffs: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    public CurrentSession = SessionLocator.SelectedSession;
    IsRouteRate: boolean = false;
    public IsPriceCheckVisible: boolean = false;
    private entityResourceService: EntityResourceService = new EntityResourceService();;
    public ComponentRef: any;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.TransportModeId = this.EntityPM.TransportModeId;
        this.IsAdhoc = this.EntityPM.QuoteTypeCode == "A" ? true : false;
        this.IsRoutingRate = !this.IsAdhoc;
        this.LocalCurrencyId = SessionLocator.LocalCurrencyId;
        this.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
        this.ItemsSource = new ObservableCollection([]);

        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }
        
        if (this.EntityPM.TransportModeId == "A" && FeatureLocator.HasFeaturePermession(this.ObjectTableName, "TARIFFS")) {
            this.DisplayTariffs = true;
        }

        this.IsPriceCheckVisible = QuoteUtilities.IsPriceCheckVisible(this.EntityPM);
        
        this.InitializeServices();
        this.LoadRequiredData();
        this.SetLabels();
        this.SetUIProperties();
        this.CheckUpdateQuantities();
        this.BuildItemsSource();
        this.InitializeProfit();
        this.GetQuoteType();
        this.Listen();
        this.SetRegionalTaxVisibility();
    }

    GetQuoteType() {
        if (this.EntityPM != null) {
            if (this.EntityPM.QuoteTypeCode == "P") {
                this.IsRouteRate = true;
            }
        }
    }

    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null; 
    Listen() {
        if (this.entityArgs.EditComponent) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.LoadRequiredData();
                    this.SetUIProperties();
                    this.BuildItemsSource();
                    this.BuildProfitData();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.LoadRequiredData();
                    this.SetUIProperties();
                    this.BuildItemsSource();
                    this.BuildProfitData();
                }
            });

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "QTCH") {
                    if (this.IsAdhoc) {
                        if (this.ItemsSource.Collection.filter(d => d.SaleUnitPrice != null || d.CostUnitPrice != null).length > 0) {
                            this.CheckUpdateQuantities();
                        }

                        else {
                            this.UpdateQuantitiesClicked();
                        }
                    }
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    myCardListService: CardListService;
    myVatTypeService: VatTypeListService;
    myCurrencyService: CurrencyListService;
    myChargesTypeService: ChargesTypeListService;
    myMeasurementService: MeasurementListService;
    myCurrencyRatesService: CurrencyRatesService;
    myCommonDomainService: CommonDomainService;
    InitializeServices() {
        this.myCardListService = new CardListService();
        this.myVatTypeService = new VatTypeListService();
        this.myCurrencyService = new CurrencyListService();
        this.myChargesTypeService = new ChargesTypeListService();
        this.myMeasurementService = new MeasurementListService();
        this.myCurrencyRatesService = new CurrencyRatesService();
        this.myCommonDomainService = new CommonDomainService();
    }

    public AllRates: LastRate[] = [];
    public AllCurrencies: CurrencyList[] = [];
    public AllMeasurements: MeasurementList[] = [];
    public AllVatTypes: VatTypeList[] = [];
    public AllVatPercentages: VatTypePercentagePM[] = [];
    LoadRequiredData() {
        var isEditingEnabled = QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);
        if (isEditingEnabled) {

            this.myCurrencyRatesService.getAll(this.LocalCurrencyId, DateTool.GetCurrentDateAsUtc()).subscribe((myResponse0: ServiceResponse) => {
                if (!myResponse0.HasError) {
                    this.AllRates = myResponse0.Result;
                }
            });

            this.myCommonDomainService.GetVatTypePercentagePMByDate(DateTool.GetCurrentDateAsUtc()).subscribe((myResponse1: ServiceResponse) => {
                if (!myResponse1.HasError) {
                    this.AllVatPercentages = myResponse1.Result;
                }
            });
        }

        this.myCurrencyService.getAllFromCache().subscribe((myResponse2: ServiceResponse) => {
            if (!myResponse2.HasError) {
                this.AllCurrencies = myResponse2.Result;
            }
        });

        this.myMeasurementService.getAllFromCache().subscribe((myResponse3: ServiceResponse) => {
            if (!myResponse3.HasError) {
                this.AllMeasurements = myResponse3.Result;
            }
        });

        this.myVatTypeService.getAllFromCache().subscribe((myResponse4: ServiceResponse) => {
            if (!myResponse4.HasError) {
                this.AllVatTypes = myResponse4.Result;
            }
        });
    }
    GetVatTypePercentage(vatTypeId: string) {
        var myResult: number = null;

        var vatTypePercentagePM = this.AllVatPercentages.filter(d => d.VatTypeId == vatTypeId)[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
        }

        return myResult;
    }

    // SetLabels
    public CostQuentityHeader: any[] = [];
    public CostPriceHeader: any[] = [];
    public CostAmountHeader: any[] = [];
    public SaleQuantityHeader: any = [];
    public SaleLocalAmountHeader: any[] = [];
    public SalePriceHeader: any = [];
    public SaleAmountHeader: any = [];
    public CostMinAmountHeader: any = [];
    public CostMaxAmountHeader: any = [];
    public SaleMinAmountHeader: any = [];
    public SaleMaxAmountHeader: any = [];
    SetLabels() {
        this.CostQuentityHeader = TextCodeTranslator.Translate("Quote.O.Charges.CostQuantity", false).split('%n');
        this.CostPriceHeader = TextCodeTranslator.Translate("Quote.O.Charges.CostPrice", false).split('%n');
        this.CostAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.CostAmount", false).split('%n');
        this.SaleQuantityHeader = TextCodeTranslator.Translate("Quote.O.Charges.SaleQuantity", false).split('%n');
        this.SaleLocalAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.SaleAmountLocal", false).replace("%LocalCurrencyCode", this.LocalCurrencyCode).split('%n');
        this.CostMinAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.CostMinAmount", false).split('%n');
        this.CostMaxAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.CostMaxAmount", false).split('%n');
        this.SaleMinAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.SaleMinAmount", false).split('%n');
        this.SaleMaxAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.SaleMaxAmount", false).split('%n');
        this.SetLabelsAttached();
    }
    SetLabelsAttached() {
        if (this.IsSaleCurrencySameAsCost) {
            this.SalePriceHeader = TextCodeTranslator.Translate("Quote.O.Charges.SalePrice", false).replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").split('%n');
            this.SaleAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.SaleAmount", false).replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").split('%n');
        }

        else {
            this.SalePriceHeader = TextCodeTranslator.Translate("Quote.O.Charges.SalePrice", false).replace("%SaleCurrencyCode", this.SaleCurrencyCode).split('%n');
            this.SaleAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.SaleAmount", false).replace("%SaleCurrencyCode", this.SaleCurrencyCode).split('%n');
        }
    }

    public VATColumnWidth: number = 100;
    SetGridColumnsWidth() {
        var myVATColumnWidth = 100;

        this.ItemsSource.Collection.forEach(item => {

            var myVATTextWidth = AppTool.GetTextWidth(item.VatTypeCell, 12) + 10;
            if (item.VatTypeUpdateIsVisible) {
                myVATTextWidth += 25;
            }

            if (myVATTextWidth > myVATColumnWidth) {
                myVATColumnWidth = myVATTextWidth;
            }
        });

        this.VATColumnWidth = myVATColumnWidth;
    }

    // SetUIProperties
    public IsEditingEnabled: boolean = false;
    public IsExchangeRateEnabled: boolean = true;
    public IsCurrencyFilterVisible: boolean = true;
    SetUIProperties() {
        this.IsEditingEnabled = QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);

        this.ItemsSource.Collection.forEach((item: QuoteChargeItem) => {
            item.SetUIProperties();
        });

        this.UIProperties.SetEnabled("RegionalTaxId", this.ObjectTableName, this.IsEditingEnabled);

        this.SetUIProperties_Summary();
    }
    SetUIProperties_Summary() {
        var isExchangeRateEnabled = false;

        if (this.IsEditingEnabled) {
            if (FeatureLocator.HasFeaturePermession("Quote", "QouteEditExchangeRate")) {
                if (this.SaleCurrencyId) {
                    if (this.SaleCurrencyId != SessionLocator.LocalCurrencyId) {
                        isExchangeRateEnabled = true;
                    }
                }                 
            }
        }      

        this.IsExchangeRateEnabled = isExchangeRateEnabled;
        this.IsCurrencyFilterVisible = this.LocalCurrencyId == this.EntityPM.SaleCurrencyId ? false : true;
        this.UIProperties.SetEnabled("SaleCurrencyId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ExchangeRate", this.ObjectTableName, isExchangeRateEnabled);
        this.UIProperties.SetEnabled("IsFixedPrice", this.ObjectTableName, this.IsEditingEnabled);
    }


    public SelectedRow: QuoteChargeItem = null;
    OnRowSelected(itemComponent: QuoteChargeItem) {
        this.SelectedRow = itemComponent;
    }

    BuildItemsSource() {

        var itemsCollection: QuoteChargeItem[] = [];

        this.EntityPM.QuoteCharges.filter(f => f.ChargesGroupCode == "FRT").forEach((item) => {
            itemsCollection.push(new QuoteChargeItem(item, this, false));
        })

        this.EntityPM.QuoteCharges.filter(f => f.ChargesGroupCode != "FRT").forEach((item) => {
            itemsCollection.push(new QuoteChargeItem(item, this, false));
        })

        this.ItemsSource.InsertCollection(itemsCollection);
        this.SetGridColumnsWidth();
    }

    // Commands
    AddChargeClicked() {        
            var newItem = new QuoteChargePM(null);
            newItem.Tenant = SessionLocator.Tenant;
            newItem.QuoteId = this.EntityPM.Id;            
            newItem.UpdatedByUserId = SessionLocator.LoggedUserId;
            newItem.MarkUpTypeCode = "F";
            newItem.MarkUpValue = 0;
            newItem.QuoteTypeCode = this.EntityPM.QuoteTypeCode;
            newItem.SaleCurrencyId = this.EntityPM.SaleCurrencyId;
            newItem.SaleCurrencyCode = this.EntityPM.SaleCurrencyCode;
            newItem.SaleExchangeRate = this.EntityPM.ExchangeRate;
            newItem.IsAllIN = false;
            newItem.CostIsFixedRate = false;
            newItem.SaleIsFixedRate = false;
            newItem.IsChargeBySteps = false;
            var itemComponent = new QuoteChargeItem(newItem, this, true);

            var title = TextCodeTranslator.Translate("Quote.O.Charges.AddCharges");
            this.RunAddEditCharge(itemComponent, title);
             
    }
    EditChargeClicked(itemComponent: QuoteChargeItem) {
        var title = TextCodeTranslator.Translate("Quote.O.Charges.EditCharges");
        this.RunAddEditCharge(itemComponent, title);
    }
    DeleteChargeClicked(itemComponent: QuoteChargeItem) {
        if ((itemComponent.EntityPM.ChargesGroupCode == "FRT" && this.EntityPM.QuoteCharges.filter(d => d.IsAllIN).length > 0) ||
            (itemComponent.EntityPM.ChargesGroupCode == "FRT" && this.EntityPM.QuoteCharges.filter(d => d.IsCostAllIn).length > 0)) {
            var window = new MessageWindow();
            window.Show("Can't delete this charge because it's connected to other All In charges");            
        }

        else if (itemComponent.EntityPM.IsAllIN) {
            var window = new MessageWindow();
            window.Show("Can't delete this charge because it's All In");            
        }

        else {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator.Translate("Quote.M.DeleteThisCharge"));
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.EntityPM.RemoveQuoteChargePM(itemComponent.EntityPM);

                    if (itemComponent.ChargesGroupCode == "FRT") {
                        this.OnFreightAmountChanged();
                    }

                    this.BuildItemsSource();
                    this.ComputeTotals();
                }
            });
        }
    }
    RunAddEditCharge(itemComponent: QuoteChargeItem, windowTitle: string) {

        itemComponent.SetEditScreenGridHeaders();

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.Width = 880;
        logitudeWindow.Height = 550;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./QuoteModules/QuoteCharges/Components/AddEditLCLChargeComponent');
    }
    ShowTariffsClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = TextCodeTranslator.Translate("TariffHeader.O.Tariffs")
        logWindow.WindowArgs = this;
        logWindow.Show('./QuoteModules/QuoteTabs/Components/Tariffs/TariffsComponent');
       
    }
    GetCurrencyCode(myCurrencyId: string) {
        var myCode = null;

        if (!AppTool.IsNullOrEmpty(myCurrencyId)) {
            var list: CurrencyList = this.AllCurrencies.filter(d => d.Id == myCurrencyId)[0];
            if (list != null) {
                myCode = list.Code;
            }
        }

        return myCode;
    }
    GetCurrencyRate(myCurrencyId: string) {
        var myResult = null;

        if (!AppTool.IsNullOrEmpty(myCurrencyId)) {
            if (myCurrencyId == SessionLocator.LocalCurrencyId) {
                myResult = 1;
            }

            else if (myCurrencyId == this.SaleCurrencyId) {
                myResult = this.ExchangeRate;
            }

            else {
                var lastRate: LastRate = this.AllRates.filter(d => d.ForeignCurrencyId == myCurrencyId)[0];
                if (lastRate != null) {
                    myResult = lastRate.Rate;
                }
            }
        }

        return myResult;
    }
    GetCurrencyRateDate(myCurrencyId: string) {
        var myResult = null;

        if (!AppTool.IsNullOrEmpty(myCurrencyId)) {
            if (myCurrencyId == SessionLocator.LocalCurrencyId) {
                myResult = null;
            }

            else {
                var lastRate: LastRate = this.AllRates.filter(d => d.ForeignCurrencyId == myCurrencyId)[0];
                if (lastRate != null) {
                    myResult = lastRate.ValueDate;
                }
            }
        }

        return myResult;
    }

    PriceCheck() {
        this.entityResourceService.getEntityResourceByTableName("TariffLine").subscribe((res1: any) => {
            ServiceLocator.SendTotangoUserActivity("Tariff", "Generate from Quote");

            var betweenDate: Date = DateTool.GetCurrentDateAsUtc();

            if (this.EntityPM.DirectionId == "I") {
                betweenDate = this.EntityPM.ETA;
            }
            else  {
                betweenDate = this.EntityPM.ETD;
            }

            var tariffType = "";
            if (this.EntityPM.TransportModeId == "O" && QuoteUtilities.IsLCLQuote(this.EntityPM)){
                tariffType = "OLC";
            }

            else if (this.EntityPM.TransportModeId == "A") {
                tariffType = "AFC";
            }
            
            var WindowArgs: any =
            {
                BetweenDate: betweenDate,
                FromPort: this.EntityPM.FromPortId,
                ToPort: this.EntityPM.ToPortId,
                GrossWeight: this.EntityPM.GrossWeight,
                ChargeableWeight: this.EntityPM.ChargeableWeight,
                Volume: this.EntityPM.Volume,
                ChargeableWeightUnit: this.EntityPM.ChargeableWeightUnitCode,
                GrossWeightUnit: this.EntityPM.GrossWeightUnitCode,
                VolumeUnit: this.EntityPM.VolumeUnitCode,
                IsQuote: true,
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
    EditTariffClicked(item: QuoteChargeItem) {
        if (item != null) {
            var editWindow = new LogitudeWindow();
            editWindow.ShowHeaderButtons = true;
            editWindow.Title = "Price Check";
            editWindow.Height = 770;
            editWindow.Width = 1500;
            editWindow.ShowEditComponent(item.TariffId, "Tariff", item.TariffVersion + "");
        }
    }

    DeleteTariff(item: QuoteChargeItem) {
        if (item != null) {
            item.TariffId = null;
            item.TariffNumber = null;
            item.SetUIProperties();
        }
    }

    // Profit
    InitializeProfit() {
        this.SelectedCurrencyCode = this.SaleCurrencyCode;
        this.IsSameCostCurrency = this.IsSaleCurrencySameAsCost;
        this.IsFixedCurrency = !this.IsSameCostCurrency;
        this.BuildProfitData();
    }

    IsLocalCurrency: boolean = false;
    OnSelectCurrency(mySelectedCode: string) {
        this.SelectedCurrencyCode = mySelectedCode;

        if (mySelectedCode == this.LocalCurrencyCode) {
            this.IsLocalCurrency = true;
        }

        else {
            this.IsLocalCurrency = false;
        }

        this.BuildProfitData();
    }
    SetFixedSameCurrency(setType: string) {
        this.IsFixedCurrency = null;
        this.IsSameCostCurrency = null;
        var allInItems = this.ItemsSource.Collection.filter(d => d.IsAllIN == true);

        if (setType == "F") {
            
            allInItems.forEach((item: QuoteChargeItem) => {
                item.IsAllIN = false;
            });

            this.IsFixedCurrency = true;
            this.IsSameCostCurrency = false;
            this.IsSaleCurrencySameAsCost = false;
            this.OnFixedSameChanges();

            allInItems.forEach((item: QuoteChargeItem) => {
                item.IsAllIN = true;
            });
        }

        else {
            var itemFrieght = this.EntityPM.QuoteCharges.filter(f => f.ChargesGroupCode == "FRT")[0];

            if (this.EntityPM.QuoteCharges.filter(d => d.IsAllIN == true && d.CostCurrencyId != itemFrieght.CostCurrencyId).length > 0) {
                var messageWindow = new MessageWindow();
                messageWindow.Show("You can't switch to multi-currency mode till you drop the all-in checks");
                messageWindow.WindowClosed.subscribe((event: any) => {
                    this.IsFixedCurrency = true;
                    this.IsSameCostCurrency = false;
                    this.IsSaleCurrencySameAsCost = false;
                });
            }

            else {
                allInItems.forEach((item: QuoteChargeItem) => {
                    item.IsAllIN = false;
                });

                this.IsFixedCurrency = false;
                this.IsSameCostCurrency = true;
                this.IsSaleCurrencySameAsCost = true;
                this.OnFixedSameChanges();

                allInItems.forEach((item: QuoteChargeItem) => {
                    item.IsAllIN = true;
                });
            }
        }
    }
    OnFixedSameChanges() {

        this.ItemsSource.Collection.forEach((item: QuoteChargeItem) => {

            if (item.CostCurrencyId) {
                if (item.CostCurrencyId != this.EntityPM.SaleCurrencyId) {

                    if (AppTool.IsNullOrEmpty(item.CostUnitPrice)) {
                        item.SaleUnitPrice = null;
                    }
                }
            }

            item.OnQuoteSaleCurrencySameAsCost();
        })
    }

    private selectedCurrencyCode: string;
    get SelectedCurrencyCode() { return this.selectedCurrencyCode; }
    set SelectedCurrencyCode(value: string) {
        if (AppTool.IsNullOrEmpty(value)) {
            this.selectedCurrencyCode = "";
        }

        else {
            this.selectedCurrencyCode = value;
        }
    }

    private isFixedCurrency: boolean = false;
    get IsFixedCurrency() { return this.isFixedCurrency; }
    set IsFixedCurrency(value: boolean) {
        if (this.isFixedCurrency != value) {
            this.isFixedCurrency = value;
        }
    }

    private isSameCostCurrency: boolean = false;
    get IsSameCostCurrency() { return this.isSameCostCurrency; }
    set IsSameCostCurrency(value: boolean) {
        if (this.isSameCostCurrency != value) {
            this.isSameCostCurrency = value;
        }
    }

    get IsSaleCurrencySameAsCost() { return this.EntityPM.IsSaleCurrencySameAsCost; }
    set IsSaleCurrencySameAsCost(newValue: boolean) {
        if (this.EntityPM.IsSaleCurrencySameAsCost != newValue) {
            this.EntityPM.IsSaleCurrencySameAsCost = newValue;
            this.SetLabelsAttached();
        }
    }

    get SaleCurrencyId() { return this.EntityPM.SaleCurrencyId; }
    set SaleCurrencyId(value: string) {
        if (this.EntityPM.SaleCurrencyId != value) {
            this.EntityPM.SaleCurrencyId = value;
            this.SetUIProperties_Summary();
            var allInItems = this.ItemsSource.Collection.filter(d => d.IsAllIN == true);

            this.SaleCurrencyCode = this.SelectedCurrencyCode = this.GetCurrencyCode(value);

            var myExchangeRate = null;

            if (!AppTool.IsNullOrEmpty(value)) {
                if (value == SessionLocator.TenantPM.CurrencyId) {
                    myExchangeRate = 1;
                }

                else {
                    var lastRate: LastRate = this.AllRates.filter(d => d.ForeignCurrencyId == value)[0];
                    if (lastRate != null) {
                        myExchangeRate = lastRate.Rate;
                    }
                }
            }

            if (this.ExchangeRate != myExchangeRate) {
                this.ExchangeRate = myExchangeRate;
            }

            else {

                allInItems.forEach((item: QuoteChargeItem) => {
                    item.IsAllIN = false;
                });

                this.ItemsSource.Collection.forEach((item: QuoteChargeItem) => {
                    item.OnQuoteSaleCurrencyChanged();
                });

                allInItems.forEach((item: QuoteChargeItem) => {
                    item.IsAllIN = true;
                });

                this.ComputeTotals();
            }
        }
    }

    get SaleCurrencyCode() { return this.EntityPM.SaleCurrencyCode; }
    set SaleCurrencyCode(value: string) {
        if (this.EntityPM.SaleCurrencyCode != value) {
            this.EntityPM.SaleCurrencyCode = value;
            this.SetLabelsAttached();
        }
    }
    
    get ExchangeRate() { return this.EntityPM.ExchangeRate; }
    set ExchangeRate(newValue: number) {
        if (this.EntityPM.ExchangeRate != newValue) {
            this.EntityPM.ExchangeRate = AppTool.Round(newValue, 5);
            var allInItems = this.ItemsSource.Collection.filter(d => d.IsAllIN == true);

            allInItems.forEach((item: QuoteChargeItem) => {
                item.IsAllIN = false;
            });

            this.ItemsSource.Collection.forEach((item: QuoteChargeItem) => {
                item.OnQuoteSaleCurrencyChanged();
            });

            allInItems.forEach((item: QuoteChargeItem) => {
                item.IsAllIN = true;
            });

            this.ComputeTotals();
        }
    }

    UpdateCurrencyRateClicked() {
        var loadingDate = DateTool.GetCurrentDateAsUtc();

        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 350;
        logWindow.Title = "Update Currency Rate";
        logWindow.WindowArgs = { CurrencyId: this.SaleCurrencyId, CurrencyCode: this.SaleCurrencyCode, Rate: this.ExchangeRate, Date: loadingDate };
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.AllRates = comp.RatesList;
                    this.ExchangeRate = AppTool.Round(comp.Rate, 5);
                }
            });
        });

        logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
    }

    get IsFixedPrice() { return this.EntityPM.IsFixedPrice; }
    set IsFixedPrice(newValue: boolean) {
        if (this.EntityPM.IsFixedPrice != newValue) {
            this.EntityPM.IsFixedPrice = newValue;
        }
    }

    get EstimateProfit() { return this.EntityPM.EstimateProfit; }
    set EstimateProfit(value: number) {
        if (this.EntityPM.EstimateProfit != value) {
            this.EntityPM.EstimateProfit = AppTool.Round(value, 2);
        }
    }

    get EstimatedProfitInLocal() { return this.EntityPM.EstimatedProfitInLocal; }
    set EstimatedProfitInLocal(value: number) {
        if (this.EntityPM.EstimatedProfitInLocal != value) {
            this.EntityPM.EstimatedProfitInLocal = AppTool.Round(value, 2);
        }
    }

    get EstimatedProfitInProfit() { return this.EntityPM.EstimatedProfitInProfit; }
    set EstimatedProfitInProfit(value: number) {
        if (this.EntityPM.EstimatedProfitInProfit != value) {
            this.EntityPM.EstimatedProfitInProfit = AppTool.Round(value, 2);
        }
    }

    get EstimateProfitEdited() { return this.EntityPM.EstimateProfitEdited; }
    set EstimateProfitEdited(newValue: boolean) {
        if (this.EntityPM.EstimateProfitEdited != newValue) {
            this.EntityPM.EstimateProfitEdited = newValue;
        }
    }

    public SummaryHeader: string = "";
    public SummaryCostAmount: number = 0;
    public SummarySaleAmount: number = 0;
    public SummaryMarkupAmount: number = 0;
    public SummaryProfitAmount: number = 0;
    public SummaryProfitColor: string = FontTool.Black;
    public SubTotal: number = 0;
    public TotalVAT: number = 0;
    public TotalSale: number = 0;
    BuildProfitData() {

        this.SummaryCostAmount = 0;
        this.SummarySaleAmount = 0;
        this.SummaryProfitAmount = 0;
        this.SummaryMarkupAmount = 0;
        this.SummaryProfitColor = FontTool.Black;
        this.SubTotal = 0;
        this.TotalVAT = 0;
        this.TotalSale = 0;

        if (this.EntityPM) {
            var myCostAmountLocal = AppTool.Round(ArrayTool.Sum(this.EntityPM.QuoteCharges, "CostTotalAmountLocal"), 2);
            var mySaleAmountLocal = AppTool.Round(ArrayTool.Sum(this.EntityPM.QuoteCharges.filter(f => f.IsAllIN == false), "SaleTotalAmountLocal"), 2);
            var mySaleProfitLocal = AppTool.Round(mySaleAmountLocal - myCostAmountLocal, 2);

            if (this.IsLocalCurrency) {
                this.SummaryHeader = TextCodeTranslator.Translate("Quote.S.Charges.ProfitInLocalCurrency") + " (" + this.SelectedCurrencyCode + ")";
                this.SummaryCostAmount = AppTool.Round(myCostAmountLocal, 2);
                this.SummarySaleAmount = AppTool.Round(mySaleAmountLocal, 2);
                this.SummaryProfitAmount = AppTool.Round(mySaleProfitLocal, 2);

                this.SubTotal = this.SummarySaleAmount;
                this.TotalVAT = AppTool.Round(ArrayTool.Sum(this.EntityPM.TotalVATs, "LocalCurrencyVATAmount"), 2);
                this.TotalSale = this.SubTotal + this.TotalVAT;
            }

            else {
                this.SummaryHeader = TextCodeTranslator.Translate("Quote.S.Charges.ProfitInSaleCurrency") + " (" + this.SelectedCurrencyCode + ")";
                if (!AppTool.IsNullOrZero(this.ExchangeRate)) {
                    this.SummaryCostAmount = AppTool.Round(myCostAmountLocal / this.ExchangeRate, 2);
                    this.SummarySaleAmount = AppTool.Round(mySaleAmountLocal / this.ExchangeRate, 2);
                    this.SummaryProfitAmount = AppTool.Round(mySaleProfitLocal / this.ExchangeRate, 2);

                    this.SubTotal = this.SummarySaleAmount;
                    this.TotalVAT = AppTool.Round(ArrayTool.Sum(this.EntityPM.TotalVATs, "QuoteCurrencyVATAmount"), 2);
                    this.TotalSale = this.SubTotal + this.TotalVAT;
                }
            }

            if (this.SummaryCostAmount < this.SummarySaleAmount) {
                this.SummaryProfitColor = FontTool.Green;
            }

            else if (this.SummaryCostAmount > this.SummarySaleAmount) {
                this.SummaryProfitColor = FontTool.Red
            }

            if (!AppTool.IsNullOrEmpty(this.SummaryCostAmount) && !AppTool.IsNullOrEmpty(this.SummarySaleAmount)) {
                if (!AppTool.IsNullOrZero(this.SummaryCostAmount)){
                    this.SummaryMarkupAmount = ((this.SummarySaleAmount - this.SummaryCostAmount) * 100) / this.SummaryCostAmount;
                }
            }
        }
    }
    
    // Update Quantities
    public UpdateQuantitiesMessage: string;
    public UpdateQuantitiesMessageWidth: number = 0;
    public IsUpdateQuantitiesVisible: boolean = false;
    CheckUpdateQuantities() {
        if (this.IsAdhoc) {
            var updateMessage = null;

            if (this.EntityPM.QuoteCharges.filter(d => d.SaleUnitPrice != null || d.CostUnitPrice != null).length > 0) {

                var list: any[] = [];
                var entityQuantity: number = null;
                var isDifferentOrders: boolean = false;
                var isDifferentPRVL: boolean = false;
                var isDifferentPRFR: boolean = false;
                
                //"GRWT"
                entityQuantity = this.EntityPM.GrossWeight;
                if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "GRWT" && f.CostQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "GRWT" && f.SaleQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }
                
                //"GWTN"
                entityQuantity = this.EntityPM.GrossWeightPerTon;
                if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "GWTN" && f.CostQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "GWTN" && f.SaleQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }

                //"CHWT"
                entityQuantity = this.EntityPM.ChargeableWeight;
                if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "CHWT" && f.CostQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "CHWT" && f.SaleQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }

                //CWKG
                entityQuantity = this.EntityPM.ChargeableWeightInKG;
                if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "CWKG" && f.CostQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "CWKG" && f.SaleQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }

                 //GWKG
                entityQuantity = this.EntityPM.GrossWeightInKG;
                if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "GWKG" && f.CostQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "GWKG" && f.SaleQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }

                //"VOLU"
                entityQuantity = this.EntityPM.Volume;
                if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "VOLU" && f.CostQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "VOLU" && f.SaleQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }

                //"VCBM"
                entityQuantity = this.EntityPM.VolumeInCBM;
                if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "VCBM" && f.CostQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "VCBM" && f.SaleQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }

                // PDCW
                entityQuantity = this.EntityPM.PickupDeliveryChargeableWeight;
                if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "PDCW" && f.CostQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "PDCW" && f.SaleQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }

                //"BTEU"
                entityQuantity = this.EntityPM.TEU;
                if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "BTEU" && f.CostQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "BTEU" && f.SaleQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }

                //"QTY"
                entityQuantity = this.EntityPM.NumberOfPackages;
                if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "QTY" && f.CostQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "QTY" && f.SaleQuantity != entityQuantity).length > 0) {
                    isDifferentOrders = true;
                }

                //"PRVL"
                entityQuantity = this.EntityPM.ValueOfGoods;
                if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "PRVL" && f.CostQuantity != entityQuantity).length > 0) {
                    isDifferentPRVL = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "PRVL" && f.SaleQuantity != entityQuantity).length > 0) {
                    isDifferentPRVL = true;
                }
                
                //"PRFR"
                if (this.EntityPM.QuoteCharges.filter(f => f.ChargesGroupCode == "FRT").length > 0) {
                    if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "PRFR" || f.SaleMeasurementCode == "PRFR").length > 0) {

                        var FRT_CostQuantity = this.EntityPM.QuoteCharges.filter(f => f.ChargesGroupCode == "FRT")[0].CostTotalAmount;
                        var FRT_SaleQuantity = this.EntityPM.QuoteCharges.filter(f => f.ChargesGroupCode == "FRT")[0].SaleTotalAmount;

                        if (AppTool.IsNullOrZero(FRT_CostQuantity)) {
                            FRT_CostQuantity = 0;
                        }

                        if (AppTool.IsNullOrZero(FRT_SaleQuantity)) {
                            FRT_SaleQuantity = 0;
                        }

                        if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "PRFR" && f.CostQuantity != null && f.CostQuantity != 0 && f.CostQuantity != FRT_CostQuantity).length > 0) {
                            isDifferentPRFR = true;
                        }

                        if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "PRFR" && f.SaleQuantity != null && f.SaleQuantity != 0 && f.SaleQuantity != FRT_SaleQuantity).length > 0) {
                            isDifferentPRFR = true;
                        }
                    }
                }

                if (isDifferentOrders) {
                    updateMessage = "You have updated the expected order details, apply the new values?";
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
    }
    UpdateQuantitiesClicked() {
        if (this.IsAdhoc) {
            this.ItemsSource.Collection.forEach((item) => {
                item.SetCostQuantity();
                item.SetSaleQuantity();
            })

            this.CurrentSession.SessionEvent.emit("QuantitiesUpdated");
            this.CheckUpdateQuantities();
        }
    }

    ComputeTotals() {
        this.EstimateProfitEdited = false;
        this.BuildTotalVATs();
        this.BuildProfitData();

        var myProfitInSaleCurrency = this.SummaryProfitAmount;
        if (this.IsLocalCurrency) {
            myProfitInSaleCurrency = this.ExchangeRate ? this.SummaryProfitAmount / this.ExchangeRate : null;
        }
        
        if (this.EntityPM.EstimateProfit != myProfitInSaleCurrency) {
            this.EntityPM.EstimateProfit = AppTool.Round(myProfitInSaleCurrency, 2);
        }

        this.SetUIProperties();
    }

    get IsChargesByVAT() { return this.EntityPM.IsChargesByVAT; }
    set IsChargesByVAT(value: boolean) {
        if (this.EntityPM.IsChargesByVAT != value) {
            this.EntityPM.IsChargesByVAT = value;

            this.ItemsSource.Collection.forEach((item: QuoteChargeItem) => {
                if (value == true) {
                    this.myChargesTypeService.getSingleFromCache(item.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var list: ChargesTypeList = myResponse.Result;
                            if (list) {
                                item.VatTypeId = list.VatTypeId;
                            }
                        }
                    });
                }

                else {
                    item.VatTypeId = null;
                    item.IsRegionalTax = false;
                }

                item.SetUIProperties_VAT();
            });

            this.SetGridColumnsWidth();
        }
    }

    BuildTotalVATs() {
        this.EntityPM.TotalVATs = [];

        if (this.EntityPM.QuoteTypeCode == "A") {
            if (this.IsChargesByVAT) {
                var myCharges: QuoteChargePM[] = this.EntityPM.QuoteCharges.filter(f => f.IsAllIN == false && f.VatTypeId != null);
                if (myCharges.length > 0) {


                    // Build Group Source
                    var group_Source: QuoteTotalVATPM[] = [];
                    myCharges.forEach(item => {
                        var lineVatType = this.AllVatTypes.filter(f => f.Id == item.VatTypeId)[0];
                        if (lineVatType) {

                            if (AppTool.IsNullOrEmpty(item.SaleTotalAmount)) {
                                item.SaleTotalAmount = 0;
                            }

                            if (AppTool.IsNullOrEmpty(item.SaleTotalAmountLocal)) {
                                item.SaleTotalAmountLocal = 0;
                            }

                            if (AppTool.IsNullOrEmpty(item.SaleAmountInSaleCurrency)) {
                                item.SaleAmountInSaleCurrency = 0;
                            }

                            if (!lineVatType.IsMultiPercentage) {
                                if (item.VatPercentage != null) {
                                    var myQroupItem = new QuoteTotalVATPM(null);
                                    myQroupItem.Tenant = SessionLocator.Tenant;
                                    myQroupItem.QuoteId = this.EntityPM.Id;
                                    myQroupItem.Id = item.VatTypeId;
                                    myQroupItem.VatTypeId = item.VatTypeId;
                                    myQroupItem.VatPercent = item.VatPercentage;
                                    myQroupItem.ExternalVATCard = lineVatType.ReceivablesExternalId;
                                    myQroupItem.ExternalTAXItemId = lineVatType.ExternalTAXItemId;
                                    myQroupItem.QuoteCurrencyVatableAmount = item.SaleAmountInSaleCurrency;
                                    myQroupItem.LocalCurrencyVatableAmount = item.SaleTotalAmountLocal;

                                    if (item.IsRegionalTax) {
                                        myQroupItem.QuoteCurrencyVatableAmount = item.SaleAmountInSaleCurrency + item.SaleAmountInSaleCurrency * (this.RegionalTaxPercentage / 100);
                                        myQroupItem.LocalCurrencyVatableAmount = item.SaleTotalAmountLocal + item.SaleTotalAmountLocal * (this.RegionalTaxPercentage / 100);

                                        var regionalTaxItem = new QuoteTotalVATPM(null);
                                        regionalTaxItem.Tenant = SessionLocator.Tenant;
                                        regionalTaxItem.QuoteId = this.EntityPM.Id;
                                        regionalTaxItem.Id = this.RegionalTaxId;
                                        regionalTaxItem.VatTypeId = this.RegionalTaxId;
                                        regionalTaxItem.VatPercent = this.RegionalTaxPercentage;
                                        regionalTaxItem.ExternalVATCard = lineVatType.ReceivablesExternalId;
                                        regionalTaxItem.ExternalTAXItemId = lineVatType.ExternalTAXItemId;
                                        regionalTaxItem.QuoteCurrencyVatableAmount = item.SaleAmountInSaleCurrency;
                                        regionalTaxItem.LocalCurrencyVatableAmount = item.SaleTotalAmountLocal;
                                        group_Source.push(regionalTaxItem);
                                    }

                                    group_Source.push(myQroupItem);
                                }
                            }

                            else {
                                var myVatGroups = SessionLocator.AllVatTypesGroups.filter(f => f.GroupVATTypeId == item.VatTypeId);

                                myVatGroups.forEach(itemGroup => {

                                    var lineSingleVatType = this.AllVatTypes.filter(f => f.Id == itemGroup.SingleVATTypeId)[0];

                                    if (lineSingleVatType) {

                                        var myQroupItem = new QuoteTotalVATPM(null);
                                        myQroupItem.Tenant = SessionLocator.Tenant;
                                        myQroupItem.QuoteId = this.EntityPM.Id;
                                        myQroupItem.Id = itemGroup.SingleVATTypeId;
                                        myQroupItem.VatTypeId = itemGroup.SingleVATTypeId;
                                        myQroupItem.VatPercent = this.GetVatTypePercentage(itemGroup.SingleVATTypeId);
                                        myQroupItem.ExternalVATCard = lineSingleVatType.ReceivablesExternalId;
                                        myQroupItem.ExternalTAXItemId = lineSingleVatType.ExternalTAXItemId;
                                        myQroupItem.QuoteCurrencyVatableAmount = item.SaleAmountInSaleCurrency;
                                        myQroupItem.LocalCurrencyVatableAmount = item.SaleTotalAmountLocal;
                                        group_Source.push(myQroupItem);
                                    }
                                });
                            }
                        }
                    });

                    // Build Grouped Data
                    var group_data: QuoteTotalVATPM[] = [];
                    group_Source.forEach(item => {
                        var record: QuoteTotalVATPM = group_data.filter(f => f.VatTypeId == item.VatTypeId && f.VatPercent == item.VatPercent && f.ExternalVATCard == item.ExternalVATCard && f.ExternalTAXItemId == item.ExternalTAXItemId)[0];
                        if (record) {
                            record.QuoteCurrencyVatableAmount += item.QuoteCurrencyVatableAmount;
                            record.LocalCurrencyVatableAmount += item.LocalCurrencyVatableAmount;
                        }

                        else {
                            record = new QuoteTotalVATPM(null);
                            record.Id = item.Id;
                            record.VatTypeId = item.VatTypeId;
                            record.VatPercent = item.VatPercent;
                            record.ExternalVATCard = item.ExternalVATCard;
                            record.ExternalTAXItemId = item.ExternalTAXItemId;
                            record.QuoteCurrencyVatableAmount = item.QuoteCurrencyVatableAmount;
                            record.LocalCurrencyVatableAmount = item.LocalCurrencyVatableAmount;
                            group_data.push(record);
                        }
                    });

                    // Build Quote Total VATs
                    group_data.forEach(item => {

                        var itemVatType = this.AllVatTypes.filter(f => f.Id == item.VatTypeId)[0];

                        var itemTotalVAT = new QuoteTotalVATPM(null);
                        itemTotalVAT.Tenant = SessionLocator.Tenant;
                        itemTotalVAT.QuoteId = this.EntityPM.Id;
                        itemTotalVAT.VatTypeId = item.VatTypeId;
                        itemTotalVAT.VatTypeName = itemVatType ? itemVatType.EnglishName : "";
                        itemTotalVAT.VatPercent = AppTool.Round(item.VatPercent, 3);
                        itemTotalVAT.VatTypeCell = itemTotalVAT.VatTypeName + " (" + itemTotalVAT.VatPercent + "%)";
                        itemTotalVAT.ExternalVATCard = item.ExternalVATCard;
                        itemTotalVAT.ExternalTAXItemId = item.ExternalTAXItemId;
                        itemTotalVAT.LocalCurrencyVatableAmount = AppTool.Round(item.LocalCurrencyVatableAmount, 2);
                        itemTotalVAT.QuoteCurrencyVatableAmount = AppTool.Round(item.QuoteCurrencyVatableAmount, 2);
                        itemTotalVAT.LocalCurrencyVATAmount = AppTool.Round((itemTotalVAT.LocalCurrencyVatableAmount * itemTotalVAT.VatPercent / 100), 2);
                        itemTotalVAT.QuoteCurrencyVATAmount = AppTool.Round((itemTotalVAT.QuoteCurrencyVatableAmount * itemTotalVAT.VatPercent / 100), 2);
                        this.EntityPM.AddQuoteTotalVATPM(itemTotalVAT);
                    });
                }
            }
        }
    }

    VATDetailsClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = TextCodeTranslator.Translate("Quote.O.Charges.VATDetails");
        logitudeWindow.WindowArgs = { IsLocalCurrency: this.IsLocalCurrency, SaleCurrencyCode: this.SaleCurrencyCode, IsCurrencyFilterVisible: this.IsCurrencyFilterVisible, TotalVATs: this.EntityPM.TotalVATs };
        logitudeWindow.Show('./QuoteModules/QuoteCharges/Components/QuoteVATDetailsComponent');
    }

    OnFreightAmountChanged() {
        this.ItemsSource.Collection.filter(f => f.ChargesGroupCode != "FRT").forEach((item: QuoteChargeItem) => {
            if (item.CostMeasurementCode == "PRFR") {
                item.SetCostQuantity();
            }

            if (item.SaleMeasurementCode == "PRFR") {
                item.SetSaleQuantity();
            }
        });
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

                if (AppTool.IsNullOrEmpty(oldValue)) {
                    this.ItemsSource.Collection.filter(f => f.IsRegionalTax == false && f.VatIsMultiPercentage == false).forEach(item => {
                        this.myChargesTypeService.getSingleFromCache(item.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                var list: ChargesTypeList = myResponse.Result;
                                if (list != null) {
                                    item.IsRegionalTax = list.ApplyRegionalTax;
                                }
                            }
                        });
                    });
                }
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
export class QuoteChargeItem extends BaseComponent {
    public QuotePM: QuotePM;
    public EntityPM: QuoteChargePM;   
    public ObjectTableName: string = "QuoteCharge";
    public DataContext = this;
    public IsNew: boolean = false;
    public TransportModeId: string;
    public IsAdhoc: boolean = false;
    public IsRoutingRate: boolean = false;
    public IsAddPricestepsEnabled: boolean = false;
    constructor(entity: QuoteChargePM, public fatherComponent: LCLChargesComponent, isNew: boolean) {
        super(); 
        this.IsNew = isNew;
        this.EntityPM = entity;
        this.QuotePM = fatherComponent.EntityPM;
        this.IsAdhoc = fatherComponent.IsAdhoc;
        this.IsRoutingRate = fatherComponent.IsRoutingRate;
        this.TransportModeId = fatherComponent.TransportModeId;
        this.SetUIProperties();
        this.ReadVatTypeData();
        this.ComputeMarkUpString();
        this.SetUIProperties_CellsColors();
        this.BuildPriceBreaksTooltips();
    }

    public HasCostPriceBreaks: boolean = false;
    public HasSalePriceBreaks: boolean = false;
    public CostPriceBreaksTooltip: string = "";
    public SalePriceBreaksTooltip: string = "";
    BuildPriceBreaksTooltips() {
        this.HasCostPriceBreaks = false;
        this.HasSalePriceBreaks = false;
        this.CostPriceBreaksTooltip = "";
        this.SalePriceBreaksTooltip = "";

        if (this.IsChargeBySteps) {
            var items = this.EntityPM.QuoteChargePriceSteps.filter(f => !AppTool.IsNullOrEmpty(f.Step));

            if (items.length > 0) {
                if (items.filter(f => !AppTool.IsNullOrEmpty(f.CostUnitPrice)).length > 0) {
                    this.HasCostPriceBreaks = true;

                    items.filter(f => !AppTool.IsNullOrEmpty(f.CostUnitPrice)).forEach(item => {
                        if (AppTool.IsNullOrEmpty(this.CostPriceBreaksTooltip)) {
                            this.CostPriceBreaksTooltip = "+" + item.Step + ": " + this.GetValueOrZero(item.CostUnitPrice);
                        }

                        else {
                            this.CostPriceBreaksTooltip += " / " + "+" + item.Step + ": " + this.GetValueOrZero(item.CostUnitPrice);
                        }
                    });
                }

                if (items.filter(f => !AppTool.IsNullOrEmpty(f.SaleUnitPrice)).length > 0) {
                    this.HasSalePriceBreaks = true;

                    items.filter(f => !AppTool.IsNullOrEmpty(f.SaleUnitPrice)).forEach(item => {
                        if (AppTool.IsNullOrEmpty(this.SalePriceBreaksTooltip)) {
                            this.SalePriceBreaksTooltip = "+" + item.Step + ": " + this.GetValueOrZero(item.SaleUnitPrice);
                        }

                        else {
                            this.SalePriceBreaksTooltip += " / " + "+" + item.Step + ": " + this.GetValueOrZero(item.SaleUnitPrice);
                        }
                    });
                }
            }
        }
    }

    private GetValueOrZero(value: number) {
        if (AppTool.IsNullOrEmpty(value)) {
            return 0;
        }

        return value;
    }

    public IsEditingEnabled: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    SetUIProperties() {
        this.IsEditExchangeRateVisible = this.fatherComponent.IsEditExchangeRateVisible;       

        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;
        this.SetUIProperties_AllIn();
        this.SetUIProperties_CostMinMax();
        this.SetUIProperties_SaleMinMax();
        this.SetUIProperties_CostFields();
        this.SetUIProperties_SaleFields();
        this.SetUIProperties_CellsColors();
        this.SetUIProperties_VAT();
        this.SetUIProperties_IsChargeBySteps();
        this.SetUIProperties_IsRegionalTax();
        
        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("CostIsFixedRate", this.ObjectTableName, this.IsEditingEnabled);

        this.UIProperties.SetEnabled("Step","QuotePriceSteps", this.IsEditingEnabled);
        this.UIProperties.SetEnabled("CostUnitPrice", "QuotePriceSteps", this.IsEditingEnabled);
        this.UIProperties.SetEnabled("MarkupValue", "QuotePriceSteps", this.IsEditingEnabled);
        this.UIProperties.SetEnabled("SaleUnitPrice", "QuotePriceSteps", this.IsEditingEnabled);
    }

    public IsRegionalTaxEnabled: boolean = false;
    SetUIProperties_IsRegionalTax() {
        var isEnabled = false;

        if (this.IsEditingEnabled) {
            if (!this.IsAllIN) {
                isEnabled = true;
            }
        }

        this.IsRegionalTaxEnabled = isEnabled;
        this.UIProperties.SetEnabled("IsRegionalTax", this.ObjectTableName, isEnabled);
    }

    public IsAllInCheckBoxVisible: boolean = false;
    public SetUIProperties_AllIn() {
        var isAllInCheckBoxVisible = false;

        if (this.ChargesGroupCode != "FRT" && this.IsAdhoc) {
            var itemFrieght: QuoteChargePM = this.QuotePM.QuoteCharges.filter(d => d.ChargesGroupCode == "FRT")[0];

            if (itemFrieght) {
                if (this.QuotePM.IsSaleCurrencySameAsCost) {
                    if (this.CostCurrencyId == itemFrieght.CostCurrencyId) {
                        isAllInCheckBoxVisible = true;
                    }
                }

                else {
                    isAllInCheckBoxVisible = true;
                }
            }
        }

        this.IsAllInCheckBoxVisible = isAllInCheckBoxVisible;
        this.SetUIProperties_AllIn_CostCurrency();
    }
    SetUIProperties_AllIn_CostCurrency() {

        var isEnabled_CostCurrencyId = false;

        if (this.IsEditingEnabled) {
            isEnabled_CostCurrencyId = true;

            if (this.IsAllIN) {
                isEnabled_CostCurrencyId = false;
            }

            else if (this.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(d => d.IsAllIN).length > 0) {
                isEnabled_CostCurrencyId = false;
            }
        }

        this.UIProperties.SetEnabled("CostCurrencyId", this.ObjectTableName, isEnabled_CostCurrencyId);
       
    }

  SetUIProperties_AllInCost() {
    var isFromTariff = this.EntityPM != null && this.EntityPM.TariffId != null;
    if (this.IsEditingEnabled && isFromTariff) {
      var isEnabled_CostCurrencyId = true;
      if (this.IsCostAllIn) {
        isEnabled_CostCurrencyId = false;
      }
      this.UIProperties.SetEnabled("CostCurrencyId", this.ObjectTableName, isEnabled_CostCurrencyId || isFromTariff);
      this.UIProperties.SetEnabled("CostTotalAmount", this.ObjectTableName, isEnabled_CostCurrencyId || isFromTariff);
      this.UIProperties.SetEnabled("CostUnitPrice", this.ObjectTableName, isEnabled_CostCurrencyId);
      this.IsEnabled_CostUnitPrice = isEnabled_CostCurrencyId;
    }
  }

    public IsEnabled_CostQuantity: boolean = false;
    public IsEnabled_CostUnitPrice: boolean = false;
    public IsEnabled_CostMinAmount: boolean = false;
    public IsEnabled_CostExchangeRate: boolean = false;
    SetUIProperties_CostFields() {
        var isEnabled_CostQuantity = false;
        var isEnabled_CostUnitPrice = false;
        var isEnabled_CostMinAmount = false;
        var isEnabled_CostMeasurement = false;
        var isEnabled_AddPriceSteps = false;

        if (this.IsEditingEnabled) {
            isEnabled_CostQuantity = true;
            isEnabled_CostUnitPrice = true;
            isEnabled_CostMinAmount = true;
            isEnabled_CostMeasurement = true;

            switch (this.CostMeasurementCode) {
                case "GRWT":
                case "CHWT":
                case "CWKG":
                case "GWKG":
                case "VCBM":
                case "BTEU":
                case "FIXD":
                case "BCNT":
                case "PRVL":
                case "PRFR":
                case "GWTN":
                case "QTY":
                case"PDCW":
                    {
                        isEnabled_CostQuantity = false;
                        break;
                    }
            }

            if (this.IsAllIN) {
                isEnabled_CostUnitPrice = false;
                isEnabled_CostMinAmount = false;
                //isEnabled_CostMeasurement = false;
            }

            else if (this.IsChargeBySteps) {
                isEnabled_CostUnitPrice = false;
                isEnabled_CostMinAmount = false;
            }

            else if (this.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(d => d.IsAllIN).length > 0) {
                isEnabled_CostUnitPrice = false;
                isEnabled_CostMinAmount = false;
                //isEnabled_CostMeasurement = false;
            }

            if (this.CostMeasurementCode != "FIXD") {
                isEnabled_AddPriceSteps = true;
            }
        }

        this.IsAddPricestepsEnabled = isEnabled_AddPriceSteps;
        this.IsEnabled_CostQuantity = isEnabled_CostQuantity;
        this.IsEnabled_CostUnitPrice = isEnabled_CostUnitPrice;
        this.IsEnabled_CostMinAmount = isEnabled_CostMinAmount;
        this.UIProperties.SetEnabled("CostMeasurementId", this.ObjectTableName, isEnabled_CostMeasurement);
        this.UIProperties.SetEnabled("CostMinAmount", this.ObjectTableName, isEnabled_CostMinAmount);
        this.UIProperties.SetEnabled("CostMaxAmount", this.ObjectTableName, isEnabled_CostMinAmount);
        this.SetUIProperties_CostRate();
        this.SetUIProperties_AllInCost();
    }
    SetUIProperties_CostRate() {
        var isEnabled = false;

        if (this.IsEditingEnabled) {
            if (FeatureLocator.HasFeaturePermession("Quote", "QouteEditExchangeRate")) {
                if (this.CostCurrencyId) {
                    if (this.CostCurrencyId != SessionLocator.LocalCurrencyId) {
                        if (this.CostCurrencyId != this.fatherComponent.SaleCurrencyId) {
                            isEnabled = true;
                        }
                    }
                }                                 
            }
        }

        this.IsEnabled_CostExchangeRate = isEnabled;
        this.UIProperties.SetEnabled("CostExchangeRate", this.ObjectTableName, isEnabled);
    }

    public IsEnabled_SaleQuantity: boolean = false;
    public IsEnabled_SaleUnitPrice: boolean = false;
    public IsEnabled_SaleMinAmount: boolean = false;
    public IsEnabled_SaleTotalAmount: boolean = false;
    public IsEnabled_SaleTotalAmountLocal: boolean = false;
    SetUIProperties_SaleFields() {
        var isEnabled_SaleQuantity = false;
        var isEnabled_SaleUnitPrice = false;
        var isEnabled_SaleMinAmount = false;
        var isEnabled_SaleTotalAmount = false;
        var isEnabled_SaleTotalAmountLocal = false;
        var isEnabled_SaleMeasurement = false;

        if (this.IsEditingEnabled) {
            isEnabled_SaleQuantity = true;
            isEnabled_SaleUnitPrice = true;
            isEnabled_SaleMinAmount = true;
            isEnabled_SaleTotalAmount = true;
            isEnabled_SaleTotalAmountLocal = true;
            isEnabled_SaleMeasurement = true;

            switch (this.SaleMeasurementCode) {
                case "GRWT":
                case "CHWT":
                case "CWKG":
                case "GWKG":
                case "VCBM":
                case "BTEU":
                case "FIXD":
                case "BCNT":
                case "PRVL":
                case "PRFR":
                case "GWTN":
                case "QTY":
                case "PDCW":
                    {
                        isEnabled_SaleQuantity = false;
                        break;
                    }
            }

            if (this.EntityPM.IsAllIN) {
                isEnabled_SaleUnitPrice = false;
                isEnabled_SaleMinAmount = false;
                isEnabled_SaleTotalAmount = false;
                isEnabled_SaleTotalAmountLocal = false;
                //isEnabled_SaleMeasurement = false;
            }

            else if (this.IsChargeBySteps) {
                isEnabled_SaleUnitPrice = false;
                isEnabled_SaleMinAmount = false;
            }

            else if (this.EntityPM.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(d => d.IsAllIN).length > 0) {
                isEnabled_SaleUnitPrice = false;
                isEnabled_SaleMinAmount = false;
                //isEnabled_SaleMeasurement = false;
            }
        }

        this.IsEnabled_SaleQuantity = isEnabled_SaleQuantity;
        this.IsEnabled_SaleUnitPrice = isEnabled_SaleUnitPrice;
        this.IsEnabled_SaleMinAmount = isEnabled_SaleMinAmount;
        this.IsEnabled_SaleTotalAmount = isEnabled_SaleTotalAmount;
        this.IsEnabled_SaleTotalAmountLocal = isEnabled_SaleTotalAmountLocal;
        this.UIProperties.SetEnabled("SaleMeasurementId", this.ObjectTableName, isEnabled_SaleMeasurement);
        this.UIProperties.SetEnabled("SaleMinAmount", this.ObjectTableName, isEnabled_SaleMinAmount);
        this.UIProperties.SetEnabled("SaleMaxAmount", this.ObjectTableName, isEnabled_SaleMinAmount);
    }

    public CostMinMaxIconTitle: string = "";
    public CostMinMaxIconIsVisibile: boolean = false;
    SetUIProperties_CostMinMax() {
        var iTitle: string = null;
        var iVisible: boolean = false;

        var iAmount = null;
        if (this.EntityPM.CostQuantity != null && this.EntityPM.CostUnitPrice != null) {
            iAmount = AppTool.Round(this.EntityPM.CostQuantity * this.EntityPM.CostUnitPrice, 2);
        }

        if (iAmount != null) {
            if (this.CostMinAmount != null) {
                if (iAmount < this.CostMinAmount) {
                    iAmount = this.CostMinAmount;
                    iVisible = true;

                    iTitle = "Amount is due to Charge Min Amount";
                    iTitle += "\n";
                    iTitle += "Cost Min Amount: " + DecimalFormatter.format(this.CostMinAmount, 3);

                    if (this.CostMaxAmount != null) {
                        iTitle += "\n";
                        iTitle += "Cost Max Amount: " + DecimalFormatter.format(this.CostMaxAmount, 3);
                    }
                }
            }

            if (this.CostMaxAmount != null) {
                if (iAmount > this.CostMaxAmount) {
                    iAmount = this.CostMaxAmount;
                    iVisible = true;

                    iTitle = "Amount is due to Charge Max Amount";

                    if (this.CostMinAmount != null) {
                        iTitle += "\n";
                        iTitle += "Cost Min Amount: " + DecimalFormatter.format(this.CostMinAmount, 3);
                    }

                    iTitle += "\n";
                    iTitle += "Cost Max Amount: " + DecimalFormatter.format(this.CostMaxAmount, 3);
                }
            }
        }

        this.CostMinMaxIconTitle = iTitle;
        this.CostMinMaxIconIsVisibile = iVisible;
    }

    public SaleMinMaxIconTitle: string = "";
    public SaleMinMaxIconIsVisibile: boolean = false;
    SetUIProperties_SaleMinMax() {
        var iTitle: string = null;
        var iVisible: boolean = false;

        var iAmount = null;
        if (this.EntityPM.SaleQuantity != null && this.EntityPM.SaleUnitPrice != null) {
            iAmount = AppTool.Round(this.EntityPM.SaleQuantity * this.EntityPM.SaleUnitPrice, 2);
        }

        if (iAmount != null) {
            if (this.SaleMinAmount != null) {
                if (iAmount < this.SaleMinAmount) {
                    iAmount = this.SaleMinAmount;
                    iVisible = true;
                    iTitle = "Amount is due to Charge Min Amount";
                    iTitle += "\n";
                    iTitle += "Sale Min Amount: " + DecimalFormatter.format(this.SaleMinAmount, 3);

                    if (this.SaleMaxAmount != null) {
                        iTitle += "\n";
                        iTitle += "Sale Max Amount: " + DecimalFormatter.format(this.SaleMaxAmount, 3);
                    }
                }
            }

            if (this.SaleMaxAmount != null) {
                if (iAmount > this.SaleMaxAmount) {
                    iAmount = this.SaleMaxAmount;
                    iVisible = true;
                    iTitle = "Amount is due to Charge Max Amount";

                    if (this.SaleMinAmount != null) {
                        iTitle += "\n";
                        iTitle += "Sale Min Amount: " + DecimalFormatter.format(this.SaleMinAmount, 3);
                    }

                    iTitle += "\n";
                    iTitle += "Sale Max Amount: " + DecimalFormatter.format(this.SaleMaxAmount, 3);
                }
            }
        }

        this.SaleMinMaxIconTitle = iTitle;
        this.SaleMinMaxIconIsVisibile = iVisible;
    }

    public SaleUnitPriceColor: string = FontTool.Black;
    public SaleTotalAmountColor: string = FontTool.Black;
    public SaleTotalAmountLocalColor: string = FontTool.Black;
    SetUIProperties_CellsColors() {

        // Price
        var myCostPrice: number = this.CostUnitPriceInSaleCurrency;
        var mySalePrice: number = this.SaleUnitPrice;

        if (AppTool.IsNullOrEmpty(this.CellMarkupText) && QuoteTool.IsQuoteStageDraft(this.fatherComponent.EntityPM) && !AppTool.IsNullOrEmpty(myCostPrice) && !AppTool.IsNullOrEmpty(mySalePrice) && (myCostPrice == mySalePrice) ) {
            this.SaleUnitPriceColor = this.IsEditingEnabled ? FontTool.Orange : FontTool.CellDisabledColor;
        }

        else if (AppTool.IsNullOrEmpty(myCostPrice) || AppTool.IsNullOrEmpty(mySalePrice)) {
            this.SaleUnitPriceColor = this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor;
        }

        else {
            this.SaleUnitPriceColor = mySalePrice < myCostPrice ? FontTool.Red : (mySalePrice > myCostPrice ? FontTool.Green : (this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor));
        }

        // Total Amount
        var myCostTotalAmount: number = this.CostAmountInSaleCurrency;
        var mySaleTotalAmount: number = this.SaleTotalAmount;
        if (AppTool.IsNullOrEmpty(myCostTotalAmount) || AppTool.IsNullOrEmpty(mySaleTotalAmount)) {
            this.SaleTotalAmountColor = this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor;
        }

        else {
            this.SaleTotalAmountColor = mySaleTotalAmount < myCostTotalAmount ? FontTool.Red : (mySaleTotalAmount > myCostTotalAmount ? FontTool.Green : (this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor));
        }

        // Total Amount Local
        var myCostTotalAmountLocal: number = this.CostTotalAmountLocal;
        var mySaleTotalAmountLocal: number = this.SaleTotalAmountLocal;
        if (AppTool.IsNullOrEmpty(myCostTotalAmountLocal) || AppTool.IsNullOrEmpty(mySaleTotalAmountLocal)) {
            this.SaleTotalAmountLocalColor = this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor;

        }

        else {
            this.SaleTotalAmountLocalColor = mySaleTotalAmountLocal < myCostTotalAmountLocal ? FontTool.Red : (mySaleTotalAmountLocal > myCostTotalAmountLocal ? FontTool.Green : (this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor));
        }
    }
    SetUIProperties_VAT() {
        this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VatPercentage", this.ObjectTableName, this.IsEditingEnabled);

        if (this.VatIsMultiPercentage) {
            this.UIProperties.SetEnabled("VatPercentage", this.ObjectTableName, false);
        }

        var isVatPercentageRequired = false;

        if (this.QuotePM.IsChargesByVAT) {
            if (!AppTool.IsNullOrEmpty(this.VatTypeId)) {
                if (!this.VatIsMultiPercentage) {
                    if (AppTool.IsNullOrEmpty(this.VatPercentage)) {
                        isVatPercentageRequired = true;
                    }
                }
            }
        }

        this.UIProperties.SetRequired("VatPercentage", this.ObjectTableName, isVatPercentageRequired);
    }

    // Charges Type
    get ChargesTypeId() { return this.EntityPM.ChargesTypeId; }
    set ChargesTypeId(value: string) {
        if (this.EntityPM.ChargesTypeId != value) {
            this.EntityPM.ChargesTypeId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.OnChargesTypeChanged(null);
            }

            else {
                this.fatherComponent.myChargesTypeService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: ChargesTypeList = myResponse.Result;

                        if (list) {
                            this.OnChargesTypeChanged(list);
                        }

                        else {
                            this.fatherComponent.myChargesTypeService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
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
            if (this.fatherComponent.IsChargesByVAT) {
                this.VatTypeId = list.VatTypeId;
            }

            this.ChargesTypeCode = list.Code;
            this.ChargesTypeName = list.EnglishName;
            this.ChargesGroupCode = list.ChargesGroupCode;
            this.CostMeasurementId = list.MeasurementId;
            this.EntityPM.IsBackToBack = list.IsBackToBack;

            if (!AppTool.IsNullOrEmpty(list.PayablesDefaultCurrencyId)) {
                this.CostCurrencyId = list.PayablesDefaultCurrencyId;
            }
            else {
                if (this.ChargesGroupCode == "FRT" || this.ChargesGroupCode == "SCH") {
                    this.CostCurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
                }

                else {
                    this.CostCurrencyId = SessionLocator.TenantPM.OtherChargesCurrencyId;
                }
            }

            if (this.fatherComponent.IsChargesByVAT) {
                this.IsRegionalTax = list.ApplyRegionalTax;
            }
        }

        else {
            this.VatTypeId = null;
            this.ChargesTypeCode = null;
            this.ChargesTypeName = null;
            this.ChargesGroupCode = null;
            this.CostMeasurementId = null;
            this.CostCurrencyId = null;
            this.EntityPM.IsBackToBack = false;
            this.IsRegionalTax = false;
        }

        this.SetUIProperties();
    }

    get ChargesTypeCode() { return this.EntityPM.ChargesTypeCode; }
    set ChargesTypeCode(newValue: string) {
        if (this.EntityPM.ChargesTypeCode != newValue) {
            this.EntityPM.ChargesTypeCode = newValue;
        }
    }

    get ChargesTypeName() { return this.EntityPM.ChargesTypeName; }
    set ChargesTypeName(newValue: string) {
        if (this.EntityPM.ChargesTypeName != newValue) {
            this.EntityPM.ChargesTypeName = newValue;
        }
    }
    
    get ChargesGroupCode() { return this.EntityPM.ChargesGroupCode; }
    set ChargesGroupCode(newValue: string) {
        if (this.EntityPM.ChargesGroupCode != newValue) {
            this.EntityPM.ChargesGroupCode = newValue;
            this.SetUIProperties_CostFields();
            this.SetUIProperties_SaleFields();
        }
    }

    get ChargesTypeCodeName() {
        var myResult = "";

        if (!AppTool.IsNullOrEmpty(this.ChargesTypeCode) && !AppTool.IsNullOrEmpty(this.ChargesTypeName)) {
            myResult = "(" + this.ChargesTypeCode + ") " + this.ChargesTypeName;
        }

        return myResult;
    }

    // VAT Type
    get VatTypeId() { return this.EntityPM.VatTypeId; }
    set VatTypeId(value: string) {
        if (this.EntityPM.VatTypeId != value) {
            this.EntityPM.VatTypeId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.VatTypeName = null;
                this.VatPercentage = null;
                this.VatIsMultiPercentage = false;
                this.EntityPM.ExternalVATCard = null;
                this.EntityPM.ExternalTAXItemId = null;
                this.ReadVatTypeData();
                this.fatherComponent.ComputeTotals();
                this.fatherComponent.SetGridColumnsWidth();
            }

            else {
                this.fatherComponent.myVatTypeService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: VatTypeList = myResponse.Result;
                        if (list) {
                            this.VatTypeName = list.EnglishName;
                            this.VatIsMultiPercentage = list.IsMultiPercentage;
                            this.EntityPM.ExternalVATCard = list.ReceivablesExternalId;
                            this.EntityPM.ExternalTAXItemId = list.ExternalTAXItemId;

                            if (list.IsMultiPercentage) {
                                this.VatPercentage = null;
                            }

                            else {
                                this.VatPercentage = this.fatherComponent.GetVatTypePercentage(value);
                            }

                            this.ReadVatTypeData();
                            this.fatherComponent.ComputeTotals();
                            this.fatherComponent.SetGridColumnsWidth();
                        }
                    }
                });
            }
        }
    }

    get VatTypeName() { return this.EntityPM.VatTypeName; }
    set VatTypeName(value: string) {
        if (this.EntityPM.VatTypeName != value) {
            this.EntityPM.VatTypeName = value;
        }
    }

    get VatPercentage() { return this.EntityPM.VatPercentage; }
    set VatPercentage(value: number) {
        if (this.EntityPM.VatPercentage != value) {
            this.EntityPM.VatPercentage = value;
            this.ReadVatTypeData();
            this.SetUIProperties_VAT();
            this.fatherComponent.ComputeTotals();
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
                myValue = this.VatTypeName + " (" + this.VatPercentage + "%)";
                myColor = FontTool.Black;
            }

            else {
                myValue = TextCodeTranslator.Translate("Quote.O.Charges.NoVat");
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

                        this.fatherComponent.ItemsSource.Collection.filter(f => f.VatTypeId == this.VatTypeId).forEach(item => {
                            item.VatPercentage = comp.Percentage;
                            item.ReadVatTypeData();
                        });

                        this.fatherComponent.ComputeTotals();
                    }
                });
            });

            logWindow.Show('./CommonModules/CommonOthers/Components/UpdateVATPercentage/UpdateVATPercentageComponent');
        }
    }

    // Vendor
    get VendorId() { return this.EntityPM.VendorId; }
    set VendorId(value: string) {
        if (this.EntityPM.VendorId != value) {
            this.EntityPM.VendorId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.VendorName = null;
            }

            else {
                this.fatherComponent.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;

                        if (list != null) {
                            this.VendorName = list.EnglishName;
                        }
                    }
                });
            }
        }
    }

    get VendorName() { return this.EntityPM.VendorName; }
    set VendorName(newValue: string) {
        if (this.EntityPM.VendorName != newValue) {
            this.EntityPM.VendorName = newValue;
        }
    }

    // Cost
    get CostMeasurementId() { return this.EntityPM.CostMeasurementId; }
    set CostMeasurementId(value: string) {
        if (this.EntityPM.CostMeasurementId != value) {
            this.EntityPM.CostMeasurementId = value;
            this.SaleMeasurementId = value;
            this.OnMeasurementsChanged();

            if (AppTool.IsNullOrEmpty(value)) {
                this.CostMeasurementCode = null;
                this.CostMeasurementShortName = null;
            }

            else {
                var list: MeasurementList = this.fatherComponent.AllMeasurements.filter(d => d.Id == value)[0];
                if (list != null) {
                    this.CostMeasurementCode = list.Code;
                    this.CostMeasurementShortName = list.ShortName;

                    if (list.Code == "PRVL") {
                        this.CostCurrencyId = this.QuotePM.ValueOfGoodsCurrencyId;
                    }

                    if (list.Code == "PRFR") {
                        this.CostCurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
                    }                    
                }
            }

            this.fatherComponent.CurrentSession.SessionEvent.emit("CostMeasurementIdChanged");
        }
    }

    get CostMeasurementCode() { return this.EntityPM.CostMeasurementCode; }
    set CostMeasurementCode(newValue: string) {
        if (this.EntityPM.CostMeasurementCode != newValue) {
            this.EntityPM.CostMeasurementCode = newValue;
            this.SetCostQuantity();
            this.SetUIProperties_CostFields();
            this.fatherComponent.CheckUpdateQuantities();
            this.OnMeasurementsCodeChanged();
        }
    }

    get CostMeasurementShortName() { return this.EntityPM.CostMeasurementShortName; }
    set CostMeasurementShortName(newValue: string) {
        if (this.EntityPM.CostMeasurementShortName != newValue) {
            this.EntityPM.CostMeasurementShortName = newValue;            
        }
    }

    get CostCurrencyId() { return this.EntityPM.CostCurrencyId; }
    set CostCurrencyId(value: string) {
        if (this.EntityPM.CostCurrencyId != value) {
            this.EntityPM.CostCurrencyId = value;
            this.SetUIProperties_CostRate();

            this.CostCurrencyCode = this.fatherComponent.GetCurrencyCode(value);
            this.CostExchangeRate = this.fatherComponent.GetCurrencyRate(value);
            this.RelativeRateDate = DateTool.GetRelativeRateDate(DateTool.GetCurrentDateAsUtc(), this.fatherComponent.GetCurrencyRateDate(value), "ago");

            if (this.QuotePM.IsSaleCurrencySameAsCost) {
                this.SaleCurrencyId = value;
            }
        }
    }

    get CostCurrencyCode() { return this.EntityPM.CostCurrencyCode; }
    set CostCurrencyCode(newValue: string) {
        if (this.EntityPM.CostCurrencyCode != newValue) {
            this.EntityPM.CostCurrencyCode = newValue;
            this.SetEditScreenGridHeaders();
        }
    }

    get CostQuantity() { return this.EntityPM.CostQuantity; }
    set CostQuantity(value: number) {
        if (this.EntityPM.CostQuantity != value) {
            this.EntityPM.CostQuantity = AppTool.Round(value, 3);
            this.ComputeCostAmounts();
            this.fatherComponent.CheckUpdateQuantities();
        }
    }

    get CostUnitPrice() { return this.EntityPM.CostUnitPrice; }
    set CostUnitPrice(value: number) {
        if (this.EntityPM.CostUnitPrice != value) {
            this.EntityPM.CostUnitPrice = AppTool.Round(value, 3);

            if (AppTool.IsNullOrEmpty(value)) {
                this.MarkUpValue = 0;
                this.MarkUpTypeCode = "F";
            }

            this.ComputeCostAmounts();
            this.ComputeCostInSalePrice();
            this.fatherComponent.CheckUpdateQuantities();
        }
    }
    
    get CostUnitPriceInSaleCurrency() { return this.EntityPM.CostUnitPriceInSaleCurrency; }
    set CostUnitPriceInSaleCurrency(value: number) {
        if (this.EntityPM.CostUnitPriceInSaleCurrency != value) {
            this.EntityPM.CostUnitPriceInSaleCurrency = AppTool.Round(value, 3);
            this.ComputeSalePrice();
            this.SetUIProperties_CellsColors();
        }
    }

    get CostTotalAmount() { return this.EntityPM.CostTotalAmount; }
    set CostTotalAmount(value: number) {
        if (this.EntityPM.CostTotalAmount != value) {
            this.EntityPM.CostTotalAmount = AppTool.Round(value, 2);

            if (this.ChargesGroupCode == "FRT") {
                this.fatherComponent.OnFreightAmountChanged();
            }           
        }
    }

    get CostTotalAmountLocal() { return this.EntityPM.CostTotalAmountLocal; }
    set CostTotalAmountLocal(value: number) {
        if (this.EntityPM.CostTotalAmountLocal != value) {
            this.EntityPM.CostTotalAmountLocal = AppTool.Round(value, 2);
        }
    }

    get CostAmountInSaleCurrency() { return this.EntityPM.CostAmountInSaleCurrency; }
    set CostAmountInSaleCurrency(value: number) {
        if (this.EntityPM.CostAmountInSaleCurrency != value) {
            this.EntityPM.CostAmountInSaleCurrency = AppTool.Round(value, 2);
            this.SetUIProperties_CellsColors();
        }
    }

    get CostExchangeRate() { return this.EntityPM.CostExchangeRate; }
    set CostExchangeRate(value: number) {
        if (this.EntityPM.CostExchangeRate != value) {
            this.EntityPM.CostExchangeRate = AppTool.Round(value, 5);

            if (this.QuotePM.IsSaleCurrencySameAsCost) {
                this.SaleExchangeRate = value;
            }

            this.ComputeCostAmounts();
            this.ComputeCostInSalePrice();
        }
    }

    get CostMinAmount() { return this.EntityPM.CostMinAmount; }
    set CostMinAmount(value: number) {
        if (this.EntityPM.CostMinAmount != value) {
            this.EntityPM.CostMinAmount = AppTool.Round(value, 2);
            this.ComputeCostAmounts();
        }
    }

    get CostMaxAmount() { return this.EntityPM.CostMaxAmount; }
    set CostMaxAmount(value: number) {
        if (this.EntityPM.CostMaxAmount != value) {
            this.EntityPM.CostMaxAmount = AppTool.Round(value, 2);
            this.ComputeCostAmounts();
        }
    }

    get CostIsFixedRate() { return this.EntityPM.CostIsFixedRate; }
    set CostIsFixedRate(value: boolean) {
        if (this.EntityPM.CostIsFixedRate != value) {
            this.EntityPM.CostIsFixedRate = value;
        }
    }

    get IsCostAllIn() { return this.EntityPM.IsCostAllIn; }
    set IsCostAllIn(value: boolean) {
        if (this.EntityPM.IsCostAllIn != value) {
            this.EntityPM.IsCostAllIn = value;
        }
    }

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

    public RelativeRateDate: string;
    UpdateCurrencyRateClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 350;
        logWindow.Title = "Update Currency Rate";
        logWindow.WindowArgs = { CurrencyId: this.CostCurrencyId, CurrencyCode: this.CostCurrencyCode, Rate: this.CostExchangeRate, Date: DateTool.GetCurrentDateAsUtc() };
        logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');

        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.fatherComponent.AllRates = comp.RatesList;
                    this.CostExchangeRate = AppTool.Round(comp.Rate, 5);
                    this.RelativeRateDate = DateTool.GetRelativeRateDate(DateTool.GetCurrentDateAsUtc(), comp.RateDate, "ago");
                }
            });
        });
    }

  SetCostQuantity(ChargesGroupCode: string = "FRT") {
        var myResult = null;

        if (this.IsAdhoc) {
            switch (this.CostMeasurementCode) {
                case "GRWT": { myResult = this.QuotePM.GrossWeight; break; }
                case "CHWT": { myResult = this.QuotePM.ChargeableWeight; break; }
                case "VOLU": { myResult = this.QuotePM.Volume; break; }
                case "BTEU": { myResult = this.QuotePM.TEU; break; }
                case "FIXD": { myResult = 1; break; }
                case "PRVL": { myResult = this.QuotePM.ValueOfGoods; break; }
                case "PRFR": { myResult = ArrayTool.Sum(this.QuotePM.QuoteCharges.filter(d => d.ChargesGroupCode == ChargesGroupCode), "CostTotalAmount"); break; }
                case "GWTN": { myResult = this.QuotePM.GrossWeightPerTon; break; }
                case "QTY": { myResult = this.QuotePM.NumberOfPackages; break; }
                case "CWKG": { myResult = this.QuotePM.ChargeableWeightInKG; break; }
                case "GWKG": { myResult = this.QuotePM.GrossWeightInKG; break; }
                case "VCBM": { myResult = this.QuotePM.VolumeInCBM; break; }
                case "PDCW": { myResult = this.QuotePM.PickupDeliveryChargeableWeight; break; }
                default: { break; }
            }
        }

        this.CostQuantity = myResult;
    }
    ComputeCostAmounts() {
        var iAmount: number = null;

        if (!AppTool.IsNullOrEmpty(this.CostQuantity) && !AppTool.IsNullOrEmpty(this.CostUnitPrice)) {
            if (this.CostMeasurementCode == "PRVL" || this.CostMeasurementCode == "PRFR") {
                iAmount = this.CostQuantity * this.CostUnitPrice / 100;
            }

            else {
                iAmount = this.CostQuantity * this.CostUnitPrice;
            }
        }

        /* MinMax */
        if (iAmount != null) {
            if (this.CostMinAmount != null) {
                if (iAmount < this.CostMinAmount) {
                    iAmount = this.CostMinAmount;
                }
            }

            if (this.CostMaxAmount != null) {
                if (iAmount > this.CostMaxAmount) {
                    iAmount = this.CostMaxAmount;
                }
            }
        }

        // Amounts
        if (AppTool.IsNullOrEmpty(iAmount)) {
            this.CostTotalAmount = null;
            this.CostTotalAmountLocal = null;
            this.CostAmountInSaleCurrency = null;
        }

        else {
            this.CostTotalAmount = iAmount;

            if (AppTool.IsNullOrEmpty(this.CostExchangeRate)) {
                this.CostTotalAmountLocal = null;
            }

            else {
                this.CostTotalAmountLocal = iAmount * this.CostExchangeRate;
            }

            this.ComputeCostInSaleAmount();
        }

        this.SetUIProperties_CostMinMax();                
        this.fatherComponent.ComputeTotals();
        this.fatherComponent.CheckUpdateQuantities();
    }
    ComputeCostInSaleAmount() {
        var myResult = null;

        if (this.QuotePM.IsSaleCurrencySameAsCost) {
            if (!AppTool.IsNullOrEmpty(this.CostTotalAmount)) {
                myResult = this.CostTotalAmount;
            }
        }

        else {
            if (!AppTool.IsNullOrEmpty(this.CostTotalAmountLocal) && !AppTool.IsNullOrEmpty(this.fatherComponent.ExchangeRate)) {
                myResult = this.CostTotalAmountLocal / this.fatherComponent.ExchangeRate;
            }
        }

        this.CostAmountInSaleCurrency = myResult;
    }
    ComputeCostInSalePrice() {
        var myResult = null;

        if (this.QuotePM.IsSaleCurrencySameAsCost) {
            if (!AppTool.IsNullOrEmpty(this.CostUnitPrice)) {
                myResult = this.CostUnitPrice;
            }
        }

        else {
            if (!AppTool.IsNullOrEmpty(this.CostUnitPrice) && !AppTool.IsNullOrEmpty(this.CostExchangeRate) && !AppTool.IsNullOrEmpty(this.fatherComponent.ExchangeRate)) {
                myResult = this.CostUnitPrice * this.CostExchangeRate / this.fatherComponent.ExchangeRate;
            }
        }

        this.CostUnitPriceInSaleCurrency = myResult;
    }

    // Sale
    get SaleMeasurementId() { return this.EntityPM.SaleMeasurementId; }
    set SaleMeasurementId(value: string) {
        if (this.EntityPM.SaleMeasurementId != value) {
            this.EntityPM.SaleMeasurementId = value;
            this.OnMeasurementsChanged();

            if (AppTool.IsNullOrEmpty(value)) {
                this.SaleMeasurementCode = null;
                this.SaleMeasurementShortName = null;
            }

            else {
                var list: MeasurementList = this.fatherComponent.AllMeasurements.filter(d => d.Id == value)[0];
                if (list != null) {
                    this.SaleMeasurementCode = list.Code;
                    this.SaleMeasurementShortName = list.ShortName;
                }
            }
        }
    }

    get SaleMeasurementCode() { return this.EntityPM.SaleMeasurementCode; }
    set SaleMeasurementCode(newValue: string) {
        if (this.EntityPM.SaleMeasurementCode != newValue) {
            this.EntityPM.SaleMeasurementCode = newValue;
            this.SetSaleQuantity();
            this.SetUIProperties_SaleFields();
            this.fatherComponent.CheckUpdateQuantities();
            this.OnMeasurementsCodeChanged();
        }
    }

    get SaleMeasurementShortName() { return this.EntityPM.SaleMeasurementShortName; }
    set SaleMeasurementShortName(newValue: string) {
        if (this.EntityPM.SaleMeasurementShortName != newValue) {
            this.EntityPM.SaleMeasurementShortName = newValue;
        }
    }

    get SaleCurrencyId() { return this.EntityPM.SaleCurrencyId; }
    set SaleCurrencyId(value: string) {
        if (this.EntityPM.SaleCurrencyId != value) {
            this.EntityPM.SaleCurrencyId = value;
            this.SaleCurrencyCode = this.fatherComponent.GetCurrencyCode(value);
            this.SaleExchangeRate = this.fatherComponent.GetCurrencyRate(value);
        }
    }

    get SaleCurrencyCode() { return this.EntityPM.SaleCurrencyCode; }
    set SaleCurrencyCode(newValue: string) {
        if (this.EntityPM.SaleCurrencyCode != newValue) {
            this.EntityPM.SaleCurrencyCode = newValue;
        }
    }

    get SaleQuantity() { return this.EntityPM.SaleQuantity; }
    set SaleQuantity(value: number) {
        if (this.EntityPM.SaleQuantity != value) {
            this.EntityPM.SaleQuantity = AppTool.Round(value, 3);
            this.ComputeSaleAmounts();
            this.fatherComponent.CheckUpdateQuantities();
        }
    }

    get SaleUnitPrice() { return this.EntityPM.SaleUnitPrice; }
    set SaleUnitPrice(value: number) {
        if (this.EntityPM.SaleUnitPrice != value) {
            this.EntityPM.SaleUnitPrice = AppTool.Round(value, 3);
            this.ComputeSaleAmounts();
            this.fatherComponent.CheckUpdateQuantities();            
        }
    }
    
    get SaleMinAmount() { return this.EntityPM.SaleMinAmount; }
    set SaleMinAmount(newValue: number) {
        if (this.EntityPM.SaleMinAmount != newValue) {
            this.EntityPM.SaleMinAmount = newValue;
            this.ComputeSaleAmounts();
        }
    }

    get SaleMaxAmount() { return this.EntityPM.SaleMaxAmount; }
    set SaleMaxAmount(newValue: number) {
        if (this.EntityPM.SaleMaxAmount != newValue) {
            this.EntityPM.SaleMaxAmount = newValue;
            this.ComputeSaleAmounts();
        }
    }

    get SaleTotalAmount() { return this.EntityPM.SaleTotalAmount; }
    set SaleTotalAmount(ivalue: number) {

        var value = ivalue;

        if (value != null) {
            if (this.SaleMinAmount != null) {
                if (value < this.SaleMinAmount) {
                    value = this.SaleMinAmount;
                }
            }

            if (this.SaleMaxAmount != null) {
                if (value > this.SaleMaxAmount) {
                    value = this.SaleMaxAmount;
                }
            }
        }

        if (this.EntityPM.SaleTotalAmount != value) {
            this.EntityPM.SaleTotalAmount = AppTool.Round(value, 2);

            if (this.ChargesGroupCode == "FRT") {
                this.fatherComponent.OnFreightAmountChanged();
            } 

            var myTotalAmount = value;
            var myPrice = this.SaleUnitPrice;

            if (myTotalAmount) {
                if (this.SaleQuantity > 0) {
                    myPrice = myTotalAmount / this.SaleQuantity;
                }
            }

            this.EntityPM.SaleTotalAmountLocal = AppTool.Round(value * this.SaleExchangeRate, 2);
            this.EntityPM.SaleUnitPrice = AppTool.Round(myPrice, 3);
            this.ComputeMarkUp();

            this.SetUIProperties_SaleMinMax();
            this.fatherComponent.ComputeTotals();
            this.SetUIProperties_CellsColors();
        }        
    }
    
    get SaleTotalAmountLocal() { return this.EntityPM.SaleTotalAmountLocal; }
    set SaleTotalAmountLocal(ivalue: number) {

        var value = ivalue;

        if (value != null) {
            var iTotalAmount = null;

            if (!AppTool.IsNullOrEmpty(value)) {
                if (!AppTool.IsNullOrZero(this.SaleExchangeRate)) {
                    iTotalAmount = value / this.SaleExchangeRate;
                }
            }

            if (iTotalAmount != null) {
                if (this.SaleMinAmount != null) {
                    if (iTotalAmount < this.SaleMinAmount) {
                        value = this.EntityPM.SaleTotalAmountLocal;
                    }
                }

                if (this.SaleMaxAmount != null) {
                    if (iTotalAmount > this.SaleMaxAmount) {
                        value = this.EntityPM.SaleTotalAmountLocal;
                    }
                }
            }
        }

        if (this.EntityPM.SaleTotalAmountLocal != value) {
            this.EntityPM.SaleTotalAmountLocal = AppTool.Round(value, 2);

            var myTotalAmount = null;
            if (value == 0) {
                myTotalAmount = 0;
            }

            else if (!AppTool.IsNullOrEmpty(value)) {
                if (!AppTool.IsNullOrZero(this.SaleExchangeRate)) {
                    myTotalAmount = value / this.SaleExchangeRate;
                }
            }


            var myPrice = this.SaleUnitPrice;

            if (myTotalAmount) {
                if (this.SaleQuantity > 0) {
                    myPrice = myTotalAmount / this.SaleQuantity;
                }
            }

            this.EntityPM.SaleTotalAmount = AppTool.Round(myTotalAmount, 2);
            this.EntityPM.SaleUnitPrice = AppTool.Round(myPrice, 3);
            this.ComputeMarkUp();

            this.SetUIProperties_SaleMinMax();
            this.fatherComponent.ComputeTotals();
            this.SetUIProperties_CellsColors();
        }
    }
    
    get SaleExchangeRate() { return this.EntityPM.SaleExchangeRate; }
    set SaleExchangeRate(value: number) {
        if (this.EntityPM.SaleExchangeRate != value) {
            this.EntityPM.SaleExchangeRate = AppTool.Round(value, 5);
            this.ComputeSaleAmounts();
        }
    }


    SetSaleQuantity(ChargesGroupCode: string = "FRT") {
        var myResult = null;

        if (this.IsAdhoc) {
            switch (this.SaleMeasurementCode) {
                case "GRWT": { myResult = this.QuotePM.GrossWeight; break; }
                case "CHWT": { myResult = this.QuotePM.ChargeableWeight; break; }
                case "VOLU": { myResult = this.QuotePM.Volume; break; }
                case "BTEU": { myResult = this.QuotePM.TEU; break; }
                case "FIXD": { myResult = 1; break; }
                case "PRVL": { myResult = this.QuotePM.ValueOfGoods; break; }
                case "PRFR": { myResult = ArrayTool.Sum(this.QuotePM.QuoteCharges.filter(d => d.ChargesGroupCode == ChargesGroupCode), "SaleTotalAmount"); break; }
                case "GWTN": { myResult = this.QuotePM.GrossWeightPerTon; break; }
                case "QTY": { myResult = this.QuotePM.NumberOfPackages; break; }
                case "CWKG": { myResult = this.QuotePM.ChargeableWeightInKG; break; }
                case "GWKG": { myResult = this.QuotePM.GrossWeightInKG; break; }
                case "VCBM": { myResult = this.QuotePM.VolumeInCBM; break; }
                case "PDCW": { myResult = this.QuotePM.PickupDeliveryChargeableWeight; break; }
                default: { break; }
            }
        }

        this.SaleQuantity = myResult;
    }


    ComputeSaleAmounts() {
        var totalAmount = null;

        if (!AppTool.IsNullOrEmpty(this.SaleUnitPrice) && !AppTool.IsNullOrEmpty(this.SaleQuantity)) {
            if (this.SaleMeasurementCode == "PRVL" || this.SaleMeasurementCode == "PRFR") {
                totalAmount = this.SaleQuantity * this.SaleUnitPrice / 100;
            }

            else {
                totalAmount = this.SaleQuantity * this.SaleUnitPrice;
            }
        }

        /* MinMax */
        if (totalAmount != null) {
            if (this.SaleMinAmount != null) {
                if (totalAmount < this.SaleMinAmount) {
                    totalAmount = this.SaleMinAmount;
                }
            }

            if (this.SaleMaxAmount != null) {
                if (totalAmount > this.SaleMaxAmount) {
                    totalAmount = this.SaleMaxAmount;
                }
            }
        }


        this.EntityPM.SaleTotalAmount = AppTool.Round(totalAmount, 2);
        this.EntityPM.SaleTotalAmountLocal = AppTool.IsNullOrEmpty(totalAmount) ? null : AppTool.Round(totalAmount * this.SaleExchangeRate, 2);
        this.EntityPM.SaleAmountInSaleCurrency = AppTool.IsNullOrEmpty(this.EntityPM.SaleTotalAmountLocal) ? null : AppTool.Round(this.EntityPM.SaleTotalAmountLocal / this.QuotePM.ExchangeRate, 2);

        this.EntityPM.SaleUnitPriceInSaleCurrency = this.GetSalePriceInSaleCurrency(this.EntityPM.SaleUnitPrice);

        if (this.ChargesGroupCode == "FRT") {
            this.fatherComponent.ItemsSource.Collection.filter(f => f.SaleMeasurementCode == "PRFR").forEach((item: QuoteChargeItem) => {
                item.SetSaleQuantity();
            });
        }

        this.SetUIProperties_CellsColors();

        if (this.ChargesGroupCode == "FRT") {
            this.fatherComponent.OnFreightAmountChanged();
        }

        this.SetUIProperties_SaleMinMax();
        this.fatherComponent.ComputeTotals();
        this.fatherComponent.CheckUpdateQuantities();
    }

    GetSalePriceInSaleCurrency(saleUnitPrice: number): number {

        var output: number = null;

        if (!AppTool.IsNullOrZero(saleUnitPrice)) {
            if (this.EntityPM.SaleCurrencyId == this.QuotePM.SaleCurrencyId) {
                output = saleUnitPrice;
            }

            else {
                output = AppTool.Round((saleUnitPrice * this.EntityPM.SaleExchangeRate / this.QuotePM.ExchangeRate), 2);
            }
        }

        return output;
    }

    ComputeSalePrice() {
        if (AppTool.IsNullOrEmpty(this.CostUnitPriceInSaleCurrency)) {
            this.SaleUnitPrice = null;
        }

        else {
            var myResult = this.SaleUnitPrice;
            var markup = this.MarkUpValue == null ? 0 : this.MarkUpValue;

            if (this.MarkUpTypeCode == "P") {
                myResult = this.CostUnitPriceInSaleCurrency + (this.CostUnitPriceInSaleCurrency * (markup / 100));
            }

            else {
                myResult = this.CostUnitPriceInSaleCurrency + markup;
            }

            if (AppTool.IsNullOrZero(myResult)) {
                myResult = null;
            }

            this.SaleUnitPrice = myResult;
        }
    }

    private mySaleUnitPriceString: string = null;
    get SaleUnitPriceString() {
        var myResult = null;

        if (!AppTool.IsNullOrEmpty(this.SaleUnitPrice)) {
            myResult = this.SaleUnitPrice + "";
        }

        this.mySaleUnitPriceString = myResult;
        return this.mySaleUnitPriceString;
    }
    set SaleUnitPriceString(value: string) {
        if (this.mySaleUnitPriceString != value) {
            var mySalePrice = null;
            var myMarkUpValue = 0;
            var myMarkUpCode = "F";
            var myCostPrice = this.CostUnitPriceInSaleCurrency;

            if (!AppTool.IsNullOrEmpty(value)) {
                if (value.indexOf("-") > -1 || value.indexOf("+") > -1 || value.indexOf("%") > -1) {
                    var myMarkUpValueInput = value.replace("-", "").replace("+", "").replace("%", "");

                    if (!AppTool.IsNullOrEmpty(myMarkUpValueInput)) {

                        if (myMarkUpValueInput.indexOf(',') > -1) {
                            myMarkUpValue = +(myMarkUpValueInput.replace(/,/g, '.'));
                        }
                        else {
                            myMarkUpValue = +myMarkUpValueInput;
                        }

                        if (value.indexOf("%") > -1) {
                            myMarkUpCode = "P";
                        }

                        if (!AppTool.IsNullOrEmpty(myCostPrice)) {                            
                            if (value.indexOf("+") > -1) {
                                if (myMarkUpCode == "F") {
                                    mySalePrice = myCostPrice + myMarkUpValue;
                                }

                                else {
                                    mySalePrice = myCostPrice + (myCostPrice * myMarkUpValue / 100);
                                }
                            }

                            else if (value.indexOf("-") > -1) {
                                if (myMarkUpCode == "F") {
                                    mySalePrice = myCostPrice - myMarkUpValue;
                                }

                                else {
                                    mySalePrice = myCostPrice - (myCostPrice * myMarkUpValue / 100);
                                }
                            }
                        }
                    }
                }

                else {
                    if (value.indexOf(',') > -1) {
                        mySalePrice = +(value.replace(/,/g, '.'));
                    }
                    else {
                        mySalePrice = +value;
                    }

                    if (!AppTool.IsNullOrEmpty(myCostPrice)) {
                        myMarkUpValue = mySalePrice - myCostPrice;
                    }
                }
            }

            this.SaleUnitPrice = mySalePrice;
            this.MarkUpTypeCode = myMarkUpCode;
            this.MarkUpValue = AppTool.Round(myMarkUpValue, 3);
        }
    }

    // Markup
    get MarkUpValue() { return this.EntityPM.MarkUpValue; }
    set MarkUpValue(value: number) {
        if (this.EntityPM.MarkUpValue != value) {
            this.EntityPM.MarkUpValue = AppTool.Round(value, 3);
            this.ComputeMarkUpString();
            this.SetUIProperties_CellsColors();
        }
    }

    get MarkUpTypeCode() { return this.EntityPM.MarkUpTypeCode; }
    set MarkUpTypeCode(newValue: string) {
        if (this.EntityPM.MarkUpTypeCode != newValue) {
            this.EntityPM.MarkUpTypeCode = newValue;
            this.ComputeMarkUpString();
        }
    }

    private myCellMarkupText: string;
    get CellMarkupText() { return this.myCellMarkupText; }
    set CellMarkupText(value: string) {
        if (this.myCellMarkupText != value) {
            this.myCellMarkupText = value;
        }
    }

    ComputeMarkUp() {
        if (this.EntityPM.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(d => d.IsAllIN).length > 0) {
            return;
        }

        var myResult = this.EntityPM.MarkUpValue;

        if (!AppTool.IsNullOrEmpty(this.CostUnitPriceInSaleCurrency) && !AppTool.IsNullOrEmpty(this.SaleUnitPrice)) {
            if (this.EntityPM.MarkUpTypeCode == "P") {
                myResult = ((this.SaleUnitPrice - this.CostUnitPriceInSaleCurrency) * 100) / this.CostUnitPriceInSaleCurrency;
            }

            else {
                myResult = this.SaleUnitPrice - this.CostUnitPriceInSaleCurrency;
            }
        }

        this.MarkUpValue = myResult == null ? 0 : myResult;
    }
    ComputeMarkUpString() {
        var myResult: string = null;

        var markUpValue = this.MarkUpValue;
        var markUpCode = this.MarkUpTypeCode;

        if (!AppTool.IsNullOrZero(markUpValue)) {

            var myCostPrice: number = AppTool.IsNullOrEmpty(this.CostUnitPriceInSaleCurrency) ? 0 : this.CostUnitPriceInSaleCurrency;
            var mySalePrice: number = AppTool.IsNullOrEmpty(this.SaleUnitPrice) ? 0 : this.SaleUnitPrice;

            if (mySalePrice > myCostPrice) {
                myResult = "+" + markUpValue;
            }

            else if (mySalePrice < myCostPrice) {
                if (markUpValue < 0) {
                    myResult = "" + markUpValue;
                }

                else {
                    myResult = "-" + markUpValue;
                }
            }

            else {
                myResult = "+" + markUpValue;
            }

            if (markUpCode == "P") {
                myResult = myResult + "%";
            }
        }

        this.CellMarkupText = myResult;
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newValue: string) {
        if (this.EntityPM.Notes != newValue) {
            this.EntityPM.Notes = newValue;
        }
    }

    get IsChargeBySteps() { return this.EntityPM.IsChargeBySteps; }
    set IsChargeBySteps(newValue: boolean) {
        if (this.EntityPM.IsChargeBySteps != newValue) {
            this.EntityPM.IsChargeBySteps = newValue;

            if (this.IsChargeBySteps) {
                this.CostUnitPrice = null;
                //this.EntityPM.CostUnitPriceInSaleCurrency = null;
                this.SaleUnitPrice = null;
                this.MarkUpValue = 0;
                this.MarkUpTypeCode = "F";
                //this.ComputeMarkUpString();
                //this.SetUIProperties_CellsColors();
            }

            else {
                this.CostMinAmount = null;
                this.SaleMinAmount = null;
                this.EntityPM.QuoteChargePriceSteps = [];
            }

            if (newValue && (this.EntityPM.QuoteChargePriceSteps == null || (this.EntityPM.QuoteChargePriceSteps != null && this.EntityPM.QuoteChargePriceSteps.length == 0))) {
                this.fatherComponent.CurrentSession.SessionEvent.emit("AddDefaultPriceStep");
            }

            this.SetUIProperties();
        }
    }

    get IsAllIN() { return this.EntityPM.IsAllIN; }
    set IsAllIN(newValue: boolean) {
        if (this.EntityPM.IsAllIN != newValue) {
            this.EntityPM.IsAllIN = newValue;
            this.SetUIProperties();
            this.ApplyAllIn();
            this.IsRegionalTax = false;
            this.SetUIProperties_IsRegionalTax();
        }
    }

    ApplyAllIn() {
        var allFreightModel: QuoteChargeItem[] = this.fatherComponent.ItemsSource.Collection.filter(d => d.EntityPM.ChargesGroupCode == "FRT");
        allFreightModel.forEach(item => {
            item.SetUIProperties();
        });

        if (this.SaleTotalAmountLocal > 0) {
            var freightModel: QuoteChargeItem = allFreightModel[0];
            if (freightModel != null) {

                if (!AppTool.IsNullOrEmpty(freightModel.SaleTotalAmountLocal)) {
                    freightModel.SetUIProperties();


                    var freightTotalAmountLocal = AppTool.IsNullOrEmpty(freightModel.SaleTotalAmountLocal) ? 0 : freightModel.SaleTotalAmountLocal;

                    if (this.IsAllIN) {
                        freightModel.SaleTotalAmountLocal = freightTotalAmountLocal + this.EntityPM.SaleTotalAmountLocal;
                    }

                    else {
                        freightModel.SaleTotalAmountLocal = freightTotalAmountLocal - this.EntityPM.SaleTotalAmountLocal;
                    }
                }
            }
        }
    }

    OnQuoteSaleCurrencyChanged() {
        if (this.CostCurrencyId == this.fatherComponent.SaleCurrencyId) {
            if (this.CostExchangeRate != this.fatherComponent.ExchangeRate) {
                this.CostExchangeRate = this.fatherComponent.ExchangeRate;
            }
        }

        else {
            this.CostExchangeRate = this.fatherComponent.GetCurrencyRate(this.CostCurrencyId);
        }

        if (this.QuotePM.IsSaleCurrencySameAsCost) {
            this.EntityPM.SaleCurrencyId = this.EntityPM.CostCurrencyId;
            this.EntityPM.SaleCurrencyCode = this.EntityPM.CostCurrencyCode;
            this.EntityPM.SaleExchangeRate = this.EntityPM.CostExchangeRate;
        }

        else {
            this.EntityPM.SaleCurrencyId = this.fatherComponent.SaleCurrencyId;
            this.EntityPM.SaleCurrencyCode = this.fatherComponent.SaleCurrencyCode;
            this.EntityPM.SaleExchangeRate = this.fatherComponent.ExchangeRate;
        }

        // InSaleCurrency
        this.ComputeCostInSalePrice();
        this.ComputeCostInSaleAmount();
        this.EntityPM.SaleTotalAmountLocal = AppTool.IsNullOrEmpty(this.SaleTotalAmount) ? null : AppTool.Round(this.SaleTotalAmount * this.SaleExchangeRate, 2);
        this.SetUIProperties_AllIn();
    }
    OnQuoteSaleCurrencySameAsCost() {
        if (this.CostCurrencyId == this.QuotePM.SaleCurrencyId) {
            if (this.CostExchangeRate != this.QuotePM.ExchangeRate) {
                this.CostExchangeRate = this.QuotePM.ExchangeRate;
            }
        }

        else {
            this.CostExchangeRate = this.fatherComponent.GetCurrencyRate(this.CostCurrencyId);
        }

        if (this.QuotePM.IsSaleCurrencySameAsCost) {
            this.EntityPM.SaleCurrencyId = this.EntityPM.CostCurrencyId;
            this.EntityPM.SaleCurrencyCode = this.EntityPM.CostCurrencyCode;
            this.EntityPM.SaleExchangeRate = this.EntityPM.CostExchangeRate;
        }

        else {
            this.EntityPM.SaleCurrencyId = this.fatherComponent.SaleCurrencyId;
            this.EntityPM.SaleCurrencyCode = this.fatherComponent.SaleCurrencyCode;
            this.EntityPM.SaleExchangeRate = this.fatherComponent.ExchangeRate;            
        }

        // InSaleCurrency
        this.ComputeCostInSalePrice();
        this.ComputeCostInSaleAmount();
        this.EntityPM.SaleTotalAmountLocal = AppTool.IsNullOrEmpty(this.SaleTotalAmount) ? null : AppTool.Round(this.SaleTotalAmount * this.SaleExchangeRate, 2);
        this.SetUIProperties_AllIn();
    }
    OnMeasurementsChanged() {
        if (this.CostMeasurementId != this.SaleMeasurementId) {

            this.EntityPM.MarkUpValue = 0;

            if (this.SaleUnitPrice) {
                this.mySaleUnitPriceString = this.SaleUnitPrice + "";
            }

            else {
                this.mySaleUnitPriceString = null;
            }
        }

        else {
            this.ComputeMarkUp();
        }
    }

    public SalePriceHeader: any = [];
    public SaleAmountHeader: any = [];
    SetEditScreenGridHeaders() {
        if (this.fatherComponent.IsSaleCurrencySameAsCost) {
            var myCurrencyCode = AppTool.IsNullOrEmpty(this.CostCurrencyCode) ? "" : this.CostCurrencyCode;
            this.SalePriceHeader = TextCodeTranslator.Translate("Quote.O.Charges.SalePrice",false).replace("%SaleCurrencyCode", myCurrencyCode).split('%n');
            this.SaleAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.SaleAmount", false).replace("%SaleCurrencyCode", myCurrencyCode).split('%n');
        }

        else {
            var myCurrencyCode = AppTool.IsNullOrEmpty(this.fatherComponent.SaleCurrencyCode) ? "" : this.fatherComponent.SaleCurrencyCode;
            this.SalePriceHeader = TextCodeTranslator.Translate("Quote.O.Charges.SalePrice", false).replace("%SaleCurrencyCode", myCurrencyCode).split('%n');
            this.SaleAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.SaleAmount", false).replace("%SaleCurrencyCode", myCurrencyCode).split('%n');
        }
    }

    OnMeasurementsCodeChanged() {
        this.SetUIProperties_IsChargeBySteps();
    }

    SetUIProperties_IsChargeBySteps() {

        var isEnabled = true;

        if (!this.IsEditingEnabled) {
            isEnabled = false;
        }

        else {          
            if (this.CostMeasurementCode == "FIXD" && this.SaleMeasurementCode == "FIXD") {
                isEnabled = false;
            }
        }

        this.UIProperties.SetEnabled("IsChargeBySteps", this.ObjectTableName, isEnabled);
    }

    get IsRegionalTax() { return this.EntityPM.IsRegionalTax; }
    set IsRegionalTax(newValue: boolean) {
        if (this.EntityPM.IsRegionalTax != newValue) {
            this.EntityPM.IsRegionalTax = newValue;
            this.fatherComponent.ComputeTotals();
        }
    }
}
