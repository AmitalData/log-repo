import {Component, OnInit, Output, EventEmitter, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPackagePM} from '../../../../Shipment/EntityPMs/ShipmentPackagePM';
import {ShipmentPackageItemPM} from '../../../../Shipment/EntityPMs/ShipmentPackageItemPM';
import {InsideShipmentPackagePM} from '../../../../Shipment/EntityPMs/InsideShipmentPackagePM';
import {AppTool, FormatTool, ArrayTool, DateTool} from '../../../../Infrastructure/Tools';
import {ShipmentTool} from '../../../../Shipment/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {PackageTypeList} from '../../../../Common/EntityLists/PackageTypeList';
import {PackageTypeListService} from '../../../../Common/Services/StandardLists/PackageTypeListService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ShipmentDomainService, ExcelPackageFilter, ExcelPackage} from '../../../../Shipment/Services/ShipmentDomainService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {ShipmentDeliveryPM} from '../../../../Shipment/EntityPMs/ShipmentDeliveryPM';
import {ShipmentPickUpDeliveryPackagePM} from '../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM';
import {WarehouseReleasePackageListExtendedService} from '../../../../Warehouse/Services/ExtendedLists/WarehouseReleasePackageListExtendedService';
import {PickUpDeliveryPackageHarmonizePM} from '../../../../Shipment/EntityPMs/PickUpDeliveryPackageHarmonizePM';
import { CountryListService } from '../../../../Common/Services/StandardLists/CountryListService';
import { DocumentsFilingExtendedPMService } from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import {WarehouseReleasePMExtendedService} from '../../../../Warehouse/Services/ExtendedPMs/WarehouseReleasePMExtendedService';
import { PackageAmountCalculator } from '../../../../Infrastructure/Utilities/PackageAmountCalculator';
import { ShipmentReceivablePM } from '../../../../Shipment/EntityPMs/ShipmentReceivablePM';
import { HorseList } from '../../../../Common/EntityLists/HorseList';
import { ShipmentSubTypeListService } from '../../../../shipment/services/standardlists/shipmentsubtypelistservice';
declare var ResultAsArray: any;

@Component({    
    templateUrl: './PackagesTabComponent.html',
})

export class PackagesTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: ShipmentPM = null;
    public ObjectTableName: string = null;
    public DataContext = this;
    public ItemsSource: ObservableCollection;
    public DirectionId: string;
    public TransportModeId: string;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public IsResourcesReady: boolean = false;
    public IsContainersFUVisible: boolean = false;
    public IsCommodityNameVisible: boolean = false;
    public IsShowReleaseNumber: boolean = false;
    public IsCommodityNumberVisible: boolean = false;
    public IsShippingInstructionsVisible: boolean = false;
    public IsDeletePackagesButtonVisible: boolean = false;
    public IsDownloadUploadPackagesVisible: boolean = false;
    public HorseFieldIsVisible: boolean = false;
    private warehouseReleasePMExtendedService: WarehouseReleasePMExtendedService;
    @Output() ReloadDetails = new EventEmitter();
    warehouseReleasePackageListExtendedService: WarehouseReleasePackageListExtendedService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;
        this.DirectionId = this.EntityPM.DirectionId;
        this.TransportModeId = this.EntityPM.TransportModeId;
        this.ItemsSource = new ObservableCollection([]);
        this.Listen();
        this.setDigits();
    }

    private SessionEvent: any = null;
    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null; 
    private CrossDockReleasesEvent: any = null;
    private firstDigit: string = ",";
    private secondDigit: string = ".";

    private IsDisconnectWarehouseReleasePackage: boolean = false;

    private chooseShipmentPackageFromWarehouseReleasePackages: boolean = false;
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "AWBWizardClosed") {
                    this.SetUIProperties();
                    this.SetGenerateData();
                    this.BuildItemsSource();
                }
            });
        
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;                    
                    
                    this.SetUIProperties();
                    this.SetGenerateData();
                    this.BuildItemsSource();

                    if (this.dowonload) {
                        this.dowonload = false;
                        this.DownloadPackages();
                    }

                    if (this.saveAfterDeletePackages) {
                        this.saveAfterDeletePackages = false;
                        this.CreatePackagesFromExcel();
                    }

                    if (this.chooseShipmentPackageFromWarehouseReleasePackages) {
                        this.chooseShipmentPackageFromWarehouseReleasePackages = false;
                        this.OpenChooseShipmentPackageFromWarehouseReleasePackagesWindow();
                    }

                    if (this.IsDisconnectWarehouseReleasePackage) {
                        this.IsDisconnectWarehouseReleasePackage = false;
                        this.EnableWarehouseRelaseForUse();
                    }


                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                    this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                    this.DirectionId = this.EntityPM.DirectionId;

                    this.OnResourcesReady();
                    //this.SetUIProperties();
                    //this.SetGenerateData();
                    //this.BuildItemsSource();
                }
            });

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "SHPK" || tabCode == "JHPK") {
                    this.SetUIProperties();
                }
            });


            this.CrossDockReleasesEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "CrossDockReleases") {
                    this.SetGenerateData();
                }
            });
        }
    }

    private ChargeableWeightPasted: boolean = false;
    ChargeableWeightPaste($event) {
        this.ChargeableWeightPasted = true;
    }

    private GrossWeightPasted: boolean = false;
    GrossWeightPaste($event) {
        this.GrossWeightPasted = true;
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
        AppTool.KillEventEmitter(this.TabSelectedEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
        AppTool.KillEventEmitter(this.CrossDockReleasesEvent);        
    }

    ngOnInit() {
        if (this.EntityPM != null) {
            this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);

            if (FeatureLocator.HasFeaturePermession("Shipment", "DELETEPACKAGES")) {
                this.IsDeletePackagesButtonVisible = true;
            }

            if (this.IsFCLEntity && FeatureLocator.HasFeaturePermession("Shipment", "DOWNUPLPACAKGES")) {
                this.IsDownloadUploadPackagesVisible = true;
            }

            if (FeatureLocator.HasFeaturePermession("Shipment", "COMMODITYNUMBER")) {
                this.IsCommodityNumberVisible = true;
            }

            if (FeatureLocator.HasFeaturePermession("Shipment", "COMMODITYNAME")) {
                this.IsCommodityNameVisible = true;
            }

            if (FeatureLocator.HasFeaturePermession("Shipment", "RELEASENUMBER") && this.DirectionId != "I") {
                this.IsShowReleaseNumber = true;
            }

            this.CheckHorseVisiblility();

            if (this.IsFCLEntity) {
                this.entityResourceService.getEntityResourceByTableName("ShipmentPackage").subscribe((res1: any) => {
                    this.entityResourceService.getEntityResourceByTableName("ShipmentPackageItem").subscribe((res2: any) => {
                        this.entityResourceService.getEntityResourceByTableName("InsideShipmentPackage").subscribe((res3: any) => {
                            this.IsResourcesReady = true;
                            this.OnResourcesReady();
                        });
                    });
                });
            }

            else {
                this.entityResourceService.getEntityResourceByTableName("ShipmentPackage").subscribe((res1: any) => {
                    this.entityResourceService.getEntityResourceByTableName("ShipmentPackageItem").subscribe((res2: any) => {
                        this.IsResourcesReady = true;
                        this.OnResourcesReady();
                    });
                });
            }
        }
    }
    private CheckHorseVisiblility() {
        var service: ShipmentSubTypeListService = new ShipmentSubTypeListService();
        service.getSingleFromCache(this.EntityPM.ShipmentSubTypeId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var mySubType = myResponse.Result;

                var FeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "HRS" && d.TenantNumber == SessionLocator.Tenant)[0];
                if (FeatureToggle && mySubType && mySubType.Code == "HORSE") {
                    this.HorseFieldIsVisible = true;
                }
            }
        });       
    }

    public IsGroupageEntity: boolean = false;
    public IsInsideButtonVisible: boolean = false;
    private AllPackageTypes: PackageTypeList[] = [];
    OnResourcesReady() {
        var isInsideButtonVisible = true;

        if (this.IsFCLEntity) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.ShipmentTypeId)) {
                var myShipmentTypeId = this.EntityPM.ShipmentTypeId.toUpperCase();
                if (myShipmentTypeId.indexOf("MYG") > -1) {
                    this.IsGroupageEntity = true;
                    isInsideButtonVisible = false;
                }
            }
            
            if (FeatureLocator.HasFeaturePermession("Shipment", "Area.ContainersFU")) {
                this.IsContainersFUVisible = true;
            }
        }
        else {
            isInsideButtonVisible = false
        }

        this.IsInsideButtonVisible = isInsideButtonVisible;

        this.SetLabels();
        this.SetUIProperties();
        this.SetGenerateData();
        this.BuildItemsSource();

        this.CurrentSession.SessionEvent.subscribe(s => {
            if (s == "UpdatePackagesTab") {
                this.SetGenerateData();
            }
        });

        if (this.IsEditingEnabled) {
            var myService: PackageTypeListService = new PackageTypeListService();
            myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.AllPackageTypes = myResponse.Result;
                }
            });           
        }

        if (this.IsFCLEntity) {
            if (FeatureLocator.HasFeaturePermession("Shipment", "ShippingInstructions")) {
                if (this.EntityPM.TransportModeId == "O" && (this.EntityPM.DirectionId == "E" || this.EntityPM.DirectionId == "I")) {
                    if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "C") {
                        this.IsShippingInstructionsVisible = true;
                    }

                    else if (this.EntityPM.ShipmentLevelCode == "H" && this.EntityPM.MasterShipmentDataId != null) {
                        this.IsShippingInstructionsVisible = true;
                    }
                }
            }
        }
    }

    // Labels
    public AddButtonLabel: string;
    public VolumeColumnHeader: string;
    public WeightColumnHeader: string;
    public DimensionsColumnHeader: string;
    public VolumetricWeightColumnHeader: string;
    public ChargeableWeightUnitCodeLabel: string;
    public PackageTypeColumnHeader: string;
    public ChargeableWeightLabel: string;
    public VolumeLabel: string;
    public GrossWeightLabel: string;
    public VolumetricWeightLabel: string;
    public QuantityLabel: string;
    SetLabels() {

        if (this.EntityPM.TransportModeId == "A") {
            this.ChargeableWeightUnitCodeLabel = TextCodeTranslator.Translate("Shipment.F.ChargeableWeightUnitCode");
        }

        else {
            this.ChargeableWeightUnitCodeLabel = TextCodeTranslator.Translate("Shipment.F.ChargeableWeightUnitCode.Short");
        }

        if (this.IsLCLEntity) {
            this.AddButtonLabel = TextCodeTranslator.Translate("Shipment.B.Packages.AddPackage");
            this.PackageTypeColumnHeader = TextCodeTranslator.Translate("ShipmentPackage.F.PackageTypeId");
            this.QuantityLabel = TextCodeTranslator.Translate("Shipment.F.NumberOfPackages");
        }

        else {
            this.AddButtonLabel = TextCodeTranslator.Translate("Shipment.B.Packages.AddContainer");
            if(this.EntityPM.TransportModeId == "I")
                this.AddButtonLabel = TextCodeTranslator.Translate("Shipment.B.Packages.AddFullTruckLoad");
            this.PackageTypeColumnHeader = TextCodeTranslator.Translate("ShipmentPackage.F.ContainerTypeId");
            this.QuantityLabel = TextCodeTranslator.Translate("Shipment.F.NumberOfContainers");
        }

        this.SetAttachedLabels();
    }
    SetAttachedLabels() {
        if (this.EntityPM.TransportModeId == "A") {
            this.ChargeableWeightLabel = TextCodeTranslator.Translate("Shipment.F.ChargeableWeight").replace('%ChargWeightCode', this.ChargeableWeightUnitCode);
        }

        else {
            this.ChargeableWeightLabel = TextCodeTranslator.Translate("Shipment.F.ChargeableWeight.Short").replace('%ChargWeightCode', this.ChargeableWeightUnitCode);
        }

        this.VolumeColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.Volume").replace("%UnitCode", this.EntityPM.VolumeUnitCode);
        this.WeightColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.GrossWeight").replace("%UnitCode", this.EntityPM.GrossWeightUnitCode);
        this.DimensionsColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.Dimensions").replace("%UnitCode", this.EntityPM.DimensionsUnitCode);
        this.VolumetricWeightColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.VolWeight").replace("%UnitCode", this.EntityPM.ChargeableWeightUnitCode);
        this.VolumeLabel = TextCodeTranslator.Translate("Shipment.F.Volume").replace('%VolumeCode', this.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate("Shipment.F.GrossWeight").replace('%GrossWeightCode', this.GrossWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator.Translate("Shipment.F.VolumetricWeight").replace('%ChargWeightCode', this.ChargeableWeightUnitCode);
    }

    // SetUIProperties
    public IsEditingEnabled: boolean = true;
    public IsTotalsFieldEnabled: boolean = true;
    public IsAddInsideButtonEnabled: boolean = false;
    SetUIProperties() {

        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);

        this.UIProperties.SetEnabled("DimensionsUnitCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("GrossWeightUnitCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ChargeableWeightUnitCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Ratio", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DimFactor", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DescriptionOfGoods", this.ObjectTableName, this.IsEditingEnabled);

        var isTotalsFieldEnabled = false;
        if (this.IsEditingEnabled) {
            if (this.EntityPM.ShipmentPackages.length > 0) {
                isTotalsFieldEnabled = true;
            }
        }

        this.IsTotalsFieldEnabled = isTotalsFieldEnabled;
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isTotalsFieldEnabled);
        this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, isTotalsFieldEnabled);
        this.UIProperties.SetEnabled("IsDangerous", this.ObjectTableName, isTotalsFieldEnabled);
        this.UIProperties.SetEnabled("AWBCommodityItemNumber", this.ObjectTableName, isTotalsFieldEnabled);
        this.SetUIProperties_DimFactor();
        this.SetUIProperties_DimensionsUnitCode();
    }
    SetUIProperties_InsideButton() {
        var isButtonEnabled = false;

        if (this.IsEditingEnabled) {
            if (this.IsFCLEntity) {
                if (this.SelectedRow != null) {
                    if (!this.SelectedRow.IsConnectedToRouting) {
                        isButtonEnabled = true;
                    }
                }
            }
        }

        this.IsAddInsideButtonEnabled = isButtonEnabled;
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

    BuildItemsSource() {
        var itemsCollection: ShipmentPackageItem[] = [];

        this.EntityPM.ShipmentPackages.forEach((item) => {
            itemsCollection.push(new ShipmentPackageItem(item, this));
        })

        this.ItemsSource.InsertCollection(itemsCollection);
        this.SetGenerateData();
    }

    // Measurments
    MeasurmentsButtonToolTip: string = TextCodeTranslator.Translate("Shipment.B.Packages.MeasurmentsSettings");
    IsMeasurmentsHidden: boolean = true;
    MeasurmentsSettingsClicked() {
        this.IsMeasurmentsHidden = !this.IsMeasurmentsHidden;

        if (this.IsMeasurmentsHidden) {
            this.MeasurmentsButtonToolTip = TextCodeTranslator.Translate("Shipment.B.Packages.HideMeasurmentsSettings");
        }

        else {
            this.MeasurmentsButtonToolTip = TextCodeTranslator.Translate("Shipment.B.Packages.MeasurmentsSettings");
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

    get DimensionsUnitCode() { return this.EntityPM.DimensionsUnitCode; }
    set DimensionsUnitCode(newValue: string) {
        if (this.EntityPM.DimensionsUnitCode != newValue) {
            this.EntityPM.DimensionsUnitCode = newValue;

            this.ComputeDimFactor();
            this.SetUIProperties_DimFactor();
            this.OnMeasurmentsSettingsChanged();
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
            this.ComputeChargeableWeight_Kg();

        }
    }

    get Ratio() { return this.EntityPM.Ratio; }
    set Ratio(newValue: number) {
        if (this.EntityPM.Ratio != newValue) {
            this.EntityPM.Ratio = newValue;

            this.ComputeDimFactor();
            ShipmentTool.OnShipmentRatioChanged(this.EntityPM);
        }
    }

    get DimFactor() { return this.EntityPM.DimFactor; }
    set DimFactor(newValue: number) {
        if (this.EntityPM.DimFactor != newValue) {
            this.EntityPM.DimFactor = newValue;

            this.EntityPM.Ratio = AppTool.GetRatioFromDimFactor(this.DimFactor, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
            ShipmentTool.OnShipmentRatioChanged(this.EntityPM);
        }
    }

    ComputeDimFactor() {
        this.EntityPM.DimFactor = AppTool.GetDimFactorFromRatio(this.Ratio, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
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
    private ComputeChargeableWeight_Kg() {
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
    
    OnMeasurmentsSettingsChanged() {
        this.SetAttachedLabels();
        ShipmentTool.RecalculateShipmentFields(this.EntityPM);
    }
    SetUIProperties_DimFactor() {
        var isDimFactorVisibile: boolean = false;

        if (!AppTool.IsNullOrEmpty(this.DimensionsUnitCode)) {
            if (this.DimensionsUnitCode.toUpperCase() == "INC") {
                isDimFactorVisibile = true;
            }
        }

        this.UIProperties.SetVisibility("DimFactor", this.ObjectTableName, isDimFactorVisibile);
    }

    // Summary
    get TEU() { return this.EntityPM.TEU == null ? 0 : this.EntityPM.TEU; }
    set TEU(newValue: number) {
        if (this.EntityPM.TEU != newValue) {
            this.EntityPM.TEU = AppTool.Round(newValue, 3);
        }
    }

    get Volume() { return this.EntityPM.Volume == null ? 0 : this.EntityPM.Volume; }
    set Volume(newValue: number) {
        if (this.EntityPM.Volume != newValue) {
            this.EntityPM.Volume = AppTool.Round(newValue, 3);
            this.ComputeVolume_CBM();
        }
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
        }
    }

    get GrossWeightPerStorageDays() { return this.EntityPM.GrossWeightPerStorageDays == null ? 0 : this.EntityPM.GrossWeightPerStorageDays; }
    set GrossWeightPerStorageDays(newValue: number) {
        if (this.EntityPM.GrossWeightPerStorageDays != newValue) {
            this.EntityPM.GrossWeightPerStorageDays = AppTool.Round(newValue, 3);
        }
    }

    private ComputeGrossWeight_PerStorageDays() {
        var StorageDays = DateTool.GetDaysBetweenDates(this.EntityPM.WarehouseLegActualReleaseDate, this.EntityPM.WarehouseLegActualEntryDate);
        var weightPerStorageDays;
        if (this.EntityPM.TransportModeId != "A") {
            weightPerStorageDays = Math.ceil(this.EntityPM.GrossWeightPerTon) * (StorageDays - this.EntityPM.WarehouseStorageFreeDays);
        }
        else {
            weightPerStorageDays = this.EntityPM.ChargeableWeight * (StorageDays - this.EntityPM.WarehouseStorageFreeDays);
        }

        this.GrossWeightPerStorageDays = weightPerStorageDays < 0 ? 0 : weightPerStorageDays;

        this.CheckStorageReceivableAmount();
    }
    private CheckStorageReceivableAmount() {
        var storageReceivable: ShipmentReceivablePM = this.EntityPM.ShipmentReceivables.filter(d => d.ChargesTypeCode == "ISTOR" && d.MeasurementCode == "STFE" && AppTool.IsNullOrEmpty(d.ARInvoiceId))[0];

        if (storageReceivable) {
            var storageDays: number = null;
            if (this.EntityPM.WarehouseLegActualEntryDate != null && this.EntityPM.WarehouseLegActualReleaseDate != null) {
                if (DateTool.GetDateFromDate(this.EntityPM.WarehouseLegActualReleaseDate) >= DateTool.GetDateFromDate(this.EntityPM.WarehouseLegActualEntryDate)) {
                    var days = DateTool.GetDaysBetweenDates(this.EntityPM.WarehouseLegActualEntryDate, this.EntityPM.WarehouseLegActualReleaseDate);
                    storageDays = days;
                }
            }

            var amount: number = ShipmentTool.ComputeImportStorageReceivableAmount(storageDays, this.EntityPM);

            storageReceivable.TotalAmount = amount;
            storageReceivable.TotalAmountLocal = AppTool.Round(storageReceivable.TotalAmount * storageReceivable.Rate, 2);

            if (storageReceivable.CurrencyId == this.EntityPM.ProfitCurrencyId) {
                storageReceivable.AmountInProfitCurrency = storageReceivable.TotalAmount;
            }

            else {
                storageReceivable.AmountInProfitCurrency = (storageReceivable.TotalAmountLocal / storageReceivable.ProfitCurrencyExchangeRate);
            }
        }
    }

    get NumberOfPackages() {

        var myResult: number = 0;

        if (this.IsFCLEntity) {
            myResult = this.EntityPM.NumberOfContainers == null ? 0 : this.EntityPM.NumberOfContainers;
        }

        else {
            myResult = this.EntityPM.NumberOfPackages == null ? 0 : this.EntityPM.NumberOfPackages;
        }

        return myResult;
    }
    set NumberOfPackages(value: number) {
        if (this.IsFCLEntity) {
            if (this.EntityPM.NumberOfContainers != value) {
                this.EntityPM.NumberOfContainers = value;
            }
        }

        else {
            if (this.EntityPM.NumberOfPackages != value) {
                this.EntityPM.NumberOfPackages = value;
            }
        }
    }

    get GrossWeight() { return this.EntityPM.GrossWeight == null ? 0 : this.EntityPM.GrossWeight; }
    set GrossWeight(newValue: number) {
        if (this.EntityPM.GrossWeight != newValue) {
            this.EntityPM.GrossWeight = AppTool.Round(newValue, 3);
            this.ComputeGrossWeigh_Kg_Ton();
            this.ComputeGrossWeight_PerStorageDays();
        }
    }

    get ChargeableWeight() { return this.EntityPM.ChargeableWeight == null ? 0 : this.EntityPM.ChargeableWeight; }
    set ChargeableWeight(newValue: number) {
        if (this.EntityPM.ChargeableWeight != newValue) {
            this.EntityPM.ChargeableWeight = AppTool.Round(newValue, 3);
            this.ComputeChargeableWeight_Kg();
            this.ComputeGrossWeight_PerStorageDays();
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

    get AWBCommodityItemNumber() { return this.EntityPM.AWBCommodityItemNumber; }
    set AWBCommodityItemNumber(newValue: string) {
        if (this.EntityPM.AWBCommodityItemNumber != newValue) {
            this.EntityPM.AWBCommodityItemNumber = newValue;
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

    private setDigits() {
        this.firstDigit = ",";
        this.secondDigit = ".";
        //switch (SessionLocator.TenantPM.NumberFormatCode) {
        //    case "CD": {
        //        this.firstDigit = ",";
        //        this.secondDigit = ".";
        //        break;
        //    }

        //    case "DC": {
        //        this.firstDigit = ".";
        //        this.secondDigit = ",";
        //        break;
        //    }

        //    case "AD": {
        //        this.firstDigit = "'";
        //        this.secondDigit = ".";
        //        break;
        //    }

        //    default:
        //        {
        //            this.firstDigit = ",";
        //            this.secondDigit = ".";
        //            break;
        //        }
        //}
    }

    GrossWeightLostFocus(input: any) {        

        var valueComputed: number = 0;
        var valueInserted: number = 0;

        this.EntityPM.ShipmentPackages.forEach((item) => {
            if (!AppTool.IsNullOrEmpty(item.Weight)) {
                valueComputed += item.Weight;
            }
        });

        // if (!AppTool.IsNullOrEmpty(input)) {
        //     input = AppTool.Replace(input, ",", "");
        //     valueInserted = Number(input);
        // }

        valueComputed = valueComputed == null ? 0 : valueComputed;
        valueInserted = AppTool.GetNumberFromText(input);

        if (valueComputed != valueInserted) {
            this.GrossWeightEdited = true;
            this.GrossWeight = valueInserted;
            this.ComputeTotals();
        }
    }
    ChargeableWeightLostFocus(input: any) {

        var valueComputed: number = 0;
        var valueInserted: number = 0;

        valueComputed = AppTool.CalculateChargeableWeight(this.EntityPM.GrossWeight, this.EntityPM.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);

        if (!AppTool.IsNullOrEmpty(input)) {
            valueInserted = AppTool.GetNumberFromText(input);
            valueInserted = AppTool.RoundChargeableWeight(valueInserted, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
        }

        valueComputed = valueComputed == null ? 0 : valueComputed;
        valueInserted = valueInserted == null ? 0 : valueInserted;
        if (valueComputed != valueInserted) {
            this.ChargeableWeightEdited = true;
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
    ComputeTotals() {

        if (this.EntityPM.ShipmentPackages.length == 0) {
            this.TEU = null;
            this.NumberOfPackages = null;
            this.GrossWeight = null;
            this.Volume = null;
            this.VolumetricWeight = null;
            this.ChargeableWeight = null;
            this.AWBCommodityItemNumber = null;
            this.GrossWeightEdited = false;
            this.ChargeableWeightEdited = false;
        }

        else {
            var myTEU: number = 0;
            var myQuantity: number = 0;
            var myVolume: number = 0;
            var myGrossWeight: number = 0;
            var myVolumetricWeight: number = 0;

            this.EntityPM.ShipmentPackages.forEach((item) => {

                if (item.Quantity != null) {
                    myQuantity += item.Quantity;
                }

                if (item.Volume != null) {
                    myVolume += item.Volume;
                }

                if (item.VolumetricWeight != null) {
                    myVolumetricWeight += item.VolumetricWeight;
                }

                if (item.Weight != null) {
                    myGrossWeight += item.Weight;
                }

                if (!AppTool.IsNullOrEmpty(item.PackageTypeId)) {
                    var myPackageType: PackageTypeList = this.AllPackageTypes.filter(f => f.Id == item.PackageTypeId)[0];
                    if (myPackageType) {
                        if (myPackageType.TEU) {
                            myTEU += myPackageType.TEU;
                        }
                    }
                }
            });

            this.TEU = myTEU;
            this.NumberOfPackages = myQuantity;
            this.Volume = AppTool.Round(myVolume, 3);
            this.VolumetricWeight = AppTool.Round(myVolumetricWeight, 3);

            if (!this.GrossWeightEdited) {
                this.GrossWeight = AppTool.Round(myGrossWeight, 3);
            }

            if (!this.ChargeableWeightEdited) {
                this.ChargeableWeight = AppTool.CalculateChargeableWeight(this.EntityPM.GrossWeight, this.EntityPM.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
            }
        }

        this.SetUIProperties();
        ShipmentTool.OnShipmentQuantitiesChanged(this.EntityPM);
    }

    // Generate
    public IsGenerateControlVisible: boolean = false;
    public GenerateButtonLabel: string;
    public IsGenerateButtonVisible: boolean = false;
    public IsGenerateButtonEnabled: boolean = false;
    public BuildButtonLabel: string;
    public IsBuildButtonVisible: boolean = false;
    public IsBuildButtonEnabled: boolean = false;
    public IsNoPackagesLoadedTextVisible: boolean = false;
    public IsRebuildButtonVisible: boolean = false;
    IsGeneratePackagesfromCrossDockReleasesButtonVisible: boolean = false;
    //IsGeneratePackagesfromCrossDockReleasesButtonEnabled: boolean = false;
    GenerateCrossDockReleasesButtonLabel: string;

    GeneratePackagesfromCrossDockReleasesButtonClicked() {

        if (!this.IsLCLEntity) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show("Only Container Packages can be added to your shipment packages");
            confirmWindow.NoButtonText = "Cancel";
            confirmWindow.YesButtonText = "Add";
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.GeneratePackagesfromCrossDockReleases();
                }
            });

        } else this.GeneratePackagesfromCrossDockReleases();

    

    }

    GeneratePackagesfromCrossDockReleases() {

        if (this.EntityPM.IsDirty) {
            this.chooseShipmentPackageFromWarehouseReleasePackages = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
        else this.OpenChooseShipmentPackageFromWarehouseReleasePackagesWindow();
    }

    OpenChooseShipmentPackageFromWarehouseReleasePackagesWindow() {

        var windowArgs: any = {};

        windowArgs.ViewModelTrigger = this;
        windowArgs.IsContainer = !this.IsLCLEntity;
        var logWindow = new LogitudeWindow();
 
        logWindow.Width = !windowArgs.IsContainer ? 1200:1130;
        logWindow.Height = 550;
        logWindow.Title = "Choose Packages";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Warehouse/Components/ChoosePackagesFromWarehousePackageReleasesComponent");

    }
    
    SetGenerateData() {
        this.IsGenerateControlVisible = this.EntityPM.ShipmentPackages.length == 0 ? true : false;
        if (this.IsGenerateControlVisible) {

            var count = 0;
            if (this.EntityPM.BookingNumberOfPackages) {
                count = this.EntityPM.BookingNumberOfPackages;
            }

            if (this.EntityPM.ShipmentLevelCode == "C") {
                this.BuildButtonLabel = "Build From " + this.EntityPM.ShipmentConsoleShipments.length + " Shipments";
                this.IsBuildButtonVisible = this.EntityPM.ShipmentConsoleShipments.length > 0 ? true : false;
                this.IsBuildButtonEnabled = true;
            }

            else {
                this.GenerateButtonLabel = TextCodeTranslator.Translate("Shipment.O.GenerateFromOrderPackages").replace("%Number", count.toString());
                this.IsGenerateButtonVisible = true;
                this.IsGenerateButtonEnabled = this.EntityPM.BookingNumberOfPackages > 0 ? true : false;
            }
        }

        var isRebuildButtonVisible = false;
        if (this.EntityPM.ShipmentLevelCode == "C") {
            if (this.EntityPM.ShipmentConsoleShipments.length > 0) {
                if (this.EntityPM.ShipmentPackages.length > 0) {
                    isRebuildButtonVisible = true;
                }
            }
        }

        this.IsRebuildButtonVisible = isRebuildButtonVisible;

        if (!this.IsEditingEnabled) {
            this.IsGenerateButtonEnabled = false;
            //this.IsGeneratePackagesfromCrossDockReleasesButtonEnabled = false;
        }

        //Cross Dock
        this.IsGeneratePackagesfromCrossDockReleasesButtonVisible = false;
        if (FeatureLocator.HasFeaturePermession("General", "CROSSDOCKS")) {
            if (this.IsGenerateButtonVisible && this.IsEditingEnabled) {
                if ((this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") && this.EntityPM.DirectionId !="I") {
                    this.GenerateCrossDockReleasesButtonLabel = "Generate from Cross Dock Releases Packages";
                    this.IsGeneratePackagesfromCrossDockReleasesButtonVisible = true;
                    //var count: number = 0;
                    //var warehouseReleasePackageListExtendedService: WarehouseReleasePackageListExtendedService = new WarehouseReleasePackageListExtendedService();
                    //warehouseReleasePackageListExtendedService.getCheckIfShipmentHasReleasePackages(this.EntityPM.Id, this.EntityPM.Tenant).subscribe((myResponse: ServiceResponse) => {
                    //    if (!myResponse.HasError) {
                    //        var result: any = myResponse.Result;
                    //        if (result == true) {
                    //            this.IsGeneratePackagesfromCrossDockReleasesButtonEnabled = true;
                    //        }
                    //    }
                    //});

                }
            }
        }

    }
    GenerateButtonClicked() {
        if (this.IsLCLEntity) {
            this.Generate_LCL();
        }

        else {
            this.Generate_FCL();
        }
        this.RefreshPackages();
     
    }
    
    Generate_LCL() {
        if (this.EntityPM.ShipmentOrderPackages.length > 0) {
            this.EntityPM.ShipmentOrderPackages.forEach(item => {
                var itemPM = new ShipmentPackagePM(null);
                itemPM.Tenant = this.EntityPM.Tenant;
                itemPM.ShipmentId = this.EntityPM.Id;
                itemPM.Quantity = item.Quantity;
                itemPM.Weight = item.GrossWeight;
                itemPM.Volume = item.Volume;
                itemPM.VolumetricWeight = item.VolumetricWeight;
                itemPM.PackageTypeId = item.PackageTypeId;
                itemPM.PackageTypeName = item.PackageTypeName;
                itemPM.Width = item.Width;
                itemPM.Length = item.Length;
                itemPM.Height = item.Height;
                this.EntityPM.AddPackage(itemPM);

            });
        }

        else if (this.EntityPM.BookingNumberOfPackages > 0) {
            var itemPM = new ShipmentPackagePM(null);
            itemPM.Tenant = this.EntityPM.Tenant;
            itemPM.ShipmentId = this.EntityPM.Id;
            itemPM.Quantity = this.EntityPM.BookingNumberOfPackages;
            itemPM.Weight = this.EntityPM.OrderGrossWeight;
            itemPM.Volume = this.EntityPM.BookingVolume;
            itemPM.VolumetricWeight = this.EntityPM.OrderVolumetricWeight;
            this.EntityPM.AddPackage(itemPM);
        }
    }
    Generate_FCL() {
        this.EntityPM.ShipmentOrderPackages.forEach(item => {
            var count = item.Quantity;
            var i = 1;
            while (i <= count) {
                var itemPM = new ShipmentPackagePM(null);
                itemPM.Tenant = this.EntityPM.Tenant;
                itemPM.ShipmentId = this.EntityPM.Id;
                itemPM.Quantity = 1;
                itemPM.Weight = item.GrossWeight;
                itemPM.Volume = item.Volume;
                itemPM.VolumetricWeight = item.VolumetricWeight;
                itemPM.PackageTypeId = item.PackageTypeId;
                itemPM.PackageTypeName = item.PackageTypeName;
                itemPM.IsContainer = item.IsContainer;
                this.EntityPM.AddPackage(itemPM);
                i++;
            }
        });
    }
    GeneratePackagesFromWarehouseReleasesPackages(allPackages: any[]) {
        if (allPackages) {
            allPackages.forEach(item => {
                var newPackage = new ShipmentPackagePM(null);
                newPackage.ShipperSeal = item.ShipperSeal;
                newPackage.Tenant = item.Tenant;
                newPackage.ShipmentId = this.EntityPM.Id;
                newPackage.ContainerNumber = item.ContainerNumber;
                newPackage.Description = item.Description;
                newPackage.Harmonize = item.Harmonize;
                newPackage.Height = item.Height;
                newPackage.IsContainer = item.IsContainer;
                newPackage.Length = item.Length;
                newPackage.PackageTypeId = item.PackageTypeId;
                newPackage.PackageTypeName = item.PackageTypeName;
                newPackage.Quantity = item.Quantity;
                newPackage.Volume = item.Volume;
                newPackage.Weight = item.Weight;
                newPackage.Width = item.Width;
                newPackage.WarehouseReleaseNumber = item.ReleaseNumber;
                newPackage.WarehouseReleaseId = item.WarehouseReleaseId;
                this.EntityPM.AddPackage(newPackage);
            });

            this.RefreshPackages();
        }
    }


    WarehouseReleaseNumber: string;

    DisconnectWarehouseReleasePackageButtonClick(item: any) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("All packages connected to the release you are disconnecting will be deleted ?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.DisconnectWarehouseReleasePackage(item);
            }
        });
    }


    DisconnectWarehouseReleasePackage(item: any) {
        this.WarehouseReleaseNumber = item.WarehouseReleaseNumber;
        this.IsDisconnectWarehouseReleasePackage = true;

        this.ItemsSource.Collection.filter(d => d.WarehouseReleaseNumber == item.WarehouseReleaseNumber).forEach((item) => {
            this.DeletePackage(item);
        });

        this.DisconnectWarehouseReleaseOnShipment(item);
        this.CurrentSession.CurrentEditComponent.SaveChanges();


    }

    EnableWarehouseRelaseForUse() {
        if (!AppTool.IsNullOrEmpty(this.WarehouseReleaseNumber)) {
            this.CurrentSession.StartBusyIndicator("Saving...");
            if (this.warehouseReleasePMExtendedService == null) this.warehouseReleasePMExtendedService = new WarehouseReleasePMExtendedService();
            this.warehouseReleasePMExtendedService.EnableWarehouseRelaseForUse(this.WarehouseReleaseNumber, this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
            });
        }
    }


    RefreshPackages() {

        this.BuildItemsSource();
        this.ComputeTotals();
        this.EntityPM.IsDangerous = this.EntityPM.OrderIsDangerouseGoods;

        if (this.IsLCLEntity) {
            if (this.ItemsSource.Length > 0) {
                if (this.EntityPM.OrderGrossWeight != null) {
                    if (this.EntityPM.OrderGrossWeight != this.GrossWeight) {
                        this.GrossWeight = this.EntityPM.OrderGrossWeight;
                        this.GrossWeightEdited = true;
                    }
                }

                if (this.EntityPM.OrderChargeableWeight != null) {
                    if (this.EntityPM.OrderChargeableWeight != this.ChargeableWeight) {
                        this.ChargeableWeight = this.EntityPM.OrderChargeableWeight;
                        this.ChargeableWeightEdited = true;
                    }
                }
            }
        }
    }
       
    BuildButtonClicked() {

        this.CurrentSession.StartBusyIndicatorLoading();

        var myDomainService = new ShipmentDomainService();

        myDomainService.GetShipmentConsolidationPackages(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();

            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var allPackages: ShipmentPackagePM[] = myResponse.Result;

                    if (allPackages.length == 0) {
                        this.IsNoPackagesLoadedTextVisible = true;
                    }

                    else {
                        this.IsNoPackagesLoadedTextVisible = false;

                        if (this.IsGroupageEntity) {
                            var logWindow = new LogitudeWindow();
                            logWindow.WindowArgs = { FatherComponent: this, AllPackages: allPackages };
                            logWindow.Title = "Build Master Packages";
                            logWindow.IsFillScreen = true;
                            logWindow.Show("./ShipmentModules/ShipmentTabs/Components/Windows/Groupage/GroupageComponent");
                            logWindow.WindowClosed.subscribe(s => {
                                if (s) {
                                    this.BuildItemsSource();
                                    this.ComputeTotals();
                                }
                            });
                        }

                        else {
                            this.BuildPackagesFromList(allPackages);
                        }
                    }
                }
            }
        });
    }
    RebuildButtonClicked() {
        var confirmWindow = new ConfirmWindow();

        if (this.EntityPM.ShipmentPackages.filter(d => !AppTool.IsNullOrEmpty(d.LastStatusCode)).length > 0) {
            confirmWindow.Show("Note that this will result in deleting container level statuses. Rebuild Packages?");
        }

        else {
            confirmWindow.Show("Rebuild Packages?");
        }
        
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                if (this.EntityPM.ShipmentPackages.length > 0) {
                    this.EntityPM.ShipmentPackages = [];
                    this.EntityPM.IsDirty = true;
                    this.BuildItemsSource();
                    this.ComputeTotals();
                }
                
                this.BuildButtonClicked();
            }
        });
    }
    BuildPackagesFromList(allPackages: ShipmentPackagePM[]) {

        if (this.TransportModeId == "A") {
            allPackages.forEach(item => {
                if (!AppTool.IsNullOrEmpty(item.HorseId)) {
                    var newPackage = new ShipmentPackagePM(null);
                    newPackage.ShipmentId = this.EntityPM.Id;
                    newPackage.ClassNumber = item.ClassNumber;
                    newPackage.ContainerNumber = item.ContainerNumber;
                    newPackage.Description = item.Description;
                    newPackage.FlashPoint = item.FlashPoint;
                    newPackage.Harmonize = item.Harmonize;
                    newPackage.Height = item.Height;
                    newPackage.IMDGCode = item.IMDGCode;
                    newPackage.FlashPointTemperatureUnitCode = item.FlashPointTemperatureUnitCode;
                    newPackage.IsContainer = item.IsContainer;
                    newPackage.IsDangerous = item.IsDangerous;
                    newPackage.Length = item.Length;
                    newPackage.MarksAndNumbers = item.MarksAndNumbers;
                    newPackage.MaterialDescription = item.MaterialDescription;
                    newPackage.PackageTypeId = item.PackageTypeId;
                    newPackage.PackageTypeName = item.PackageTypeName;
                    newPackage.PackagingGroup = item.PackagingGroup;
                    newPackage.Quantity = item.Quantity;
                    newPackage.ShipperSeal = item.ShipperSeal;
                    newPackage.CarrierSeal = item.CarrierSeal;
                    newPackage.SOC = item.SOC;
                    newPackage.Tare = item.Tare;
                    newPackage.Temperature = item.Temperature;
                    newPackage.Tenant = item.Tenant;
                    newPackage.UnNumber = item.UnNumber;
                    newPackage.Ventilation = item.Ventilation;
                    newPackage.Volume = item.Volume;
                    newPackage.VolumetricWeight = item.VolumetricWeight;
                    newPackage.Weight = item.Weight;
                    newPackage.Width = item.Width;
                    newPackage.OriginalShipmentPackageId = item.Id;
                    newPackage.Reference1 = item.Reference1;
                    newPackage.Reference2 = item.Reference2;
                    newPackage.Reference3 = item.Reference3;
                    newPackage.Reference4 = item.Reference4;
                    newPackage.CommodityNumber = item.CommodityNumber;
                    newPackage.CommodityName = item.CommodityName;
                    newPackage.HorseId = item.HorseId;
                    newPackage.HorseName = item.HorseName;
                    this.EntityPM.AddPackage(newPackage);
                }

                else {
                    var matchedItem = this.EntityPM.ShipmentPackages.filter(f => AppTool.IsNullOrEmpty(f.HorseId) && f.Height == item.Height && f.Width == item.Width && f.Length == item.Length && f.PackageTypeId == item.PackageTypeId)[0];
                    if (matchedItem != null) {

                        // Quantity
                        if (AppTool.IsNullOrEmpty(matchedItem.Quantity)) {
                            matchedItem.Quantity = item.Quantity;
                        }

                        else {
                            matchedItem.Quantity = matchedItem.Quantity + item.Quantity;
                        }

                        // Weight
                        if (AppTool.IsNullOrEmpty(matchedItem.Weight)) {
                            matchedItem.Weight = item.Weight;
                        }

                        else {
                            matchedItem.Weight = matchedItem.Weight + item.Weight;
                        }

                        // Volume
                        if (AppTool.IsNullOrEmpty(matchedItem.Width) || AppTool.IsNullOrEmpty(matchedItem.Height) || AppTool.IsNullOrEmpty(matchedItem.Length)) {
                            matchedItem.Volume = (matchedItem.Weight * this.EntityPM.Ratio) / 1000;
                            matchedItem.VolumetricWeight = matchedItem.Weight;
                        }

                        else {
                            matchedItem.Volume = (matchedItem.Width * matchedItem.Height * matchedItem.Length * matchedItem.Quantity) / 1000000;
                            matchedItem.VolumetricWeight = (matchedItem.Volume * 1000) / this.EntityPM.Ratio;
                        }
                    }

                    else {
                        var newPackage = new ShipmentPackagePM(null);
                        newPackage.ShipmentId = this.EntityPM.Id;
                        newPackage.ClassNumber = item.ClassNumber;
                        newPackage.ContainerNumber = item.ContainerNumber;
                        newPackage.Description = item.Description;
                        newPackage.FlashPoint = item.FlashPoint;
                        newPackage.Harmonize = item.Harmonize;
                        newPackage.Height = item.Height;
                        newPackage.IMDGCode = item.IMDGCode;
                        newPackage.FlashPointTemperatureUnitCode = item.FlashPointTemperatureUnitCode;
                        newPackage.IsContainer = item.IsContainer;
                        newPackage.IsDangerous = item.IsDangerous;
                        newPackage.Length = item.Length;
                        newPackage.MarksAndNumbers = item.MarksAndNumbers;
                        newPackage.MaterialDescription = item.MaterialDescription;
                        newPackage.PackageTypeId = item.PackageTypeId;
                        newPackage.PackageTypeName = item.PackageTypeName;
                        newPackage.PackagingGroup = item.PackagingGroup;
                        newPackage.Quantity = item.Quantity;
                        newPackage.ShipperSeal = item.ShipperSeal;
                        newPackage.CarrierSeal = item.CarrierSeal;
                        newPackage.SOC = item.SOC;
                        newPackage.Tare = item.Tare;
                        newPackage.Temperature = item.Temperature;
                        newPackage.Tenant = item.Tenant;
                        newPackage.UnNumber = item.UnNumber;
                        newPackage.Ventilation = item.Ventilation;
                        newPackage.Volume = item.Volume;
                        newPackage.VolumetricWeight = item.VolumetricWeight;
                        newPackage.Weight = item.Weight;
                        newPackage.Width = item.Width;
                        newPackage.OriginalShipmentPackageId = item.Id;
                        newPackage.Reference1 = item.Reference1;
                        newPackage.Reference2 = item.Reference2;
                        newPackage.Reference3 = item.Reference3;
                        newPackage.Reference4 = item.Reference4;
                        newPackage.CommodityNumber = item.CommodityNumber;
                        newPackage.CommodityName = item.CommodityName;
                        newPackage.HorseId = item.HorseId;
                        newPackage.HorseName = item.HorseName;
                        this.EntityPM.AddPackage(newPackage);
                    }
                }
            });
        }

        else {
            allPackages.forEach(item => {
                var newPackage = new ShipmentPackagePM(null);
                newPackage.ShipmentId = this.EntityPM.Id;
                newPackage.ClassNumber = item.ClassNumber;
                newPackage.ContainerNumber = item.ContainerNumber;
                newPackage.Description = item.Description;
                newPackage.FlashPoint = item.FlashPoint;
                newPackage.Harmonize = item.Harmonize;
                newPackage.Height = item.Height;
                newPackage.IMDGCode = item.IMDGCode;
                newPackage.FlashPointTemperatureUnitCode = item.FlashPointTemperatureUnitCode;                    
                newPackage.IsContainer = item.IsContainer;
                newPackage.IsDangerous = item.IsDangerous;
                newPackage.Length = item.Length;
                newPackage.MarksAndNumbers = item.MarksAndNumbers;
                newPackage.MaterialDescription = item.MaterialDescription;
                newPackage.PackageTypeId = item.PackageTypeId;
                newPackage.PackageTypeName = item.PackageTypeName;
                newPackage.PackagingGroup = item.PackagingGroup;
                newPackage.Quantity = item.Quantity;
                newPackage.ShipperSeal = item.ShipperSeal;
                newPackage.CarrierSeal = item.CarrierSeal;
                newPackage.SOC = item.SOC;
                newPackage.Tare = item.Tare;
                newPackage.Temperature = item.Temperature;
                newPackage.Tenant = item.Tenant;
                newPackage.UnNumber = item.UnNumber;
                newPackage.Ventilation = item.Ventilation;
                newPackage.Volume = item.Volume;
                newPackage.VolumetricWeight = item.VolumetricWeight;
                newPackage.Weight = item.Weight;
                newPackage.Width = item.Width;
                newPackage.OriginalShipmentPackageId = item.Id;
                newPackage.Reference1 = item.Reference1;
                newPackage.Reference2 = item.Reference2;
                newPackage.Reference3 = item.Reference3;
                newPackage.Reference4 = item.Reference4;
                newPackage.CommodityNumber = item.CommodityNumber;
                newPackage.CommodityName = item.CommodityName;

                item.InsideShipmentPackages.forEach(itemInside => {
                    var newInsidePackage = new InsideShipmentPackagePM(null);
                    newInsidePackage.Quantity = itemInside.Quantity;
                    newInsidePackage.Height = itemInside.Height;
                    newInsidePackage.Length = itemInside.Length;
                    newInsidePackage.Width = itemInside.Width;
                    newInsidePackage.Weight = itemInside.Weight;
                    newInsidePackage.PackageTypeId = itemInside.PackageTypeId;
                    newInsidePackage.PackageTypeName = itemInside.PackageTypeName;
                    newInsidePackage.Volume = itemInside.Volume;
                    newInsidePackage.VolumetricWeight = itemInside.VolumetricWeight;
                    newInsidePackage.Tenant = itemInside.Tenant;
                    newInsidePackage.Description = itemInside.Description;
                    newInsidePackage.OriginalInsideShipmentPackageId = itemInside.Id;
                    newInsidePackage.Reference1 = itemInside.Reference1;
                    newInsidePackage.Reference2 = itemInside.Reference2;
                    newInsidePackage.Reference3 = itemInside.Reference3;
                    newInsidePackage.Reference4 = itemInside.Reference4;
                    newInsidePackage.CommodityNumber = itemInside.CommodityNumber;
                    newInsidePackage.CommodityName = itemInside.CommodityName;
                    newInsidePackage.Harmonize = itemInside.Harmonize;
                    newPackage.AddInsideShipmentPackagePM(newInsidePackage);
                });

                this.EntityPM.AddPackage(newPackage);
            });
        }

        this.BuildItemsSource();
        this.ComputeTotals();
    }

    // Commands
    public SelectedRow: ShipmentPackageItem = null;
    OnRowSelected(itemComponent: ShipmentPackageItem) {
        this.SelectedRow = itemComponent;
        this.SetUIProperties_InsideButton();
    }
    OnRowLoaded(myRow: any) {


        if (myRow) {
            var isExpandaple = false;

            var item: ShipmentPackageItem = myRow.rowData;
            if (item) {
                item.Row = myRow;

                if (item.InsideItemsSource.length > 0) {
                    isExpandaple = true;
                }
            }

            myRow.SetExpandaple(isExpandaple);
        }
    }

    AddPackageClicked() {

        var logWindow = new LogitudeWindow();
        var itemPM = new ShipmentPackagePM(null);
        itemPM.NonActiveContainer = false;

        if (this.IsLCLEntity) {
            itemPM.IsContainer = false;
            itemPM.Tenant = SessionLocator.Tenant;
            logWindow.Title = TextCodeTranslator.Translate("ShipmentPackage.O.AddPackage");

        }

        else {
            itemPM.Quantity = 1;
            itemPM.IsContainer = true;
            itemPM.Tenant = SessionLocator.Tenant;
            itemPM.TemperatureUnitCode = SessionLocator.TenantPM.TemperatureUnitCode;
            itemPM.FlashPointTemperatureUnitCode = SessionLocator.TenantPM.TemperatureUnitCode;
            logWindow.Title = TextCodeTranslator.Translate("ShipmentPackage.O.AddContainer");
            if(this.EntityPM.TransportModeId == "I")
              logWindow.Title = TextCodeTranslator.Translate("ShipmentPackage.O.AddFullTruckLoad");
        }

        var myPath: string;
        if (this.TransportModeId == "A") {
            myPath = "./ShipmentModules/ShipmentPackages/Components/Packages/AddEditAirPackageComponent";
            logWindow.Height = 550;
        }

        else {
            myPath = "./ShipmentModules/ShipmentPackages/Components/Packages/AddEditOceanPackageComponent";
            logWindow.Width = 940;
            logWindow.Height = 610;
        }

        var itemComponent = new ShipmentPackageItem(itemPM, this, true);
        logWindow.DataContext = itemComponent;
        logWindow.Show(myPath);
    }
    EditPackageClicked(itemComponent: ShipmentPackageItem) {

        var logWindow = new LogitudeWindow();

        if (this.IsLCLEntity) {
            logWindow.Title = TextCodeTranslator.Translate("ShipmentPackage.O.EditPackage");
        }

        else {
            logWindow.Title = TextCodeTranslator.Translate("ShipmentPackage.O.EditContainer");
            if(this.EntityPM.TransportModeId == "I")
              logWindow.Title = TextCodeTranslator.Translate("ShipmentPackage.O.EditFullTruckLoad");
        }

        var myPath: string;
        if (this.TransportModeId == "A") {
            myPath = "./ShipmentModules/ShipmentPackages/Components/Packages/AddEditAirPackageComponent";
            itemComponent.CopyPackageItems();
            itemComponent.BuildPackageItems();
        }

        else {
            myPath = "./ShipmentModules/ShipmentPackages/Components/Packages/AddEditOceanPackageComponent";
            logWindow.Width = 940;
            logWindow.Height = 610;
        }

        logWindow.DataContext = itemComponent;
        logWindow.Show(myPath);
    }
    DeletePackageClicked(itemComponent: ShipmentPackageItem) {
        var message: string = "";

        if (this.IsLCLEntity) {
            message = TextCodeTranslator.Translate("Shipment.M.DeleteThisPackage");
        }

        else {
            if (!AppTool.IsNullOrEmpty(itemComponent.EntityPM.LastStatusCode)) {
                message = "Note that this will result in deleting container level statuses. Delete This Container?";
            }

            else {
                message = "Delete This Container";
            }
        }

        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(message);
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                this.DeletePackage(itemComponent);

                if (itemComponent.WarehouseReleaseNumber) {
                    this.DisconnectWarehouseReleaseOnShipment(itemComponent);
                }


                this.ComputeTotals();
                this.SetUIProperties();
                this.SetGenerateData();
            }
        });
    }

  DeletePackage(shipmentPackageItem: ShipmentPackageItem) {
    if (shipmentPackageItem) {

      if (shipmentPackageItem == this.SelectedRow) {
        this.SelectedRow = null;
        this.SetUIProperties_InsideButton();

      }
      if (shipmentPackageItem.EntityPM.ShipmentPackageItems != null) {
        shipmentPackageItem.EntityPM.ShipmentPackageItems.forEach(itemPackage => {
          shipmentPackageItem.EntityPM.RemoveShipmentPackageItemPM(itemPackage);
        });
      }

      if (shipmentPackageItem.EntityPM.InsideShipmentPackages != null) {
        shipmentPackageItem.EntityPM.InsideShipmentPackages.forEach(itemInside => {
          shipmentPackageItem.EntityPM.RemoveInsideShipmentPackagePM(itemInside);
        });
      }

      this.EntityPM.RemovePackage(shipmentPackageItem.EntityPM);
      this.ItemsSource.Remove(shipmentPackageItem);
    }
  }

    DisconnectWarehouseReleaseOnShipment(shipmentPackageItem: ShipmentPackageItem) {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.WarehouseReleasesIds) && !AppTool.IsNullOrEmpty(shipmentPackageItem.EntityPM.WarehouseReleaseId)) {
            var warehouseReleasesIds = "";
            this.EntityPM.WarehouseReleasesIds.split(',').forEach(item => {
                if (!AppTool.IsNullOrEmpty(item)) {
                    var packageitem = this.EntityPM.ShipmentPackages.filter(d => d.WarehouseReleaseId == item)[0];
                    if (packageitem) {
                        warehouseReleasesIds += (item + ",");
                    }

                }
            });
            if (!AppTool.IsNullOrEmpty(warehouseReleasesIds)) {
                warehouseReleasesIds += ")";
                warehouseReleasesIds = warehouseReleasesIds.replace(",)", "")
            }
            this.EntityPM.WarehouseReleasesIds = warehouseReleasesIds;
        }

    }

    AddInsideButtonClicked() {
        if (this.SelectedRow != null) {
            var itemPM = new InsideShipmentPackagePM(null);
            itemPM.Tenant = SessionLocator.Tenant;
            itemPM.ShipmentPackageId = this.SelectedRow.EntityPM.Id;

            var itemComponent = new InsideShipmentPackageItem(itemPM, this.SelectedRow, true);

            var logWindow = new LogitudeWindow();
            logWindow.DataContext = itemComponent;
            logWindow.Title = TextCodeTranslator.Translate("InsideShipmentPackage.O.AddInsidePackage");
            logWindow.Show("./ShipmentModules/ShipmentPackages/Components/Packages/AddEditInsidePackageComponent");
        }
    }
    EditInsideButtonClicked(itemComponent: InsideShipmentPackageItem) {
        if (itemComponent) {
            var logWindow = new LogitudeWindow();
            logWindow.DataContext = itemComponent;
            logWindow.Title = TextCodeTranslator.Translate("InsideShipmentPackage.O.EditInsidePackage");
            logWindow.Show("./ShipmentModules/ShipmentPackages/Components/Packages/AddEditInsidePackageComponent");
        }
    }
    DeleteInsideButtonClicked(itemComponent: InsideShipmentPackageItem) {
        if (itemComponent) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator.Translate("Shipment.M.DeleteThisPackage"));
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    itemComponent.ShipmentPackagePM.RemoveInsideShipmentPackagePM(itemComponent.EntityPM);
                    itemComponent.fatherComponent.BuildInsideItemsSource();
                    itemComponent.fatherComponent.ComputeFromInsidePackages();

                    if (itemComponent.fatherComponent.Row) {
                        var isExpandaple = false;

                        if (itemComponent.fatherComponent.InsideItemsSource.length > 0) {
                            isExpandaple = true;
                        }

                        itemComponent.fatherComponent.Row.SetExpandaple(isExpandaple);
                    }

                    if (itemComponent.ShipmentPackagePM.InsideShipmentPackages.length == 0) {
                        this.BuildItemsSource();
                    }
                }
            });
        }
    }

    ChooseCommodityClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 775;
        logitudeWindow.Height = 570;
        logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, FieldName: 'AWBCommodityItemNumber' };
        logitudeWindow.Title = TextCodeTranslator.TranslateTablePlural("Commodity") + " Search";      
        logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBChooseCommodityComponent");
        logitudeWindow.WindowClosed.subscribe(d => {
            this.SetUIProperties();
        });
    }
    EditDangerouseClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.O.Packages.EditDangerousGoods");
        logitudeWindow.WindowArgs = this.EntityPM;
        logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBDangerousPackageComponent");
    }

    SetMouseHoverRow(item: ShipmentPackageItem, isRowHover: boolean) {
        if (item) {
            item.IsRowHover = isRowHover;
        }
    }

    ViewStatusesClicked(item: ShipmentPackageItem) {
        if (item) {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Width = 950;
            logitudeWindow.Height = 595;
            logitudeWindow.IsFillScreen = true;
            logitudeWindow.Title = "Container Statuses";
            logitudeWindow.WindowArgs = { ShipmentId: this.EntityPM.Id, ContainerId: item.EntityPM.Id };
            logitudeWindow.Show("./ShipmentModules/ShipmentPackages/Components/Packages/LastStatusComponent");
        }
    }

    DeletePackagesButtonClicked() {
        if (this.EntityPM.ShipmentPackages.length > 0) {
            var confirmWindow = new ConfirmWindow();

            if (this.EntityPM.ShipmentPackages.filter(d => !AppTool.IsNullOrEmpty(d.LastStatusCode)).length > 0) {
                confirmWindow.Show("Note that this will result in deleting container level statuses. Delete Packages?");
            }

            else {
                confirmWindow.Show("Are you sure you want to delete all packages?");
            }
            
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.StartDelete();
                }
            });
        }
    }
    private StartDelete() {
        var numberOfPackages: number = this.EntityPM.ShipmentPackages.length;

        for (var i = this.EntityPM.ShipmentPackages.length - 1; i >= 0; i--) {
            var shipmentPackage = this.EntityPM.ShipmentPackages[i];

            //Package items
            if (shipmentPackage.ShipmentPackageItems != null && shipmentPackage.ShipmentPackageItems.length > 0) {
                for (var j = shipmentPackage.ShipmentPackageItems.length - 1; j >= 0; j--) {
                    shipmentPackage.RemoveShipmentPackageItemPM(shipmentPackage.ShipmentPackageItems[j]);
                }
            }

            // Inside packages
            if (shipmentPackage.InsideShipmentPackages != null && shipmentPackage.InsideShipmentPackages.length > 0) {
                for (var k = shipmentPackage.InsideShipmentPackages.length - 1; k >= 0; k--) {
                    var insidePackage = shipmentPackage.InsideShipmentPackages[k];

                    if (insidePackage.InsidePackageHarmonizes != null && insidePackage.InsidePackageHarmonizes.length > 0) {
                        for (var f = insidePackage.InsidePackageHarmonizes.length - 1; f >= 0; f--) {
                            insidePackage.RemoveInsidePackageHarmonizePM(insidePackage.InsidePackageHarmonizes[f]);
                        }
                    }

                    shipmentPackage.RemoveInsideShipmentPackagePM(insidePackage);
                }
            }

            // package harmonize
            if (shipmentPackage.ShipmentPackageHarmonizes != null && shipmentPackage.ShipmentPackageHarmonizes.length > 0) {
                for (var m = shipmentPackage.ShipmentPackageHarmonizes.length - 1; m >= 0; m--) {
                    shipmentPackage.RemoveShipmentPackageHarmonizePM(shipmentPackage.ShipmentPackageHarmonizes[m]);
                }
            }

            if (!AppTool.IsNullOrEmpty(shipmentPackage.DeliveryId)) {
                var delivery = this.EntityPM.ShipmentDeliveries.filter(f => f.Id == shipmentPackage.DeliveryId)[0];
                if (delivery) {
                    if (delivery.ShipmentPickUpDeliveryPackages != null && delivery.ShipmentPickUpDeliveryPackages.length > 0) {
                        for (var n = delivery.ShipmentPickUpDeliveryPackages.length - 1; n >= 0; n--) {
                            var deliveryPackage = delivery.ShipmentPickUpDeliveryPackages[n]

                            if (deliveryPackage) {
                                if (deliveryPackage.PickUpDeliveryPackageHarmonizes != null && deliveryPackage.PickUpDeliveryPackageHarmonizes.length > 0) {
                                    for (var f = deliveryPackage.PickUpDeliveryPackageHarmonizes.length - 1; f >= 0; f--) {
                                        deliveryPackage.RemovePickUpDeliveryPackageHarmonizePM(deliveryPackage.PickUpDeliveryPackageHarmonizes[f]);
                                    }
                                }

                                delivery.RemovePackage(deliveryPackage);
                            }
                        }
                    }

                    this.EntityPM.RemoveDelivery(delivery);
                }
            }

            if (!AppTool.IsNullOrEmpty(shipmentPackage.EmptyContainerReturnId)) {
                var emptyContainer = this.EntityPM.ShipmentDeliveries.filter(f => f.Id == shipmentPackage.EmptyContainerReturnId)[0];
                if (emptyContainer) {
                    if (emptyContainer.ShipmentPickUpDeliveryPackages != null && emptyContainer.ShipmentPickUpDeliveryPackages.length > 0) {
                        for (var n = emptyContainer.ShipmentPickUpDeliveryPackages.length - 1; n >= 0; n--) {
                            var deliveryPackage = emptyContainer.ShipmentPickUpDeliveryPackages[n]

                            if (deliveryPackage) {
                                if (deliveryPackage.PickUpDeliveryPackageHarmonizes != null && deliveryPackage.PickUpDeliveryPackageHarmonizes.length > 0) {
                                    for (var f = deliveryPackage.PickUpDeliveryPackageHarmonizes.length - 1; f >= 0; f--) {
                                        deliveryPackage.RemovePickUpDeliveryPackageHarmonizePM(deliveryPackage.PickUpDeliveryPackageHarmonizes[f]);
                                    }
                                }

                                emptyContainer.RemovePackage(deliveryPackage);
                            }
                        }
                    }

                    this.EntityPM.RemoveDelivery(emptyContainer);
                }
            }
            
            this.EntityPM.RemovePackage(shipmentPackage);
        }

        this.EntityPM.PackagesDeleted = true;
        this.EntityPM.EventNote = numberOfPackages + " packages deleted";
        this.ItemsSource = new ObservableCollection([]);
        this.ComputeTotals();
        this.SetUIProperties();
        this.SetGenerateData();
    }

    private dowonload: boolean = false;
    DownloadClicked() {
        this.dowonload = true;
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    }
    private DownloadPackages() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Downloading Packages";
        logWindow.Width = 500;
        logWindow.Height = 200;
        logWindow.Show('./ShipmentModules/ShipmentPackages/Components/Packages/DownloadPackagesFileComponent');
        logWindow.ComponentLoaded.subscribe(comp => {
            comp.Download(this.EntityPM.Id, this.EntityPM.ShipmentNumber);
        });
    }

    OnFileChanged(fileEvent) {
        var file = fileEvent.target.files[0];

        if (file) {
            var extension: string = file.name.split('.')[1];

            if (extension.includes("xls")) {
                this.SelectExcelFile(fileEvent);
            }

            else {
                var messageWindow: MessageWindow = new MessageWindow();
                messageWindow.Show("You have to upload excel files only");
            }
        }   
    }
    SelectExcelFile(fileEvent) {
        this.CurrentSession.StartBusyIndicatorLoading();

        var file = fileEvent.target.files[0];

        if (file && file.size > 0) {
            var documentExtendedService: DocumentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
            documentExtendedService.GetFileSizeAndUnit(file.size).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    var myResult = response.Result;
                    if (myResult) {
                        this.StartUploadingExcelFile(file);
                    }
                }
            });
        }
    }
    StartUploadingExcelFile(file: any) {
        if (file && file.size > 0) {
            var filebuffer = file.slice(0, file.size);
            this.ConvertArrayBufferToBase64(filebuffer, this);
        }
    }
    ConvertArrayBufferToBase64(file: any, context: any) {
        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(ResultAsArray(e));
            var len = bytes.byteLength;

            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }

            var filter = new ExcelPackageFilter();
            filter.FileData = window.btoa(binary);
            filter.ShipmentId = context.EntityPM.Id;

            context.SendExcelToServer(filter);
        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    }
    
    private saveAfterDeletePackages: boolean = false;
    private packages: ExcelPackage[];
    SendExcelToServer(filter: any) {
        var myDomainService: ShipmentDomainService = new ShipmentDomainService();

        myDomainService.PostUploadExcelFile(filter).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.packages = response.Result;

                if (this.packages.filter(d => d.HasErrors).length > 0) {
                    this.CurrentSession.StopBusyIndicator();

                    var window: MessageWindow = new MessageWindow();
                    window.Show("File contains errors, please validate the data and try again");                    
                }

                else {
                    if (this.EntityPM.ShipmentPackages.length > 0) {
                        this.CurrentSession.StopBusyIndicator();

                        var confirmWindow = new ConfirmWindow();
                        confirmWindow.Show("Uploading packages will result in deleting existing packages and all its data");
                        confirmWindow.WindowClosed.subscribe((event: any) => {
                            if (confirmWindow.Yes) {
                                this.saveAfterDeletePackages = true;
                                this.StartDelete();
                                this.CurrentSession.CurrentEditComponent.SaveChanges();
                            }
                        });
                    }

                    else {
                        this.CreatePackagesFromExcel();
                    }
                }
            }

            else {
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
    
    private CreatePackagesFromExcel() {
        this.packages.forEach(item => {
            var shipmentPackage = new ShipmentPackagePM(null);
            shipmentPackage.Quantity = 1;
            shipmentPackage.IsContainer = true;
            shipmentPackage.Tenant = SessionLocator.Tenant;
            shipmentPackage.TemperatureUnitCode = SessionLocator.TenantPM.TemperatureUnitCode;
            shipmentPackage.FlashPointTemperatureUnitCode = SessionLocator.TenantPM.TemperatureUnitCode;

            shipmentPackage.PackageTypeId = item.ContainerTypeId;
            shipmentPackage.PackageTypeCode = item.ContainerTypeCode;
            shipmentPackage.PackageTypeName = item.ContainerTypeName;
            shipmentPackage.ContainerNumber = item.ContainerNumber;
            shipmentPackage.Volume = item.Volume;
            shipmentPackage.Weight = item.GrossWeight;
            shipmentPackage.Tare = item.Tare;
            shipmentPackage.ShipperSeal = item.ShipperSeal;
            shipmentPackage.CarrierSeal = item.CarrierSeal;
            shipmentPackage.MarksAndNumbers = item.MarksAndNumbers;
            shipmentPackage.Description = item.Description;
            shipmentPackage.IsContainerRefrigerated = item.IsRefrigerated;

            var ratio: number = this.EntityPM.Ratio;
            if (AppTool.IsNullOrZero(ratio)) {
                ratio = AppTool.GetRatio(this.EntityPM.DirectionId, this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
            }

            if (AppTool.IsNullOrZero(shipmentPackage.Volume)) {
                shipmentPackage.Volume = PackageAmountCalculator.ComputeVolume(shipmentPackage.Volume, shipmentPackage.Quantity, shipmentPackage.Width, shipmentPackage.Height, shipmentPackage.Length, shipmentPackage.Weight, ratio, this.EntityPM.DimensionsUnitCode, this.EntityPM.VolumeUnitCode, this.EntityPM.GrossWeightUnitCode);
            }

            shipmentPackage.VolumetricWeight = PackageAmountCalculator.ComputeVolumetricWeight(shipmentPackage.VolumetricWeight, shipmentPackage.Volume, shipmentPackage.Weight, ratio, this.EntityPM.VolumeUnitCode, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode);

            if (item.IsRefrigerated == false) {
                shipmentPackage.NonActiveContainer = false;
            }
            
            this.EntityPM.AddPackage(shipmentPackage);
        });

        this.BuildItemsSource();
        this.ResetTotalEditedValues();
        this.ComputeTotals();
        this.CurrentSession.StopBusyIndicator();
    }
}

export class ShipmentPackageItem extends BaseComponent {
    public EntityPM: ShipmentPackagePM;
    public ShipmentPM: ShipmentPM;
    public ObjectTableName: string = "ShipmentPackage";
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public IsNewEntity: boolean = false;
    public InsideItemsSource: InsideShipmentPackageItem[] = [];
    public PackageItemsList: ObservableCollection;
    public IsRowHover: boolean = false;
    public Row: any;
    public IsCommodityNameVisible: boolean = false;
    public IsCommodityNumberVisible: boolean = false;
    public IsVehicleDetails: boolean = false;
    public IsShowReleaseNumber: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public HorseFieldIsVisible: boolean = false;
    WarehouseReleaseNumber: string;
    constructor(entity: ShipmentPackagePM, public fatherComponent: PackagesTabComponent, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.ShipmentPM = fatherComponent.EntityPM;
        this.IsLCLEntity = fatherComponent.IsLCLEntity;
        this.IsFCLEntity = fatherComponent.IsFCLEntity;
        this.IsCommodityNumberVisible = fatherComponent.IsCommodityNumberVisible;
        this.IsCommodityNameVisible = fatherComponent.IsCommodityNameVisible;
        this.HorseFieldIsVisible = fatherComponent.HorseFieldIsVisible;

        this.IsNewEntity = isNew;
        this.SetUIProperties();
        this.BuildInsideItemsSource();
        this.ValidateContainerNumber(this.ContainerNumber);
        this.BuildPackageItems();

        this.WarehouseReleaseNumber = this.EntityPM.WarehouseReleaseNumber;
        if (this.WarehouseReleaseNumber) this.IsShowReleaseNumber = true;
        this.maxPackageItemsLineNumber = ArrayTool.Max(this.PackageItemsList.Collection, "LineNumber");

        if (this.IsNewEntity) {
            this.SetUIProperties_Cars(false);
            this.IsVehicleDetails = false;
        }
    }

    public IsEditingEnabled: boolean = false;
    public IsVolumeEnabled: boolean = false;
    public IsBuildButtonEnabled: boolean = false;
    public IsEditingFieldsEnabled: boolean = false;
    public IsConnectedToRouting: boolean = false;
    public IsDeliveryConnectedWithMultiContainers: boolean = false;
    SetUIProperties() {
        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;

        var isConnectedToRouting = false;

        if (!AppTool.IsNullOrEmpty(this.DeliveryId) || !AppTool.IsNullOrEmpty(this.EmptyContainerReturnId)) {
            isConnectedToRouting = true;
        }

        var isEditingFieldsEnabled = true;
        if (!this.IsEditingEnabled) {
            isEditingFieldsEnabled = false;
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.DeliveryId)) {
            var allPackages = this.ShipmentPM.ShipmentPackages.filter(f => f.DeliveryId == this.EntityPM.DeliveryId);
            if (allPackages.length > 1) {
                this.IsDeliveryConnectedWithMultiContainers = true;
            }
        }

        this.IsEditingFieldsEnabled = isEditingFieldsEnabled;
        this.IsConnectedToRouting = isConnectedToRouting;
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
        this.SetUIProperties_Package();
        this.SetUIProperties_Container();
        this.SetUIProperties_Dangerous();
        this.SetUIProperties_BuildButton();
        this.SetUIProperties_Harmonize();
        if (!AppTool.IsNullOrEmpty(this.PackageTypeId)) {
            var myService: PackageTypeListService = new PackageTypeListService();
            myService.getSingle(this.PackageTypeId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: PackageTypeList = myResponse.Result;
                    if (list != null) {
                        this.SetUIProperties_Cars(this.IsEditingEnabled && list.IsVehicle);
                        this.IsVehicleDetails = list.IsVehicle;
                    }
                }
            });
        }
    }
    SetUIProperties_Package() {
        if (this.ShipmentPM.TransportModeId == "A") {
            var isVolumeEnabled: boolean = false;
            var isDimensionEnabled: boolean = false;
            var isGrossWeightEnabled: boolean = false;

            if (this.IsEditingFieldsEnabled) {
                if (this.Quantity > 0) {
                    isVolumeEnabled = true;
                    isDimensionEnabled = true;
                    isGrossWeightEnabled = true;

                    if (this.Height != null || this.Width != null || this.Length != null) {
                        isVolumeEnabled = false;
                    }

                    else if (this.Volume != null) {
                        isDimensionEnabled = false;
                    }
                }
            }

            this.IsVolumeEnabled = isVolumeEnabled;
            this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
            this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
            this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
            this.UIProperties.SetEnabled("Weight", this.ObjectTableName, isGrossWeightEnabled);

            this.UIProperties.SetEnabled("CommodityNumber", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Reference1", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Reference2", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Reference3", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Reference4", this.ObjectTableName, this.IsEditingFieldsEnabled);
        }
    }
    SetUIProperties_Container() {
        if (this.ShipmentPM.TransportModeId != "A") {
            this.UIProperties.SetRequired('Weight', this.ObjectTableName, AppTool.IsNullOrEmpty(this.Weight) ? true : false);
            this.UIProperties.SetRequired('PackageTypeId', this.ObjectTableName, AppTool.IsNullOrEmpty(this.PackageTypeId) ? true : false);

            this.UIProperties.SetEnabled("PackageTypeId", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("ContainerNumber", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsEditingFieldsEnabled);

            var isVolumeEnabled: boolean = false;
            var isDimensionEnabled: boolean = false;
            var isGrossWeightEnabled: boolean = false;
            if (this.IsEditingFieldsEnabled) {

                if (this.IsFCLEntity) {
                    isVolumeEnabled = true;
                    isDimensionEnabled = true;
                    isGrossWeightEnabled = true;
                }

                else {
                    if (this.Quantity > 0) {
                        isVolumeEnabled = true;
                        isDimensionEnabled = true;
                        isGrossWeightEnabled = true;

                        if (this.Height != null || this.Width != null || this.Length != null) {
                            isVolumeEnabled = false;
                        }

                        else if (this.Volume != null) {
                            isDimensionEnabled = false;
                        }
                    }
                }
            }

            this.IsVolumeEnabled = isVolumeEnabled;
            this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
            this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
            this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
            this.UIProperties.SetEnabled("Weight", this.ObjectTableName, isGrossWeightEnabled);
            this.UIProperties.SetEnabled("ShipperSeal", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Tare", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("MarksAndNumbers", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Description", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Reference1", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Reference2", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Reference3", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Reference4", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("CommodityNumber", this.ObjectTableName, this.IsEditingFieldsEnabled);

            var isFCLFieldEnabled = false;
            if (this.IsEditingFieldsEnabled) {
                if (this.IsFCLEntity) {
                    isFCLFieldEnabled = true;
                }
            }
            
            this.UIProperties.SetEnabled("VGM", this.ObjectTableName, isFCLFieldEnabled);
            this.UIProperties.SetEnabled("CarrierSeal", this.ObjectTableName, isFCLFieldEnabled);
            this.UIProperties.SetEnabled("Ventilation", this.ObjectTableName, isFCLFieldEnabled);

            var isTemperatureEnabled = false;
            if (isFCLFieldEnabled) {
                if (this.NonActiveContainer == false) {
                    isTemperatureEnabled = true;
                }
            }

            this.UIProperties.SetEnabled("Temperature", this.ObjectTableName, isTemperatureEnabled);

            this.SetUIProperties_ContainerFU();
            this.SetUIProperties_NonActiveContainer();
        }
    }
    SetUIProperties_Dangerous() {
        if (this.ShipmentPM.TransportModeId != "A") {
            var isFieldEnabled = false;

            if (this.IsEditingFieldsEnabled) {
                if (this.EntityPM.IsDangerous) {
                    isFieldEnabled = true;
                }
            }

            this.UIProperties.SetEnabled("IsDangerous", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("ClassNumber", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetEnabled("UnNumber", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetEnabled("PackagingGroup", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetEnabled("IMDGCode", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetEnabled("FlashPoint", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetEnabled("MaterialDescription", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetEnabled("FlashPointTemperatureUnitCode", this.ObjectTableName, isFieldEnabled);
        }
    }
    SetUIProperties_BuildButton() {
        if (this.ShipmentPM.TransportModeId != "A") {
            var isEnabled = false;

            if (this.IsEditingFieldsEnabled) {
                if (this.PackageTypeId != null) {
                    isEnabled = true;
                }
            }

            this.IsBuildButtonEnabled = isEnabled
        }
    }
    SetUIProperties_ContainerFU() {

        var isDeliveryFieldsEnabled: boolean = false;
        var isDeliveryPlacesEnabled: boolean = false;
        if (this.IsEditingEnabled) {
            if (this.IsDeliveryFU) {

                if (this.IsDeliveryConnectedWithMultiContainers == false) {

                    isDeliveryFieldsEnabled = true;

                    if (AppTool.IsNullOrEmpty(this.DeliveryId)) {
                        isDeliveryPlacesEnabled = true;
                    }
                }
            }
        }

        this.UIProperties.SetEnabled("IsDeliveryFU", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DeliveryETD", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryATD", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryETA", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryATA", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryTransportModeCode", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryFrom", this.ObjectTableName, isDeliveryPlacesEnabled);
        this.UIProperties.SetEnabled("DeliveryTo", this.ObjectTableName, isDeliveryPlacesEnabled);

        var isEmptyContainerReturnFieldsEnabled: boolean = false;
        var isEmptyContainerReturnPlacesEnabled: boolean = false;
        if (this.IsEditingEnabled) {
            if (this.IsEmptyContainerReturnFU) {
                isEmptyContainerReturnFieldsEnabled = true;

                if (AppTool.IsNullOrEmpty(this.EmptyContainerReturnId)) {
                    isEmptyContainerReturnPlacesEnabled = true;
                }
            }
        }

        this.UIProperties.SetEnabled("IsEmptyContainerReturnFU", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnETD", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnATD", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnETA", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnATA", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("ECRTransportModeCode", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnFrom", this.ObjectTableName, isEmptyContainerReturnPlacesEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnTo", this.ObjectTableName, isEmptyContainerReturnPlacesEnabled);
    }
    SetUIProperties_NonActiveContainer() {

        var isFieldEnabled: boolean = false;
        if (this.IsEditingEnabled) {
            if (this.IsContainer && this.IsContainerRefrigerated) {
                isFieldEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("NonActiveContainer", this.ObjectTableName, isFieldEnabled);
    }
    SetUIProperties_Harmonize() {

        var isFieldEnabled: boolean = true;
        if (this.IsEditingEnabled) {

            isFieldEnabled = true;

            if (this.IsMultiHarmonize == true) {
                isFieldEnabled = false;
            }
        }

        this.UIProperties.SetEnabled("Harmonize", this.ObjectTableName, isFieldEnabled);
    }
    SetUIProperties_Cars(isEnabled: boolean) {
        if (this.IsEditingEnabled && !isEnabled) {
            this.Make = null;
            this.Model = null;
            this.Color = null;
            this.Year = null;
            this.CountryId = null;
            this.ChassisNumber = null;
            this.RegistrationNumber = null;
        }
        this.UIProperties.SetEnabled("Make", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Model", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Color", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Year", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("CountryId", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("ChassisNumber", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("RegistrationNumber", this.ObjectTableName, isEnabled);
    }

    public PackageTypeList: PackageTypeList;
    get PackageTypeTextCode() { return this.IsLCLEntity ? "ShipmentPackage.F.PackageTypeId" : "ShipmentPackage.F.ContainerTypeId"; }
    get PackageTypeId() { return this.EntityPM.PackageTypeId; }
    set PackageTypeId(value: string) {
        if (this.EntityPM.PackageTypeId != value) {
            this.EntityPM.PackageTypeId = value;
            this.SetUIProperties_BuildButton();

            if (this.ShipmentPM.TransportModeId != "A") {
                this.UIProperties.SetRequired('PackageTypeId', this.ObjectTableName, AppTool.IsNullOrEmpty(this.PackageTypeId) ? true : false);
            }

            if (AppTool.IsNullOrEmpty(value)) {
                this.PackageTypeName = null;
                this.EntityPM.PackageTypeCode = null;
                this.IsContainer = false;
                this.IsContainerRefrigerated = false;
                this.NonActiveContainer = false;
                this.SetUIProperties_NonActiveContainer();
                this.SetUIProperties_Cars(false);
                this.IsVehicleDetails = false;
            }

            else {
                var myService: PackageTypeListService = new PackageTypeListService();
                myService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PackageTypeList = myResponse.Result;
                        if (list != null) {
                            this.PackageTypeName = list.EnglishName;
                            this.EntityPM.PackageTypeCode = list.Code;
                            this.IsContainer = list.IsContainer;
                            this.IsContainerRefrigerated = list.IsRefrigerated;

                            if (list.IsRefrigerated == false) {
                                this.NonActiveContainer = false;
                            }

                            if (this.IsNewEntity && this.IsContainer) {
                                this.Quantity = 1;
                            }

                            this.SetUIProperties_NonActiveContainer();
                            this.SetUIProperties_Cars(list.IsVehicle);
                            this.IsVehicleDetails = list.IsVehicle;
                        }
                        else {
                            this.SetUIProperties_Cars(false);
                            this.IsVehicleDetails = false;
                        }
                    }
                });
            }
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newValue: string) {
        if (this.EntityPM.Notes != newValue) {
            this.EntityPM.Notes = newValue;
        }
    }

    get PackageTypeName() { return this.EntityPM.PackageTypeName; }
    set PackageTypeName(newValue: string) {
        if (this.EntityPM.PackageTypeName != newValue) {
            this.EntityPM.PackageTypeName = newValue;
        }
    }
    
    get IsContainer() { return this.EntityPM.IsContainer; }
    set IsContainer(newValue: boolean) {
        if (this.EntityPM.IsContainer != newValue) {
            this.EntityPM.IsContainer = newValue;
        }
    }

    get IsContainerRefrigerated() { return this.EntityPM.IsContainerRefrigerated; }
    set IsContainerRefrigerated(newValue: boolean) {
        if (this.EntityPM.IsContainerRefrigerated != newValue) {
            this.EntityPM.IsContainerRefrigerated = newValue;
        }
    }

    get NonActiveContainer() { return this.EntityPM.NonActiveContainer; }
    set NonActiveContainer(value: boolean) {
        if (this.EntityPM.NonActiveContainer != value) {
            this.EntityPM.NonActiveContainer = value;

            if (value) {
                this.Temperature = '999';
            }

            this.SetUIProperties_Container();
        }
    }

    get ContainerNumber() { return this.EntityPM.ContainerNumber; }
    set ContainerNumber(newValue: string) {
        if (this.EntityPM.ContainerNumber != newValue) {

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.EntityPM.ContainerNumber = newValue;
            }

            else {
                this.EntityPM.ContainerNumber = newValue.toUpperCase();
            }

            this.ValidateContainerNumber(newValue);
        }
    }
    
    public WarningErrorsList: string[] = [];
    public ContainerNumberWarning: string = null;
    ContainerNumberLostFocus(input: string) {
        this.ValidateContainerNumber(input);
    }

    ValidateContainerNumber(input: string) {
        var warnings: string[] = [];
        var error = FormatTool.ValidateContainerNumber(input);

        if (!AppTool.IsNullOrEmpty(error)) {
            warnings.push(error);
        }

        this.WarningErrorsList = warnings;
        this.ContainerNumberWarning = error;
    }

    // Dimensions
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
        var myDimensions: string;

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

    get Weight() { return this.EntityPM.Weight; }
    set Weight(newValue: number) {
        var myValue: number = AppTool.Round(newValue, 3);

        if (this.EntityPM.Weight != myValue) {
            this.EntityPM.Weight = myValue

            if (this.ShipmentPM.TransportModeId != "A") {
                this.UIProperties.SetRequired('Weight', this.ObjectTableName, AppTool.IsNullOrEmpty(this.Weight) ? true : false);

            }

            if (this.fatherComponent.ItemsSource.Collection.indexOf(this) > -1) {
                this.fatherComponent.ResetTotalEditedValues();
                this.fatherComponent.ComputeTotals();
            }
        }
    }

    OnGrossWeightLostFocus(input1: number) {
        if (this.fatherComponent.IsLCLEntity) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {
                if (this.Width == null || this.Height == null || this.Length == null) {
                    this.EntityPM.VolumetricWeight = AppTool.GetWeightFromWeight(this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode, this.EntityPM.Weight);
                    this.EntityPM.Volume = AppTool.GetVolumeFromWeight(this.ShipmentPM.ChargeableWeightUnitCode, this.ShipmentPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.ShipmentPM.Ratio);

                    this.SetUIProperties();

                    if (this.fatherComponent.ItemsSource.Collection.indexOf(this) > -1) {
                        this.fatherComponent.ResetTotalEditedValues();
                        this.fatherComponent.ComputeTotals();
                    }
                }
            }
        }
    }

    private ComputeVolume() {

        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        this.Volume = AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode);
    }
    private ComputeVolumetricWeight() {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        this.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode);
    }

    // IsDangerous
    get IsDangerous() { return this.EntityPM.IsDangerous; }
    set IsDangerous(newValue: boolean) {
        if (this.EntityPM.IsDangerous != newValue) {
            this.EntityPM.IsDangerous = newValue;

            this.ClassNumber = null;
            this.UnNumber = null;
            this.PackagingGroup = null;
            this.IMDGCode = null;
            this.FlashPoint = null;
            this.MaterialDescription = null;
            this.EntityPM.CeficClass = null;
            this.EntityPM.KelmerCode = null;
            this.EntityPM.ProperShippingName = null;
            this.EntityPM.EMS = null;
            this.EntityPM.MarinePollutant = false;
            this.SetUIProperties_Dangerous();
        }
    }
    
    get ClassNumber() { return this.EntityPM.ClassNumber; }
    set ClassNumber(newValue: string) {
        if (this.EntityPM.ClassNumber != newValue) {
            this.EntityPM.ClassNumber = newValue;
        }
    }

    get UnNumber() { return this.EntityPM.UnNumber; }
    set UnNumber(newValue: string) {
        if (this.EntityPM.UnNumber != newValue) {
            this.EntityPM.UnNumber = newValue;
        }
    }

    get PackagingGroup() { return this.EntityPM.PackagingGroup; }
    set PackagingGroup(newValue: string) {
        if (this.EntityPM.PackagingGroup != newValue) {
            this.EntityPM.PackagingGroup = newValue;
        }
    }

    get IMDGCode() { return this.EntityPM.IMDGCode; }
    set IMDGCode(newValue: string) {
        if (this.EntityPM.IMDGCode != newValue) {
            this.EntityPM.IMDGCode = newValue;
        }
    }

    get FlashPoint() { return this.EntityPM.FlashPoint; }
    set FlashPoint(newValue: string) {
        if (this.EntityPM.FlashPoint != newValue) {
            this.EntityPM.FlashPoint = newValue;
        }
    }

    get FlashPointTemperatureUnitCode() { return this.EntityPM.FlashPointTemperatureUnitCode; }
    set FlashPointTemperatureUnitCode(newValue: string) {
        if (this.EntityPM.FlashPointTemperatureUnitCode != newValue) {
            this.EntityPM.FlashPointTemperatureUnitCode = newValue;
        }
    }

    get MaterialDescription() { return this.EntityPM.MaterialDescription; }
    set MaterialDescription(newValue: string) {
        if (this.EntityPM.MaterialDescription != newValue) {
            this.EntityPM.MaterialDescription = newValue;
        }
    }

    // Other Properties
    get ShipperSeal() { return this.EntityPM.ShipperSeal; }
    set ShipperSeal(newValue: string) {
        if (this.EntityPM.ShipperSeal != newValue) {
            this.EntityPM.ShipperSeal = newValue;
        }
    }

    get CarrierSeal() { return this.EntityPM.CarrierSeal; }
    set CarrierSeal(newValue: string) {
        if (this.EntityPM.CarrierSeal != newValue) {
            this.EntityPM.CarrierSeal = newValue;
        }
    }

    get Tare() { return this.EntityPM.Tare; }
    set Tare(newValue: number) {
        if (this.EntityPM.Tare != newValue) {
            this.EntityPM.Tare = AppTool.Round(newValue, 3);
        }
    }

    get Harmonize() { return this.EntityPM.Harmonize; }
    set Harmonize(newValue: string) {
        if (this.EntityPM.Harmonize != newValue) {
            this.EntityPM.Harmonize = newValue;
        }
    }

    get IsMultiHarmonize() { return this.EntityPM.IsMultiHarmonize }
    set IsMultiHarmonize(newValue: boolean) {
        if (this.EntityPM.IsMultiHarmonize != newValue) {
            this.EntityPM.IsMultiHarmonize = newValue;
        }
    }

    get Temperature() { return this.EntityPM.Temperature; }
    set Temperature(newValue: string) {
        if (this.EntityPM.Temperature != newValue) {
            this.EntityPM.Temperature = newValue;
        }
    }

    get Ventilation() { return this.EntityPM.Ventilation; }
    set Ventilation(newValue: number) {
        if (this.EntityPM.Ventilation != newValue) {
            this.EntityPM.Ventilation = AppTool.Round(newValue, 3);
        }
    }

    get MarksAndNumbers() { return this.EntityPM.MarksAndNumbers; }
    set MarksAndNumbers(newValue: string) {
        if (this.EntityPM.MarksAndNumbers != newValue) {
            this.EntityPM.MarksAndNumbers = newValue;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    get SOC() { return this.EntityPM.SOC; }
    set SOC(newValue: number) {
        if (this.EntityPM.SOC != newValue) {
            this.EntityPM.SOC = AppTool.Round(newValue, 0);
        }
    }

    get VGM() { return this.EntityPM.VGM; }
    set VGM(newValue: number) {
        if (this.EntityPM.VGM != newValue) {
            this.EntityPM.VGM = AppTool.Round(newValue, 3);
        }
    }

    get MethodUsed() { return this.EntityPM.MethodUsed; }
    set MethodUsed(newValue: string) {
        if (this.EntityPM.MethodUsed != newValue) {
            this.EntityPM.MethodUsed = newValue;
        }
    }

    private selectedMethod: any = null;
    get SelectedMethod() { return this.selectedMethod; };
    set SelectedMethod(value: any) {
        if (this.selectedMethod != value) {
            this.selectedMethod = value;

            if (value) {
                this.MethodUsed = value.Name;
            }

            else {
                this.MethodUsed = null;
            }
        }
    }

    get Reference1() { return this.EntityPM.Reference1; }
    set Reference1(newValue: string) {
        if (this.EntityPM.Reference1 != newValue) {
            this.EntityPM.Reference1 = newValue;
        }
    }

    get Reference2() { return this.EntityPM.Reference2; }
    set Reference2(newValue: string) {
        if (this.EntityPM.Reference2 != newValue) {
            this.EntityPM.Reference2 = newValue;
        }
    }

    get Reference3() { return this.EntityPM.Reference3; }
    set Reference3(newValue: string) {
        if (this.EntityPM.Reference3 != newValue) {
            this.EntityPM.Reference3 = newValue;
        }
    }

    get Reference4() { return this.EntityPM.Reference4; }
    set Reference4(newValue: string) {
        if (this.EntityPM.Reference4 != newValue) {
            this.EntityPM.Reference4 = newValue;
        }
    }

    get CommodityNumber() { return this.EntityPM.CommodityNumber; }
    set CommodityNumber(newValue: string) {
        if (this.EntityPM.CommodityNumber != newValue) {
            this.EntityPM.CommodityNumber = newValue;

            this.CommodityName = null;
        }
    }

    get CommodityName() { return this.EntityPM.CommodityName; }
    set CommodityName(newValue: string) {
        if (this.EntityPM.CommodityName != newValue) {
            this.EntityPM.CommodityName = newValue;
        }
    }

    get LastStatusName() { return this.EntityPM.LastStatusName; }

    public MethodsList: any[] = [];
    FillMethodsList() {
        this.MethodsList = [];
        //this.MethodsList.push({ Code: "0", Name: null });
        this.MethodsList.push({ Code: "1", Name: "Method 1" });
        this.MethodsList.push({ Code: "2", Name: "Method 2" });

        if (!AppTool.IsNullOrEmpty(this.MethodUsed)) {
            this.selectedMethod = this.MethodsList.filter(f => f.Name == this.MethodUsed)[0];
        }
    }
    BuildButtonClicked() {
        var myResult: string = "";
        var importer: string = "";
        var importerRef1: string = "";

        if (this.ShipmentPM.DirectionId == "E" || this.ShipmentPM.DirectionId == "I") {
            importer = this.ShipmentPM.ConsigneeName;
            importerRef1 = this.ShipmentPM.ConsigneeReference1;
        }

        if (this.EntityPM.IsContainer) {
            if (!AppTool.IsNullOrEmpty(this.ContainerNumber)) {
                myResult = myResult + this.ContainerNumber + "\n";
            }

            if (!AppTool.IsNullOrEmpty(this.ShipperSeal)) {
                myResult = myResult + "Shipper Seal: " + this.ShipperSeal + "\n";
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.CarrierSeal)) {
                myResult = myResult + "Carrier Seal: " + this.EntityPM.CarrierSeal + "\n";
            }

            if (this.Tare != null) {
                myResult = myResult + "Tare: " + this.Tare.toString() + "\n";
            }

            if (!AppTool.IsNullOrEmpty(importer)) {
                myResult = myResult + importer;
            }
        }

        else {
            if (!AppTool.IsNullOrEmpty(importer)) {
                myResult = myResult + importer + "\n";
            }

            if (!AppTool.IsNullOrEmpty(importerRef1)) {
                myResult = myResult + "PO: " + importerRef1;
            }
        }

        this.MarksAndNumbers = myResult;
    }

    // Inside Packages
    public RowDetailsHeights: number = 0;
    BuildInsideItemsSource() {

        this.InsideItemsSource = [];

        this.EntityPM.InsideShipmentPackages.forEach(item => {
            this.InsideItemsSource.push(new InsideShipmentPackageItem(item, this));
        });

        this.RowDetailsHeights = (this.InsideItemsSource.length * 26) + 20 + 28;
        this.fatherComponent.ReloadDetails.emit("");
    }
    ComputeFromInsidePackages() {

        var myWeight = 0;
        var myVolume = 0;

        this.InsideItemsSource.forEach(item => {
            if (!AppTool.IsNullOrEmpty(item.Weight)) {
                myWeight += item.Weight;
            }

            if (!AppTool.IsNullOrEmpty(item.Volume)) {
                myVolume += item.Volume;
            }
        });

        this.Weight = myWeight;
        this.Volume = myVolume;
    }

    // Package Items
    BuildPackageItems() {
        if (this.PackageItemsList == null) {
            this.PackageItemsList = new ObservableCollection([]);
        }
        else {
            this.PackageItemsList.Collection.forEach(item => {
                this.PackageItemsList.Clear();
            });
        }

        var itemsCollection: PackageItem[] = [];

        this.EntityPM.ShipmentPackageItems.forEach(item => {
            itemsCollection.push(new PackageItem(item, this, false));
        });

        this.PackageItemsList.InsertCollection(itemsCollection);
    }

    private maxPackageItemsLineNumber = 0;
    public savedItems: ShipmentPackageItemPM[] = [];
    public CopyPackageItems() {
        this.savedItems = [];
        if (this.EntityPM.ShipmentPackageItems.length > 0) {
           // this.maxPackageItemsLineNumber = this.EntityPM.ShipmentPackageItems.Max(m => m.LineNumber);
            this.EntityPM.ShipmentPackageItems.forEach(item => {
                this.maxPackageItemsLineNumber = 0;
                if (item.LineNumber > this.maxPackageItemsLineNumber) {
                    this.maxPackageItemsLineNumber = item.LineNumber;
                }
                var packageItem= new ShipmentPackageItemPM(null);
                packageItem.PackageId = item.PackageId;
                packageItem.Tenant = item.Tenant;
                packageItem.Quantity = item.Quantity;
                packageItem.LineNumber = item.LineNumber;
                packageItem.Description = item.Description;
                packageItem.GoodsValue = item.GoodsValue;
                this.savedItems.push(packageItem);
            });
        }
    }

    public ResetPackageItems() {
        if (this.savedItems != null) {
            var items: ShipmentPackageItemPM[] = this.EntityPM.ShipmentPackageItems;
            items.forEach(item => {
                var savedItem: ShipmentPackageItemPM = this.savedItems.filter(d => d.LineNumber == item.LineNumber)[0];
                if (savedItem == null) {
                    if (this.EntityPM.ShipmentPackageItems.indexOf(item) != -1) {
                        this.EntityPM.RemoveShipmentPackageItemPM(item);
                    }
                }

                else {
                    item.Quantity = savedItem.Quantity;
                    item.Description = savedItem.Description;
                    item.GoodsValue = savedItem.GoodsValue;
                }
            });

            this.savedItems.forEach(item => {
                var list = this.EntityPM.ShipmentPackageItems.filter(d => d.LineNumber == item.LineNumber);
                if (list == null) {
                    this.EntityPM.ShipmentPackageItems.push(item);
                }
            });
        }
    }

    AddPackageItemMethod() {
        this.maxPackageItemsLineNumber += 1;
        var item: ShipmentPackageItemPM = new ShipmentPackageItemPM(null);
        item.Tenant = SessionLocator.Tenant;
        item.PackageId = this.EntityPM.Id;
        item.LineNumber = this.maxPackageItemsLineNumber;
        this.PackageItemsList.Insert(new PackageItem(item, this, true));
        //this.CurrentSession.LogitudeGridHelper.ResetRowIndex();
    }
    OnRowEnded($event) { 
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        //alert("Oh Yea !! " + $event + " " + this.PackageItemsList.Length);
        //if (errors != null && errors.length == 0) {
            if (($event) == this.PackageItemsList.Length) {
                this.AddPackageItemMethod();
                //this.CurrentSession.ResetRowIndex();
            }
        //}
    }
    Ondblclick() {
        if (this.PackageItemsList.Length == 0) {
            this.AddPackageItemMethod();
        }
    }

    // Container Follow U\p
    get IsDeliveryFU() { return this.EntityPM.IsDeliveryFU; }
    set IsDeliveryFU(value: boolean) {
        if (this.EntityPM.IsDeliveryFU != value) {
            this.EntityPM.IsDeliveryFU = value;
            this.SetUIProperties_ContainerFU();
        }
    }

    get DeliveryId() { return this.EntityPM.DeliveryId; }
    set DeliveryId(value: string) {
        if (this.EntityPM.DeliveryId != value) {
            this.EntityPM.DeliveryId = value;
            this.SetUIProperties_ContainerFU();
        }
    }

    get DeliveryFrom() { return this.EntityPM.DeliveryFrom; }
    set DeliveryFrom(value: string) {
        if (this.EntityPM.DeliveryFrom != value) {
            this.EntityPM.DeliveryFrom = value;
        }
    }

    get DeliveryTo() { return this.EntityPM.DeliveryTo; }
    set DeliveryTo(value: string) {
        if (this.EntityPM.DeliveryTo != value) {
            this.EntityPM.DeliveryTo = value;
        }
    }

    get DeliveryTransportModeCode() { return this.EntityPM.DeliveryTransportModeCode; }
    set DeliveryTransportModeCode(value: string) {
        if (this.EntityPM.DeliveryTransportModeCode != value) {
            this.EntityPM.DeliveryTransportModeCode = value;
        }
    }

    get DeliveryETD() { return this.EntityPM.DeliveryETD; }
    set DeliveryETD(value: Date) {
        if (this.EntityPM.DeliveryETD != value) {
            this.EntityPM.DeliveryETD = value;
        }
    }

    get DeliveryATD() { return this.EntityPM.DeliveryATD; }
    set DeliveryATD(value: Date) {
        if (this.EntityPM.DeliveryATD != value) {
            this.EntityPM.DeliveryATD = value;
            this.SetUIProperties_ValidateActualDates_D();
        }
    }

    get DeliveryETA() { return this.EntityPM.DeliveryETA; }
    set DeliveryETA(value: Date) {
        if (this.EntityPM.DeliveryETA != value) {
            this.EntityPM.DeliveryETA = value;
        }
    }

    get DeliveryATA() { return this.EntityPM.DeliveryATA; }
    set DeliveryATA(value: Date) {
        if (this.EntityPM.DeliveryATA != value) {
            this.EntityPM.DeliveryATA = value;
            this.SetUIProperties_ValidateActualDates_D();
        }
    }

    get IsEmptyContainerReturnFU() { return this.EntityPM.IsEmptyContainerReturnFU; }
    set IsEmptyContainerReturnFU(value: boolean) {
        if (this.EntityPM.IsEmptyContainerReturnFU != value) {
            this.EntityPM.IsEmptyContainerReturnFU = value;
            this.SetUIProperties_ContainerFU();
        }
    }

    get EmptyContainerReturnId() { return this.EntityPM.EmptyContainerReturnId; }
    set EmptyContainerReturnId(value: string) {
        if (this.EntityPM.EmptyContainerReturnId != value) {
            this.EntityPM.EmptyContainerReturnId = value;
            this.SetUIProperties_ContainerFU();
        }
    }

    get EmptyContainerReturnFrom() { return this.EntityPM.EmptyContainerReturnFrom; }
    set EmptyContainerReturnFrom(value: string) {
        if (this.EntityPM.EmptyContainerReturnFrom != value) {
            this.EntityPM.EmptyContainerReturnFrom = value;
        }
    }

    get EmptyContainerReturnTo() { return this.EntityPM.EmptyContainerReturnTo; }
    set EmptyContainerReturnTo(value: string) {
        if (this.EntityPM.EmptyContainerReturnTo != value) {
            this.EntityPM.EmptyContainerReturnTo = value;
        }
    }

    get ECRTransportModeCode() { return this.EntityPM.ECRTransportModeCode; }
    set ECRTransportModeCode(value: string) {
        if (this.EntityPM.ECRTransportModeCode != value) {
            this.EntityPM.ECRTransportModeCode = value;
        }
    }

    get EmptyContainerReturnETD() { return this.EntityPM.EmptyContainerReturnETD; }
    set EmptyContainerReturnETD(value: Date) {
        if (this.EntityPM.EmptyContainerReturnETD != value) {
            this.EntityPM.EmptyContainerReturnETD = value;
        }
    }

    get EmptyContainerReturnATD() { return this.EntityPM.EmptyContainerReturnATD; }
    set EmptyContainerReturnATD(value: Date) {
        if (this.EntityPM.EmptyContainerReturnATD != value) {
            this.EntityPM.EmptyContainerReturnATD = value;
            this.SetUIProperties_ValidateActualDates_R();
        }
    }

    get EmptyContainerReturnETA() { return this.EntityPM.EmptyContainerReturnETA; }
    set EmptyContainerReturnETA(value: Date) {
        if (this.EntityPM.EmptyContainerReturnETA != value) {
            this.EntityPM.EmptyContainerReturnETA = value;
        }
    }

    get EmptyContainerReturnATA() { return this.EntityPM.EmptyContainerReturnATA; }
    set EmptyContainerReturnATA(value: Date) {
        if (this.EntityPM.EmptyContainerReturnATA != value) {
            this.EntityPM.EmptyContainerReturnATA = value;
            this.SetUIProperties_ValidateActualDates_R();
        }
    }
    
    get Make() { return this.EntityPM.Make; }
    set Make(newValue: string) {
        if (this.EntityPM.Make != newValue) {
            this.EntityPM.Make = newValue;
        }
    }

    get Model() { return this.EntityPM.Model; }
    set Model(newValue: string) {
        if (this.EntityPM.Model != newValue) {
            this.EntityPM.Model = newValue;
        }
    }
    
    get Year() { return this.EntityPM.Year; }
    set Year(newValue: string) {
        if (this.EntityPM.Year != newValue) {
            this.EntityPM.Year = newValue;
        }
    }

    get Color() { return this.EntityPM.Color; }
    set Color(newValue: string) {
        if (this.EntityPM.Color != newValue) {
            this.EntityPM.Color = newValue;
        }
    }

    get ChassisNumber() { return this.EntityPM.ChassisNumber; }
    set ChassisNumber(newValue: string) {
        if (this.EntityPM.ChassisNumber != newValue) {
            this.EntityPM.ChassisNumber = newValue;
        }
    }

    get RegistrationNumber() { return this.EntityPM.RegistrationNumber ; }
    set RegistrationNumber (newValue: string) {
        if (this.EntityPM.RegistrationNumber  != newValue) {
            this.EntityPM.RegistrationNumber  = newValue;
        }
    }

    get CountryId() { return this.EntityPM.CountryId; }
    set CountryId(newValue: string) {
        if (this.EntityPM.CountryId != newValue) {
            this.EntityPM.CountryId = newValue;
        }
    }

    get HorseId() { return this.EntityPM.HorseId; }
    set HorseId(newValue: string) {
        if (this.EntityPM.HorseId != newValue) {
            this.EntityPM.HorseId = newValue;
        }
    }

    get HorseName() { return this.EntityPM.HorseName; }
    set HorseName(newValue: string) {
        if (this.EntityPM.HorseName != newValue) {
            this.EntityPM.HorseName = newValue;
        }
    }

    horse: HorseList;
    get Horse() { return this.horse; }
    set Horse(value: HorseList) {
        if (this.horse != value) {
            this.horse = value;
        }

        if (value != null) {
            this.HorseName = value.Name;
        }
        else {
            this.HorseName = null;
        }
    }

    private SaveCommandCode: string;
    private SaveCompletedEvent: any = null;
    SetUIProperties_ValidateActualDates_D() {

        this.UIProperties.SetValidity("DeliveryATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("DeliveryATA", this.ObjectTableName, true, null);

        if (!DateTool.IsActualDateValid(this.DeliveryATD)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPackage.F.DeliveryATD"));
            this.UIProperties.SetValidity("DeliveryATD", this.ObjectTableName, false, errorMessage);
        }

        if (!DateTool.IsActualDateValid(this.DeliveryATA)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPackage.F.DeliveryATA"));
            this.UIProperties.SetValidity("DeliveryATA", this.ObjectTableName, false, errorMessage);
        }
    }
    SetUIProperties_ValidateActualDates_R() {

        this.UIProperties.SetValidity("EmptyContainerReturnATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("EmptyContainerReturnATA", this.ObjectTableName, true, null);

        if (!DateTool.IsActualDateValid(this.EmptyContainerReturnATD)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPackage.F.EmptyContainerReturnATD"));
            this.UIProperties.SetValidity("EmptyContainerReturnATD", this.ObjectTableName, false, errorMessage);
        }

        if (!DateTool.IsActualDateValid(this.EmptyContainerReturnATA)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPackage.F.EmptyContainerReturnATA"));
            this.UIProperties.SetValidity("EmptyContainerReturnATA", this.ObjectTableName, false, errorMessage);
        }
    }
    DeliveryIconClicked() {
        var typeCode = "D";

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.SaveCommandCode = typeCode;
            this.EntityPM.DummyIdGuid = AppTool.GetNewGuid();
            this.SaveChanges();
        }

        else {

            if (!AppTool.IsNullOrEmpty(this.DeliveryId) || this.IsDeliveryFU) {
                this.ShowFollowupWindow(typeCode, false);
            }

            else {
                this.ShowActionsWindow(typeCode);
            }
        }
    }
    EmptyContainerIconClicked() {
        var typeCode = "R";

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.SaveCommandCode = typeCode;
            this.EntityPM.DummyIdGuid = AppTool.GetNewGuid();
            this.SaveChanges();
        }

        else {
            if (!AppTool.IsNullOrEmpty(this.EmptyContainerReturnId) || this.IsEmptyContainerReturnFU) {
                this.ShowFollowupWindow(typeCode, false);
            }

            else {
                this.ShowActionsWindow(typeCode);
            }
        }
    }
    SaveChanges() {
        if (!this.SaveCompletedEvent) {
            this.SaveCompletedEvent = this.fatherComponent.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {

                    this.ShipmentPM = this.fatherComponent.entityArgs.EntityPM;
                    this.EntityPM = this.ShipmentPM.ShipmentPackages.filter(f => f.DummyIdGuid == this.EntityPM.DummyIdGuid)[0];
                    this.ShowActionsWindow(this.SaveCommandCode);
                }
               
                AppTool.KillEventEmitter(this.SaveCompletedEvent);
                this.SaveCompletedEvent = null;
            });

            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    }
    ShowActionsWindow(typeCode: string) {

        var windowTitle = typeCode == "D" ? "Container Delivery Actions" : "Empty Container Return Actions";
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { Code: typeCode };
        logWindow.ShowCloseButton = true;
        logWindow.Title = windowTitle;
        logWindow.Width = 400;
        logWindow.Height = 150;
        logWindow.Show("./ShipmentModules/ShipmentPackages/Components/Packages/ContainerFU/ContainerFollowupActionsComponent");
        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                switch (s) {
                    case "Follow": {
                        this.ShowFollowupWindow(typeCode, true);
                        break;
                    }

                    case "Routing": {
                        this.AddRouting(typeCode);
                        break;
                    }                    
                }
            }
        });
    }
    ShowFollowupWindow(typeCode: string, isNewFollowup: boolean) {

        var windowTitle: string;
        switch (typeCode) {
            case "D": {
                windowTitle = isNewFollowup ? "Add Delivery Follow up" : "Edit Delivery Follow up";

                //if (isNewFollowup) {
                //    this.IsDeliveryFU = true;
                //}

                break;
            }

            case "R": {
                windowTitle = isNewFollowup ? "Add Empty Container Return Follow up" : "Edit Empty Container Return Follow up";

                //if (isNewFollowup) {
                //    this.IsEmptyContainerReturnFU = true;
                //}

                break;
            }
        }

        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { DataContext: this, Code: typeCode, IsNewFollowup: isNewFollowup };
        logWindow.Title = windowTitle;
        logWindow.Show("./ShipmentModules/ShipmentPackages/Components/Packages/ContainerFU/ContainerFollowupWindowComponent");
        logWindow.WindowClosed.subscribe(s => {
            if (s) {

            }
        });
    }
    AddRouting(typeCode: string) {

        var myDeliveryIndex = 1;
        var myWindowTitle: string = null;
        var myPickUpDeliveryTypeCode: string = null;
        switch (typeCode) {
            case "R": {
                myPickUpDeliveryTypeCode = "EMPT";

                myWindowTitle = TextCodeTranslator.Translate("Shipment.O.Routings.AddEmptyCR");

                if (this.ShipmentPM.ShipmentContainerReturnIndex) {
                    myDeliveryIndex = this.ShipmentPM.ShipmentContainerReturnIndex + 1;
                }

                break;
            }

            default: {
                myPickUpDeliveryTypeCode = "DELV";

                myWindowTitle = TextCodeTranslator.Translate("Shipment.O.Routings.AddDelivery");

                if (this.ShipmentPM.ShipmentDeliveryIndex) {
                    myDeliveryIndex = this.ShipmentPM.ShipmentDeliveryIndex + 1;
                }

                break;
            }
        }

        var newDeliveryPM = new ShipmentDeliveryPM(null);
        newDeliveryPM.FullResponsibility = true;
        newDeliveryPM.Tenant = this.ShipmentPM.Tenant;
        newDeliveryPM.ShipmentId = this.ShipmentPM.Id;
        newDeliveryPM.ShipmentNumber = this.ShipmentPM.ShipmentNumber;
        newDeliveryPM.PickUpDeliveryNumber = this.ShipmentPM.ShipmentNumber + "/" + myDeliveryIndex;
        newDeliveryPM.PickUpDeliveryTypeCode = myPickUpDeliveryTypeCode;
        newDeliveryPM.ConnectedPackageId = this.EntityPM.Id;

        switch (typeCode) {
            case "D": {
                newDeliveryPM.ETD = this.DeliveryETD;
                newDeliveryPM.ATD = this.DeliveryATD;
                newDeliveryPM.ETA = this.DeliveryETA;
                newDeliveryPM.ATA = this.DeliveryATA;
                break;
            }

            case "R": {
                newDeliveryPM.ETD = this.EmptyContainerReturnETD;
                newDeliveryPM.ATD = this.EmptyContainerReturnATD;
                newDeliveryPM.ETA = this.EmptyContainerReturnETA;
                newDeliveryPM.ATA = this.EmptyContainerReturnATA;
                break;
            }
        }

        var newDeliveryPackagePM = new ShipmentPickUpDeliveryPackagePM(null);
        newDeliveryPackagePM.Tenant = this.EntityPM.Tenant;
        newDeliveryPackagePM.ContainerNumber = this.EntityPM.ContainerNumber;
        newDeliveryPackagePM.Description = this.EntityPM.Description;
        newDeliveryPackagePM.PackageTypeId = this.EntityPM.PackageTypeId;
        newDeliveryPackagePM.PackageTypeName = this.EntityPM.PackageTypeName;
        newDeliveryPackagePM.Quantity = this.EntityPM.Quantity;
        newDeliveryPackagePM.Volume = this.EntityPM.Volume;
        newDeliveryPackagePM.Weight = this.EntityPM.Weight;
        newDeliveryPackagePM.ShipperSeal = this.EntityPM.ShipperSeal;
        newDeliveryPackagePM.Width = this.EntityPM.Width;
        newDeliveryPackagePM.Height = this.EntityPM.Height;
        newDeliveryPackagePM.Length = this.EntityPM.Length;
        newDeliveryPackagePM.Harmonize = this.EntityPM.Harmonize;
        newDeliveryPackagePM.OriginalShipmentPackageId = this.EntityPM.Id;
        newDeliveryPackagePM.IsMultiHarmonize = this.EntityPM.IsMultiHarmonize;

        this.EntityPM.ShipmentPackageHarmonizes.forEach(harmonizeItem => {
            var harmonize = new PickUpDeliveryPackageHarmonizePM(null);
            harmonize.Harmonize = harmonizeItem.Harmonize;
            harmonize.Tenant = harmonizeItem.Tenant;
            newDeliveryPackagePM.AddPickUpDeliveryPackageHarmonizePM(harmonize);
        });

        newDeliveryPM.AddPackage(newDeliveryPackagePM);

        var isCreatingContainerDelivery = false;
        if (typeCode == "D") {
            isCreatingContainerDelivery = true;
        }

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = myWindowTitle;
        logitudeWindow.WindowArgs = { ShipmentPM: this.ShipmentPM, EntityPM: newDeliveryPM, IsNewEntity: true, ContainerReturnDeliveryId: this.DeliveryId, IsCreatingContainerDelivery: isCreatingContainerDelivery };
        logitudeWindow.Width = 950;
        logitudeWindow.Height = 595;

        logitudeWindow.WindowClosed.subscribe(s => {
            if (s) {
             
                // after save must refresh entities
                this.ShipmentPM = this.fatherComponent.EntityPM;

                if (!this.IsNewEntity) {
                    this.EntityPM = this.ShipmentPM.ShipmentPackages.filter(f => f.Id == this.EntityPM.Id)[0];
                }

                this.fatherComponent.SetUIProperties_InsideButton();

                this.SetUIProperties();

                this.InsideItemsSource.forEach(item => {
                    item.SetUIProperties();
                });
            }
        });

        logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditDeliveryComponent');
    }
    EditRouting(typeCode: string) {

        var myDeliveryId: string = null;
        var myWindowTitle: string = null;
        var myEditedDelivery: ShipmentDeliveryPM = null;
        switch (typeCode) {
            case "R": {
                myDeliveryId = this.EmptyContainerReturnId;
                myWindowTitle = TextCodeTranslator.Translate("Shipment.O.Routings.EditEmptyCR");
                break;
            }

            default: {
                myDeliveryId = this.DeliveryId;
                myWindowTitle = TextCodeTranslator.Translate("Shipment.O.Routings.EditDelivery");
                break;
            }
        }

        var myEditedDelivery = this.ShipmentPM.ShipmentDeliveries.filter(f => f.Id == myDeliveryId)[0];

        if (myEditedDelivery) {
            var windowTitle = myWindowTitle + ": " + myEditedDelivery.PickUpDeliveryNumber;
            
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = windowTitle;
            logitudeWindow.WindowArgs = { ShipmentPM: this.ShipmentPM, EntityPM: myEditedDelivery, IsNewEntity: false };
            logitudeWindow.Width = 950;
            logitudeWindow.Height = 595;

            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {

                    // after save must refresh entities
                    this.ShipmentPM = this.fatherComponent.EntityPM;

                    if (!this.IsNewEntity) {
                        this.EntityPM = this.ShipmentPM.ShipmentPackages.filter(f => f.Id == this.EntityPM.Id)[0];
                    }
                }
            });

            logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditDeliveryComponent');
        }
    }
    ChooseCommodityClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 775;
        logitudeWindow.Height = 570;
        logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, FieldName: 'CommodityNumber', NameProperty: 'CommodityName' };
        logitudeWindow.Title = TextCodeTranslator.TranslateTablePlural("Commodity") + " Search";
        logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBChooseCommodityComponent");
    }
    AdvancedDangerousClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 400;
        logitudeWindow.WindowArgs = this.EntityPM;
        logitudeWindow.Title = "Dangerous Goods Advanced";
        logitudeWindow.Show("./ShipmentModules/ShipmentPackages/Components/Packages/AdvancedDangerousGoodsComponent");
    }

    get HasContainerException() { return this.EntityPM.HasContainerException; }
    set HasContainerException(value: boolean) {
        if (this.EntityPM.HasContainerException != value) {
            this.EntityPM.HasContainerException = value;            
        }
    }
}
export class InsideShipmentPackageItem extends BaseComponent {
    public EntityPM: InsideShipmentPackagePM;
    public ShipmentPM: ShipmentPM;
    public ShipmentPackagePM: ShipmentPackagePM;
    public ObjectTableName: string = "InsideShipmentPackage";
    public IsNewEntity: boolean = false;
    public IsVehicleDetails: boolean = false;
    public CountryListService: CountryListService;
    public IsEditingFieldsEnabled: boolean = false;
    constructor(entity: InsideShipmentPackagePM, public fatherComponent: ShipmentPackageItem, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.ShipmentPM = fatherComponent.ShipmentPM;
        this.ShipmentPackagePM = fatherComponent.EntityPM;
        this.IsNewEntity = isNew;
        this.CountryListService = new CountryListService();
        this.IsEditingFieldsEnabled = fatherComponent.IsEditingFieldsEnabled;

        this.SetUIProperties();
        if (this.IsNewEntity) {
            this.SetUIPropertiesOfCars(false);
            this.IsVehicleDetails = false;
        }
    }

    public IsEditingEnabled: boolean = false;
    public SetUIProperties() {

        var isFieldEnabled = false;
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;

        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;

        if (this.fatherComponent.IsConnectedToRouting) {
            this.IsEditingEnabled = false;
        }

        if (this.IsEditingEnabled) {
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

        this.SetUIProperties_Harmonize();

        this.UIProperties.SetEnabled("PackageTypeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Description", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("Weight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("Reference1", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Reference2", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Reference3", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Reference4", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("CommodityNumber", this.ObjectTableName, this.IsEditingEnabled);

        if (!AppTool.IsNullOrEmpty(this.PackageTypeId)) {
            var myService: PackageTypeListService = new PackageTypeListService();
            myService.getSingle(this.PackageTypeId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: PackageTypeList = myResponse.Result;
                    if (list != null) {
                        this.SetUIPropertiesOfCars(this.IsEditingEnabled && list.IsVehicle);
                        this.IsVehicleDetails = list.IsVehicle;
                    }
                }
            });
        }
    }
    private SetUIPropertiesOfCars(isEnabled: boolean) {
        if (this.IsEditingEnabled && !isEnabled) {
            this.Make = null;
            this.Model = null;
            this.Color = null;
            this.Year = null;
            this.CountryId = null;
            this.ChassisNumber = null;
            this.RegistrationNumber = null;
        }
        this.UIProperties.SetEnabled("Make", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Model", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Color", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Year", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("CountryId", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("ChassisNumber", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("RegistrationNumber", this.ObjectTableName, isEnabled);
    }
    SetUIProperties_Harmonize() {

        var isFieldEnabled: boolean = true;
        if (this.IsEditingEnabled) {

            isFieldEnabled = true;

            if (this.IsMultiHarmonize == true) {
                isFieldEnabled = false;
            }
        }

        this.UIProperties.SetEnabled("Harmonize", this.ObjectTableName, isFieldEnabled);
    }

    get Harmonize() { return this.EntityPM.Harmonize; }
    set Harmonize(newValue: string) {
        if (this.EntityPM.Harmonize != newValue) {
            this.EntityPM.Harmonize = newValue;
        }
    }

    get IsMultiHarmonize() { return this.EntityPM.IsMultiHarmonize }
    set IsMultiHarmonize(newValue: boolean) {
        if (this.EntityPM.IsMultiHarmonize != newValue) {
            this.EntityPM.IsMultiHarmonize = newValue;
        }
    }

    get PackageTypeId() { return this.EntityPM.PackageTypeId; }
    set PackageTypeId(newValue: string) {
        if (this.EntityPM.PackageTypeId != newValue) {
            this.EntityPM.PackageTypeId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.PackageTypeName = null;
                this.SetUIPropertiesOfCars(false);
                this.IsVehicleDetails = false;
            }

            else {
                var myService: PackageTypeListService = new PackageTypeListService();
                myService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PackageTypeList = myResponse.Result;
                        if (list != null) {
                            this.PackageTypeName = list.EnglishName;
                            this.SetUIPropertiesOfCars(list.IsVehicle);
                            this.IsVehicleDetails = list.IsVehicle;
                        }
                        else {
                            this.SetUIPropertiesOfCars(false);
                            this.IsVehicleDetails = false;
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
            //this.fatherComponent.ComputeTotals();
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
        var myDimensions: string;

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

            if (!this.IsNewEntity) {
                this.fatherComponent.ComputeFromInsidePackages();
            }
        }
    }

    get VolumetricWeight() { return this.EntityPM.VolumetricWeight; }
    set VolumetricWeight(newValue: number) {
        if (this.EntityPM.VolumetricWeight != newValue) {
            this.EntityPM.VolumetricWeight = AppTool.Round(newValue, 3);
            //this.fatherComponent.ComputeTotals();
        }
    }

    get Weight() { return this.EntityPM.Weight; }
    set Weight(newValue: number) {
        var myValue: number = AppTool.Round(newValue, 3);

        if (this.EntityPM.Weight != myValue) {
            this.EntityPM.Weight = myValue

            if (!this.IsNewEntity) {
                this.fatherComponent.ComputeFromInsidePackages();
            }
        }
    }

    OnGrossWeightLostFocus(input1: number) {
        if (AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {
            if (this.Width == null || this.Height == null || this.Length == null) {
                this.EntityPM.VolumetricWeight = AppTool.GetWeightFromWeight(this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode, this.EntityPM.Weight);
                this.EntityPM.Volume = AppTool.GetVolumeFromWeight(this.ShipmentPM.ChargeableWeightUnitCode, this.ShipmentPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.ShipmentPM.Ratio);

                this.SetUIProperties();

                if (!this.IsNewEntity) {
                    this.fatherComponent.ComputeFromInsidePackages();
                }
            }
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    get Reference1() { return this.EntityPM.Reference1; }
    set Reference1(newValue: string) {
        if (this.EntityPM.Reference1 != newValue) {
            this.EntityPM.Reference1 = newValue;
        }
    }

    get Reference2() { return this.EntityPM.Reference2; }
    set Reference2(newValue: string) {
        if (this.EntityPM.Reference2 != newValue) {
            this.EntityPM.Reference2 = newValue;
        }
    }

    get Reference3() { return this.EntityPM.Reference3; }
    set Reference3(newValue: string) {
        if (this.EntityPM.Reference3 != newValue) {
            this.EntityPM.Reference3 = newValue;
        }
    }

    get Reference4() { return this.EntityPM.Reference4; }
    set Reference4(newValue: string) {
        if (this.EntityPM.Reference4 != newValue) {
            this.EntityPM.Reference4 = newValue;
        }
    }

    get CommodityNumber() { return this.EntityPM.CommodityNumber; }
    set CommodityNumber(newValue: string) {
        if (this.EntityPM.CommodityNumber != newValue) {
            this.EntityPM.CommodityNumber = newValue;

            this.CommodityName = null;
        }
    }

    get CommodityName() { return this.EntityPM.CommodityName; }
    set CommodityName(newValue: string) {
        if (this.EntityPM.CommodityName != newValue) {
            this.EntityPM.CommodityName = newValue;
        }
    }

    private ComputeVolume() {

        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        this.Volume = AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode);
    }
    private ComputeVolumetricWeight() {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        this.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode);
    }

    get Make() { return this.EntityPM.Make; }
    set Make(newValue: string) {
        if (this.EntityPM.Make != newValue) {
            this.EntityPM.Make = newValue;
        }
    }

    get Model() { return this.EntityPM.Model; }
    set Model(newValue: string) {
        if (this.EntityPM.Model != newValue) {
            this.EntityPM.Model = newValue;
        }
    }


    get Year() { return this.EntityPM.Year; }
    set Year(newValue: string) {
        if (this.EntityPM.Year != newValue) {
            this.EntityPM.Year = newValue;
        }
    }

    get Color() { return this.EntityPM.Color; }
    set Color(newValue: string) {
        if (this.EntityPM.Color != newValue) {
            this.EntityPM.Color = newValue;
        }
    }

    get ChassisNumber() { return this.EntityPM.ChassisNumber; }
    set ChassisNumber(newValue: string) {
        if (this.EntityPM.ChassisNumber != newValue) {
            this.EntityPM.ChassisNumber = newValue;
        }
    }

    get RegistrationNumber() { return this.EntityPM.RegistrationNumber; }
    set RegistrationNumber(newValue: string) {
        if (this.EntityPM.RegistrationNumber != newValue) {
            this.EntityPM.RegistrationNumber = newValue;
        }
    }

    get CountryId() { return this.EntityPM.CountryId; }
    set CountryId(newValue: string) {
        if (this.EntityPM.CountryId != newValue) {
            this.EntityPM.CountryId = newValue;
            this.CountryListService.getSingle(this.EntityPM.CountryId).subscribe((result:any) => {
                var country = result.Result;
                if (country != null) {
                    this.EntityPM.CountryCode = country.Code;
                    this.EntityPM.CountryName = country.EnglishName;
                }
            });
        }
    }

    ChooseCommodityClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 775;
        logitudeWindow.Height = 570;
        logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, FieldName: 'CommodityNumber', NameProperty: 'CommodityName' };
        logitudeWindow.Title = TextCodeTranslator.TranslateTablePlural("Commodity") + " Search";
        logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBChooseCommodityComponent");
    }
}
export class PackageItem extends BaseComponent {
    public EntityPM: ShipmentPackageItemPM;
    public ObjectTableName: string = "ShipmentPackageItem";
    public IsNewEntity: boolean = false;


    constructor(entity: ShipmentPackageItemPM, public fatherComponent: ShipmentPackageItem, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
        this.SetButtonHandler();
    }

    private SetButtonHandler() {


    }

    get Description() {
        var myResult = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.Description;
        }

        return myResult;
    }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    get Quantity() {
        var myResult = null;
        if (this.EntityPM != null) {
            myResult = this.EntityPM.Quantity;
        }
        return myResult;
    }
    set Quantity(newValue: number) {
        if (this.EntityPM.Quantity != newValue) {
            this.EntityPM.Quantity = newValue;
        }
    }

    get GoodsValue() {
        var myResult = null;
        if (this.EntityPM != null) {
            myResult = this.EntityPM.GoodsValue;
        }
        return myResult;
    }
    set GoodsValue(newValue: number) {
        if (this.EntityPM.GoodsValue != newValue) {
            this.EntityPM.GoodsValue = newValue;
        }
    }

    RemoveLine(item) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this item ?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                if (this.fatherComponent.EntityPM.ShipmentPackageItems.indexOf(this.EntityPM) != -1) {
                    this.fatherComponent.EntityPM.RemoveShipmentPackageItemPM(this.EntityPM);
                }

                if (this.fatherComponent.PackageItemsList.Collection.indexOf(this) != -1) {
                    this.fatherComponent.PackageItemsList.Remove(this);
                }
            }
        });
    }
}
class MethodItem {
    public Code: string;
    public Name: string;
}
