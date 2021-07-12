import {Component} from '@angular/core';
import {AppTool, FormatTool} from '../../../../../Infrastructure/Tools';
import {ShipmentTool} from '../../../../../Shipment/Tools';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPickUpPM} from '../../../../../Shipment/EntityPMs/ShipmentPickUpPM';
import {ShipmentPickUpDeliveryPackagePM} from '../../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {PackageTypeList} from '../../../../../Common/EntityLists/PackageTypeList';
import {PackageTypeListService} from '../../../../../Common/Services/StandardLists/PackageTypeListService';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import { FeatureToggleList } from '../../../../../Infrastructure/EntityLists/FeatureToggleList';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { ShipmentPackagePM } from '../../../../../Shipment/EntityPMs/ShipmentPackagePM';
import { PackagesTabComponent, ShipmentPackageItem } from '../../../../ShipmentPackages/Components/Packages/PackagesTabComponent';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';

@Component({
    
    templateUrl: './PickupPackagesTabComponent.html',
})

export class PickupPackagesTabComponent {
    public EntityPM: ShipmentPickUpPM;
    public ShipmentPM: ShipmentPM;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public TransportModeId: string;
    public ObjectTableName: string = "ShipmentPickUpDelivery";
    public ItemsSource: PickupPackageItem[] = [];
    public IsAddContainerVisible: boolean = false;
    public IsAddContainerEnabled: boolean = false;
    public DataContext = this;
    constructor() {

    }

    InitTab(myEntityPM: ShipmentPickUpPM, myShipmentPM: ShipmentPM) {
        this.EntityPM = myEntityPM;
        this.ShipmentPM = myShipmentPM;
        this.TransportModeId = this.ShipmentPM.TransportModeId;
        this.IsLCLEntity = AppTool.IsLCLEntity(this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId);
        this.IsFCLEntity = AppTool.IsFCLEntity(this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId);
        this.SetLabels();
        this.SetUIProperties();
        this.BuildItemsSource();
    }

    public VolumeLabel: string;
    public DimensionsLabel: string;
    public GrossWeightLabel: string;
    SetLabels() {
        this.VolumeLabel = TextCodeTranslator.Translate('ShipmentPickUpDeliveryPackage.O.Packages.Volume').replace('%UnitCode', this.ShipmentPM.VolumeUnitCode);
        this.DimensionsLabel = "Dimensions (L-W-H) (" + this.ShipmentPM.DimensionsUnitCode + ")";
        this.GrossWeightLabel = TextCodeTranslator.Translate('ShipmentPickUpDeliveryPackage.O.Packages.GrossWeight').replace('%UnitCode', this.ShipmentPM.GrossWeightUnitCode);
    }

    public IsEditingEnabled: boolean = true;
    SetUIProperties() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.StandaloneShipmentId)) {
            this.IsEditingEnabled = false;
        }

        else {
            this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.ShipmentPM);
        }

        this.IsAddContainerVisible = false;
        if (this.IsFCLEntity) {
            var featureToggle: FeatureToggleList = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "OIC")[0];
            if (featureToggle) {
                this.IsAddContainerVisible = true;
                this.IsEditingEnabled = false;
            }
        }

        this.IsAddContainerEnabled = false;
        if (this.IsAddContainerVisible) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.StandaloneShipmentId)) {
                this.IsAddContainerEnabled = true;
            }
        }
    }

    public SelectedItem: PickupPackageItem = null;
    public PackageTypeColumnWidth: number = 80;
    BuildItemsSource() {
        this.ItemsSource = [];
        this.SelectedItem = null;
        var myPackageTypeColumnWidth: number = 80;

        this.EntityPM.ShipmentPickUpDeliveryPackages.forEach(item => {

            var widthOfLabel = AppTool.GetTextWidth(item.PackageTypeName) + 10;
            if (widthOfLabel > myPackageTypeColumnWidth) {
                myPackageTypeColumnWidth = widthOfLabel;
            }

            this.ItemsSource.push(new PickupPackageItem(item, this));
        });

        if (myPackageTypeColumnWidth > 190) {
            myPackageTypeColumnWidth = 190;
        }

        this.PackageTypeColumnWidth = myPackageTypeColumnWidth;
    }

    CopyfromShipmentPackagesButtonClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Copy from Shipment Packages";
        logWindow.WindowArgs = this;
        logWindow.Show("./ShipmentModules/ShipmentRouting/Components/Routings/PickupTabs/PickupPackagesChooseComponent");
    }

    AddButtonClicked() {
        if (this.IsAddContainerVisible) {
            this.ValidateNumberOfStandAloneShipmentPackages()
        } else {
            this.ViewAddPickupPackagesWindow();
        }
    }

    ValidateNumberOfStandAloneShipmentPackages() {
        var numberOfAllowedPackages = 1;
        if (this.EntityPM.ShipmentPickUpDeliveryPackages.length >= numberOfAllowedPackages && !AppTool.IsNullOrEmpty(this.EntityPM.StandaloneShipmentId)) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("Can't Add Another Container Since Pickup is Connected to a Stand Alone Shipment");
        } else {
            this.ViewAddPickupPackagesWindow();
        }
    }

    ViewAddPickupPackagesWindow() {
        var itemPM = new ShipmentPickUpDeliveryPackagePM(null);
        itemPM.Tenant = this.EntityPM.Tenant;
        itemPM.ShipmentPickUpDeliveryId = this.EntityPM.Id;

        var itemComponent = new PickupPackageItem(itemPM, this, true);

        var logWindow = new LogitudeWindow();
        logWindow.Title = TextCodeTranslator.Translate("ShipmentPickUpDeliveryPackage.O.AddPickUpPackage");
        logWindow.DataContext = itemComponent;
        logWindow.Show("./ShipmentModules/ShipmentRouting/Components/Routings/PickupTabs/PickupPackagesAddEditComponent");
        logWindow.WindowClosed.subscribe((event: any) => {
            this.SetUIProperties();
        });
    }

    EditPackageClicked(itemComponent: PickupPackageItem) {
        if (this.IsAddContainerVisible) {
            this.EditContainerClicked(itemComponent);
        } else {
            var logWindow = new LogitudeWindow();
            logWindow.Title = TextCodeTranslator.Translate("ShipmentPickUpDeliveryPackage.O.EditPickUpPackage");
            logWindow.DataContext = itemComponent;
            logWindow.Show("./ShipmentModules/ShipmentRouting/Components/Routings/PickupTabs/PickupPackagesAddEditComponent");
        }
    }

    DeleteButtonClicked(itemComponent: PickupPackageItem) {
        if (itemComponent) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator.Translate("ShipmentPickUpDeliveryPackage.M.DeleteThisPickUpPackage"));
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.EntityPM.RemovePackage(itemComponent.EntityPM);
                    this.BuildItemsSource();
                    this.SetUIProperties();
                }
            });
        }
    }

    AddContainerClicked() {
        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.ShipmentPM = this.ShipmentPM;
        windowArgs.IsLCLEntity = this.IsLCLEntity;
        windowArgs.IsFCLEntity = this.IsFCLEntity;
        windowArgs.TransportModeId = this.TransportModeId;

        var logWindow = new LogitudeWindow();
        logWindow.Title = "Add Container";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./ShipmentModules/ShipmentRouting/Components/Routings/SelectStandalonePackagesComponent");
        logWindow.WindowClosed.subscribe((s: any) => {
            if (s) {
                this.BuildItemsSource();
            }
        });
    }

    EditContainerClicked(pickupItemComponent: PickupPackageItem) {

        var shipmentPackage = this.MapShipmentPackageFromPickupDeliveryPackage(pickupItemComponent);
        var logWindow = new LogitudeWindow();
        logWindow.Title = this.GetEditPackageWindowTitle();
        var entityArgs: EntityArgs = new EntityArgs();
        entityArgs.EntityPM = this.ShipmentPM;
        entityArgs.ObjectTableName = "Shipment";

        var packagesTabComponent: PackagesTabComponent = new PackagesTabComponent(entityArgs, new EntityResourceService());
        packagesTabComponent.ngOnInit();
        packagesTabComponent.IsEditingEnabled = AppTool.IsNullOrEmpty(this.EntityPM.StandaloneShipmentId) ? true : false;
        var itemComponent = new ShipmentPackageItem(shipmentPackage, packagesTabComponent, false);
        logWindow.Width = 940;
        logWindow.Height = 610;
        logWindow.DataContext = itemComponent;
        logWindow.Show('./ShipmentModules/ShipmentPackages/Components/Packages/AddEditOceanPackageComponent');
        logWindow.ComponentLoaded.subscribe(component => {
            logWindow.WindowClosed.subscribe(result => {
                if (result) {
                    pickupItemComponent.Quantity = component.DataContext.Quantity;
                    pickupItemComponent.Volume = component.DataContext.Volume;
                    pickupItemComponent.Width = component.DataContext.Width;
                    pickupItemComponent.Height = component.DataContext.Height;
                    pickupItemComponent.Weight = component.DataContext.Weight;
                    pickupItemComponent.PackageTypeId = component.DataContext.PackageTypeId;
                    pickupItemComponent.PackageTypeName = component.DataContext.PackageTypeName;
                    pickupItemComponent.ContainerNumber = component.DataContext.ContainerNumber;
                    pickupItemComponent.Description = component.DataContext.Description;
                    pickupItemComponent.ShipperSeal = component.DataContext.ShipperSeal;
                    this.BuildItemsSource();
                    packagesTabComponent.BuildItemsSource();
                }
            });
        });
    }

    MapShipmentPackageFromPickupDeliveryPackage(pickupItemComponent: PickupPackageItem) {
        var shipmentPackage = new ShipmentPackagePM(null);
        shipmentPackage.NonActiveContainer = false;
        shipmentPackage.Quantity = pickupItemComponent.Quantity;
        shipmentPackage.IsContainer = true;
        shipmentPackage.Tenant = SessionLocator.Tenant;
        shipmentPackage.TemperatureUnitCode = SessionLocator.TenantPM.TemperatureUnitCode;
        shipmentPackage.FlashPointTemperatureUnitCode = SessionLocator.TenantPM.TemperatureUnitCode;
        shipmentPackage.IsPackageCheckedInLeg = true;
        shipmentPackage.Volume = pickupItemComponent.Volume;
        shipmentPackage.Width = pickupItemComponent.Width;
        shipmentPackage.Height = pickupItemComponent.Height;
        shipmentPackage.Weight = pickupItemComponent.Weight;
        shipmentPackage.PackageTypeId = pickupItemComponent.PackageTypeId;
        shipmentPackage.PackageTypeName = pickupItemComponent.PackageTypeName;
        shipmentPackage.ContainerNumber = pickupItemComponent.ContainerNumber;
        shipmentPackage.Description = pickupItemComponent.Description;
        shipmentPackage.ShipperSeal = pickupItemComponent.ShipperSeal;
        return shipmentPackage;
    }

    GetEditPackageWindowTitle() {
        var logWindowTitle = "";
        if (this.IsLCLEntity) {
            logWindowTitle = TextCodeTranslator.Translate("ShipmentPackage.O.EditPackage");
        }

        else {
            logWindowTitle = TextCodeTranslator.Translate("ShipmentPackage.O.EditContainer");
            if (this.ShipmentPM.TransportModeId == "I")
                logWindowTitle = TextCodeTranslator.Translate("ShipmentPackage.O.EditFullTruckLoad");
        }
        return logWindowTitle;
    }

}
export class PickupPackageItem extends BaseComponent {
    public EntityPM: ShipmentPickUpDeliveryPackagePM;
    public ObjectTableName: string = "ShipmentPickUpDeliveryPackage";
    public DataContext = this;
    public IsNewEntity: boolean = false;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public IsAirShipment: boolean = false;
    public IsVehicleDetails: boolean = false;

    constructor(item: ShipmentPickUpDeliveryPackagePM, public fatherComponent: PickupPackagesTabComponent, isNewEntity: boolean = false) {
        super();
        this.EntityPM = item;
        this.IsNewEntity = isNewEntity;
        this.IsLCLEntity = fatherComponent.IsLCLEntity;
        this.IsFCLEntity = fatherComponent.IsFCLEntity;
        this.IsAirShipment = fatherComponent.TransportModeId == "A" ? true : false;
        this.SetUIProperties();

        if (this.IsNewEntity) {
            this.SetUIPropertiesOfCars(false);
            this.IsVehicleDetails = false;
        }
    }

    public IsContainer: boolean = false;
    public IsEditingEnabled: boolean = true;
    public IsAddContainerEnabled: boolean = false;

    SetUIProperties() {
        this.IsAddContainerEnabled = this.fatherComponent.IsAddContainerEnabled;
        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;
        this.SetUIProperties_IsContainer();
        this.SetUIProperties_Harmonize();

        if (!AppTool.IsNullOrEmpty(this.PackageTypeId)) {
            var myService: PackageTypeListService = new PackageTypeListService();
            myService.getSingle(this.PackageTypeId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: PackageTypeList = myResponse.Result;
                    if (list != null) {
                        this.IsContainer = list.IsContainer;
                        this.SetUIProperties_IsContainer();
                        this.SetUIPropertiesOfCars(this.IsEditingEnabled && list.IsVehicle);
                        this.IsVehicleDetails = list.IsVehicle;
                    }
                }
            });
        }
        this.UIProperties.SetEnabled("PackageTypeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ContainerNumber", this.ObjectTableName, this.IsEditingEnabled);        
        this.UIProperties.SetEnabled("Weight", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ShipperSeal", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Harmonize", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Description", this.ObjectTableName, this.IsEditingEnabled);

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

    SetUIProperties_IsContainer() {
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;

        if (this.IsEditingEnabled) {
            if (this.Quantity > 0) {
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

        this.UIProperties.SetVisibility("Length", this.ObjectTableName, !this.IsContainer);
        this.UIProperties.SetVisibility("Width", this.ObjectTableName, !this.IsContainer);
        this.UIProperties.SetVisibility("Height", this.ObjectTableName, !this.IsContainer);

        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, !this.IsContainer && this.IsEditingEnabled);
        this.UIProperties.SetVisibility("ShipperSeal", this.ObjectTableName, this.IsContainer);
        //this.UIProperties.SetVisibility("ContainerNumber", this.ObjectTableName, this.IsContainer);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);   

        if (this.IsContainer) {
            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, this.IsEditingEnabled);
        }

        else {
            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        }
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

    get PackageTypeId() { return this.EntityPM.PackageTypeId; }
    set PackageTypeId(value: string) {
        if (this.EntityPM.PackageTypeId != value) {
            this.EntityPM.PackageTypeId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.PackageTypeName = null;
                this.PackageTypeTEU = null;
                this.ContainerNumber = null;
                this.Quantity = null;
                this.ShipperSeal = null;
                this.IsContainer = false;
                this.SetUIProperties_IsContainer();
                this.SetUIPropertiesOfCars(false);
                this.IsVehicleDetails = false;
            }

            else {
                var myService: PackageTypeListService = new PackageTypeListService();
                myService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PackageTypeList = myResponse.Result;
                        if (list != null) {
                            this.PackageTypeName = list.EnglishName;
                            this.PackageTypeTEU = list.TEU;
                            this.IsContainer = list.IsContainer;

                            if (this.IsContainer) {
                                this.Quantity = 1;
                            }

                            else {
                                this.ContainerNumber = null;
                                this.Quantity = null;
                                this.ShipperSeal = null;
                            }

                            this.SetUIProperties_IsContainer();
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

    get PackageTypeName() { return this.EntityPM.PackageTypeName; }
    set PackageTypeName(value: string) {
        if (this.EntityPM.PackageTypeName != value) {
            this.EntityPM.PackageTypeName = value;
        }
    }

    get PackageTypeTEU() { return this.EntityPM.PackageTypeTEU; }
    set PackageTypeTEU(value: number) {
        if (this.EntityPM.PackageTypeTEU != value) {
            this.EntityPM.PackageTypeTEU = value;
        }
    }

    get ContainerNumber() { return this.EntityPM.ContainerNumber; }
    set ContainerNumber(value: string) {
        if (this.EntityPM.ContainerNumber != value) {
            this.EntityPM.ContainerNumber = value;
            this.ValidateContainerNumber(value);
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
        }
    }
    get IsMultiHarmonize() { return this.EntityPM.IsMultiHarmonize }
    set IsMultiHarmonize(newValue: boolean) {
        if (this.EntityPM.IsMultiHarmonize != newValue) {
            this.EntityPM.IsMultiHarmonize = newValue;
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

    get ShipperSeal() { return this.EntityPM.ShipperSeal; }
    set ShipperSeal(value: string) {
        if (this.EntityPM.ShipperSeal != value) {
            this.EntityPM.ShipperSeal = value;
        }
    }

    get Harmonize() { return this.EntityPM.Harmonize; }
    set Harmonize(value: string) {
        if (this.EntityPM.Harmonize != value) {
            this.EntityPM.Harmonize = value;
        }
    }

    get Quantity() { return this.EntityPM.Quantity; }
    set Quantity(value: number) {
        if (this.EntityPM.Quantity != value) {
            this.EntityPM.Quantity = value;

            this.ComputeVolume();
            this.SetUIProperties();
        }
    }

    get Volume() { return this.EntityPM.Volume; }
    set Volume(value: number) {
        if (this.EntityPM.Volume != value) {
            this.EntityPM.Volume = AppTool.Round(value, 3);

            this.SetUIProperties();
        }
    }

    get Weight() { return this.EntityPM.Weight; }
    set Weight(value: number) {
        if (this.EntityPM.Weight != value) {
            this.EntityPM.Weight = AppTool.Round(value, 3);
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

    get Description() { return this.EntityPM.Description; }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }

    private ComputeVolume() {
        if (this.fatherComponent.ShipmentPM.Ratio == null) {
            this.fatherComponent.ShipmentPM.Ratio = AppTool.GetRatio(this.fatherComponent.ShipmentPM.DirectionId, this.fatherComponent.ShipmentPM.TransportModeId, this.fatherComponent.ShipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        this.Volume = AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.Weight, this.fatherComponent.ShipmentPM.Ratio, this.fatherComponent.ShipmentPM.DimensionsUnitCode, this.fatherComponent.ShipmentPM.VolumeUnitCode, this.fatherComponent.ShipmentPM.GrossWeightUnitCode);
    }
}
