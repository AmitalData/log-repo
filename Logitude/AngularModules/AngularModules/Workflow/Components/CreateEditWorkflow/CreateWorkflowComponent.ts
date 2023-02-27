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
    public EntityPM: WorkFlowPM;
    public ObjectTableName: string = "WorkFlow";
    public DataContext: CreateWorkflowComponent = this;
    public ValidationErrorsList: string[];

    public CurrentSession = SessionLocator.SelectedSession;

    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public BusyIndicatorWidth: number = 200;

    public ShowAdvancedSettings: boolean = false;
    public AdvancedSettingsTitle: string = "Show Advanced Settings";
    public RetriesDelayArray: string[]
    public RetriedDelayDictionary: number[] = [1, 5, 30, 90, 180]
    public MaxDifferenceTime: number = 1
    public MaxRetryNumber: number = 5
    public SelectedRetryNumber: number = 5

    public WorkFlowPMService: WorkFlowPMService = new WorkFlowPMService();

    constructor() {
        super();
    }

    ngOnInit() {
        this.initializeWorkFlow();
    }

    trackByFn(idx) {
        return idx;
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
        this.EntityPM.RetriesNumber = this.MaxRetryNumber;
        this.EntityPM.RetriesDelay = "1,5,30,90,180";
        this.RetriesDelayArray = this.EntityPM.RetriesDelay.split(',');
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
            this.resetRetriesDelay(value);
            this.setUIProperties()
        }
    }

    resetRetriesDelay(retriesNumber: number) {
        if (!retriesNumber || retriesNumber > this.MaxRetryNumber) {
            return;
        }
        let retriesDelayArray = this.EntityPM.RetriesDelay.split(',');
        var numberOfStoredDelaies = retriesDelayArray.length;
        if (numberOfStoredDelaies > retriesNumber) {
            retriesDelayArray = this.handleDecreaseRetryNumber(retriesDelayArray, retriesNumber, numberOfStoredDelaies);
        } else {
            retriesDelayArray = this.handleIncreaseRetryNumber(retriesDelayArray, retriesNumber, numberOfStoredDelaies);
        }
        this.RetriesDelayArray = retriesDelayArray;
        this.EntityPM.RetriesDelay = retriesDelayArray.join(',');
        this.SelectedRetryNumber = retriesNumber;
        this.ValidateRetryDelayDifference();
    }

    handleDecreaseRetryNumber(retriesDelayArray: string[], retriesNumber: number, numberOfStoredDelaies: number) {
        retriesDelayArray = retriesDelayArray.slice(0, retriesNumber);
        for (let i = retriesNumber; i < numberOfStoredDelaies; i++) {
            this.UIProperties.SetValidity("RetriesDelay" + i, this.ObjectTableName, true, "");
        }
        return retriesDelayArray;
    }

    handleIncreaseRetryNumber(retriesDelayArray: string[], retriesNumber: number, numberOfStoredDelaies: number) {
        for (let i = numberOfStoredDelaies; i < retriesNumber; i++) {
            var defaultDelay = this.RetriedDelayDictionary[i]
            retriesDelayArray.push(defaultDelay.toString())
        }
        return retriesDelayArray;
    }

    updateRetriesDelayValue(value, index) {
        if (this.RetriesDelayArray[index] != value) {
            this.updateRetriesDelay(index, value)
        }
    }

    updateRetriesDelay(index: number, value: number) {
        this.RetriesDelayArray[index] = value ? value.toString() : null;
        this.EntityPM.RetriesDelay = this.RetriesDelayArray.join(',');
        this.ValidateRetryDelayDifference();
    }

    showAdvancedSettings() {
        this.ShowAdvancedSettings = !this.ShowAdvancedSettings;
        this.AdvancedSettingsTitle = this.ShowAdvancedSettings ? "Hide Advanced Settings" : "Show Advanced Settings"
    }

    setUIProperties() {
        if (this.EntityPM.RetriesNumber == 0) {
            this.UIProperties.SetValidity("RetriesNumber", this.ObjectTableName, false, "Retries number field is required.")
        }
        else if (this.EntityPM.RetriesNumber && this.EntityPM.RetriesNumber > this.MaxRetryNumber) {
            this.UIProperties.SetValidity("RetriesNumber", this.ObjectTableName, false, "The maximum number of retries is " + this.MaxRetryNumber.toString());
        } else {
            this.UIProperties.SetValidity("RetriesNumber", this.ObjectTableName, true, "");
        }
    }

    ValidateRetryDelayDifference() {
        if (this.EntityPM.RetriesDelay && this.RetriesDelayArray.length > 0) {
            for (let i = 0; i < this.RetriesDelayArray.length; i++) {
                if (!Number(this.RetriesDelayArray[i])) {
                    this.UIProperties.SetValidity("RetriesDelay" + i, this.ObjectTableName, false, "Retry No. " + (i + 1) + " is required.")
                }
                else if (i != 0 && ((Number(this.RetriesDelayArray[i]) - Number(this.RetriesDelayArray[i - 1])) < this.MaxDifferenceTime)) {
                    this.UIProperties.SetValidity("RetriesDelay" + i, this.ObjectTableName, false, "Retry No. " + (i + 1) + " must be greater than Retry No. " + i + '.')
                } else {
                    this.UIProperties.SetValidity("RetriesDelay" + i, this.ObjectTableName, true, "");
                }
            }
        }
    }

    saveButtonClicked() {
        let errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.DataContext.ObjectTableName, errors);
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        let validationErrors = notValidUIProperties.map(t => { return t.ValidationError ? t.ValidationError.replace(/\_/gi, " ") : null });

        this.ValidationErrorsList = [];

        if (errors.length == 0 && validationErrors.length == 0) {
            this.createWorkflow();
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

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
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
