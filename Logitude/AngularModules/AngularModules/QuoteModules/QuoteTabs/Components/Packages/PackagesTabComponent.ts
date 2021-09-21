import {Component, OnInit, ViewChild, ViewContainerRef, OnDestroy} from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {QuotePackagePM} from '../../../../Quote/EntityPMs/QuotePackagePM';
import {QuoteUtilities} from '../../../../Quote/Utilities/QuoteUtilities';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {PackageTypeListService} from '../../../../Common/Services/StandardLists/PackageTypeListService';
import {PackageTypeList} from '../../../../Common/EntityLists/PackageTypeList';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {QuoteTool} from '../../../../Quote/Tools';

@Component({
    selector: 'PackagesTabComponent',
    
    templateUrl: './PackagesTabComponent.html',
})

export class PackagesTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: QuotePM;
    public DataContext: PackagesTabComponent = this;
    public ObjectTableName: string = "Quote";
    public IsResourcesReady: boolean = false;
    public ItemsSource: ObservableCollection;
    public TransportModeId: string;
    public QuoteIsFCL: boolean = true;    
    private CurrentSession = SessionLocator.SelectedSession;
    public IsHybrid: boolean;

    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.ItemsSource = new ObservableCollection([]);
        this.IsHybrid = SessionLocator.TenantPM.IsHybrid;
        this.Listen();
    }

    ngOnInit() {
        if (this.EntityPM != null) {
            this.TransportModeId = this.EntityPM.TransportModeId;
            this.QuoteIsFCL = !QuoteUtilities.IsLCLQuote(this.EntityPM);            

            this.entityResourceService.getEntityResourceByTableName("QuotePackage").subscribe((res: any) => {
                this.IsResourcesReady = true;
            });

            this.GetDescriptionFlowDirection();
            this.SetLabels();
            this.SetUIProperties();
            this.BuildItemsSource();
        }
    }

    SetUIPropertiesToRoutingRatesType(){
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("NumberOfPackages", this.ObjectTableName, false);
    }

    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null; 
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.QuoteIsFCL = !QuoteUtilities.IsLCLQuote(this.EntityPM);                                
                    this.SetUIProperties();
                    this.BuildItemsSource();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.QuoteIsFCL = !QuoteUtilities.IsLCLQuote(this.EntityPM);
                    this.SetLabels();
                    this.SetUIProperties();
                    this.BuildItemsSource();
                }
            });

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "QTPK") {
                    this.SetUIProperties_DimFactor();
                    this.SetUIProperties_Expected_Details();
                    this.SetUIProperties_DimensionsUnitCode();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    public IsEditingEnabled: boolean = true;
    public IsAddButtonEnabled: boolean = false;
    public IsDimFactorVisible: boolean = false
    public IsQuantitiesVisible: boolean = false;
    SetUIProperties() {
        this.IsAddButtonEnabled = false;

        this.IsEditingEnabled = QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);

        if (this.IsEditingEnabled) {
            if (this.EntityPM.QuoteTypeCode == "A") {
                this.IsAddButtonEnabled = true;
            }
        }

        if (this.QuoteIsFCL) {
            var isQuantitiesVisible = (this.EntityPM.QuoteTypeCode == "A");
            this.IsQuantitiesVisible = isQuantitiesVisible;

            this.UIProperties.SetVisibility("PackageType1Quantity", this.ObjectTableName, isQuantitiesVisible);
            this.UIProperties.SetVisibility("PackageType2Quantity", this.ObjectTableName, isQuantitiesVisible);
            this.UIProperties.SetVisibility("PackageType3Quantity", this.ObjectTableName, isQuantitiesVisible);
            this.UIProperties.SetVisibility("PackageType4Quantity", this.ObjectTableName, isQuantitiesVisible);
            this.UIProperties.SetVisibility("PackageType5Quantity", this.ObjectTableName, isQuantitiesVisible);

            this.SetUIProperties_Expected_Details();
        }

        this.SetUIProperties_EntityClosed();
        this.SetUIProperties_Totals();
        this.SetUIProperties_DimFactor();
        this.SetUIProperties_DimensionsUnitCode();
    }
    private SetUIProperties_EntityClosed() {
        this.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DimensionsUnitCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("GrossWeightUnitCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ChargeableWeightUnitCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Ratio", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DimFactor", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("IsDangerous", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DescriptionOfGoods", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PickupDeliveryRatio", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PickupDeliveryCWeightUnitCode", this.ObjectTableName, this.IsEditingEnabled);
    }


    public Delete1IsVisible: boolean = false;
    public Delete2IsVisible: boolean = false;
    public Delete3IsVisible: boolean = false;
    public Delete4IsVisible: boolean = false;
    public Delete5IsVisible: boolean = false;
    private SetUIProperties_Expected_Details() {
        this.UIProperties.SetEnabled("PackageType1Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PackageType2Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PackageType3Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PackageType4Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PackageType5Quantity", this.ObjectTableName, this.IsEditingEnabled);

        if (this.EntityPM.QuoteTypeCode == "P") {
            this.UIProperties.SetEnabled("PackageType1Id", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("PackageType2Id", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("PackageType3Id", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("PackageType4Id", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("PackageType5Id", this.ObjectTableName, this.IsEditingEnabled);
        }

        else {
            var package1ControlIsEnabled: boolean = true;
            var package2ControlIsEnabled: boolean = true;
            var package3ControlIsEnabled: boolean = true;
            var package4ControlIsEnabled: boolean = true;
            var package5ControlIsEnabled: boolean = true;

            //1
            if (AppTool.IsNullOrZero(this.PackageType1Quantity)) {
                package1ControlIsEnabled = false;
            }

            else if (this.EntityPM.QuoteCharges.filter(d => d.CostContainerType1UnitPrice != null).length > 0 && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType1Id)) {
                package1ControlIsEnabled = false;
            }

            //2
            if (AppTool.IsNullOrZero(this.PackageType2Quantity)) {
                package2ControlIsEnabled = false;
            }

            else if (this.EntityPM.QuoteCharges.filter(d => d.CostContainerType2UnitPrice != null).length > 0 && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType2Id)) {
                package2ControlIsEnabled = false;
            }

            //3
            if (AppTool.IsNullOrZero(this.PackageType3Quantity)) {
                package3ControlIsEnabled = false;
            }

            else if (this.EntityPM.QuoteCharges.filter(d => d.CostContainerType3UnitPrice != null).length > 0 && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType3Id)) {
                package3ControlIsEnabled = false;
            }

            //4
            if (AppTool.IsNullOrZero(this.PackageType4Quantity)) {
                package4ControlIsEnabled = false;
            }

            else if (this.EntityPM.QuoteCharges.filter(d => d.CostContainerType4UnitPrice != null).length > 0 && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType4Id)) {
                package4ControlIsEnabled = false;
            }

            //5
            if (AppTool.IsNullOrZero(this.PackageType5Quantity)) {
                package5ControlIsEnabled = false;
            }

            else if (this.EntityPM.QuoteCharges.filter(d => d.CostContainerType5UnitPrice != null).length > 0 && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType5Id != null)) {
                package5ControlIsEnabled = false;
            }

            this.UIProperties.SetEnabled("PackageType1Id", this.ObjectTableName, package1ControlIsEnabled && this.IsEditingEnabled);
            this.UIProperties.SetEnabled("PackageType2Id", this.ObjectTableName, package2ControlIsEnabled && this.IsEditingEnabled);
            this.UIProperties.SetEnabled("PackageType3Id", this.ObjectTableName, package3ControlIsEnabled && this.IsEditingEnabled);
            this.UIProperties.SetEnabled("PackageType4Id", this.ObjectTableName, package4ControlIsEnabled && this.IsEditingEnabled);
            this.UIProperties.SetEnabled("PackageType5Id", this.ObjectTableName, package5ControlIsEnabled && this.IsEditingEnabled);
        }

        if (this.EntityPM.QuoteCharges.filter(d => d.CostContainerType1UnitPrice != null).length > 0 && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType1Id)) {
            this.Delete1IsVisible = true;
        }

        else {
            this.Delete1IsVisible = false;
        }

        //2
        if (this.EntityPM.QuoteCharges.filter(d => d.CostContainerType2UnitPrice != null).length > 0 && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType2Id)) {
            this.Delete2IsVisible = true;
        }

        else {
            this.Delete2IsVisible = false;
        }

        //3
        if (this.EntityPM.QuoteCharges.filter(d => d.CostContainerType3UnitPrice != null).length > 0 && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType3Id)) {
            this.Delete3IsVisible = true;
        }

        else {
            this.Delete3IsVisible = false;
        }

        //4
        if (this.EntityPM.QuoteCharges.filter(d => d.CostContainerType4UnitPrice != null).length > 0 && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType4Id)) {
            this.Delete4IsVisible = true;
        }

        else {
            this.Delete4IsVisible = false;
        }

        //5
        if (this.EntityPM.QuoteCharges.filter(d => d.CostContainerType5UnitPrice != null).length > 0 && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType5Id != null)) {
            this.Delete5IsVisible = true;
        }

        else {
            this.Delete5IsVisible = false;
        }
    }

    private SetUIProperties_Totals() {
        var isTotalsFieldEnabled = false;
        var isTotalsEditedFieldEnabled = false;

        if (this.IsEditingEnabled) {
            isTotalsFieldEnabled = true;
            isTotalsEditedFieldEnabled = true;

            if (this.EntityPM.QuotePackages.length > 0) {
                isTotalsFieldEnabled = false;
            }
        }

      if (this.EntityPM.QuoteTypeCode == "P") {
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("NumberOfPackages", this.ObjectTableName, false);
      }

      else {
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isTotalsFieldEnabled);
        this.UIProperties.SetEnabled("NumberOfPackages", this.ObjectTableName, isTotalsFieldEnabled);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isTotalsEditedFieldEnabled);
        this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, isTotalsEditedFieldEnabled);
      }
    }
    private SetUIProperties_DimFactor() {
        var isDimFactorVisibile = false;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.DimensionsUnitCode)) {
            if (this.EntityPM.DimensionsUnitCode.toUpperCase() == "INC") {
                isDimFactorVisibile = true;
            }
        }

        this.IsDimFactorVisible = isDimFactorVisibile;
        this.UIProperties.SetVisibility("DimFactor", this.ObjectTableName, isDimFactorVisibile);
    }

    public DimensionsDependencyProperty1: string = null;
    public DimensionsDependencyProperty1IsList: boolean = false;
    private SetUIProperties_DimensionsUnitCode() {
        var isFieldEnabled: boolean = false;

        if (this.IsEditingEnabled && this.VolumeUnitCode == "CBF") {
            isFieldEnabled = true;
        }

        if (this.VolumeUnitCode == "CBF") {
            this.DimensionsDependencyProperty1 = "Ft,Inc";
            this.DimensionsDependencyProperty1IsList = true;
        }

        else {
            this.DimensionsDependencyProperty1 = null;
            this.DimensionsDependencyProperty1IsList = false;
        }

        this.UIProperties.SetEnabled("DimensionsUnitCode", this.ObjectTableName, isFieldEnabled);
    }

    
    ///////// Measurments ///////////
    public MeasurmentsButtonToolTip: string = TextCodeTranslator.Translate("Quote.B.Details.MeasurmentsSettings");
    IsMeasurmentsHidden: boolean = true;
    MeasurmentsSettingsClicked() {
        this.IsMeasurmentsHidden = !this.IsMeasurmentsHidden;

        if (this.IsMeasurmentsHidden) {
            this.MeasurmentsButtonToolTip = TextCodeTranslator.Translate("Quote.B.Details.MeasurmentsSettings");
        }

        else {
            this.MeasurmentsButtonToolTip = TextCodeTranslator.Translate("Quote.B.Details.HideMeasurmentsSettings");
        }
    }

    get GrossWeightUnitCode() { return this.EntityPM.GrossWeightUnitCode; }
    set GrossWeightUnitCode(newValue: string) {
        if (this.EntityPM.GrossWeightUnitCode != newValue) {
            this.EntityPM.GrossWeightUnitCode = newValue;

            this.OnMeasurmentsSettingsChanged();
            this.ComputeGrossWeigh_Kg_Ton();           
        }
    }

    get ChargeableWeightUnitCode() { return this.EntityPM.ChargeableWeightUnitCode; }
    set ChargeableWeightUnitCode(newValue: string) {
        if (this.EntityPM.ChargeableWeightUnitCode != newValue) {
            this.EntityPM.ChargeableWeightUnitCode = newValue;

            this.ComputeDimFactor();
            this.OnMeasurmentsSettingsChanged();
            this.ChargeableWeight_Kg();
        }
    }

    get PickupDeliveryCWeightUnitCode() { return this.EntityPM.PickupDeliveryCWeightUnitCode; }
    set PickupDeliveryCWeightUnitCode(newValue: string) {
        if (this.EntityPM.PickupDeliveryCWeightUnitCode != newValue) {
            this.EntityPM.PickupDeliveryCWeightUnitCode = newValue;
            this.OnMeasurmentsSettingsChanged();
        }
    }


    get DimensionsUnitCode() { return this.EntityPM.DimensionsUnitCode; }
    set DimensionsUnitCode(newValue: string) {
        if (this.EntityPM.DimensionsUnitCode != newValue) {
            this.EntityPM.DimensionsUnitCode = newValue;

            this.ComputeDimFactor();
            this.SetUIProperties_DimFactor();
            this.OnMeasurmentsSettingsChanged();
        }
    }

    get VolumeUnitCode() { return this.EntityPM.VolumeUnitCode; }
    set VolumeUnitCode(newValue: string) {
        if (this.EntityPM.VolumeUnitCode != newValue) {
            this.EntityPM.VolumeUnitCode = newValue;

            this.EntityPM.DimensionsUnitCode = AppTool.GetDimentionsCodeFromVolumeCode(newValue);

            this.ComputeDimFactor();
            this.SetUIProperties_DimFactor();
            this.SetUIProperties_DimensionsUnitCode();
            this.OnMeasurmentsSettingsChanged();
        }
    }

    get Ratio() { return this.EntityPM.Ratio; }
    set Ratio(newValue: number) {
        if (this.EntityPM.Ratio != newValue) {
            this.EntityPM.Ratio = newValue;

            this.ComputeDimFactor();
            QuoteUtilities.OnQuoteRatioChanged(this.EntityPM);
        }
    }

    get PickupDeliveryRatio() { return this.EntityPM.PickupDeliveryRatio; }
    set PickupDeliveryRatio(newValue: number) {
        if (this.EntityPM.PickupDeliveryRatio != newValue) {
            this.EntityPM.PickupDeliveryRatio = newValue;
            QuoteUtilities.OnQuotePickupDeliveryRatioChanged(this.EntityPM);
        }
    }

    get DimFactor() { return this.EntityPM.DimFactor; }
    set DimFactor(newValue: number) {
        if (this.EntityPM.DimFactor != newValue) {
            this.EntityPM.DimFactor = newValue;

            this.EntityPM.Ratio = AppTool.GetRatioFromDimFactor(this.DimFactor, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
            QuoteUtilities.OnQuoteRatioChanged(this.EntityPM);
        }
    }

    private ComputeDimFactor() {
        this.EntityPM.DimFactor = AppTool.GetDimFactorFromRatio(this.Ratio, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
    }    
    private OnMeasurmentsSettingsChanged() {
        this.SetLabels();

        QuoteUtilities.RecalculateQuoteFields(this.EntityPM);
    }

    BuildItemsSource() {
        this.ItemsSource.Clear();

        this.EntityPM.QuotePackages.forEach((item) => {
            this.ItemsSource.Insert(new QuotePackageItem(item, this, false));
        })

        this.SetUIProperties_Totals();
    }

    public SelectedRow: QuotePackageItem = null;
    OnRowSelected(itemComponent: QuotePackageItem) {
        this.SelectedRow = itemComponent;
    }
    
    // FCL
    get PackageType1Quantity() { return this.EntityPM.PackageType1Quantity; }
    set PackageType1Quantity(newValue: number) {
        if (this.EntityPM.PackageType1Quantity != newValue) {
            this.EntityPM.PackageType1Quantity = newValue;

            if (this.EntityPM.QuoteTypeCode != "P") {
                if (AppTool.IsNullOrZero(newValue)) {
                    this.PackageType1Id = null;
                }
            }

            this.SetUIProperties_Expected_Details();
            this.UpdateCharges();
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
        }
    }

    get PackageType2Quantity() { return this.EntityPM.PackageType2Quantity; }
    set PackageType2Quantity(newValue: number) {
        if (this.EntityPM.PackageType2Quantity != newValue) {
            this.EntityPM.PackageType2Quantity = newValue;

            if (this.EntityPM.QuoteTypeCode != "P") {
                if (AppTool.IsNullOrZero(newValue)) {
                    this.PackageType2Id = null;
                }
            }

            this.SetUIProperties_Expected_Details();
            this.UpdateCharges();
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
        }
    }

    get PackageType3Quantity() { return this.EntityPM.PackageType3Quantity; }
    set PackageType3Quantity(newValue: number) {
        if (this.EntityPM.PackageType3Quantity != newValue) {
            this.EntityPM.PackageType3Quantity = newValue;

            if (this.EntityPM.QuoteTypeCode != "P") {
                if (AppTool.IsNullOrZero(newValue)) {
                    this.PackageType3Id = null;
                }
            }

            this.SetUIProperties_Expected_Details();
            this.UpdateCharges();
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
        }
    }

    get PackageType4Quantity() { return this.EntityPM.PackageType4Quantity; }
    set PackageType4Quantity(newValue: number) {
        if (this.EntityPM.PackageType4Quantity != newValue) {
            this.EntityPM.PackageType4Quantity = newValue;

            if (this.EntityPM.QuoteTypeCode != "P") {
                if (AppTool.IsNullOrZero(newValue)) {
                    this.PackageType4Id = null;
                }
            }

            this.SetUIProperties_Expected_Details();
            this.UpdateCharges();
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
        }
    }

    get PackageType5Quantity() { return this.EntityPM.PackageType5Quantity; }
    set PackageType5Quantity(newValue: number) {
        if (this.EntityPM.PackageType5Quantity != newValue) {
            this.EntityPM.PackageType5Quantity = newValue;

            if (this.EntityPM.QuoteTypeCode != "P") {
                if (AppTool.IsNullOrZero(newValue)) {
                    this.PackageType5Id = null;
                }
            }

            this.SetUIProperties_Expected_Details();
            this.UpdateCharges();
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
        }
    }

    get PackageType1Id() { return this.EntityPM.PackageType1Id; }
    set PackageType1Id(newValue: string) {
        if (this.EntityPM.PackageType1Id != newValue) {
            this.EntityPM.PackageType1Id = newValue;
            this.UpdateCharges();
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            this.SetUIProperties_Expected_Details();
        }
    }

    get PackageType2Id() { return this.EntityPM.PackageType2Id; }
    set PackageType2Id(newValue: string) {
        if (this.EntityPM.PackageType2Id != newValue) {
            this.EntityPM.PackageType2Id = newValue;
            this.UpdateCharges();
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            this.SetUIProperties_Expected_Details();
        }
    }

    get PackageType3Id() { return this.EntityPM.PackageType3Id; }
    set PackageType3Id(newValue: string) {
        if (this.EntityPM.PackageType3Id != newValue) {
            this.EntityPM.PackageType3Id = newValue;
            this.UpdateCharges();
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            this.SetUIProperties_Expected_Details();
        }
    }

    get PackageType4Id() { return this.EntityPM.PackageType4Id; }
    set PackageType4Id(newValue: string) {
        if (this.EntityPM.PackageType4Id != newValue) {
            this.EntityPM.PackageType4Id = newValue;
            this.UpdateCharges();
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            this.SetUIProperties_Expected_Details();
        }
    }

    get PackageType5Id() { return this.EntityPM.PackageType5Id; }
    set PackageType5Id(newValue: string) {
        if (this.EntityPM.PackageType5Id != newValue) {
            this.EntityPM.PackageType5Id = newValue;
            this.UpdateCharges();
            this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            this.SetUIProperties_Expected_Details();
        }
    }

    private UpdateCharges() {
        var sum = 0;
        //if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType1Id)) {
        sum = sum + (AppTool.IsNullOrEmpty(this.PackageType1Quantity) ? 0 : this.PackageType1Quantity);
        //}

        //if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType2Id)) {
        sum = sum + (AppTool.IsNullOrEmpty(this.PackageType2Quantity) ? 0 : this.PackageType2Quantity);
        //}

        //if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType3Id)) {
        sum = sum + (AppTool.IsNullOrEmpty(this.PackageType3Quantity) ? 0 : this.PackageType3Quantity);
        //}

        //if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType4Id)) {
        sum = sum + (AppTool.IsNullOrEmpty(this.PackageType4Quantity) ? 0 : this.PackageType4Quantity);
        //}

        //if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType5Id)) {
        sum = sum + (AppTool.IsNullOrEmpty(this.PackageType5Quantity) ? 0 : this.PackageType5Quantity);
        //}

        if (this.QuoteIsFCL) {
            this.EntityPM.NumberOfContainers = sum;
        }

        else {
            this.NumberOfPackages = sum;
        }

        this.EntityPM.QuoteCharges.forEach(item => {
            if (AppTool.IsNullOrEmpty(this.PackageType1Id)) {
                item.CostContainerType1UnitPrice = null;
                item.SaleContainerType1UnitPrice = null;
                item.CostUnitPrice1InSaleCurrency = null;
                item.ContainerType1MarkUpValue = 0;
                item.ContainerType1MarkUpTypeCode = "F";
            }

            if (AppTool.IsNullOrEmpty(this.PackageType2Id)) {
                item.CostContainerType2UnitPrice = null;
                item.SaleContainerType2UnitPrice = null;
                item.CostUnitPrice2InSaleCurrency = null;
                item.ContainerType2MarkUpValue = 0;
                item.ContainerType2MarkUpTypeCode = "F";
            }

            if (AppTool.IsNullOrEmpty(this.PackageType3Id)) {
                item.CostContainerType3UnitPrice = null;
                item.SaleContainerType3UnitPrice = null;
                item.CostUnitPrice3InSaleCurrency = null;
                item.ContainerType3MarkUpValue = 0;
                item.ContainerType3MarkUpTypeCode = "F";
            }

            if (AppTool.IsNullOrEmpty(this.PackageType4Id)) {
                item.CostContainerType4UnitPrice = null;
                item.SaleContainerType4UnitPrice = null;
                item.CostUnitPrice4InSaleCurrency = null;
                item.ContainerType4MarkUpValue = 0;
                item.ContainerType4MarkUpTypeCode = "F";
            }

            if (AppTool.IsNullOrEmpty(this.PackageType5Id)) {
                item.CostContainerType5UnitPrice = null;
                item.SaleContainerType5UnitPrice = null;
                item.CostUnitPrice5InSaleCurrency = null;
                item.ContainerType5MarkUpValue = 0;
                item.ContainerType5MarkUpTypeCode = "F";
            }

            this.ComputeCostAmounts(item);
            this.ComputeSaleAmounts(item);            
        });

        this.ComputeChargesTotals();
    }
    ComputeCostAmounts(item: any) {
        if (item.CostMeasurementCode == "BCNT") {
            var myTotalAmount = null;

            if (!AppTool.IsNullOrEmpty(item.CostContainerType1UnitPrice) && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType1Quantity)) {
                var R1 = item.CostContainerType1UnitPrice * this.EntityPM.PackageType1Quantity;
                myTotalAmount = myTotalAmount == null ? R1 : myTotalAmount + R1;
            }

            if (!AppTool.IsNullOrEmpty(item.CostContainerType2UnitPrice) && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType2Quantity)) {
                var R2 = item.CostContainerType2UnitPrice * this.EntityPM.PackageType2Quantity;
                myTotalAmount = myTotalAmount == null ? R2 : myTotalAmount + R2;
            }

            if (!AppTool.IsNullOrEmpty(item.CostContainerType3UnitPrice) && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType3Quantity)) {
                var R3 = item.CostContainerType3UnitPrice * this.EntityPM.PackageType3Quantity;
                myTotalAmount = myTotalAmount == null ? R3 : myTotalAmount + R3;
            }

            if (!AppTool.IsNullOrEmpty(item.CostContainerType4UnitPrice) && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType4Quantity)) {
                var R4 = item.CostContainerType4UnitPrice * this.EntityPM.PackageType4Quantity;
                myTotalAmount = myTotalAmount == null ? R4 : myTotalAmount + R4;
            }

            if (!AppTool.IsNullOrEmpty(item.CostContainerType5UnitPrice) && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType5Quantity)) {
                var R5 = item.CostContainerType5UnitPrice * this.EntityPM.PackageType5Quantity;
                myTotalAmount = myTotalAmount == null ? R5 : myTotalAmount + R5;
            }

            if (AppTool.IsNullOrEmpty(myTotalAmount)) {
                item.CostTotalAmount = null;
                item.CostTotalAmountLocal = null;
                item.CostAmountInSaleCurrency = null;
            }

            else {
                item.CostTotalAmount = AppTool.Round(myTotalAmount, 2);

                if (AppTool.IsNullOrEmpty(item.CostExchangeRate)) {
                    item.CostTotalAmountLocal = null;
                }

                else {
                    item.CostTotalAmountLocal = AppTool.Round(myTotalAmount * item.CostExchangeRate, 2);
                }

                this.ComputeCostInSaleAmount(item);
            }
        }
    }
    ComputeSaleAmounts(item: any) {
        if (item.SaleMeasurementCode == "BCNT") {
            var myTotalAmount = null;

            if (!AppTool.IsNullOrEmpty(item.SaleContainerType1UnitPrice) && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType1Quantity)) {
                var R1 = item.SaleContainerType1UnitPrice * this.EntityPM.PackageType1Quantity;
                myTotalAmount = myTotalAmount == null ? R1 : myTotalAmount + R1;
            }

            if (!AppTool.IsNullOrEmpty(item.SaleContainerType2UnitPrice) && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType2Quantity)) {
                var R2 = item.SaleContainerType2UnitPrice * this.EntityPM.PackageType2Quantity;
                myTotalAmount = myTotalAmount == null ? R2 : myTotalAmount + R2;
            }

            if (!AppTool.IsNullOrEmpty(item.SaleContainerType3UnitPrice) && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType3Quantity)) {
                var R3 = item.SaleContainerType3UnitPrice * this.EntityPM.PackageType3Quantity;
                myTotalAmount = myTotalAmount == null ? R3 : myTotalAmount + R3;
            }

            if (!AppTool.IsNullOrEmpty(item.SaleContainerType4UnitPrice) && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType4Quantity)) {
                var R4 = item.SaleContainerType4UnitPrice * this.EntityPM.PackageType4Quantity;
                myTotalAmount = myTotalAmount == null ? R4 : myTotalAmount + R4;
            }

            if (!AppTool.IsNullOrEmpty(item.SaleContainerType5UnitPrice) && !AppTool.IsNullOrEmpty(this.EntityPM.PackageType5Quantity)) {
                var R5 = item.SaleContainerType5UnitPrice * this.EntityPM.PackageType5Quantity;
                myTotalAmount = myTotalAmount == null ? R5 : myTotalAmount + R5;
            }

            item.SaleTotalAmount = AppTool.Round(myTotalAmount, 2);
            item.SaleTotalAmountLocal = AppTool.IsNullOrEmpty(myTotalAmount) ? null : AppTool.Round(myTotalAmount * item.SaleExchangeRate, 2);
        }
    }
    ComputeCostInSaleAmount(item: any) {
        var myResult = null;

        if (this.EntityPM.IsSaleCurrencySameAsCost) {
            if (!AppTool.IsNullOrEmpty(item.CostTotalAmount)) {
                myResult = item.CostTotalAmount;
            }
        }

        else {
            if (!AppTool.IsNullOrEmpty(item.CostTotalAmountLocal) && !AppTool.IsNullOrEmpty(this.EntityPM.ExchangeRate)) {
                myResult = item.CostTotalAmountLocal / this.EntityPM.ExchangeRate;
            }
        }

        item.CostAmountInSaleCurrency = AppTool.Round(myResult,2);
    }
    ComputeChargesTotals() {
        var myProfitAmount = 0;

        var myCostAmountLocal = AppTool.Round(ArrayTool.Sum(this.EntityPM.QuoteCharges, "CostTotalAmountLocal"), 2);
        var mySaleAmountLocal = AppTool.Round(ArrayTool.Sum(this.EntityPM.QuoteCharges.filter(f => f.IsAllIN == false), "SaleTotalAmountLocal"), 2);
        var mySaleProfitLocal = AppTool.Round(mySaleAmountLocal - myCostAmountLocal, 2);

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ExchangeRate)) {
            myProfitAmount = AppTool.Round(mySaleProfitLocal / this.EntityPM.ExchangeRate, 2);
        }

        if (this.EntityPM.EstimateProfit != myProfitAmount) {
            this.EntityPM.EstimateProfit = AppTool.Round(myProfitAmount, 2);
        }

        this.CurrentSession.FireEvent("FCLPackagesChanged");
    }

    DeleteFCLPackage(packageIndex: string) {
        switch (packageIndex) {
            case "1": {
                this.PackageType1Id = null;                
                break;
            }

            case "2": {
                this.PackageType2Id = null; 
                break;
            }

            case "3": {
                this.PackageType3Id = null; 
                break;
            }

            case "4": {
                this.PackageType4Id = null; 
                break;
            }

            case "5": {
                this.PackageType5Id = null; 
                break;
            }
        }

        this.SetUIProperties_Expected_Details();
    }

    //////////// Summary /////////////
    public VolumeLabel: string;
    public GrossWeightLabel: string;
    public ChargeableWeightLabel: string;
    public ChargeableWeightUnitCodeLabel: string;
    private SetLabels() {
        this.VolumeLabel = TextCodeTranslator.Translate("Quote.F.Volume").replace("%VolumeCode", this.EntityPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate("Quote.F.GrossWeight").replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);

        if (this.EntityPM.TransportModeId == "A") {
            this.ChargeableWeightLabel = TextCodeTranslator.Translate("Quote.F.ChargeableWeight").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
            this.ChargeableWeightUnitCodeLabel = TextCodeTranslator.Translate("Quote.F.ChargeableWeightUnitCode");
        }

        else {
            this.ChargeableWeightLabel = TextCodeTranslator.Translate("Quote.F.ChargeableWeight.Short").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
            this.ChargeableWeightUnitCodeLabel = TextCodeTranslator.Translate("Quote.F.ChargeableWeightUnitCode.Short");
        }
    }

    ComputeTotals() {
        if (this.EntityPM.QuotePackages.length == 0) {
            this.Volume = null;
            this.GrossWeight = null;
            this.ChargeableWeight = null;
            this.VolumetricWeight = null;
            this.NumberOfPackages = null;
            this.GrossWeightEdited = false;
            this.ChargeableWeightEdited = false;
            this.PickupDeliveryChargeableWeight = null;
            this.PickupDeliveryVolumetricWeight = null;
        }

        else {
            var myNumberOfPackages: number = 0;
            var myVolume: number = 0;
            var myGrossWeight: number = 0;
            var myVolumetricWeight: number = 0;
            this.EntityPM.QuotePackages.forEach((item) => {

                if (!AppTool.IsNullOrEmpty(item.Quantity)) {
                    myNumberOfPackages += item.Quantity;
                }

                if (!AppTool.IsNullOrEmpty(item.Volume)) {
                    myVolume += item.Volume;
                }

                if (!AppTool.IsNullOrEmpty(item.VolumetricWeight)) {
                    myVolumetricWeight += item.VolumetricWeight;
                }

                if (!AppTool.IsNullOrEmpty(item.GrossWeight)) {
                    myGrossWeight += item.GrossWeight;
                }
            })

            this.NumberOfPackages = myNumberOfPackages;
            this.Volume = myVolume;
            this.VolumetricWeight = AppTool.Round(myVolumetricWeight, 3);
            this.PickupDeliveryVolumetricWeight = AppTool.ComputePackageVolumetricWeight(null, null, null, null, this.Volume, this.GrossWeight, this.PickupDeliveryRatio, this.DimensionsUnitCode, this.VolumeUnitCode, this.GrossWeightUnitCode, this.PickupDeliveryCWeightUnitCode);
            this.PickupDeliveryChargeableWeight = AppTool.CalculateChargeableWeight(this.GrossWeight, this.PickupDeliveryVolumetricWeight, this.GrossWeightUnitCode, this.PickupDeliveryCWeightUnitCode, this.EntityPM.DirectionId, this.TransportModeId);
            if (!this.GrossWeightEdited) {
                this.GrossWeight = AppTool.Round(myGrossWeight, 3);
            }
            if (!this.ChargeableWeightEdited) {
                this.ChargeableWeight = QuoteUtilities.ComputeChargeableWeight(this.EntityPM);
            }
        }

        this.ComputeGrossWeigh_Kg_Ton();
        this.ChargeableWeight_Kg();
        QuoteTool.OnQuoteQuantitiesChanged(this.EntityPM);

        this.SetUIProperties_Totals();
    }

    get GrossWeight() { return this.EntityPM.GrossWeight; }
    set GrossWeight(newValue: number) {
        if (this.EntityPM.GrossWeight != newValue) {
            this.EntityPM.GrossWeight = AppTool.Round(newValue, 3);
            this.ComputeChargeableWeight();
            //this.EntityPM.ChargeableWeight = QuoteUtilities.ComputeChargeableWeight(this.EntityPM);
        }
    }

    get Volume() { return this.EntityPM.Volume; }
    set Volume(newValue: number) {
        if (this.EntityPM.Volume != newValue) {
            this.EntityPM.Volume = AppTool.Round(newValue, 3);
            this.ComputeVolumetricWeight();
            //this.EntityPM.VolumetricWeight = QuoteUtilities.ComputeVolumetricWeight(this.EntityPM);
        }
    }


    ComputeChargeableWeight() {
        this.ChargeableWeight = AppTool.CalculateChargeableWeight(this.GrossWeight, this.EntityPM.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
        this.ComputePickupDeliveryChargeableWeight();
    }

    ComputePickupDeliveryChargeableWeight() {
        this.PickupDeliveryChargeableWeight = AppTool.CalculateChargeableWeight(this.GrossWeight, this.PickupDeliveryVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.PickupDeliveryCWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
    }

    ComputeVolumetricWeight() {
        var myResult = null;

        if (this.Volume != null) {
            myResult = AppTool.GetWeightFromVolume(this.EntityPM.VolumeUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.Volume, this.EntityPM.Ratio);
        }

        else if (this.GrossWeight != null) {
            myResult = AppTool.GetWeightFromWeight(this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeight);
        }

        this.VolumetricWeight = myResult;
        this.ComputePickupDeliveryVolumetricWeight();
    }

    ComputePickupDeliveryVolumetricWeight() {
        var myResult = null;

        if (this.Volume != null) {
            myResult = AppTool.GetWeightFromVolume(this.EntityPM.VolumeUnitCode, this.EntityPM.PickupDeliveryCWeightUnitCode, this.Volume, this.EntityPM.PickupDeliveryRatio);
        }

        else if (this.GrossWeight != null) {
            myResult = AppTool.GetWeightFromWeight(this.EntityPM.GrossWeightUnitCode, this.EntityPM.PickupDeliveryCWeightUnitCode, this.EntityPM.GrossWeight);
        }

        this.PickupDeliveryVolumetricWeight = myResult;
    }

    private ComputeVolume_CBM() {
        var volume_CBM: number = null;

        if (this.Volume != null) {
            var factorOfConvert: number = 1;

            if (!AppTool.IsNullOrEmpty(this.EntityPM.VolumeUnitCode)) {
                switch (this.EntityPM.VolumeUnitCode.toUpperCase()) {
                    case "CBM": { factorOfConvert = 1; break; }
                    case "CBI": { factorOfConvert = 61024; break; }      // 1m³ = 61024in³
                    case "CBF": { factorOfConvert = 35.315; break; }     // 1m³ = 35.315ft³
                }
            }

            volume_CBM = this.Volume / factorOfConvert;
        }

        if (volume_CBM != null) {
            volume_CBM = AppTool.Round(volume_CBM, 3);
        }
        this.EntityPM.VolumeInCBM = volume_CBM;
    }

    get VolumetricWeight() { return this.EntityPM.VolumetricWeight == null ? 0 : this.EntityPM.VolumetricWeight; }
    set VolumetricWeight(newValue: number) {
        if (this.EntityPM.VolumetricWeight != newValue) {
            this.EntityPM.VolumetricWeight = AppTool.Round(newValue, 3);

            if (this.EntityPM.QuotePackages.length == 0) {
                this.EntityPM.ChargeableWeight = QuoteUtilities.ComputeChargeableWeight(this.EntityPM);
            }
        }
    }

    get PickupDeliveryVolumetricWeight() { return this.EntityPM.PickupDeliveryVolumetricWeight == null ? 0 : this.EntityPM.PickupDeliveryVolumetricWeight; }
    set PickupDeliveryVolumetricWeight(newValue: number) {
        if (this.EntityPM.PickupDeliveryVolumetricWeight != newValue) {
            this.EntityPM.PickupDeliveryVolumetricWeight = AppTool.Round(newValue, 3);

            if (this.EntityPM.QuotePackages.length == 0) {
                this.EntityPM.PickupDeliveryChargeableWeight = AppTool.CalculateChargeableWeight(this.GrossWeight, this.EntityPM.PickupDeliveryVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.PickupDeliveryCWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
            }
        }
    }

    get PickupDeliveryChargeableWeight() { return AppTool.IsNullOrZero(this.EntityPM.PickupDeliveryChargeableWeight) ? null : this.EntityPM.PickupDeliveryChargeableWeight; }
    set PickupDeliveryChargeableWeight(newValue: number) {
        if (this.EntityPM.PickupDeliveryChargeableWeight != newValue) {
            var result = AppTool.Round(newValue, 3);
            this.EntityPM.PickupDeliveryChargeableWeight = result;

            if (this.GrossWeight == null && this.EntityPM.PickupDeliveryVolumetricWeight == null) {
                this.EntityPM.PickupDeliveryVolumetricWeight = result;
                this.EntityPM.GrossWeight = AppTool.GetWeightFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeightUnitCode, result);
                this.EntityPM.Volume = AppTool.GetVolumeFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.EntityPM.PickupDeliveryRatio);
            }
        }
    }

    get ChargeableWeight() { return AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight) ? null : this.EntityPM.ChargeableWeight; }
    set ChargeableWeight(newValue: number) {
        if (this.EntityPM.ChargeableWeight != newValue) {
            var result = AppTool.Round(newValue, 3);
            this.EntityPM.ChargeableWeight = result;

            if (this.GrossWeight == null && this.EntityPM.VolumetricWeight == null) {
                this.EntityPM.VolumetricWeight = result;
                this.EntityPM.GrossWeight = AppTool.GetWeightFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeightUnitCode, result);
                this.EntityPM.Volume = AppTool.GetVolumeFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.EntityPM.Ratio);
            }
        }
    }

    get NumberOfPackages() { return this.EntityPM.NumberOfPackages; }
    set NumberOfPackages(newValue: number) {
        if (this.EntityPM.NumberOfPackages != newValue) {
            this.EntityPM.NumberOfPackages = newValue;
        }
    }

    get IsDangerous() { return this.EntityPM.IsDangerous; }
    set IsDangerous(newValue: boolean) {
        if (this.EntityPM.IsDangerous != newValue) {
            this.EntityPM.IsDangerous = newValue;
        }
    }

    get DescriptionOfGoods() { return this.EntityPM.DescriptionOfGoods; }
    set DescriptionOfGoods(newValue: string) {
        if (this.EntityPM.DescriptionOfGoods != newValue) {
            this.EntityPM.DescriptionOfGoods = newValue;
        }
    }

    get GrossWeightEdited() { return this.EntityPM.GrossWeightEdited; }
    set GrossWeightEdited(value: boolean) {
        if (this.EntityPM.GrossWeightEdited != value) {
            this.EntityPM.GrossWeightEdited = value;
        }
    }

    get ChargeableWeightEdited() { return this.EntityPM.ChargeableWeightEdited; }
    set ChargeableWeightEdited(value: boolean) {
        if (this.EntityPM.ChargeableWeightEdited != value) {
            this.EntityPM.ChargeableWeightEdited = value;
        }
    }

    GrossWeightLostFocus(input: any) {
        if (this.EntityPM.QuotePackages.length > 0) {
            var valueComputed: number = 0;
            var valueInserted: number = 0;

            this.EntityPM.QuotePackages.forEach((item) => {
                if (!AppTool.IsNullOrEmpty(item.GrossWeight)) {
                    valueComputed += item.GrossWeight;
                }
            });

            // if (!AppTool.IsNullOrEmpty(input)) {
            //     input = AppTool.Replace(input, ",", "");
            //     valueInserted = Number(input);
            // }

            valueComputed = valueComputed == 0 ? null : valueComputed;
            valueInserted = valueInserted = AppTool.GetNumberFromText(input);
            this.GrossWeightEdited = !(valueComputed == valueInserted);
            this.GrossWeight = valueInserted;
            this.ComputeTotals();
        }
    }
    ChargeableWeightLostFocus(input: any) {
        if (this.EntityPM.QuotePackages.length > 0) {
            var valueComputed: number = 0;
            var valueInserted: number = 0;

            valueComputed = AppTool.CalculateChargeableWeight(this.EntityPM.GrossWeight, this.EntityPM.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);

            // if (!AppTool.IsNullOrEmpty(input)) {
            //     input = AppTool.Replace(input, ",", "");
            //     valueInserted = Number(input);
            // }

            valueComputed = valueComputed == 0 ? null : valueComputed;
            valueInserted = AppTool.GetNumberFromText(input);
            this.ChargeableWeightEdited = !(valueComputed == valueInserted);
            this.ChargeableWeight = AppTool.RoundChargeableWeight(valueInserted, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
            this.ComputeTotals();
        }
    }
    ResetGrossWeightEdited() {
        this.GrossWeightEdited = false;
        this.ComputeTotals();
    }
    ResetChargeableWeightEdited() {
        this.ChargeableWeightEdited = false;
        this.ComputeTotals();
    }
    ResetTotalEditedValues() {
        this.GrossWeightEdited = false;
        this.ChargeableWeightEdited = false;
    }

    ComputeGrossWeigh_Kg_Ton() {
        var weigh_Kg: number = null;
        var weigh_Ton: number = null;

        if (this.GrossWeight != null) {
            var factorOfConvert: number = 1;

            if (!AppTool.IsNullOrEmpty(this.GrossWeightUnitCode)) {
                switch (this.GrossWeightUnitCode.toUpperCase()) {
                    case "KG": { factorOfConvert = 1; break; }
                    case "LB": { factorOfConvert = 0.45359237; break; }
                    case "MT": { factorOfConvert = 1000; break; }
                }
            }

            weigh_Kg = this.GrossWeight * factorOfConvert;
        }

        if (weigh_Kg != null) {
            weigh_Kg = AppTool.Round(weigh_Kg, 3);

            weigh_Ton = weigh_Kg / 1000;
        }

        if (weigh_Ton != null) {
            weigh_Ton = AppTool.Round(weigh_Ton, 3);
        }

        this.EntityPM.GrossWeightInKG = weigh_Kg;
        this.EntityPM.GrossWeightPerTon = weigh_Ton;
    }
    ChargeableWeight_Kg() {
        var weigh_Kg: number = null;
        var weigh_Ton: number = null;

        if (this.ChargeableWeight != null) {
            var factorOfConvert: number = 1;

            if (!AppTool.IsNullOrEmpty(this.ChargeableWeightUnitCode)) {
                switch (this.ChargeableWeightUnitCode.toUpperCase()) {
                    case "KG": { factorOfConvert = 1; break; }
                    case "LB": { factorOfConvert = 0.45359237; break; }
                    case "MT": { factorOfConvert = 1000; break; }
                }
            }

            weigh_Kg = this.ChargeableWeight * factorOfConvert;
        }

        if (weigh_Kg != null) {
            weigh_Kg = AppTool.Round(weigh_Kg, 3);
        }
        this.EntityPM.ChargeableWeightInKG = weigh_Kg;
    }
    AddPackageClicked() {
        var itemPM = new QuotePackagePM(null);
        itemPM.Tenant = SessionLocator.Tenant;
        itemPM.QuoteId = this.EntityPM.Id;

        var itemComponent = new QuotePackageItem(itemPM, this, true);
        this.RunAddEditPackage(itemComponent, TextCodeTranslator.Translate("Quote.S.Packages.AddPackage"));
    }
    EditPackageClicked(itemComponent: QuotePackageItem) {
        this.RunAddEditPackage(itemComponent, TextCodeTranslator.Translate("Quote.S.Packages.EditPackage"));
    }
    RunAddEditPackage(itemComponent: QuotePackageItem, windowTitle: string) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./QuoteModules/QuoteTabs/Components/Packages/AddEditPackageComponent');
    }
    DeletePackageClicked(itemComponent: QuotePackageItem) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator.Translate("Quote.M.DeleteThisPackage"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                this.EntityPM.RemoveQuotePackagePM(itemComponent.EntityPM);

                this.BuildItemsSource();
                this.ComputeTotals();
            }
        });
    }

    get DescriptionRightToLeft() { return this.EntityPM.DescriptionRightToLeft; }
    set DescriptionRightToLeft(value: boolean) {
        if (this.EntityPM.DescriptionRightToLeft != value) {
            this.EntityPM.DescriptionRightToLeft = value;
        }
    }

    get IsDescriptionRightToLeftEnabled() {
        var myResult = false;
        if (SessionLocator.TenantPM.IsNotesRightToLeftEnabled) {
            myResult = true;
        }
        return myResult;
    }

    public DescriptionFlowDirection: string = "ltr";
    private GetDescriptionFlowDirection() {
        var myResult = "ltr";
        if (SessionLocator.TenantPM.IsNotesRightToLeftEnabled) {
            myResult = "rtl";

            if (this.DescriptionRightToLeft) {
                myResult = "rtl";
            }
            else {
                myResult = "ltr";
            }
        }

        this.DescriptionFlowDirection = myResult;
    }

    public AlignDescriptionLeftClicked() {
        this.DescriptionRightToLeft = false;
        this.GetDescriptionFlowDirection();
    }
    public AlignDescriptionRightClicked() {
        this.DescriptionRightToLeft = true;
        this.GetDescriptionFlowDirection();
    }
}
export class QuotePackageItem extends BaseComponent {
    public EntityPM: QuotePackagePM;
    public QuotePM: QuotePM;
    public ObjectTableName: string = "QuotePackage";
    public IsNewEntity: boolean = false;
    public TransportModeId: string;
    public CellReadOnlyBackground = "#E6E7E8";

    constructor(entity: QuotePackagePM, public fatherComponent: PackagesTabComponent, isNew: boolean) {
        super();
        this.EntityPM = entity;
        this.QuotePM = fatherComponent.EntityPM;
        this.TransportModeId = fatherComponent.EntityPM.TransportModeId;
        this.IsNewEntity = isNew;
        this.SetUIProperties();
    }

    public IsQuoteEditEnabled: boolean = false;
    public IsVolumeEnabled: boolean = false;
    SetUIProperties() {
        var isFieldEnabled = false;
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;
        this.IsQuoteEditEnabled = QuoteUtilities.IsQuoteEditEnabled(this.QuotePM);

        if (this.IsQuoteEditEnabled) {
            if (this.Quantity > 0) {
                isFieldEnabled = true;
                isVolumeEnabled = true;
                isDimensionEnabled = true;

                if (this.Height != null || this.Width != null || this.Length != null) {
                    isVolumeEnabled = false;
                }

                else if (this.Volume != null) {
                    isDimensionEnabled = false;
                }
            }
        }

        this.IsVolumeEnabled = isVolumeEnabled;
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("PackageTypeId", this.ObjectTableName, isFieldEnabled);

        if (this.TransportModeId == "A") {
            this.UIProperties.SetVisibility("PackageTypeId", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetVisibility("PackageTypeId", this.ObjectTableName, true);
            if (AppTool.IsNullOrEmpty(this.PackageTypeId)) {
                this.UIProperties.SetRequired("PackageTypeId", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetRequired("PackageTypeId", this.ObjectTableName, false);
            }
            if (AppTool.IsNullOrEmpty(this.GrossWeight)) {
                this.UIProperties.SetRequired("GrossWeight", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetRequired("GrossWeight", this.ObjectTableName, false);
            }
        }
    }

    public PackageTypeList: PackageTypeList;
    get PackageTypeId() { return this.EntityPM.PackageTypeId; }
    set PackageTypeId(newValue: string) {
        if (this.EntityPM.PackageTypeId != newValue) {
            this.EntityPM.PackageTypeId = newValue;

            if (this.TransportModeId != "A") {
                this.UIProperties.SetRequired('PackageTypeId', this.ObjectTableName, AppTool.IsNullOrEmpty(this.PackageTypeId) ? true : false);
            }

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.PackageTypeName = null;
            }

            else {
                var myService: PackageTypeListService = new PackageTypeListService();
                myService.getSingleFromCache(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PackageTypeList = myResponse.Result;
                        if (list != null) {
                            this.PackageTypeName = list.EnglishName;
                        }
                    }
                });
            }
        }
    }

    get PackageTypeName() { return this.EntityPM.PackageTypeName; }
    set PackageTypeName(newValue: string) {
        if (this.EntityPM.PackageTypeName != newValue) {
            this.EntityPM.PackageTypeName = newValue;
        }
    }

    get Quantity() { return this.EntityPM.Quantity; }
    set Quantity(newValue: number) {
        if (this.EntityPM.Quantity != newValue) {
            this.EntityPM.Quantity = AppTool.Round(newValue, 0);
            this.ComputeVolume();
            this.fatherComponent.ComputeTotals();
            this.SetUIProperties();
        }
    }

    get Length() { return this.EntityPM.Length; }
    set Length(newValue: number) {
        if (this.EntityPM.Length != newValue) {
            this.EntityPM.Length = AppTool.Round(newValue, 2);
            this.ComputeVolume();
            this.SetUIProperties();
        }
    }

    get Width() { return this.EntityPM.Width; }
    set Width(newValue: number) {
        if (this.EntityPM.Width != newValue) {
            this.EntityPM.Width = AppTool.Round(newValue, 2);
            this.ComputeVolume();
            this.SetUIProperties();
        }
    }

    get Height() { return this.EntityPM.Height; }
    set Height(newValue: number) {
        if (this.EntityPM.Height != newValue) {
            this.EntityPM.Height = AppTool.Round(newValue, 2);
            this.ComputeVolume();
            this.SetUIProperties();
        }
    }

    get Dimensions() {
        var myDimensions: string = " - - ";

        if (this.Length == null && this.Width == null && this.Height == null) {
            myDimensions = " - - ";
        }

        else {
            var myLength: number = 0;
            var myWidth: number = 0;
            var myHeight: number = 0;

            if (this.Length != null) {
                myLength = this.Length;
            }

            if (this.Width != null) {
                myWidth = this.Width;
            }

            if (this.Height != null) {
                myHeight = this.Height;
            }

            myDimensions = myLength + "-" + myWidth + "-" + myHeight;
        }

        return myDimensions;
    }

    get Volume() { return this.EntityPM.Volume; }
    set Volume(newValue: number) {
        if (this.EntityPM.Volume != newValue) {
            this.EntityPM.Volume = AppTool.Round(newValue, 3);
            this.ComputeVolumetricWeight();
            this.SetUIProperties();
        }
    }

    get VolumetricWeight() { return this.EntityPM.VolumetricWeight; }
    set VolumetricWeight(newValue: number) {
        if (this.EntityPM.VolumetricWeight != newValue) {
            this.EntityPM.VolumetricWeight = AppTool.Round(newValue, 3);
            this.fatherComponent.ComputeTotals();
        }
    }

    get GrossWeight() { return this.EntityPM.GrossWeight; }
    set GrossWeight(newValue: number) {
        var myValue: number = AppTool.Round(newValue, 2);

        if (this.EntityPM.GrossWeight != myValue) {
            this.EntityPM.GrossWeight = myValue;

            this.SetUIProperties();

            if (this.fatherComponent.ItemsSource.Collection.indexOf(this) > -1) {
                this.fatherComponent.ResetTotalEditedValues();
                this.fatherComponent.ComputeTotals();
            }
        }
    }

    OnGrossWeightLostFocus(input: number) {
        if (AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {
            if (this.Width == null || this.Height == null || this.Length == null) {
                this.EntityPM.VolumetricWeight = AppTool.GetWeightFromWeight(this.QuotePM.GrossWeightUnitCode, this.QuotePM.ChargeableWeightUnitCode, this.EntityPM.GrossWeight);
                this.EntityPM.Volume = AppTool.GetVolumeFromWeight(this.QuotePM.ChargeableWeightUnitCode, this.QuotePM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.QuotePM.Ratio);

                this.SetUIProperties();

                if (this.fatherComponent.ItemsSource.Collection.indexOf(this) > -1) {
                    this.fatherComponent.ResetTotalEditedValues();
                    this.fatherComponent.ComputeTotals();
                }
            }
        }
    }

    private ComputeVolume() {
        if (this.fatherComponent.Ratio == null) {
            this.fatherComponent.Ratio = AppTool.GetRatio(this.QuotePM.DirectionId, this.QuotePM.TransportModeId, this.QuotePM.ShipmentTypeId, InfraSettings.TenantPM.CountryCode);
        }

        this.Volume = AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.GrossWeight, this.QuotePM.Ratio, this.QuotePM.DimensionsUnitCode, this.QuotePM.VolumeUnitCode, this.QuotePM.GrossWeightUnitCode);
    }

    private ComputeVolumetricWeight() {
        if (this.fatherComponent.Ratio == null) {
            this.fatherComponent.Ratio = AppTool.GetRatio(this.QuotePM.DirectionId, this.QuotePM.TransportModeId, this.QuotePM.ShipmentTypeId, InfraSettings.TenantPM.CountryCode);
        }

        this.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.GrossWeight, this.QuotePM.Ratio, this.QuotePM.DimensionsUnitCode, this.QuotePM.VolumeUnitCode, this.QuotePM.GrossWeightUnitCode, this.QuotePM.ChargeableWeightUnitCode);
    }
}
