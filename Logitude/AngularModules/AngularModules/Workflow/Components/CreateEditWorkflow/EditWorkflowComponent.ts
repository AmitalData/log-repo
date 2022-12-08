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
        this.SetButtonStates()
        this.Listen();
    }

    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.entityArgs.EntityArgEventEmitter.subscribe(
                theMessage => {
                    if (theMessage == "workflowBuilderEdited") {
                        this.SetButtonStates();
                    }
                }
            );
        }
    }

    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
            this.SetButtonStates()
            this.entityArgs.SendMessage("workflowEdited");
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
            this.SetButtonStates()
            this.entityArgs.SendMessage("workflowEdited");
        }
    }

    get OwnerId() { return this.EntityPM.OwnerId; }
    set OwnerId(value: string) {
        if (this.EntityPM.OwnerId != value) {
            this.EntityPM.OwnerId = value;
            this.SetButtonStates()
            this.entityArgs.SendMessage("workflowEdited");
        }
    }

    saveWorkflow() {
        this.startBusyIndicator("Saving ...");
        this.WorkFlowPMService.update(this.EntityPM).subscribe((serviceResponse: ServiceResponse) => { this.handleUpdateWorkflowResponse(serviceResponse); });
    }

    public SetButtonStates() {
        this.IsSaveDisabled = true
        if ((!this.isWorkflowHasVersion()) || (this.isDraftVersion() && this.isWorkflowHasChanges())) {
            this.IsSaveDisabled = false
        }
        if (this.IsSaveDisabled) {
            this.EntityPM.IsDirty = false
        }
    }

    handleUpdateWorkflowResponse(serviceResponse: ServiceResponse,) {
        if (!serviceResponse.HasError) {
            this.EntityPM = serviceResponse.Result;
            this.stopBusyIndicator();
            this.SetButtonStates();
            this.entityArgs.SendMessage("workflowEdited");
        }
    }

    startBusyIndicator(message: string) {
        this.BusyIndicatorText = message;
        this.ShowBusyIndicator = true;
    }

    stopBusyIndicator() {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }

    isWorkflowHasVersion() {
        return this.EntityPM.WorkFlowActiveVersionId != null
    }

    isWorkflowHasChanges() {
        return this.EntityPM.IsDirty
    }

    isDraftVersion() {
        return this.EntityPM.WorkFlowVersionStatusCode == "DRFT"
    }
}
