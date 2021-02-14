import {Component} from '@angular/core';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPackagePM} from '../../../../../Shipment/EntityPMs/ShipmentPackagePM';
import {InsideShipmentPackagePM} from '../../../../../Shipment/EntityPMs/InsideShipmentPackagePM';
import {PackagesTabComponent} from '../../../../ShipmentPackages/Components/Packages/PackagesTabComponent';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../../../Controls/Windows/MessageWindow';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceLocator} from '../../../../../Infrastructure/Locators/ServiceLocator';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';

@Component({
    
    templateUrl: './GroupageComponent.html',
})

export class GroupageComponent {
    public EntityPM: ShipmentPM;
    public FatherComponent: PackagesTabComponent;
    public AllPackages: ShipmentPackagePM[] = [];
    public ShipmentsPackages: GroupageListItem[] = [];
    public MyGroupagePackages: GroupageListItem[] = [];
    public ToggleItems: ToggleItem[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetWindowArgs(args: any) {
        this.AllPackages = args['AllPackages'];
        this.FatherComponent = args['FatherComponent'];
        this.EntityPM = this.FatherComponent.EntityPM;

        this.SetLabels();                
        this.BuildMyGroupagePackages();
        this.BuildShipmentsPackages();
    }

    public VolumeColumnHeader: string;
    public WeightColumnHeader: string;
    public DimensionsColumnHeader: string;
    SetLabels() {
        this.VolumeColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.Volume").replace("%UnitCode", this.EntityPM.VolumeUnitCode);
        this.WeightColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.GrossWeight").replace("%UnitCode", this.EntityPM.GrossWeightUnitCode);
        this.DimensionsColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.Dimensions").replace("%UnitCode", this.EntityPM.DimensionsUnitCode);
    }

    BuildShipmentsPackages() {
        this.ShipmentsPackages = [];

        this.AllPackages.forEach(item => {

            var isAlreadyAddedToContainer: boolean = false;
            this.MyGroupagePackages.forEach(itemContainer => {
                if (!isAlreadyAddedToContainer) {
                    var itemInsideContainer = itemContainer.ItemsSource.filter(f => f.EntityPM.OriginalShipmentPackageId == item.Id)[0];
                    if (itemInsideContainer) {
                        isAlreadyAddedToContainer = true;
                    }
                }
            });

            if (!isAlreadyAddedToContainer) {
                var itemComponent = new GroupageListItem(item, this, false);
                this.ShipmentsPackages.push(itemComponent);
                itemComponent.UpdateItem();
            }
        });
    }
    BuildMyGroupagePackages() {
        this.MyGroupagePackages = [];

        this.EntityPM.ShipmentPackages.forEach(item => {
            var itemComponent = new GroupageListItem(item, this, true);
            this.MyGroupagePackages.push(itemComponent);
            itemComponent.UpdateItem();
        });

        this.BuildToggleItems();
    }
    BuildToggleItems() {
        this.ToggleItems = [];

        this.MyGroupagePackages.forEach(item => {
            var index: number = this.MyGroupagePackages.indexOf(item) + 1;
            var itemString = "Container #" + index;

            if (!AppTool.IsNullOrEmpty(item.ContainerNumber)) {
                itemString += ": " + item.ContainerNumber;
            }

            this.ToggleItems.push(new ToggleItem(itemString, item.ContainerNumber, index));
        });

        this.ToggleItems.push(new ToggleItem("New Container", null));
    }

    AddButtonClicked(toggleItem: ToggleItem, shipmentListItem: GroupageListItem) {
        if (toggleItem.Label == "New Container") {
            this.AddToNewContainer(toggleItem, shipmentListItem);
        }
        else {
            this.AddToExistingContainer(toggleItem, shipmentListItem);
        }
    }
    AddToNewContainer(toggleItem: ToggleItem, shipmentListItem: GroupageListItem) {
        var shipmentPackagePM = new ShipmentPackagePM(null);
        shipmentPackagePM.Tenant = SessionLocator.Tenant;
        shipmentPackagePM.ShipmentId = this.EntityPM.Id;
        shipmentPackagePM.ShipmentNumber = this.EntityPM.ShipmentNumber;
        shipmentPackagePM.IsContainer = true;
        shipmentPackagePM.Quantity = 1;
        shipmentPackagePM.ContainerNumber = shipmentListItem.ContainerNumber;
        shipmentPackagePM.Weight = 0;

        var insideShipmentPack = new InsideShipmentPackagePM(shipmentPackagePM);
        insideShipmentPack.Quantity = shipmentListItem.EntityPM.Quantity;
        insideShipmentPack.Height = shipmentListItem.EntityPM.Height;
        insideShipmentPack.Length = shipmentListItem.EntityPM.Length;
        insideShipmentPack.Width = shipmentListItem.EntityPM.Width;
        insideShipmentPack.Weight = shipmentListItem.EntityPM.Weight;
        shipmentPackagePM.PackageTypeId = shipmentListItem.EntityPM.PackageTypeId;
        insideShipmentPack.PackageTypeName = shipmentListItem.EntityPM.PackageTypeName;
        insideShipmentPack.Volume = shipmentListItem.EntityPM.Volume;
        insideShipmentPack.VolumetricWeight = shipmentListItem.EntityPM.VolumetricWeight;
        insideShipmentPack.Tenant = shipmentListItem.EntityPM.Tenant;
        insideShipmentPack.Description = shipmentListItem.EntityPM.Description;
        insideShipmentPack.OriginalShipmentPackageId = shipmentListItem.EntityPM.Id;
        insideShipmentPack.Reference1 = shipmentListItem.Reference1;
        insideShipmentPack.Reference2 = shipmentListItem.Reference2;
        insideShipmentPack.Reference3 = shipmentListItem.Reference3;
        insideShipmentPack.Reference4 = shipmentListItem.Reference4;
        insideShipmentPack.CommodityNumber = shipmentListItem.CommodityNumber;
        insideShipmentPack.CommodityName = shipmentListItem.CommodityName;
        shipmentPackagePM.AddInsideShipmentPackagePM(insideShipmentPack);

        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { EntityPM: shipmentPackagePM, ShipmentListItem: shipmentListItem, FatherComponent: this };
        logWindow.Title = "New Container";
        logWindow.Show("./ShipmentModules/ShipmentTabs/Components/Windows/Groupage/GroupageContainerComponent");
    }
    AddToExistingContainer(toggleItem: ToggleItem, shipmentListItem: GroupageListItem) {

        var allMatchedContainers = this.MyGroupagePackages.filter(f => f.ContainerNumber == toggleItem.ContainerNumber);

        if (allMatchedContainers.length > 1) {
            allMatchedContainers = this.MyGroupagePackages.filter(f => f.Index == toggleItem.Index);
        }

        var MasterListItem: GroupageListItem = allMatchedContainers[0];
        if (MasterListItem) {

            if (shipmentListItem.EntityPM.LCLContainerTypeId != MasterListItem.EntityPM.PackageTypeId) {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Show(" Please notice that the container type on the package level will be adjusted ");
                confirmWindow.NoButtonText = "Cancel";
                confirmWindow.YesButtonText = "Ok";
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        //shipmentListItem.EntityPM.LCLContainerTypeId = MasterListItem.EntityPM.PackageTypeId;
                        this.AddInsideShipmentPackage(MasterListItem, toggleItem, shipmentListItem);
                    }
                });
            }
            else {
                this.AddInsideShipmentPackage(MasterListItem, toggleItem, shipmentListItem);
            }
        }
    }

    AddInsideShipmentPackage(MasterListItem: GroupageListItem, toggleItem: ToggleItem, shipmentListItem: GroupageListItem) {
        var insideShipmentPack = new InsideShipmentPackagePM(null);
        insideShipmentPack.Quantity = shipmentListItem.EntityPM.Quantity;
        insideShipmentPack.Height = shipmentListItem.EntityPM.Height;
        insideShipmentPack.Length = shipmentListItem.EntityPM.Length;
        insideShipmentPack.Width = shipmentListItem.EntityPM.Width;
        insideShipmentPack.Weight = shipmentListItem.EntityPM.Weight;
        insideShipmentPack.PackageTypeId = shipmentListItem.EntityPM.PackageTypeId;
        insideShipmentPack.PackageTypeName = shipmentListItem.EntityPM.PackageTypeName;
        insideShipmentPack.Volume = shipmentListItem.EntityPM.Volume;
        insideShipmentPack.VolumetricWeight = shipmentListItem.EntityPM.VolumetricWeight;
        insideShipmentPack.Tenant = shipmentListItem.EntityPM.Tenant;
        insideShipmentPack.Description = shipmentListItem.EntityPM.Description;
        insideShipmentPack.OriginalShipmentPackageId = shipmentListItem.EntityPM.Id;
        insideShipmentPack.Reference1 = shipmentListItem.Reference1;
        insideShipmentPack.Reference2 = shipmentListItem.Reference2;
        insideShipmentPack.Reference3 = shipmentListItem.Reference3;
        insideShipmentPack.Reference4 = shipmentListItem.Reference4;
        insideShipmentPack.CommodityNumber = shipmentListItem.CommodityNumber;
        insideShipmentPack.CommodityName = shipmentListItem.CommodityName;
        MasterListItem.EntityPM.AddInsideShipmentPackagePM(insideShipmentPack);

        var indexOfItem = this.ShipmentsPackages.indexOf(shipmentListItem);
        if (indexOfItem > -1) {
            this.ShipmentsPackages.splice(indexOfItem, 1);
        }

        MasterListItem.BuildItems();
        MasterListItem.ComputeFromInsidePackages();
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {

        if (this.EntityPM.ShipmentPackages.length > 0 || this.MyGroupagePackages.length > 0) {

            this.EntityPM.ShipmentPackages.forEach(item => {
                this.EntityPM.RemovePackage(item);
            });

            this.MyGroupagePackages.forEach(item => {
                var newPackage = new ShipmentPackagePM(this.EntityPM);
                newPackage.ShipmentId = this.EntityPM.Id;
                newPackage.ShipmentNumber = this.EntityPM.ShipmentNumber;
                newPackage.Tenant = this.EntityPM.Tenant;
                newPackage.ClassNumber = item.EntityPM.ClassNumber;
                newPackage.ContainerNumber = item.EntityPM.ContainerNumber;
                newPackage.Description = item.EntityPM.Description;
                newPackage.FlashPoint = item.EntityPM.FlashPoint;
                newPackage.Harmonize = item.EntityPM.Harmonize;
                newPackage.Height = item.EntityPM.Height;
                newPackage.IMDGCode = item.EntityPM.IMDGCode;
                newPackage.IsContainer = item.EntityPM.IsContainer;
                newPackage.IsDangerous = item.EntityPM.IsDangerous;
                newPackage.Length = item.EntityPM.Length;
                newPackage.MarksAndNumbers = item.EntityPM.MarksAndNumbers;
                newPackage.MaterialDescription = item.EntityPM.MaterialDescription;
                newPackage.PackageTypeId = item.EntityPM.PackageTypeId;
                newPackage.LCLContainerTypeId = item.EntityPM.LCLContainerTypeId;
                newPackage.PackageTypeName = item.EntityPM.PackageTypeName;
                newPackage.PackagingGroup = item.EntityPM.PackagingGroup;
                newPackage.Quantity = item.EntityPM.Quantity;
                newPackage.ShipperSeal = item.EntityPM.ShipperSeal;
                newPackage.CarrierSeal = item.EntityPM.CarrierSeal;
                newPackage.SOC = item.EntityPM.SOC;
                newPackage.Tare = item.EntityPM.Tare;
                newPackage.Temperature = item.EntityPM.Temperature;
                newPackage.UnNumber = item.EntityPM.UnNumber;
                newPackage.Ventilation = item.EntityPM.Ventilation;
                newPackage.Volume = item.EntityPM.Volume;
                newPackage.VolumetricWeight = item.EntityPM.VolumetricWeight;
                newPackage.Weight = item.EntityPM.Weight;
                newPackage.Width = item.EntityPM.Width;
                newPackage.OriginalShipmentPackageId = item.EntityPM.OriginalShipmentPackageId;
                newPackage.Reference1 = item.EntityPM.Reference1;
                newPackage.Reference2 = item.EntityPM.Reference2;
                newPackage.Reference3 = item.EntityPM.Reference3;
                newPackage.Reference4 = item.EntityPM.Reference4;
                newPackage.CommodityNumber = item.EntityPM.CommodityNumber;
                newPackage.CommodityName = item.EntityPM.CommodityName;
                newPackage.HorseId = item.EntityPM.HorseId;
                newPackage.HorseName = item.EntityPM.HorseName;
                this.EntityPM.AddPackage(newPackage);

                item.EntityPM.InsideShipmentPackages.forEach(insideItem => {
                    var newInsidePackage = new InsideShipmentPackagePM(newPackage);
                    newInsidePackage.ShipmentPackageId = newPackage.Id;
                    newInsidePackage.Quantity = insideItem.Quantity;
                    newInsidePackage.Height = insideItem.Height;
                    newInsidePackage.Length = insideItem.Length;
                    newInsidePackage.Width = insideItem.Width;
                    newInsidePackage.Weight = insideItem.Weight;
                    newInsidePackage.PackageTypeId = insideItem.PackageTypeId;
                    newInsidePackage.PackageTypeName = insideItem.PackageTypeName;
                    newInsidePackage.Volume = insideItem.Volume;
                    newInsidePackage.VolumetricWeight = insideItem.VolumetricWeight;
                    newInsidePackage.Tenant = insideItem.Tenant;
                    newInsidePackage.Description = insideItem.Description;
                    newInsidePackage.OriginalShipmentPackageId = insideItem.OriginalShipmentPackageId;
                    newInsidePackage.OriginalInsideShipmentPackageId = insideItem.OriginalInsideShipmentPackageId;
                    newInsidePackage.Reference1 = insideItem.Reference1;
                    newInsidePackage.Reference2 = insideItem.Reference2;
                    newInsidePackage.Reference3 = insideItem.Reference3;
                    newInsidePackage.Reference4 = insideItem.Reference4;
                    newInsidePackage.CommodityNumber = insideItem.CommodityNumber;
                    newInsidePackage.CommodityName = insideItem.CommodityName;
                    newPackage.AddInsideShipmentPackagePM(newInsidePackage);
                });
            });

            ServiceLocator.SendTotangoUserActivity("Master", "Building packages for ocean groupage");

            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }


        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    }
}
export class GroupageListItem {
    public Index: number = null;
    public IsGroupageItem: boolean = false;
    public EntityPM: ShipmentPackagePM;
    public ShipmentPM: ShipmentPM = null;
    public ItemsSource: GroupageInsideItem[] = [];
    public InsideGridHeight: number = 70;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(item: ShipmentPackagePM, public fatherComponent: GroupageComponent, isGroupage: boolean) {
        this.EntityPM = item;
        this.ShipmentPM = fatherComponent.EntityPM;
        this.IsGroupageItem = isGroupage;
        this.BuildItems();
        this.UpdateItem();
    }

    BuildItems() {
        this.ItemsSource = [];

        this.EntityPM.InsideShipmentPackages.forEach(item => {

            var myRatio = this.fatherComponent.EntityPM.Ratio;
            if (AppTool.IsNullOrZero(myRatio)) {
                myRatio = 1;
            }

            item.VolumetricWeight = (item.Volume * 1000) / myRatio;

            this.ItemsSource.push(new GroupageInsideItem(item));
        });

        this.InsideGridHeight = (this.ItemsSource.length * 26) + 44;
    }
    UpdateItem() {
        if (this.IsGroupageItem) {
            this.Index = this.fatherComponent.MyGroupagePackages.indexOf(this) + 1;
            this.PackageTypeImage = "./Images/Icons/Container.png";
        }

        else {
            this.Index = this.fatherComponent.ShipmentsPackages.indexOf(this) + 1;

            if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageTypeId)) {
                if (this.EntityPM.IsContainer) {
                    this.PackageTypeImage = "./Images/Icons/Container.png";
                }

                else {
                    this.PackageTypeImage = "./Images/Icons/Package.png";
                }
            }
        }
    }
    ComputeFromInsidePackages() {
        if (this.IsGroupageItem) {
            var myWeight = 0;
            var myVolume = 0;

            this.EntityPM.InsideShipmentPackages.forEach(item => {
                if (!AppTool.IsNullOrEmpty(item.Weight)) {
                    myWeight += item.Weight;
                }

                if (!AppTool.IsNullOrEmpty(item.Volume)) {
                    myVolume += item.Volume;
                }
            });

            this.Weight = AppTool.Round(myWeight, 3);
            this.Volume = AppTool.Round(myVolume, 3);
        }
    }

    public PackageTypeImage: string = null;
    get PackageTypeName() { return this.EntityPM.PackageTypeName; }
    get Quantity() { return this.EntityPM.Quantity; }
    get ContainerNumber() { return this.EntityPM.ContainerNumber; }
    get Description() { return this.EntityPM.Description; }
    get ShipmentNumber() { return this.EntityPM.ShipmentNumber; }

    get Weight() { return this.EntityPM.Weight; }
    set Weight(value: number) {
        if (this.EntityPM.Weight != value) {
            this.EntityPM.Weight = AppTool.Round(value, 3);
        }
    }

    get Volume() { return this.EntityPM.Volume; }
    set Volume(value: number) {
        if (this.EntityPM.Volume != value) {
            this.EntityPM.Volume = AppTool.Round(value, 3);
            this.ComputeVolumetricWeight();
        }
    }

    get VolumetricWeight() { return this.EntityPM.VolumetricWeight; }
    set VolumetricWeight(value: number) {
        if (this.EntityPM.VolumetricWeight != value) {
            this.EntityPM.VolumetricWeight = AppTool.Round(value, 3);
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
        }
    }

    get CommodityName() { return this.EntityPM.CommodityName; }
    set CommodityName(newValue: string) {
        if (this.EntityPM.CommodityName != newValue) {
            this.EntityPM.CommodityName = newValue;
        }
    }

    private ComputeVolumetricWeight() {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        this.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(this.EntityPM.Quantity, this.EntityPM.Width, this.EntityPM.Height, this.EntityPM.Length, this.Volume, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode);
    }

    DeleteButtonClicked() {
        if (this.ItemsSource.length > 0) {
            var messageWindow = new MessageWindow();
            messageWindow.Title = "Remove Container";
            messageWindow.Show("Cant remove this container while it contains inside items");
        }

        else {
            var indexOfItem = this.fatherComponent.MyGroupagePackages.indexOf(this);
            if (indexOfItem > -1) {
                this.fatherComponent.MyGroupagePackages.splice(indexOfItem, 1);
            }

            this.fatherComponent.MyGroupagePackages.forEach(item => {
                item.UpdateItem();
            });

            this.fatherComponent.BuildToggleItems();
        }
    }

    DeleteInsideButtonClicked(item: GroupageInsideItem) {
        if (item) {
            this.EntityPM.RemoveInsideShipmentPackagePM(item.EntityPM);

            var indexOfItem = this.ItemsSource.indexOf(item);
            if (indexOfItem > -1) {
                this.ItemsSource.splice(indexOfItem, 1);
            }

            this.ComputeFromInsidePackages();
            this.fatherComponent.BuildToggleItems();
            this.fatherComponent.BuildShipmentsPackages();
        }

        //if (trigger.shipmentPackagePM.InsideShipmentPackages.Contains(insidePackage)) {
        //    trigger.shipmentPackagePM.InsideShipmentPackages.Remove(insidePackage);
        //}

        //if (trigger.InsideObsList.Contains(this)) {
        //    trigger.InsideObsList.Remove(this);
        //}

        //ShipmentPackagePM pp = trigger.Trigger.OriginList.Where(d => d.Id == this.insidePackage.OriginalShipmentPackageId).FirstOrDefault();
        //if (pp != null) {
        //    trigger.Trigger.ShipmentsPackagesList.Add(new BuildPackagesListBoxItemViewModel(trigger.Trigger, pp));
        //    trigger.ComputeFromInsidePackages();
        //}

        //trigger.UpdateInsidePackages();   
    }
}
export class GroupageInsideItem {
    public EntityPM: InsideShipmentPackagePM = null;
    constructor(entity: InsideShipmentPackagePM) {
        this.EntityPM = entity;
    }

    get PackageTypeName() { return this.EntityPM.PackageTypeName; }
    get Quantity() { return this.EntityPM.Quantity; }
    get Weight() { return this.EntityPM.Weight; }
    get Volume() { return this.EntityPM.Volume; }
    get VolumeKG() { return this.EntityPM.VolumetricWeight; }

    get Length() { return this.EntityPM.Length; }
    get Width() { return this.EntityPM.Width; }
    get Height() { return this.EntityPM.Height; }

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
}
class ToggleItem {
    public Index: number = null;
    public Label: string;
    public ContainerNumber: string;
    constructor(label: string, myContainerNumber: string, index: number = null) {
        this.Index = index;
        this.Label = label;
        this.ContainerNumber = myContainerNumber;        
    }
}
