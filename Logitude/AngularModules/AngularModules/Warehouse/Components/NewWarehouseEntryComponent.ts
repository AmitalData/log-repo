
declare var System: any;
declare var window: any;
import {Component, OnInit, ViewChildren, QueryList} from '@angular/core';
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ConfirmWindow} from '../../Controls/Windows/ConfirmWindow';

import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';

import {WarehouseEntryPM} from '../../Warehouse/EntityPMs/WarehouseEntryPM';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {WarehouseEntryPackagePM} from '../../Warehouse/EntityPMs/WarehouseEntryPackagePM';


import {ShipmentPM} from '../../Shipment/EntityPMs/ShipmentPM';

import {PackageTypeList} from '../../Common/EntityLists/PackageTypeList';
import {ClassLevelValidator} from '../../Infrastructure/Validators/ClassLevelValidator';
import {CommonDomainService} from '../../Common/Services/CommonDomainService';
import {WarehouseHelper} from '../Helpers/WarehouseHelper';
import {LocationDirective} from '../../Infrastructure/Utilities/LocationDirective';

@Component({
    moduleId: module.id,
    selector: 'NewWarehouseEntryComponent',
    templateUrl: './NewWarehouseEntryComponent.html',
 
})

export class NewWarehouseEntryComponent extends BaseComponent implements OnInit {
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    warehouseHelper: WarehouseHelper = new WarehouseHelper();
    public ValidationErrorsList: string[];

    IsNotSetWarehouseIdForWarehouseLegShipment: boolean = false;
    DataContext: any = this;
    ShipmentPM: ShipmentPM;
    warehouseEntryPM: WarehouseEntryPM = new WarehouseEntryPM();
    SelectedWarehouseEntryPackage: WarehouseEntryPackagePM;

    IsNewEntity: boolean = false;
    validator: ClassLevelValidator;
    ObjectTableId: string;
    IsFromShipment: boolean = true;
    IsLoadPage: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.warehouseEntryPM = this.warehouseHelper.GetNewWarehouseEntry(this);

        this.validator = new ClassLevelValidator();

        var table = window.ObjectTables.filter(d=> d.Name == "WarehouseEntry")[0];
        if (table) this.ObjectTableId = table.Id;

    } ngOnInit() {


    }

    SetWindowArgs(args: any) {
        this._entityResourceService.getEntityResourceByTableName("WarehouseEntry").subscribe(response => {

            this.Start(args);
        });

    }

    IsLCLEntity: boolean = true;



  
    Start(args:any) {

        this.RunComponent();
        this.ShipmentPM = args.ShipmentPM;
        
        if (args.WarehouseEntryPackagesLists){
            args.WarehouseEntryPackagesLists.forEach((item) => {
                this.warehouseEntryPM.AddWarehouseEntryPackage(item);
            });

        
        }

        this.SetValue(args);
        this.IsLoadPage = true;


    }

  


    SetValue(args: any) {

        if (this.warehouseEntryPM) {
        
            if (this.ShipmentPM) {
                if (this.ShipmentPM.ShipmentLevelCode == "D") this.warehouseEntryPM.CustomerId = this.ShipmentPM.CustomerId;
                this.warehouseEntryPM.ShipmentId = this.ShipmentPM.Id;
                this.warehouseEntryPM.ShipmentNumber = this.ShipmentPM.ShipmentNumber;
                this.warehouseEntryPM.HouseNumber = this.ShipmentPM.House;
                this.warehouseEntryPM.MasterNumber = this.ShipmentPM.LongMaster;
                this.warehouseEntryPM.ShipmentLevelCode = this.ShipmentPM.ShipmentLevelCode;
                this.warehouseEntryPM.TransportModeId = this.ShipmentPM.TransportModeId;
                this.warehouseEntryPM.ShipmentTypeId = this.ShipmentPM.ShipmentTypeId;
                this.warehouseEntryPM.ConnectedTo = args.ConnectedTo;

                this.warehouseEntryPM.DirectionId = this.ShipmentPM.DirectionId;

                this.warehouseEntryPM.ShipperId = this.ShipmentPM.ShipperId;
                this.warehouseEntryPM.ShipperName = this.ShipmentPM.ShipperName;
                this.warehouseEntryPM.ShipperReference1 = this.ShipmentPM.ShipperReference1;
                this.warehouseEntryPM.ShipperReference2 = this.ShipmentPM.ShipperReference2;

                this.warehouseEntryPM.ConsigneeId = this.ShipmentPM.ConsigneeId;
                this.warehouseEntryPM.ConsigneeName = this.ShipmentPM.ConsigneeName;
                this.warehouseEntryPM.ConsigneeReference1 = this.ShipmentPM.ConsigneeReference1;
                this.warehouseEntryPM.ConsigneeReference2 = this.ShipmentPM.ConsigneeReference2;


                if (this.warehouseEntryPM.DirectionId == "D" && this.warehouseEntryPM.TransportModeId == "I") {
                    this.warehouseEntryPM.FromAddressId = this.ShipmentPM.MainCarriageFromAddressId;
                    this.warehouseEntryPM.ToAddressId = this.ShipmentPM.MainCarriageToAddressId;
                    this.warehouseEntryPM.FromPartnerId = this.ShipmentPM.MainCarriageFromPartnerId;
                    this.warehouseEntryPM.ToPartnerId = this.ShipmentPM.MainCarriageToPartnerId;
                    this.warehouseEntryPM.FromTypeCode = "PART";
                    this.warehouseEntryPM.ToTypeCode = "PART";

                }
                else {
                    this.warehouseEntryPM.FromPortId = this.ShipmentPM.MainCarriageFromPortId ? this.ShipmentPM.MainCarriageFromPortId : this.ShipmentPM.FromPortId;
                    this.warehouseEntryPM.ToPortId = this.ShipmentPM.ShipmentLevelCode == "H" ? this.ShipmentPM.MainCarriageFinalDestinationPortId : this.ShipmentPM.FinalDistenationPortId;
                    this.warehouseEntryPM.FromTypeCode = "PORT";
                    this.warehouseEntryPM.ToTypeCode = "PORT";
                }


            }


            this.warehouseEntryPM.Ratio = AppTool.GetRatio(this.warehouseEntryPM.DirectionId, this.warehouseEntryPM.TransportModeId, this.warehouseEntryPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);









            this.IsLCLEntity = AppTool.IsLCLEntity(this.warehouseEntryPM.TransportModeId, this.warehouseEntryPM.ShipmentTypeId);
            this.IsNotSetWarehouseIdForWarehouseLegShipment = args.IsNotSetWarehouseIdForWarehouseLegShipment;
            var myCommonDomain = new CommonDomainService();
            myCommonDomain.GetDeafaultMyWarehouse().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var warehouseId = myResponse.Result;
                    if (!AppTool.IsNullOrEmpty(warehouseId) && AppTool.IsNullOrEmpty(args.WarehouseId) ) {
                        this.warehouseEntryPM.WarehouseId = warehouseId;
                    }
                    else {
                        this.warehouseEntryPM.WarehouseId = args.WarehouseId;
                    }
                }
                else {
                    this.warehouseEntryPM.WarehouseId = args.WarehouseId;
                }
            });

            this.warehouseEntryPM.ExpectedEntryDate = args.ExpectedEntryDate;
            this.warehouseEntryPM.ActualEntryDate = args.ActualEntryDate;
            this.ActualEntryDate = this.warehouseEntryPM.ActualEntryDate;


        }
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
 
        let warehouseEntryPackagesDetailsComponenttLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "WEPD")[0];
        if (warehouseEntryPackagesDetailsComponenttLocation != null) {
            SessionLocator.DynamicLoader.Load('./Warehouse/Components/WarehouseEntryPackagesDetailsComponent', warehouseEntryPackagesDetailsComponenttLocation.viewContainerRef)
                    .then(cmpRef => {
                        var windowArgs: any = { WarehouseEntryPM: this.warehouseEntryPM, ViewModelTrigger: this};
                        cmpRef.instance.SetWindowArgs(windowArgs);
                  
                    });

            }
        

     
    }

    
    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }


    SaveButtonClicked() {
        this.warehouseEntryPM.ConnectedToShipment = true;
        this.warehouseHelper.CreateWarehouseEntry(this.warehouseEntryPM, this);

    }



    OnActualEntryDateDatePickerChange(value) {

        this.warehouseEntryPM.UIProperties.SetValidity("ActualEntryDate", "WarehouseEntry", true, null);

        if (!DateTool.IsActualDateValid(value)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", "Actual Entry Date");
            this.warehouseEntryPM.UIProperties.SetValidity("ActualEntryDate", "WarehouseEntry", false, errorMessage);
        }
    }



    get ActualEntryDate() {
        var actualEntryDate: Date = null;
        if (this.warehouseEntryPM) actualEntryDate = this.warehouseEntryPM.ActualEntryDate;
        return actualEntryDate;
    }
    set ActualEntryDate(value: Date) {
        if (this.warehouseEntryPM != null) {
            if (value != this.warehouseEntryPM.ActualEntryDate) {
                this.warehouseEntryPM.ActualEntryDate = value;
                this.OnActualEntryDateDatePickerChange(value);
            }
        }
    }





    SetActualDateClicked(fieldName: string) {
        this.ActualEntryDate = DateTool.GetDateParts(this.warehouseEntryPM.ExpectedEntryDate).DateObject;
    }


}
