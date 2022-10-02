import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectTablePM } from "Infrastructure/EntityPMs/ObjectTablePM";
import { AppTool, FormatTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { DataTypesList } from "Workflow/Models/DataTypesList";
import { ListItem } from "Workflow/Models/ListItem";

@Component({
    templateUrl: "./DeclareVariablePropertiesComponent.html"
})

export class DeclareVariablePropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public Data: any;
    public WorkflowEntity: string;
    public VariableName: string = null;
    public VariableType: string = null;
    public VariableValue: string = null;
    public ValidationErrorsList: string[];
    public IsValidConditions: boolean = true;
    public WorkflowEntityTable: ObjectTablePM;
    public IsNew: boolean = true;

    public DataTypesItems: ListItem[] = new DataTypesList().Items;

    public ListItem = (itemCode: string) => { return new ListItem(itemCode) };

    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};

        this.initialize();
    }

    initialize() {
        this.VariableName = this.Data["VariableName"] || null;
        this.VariableType = this.Data["VariableType"] || null;
        this.VariableValue = this.Data["VariableValue"] || null;
        this.IsNew = this.Data["VariableType"] ? false : true;

        this.setUIProperties();
    }

    updateVariableName(variableName: string) {
        this.Data["VariableName"] = variableName;
        this.VariableName = variableName;

        if (variableName) {
            this.Data["VariableCode"] = variableName.replace(/\s/g, '').trim().toLowerCase();
        }
        this.setUIProperties();
    }

    updateVariableType(VariableType: any) {
        this.Data["VariableType"] = VariableType;
        this.VariableType = VariableType;

        this.Data["VariableValue"] = "";
        this.VariableValue = "";

        this.setUIProperties();
    }

    updateVariableValue(VariableValue: any) {
        this.Data["VariableValue"] = VariableValue;
        this.VariableValue = VariableValue;

        this.setUIProperties();
    }

    setUIProperties() {
        this.UIProperties.SetRequired("VariableName", null, AppTool.IsNullOrEmpty(this.VariableName));
        this.UIProperties.SetRequired("VariableType", null, AppTool.IsNullOrEmpty(this.VariableType));
        if (!FormatTool.IsValidNameText(this.VariableName)) {
            let error = "Name Format is Invalid <Must start with Letters and can contain only '-' >"
            this.UIProperties.SetValidity("VariableName", null, AppTool.IsNullOrEmpty(this.VariableName), error);
        }
    }

    DataTypeSelectionMethod(fieldDataType: any) {
        if (fieldDataType != null) {
            this.updateVariableType(fieldDataType.Code)
        }
    }

    GetSelectedDataType() {
        return this.DataTypesItems.filter(i => i.Code == this.VariableType)[0];
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        if (notValidUIProperties.length === 0) {

            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors

            if (!this.IsValidConditions)
                this.ValidationErrorsList.push("Invalid Conditions");
        }
    }
}