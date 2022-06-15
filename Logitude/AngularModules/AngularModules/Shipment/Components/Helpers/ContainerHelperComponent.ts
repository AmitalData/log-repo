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
import { GeneralContainerTrackingArgs } from 'Shipment/DataContract/GeneralContainerTrackingArgs';
import { MessageWindow } from 'Controls/Windows/MessageWindow';

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
    public IsSimulatorVisible: boolean = false;
    public IsVisionRequestStatus: boolean = false;
    public IsGeneralSimulatorVisible: boolean = false;
    ValidationErrorsList: any[];
    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (this.EntityPM) {
            this.IsSimulatorVisible = FeatureLocator.HasFeaturePermession("Shipment", "ContainerStatusSimulator");
            this.IsGeneralSimulatorVisible = FeatureLocator.HasFeaturePermession("Shipment", "VisionContainerStatusSimulator");
            this.IsVisionRequestStatus = FeatureLocator.HasFeaturePermession("Shipment", "VizionRequestStatus");
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
    GeneralShipmentContainersSimulatorClicked() {
        var logWindow = new LogitudeWindow();
        var args: GeneralContainerTrackingArgs = <GeneralContainerTrackingArgs> {
            ContainerId : this.EntityPM.Id,
            ContainerNumber : this.EntityPM.ContainerNumber,
            ContainerStatusSourceCode:null,
            IsFromContainer : true,
            ShipmentId:this.EntityPM.ShipmentId,
            Tenant :this.EntityPM.Tenant,
            IsSimulator:true,
            Data:null
        }
        logWindow.WindowArgs = args;
        logWindow.Title = "Shipment Containers Statuses Simulator";
        logWindow.Show('./ShipmentModules/ShipmentOthers/Components/GeneralContainersStatusesSimulator/GeneralContainersStatusesSimulatorComponent');
    }
    VizionSimulateClicked() {
        this.CurrentSession.StartBusyIndicator("Simulating...");
        var args: GeneralContainerTrackingArgs = <GeneralContainerTrackingArgs> {
            ContainerId : this.EntityPM.Id,
            ContainerNumber : this.EntityPM.ContainerNumber,
            ContainerStatusSourceCode:null,
            IsFromContainer : true,
            ShipmentId:this.EntityPM.ShipmentId,
            Tenant :this.EntityPM.Tenant,
            IsSimulator:false,
            Data:null,
            SourceCode:'VZN'
        }
        var myService = new ShipmentContainersWebService();
        myService.GeneralContainerSimulator(args).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();
            var messageWindow = new MessageWindow();
            if (myResponse.Result.Success) {
                this.ValidationErrorsList = [];
                messageWindow.Show("Request Sent Successfully");
            }else{
                this.ValidationErrorsList = myResponse.Result.Errors;
            } 

        });
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }
}
