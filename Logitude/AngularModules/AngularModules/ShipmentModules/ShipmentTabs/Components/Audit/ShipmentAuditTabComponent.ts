
import {Component}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './ShipmentAuditTabComponent.html',
})

export class ShipmentAuditTabComponent {
    public EntityId: string;
    public EntityPM: ShipmentPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (this.EntityPM != null) {
            this.EntityId = this.EntityPM.Id;

        }

        this.Listen();
    }

    private TabSelectedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "SHAU") {
                    this.CurrentSession.FireEvent("Shipment");
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
    }
}
