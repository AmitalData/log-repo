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
    public ShowAdvancedSettings: boolean = false;
    
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

    get RetriesNumber() { return this.EntityPM.RetriesNumber }
    set RetriesNumber(value: number) {
        if (this.EntityPM.RetriesNumber != value) {
            this.EntityPM.RetriesNumber = value;
            this.setUIProperties()
        }
    }

    get RetriesDelay() { return this.EntityPM.RetriesDelay; }
    set RetriesDelay(value: number) {
        if (this.EntityPM.RetriesDelay != value) {
            this.EntityPM.RetriesDelay = value;
            this.setUIProperties()
        }
    }
    
    showAdvancedSettings(){
        this.ShowAdvancedSettings = true;
    }

    setUIProperties() {
        if (this.EntityPM.RetriesNumber > 10) {
            this.UIProperties.SetValidity("RetriesNumber", this.ObjectTableName, false, "The maximum number of retries is 10");
        } else {
            this.UIProperties.SetValidity("RetriesNumber", this.ObjectTableName, true, "");
        }

        if (this.EntityPM.RetriesDelay < 120) {
            this.UIProperties.SetValidity("RetriesDelay", this.ObjectTableName, false, "The minimum number of retries delay is 120");
        } else {
            this.UIProperties.SetValidity("RetriesDelay", this.ObjectTableName, true, "");
        }
    }
}
