import { Component } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';

@Component({
    templateUrl: "WorkFlowShortTitleComponent.html",
})

export class WorkFlowShortTitleComponent {
    public EntityPM: WorkFlowPM;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
    }

    get WorkflowName() {
        var result = "";
        if (this.EntityPM != null) {
            result = this.EntityPM.Name;
        }
        return result;
    }
}
