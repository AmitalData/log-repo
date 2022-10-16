import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { SetValueOperators } from "Workflow/Constants/SetValueOperators";
import { FlowReader } from "Workflow/Models/FlowReader";
import { FlowVariablesTreeList } from "Workflow/Models/FlowVariablesTreeList";
import { ListItem } from "Workflow/Models/ListItem";
import { SetValue } from "Workflow/Models/SetValue";
import { SetValueOperatorsList } from "Workflow/Models/SetValueOperatorsList";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

@Component({
    templateUrl: "./SetValuePropertiesComponent.html"
})

export class SetValuePropertiesComponent extends BaseComponent {
    public DataContext: any = this;
    public Name: string = null;
    public SetValues: SetValue[];
    public ObjectFieldsDictionary: any = {};

    public Data: any;
    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowObjectFields: ObjectFieldPM[];
    public CurrentSession = SessionLocator.SelectedSession;

    public IsValidSetValue: boolean = true;
    public ValidationErrorsList: string[];

    public FlowVariablesTreeItems: TreeSelectItem[];
    public SetValuesOperatorsItems = new SetValueOperatorsList().Items;

    public ListItem = (itemCode: string) => { return new ListItem(itemCode) };

    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
        this.FlowObjectFields = args.FlowObjectFields ? args.FlowObjectFields : [];

        this.initialize();
    }

    ngOnInit() {
        this.initializeFlowVariablesTreeItems();
    }

    ngOnChanges() {
        this.SetValuesOperatorsItems = new SetValueOperatorsList().Items;
    }

    initialize() {
        this.Name = this.Data["name"] || null;
        this.SetValues = this.Data["setValues"] || [];

        this.initializeObjectFieldDictionary();
        this.initializeSetValue();

        this.setUIProperties();
    }

    initializeFlowVariablesTreeItems() {
        this.FlowVariablesTreeItems = new FlowVariablesTreeList(this.FlowObjectFields, this.FlowObject, this.CurrentNodeId).Items;
    }

    initializeObjectFieldDictionary() {
        if (this.SetValues.length > 0) {
            this.SetValues.forEach(setvalue => {
                this.fillObjectFieldDictionary(setvalue.field);
            });
        }
    }

    initializeSetValue() {
        if (this.SetValues.length == 0) {
            this.IsValidSetValue = false;
            let setValue = new SetValue();
            this.SetValues.push(setValue);
        }
    }

    updateName(name: any) {
        this.Data["name"] = name;
        this.Name = name;
        this.setUIProperties();
    }

    updateSetValueField(field: string, setValueIndex: number) {
        if (field !== this.SetValues[setValueIndex]?.value) {
            this.SetValues[setValueIndex].field = field ? field.toString() : null;
            this.SetValues[setValueIndex].operator = SetValueOperators.Equals;
            this.SetValues[setValueIndex].value = null;
            this.fillObjectFieldDictionary(field);
            this.SetValues[setValueIndex].type = field ? this.getFieldType(field) : null;
        }
        this.IsValidSetValue = this.isValidSetValue();
    }

    updateSetValueOperator(operatorCode: string, setValueIndex: number) {
        if (operatorCode !== this.SetValues[setValueIndex]?.operator) {
            this.SetValues[setValueIndex].operator = operatorCode;
            this.SetValues[setValueIndex].value = null;
        }
        this.IsValidSetValue = this.isValidSetValue();
    }

    updateSetValue(value: string, setValueIndex: number) {
        if (value !== this.SetValues[setValueIndex]?.value) {
            this.SetValues[setValueIndex].value = value;
        }
        this.IsValidSetValue = this.isValidSetValue();
    }

    addSetValue() {
        if (this.IsValidSetValue) {
            let setvalue = new SetValue();
            this.SetValues.push(setvalue);
        }
        this.IsValidSetValue = this.isValidSetValue();
    }

    deleteSetValue(setValueIndex: number) {
        let setvalue = this.SetValues[setValueIndex];
        if (setvalue) {
            this.SetValues.splice(setValueIndex, 1);
        }
        this.IsValidSetValue = this.isValidSetValue();
    }

    fillObjectFieldDictionary(field: string) {
        if (field && this.isObjectField(field)) {
            let objectfieldCode = field.split('_')[1]
            this.saveInObjectFieldsDictionary(objectfieldCode)
        }
    }

    saveInObjectFieldsDictionary(objectfieldCode: string) {
        if (!this.ObjectFieldsDictionary[objectfieldCode]) {
            var objectfield = this.FlowObjectFields.find(e => e.FieldCode == objectfieldCode)
            this.ObjectFieldsDictionary[objectfieldCode] = objectfield;
        }
    }

    isFieldCompareOperator(operatorCode: string) {
        return operatorCode && operatorCode.endsWith("<field>");
    }

    isObjectField(field: string) {
        let parent = field.split('_')[0]
        return parent != "declaredvariables";
    }

    isValidSetValue() {
        let result = true;
        for (let setValues of (this.SetValues)) {
            if (!setValues.field || !setValues.value || !setValues.operator) {
                result = false;
                break;
            }
        }
        return result;
    }

    getDeclareVariableType(field: string) {
        let fieldCode = field.split('_')[1];
        let declareVariableNode = FlowReader.getNodes(this.FlowObject, "declareVariableNode").find((n: any) => n.data["variableCode"] === fieldCode);
        return declareVariableNode ? declareVariableNode.data["variableType"] : null;
    }

    getObjectFieldCode(field: string) {
        let objectfieldCode = field.split('_')[1];
        return objectfieldCode;
    }

    getFieldType(field: string) {
        if (this.isObjectField(field)) {
            let object: ObjectFieldPM = this.ObjectFieldsDictionary[this.getObjectFieldCode(field)]
            return object.DataTypeCode
        } else {
            return this.getDeclareVariableType(field);
        }
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Name", null, AppTool.IsNullOrEmpty(this.Name));
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        if (notValidUIProperties.length === 0 && this.IsValidSetValue) {
            this.setValuesData();

            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors

            if (!this.IsValidSetValue)
                this.ValidationErrorsList.push("Invalid SetValues");
        }
    }

    setValuesData() {
        this.Data["setValues"] = this.SetValues;
    }
}