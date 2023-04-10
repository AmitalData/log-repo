import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { AppTool, DateTool } from 'Infrastructure/Tools';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';
import { WorkFlowPMService } from 'Workflow/Services/StandardPMs/WorkFlowPMService';

@Component({
    selector: "CreateEditWorkflow",
    templateUrl: "./CreateEditWorkflowComponent.html"
})

export class CreateEditWorkflowComponent extends BaseComponent implements OnInit {

    @Input() Workflow: WorkFlowPM | null = null;

    public IsNew: boolean;
    public ObjectTableName: string = "WorkFlow";
    public DataContext = this;
    public ValidationErrors: string[];
    public ShowAdvancedSettings: boolean = false;
    public AdvancedSettingsTitle: string = "Show Advanced Settings";
    public MaxRetriesNumber: number = 5;
    public DefaultRetriesDelay: string[] = ["1", "5", "30", "90", "180"];
    public RetriesDelayArray: { Value: string }[] = [];
    public RetriesDelayFieldCodePrefix = "RetriesDelay";
    public RetriesDelayFieldLabelPrefix = "Retry No. ";
    public RetriesDelaySplitter = ",";
    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public BusyIndicatorWidth: number = 200;
    public WorkFlowPMService: WorkFlowPMService = new WorkFlowPMService();
    public CurrentSession = SessionLocator.SelectedSession;
    public RecordTriggered: string = "RecordTriggered";

    constructor() {
        super();
    }

    ngOnInit() {
        this.IsNew = this.Workflow === null;

        this.initialize();
        this.setRetriesDelayArray();
    }

    initialize() {
        if (this.IsNew) {
            let todayDate: Date = DateTool.GetCurrentDateAsUtc();
            this.EntityPM = new WorkFlowPM();
            this.EntityPM.Name = "";
            this.EntityPM.Tenant = SessionLocator.Tenant;
            this.EntityPM.CreateDate = todayDate;
            this.EntityPM.UpdateDate = todayDate;
            this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            this.EntityPM.StatusCode = "DRFT";
            this.EntityPM.RetriesNumber = this.MaxRetriesNumber;
            this.EntityPM.RetriesDelay = this.DefaultRetriesDelay.join(this.RetriesDelaySplitter);
            this.EntityPM.WorkFlowTriggerTypeCode = this.RecordTriggered
        } else {
            this.EntityPM = this.Workflow;
        }
    }

    setRetriesDelayArray() {
        this.RetriesDelayArray = this.RetriesDelay ? this.RetriesDelay.split(this.RetriesDelaySplitter).map(r => { return { Value: r }; }) : [];
    }

    get Name() { return this.EntityPM.Name; }
    set Name(name: string) {
        if (this.EntityPM.Name != name) {
            this.EntityPM.Name = name;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(description: string) {
        if (this.EntityPM.Description != description) {
            this.EntityPM.Description = description;
        }
    }

    get RetriesNumber() { return this.EntityPM.RetriesNumber }
    set RetriesNumber(retriesNumber: number) {
        if (this.EntityPM.RetriesNumber != retriesNumber) {
            this.EntityPM.RetriesNumber = retriesNumber;
            this.validateRetriesNumber();
            this.handleRetriesNumberChange();
            this.setRetriesDelayArray();
        }
    }

    get RetriesDelay() { return this.EntityPM.RetriesDelay }
    set RetriesDelay(retriesDelay: string) {
        if (this.EntityPM.RetriesDelay != retriesDelay) {
            this.EntityPM.RetriesDelay = retriesDelay;
            this.validateRetriesDelay();
        }
    }

    get WorkFlowTriggerTypeCode() { return this.EntityPM.WorkFlowTriggerTypeCode }
    set WorkFlowTriggerTypeCode(trigger: string) {
        if (this.EntityPM.WorkFlowTriggerTypeCode != trigger) {
            this.EntityPM.WorkFlowTriggerTypeCode = trigger;
        }
    }

    updateRetriesDelay(retryDelay: string, index: number) {
        this.RetriesDelayArray[index].Value = retryDelay !== null && retryDelay !== undefined && retryDelay !== "" ? retryDelay : "0";
        this.RetriesDelay = this.RetriesDelayArray.map(r => { return r.Value; }).join(this.RetriesDelaySplitter);
    }

    handleRetriesNumberChange() {
        if (this.RetriesNumber && this.RetriesNumber > 0 && this.RetriesNumber <= this.MaxRetriesNumber) {
            let retries = this.RetriesDelay ? this.RetriesDelay.split(this.RetriesDelaySplitter) : [];
            if (this.RetriesNumber < retries.length) {
                this.handleDecreaseRetriesNumber(retries);
            } else if (this.RetriesNumber > retries.length) {
                this.handleIncreaseRetriesNumber(retries);
            }
        } else {
            this.RetriesDelay = null;
        }
    }

    handleDecreaseRetriesNumber(retries: string[]) {
        retries = retries.slice(0, (this.RetriesNumber - retries.length));
        this.RetriesDelay = retries.join(this.RetriesDelaySplitter);
    }

    handleIncreaseRetriesNumber(retries: string[]) {
        let retriesOldCount = retries.length;
        let newRetries = Array((this.RetriesNumber - retries.length)).fill("0");
        retries = retries.concat(newRetries);
        retries.forEach((_retry, index) => {
            if (index >= retriesOldCount) { retries[index] = this.DefaultRetriesDelay[index] || "0"; }
        });
        this.RetriesDelay = retries.join(this.RetriesDelaySplitter);
    }

    validateRetriesNumber() {
        this.UIProperties.SetValidity("RetriesNumber", this.ObjectTableName, true, "");
        if (this.RetriesNumber > this.MaxRetriesNumber) {
            this.UIProperties.SetValidity("RetriesNumber", this.ObjectTableName, false, "The maximum number of retries is " + this.MaxRetriesNumber);
        }
    }

    validateRetriesDelay() {
        this.resetRetriesDelayUIProperties();
        if (this.RetriesDelay) {
            let retries = this.RetriesDelay.split(this.RetriesDelaySplitter);
            retries.forEach((retryDelay, index) => {
                this.setDefaultRetriesDelayUIProperties(index);

                if (AppTool.IsNullOrEmpty(this.getDelay(retryDelay))) {
                    this.setRetryDelayFieldRequired(index);
                }
                else {
                    this.validateFirstRetryDelay(index, retries);
                    this.validateLastRetryDelay(index, retries);
                    this.validateBetweenRetryDelay(index, retries);
                }
            });
        }
    }

    resetRetriesDelayUIProperties() {
        this.UIProperties.UIPropertyList.filter(x => x.FieldName.startsWith(this.RetriesDelayFieldCodePrefix)).forEach(element => {
            element.IsRequired = false;
            element.ValidValue = true;
            element.ValidationError = "";
        });
    }

    setDefaultRetriesDelayUIProperties(index: number) {
        let fieldCode = this.getRetriesDelayField(index);
        let fieldLabel = this.getRetriesDelayField(index, true);
        this.UIProperties.SetRequired(fieldCode, null, false, fieldLabel);
        this.UIProperties.SetValidity(fieldCode, null, true, "");
    }

    setRetryDelayFieldRequired(index: number) {
        let fieldCode = this.getRetriesDelayField(index);
        let fieldLabel = this.getRetriesDelayField(index, true);
        this.UIProperties.SetRequired(fieldCode, null, true, fieldLabel);
    }

    validateFirstRetryDelay(index: number, retries: string[]) {
        if (index === 0) {
            let delay = this.getDelay(retries[index]);
            let nextDelay = this.getNextDelay(index, retries);
            let notValid = delay < 1 || (nextDelay !== null && delay >= nextDelay);
            if (notValid) {
                let fieldCode = this.getRetriesDelayField(index);
                let fieldLabel = this.getRetriesDelayField(index, true);
                let nextFieldLabel = this.getNextRetriesDelayFieldLabel(index, retries);
                let error = delay < 1 ?
                    "Delay of " + fieldLabel + " should be at least 1" :
                    "Delay of " + fieldLabel + " should be less than delay of " + nextFieldLabel;
                this.UIProperties.SetValidity(fieldCode, null, false, error);
            }
        }
    }

    validateLastRetryDelay(index: number, retries: string[]) {
        if (index === (retries.length - 1)) {
            let delay = this.getDelay(retries[index]);
            let previousDelay = this.getPreviousDelay(index, retries);
            let notValid = previousDelay !== null && delay <= previousDelay;
            if (notValid) {
                let fieldCode = this.getRetriesDelayField(index);
                let fieldLabel = this.getRetriesDelayField(index, true);
                let previousFieldLabel = this.getPreviousRetriesDelayFieldLabel(index);
                let error = "Delay of " + fieldLabel + " should be greater than delay of " + previousFieldLabel;
                this.UIProperties.SetValidity(fieldCode, null, false, error);
            }
        }
    }

    validateBetweenRetryDelay(index: number, retries: string[]) {
        if (index > 0 && index < (retries.length - 1)) {
            let delay = this.getDelay(retries[index]);
            let previousDelay = this.getPreviousDelay(index, retries);
            let nextDelay = this.getNextDelay(index, retries);
            let notValid = (previousDelay !== null && delay <= previousDelay) || (nextDelay !== null && delay >= nextDelay);
            if (notValid) {
                let fieldCode = this.getRetriesDelayField(index);
                let fieldLabel = this.getRetriesDelayField(index, true);
                let previousFieldLabel = this.getPreviousRetriesDelayFieldLabel(index);
                let nextFieldLabel = this.getNextRetriesDelayFieldLabel(index, retries);
                let error = delay <= previousDelay ?
                    "Delay of " + fieldLabel + " should be greater than delay of " + previousFieldLabel :
                    "Delay of " + fieldLabel + " should be less than delay of " + nextFieldLabel;
                this.UIProperties.SetValidity(fieldCode, null, false, error);
            }
        }
    }

    getPreviousRetriesDelayFieldLabel(index: number) {
        return index > 0 ? this.getRetriesDelayField(index - 1, true) : null;
    }

    getNextRetriesDelayFieldLabel(index: number, retries: string[]) {
        return index < (retries.length - 1) ? this.getRetriesDelayField(index + 1, true) : null;
    }

    getRetriesDelayField(index: number, isLabel: boolean = false) {
        return !isLabel ? (this.RetriesDelayFieldCodePrefix + index) : (this.RetriesDelayFieldLabelPrefix + (index + 1));
    }

    getPreviousDelay(index: number, retries: string[]) {
        return index > 0 ? this.getDelay(retries[index - 1]) : null;
    }

    getNextDelay(index: number, retries: string[]) {
        return index < (retries.length - 1) ? this.getDelay(retries[index + 1]) : null;
    }

    getDelay(retryDelay: string) {
        let delay = AppTool.IsNullOrEmpty(retryDelay) ? 0 : Number(retryDelay);
        return delay === 0 ? null : delay;
    }

    create() {
        if (this.IsNew) {
            let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrors = validationErrors;
            if (this.ValidationErrors.length == 0) {
                this.completeCreateWorkflow();
            }
        }
    }

    completeCreateWorkflow() {
        this.startBusyIndicator();
        this.WorkFlowPMService.insert(this.EntityPM).subscribe((createServiceResponse: ServiceResponse) => {
            this.handleCreateWorkflowResponse(createServiceResponse);
        });
    }

    handleCreateWorkflowResponse(createServiceResponse: ServiceResponse) {
        if (createServiceResponse && !createServiceResponse.HasError) {
            this.WorkFlowPMService.get(createServiceResponse.Result.Id).subscribe((getServiceResponse: ServiceResponse) => {
                this.handleGetWorkflowResponse(getServiceResponse);
            });
        }
        else {
            this.handleServiceResponseError(createServiceResponse);
        }
    }

    handleGetWorkflowResponse(getServiceResponse: ServiceResponse) {
        if (getServiceResponse && !getServiceResponse.HasError) {
            this.stopBusyIndicator();
            this.CurrentSession.CloseCurrentWindowEmit("ok");

            SessionLocator.DynamicLoader
                .Load("./Infrastructure/Components/EditComponent/EditComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                .then((component: any) => {
                    component.instance.ComponentRef = component;
                    component.instance.Run({
                        EntityId: getServiceResponse.Result.Id,
                        EntityPM: getServiceResponse.Result,
                        ObjectTableName: "WorkFlow",
                        BackButtonLabel: "Workflows",
                        IsFirstOpen: true
                    });
                });
        } else {
            this.handleServiceResponseError(getServiceResponse);
        }
    }

    handleServiceResponseError(serviceResponse: ServiceResponse) {
        this.ValidationErrors = serviceResponse ? serviceResponse.ErrorsArray : [];
        this.stopBusyIndicator();
    }

    cancel() {
        this.CurrentSession.CloseCurrentWindow();
    }

    showAdvancedSettings() {
        this.ShowAdvancedSettings = !this.ShowAdvancedSettings;
        this.AdvancedSettingsTitle = this.ShowAdvancedSettings ? "Hide Advanced Settings" : "Show Advanced Settings"
    }

    startBusyIndicator(message: string = "Saving ...") {
        this.BusyIndicatorText = message;
        this.ShowBusyIndicator = true;
    }

    stopBusyIndicator() {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }
}