import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { DateTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { Validator } from "Infrastructure/Validators/Validator";
import { WorkFlowVersionPM } from "Workflow/EntityPMs/WorkFlowVersionPM";
import { WorkFlowVersionPMService } from "Workflow/Services/StandardPMs/WorkFlowVersionPMService";

@Component({
    templateUrl: "./CreateWorkflowVersionComponent.html"
})

export class CreateWorkflowVersionComponent extends BaseComponent {

    public DataContext: any = this;
    public ValidationErrorsList: string[] = [];
    public CurrentSession = SessionLocator.SelectedSession;

    public ObjectTableName: string = "WorkFlowVersion";
    public WorkflowId: string;
    public FlowJson: string;
    public Entity: string;
    public Trigger: string;

    public VersionDescription: string

    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public BusyIndicatorWidth: number = 200;

    public WorkFlowVersionPMService: WorkFlowVersionPMService = new WorkFlowVersionPMService();

    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.WorkflowId = args['WorkflowId'];
        this.FlowJson = args['FlowJson'];
        this.Entity = args['Entity'];
        this.Trigger = args['Trigger'];
    }

    ngOnInit() {
        this.initializeWorkFlow();
    }

    initializeWorkFlow() {
        let todayDate: Date = DateTool.GetCurrentDateAsUtc();
        this.EntityPM = new WorkFlowVersionPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.WorkflowId = this.WorkflowId
        this.EntityPM.FlowJson = this.FlowJson;
        this.EntityPM.Entity = this.Entity;
        this.EntityPM.Trigger = this.Trigger;
        this.EntityPM.VersionNumber = 0;
    }


    updateVersionDescription(value: string) {
        this.VersionDescription = value;
        this.EntityPM.Description = value;
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        if (this.VersionDescription) {
            this.EntityPM.Description = this.VersionDescription;
            this.createWorkflowVersion();
        } else {
            this.ValidationErrorsList.push("Description is required");
        }
    }

    createWorkflowVersion() {
        this.startBusyIndicator("Saving ...");
        this.WorkFlowVersionPMService.insert(this.EntityPM).subscribe((serviceResponse: ServiceResponse) => {
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