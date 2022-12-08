import { Component } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';
import { WorkFlowVersionPMService } from 'Workflow/Services/StandardPMs/WorkFlowVersionPMService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';

@Component({
    templateUrl: "WorkFlowShortTitleComponent.html",
})

export class WorkFlowShortTitleComponent {
    public EntityPM: WorkFlowPM;
    public workFlowVersionPMService: WorkFlowVersionPMService = new WorkFlowVersionPMService();
    private lastId:string;

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

    get WorkflowVersionNumber() {
        if (this.EntityPM != null && this.EntityPM.WorkFlowVersionNumber) {
            return this.EntityPM.WorkFlowVersionNumber.toString();
        }
        return "";
    }
}
