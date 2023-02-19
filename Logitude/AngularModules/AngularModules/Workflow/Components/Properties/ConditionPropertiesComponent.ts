import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { AppTool } from "Infrastructure/Tools";
import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { Condition } from "Workflow/Models/Condition";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";
import { ObjectTableList } from "Infrastructure/EntityLists/ObjectTableList";
import { FlowReader } from "Workflow/Utilities/FlowReader";

@Component({
    templateUrl: "./ConditionPropertiesComponent.html"
})

export class ConditionPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public Data: any;
    public IsNew: boolean;
    public WorkflowEntity: string;
    public Name: string = null;
    public MetLabel: string = null;
    public OtherwiseLabel: string = null;
    public Conditions: Condition[];
    public ConditionsOperation: string;
    public IsValidConditions: boolean = true;
    public ValidationErrorsList: string[];
    public WorkflowEntityTable: ObjectTableList;
    public FlowObject: any;
    public CurrentNodeId: string;

    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.WorkflowEntity = args.WorkflowEntity ? args.WorkflowEntity : null;
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
    }

    ngOnInit() {
        this.initializeWindowEvents();
        this.initialize();
    }

    initializeWindowEvents() {
        this.CurrentSession.CurrentWindow.FooterButtonsClicked.subscribe((e: any) => {
            if (e === "submit") {
                this.saveButtonClicked();
            } else {
                this.cancelButtonClicked();
            }
        });
    }

    initialize() {
        this.IsNew = Object.keys(this.Data).length === 0;

        if (this.WorkflowEntity) {
            this.setWorkflowEntityTable();

            this.Name = this.Data["label"] || this.Data["name"] || null;

            this.MetLabel = this.Data["metLabel"] || "True";
            this.Data["metLabel"] = this.MetLabel;

            this.OtherwiseLabel = this.Data["otherwiseLabel"] || "False";
            this.Data["otherwiseLabel"] = this.OtherwiseLabel;

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
        if (this.IsNew) {
            this.Data["name"] = name;
        }

        this.Data["label"] = name;
        this.Name = name;

        this.setUIProperties();
    }

    updateMetLabel(metLabel: string) {
        this.Data["metLabel"] = metLabel;
        this.MetLabel = metLabel;

        this.setUIProperties();
    }

    updateOtherwiseLabel(otherwiseLabel: string) {
        this.Data["otherwiseLabel"] = otherwiseLabel;
        this.OtherwiseLabel = otherwiseLabel;

        this.setUIProperties();
    }

    UpdateIsValidConditions(isValidConditions: boolean) {
        this.IsValidConditions = isValidConditions;
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Name", null, AppTool.IsNullOrEmpty(this.Name));
        this.UIProperties.SetRequired("MetLabel", null, AppTool.IsNullOrEmpty(this.MetLabel));
        this.UIProperties.SetRequired("OtherwiseLabel", null, AppTool.IsNullOrEmpty(this.OtherwiseLabel));
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        if (this.WorkflowEntity) {
            this.ValidationErrorsList = [];
            let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
            let isValidName = !this.IsNew || !FlowReader.isNodeCodeExists(this.FlowObject, this.Name);
            if (notValidUIProperties.length === 0 && this.IsValidConditions && isValidName) {
                this.setConditionsData();
                //console.log(this.Data);
                this.CurrentSession.CurrentWindow.Close(this.Data);
            } else {
                let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
                this.ValidationErrorsList = validationErrors;

                if (!this.IsValidConditions)
                    this.ValidationErrorsList.push("Invalid Conditions");

                if (!isValidName) {
                    this.ValidationErrorsList.push("The Name Should be Unique.");
                }
            }
        }
    }

    setConditionsData() {
        this.Data["conditions"] = this.Conditions;
        this.Data["conditionsOperation"] = this.Conditions.length === 0 ? null : this.ConditionsOperation;
    }
}