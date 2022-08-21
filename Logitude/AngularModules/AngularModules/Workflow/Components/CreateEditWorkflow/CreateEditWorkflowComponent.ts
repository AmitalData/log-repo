import { Component, OnInit } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { AppTool, DateTool } from 'Infrastructure/Tools';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { Validator } from 'Infrastructure/Validators/Validator';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';
import { WorkFlowPMService } from 'Workflow/Services/StandardPMs/WorkFlowPMService';

@Component({
    templateUrl: './CreateEditWorkflowComponent.html',
})

export class CreateEditWorkflowComponent extends BaseComponent implements OnInit {
    public IsNewEntity: boolean = true;
    public EntityPM: WorkFlowPM;
    public EntityId: string;
    public ObjectTableName: string = "WorkFlow";
    public DataContext: CreateEditWorkflowComponent = this;
    public ValidationErrorsList: string[];

    public CurrentSession = SessionLocator.SelectedSession;

    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public BusyIndicatorWidth: number = 200;

    public WorkFlowPMService: WorkFlowPMService = new WorkFlowPMService();

    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.EntityId = args['EntityId'];
        this.IsNewEntity = args['IsNewEntity'];
    }

    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
            //this.setUIProperties();
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

    ngOnInit() {
        if (this.IsNewEntity) {
            this.initializeWorkFlow();
        } else {
            this.loadWorkflow();
        }
    }

    initializeWorkFlow() {
        let todayDate: Date = DateTool.GetCurrentDateAsUtc();
        this.EntityPM = new WorkFlowPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.StatusCode = "DRFT";
        this.EntityPM.FlowJson = "";
        //this.setUIProperties();
    }

    loadWorkflow() {
        this.startBusyIndicator("Loading ...");
        this.WorkFlowPMService.get(this.EntityId).subscribe((serviceResponse: ServiceResponse) => {
            if (!serviceResponse.HasError) {
                this.EntityPM = serviceResponse.Result;
                this.stopBusyIndicator();
                //this.setUIProperties();
            }
        });
    }

    // setUIProperties() {
    //     this.UIProperties.SetRequired("Name", this.ObjectTableName, AppTool.IsNullOrEmpty(this.Name));
    // }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        let errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.DataContext.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.IsNewEntity) {
                this.createWorkflow();
            } else {
                this.editWorkflow();
            }
        }
    }

    createWorkflow() {
        this.startBusyIndicator("Saving ...");
        this.WorkFlowPMService.insert(this.EntityPM).subscribe((serviceResponse: ServiceResponse) => {
            if (serviceResponse) {
                this.stopBusyIndicator();
                if (!serviceResponse.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit("ok");

                    let newEntity = serviceResponse.Result;
                    SessionLocator.DynamicLoader.Load("./Workflow/Components/WorkflowBuilder/WorkflowBuilderComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({
                                ObjectTableName: 'WorkFlow',
                                EntityId: newEntity.Id
                            });
                        });
                }
                else {
                    this.ValidationErrorsList = serviceResponse.ErrorsArray;
                }
            }
        });
    }

    editWorkflow() {
        this.startBusyIndicator("Saving ...");
        this.WorkFlowPMService.update(this.EntityPM).subscribe((serviceResponse: ServiceResponse) => {
            if (serviceResponse) {
                this.stopBusyIndicator();
                if (!serviceResponse.HasError) {
                    this.CurrentSession.CurrentWindow.Close(serviceResponse.Result);
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