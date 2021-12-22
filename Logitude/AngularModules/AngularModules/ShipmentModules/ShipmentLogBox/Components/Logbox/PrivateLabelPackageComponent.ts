import { Component } from '@angular/core';
import { PackageTypeList } from '../../../../Common/EntityLists/PackageTypeList';
import { PackageTypeListService } from '../../../../Common/Services/StandardLists/PackageTypeListService';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator'; 
import { ShipmentOrderPackagePM } from '../../../../Shipment/EntityPMs/ShipmentOrderPackagePM';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';

@Component({

    templateUrl: './PrivateLabelPackageComponent.html',
})

export class PrivateLabelPackageComponent {
    public EntityPM: ShipmentPM;
    public DataContext = this;
    public ObjectTableName: string = "Shipment";
    public ItemsSource: WizardDimensionItem[] = [];
    public ValidationErrorsList: string[] = [];
    public IsResourcesReady: boolean = false;
    public IsPackageTypeVisible: boolean = false;
    public IsRquiredDimensions: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {

    }

    SetWindowArgs(entityPM: ShipmentPM) {
        this.EntityPM = entityPM;
        this.IsPackageTypeVisible = this.EntityPM.TransportModeId == "A" ? false : true;

        this.entityResourceService.getEntityResourceByTableName("ShipmentOrderPackage").subscribe((response: any) => {
            this.IsResourcesReady = true;
            this.SaveData();
            this.SetLabels();
            this.BuildData();
        });
    }

    private savedPackages: ShipmentOrderPackagePM[] = [];
    private savedVolume: number = null;
    private savedGrossWeight: number = null;
    private savedChargeableWeight: number = null;
    private savedVolumetricWeight: number = null;
    private savedNumberOfPackages: number = null;
    SaveData() {

        this.EntityPM.ShipmentOrderPackages.forEach(item => {
            var newItem = new ShipmentOrderPackagePM(null);
            newItem.Id = item.Id;
            newItem.GrossWeight = item.GrossWeight;
            newItem.Height = item.Height;
            newItem.Length = item.Length;
            newItem.PackageTypeId = item.PackageTypeId;
            newItem.PackageTypeName = item.PackageTypeName;
            newItem.Quantity = item.Quantity;
            newItem.ShipmentId = item.ShipmentId;
            newItem.Tenant = item.Tenant;
            newItem.Volume = item.Volume;
            newItem.VolumetricWeight = item.VolumetricWeight;
            newItem.Width = item.Width;
            newItem.ContainerTypeId = item.ContainerTypeId;
            newItem.IsContainer = item.IsContainer;

            this.savedPackages.push(newItem);
        });

        this.savedVolume = this.EntityPM.BookingVolume;
        this.savedGrossWeight = this.EntityPM.OrderGrossWeight;
        this.savedChargeableWeight = this.EntityPM.OrderChargeableWeight;
        this.savedVolumetricWeight = this.EntityPM.OrderVolumetricWeight;
        this.savedNumberOfPackages = this.EntityPM.BookingNumberOfPackages;
    }

    public VolumeLabel: string = null;
    public GrossWeightLabel: string = null;
    public ChargeableWeightLabel: string = null;
    public VolumetricWeightLabel: string = null;
    public DimensionsColumnHeader: string = null;
    public VolumeColumnHeader: string = null;
    public VolumetricWeightColumnHeader: string = null;
    public WeightColumnHeader: string = null;
    SetLabels() {
        this.VolumeLabel = TextCodeTranslator.Translate("Shipment.F.BookingVolume.Short").replace("%VolumeCode", this.EntityPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate("Shipment.F.OrderGrossWeight.Short").replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);
        this.ChargeableWeightLabel = TextCodeTranslator.Translate("Shipment.F.OrderChargeableWeight.Short").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator.Translate("Shipment.F.OrderVolumetricWeight.Short").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);

        this.VolumeColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.Volume").replace("%UnitCode", this.EntityPM.VolumeUnitCode);
        this.WeightColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.GrossWeight").replace("%UnitCode", this.EntityPM.GrossWeightUnitCode);
        this.DimensionsColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.Dimensions").replace("%UnitCode", this.EntityPM.DimensionsUnitCode);
        this.VolumetricWeightColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.VolWeight").replace("%UnitCode", this.EntityPM.ChargeableWeightUnitCode);
    }

    BuildData() {
        this.ItemsSource = [];

        var list: ShipmentOrderPackagePM[] = [];
        this.EntityPM.ShipmentOrderPackages.forEach((item) => {
            list.push(item);
        });

        if (list.length < 5) {
            for (var i = list.length; i < 5; i++) {
                var item: ShipmentOrderPackagePM = new ShipmentOrderPackagePM(null);
                item.Tenant = this.EntityPM.Tenant;
                item.ShipmentId = this.EntityPM.Id;
                list.push(item);
            }
        }

        list.forEach(item => {
            this.ItemsSource.push(new WizardDimensionItem(item, this));
        });
    }

    get BookingVolume() { return AppTool.IsNullOrEmpty(this.EntityPM.BookingVolume) ? 0 : this.EntityPM.BookingVolume; }
    set BookingVolume(newValue: number) {
        if (this.EntityPM.BookingVolume != newValue) {
            this.EntityPM.BookingVolume = AppTool.Round(newValue, 3);
        }
    }

    get OrderGrossWeight() { return AppTool.IsNullOrEmpty(this.EntityPM.OrderGrossWeight) ? 0 : this.EntityPM.OrderGrossWeight; }
    set OrderGrossWeight(newValue: number) {
        if (this.EntityPM.OrderGrossWeight != newValue) {
            this.EntityPM.OrderGrossWeight = AppTool.Round(newValue, 3);
        }
    }

    get OrderChargeableWeight() { return AppTool.IsNullOrEmpty(this.EntityPM.OrderChargeableWeight) ? 0 : this.EntityPM.OrderChargeableWeight; }
    set OrderChargeableWeight(newValue: number) {
        if (this.EntityPM.OrderChargeableWeight != newValue) {
            this.EntityPM.OrderChargeableWeight = AppTool.Round(newValue, 3);
        }
    }

    get OrderVolumetricWeight() { return AppTool.IsNullOrEmpty(this.EntityPM.OrderVolumetricWeight) ? 0 : this.EntityPM.OrderVolumetricWeight; }
    set OrderVolumetricWeight(newValue: number) {
        if (this.EntityPM.OrderVolumetricWeight != newValue) {
            this.EntityPM.OrderVolumetricWeight = AppTool.Round(newValue, 3);
        }
    }

    get BookingNumberOfPackages() { return AppTool.IsNullOrEmpty(this.EntityPM.BookingNumberOfPackages) ? 0 : this.EntityPM.BookingNumberOfPackages; }
    set BookingNumberOfPackages(newValue: number) {
        if (this.EntityPM.BookingNumberOfPackages != newValue) {
            this.EntityPM.BookingNumberOfPackages = newValue;
        }
    }

    ComputeTotals() {
        if (this.EntityPM.ShipmentOrderPackages.length == 0) {
            this.BookingVolume = null;
            this.OrderGrossWeight = null;
            this.OrderChargeableWeight = null;
            this.OrderVolumetricWeight = null;
            this.BookingNumberOfPackages = null;
        }

        else {

            var myQuantity: number = 0;
            var myVolume: number = 0;
            var myGrossWeight: number = 0;
            var myVolumetricWeight: number = 0;

            this.EntityPM.ShipmentOrderPackages.forEach((item) => {

                if (!AppTool.IsNullOrEmpty(item.Quantity)) {
                    myQuantity += item.Quantity;
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
        }

        this.BookingNumberOfPackages = myQuantity;
        this.BookingVolume = myVolume;
        this.OrderVolumetricWeight = myVolumetricWeight;
        this.OrderGrossWeight = AppTool.Round(myGrossWeight, 3);
        this.OrderChargeableWeight = AppTool.CalculateChargeableWeight(this.OrderGrossWeight, this.OrderVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
    }

    AddPackageClicked() {
        var itemPM = new ShipmentOrderPackagePM(null);
        itemPM.Tenant = SessionLocator.Tenant;
        itemPM.ShipmentId = this.EntityPM.Id;

        var logeWindow = new LogitudeWindow();
        logeWindow.Title = "Add line";
        logeWindow.WindowArgs = new WizardDimensionItem(itemPM, this);
        logeWindow.Show("./Shipment/Components/NewEntity/WizardAddEditDimensionsComponent");
        logeWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.ComputeTotals();
            }
        });
    }
    DeletePackage(item: WizardDimensionItem) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this package?");

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                var itemIndex = this.ItemsSource.indexOf(item);
                if (itemIndex > -1) {
                    this.ItemsSource.splice(itemIndex, 1);
                }

                this.EntityPM.RemoveOrderPackage(item.EntityPM);

                this.ComputeTotals();
            }
        });
    }

    CancelButtonClicked() {

        this.EntityPM.ShipmentOrderPackages = [];

        this.savedPackages.forEach(item => {
            this.EntityPM.AddOrderPackage(item);
        });

        this.EntityPM.BookingVolume = this.savedVolume;
        this.EntityPM.OrderGrossWeight = this.savedGrossWeight;
        this.EntityPM.OrderChargeableWeight = this.savedChargeableWeight;
        this.EntityPM.OrderVolumetricWeight = this.savedVolumetricWeight;
        this.EntityPM.BookingNumberOfPackages = this.savedNumberOfPackages;

        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.ValidateRequiredFields();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    }

    ValidateRequiredFields() {

        this.ValidationErrorsList = [];
        this.IsRquiredDimensions = false;
        this.ValidateShipmentOrderPackages();

    }

    private ValidateShipmentOrderPackages() {
        this.EntityPM.ShipmentOrderPackages.forEach(item => {
            this.ValidateDimensionsFields(item);
        });
    }

    private ValidateDimensionsFields(item: ShipmentOrderPackagePM) {
        if (AppTool.IsNullOrZero(item.Quantity)) return;
        if (this.HasDimensionsValues(item)) return;
        this.ValidatePackageType(item);
        this.ValidationErrorsList.push("Dimensions (L-W-H) fields are required.");
        this.IsRquiredDimensions = true; 
    }

    private ValidatePackageType(item: ShipmentOrderPackagePM) {
        if (AppTool.IsNullOrEmpty(item.PackageTypeId)) {
            this.ValidationErrorsList.push("Package Type field is required.");
        }
    }

    private HasDimensionsValues(item: ShipmentOrderPackagePM) {
        return !AppTool.IsNullOrZero(item.Height) && !AppTool.IsNullOrZero(item.Width) && !AppTool.IsNullOrZero(item.Length);
    }
}
export class WizardDimensionItem extends BaseComponent {
    public EntityPM: ShipmentOrderPackagePM;
    public ShipmentPM: ShipmentPM;
    public DataContext = this;
    public ObjectTableName: string = "ShipmentOrderPackage";
    public IsWindowMode: boolean = false;
    public IsPackageTypeVisible: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(item: ShipmentOrderPackagePM, public fatherComponent: PrivateLabelPackageComponent) {
        super();
        this.EntityPM = item;
        this.ShipmentPM = fatherComponent.EntityPM;
        this.IsPackageTypeVisible = fatherComponent.IsPackageTypeVisible;
        this.SetUIProperties();
    }

    // SetUIProperties
    public IsEditingEnabled: boolean = false;
    SetUIProperties() {
        var isFieldEnabled = false;
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;

        this.IsEditingEnabled = true;

        if (this.IsEditingEnabled) {
            if (this.Quantity > 0 || this.hasValue) {
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

        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("PackageTypeId", this.ObjectTableName, isFieldEnabled);

        if (this.IsPackageTypeVisible) {
            this.UIProperties.SetRequired("PackageTypeId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.PackageTypeId) ? true : false);
            this.UIProperties.SetRequired("GrossWeight", this.ObjectTableName, AppTool.IsNullOrZero(this.GrossWeight) ? true : false);
            }
        }

    private hasValue: boolean;
    public HasValue(hasValue: boolean) {
        this.hasValue = hasValue;
        this.SetUIProperties();
    }

    // Properties
    get PackageTypeId() { return this.EntityPM.PackageTypeId; }
    set PackageTypeId(value: string) {
        if (this.EntityPM.PackageTypeId != value) {
            this.EntityPM.PackageTypeId = value;

            this.SetUIProperties();

            if (AppTool.IsNullOrEmpty(value)) {
                this.PackageTypeName = null;
            }

            else {
                var myService: PackageTypeListService = new PackageTypeListService();
                myService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
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

            var itemIndex = this.ShipmentPM.ShipmentOrderPackages.indexOf(this.EntityPM);

            if (AppTool.IsNullOrZero(this.EntityPM.Quantity)) {
                this.Height = null;
                this.Length = null;
                this.Width = null;
                this.Volume = null;
                this.GrossWeight = null;
                this.VolumetricWeight = null;

                if (!this.IsWindowMode) {
                    if (itemIndex > -1) {
                        this.ShipmentPM.RemoveOrderPackage(this.EntityPM);
                    }
                }
            }

            else {
                if (!this.IsWindowMode) {
                    if (itemIndex == -1) {
                        this.ShipmentPM.AddOrderPackage(this.EntityPM);
                    }
                }
            }

            this.SetUIProperties();
            this.ComputeVolume();
            this.fatherComponent.ComputeTotals();
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

    get GrossWeight() { return this.EntityPM.GrossWeight; }
    set GrossWeight(newValue: number) {
        var myValue: number = AppTool.Round(newValue, 3);

        if (this.EntityPM.GrossWeight != myValue) {
            this.EntityPM.GrossWeight = myValue;

            this.SetUIProperties();
            this.fatherComponent.ComputeTotals();
        }
    }

    OnGrossWeightLostFocus(input1: number) {
        if (AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {
            if (this.Width == null || this.Height == null || this.Length == null) {
                this.EntityPM.VolumetricWeight = AppTool.GetWeightFromWeight(this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeight);
                this.EntityPM.Volume = AppTool.GetVolumeFromWeight(this.ShipmentPM.ChargeableWeightUnitCode, this.ShipmentPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.ShipmentPM.Ratio);

                this.SetUIProperties();
                this.fatherComponent.ComputeTotals();
            }
        }
    }

    private ComputeVolume() {

        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        this.Volume = AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.GrossWeight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode);
    }
    private ComputeVolumetricWeight() {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        this.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.GrossWeight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode);
    }
}
