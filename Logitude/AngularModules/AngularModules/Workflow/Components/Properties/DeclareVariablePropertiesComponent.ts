import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { AppTool, FormatTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { DataTypesList } from "Workflow/Models/DataTypesList";
import { EntitiesTreeList } from "Workflow/Models/EntitiesTreeList";
import { Formatter } from "Workflow/Models/Formatter";
import { ListItem } from "Workflow/Models/ListItem";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

@Component({
    templateUrl: "./DeclareVariablePropertiesComponent.html"
})

export class DeclareVariablePropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public Data: any;
    public VariableName: string = null;
    public VariableType: string = null;
    public VariableValue: string = null;
    public RecordObject: string = null;
    public IsCollectionVariable: boolean = false;
    public ValidationErrorsList: string[];
    public IsNew: boolean;
    public VariableTypeChangedToggle: boolean = false;
    public DataTypesItems: ListItem[] = new DataTypesList().Items;
    public EntitiesTreeItems: TreeSelectItem[];
    public ExcludedEntities: string[] = ["Shipment.Container", "Shipment.ARInvoice", "Shipment.APInvoice"];
    public FieldTypes = FieldTypes;
    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};

        this.initialize();
        this.initializeEntitiesTreeItems();
    }

    initialize() {
        this.IsNew = Object.keys(this.Data).length === 0;

        let variableNameData = this.Data["variableName"];
        let variableTypeData = this.Data["variableType"];
        let variableValueData = this.Data["variableValue"];
        let recordObjectData = this.Data["recordObject"];

        this.VariableName = variableNameData || null;
        this.VariableType = this.formatVariableType(variableTypeData);
        this.VariableValue = variableValueData || null;
        this.RecordObject = recordObjectData || null;

        if (variableTypeData && variableTypeData.toString().endsWith("[]")) {
            this.IsCollectionVariable = true;
        }

        this.setUIProperties();
    }

    initializeEntitiesTreeItems() {
        this.EntitiesTreeItems = new EntitiesTreeList("child").Items;
    }

    updateVariableName(variableName: string) {
        this.Data["name"] = variableName;
        this.Data["variableName"] = variableName;
        this.Data["variableCode"] = Formatter.getCodeFromName(variableName);
        this.VariableName = variableName;

        this.setUIProperties();
    }

    updateVariableType(type: string) {
        if (type) {
            if (this.IsCollectionVariable) {
                type = type + "[]";
            } else {
                type = type.replace("[]", "");
            }
        }

        this.updateVariableValue(null);
        if (this.VariableType !== FieldTypes.Record) {
            this.updateRecordObject(null);
        }

        this.Data["variableType"] = type;
        this.VariableType = this.formatVariableType(type);
        this.VariableTypeChangedToggle = !this.VariableTypeChangedToggle;

        this.setUIProperties();
    }

    updateIsCollectionVariable(isCollection: boolean) {
        this.IsCollectionVariable = isCollection;
        this.updateVariableType(this.VariableType);

        this.setUIProperties();
    }

    updateVariableValue(value: string) {
        this.Data["variableValue"] = value || null;
        this.VariableValue = value || null;

        this.setUIProperties();
    }

    updateRecordObject(recordObject: string) {
        this.Data["recordObject"] = recordObject || null;
        this.RecordObject = recordObject || null;

        this.setUIProperties();
    }

    setUIProperties() {
        this.UIProperties.SetRequired("VariableName", null, AppTool.IsNullOrEmpty(this.VariableName));
        this.UIProperties.SetRequired("VariableType", null, AppTool.IsNullOrEmpty(this.VariableType));
        if (this.VariableType === FieldTypes.Record) {
            this.UIProperties.SetRequired("RecordObject", null, AppTool.IsNullOrEmpty(this.RecordObject));
        } else {
            this.UIProperties.SetRequired("RecordObject", null, false);
        }
        if (!FormatTool.IsValidNameText(this.VariableName)) {
            let error = "Name Format is Invalid <Must start with Letters and can contain only '-' >"
            this.UIProperties.SetValidity("VariableName", null, AppTool.IsNullOrEmpty(this.VariableName), error);
        }
    }

    formatVariableType(type: string) {
        if (type) {
            return type.toString().endsWith("[]") ? type.toString().replace("[]", "") : type;
        }
        return null;
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        if (notValidUIProperties.length === 0) {
            //console.log(this.Data);
            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors
        }
    }
}