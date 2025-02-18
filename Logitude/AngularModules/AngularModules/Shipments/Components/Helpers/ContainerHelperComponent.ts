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
import { ServiceLocator } from 'Infrastructure/Locators/ServiceLocator';

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
    public IsTrackContainerVisible: boolean = false;
    public IsGeneralSimulatorVisible: boolean = false;
    public IsContainersRequestStatusVisible: boolean = false;
    ValidationErrorsList: any[];
    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (this.EntityPM) {
            this.IsSimulatorVisible = FeatureLocator.HasFeaturePermession("Shipment", "ContainerStatusSimulator");
            this.IsGeneralSimulatorVisible = FeatureLocator.HasFeaturePermession("Shipment", "VisionContainerStatusSimulator") && SessionLocator.TenantManagementJS.IsContainerTrackingPrepaid;
            this.IsContainersRequestStatusVisible = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "OIC")[0] != null;
            this.IsTrackContainerVisible = this.IsTrackContainerAllowed();
            this.Listen();
        }
    }
    private IsTrackContainerAllowed(): boolean {
        if (SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "VIP")[0] != null)
            return false;

        if (!FeatureLocator.HasFeaturePermession("Shipment", "VizionRequestStatus"))
            return false;

        if (!SessionLocator.TenantManagementJS.IsContainerTrackingPrepaid)
            return false;

        return true;
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
        ServiceLocator.SendTotangoUserActivity("Container", "Container Request Status Clicked");
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
        ServiceLocator.SendTotangoUserActivity("Container", "Shipment Containers Simulator Clicked");
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { ShipmentId: this.EntityPM.ShipmentId, IsFromContainer: true, ContainerNumber: this.EntityPM.ContainerNumber, CarrierId: this.EntityPM.MainCarriageCarrierId };
        logWindow.Title = "Shipment Containers Statuses Simulator";
        logWindow.Show('./ShipmentModules/ShipmentOthers/Components/ShipmentContainersStatuses/ContainersStatusesSimulatorComponent');
    }
    GeneralShipmentContainersSimulatorClicked() {
        ServiceLocator.SendTotangoUserActivity("Container", "General Shipment Containers Simulator Clicked");
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
    TrackContainerClicked() {
        ServiceLocator.SendTotangoUserActivity("Container", "Track Container Clicked");
        this.CurrentSession.StartBusyIndicator("Sending...");
        var args: GeneralContainerTrackingArgs = <GeneralContainerTrackingArgs> {
            ContainerId : this.EntityPM.Id,
            ContainerNumber : this.EntityPM.ContainerNumber,
            IsFromContainer : true,
            ShipmentId:this.EntityPM.ShipmentId,
            Tenant :this.EntityPM.Tenant,
            IsSimulator:false,
            Data:null,
            ContainerStatusSourceCode:'VZN'
        }
        var myService = new ShipmentContainersWebService();
        myService.TrackContainer(args).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();
            var messageWindow = new MessageWindow();
            if (myResponse.Result.Success) {
                this.entityArgs.EditComponent.ValidationErrorsList= [];
                messageWindow.Show("Request Sent Successfully");
            }else{
                this.entityArgs.EditComponent.ValidationErrorsList = myResponse.Result.Errors;
            } 

        });
    }
    
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }
}
