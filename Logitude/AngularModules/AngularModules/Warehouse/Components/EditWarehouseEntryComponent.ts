declare var System: any;
declare var window: any;

import {ConfirmWindow} from '../../Controls/Windows/ConfirmWindow';
import {Component, OnInit, ViewChildren, QueryList} from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import {EntityArgs} from '../../Infrastructure/DataContracts/EntityArgs';
import {WarehouseEntryPM} from '../../Warehouse/EntityPMs/WarehouseEntryPM';
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, DateTool, FormatTool} from '../../Infrastructure/Tools';
import {EventTypeArgs} from '../../Infrastructure/DataContracts/EventTypeArgs';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import {TraceEventExtendedPMService } from '../../Infrastructure/Services/ExtendedPMs/TraceEventExtendedPMService';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import {EventTypeClass} from '../../Infrastructure/DataContracts/EventTypeArgs';
import {LocationDirective} from '../../Infrastructure/Utilities/LocationDirective';
import {CardListService} from '../../Common/Services/StandardLists/CardListService';

@Component({
    moduleId: module.id,
    selector: 'EditWarehouseEntryComponent',
    templateUrl: './EditWarehouseEntryComponent.html',
})


export class EditWarehouseEntryComponent extends BaseComponent implements OnInit {
    public ValidationErrorsList: string[];
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    DataContext: any = this;

    warehouseEntryPM: WarehouseEntryPM;
    private myCardListService: CardListService;

    IsLoadPage: boolean = false;
    ObjectTableId: string;
    IsContainerShipment: boolean = false;
    ObjectTableName: string;
    ActualEntryDateOldValue: Date;
    ExpectedEntryDateOldValue: Date;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.warehouseEntryPM = this.entityArgs.EntityPM;
        if (AppTool.IsNullOrEmpty(this.warehouseEntryPM.ReceivedBy)) {
            this.warehouseEntryPM.ReceivedBy = SessionLocator.LoggedUserPM.EnglishName;
            this.warehouseEntryPM.IsDirty = false;
        }


        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.Listen();
       // this.RunComponent();
        this.myCardListService = new CardListService();
        this.warehouseEntryPM.UIProperties.SetEnabled("CreatedByUserId", "WarehouseEntry", false);



        var table = window.ObjectTables.filter(d=> d.Name == this.ObjectTableName)[0];
        if (table) {
            this.ObjectTableId = table.Id;
        }



    }

    ngOnInit(

    ) {

        this._entityResourceService.getEntityResourceByTableName("WarehouseEntry", 0).subscribe(response => {
            this.InitializeEditWarehouseEntry();
        });


    }

    SaveCompletedEvent: any;
    LoadCompletedEvent: any;
    

    Listen() {


        if (this.CurrentSession.CurrentEditComponent != null) {

            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.CurrentSession.FireEvent("LoadEventTabData");
                    }

                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.warehouseEntryPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }

    }



    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);

    }


    IsLCLEntity: boolean;

    InitializeEditWarehouseEntry() {

        if (this.warehouseEntryPM)
        {
           

            this.IsLCLEntity = AppTool.IsLCLEntity(this.warehouseEntryPM.TransportModeId, this.warehouseEntryPM.ShipmentTypeId);
            this.IsContainerShipment = !this.IsLCLEntity;
     
            this.ExpectedEntryDateOldValue = this.warehouseEntryPM.ExpectedEntryDate;
            this.ActualEntryDateOldValue = this.warehouseEntryPM.ActualEntryDate;
            this.SetUIProperties();
            this.IsLoadPage = true;
        }

      
    }




    //@ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    //private timerToken: any;
    //private Retries: number = 0;
    //private GeneratedComponent: any;

    //RunComponent() {
    //    if (this.AllLocations) {

    //        if (this.AllLocations.length == 0) {
    //            this.RunComponentTimer();
    //        }

    //        else {
    //            this.LoadChildComponent();
    //        }
    //    }

    //    else {
    //        this.RunComponentTimer();
    //    }
    //}


    //RunComponentTimer() {
    //    this.Retries++;

    //    if (this.timerToken) {
    //        clearTimeout(this.timerToken);
    //    }

    //    if (this.Retries < 20) {
    //        this.timerToken = setTimeout(() => this.RunComponent(), 1);
    //    }
    //}


    //WarehouseEntryPackagesDetailsComponent: any;
    //LoadChildComponent() {

    //    let warehouseEntryPackagesDetailsComponenttLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "WEPD")[0];
    //    if (warehouseEntryPackagesDetailsComponenttLocation != null) {
    //        SessionLocator.DynamicLoader.Load('./Warehouse/Components/WarehouseEntryPackagesDetailsComponent', warehouseEntryPackagesDetailsComponenttLocation.viewContainerRef)
    //            .then(cmpRef => {

    //                this.WarehouseEntryPackagesDetailsComponent = cmpRef.instance;
    //                var windowArgs: any = { WarehouseEntryPM: this.warehouseEntryPM, ViewModelTrigger: this, IsFromShipment: true, IsEditMode:true };
    //                cmpRef.instance.SetWindowArgs(windowArgs);

    //            });

    //    }



    //}


    get WarehouseId() {
        var warehouseId: string = null;
        if (this.warehouseEntryPM) warehouseId = this.warehouseEntryPM.WarehouseId;
        return warehouseId;
    }
    set WarehouseId(newValue: string) {
        if (this.warehouseEntryPM.WarehouseId != newValue) {
            this.warehouseEntryPM.WarehouseId = newValue;
            if (AppTool.IsNullOrEmpty(newValue)) {
                this.warehouseEntryPM.WarehouseName = null;
   
            }
            
            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myCardList: any = myResponse.Result;
                        if (myCardList) {

                            this.warehouseEntryPM.WarehouseName = myCardList.EnglishName;
              
                        }
                    }
                });
            }
        }
    }



    SetUIProperties() {
        this.warehouseEntryPM.UIProperties.SetEnabled("CustomerId", "WarehouseEntry", false);
        //this.warehouseEntryPM.UIProperties.SetEnabled("WarehouseId", "WarehouseEntry", false);

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

    

    OnActualEntryDateDatePickerChange(value) {

        this.warehouseEntryPM.UIProperties.SetValidity("ActualEntryDate", "WarehouseEntry", true, null);

        if (value) {
            if (!DateTool.IsActualDateValid(value)) {
                var errorMessage = DateTool.ActualDateMessage.replace("Field", "Actual Entry Date");
                this.warehouseEntryPM.UIProperties.SetValidity("ActualEntryDate", "WarehouseEntry", false, errorMessage);
            }

            else {
                if (this.warehouseEntryPM.StatusCode != "ENTE") this.warehouseEntryPM.StatusCode = "ENTE";
            }
        } else {
            if (!this.warehouseEntryPM.ActualEntryDate) {
                this.warehouseEntryPM.StatusCode = "CREA";
            } 
        }
    }
    

    SetActualDateClicked(fieldName: string) {
        this.ActualEntryDate = DateTool.GetDateParts(this.warehouseEntryPM.ExpectedEntryDate).DateObject;
    }
}
