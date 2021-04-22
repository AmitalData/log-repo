declare var System: any;
declare var window: any;
import {ConfirmWindow} from '../../Controls/Windows/ConfirmWindow';
import {Component, OnInit, ViewChildren, QueryList}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import {ShipmentPM} from '../../Shipment/EntityPMs/ShipmentPM';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {WarehouseEntryPackagePMExtendedService} from '../../Warehouse/Services/ExtendedPMs/WarehouseEntryPackagePMExtendedService';
import {WarehouseReleasePM} from '../../Warehouse/EntityPMs/WarehouseReleasePM';
import {WarehouseReleasePMExtendedService} from '../../Warehouse/Services/ExtendedPMs/WarehouseReleasePMExtendedService';

import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {WarehouseReleasePackagePM} from '../../Warehouse/EntityPMs/WarehouseReleasePackagePM';
import {WarehouseEntryPackagePM} from '../../Warehouse/EntityPMs/WarehouseEntryPackagePM';

import {AppTool, DateTool, FormatTool, DateParts} from '../../Infrastructure/Tools';

import {EventTypeArgs} from '../../Infrastructure/DataContracts/EventTypeArgs';
import {EventTypeClass} from '../../Infrastructure/DataContracts/EventTypeArgs';

import {ClassLevelValidator} from '../../Infrastructure/Validators/ClassLevelValidator';
import {CommonDomainService} from '../../Common/Services/CommonDomainService';

import {DateAgeHelper} from '../../Infrastructure/Utilities/DateAgeHelper';

import {WarehouseHelper} from '../Helpers/WarehouseHelper';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';


import {LocationDirective} from '../../Infrastructure/Utilities/LocationDirective';



@Component({
    
    selector: 'NewWarehouseReleaseComponent',
    templateUrl: './NewWarehouseReleaseComponent.html',
    providers: [WarehouseReleasePMExtendedService, WarehouseEntryPackagePMExtendedService],

})
export class NewWarehouseReleaseComponent extends BaseComponent implements OnInit {
  public ExpectedReleaseDate: any;

    public WarehouseReleasePackagesLists: WarehouseReleasePackagePM[] = [];
    ObjectTableName: string = "WarehouseRelease";
    public ValidationErrorsList: string[];
    ShipmentPM: any;
    warehouseReleasePM: WarehouseReleasePM = new WarehouseReleasePM();
    public VolumeColumnHeader: string;
    public WeightColumnHeader: string;
    public DimensionsColumnHeader: string;
    public VolumetricWeightColumnHeader: string;
    public PackageTypeColumnHeader: string;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    validator: ClassLevelValidator;
    ObjectTableId: string;
    IsFromShipment: boolean = true;
    ActualReleaseDateOldValue: Date;
    ExpectedReleaseDateOldValue: Date;
    SelectedWarehouseReleasePackage: WarehouseReleasePackagePM;
    AllWarehouseEntryPackagesLists: WarehouseEntryPackagePM[] = [];
    CustomWarehouseEntryPackagesLists: WarehouseEntryPackagePM[] = [];
    IsChangeWarehouseIdOrCustomerId: boolean = false;
    VolumeLabel: string;
    GrossWeightLabel: string;
    DimensionsLabel: string;
    ChargeableWeightLabel: string;
    DataContext: any = this;
    IsFilterByShipmentId: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    public FromPortId: string;
    public ToPortId: string;
    constructor(public _warehouseReleasePMExtendedService: WarehouseReleasePMExtendedService, private warehouseEntryPackagePMExtendedService: WarehouseEntryPackagePMExtendedService) {
        super();
        this.GetNewInstance();
        this.validator = new ClassLevelValidator();

        var table = window.ObjectTables.filter(d => d.Name == "WarehouseRelease")[0];
        if (table) {
            this.ObjectTableId = table.Id;
        }

    }

    ngOnInit() {


    }





    GetNewInstance() {

        this.warehouseReleasePM.Tenant = SessionLocator.TenantPM.Id;
        this.warehouseReleasePM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.warehouseReleasePM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.warehouseReleasePM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.warehouseReleasePM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.warehouseReleasePM.StatusCode = "CREA";
        this.warehouseReleasePM.Id = "1-1";
        this.warehouseReleasePM.GrossWeightUnitCode = SessionLocator.TenantPM.GrossWeightUnitCode;
        this.warehouseReleasePM.VolumeUnitCode = SessionLocator.TenantPM.VolumeUnitCode;
        this.warehouseReleasePM.DimensionsUnitCode = SessionLocator.TenantPM.DimensionsUnitCode;

        this.warehouseReleasePM.ChargeableWeightUnitCode = SessionLocator.TenantPM.ChargeableWeightUnitCode;
        this.warehouseReleasePM.TotalVolume = 0;
        this.warehouseReleasePM.TotalGrossWeight = 0;
        this.warehouseReleasePM.TotalPieces = 0;
        this.warehouseReleasePM.ReleaseNumber = "123";
    }



    IsLoadPage: boolean = false;
    SetWindowArgs(args: any) {
        this._entityResourceService.getEntityResourceByTableName("WarehouseRelease").subscribe((response:any) => {
            this.Start(args);

        });

    }
    IsLCLEntity: boolean = true;

    WarehouseEntryId: string = "";
    Start(args: any) {

        this.ShipmentPM = args.ShipmentPM;
        this.SetValue(args);
        this.RunComponent();
        this.IsLoadPage = true;



    }
    IsLoadWarehouse: boolean = false;
    FromType: string;;
    SetValue(args: any) {
        this.FromType = args.FromType;

        if (this.warehouseReleasePM) {
            if (this.ShipmentPM) {
                this.UIProperties.SetEnabled("ShipmentId", "WarehouseRelease", false);
                this.IsFilterByShipmentId = false;
                if (this.ShipmentPM.ShipmentLevelCode == "D") this.warehouseReleasePM.CustomerId = this.ShipmentPM.CustomerId;
                this.warehouseReleasePM.ShipmentId = this.ShipmentPM.Id;
                this.warehouseReleasePM.ShipmentNumber = this.ShipmentPM.ShipmentNumber;
                this.warehouseReleasePM.HouseNumber = this.ShipmentPM.House;
                this.warehouseReleasePM.MasterNumber = this.ShipmentPM.LongMaster;
                this.warehouseReleasePM.ShipmentLevelCode = this.ShipmentPM.ShipmentLevelCode;
                this.warehouseReleasePM.TransportModeId = this.ShipmentPM.TransportModeId;
                this.warehouseReleasePM.ShipmentTypeId = this.ShipmentPM.ShipmentTypeId;
                this.warehouseReleasePM.DirectionId = this.ShipmentPM.DirectionId;
                this.warehouseReleasePM.ConnectedTo = args.ConnectedTo;
                this.warehouseReleasePM.ChildEntityReference = args.ChildEntityReference;
                this.MapConnectedShipmentFields(args.ConnectedTo);
            } else {
                this.warehouseReleasePM.ShipmentId = args.ShipmentId;
                this.warehouseReleasePM.ConnectedTo = args.ConnectedTo;
                this.warehouseReleasePM.ChildEntityReference = args.ChildEntityReference;
            }


            this.SetDefultCustomerValue(args);
            this.SetDefultWarehouseValue(args);


            this.IsLCLEntity = AppTool.IsLCLEntity(this.warehouseReleasePM.TransportModeId, this.warehouseReleasePM.ShipmentTypeId);
            if (this.warehouseReleasePM.ShipmentId) this.UIProperties.SetEnabled("ShipmentId", "WarehouseRelease", false);
            this.WarehouseEntryId = args.WarehouseEntryId;
            this.FromPortId = args.FromPortId;
            this.ToPortId = args.ToPortId;
            this.ConnectedTo = this.warehouseReleasePM.ConnectedTo;
            if (this.FromType == "WarehouseEntry") {
                this.warehouseReleasePM.UIProperties.SetEnabled("CustomerId", "WarehouseRelease", false);
                this.warehouseReleasePM.UIProperties.SetEnabled("WarehouseId", "WarehouseRelease", false);
            }
            this.warehouseReleasePM.ExpectedReleaseDate = args.ExpectedReleaseDate;
            this.warehouseReleasePM.ActualReleaseDate = args.ActualReleaseDate;
            this.ActualReleaseDateOldValue = this.warehouseReleasePM.ActualReleaseDate;
            this.ExpectedReleaseDateOldValue = this.warehouseReleasePM.ExpectedReleaseDate;
  

        }
    }

    MapConnectedShipmentFields(connectedTo: string) {
        if (connectedTo == "Shipment") {
            this.MapMasterShipmentNumber();
        }
    }
   
    MapMasterShipmentNumber() {
        if (this.ShipmentPM.ShipmentLevelCode == "H") {
            this.warehouseReleasePM.MasterShipmentNumber = this.ShipmentPM.MasterShipmentNumber;
        }
        else {
            this.warehouseReleasePM.MasterShipmentNumber = this.ShipmentPM.ShipmentNumber;
        }
    }

    SetDefultWarehouseValue(args: any) {
        if (!AppTool.IsNullOrEmpty(args.WarehouseId)) this.warehouseReleasePM.WarehouseId = args.WarehouseId;
        else if (this.ShipmentPM && this.ShipmentPM.WarehouseLegWarehouseId) this.warehouseReleasePM.WarehouseId = this.ShipmentPM.WarehouseLegWarehouseId;
        else {
            var myCommonDomain = new CommonDomainService();
            myCommonDomain.GetDeafaultMyWarehouse().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.warehouseReleasePM.WarehouseId = myResponse.Result;
                }
            });
        }

    }


    SetDefultCustomerValue(args: any) {
        if (!AppTool.IsNullOrEmpty(args.CustomerId)) this.warehouseReleasePM.CustomerId = args.CustomerId;
        else if (this.ShipmentPM && this.ShipmentPM.CustomerId) this.warehouseReleasePM.CustomerId = this.ShipmentPM.CustomerId;
    }


    CustomerValueChange(item) {
        if (item) {
            this.RefreshWarehouseEntryPackagesLists();
        }
    }
    WarehouseValueChange(item) {

        if (item) {
            this.RefreshWarehouseEntryPackagesLists();
        }
    }



    RefreshWarehouseEntryPackagesLists() {
        this.IsChangeWarehouseIdOrCustomerId = true;
        this.WarehouseReleasePackagesLists = [];
    }



    get ShipmentId() { return this.warehouseReleasePM.ShipmentId; }
    set ShipmentId(newValue: string) {
        if (this.warehouseReleasePM.ShipmentId != newValue) {
            this.warehouseReleasePM.ShipmentId = newValue;
            this.RefreshWarehouseEntryPackagesLists();

        }
    }

    ShipmentValueChange(shipment: any) {
        this.warehouseReleasePM.ShipmentNumber = null;
        if (shipment) {
            if (this.ShipmentPM) {
                if (this.ShipmentPM.ShipmentNumber != shipment.ShipmentNumber) {
                    this.ShipmentPM = shipment;
                    this.warehouseReleasePackagesDetailsComponent.SetPortData();
                }

            } else {

                this.ShipmentPM = shipment;
                this.warehouseReleasePackagesDetailsComponent.SetPortData();
            }
        }

        if (this.ShipmentPM) {
            this.warehouseReleasePM.ShipmentNumber = shipment.ShipmentNumber;
            this.MapConnectedShipmentFields("Shipment");
        }



    }



    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }


    SaveButtonClicked() {

        this.ValidationErrorsList = [];

        var warehouseHelper: WarehouseHelper = new WarehouseHelper();
        warehouseHelper.CreateWarehouseRelease(this.warehouseReleasePM, this);



    }




    ComputeAndFullTotalPackage() {
        var totalPieces: number = 0;
        var totalVolume: number = 0;
        var totalGrossWeight: number = 0;

        if (this.WarehouseReleasePackagesLists && this.WarehouseReleasePackagesLists.length > 0) {
            this.WarehouseReleasePackagesLists.forEach((item) => {
                if (item.Quantity) totalPieces += item.Quantity;
                if (item.Volume) totalVolume += item.Volume;
                if (item.Weight) totalGrossWeight += item.Weight;

            });
        }

        this.warehouseReleasePM.TotalPieces = totalPieces;
        this.warehouseReleasePM.TotalVolume = totalVolume;
        this.warehouseReleasePM.TotalGrossWeight = totalGrossWeight;
        this.warehouseReleasePM.GrossWeightUnitCode = "KG";
        this.warehouseReleasePM.VolumeUnitCode = "CBM";
    }





    IsPackageOpen: boolean = false;
    WarehouseId: string = "";
    IsRefreshCustomer: boolean = false;
  


    //FromPortId: string;
    //ToPortId: string;
    ConnectedTo: string;


    OnActualReleaseDateDatePickerChange(value) {

        this.warehouseReleasePM.UIProperties.SetValidity("ActualReleaseDate", "WarehouseRelease", true, null);

        if (!DateTool.IsActualDateValid(value)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", "Actual Release Date");
            this.warehouseReleasePM.UIProperties.SetValidity("ActualReleaseDate", "WarehouseRelease", false, errorMessage);
        }
    }

    //get FromPortId() {
    //    var fromportid: string = null;
    //    if (this.warehouseReleasePM) fromportid = this.warehouseReleasePM.FromPortId;
    //    return fromportid;
    //}
    //set FromPortId(value: string) {
    //    if (this.warehouseReleasePM != null) {
    //        if (value != this.warehouseReleasePM.FromPortId) {
    //            this.warehouseReleasePM.FromPortId = value;
    //            // this.OnActualReleaseDateDatePickerChange(value);
    //        }
    //    }
    //}


    //get ToPortId() {
    //    var toportid: string = null;
    //    if (this.warehouseReleasePM) toportid = this.warehouseReleasePM.ToPortId;
    //    return toportid;
    //}
    //set ToPortId(value: string) {
    //    if (this.warehouseReleasePM != null) {
    //        if (value != this.warehouseReleasePM.ToPortId) {
    //            this.warehouseReleasePM.ToPortId = value;
    //            // this.OnActualReleaseDateDatePickerChange(value);
    //        }
    //    }
    //}

    get ActualReleaseDate() {
        var actualReleaseDate: Date = null;
        if (this.warehouseReleasePM) actualReleaseDate = this.warehouseReleasePM.ActualReleaseDate;
        return actualReleaseDate;
    }
    set ActualReleaseDate(value: Date) {
        if (this.warehouseReleasePM != null) {
            if (value != this.warehouseReleasePM.ActualReleaseDate) {
                this.warehouseReleasePM.ActualReleaseDate = value;
                this.OnActualReleaseDateDatePickerChange(value);
            }
        }
    }


    SetActualDateClicked() {
        this.ActualReleaseDate = DateTool.GetDateParts(this.warehouseReleasePM.ExpectedReleaseDate).DateObject;
    }

    EditPackage(warehouseReleasePackagePM: WarehouseReleasePackagePM) {



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
            }
        });



    }



    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private timerToken: any;
    private Retries: number = 0;
    private GeneratedComponent: any;

    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.LoadChildComponent();
            }
        }

        else {
            this.RunComponentTimer();
        }
    }


    RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }
    warehouseReleasePackagesDetailsComponent: any;
    LoadChildComponent() {
       
        let warehouseEntryPackagesDetailsComponenttLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "WRPD")[0];
        if (warehouseEntryPackagesDetailsComponenttLocation != null) {
            SessionLocator.DynamicLoader.Load('./Warehouse/Components/WarehouseReleasePackagesDetailsComponent', warehouseEntryPackagesDetailsComponenttLocation.viewContainerRef)
                .then(cmpRef => {
                    var windowArgs: any = { WarehouseReleasePM: this.warehouseReleasePM, ViewModelTrigger: this, ShipmentPM: this.ShipmentPM, WarehouseEntryId: this.WarehouseEntryId, IsFilterByShipmentId: this.IsFilterByShipmentId };
                    cmpRef.instance.SetWindowArgs(windowArgs);
                    this.warehouseReleasePackagesDetailsComponent = cmpRef.instance;
                });

        }



    }




}
