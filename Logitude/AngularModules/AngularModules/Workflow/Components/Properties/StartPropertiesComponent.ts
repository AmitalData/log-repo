import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ConditionGroupOperations } from "Workflow/Constants/ConditionGroupOperations";
import { Condition } from "Workflow/Models/Condition";

@Component({
    templateUrl: "./StartPropertiesComponent.html"
})

export class StartPropertiesComponent extends BaseComponent {

    public EntityId: string = null;
    public Trigger: string = null;

    public Conditions: Condition[];
    public ConditionsOperation: string;

    public ValidationErrorsList: string[];

    public CurrentSession = SessionLocator.SelectedSession;

    DataContext: any = this;
    Data: any;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};

        this.initialize();
    }

    initialize() {
        this.EntityId = this.Data["entityId"] || null;
        this.Trigger = this.Data["trigger"] || null;

        this.Conditions = this.Data["conditions"] || [];
        this.ConditionsOperation = this.Data["conditionsOperation"] || ConditionGroupOperations.And;

        this.setUIProperties();
    }

    initializeCondition() {
        let condition = new Condition();
        this.Conditions.push(condition);
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        if (notValidUIProperties.length === 0) {

            this.setConditionsData();

            //console.log(this.Data);

            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors;
        }
    }

    setConditionsData() {
        this.Data["conditions"] = this.Conditions;
        this.Data["conditionsOperation"] = this.Conditions.length === 0 ? null : this.ConditionsOperation;
    }

    updateEntity(entity: any) {
        this.Data["entityId"] = entity ? entity.Id : null;
        this.Data["entity"] = entity ? entity.Name : null;
        this.EntityId = entity ? entity.Id : null;
        this.setUIProperties();
    }

    updateTrigger(trigger: string) {
        this.Data["trigger"] = trigger;
        this.Trigger = trigger;
        this.setUIProperties();
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Object", null, AppTool.IsNullOrEmpty(this.EntityId));
        this.UIProperties.SetRequired("Trigger", null, AppTool.IsNullOrEmpty(this.Trigger));
    }

    getObjectTablesQueryFilters() {
        let apiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.addAdditionalFilter("Name", "Shipment", null, null, "Equals", false, false, false, "Text");
        return apiQueryFilters;
    }
}