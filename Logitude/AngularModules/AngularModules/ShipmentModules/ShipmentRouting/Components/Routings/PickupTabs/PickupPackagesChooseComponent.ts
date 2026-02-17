import {Component} from '@angular/core';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPickUpPM} from '../../../../../Shipment/EntityPMs/ShipmentPickUpPM';
import {ShipmentPackagePM} from '../../../../../Shipment/EntityPMs/ShipmentPackagePM';
import {InsideShipmentPackagePM} from '../../../../../Shipment/EntityPMs/InsideShipmentPackagePM';
import {ShipmentPickUpDeliveryPackagePM} from '../../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM';
import {PickupPackagesTabComponent} from './PickupPackagesTabComponent';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { PickUpDeliveryPackageHarmonizePM } from '../../../../../Shipment/EntityPMs/PickUpDeliveryPackageHarmonizePM';
import { ShipmentPackageHarmonizePM } from '../../../../../Shipment/EntityPMs/ShipmentPackageHarmonizePM';

@Component({
    moduleId: module.id,
    templateUrl: './PickupPackagesChooseComponent.html',
})

export class PickupPackagesChooseComponent {
    public EntityPM: ShipmentPickUpPM;
    public ShipmentPM: ShipmentPM;
    public DataContext = this;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public TransportModeId: string;
    public ItemsSource: PickupPackagesChooseItem[] = [];
    public IsOkButtonEnabled: boolean = false;
    private fatherComponent: PickupPackagesTabComponent;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetWindowArgs(args: PickupPackagesTabComponent) {
        this.fatherComponent = args;
        this.EntityPM = args.EntityPM;
        this.ShipmentPM = args.ShipmentPM;
        this.IsLCLEntity = args.IsLCLEntity;
        this.IsFCLEntity = args.IsFCLEntity;
        this.TransportModeId = args.TransportModeId;
        this.SetLabels();
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

    public SelectedItem: PickupPackagesChooseItem = null;
    public PackageTypeColumnWidth: number = 80;
    BuildItemsSource() {
        this.ItemsSource = [];
        this.SelectedItem = null;
        var myPackageTypeColumnWidth: number = 80;

        this.ShipmentPM.ShipmentPackages.forEach(item => {
            var widthOfLabel = AppTool.GetTextWidth(item.PackageTypeName);
            if (widthOfLabel > myPackageTypeColumnWidth) {
                myPackageTypeColumnWidth = widthOfLabel;
            }

            this.ItemsSource.push(new PickupPackagesChooseItem(item, null, this));

            item.InsideShipmentPackages.forEach(insideItem => {
                this.ItemsSource.push(new PickupPackagesChooseItem(item, insideItem, this));
            });
        });

        if (myPackageTypeColumnWidth > 190) {
            myPackageTypeColumnWidth = 190;
        }

        this.PackageTypeColumnWidth = myPackageTypeColumnWidth;
        this.OnItemsChecked();
    }

    OnItemsChecked() {
        var isChecked: boolean = false;

        if (this.ItemsSource) {
            if (this.ItemsSource.filter(f => f.IsChecked)[0]) {
                isChecked = true;
            }
        }

        this.IsOkButtonEnabled = isChecked;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ItemsSource.filter(f => f.IsChecked).forEach(item => {
            var newPickUpPackPM = new ShipmentPickUpDeliveryPackagePM(this.EntityPM);
            newPickUpPackPM.Tenant = this.EntityPM.Tenant;
            newPickUpPackPM.ContainerNumber = item.ContainerNumber;
            newPickUpPackPM.Description = item.Description;
            newPickUpPackPM.PackageTypeId = item.PackageTypeId;
            newPickUpPackPM.PackageTypeName = item.PackageTypeName;
            newPickUpPackPM.Quantity = item.Quantity;
            newPickUpPackPM.Volume = item.Volume;
            newPickUpPackPM.Weight = item.Weight;
            newPickUpPackPM.ShipperSeal = item.ShipperSeal;
            newPickUpPackPM.Width = item.Width;
            newPickUpPackPM.Height = item.Height;
            newPickUpPackPM.Length = item.Length;
            newPickUpPackPM.ShipmentPickUpDeliveryId = this.EntityPM.Id;
            newPickUpPackPM.IsMultiHarmonize = item.IsMultiHarmonize;

            item.HarmonizeList.forEach(harmonizeItem => {
                var newHarmonizePM = new PickUpDeliveryPackageHarmonizePM(this.EntityPM);
                newHarmonizePM.Tenant = harmonizeItem.Tenant;
                newHarmonizePM.Harmonize = harmonizeItem.Harmonize;

                newPickUpPackPM.AddPickUpDeliveryPackageHarmonizePM(newHarmonizePM);
            });

            this.EntityPM.AddPackage(newPickUpPackPM);
        });

        this.fatherComponent.BuildItemsSource();
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    }
}
export class PickupPackagesChooseItem {
    public EntityPM: ShipmentPackagePM;
    public InsideEntityPM: InsideShipmentPackagePM;
    public IsContainer: boolean = false;
    public HarmonizeList: ShipmentPackageHarmonizePM[];
    constructor(entity: ShipmentPackagePM, InsideEntityPM: InsideShipmentPackagePM, private fatherComponent: PickupPackagesChooseComponent) {
        this.EntityPM = entity;
        this.InsideEntityPM = InsideEntityPM;

        if (this.InsideEntityPM != null && !AppTool.IsNullOrEmpty(this.InsideEntityPM.PackageTypeId)) {
            this.IsContainer = this.InsideEntityPM.IsContainer;
        }

        else if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageTypeId)) {
            this.IsContainer = this.EntityPM.IsContainer;
        }

        this.HarmonizeList = entity.ShipmentPackageHarmonizes;
    }

    private isChecked: boolean = false;
    get IsChecked() { return this.isChecked; }
    set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;
            this.fatherComponent.OnItemsChecked();
        }
    }

    get PackageTypeId() { return this.InsideEntityPM != null ? this.InsideEntityPM.PackageTypeId : this.EntityPM.PackageTypeId; }
    get PackageTypeName() { return this.InsideEntityPM != null ? this.InsideEntityPM.PackageTypeName : this.EntityPM.PackageTypeName; }
    get ContainerNumber() { return this.EntityPM.ContainerNumber; }
    get Description() { return this.InsideEntityPM != null ? this.InsideEntityPM.Description : this.EntityPM.Description; }
    get Quantity() { return this.InsideEntityPM != null ? this.InsideEntityPM.Quantity : this.EntityPM.Quantity; }
    get Weight() { return this.InsideEntityPM != null ? this.InsideEntityPM.Weight : this.EntityPM.Weight; }
    get Volume() { return this.InsideEntityPM != null ? this.InsideEntityPM.Volume : this.EntityPM.Volume; }
    get Length() { return this.InsideEntityPM != null ? this.InsideEntityPM.Length : this.EntityPM.Length; }
    get Width() { return this.InsideEntityPM != null ? this.InsideEntityPM.Width : this.EntityPM.Width; }
    get Height() { return this.InsideEntityPM != null ? this.InsideEntityPM.Height : this.EntityPM.Height; }
    get ShipperSeal() { return this.EntityPM.ShipperSeal; }
    get IsMultiHarmonize() { return this.EntityPM.IsMultiHarmonize; }

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
