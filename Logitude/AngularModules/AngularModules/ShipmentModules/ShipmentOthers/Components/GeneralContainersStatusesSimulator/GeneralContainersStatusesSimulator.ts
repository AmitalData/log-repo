import { Component, Type } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ShipmentContainersWebService, ShipmentContainerSimulator } from '../../../../Shipment/Services/ShipmentContainersWebService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { GeneralContainerStatusSimulatorArgs } from 'Shipment/DataContract/GeneralContainerStatusSimulatorArgs';

@Component({

    templateUrl: './GeneralContainersStatusesSimulatorComponent.html',
})

export class GeneralContainersStatusesSimulator {
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    private isFromContainer = false;
    private shipmentId: string;
    private containerNumber: string;
    private carrierId: string;
    public Data: string = null;


    constructor() {

    }

    SetWindowArgs(args: any) {
        if (args) {
            this.isFromContainer = args.IsFromContainer;
            this.shipmentId = args.ShipmentId;
            this.containerNumber = args.ContainerNumber;
            this.carrierId = args.CarrierId;
        }
    }


    

    CancelClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SimulateClicked() {
        var errors: string[] = [];
        this.ValidationErrorsList = errors;

        this.CurrentSession.StartBusyIndicator("Simulating...");

        var simulatorArgs = new GeneralContainerStatusSimulatorArgs();
        simulatorArgs.Data = this.Data;
        simulatorArgs.IsFromContainer = this.isFromContainer;
        simulatorArgs.ShipmentId = this.shipmentId;
        simulatorArgs.ContainerNumber = this.containerNumber;
        simulatorArgs.CarrierId = this.carrierId;


        this.semulator(simulatorArgs)





    }

    semulator(simulatorArgs: GeneralContainerStatusSimulatorArgs) {
        var myService = new ShipmentContainersWebService();
        myService.VisionContainerSimulator(simulatorArgs).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();

            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                var myResult: ShipmentContainerSimulator = myResponse.Result;

                if (myResult.Success) {
                    var messageWindow = new MessageWindow();
                    messageWindow.Show("Simulated Successfully");
                }

                else {
                    this.ValidationErrorsList = myResult.Errors;
                }
            }
        });
    }
}

