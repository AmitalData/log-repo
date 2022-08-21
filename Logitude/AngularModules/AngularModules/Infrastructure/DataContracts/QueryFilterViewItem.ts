import { QueryFilterTreeComponent } from '../../InfrastructureModules/InfrastructureCustomization/Components/Customization/QueryFilterTreeComponent';
import { SessionLocator } from '../Utilities/SessionLocator';
import { UIProperties } from '../Components/LogitudeComponents/UIProperties';
import { Output, EventEmitter } from '@angular/core';
import { AppTool } from '../Tools';
import { ObjectFieldPM } from '../EntityPMs/ObjectFieldPM';
import { ApiQueryFilters, FilterItem } from './ApiQueryFilters';
import { FieldValueResolver } from '../Utilities/FieldValueResolver';
declare var window: any;

export class QueryFilterViewItem extends FilterItem  {
    public MyParentClass: QueryFilterTreeComponent;
    public BaseTreeFilter: any;
    public Operators: any;
    public UIProperties: UIProperties;
    public MainEntityFields: any[];
    public MainEntityFieldsFilterItems: ApiQueryFilters;
    public SecondaryEntityFieldsFilterItems: ApiQueryFilters;
    private andOr: string = "And";
    public AndOrOps = ["And", "Or"];
    public BooleanList: boolean[] = [true, false];
    public SelectedOperator: Operator;
    public IsChecked: boolean;
    private SelectedObjectFieldPM: ObjectFieldPM;

    constructor(TreeFilter: any = null, ParentClass: QueryFilterTreeComponent = null) {
        super();
        this.BaseTreeFilter = TreeFilter;
        this.UIProperties = new UIProperties;

        if (ParentClass != null) {
            this.MyParentClass = ParentClass;
        }

        if (this.BaseTreeFilter) {
            this.FillData();
        }

        if (!this.AdditionalFilters) {
            this.AdditionalFilters = [];
        }

        if (!this.Operator) {
            this.FillOperators("Text");
        }

        this.RefreshEntityFieldsFilterItems();
    }

    RefreshEntityFieldsFilterItems() {
        this.RefreshMainEntityFieldsFilterItems();
        this.RefreshSecondaryEntityFieldsFilterItems();
    }

    RefreshMainEntityFieldsFilterItems() {
        this.MainEntityFieldsFilterItems = new ApiQueryFilters();
        this.MainEntityFieldsFilterItems.addAdditionalFilter("Tenant", -1, null, null, "Equals", false, false, false, "number");
    }

    RefreshSecondaryEntityFieldsFilterItems() {
        this.SecondaryEntityFieldsFilterItems = new ApiQueryFilters();
        this.SecondaryEntityFieldsFilterItems.addAdditionalFilter("Tenant", -1, null, null, "Equals", false, false, false, "number");
    }

    BooleanListValueChanged(newValue: boolean) {
        this.IsChecked = newValue;
        this.FieldValue = this.IsChecked ? "true" : "false";
    }

    TextBoxCondationValueChange(newValue) {
        this.FieldValue = newValue;
    }

    DatePickerCondationValueChange(newValue) {
        if (newValue) {
            this.FieldValue = FieldValueResolver.ConvertUTCDateToString(newValue);
        }
        else {
            this.FieldValue = "";
        }
    }

    LogLovCondationValueChange(newValue) {
        this.FieldValue = newValue ? !AppTool.IsNullOrEmpty(newValue.Id) ? newValue.Id : newValue.Code : "";
    }

    FillData() {
        this.FillMainEntityName();
        this.FillSecondaryEntityName();
        this.FieldValue = this.BaseTreeFilter.FieldValue;
        this.Operator = this.BaseTreeFilter.Operator;
        this.IsCustom = this.BaseTreeFilter.IsCustom;
        this.FieldDataType = this.BaseTreeFilter.FieldDataType;
        this.FilterType = this.BaseTreeFilter.FilterType;
        this.AdditionalFilters = this.BaseTreeFilter.AdditionalFilters;
        this.FillOperators(this.FieldDataType);
        this.SelectedOperator = this.Operators.filter(x => x.Code == this.BaseTreeFilter.Operator)[0];
    }

    FillMainEntityName() {
        let fieldName: string = this.BaseTreeFilter?.FieldName;
        let fieldNameAndObjectTableName = fieldName?.split('.');
        if (!fieldNameAndObjectTableName) return;

        this.FieldName = fieldNameAndObjectTableName.length == 1 ? fieldNameAndObjectTableName[0] : this.MyParentClass.objectTableName;
        this.MainEntityName = fieldNameAndObjectTableName.length == 1 ? fieldNameAndObjectTableName[1] : this.MyParentClass.ParentObjectTableName;
    }

    FillSecondaryEntityName() {
        let fieldValue: string = this.BaseTreeFilter?.FieldValue;
        let fieldValueAndObjectTableName = fieldValue?.split('.');
        if (!fieldValueAndObjectTableName) return;

        this.FieldValue = fieldValueAndObjectTableName.length == 1 ? fieldValueAndObjectTableName[0] : this.MyParentClass.objectTableName;
        this.secondaryEntityName = fieldValueAndObjectTableName.length == 1 ? fieldValueAndObjectTableName[1] : this.MyParentClass.ParentObjectTableName;
    }

    IsObjectTableChanged(selectedEntity) {
        return !AppTool.IsNullOrEmpty(selectedEntity) && this.MyParentClass && selectedEntity != this.MyParentClass.parentObjectTableName && selectedEntity != this.MyParentClass.objectTableName;     
    }


    public get ObjectTableName() {
        if (AppTool.IsNullOrEmpty(this.FieldName))
            return "";
        return this.FieldName.split('.')[0];
    }

    public get ObjectFieldName() {
        if (AppTool.IsNullOrEmpty(this.FieldName))
            return "";
        if (this.FieldName.indexOf('.') > -1)
            return this.FieldName.split('.')[1];
        return this.FieldName;
    }

    public get ObjectFieldLookUpTableName() {
        if (!this.SelectedObjectFieldPM)
            return "";

        let targetObjectField = window.ObjectFields.filter(f => this.SelectedObjectFieldPM.Id == f.Id);
        if (targetObjectField && targetObjectField[0]) {
            return targetObjectField[0].ObjectTable_LookUpTableName
        }
    }

    public mainEntityName: string;
    public get MainEntityName() {
        if (this.IsObjectTableChanged(this.mainEntityName)) {
            this.MainEntityChanged("");
        }
        return this.mainEntityName;
    }
    public set MainEntityName(newValue: string) {
        this.mainEntityName = newValue;
        this.FillMainEntityFields();
    }

    public secondaryEntityName: string;
    public get SecondaryEntityName() {
        if (this.IsObjectTableChanged(this.secondaryEntityName)) {
            this.SecondaryEntityChanged("");
        }
        return this.secondaryEntityName;
    }
    public set SecondaryEntityName(newValue: string) {
        this.secondaryEntityName = newValue;
        this.FillSecondaryEntityFields();
    }

    private indexOrder: number;
    public get IndexOrder() { return this.indexOrder; }
    public set IndexOrder(newValue: number) { this.indexOrder = newValue; }

    private isGroup: boolean = false;
    public get IsGroup() { return this.isGroup; }
    public set IsGroup(newValue: boolean) { this.isGroup = newValue; }

    private showBtns: boolean = false;
    public get ShowBtns() { return this.showBtns; }
    public set ShowBtns(newValue: boolean) { this.showBtns = newValue; }

    public get ShowOtherEntity() {
        if (!this.Operator) return false;
        return this.Operator.indexOf('Field') > -1;
    }

    public setAndOrOperation(Newvalue: string) {
        this.AndOr = Newvalue;
    }

    public get AndOr() {
        let temp = AppTool.IsNullOrEmpty(this.andOr) ? "And" : this.andOr;
        this.FilterType = temp;
        return temp;
    }

    public set AndOr(newValue: string) {
        this.FilterType = newValue;
        this.andOr = newValue;
    }

    private FillOperators(dataTypeCode: string) {
        this.Operators = [];

        if (dataTypeCode == "DateTime" || dataTypeCode == "Date" || dataTypeCode == "Integer" || dataTypeCode == "Decimal" || dataTypeCode == "Double") {
            this.Operators.push(new Operator("Equal", "Equal"));
            this.Operators.push(new Operator("Does Not Equal", "NotEqual"));
            this.Operators.push(new Operator("Greater Than", "GreaterThan"));
            this.Operators.push(new Operator("Less Than", "LessThan"));
            this.Operators.push(new Operator("Greater Than Or Equal", "GreaterThanOrEqual"));
            this.Operators.push(new Operator("Less Than Or Equal", "LessThanOrEqual"));
            this.Operators.push(new Operator("Equal [Field]", "EqualField"));
            this.Operators.push(new Operator("Does Not Equal [Field]", "NotEqualField"));
            this.Operators.push(new Operator("Greater Than [Field]", "GreaterThanField"));
            this.Operators.push(new Operator("Less Than [Field]", "LessThanField"));
            this.Operators.push(new Operator("Greater Than Or Equal [Field]", "GreaterThanOrEqualField"));
            this.Operators.push(new Operator("Less Than Or Equal [Field]", "LessThanOrEqualField"));
        }
        else if (dataTypeCode == "Boolean") {
            this.Operators.push(new Operator("Equal", "Equal"));}
        else if (dataTypeCode == "LookUp") {
            this.Operators.push(new Operator("Equal", "Equal"));
            this.Operators.push(new Operator("Does Not Equal", "NotEqual"));
            this.Operators.push(new Operator("Equal [Field]", "EqualField"));
            this.Operators.push(new Operator("Does Not Equal [Field]", "NotEqualField"));
        }
        else {
            this.Operators.push(new Operator("Equal", "Equal"));
            this.Operators.push(new Operator("Does Not Equal", "NotEqual"));
            this.Operators.push(new Operator("Equal [Field]", "EqualField"));
            this.Operators.push(new Operator("Does Not Equal [Field]", "NotEqualField"));
            this.Operators.push(new Operator("Contains", "Contains"));
            this.Operators.push(new Operator("Does Not Contain", "NotContains"));
            this.Operators.push(new Operator("Contains [Field]", "ContainsField"));
            this.Operators.push(new Operator("Does Not Contain [Field]", "NotContainsField"));
        }

        this.Operators.push(new Operator("Is Empty", "IsEmpty"));
        this.Operators.push(new Operator("Is not Empty", "IsNotEmpty"));
    }

    public OperationValueChanged(operator) {
        this.SelectedOperator = operator;
        if (operator) {
            this.Operator = operator.Code;
        }
        else {
            this.Operator = null;
        }
    }

    public AndOrOpsChanged(value) {
        this.AndOr = value;
        this.FilterType = value;
    }

    public OnMouseOver(event) {
        var e = event.toElement;
        if (e && e.className == "LinkBtn") {
            return;
        }
        if (this.ShowBtns == false) {
            this.ShowBtns = true;
        }
    }

    public OnMouseOut(event) {
        var e = event.toElement;
        if (e && e.className == "LinkBtn") {
            return;
        }
        if (this.ShowBtns == true) {
            this.ShowBtns = false;
        }
    }

    public preventInnerHover(event) {
        event.stopPropagation();
    }

    public MainEntityChanged(mainEntityName) {
        this.MainEntityName = mainEntityName;
        this.FieldChanged(null);
        if (AppTool.IsNullOrEmpty(mainEntityName)) {
            this.RefreshMainEntityFieldsFilterItems();
        }
    }

    public SecondaryEntityChanged(secondaryEntityName) {
        this.SecondaryEntityName = secondaryEntityName;
        this.ValueChanged(null);
        if (AppTool.IsNullOrEmpty(secondaryEntityName)) {
            this.RefreshSecondaryEntityFieldsFilterItems();
        }
    }

    public FillMainEntityFields() {
        this.MainEntityFieldsFilterItems = new ApiQueryFilters();
        let mainObjectTable = window.ObjectTables.filter(t => t.Name == this.MainEntityName);
        if (!mainObjectTable || !mainObjectTable[0].Id) return;
        this.MainEntityFieldsFilterItems.addAdditionalFilter("ObjectTableId", mainObjectTable[0].Id, null, null, "Equals", false, false, false, "string");
        this.MainEntityFieldsFilterItems.addAdditionalFilter("CanFilter", true, null, null, "Equals", false, false, false, "boolean");
    }

    public FillSecondaryEntityFields() {
        this.SecondaryEntityFieldsFilterItems = new ApiQueryFilters();
        let SecondaryEntityName = window.ObjectTables.filter(t => t.Name == this.SecondaryEntityName);
        if (!SecondaryEntityName || !SecondaryEntityName[0].Id) return;
        this.SecondaryEntityFieldsFilterItems.addAdditionalFilter("ObjectTableId", SecondaryEntityName[0].Id, null, null, "Equals", false, false, false, "string");
        this.SecondaryEntityFieldsFilterItems.addAdditionalFilter("CanFilter", true, null, null, "Equals", false, false, false, "boolean");
        this.SecondaryEntityFieldsFilterItems.addAdditionalFilter("DataTypeCode", this.FieldDataType, null, null, "Equals", false, false, false, "string");
    }

    FieldChanged(objectFieldPM: ObjectFieldPM) {
        this.SelectedObjectFieldPM = objectFieldPM;
        if (!objectFieldPM) {
            this.FieldName = "";
            return;
        }
        if (this.MyParentClass && this.MyParentClass.ParentObjectTableName == this.MainEntityName) {
            this.FieldName = this.MainEntityName + '.' + objectFieldPM.FieldName;
        }
        else {
            this.FieldName = objectFieldPM.FieldName;
        }
        this.IsCustom = objectFieldPM.IsCustom;
        this.FieldDataType = objectFieldPM.DataTypeCode;
        this.FillOperators(objectFieldPM.DataTypeCode);
    }

    ValueChanged(objectFieldPM: ObjectFieldPM) {
        if (!objectFieldPM) {
            this.FieldValue = "";
            return;
        }
        if (this.MyParentClass && this.MyParentClass.ParentObjectTableName == this.SecondaryEntityName) {
            this.FieldValue = this.SecondaryEntityName + '.' + objectFieldPM.FieldName;
        }
        else {
            this.FieldValue = objectFieldPM.FieldName;
        }
    }
}

export class Operator {
    Code: string;
    Name: string;

    constructor(name: string, code: string) {
        this.Code = code;
        this.Name = name;
    }
}
