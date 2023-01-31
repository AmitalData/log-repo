import { Component } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../../Infrastructure/Tools';
import { WarehouseReleasePM } from '../../../EntityPMs/WarehouseReleasePM';

@Component({

    templateUrl: './WarehouseReleaseAuditTabComponent.html',
})

export class WarehouseReleaseAuditTabComponent {
    public EntityId: string;
    public EntityPM: WarehouseReleasePM;
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
                if (tabCode == "WRAU") {
                    this.CurrentSession.FireEvent("WarehouseRelease");
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
    }
}
