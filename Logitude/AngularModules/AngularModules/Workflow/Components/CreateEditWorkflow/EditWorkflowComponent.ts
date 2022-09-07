import { Component } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';

@Component({
    templateUrl: './EditWorkflowComponent.html',
})

export class EditWorkflowComponent extends BaseComponent {
    public EntityPM: WorkFlowPM;
    public ObjectTableName: string = "WorkFlow";
    public DataContext: EditWorkflowComponent = this;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
    }

    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }

    get OwnerId() { return this.EntityPM.OwnerId; }
    set OwnerId(value: string) {
        if (this.EntityPM.OwnerId != value) {
            this.EntityPM.OwnerId = value;
        }
    }
}