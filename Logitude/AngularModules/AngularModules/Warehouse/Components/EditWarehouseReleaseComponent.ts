declare var System: any;
declare var window: any;


import {ConfirmWindow} from '../../Controls/Windows/ConfirmWindow';
import {Component, OnInit, ViewChildren, QueryList}  from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import {EntityArgs} from '../../Infrastructure/DataContracts/EntityArgs';
import {WarehouseReleasePackagePM} from '../../Warehouse/EntityPMs/WarehouseReleasePackagePM';
import {WarehouseReleasePM} from '../../Warehouse/EntityPMs/WarehouseReleasePM';
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, DateTool, FormatTool} from '../../Infrastructure/Tools';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';

import {ShipmentPMService } from '../../Shipment/Services/StandardPMs/ShipmentPMService';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import {LocationDirective} from '../../Infrastructure/Utilities/LocationDirective';

@Component({
    moduleId: module.id,
    selector: 'EditWarehouseReleaseComponent',
    templateUrl: './EditWarehouseReleaseComponent.html',

})
export class EditWarehouseReleaseComponent extends BaseComponent implements OnInit {

    DataContext: any = this;
    public ValidationErrorsList: string[];
    private _entityResourceService: EntityResourceService = new EntityResourceService();

    WarehouseReleasePackagesLists: WarehouseReleasePackagePM[] = [];
  
    ShipmentPM: any;
    SelectedWarehouseReleasePackage: WarehouseReleasePackagePM;
    warehouseReleasePM: WarehouseReleasePM;
    private myShipmentPMService: ShipmentPMService;

    IsLoadPage: boolean = false;
    ObjectTableId: string;
    IsContainerShipment: boolean = false;
    VolumeLabel: string;
    GrossWeightLabel: string;
    DimensionsLabel: string;
    ObjectTableName: string;
    VolumetricWeightLabel: string;
    ActualReleaseDateOldValue: Date;
    ExpectedReleaseDateOldValue: Date;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.myShipmentPMService = new ShipmentPMService();

        if (this.entityArgs.EntityPM) {
            this.warehouseReleasePM = this.entityArgs.EntityPM;
            this.WarehouseReleasePackagesLists = this.entityArgs.EntityPM.WarehouseReleasePackages;
        }
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.Listen();
        this.warehouseReleasePM.UIProperties.SetEnabled("CreatedByUserId", "WarehouseRelease", false);
        var table = window.ObjectTables.filter(d => d.Name == this.ObjectTableName)[0];
        if (table) {
            this.ObjectTableId = table.Id;
        }



    }

    ngOnInit(

    ) {

        this._entityResourceService.getEntityResourceByTableName("WarehouseRelease", 0).subscribe(response => {

            if (this.entityArgs.EntityPM) {
                this.InitializeEditWarehouseRelease();
            }
        });


    }

    private CancelReleaseChangedEvent: any = null;
    private SaveCompletedChangedEvent: any = null;
    private LoadCompletedChangedEvent: any = null;
    Listen() {

        if (this.CurrentSession.CurrentEditComponent != null) {


            if (!this.SaveCompletedChangedEvent) {
                this.SaveCompletedChangedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.CurrentSession.FireEvent("LoadEventTabData");
                    }
                });
            }


            if (!this.LoadCompletedChangedEvent) {
                this.LoadCompletedChangedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.warehouseReleasePM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

        }

        if (!this.CancelReleaseChangedEvent) {
            this.CancelReleaseChangedEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "CancelRelease") {
                    this.SetEnableProperties();
                }
            });
        }

    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.CancelReleaseChangedEvent);
        AppTool.KillEventEmitter(this.SaveCompletedChangedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedChangedEvent);

    }








    IsLCLEntity: boolean;
    InitializeEditWarehouseRelease() {

        if (this.warehouseReleasePM) {
            if (!AppTool.IsNullOrEmpty(this.warehouseReleasePM.TransportModeId)) {
                this.IsLCLEntity = AppTool.IsLCLEntity(this.warehouseReleasePM.TransportModeId, this.warehouseReleasePM.ShipmentTypeId);
                if (!this.IsLCLEntity) {
                    this.IsContainerShipment = true;
                }
            }

            else if (!AppTool.IsNullOrEmpty(this.warehouseReleasePM.ShipmentId)) {
                this.CurrentSession.StartBusyIndicatorLoading();
                this.myShipmentPMService.get(this.warehouseReleasePM.ShipmentId).subscribe(res => {
                    var shipResponse: ServiceResponse = res;
                    this.CurrentSession.StopBusyIndicator();
                    if (!shipResponse.HasError) {
                        this.ShipmentPM = shipResponse.Result;
                        if (this.ShipmentPM) {
                            this.IsLCLEntity = AppTool.IsLCLEntity(this.ShipmentPM.TransportModeId, this.warehouseReleasePM.ShipmentTypeId);
                            if (!this.IsLCLEntity) {
                                this.IsContainerShipment = true;
                            }
                        }
                    }
                });
            }
        

      
            this.ActualReleaseDateOldValue = this.warehouseReleasePM.ActualReleaseDate;
            this.ExpectedReleaseDateOldValue = this.warehouseReleasePM.ExpectedReleaseDate;
            this.RunComponent();
          
            this.SetLabel();
            this.SetUIProperties();
            this.SetEnableProperties();
            this.IsLoadPage = true;
        }


    }

    SetUIProperties() {
        this.warehouseReleasePM.UIProperties.SetEnabled("CustomerId", "WarehouseRelease", false);
        this.warehouseReleasePM.UIProperties.SetEnabled("WarehouseId", "WarehouseRelease", false);

    }

    SetLabel() {

        this.VolumeLabel = "Volume (" + this.warehouseReleasePM.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Gross Weight (" + this.warehouseReleasePM.GrossWeightUnitCode + ")";
        this.DimensionsLabel = "Dim(L-W-H) (" + SessionLocator.TenantPM.DimensionsUnitCode + ")";
        this.VolumetricWeightLabel = "Volumetric Weight (" + this.warehouseReleasePM.ChargeableWeightUnitCode + ")";
        
    }

    IsScreenEnabled: boolean = true;
    SetEnableProperties() {
        if (this.warehouseReleasePM.StatusCode == "CARE") {
            this.warehouseReleasePM.UIProperties.SetEnabled("CustomerRef1", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("CustomerRef2", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("CreatedByUserId", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("ReleaseBy", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("MasterNumber", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("HouseNumber", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("ExpectedReleaseDate", "WarehouseRelease", false);
            this.UIProperties.SetEnabled("ActualReleaseDate", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("SpecialInstruction", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("Notes", "WarehouseRelease", false);

            this.warehouseReleasePM.UIProperties.SetEnabled("CustomerId", "WarehouseRelease", false);
            this.warehouseReleasePM.UIProperties.SetEnabled("WarehouseId", "WarehouseRelease", false);

            this.IsScreenEnabled = false;

        }
    }


   

   

    
    OnActualReleaseDateDatePickerChange(value) {

        this.warehouseReleasePM.UIProperties.SetValidity("ActualReleaseDate", "WarehouseRelease", true, null);

        if (value) {
            if (!DateTool.IsActualDateValid(value)) {
                var errorMessage = DateTool.ActualDateMessage.replace("Field", "Actual Release Date");
                this.warehouseReleasePM.UIProperties.SetValidity("ActualReleaseDate", "WarehouseRelease", false, errorMessage);
            }

            else {
                if (this.warehouseReleasePM.StatusCode != "RELE") this.warehouseReleasePM.StatusCode = "RELE";
            }
        }
        else {
            if (!this.warehouseReleasePM.ActualReleaseDate) {
                this.warehouseReleasePM.StatusCode = "CREA";
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

    LoadChildComponent() {

        let warehouseEntryPackagesDetailsComponenttLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "WRPD")[0];
        if (warehouseEntryPackagesDetailsComponenttLocation != null) {
            SessionLocator.DynamicLoader.Load('./Warehouse/Components/WarehouseReleasePackagesDetailsComponent', warehouseEntryPackagesDetailsComponenttLocation.viewContainerRef)
                .then(cmpRef => {
                    var windowArgs: any = { WarehouseReleasePM: this.warehouseReleasePM, ViewModelTrigger: this, ShipmentPM: this.ShipmentPM, IsEditMode: true };
                    cmpRef.instance.SetWindowArgs(windowArgs);

                });

        }



    }


}
