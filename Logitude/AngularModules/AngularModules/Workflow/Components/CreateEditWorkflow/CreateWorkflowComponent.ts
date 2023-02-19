import { Component, OnInit } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { DateTool } from 'Infrastructure/Tools';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { Validator } from 'Infrastructure/Validators/Validator';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';
import { WorkFlowPMService } from 'Workflow/Services/StandardPMs/WorkFlowPMService';

@Component({
    templateUrl: './CreateWorkflowComponent.html',
})

export class CreateWorkflowComponent extends BaseComponent implements OnInit {
    public IsNewEntity: boolean = true;
    public EntityPM: WorkFlowPM;
    public EntityId: string;
    public ObjectTableName: string = "WorkFlow";
    public DataContext: CreateWorkflowComponent = this;
    public ValidationErrorsList: string[];
    public ShowAdvancedSettings: boolean = false;

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
        this.ShowAdvancedSettings = !this.ShowAdvancedSettings;
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
        this.EntityPM.RetriesNumber = 5;
        this.EntityPM.RetriesDelay = 300;
    }

    loadWorkflow() {
        this.startBusyIndicator("Loading ...");
        this.WorkFlowPMService.get(this.EntityId).subscribe((serviceResponse: ServiceResponse) => {
            if (!serviceResponse.HasError) {
                this.EntityPM = serviceResponse.Result;
                this.stopBusyIndicator();
            }
        });
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    setUIProperties() {
        if (this.EntityPM.RetriesNumber && this.EntityPM.RetriesNumber > 10) {
            this.UIProperties.SetValidity("RetriesNumber", this.ObjectTableName, false, "The maximum number of retries is 10");
        } else {
            this.UIProperties.SetValidity("RetriesNumber", this.ObjectTableName, true, "");
        }

        if (this.EntityPM.RetriesDelay && this.EntityPM.RetriesDelay < 120) {
            this.UIProperties.SetValidity("RetriesDelay", this.ObjectTableName, false, "The minimum number of retries delay is 120");
        } else {
            this.UIProperties.SetValidity("RetriesDelay", this.ObjectTableName, true, "");
        }
    }

    saveButtonClicked() {
        let errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.DataContext.ObjectTableName, errors);
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        let validationErrors = notValidUIProperties.map(t => { return t.ValidationError ? t.ValidationError.replace(/\_/gi, " ") : null });

        this.ValidationErrorsList = [];

        if (errors.length == 0 && validationErrors.length == 0) {
            if (this.IsNewEntity) {
                this.createWorkflow();
            } else {
                this.editWorkflow();
            }
        } else {
            this.ValidationErrorsList = this.ValidationErrorsList.concat(errors).concat(validationErrors)
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

                    this.WorkFlowPMService.get(newEntity.Id).subscribe((serviceResponse: ServiceResponse) => {
                        if (!serviceResponse.HasError) {
                            var entitypm: WorkFlowPM
                            entitypm = serviceResponse.Result
                            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                                .then(cmpRef => {
                                    cmpRef.instance.ComponentRef = cmpRef;
                                    cmpRef.instance.Run({ EntityId: entitypm.Id, EntityPM: entitypm, ObjectTableName: 'WorkFlow', BackButtonLabel: "WorkFlows", IsFirstOpen: true });
                                });
                        }
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
