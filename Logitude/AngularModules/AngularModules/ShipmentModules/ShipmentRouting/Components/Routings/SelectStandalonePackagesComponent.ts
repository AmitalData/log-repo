import { Component } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { ShipmentPickUpPM } from '../../../../Shipment/EntityPMs/ShipmentPickUpPM';
import { ShipmentPackagePM } from '../../../../Shipment/EntityPMs/ShipmentPackagePM';
import { ShipmentPickUpDeliveryPackagePM } from '../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ShipmentDeliveryPM } from '../../../../Shipment/EntityPMs/ShipmentDeliveryPM';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { ShipmentPackageItem, PackagesTabComponent } from '../../../ShipmentPackages/Components/Packages/PackagesTabComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';

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
    public IsFromShipmentPackageTab: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsVisible = false;
    public IsAddNewContainerVisible: boolean = true;
    constructor(private _entityResourceService: EntityResourceService) {

    }

    SetWindowArgs(args: any) {
        this._entityResourceService.getEntityResourceByTableName("ShipmentPickUpDeliveryPackage", 0).subscribe((response: any) => {
            this.IsVisible = true;
            this.EntityPM = args.EntityPM;
            this.ShipmentPM = args.ShipmentPM;
            this.IsLCLEntity = args.IsLCLEntity;
            this.IsFCLEntity = args.IsFCLEntity;
            this.TransportModeId = args.TransportModeId;
            this.IsFromShipmentPackageTab = args.IsFromShipmentPackageTab;

            if (this.EntityPM instanceof ShipmentDeliveryPM) {
                this.IsAddNewContainerVisible = false;
            }

            if (this.IsFromShipmentPackageTab) {
                this.SetShipmentLabels();
                this.BuildShipmetItemsSource();
            }
            else {
                this.SetPickupDeliveryLabels();
                this.BuildPickupDeliveryItemsSource();
            }
        });
    }

    public VolumeLabel: string;
    public GrossWeightLabel: string;
    SetShipmentLabels() {
        this.VolumeLabel = TextCodeTranslator.Translate('Shipment.O.Packages.Volume').replace('%UnitCode', this.ShipmentPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate('Shipment.O.Packages.GrossWeight').replace('%UnitCode', this.ShipmentPM.GrossWeightUnitCode);
    }

    SetPickupDeliveryLabels() {
        this.VolumeLabel = TextCodeTranslator.Translate('ShipmentPickUpDeliveryPackage.O.Packages.Volume').replace('%UnitCode', this.ShipmentPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate('ShipmentPickUpDeliveryPackage.O.Packages.GrossWeight').replace('%UnitCode', this.ShipmentPM.GrossWeightUnitCode);
    }

    get IsAddNewContainerEnabled() {
        var myResult: boolean = true;
        if (this.IsFromShipmentPackageTab) {
            myResult = this.ItemsSource.filter(f => f.IsChecked).length > 0 ? false : myResult;
        }

        else {
            if (this.EntityPM != null && AppTool.IsNullOrEmpty(this.EntityPM.StandaloneShipmentId)) {
                myResult = true;
            }

            else {
                myResult = this.ItemsSource.filter(f => f.IsChecked).length > 0 ? false : myResult;
            }
        }
        return myResult;
    }

    public SelectedItem: PackagesSelectItem = null;
    public PackageTypeColumnWidth: number = 80;
    BuildPickupDeliveryItemsSource() {
        this.ItemsSource = [];
        this.SelectedItem = null;
        var myPackageTypeColumnWidth: number = 80;

        var pickUpDliveryPackagescontainersIds: string[] = [];
        var sameParent: boolean = false;

        if (this.EntityPM instanceof ShipmentPickUpPM) {
            this.ShipmentPM.ShipmentPickUps.forEach(item => {
                sameParent = false;

                if (!AppTool.IsNullOrEmpty(item.ParentPickUpDeliveryId) && !AppTool.IsNullOrEmpty(this.EntityPM.ParentPickUpDeliveryId)) {
                    if (item.ParentPickUpDeliveryId == this.EntityPM.ParentPickUpDeliveryId) {
                        sameParent = true;
                    }
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.ParentPickUpDeliveryId)) {
                    item.ShipmentPickUpDeliveryPackages.forEach(item1 => {
                        pickUpDliveryPackagescontainersIds.push(item1.ContainerEntityId);
                    });
                }

                else {
                    if (!sameParent) {
                        if (item.Id != this.EntityPM.ParentPickUpDeliveryId) {
                            item.ShipmentPickUpDeliveryPackages.forEach(item1 => {
                                pickUpDliveryPackagescontainersIds.push(item1.ContainerEntityId);
                            });
                        }
                    }                    
                }
            });
        }

        else if (this.EntityPM instanceof ShipmentDeliveryPM) {
            this.ShipmentPM.ShipmentDeliveries.forEach(item => {
                sameParent = false;

                if (!AppTool.IsNullOrEmpty(item.ParentPickUpDeliveryId) && !AppTool.IsNullOrEmpty(this.EntityPM.ParentPickUpDeliveryId)) {
                    if (item.ParentPickUpDeliveryId == this.EntityPM.ParentPickUpDeliveryId) {
                        sameParent = true;
                    }
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.ParentPickUpDeliveryId)) {
                    item.ShipmentPickUpDeliveryPackages.forEach(item1 => {
                        pickUpDliveryPackagescontainersIds.push(item1.ContainerEntityId);
                    });
                }

                else {
                    if (!sameParent) {
                        if (item.Id != this.EntityPM.ParentPickUpDeliveryId) {
                            item.ShipmentPickUpDeliveryPackages.forEach(item1 => {
                                pickUpDliveryPackagescontainersIds.push(item1.ContainerEntityId);
                            });
                        }
                    }
                }
            });
        }

        if (this.EntityPM.ShipmentPickUpDeliveryPackages != null) {
            this.EntityPM.ShipmentPickUpDeliveryPackages.forEach(item => {
                pickUpDliveryPackagescontainersIds.push(item.ContainerEntityId);
            });
        }

        this.ShipmentPM.ShipmentPackages.filter(d => AppTool.IsNullOrEmpty(d.ContainerEntityId) || pickUpDliveryPackagescontainersIds.indexOf(d.ContainerEntityId) == -1).forEach(item => {
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

    BuildShipmetItemsSource() {
        this.ItemsSource = [];
        this.SelectedItem = null;
        var myPackageTypeColumnWidth: number = 80;

        var pickUpDliveryPackagescontainersIds: string[] = [];
        if (this.ShipmentPM.ShipmentPickUps != null) {
            this.ShipmentPM.ShipmentPickUps.forEach(pickUp => {
                pickUp.ShipmentPickUpDeliveryPackages.forEach(item => {
                    pickUpDliveryPackagescontainersIds.push(item.ContainerEntityId);
                });
            });
        }

        if (this.ShipmentPM.ShipmentDeliveries != null) {
            this.ShipmentPM.ShipmentDeliveries.forEach(delivery => {
                delivery.ShipmentPickUpDeliveryPackages.forEach(item => {
                    pickUpDliveryPackagescontainersIds.push(item.ContainerEntityId);
                });
            });
        }

        this.ShipmentPM.ShipmentPackages.filter(d => AppTool.IsNullOrEmpty(d.ContainerEntityId) || pickUpDliveryPackagescontainersIds.indexOf(d.ContainerEntityId) == -1).forEach(item => {
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
        if (this.IsFromShipmentPackageTab) {
            isChecked = this.ItemsSource.filter(f => f.IsChecked)[0] ? true : isChecked;
        } else {
            if (this.EntityPM != null && AppTool.IsNullOrEmpty(this.EntityPM.StandaloneShipmentId)) {
                isChecked = true;
            } else {
                isChecked = this.ItemsSource.filter(f => f.IsChecked)[0] ? true : isChecked;
            }
        }
        this.IsOkButtonEnabled = isChecked;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        if (!this.IsFromShipmentPackageTab) {
            this.AddShipmentPickUpDeliveryPackagePM();
        }

        this.CurrentSession.CloseCurrentWindowEmit("OK");
    }

    AddShipmentPickUpDeliveryPackagePM() {
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
            this.EntityPM.AddPackage(newPackage);
            item.EntityPM.IsPackageAddedManually = true;
            item.EntityPM.IsPackageAddedManually = false;
        });
    }

    AddContainerClicked() {
        var newShipmentPackage = new ShipmentPackagePM(null);
        newShipmentPackage.NonActiveContainer = false;
        newShipmentPackage.Quantity = 1;
        newShipmentPackage.IsContainer = true;
        newShipmentPackage.Tenant = SessionLocator.Tenant;
        newShipmentPackage.TemperatureUnitCode = SessionLocator.TenantPM.TemperatureUnitCode;
        newShipmentPackage.FlashPointTemperatureUnitCode = SessionLocator.TenantPM.TemperatureUnitCode;
        newShipmentPackage.IsPackageAddedManually = true;

        var logWindow = new LogitudeWindow();
        logWindow.Title = TextCodeTranslator.Translate("ShipmentPackage.O.AddContainer");

        if (this.TransportModeId == "I") {
            logWindow.Title = TextCodeTranslator.Translate("ShipmentPackage.O.AddFullTruckLoad");
        }

        logWindow.Width = 940;
        logWindow.Height = 610;

        var entityArgs: EntityArgs = new EntityArgs();
        entityArgs.EntityPM = this.ShipmentPM;
        entityArgs.ObjectTableName = "Shipment";

        var packagesTabComponent: PackagesTabComponent = new PackagesTabComponent(entityArgs, new EntityResourceService());
        packagesTabComponent.ngOnInit();
        var itemComponent = new ShipmentPackageItem(newShipmentPackage, packagesTabComponent, true);
        logWindow.DataContext = itemComponent;
        logWindow.Show('./ShipmentModules/ShipmentPackages/Components/Packages/AddEditOceanPackageComponent');
        logWindow.WindowClosed.subscribe((s: any) => {
            if (s) {
                this.BuildPickupDeliveryItemsSource();
            }
        });
    }
}
export class PackagesSelectItem {
    public EntityPM: ShipmentPackagePM;
    public IsContainer: boolean = false;
    constructor(entity: ShipmentPackagePM, private fatherComponent: SelectStandalonePackagesComponent) {
        this.EntityPM = entity;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageTypeId)) {
            this.IsContainer = this.EntityPM.IsContainer;
        }

        if (this.EntityPM.IsPackageAddedManually) {
            this.IsChecked = true;
        }
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
        if (this.fatherComponent.IsFromShipmentPackageTab) {
            myResult = this.fatherComponent.ItemsSource.filter(f => f.IsChecked).length > 0 && !this.IsChecked ? false : myResult;
        } else {
            if (this.EntityPM != null && AppTool.IsNullOrEmpty(this.fatherComponent.EntityPM.StandaloneShipmentId)) {
                myResult = true;
            } else {
                myResult = this.fatherComponent.ItemsSource.filter(f => f.IsChecked).length > 0 && !this.IsChecked ? false : myResult;
            }
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
