import { Component } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../../Infrastructure/Tools';
import { QuoteOPPM } from '../../../../QuoteOPM/EntityPMs/QuoteOPPM';

@Component({

    templateUrl: './QuoteAuditTabComponent.html',
})

export class QuoteAuditTabComponent {
    public EntityId: string;
    public EntityPM: QuoteOPPM;
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
                if (tabCode == "QTAU") {
                    this.CurrentSession.FireEvent("Quote");
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
    }
}
