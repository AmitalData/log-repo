import {Component, OnDestroy} from '@angular/core';
import {QuotePM} from '../../../Quote/EntityPMs/QuotePM';
import {QuoteChargePM} from '../../../Quote/EntityPMs/QuoteChargePM';
import {QuoteUtilities} from '../../../Quote/Utilities/QuoteUtilities';
import {AppTool, DateTool, FontTool, ArrayTool} from '../../../Infrastructure/Tools';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {CardList} from '../../../Common/EntityLists/CardList';
import {VatTypeList} from '../../../Common/EntityLists/VatTypeList';
import {ChargesTypeList} from '../../../Common/EntityLists/ChargesTypeList';
import {MeasurementList} from '../../../Common/EntityLists/MeasurementList';
import {PackageTypeList} from '../../../Common/EntityLists/PackageTypeList';
import {VATTypesGroupPM} from '../../../Common/EntityPMs/VATTypesGroupPM';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import { DecimalFormatter } from '../../../Infrastructure/Utilities/DecimalFormatter';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { QuoteTool } from '../../../Quote/Tools';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';
import { QuoteChargesBehaviours } from '../Behaviours/QuoteChargesBehaviours';
import { QuoteTariffsBehaviours } from '../Behaviours/QuoteTariffsBehaviours';

@Component({
    selector: 'FCLChargesComponent',    
    templateUrl: './FCLChargesComponent.html',
})

export class FCLChargesComponent extends BaseComponent implements OnDestroy {
    public EntityPM: QuotePM = null;
    public ObjectTableName: string = "Quote";
    public DataContext = this;
    public ItemsSource: ObservableCollection;
    public IsAdhoc: boolean = true;
    public TransportModeId: string;
    public LocalCurrencyId: string;
    public LocalCurrencyCode: string;
    public AllInMatchText: string;
    IsShowTotalPerContainer: boolean = false;
    public IsPriceCheckVisible: boolean = false;
    private entityResourceService: EntityResourceService = new EntityResourceService();;
    public ComponentRef: any;

    private CurrentSession = SessionLocator.SelectedSession;
    public HideFCLAllIn: boolean = false;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.TransportModeId = this.EntityPM.TransportModeId;
        this.IsAdhoc = this.EntityPM.QuoteTypeCode == "A" ? true : false;
        this.LocalCurrencyId = SessionLocator.LocalCurrencyId;
        this.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
        this.ItemsSource = new ObservableCollection([]);
        this.AllInMatchText = TextCodeTranslator.Translate("Quote.M.UnableToDoAllIn") + "\n" + TextCodeTranslator.Translate("Quote.M.IfMatchesFrieghtCharge");
        this.HideFCLAllIn = SessionLocator.TenantPM.HideFCLAllIn;

       if( FeatureLocator.HasFeaturePermession("Quote", "TOTALPERCONTAINER")) {
            this.IsShowTotalPerContainer = true;
        }

        var featureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "TAR" && d.TenantNumber == SessionLocator.Tenant)[0];
        if (FeatureLocator.HasFeaturePermession("Quote", "QuotePriceCheck") && featureToggle != null) {
            this.IsPriceCheckVisible = true;
        }

        this.InitBehaviours();

        this.SetLabels();
        this.SetFeaturesAndFlags();
        this.SetUIProperties();
        this.CheckUpdateQuantities();
        this.SetGridColumns();
        this.BuildItemsSource();
        this.InitializeProfit();
        this.Listen();
    }

    public Behaviours: QuoteChargesBehaviours;
    public TariffBehaviours: QuoteTariffsBehaviours;
    InitBehaviours() {
        this.Behaviours = new QuoteChargesBehaviours(this.EntityPM);
        this.TariffBehaviours = new QuoteTariffsBehaviours(this.EntityPM);
        this.OnBehavioursEvents();
    }
    UpdateBehaviours() {
        this.Behaviours.EntityPM = this.EntityPM;
        this.TariffBehaviours.EntityPM = this.EntityPM;
    }
    OnBehavioursEvents() {
        this.Behaviours.PackageTypesLoadCompleted.subscribe(() => {
            this.SetLabelsAttached();
        });
    }

    public IsRegionalTaxVisible: boolean = false;
    SetFeaturesAndFlags() {
        this.IsRegionalTaxVisible = this.Behaviours.IsRegionalTaxVisible();
    }
        
    private SessionEvent: any = null;
    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;  
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "FCLPackagesChanged") {
                    this.SetUIProperties();
                    this.BuildItemsSource();
                    this.BuildProfitData();
                }
            });

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.UpdateBehaviours();

                    this.SetUIProperties();
                    this.BuildItemsSource();
                    this.BuildProfitData();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.UpdateBehaviours();

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

                    this.SetLabelsAttached();
                    this.SetUIProperties();
                    this.UpdateCharges();
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

    // SetLabels
    public CostQuentityHeader: any[] = [];
    public CostPriceHeader: any[] = [];
    public CostAmountHeader: any[] = [];
    public SaleQuantityHeader: any[] = [];
    public SalePriceHeader: any[] = [];
    public SaleAmountHeader: any[] = [];
    public SaleLocalAmountHeader: any[] = [];
    public Cost1Header: any[] = [];
    public Cost2Header: any[] = [];
    public Cost3Header: any[] = [];
    public Cost4Header: any[] = [];
    public Cost5Header: any[] = [];
    public Sale1Header: any[] = [];
    public Sale2Header: any[] = [];
    public Sale3Header: any[] = [];
    public Sale4Header: any[] = [];
    public Sale5Header: any[] = [];
    public CostMinAmountHeader: any = [];
    public CostMaxAmountHeader: any = [];
    public SaleMinAmountHeader: any = [];
    public SaleMaxAmountHeader: any = [];
    public Cost1HeaderTooltip: string = null;
    public Cost2HeaderTooltip: string = null;
    public Cost3HeaderTooltip: string = null;
    public Cost4HeaderTooltip: string = null;
    public Cost5HeaderTooltip: string = null;
    public Sale1HeaderTooltip: string = null;
    public Sale2HeaderTooltip: string = null;
    public Sale3HeaderTooltip: string = null;
    public Sale4HeaderTooltip: string = null;
    public Sale5HeaderTooltip: string = null;
    SetLabels() {
        this.CostQuentityHeader = TextCodeTranslator.Translate("Quote.O.Charges.CostQuantity").split('%n');
        this.CostPriceHeader = TextCodeTranslator.Translate("Quote.O.Charges.CostPrice").split('%n');
        this.CostAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.CostAmount").split('%n');
        this.SaleQuantityHeader = TextCodeTranslator.Translate("Quote.O.Charges.SaleQuantity").split('%n');
        this.SaleLocalAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.SaleAmountLocal").replace("%LocalCurrencyCode", this.LocalCurrencyCode).split('%n');
        this.CostMinAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.CostMinAmount", false).split('%n');
        this.CostMaxAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.CostMaxAmount", false).split('%n');
        this.SaleMinAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.SaleMinAmount", false).split('%n');
        this.SaleMaxAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.SaleMaxAmount", false).split('%n');
        this.SetLabelsAttached();
    }
    SetLabelsAttached() {
        var myCostString = TextCodeTranslator.Translate("Quote.O.Charges.Cost");
        var mySaleCurrencyCode = AppTool.IsNullOrEmpty(this.SaleCurrencyCode) ? "" : this.SaleCurrencyCode;

        if (this.IsSaleCurrencySameAsCost || this.IsMultiCurrency) {
            this.SalePriceHeader = TextCodeTranslator.Translate("Quote.O.Charges.SalePrice").replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").split('%n');
            this.SaleAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.SaleAmount").replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").split('%n');
        }

        else {
            this.SalePriceHeader = TextCodeTranslator.Translate("Quote.O.Charges.SalePrice").replace("%SaleCurrencyCode", mySaleCurrencyCode).split('%n');
            this.SaleAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.SaleAmount").replace("%SaleCurrencyCode", mySaleCurrencyCode).split('%n');
        }

        this.Cost1Header = [2];
        this.Cost2Header = [2];
        this.Cost3Header = [2];
        this.Cost4Header = [2];
        this.Cost5Header = [2];
        this.Sale1Header = [3];
        this.Sale2Header = [3];
        this.Sale3Header = [3];
        this.Sale4Header = [3];
        this.Sale5Header = [3];

        var q1 = AppTool.IsNullOrZero(this.EntityPM.PackageType1Quantity) ? "" : this.EntityPM.PackageType1Quantity.toString() + "X";
        var q2 = AppTool.IsNullOrZero(this.EntityPM.PackageType2Quantity) ? "" : this.EntityPM.PackageType2Quantity.toString() + "X";
        var q3 = AppTool.IsNullOrZero(this.EntityPM.PackageType3Quantity) ? "" : this.EntityPM.PackageType3Quantity.toString() + "X";
        var q4 = AppTool.IsNullOrZero(this.EntityPM.PackageType4Quantity) ? "" : this.EntityPM.PackageType4Quantity.toString() + "X";
        var q5 = AppTool.IsNullOrZero(this.EntityPM.PackageType5Quantity) ? "" : this.EntityPM.PackageType5Quantity.toString() + "X";

        var p1: string = "";
        var p2: string = "";
        var p3: string = "";
        var p4: string = "";
        var p5: string = "";
        if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType1Id)) {
            var item = this.Behaviours.AllPackageTypes.filter(d => d.Id == this.EntityPM.PackageType1Id)[0];
            if (item) {
                p1 = item.Code;

                var value1 = q1 + " " + item.EnglishName;
                this.Cost1HeaderTooltip = myCostString + " " + q1 + value1;
                this.Sale1HeaderTooltip = TextCodeTranslator.Translate("Quote.O.Charges.SalePrice").replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").replace("%n", " ").replace("\n"," ") + " " + value1;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType2Id)) {
            var item = this.Behaviours.AllPackageTypes.filter(d => d.Id == this.EntityPM.PackageType2Id)[0];
            if (item) {
                p2 = item.Code;

                var value2 = q2 + " " + item.EnglishName;
                this.Cost2HeaderTooltip = myCostString + " " + value2;
                this.Sale2HeaderTooltip = TextCodeTranslator.Translate("Quote.O.Charges.SalePrice").replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").replace("%n", " ").replace("\n", " ") + " " + value2;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType3Id)) {
            var item = this.Behaviours.AllPackageTypes.filter(d => d.Id == this.EntityPM.PackageType3Id)[0];
            if (item) {
                p3 = item.Code;

                var value3 = q3 + " " + item.EnglishName;
                this.Cost3HeaderTooltip = myCostString + " " + value3;
                this.Sale3HeaderTooltip = TextCodeTranslator.Translate("Quote.O.Charges.SalePrice").replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").replace("%n", " ").replace("\n", " ") + " " + value3;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType4Id)) {
            var item = this.Behaviours.AllPackageTypes.filter(d => d.Id == this.EntityPM.PackageType4Id)[0];
            if (item) {
                p4 = item.Code;

                var value4 = q4 + " " + item.EnglishName;
                this.Cost4HeaderTooltip = myCostString + " " + value4;
                this.Sale4HeaderTooltip = TextCodeTranslator.Translate("Quote.O.Charges.SalePrice").replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").replace("%n", " ").replace("\n", " ") + " " + value4;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType5Id)) {
            var item = this.Behaviours.AllPackageTypes.filter(d => d.Id == this.EntityPM.PackageType5Id)[0];
            if (item) {
                p5 = item.Code;

                var value5 = q5 + " " + item.EnglishName;
                this.Cost5HeaderTooltip = myCostString + " " + value5;
                this.Sale5HeaderTooltip = TextCodeTranslator.Translate("Quote.O.Charges.SalePrice").replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").replace("%n", " ").replace("\n", " ") + " " + value5;
            }
        }

        this.Cost1Header[0] = myCostString;
        this.Cost2Header[0] = myCostString;
        this.Cost3Header[0] = myCostString;
        this.Cost4Header[0] = myCostString;
        this.Cost5Header[0] = myCostString;
        this.Cost1Header[1] = q1 + p1;
        this.Cost2Header[1] = q2 + p2;
        this.Cost3Header[1] = q3 + p3;
        this.Cost4Header[1] = q4 + p4;
        this.Cost5Header[1] = q5 + p5;

        this.Sale1Header[0] = this.SalePriceHeader[0];
        this.Sale2Header[0] = this.SalePriceHeader[0];
        this.Sale3Header[0] = this.SalePriceHeader[0];
        this.Sale4Header[0] = this.SalePriceHeader[0];
        this.Sale5Header[0] = this.SalePriceHeader[0];
        this.Sale1Header[1] = this.SalePriceHeader[1];
        this.Sale2Header[1] = this.SalePriceHeader[1];
        this.Sale3Header[1] = this.SalePriceHeader[1];
        this.Sale4Header[1] = this.SalePriceHeader[1];
        this.Sale5Header[1] = this.SalePriceHeader[1];
        this.Sale1Header[2] = q1 + p1;
        this.Sale2Header[2] = q2 + p2;
        this.Sale3Header[2] = q3 + p3;
        this.Sale4Header[2] = q4 + p4;
        this.Sale5Header[2] = q5 + p5;
    }

    public VATColumnWidth: number = 100;
    public IsCostMinMaxColumnVisible: boolean = false;
    public IsSaleMinMaxColumnVisible: boolean = false;
    SetGridColumns() {
        var isCostMinMaxColumnVisible: boolean = false;
        var isSaleMinMaxColumnVisible: boolean = false;

        if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode != "BCNT").length > 0) {
            isCostMinMaxColumnVisible = true;
        }

        if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode != "BCNT").length > 0) {
            isSaleMinMaxColumnVisible = true;
        }

        this.IsCostMinMaxColumnVisible = isCostMinMaxColumnVisible;
        this.IsSaleMinMaxColumnVisible = isSaleMinMaxColumnVisible;
    }
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

        this.ItemsSource.Collection.forEach((item: FCLChargesComponent) => {
            item.SetUIProperties();
        });

        this.UIProperties.SetEnabled("RegionalTaxId", this.ObjectTableName, this.IsEditingEnabled);

        this.SetUIProperties_Summary();
        this.SetUIProperties_Columns();
    }
    SetUIProperties_Summary() {
        var isExchangeRateEnabled = false;

        if (this.IsEditingEnabled) {
            if (FeatureLocator.HasFeaturePermession("Quote", "QouteEditExchangeRate")) {
                if (this.SaleCurrencyId) {
                    if (this.SaleCurrencyId != SessionLocator.LocalCurrencyId) {
                        isExchangeRateEnabled = false;
                    }
                }
            }
        }

        this.IsExchangeRateEnabled = isExchangeRateEnabled;
        this.IsCurrencyFilterVisible = this.LocalCurrencyId == this.EntityPM.SaleCurrencyId ? false : true;
        this.UIProperties.SetEnabled("SaleCurrencyId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ExchangeRate", this.ObjectTableName, isExchangeRateEnabled);
        this.UIProperties.SetEnabled("IsFixedPrice", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("TotalPerContainer", this.ObjectTableName, this.IsEditingEnabled);
    }

    public IsCostQuantityVisible: boolean = false;
    public IsCostPriceVisible: boolean = false;
    public IsSaleQuantityVisible: boolean = false;
    public IsSalePriceVisible: boolean = false;
    public IsCost1Visible: boolean = false;
    public IsCost2Visible: boolean = false;
    public IsCost3Visible: boolean = false;
    public IsCost4Visible: boolean = false;
    public IsCost5Visible: boolean = false;
    public IsSale1Visible: boolean = false;
    public IsSale2Visible: boolean = false;
    public IsSale3Visible: boolean = false;
    public IsSale4Visible: boolean = false;
    public IsSale5Visible: boolean = false;
    private SetUIProperties_Columns() {
        var isCostQuantityVisible: boolean = false;
        var isCostPriceVisible: boolean = false;
        var isSaleQuantityVisible: boolean = false;
        var isSalePriceVisible: boolean = false;

        var isCost1Visible: boolean = false;
        var isCost2Visible: boolean = false;
        var isCost3Visible: boolean = false;
        var isCost4Visible: boolean = false;
        var isCost5Visible: boolean = false;

        var isSale1Visible: boolean = false;
        var isSale2Visible: boolean = false;
        var isSale3Visible: boolean = false;
        var isSale4Visible: boolean = false;
        var isSale5Visible: boolean = false;

        if (this.EntityPM.QuoteCharges.filter(d => d.CostMeasurementCode != "BCNT").length > 0) {
            if (this.IsAdhoc) {
                isCostQuantityVisible = true;
                isSaleQuantityVisible = true;
            }

            isCostPriceVisible = true;
            isSalePriceVisible = true;
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType1Id)) {
            isCost1Visible = true;
            isSale1Visible = true;
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType2Id)) {
            isCost2Visible = true;
            isSale2Visible = true;
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType3Id)) {
            isCost3Visible = true;
            isSale3Visible = true;
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType4Id)) {
            isCost4Visible = true;
            isSale4Visible = true;
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType5Id)) {
            isCost5Visible = true;
            isSale5Visible = true;
        }

        this.IsCostQuantityVisible = isCostQuantityVisible;
        this.IsCostPriceVisible = isCostPriceVisible;
        this.IsSaleQuantityVisible = isSaleQuantityVisible;
        this.IsSalePriceVisible = isSalePriceVisible;

        this.IsCost1Visible = isCost1Visible;
        this.IsCost2Visible = isCost2Visible;
        this.IsCost3Visible = isCost3Visible;
        this.IsCost4Visible = isCost4Visible;
        this.IsCost5Visible = isCost5Visible;

        this.IsSale1Visible = isSale1Visible;
        this.IsSale2Visible = isSale2Visible;
        this.IsSale3Visible = isSale3Visible;
        this.IsSale4Visible = isSale4Visible;
        this.IsSale5Visible = isSale5Visible;

    }

    public SelectedRow: FCLQuoteChargeItem = null;
    OnRowSelected(itemComponent: FCLQuoteChargeItem) {
        this.SelectedRow = itemComponent;
    }

    BuildItemsSource() {

        var itemsCollection: FCLQuoteChargeItem[] = [];

        this.EntityPM.QuoteCharges.filter(f => f.ChargesGroupCode == "FRT").forEach((item) => {
            itemsCollection.push(new FCLQuoteChargeItem(item, this, false));
        })

        this.EntityPM.QuoteCharges.filter(f => f.ChargesGroupCode != "FRT").forEach((item) => {
            itemsCollection.push(new FCLQuoteChargeItem(item, this, false));
        })

        this.ItemsSource.InsertCollection(itemsCollection);

        this.SetGridColumnsWidth();
    }

    AddChargeClicked() {
        var itemComponent = new FCLQuoteChargeItem(this.Behaviours.CreateQuoteCharge(), this, true);
        this.RunAddEditCharge(itemComponent, this.Behaviours.AddChargeLabel);
    }
    EditChargeClicked(itemComponent: FCLQuoteChargeItem) {
        this.RunAddEditCharge(itemComponent, this.Behaviours.EditChargeLabel);
    }
    DeleteChargeClicked(itemComponent: FCLQuoteChargeItem) {

        this.Behaviours.DeleteChargeCompleted.subscribe(() => {
            if (itemComponent.ChargesGroupCode == "FRT") {
                this.OnFreightAmountChanged();
            }

            this.SetGridColumns();
            this.BuildItemsSource();
            this.ComputeTotals();
        });

        this.Behaviours.DeleteCharge(itemComponent.EntityPM);
    }
    RunAddEditCharge(itemComponent: FCLQuoteChargeItem, windowTitle: string) {

        itemComponent.SetEditScreenGridHeaders();

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.Width = 880;
        logitudeWindow.Height = 550;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./QuoteModules/QuoteCharges/Components/AddEditFCLChargeComponent');

        logitudeWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.SetGridColumns();
            }
        });
    }

    PriceCheck() {
        this.entityResourceService.getEntityResourceByTableName("TariffLine").subscribe((res1: any) => {
            ServiceLocator.SendTotangoUserActivity("Tariff", "Generate from Quote");

            var betweenDate: Date = DateTool.GetCurrentDateAsUtc();

            if (this.EntityPM.DirectionId == "I") {
                betweenDate = this.EntityPM.ETA;
            }
            else {
                betweenDate = this.EntityPM.ETD;
            }

            var tariffType = "OFC";
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
    EditTariffClicked(item: FCLQuoteChargeItem) {
        if (item != null) {
            var editWindow = new LogitudeWindow();
            editWindow.ShowHeaderButtons = true;
            editWindow.Title = "Price Check";
            editWindow.Height = 770;
            editWindow.Width = 1500;
            editWindow.ShowEditComponent(item.TariffId, "Tariff", item.TariffVersion + "");
        }
    }
    DeleteTariff(item: FCLQuoteChargeItem) {
        if (item != null) {
            item.TariffId = null;
            item.TariffNumber = null;
            item.SetUIProperties();
        }
    }

    // Profit
    InitializeProfit() {
        this.SelectedCurrencyCode = this.SaleCurrencyCode;
        this.BuildProfitData();
    }
    IsLocalCurrency: boolean = false;
    OnSelectCurrency(myArgs: string) {
        this.SelectedCurrencyCode = myArgs;

        if (myArgs == this.LocalCurrencyCode) {
            this.IsLocalCurrency = true;
        }

        else {
            this.IsLocalCurrency = false;
        }

        this.BuildProfitData();
    }

    OnSaleCurrencyModeChanged() {

        this.SetLabelsAttached();

        this.Behaviours.ChargesTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.Behaviours.AllChargesTypes = myResponse.Result;

                var allInItems = this.ItemsSource.Collection.filter(d => d.IsAllIN == true);

                allInItems.forEach((item: FCLQuoteChargeItem) => {
                    item.IsAllIN = false;
                });

                this.ItemsSource.Collection.forEach((item: FCLQuoteChargeItem) => {
                    item.OnQuoteSaleCurrencyModeChanged();
                });

                allInItems.forEach((item: FCLQuoteChargeItem) => {
                    item.IsAllIN = true;
                });

                this.ComputeTotals();
            }
        });
    }
    OnQuoteSaleCurrencyDataChanged() {

        this.SetLabelsAttached();

        var allInItems = this.ItemsSource.Collection.filter(d => d.IsAllIN == true);

        allInItems.forEach((item: FCLQuoteChargeItem) => {
            item.IsAllIN = false;
        });

        this.ItemsSource.Collection.forEach((item: FCLQuoteChargeItem) => {
            item.OnQuoteSaleCurrencyDataChanged();
        });

        allInItems.forEach((item: FCLQuoteChargeItem) => {
            item.IsAllIN = true;
        });

        this.ComputeTotals();
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

    get IsMultiCurrency() { return this.EntityPM.IsMultiCurrency; }
    set IsMultiCurrency(newValue: boolean) {
        if (this.EntityPM.IsMultiCurrency != newValue) {
            this.EntityPM.IsMultiCurrency = newValue;
            this.SetLabelsAttached();
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

            this.SaleCurrencyCode = this.SelectedCurrencyCode = this.Behaviours.GetCurrencyCode(value);

            var myExchangeRate = this.Behaviours.GetQuoteSaleCurrencyRate(value);

            if (this.ExchangeRate != myExchangeRate) {
                this.ExchangeRate = myExchangeRate;
            }

            else {
                this.OnQuoteSaleCurrencyDataChanged();
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
            this.OnQuoteSaleCurrencyDataChanged();
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
                    this.Behaviours.AllRates = comp.RatesList;
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

    get TotalPerContainer() {
        return this.EntityPM.TotalPerContainer;
    }
    set TotalPerContainer(newValue: boolean) {
        if (this.EntityPM.TotalPerContainer != newValue) {
            this.EntityPM.TotalPerContainer = newValue;
           // IsSameCostCurrency
            //if (newValue == true) {
            //    this.IsSameCostCurrency = false;
            //    if (this.IsFixedCurrency == false) {
            //        this.IsFixedCurrency = true;
            //        this.SetFixedSameCurrency("F");
            //    }
            //}
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
            var mySaleProfitLocal = AppTool.Round(mySaleAmountLocal - myCostAmountLocal,2);

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
                if (!AppTool.IsNullOrZero(this.SummaryCostAmount)) {
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
            var entityQuantity: number = null;
            var isDifferentOrders: boolean = false;


            entityQuantity = this.EntityPM.TEU;
            if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "BTEU" && f.CostQuantity != entityQuantity).length > 0) {
                isDifferentOrders = true;
            }
            else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "BTEU" && f.SaleQuantity != entityQuantity).length > 0) {
                isDifferentOrders = true;
            }


            if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "QTY" && f.CostQuantity != this.EntityPM.NumberOfContainers).length > 0) {
                isDifferentOrders = true;
            }
            else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "QTY" && f.SaleQuantity != this.EntityPM.NumberOfContainers).length > 0) {
                isDifferentOrders = true;
            }


            if (this.EntityPM.QuoteCharges.filter(d => d.SaleUnitPrice != null || d.CostUnitPrice != null).length > 0) {
                if (this.EntityPM.QuoteCharges.filter(d => (d.CostMeasurementCode == "PRVL" && d.CostQuantity != this.EntityPM.ValueOfGoods) || (d.CostMeasurementCode == "PRVL" && d.CostQuantity != this.EntityPM.ValueOfGoods)).length > 0) {
                    updateMessage = "You have updated the Value of Goods, apply the new values?";
                }

                else if (this.EntityPM.QuoteCharges.filter(d => d.SaleMeasurementCode == "PRVL" && d.SaleQuantity != this.EntityPM.ValueOfGoods).length > 0) {
                    updateMessage = "You have updated the Value of Goods, apply the new values?";
                }
            }


            if (isDifferentOrders) {
                updateMessage = "You have updated the expected order details, apply the new values?";
            }

            this.UpdateQuantitiesMessage = updateMessage;
            this.UpdateQuantitiesMessageWidth = AppTool.GetTextWidth(updateMessage, 11);
            this.IsUpdateQuantitiesVisible = AppTool.IsNullOrEmpty(updateMessage) ? false : true;        
        }
    }
    UpdateQuantitiesClicked() {
        this.ItemsSource.Collection.forEach((item) => {
            item.SetCostQuantity();
            item.SetSaleQuantity();
        })

        this.CurrentSession.SessionEvent.emit("QuantitiesUpdated");
        //this.SetUIProperties_UpdateCharges();
    }
    UpdateCharges() {
        this.EntityPM.QuoteCharges.forEach((item) => {
            if (AppTool.IsNullOrEmpty(this.EntityPM.PackageType1Id)) {
                item.CostContainerType1UnitPrice = null;
                item.SaleContainerType1UnitPrice = null;
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.PackageType2Id)) {
                item.CostContainerType2UnitPrice = null;
                item.SaleContainerType2UnitPrice = null;
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.PackageType3Id)) {
                item.CostContainerType3UnitPrice = null;
                item.SaleContainerType3UnitPrice = null;
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.PackageType4Id)) {
                item.CostContainerType4UnitPrice = null;
                item.SaleContainerType4UnitPrice = null;
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.PackageType5Id)) {
                item.CostContainerType5UnitPrice = null;
                item.SaleContainerType5UnitPrice = null;
            }
        })

        this.BuildItemsSource();
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

            this.ItemsSource.Collection.forEach((item: FCLQuoteChargeItem) => {
                if (value == true) {
                    this.Behaviours.ChargesTypeListService.getSingleFromCache(item.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
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
        this.Behaviours.BuildTotalVATs();
    }

    VATDetailsClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = TextCodeTranslator.Translate("Quote.O.Charges.VATDetails");
        logitudeWindow.WindowArgs = { IsLocalCurrency: this.IsLocalCurrency, SaleCurrencyCode: this.SaleCurrencyCode, IsCurrencyFilterVisible: this.IsCurrencyFilterVisible, TotalVATs: this.EntityPM.TotalVATs };
        logitudeWindow.Show('./QuoteModules/QuoteCharges/Components/QuoteVATDetailsComponent');
    }

    OnFreightAmountChanged() {
        this.ItemsSource.Collection.filter(f => f.ChargesGroupCode != "FRT").forEach((item: FCLQuoteChargeItem) => {
            if (item.CostMeasurementCode == "PRFR") {
                item.SetCostQuantity();
            }

            if (item.SaleMeasurementCode == "PRFR") {
                item.SetSaleQuantity();
            }
        });
    }

    // RegionalTaxId
    get RegionalTaxId() { return this.EntityPM.RegionalTaxId; }
    set RegionalTaxId(newValue: string) {

        var oldValue = this.EntityPM.RegionalTaxId;

        if (this.EntityPM.RegionalTaxId != newValue) {
            this.EntityPM.RegionalTaxId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.RegionalTaxPercentage = null;
            }

            else {
                this.RegionalTaxPercentage = this.Behaviours.GetVatTypePercentage(newValue);

                if (AppTool.IsNullOrEmpty(oldValue)) {
                    this.ItemsSource.Collection.filter(f => f.IsRegionalTax == false && f.VatIsMultiPercentage == false).forEach(item => {
                        this.Behaviours.ChargesTypeListService.getSingleFromCache(item.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
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
export class FCLQuoteChargeItem extends BaseComponent {
    public QuotePM: QuotePM;
    public EntityPM: QuoteChargePM;   
    public ObjectTableName: string = "QuoteCharge";
    public DataContext = this;
    public IsNew: boolean = false;
    public TransportModeId: string;
    public IsAdhoc: boolean = false;
    public AllInMatchText: string;
    constructor(entity: QuoteChargePM, public fatherComponent: FCLChargesComponent, isNew: boolean) {
        super();
        this.IsNew = isNew;
        this.EntityPM = entity;    
        this.QuotePM = fatherComponent.EntityPM;
        this.IsAdhoc = fatherComponent.IsAdhoc;
        this.TransportModeId = fatherComponent.TransportModeId;
        this.AllInMatchText = fatherComponent.AllInMatchText;
        this.SetUIProperties();
        this.ReadVatTypeData();
        this.ComputeMarkUpString();
        this.SetUIProperties_CellsColors();
        this.ComputeMarkUp1String();
        this.ComputeMarkUp2String();
        this.ComputeMarkUp3String();
        this.ComputeMarkUp4String();
        this.ComputeMarkUp5String();
    }

    // SetUIProperties
    public IsEditingEnabled: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    SetUIProperties() {
        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {          
            if (this.IsAdhoc) {
                this.IsEditExchangeRateVisible = true;
            }
        }

        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;
        this.SetUIProperties_AllIn();
        this.SetUIProperties_Columns();
        this.SetUIProperties_CostFields();
        this.SetUIProperties_SaleFields();
        this.SetUIProperties_CellsColors();
        this.SetUIProperties_VAT();
        this.SetUIProperties_CostMinMax();
        this.SetUIProperties_SaleMinMax();
        this.SetUIProperties_IsRegionalTax();

        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, (this.IsEditingEnabled && !this.IsAllIN) ? true : false);
        this.UIProperties.SetEnabled("CostIsFixedRate", this.ObjectTableName, (this.IsEditingEnabled && !this.IsAllIN) ? true : false);
        this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("IsChargeBySteps", this.ObjectTableName, this.IsEditingEnabled);
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

    public AllInInfoTitle: string;
    public IsAllInCheckBoxVisible: boolean = false;
    public IsAllInInfoIconVisible: boolean = false;
    public IsAllInInfoIconVisible_AmoutLocal: boolean = false;
    SetUIProperties_AllIn() {
        var isAllInCheckBoxVisible = false;
        var isAllInInfoIconVisible = false;

        if (this.ChargesGroupCode != "FRT" && this.IsAdhoc) {
            var itemFrieght: QuoteChargePM = this.QuotePM.QuoteCharges.filter(d => d.ChargesGroupCode == "FRT")[0];

            if (itemFrieght) {
                if (this.SaleMeasurementId == itemFrieght.SaleMeasurementId && this.SaleCurrencyId == itemFrieght.SaleCurrencyId) {
                    isAllInCheckBoxVisible = true;
                }

                else {
                    isAllInInfoIconVisible = true;
                }
            }
        }

        this.IsAllInCheckBoxVisible = isAllInCheckBoxVisible;
        this.IsAllInInfoIconVisible = isAllInInfoIconVisible;
        this.IsAllInInfoIconVisible_AmoutLocal = this.IsAllIN && this.CostMeasurementCode == "BCNT" ? true : false;
        this.SetUIProperties_AllIn_CostCurrency();
        this.SetUIProperties_AllIn_SaleCurrency();
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
    SetUIProperties_AllIn_SaleCurrency() {

        var isEnabled_Id = false;
        var isEnabled_Rate = false;

        if (this.IsEditingEnabled) {
            if (this.QuotePM.IsMultiCurrency) {

                if (this.ChargesGroupCode == "FRT") {
                    if (this.QuotePM.QuoteCharges.filter(d => d.IsAllIN).length == 0) {
                        isEnabled_Id = true;
                    }
                }

                else if (!this.IsAllIN) {
                    isEnabled_Id = true;
                }

                if (isEnabled_Id) {
                    if (this.SaleCurrencyId) {
                        if (this.SaleCurrencyId != SessionLocator.LocalCurrencyId) {
                            if (this.SaleCurrencyId != this.fatherComponent.SaleCurrencyId) {
                                if (this.SaleCurrencyId != this.CostCurrencyId) {
                                    isEnabled_Rate = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        this.UIProperties.SetEnabled("SaleCurrencyId", this.ObjectTableName, isEnabled_Id);
        this.UIProperties.SetEnabled("SaleExchangeRate", this.ObjectTableName, isEnabled_Rate);
        this.UIProperties.SetEnabled("SaleIsFixedRate", this.ObjectTableName, false);
    }
    
    public IsEnabled_CostQuantity: boolean = false;
    public IsEnabled_CostUnitPrice: boolean = false;
    public IsEnabled_CostMinAmount: boolean = false;
    public IsEnabled_CostExchangeRate: boolean = false;
    public IsEnabled_CostUnitPriceFCL: boolean = false;
    SetUIProperties_CostFields() {
        var isEnabled_CostQuantity = false;
        var isEnabled_CostUnitPrice = false;
        var isEnabled_CostMinAmount = false;
        var isEnabled_CostMeasurement = false;
        var isEnabled_CostUnitPriceFCL = false;

        if (this.IsEditingEnabled) {
            isEnabled_CostQuantity = true;
            isEnabled_CostUnitPrice = true;
            isEnabled_CostMinAmount = true;
            isEnabled_CostMeasurement = true;
            isEnabled_CostUnitPriceFCL = true;

            switch (this.CostMeasurementCode) {
                //case "GRWT":
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
                        isEnabled_CostQuantity = false;
                        break;
                    }
            }

            if (this.IsAllIN) {
                isEnabled_CostUnitPrice = false;
                isEnabled_CostMinAmount = false;
                isEnabled_CostMeasurement = false;
                isEnabled_CostUnitPriceFCL = false;
            }

            if (this.CostMeasurementCode != "BCNT") {
                isEnabled_CostUnitPriceFCL = false;
            }

            if (this.CostMeasurementCode == "BCNT") {
                isEnabled_CostQuantity = false;
                isEnabled_CostUnitPrice = false;
            }

            if (this.IsChargeBySteps) {
                isEnabled_CostUnitPrice = false;
                isEnabled_CostMinAmount = false;
                isEnabled_CostUnitPriceFCL = false;
            }

            if (this.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(d => d.IsAllIN).length > 0) {
                isEnabled_CostQuantity = false;
                isEnabled_CostUnitPrice = false;
                isEnabled_CostMinAmount = false;
                isEnabled_CostMeasurement = false;
                isEnabled_CostUnitPriceFCL = false;
            }

            if (this.ChargesGroupCode == "FRT") {
                isEnabled_CostMeasurement = false;
            }

            if (this.CostMeasurementCode == "BCNT") {
                isEnabled_CostMinAmount = false;
            }
        }

        this.IsEnabled_CostQuantity = isEnabled_CostQuantity;
        this.IsEnabled_CostUnitPrice = isEnabled_CostUnitPrice;
        this.IsEnabled_CostMinAmount = isEnabled_CostMinAmount;
        this.IsEnabled_CostUnitPriceFCL = isEnabled_CostUnitPriceFCL;
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
            this.IsEnabled_CostUnitPriceFCL = isEnabled_CostCurrencyId;
        }
    }

    public IsEnabled_SaleQuantity: boolean = false;
    public IsEnabled_SaleUnitPrice: boolean = false;
    public IsEnabled_SaleMinAmount: boolean = false;
    public IsEnabled_SaleUnitPriceFCL: boolean = false;
    SetUIProperties_SaleFields() {
        var isEnabled_SaleQuantity = false;
        var isEnabled_SaleUnitPrice = false;
        var isEnabled_SaleMinAmount = false;
        var isEnabled_SaleMeasurement = false;
        var isEnabled_SaleUnitPriceFCL = false;

        if (this.IsEditingEnabled) {
            isEnabled_SaleQuantity = true;
            isEnabled_SaleUnitPrice = true;
            isEnabled_SaleMinAmount = true;
            isEnabled_SaleMeasurement = true;
            isEnabled_SaleUnitPriceFCL = true;

            switch (this.SaleMeasurementCode) {
                //case "GRWT":
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

            if (this.IsAllIN) {
                isEnabled_SaleUnitPrice = false;
                isEnabled_SaleMinAmount = false;
                isEnabled_SaleMeasurement = false;
                isEnabled_SaleUnitPriceFCL = false;
            }

            if (this.CostMeasurementCode != "BCNT") {
                isEnabled_SaleUnitPriceFCL = false;
            }

            if (this.SaleMeasurementCode == "BCNT") {
                isEnabled_SaleQuantity = false;
                isEnabled_SaleUnitPrice = false;
            }

            if (this.IsChargeBySteps) {
                isEnabled_SaleUnitPrice = false;
                isEnabled_SaleMinAmount = false;
                isEnabled_SaleUnitPriceFCL = false;
            }

            if (this.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(d => d.IsAllIN).length > 0) {
                isEnabled_SaleQuantity = false;
                isEnabled_SaleUnitPrice = false;
                isEnabled_SaleMinAmount = false;
                isEnabled_SaleMeasurement = false;
                isEnabled_SaleUnitPriceFCL = false;
            }

            if (this.ChargesGroupCode == "FRT") {
                isEnabled_SaleMeasurement = false;
            }

            if (this.SaleMeasurementCode == "BCNT") {
                isEnabled_SaleMinAmount = false;
            }
        }

        this.IsEnabled_SaleQuantity = isEnabled_SaleQuantity;
        this.IsEnabled_SaleUnitPrice = isEnabled_SaleUnitPrice;
        this.IsEnabled_SaleMinAmount = isEnabled_SaleMinAmount;
        this.IsEnabled_SaleUnitPriceFCL = isEnabled_SaleUnitPriceFCL;
        this.UIProperties.SetEnabled("SaleMeasurementId", this.ObjectTableName, isEnabled_SaleMeasurement);
        this.UIProperties.SetEnabled("SaleMinAmount", this.ObjectTableName, isEnabled_SaleMinAmount);
        this.UIProperties.SetEnabled("SaleMaxAmount", this.ObjectTableName, isEnabled_SaleMinAmount);
    }

    public SaleUnitPriceColor: string = FontTool.Black;
    public SaleUnitPrice1Color: string = FontTool.Black;
    public SaleUnitPrice2Color: string = FontTool.Black;
    public SaleUnitPrice3Color: string = FontTool.Black;
    public SaleUnitPrice4Color: string = FontTool.Black;
    public SaleUnitPrice5Color: string = FontTool.Black;
    public SaleTotalAmountColor: string = FontTool.Black;
    public SaleTotalAmountLocalColor: string = FontTool.Black;
    SetUIProperties_CellsColors() {

        // Price
        var myCostPrice: number = this.CostUnitPriceInSaleCurrency;
        var mySalePrice: number = this.SaleUnitPrice;

        if (AppTool.IsNullOrEmpty(this.CellMarkupText) && QuoteTool.IsQuoteStageDraft(this.fatherComponent.EntityPM) && !AppTool.IsNullOrEmpty(myCostPrice) && !AppTool.IsNullOrEmpty(mySalePrice) && (myCostPrice == mySalePrice)) {
            this.SaleUnitPriceColor = this.IsEditingEnabled ? FontTool.Orange : FontTool.CellDisabledColor;
        }

        else if (AppTool.IsNullOrEmpty(myCostPrice) || AppTool.IsNullOrEmpty(mySalePrice)) {
            this.SaleUnitPriceColor = this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor;
        }

        else {
            this.SaleUnitPriceColor = mySalePrice < myCostPrice ? FontTool.Red : (mySalePrice > myCostPrice ? FontTool.Green : (this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor));
        }

        // Price1
        var myCostPrice1: number = this.CostUnitPrice1InSaleCurrency;
        var mySalePrice1: number = this.SaleContainerType1UnitPrice;
        if (AppTool.IsNullOrEmpty(this.CellMarkup1Text) && QuoteTool.IsQuoteStageDraft(this.fatherComponent.EntityPM) && !AppTool.IsNullOrEmpty(myCostPrice1) && !AppTool.IsNullOrEmpty(mySalePrice1) && (myCostPrice1 == mySalePrice1)) {
            this.SaleUnitPrice1Color = this.IsEditingEnabled ? FontTool.Orange : FontTool.CellDisabledColor;
        }

        else if (AppTool.IsNullOrEmpty(myCostPrice1) || AppTool.IsNullOrEmpty(mySalePrice1)) {
            this.SaleUnitPrice1Color = this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor;
        }

        else {
            this.SaleUnitPrice1Color = mySalePrice1 < myCostPrice1 ? FontTool.Red : (mySalePrice1 > myCostPrice1 ? FontTool.Green : (this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor));
        }

        // Price2
        var myCostPrice2: number = this.CostUnitPrice2InSaleCurrency;
        var mySalePrice2: number = this.SaleContainerType2UnitPrice;
        if (AppTool.IsNullOrEmpty(this.CellMarkup2Text) && QuoteTool.IsQuoteStageDraft(this.fatherComponent.EntityPM) && !AppTool.IsNullOrEmpty(myCostPrice2) && !AppTool.IsNullOrEmpty(mySalePrice2) && (myCostPrice2 == mySalePrice2)) {
            this.SaleUnitPrice2Color = this.IsEditingEnabled ? FontTool.Orange : FontTool.CellDisabledColor;
        }

        else if (AppTool.IsNullOrEmpty(myCostPrice2) || AppTool.IsNullOrEmpty(mySalePrice2)) {
            this.SaleUnitPrice2Color = this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor;
        }

        else {
            this.SaleUnitPrice2Color = mySalePrice2 < myCostPrice2 ? FontTool.Red : (mySalePrice2 > myCostPrice2 ? FontTool.Green : (this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor));
        }

        // Price3
        var myCostPrice3: number = this.CostUnitPrice3InSaleCurrency;
        var mySalePrice3: number = this.SaleContainerType3UnitPrice;
        if (AppTool.IsNullOrEmpty(this.CellMarkup3Text) && QuoteTool.IsQuoteStageDraft(this.fatherComponent.EntityPM) && !AppTool.IsNullOrEmpty(myCostPrice3) && !AppTool.IsNullOrEmpty(mySalePrice3) && (myCostPrice3 == mySalePrice3)) {
            this.SaleUnitPrice3Color = this.IsEditingEnabled ? FontTool.Orange : FontTool.CellDisabledColor;
        }

        else if (AppTool.IsNullOrEmpty(myCostPrice3) || AppTool.IsNullOrEmpty(mySalePrice3)) {
            this.SaleUnitPrice3Color = this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor;
        }

        else {
            this.SaleUnitPrice3Color = mySalePrice3 < myCostPrice3 ? FontTool.Red : (mySalePrice3 > myCostPrice3 ? FontTool.Green : (this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor));
        }

        // Price4
        var myCostPrice4: number = this.CostUnitPrice4InSaleCurrency;
        var mySalePrice4: number = this.SaleContainerType4UnitPrice;
        if (AppTool.IsNullOrEmpty(this.CellMarkup4Text) && QuoteTool.IsQuoteStageDraft(this.fatherComponent.EntityPM) && !AppTool.IsNullOrEmpty(myCostPrice4) && !AppTool.IsNullOrEmpty(mySalePrice4) && (myCostPrice4 == mySalePrice4)) {
            this.SaleUnitPrice4Color = this.IsEditingEnabled ? FontTool.Orange : FontTool.CellDisabledColor;
        }

        else if (AppTool.IsNullOrEmpty(myCostPrice4) || AppTool.IsNullOrEmpty(mySalePrice4)) {
            this.SaleUnitPrice4Color = this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor;
        }

        else {
            this.SaleUnitPrice4Color = mySalePrice4 < myCostPrice4 ? FontTool.Red : (mySalePrice4 > myCostPrice4 ? FontTool.Green : (this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor));
        }

        // Price5
        var myCostPrice5: number = this.CostUnitPrice5InSaleCurrency;
        var mySalePrice5: number = this.SaleContainerType5UnitPrice;
        if (AppTool.IsNullOrEmpty(this.CellMarkup5Text) && QuoteTool.IsQuoteStageDraft(this.fatherComponent.EntityPM) && !AppTool.IsNullOrEmpty(myCostPrice5) && !AppTool.IsNullOrEmpty(mySalePrice5) && (myCostPrice5 == mySalePrice5)) {
            this.SaleUnitPrice5Color = this.IsEditingEnabled ? FontTool.Orange : FontTool.CellDisabledColor;
        }

        else if (AppTool.IsNullOrEmpty(myCostPrice5) || AppTool.IsNullOrEmpty(mySalePrice5)) {
            this.SaleUnitPrice5Color = this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor;
        }

        else {
            this.SaleUnitPrice5Color = mySalePrice5 < myCostPrice5 ? FontTool.Red : (mySalePrice5 > myCostPrice5 ? FontTool.Green : (this.IsEditingEnabled ? FontTool.Black : FontTool.CellDisabledColor));
        }
    }
  
    public IsCostQuantityColumnVisible: boolean = false;
    public IsCostPriceColumnVisible: boolean = false;
    public IsSaleQuantityColumnVisible: boolean = false;
    public IsSalePriceColumnVisible: boolean = false;
    public IsCost1Visible: boolean = false;
    public IsCost2Visible: boolean = false;
    public IsCost3Visible: boolean = false;
    public IsCost4Visible: boolean = false;
    public IsCost5Visible: boolean = false;
    public IsSale1Visible: boolean = false;
    public IsSale2Visible: boolean = false;
    public IsSale3Visible: boolean = false;
    public IsSale4Visible: boolean = false;
    public IsSale5Visible: boolean = false;
    public SetUIProperties_Columns() {
        var isCostQuantityColumnVisible: boolean = false;
        var isCostPriceColumnVisible: boolean = false;
        var isSaleQuantityColumnVisible: boolean = false;
        var isSalePriceColumnVisible: boolean = false;

        var isCost1Visible: boolean = false;
        var isCost2Visible: boolean = false;
        var isCost3Visible: boolean = false;
        var isCost4Visible: boolean = false;
        var isCost5Visible: boolean = false;

        var isSale1Visible: boolean = false;
        var isSale2Visible: boolean = false;
        var isSale3Visible: boolean = false;
        var isSale4Visible: boolean = false;
        var isSale5Visible: boolean = false;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.CostMeasurementCode)) {

            if (this.EntityPM.CostMeasurementCode != "BCNT") {
                isCostPriceColumnVisible = true;
                isSalePriceColumnVisible = true;

                if (this.QuotePM.QuoteTypeCode == "A") {
                    isCostQuantityColumnVisible = true;
                    isSaleQuantityColumnVisible = true;
                }
            }

            if (!AppTool.IsNullOrEmpty(this.QuotePM.PackageType1Id) && this.EntityPM.CostMeasurementCode == "BCNT") {
                isCost1Visible = true;
                isSale1Visible = true;
            }

            if (!AppTool.IsNullOrEmpty(this.QuotePM.PackageType2Id) && this.EntityPM.CostMeasurementCode == "BCNT") {
                isCost2Visible = true;
                isSale2Visible = true;
            }

            if (!AppTool.IsNullOrEmpty(this.QuotePM.PackageType3Id) && this.EntityPM.CostMeasurementCode == "BCNT") {
                isCost3Visible = true;
                isSale3Visible = true;
            }

            if (!AppTool.IsNullOrEmpty(this.QuotePM.PackageType4Id) && this.EntityPM.CostMeasurementCode == "BCNT") {
                isCost4Visible = true;
                isSale4Visible = true;
            }

            if (!AppTool.IsNullOrEmpty(this.QuotePM.PackageType5Id) && this.EntityPM.CostMeasurementCode == "BCNT") {
                isCost5Visible = true;
                isSale5Visible = true;
            }
        }

        this.IsCostQuantityColumnVisible = isCostQuantityColumnVisible;
        this.IsCostPriceColumnVisible = isCostPriceColumnVisible;
        this.IsSaleQuantityColumnVisible = isSaleQuantityColumnVisible;
        this.IsSalePriceColumnVisible = isSalePriceColumnVisible;

        this.IsCost1Visible = isCost1Visible;
        this.IsCost2Visible = isCost2Visible;
        this.IsCost3Visible = isCost3Visible;
        this.IsCost4Visible = isCost4Visible;
        this.IsCost5Visible = isCost5Visible;

        this.IsSale1Visible = isSale1Visible;
        this.IsSale2Visible = isSale2Visible;
        this.IsSale3Visible = isSale3Visible;
        this.IsSale4Visible = isSale4Visible;
        this.IsSale5Visible = isSale5Visible;
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

    public CostMinMaxIconTitle: string = "";
    public CostMinMaxIconIsVisibile: boolean = false;
    SetUIProperties_CostMinMax() {
        var iTitle: string = null;
        var iVisible: boolean = false;

        if (this.CostMeasurementCode != "BCNT") {
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
        }

        this.CostMinMaxIconTitle = iTitle;
        this.CostMinMaxIconIsVisibile = iVisible;
    }

    public SaleMinMaxIconTitle: string = "";
    public SaleMinMaxIconIsVisibile: boolean = false;
    SetUIProperties_SaleMinMax() {
        var iTitle: string = null;
        var iVisible: boolean = false;

        if (this.SaleMeasurementCode != "BCNT") {
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
        }

        this.SaleMinMaxIconTitle = iTitle;
        this.SaleMinMaxIconIsVisibile = iVisible;
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
                this.fatherComponent.Behaviours.ChargesTypeListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: ChargesTypeList = myResponse.Result;

                        if (list) {
                            this.OnChargesTypeChanged(list);
                        }

                        else {
                            this.fatherComponent.Behaviours.ChargesTypeListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
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
                this.IsRegionalTax = list.ApplyRegionalTax;
            }

            this.ChargesTypeCode = list.Code;
            this.ChargesTypeName = list.EnglishName;
            this.ChargesGroupCode = list.ChargesGroupCode;
            this.CostMeasurementId = !AppTool.IsNullOrEmpty(list.ContainerMeasurementId) ? list.ContainerMeasurementId : list.MeasurementId;
            this.EntityPM.IsBackToBack = list.IsBackToBack;

            this.CostCurrencyId = this.fatherComponent.Behaviours.GetCostCurrencyOnChargeTypeChanged(list);
            this.SaleCurrencyId = this.fatherComponent.Behaviours.GetSaleCurrencyOnChargeTypeChanged(list, this.EntityPM);
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
                this.fatherComponent.Behaviours.VatTypeListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
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
                                this.VatPercentage = this.fatherComponent.Behaviours.GetVatTypePercentage(value);
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
            this.ReadVatTypeData
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
                this.fatherComponent.Behaviours.CardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
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
                var list: MeasurementList = this.fatherComponent.Behaviours.AllMeasurements.filter(d => d.Id == value)[0];
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
        }
    }

    get CostMeasurementCode() { return this.EntityPM.CostMeasurementCode; }
    set CostMeasurementCode(value: string) {
        if (this.EntityPM.CostMeasurementCode != value) {
            this.EntityPM.CostMeasurementCode = value;

            if (value == "BCNT") {
                this.EntityPM.CostMinAmount = null;
                this.EntityPM.CostMaxAmount = null;
            }

            this.SetUIProperties_CostMinMax();
            this.fatherComponent.SetGridColumns();

            this.SetCostQuantity();
            this.SetUIProperties_CostFields();
            this.SetUIProperties_AllIn();
            this.UpdateCostSaleDataVisibility();            
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
            this.SetUIProperties_AllIn();
            this.SetUIProperties_AllIn_CostCurrency();
            this.SetUIProperties_AllIn_SaleCurrency();

            this.CostCurrencyCode = this.fatherComponent.Behaviours.GetCurrencyCode(value);
            this.CostExchangeRate = this.fatherComponent.Behaviours.GetCurrencyRate(value);
            this.RelativeRateDate = DateTool.GetRelativeRateDate(DateTool.GetCurrentDateAsUtc(), this.fatherComponent.Behaviours.GetCurrencyRateDate(value), "ago");

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
            this.EntityPM.CostQuantity = AppTool.Round(value, 2);
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

    get CostContainerType1UnitPrice() { return this.EntityPM.CostContainerType1UnitPrice; }
    set CostContainerType1UnitPrice(value: number) {
        if (this.EntityPM.CostContainerType1UnitPrice != value) {
            this.EntityPM.CostContainerType1UnitPrice = AppTool.Round(value, 3);

            if (AppTool.IsNullOrEmpty(value)) {
                this.ContainerType1MarkUpValue = 0;
                this.ContainerType1MarkUpTypeCode = "F";
            }

            this.ComputeCostAmounts();
            this.ComputeCostInSalePrice1();
        }
    }

    get CostContainerType2UnitPrice() { return this.EntityPM.CostContainerType2UnitPrice; }
    set CostContainerType2UnitPrice(value: number) {
        if (this.EntityPM.CostContainerType2UnitPrice != value) {
            this.EntityPM.CostContainerType2UnitPrice = AppTool.Round(value, 3);

            if (AppTool.IsNullOrEmpty(value)) {
                this.ContainerType2MarkUpValue = 0;
                this.ContainerType2MarkUpTypeCode = "F";
            }

            this.ComputeCostAmounts();
            this.ComputeCostInSalePrice2();
        }
    }

    get CostContainerType3UnitPrice() { return this.EntityPM.CostContainerType3UnitPrice; }
    set CostContainerType3UnitPrice(value: number) {
        if (this.EntityPM.CostContainerType3UnitPrice != value) {
            this.EntityPM.CostContainerType3UnitPrice = AppTool.Round(value, 3);

            if (AppTool.IsNullOrEmpty(value)) {
                this.ContainerType3MarkUpValue = 0;
                this.ContainerType3MarkUpTypeCode = "F";
            }

            this.ComputeCostAmounts();
            this.ComputeCostInSalePrice3();
        }
    }

    get CostContainerType4UnitPrice() { return this.EntityPM.CostContainerType4UnitPrice; }
    set CostContainerType4UnitPrice(value: number) {
        if (this.EntityPM.CostContainerType4UnitPrice != value) {
            this.EntityPM.CostContainerType4UnitPrice = AppTool.Round(value, 3);

            if (AppTool.IsNullOrEmpty(value)) {
                this.ContainerType4MarkUpValue = 0;
                this.ContainerType4MarkUpTypeCode = "F";
            }

            this.ComputeCostAmounts();
            this.ComputeCostInSalePrice4();
        }
    }

    get CostContainerType5UnitPrice() { return this.EntityPM.CostContainerType5UnitPrice; }
    set CostContainerType5UnitPrice(value: number) {
        if (this.EntityPM.CostContainerType5UnitPrice != value) {
            this.EntityPM.CostContainerType5UnitPrice = AppTool.Round(value, 3);

            if (AppTool.IsNullOrEmpty(value)) {
                this.ContainerType5MarkUpValue = 0;
                this.ContainerType5MarkUpTypeCode = "F";
            }

            this.ComputeCostAmounts();
            this.ComputeCostInSalePrice5();
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

    // InSaleCurrency
    get CostUnitPriceInSaleCurrency() { return this.EntityPM.CostUnitPriceInSaleCurrency; }
    set CostUnitPriceInSaleCurrency(value: number) {
        if (this.EntityPM.CostUnitPriceInSaleCurrency != value) {
            this.EntityPM.CostUnitPriceInSaleCurrency = AppTool.Round(value, 3);
            this.ComputeSalePrice();
        }
    }

    get CostUnitPrice1InSaleCurrency() { return this.EntityPM.CostUnitPrice1InSaleCurrency; }
    set CostUnitPrice1InSaleCurrency(value: number) {
        if (this.EntityPM.CostUnitPrice1InSaleCurrency != value) {
            this.EntityPM.CostUnitPrice1InSaleCurrency = AppTool.Round(value, 3);
            this.ComputeSalePrice1();
        }
    }

    get CostUnitPrice2InSaleCurrency() { return this.EntityPM.CostUnitPrice2InSaleCurrency; }
    set CostUnitPrice2InSaleCurrency(value: number) {
        if (this.EntityPM.CostUnitPrice2InSaleCurrency != value) {
            this.EntityPM.CostUnitPrice2InSaleCurrency = AppTool.Round(value, 3);
            this.ComputeSalePrice2();
        }
    }

    get CostUnitPrice3InSaleCurrency() { return this.EntityPM.CostUnitPrice3InSaleCurrency; }
    set CostUnitPrice3InSaleCurrency(value: number) {
        if (this.EntityPM.CostUnitPrice3InSaleCurrency != value) {
            this.EntityPM.CostUnitPrice3InSaleCurrency = AppTool.Round(value, 3);
            this.ComputeSalePrice3();
        }
    }

    get CostUnitPrice4InSaleCurrency() { return this.EntityPM.CostUnitPrice4InSaleCurrency; }
    set CostUnitPrice4InSaleCurrency(value: number) {
        if (this.EntityPM.CostUnitPrice4InSaleCurrency != value) {
            this.EntityPM.CostUnitPrice4InSaleCurrency = AppTool.Round(value, 3);
            this.ComputeSalePrice4();
        }
    }

    get CostUnitPrice5InSaleCurrency() { return this.EntityPM.CostUnitPrice5InSaleCurrency; }
    set CostUnitPrice5InSaleCurrency(value: number) {
        if (this.EntityPM.CostUnitPrice5InSaleCurrency != value) {
            this.EntityPM.CostUnitPrice5InSaleCurrency = AppTool.Round(value, 3);
            this.ComputeSalePrice5();
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

            if (this.CostCurrencyId == this.SaleCurrencyId) {
                this.SaleExchangeRate = value;
            }

            this.ComputeCostAmounts();
            this.ComputeCostInSalePrice();
            this.ComputeCostInSalePrice1();
            this.ComputeCostInSalePrice2();
            this.ComputeCostInSalePrice3();
            this.ComputeCostInSalePrice4();
            this.ComputeCostInSalePrice5();
        }
    }

    get CostMinAmount() { return this.EntityPM.CostMinAmount; }
    set CostMinAmount(newValue: number) {
        if (this.EntityPM.CostMinAmount != newValue) {
            this.EntityPM.CostMinAmount = newValue;
            this.ComputeCostAmounts();
        }
    }

    get CostMaxAmount() { return this.EntityPM.CostMaxAmount; }
    set CostMaxAmount(newValue: number) {
        if (this.EntityPM.CostMaxAmount != newValue) {
            this.EntityPM.CostMaxAmount = newValue;
            this.ComputeCostAmounts();
        }
    }

    get CostIsFixedRate() { return this.EntityPM.CostIsFixedRate; }
    set CostIsFixedRate(value: boolean) {
        if (this.EntityPM.CostIsFixedRate != value) {
            this.EntityPM.CostIsFixedRate = value;
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
                    this.fatherComponent.Behaviours.AllRates = comp.RatesList;
                    this.CostExchangeRate = AppTool.Round(comp.Rate, 5);
                    this.RelativeRateDate = DateTool.GetRelativeRateDate(DateTool.GetCurrentDateAsUtc(), comp.RateDate, "ago");
                }
            });
        });
    }

    SetCostQuantity() {
        var myResult = null;

        if (this.IsAdhoc) {
            switch (this.CostMeasurementCode) {
                case "GRWT": { myResult = this.QuotePM.GrossWeight; break; }
                case "CHWT": { myResult = this.QuotePM.ChargeableWeight; break; }
                case "VOLU": { myResult = this.QuotePM.Volume; break; }
                case "BTEU": { myResult = this.QuotePM.TEU; break; }
                case "FIXD": { myResult = 1; break; }
                case "PRVL": { myResult = this.QuotePM.ValueOfGoods; break; }
                case "PRFR": { myResult = ArrayTool.Sum(this.QuotePM.QuoteCharges.filter(d => d.ChargesGroupCode == "FRT"), "CostTotalAmount"); break; }
                case "GWTN": { myResult = this.QuotePM.GrossWeightPerTon; break; }
                case "QTY": { myResult = this.QuotePM.NumberOfContainers; break; }
                case "CWKG": { myResult = this.QuotePM.ChargeableWeightInKG; break; }
                case "GWKG": { myResult = this.QuotePM.GrossWeightInKG; break; }
                case "VCBM": { myResult = this.QuotePM.VolumeInCBM; break; }
                case "PDCW": { myResult = this.QuotePM.PickupDeliveryChargeableWeight; break; }
                default:
                    {
                        if (!AppTool.IsNullOrEmpty(this.CostMeasurementId)) {
                            var list: PackageTypeList = this.fatherComponent.Behaviours.AllPackageTypes.filter(d => d.MeasurementId == this.CostMeasurementId)[0];
                            if (list != null) {
                                myResult = 0;

                                if (list.Id == this.QuotePM.PackageType1Id) { myResult = this.QuotePM.PackageType1Quantity == null ? myResult : myResult + this.QuotePM.PackageType1Quantity; }
                                if (list.Id == this.QuotePM.PackageType2Id) { myResult = this.QuotePM.PackageType2Quantity == null ? myResult : myResult + this.QuotePM.PackageType2Quantity; }
                                if (list.Id == this.QuotePM.PackageType3Id) { myResult = this.QuotePM.PackageType3Quantity == null ? myResult : myResult + this.QuotePM.PackageType3Quantity; }
                                if (list.Id == this.QuotePM.PackageType4Id) { myResult = this.QuotePM.PackageType4Quantity == null ? myResult : myResult + this.QuotePM.PackageType4Quantity; }
                                if (list.Id == this.QuotePM.PackageType5Id) { myResult = this.QuotePM.PackageType5Quantity == null ? myResult : myResult + this.QuotePM.PackageType5Quantity; }

                                myResult = myResult == 0 ? null : myResult;
                            }
                        }

                        break;
                    }
            }
        }

        this.CostQuantity = myResult;
        this.SetUIProperties();
    }
    ComputeCostAmounts() {
        var myTotalAmount = null;

        if (this.CostMeasurementCode == "BCNT") {
            if (!AppTool.IsNullOrEmpty(this.CostContainerType1UnitPrice) && !AppTool.IsNullOrEmpty(this.QuotePM.PackageType1Quantity)) {
                var R1 = this.CostContainerType1UnitPrice * this.QuotePM.PackageType1Quantity;
                myTotalAmount = myTotalAmount == null ? R1 : myTotalAmount + R1;
            }

            if (!AppTool.IsNullOrEmpty(this.CostContainerType2UnitPrice) && !AppTool.IsNullOrEmpty(this.QuotePM.PackageType2Quantity)) {
                var R2 = this.CostContainerType2UnitPrice * this.QuotePM.PackageType2Quantity;
                myTotalAmount = myTotalAmount == null ? R2 : myTotalAmount + R2;
            }

            if (!AppTool.IsNullOrEmpty(this.CostContainerType3UnitPrice) && !AppTool.IsNullOrEmpty(this.QuotePM.PackageType3Quantity)) {
                var R3 = this.CostContainerType3UnitPrice * this.QuotePM.PackageType3Quantity;
                myTotalAmount = myTotalAmount == null ? R3 : myTotalAmount + R3;
            }

            if (!AppTool.IsNullOrEmpty(this.CostContainerType4UnitPrice) && !AppTool.IsNullOrEmpty(this.QuotePM.PackageType4Quantity)) {
                var R4 = this.CostContainerType4UnitPrice * this.QuotePM.PackageType4Quantity;
                myTotalAmount = myTotalAmount == null ? R4 : myTotalAmount + R4;
            }

            if (!AppTool.IsNullOrEmpty(this.CostContainerType5UnitPrice) && !AppTool.IsNullOrEmpty(this.QuotePM.PackageType5Quantity)) {
                var R5 = this.CostContainerType5UnitPrice * this.QuotePM.PackageType5Quantity;
                myTotalAmount = myTotalAmount == null ? R5 : myTotalAmount + R5;
            }
        }

        else {
            if (!AppTool.IsNullOrEmpty(this.CostQuantity) && !AppTool.IsNullOrEmpty(this.CostUnitPrice)) {
                if (this.CostMeasurementCode == "PRVL" || this.CostMeasurementCode == "PRFR") {
                    myTotalAmount = this.CostQuantity * this.CostUnitPrice / 100;
                }

                else {
                    myTotalAmount = this.CostQuantity * this.CostUnitPrice;
                }
            }
        }

        /* MinMax */
        if (myTotalAmount != null) {
            if (this.CostMinAmount != null) {
                if (myTotalAmount < this.CostMinAmount) {
                    myTotalAmount = this.CostMinAmount;
                }
            }

            if (this.CostMaxAmount != null) {
                if (myTotalAmount > this.CostMaxAmount) {
                    myTotalAmount = this.CostMaxAmount;
                }
            }
        }

        // Amounts
        if (AppTool.IsNullOrEmpty(myTotalAmount)) {
            this.CostTotalAmount = null;
            this.CostTotalAmountLocal = null;
            this.CostAmountInSaleCurrency = null;
        }

        else {
            this.CostTotalAmount = myTotalAmount;

            if (AppTool.IsNullOrEmpty(this.CostExchangeRate)) {
                this.CostTotalAmountLocal = null;
            }

            else {
                this.CostTotalAmountLocal = myTotalAmount * this.CostExchangeRate;
            }

            this.ComputeCostInSaleAmount();
        }

        this.SetUIProperties_CostMinMax();                
        this.fatherComponent.ComputeTotals();
        this.fatherComponent.CheckUpdateQuantities();
    }
    ComputeCostInSaleAmount() {
        var myResult = null;

        if (this.CostCurrencyId == this.SaleCurrencyId) {
            if (!AppTool.IsNullOrEmpty(this.CostTotalAmount)) {
                myResult = this.CostTotalAmount;
            }
        }

        else {
            if (!AppTool.IsNullOrEmpty(this.CostTotalAmountLocal) && !AppTool.IsNullOrEmpty(this.SaleExchangeRate)) {
                myResult = this.CostTotalAmountLocal / this.SaleExchangeRate;
            }
        }

        this.CostAmountInSaleCurrency = myResult;
    }
    ComputeCostInSalePrice() {
        this.CostUnitPriceInSaleCurrency = this.fatherComponent.Behaviours.ComputeCostPriceInSaleCurrency(this.EntityPM);
    }
    ComputeCostInSalePrice1() {
        this.CostUnitPrice1InSaleCurrency = this.fatherComponent.Behaviours.ComputeCostPriceInSaleCurrency(this.EntityPM,1);
    }
    ComputeCostInSalePrice2() {
        this.CostUnitPrice2InSaleCurrency = this.fatherComponent.Behaviours.ComputeCostPriceInSaleCurrency(this.EntityPM, 2);

    }
    ComputeCostInSalePrice3() {
        this.CostUnitPrice3InSaleCurrency = this.fatherComponent.Behaviours.ComputeCostPriceInSaleCurrency(this.EntityPM, 3);

    }
    ComputeCostInSalePrice4() {
        this.CostUnitPrice4InSaleCurrency = this.fatherComponent.Behaviours.ComputeCostPriceInSaleCurrency(this.EntityPM, 4);

    }
    ComputeCostInSalePrice5() {
        this.CostUnitPrice5InSaleCurrency = this.fatherComponent.Behaviours.ComputeCostPriceInSaleCurrency(this.EntityPM, 5);
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
                var list: MeasurementList = this.fatherComponent.Behaviours.AllMeasurements.filter(d => d.Id == value)[0];
                if (list != null) {
                    this.SaleMeasurementCode = list.Code;
                    this.SaleMeasurementShortName = list.ShortName;
                }
            }
        }
    }

    get SaleMeasurementCode() { return this.EntityPM.SaleMeasurementCode; }
    set SaleMeasurementCode(value: string) {
        if (this.EntityPM.SaleMeasurementCode != value) {
            this.EntityPM.SaleMeasurementCode = value;

            if (value == "BCNT") {
                this.EntityPM.SaleMinAmount = null;
                this.EntityPM.SaleMaxAmount = null;
            }

            this.SetUIProperties_SaleMinMax();
            this.fatherComponent.SetGridColumns();

            this.SetSaleQuantity();
            this.SetUIProperties_SaleFields();
            this.fatherComponent.CheckUpdateQuantities();

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
            this.SaleCurrencyCode = this.fatherComponent.Behaviours.GetCurrencyCode(value);
            this.SaleExchangeRate = this.fatherComponent.Behaviours.GetCurrencyRate(value);
            this.SetUIProperties_AllIn_CostCurrency();
            this.SetUIProperties_AllIn_SaleCurrency();
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
            this.EntityPM.SaleQuantity = AppTool.Round(value, 2);
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
            this.SetUIProperties_CellsColors();
        }
    }

    get SaleContainerType1UnitPrice() { return this.EntityPM.SaleContainerType1UnitPrice; }
    set SaleContainerType1UnitPrice(value: number) {
        if (this.EntityPM.SaleContainerType1UnitPrice != value) {
            this.EntityPM.SaleContainerType1UnitPrice = AppTool.Round(value, 3);
            this.ComputeSaleAmounts();
            this.SetUIProperties_CellsColors();
        }
    }

    get SaleContainerType2UnitPrice() { return this.EntityPM.SaleContainerType2UnitPrice; }
    set SaleContainerType2UnitPrice(value: number) {
        if (this.EntityPM.SaleContainerType2UnitPrice != value) {
            this.EntityPM.SaleContainerType2UnitPrice = AppTool.Round(value, 3);
            this.ComputeSaleAmounts();
            this.SetUIProperties_CellsColors();
        }
    }

    get SaleContainerType3UnitPrice() { return this.EntityPM.SaleContainerType3UnitPrice; }
    set SaleContainerType3UnitPrice(value: number) {
        if (this.EntityPM.SaleContainerType3UnitPrice != value) {
            this.EntityPM.SaleContainerType3UnitPrice = AppTool.Round(value, 3);
            this.ComputeSaleAmounts();
            this.SetUIProperties_CellsColors();
        }
    }

    get SaleContainerType4UnitPrice() { return this.EntityPM.SaleContainerType4UnitPrice; }
    set SaleContainerType4UnitPrice(value: number) {
        if (this.EntityPM.SaleContainerType4UnitPrice != value) {
            this.EntityPM.SaleContainerType4UnitPrice = AppTool.Round(value, 3);
            this.ComputeSaleAmounts();
            this.SetUIProperties_CellsColors();
        }
    }

    get SaleContainerType5UnitPrice() { return this.EntityPM.SaleContainerType5UnitPrice; }
    set SaleContainerType5UnitPrice(value: number) {
        if (this.EntityPM.SaleContainerType5UnitPrice != value) {
            this.EntityPM.SaleContainerType5UnitPrice = AppTool.Round(value, 3);
            this.ComputeSaleAmounts();
            this.SetUIProperties_CellsColors();
        }
    }

    get SaleTotalAmount() { return this.EntityPM.SaleTotalAmount; }
    set SaleTotalAmount(value: number) {
        if (this.EntityPM.SaleTotalAmount != value) {
            this.EntityPM.SaleTotalAmount = AppTool.Round(value, 2);

            if (this.ChargesGroupCode == "FRT") {
                this.fatherComponent.OnFreightAmountChanged();
            }

            this.SetUIProperties_SaleMinMax();
        }
    }

    get SaleTotalAmountLocal() { return this.EntityPM.SaleTotalAmountLocal; }
    set SaleTotalAmountLocal(value: number) {
        if (this.EntityPM.SaleTotalAmountLocal != value) {
            this.EntityPM.SaleTotalAmountLocal = AppTool.Round(value, 2);
            this.SetUIProperties_SaleMinMax();
        }
    }

    get SaleExchangeRate() { return this.EntityPM.SaleExchangeRate; }
    set SaleExchangeRate(value: number) {
        if (this.EntityPM.SaleExchangeRate != value) {
            this.EntityPM.SaleExchangeRate = AppTool.Round(value, 5);
            this.ComputeSaleAmounts();

            this.ComputeCostInSaleAmount();
            this.ComputeCostInSalePrice();
            this.ComputeCostInSalePrice1();
            this.ComputeCostInSalePrice2();
            this.ComputeCostInSalePrice3();
            this.ComputeCostInSalePrice4();
            this.ComputeCostInSalePrice5();
        }
    }

    get SaleIsFixedRate() { return this.EntityPM.SaleIsFixedRate; }
    set SaleIsFixedRate(value: boolean) {
        if (this.EntityPM.SaleIsFixedRate != value) {
            this.EntityPM.SaleIsFixedRate = value;
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

    SetSaleQuantity() {
        var myResult = null;

        if (this.IsAdhoc) {
            switch (this.SaleMeasurementCode) {
                case "GRWT": { myResult = this.QuotePM.GrossWeight; break; }
                case "CHWT": { myResult = this.QuotePM.ChargeableWeight; break; }
                case "VOLU": { myResult = this.QuotePM.Volume; break; }
                case "BTEU": { myResult = this.QuotePM.TEU; break; }
                case "FIXD": { myResult = 1; break; }
                case "PRVL": { myResult = this.QuotePM.ValueOfGoods; break; }
                case "PRFR": { myResult = ArrayTool.Sum(this.QuotePM.QuoteCharges.filter(d => d.ChargesGroupCode == "FRT"), "SaleTotalAmount"); break; }
                case "GWTN": { myResult = this.QuotePM.GrossWeightPerTon; break; }
                case "QTY": { myResult = this.QuotePM.NumberOfContainers; break; }
                case "CWKG": { myResult = this.QuotePM.ChargeableWeightInKG; break; }
                case "GWKG": { myResult = this.QuotePM.GrossWeightInKG; break; }
                case "VCBM": { myResult = this.QuotePM.VolumeInCBM; break; }
                case "PDCW": { myResult = this.QuotePM.PickupDeliveryChargeableWeight; break; }
                default:
                    {
                        if (!AppTool.IsNullOrEmpty(this.SaleMeasurementId)) {
                            var list: PackageTypeList = this.fatherComponent.Behaviours.AllPackageTypes.filter(d => d.MeasurementId == this.SaleMeasurementId)[0];
                            if (list != null) {
                                myResult = 0;

                                if (list.Id == this.QuotePM.PackageType1Id) { myResult = this.QuotePM.PackageType1Quantity == null ? myResult : myResult + this.QuotePM.PackageType1Quantity; }
                                if (list.Id == this.QuotePM.PackageType2Id) { myResult = this.QuotePM.PackageType2Quantity == null ? myResult : myResult + this.QuotePM.PackageType2Quantity; }
                                if (list.Id == this.QuotePM.PackageType3Id) { myResult = this.QuotePM.PackageType3Quantity == null ? myResult : myResult + this.QuotePM.PackageType3Quantity; }
                                if (list.Id == this.QuotePM.PackageType4Id) { myResult = this.QuotePM.PackageType4Quantity == null ? myResult : myResult + this.QuotePM.PackageType4Quantity; }
                                if (list.Id == this.QuotePM.PackageType5Id) { myResult = this.QuotePM.PackageType5Quantity == null ? myResult : myResult + this.QuotePM.PackageType5Quantity; }

                                myResult = myResult == 0 ? null : myResult;
                            }
                        }

                        break;
                    }
            }
        }

        this.SaleQuantity = myResult;
        this.SetUIProperties();
    }
    ComputeSaleAmounts() {
        var myTotalAmount = null;

        if (this.SaleMeasurementCode == "BCNT") {
            if (!AppTool.IsNullOrEmpty(this.SaleContainerType1UnitPrice) && !AppTool.IsNullOrEmpty(this.QuotePM.PackageType1Quantity)) {
                var R1 = this.SaleContainerType1UnitPrice * this.QuotePM.PackageType1Quantity;
                myTotalAmount = myTotalAmount == null ? R1 : myTotalAmount + R1;
            }

            if (!AppTool.IsNullOrEmpty(this.SaleContainerType2UnitPrice) && !AppTool.IsNullOrEmpty(this.QuotePM.PackageType2Quantity)) {
                var R2 = this.SaleContainerType2UnitPrice * this.QuotePM.PackageType2Quantity;
                myTotalAmount = myTotalAmount == null ? R2 : myTotalAmount + R2;
            }

            if (!AppTool.IsNullOrEmpty(this.SaleContainerType3UnitPrice) && !AppTool.IsNullOrEmpty(this.QuotePM.PackageType3Quantity)) {
                var R3 = this.SaleContainerType3UnitPrice * this.QuotePM.PackageType3Quantity;
                myTotalAmount = myTotalAmount == null ? R3 : myTotalAmount + R3;
            }

            if (!AppTool.IsNullOrEmpty(this.SaleContainerType4UnitPrice) && !AppTool.IsNullOrEmpty(this.QuotePM.PackageType4Quantity)) {
                var R4 = this.SaleContainerType4UnitPrice * this.QuotePM.PackageType4Quantity;
                myTotalAmount = myTotalAmount == null ? R4 : myTotalAmount + R4;
            }

            if (!AppTool.IsNullOrEmpty(this.SaleContainerType5UnitPrice) && !AppTool.IsNullOrEmpty(this.QuotePM.PackageType5Quantity)) {
                var R5 = this.SaleContainerType5UnitPrice * this.QuotePM.PackageType5Quantity;
                myTotalAmount = myTotalAmount == null ? R5 : myTotalAmount + R5;
            }
        }

        else {
            if (!AppTool.IsNullOrEmpty(this.SaleQuantity) && !AppTool.IsNullOrEmpty(this.SaleUnitPrice)) {
                if (this.SaleMeasurementCode == "PRVL" || this.SaleMeasurementCode == "PRFR") {
                    myTotalAmount = this.SaleQuantity * this.SaleUnitPrice / 100;
                }

                else {
                    myTotalAmount = this.SaleQuantity * this.SaleUnitPrice;
                }
            }
        }

        /* MinMax */
        if (myTotalAmount != null) {
            if (this.SaleMinAmount != null) {
                if (myTotalAmount < this.SaleMinAmount) {
                    myTotalAmount = this.SaleMinAmount;
                }
            }

            if (this.SaleMaxAmount != null) {
                if (myTotalAmount > this.SaleMaxAmount) {
                    myTotalAmount = this.SaleMaxAmount;
                }
            }
        }

        this.EntityPM.SaleTotalAmount = AppTool.Round(myTotalAmount, 2);
        this.EntityPM.SaleTotalAmountLocal = AppTool.IsNullOrEmpty(myTotalAmount) ? null : AppTool.Round(myTotalAmount * this.SaleExchangeRate, 2);
        this.EntityPM.SaleAmountInSaleCurrency = AppTool.IsNullOrEmpty(this.EntityPM.SaleTotalAmountLocal) ? null : AppTool.Round(this.EntityPM.SaleTotalAmountLocal / this.QuotePM.ExchangeRate, 2);

        this.EntityPM.SaleUnitPriceInSaleCurrency = this.GetSalePriceInSaleCurrency(this.EntityPM.SaleUnitPrice);
        this.EntityPM.SaleUnitPrice1InSaleCurrency = this.GetSalePriceInSaleCurrency(this.EntityPM.SaleContainerType1UnitPrice);
        this.EntityPM.SaleUnitPrice2InSaleCurrency = this.GetSalePriceInSaleCurrency(this.EntityPM.SaleContainerType2UnitPrice);
        this.EntityPM.SaleUnitPrice3InSaleCurrency = this.GetSalePriceInSaleCurrency(this.EntityPM.SaleContainerType3UnitPrice);
        this.EntityPM.SaleUnitPrice4InSaleCurrency = this.GetSalePriceInSaleCurrency(this.EntityPM.SaleContainerType4UnitPrice);
        this.EntityPM.SaleUnitPrice5InSaleCurrency = this.GetSalePriceInSaleCurrency(this.EntityPM.SaleContainerType5UnitPrice);

        this.SetUIProperties_CellsColors();

        if (this.ChargesGroupCode == "FRT") {
            this.fatherComponent.OnFreightAmountChanged();
        }

        this.SetUIProperties_SaleMinMax();
        this.fatherComponent.ComputeTotals();
        this.fatherComponent.CheckUpdateQuantities();

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
    ComputeSalePrice1() {
        if (AppTool.IsNullOrEmpty(this.CostUnitPrice1InSaleCurrency)) {
            this.SaleContainerType1UnitPrice = null;
        }

        else {
            var myResult = this.SaleContainerType1UnitPrice;
            var markup = this.ContainerType1MarkUpValue == null ? 0 : this.ContainerType1MarkUpValue;

            if (this.ContainerType1MarkUpTypeCode == "P") {
                myResult = this.CostUnitPrice1InSaleCurrency + (this.CostUnitPrice1InSaleCurrency * (markup / 100));
            }

            else {
                myResult = this.CostUnitPrice1InSaleCurrency + markup;
            }

            if (AppTool.IsNullOrZero(myResult)) {
                myResult = null;
            }

            this.SaleContainerType1UnitPrice = myResult;
        }
    }
    ComputeSalePrice2() {
        if (AppTool.IsNullOrEmpty(this.CostUnitPrice2InSaleCurrency)) {
            this.SaleContainerType2UnitPrice = null;
        }

        else {
            var myResult = this.SaleContainerType2UnitPrice;
            var markup = this.ContainerType2MarkUpValue == null ? 0 : this.ContainerType2MarkUpValue;

            if (this.ContainerType2MarkUpTypeCode == "P") {
                myResult = this.CostUnitPrice2InSaleCurrency + (this.CostUnitPrice2InSaleCurrency * (markup / 100));
            }

            else {
                myResult = this.CostUnitPrice2InSaleCurrency + markup;
            }

            if (AppTool.IsNullOrZero(myResult)) {
                myResult = null;
            }

            this.SaleContainerType2UnitPrice = myResult;
        }
    }
    ComputeSalePrice3() {
        if (AppTool.IsNullOrEmpty(this.CostUnitPrice3InSaleCurrency)) {
            this.SaleContainerType3UnitPrice = null;
        }

        else {
            var myResult = this.SaleContainerType3UnitPrice;
            var markup = this.ContainerType3MarkUpValue == null ? 0 : this.ContainerType3MarkUpValue;

            if (this.ContainerType3MarkUpTypeCode == "P") {
                myResult = this.CostUnitPrice3InSaleCurrency + (this.CostUnitPrice3InSaleCurrency * (markup / 100));
            }

            else {
                myResult = this.CostUnitPrice3InSaleCurrency + markup;
            }

            if (AppTool.IsNullOrZero(myResult)) {
                myResult = null;
            }

            this.SaleContainerType3UnitPrice = myResult;
        }
    }
    ComputeSalePrice4() {
        if (AppTool.IsNullOrEmpty(this.CostUnitPrice4InSaleCurrency)) {
            this.SaleContainerType4UnitPrice = null;
        }

        else {
            var myResult = this.SaleContainerType4UnitPrice;
            var markup = this.ContainerType4MarkUpValue == null ? 0 : this.ContainerType4MarkUpValue;

            if (this.ContainerType4MarkUpTypeCode == "P") {
                myResult = this.CostUnitPrice4InSaleCurrency + (this.CostUnitPrice4InSaleCurrency * (markup / 100));
            }

            else {
                myResult = this.CostUnitPrice4InSaleCurrency + markup;
            }

            if (AppTool.IsNullOrZero(myResult)) {
                myResult = null;
            }

            this.SaleContainerType4UnitPrice = myResult;
        }
    }
    ComputeSalePrice5() {
        if (AppTool.IsNullOrEmpty(this.CostUnitPrice5InSaleCurrency)) {
            this.SaleContainerType5UnitPrice = null;
        }

        else {
            var myResult = this.SaleContainerType5UnitPrice;
            var markup = this.ContainerType5MarkUpValue == null ? 0 : this.ContainerType5MarkUpValue;

            if (this.ContainerType5MarkUpTypeCode == "P") {
                myResult = this.CostUnitPrice5InSaleCurrency + (this.CostUnitPrice5InSaleCurrency * (markup / 100));
            }

            else {
                myResult = this.CostUnitPrice5InSaleCurrency + markup;
            }

            if (AppTool.IsNullOrZero(myResult)) {
                myResult = null;
            }

            this.SaleContainerType5UnitPrice = myResult;
        }
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

    private mySaleUnitPrice1String: string = null;
    get SaleUnitPrice1String() {
        var myResult = null;

        if (!AppTool.IsNullOrEmpty(this.SaleContainerType1UnitPrice)) {
            myResult = this.SaleContainerType1UnitPrice + "";
        }

        this.mySaleUnitPrice1String = myResult;
        return this.mySaleUnitPrice1String;
    }
    set SaleUnitPrice1String(value: string) {
        if (this.mySaleUnitPrice1String != value) {
            var mySalePrice = null;
            var myMarkUpValue = 0;
            var myMarkUpCode = "F";
            var myCostPrice = this.CostUnitPrice1InSaleCurrency;

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

            this.SaleContainerType1UnitPrice = mySalePrice;
            this.ContainerType1MarkUpTypeCode = myMarkUpCode;
            this.ContainerType1MarkUpValue = AppTool.Round(myMarkUpValue, 3);
        }
    }

    private mySaleUnitPrice2String: string = null;
    get SaleUnitPrice2String() {
        var myResult = null;

        if (!AppTool.IsNullOrEmpty(this.SaleContainerType2UnitPrice)) {
            myResult = this.SaleContainerType2UnitPrice + "";
        }

        this.mySaleUnitPrice2String = myResult;
        return this.mySaleUnitPrice2String;
    }
    set SaleUnitPrice2String(value: string) {
        if (this.mySaleUnitPrice2String != value) {
            var mySalePrice = null;
            var myMarkUpValue = 0;
            var myMarkUpCode = "F";
            var myCostPrice = this.CostUnitPrice2InSaleCurrency;

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

            this.SaleContainerType2UnitPrice = mySalePrice;
            this.ContainerType2MarkUpTypeCode = myMarkUpCode;
            this.ContainerType2MarkUpValue = AppTool.Round(myMarkUpValue, 3);
        }
    }

    private mySaleUnitPrice3String: string = null;
    get SaleUnitPrice3String() {
        var myResult = null;

        if (!AppTool.IsNullOrEmpty(this.SaleContainerType3UnitPrice)) {
            myResult = this.SaleContainerType3UnitPrice + "";
        }

        this.mySaleUnitPrice3String = myResult;
        return this.mySaleUnitPrice3String;
    }
    set SaleUnitPrice3String(value: string) {
        if (this.mySaleUnitPrice3String != value) {
            var mySalePrice = null;
            var myMarkUpValue = 0;
            var myMarkUpCode = "F";
            var myCostPrice = this.CostUnitPrice3InSaleCurrency;

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

            this.SaleContainerType3UnitPrice = mySalePrice;
            this.ContainerType3MarkUpTypeCode = myMarkUpCode;
            this.ContainerType3MarkUpValue = AppTool.Round(myMarkUpValue, 3);
        }
    }

    private mySaleUnitPrice4String: string = null;
    get SaleUnitPrice4String() {
        var myResult = null;

        if (!AppTool.IsNullOrEmpty(this.SaleContainerType4UnitPrice)) {
            myResult = this.SaleContainerType4UnitPrice + "";
        }

        this.mySaleUnitPrice4String = myResult;
        return this.mySaleUnitPrice4String;
    }
    set SaleUnitPrice4String(value: string) {
        if (this.mySaleUnitPrice4String != value) {
            var mySalePrice = null;
            var myMarkUpValue = 0;
            var myMarkUpCode = "F";
            var myCostPrice = this.CostUnitPrice4InSaleCurrency;

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

            this.SaleContainerType4UnitPrice = mySalePrice;
            this.ContainerType4MarkUpTypeCode = myMarkUpCode;
            this.ContainerType4MarkUpValue = AppTool.Round(myMarkUpValue, 3);
        }
    }

    private mySaleUnitPrice5String: string = null;
    get SaleUnitPrice5String() {
        var myResult = null;

        if (!AppTool.IsNullOrEmpty(this.SaleContainerType5UnitPrice)) {
            myResult = this.SaleContainerType5UnitPrice + "";
        }

        this.mySaleUnitPrice5String = myResult;
        return this.mySaleUnitPrice5String;
    }
    set SaleUnitPrice5String(value: string) {
        if (this.mySaleUnitPrice5String != value) {
            var mySalePrice = null;
            var myMarkUpValue = 0;
            var myMarkUpCode = "F";
            var myCostPrice = this.CostUnitPrice5InSaleCurrency;

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

            this.SaleContainerType5UnitPrice = mySalePrice;
            this.ContainerType5MarkUpTypeCode = myMarkUpCode;
            this.ContainerType5MarkUpValue = AppTool.Round(myMarkUpValue, 3);
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

    get ContainerType1MarkUpValue() { return this.EntityPM.ContainerType1MarkUpValue; }
    set ContainerType1MarkUpValue(value: number) {
        if (this.EntityPM.ContainerType1MarkUpValue != value) {
            this.EntityPM.ContainerType1MarkUpValue = AppTool.Round(value, 3);
            this.ComputeMarkUp1String();
            this.SetUIProperties_CellsColors();
        }
    }

    get ContainerType2MarkUpValue() { return this.EntityPM.ContainerType2MarkUpValue; }
    set ContainerType2MarkUpValue(value: number) {
        if (this.EntityPM.ContainerType2MarkUpValue != value) {
            this.EntityPM.ContainerType2MarkUpValue = AppTool.Round(value, 3);
            this.ComputeMarkUp2String();
            this.SetUIProperties_CellsColors();
        }
    }

    get ContainerType3MarkUpValue() { return this.EntityPM.ContainerType3MarkUpValue; }
    set ContainerType3MarkUpValue(value: number) {
        if (this.EntityPM.ContainerType3MarkUpValue != value) {
            this.EntityPM.ContainerType3MarkUpValue = AppTool.Round(value, 3);
            this.ComputeMarkUp3String();
            this.SetUIProperties_CellsColors();
        }
    }

    get ContainerType4MarkUpValue() { return this.EntityPM.ContainerType4MarkUpValue; }
    set ContainerType4MarkUpValue(value: number) {
        if (this.EntityPM.ContainerType4MarkUpValue != value) {
            this.EntityPM.ContainerType4MarkUpValue = AppTool.Round(value, 3);
            this.ComputeMarkUp4String();
            this.SetUIProperties_CellsColors();
        }
    }

    get ContainerType5MarkUpValue() { return this.EntityPM.ContainerType5MarkUpValue; }
    set ContainerType5MarkUpValue(value: number) {
        if (this.EntityPM.ContainerType5MarkUpValue != value) {
            this.EntityPM.ContainerType5MarkUpValue = AppTool.Round(value, 3);
            this.ComputeMarkUp5String();
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

    get ContainerType1MarkUpTypeCode() { return this.EntityPM.ContainerType1MarkUpTypeCode; }
    set ContainerType1MarkUpTypeCode(value: string) {
        if (this.EntityPM.ContainerType1MarkUpTypeCode != value) {
            this.EntityPM.ContainerType1MarkUpTypeCode = value;
            this.ComputeMarkUp1String();
        }
    }

    get ContainerType2MarkUpTypeCode() { return this.EntityPM.ContainerType2MarkUpTypeCode; }
    set ContainerType2MarkUpTypeCode(value: string) {
        if (this.EntityPM.ContainerType2MarkUpTypeCode != value) {
            this.EntityPM.ContainerType2MarkUpTypeCode = value;
            this.ComputeMarkUp2String();
        }
    }

    get ContainerType3MarkUpTypeCode() { return this.EntityPM.ContainerType3MarkUpTypeCode; }
    set ContainerType3MarkUpTypeCode(value: string) {
        if (this.EntityPM.ContainerType3MarkUpTypeCode != value) {
            this.EntityPM.ContainerType3MarkUpTypeCode = value;
            this.ComputeMarkUp3String();
        }
    }

    get ContainerType4MarkUpTypeCode() { return this.EntityPM.ContainerType4MarkUpTypeCode; }
    set ContainerType4MarkUpTypeCode(value: string) {
        if (this.EntityPM.ContainerType4MarkUpTypeCode != value) {
            this.EntityPM.ContainerType4MarkUpTypeCode = value;
            this.ComputeMarkUp4String();
        }
    }

    get ContainerType5MarkUpTypeCode() { return this.EntityPM.ContainerType5MarkUpTypeCode; }
    set ContainerType5MarkUpTypeCode(value: string) {
        if (this.EntityPM.ContainerType5MarkUpTypeCode != value) {
            this.EntityPM.ContainerType5MarkUpTypeCode = value;
            this.ComputeMarkUp5String();
        }
    }

    public CellMarkupText: string = null;
    public CellMarkup1Text: string = null;
    public CellMarkup2Text: string = null;
    public CellMarkup3Text: string = null;
    public CellMarkup4Text: string = null;
    public CellMarkup5Text: string = null;
    ComputeMarkUp() {
        if (this.EntityPM.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(d => d.IsAllIN).length > 0) {
            return;
        }

        this.MarkUpValue = this.fatherComponent.Behaviours.ComputeMarkUp(this.EntityPM);
    }
    ComputeMarkUp1() {
        if (this.EntityPM.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(d => d.IsAllIN).length > 0) {
            return;
        }

        this.ContainerType1MarkUpValue = this.fatherComponent.Behaviours.ComputeMarkUp(this.EntityPM, 1);
    }
    ComputeMarkUp2() {
        if (this.EntityPM.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(d => d.IsAllIN).length > 0) {
            return;
        }

        this.ContainerType2MarkUpValue = this.fatherComponent.Behaviours.ComputeMarkUp(this.EntityPM, 2);
    }
    ComputeMarkUp3() {
        if (this.EntityPM.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(d => d.IsAllIN).length > 0) {
            return;
        }

        this.ContainerType3MarkUpValue = this.fatherComponent.Behaviours.ComputeMarkUp(this.EntityPM, 3);
    }
    ComputeMarkUp4() {
        if (this.EntityPM.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(d => d.IsAllIN).length > 0) {
            return;
        }

        this.ContainerType4MarkUpValue = this.fatherComponent.Behaviours.ComputeMarkUp(this.EntityPM, 4);
    }
    ComputeMarkUp5() {
        if (this.EntityPM.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(d => d.IsAllIN).length > 0) {
            return;
        }

        this.ContainerType5MarkUpValue = this.fatherComponent.Behaviours.ComputeMarkUp(this.EntityPM, 5);
    }

    ComputeMarkUpString() {
        this.CellMarkupText = this.fatherComponent.Behaviours.GetMarkUpString(this.EntityPM);
    }
    ComputeMarkUp1String() {
        this.CellMarkup1Text= this.fatherComponent.Behaviours.GetMarkUpString(this.EntityPM, 1);
    }
    ComputeMarkUp2String() {
        this.CellMarkup2Text = this.fatherComponent.Behaviours.GetMarkUpString(this.EntityPM, 2);
    }
    ComputeMarkUp3String() {
        this.CellMarkup3Text = this.fatherComponent.Behaviours.GetMarkUpString(this.EntityPM, 3);
    }
    ComputeMarkUp4String() {
        this.CellMarkup4Text = this.fatherComponent.Behaviours.GetMarkUpString(this.EntityPM, 4);
    }
    ComputeMarkUp5String() {
        this.CellMarkup5Text = this.fatherComponent.Behaviours.GetMarkUpString(this.EntityPM, 5);
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
        }
    }

    get IsAllIN() { return this.EntityPM.IsAllIN; }
    set IsAllIN(newValue: boolean) {
        if (this.EntityPM.IsAllIN != newValue) {
            this.EntityPM.IsAllIN = newValue;
            this.SetUIProperties_AllIn();
            this.ApplyAllIn();
            this.IsRegionalTax = false;
            this.SetUIProperties_IsRegionalTax();
        }
    }

    ApplyAllIn() {
        this.UpdateCostSaleDataVisibility();
        this.UpdateAllInFreight();
    }

    private UpdateAllInFreight() {
        var allFreightModel: FCLQuoteChargeItem[] = this.fatherComponent.ItemsSource.Collection.filter(d => d.EntityPM.ChargesGroupCode == "FRT");
        allFreightModel.forEach(item => {
            item.SetUIProperties();
        });

        var freightModel = allFreightModel[0];
        if (freightModel != null) {

            if (this.EntityPM.IsAllIN) {
                if (this.EntityPM.CostMeasurementCode == "BCNT") {
                    // SalePrice1
                    if (this.QuotePM.PackageType1Id != null) {
                        var freightPrice = freightModel.SaleContainerType1UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType1UnitPrice;

                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice + myAllInPrice;
                        }

                        else {
                            freightPrice = freightPrice == null ? myAllInPrice : freightPrice;
                        }

                        freightModel.SaleContainerType1UnitPrice = freightPrice;
                    }

                    //SalePrice2
                    if (this.QuotePM.PackageType2Id != null) {
                        var freightPrice = freightModel.SaleContainerType2UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType2UnitPrice;

                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice + myAllInPrice;
                        }

                        else {
                            freightPrice = freightPrice == null ? myAllInPrice : freightPrice;
                        }

                        freightModel.SaleContainerType2UnitPrice = freightPrice;
                    }

                    //SalePrice3
                    if (this.QuotePM.PackageType3Id != null) {
                        var freightPrice = freightModel.SaleContainerType3UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType3UnitPrice;

                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice + myAllInPrice;
                        }

                        else {
                            freightPrice = freightPrice == null ? myAllInPrice : freightPrice;
                        }

                        freightModel.SaleContainerType3UnitPrice = freightPrice;
                    }

                    // SalePrice4
                    if (this.QuotePM.PackageType4Id != null) {
                        var freightPrice = freightModel.SaleContainerType4UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType4UnitPrice;

                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice + myAllInPrice;
                        }

                        else {
                            freightPrice = freightPrice == null ? myAllInPrice : freightPrice;
                        }

                        freightModel.SaleContainerType4UnitPrice = freightPrice;
                    }

                    // SalePrice5
                    if (this.QuotePM.PackageType5Id != null) {
                        var freightPrice = freightModel.SaleContainerType5UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType5UnitPrice;

                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice + myAllInPrice;
                        }

                        else {
                            freightPrice = freightPrice == null ? myAllInPrice : freightPrice;
                        }

                        freightModel.SaleContainerType5UnitPrice = freightPrice;
                    }
                }

                else {

                }
            }

            else {
                if (this.EntityPM.CostMeasurementCode == "BCNT") {
                    // SalePrice1
                    if (this.QuotePM.PackageType1Id != null) {
                        var freightPrice = freightModel.SaleContainerType1UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType1UnitPrice;

                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice - myAllInPrice;
                        }

                        else {
                            freightPrice = myAllInPrice == null ? freightPrice : null;
                        }

                        freightModel.SaleContainerType1UnitPrice = freightPrice;
                    }

                    // SalePrice2
                    if (this.QuotePM.PackageType2Id != null) {
                        var freightPrice = freightModel.SaleContainerType2UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType2UnitPrice;

                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice - myAllInPrice;
                        }

                        else {
                            freightPrice = myAllInPrice == null ? freightPrice : null;
                        }

                        freightModel.SaleContainerType2UnitPrice = freightPrice;
                    }

                    // SalePrice3
                    if (this.QuotePM.PackageType3Id != null) {
                        var freightPrice = freightModel.SaleContainerType3UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType3UnitPrice;

                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice - myAllInPrice;
                        }

                        else {
                            freightPrice = myAllInPrice == null ? freightPrice : null;
                        }

                        freightModel.SaleContainerType3UnitPrice = freightPrice;
                    }

                    // SalePrice4
                    if (this.QuotePM.PackageType4Id != null) {
                        var freightPrice = freightModel.SaleContainerType4UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType4UnitPrice;

                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice - myAllInPrice;
                        }

                        else {
                            freightPrice = myAllInPrice == null ? freightPrice : null;
                        }

                        freightModel.SaleContainerType4UnitPrice = freightPrice;
                    }

                    // SalePrice5
                    if (this.QuotePM.PackageType5Id != null) {
                        var freightPrice = freightModel.SaleContainerType5UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType5UnitPrice;

                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice - myAllInPrice;
                        }

                        else {
                            freightPrice = myAllInPrice == null ? freightPrice : null;
                        }

                        freightModel.SaleContainerType5UnitPrice = freightPrice;
                    }
                }

                else {

                }
            }
        }

        this.fatherComponent.ComputeTotals();
    }
    private UpdateCostSaleDataVisibility() {
        if (this.EntityPM.CostMeasurementCode == "BCNT") {
            this.EntityPM.CostUnitPrice = null;
            this.EntityPM.SaleUnitPrice = null;
        }

        else {
            this.EntityPM.CostContainerType1UnitPrice = null;
            this.EntityPM.CostContainerType2UnitPrice = null;
            this.EntityPM.CostContainerType3UnitPrice = null;
            this.EntityPM.CostContainerType4UnitPrice = null;
            this.EntityPM.CostContainerType5UnitPrice = null;
            this.EntityPM.SaleContainerType1UnitPrice = null;
            this.EntityPM.SaleContainerType2UnitPrice = null;
            this.EntityPM.SaleContainerType3UnitPrice = null;
            this.EntityPM.SaleContainerType4UnitPrice = null;
            this.EntityPM.SaleContainerType5UnitPrice = null;
        }

        this.ComputeCostAmounts();
        this.ComputeSaleAmounts();
        this.SetUIProperties();
        this.fatherComponent.ComputeTotals();
    }

    OnMeasurementsChanged() {
        if (this.CostMeasurementId != this.SaleMeasurementId) {

            this.EntityPM.MarkUpValue = 0;
            this.EntityPM.ContainerType1MarkUpValue = 0;
            this.EntityPM.ContainerType2MarkUpValue = 0;
            this.EntityPM.ContainerType3MarkUpValue = 0;
            this.EntityPM.ContainerType4MarkUpValue = 0;
            this.EntityPM.ContainerType5MarkUpValue = 0;

            this.mySaleUnitPriceString = this.SaleUnitPrice ? this.SaleUnitPrice + "" : null;
            this.mySaleUnitPrice1String = this.SaleContainerType1UnitPrice ? this.SaleContainerType1UnitPrice + "" : null;
            this.mySaleUnitPrice2String = this.SaleContainerType2UnitPrice ? this.SaleContainerType2UnitPrice + "" : null;
            this.mySaleUnitPrice3String = this.SaleContainerType3UnitPrice ? this.SaleContainerType3UnitPrice + "" : null;
            this.mySaleUnitPrice4String = this.SaleContainerType4UnitPrice ? this.SaleContainerType4UnitPrice + "" : null;
            this.mySaleUnitPrice5String = this.SaleContainerType5UnitPrice ? this.SaleContainerType5UnitPrice + "" : null;
        }

        else {
            this.ComputeMarkUp();
            this.ComputeMarkUp1();
            this.ComputeMarkUp2();
            this.ComputeMarkUp3();
            this.ComputeMarkUp4();
            this.ComputeMarkUp5();
        }
    }

    public SalePriceHeader: any = [];
    public SaleAmountHeader: any = [];
    public Sale1Header: any[] = [];
    public Sale2Header: any[] = [];
    public Sale3Header: any[] = [];
    public Sale4Header: any[] = [];
    public Sale5Header: any[] = [];
    SetEditScreenGridHeaders() {

        this.Sale1Header = [3];
        this.Sale2Header = [3];
        this.Sale3Header = [3];
        this.Sale4Header = [3];
        this.Sale5Header = [3];

        var q1 = AppTool.IsNullOrZero(this.QuotePM.PackageType1Quantity) ? "" : this.QuotePM.PackageType1Quantity.toString() + "X";
        var q2 = AppTool.IsNullOrZero(this.QuotePM.PackageType2Quantity) ? "" : this.QuotePM.PackageType2Quantity.toString() + "X";
        var q3 = AppTool.IsNullOrZero(this.QuotePM.PackageType3Quantity) ? "" : this.QuotePM.PackageType3Quantity.toString() + "X";
        var q4 = AppTool.IsNullOrZero(this.QuotePM.PackageType4Quantity) ? "" : this.QuotePM.PackageType4Quantity.toString() + "X";
        var q5 = AppTool.IsNullOrZero(this.QuotePM.PackageType5Quantity) ? "" : this.QuotePM.PackageType5Quantity.toString() + "X";

        var p1: string = "";
        var p2: string = "";
        var p3: string = "";
        var p4: string = "";
        var p5: string = "";
        if (!AppTool.IsNullOrEmpty(this.QuotePM.PackageType1Id)) {
            var item = this.fatherComponent.Behaviours.AllPackageTypes.filter(d => d.Id == this.QuotePM.PackageType1Id)[0];
            if (item) {
                p1 = item.Code;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.QuotePM.PackageType2Id)) {
            var item = this.fatherComponent.Behaviours.AllPackageTypes.filter(d => d.Id == this.QuotePM.PackageType2Id)[0];
            if (item) {
                p2 = item.Code;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.QuotePM.PackageType3Id)) {
            var item = this.fatherComponent.Behaviours.AllPackageTypes.filter(d => d.Id == this.QuotePM.PackageType3Id)[0];
            if (item) {
                p3 = item.Code;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.QuotePM.PackageType4Id)) {
            var item = this.fatherComponent.Behaviours.AllPackageTypes.filter(d => d.Id == this.QuotePM.PackageType4Id)[0];
            if (item) {
                p4 = item.Code;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.QuotePM.PackageType5Id)) {
            var item = this.fatherComponent.Behaviours.AllPackageTypes.filter(d => d.Id == this.QuotePM.PackageType5Id)[0];
            if (item) {
                p5 = item.Code;
            }
        }

        var myCurrencyCode: string = "";

        if (this.fatherComponent.IsMultiCurrency) {
            myCurrencyCode = "";
        }

        else if (this.fatherComponent.IsSaleCurrencySameAsCost) {
            myCurrencyCode = AppTool.IsNullOrEmpty(this.CostCurrencyCode) ? "" : this.CostCurrencyCode;
        }

        else {
            myCurrencyCode = AppTool.IsNullOrEmpty(this.fatherComponent.SaleCurrencyCode) ? "" : this.fatherComponent.SaleCurrencyCode;
        }

        if (AppTool.IsNullOrEmpty(myCurrencyCode)) {
            this.SalePriceHeader = TextCodeTranslator.Translate("Quote.O.Charges.SalePrice", false).replace("%SaleCurrencyCode", myCurrencyCode).replace("(", "").replace(")", "").split('%n');
            this.SaleAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.SaleAmount", false).replace("%SaleCurrencyCode", myCurrencyCode).replace("(", "").replace(")", "").split('%n');
        }

        else {
            this.SalePriceHeader = TextCodeTranslator.Translate("Quote.O.Charges.SalePrice", false).replace("%SaleCurrencyCode", myCurrencyCode).split('%n');
            this.SaleAmountHeader = TextCodeTranslator.Translate("Quote.O.Charges.SaleAmount", false).replace("%SaleCurrencyCode", myCurrencyCode).split('%n');
        }

        this.Sale1Header[0] = this.SalePriceHeader[0];
        this.Sale2Header[0] = this.SalePriceHeader[0];
        this.Sale3Header[0] = this.SalePriceHeader[0];
        this.Sale4Header[0] = this.SalePriceHeader[0];
        this.Sale5Header[0] = this.SalePriceHeader[0];
        this.Sale1Header[1] = this.SalePriceHeader[1];
        this.Sale2Header[1] = this.SalePriceHeader[1];
        this.Sale3Header[1] = this.SalePriceHeader[1];
        this.Sale4Header[1] = this.SalePriceHeader[1];
        this.Sale5Header[1] = this.SalePriceHeader[1];
        this.Sale1Header[2] = q1 + p1;
        this.Sale2Header[2] = q2 + p2;
        this.Sale3Header[2] = q3 + p3;
        this.Sale4Header[2] = q4 + p4;
        this.Sale5Header[2] = q5 + p5;
    }

    get IsRegionalTax() { return this.EntityPM.IsRegionalTax; }
    set IsRegionalTax(newValue: boolean) {
        if (this.EntityPM.IsRegionalTax != newValue) {
            this.EntityPM.IsRegionalTax = newValue;
            this.fatherComponent.ComputeTotals();
        }
    }

    OnQuoteSaleCurrencyModeChanged() {

        this.UpgradeCostData();

        if (this.QuotePM.IsMultiCurrency) {

            var list: ChargesTypeList = this.fatherComponent.Behaviours.AllChargesTypes.filter(f => f.Id == this.ChargesTypeId)[0];

            if (list) {
                var newSaleCurrencyId = list.ReceivablesDefaultCurrencyId;

                if (!newSaleCurrencyId) {
                    if (list.ChargesGroupCode == "FRT" || list.ChargesGroupCode == "SCH") {
                        newSaleCurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
                    }

                    else {
                        newSaleCurrencyId = SessionLocator.TenantPM.OtherChargesCurrencyId;
                    }
                }

                if (newSaleCurrencyId) {
                    if (newSaleCurrencyId != this.SaleCurrencyId) {

                        if (newSaleCurrencyId == this.CostCurrencyId) {
                            this.EntityPM.SaleCurrencyId = this.EntityPM.CostCurrencyId;
                            this.EntityPM.SaleCurrencyCode = this.EntityPM.CostCurrencyCode;
                            this.EntityPM.SaleExchangeRate = this.EntityPM.CostExchangeRate;
                        }

                        else if (newSaleCurrencyId == this.QuotePM.SaleCurrencyId) {
                            this.EntityPM.SaleCurrencyId = this.QuotePM.SaleCurrencyId;
                            this.EntityPM.SaleCurrencyCode = this.QuotePM.SaleCurrencyCode;
                            this.EntityPM.SaleExchangeRate = this.QuotePM.ExchangeRate;
                        }

                        else {
                            this.SaleCurrencyId = newSaleCurrencyId;
                        }
                    }
                }
            }
        }

        else if (this.QuotePM.IsSaleCurrencySameAsCost) {
            this.EntityPM.SaleCurrencyId = this.EntityPM.CostCurrencyId;
            this.EntityPM.SaleCurrencyCode = this.EntityPM.CostCurrencyCode;
            this.EntityPM.SaleExchangeRate = this.EntityPM.CostExchangeRate;
        }

        else {
            this.EntityPM.SaleCurrencyId = this.QuotePM.SaleCurrencyId;
            this.EntityPM.SaleCurrencyCode = this.QuotePM.SaleCurrencyCode;
            this.EntityPM.SaleExchangeRate = this.QuotePM.ExchangeRate;
        }

        this.OnChargeCurrencyChanged();
    }
    OnQuoteSaleCurrencyDataChanged() {

        this.UpgradeCostData();

        if (this.QuotePM.IsMultiCurrency) {
            if (this.SaleCurrencyId == this.QuotePM.SaleCurrencyId) {
                this.EntityPM.SaleCurrencyCode = this.QuotePM.SaleCurrencyCode;
                this.EntityPM.SaleExchangeRate = this.QuotePM.ExchangeRate;
            }
        }

        else if (this.QuotePM.IsSaleCurrencySameAsCost) {
            this.EntityPM.SaleCurrencyId = this.EntityPM.CostCurrencyId;
            this.EntityPM.SaleCurrencyCode = this.EntityPM.CostCurrencyCode;
            this.EntityPM.SaleExchangeRate = this.EntityPM.CostExchangeRate;
        }

        else {
            this.EntityPM.SaleCurrencyId = this.QuotePM.SaleCurrencyId;
            this.EntityPM.SaleCurrencyCode = this.QuotePM.SaleCurrencyCode;
            this.EntityPM.SaleExchangeRate = this.QuotePM.ExchangeRate;
        }

        this.OnChargeCurrencyChanged();
    }
    UpgradeCostData() {
        if (this.CostCurrencyId) {

            if (this.CostCurrencyId == this.QuotePM.SaleCurrencyId) {
                if (this.CostExchangeRate != this.QuotePM.ExchangeRate) {
                    this.CostExchangeRate = this.QuotePM.ExchangeRate;
                }

                if (this.CostCurrencyCode != this.QuotePM.SaleCurrencyCode) {
                    this.CostCurrencyCode = this.QuotePM.SaleCurrencyCode;
                }
            }

            else {
                this.CostExchangeRate = this.fatherComponent.Behaviours.GetCurrencyRate(this.CostCurrencyId);
                this.CostCurrencyCode = this.fatherComponent.Behaviours.GetCurrencyCode(this.CostCurrencyId);

                this.CheckAndRemoveItemSalePrice();
            }
        }
    }
    CheckAndRemoveItemSalePrice() {
        if (this.CostCurrencyId) {
            if (this.CostCurrencyId != this.QuotePM.SaleCurrencyId) {

                if (AppTool.IsNullOrEmpty(this.CostUnitPrice)) {
                    this.SaleUnitPrice = null;
                }

                if (AppTool.IsNullOrEmpty(this.CostContainerType1UnitPrice)) {
                    this.SaleUnitPrice1String = null;
                }

                if (AppTool.IsNullOrEmpty(this.CostContainerType2UnitPrice)) {
                    this.SaleUnitPrice2String = null;
                }

                if (AppTool.IsNullOrEmpty(this.CostContainerType3UnitPrice)) {
                    this.SaleUnitPrice3String = null;
                }

                if (AppTool.IsNullOrEmpty(this.CostContainerType4UnitPrice)) {
                    this.SaleUnitPrice4String = null;
                }

                if (AppTool.IsNullOrEmpty(this.CostContainerType5UnitPrice)) {
                    this.SaleUnitPrice5String = null;
                }
            }
        }
    }
    OnChargeCurrencyChanged() {
        this.ComputeCostInSalePrice();
        this.ComputeCostInSalePrice1();
        this.ComputeCostInSalePrice2();
        this.ComputeCostInSalePrice3();
        this.ComputeCostInSalePrice4();
        this.ComputeCostInSalePrice5();
        this.ComputeCostInSaleAmount();
        this.EntityPM.SaleTotalAmountLocal = AppTool.IsNullOrEmpty(this.SaleTotalAmount) ? null : AppTool.Round(this.SaleTotalAmount * this.SaleExchangeRate, 2);
        this.EntityPM.SaleAmountInSaleCurrency = AppTool.IsNullOrEmpty(this.EntityPM.SaleTotalAmountLocal) ? null : AppTool.Round(this.EntityPM.SaleTotalAmountLocal / this.QuotePM.ExchangeRate, 2);

        this.SetUIProperties_AllIn();
    }

}
