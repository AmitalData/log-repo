import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { GetRecordsLimit} from "Workflow/Constants/GetRecordsLimit";
import { Condition } from "Workflow/Models/Condition";

@Component({
    templateUrl: "./GetRecordPropertiesComponent.html"
})

export class GetRecordPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public Data: any;
    public Name: string = null;
    public EntityId: string = null;
    public EntityName: string = null;
    public RecordsLimit: string = null;
    public Conditions: Condition[];
    public ConditionsOperation: string;

    public ValidationErrorsList: string[];
    public IsValidConditions: boolean = true;

    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.initialize();
    }

    initialize() {

        this.Name = this.Data["name"] || null;
        this.EntityId = this.Data["entityId"] || null;
        this.EntityName = this.Data["entityName"] || null;
        this.RecordsLimit = this.Data["recordsLimit"] ? this.Data["recordsLimit"] : GetRecordsLimit.FirstRecord

        this.Conditions = this.Data["conditions"] || [];
        this.ConditionsOperation = this.Data["conditionsOperation"] || ConditionOperations.And;

        if (this.Conditions.length == 0) {
            this.IsValidConditions = false;
            let condition = new Condition();
            this.Conditions.push(condition);
        }

        this.setUIProperties();
    }

    updateName(Name: any) {
        this.Data["name"] = Name;
        this.Name = Name;

        this.setUIProperties();
    }

    updateEntity(entity: any) {
        this.Data["entityId"] = entity ? entity.Id : null;
        this.Data["entityName"] = entity ? entity.Name : null;
        this.EntityId = entity ? entity.Id : null;
        this.EntityName = entity ? entity.Name : null;
        this.setUIProperties();
    }

    updateRecordsLimit(recordsLimit: string) {
        this.Data["recordsLimit"] = recordsLimit;
        this.RecordsLimit = recordsLimit;
    }

    getObjectTablesQueryFilters() {
        let apiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.addAdditionalFilter("Name", "Shipment", null, null, "Equals", false, false, false, "Text");
        return apiQueryFilters;
    }

    UpdateIsValidConditions(isValidConditions: boolean) {
        this.IsValidConditions = isValidConditions;
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Name", null, AppTool.IsNullOrEmpty(this.Name));
        this.UIProperties.SetRequired("Object", null, AppTool.IsNullOrEmpty(this.EntityId));
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        if (notValidUIProperties.length === 0 && this.IsValidConditions) {
            this.setConditionsData();

            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors;

            if (!this.IsValidConditions)
                this.ValidationErrorsList.push("Invalid Conditions");
        }
    }

    setConditionsData() {
        this.Data["conditions"] = this.Conditions;
        this.Data["conditionsOperation"] = this.Conditions.length === 0 ? null : this.ConditionsOperation;
    }
}