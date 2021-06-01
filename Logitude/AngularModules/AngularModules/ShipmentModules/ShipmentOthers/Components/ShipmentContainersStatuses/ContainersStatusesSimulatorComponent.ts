import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import { ShipmentContainersWebService, ShipmentContainerSimulator } from '../../../../Shipment/Services/ShipmentContainersWebService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';

@Component({
    
    templateUrl: './ContainersStatusesSimulatorComponent.html',
})

export class ContainersStatusesSimulatorComponent {
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    public AnalyzeQueueId: string = null;
    public XML_Text: string = null;

    private useAnalyzeQueueId: boolean = false;
    get UseAnalyzeQueueId() { return this.useAnalyzeQueueId; }
    set UseAnalyzeQueueId(value: boolean) {
        if (this.useAnalyzeQueueId != value) {
            this.useAnalyzeQueueId = value;

            if (value) {
                this.XML_Text = null;
            }

            else {
                this.AnalyzeQueueId = null;
            }
        }
    }

    CancelClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SimulateClicked() {
        var errors: string[] = [];

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicator("Simulating...");

            var simulator = new ShipmentContainerSimulator();
            simulator.AnalyzeQueueId = this.AnalyzeQueueId;
            simulator.XmlString = this.XML_Text;

            var myService = new ShipmentContainersWebService();

            myService.Simulate(simulator).subscribe((myResponse: ServiceResponse) => {

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
}
