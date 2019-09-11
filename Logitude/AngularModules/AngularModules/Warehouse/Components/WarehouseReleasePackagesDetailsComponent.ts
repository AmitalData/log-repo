declare var System: any;
declare var window: any;
import {AppTool, DateTool, ArrayTool} from '../../Infrastructure/Tools';
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';

import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';

import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import {ConfirmWindow} from '../../Controls/Windows/ConfirmWindow';

import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {WarehouseReleasePM} from '../../Warehouse/EntityPMs/WarehouseReleasePM';
import {WarehouseReleasePackagePM} from '../../Warehouse/EntityPMs/WarehouseReleasePackagePM';

import {WarehouseEntryPackagePM} from '../../Warehouse/EntityPMs/WarehouseEntryPackagePM';



import {PackageTypeList} from '../../Common/EntityLists/PackageTypeList';
import {EventTypeArgs} from '../../Infrastructure/DataContracts/EventTypeArgs';
import {PackageTypeListService} from '../../Common/Services/StandardLists/PackageTypeListService';
import {ClassLevelValidator} from '../../Infrastructure/Validators/ClassLevelValidator';
import {ObservableCollection} from '../../Infrastructure/Utilities/ObservableCollection';
import {WarehouseEntryPackageItem} from '../../Warehouse/Components/AddEditWarehouseEntryPackagesAndContainers';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {WarehouseEntryPackagePMExtendedService} from '../../Warehouse/Services/ExtendedPMs/WarehouseEntryPackagePMExtendedService';

@Component({
    moduleId: module.id,
    selector: 'WarehouseReleasePackagesDetailsComponent',
    templateUrl: './WarehouseReleasePackagesDetailsComponent.html',
    providers: [ WarehouseEntryPackagePMExtendedService],
})

export class WarehouseReleasePackagesDetailsComponent extends BaseComponent implements OnInit {
    public AllPackageTypes: PackageTypeList[] = [];
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    DataContext: any = this;
    ViewModelTrigger: any;
    IsLoadPage: boolean = false;
    IsLCLEntity: boolean = false;
    ShowPackageSummary: boolean;
    WarehouseReleasePackagesLists: WarehouseReleasePackagePM[] = [];
    warehouseReleasePM: WarehouseReleasePM;
    ObjectTableName: string = "WarehouseRelease";
    VolumetricWeightLabel: string;
    VolumeLabel: string;
    GrossWeightLabel: string;
    DimensionsLabel: string;
    VolumeColumnHeader: string;
    WeightColumnHeader: string;
    DimensionsColumnHeader: string;
    VolumetricWeightColumnHeader: string;
    PackageTypeColumnHeader: string;

    ShowAddPackageButton: boolean = false;
    SelectedWarehouseReleasePackage: WarehouseReleasePackagePM;
    private CurrentSession = SessionLocator.SelectedSession;
    IsEditMode: boolean = false;
    IsFromFullWarehouseEntryComponent: boolean = false;
    constructor(private warehouseEntryPackagePMExtendedService: WarehouseEntryPackagePMExtendedService) {
        super();


    }

    ngOnInit(

    ) {


    }

    SetWindowArgs(args: any) {

       
        this.Start(args);
        //this.IsFromFullWarehouseEntryComponent = args.IsFromFullWarehouseEntryComponent;
        //this.IsEditMode = args.IsEditMode;
        //this.ShowPackageSummary = args.ShowPackageSummary;
        //this.ShowAddPackageButton = args.ShowAddPackageButton;
        //if (!this.IsEditMode) {
        //    this.ShowAddPackageButton = true;
        //}

        //this.myPackageTypeService.getAllFromCache().subscribe((resp: any) => {
        //    if (!resp.HasError) {
        //        this.AllPackageTypes = resp.Result;
        //    }

         
        //});


    }

    ShipmentPM: any;
    SetValue() {

        this.TransportModeId = this.warehouseReleasePM.TransportModeId;
        this.DirectionId = this.warehouseReleasePM.DirectionId;
        this.CustomerId = this.warehouseReleasePM.CustomerId;

        if (this.ShipmentPM) {
            this.FromPortId = this.ShipmentPM ? this.ShipmentPM.MainCarriageFromPortId ? this.ShipmentPM.MainCarriageFromPortId : this.ShipmentPM.FromPortId : "";
            this.ToPortId = this.ShipmentPM.ShipmentLevelCode == "H" ? this.ShipmentPM.MainCarriageFinalDestinationPortId : this.ShipmentPM.FinalDistenationPortId;
            if (!this.ToPortId) {
                this.ToPortId = this.ShipmentPM.ToPortId;
            }
        }
        this.IsLCLEntity = AppTool.IsLCLEntity(this.warehouseReleasePM.TransportModeId, this.warehouseReleasePM.ShipmentTypeId);
    }


    SetLabel() {

        this.VolumeLabel = "Volume (" + SessionLocator.TenantPM.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Gross Weight (" + SessionLocator.TenantPM.GrossWeightUnitCode + ")";
        this.DimensionsLabel = "Dim(L-W-H) (" + SessionLocator.TenantPM.DimensionsUnitCode + ")";
        this.WeightColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.GrossWeight").replace("%UnitCode", this.warehouseReleasePM.GrossWeightUnitCode);
        this.DimensionsColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.Dimensions").replace("%UnitCode", this.warehouseReleasePM.DimensionsUnitCode);
        this.VolumetricWeightColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.VolWeight").replace("%UnitCode", this.warehouseReleasePM.ChargeableWeightUnitCode);
        this.PackageTypeColumnHeader = TextCodeTranslator.Translate("ShipmentPackage.F.PackageTypeId");
        this.VolumetricWeightLabel = "Volumetric Weight (" + this.warehouseReleasePM.ChargeableWeightUnitCode + ")";
    }

    Start(args) {
        this.warehouseReleasePM = args.WarehouseEntryPM;
        this.ViewModelTrigger = args.ViewModelTrigger;
        this.IsEditMode = args.IsEditMode;
        
        this.ShipmentPM = args.ShipmentPM;
        
        if (this.warehouseReleasePM) {
            this.WarehouseReleasePackagesLists = this.warehouseReleasePM.WarehouseReleasePackages;

        }
        this.SetLabel();

        this.SetValue();
        
    }


    IsChoosePackageOpen: boolean = false;
    WarehouseId: string;
    IsPackageOpen: boolean = false;
    AllWarehouseEntryPackagesLists: WarehouseEntryPackagePM[] = [];

    ChoosePackage(packageType: string) {
        this.IsChoosePackageOpen = true;

        if (!this.IsPackageOpen) {
            this.IsPackageOpen = true;
            this.IsChoosePackageOpen = true;
            if (this.CustomerId != this.warehouseReleasePM.CustomerId || this.WarehouseId != this.warehouseReleasePM.WarehouseId) {

                var shipmentId: string = this.warehouseReleasePM.ShipmentId;
                this.AllWarehouseEntryPackagesLists = [];
                this.warehouseEntryPackagePMExtendedService.GetWarehouseEntryPackagePMListsByShipmentIdAndWarehouseIdAndCustomerId(shipmentId, this.warehouseReleasePM.CustomerId, this.warehouseReleasePM.WarehouseId, this.warehouseReleasePM.Tenant).subscribe((res: any) => {
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        this.AllWarehouseEntryPackagesLists = pmResponse.Result;
                        this.OpenChoosePackage(packageType);
                    }

                });

            } else {
                this.OpenChoosePackage(packageType);
            }
        }

    }


    DeletePackage(item: WarehouseReleasePackagePM) {

        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this package");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                var index = this.WarehouseReleasePackagesLists.indexOf(item);
                if (index != -1) this.WarehouseReleasePackagesLists.splice(index, 1);

                var entry = this.AllWarehouseEntryPackagesLists.filter(d => d.Id == item.EntryPackageId)[0];
                if (entry) {
                    entry.ReleaseQTY = 0;
                    entry.IsSelected = false;
                }

                if (this.WarehouseReleasePackagesLists.length == 0) {
                    this.warehouseReleasePM.UIProperties.SetEnabled("CustomerId", "WarehouseRelease", true);
                    this.warehouseReleasePM.UIProperties.SetEnabled("WarehouseId", "WarehouseRelease", true);
                }
                this.warehouseReleasePM.WarehouseReleasePackages = this.WarehouseReleasePackagesLists;

            }
        });



    }





    TransportModeId: string;
    DirectionId: string;
    CustomerId: string;
    FromPortId: string;
    ToPortId: string;
    ConnectedTo: string;


    OpenChoosePackage(packageType: string) {
    
        this.WarehouseId = this.warehouseReleasePM.WarehouseId;
        this.CustomerId = this.warehouseReleasePM.CustomerId;

        this.IsPackageOpen = false;
        var windowArgs: any = {};
        windowArgs.WarehouseReleasePM = this.warehouseReleasePM;

        windowArgs.WarehouseEntryPackagesLists = this.AllWarehouseEntryPackagesLists;
        windowArgs.ViewModelTrigger = this;
        windowArgs.PackageType = packageType;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1150;
        logWindow.Height = 550;
        logWindow.Title = "Choose Packages";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Warehouse/Components/WarehouseReleaseChoosePackagesComponent");


        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event) {
                if (this.WarehouseReleasePackagesLists.length > 0 && this.IsChoosePackageOpen) {
                    this.IsChoosePackageOpen = false;
                    this.warehouseReleasePM.UIProperties.SetEnabled("CustomerId", "WarehouseRelease", false);
                    this.warehouseReleasePM.UIProperties.SetEnabled("WarehouseId", "WarehouseRelease", false);
                }

                this.warehouseReleasePM.WarehouseReleasePackages = this.WarehouseReleasePackagesLists;
            }
        });


    }









    CancelButtonClicked() {


        this.CloseButtonClicked();

    }

    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }


    SaveButtonClicked() {


        this.CurrentSession.CurrentWindow.Close("Refresh");
    }




}
