import {Component} from '@angular/core';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ArtemusWebService} from '../../../../Infrastructure/Services/WebServices/ArtemusWebService';
import {ArtemusWizardArgs} from '../../../../Shipment/Args';

@Component({
    selector: 'ArtemusWizardComponent',
    moduleId: module.id,
    templateUrl: './ArtemusWizardComponent.html',
})

export class ArtemusWizardComponent {
    public ValidationErrorsList: string[] = [];
    private myArtemusWebService: ArtemusWebService;
    private ShipmentId: string;
    private Type: string;
    public MessageText: string;
    public IsMessageValid: boolean;
    public IsVisible = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
    }

    SetWindowArgs(windowArgs: ArtemusWizardArgs) {
        this.myArtemusWebService = new ArtemusWebService();
        this.ShipmentId = windowArgs.ShipmentId;
        this.Type = windowArgs.Type;
        this.CurrentSession.StartBusyIndicator("Sending...");
        if (this.Type == "BOL") {
            this.SendToArtemus_Bill();
        }
        else {
            this.SendToArtemus_Voyage();
        }
    }

    private SendToArtemus_Voyage() {
        this.myArtemusWebService.SendAMS_Voyage(this.ShipmentId).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            this.IsVisible = true;
            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
                this.MessageText = "Checking Required Fields in Shipment...";
                this.IsMessageValid = false;
            }
            else {
                this.MessageText = "Voyage message has been sent successfully";
                this.IsMessageValid = true;
            }
        });
    }

    private SendToArtemus_Bill() {
        this.myArtemusWebService.SendAMS_Bill(this.ShipmentId).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            this.IsVisible = true;
            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
                this.MessageText = "Checking Required Fields in Shipment...";
                this.IsMessageValid = false;
            }
            else {
                this.MessageText = "BOL message has been sent successfully";
                this.IsMessageValid = true;
            }
        });
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
