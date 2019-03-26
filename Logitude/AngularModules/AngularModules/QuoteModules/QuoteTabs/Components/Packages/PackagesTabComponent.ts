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
    moduleId: module.id,
    templateUrl: './PackagesTabComponent.html',
})

export class PackagesTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: QuotePM;
    public DataContext: PackagesTabComponent = this;
    public ObjectTableName: string = "Quote";
    public IsResourcesReady: boolean = false;
    public ItemsSource: ObservableCollection;
    public TransportModeId: string;
    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.ItemsSource = new ObservableCollection([]);

        this.Listen();
    }

    public QuoteIsFCL: boolean = true;    
    ngOnInit() {
        if (this.EntityPM != null) {
            this.TransportModeId = this.EntityPM.TransportModeId;
            this.QuoteIsFCL = !QuoteUtilities.IsLCLQuote(this.EntityPM);            

            this.entityResourceService.getEntityResourceByTableName("QuotePackage").subscribe((res: any) => {
                this.IsResourcesReady = true;
            });

            this.SetLabels();
            this.SetUIProperties();
            this.BuildItemsSource();
        }
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
    public ScreenIsEnabled: boolean = true;
    SetUIProperties() {
        this.SetScreenIsEnabled();
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
    }
    SetScreenIsEnabled() {
        var myResult = true;

        if (this.EntityPM.IsClosed) {
            myResult = false;
        }
        else if (this.EntityPM.IsCancelled) {
            myResult = false;
        }
        this.ScreenIsEnabled =  myResult;
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
        var myResult = true;

        if (!this.ScreenIsEnabled) {
            myResult = false;
        }

        else if (this.ItemsSource.Length > 0) {
            myResult = false;
        }

        else if (this.EntityPM.QuoteTypeCode != "A") {
            myResult = false;
        }

        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, myResult);
        this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, myResult);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, myResult);
        this.UIProperties.SetEnabled("NumberOfPackages", this.ObjectTableName, myResult);
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

    public ComputeTotals() {
        if (this.EntityPM.QuotePackages.length == 0) {
            this.Volume = null;
            this.GrossWeight = null;
            this.ChargeableWeight = null;
            this.EntityPM.VolumetricWeight = null;
            this.NumberOfPackages = null;
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
            this.EntityPM.VolumetricWeight = myVolumetricWeight;
            this.GrossWeight = myGrossWeight;
            this.ChargeableWeight = QuoteUtilities.ComputeChargeableWeight(this.EntityPM);
        }

        this.ComputeGrossWeigh_Kg_Ton();
        QuoteTool.OnQuoteQuantitiesChanged(this.EntityPM);

        this.SetUIProperties_Totals();
    }
    private ComputeGrossWeigh_Kg_Ton() {
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

    ///////// Add-Edit-Delete Package ////////////////   
    AddPackageClicked() {

        var itemPM = new QuotePackagePM(null);
        itemPM.Tenant = SessionLocator.Tenant;
        itemPM.QuoteId = this.EntityPM.Id;

        var itemComponent = new QuotePackageItem(itemPM, this, true);
        this.RunAddEditPackage(itemComponent, TextCodeTranslator.Translate("Quote.S.Packages.AddPackage"));

        //if (this.ItemsSource.Length < 10) {           
        //    var itemPM = new QuotePackagePM(null);
        //    itemPM.Tenant = SessionLocator.Tenant;
        //    itemPM.QuoteId = this.EntityPM.Id;

        //    var itemComponent = new QuotePackageItem(itemPM, this, true);
        //    this.RunAddEditPackage(itemComponent, "Add Package");
        //}

        //else {
        //    var messageWindow = new MessageWindow();
        //    messageWindow.Show("You have reached the limit of 10 lines of packages");
        //}
    };
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
            if (newValue != null && (this.PackageType2Id == newValue || this.PackageType3Id == newValue || this.PackageType4Id == newValue || this.PackageType5Id == newValue)) {

            }

            else {
                if (newValue == null) {
                    this.EntityPM.PackageType1Quantity = null;
                    this.SetUIProperties_Expected_Details();
                }

                this.EntityPM.PackageType1Id = newValue;
                this.UpdateCharges();
                this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        }
    }

    get PackageType2Id() { return this.EntityPM.PackageType2Id; }
    set PackageType2Id(newValue: string) {
        if (this.EntityPM.PackageType2Id != newValue) {
            if (newValue != null && (this.PackageType1Id == newValue || this.PackageType3Id == newValue || this.PackageType4Id == newValue || this.PackageType5Id == newValue)) {

            }

            else {
                if (newValue == null) {
                    this.EntityPM.PackageType2Quantity = null;
                    this.SetUIProperties_Expected_Details();
                }

                this.EntityPM.PackageType2Id = newValue;
                this.UpdateCharges();
                this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        }
    }

    get PackageType3Id() { return this.EntityPM.PackageType3Id; }
    set PackageType3Id(newValue: string) {
        if (this.EntityPM.PackageType3Id != newValue) {
            if (newValue != null && (this.PackageType1Id == newValue || this.PackageType2Id == newValue || this.PackageType4Id == newValue || this.PackageType5Id == newValue)) {

            }

            else {
                if (newValue == null) {
                    this.EntityPM.PackageType3Quantity = null;
                    this.SetUIProperties_Expected_Details();
                }

                this.EntityPM.PackageType3Id = newValue;
                this.UpdateCharges();
                this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        }
    }

    get PackageType4Id() { return this.EntityPM.PackageType4Id; }
    set PackageType4Id(newValue: string) {
        if (this.EntityPM.PackageType4Id != newValue) {
            if (newValue != null && (this.PackageType1Id == newValue || this.PackageType2Id == newValue || this.PackageType3Id == newValue || this.PackageType5Id == newValue)) {

            }

            else {
                if (newValue == null) {
                    this.EntityPM.PackageType4Quantity = null;
                    this.SetUIProperties_Expected_Details();
                }

                this.EntityPM.PackageType4Id = newValue;
                this.UpdateCharges();
                this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        }
    }

    get PackageType5Id() { return this.EntityPM.PackageType5Id; }
    set PackageType5Id(newValue: string) {
        if (this.EntityPM.PackageType5Id != newValue) {
            if (newValue != null && (this.PackageType1Id == newValue || this.PackageType2Id == newValue || this.PackageType3Id == newValue || this.PackageType4Id == newValue)) {

            }

            else {
                if (newValue == null) {
                    this.EntityPM.PackageType5Quantity = null;
                    this.SetUIProperties_Expected_Details();
                }

                this.EntityPM.PackageType5Id = newValue;
                this.UpdateCharges();
                this.EntityPM.TEU = QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        }
    }

    private UpdateCharges() {
        var sum = 0;
        //if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType1Id)) {
            sum = sum + this.PackageType1Quantity;
        //}

        //if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType2Id)) {
            sum = sum + this.PackageType2Quantity;
        //}

        //if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType3Id)) {
            sum = sum + this.PackageType3Quantity;
        //}

        //if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType4Id)) {
            sum = sum + this.PackageType4Quantity;
        //}

        //if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType5Id)) {
            sum = sum + this.PackageType5Quantity;
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

        SessionLocator.CurrentSession.FireEvent("FCLPackagesChanged");
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
            this.ChargeableWeightLabel = TextCodeTranslator.Translate("Quote.F.WtMsr.Short").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
            this.ChargeableWeightUnitCodeLabel = TextCodeTranslator.Translate("Quote.F.WtMsrUnitCode.Short");
        }
    }

    get GrossWeight() { return this.EntityPM.GrossWeight; }
    set GrossWeight(newValue: number) {
        if (this.EntityPM.GrossWeight != newValue) {
            this.EntityPM.GrossWeight = AppTool.Round(newValue, 2);

            this.EntityPM.ChargeableWeight = QuoteUtilities.ComputeChargeableWeight(this.EntityPM);
            this.ComputeGrossWeigh_Kg_Ton();
        }
    }

    get Volume() { return this.EntityPM.Volume; }
    set Volume(newValue: number) {
        if (this.EntityPM.Volume != newValue) {
            this.EntityPM.Volume = AppTool.Round(newValue, 2);

            this.EntityPM.VolumetricWeight = QuoteUtilities.ComputeVolumetricWeight(this.EntityPM);
        }
    }

    get ChargeableWeight() { return this.EntityPM.ChargeableWeight; }
    set ChargeableWeight(newValue: number) {
        if (this.EntityPM.ChargeableWeight != newValue) {
            var result = AppTool.Round(newValue, 2);
            this.EntityPM.ChargeableWeight = result;

            if (this.GrossWeight == null && this.EntityPM.VolumetricWeight == null) {
                this.EntityPM.VolumetricWeight = result;
                this.EntityPM.GrossWeight = AppTool.GetWeightFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeightUnitCode, result);
                this.EntityPM.Volume = AppTool.GetVolumeFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.EntityPM.Ratio);
            }

            if (this.EntityPM.QuoteTypeCode == "A") {
                //if (quoteViewModel.CurrentLCLChargesTrigger.ObsList.Where(d => d.SaleUnitPrice != null || d.CostUnitPrice != null).Any()) {

                //}
                //else {
                //    UpdateChargesQuantities();
                //}
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

            this.fatherComponent.ComputeTotals();
            this.SetUIProperties();
        }
    }

    OnGrossWeightLostFocus(input: number) {
        if (AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {
            if (this.Width == null || this.Height == null || this.Length == null) {
                this.EntityPM.VolumetricWeight = AppTool.GetWeightFromWeight(this.QuotePM.GrossWeightUnitCode, this.QuotePM.ChargeableWeightUnitCode, this.EntityPM.GrossWeight);
                this.EntityPM.Volume = AppTool.GetVolumeFromWeight(this.QuotePM.ChargeableWeightUnitCode, this.QuotePM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.QuotePM.Ratio);

                this.SetUIProperties();
                this.fatherComponent.ComputeTotals();
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
