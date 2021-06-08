import { Component } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { ShipmentPickUpPM } from '../../../../Shipment/EntityPMs/ShipmentPickUpPM';
import { ShipmentPackagePM } from '../../../../Shipment/EntityPMs/ShipmentPackagePM';
import { ShipmentPickUpDeliveryPackagePM } from '../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { PickUpDeliveryPackageHarmonizePM } from '../../../../Shipment/EntityPMs/PickUpDeliveryPackageHarmonizePM';
import { ShipmentPackageHarmonizePM } from '../../../../Shipment/EntityPMs/ShipmentPackageHarmonizePM';
import { ShipmentDeliveryPM } from '../../../../Shipment/EntityPMs/ShipmentDeliveryPM';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    templateUrl: './SelectStandalonePackagesComponent.html',
})

export class SelectStandalonePackagesComponent {
    public EntityPM: any;
    public ShipmentPM: ShipmentPM;
    public DataContext = this;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public TransportModeId: string;
    public ItemsSource: PackagesSelectItem[] = [];
    public IsOkButtonEnabled: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;    
    constructor() {

    }

    SetWindowArgs(args: any) {
        this.EntityPM = args.EntityPM;
        this.ShipmentPM = args.ShipmentPM;
        this.IsLCLEntity = args.IsLCLEntity;
        this.IsFCLEntity = args.IsFCLEntity;
        this.TransportModeId = args.TransportModeId;

        this.SetLabels();
        this.BuildItemsSource();
    }

    public VolumeLabel: string;
    public GrossWeightLabel: string;
    SetLabels() {
        this.VolumeLabel = TextCodeTranslator.Translate('ShipmentPickUpDeliveryPackage.O.Packages.Volume').replace('%UnitCode', this.ShipmentPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate('ShipmentPickUpDeliveryPackage.O.Packages.GrossWeight').replace('%UnitCode', this.ShipmentPM.GrossWeightUnitCode);
    }

    get IsAddNewContainerEnabled() {
        var myResult: boolean = true;

        if (this.ItemsSource.filter(f => f.IsChecked).length > 0) {
            myResult = false;
        }

        return myResult;
    }

    public SelectedItem: PackagesSelectItem = null;
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

            this.ItemsSource.push(new PackagesSelectItem(item, this));
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
            var newPackage = new ShipmentPickUpDeliveryPackagePM(this.EntityPM);
            newPackage.Tenant = this.EntityPM.Tenant;
            newPackage.ContainerNumber = item.ContainerNumber;
            newPackage.Description = item.Description;
            newPackage.PackageTypeId = item.PackageTypeId;
            newPackage.PackageTypeName = item.PackageTypeName;
            newPackage.Quantity = item.Quantity;
            newPackage.Volume = item.Volume;
            newPackage.Weight = item.Weight;            
            newPackage.ShipmentPickUpDeliveryId = this.EntityPM.Id;
            newPackage.ContainerEntityId = item.ContainerEntityId;

            //newPickUnewPackagepPackPM.IsMultiHarmonize = item.IsMultiHarmonize;

            //item.HarmonizeList.forEach(harmonizeItem => {
            //    var newHarmonizePM = new PickUpDeliveryPackageHarmonizePM(this.EntityPM);
            //    newHarmonizePM.Tenant = harmonizeItem.Tenant;
            //    newHarmonizePM.Harmonize = harmonizeItem.Harmonize;

            //    newPackage.AddPickUpDeliveryPackageHarmonizePM(newHarmonizePM);
            //});

            this.EntityPM.AddPackage(newPackage);
        });

        //this.fatherComponent.BuildItemsSource();
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    }

    AddContainerClicked() {
        //var logWindow = new LogitudeWindow();
        //var itemPM = new ShipmentPackagePM(null);
        //itemPM.NonActiveContainer = false;

        //itemPM.Quantity = 1;
        //itemPM.IsContainer = true;
        //itemPM.Tenant = SessionLocator.Tenant;
        //itemPM.TemperatureUnitCode = SessionLocator.TenantPM.TemperatureUnitCode;
        //itemPM.FlashPointTemperatureUnitCode = SessionLocator.TenantPM.TemperatureUnitCode;
        //logWindow.Title = TextCodeTranslator.Translate("ShipmentPackage.O.AddContainer");

        //if (this.TransportModeId == "I") {
        //    logWindow.Title = TextCodeTranslator.Translate("ShipmentPackage.O.AddFullTruckLoad");
        //}

        //logWindow.Width = 940;
        //logWindow.Height = 610;

        //var itemComponent = new ShipmentPackageItem(itemPM, this, true);
        //logWindow.DataContext = itemComponent;
        //logWindow.Show('./ShipmentModules/ShipmentPackages / Components / Packages / AddEditOceanPackageComponent');
    }
}
export class PackagesSelectItem {
    public EntityPM: ShipmentPackagePM;
    public IsContainer: boolean = false;
    public HarmonizeList: ShipmentPackageHarmonizePM[];
    constructor(entity: ShipmentPackagePM, private fatherComponent: SelectStandalonePackagesComponent) {
        this.EntityPM = entity;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageTypeId)) {
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

    get IsSelectNewContainerEnabled() {
        var myResult: boolean = true;

        if (this.fatherComponent.ItemsSource.filter(f => f.IsChecked).length > 0 && !this.IsChecked) {
            myResult = false;
        }

        return myResult;
    }

    get ContainerEntityId() { return this.EntityPM.ContainerEntityId }
    get PackageTypeId() { return this.EntityPM.PackageTypeId; }
    get PackageTypeName() { return this.EntityPM.PackageTypeName; }
    get ContainerNumber() { return this.EntityPM.ContainerNumber; }
    get Description() { return this.EntityPM.Description; }
    get Quantity() { return this.EntityPM.Quantity; }
    get Weight() { return this.EntityPM.Weight; }
    get Volume() { return this.EntityPM.Volume; }
}
