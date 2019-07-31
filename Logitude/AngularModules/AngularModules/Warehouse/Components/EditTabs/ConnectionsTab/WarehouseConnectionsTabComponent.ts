import {Component, OnInit, OnDestroy} from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';

import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {WarehouseEntryPMExtendedService} from '../../../Services/ExtendedPMs/WarehouseEntryPMExtendedService';

@Component({
    selector: 'WarehouseConnectionsTabComponent',
    moduleId: module.id,
    templateUrl: './WarehouseConnectionsTabComponent.html',
})

export class WarehouseConnectionsTabComponent implements OnInit  {
    public EntityPM: any;
    public ObjectTableName: string;
    private warehouseEntryPMExtendedService: WarehouseEntryPMExtendedService;
    public ItemsSource: any[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.warehouseEntryPMExtendedService = new WarehouseEntryPMExtendedService();
        if (!AppTool.IsNullOrEmpty(this.EntityPM.ShipmentId)) {

            this.LoadData();
        } else this.IsShowMessageNoConnectedEntity = true;
    }

    ngOnInit() {
  
    }

    IsShowMessageNoConnectedEntity: boolean = false;

    LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ItemsSource = [];
        this.warehouseEntryPMExtendedService.GetWarehouseConnectedEntitiesByEntityId(this.EntityPM.ShipmentId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.ItemsSource = myResponse.Result;
                    if (!this.ItemsSource || this.ItemsSource.length == 0) {
                        this.IsShowMessageNoConnectedEntity = true;
                    } else this.IsShowMessageNoConnectedEntity = false;
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

}


