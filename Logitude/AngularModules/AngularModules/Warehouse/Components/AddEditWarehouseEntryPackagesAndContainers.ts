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
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import { PackageTypeListService } from '../../Common/Services/StandardLists/PackageTypeListService';



@Component({
    
    selector: 'AddEditWarehouseEntryPackagesAndContainers',
    templateUrl: './AddEditWarehouseEntryPackagesAndContainers.html',
    providers: [WarehouseEntryPackagePMExtendedService],
})

export class AddEditWarehouseEntryPackagesAndContainers implements OnInit {
  public IsDependencyFilter2Value: any;
  public WarehouseEntryPackage: any;

    private _entityResourceService: EntityResourceService = new EntityResourceService();
   
    public ValidationErrorsList: string[];
    warehouseEntryPM: any;
    warehouseEntryPackageItem: WarehouseEntryPackageItem;

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
        this._entityResourceService.getEntityResourceByTableName("WarehouseEntryPackage").subscribe((response:any) => {
            this.Start(args);
        });
    }

    IsParentDirty: boolean = false;
    IsChildDirty: boolean = false;
    public savedItem: WarehouseEntryPackageItem;
    Start(args: any) {
        this.warehouseEntryPM = args.WarehouseEntryPM;
        this.warehouseEntryPackageItem = args.WarehouseEntryPackageItem;


        this.AllPackageTypes = args.AllPackageTypes;

        if (this.warehouseEntryPM && this.warehouseEntryPackageItem) {

            this.ViewModelTrigger = args.ViewModelTrigger;
            this.IsNewEntity = args.IsNewEntity;


            if (this.IsNewEntity) {
                this.warehouseEntryPackageItem.Instock = 0;
            }
            else {
                if (this.warehouseEntryPackageItem.IsContainer) {
                    if (this.warehouseEntryPackageItem.ContainerNumber) {
                        this.warehouseEntryPackageItem.ValidateContainerNumber(this.warehouseEntryPackageItem.ContainerNumber);
                    }
                }

                if (this.warehouseEntryPackageItem) {
                    this.warehouseEntryPackageItem.ContainerNumberLostFocus(this.warehouseEntryPackageItem.ContainerNumber);
                    this.IsParentDirty = this.warehouseEntryPM.IsDirty;
                    this.IsChildDirty = this.warehouseEntryPackageItem.IsDirty;


                    this.savedItem = new WarehouseEntryPackageItem(new WarehouseEntryPackagePM(null),this);
                    this.savedItem.PackageTypeId = this.warehouseEntryPackageItem.PackageTypeId;
                    this.savedItem.ContainerNumber = this.warehouseEntryPackageItem.ContainerNumber;
                    this.savedItem.Length = this.warehouseEntryPackageItem.Length;
                    this.savedItem.Height = this.warehouseEntryPackageItem.Height;
                    this.savedItem.Width = this.warehouseEntryPackageItem.Width;
                    this.savedItem.Volume = this.warehouseEntryPackageItem.Volume;
                    this.savedItem.Weight = this.warehouseEntryPackageItem.Weight;
                    this.savedItem.Description = this.warehouseEntryPackageItem.Description;
                    this.savedItem.Seal = this.warehouseEntryPackageItem.Seal;
                    this.savedItem.Harmonize = this.warehouseEntryPackageItem.Harmonize;
                    this.savedItem.Location = this.warehouseEntryPackageItem.Location;
                    this.savedItem.Dimensions = this.warehouseEntryPackageItem.Dimensions;
                    this.savedItem.Instock = this.warehouseEntryPackageItem.Instock;
                    this.savedItem.Quantity = this.warehouseEntryPackageItem.Quantity;
                    this.savedItem.Make = this.warehouseEntryPackageItem.Make;
                    this.savedItem.Year = this.warehouseEntryPackageItem.Year;
                    this.savedItem.ChassisNumber = this.warehouseEntryPackageItem.ChassisNumber;
                    this.savedItem.RegistrationNumber = this.warehouseEntryPackageItem.RegistrationNumber;
                    this.savedItem.CountryId = this.warehouseEntryPackageItem.CountryId;
                    this.savedItem.Model = this.warehouseEntryPackageItem.Model;
                    this.savedItem.Color = this.warehouseEntryPackageItem.Color;
                    this.savedItem.ContainerNumberWarning = this.warehouseEntryPackageItem.ContainerNumberWarning;



                    if (!this.warehouseEntryPackageItem.Quantity || this.warehouseEntryPackageItem.Quantity == 0) {
                        this.warehouseEntryPackageItem.Instock = 0;
                    }

                }
            }


            this.IsLoadPage = true;
        }
    }


    ComputeAndFullTotalPackage() {

    }
    ResetPackageItem() {
        if (!this.IsNewEntity) {
            if (this.warehouseEntryPackageItem && this.savedItem) {
                this.warehouseEntryPackageItem.PackageTypeId = this.savedItem.PackageTypeId;
                this.warehouseEntryPackageItem.ContainerNumber = this.savedItem.ContainerNumber;
                this.warehouseEntryPackageItem.Length = this.savedItem.Length;
                this.warehouseEntryPackageItem.Height = this.savedItem.Height;
                this.warehouseEntryPackageItem.Width = this.savedItem.Width;
                this.warehouseEntryPackageItem.Volume = this.savedItem.Volume;
                this.warehouseEntryPackageItem.Weight = this.savedItem.Weight;
                this.warehouseEntryPackageItem.Description = this.savedItem.Description;
                this.warehouseEntryPackageItem.Seal = this.savedItem.Seal;
                this.warehouseEntryPackageItem.Harmonize = this.savedItem.Harmonize;
                this.warehouseEntryPackageItem.Location = this.savedItem.Location;
                this.warehouseEntryPackageItem.Dimensions = this.savedItem.Dimensions;
                this.warehouseEntryPackageItem.Instock = this.savedItem.Instock;
                this.warehouseEntryPackageItem.Quantity = this.savedItem.Quantity;
                this.warehouseEntryPackageItem.ContainerNumberWarning = this.savedItem.ContainerNumberWarning;
                this.warehouseEntryPackageItem.Make = this.savedItem.Make;
                this.warehouseEntryPackageItem.Year = this.savedItem.Year;
                this.warehouseEntryPackageItem.ChassisNumber = this.savedItem.ChassisNumber;
                this.warehouseEntryPackageItem.RegistrationNumber = this.savedItem.RegistrationNumber;
                this.warehouseEntryPackageItem.CountryId = this.savedItem.CountryId;
                this.warehouseEntryPackageItem.Model = this.savedItem.Model;
                this.warehouseEntryPackageItem.Color = this.savedItem.Color;
            }

            if (this.warehouseEntryPM && !this.IsParentDirty && this.warehouseEntryPM.IsDirty) {
                this.warehouseEntryPM.IsDirty = this.IsParentDirty;
            }

            if (this.warehouseEntryPackageItem && !this.IsChildDirty && this.warehouseEntryPackageItem.IsDirty) {
                this.warehouseEntryPackageItem.IsDirty = this.IsChildDirty;
            }
        }
    }

    CanceluttonClicked() {
        this.ResetPackageItem();
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveButtonClicked() {
        this.ValidationErrorsList = [];

        var errorsArray = this.validator.Validate("WarehouseEntryPackage", this.warehouseEntryPackageItem.EntityPM);
        if (errorsArray.length > 0) {
            errorsArray.forEach((item) => {
                this.ValidationErrorsList.push(item);
            });
        }

        if (this.ValidationErrorsList.length == 0) {
            if (this.warehouseEntryPackageItem.IsDirty) {
                this.ComplateSave();
            }

            else {
                this.CurrentSession.CloseCurrentWindow();
            }
        }
    }

    ComplateSave() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
        this.warehouseEntryPackageItem.EntityPM.Dimensions = this.warehouseEntryPackageItem ? this.warehouseEntryPackageItem.Dimensions : "";

        this.warehouseEntryPackageItem.PackageTypeName = "";

        this.warehouseEntryPackageItem.Instock = this.warehouseEntryPackageItem.Quantity;

        if (this.AllPackageTypes) {
            var packageTypeList: PackageTypeList = this.AllPackageTypes.filter(d => d.Id == this.warehouseEntryPackageItem.PackageTypeId)[0];
            if (packageTypeList) this.warehouseEntryPackageItem.PackageTypeName = packageTypeList.EnglishName;
        }

        if (this.IsNewEntity && this.ViewModelTrigger.WarehouseEntryPackagesLists) {

            this.ViewModelTrigger.WarehouseEntryPackagesLists.push(this.warehouseEntryPackageItem.EntityPM);
        }

        this.CurrentSession.CurrentWindow.StopBusyIndicator();
        this.CurrentSession.CurrentWindow.Close("Refresh");
    }

    IsEnable: boolean = false;

}

export class WarehouseEntryPackageItem extends BaseComponent {
    EntityPM: WarehouseEntryPackagePM;
    VolumeLabel: string;
    VolumetricWeightLabel: string;
    GrossWeightLabel: string;
    DimensionsLabel: string;
    ReleasesNumber: string;




    ObjectTableName: string = "WarehouseEntryPackage";
    FatherComponent: any;
    DimensionsUnitCode: string;
    VolumeUnitCode: string;
    ChargeableWeightUnitCode: string;
    GrossWeightUnitCode: string;
    IsDependencyFilter2Value: boolean;
    IsCFSWarehouse: boolean = false;
    WarehouseEntryPM: WarehouseEntryPM;
    public IsVehicleDetails: boolean = false;

    constructor(entity: WarehouseEntryPackagePM, public fatherComponent: any = null) {
        super();
        this.FatherComponent = fatherComponent;
        this.EntityPM = entity;
        this.WarehouseEntryPM = this.FatherComponent.warehouseEntryPM;
        this.VolumeUnitCode = this.WarehouseEntryPM.VolumeUnitCode;
        this.GrossWeightUnitCode = this.WarehouseEntryPM.GrossWeightUnitCode;
        this.ChargeableWeightUnitCode = this.WarehouseEntryPM.ChargeableWeightUnitCode;
        this.DimensionsUnitCode = this.WarehouseEntryPM.DimensionsUnitCode;
        this.IsDependencyFilter2Value = this.EntityPM.IsContainer;
        this.ReleasesNumber = this.EntityPM.ReleasesNumber;
        this.IsCFSWarehouse = this.FatherComponent.IsCFSWarehouse;

        this.SetLabel();
        this.SetUIProperties();
        this.SetUIPropertiesOfCars(false);
        this.IsVehicleDetails = false;
    }

    SetLabel() {
        this.VolumeLabel = "Volume (" + this.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Weight (" + this.GrossWeightUnitCode + ")";
        this.DimensionsLabel = "Dimensions(L-W-H) (" + this.DimensionsUnitCode + ")";
        this.VolumetricWeightLabel = "Volumetric Weight (" + this.ChargeableWeightUnitCode + ")";
    }



    IsVolumeEnabled: boolean = true;
    IsGrossWeightEnabled: boolean = true;
    IsDimensionEnabled: boolean = true;
    IsQuantityEnabled: boolean = true;
    SetUIProperties() {

        if (this.EntityPM.IsContainer) {
            if (!this.EntityPM.Id) {
                this.EntityPM.Quantity = 1;
            }
            this.UIProperties.SetEnabled("Quantity", "WarehouseEntryPackage", false)
            this.IsQuantityEnabled = false;

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
                        this.IsVehicleDetails = list.IsVehicle;
                    }
                }
            });
        }

        this.IsVolumeEnabled = isVolumeEnabled; 
        this.IsGrossWeightEnabled = isGrossWeightEnabled; 
        this.IsDimensionEnabled = isDimensionEnabled; 


        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("Weight", this.ObjectTableName, isGrossWeightEnabled);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, !this.IsCFSWarehouse);
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
    }

    ComputeVolume() {

        this.Volume = AppTool.GetVolumeFromDimentions(this.DimensionsUnitCode, this.VolumeUnitCode, this.Width, this.Height, this.Length, this.Quantity);


    }

    private ComputeVolumetricWeight() {
      

        this.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.Weight, this.WarehouseEntryPM.Ratio, this.DimensionsUnitCode, this.VolumeUnitCode, this.GrossWeightUnitCode, this.ChargeableWeightUnitCode);
    }

    WarningErrorsList: string[] = [];

 


    // Dimensions
    get Quantity() { return this.EntityPM.Quantity; }
    set Quantity(value: number) {
        if (this.EntityPM.Quantity != value) {
            this.EntityPM.Quantity = AppTool.Round(value, 0);
            this.Instock = this.EntityPM.Quantity;
            this.ComputeVolume();
            this.SetUIProperties();
            this.FatherComponent.ComputeAndFullTotalPackage();
        }
    }

    get Length() { return this.EntityPM.Length; }
    set Length(value: number) {
        if (this.EntityPM.Length != value) {
            this.EntityPM.Length = AppTool.Round(value, 2);
            this.ComputeVolume();
            this.SetUIProperties();
            this.FatherComponent.ComputeAndFullTotalPackage();
        }
    }

    get Width() { return this.EntityPM.Width; }
    set Width(value: number) {
        if (this.EntityPM.Width != value) {
            this.EntityPM.Width = AppTool.Round(value, 2);
            this.ComputeVolume();
            this.SetUIProperties();
            this.FatherComponent.ComputeAndFullTotalPackage();
        }
    }

    get Height() { return this.EntityPM.Height; }
    set Height(value: number) {
        if (this.EntityPM.Height != value) {
            this.EntityPM.Height = AppTool.Round(value, 2);
            this.ComputeVolume();
            this.SetUIProperties();
            this.FatherComponent.ComputeAndFullTotalPackage();
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
    set Dimensions(value: string) {
        if (this.EntityPM.Dimensions != value) {
            this.EntityPM.Dimensions = value;
 
        }
    }

    get Volume() { return this.EntityPM.Volume; }
    set Volume(value: number) {
        if (this.EntityPM.Volume != value) {
            this.EntityPM.Volume = AppTool.Round(value, 3);
            this.ComputeVolumetricWeight();
            this.SetUIProperties();
            this.FatherComponent.ComputeAndFullTotalPackage();
        }
    }

    get VolumetricWeight() { return this.EntityPM.VolumetricWeight; }
    set VolumetricWeight(value: number) {
        if (this.EntityPM.VolumetricWeight != value) {
            this.EntityPM.VolumetricWeight = AppTool.Round(value, 3);
            // this.fatherComponent.ComputeTotals();
            this.FatherComponent.ComputeAndFullTotalPackage();
        }
    }

    get Weight() { return this.EntityPM.Weight; }
    set Weight(value: number) {
        var myValue: number = AppTool.Round(value, 3);

        if (this.EntityPM.Weight != myValue) {
            this.EntityPM.Weight = myValue
            this.FatherComponent.ComputeAndFullTotalPackage();
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
                this.IsVehicleDetails =false;
            }
            else {
                var myService: PackageTypeListService = new PackageTypeListService();
                myService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PackageTypeList = myResponse.Result;
                        if (list != null) {
                            this.SetUIPropertiesOfCars(list.IsVehicle);
                            this.IsVehicleDetails = list.IsVehicle;
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

    get PackageTypeName() { return this.EntityPM.PackageTypeName; }
    set PackageTypeName(value: string) {
        if (this.EntityPM.PackageTypeName != value) {
            this.EntityPM.PackageTypeName = value;
        }
    }


    get Instock() { return this.EntityPM.Instock; }
    set Instock(value: number) {
        if (this.EntityPM.Instock != value) {
            this.EntityPM.Instock = value;
         
        }
    }

    get ContainerNumber() { return this.EntityPM.ContainerNumber; }
    set ContainerNumber(value: string) {
        if (this.EntityPM.ContainerNumber != value) {
            this.EntityPM.ContainerNumber = value;
        }
    }
    

    get ContainerNumberWarning() { return this.EntityPM.ContainerNumberWarning; }
    set ContainerNumberWarning(value: string) {
        if (this.EntityPM.ContainerNumberWarning != value) {
            this.EntityPM.ContainerNumberWarning = value;
        }
    }




    
    get IsContainer() { return this.EntityPM.IsContainer; }
    set IsContainer(value: boolean) {
        if (this.EntityPM.IsContainer != value) {
            this.EntityPM.IsContainer = value;
        }
    }

    


    get IsDirty() { return this.EntityPM.IsDirty; }
    set IsDirty(value: boolean) {
        if (this.EntityPM.IsDirty != value) {
            this.EntityPM.IsDirty = value;
        }
    }


    get IsDisabled() { return this.EntityPM.Quantity != this.EntityPM.Instock ? true:false }



    ContainerNumberLostFocus(input: string) {
        this.ValidateContainerNumber(input);
    }

    ValidateContainerNumber(input: string) {
        var warnings: string[] = [];
        var error = FormatTool.ValidateContainerNumber(input);

        if (!AppTool.IsNullOrEmpty(error)) {
            warnings.push(error);

            if (AppTool.IsNullOrEmpty(this.ContainerNumberWarning)) {
                this.ContainerNumberWarning = error;
            }
        }

        else {
            if (!AppTool.IsNullOrEmpty(this.ContainerNumberWarning)) {
                this.ContainerNumberWarning = null;
            }
        }

   
        this.WarningErrorsList = warnings;
        

    }




}
