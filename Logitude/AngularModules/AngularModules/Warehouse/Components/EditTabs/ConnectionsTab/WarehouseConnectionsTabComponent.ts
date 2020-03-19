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
        this.LoadData();
    }

    ngOnInit() {
  
    }

    IsShowMessageNoConnectedEntity: boolean = false;
    IsShowMessageNoConnectedReleaseEntity: boolean = false;
    LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ItemsSource = [];
        this.ReleaseItemsSource = [];
       
        if (this.EntityPM.ShipmentId) {
            this.warehouseEntryPMExtendedService.GetWarehouseConnectedEntitiesByEntityId(this.EntityPM.ShipmentId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.ItemsSource = myResponse.Result;

                        if (!this.ItemsSource || this.ItemsSource.length == 0) {

                            this.IsShowMessageNoConnectedEntity = true;
                        } else {
                            if (this.EntityPM.ConnectedTo != null) {
                                this.connectedTo = this.EntityPM.ConnectedTo;
                            }
                            this.IsShowMessageNoConnectedEntity = false;
                        }
                    }
                }
                this.CurrentSession.StopBusyIndicator();
            });
        } else this.IsShowMessageNoConnectedEntity = true;

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

    NewWarehouseReleaseButtonClicked() {
        var windowArgs: any = {};
        windowArgs.WarehouseId = this.EntityPM.WarehouseId;
        windowArgs.CustomerId = this.EntityPM.CustomerId;
        windowArgs.FromPortId = this.EntityPM.FromPortId;
        windowArgs.ToPortId = this.EntityPM.ToPortId;
        windowArgs.WarehouseEntryId = this.EntityPM.Id;

        windowArgs.FromType = "WarehouseEntry";
       if (this.EntityPM.ShipmentId) {
            windowArgs.ConnectedTo = "Shipment";
            windowArgs.ShipmentId = this.EntityPM.ShipmentId;
        }
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1030;
        logWindow.Height = 620;
        logWindow.Title = "New Cross Dock Release";
 
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Warehouse/Components/NewWarehouseReleaseComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {

            if ($event == "Refresh") {
                this.LoadData();
            }
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


