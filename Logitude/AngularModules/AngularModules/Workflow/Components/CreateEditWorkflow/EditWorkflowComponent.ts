import { Component, OnInit } from '@angular/core';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';

@Component({
    templateUrl: "./EditWorkflowComponent.html"
})

export class EditWorkflowComponent implements OnInit {

    public Workflow: WorkFlowPM;

    constructor(public entityArgs: EntityArgs) {
        this.Workflow = this.entityArgs.EntityPM;
    }

    ngOnInit() {

    }

}