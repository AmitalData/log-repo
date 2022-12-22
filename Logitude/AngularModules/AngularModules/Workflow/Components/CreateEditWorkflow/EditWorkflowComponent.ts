import { Component } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';
import { WorkFlowPMService } from 'Workflow/Services/StandardPMs/WorkFlowPMService';

@Component({
    templateUrl: './EditWorkflowComponent.html',
})

export class EditWorkflowComponent extends BaseComponent {
    public EntityPM: WorkFlowPM;
    public ObjectTableName: string = "WorkFlow";
    public DataContext: EditWorkflowComponent = this;
    public WorkFlowPMService: WorkFlowPMService = new WorkFlowPMService();;
    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public IsSaveDisabled: boolean = true;
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
