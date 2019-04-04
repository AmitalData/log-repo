declare var System: any;
declare var window: any;
import {AppTool, FormatTool} from '../../Infrastructure/Tools';
import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import {WarehouseEntryPackagePM} from '../../Warehouse/EntityPMs/WarehouseEntryPackagePM';
import {WarehouseEntryPackagePMExtendedService} from '../../Warehouse/Services/ExtendedPMs/WarehouseEntryPackagePMExtendedService';
import {PackageTypeList} from '../../Common/EntityLists/PackageTypeList';
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ClassLevelValidator} from '../../Infrastructure/Validators/ClassLevelValidator';
import {WarehouseEntryPM} from '../../Warehouse/EntityPMs/WarehouseEntryPM';
import { LogitudeWindow } from '../../Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    selector: 'AddEditWarehouseEntryPackagesAndContainers',
    templateUrl: './AddEditWarehouseEntryPackagesAndContainers.html',
    providers: [WarehouseEntryPackagePMExtendedService],
})

export class AddEditWarehouseEntryPackagesAndContainers implements OnInit {
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public WarningErrorsList: string[] = [];
    public ValidationErrorsList: string[];
    warehouseEntryPM: any;
    warehouseEntryPackageItem: WarehouseEntryPackageItem;
    warehouseEntryPackagePM: WarehouseEntryPackagePM = new WarehouseEntryPackagePM(null);
    ObjectTableName: string = "WarehouseEntryPackage";
    validator: ClassLevelValidator;
    public ContainerNumberWarning: string = null;
    IsNewEntity: boolean = false;
    ObjectTableId: string;
    Type: string;
    ViewModelTrigger: any;
    IsLoadPage: boolean = false;
    public AllPackageTypes: PackageTypeList[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _warehouseEntryPackagePMExtendedService: WarehouseEntryPackagePMExtendedService) {
        this.validator = new ClassLevelValidator();

        var table = window.ObjectTables.filter(d => d.Name == "WarehouseEntryPackage")[0];

        if (table) {
            this.ObjectTableId = table.Id;
        }
    }

    ngOnInit() {


    }

    SetWindowArgs(args: any) {
        this._entityResourceService.getEntityResourceByTableName("WarehouseEntryPackage").subscribe(response => {
            this.Start(args);
        });
    }

    IsParentDirty: boolean = false;
    IsChildDirty: boolean = false;
    public savedItem: WarehouseEntryPackagePM;
    Start(args: any) {
        this.warehouseEntryPM = args.WarehouseEntryPM;
        this.warehouseEntryPackagePM = args.WarehouseEntryPackagePM;
        this.AllPackageTypes = args.AllPackageTypes;

        if (this.warehouseEntryPM && this.warehouseEntryPackagePM) {

            this.ViewModelTrigger = args.ViewModelTrigger;
            this.IsNewEntity = args.IsNewEntity;

            if (this.IsNewEntity) {
                this.warehouseEntryPackagePM.Instock = 0;
                this.warehouseEntryPackagePM.IsContainer = this.warehouseEntryPackagePM.IsContainer;
            }
            else {
                if (this.warehouseEntryPackagePM.IsContainer) {
                    if (this.warehouseEntryPackagePM.ContainerNumber) {
                        this.ValidateContainerNumber(this.warehouseEntryPackagePM.ContainerNumber);
                    }
                }

                if (this.warehouseEntryPackagePM) {
                    this.ContainerNumberLostFocus(this.warehouseEntryPackagePM.ContainerNumber);
                    this.IsParentDirty = this.warehouseEntryPM.IsDirty;
                    this.IsChildDirty = this.warehouseEntryPackagePM.IsDirty;

                    this.savedItem = new WarehouseEntryPackagePM(null);
                    this.savedItem.PackageTypeId = this.warehouseEntryPackagePM.PackageTypeId;
                    this.savedItem.ContainerNumber = this.warehouseEntryPackagePM.ContainerNumber;
                    this.savedItem.Length = this.warehouseEntryPackagePM.Length;
                    this.savedItem.Height = this.warehouseEntryPackagePM.Height;
                    this.savedItem.Width = this.warehouseEntryPackagePM.Width;
                    this.savedItem.Volume = this.warehouseEntryPackagePM.Volume;
                    this.savedItem.Weight = this.warehouseEntryPackagePM.Weight;
                    this.savedItem.Description = this.warehouseEntryPackagePM.Description;
                    this.savedItem.Seal = this.warehouseEntryPackagePM.Seal;
                    this.savedItem.Harmonize = this.warehouseEntryPackagePM.Harmonize;
                    this.savedItem.Location = this.warehouseEntryPackagePM.Location;
                    this.savedItem.Dimensions = this.warehouseEntryPackagePM.Dimensions;
                    this.savedItem.Instock = this.warehouseEntryPackagePM.Instock;
                    this.savedItem.Quantity = this.warehouseEntryPackagePM.Quantity;
                    this.savedItem.ContainerNumberWarning = this.warehouseEntryPackagePM.ContainerNumberWarning;
                }
            }

            this.warehouseEntryPackageItem = new WarehouseEntryPackageItem(this.warehouseEntryPackagePM, this);
            this.IsLoadPage = true;
        }
    }

    ResetPackageItem() {
        if (!this.IsNewEntity) {
            if (this.warehouseEntryPackagePM && this.savedItem) {
                this.warehouseEntryPackagePM.PackageTypeId = this.savedItem.PackageTypeId;
                this.warehouseEntryPackagePM.ContainerNumber = this.savedItem.ContainerNumber;
                this.warehouseEntryPackagePM.Length = this.savedItem.Length;
                this.warehouseEntryPackagePM.Height = this.savedItem.Height;
                this.warehouseEntryPackagePM.Width = this.savedItem.Width;
                this.warehouseEntryPackagePM.Volume = this.savedItem.Volume;
                this.warehouseEntryPackagePM.Weight = this.savedItem.Weight;
                this.warehouseEntryPackagePM.Description = this.savedItem.Description;
                this.warehouseEntryPackagePM.Seal = this.savedItem.Seal;
                this.warehouseEntryPackagePM.Harmonize = this.savedItem.Harmonize;
                this.warehouseEntryPackagePM.Location = this.savedItem.Location;
                this.warehouseEntryPackagePM.Dimensions = this.savedItem.Dimensions;
                this.warehouseEntryPackagePM.Instock = this.savedItem.Instock;
                this.warehouseEntryPackagePM.Quantity = this.savedItem.Quantity;
                this.warehouseEntryPackagePM.ContainerNumberWarning = this.savedItem.ContainerNumberWarning;
            }

            if (this.warehouseEntryPM && !this.IsParentDirty && this.warehouseEntryPM.IsDirty) {
                this.warehouseEntryPM.IsDirty = this.IsParentDirty;
            }

            if (this.warehouseEntryPackagePM && !this.IsChildDirty && this.warehouseEntryPackagePM.IsDirty) {
                this.warehouseEntryPackagePM.IsDirty = this.IsChildDirty;
            }
        }
    }

    CanceluttonClicked() {
        this.ResetPackageItem();
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveButtonClicked() {
        this.ValidationErrorsList = [];

        var errorsArray = this.validator.Validate("WarehouseEntryPackage", this.warehouseEntryPackagePM);
        if (errorsArray.length > 0) {
            errorsArray.forEach((item) => {
                this.ValidationErrorsList.push(item);
            });
        }

        if (this.ValidationErrorsList.length == 0) {
            if (this.warehouseEntryPackagePM.IsDirty) {
                this.ComplateSave();
            }

            else {
                this.CurrentSession.CloseCurrentWindow();
            }
        }
    }

    ComplateSave() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");

        this.warehouseEntryPackagePM.Dimensions = this.warehouseEntryPackageItem ? this.warehouseEntryPackageItem.Dimensions : "";
        this.warehouseEntryPackagePM.PackageTypeName = "";

        this.warehouseEntryPackagePM.Instock = this.warehouseEntryPackagePM.Quantity;

        if (this.AllPackageTypes) {
            var packageTypeList: PackageTypeList = this.AllPackageTypes.filter(d => d.Id == this.warehouseEntryPackagePM.PackageTypeId)[0];
            if (packageTypeList) this.warehouseEntryPackagePM.PackageTypeName = packageTypeList.EnglishName;
        }

        if (this.IsNewEntity && this.ViewModelTrigger.WarehouseEntryPackagesLists) {

            this.ViewModelTrigger.WarehouseEntryPackagesLists.push(this.warehouseEntryPackagePM);
        }

        this.CurrentSession.CurrentWindow.StopBusyIndicator();
        this.CurrentSession.CurrentWindow.Close("Refresh");
    }

    IsEnable: boolean = false;
    ContainerNumberLostFocus(input: string) {
        this.ValidateContainerNumber(input);
    }

    ValidateContainerNumber(input: string) {
        var warnings: string[] = [];
        var error = FormatTool.ValidateContainerNumber(input);

        if (!AppTool.IsNullOrEmpty(error)) {
            warnings.push(error);

            if (AppTool.IsNullOrEmpty(this.warehouseEntryPackagePM.ContainerNumberWarning)) {
                this.warehouseEntryPackagePM.ContainerNumberWarning = error;
            }
        }

        else {
            if (!AppTool.IsNullOrEmpty(this.warehouseEntryPackagePM.ContainerNumberWarning)) {
                this.warehouseEntryPackagePM.ContainerNumberWarning = null;
            }
        }

        this.WarningErrorsList = warnings;
    }
}

export class WarehouseEntryPackageItem extends BaseComponent {
    EntityPM: WarehouseEntryPackagePM;
    VolumeLabel: string;
    VolumetricWeightLabel: string;
    GrossWeightLabel: string;
    DimensionsLabel: string;
    ObjectTableName: string = "WarehouseEntryPackage";
    FatherComponent: AddEditWarehouseEntryPackagesAndContainers;
    DimensionsUnitCode: string;
    VolumeUnitCode: string;
    ChargeableWeightUnitCode: string;
    GrossWeightUnitCode: string;
    IsDependencyFilter2Value: boolean;
    IsContainer: boolean = false;
    WarehouseEntryPM: WarehouseEntryPM;
    constructor(entity: WarehouseEntryPackagePM, public fatherComponent: AddEditWarehouseEntryPackagesAndContainers = null) {
        super();
        this.FatherComponent = fatherComponent;
        this.EntityPM = entity;
        this.WarehouseEntryPM = this.FatherComponent.warehouseEntryPM;       
        this.VolumeUnitCode = this.WarehouseEntryPM.VolumeUnitCode;
        this.GrossWeightUnitCode = this.WarehouseEntryPM.GrossWeightUnitCode;
        this.ChargeableWeightUnitCode = this.WarehouseEntryPM.ChargeableWeightUnitCode;
        this.DimensionsUnitCode = this.WarehouseEntryPM.DimensionsUnitCode;

        this.IsDependencyFilter2Value = this.IsContainer= this.EntityPM.IsContainer;   
    
        this.SetLabel();
        this.SetUIProperties();
    }
    
    SetLabel() {
        this.VolumeLabel = "Volume (" + this.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Weight (" + this.GrossWeightUnitCode + ")";
        this.DimensionsLabel = "Dimensions(L-W-H) (" + this.DimensionsUnitCode + ")";
        this.VolumetricWeightLabel = "Volumetric Weight (" + this.ChargeableWeightUnitCode + ")";
    }

    SetUIProperties() {

        if (this.EntityPM.IsContainer) {
            if (this.FatherComponent.IsNewEntity) {
                this.EntityPM.Quantity = 1;
            }
            this.UIProperties.SetEnabled("Quantity", "WarehouseEntryPackage", false)

        }

            var isVolumeEnabled: boolean = false;
            var isDimensionEnabled: boolean = false;
            var isGrossWeightEnabled: boolean = false;

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

            this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
            this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
            this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
            this.UIProperties.SetEnabled("Weight", this.ObjectTableName, isGrossWeightEnabled);
            this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
    }

    ComputeVolume() {

        this.Volume= AppTool.GetVolumeFromDimentions(this.DimensionsUnitCode, this.VolumeUnitCode, this.Width, this.Height, this.Length, this.Quantity);
        

    }

    private ComputeVolumetricWeight() {
        var ratio = AppTool.GetRatio(this.WarehouseEntryPM.DirectionId, this.WarehouseEntryPM.TransportModeId, this.WarehouseEntryPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);

        this.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.Weight, ratio, this.DimensionsUnitCode, this.VolumeUnitCode, this.GrossWeightUnitCode, this.ChargeableWeightUnitCode);
    }


    // Dimensions
    get Quantity() { return this.EntityPM.Quantity; }
    set Quantity(newValue: number) {
        if (this.EntityPM.Quantity != newValue) {
            this.EntityPM.Quantity = AppTool.Round(newValue, 0);
            this.ComputeVolume();
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
        if (!this.IsContainer) {
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
           // this.fatherComponent.ComputeTotals();
        }
    }

    get Weight() { return this.EntityPM.Weight; }
    set Weight(newValue: number) {
        var myValue: number = AppTool.Round(newValue, 3);

        if (this.EntityPM.Weight != myValue) {
            this.EntityPM.Weight = myValue

            //if (this.ShipmentPM.TransportModeId != "A") {
            //    this.UIProperties.SetRequired('Weight', this.ObjectTableName, AppTool.IsNullOrEmpty(this.Weight) ? true : false);

            //}

            //this.fatherComponent.ResetTotalEditedValues();
            //this.fatherComponent.ComputeTotals();
        }
    }


    get PackageTypeId() { return this.EntityPM.PackageTypeId; }
    set PackageTypeId(value: string) {
        if (this.EntityPM.PackageTypeId != value) {
            this.EntityPM.PackageTypeId = value;
        }
    }



    get Description() { return this.EntityPM.Description; }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }

    get Location() { return this.EntityPM.Location; }
    set Location(value: string) {
        if (this.EntityPM.Location != value) {
            this.EntityPM.Location = value;
        }
    }

    get Harmonize() { return this.EntityPM.Harmonize; }
    set Harmonize(value: string) {
        if (this.EntityPM.Harmonize != value) {
            this.EntityPM.Harmonize = value;
        }
    }

    get Seal() { return this.EntityPM.Seal; }
    set Seal(value: string) {
        if (this.EntityPM.Seal != value) {
            this.EntityPM.Seal = value;
        }
    }

    get CommodityNumber() { return this.EntityPM.CommodityNumber; }
    set CommodityNumber(value: string) {
        if (this.EntityPM.CommodityNumber != value) {
            this.EntityPM.CommodityNumber = value;
        }
    }

    ChooseCommodityClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 775;
        logitudeWindow.Height = 570;
        logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, FieldName: 'CommodityNumber' };
        logitudeWindow.Title = TextCodeTranslator.TranslateTablePlural("Commodity") + " Search";
        logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBChooseCommodityComponent");        
    } 
}
