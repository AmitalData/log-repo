import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { ObjectTablePM } from "Infrastructure/EntityPMs/ObjectTablePM";
import { GeneralDomainService } from "Infrastructure/Services/GeneralDomainService";
import { AppTool, FormatTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";

@Component({
    templateUrl: "./DeclareVariablePropertiesComponent.html"
})

export class DeclareVariablePropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public Data: any;
    public WorkflowEntity: string;
    public errors: string[];
    public VariableName: string = null;
    public VariableType: string = null;
    public VariableValue: string = null;
    public ValidationErrorsList: string[];
    public IsValidConditions: boolean = true;
    public WorkflowEntityTable: ObjectTablePM;
    DataTypeCollection: any[];
    private myService: GeneralDomainService;
    private types: any[] = [{ Code: "Text", Name: "Text" }, { Code: "Date", Name: "Date" }, { Code: "Number", Name: "Number" }, { Code: "Boolean", Name: "Boolean" }];

    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.WorkflowEntity = args.WorkflowEntity ? args.WorkflowEntity : null;

        this.initialize();
    }

    initialize() {
        if (this.WorkflowEntity) {

            this.setWorkflowEntityTable();

            this.VariableName = this.Data["VariableName"] || null;
            this.VariableType = this.Data["VariableType"] || null;
            this.VariableValue = this.Data["VariableValue"] || null;

            this.setUIProperties();
        }
        else {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push("No workflow entity selected in start event");
        }
    }

    setWorkflowEntityTable() {
        this.WorkflowEntityTable = (window as any).ObjectTables.filter((o: any) => o.Name === this.WorkflowEntity)[0];
        if (!this.WorkflowEntityTable) {
            this.WorkflowEntity = null;
            this.ValidationErrorsList.push("No woekflow entity selected in start event");
        }
    }

    updateVariableName(variableName: any) {
        this.Data["VariableName"] = variableName;
        this.VariableName = variableName;

        if (variableName) {
            this.Data["VariableCode"] = variableName.replaceAll(' ','')
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

    public get CustomFieldDataType() {
        var fieldDataType = this.types.filter(d => d.Code == this.VariableType)[0];
        return fieldDataType;
    }
    public set CustomFieldDataType(newValue: any) {
        this.VariableType = newValue.Code;
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        if (this.WorkflowEntity) {
            this.ValidationErrorsList = [];
            let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
            if (notValidUIProperties.length === 0 && AppTool.IsNullOrEmpty(this.errors)) {

                this.CurrentSession.CurrentWindow.Close(this.Data);
            } else {
                let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
                this.ValidationErrorsList = validationErrors

                if (!this.IsValidConditions)
                    this.ValidationErrorsList.push("Invalid Conditions");
            }
        }
    }
}