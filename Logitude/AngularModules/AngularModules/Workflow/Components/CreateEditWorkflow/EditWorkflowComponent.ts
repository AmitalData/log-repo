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
    public AdvancedSettingsTitle: string = "Show Advanced Settings";

    public RetriesDelayArray: string[]
    public RetriedDelayDictionary: number[] = [1, 5, 30, 90, 180]
    public MaxDifferenceTime: number = 1
    public MaxRetryNumber: number = 5

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.RetriesDelayArray = this.EntityPM.RetriesDelay.split(',');
    }

    trackByFn(idx) {
        return idx;
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
        if (this.EntityPM.RetriesNumber > this.MaxRetryNumber) {
            this.UIProperties.SetValidity("RetriesNumber", this.ObjectTableName, false, "The maximum number of retries is " + this.MaxRetryNumber);
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
}
