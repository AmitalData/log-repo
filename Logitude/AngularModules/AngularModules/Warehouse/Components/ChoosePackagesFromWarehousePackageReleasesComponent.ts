declare var System: any;
declare var window: any;
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';

import {WarehouseReleasePackagePM} from '../../Warehouse/EntityPMs/WarehouseReleasePackagePM';

import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {EventTypeArgs} from '../../Infrastructure/DataContracts/EventTypeArgs';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {WarehouseReleasePackagePMExtendedService} from '../../Warehouse/Services/ExtendedPMs/WarehouseReleasePackagePMExtendedService';
@Component({
    moduleId: module.id,
    selector: 'ChoosePackagesFromWarehousePackageReleasesComponent',
    templateUrl: './ChoosePackagesFromWarehousePackageReleasesComponent.html',
})

export class ChoosePackagesFromWarehousePackageReleasesComponent extends BaseComponent implements OnInit {

    private _entityResourceService: EntityResourceService = new EntityResourceService();

    ObjectTableName: string = "WarehouseRelelasePackage";
    //public ValidationErrorsList: string[];
    WarehouseReleasePackagePMLists: WarehouseReleasePackageClass[] = [];
    AllWarehouseReleasePackagesLists: WarehouseReleasePackagePM[] = [];
    WarehouseReleasePackagePMExtendedService: WarehouseReleasePackagePMExtendedService;
    DataContext: any = this;


    IsLoadPage: boolean = false;
    IsContainerShipment: boolean = false;
    VolumeLabel: string;
    GrossWeightLabel: string;
    DimensionsUnitCode: string;

    private CurrentSession = SessionLocator.SelectedSession;
    IsStartFilter: boolean = false;
    ViewModelTrigger: any;
    constructor() {
        super();
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

        this.WarehouseReleasePackagePMLists = [];

        this.IsContainerShipment = args.IsContainer;
        this.ViewModelTrigger = args.ViewModelTrigger;
        this.Shipment = this.ViewModelTrigger ? this.ViewModelTrigger.EntityPM:null;

        if (this.Shipment) {
            this.transportModeId = this.Shipment.TransportModeId ? this.Shipment.TransportModeId : "All";
            this.DirectionId = this.Shipment.DirectionId ? this.Shipment.DirectionId : "All";
            this.CustomerId = this.Shipment.CustomerId;
            this.FromPortId = this.Shipment.MainCarriageFromPortId ? this.Shipment.MainCarriageFromPortId : this.Shipment.FromPortId;
            this.ToPortId = this.Shipment.ShipmentLevelCode == "H" ? this.Shipment.MainCarriageFinalDestinationPortId : this.Shipment.FinalDistenationPortId;
        }

        this.SetHeaderLable();

        if (!this.CustomerId) this.LoadWarehouseReleasePackage();


    }
    VolumetricWeightLabel: string;
    SetHeaderLable() {
        this.VolumeLabel = "Volume (" + SessionLocator.TenantPM.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Gross Weight (" + SessionLocator.TenantPM.GrossWeightUnitCode + ")";
        this.DimensionsUnitCode = SessionLocator.TenantPM.DimensionsUnitCode;;
        this.VolumetricWeightLabel = "Volumetric Weight (" + SessionLocator.TenantPM.ChargeableWeightUnitCode + ")";


    }
    IsShowMessageNoResult: boolean = false;
    FilterWarehouseReleasePackageList() {
        if (this.IsStartFilter) {
            this.WarehouseReleasePackagePMLists = [];
            if (this.IsContainerShipment) {
                this.AllWarehouseReleasePackagesLists.filter(d => d.IsContainer).forEach((item) => {
                    this.WarehouseReleasePackagePMLists.push(new WarehouseReleasePackageClass(item, this.Shipment));
                });
            }
            else {
                this.AllWarehouseReleasePackagesLists.filter(d => !d.IsContainer).forEach((item) => {
                    this.WarehouseReleasePackagePMLists.push(new WarehouseReleasePackageClass(item, this.Shipment));
                });
            }

            if (!AppTool.IsNullOrEmpty(this.TransportModeId) && this.TransportModeId != "All") {
                this.WarehouseReleasePackagePMLists = this.WarehouseReleasePackagePMLists.filter(d => d.TransportModeId == this.TransportModeId);
            }

            if (!AppTool.IsNullOrEmpty(this.DirectionId) && this.DirectionId != "All") {
                this.WarehouseReleasePackagePMLists = this.WarehouseReleasePackagePMLists.filter(d => d.DirectionId == this.DirectionId);
            }

            if (!AppTool.IsNullOrEmpty(this.FromPortId)) {
                this.WarehouseReleasePackagePMLists = this.WarehouseReleasePackagePMLists.filter(d => d.FromPortId == this.FromPortId);
            }

            if (!AppTool.IsNullOrEmpty(this.ToPortId)) {
                this.WarehouseReleasePackagePMLists = this.WarehouseReleasePackagePMLists.filter(d => d.ToPortId == this.ToPortId);
            }



            if (this.WarehouseReleasePackagePMLists.length == 0) {
                this.IsShowMessageNoResult = true;
            } else this.IsShowMessageNoResult = false;

            //if (this.ViewModelTrigger && this.ViewModelTrigger.WarehouseReleasePackagesLists.length > 0) {
            //    this.WarehouseReleasePackagePMLists.forEach((item) => {
            //        var temp = this.ViewModelTrigger.WarehouseReleasePackagesLists.filter(d => d.Id == item.Id)[0];
            //        if (temp) item.IsSelected = true;
            //    });
            //}

        }

    }

    CloseButtonClicked() {
 
        //this.ViewModelTrigger.CustomerId = this.warehouseReleasePM.CustomerId = this.OldCustomerId;


        this.CurrentSession.CloseCurrentWindow();
    }

    IsDisableFilter: boolean = false;
    SetEnableProp() {
        if (this.UIProperties && this.IsLoadPage) {
            if (this.WarehouseReleasePackagePMLists.filter(d => d.IsSelected)[0]) {
                this.UIProperties.SetEnabled("TransportModeId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("DirectionId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, false);
                this.IsDisableFilter = true;
            }
            else {
                this.UIProperties.SetEnabled("TransportModeId", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("DirectionId", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, true);
                this.IsDisableFilter = false;
            }
        }
    }

    LoadWarehouseReleasePackage() {

        var warehouseReleasePackagePMExtendedService: WarehouseReleasePackagePMExtendedService = new WarehouseReleasePackagePMExtendedService();
        warehouseReleasePackagePMExtendedService.GetWarehouseReleasePackagePMThatNotUsedForAnyEntityLists().subscribe((myResponse: ServiceResponse) => {
            this.IsStartFilter = true;
            if (!myResponse.HasError) {
                var releasePackages: any = myResponse.Result;
                this.AllWarehouseReleasePackagesLists = releasePackages;
                this.FilterWarehouseReleasePackageList();

            }

            this.IsLoadPage = true;

        }); 
    }
    SaveButtonClicked() {

        var list = [];
        this.WarehouseReleasePackagePMLists.filter(d => d.IsSelected == true).forEach((item) => {
            list.push(item);
        });


        this.ViewModelTrigger.GeneratePackagesFromWarehouseReleasesPackages(list);
        this.CloseButtonClicked();
    }



    private transportModeId: string = "All";
    get TransportModeId() {
        this.SetEnableProp();
        return this.transportModeId;


    }
    set TransportModeId(newValue: string) {

        if (this.transportModeId != newValue) {
            this.transportModeId = newValue;
            this.FilterWarehouseReleasePackageList();
        }

    }

    private directionId: string = "All";
    get DirectionId() { return this.directionId; }
    set DirectionId(newValue: string) {

        if (this.directionId != newValue) {
            this.directionId = newValue;
            this.FilterWarehouseReleasePackageList();
        }

    }



    private customerId: string;
    get CustomerId() {



        return this.customerId;

    }
    set CustomerId(newValue: string) {
        if (this.customerId != newValue) {
            this.customerId = newValue;
            this.LoadWarehouseReleasePackage();
        }
    }

    private fromPortId: string;
    get FromPortId() { return this.fromPortId; }
    set FromPortId(newValue: string) {

        if (this.fromPortId != newValue) {
            this.fromPortId = newValue;
            this.FilterWarehouseReleasePackageList();

        }
    }

    private toPortId: string;
    get ToPortId() { return this.toPortId; }
    set ToPortId(newValue: string) {

        if (this.toPortId != newValue) {
            this.toPortId = newValue;
            this.FilterWarehouseReleasePackageList();

        }
    }


    ViewReleaseClicked(item: WarehouseReleasePackageClass) {

        var myBackButtonLabel = "Choose Packages";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: item.Id, ObjectTableName: "WarehouseRelease", BackButtonLabel: myBackButtonLabel });
          
            });
    }

}

export class WarehouseReleasePackageClass extends BaseComponent {

    Id: string;
    IsContainer: boolean;
    PackageTypeName: string;
    ContainerNumberWarning: string;
    ContainerNumber: string;
    Dimensions: string;
    Quantity: number;
    Volume: number;
    Weight: number;
    Description: string;

    IsConnectedToShipment: boolean = false;
    VolumetricWeight: number;
    TransportModeId: string;
    DirectionId: string;
    FromPortId: string;
    ToPortId: string;
    CustomerId: string;
    Height: number;
    Width: number;
    Length: number;
     IsSelected: boolean;
     ReleaseNumber: string;
     ReleaseStatus: string;
    constructor(entityPM: WarehouseReleasePackagePM , shipment:any) {
        super();
        this.Id = entityPM.Id;
        this.PackageTypeName = entityPM.PackageTypeName;
        this.ContainerNumberWarning = entityPM.ContainerNumberWarning;
        this.ContainerNumber = entityPM.ContainerNumber;
        this.Dimensions = entityPM.Dimensions;
        this.Quantity = entityPM.Quantity;
        this.Volume = entityPM.Volume;
        this.Weight = entityPM.Weight;
        this.VolumetricWeight = entityPM.VolumetricWeight;
        this.Height = entityPM.Height;
        this.Width = entityPM.Width;
        this.Length = entityPM.Length;
        this.ReleaseNumber = entityPM.ReleaseNumber;
        this.ReleaseStatus = entityPM.ReleaseStatus;
        this.Description = entityPM.Description;
        this.IsContainer = entityPM.IsContainer;
        this.EntityPM = entityPM
        this.IsConnectedToShipment = entityPM.ShipmentId ? true : false;
        if (shipment != null) {
            this.TransportModeId = shipment.TransportModeId;
            this.DirectionId = shipment.DirectionId;
            this.FromPortId = shipment.FromPortId;
            this.ToPortId = shipment.ToPortId;
            this.CustomerId = shipment.CustomerId;
        }


     }


    

}
