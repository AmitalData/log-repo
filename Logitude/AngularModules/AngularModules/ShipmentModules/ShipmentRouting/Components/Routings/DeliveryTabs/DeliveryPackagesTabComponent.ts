import { Component } from '@angular/core';
import { AppTool, FormatTool } from '../../../../../Infrastructure/Tools';
import { ShipmentTool } from '../../../../../Shipment/Tools';
import { ShipmentPM } from '../../../../../Shipment/EntityPMs/ShipmentPM';
import { ShipmentDeliveryPM } from '../../../../../Shipment/EntityPMs/ShipmentDeliveryPM';
import { ShipmentPickUpDeliveryPackagePM } from '../../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { PackageTypeList } from '../../../../../Common/EntityLists/PackageTypeList';
import { PackageTypeListService } from '../../../../../Common/Services/StandardLists/PackageTypeListService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { AddEditDeliveryComponent } from '../AddEditDeliveryComponent';
import { FeatureToggleList } from '../../../../../Infrastructure/EntityLists/FeatureToggleList';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { ShipmentPackagePM } from '../../../../../Shipment/EntityPMs/ShipmentPackagePM';
import { PackagesTabComponent, ShipmentPackageItem } from '../../../../ShipmentPackages/Components/Packages/PackagesTabComponent';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';

@Component({
    templateUrl: './DeliveryPackagesTabComponent.html',
})

export class DeliveryPackagesTabComponent {
    public EntityPM: ShipmentDeliveryPM;
    public ShipmentPM: ShipmentPM;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public IsContainersFUVisible: boolean = false;
    public IsShowCopyFromReleasesPackages: boolean = false;
    public DirectionId: string;
    public TransportModeId: string;
    public ObjectTableName: string = "ShipmentPickUpDelivery";
    public ItemsSource: DeliveryPackageItem[] = [];
    public DataContext = this;
    public TypeCode: string = null;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsAddContainerVisible: boolean = false;
    public IsAddContainerEnabled: boolean = false;
    constructor() {

    }

    public IsConnectedToContainer: boolean = false;
    private FatherComponent: AddEditDeliveryComponent;
    InitTab(myEntityPM: ShipmentDeliveryPM, myShipmentPM: ShipmentPM, father: AddEditDeliveryComponent) {
        this.EntityPM = myEntityPM;
        this.ShipmentPM = myShipmentPM;
        this.FatherComponent = father;
        this.DirectionId = this.ShipmentPM.DirectionId;
        this.TransportModeId = this.ShipmentPM.TransportModeId;
        this.TypeCode = this.EntityPM.PickUpDeliveryTypeCode;
        this.IsLCLEntity = AppTool.IsLCLEntity(this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId);
        this.IsFCLEntity = AppTool.IsFCLEntity(this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId);

        if (FeatureLocator.HasFeaturePermession("Shipment", "Area.ContainersFU")) {
            this.IsContainersFUVisible = true;
        }

        if (this.ShipmentPM) {
            if (this.ShipmentPM.ShipmentLevelCode == "D" || this.ShipmentPM.ShipmentLevelCode == "C") {
                if (FeatureLocator.HasFeaturePermession("WarehouseRelease", "Module")) {
                    this.IsShowCopyFromReleasesPackages = this.ShipmentPM.DirectionId == "I" ? true : false;
                }
            }
        }

        if (this.FatherComponent.IsCreatingContainerDelivery) {
            this.IsConnectedToContainer = true;
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {

        }

        else {


            if (this.ShipmentPM.ShipmentPackages.filter(f => f.DeliveryId == this.EntityPM.Id).length > 0) {
                this.IsConnectedToContainer = true;
            }

            //if (this.ShipmentPM.ShipmentPackages.filter(f => f.EmptyContainerReturnId == this.EntityPM.Id).length > 0) {
            //    this.IsConnectedToContainer = true;
            //}
        }

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

        if (this.IsShipmentStatuesDelivered()) {
            this.IsEditingEnabled = false;
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

        if (this.IsEditingEnabled) {
            if (this.IsConnectedToContainer) {
                this.IsEditingEnabled = false;
            }
        }
    }

    private IsShipmentStatuesDelivered() {
        var deliverdStausName = "Delivered";
        var IsDeliveryOptionsEnabled = SessionLocator.TenantPM != null ? SessionLocator.TenantPM.EnableDeliveryOptions : false;

        if (!IsDeliveryOptionsEnabled) {
            return false;
        }

        if (this.ShipmentPM.StatusName == (deliverdStausName)) {
            return true;
        }

        return false;
    }

    public SelectedItem: DeliveryPackageItem = null;
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

            this.ItemsSource.push(new DeliveryPackageItem(item, this));
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
        logWindow.Show("./ShipmentModules/ShipmentRouting/Components/Routings/DeliveryTabs/DeliveryPackagesChooseComponent");
    }

    AddButtonClicked() {
        if (this.IsAddContainerVisible) {
            this.ValidateNumberOfStandAloneShipmentPackages()
        } else {
            this.ViewAddDeliveryPackagesWindow();
        }
    }

    ValidateNumberOfStandAloneShipmentPackages() {
        var numberOfAllowedPackages = 1;
        if (this.EntityPM.ShipmentPickUpDeliveryPackages.length >= numberOfAllowedPackages && !AppTool.IsNullOrEmpty(this.EntityPM.StandaloneShipmentId)) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("Can't Add Another Container Since Delivery is Connected to a Stand Alone Shipment");
        } else {
            this.ViewAddDeliveryPackagesWindow();
        }
    }

    ViewAddDeliveryPackagesWindow() {
        var itemPM = new ShipmentPickUpDeliveryPackagePM(null);
        itemPM.Tenant = this.EntityPM.Tenant;
        itemPM.ShipmentPickUpDeliveryId = this.EntityPM.Id;

        var itemComponent = new DeliveryPackageItem(itemPM, this, true);

        var logWindow = new LogitudeWindow();
        logWindow.Title = TextCodeTranslator.Translate("ShipmentPickUpDeliveryPackage.O.AddDeliveryPackage");
        logWindow.DataContext = itemComponent;
        logWindow.Show("./ShipmentModules/ShipmentRouting/Components/Routings/DeliveryTabs/DeliveryPackagesAddEditComponent");
        logWindow.WindowClosed.subscribe((event: any) => {
            this.SetUIProperties();
        });
    }

    EditPackageClicked(itemComponent: DeliveryPackageItem) {
        if (this.IsAddContainerVisible) {
            this.EditContainerClicked(itemComponent);
        } else {
            var logWindow = new LogitudeWindow();
            logWindow.Title = TextCodeTranslator.Translate("ShipmentPickUpDeliveryPackage.O.EditDeliveryPackage");
            logWindow.DataContext = itemComponent;
            logWindow.Show("./ShipmentModules/ShipmentRouting/Components/Routings/DeliveryTabs/DeliveryPackagesAddEditComponent");
        }
    }

    DeleteButtonClicked(itemComponent: DeliveryPackageItem) {
        if (itemComponent) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator.Translate("ShipmentPickUpDeliveryPackage.M.DeleteThisDeliveryPackage"));
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.EntityPM.RemovePackage(itemComponent.EntityPM);
                    //this.RemoveConnectedShipmentPackage(itemComponent.EntityPM);
                    this.BuildItemsSource();
                    this.SetUIProperties();
                }
            });
        }
    }

    //RemoveConnectedShipmentPackage(pickUpDeliveryPackagePM: ShipmentPickUpDeliveryPackagePM) {
    //     var shipmentPackage = this.ShipmentPM?.ShipmentPackages?.find(p =>
    //         (p.ContainerNumber == pickUpDeliveryPackagePM.ContainerNumber) && !AppTool.IsNullOrEmpty(pickUpDeliveryPackagePM.ContainerNumber)
    //         && AppTool.IsNullOrEmpty(pickUpDeliveryPackagePM.ContainerEntityId))
    //     if (shipmentPackage != null) {
    //        this.ShipmentPM.RemovePackage(shipmentPackage);
    //     }
    //} 

    CopyFromReleasesPackages() {
        var windowArgs: any = {};
        windowArgs.ShipmentDeliveryPM = this.EntityPM;
        windowArgs.ShipmentPM = this.ShipmentPM;
        windowArgs.FatherComponent = this;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 800;
        logWindow.Height = 600;
        logWindow.Title = "Warehouse Releases";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Warehouse/Components/CopyFromReleasesPackagesComponent");
    }

    private SaveCompletedEvent: any = null;
    ConnectPackagesButtonClicked() {
        if (this.FatherComponent.IsNewEntity) {
            if (this.CurrentSession.CurrentEditComponent) {
                var isValid = this.FatherComponent.Validate();
                if (isValid) {

                    this.FatherComponent.SavedEntityId = this.EntityPM.Id;
                    this.FatherComponent.SavedEntityNumber = this.EntityPM.PickUpDeliveryNumber;

                    if (this.FatherComponent.IsNewEntity) {
                        this.ShipmentPM.AddDelivery(this.EntityPM);
                        this.FatherComponent.isEntityAdded = true;
                    }

                    if (!this.SaveCompletedEvent) {
                        this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                            this.FatherComponent.OnSaveCompleted(isSaveSuccess, false);

                            AppTool.KillEventEmitter(this.SaveCompletedEvent);
                            this.SaveCompletedEvent = null;

                            this.ShowConnectWindow();
                        });

                        this.CurrentSession.CurrentEditComponent.SaveChanges();
                    }
                }
            }
        }

        else {
            this.ShowConnectWindow();
        }
    }
    ShowConnectWindow() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Connect Packages";
        logWindow.WindowArgs = this;
        logWindow.Show("./ShipmentModules/ShipmentRouting/Components/Routings/DeliveryTabs/DeliveryPackagesConnectComponent");
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

    EditContainerClicked(deliveryItemComponent: DeliveryPackageItem) {

        var shipmentPackage = this.MapShipmentPackageFromPickupDeliveryPackage(deliveryItemComponent);
        var logWindow = new LogitudeWindow();
        logWindow.Title = this.GetEditPackageWindowTitle();
        var entityArgs: EntityArgs = new EntityArgs();
        entityArgs.EntityPM = this.ShipmentPM;
        entityArgs.ObjectTableName = "Shipment";
        //entityArgs.IsFromStandAloneScreen  = true;
        var packagesTabComponent: PackagesTabComponent = new PackagesTabComponent(entityArgs, new EntityResourceService());
        packagesTabComponent.ngOnInit();
        packagesTabComponent.IsEditingEnabled = AppTool.IsNullOrEmpty(this.EntityPM.StandaloneShipmentId) ? this.IsEditingEnabled : false;
        var itemComponent = new ShipmentPackageItem(shipmentPackage, packagesTabComponent, false);
        logWindow.Width = 940;
        logWindow.Height = 610;
        logWindow.DataContext = itemComponent;
        logWindow.Show('./ShipmentModules/ShipmentPackages/Components/Packages/AddEditOceanPackageComponent');
        logWindow.ComponentLoaded.subscribe(component => {
            logWindow.WindowClosed.subscribe(result => {
                if (result) {
                    deliveryItemComponent.Quantity = component.DataContext.Quantity;
                    deliveryItemComponent.Volume = component.DataContext.Volume;
                    deliveryItemComponent.Width = component.DataContext.Width;
                    deliveryItemComponent.Height = component.DataContext.Height;
                    deliveryItemComponent.Weight = component.DataContext.Weight;
                    deliveryItemComponent.PackageTypeId = component.DataContext.PackageTypeId;
                    deliveryItemComponent.PackageTypeName = component.DataContext.PackageTypeName;
                    deliveryItemComponent.ContainerNumber = component.DataContext.ContainerNumber;
                    deliveryItemComponent.Description = component.DataContext.Description;
                    deliveryItemComponent.ShipperSeal = component.DataContext.ShipperSeal;
                    this.BuildItemsSource();
                    packagesTabComponent.BuildItemsSource();
                }
            });
        });
    }

    MapShipmentPackageFromPickupDeliveryPackage(deliveryItemComponent: DeliveryPackageItem) {
        var shipmentPackage = new ShipmentPackagePM(null);
        shipmentPackage.NonActiveContainer = false;
        shipmentPackage.Quantity = deliveryItemComponent.Quantity;
        shipmentPackage.IsContainer = true;
        shipmentPackage.Tenant = SessionLocator.Tenant;
        shipmentPackage.TemperatureUnitCode = SessionLocator.TenantPM.TemperatureUnitCode;
        shipmentPackage.FlashPointTemperatureUnitCode = SessionLocator.TenantPM.TemperatureUnitCode;
        shipmentPackage.IsPackageCheckedInLeg = true;
        shipmentPackage.Volume = deliveryItemComponent.Volume;
        shipmentPackage.Width = deliveryItemComponent.Width;
        shipmentPackage.Height = deliveryItemComponent.Height;
        shipmentPackage.Weight = deliveryItemComponent.Weight;
        shipmentPackage.PackageTypeId = deliveryItemComponent.PackageTypeId;
        shipmentPackage.PackageTypeName = deliveryItemComponent.PackageTypeName;
        shipmentPackage.ContainerNumber = deliveryItemComponent.ContainerNumber;
        shipmentPackage.Description = deliveryItemComponent.Description;
        shipmentPackage.ShipperSeal = deliveryItemComponent.ShipperSeal;
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
export class DeliveryPackageItem extends BaseComponent {
    public EntityPM: ShipmentPickUpDeliveryPackagePM;
    public ObjectTableName: string = "ShipmentPickUpDeliveryPackage";
    public DataContext = this;
    public IsNewEntity: boolean = false;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public IsAirShipment: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsVehicleDetails: boolean = false;

    constructor(item: ShipmentPickUpDeliveryPackagePM, public fatherComponent: DeliveryPackagesTabComponent, isNewEntity: boolean = false) {
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
        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;
        this.IsAddContainerEnabled = this.fatherComponent.IsAddContainerEnabled;
        if (this.IsEditingEnabled) {
            if (this.EntityPM.OriginalShipmentPackageId) {
                this.IsEditingEnabled = false;
            }
        }

        this.UIProperties.SetVisibility("Length", this.ObjectTableName, this.IsLCLEntity);
        this.UIProperties.SetVisibility("Width", this.ObjectTableName, this.IsLCLEntity);
        this.UIProperties.SetVisibility("Height", this.ObjectTableName, this.IsLCLEntity);
        this.SetUIProperties_IsContainer();
        this.SetUIProperties_Harmonize();

        if (!AppTool.IsNullOrEmpty(this.PackageTypeId)) {
            var myService: PackageTypeListService = new PackageTypeListService();
            myService.getSingleFromCache(this.PackageTypeId).subscribe((myResponse: ServiceResponse) => {
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
        this.UIProperties.SetEnabled("Description", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Harmonize", this.ObjectTableName, this.IsEditingEnabled);
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
        var isFieldEnabled: boolean = false;
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
                myService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
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

    get Description() { return this.EntityPM.Description; }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
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
    private ComputeVolume() {
        if (this.fatherComponent.ShipmentPM.Ratio == null) {
            this.fatherComponent.ShipmentPM.Ratio = AppTool.GetRatio(this.fatherComponent.ShipmentPM.DirectionId, this.fatherComponent.ShipmentPM.TransportModeId, this.fatherComponent.ShipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        this.Volume = AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.Weight, this.fatherComponent.ShipmentPM.Ratio, this.fatherComponent.ShipmentPM.DimensionsUnitCode, this.fatherComponent.ShipmentPM.VolumeUnitCode, this.fatherComponent.ShipmentPM.GrossWeightUnitCode);
    }
}
