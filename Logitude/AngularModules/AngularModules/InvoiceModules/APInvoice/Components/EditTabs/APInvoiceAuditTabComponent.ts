
import {Component}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {APInvoicePM} from '../../../../Invoice/EntityPMs/APInvoicePM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';

@Component({

    templateUrl: './APInvoiceAuditTabComponent.html',
})

export class APInvoiceAuditTabComponent {
    public EntityId: string;
    public EntityPM: APInvoicePM;
    public ObjectTableName = "APInvoice";

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
                if (tabCode == "APAU") {
                    this.CurrentSession.FireEvent(this.ObjectTableName);
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
    }
}
