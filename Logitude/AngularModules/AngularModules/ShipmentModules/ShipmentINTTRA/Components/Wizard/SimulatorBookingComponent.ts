import { Component } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { INTRAWebService, INTTRASimulator } from '../../../../Shipment/Services/INTRAWebService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';

@Component({
    moduleId: module.id,
    templateUrl: './SimulatorBookingComponent.html',
})

export class SimulatorBookingComponent {
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    private ShipmentId: string;

    constructor() {

    }
    SetWindowArgs(args) {
        this.ShipmentId = args;
    }

    CancelClicked() {
        this.Close();
    }
    Close() {
        this.CurrentSession.CloseCurrentWindow();
    }
    SimulateClicked() {
        var errors: string[] = [];

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicator("Sending e-Booking...");
            var myService = new INTRAWebService();
            myService.SendEBooking(this.ShipmentId).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    var myResult = myResponse.Result;

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
