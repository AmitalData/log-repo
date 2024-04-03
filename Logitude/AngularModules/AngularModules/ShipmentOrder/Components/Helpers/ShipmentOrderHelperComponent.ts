import { Component } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { ShipmentOrderPM } from '../../EntityPMs/ShipmentOrderPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    templateUrl: "ShipmentOrderHelperComponent.html",
})

export class ShipmentOrderHelperComponent {
    public EntityPM: ShipmentOrderPM;
    public ObjectTableName = "Memento";
    private CurrentSession = SessionLocator.SelectedSession;

    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
        this.Listen();
    }

    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                    }
                });
            }
        }
    }
}
