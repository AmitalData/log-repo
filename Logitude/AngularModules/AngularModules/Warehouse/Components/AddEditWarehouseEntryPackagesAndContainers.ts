declare var System: any;
declare var window: any;
import {AppTool, FormatTool} from '../../Infrastructure/Tools';

import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';

import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';

import {WarehouseEntryPackagePM} from '../../Warehouse/EntityPMs/WarehouseEntryPackagePM';
import {NewWarehouseEntryComponent} from '../../Warehouse/Components/NewWarehouseEntryComponent';
import {WarehouseEntryPackagePMExtendedService} from '../../Warehouse/Services/ExtendedPMs/WarehouseEntryPackagePMExtendedService';
import {PackageTypeList} from '../../Common/EntityLists/PackageTypeList';
import {EventTypeArgs} from '../../Infrastructure/DataContracts/EventTypeArgs';

import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ClassLevelValidator} from '../../Infrastructure/Validators/ClassLevelValidator';
import {WarehouseEntryPM} from '../../Warehouse/EntityPMs/WarehouseEntryPM';

import { PackageTypeListService } from '../../Common/Services/StandardLists/PackageTypeListService';



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
    constructor(private _warehouseEntryPackagePMExtendedService: WarehouseEntryPackagePMExtendedService) {
        this.validator = new ClassLevelValidator();

        var table = window.ObjectTables.filter(d=> d.Name == "WarehouseEntryPackage")[0];
        if (table) this.ObjectTableId = table.Id;


    }

    ngOnInit(

    ) {


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
                    this.savedItem.Make = this.warehouseEntryPackagePM.Make;
                    this.savedItem.Year = this.warehouseEntryPackagePM.Year;
                    this.savedItem.ChassisNumber = this.warehouseEntryPackagePM.ChassisNumber;
                    this.savedItem.RegistrationNumber = this.warehouseEntryPackagePM.RegistrationNumber;
                    this.savedItem.CountryId = this.warehouseEntryPackagePM.CountryId;
                    this.savedItem.Model = this.warehouseEntryPackagePM.Model;
                    this.savedItem.Color = this.warehouseEntryPackagePM.Color;
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
                this.warehouseEntryPackagePM.Make = this.savedItem.Make;
                this.warehouseEntryPackagePM.Year = this.savedItem.Year;
                this.warehouseEntryPackagePM.ChassisNumber = this.savedItem.ChassisNumber;
                this.warehouseEntryPackagePM.RegistrationNumber = this.savedItem.RegistrationNumber;
                this.warehouseEntryPackagePM.CountryId = this.savedItem.CountryId;
                this.warehouseEntryPackagePM.Model = this.savedItem.Model;
                this.warehouseEntryPackagePM.Color = this.savedItem.Color;
            }

            if (this.warehouseEntryPM && !this.IsParentDirty && this.warehouseEntryPM.IsDirty) this.warehouseEntryPM.IsDirty = this.IsParentDirty;
            if (this.warehouseEntryPackagePM && !this.IsChildDirty && this.warehouseEntryPackagePM.IsDirty) this.warehouseEntryPackagePM.IsDirty = this.IsChildDirty;
        }
    }


    CanceluttonClicked() {
        this.ResetPackageItem();
        SessionLocator.CurrentSession.CloseCurrentWindow();
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

         else   SessionLocator.CurrentSession.CloseCurrentWindow();

        }


    }

    ComplateSave() {

        SessionLocator.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");

        //if (!this.warehouseEntryPackagePM.IsContainer) {

        //    var height: string = this.warehouseEntryPackagePM.Height != null ? this.warehouseEntryPackagePM.Height.toString() : "";
        //    var width: string = this.warehouseEntryPackagePM.Width != null ? this.warehouseEntryPackagePM.Width.toString() : "";
        //    var length: string = this.warehouseEntryPackagePM.Length != null ? this.warehouseEntryPackagePM.Length.toString() : "";

        //    this.warehouseEntryPackagePM.Dimensions = length + "-" + width + "-" + height;
        //}


        this.warehouseEntryPackagePM.Dimensions = this.warehouseEntryPackageItem ? this.warehouseEntryPackageItem.Dimensions:"";
        this.warehouseEntryPackagePM.PackageTypeName = "";

        this.warehouseEntryPackagePM.Instock = this.warehouseEntryPackagePM.Quantity;

        if (this.AllPackageTypes) {
            var packageTypeList: PackageTypeList = this.AllPackageTypes.filter(d => d.Id == this.warehouseEntryPackagePM.PackageTypeId)[0];
            if (packageTypeList) this.warehouseEntryPackagePM.PackageTypeName = packageTypeList.EnglishName;
        }

        if (this.IsNewEntity && this.ViewModelTrigger.WarehouseEntryPackagesLists) {

            this.ViewModelTrigger.WarehouseEntryPackagesLists.push(this.warehouseEntryPackagePM);
        }

        SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
        SessionLocator.CurrentSession.CurrentWindow.Close("Refresh");

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
        this.IsDependencyFilter2Value = this.IsContainer = this.EntityPM.IsContainer;
        this.SetLabel();
        this.SetUIProperties();
        this.SetUIPropertiesOfCars(false);
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

        if (!AppTool.IsNullOrEmpty(this.PackageTypeId)) {
            var myService: PackageTypeListService = new PackageTypeListService();
            myService.getSingle(this.PackageTypeId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: PackageTypeList = myResponse.Result;
                    if (list != null) {
                        this.SetUIPropertiesOfCars(list.IsVehicle);
                    }
                }
            });
        }

        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("Weight", this.ObjectTableName, isGrossWeightEnabled);
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
    }

    ComputeVolume() {

        this.Volume = AppTool.GetVolumeFromDimentions(this.DimensionsUnitCode, this.VolumeUnitCode, this.Width, this.Height, this.Length, this.Quantity);


    }

    private ComputeVolumetricWeight() {
        var ratio = AppTool.GetRatio(this.WarehouseEntryPM.DirectionId, this.WarehouseEntryPM.TransportModeId, this.WarehouseEntryPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);

        this.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.Weight, ratio, this.DimensionsUnitCode, this.VolumeUnitCode, this.GrossWeightUnitCode, this.ChargeableWeightUnitCode);
    }


    // Dimensions
    get Quantity() { return this.EntityPM.Quantity; }
    set Quantity(value: number) {
        if (this.EntityPM.Quantity != value) {
            this.EntityPM.Quantity = AppTool.Round(value, 0);
            this.ComputeVolume();
            this.SetUIProperties();
        }
    }

    get Length() { return this.EntityPM.Length; }
    set Length(value: number) {
        if (this.EntityPM.Length != value) {
            this.EntityPM.Length = AppTool.Round(value, 2);
            this.ComputeVolume();
            this.SetUIProperties();
        }
    }

    get Width() { return this.EntityPM.Width; }
    set Width(value: number) {
        if (this.EntityPM.Width != value) {
            this.EntityPM.Width = AppTool.Round(value, 2);
            this.ComputeVolume();
            this.SetUIProperties();
        }
    }

    get Height() { return this.EntityPM.Height; }
    set Height(value: number) {
        if (this.EntityPM.Height != value) {
            this.EntityPM.Height = AppTool.Round(value, 2);
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
    set Volume(value: number) {
        if (this.EntityPM.Volume != value) {
            this.EntityPM.Volume = AppTool.Round(value, 3);
            this.ComputeVolumetricWeight();
            this.SetUIProperties();
        }
    }

    get VolumetricWeight() { return this.EntityPM.VolumetricWeight; }
    set VolumetricWeight(value: number) {
        if (this.EntityPM.VolumetricWeight != value) {
            this.EntityPM.VolumetricWeight = AppTool.Round(value, 3);
            // this.fatherComponent.ComputeTotals();
        }
    }

    get Weight() { return this.EntityPM.Weight; }
    set Weight(value: number) {
        var myValue: number = AppTool.Round(value, 3);

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
            if (AppTool.IsNullOrEmpty(value)) {
                this.SetUIPropertiesOfCars(false);
            }
            else {
                var myService: PackageTypeListService = new PackageTypeListService();
                myService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PackageTypeList = myResponse.Result;
                        if (list != null) {
                            this.SetUIPropertiesOfCars(list.IsVehicle);
                            if (!list.IsVehicle) {
                                this.Make = null;
                                this.Model = null;
                                this.Color = null;
                                this.Year = null;
                                this.CountryId = null;
                                this.ChassisNumber = null;
                                this.RegistrationNumber = null;
                            }
                        }
                    }
                });
            }
        }
    }

    private SetUIPropertiesOfCars(isEnabled: boolean) {
        this.UIProperties.SetEnabled("Make", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Model", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Color", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Year", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("CountryId", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("ChassisNumber", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("RegistrationNumber", this.ObjectTableName, isEnabled);
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


    get Make() { return this.EntityPM.Make; }
    set Make(value: string) {
        if (this.EntityPM.Make != value) {
            this.EntityPM.Make = value;
        }
    }

    get Model() { return this.EntityPM.Model; }
    set Model(value: string) {
        if (this.EntityPM.Model != value) {
            this.EntityPM.Model = value;
        }
    }


    get Year() { return this.EntityPM.Year; }
    set Year(value: string) {
        if (this.EntityPM.Year != value) {
            this.EntityPM.Year = value;
        }
    }

    get Color() { return this.EntityPM.Color; }
    set Color(value: string) {
        if (this.EntityPM.Color != value) {
            this.EntityPM.Color = value;
        }
    }

    get ChassisNumber() { return this.EntityPM.ChassisNumber; }
    set ChassisNumber(value: string) {
        if (this.EntityPM.ChassisNumber != value) {
            this.EntityPM.ChassisNumber = value;
        }
    }

    get RegistrationNumber() { return this.EntityPM.RegistrationNumber; }
    set RegistrationNumber(value: string) {
        if (this.EntityPM.RegistrationNumber != value) {
            this.EntityPM.RegistrationNumber = value;
        }
    }

    get CountryId() { return this.EntityPM.CountryId; }
    set CountryId(value: string) {
        if (this.EntityPM.CountryId != value) {
            this.EntityPM.CountryId = value;
        }
    }
}
