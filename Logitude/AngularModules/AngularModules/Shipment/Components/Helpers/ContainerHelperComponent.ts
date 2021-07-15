import { Component, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { ContainerPM } from '../../EntityPMs/ContainerPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { ShipmentContainersWebService } from '../../../Shipment/Services/ShipmentContainersWebService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';

@Component({

    templateUrl: './ContainerHelperComponent.html',
})

export class ContainerHelperComponent implements OnDestroy {
    public EntityPM: ContainerPM;
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
        service.GetContainerStatusResult(this.EntityPM.ShipmentId, this.EntityPM.Id, true).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {

            }
            else {
                this.entityArgs.EditComponent.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }
    ShipmentContainersSimulatorClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { ShipmentId: this.EntityPM.ShipmentId, IsFromContainer: true, ContainerNumber: this.EntityPM.ContainerNumber, CarrierId: this.EntityPM.MainCarriageCarrierId };
        logWindow.Title = "Shipment Containers Statuses Simulator";
        logWindow.Show('./ShipmentModules/ShipmentOthers/Components/ShipmentContainersStatuses/ContainersStatusesSimulatorComponent');
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }
}
