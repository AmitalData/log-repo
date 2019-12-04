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
    moduleId: module.id,
    selector: 'NewWarehouseReleaseComponent',
    templateUrl: './NewWarehouseReleaseComponent.html',
    providers: [WarehouseReleasePMExtendedService, WarehouseEntryPackagePMExtendedService],

})
export class NewWarehouseReleaseComponent extends BaseComponent implements OnInit {


    public WarehouseReleasePackagesLists: WarehouseReleasePackagePM[] = [];

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
    private CurrentSession = SessionLocator.SelectedSession;
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
        this._entityResourceService.getEntityResourceByTableName("WarehouseRelease").subscribe(response => {
            this.Start(args);

        });

    }
    IsLCLEntity: boolean = true;

    Start(args: any) {

        this.ShipmentPM = args.ShipmentPM;


        this.SetLabel();
        this.SetValue(args);
        this.RunComponent();
        this.IsLoadPage = true;



    }
    IsLoadWarehouse: boolean = false;
    SetValue(args: any) {
        if (this.warehouseReleasePM) {

            if (this.ShipmentPM) {
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
            }



            this.ConnectedTo = this.warehouseReleasePM.ConnectedTo;

            //this.FromPortId = this.ShipmentPM ? this.ShipmentPM.MainCarriageFromPortId ? this.ShipmentPM.MainCarriageFromPortId : this.ShipmentPM.FromPortId : "";

            //this.ToPortId = this.ShipmentPM.ShipmentLevelCode == "H" ? this.ShipmentPM.MainCarriageFinalDestinationPortId : this.ShipmentPM.FinalDistenationPortId;

            this.IsLCLEntity = AppTool.IsLCLEntity(this.warehouseReleasePM.TransportModeId, this.warehouseReleasePM.ShipmentTypeId);

            var myCommonDomain = new CommonDomainService();
            myCommonDomain.GetDeafaultMyWarehouse().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var warehouseId = myResponse.Result;
                    if (!AppTool.IsNullOrEmpty(warehouseId) && AppTool.IsNullOrEmpty(args.WarehouseId)) {
                        this.warehouseReleasePM.WarehouseId = warehouseId;
                    }
                    else {
                        this.warehouseReleasePM.WarehouseId = args.WarehouseId;
                    }
                } else {
                    this.warehouseReleasePM.WarehouseId = args.WarehouseId;

                }



            });


            this.warehouseReleasePM.ExpectedReleaseDate = args.ExpectedReleaseDate;
            this.warehouseReleasePM.ActualReleaseDate = args.ActualReleaseDate;
            this.ActualReleaseDateOldValue = this.warehouseReleasePM.ActualReleaseDate;
            this.ExpectedReleaseDateOldValue = this.warehouseReleasePM.ExpectedReleaseDate;

        }
    }

    SetLabel() {

        this.VolumeLabel = "Volume (" + SessionLocator.TenantPM.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Gross Weight (" + SessionLocator.TenantPM.GrossWeightUnitCode + ")";
        this.DimensionsLabel = "Dim(L-W-H) (" + SessionLocator.TenantPM.DimensionsUnitCode + ")";
        this.ChargeableWeightLabel = "ChargeableWeight (" + SessionLocator.TenantPM.ChargeableWeightUnitCode + ")";


        this.WeightColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.GrossWeight").replace("%UnitCode", this.ShipmentPM.GrossWeightUnitCode);
        this.DimensionsColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.Dimensions").replace("%UnitCode", this.ShipmentPM.DimensionsUnitCode);
        this.VolumetricWeightColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.VolWeight").replace("%UnitCode", this.ShipmentPM.ChargeableWeightUnitCode);
        this.PackageTypeColumnHeader = TextCodeTranslator.Translate("ShipmentPackage.F.PackageTypeId");
    }


    LoadAllWarehouseEntryPackagesLists() {

        //if (this.ShipmentPM) {
        //    this.CurrentSession.StartBusyIndicatorLoading();
        //    this.warehouseEntryPackagePMExtendedService.GetWarehouseEntryPackagePMListsByShipmentIdAndWarehouseIdAndCustomerId(this.ShipmentPM.Id, this.warehouseReleasePM.CustomerId, this.warehouseReleasePM.WarehouseId, this.ShipmentPM.Tenant).subscribe((res: any) => {
        //        var pmResponse: ServiceResponse = res;
        //        this.CurrentSession.CurrentWindow.StopBusyIndicator();
        //        if (!pmResponse.HasError) {
        //            this.AllWarehouseEntryPackagesLists = pmResponse.Result;

        //        }

        //    });
        //}
        if (this.ShipmentPM) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.warehouseEntryPackagePMExtendedService.GetWarehouseEntryPackagePMListsByShipmentIdAndWarehouseIdAndCustomerId(null, this.warehouseReleasePM.CustomerId, this.warehouseReleasePM.WarehouseId, this.ShipmentPM.Tenant).subscribe((res: any) => {
                var pmResponse: ServiceResponse = res;
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!pmResponse.HasError) {
                    this.AllWarehouseEntryPackagesLists = pmResponse.Result;

                }

            });
        }
    }

    CustomerValueChange(item) {
        if (item) {
            this.RefreshWarehouseEntryPackagesLists("CustomerId", item);
        }
    }
    WarehouseValueChange(item) {

        if (item) {
            this.RefreshWarehouseEntryPackagesLists("WarehouseId", item);
        }
    }



    RefreshWarehouseEntryPackagesLists(fieldName: string, item: any) {
        if (this.CustomWarehouseEntryPackagesLists && this.CustomWarehouseEntryPackagesLists.length > 0) {
            var lists = [];
            if (fieldName == "WarehouseId") {
                lists = this.CustomWarehouseEntryPackagesLists.filter(d => d.WarehouseId == item.Id && d.CustomerId == this.warehouseReleasePM.CustomerId);
            }
            else {
                lists = this.CustomWarehouseEntryPackagesLists.filter(d => d.CustomerId == item.Id && d.WarehouseId == this.warehouseReleasePM.WarehouseId);
            }

            if (!lists || (lists && lists.length == 0)) {
                this.IsChangeWarehouseIdOrCustomerId = true;
                this.WarehouseReleasePackagesLists = [];
            }
        }
        else {
            this.IsChangeWarehouseIdOrCustomerId = true;
            this.WarehouseReleasePackagesLists = [];
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


    get ToPortId() {
        var toportid: string = null;
        if (this.warehouseReleasePM) toportid = this.warehouseReleasePM.ToPortId;
        return toportid;
    }
    set ToPortId(value: string) {
        if (this.warehouseReleasePM != null) {
            if (value != this.warehouseReleasePM.ToPortId) {
                this.warehouseReleasePM.ToPortId = value;
                // this.OnActualReleaseDateDatePickerChange(value);
            }
        }
    }

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


    SetActualDateClicked(fieldName: string) {
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

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    LoadChildComponent() {

        let warehouseEntryPackagesDetailsComponenttLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "WRPD")[0];
        if (warehouseEntryPackagesDetailsComponenttLocation != null) {
            SessionLocator.DynamicLoader.Load('./Warehouse/Components/WarehouseReleasePackagesDetailsComponent', warehouseEntryPackagesDetailsComponenttLocation.viewContainerRef)
                .then(cmpRef => {
                    var windowArgs: any = { WarehouseEntryPM: this.warehouseReleasePM, ViewModelTrigger: this, ShipmentPM: this.ShipmentPM };
                    cmpRef.instance.SetWindowArgs(windowArgs);

                });

        }



    }




}
