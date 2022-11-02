import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { AppTool } from "Infrastructure/Tools";
import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { Condition } from "Workflow/Models/Condition";
import { ObjectTablePM } from "Infrastructure/EntityPMs/ObjectTablePM";
import { ObjectTables } from "Workflow/Models/ObjectTables";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";

@Component({
    templateUrl: "./ConditionPropertiesComponent.html"
})

export class ConditionPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public Data: any;
    public WorkflowEntity: string;

    public Name: string = null;
    public ConditionLabel:  string = null;
    public Conditions: Condition[];
    public ConditionsOperation: string;
    public IsValidConditions: boolean = true;
    public ValidationErrorsList: string[];
    public WorkflowEntityTable: ObjectTablePM;

    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowObjectFields: ObjectFieldList[];
    
    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.WorkflowEntity = args.WorkflowEntity ? args.WorkflowEntity : null;
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
        this.FlowObjectFields = args.FlowObjectFields ? args.FlowObjectFields : [];
        
        this.initialize();
    }

    initialize() {
        if (this.WorkflowEntity) {
            this.setWorkflowEntityTable();

            this.Name = this.Data["name"] || null;
            this.ConditionLabel = this.Data["conditionLabel"] || null;
    
            this.Conditions = this.Data["conditions"] || [];
            this.ConditionsOperation = this.Data["conditionsOperation"] || ConditionOperations.And;
    
            if (this.Conditions.length == 0) {
                this.IsValidConditions = false;
                let condition = new Condition();
                this.Conditions.push(condition);
            }
    
            this.setUIProperties();
        }
        else {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push("No workflow entity selected in start event");
        }
    }

    setWorkflowEntityTable() {
        this.WorkflowEntityTable = ObjectTables.getByName(this.WorkflowEntity);
        if (!this.WorkflowEntityTable) {
            this.WorkflowEntity = null;
            this.ValidationErrorsList.push("No workflow entity selected in start event");
        }
    }

    updateName(name: string) {
        this.Data["name"] = name;
        this.Name = name;

        this.setUIProperties();
    }

    updateConditionLabel(conditionLabel: string) {
        this.Data["conditionLabel"] = conditionLabel;
        this.ConditionLabel = conditionLabel;

        this.setUIProperties();
    }

    UpdateIsValidConditions(isValidConditions: boolean) {
        this.IsValidConditions = isValidConditions;
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Name", null, AppTool.IsNullOrEmpty(this.Name));
        this.UIProperties.SetRequired("ConditionLabel", null, AppTool.IsNullOrEmpty(this.ConditionLabel));
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        if (this.WorkflowEntity) {
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
    }

    setConditionsData() {
        this.Data["conditions"] = this.Conditions;
        this.Data["conditionsOperation"] = this.Conditions.length === 0 ? null : this.ConditionsOperation;
    }
}