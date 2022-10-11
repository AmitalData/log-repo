import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { SetValueOperators } from "Workflow/Constants/SetValueOperators";
import { FlowVariablesTreeList } from "Workflow/Models/FlowVariablesTreeList";
import { ListItem } from "Workflow/Models/ListItem";
import { SetValue } from "Workflow/Models/SetValue";
import { SetValueOperatorsListDictionary } from "Workflow/Models/SetValueOperatorsListDictionary";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";


@Component({
    templateUrl: "./SetValuePropertiesComponent.html"
})

export class SetValuePropertiesComponent extends BaseComponent {
    public DataContext: any = this;
    public Data: any;
    public Label: string = null;
    public SetValuesCounter: number = 1;
    public SetValues: SetValue[];

    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowObjectFields: ObjectFieldPM[];
    public CurrentSession = SessionLocator.SelectedSession;

    public IsValidSetValue: boolean = true;
    public ValidationErrorsList: string[];

    public FlowVariablesTreeItems: TreeSelectItem[];
    public SetValuesOperatorsItemsDictionary = new SetValueOperatorsListDictionary().ItemsDictionary;

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
        this.SetValuesOperatorsItemsDictionary = new SetValueOperatorsListDictionary().ItemsDictionary;
    }

    initialize() {
        this.Label = this.Data["label"] || null;
        this.SetValues = this.Data["setValues"] || [];

        if (this.SetValues.length == 0) {
            this.IsValidSetValue = false;
            let setValue = new SetValue();
            this.SetValues.push(setValue);
        }

        this.setUIProperties();
    }

    initializeFlowVariablesTreeItems() {
        this.FlowVariablesTreeItems = new FlowVariablesTreeList(this.FlowObjectFields, this.FlowObject, "").Items;
    }

    updateLabel(label: any) {
        this.Data["label"] = label;
        this.Label = label;

        this.setUIProperties();
    }

    updateSetValueField(field: string, setValueIndex: number) {
        if (field !== this.SetValues[setValueIndex]?.value) {
            this.SetValues[setValueIndex].field = field ? field.toString() : null;
            this.SetValues[setValueIndex].operator = SetValueOperators.Equals;

        }
        this.IsValidSetValue = this.isValidSetValue();
    }

    updateSetValueOperator(operatorCode: string, setValueIndex: number) {
        if (operatorCode !== this.SetValues[setValueIndex]?.operator) {
            this.SetValues[setValueIndex].operator = operatorCode;
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
            setvalue.id = this.SetValuesCounter;
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

    setUIProperties() {
        this.UIProperties.SetRequired("Label", null, AppTool.IsNullOrEmpty(this.Label));
    }

    isFieldCompareOperator(operatorCode: string) {
        return operatorCode && operatorCode.endsWith("<field>");
    }

    isObjectField(setValueField: string,index :number) {
        let idk = setValueField.split('.')
        if(idk[0] == 'triggeringrecord'){
            let objectfield = this.FlowObjectFields.find(e=>e.Code == idk[1] )
            this.SetValues[index].fieldObjectField = objectfield
            return true
        }else{
            let node = this.FlowObject.nodes.find(n=>n.type == "declareVariableNode" && n.data['variableCode'] == idk[0])
            this.SetValues[index].fieldType = node.data['variableType']
            return false
        }
    }

    isDateTimeField(objectField: ObjectFieldPM) {
        return objectField && (objectField.DataTypeCode === FieldTypes.DateTime || objectField.DataTypeCode === FieldTypes.Date);
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