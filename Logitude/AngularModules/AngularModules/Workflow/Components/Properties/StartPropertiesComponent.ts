import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";
import { ApiQueryFiltersBuilder } from "Workflow/Models/ApiQueryFiltersBuilder";
import { Condition } from "Workflow/Models/Condition";

@Component({
    templateUrl: "./StartPropertiesComponent.html"
})

export class StartPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public Data: any;
    public Entity: string = null;
    public EntityId: string = null;
    public Trigger: string = null;
    public Conditions: Condition[];
    public ConditionsOperation: string;
    public CreateTrigger: string = "create";
    public ValidationErrorsList: string[];

    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};

        this.initialize();
    }

    initialize() {
        this.Entity = this.Data["entity"] || null;
        this.Trigger = this.Data["trigger"] || null;

        this.Conditions = this.Data["conditions"] || [];
        this.ConditionsOperation = this.Data["conditionsOperation"] || ConditionOperations.And;

        this.EntityId = this.getEntityId(this.Entity);
        this.setUIProperties();
    }

    initializeCondition() {
        if (this.Entity) {
            let condition = new Condition();
            this.Conditions.push(condition);
        }
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
        if (this.Data["entity"] !== entity?.Name) {
            this.Conditions = [];
            this.ConditionsOperation = ConditionOperations.And;
        }

        this.Data["entity"] = entity ? entity.Name : null;
        this.Entity = entity ? entity.Name : null;
        this.EntityId = this.getEntityId(entity ? entity.Name : null);
        this.setUIProperties();
    }

    updateTrigger(trigger: string) {
        this.Data["trigger"] = trigger;
        this.Trigger = trigger;

        if (trigger === this.CreateTrigger) {
            this.resetConditionsOperatorAndValue();
        }

        this.setUIProperties();
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Object", null, AppTool.IsNullOrEmpty(this.Entity));
        this.UIProperties.SetRequired("Trigger", null, AppTool.IsNullOrEmpty(this.Trigger));
    }

    getObjectTablesQueryFilters() {
        return ApiQueryFiltersBuilder.getObjectTablesApiQueryFilters("Shipment");
    }

    resetConditionsOperatorAndValue(conditions: Condition[] | null = null) {
        for (let condition of (conditions || this.Conditions)) {
            if (condition.operator === ConditionOperators.Changed) {
                condition.operator = ConditionOperators.Equals;
                condition.value = null;
            }
            if (condition.isGroup && condition.conditions && condition.conditions.length > 0) {
                this.resetConditionsOperatorAndValue(condition.conditions);
            }
        }
    }

    getEntityId(entity: string) {
        if (entity) {
            let entityObjectTable = (window as any).ObjectTables.filter((o: any) => o.Name === entity)[0];
            return entityObjectTable ? entityObjectTable.Id : null;
        }
        return null;
    }
}