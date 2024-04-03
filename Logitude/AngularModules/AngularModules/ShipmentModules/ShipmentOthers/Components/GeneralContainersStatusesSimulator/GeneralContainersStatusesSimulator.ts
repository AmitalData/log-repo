import { Component, Type } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ShipmentContainersWebService, ShipmentContainerSimulator } from '../../../../Shipment/Services/ShipmentContainersWebService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { GeneralContainerTrackingArgs } from '../../../../Shipment/DataContract/GeneralContainerTrackingArgs';

@Component({

    templateUrl: './GeneralContainersStatusesSimulatorComponent.html',
})

export class GeneralContainersStatusesSimulatorComponent extends BaseComponent {
    public ValidationErrorsList: string[] = [];

    private CurrentSession = SessionLocator.SelectedSession;
    public generalContainerStatusSimulatorArgs: GeneralContainerTrackingArgs = <GeneralContainerTrackingArgs>{};
    public sourceCode;
    DataContext = this;
    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        if (args) {
            this.generalContainerStatusSimulatorArgs = args;
        }
    }




    CancelClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SimulateClicked() {
        this.CurrentSession.StartBusyIndicator("Simulating...");
        this.generalContainerStatusSimulatorArgs.ContainerStatusSourceCode = this.sourceCode
        var myService = new ShipmentContainersWebService();
        myService.TrackContainer(this.generalContainerStatusSimulatorArgs).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();
            var messageWindow = new MessageWindow();
            if (myResponse.Result.Success) {
                this.ValidationErrorsList = [];
                messageWindow.Show("Simulated Successfully");
            }else{
                this.ValidationErrorsList = myResponse.Result.Errors;
            } 

        });
    }

    ViziionUnsubscribe() {
        var shipmentContainersWebService = new ShipmentContainersWebService();
        this.CurrentSession.StartBusyIndicator("Unsubscribe...");
        var args: GeneralContainerTrackingArgs = <GeneralContainerTrackingArgs>{
            ContainerId:this.generalContainerStatusSimulatorArgs.ContainerId,
            ShipmentId:this.generalContainerStatusSimulatorArgs.ShipmentId,
            IsFromContainer: this.generalContainerStatusSimulatorArgs.IsFromContainer,
            IsSimulator:true,
            SourceCode:'VZN'
        }
        shipmentContainersWebService.ViziionUnsubscribe(args).subscribe(e=>{
            this.CurrentSession.StopBusyIndicator();
            if(e.HasError){
                this.ValidationErrorsList = e.ErrorsArray;
            }else{
                var messageWindow = new MessageWindow();
                this.ValidationErrorsList = [];
                messageWindow.Show(e.Result.message);
            }
            
        })
    }

}

