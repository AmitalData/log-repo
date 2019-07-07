import { Component } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { INTRAWebService, INTRAResult } from '../../../../Shipment/Services/INTRAWebService';
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
    private INTRAWebService: INTRAWebService;

    constructor() {

    }

    SetWindowArgs(args) {
        this.ShipmentId = args;
        this.INTRAWebService  = new INTRAWebService();
        this.INTRAWebService .ValidateBooking(this.ShipmentId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.CurrentSession.StopBusyIndicator();
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                var myResult: INTRAResult = myResponse.Result;

                if (myResult.Errors.length > 0) {
                    this.CurrentSession.StopBusyIndicator();
                    this.ValidationErrorsList = myResult.Errors;
                }
            }
        });
    }

    CloseButtonClicked() {
        this.Close();
    }
    Close() {
        this.CurrentSession.CloseCurrentWindow();
    }
    SendButtonClicked() {
        var errors: string[] = [];

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicator("Sending e-Booking...");
            if (this.INTRAWebService == null) {
                this.INTRAWebService = new INTRAWebService();
            }
            this.INTRAWebService .SendEBooking(this.ShipmentId).subscribe((myResponse: ServiceResponse) => {
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
