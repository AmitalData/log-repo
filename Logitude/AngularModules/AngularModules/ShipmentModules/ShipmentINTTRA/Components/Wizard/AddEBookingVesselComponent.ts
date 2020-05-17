import { Component } from '@angular/core';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    templateUrl: './AddEBookingVesselComponent.html',
})

export class AddEBookingVesselComponent extends BaseComponent {

    public EntityPM: ShipmentPM;
    public ObjectTableName = "Shipment";
    public DataContext = this;
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("ok");
    }

}
