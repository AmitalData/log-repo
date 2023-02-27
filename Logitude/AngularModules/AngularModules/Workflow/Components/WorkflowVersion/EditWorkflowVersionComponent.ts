import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { WorkFlowVersionPM } from "Workflow/EntityPMs/WorkFlowVersionPM";
import { WorkFlowVersionPMService } from "Workflow/Services/StandardPMs/WorkFlowVersionPMService";

@Component({
    templateUrl: "./EditWorkflowVersionComponent.html"
})

export class EditWorkflowVersionComponent extends BaseComponent {

    public DataContext: any = this;
    public ValidationErrorsList: string[] = [];
    public CurrentSession = SessionLocator.SelectedSession;

    public ObjectTableName: string = "WorkFlowVersion";
    public WorkFlowVersion: WorkFlowVersionPM;

    public VersionDescription: string

    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public BusyIndicatorWidth: number = 200;

    public WorkFlowVersionPMService: WorkFlowVersionPMService = new WorkFlowVersionPMService();

    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.WorkFlowVersion = args['WorkFlowVersion'];
    }

    get Description() { return this.WorkFlowVersion.Description; }
    set Description(value: string) {
        if (this.WorkFlowVersion.Description != value) {
            this.WorkFlowVersion.Description = value;
        }
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        if (this.WorkFlowVersion.Description) {
            this.updateWorkflowVersion();
        } else {
            this.ValidationErrorsList.push("Description is required");
        }
    }

    updateWorkflowVersion() {
        this.startBusyIndicator("Saving ...");
        this.WorkFlowVersionPMService.update(this.WorkFlowVersion).subscribe((serviceResponse: ServiceResponse) => {
            if (serviceResponse) {
                this.stopBusyIndicator();
                if (!serviceResponse.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit(serviceResponse.Result);
                }
                else {
                    this.ValidationErrorsList = serviceResponse.ErrorsArray;
                }
            }
        });
    }

    startBusyIndicator(message: string) {
        this.BusyIndicatorText = message;
        this.ShowBusyIndicator = true;
    }

    stopBusyIndicator() {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }
}