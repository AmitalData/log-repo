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
    public IsNew: boolean;
    public VariableName: string = null;
    public VariableType: string = null;
    public VariableValue: string = null;
    public RecordType: string = null;
    public IsCollectionVariable: boolean = false;
    public ValidationErrorsList: string[];
    public VariableTypeChangedToggle: boolean = false;
    public DataTypesItems: ListItem[] = new DataTypesList().Items;
    public EntitiesTreeItems: TreeSelectItem[];
    public ExcludedEntities: string[] = ["Shipment.Container", "Shipment.ARInvoice", "Shipment.APInvoice"];
    public FieldTypes = FieldTypes;
    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
    }

    ngOnInit() {
        this.initializeWindowEvents();
        this.initialize();
        this.initializeEntitiesTreeItems();
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

        let variableTypeData = this.Data["variableType"];
        let variableValueData = this.Data["variableValue"];
        let recordTypeData = this.Data["recordType"];

        this.VariableName = this.Data["label"] || this.Data["name"] || this.Data["variableName"] || null;
        this.VariableType = this.formatVariableType(variableTypeData);
        this.VariableValue = variableValueData || null;
        this.RecordType = recordTypeData || null;

        if (variableTypeData && variableTypeData.toString().endsWith("[]")) {
            this.IsCollectionVariable = true;
        }

        this.setUIProperties();
    }

    initializeEntitiesTreeItems() {
        this.EntitiesTreeItems = new EntitiesTreeList("child").Items;
    }

    updateVariableName(variableName: string) {
        if (this.IsNew) {
            this.Data["name"] = variableName;
            this.Data["variableName"] = variableName;
            this.Data["variableCode"] = Formatter.getCodeFromName(variableName);
        }

        this.Data["label"] = variableName;
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
            this.updateRecordType(null);
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

    updateRecordType(recordType: string) {
        this.Data["recordType"] = recordType || null;
        this.RecordType = recordType || null;

        this.setUIProperties();
    }

    setUIProperties() {
        this.UIProperties.SetRequired("VariableName", null, AppTool.IsNullOrEmpty(this.VariableName));
        this.UIProperties.SetRequired("VariableType", null, AppTool.IsNullOrEmpty(this.VariableType));
        if (this.VariableType === FieldTypes.Record) {
            this.UIProperties.SetRequired("RecordType", null, AppTool.IsNullOrEmpty(this.RecordType));
        } else {
            this.UIProperties.SetRequired("RecordType", null, false);
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