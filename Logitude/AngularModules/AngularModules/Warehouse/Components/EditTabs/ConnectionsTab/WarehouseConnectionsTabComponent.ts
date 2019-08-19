import {Component, OnInit, OnDestroy} from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';

import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {WarehouseEntryPMExtendedService} from '../../../Services/ExtendedPMs/WarehouseEntryPMExtendedService';
import { WarehouseReleasePMExtendedService } from '../../../Services/ExtendedPMs/WarehouseReleasePMExtendedService';

@Component({
    selector: 'WarehouseConnectionsTabComponent',
    moduleId: module.id,
    templateUrl: './WarehouseConnectionsTabComponent.html',
})

export class WarehouseConnectionsTabComponent implements OnInit  {
    public EntityPM: any;
    public ObjectTableName: string;
    private warehouseEntryPMExtendedService: WarehouseEntryPMExtendedService;
    private warehouseReleasePMExtendedService: WarehouseReleasePMExtendedService;
    public ItemsSource: any[] = [];
    public ReleaseItemsSource: any[] = [];
 
    connectedTo: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.warehouseEntryPMExtendedService = new WarehouseEntryPMExtendedService();
        this.warehouseReleasePMExtendedService = new WarehouseReleasePMExtendedService();
        if (!AppTool.IsNullOrEmpty(this.EntityPM.ShipmentId)) {

            this.LoadData();
        } else this.IsShowMessageNoConnectedEntity = true;
    }

    ngOnInit() {
  
    }

    IsShowMessageNoConnectedEntity: boolean = false;
    IsShowMessageNoConnectedReleaseEntity: boolean = false;
    LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ItemsSource = [];
        this.ReleaseItemsSource = [];
       
       
        this.warehouseEntryPMExtendedService.GetWarehouseConnectedEntitiesByEntityId(this.EntityPM.ShipmentId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.ItemsSource = myResponse.Result;
                   
                    if (!this.ItemsSource || this.ItemsSource.length == 0) {
                        
                        this.IsShowMessageNoConnectedEntity = true;
                    } else {
                        if (this.EntityPM.ConnectedTo != null) {
                            this.connectedTo =  this.EntityPM.ConnectedTo;
                        }
                        this.IsShowMessageNoConnectedEntity = false;
                    }
                }
            }
            this.CurrentSession.StopBusyIndicator();
        });


        //////////////////
        this.warehouseReleasePMExtendedService.GetWarehouseConnectedReleaseByEntityId(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.ReleaseItemsSource = myResponse.Result;

                    if (!this.ReleaseItemsSource || this.ReleaseItemsSource.length == 0) {

                        this.IsShowMessageNoConnectedReleaseEntity = true;
                    } else {
                      
                        this.IsShowMessageNoConnectedReleaseEntity = false;
                       
                    }
                }
            }
            this.CurrentSession.StopBusyIndicator();
        });


    }

    ViewEntitytClicked() {

        var backLabel = this.ObjectTableName == "WarehouseEntry" ? "Entry " + this.EntityPM.EntryNumber : "Release " + this.EntityPM.ReleaseNumber;

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.EntityPM.ShipmentId, ObjectTableName: "Shipment", BackButtonLabel: backLabel });

                    let isEditComponentSaved = false;
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        if (isEditComponentSaved) {
                            this.entityArgs.EditComponent.ReloadEntityPM();
                        }
                    });

                    cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                        if (isSaveSuccess) {
                            isEditComponentSaved = true;
                        }
                    });
                });
        
    }



    ViewReleaseClicked(item: any) {

        var myBackButtonLabel = "Cross Docks";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: item.Id, ObjectTableName: "WarehouseRelease", BackButtonLabel: myBackButtonLabel });
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    this.LoadData();
                });
            });
    }

}


