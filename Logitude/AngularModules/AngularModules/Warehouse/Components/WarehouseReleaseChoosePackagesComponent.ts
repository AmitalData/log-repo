declare var System: any;
declare var window: any;
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import {WarehouseEntryPackagePM} from '../../Warehouse/EntityPMs/WarehouseEntryPackagePM';
import {WarehouseReleasePackagePM} from '../../Warehouse/EntityPMs/WarehouseReleasePackagePM';
import {WarehouseReleasePackagesDetailsComponent} from '../../Warehouse/Components/WarehouseReleasePackagesDetailsComponent';
import {WarehouseReleasePM} from '../../Warehouse/EntityPMs/WarehouseReleasePM';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {EventTypeArgs} from '../../Infrastructure/DataContracts/EventTypeArgs';
import {WarehouseEntryPackagePMExtendedService} from '../../Warehouse/Services/ExtendedPMs/WarehouseEntryPackagePMExtendedService';

@Component({
    
    selector: 'WarehouseReleaseChoosePackagesComponent',
    templateUrl: './WarehouseReleaseChoosePackagesComponent.html',
    providers: [WarehouseEntryPackagePMExtendedService],
})

export class WarehouseReleaseChoosePackagesComponent extends BaseComponent implements OnInit {

    private _entityResourceService: EntityResourceService = new EntityResourceService();

    ObjectTableName: string = "WarehouseEntry";
    public ValidationErrorsList: string[];
    WarehouseEntryPackagesLists: WarehouseEntryPackageClass[] = [];
    AllWarehouseEntryPackagesLists: any[] = [];
    warehouseReleasePM: WarehouseReleasePM;
    SelectedWarehouseEntryPackage: WarehouseEntryPackagePM;
    ViewModelTrigger: WarehouseReleasePackagesDetailsComponent;
    DataContext: any = this;
    IsNewEntity: boolean = false;
    ObjectTableId: string;
    Type: string;
    IsLoadPage: boolean = false;
    ShowTextBoxReleaseQTY: boolean = false;
    IsContainerShipment: boolean = false;
    VolumeLabel: string;
    GrossWeightLabel: string;
    DimensionsLabel: string;
    PackageType: string;
    IsFromFullWarehouseReleaseComponent: boolean = false;
    IsBondedWarehouse : boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    OldCustomerId: string;
    constructor(private warehouseEntryPackagePMExtendedService: WarehouseEntryPackagePMExtendedService) {
        super();
    }

    ngOnInit(

    ) {


    }

    SetWindowArgs(args: any) {
        this._entityResourceService.getEntityResourceByTableName("WarehouseEntryPackage").subscribe((response:any) => {

            this.Start(args);
        });

    }

    WarehouseEntryId: string;

    IsStartFilter: boolean = false;
    Start(args) {

        this.WarehouseEntryPackagesLists = [];
        this.IsFromFullWarehouseReleaseComponent = args.IsFromFullWarehouseReleaseComponent;
        this.warehouseReleasePM = args.WarehouseReleasePM;
        this.ViewModelTrigger = args.ViewModelTrigger;
        this.WarehouseEntryId = args.WarehouseEntryId;
        this.IsBondedWarehouse = args.IsBondedWarehouse;

        if (this.warehouseReleasePM) {
            this.OldCustomerId = this.warehouseReleasePM.CustomerId;
        }

        //this.transportModeId = this.ViewModelTrigger.TransportModeId ? this.ViewModelTrigger.TransportModeId : "All";
        //this.DirectionId = this.ViewModelTrigger.DirectionId ? this.ViewModelTrigger.DirectionId : "All";



        this.CustomerId = this.ViewModelTrigger.CustomerId;
        //this.FromPortId = this.ViewModelTrigger.FromPortId;
        //this.ToPortId = this.ViewModelTrigger.ToPortId;
  
        this.PackageType = args.PackageType;

        this.AllWarehouseEntryPackagesLists = args.WarehouseEntryPackagesLists;

        this.IsStartFilter = true;
        this.FilterWarehouseEntryPackageList();

  
        this.SetValue();
        this.IsLoadPage = true;

    }
    VolumetricWeightLabel: string;
    SetValue() {
        this.VolumeLabel = this.ViewModelTrigger.VolumeLabel;
        this.GrossWeightLabel = this.ViewModelTrigger.GrossWeightLabel;
        this.DimensionsLabel = this.ViewModelTrigger.DimensionsLabel;
        this.VolumetricWeightLabel = this.ViewModelTrigger.VolumetricWeightLabel;
        
    }
    IsShowMessageNoResult: boolean = false;
    FilterWarehouseEntryPackageList() {
        if (this.IsStartFilter) {
            this.WarehouseEntryPackagesLists = [];
            if (this.PackageType == "Container") {
                this.IsContainerShipment = true;
                this.AllWarehouseEntryPackagesLists.filter(d => d.IsContainer).forEach((item) => {
                    this.WarehouseEntryPackagesLists.push(new WarehouseEntryPackageClass(item));
                });
            }
            else {
                this.AllWarehouseEntryPackagesLists.filter(d => !d.IsContainer).forEach((item) => {
                    this.WarehouseEntryPackagesLists.push(new WarehouseEntryPackageClass(item));
                });
            }

            if (!AppTool.IsNullOrEmpty(this.TransportModeId) && this.TransportModeId != "All") {
                this.WarehouseEntryPackagesLists = this.WarehouseEntryPackagesLists.filter(d => d.TransportModeId == this.TransportModeId);
            }

            if (!AppTool.IsNullOrEmpty(this.DirectionId) && this.DirectionId != "All") {
                this.WarehouseEntryPackagesLists = this.WarehouseEntryPackagesLists.filter(d => d.DirectionId == this.DirectionId);
            }

            if (!AppTool.IsNullOrEmpty(this.FromPortId)) {
                this.WarehouseEntryPackagesLists = this.WarehouseEntryPackagesLists.filter(d => d.FromPortId == this.FromPortId);
            }

            if (!AppTool.IsNullOrEmpty(this.ToPortId)) {
                this.WarehouseEntryPackagesLists = this.WarehouseEntryPackagesLists.filter(d => d.ToPortId == this.ToPortId);
            }



            if (this.WarehouseEntryPackagesLists.length == 0) {
                this.IsShowMessageNoResult = true;
            } else this.IsShowMessageNoResult = false;

            if (this.ViewModelTrigger && this.ViewModelTrigger.WarehouseReleasePackagesLists.length > 0) {
                this.WarehouseEntryPackagesLists.forEach((item) => {
                    var temp = this.ViewModelTrigger.WarehouseReleasePackagesLists.filter(d => d.WarehouseReleaseId == item.EntityPM.Id)[0];
                    if (temp) item.IsSelected = true;
                });
            }

        }

    }

    CloseButtonClicked() {
        this.WarehouseEntryPackagesLists.forEach((item) => {
            item.EntityPM.ReleaseQTY = item.OldReleaseQTY;
            item.EntityPM.IsSelected = item.OldIsSelected;

        });


        this.ViewModelTrigger.CustomerId = this.warehouseReleasePM.CustomerId = this.OldCustomerId;


        this.CurrentSession.CloseCurrentWindow();
    }

    IsDisableFilter: boolean = false;
    SetEnableProp() {
        if (this.UIProperties && this.IsLoadPage) {
            if (this.WarehouseEntryPackagesLists.filter(d => d.IsSelected)[0] || this.WarehouseEntryId) {
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





    SaveButtonClicked() {

        this.ValidationErrorsList = [];
        var itemvalid = this.WarehouseEntryPackagesLists.filter(d => d.ReleaseQTY > d.Instock)[0];

        if (itemvalid) {
            this.ValidationErrorsList.push("The release quantity must be less than or equal to in stock quantity");
        }




        if (this.ValidationErrorsList.length == 0) {

            if (this.WarehouseEntryPackagesLists.length == 0) {
                this.ViewModelTrigger.WarehouseReleasePackagesLists = [];
            }


            this.WarehouseEntryPackagesLists.forEach((item) => {

                if (item.IsSelected) {


                    var newWarehouseReleasePackagePM: WarehouseReleasePackagePM = this.ViewModelTrigger.WarehouseReleasePackagesLists.filter(d => d.EntryPackageId == item.EntityPM.Id)[0];
                    if (!newWarehouseReleasePackagePM) var newWarehouseReleasePackagePM: WarehouseReleasePackagePM = new WarehouseReleasePackagePM(null);

                    newWarehouseReleasePackagePM.Tenant = SessionLocator.TenantPM.Id;
                    newWarehouseReleasePackagePM.CreatedByUserId = SessionLocator.LoggedUserId;
                    newWarehouseReleasePackagePM.UpdatedByUserId = SessionLocator.LoggedUserId;
                    newWarehouseReleasePackagePM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
                    newWarehouseReleasePackagePM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
                    newWarehouseReleasePackagePM.WarehouseReleaseId = this.warehouseReleasePM.Id;
                    newWarehouseReleasePackagePM.Id = "1-1";
                    newWarehouseReleasePackagePM.Quantity = item.EntityPM.ReleaseQTY;
                    newWarehouseReleasePackagePM.ContainerNumber = item.ContainerNumber;
                    newWarehouseReleasePackagePM.Width = item.EntityPM.Width;
                    newWarehouseReleasePackagePM.Dimensions = (item.EntityPM.Length ? item.EntityPM.Length : "") + "-" + (item.EntityPM.Width ? item.EntityPM.Width : "") + "-" + (item.EntityPM.Height ? item.EntityPM.Height : "");

                    newWarehouseReleasePackagePM.Volume = item.EntityPM.Volume;
                    newWarehouseReleasePackagePM.Weight = item.EntityPM.Weight;
                    newWarehouseReleasePackagePM.Harmonize = item.EntityPM.Harmonize;
                    newWarehouseReleasePackagePM.Length = item.EntityPM.Length;
                    newWarehouseReleasePackagePM.Height = item.EntityPM.Height;
                    newWarehouseReleasePackagePM.IsContainer = item.EntityPM.IsContainer;
                    newWarehouseReleasePackagePM.PackageTypeName = item.PackageTypeName;
                    newWarehouseReleasePackagePM.Seal = item.EntityPM.Seal;
                    newWarehouseReleasePackagePM.Tenant = item.EntityPM.Tenant;
                    newWarehouseReleasePackagePM.PackageTypeId = item.EntityPM.PackageTypeId;
                    newWarehouseReleasePackagePM.EntryPackageId = item.EntityPM.Id;
                    newWarehouseReleasePackagePM.Description = item.EntityPM.Description;
                    newWarehouseReleasePackagePM.ContainerNumberWarning = item.EntityPM.ContainerNumberWarning;
                    newWarehouseReleasePackagePM.ActualReleaseDate = item.EntityPM.ActualEntryDate;
                    newWarehouseReleasePackagePM.VolumetricWeight = item.EntityPM.VolumetricWeight;
                    var existItem: WarehouseReleasePackagePM = this.ViewModelTrigger.WarehouseReleasePackagesLists.filter(d => d.EntryPackageId == newWarehouseReleasePackagePM.EntryPackageId)[0];
                    if (!existItem) {
                        this.ViewModelTrigger.WarehouseReleasePackagesLists.push(newWarehouseReleasePackagePM);
                    }


                }
                else {

                    var existItem: WarehouseReleasePackagePM = this.ViewModelTrigger.WarehouseReleasePackagesLists.filter(d => d.EntryPackageId == item.EntityPM.Id)[0];
                    if (existItem) {
                        this.ViewModelTrigger.WarehouseReleasePackagesLists = this.ViewModelTrigger.WarehouseReleasePackagesLists.filter(d => d.EntryPackageId != item.EntityPM.Id);
                    }


                    item.EntityPM.ReleaseQTY = 0;
                }

            });

            this.CurrentSession.CurrentWindow.Close("Refresh");
        }






    }



    private transportModeId: string = "All";
    get TransportModeId() {
        this.SetEnableProp();
        return this.transportModeId;


    }
    set TransportModeId(newValue: string) {

        if (this.transportModeId != newValue) {
            this.transportModeId = newValue;
            this.FilterWarehouseEntryPackageList();
            this.ViewModelTrigger.TransportModeId = newValue;
        }

    }

    private directionId: string = "All";
    get DirectionId() { return this.directionId; }
    set DirectionId(newValue: string) {

        if (this.directionId != newValue) {
            this.directionId = newValue;
            this.ViewModelTrigger.DirectionId = newValue;
            this.FilterWarehouseEntryPackageList();
        }

    }



    private customerId: string;
    get CustomerId() {



        return this.customerId;

    }
    set CustomerId(newValue: string) {
        if (this.customerId != newValue) {
            this.customerId = newValue;

            if (this.warehouseReleasePM.CustomerId != newValue) {
                this.ViewModelTrigger.CustomerId = newValue;
                this.warehouseReleasePM.CustomerId = newValue;
                this.LoadWarehouseEntryPackageListsByCustomerId();
            }
        }
    }

    private fromPortId: string;
    get FromPortId() { return this.fromPortId; }
    set FromPortId(newValue: string) {

        if (this.fromPortId != newValue) {
            this.fromPortId = newValue;
            this.ViewModelTrigger.FromPortId = newValue;
            this.FilterWarehouseEntryPackageList();

        }
    }

    private toPortId: string;
    get ToPortId() { return this.toPortId; }
    set ToPortId(newValue: string) {

        if (this.toPortId != newValue) {
            this.toPortId = newValue;
            this.ViewModelTrigger.ToPortId = newValue;
            this.FilterWarehouseEntryPackageList();

        }
    }

    EditWarehouseReleases(EntryId: any) {

        var myBackButtonLabel = "Choose Cross Dock Package";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: EntryId, ObjectTableName: "WarehouseEntry", BackButtonLabel: myBackButtonLabel });
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    this.LoadWarehouseEntryPackageListsByCustomerId();
                });
            });
    }

    LoadWarehouseEntryPackageListsByCustomerId() {
        var shipmentId = this.warehouseReleasePM.ShipmentId ? this.warehouseReleasePM.ShipmentId : "";
        this.warehouseEntryPackagePMExtendedService.GetWarehouseEntryPackagePMListsByShipmentIdAndWarehouseIdAndCustomerId(shipmentId, this.CustomerId, this.warehouseReleasePM.WarehouseId, this.warehouseReleasePM.Tenant).subscribe((res: any) => {
            var pmResponse: any = res;
            if (!pmResponse.HasError) {
                this.AllWarehouseEntryPackagesLists = pmResponse.Result;
                if (this.AllWarehouseEntryPackagesLists && this.WarehouseEntryId) {
                    this.AllWarehouseEntryPackagesLists = this.AllWarehouseEntryPackagesLists.filter(d => d.WarehouseEntryId == this.WarehouseEntryId);
                }

                this.ViewModelTrigger.AllWarehouseEntryPackagesLists = pmResponse.Result;
                this.FilterWarehouseEntryPackageList();
            }

        });
    }

}


export class WarehouseEntryPackageClass extends BaseComponent {

    Instock: number;

    EntryPackageId: string;
    IsContainer: boolean;
    PackageTypeName: string;
    ContainerNumberWarning: string;
    ContainerNumber: string;
    Dimensions: string;
    Quantity: number;
    Volume: number;
    Weight: number;
    Description: string;
    EntityPM: WarehouseEntryPackagePM;
    IsFullReleaseQTYAuto: boolean;
    IsConnectedToShipment: boolean = false;
    OldReleaseQTY: number;
    OldIsSelected: boolean;
    VolumetricWeight: number;
    TransportModeId: string;
    DirectionId: string;
    FromPortId: string;
    ToPortId: string;
    CustomerId: string;
    WarehouseEntryNumber: string;

    IsSelectedKeyId: string = Guid.newGuid();
    get ReleaseQTY() {
        var releaseQTY = 0;
        if (this.EntityPM) {
            releaseQTY = this.EntityPM.ReleaseQTY;
        }
        return releaseQTY;
    }


    set ReleaseQTY(newValue: number) {
        if (this.ReleaseQTY != newValue) {

            if (newValue) {
                if (newValue > 0) {
                    this.EntityPM.ReleaseQTY = newValue;
                    this.ReleaseQTY = newValue;
                    if (!this.IsSelected) {
                        this.IsFullReleaseQTYAuto = true;
                        this.IsSelected = true;
                    }

                }

            }
            else {
                this.EntityPM.ReleaseQTY = newValue;
                this.ReleaseQTY = newValue;
                this.IsSelected = false;
                this.IsFullReleaseQTYAuto = true;
            }
        }
    }




    get IsSelected() {
        var iselected = false;
        if (this.EntityPM) {
            iselected = this.EntityPM.IsSelected;;
        }
        return iselected;
    }
    set IsSelected(newValue: boolean) {
        if (this.IsSelected != newValue) {
            this.EntityPM.IsSelected = newValue;
            this.IsSelected = newValue;

            if (newValue) {
                if (!this.IsFullReleaseQTYAuto) this.ReleaseQTY = this.Instock;
                else this.IsFullReleaseQTYAuto = false;
            } else {
                if (!this.IsFullReleaseQTYAuto) this.ReleaseQTY = 0;
                else this.IsFullReleaseQTYAuto = false;

            }


        }
    }


    ReleaseQTYLostFocusMethod(value) {
        this.ReleaseQTY = value;

    }

    Height: number;
    Width: number;
    Length: number;

    constructor(entityPM: WarehouseEntryPackagePM) {
        super();

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
        this.WarehouseEntryNumber = entityPM.WarehouseEntryNumber;

        this.Description = entityPM.Description;
        this.Instock = entityPM.Instock;
        this.IsContainer = entityPM.IsContainer;
        this.EntryPackageId = entityPM.Id;
        this.EntityPM = entityPM;
        this.OldReleaseQTY = entityPM.ReleaseQTY;
        this.OldIsSelected = entityPM.IsSelected;


        this.TransportModeId = entityPM.TransportModeId;
        this.DirectionId = entityPM.DirectionId;
        this.FromPortId = entityPM.FromPortId;
        this.ToPortId = entityPM.ToPortId;
        this.CustomerId = entityPM.CustomerId;
        this.IsConnectedToShipment = entityPM.IsConnectedToShipment;
    }

}
