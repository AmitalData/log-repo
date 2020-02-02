declare var System: any;
declare var window: any;
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import {WarehouseReleasePM} from '../../Warehouse/EntityPMs/WarehouseReleasePM';
import {WarehouseReleasePackagePM} from '../../Warehouse/EntityPMs/WarehouseReleasePackagePM';

import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {EventTypeArgs} from '../../Infrastructure/DataContracts/EventTypeArgs';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {WarehouseReleasePMExtendedService} from '../../Warehouse/Services/ExtendedPMs/WarehouseReleasePMExtendedService';
@Component({
    moduleId: module.id,
    selector: 'ChoosePackagesFromWarehousePackageReleasesComponent',
    templateUrl: './ChoosePackagesFromWarehousePackageReleasesComponent.html',
})

export class ChoosePackagesFromWarehousePackageReleasesComponent extends BaseComponent implements OnInit {

    private _entityResourceService: EntityResourceService = new EntityResourceService();

    ObjectTableName: string = "WarehouseRelelasePackage";
    WarehouseReleasePMLists: WarehouseReleasePM[] = [];
    WarehouseReleaseGroupLists: WarehouseReleaseGroup[]=[];
    AllWarehouseReleaseGroupLists: WarehouseReleaseGroup[] = [];


    warehouseReleasePMExtendedService: WarehouseReleasePMExtendedService;
    DataContext: any = this;
    private CurrentSession = SessionLocator.SelectedSession;
    ViewModelTrigger: any;
    IsLoadPage: boolean = false;
    VolumeLabel: string;
    GrossWeightLabel: string;
    DimensionsUnitCode: string;
    VolumetricWeightLabel: string;
    IsContainerShipment: boolean = false;
    IsShowMessageNoWarehouseRelease: boolean = false;
    IsShowConnectedToOtherShipments: boolean = false;
    constructor() {
        super();

        this.warehouseReleasePMExtendedService = new WarehouseReleasePMExtendedService();
    }

    ngOnInit(

    ) {

    }

    Shipment: any;
    SetWindowArgs(args: any) {
        this._entityResourceService.getEntityResourceByTableName("WarehouseReleasePackage").subscribe(response => {
            this.Initialize(args);
        });

    }

    Initialize(args) {
        this.ViewModelTrigger = args.ViewModelTrigger;
        this.Shipment = this.ViewModelTrigger ? this.ViewModelTrigger.EntityPM : null;

        this.UIProperties.SetRequired("CustomerId", "WarehouseRelease", true);
        this.UIProperties.SetRequired("Warehouseid", "WarehouseRelease", true);

        if (this.Shipment) {
            this.CustomerId = this.Shipment.CustomerId;
            this.WarehouseId = this.Shipment.WarehouseLegWarehouseId;
        }

        this.SetHeaderLable();

   
    }

    SetHeaderLable() {
        this.VolumeLabel = "Volume (" + SessionLocator.TenantPM.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Gross Weight (" + SessionLocator.TenantPM.GrossWeightUnitCode + ")";
        this.DimensionsUnitCode = SessionLocator.TenantPM.DimensionsUnitCode;;
        this.VolumetricWeightLabel = "Volumetric Weight (" + SessionLocator.TenantPM.ChargeableWeightUnitCode + ")";


    }
    IsShowMessageNoResult: boolean = false;

    CloseButtonClicked() {
 
        this.CurrentSession.CloseCurrentWindow();
    }


    HideWarehouseReleaseGroup(warehouseReleaseGroup: WarehouseReleaseGroup) {
        warehouseReleaseGroup.IsHide = !warehouseReleaseGroup.IsHide;

        if (warehouseReleaseGroup.IsHide) {
            warehouseReleaseGroup.WarehouseReleasePMLists = [];
        } else {

            var warehouseReleasePMLists = this.AllWarehouseReleaseGroupLists.filter(d => d.Title == warehouseReleaseGroup.Title)[0].WarehouseReleasePMLists;
            if (warehouseReleasePMLists) {
                warehouseReleasePMLists.forEach((item) => {
                    warehouseReleaseGroup.WarehouseReleasePMLists.push(item);
                });
            }

        }
    }

    LoadWarehouseReleasePackages() {
        this.WarehouseReleaseGroupLists = [];
        this.AllWarehouseReleaseGroupLists = [];
        
        if (this.CustomerId && this.WarehouseId) {
            this.warehouseReleasePMExtendedService.GetWarehouseReleaseByCstomerIdIdAndwarehouseId(this.customerId, this.WarehouseId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    if (myResponse.Result && myResponse.Result.length > 0) {
                        this.WarehouseReleasePMLists = myResponse.Result;
                        this.AllWarehouseReleaseGroupLists.push(new WarehouseReleaseGroup(this.WarehouseReleasePMLists.filter(d => AppTool.IsNullOrEmpty(d.ShipmentId)), "Not Connected"));
                        this.AllWarehouseReleaseGroupLists.push(new WarehouseReleaseGroup(this.WarehouseReleasePMLists.filter(d => !AppTool.IsNullOrEmpty(d.ShipmentId) && d.ShipmentId == this.Shipment.Id), "Connected to my Shipment"));
                        this.AllWarehouseReleaseGroupLists.push(new WarehouseReleaseGroup(this.WarehouseReleasePMLists.filter(d => !AppTool.IsNullOrEmpty(d.ShipmentId) && d.ShipmentId != this.Shipment.Id), "Connected to other Shipments"));


                        this.WarehouseReleaseGroupLists.push(new WarehouseReleaseGroup(this.WarehouseReleasePMLists.filter(d => AppTool.IsNullOrEmpty(d.ShipmentId)), "Not Connected"));
                        this.WarehouseReleaseGroupLists.push(new WarehouseReleaseGroup(this.WarehouseReleasePMLists.filter(d => !AppTool.IsNullOrEmpty(d.ShipmentId) && d.ShipmentId == this.Shipment.Id), "Connected to my Shipment"));
                        this.WarehouseReleaseGroupLists.push(new WarehouseReleaseGroup(this.WarehouseReleasePMLists.filter(d => !AppTool.IsNullOrEmpty(d.ShipmentId) && d.ShipmentId != this.Shipment.Id), "Connected to other Shipments"));

                    } else this.IsShowMessageNoResult = true;
                }
                this.IsLoadPage = true;

            });
        } else this.IsLoadPage = true;
    }


   



    SaveButtonClicked() {

        //var list = [];
        //this.WarehouseReleasePackagePMLists.filter(d => d.IsSelected == true).forEach((item) => {
        //    list.push(item);
        //});


        //this.ViewModelTrigger.GeneratePackagesFromWarehouseReleasesPackages(list);
        this.CloseButtonClicked();
    }




    private customerId: string;
    get CustomerId() {
        return this.customerId;

    }
    set CustomerId(newValue: string) {
        if (this.customerId != newValue) {
            this.customerId = newValue;
            this.LoadWarehouseReleasePackages();
        }
    }


    private warehouseId: string;
    get WarehouseId() {
        return this.warehouseId;

    }
    set WarehouseId(newValue: string) {
        if (this.warehouseId != newValue) {
            this.warehouseId = newValue;
            this.LoadWarehouseReleasePackages();
        }
    }

    ViewReleaseClicked(item: WarehouseReleasePM) {

        var myBackButtonLabel = "Choose Packages";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: item.Id, ObjectTableName: "WarehouseRelease", BackButtonLabel: myBackButtonLabel });
          
            });
    }

}

class WarehouseReleaseGroup {
    WarehouseReleasePMLists: WarehouseReleasePM[] = [];
    Title: string;
    IsHide: boolean = false;
    constructor(warehouseReleasePMLists: WarehouseReleasePM[] , title:string ) {
        this.WarehouseReleasePMLists = warehouseReleasePMLists;
        this.Title = title;
    }


    private haveWarehouseReleasePackages: boolean;
    get HaveWarehouseReleasePackages() {
        return ((this.WarehouseReleasePMLists && this.WarehouseReleasePMLists.length > 0) || this.IsHide ? true : false);
    }

}

