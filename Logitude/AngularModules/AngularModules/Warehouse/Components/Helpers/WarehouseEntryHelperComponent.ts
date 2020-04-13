import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { WarehouseEntryPM } from '../../EntityPMs/WarehouseEntryPM';
import { Component } from '@angular/core';

@Component({
    moduleId: module.id,
    templateUrl: "WarehouseEntryHelperComponent.html",
})

export class WarehouseEntryHelperComponent {
    public EntityPM: WarehouseEntryPM;
    public ObjectTableName = "WarehouseEntry";
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
