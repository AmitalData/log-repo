import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({

    templateUrl: './StandAlonePickupDeilveryActionsComponent.html',
})

export class StandAlonePickupDeilveryActionsComponent {
    public Code: string;
    public RoutingLinkText: string;
    public MainButtonRoutingLinkText: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetWindowArgs(args: any) {
        this.Code = args['Code'];

        switch (this.Code) {
            case "Pickup": {
                this.MainButtonRoutingLinkText = "Add Pickup"
                this.RoutingLinkText = "Add Stand Alone Shipment With Pickup";
                break;
            }

            case "Delivery": {
                this.MainButtonRoutingLinkText = "Add Delivery"
                this.RoutingLinkText = "Add Stand Alone Shipment With Delivery";
                break;
            }
        }
    }

    SelectAction(typeCode: string) {
        if (typeCode == "PickupDelivery") {
            typeCode = this.Code;
        }
        this.CurrentSession.CloseCurrentWindowEmit(typeCode);
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
