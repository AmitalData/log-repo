import { Component, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { ShipmentPM } from '../../EntityPMs/ShipmentPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { ShipmentContainersWebService } from '../../../Shipment/Services/ShipmentContainersWebService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';

@Component({

    templateUrl: './ContainerHelperComponent.html',
})

export class ContainerHelperComponent implements OnDestroy {
    public EntityPM: ShipmentPM;
    public EntityTitle: string;
    public IsFollowupsVisible: boolean = false;
    public IsAnalyzeChampXMLButtonVisible: boolean = false;
    _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;

    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef) {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) {
            this.Listen();
        }
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    }
                });
            }

            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    }
                });
            }
        }
    }

    ContainersRequestStatusClicked() {
        this.CurrentSession.StartBusyIndicator("Sending");
        var service = new ShipmentContainersWebService();
        service.GetContainerStatusResult(null, this.EntityPM.Id, true).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {

            }
            else {
                this.entityArgs.EditComponent.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }
}
