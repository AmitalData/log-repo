import {Component}  from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {ContainerPM} from '../../../../../Shipment/EntityPMs/ContainerPM';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../../Infrastructure/Tools';

@Component({  
    templateUrl: './ContainerAuditTabComponent.html',
})

export class ContainerAuditTabComponent {
    public EntityId: string;
    public EntityPM: ContainerPM;
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
                if (tabCode == "COAU") {
                    this.CurrentSession.FireEvent("ContainerPM");
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
    }
}
