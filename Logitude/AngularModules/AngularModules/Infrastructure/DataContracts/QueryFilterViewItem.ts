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
    public SelectedObjectFieldCode: string;
    public ObjectValueFieldCode: string;
    public IsRefreshField: boolean;
    public IsRefreshFieldValue: boolean;
    private SelectedObjectFieldPM: ObjectFieldPM;

    constructor(TreeFilter: any = null, ParentClass: QueryFilterTreeComponent = null) {
        super();
        this.BaseTreeFilter = TreeFilter;
        this.UIProperties = new UIProperties;
        this.Initialize(ParentClass);

    }

    Initialize(ParentClass: QueryFilterTreeComponent) {
        if (ParentClass != null) {
            this.MyParentClass = ParentClass;
        }

        if (this.BaseTreeFilter) {
            this.FillData();
        }

        if (!this.QueryFilterItems) {
            this.QueryFilterItems = [];
        }

        if (!this.Operator) {
            this.FillOperators("Text");
        }
        if (!this.BaseTreeFilter) {
            this.RefreshEntityFieldsFilterItems();
        }
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
        this.FieldValue = newValue ? FieldValueResolver.ConvertUTCDateToString(newValue,"TreeFilter") : "";
    }

    LogLovCondationValueChange(newValue) {
        this.FieldValue = newValue ? !AppTool.IsNullOrEmpty(newValue.Id) ? newValue.Id : newValue.Code : "";
    }

    FillData() {
        this.FillMainEntityName();
        this.FillSecondaryEntityName();
        this.FillFieldValue();
        this.Operator = this.BaseTreeFilter.Operator;
        this.IsCustom = this.BaseTreeFilter.IsCustom;
        this.FieldDataType = this.BaseTreeFilter.FieldDataType;
        this.FilterType = this.BaseTreeFilter.FilterType;
        this.QueryFilterItems = this.BaseTreeFilter.QueryFilterItems;
        this.FillOperators(this.FieldDataType);
        this.SelectedOperator = this.Operators.filter(x => x.Code == this.BaseTreeFilter.Operator)[0];
    }

    FillFieldValue() {
        if (this.FieldDataType == 'DateTime' || this.FieldDataType == 'Date')
            return;
        if (this.FieldDataType == 'Boolean') {
            this.BooleanListValueChanged(this.BaseTreeFilter.FieldValue?.toString() == 'true');
            return;
        }

        this.FieldValue = this.BaseTreeFilter.FieldValue;
    }

    FillMainEntityName() {
        let fieldName: string = this.BaseTreeFilter?.FieldName;
        let fieldNameAndObjectTableName = fieldName?.split('.');
        if (!fieldNameAndObjectTableName) return;

        this.FieldName = fieldNameAndObjectTableName.length == 1 ? fieldNameAndObjectTableName[0] : fieldNameAndObjectTableName[1];
        this.MainEntityName = this.FieldName ? fieldNameAndObjectTableName.length == 1 ? this.MyParentClass.objectTableName : this.MyParentClass.ParentObjectTableName : this.BaseTreeFilter?.MainEntityName;
        this.FieldChanged(window.ObjectFields.filter(f => this.FieldName == f.FieldName && this.MainEntityName == f.ObjectTableName)[0]);
    }

    FillSecondaryEntityName() {
        let fieldValue: string = this.BaseTreeFilter?.FieldValue;
        let fieldValueAndObjectTableName = fieldValue?.split('.');
        if (!fieldValueAndObjectTableName) return;

        this.FieldValue = fieldValueAndObjectTableName.length == 1 ? fieldValueAndObjectTableName[0] : fieldValueAndObjectTableName[1];
        this.SecondaryEntityName = this.FieldValue ? fieldValueAndObjectTableName.length == 1 ? this.MyParentClass.objectTableName : this.MyParentClass.ParentObjectTableName : this.BaseTreeFilter?.SecondaryEntityName;
        if (this.FieldDataType == 'Date' || this.FieldDataType == 'DateTime') {
            this.ValueChanged(window.ObjectFields.filter(f => this.FieldValue == f.FieldName && this.SecondaryEntityName == f.ObjectTableName)[0], false);
            this.FieldValue = FieldValueResolver.ConvertToDate(this.FieldValue,"TreeFilter");
        }
        else {
            this.ValueChanged(window.ObjectFields.filter(f => this.FieldValue == f.FieldName && this.SecondaryEntityName == f.ObjectTableName)[0]);
        }
    }

    IsObjectTableChanged(selectedEntity) {
        return !AppTool.IsNullOrEmpty(selectedEntity) && this.MyParentClass && selectedEntity != this.MyParentClass.parentObjectTableName && selectedEntity != this.MyParentClass.objectTableName;     
    }

    public get ObjectTableName() {
        if (AppTool.IsNullOrEmpty(this.FieldName)) return "";
        if (this.FieldName.indexOf('.') > -1) return this.FieldName.split('.')[0];
        return this.MainEntityName;
    }

    public get ObjectFieldName() {
        if (AppTool.IsNullOrEmpty(this.FieldName)) return "";
        if (this.FieldName.indexOf('.') > -1) return this.FieldName.split('.')[1];
        return this.FieldName;
    }

    SelectedObjectFieldWithLookUpTable: any;
    public get ObjectFieldLookUpTableName() {
        if (!this.SelectedObjectFieldPM) return "";

        if (this.SelectedObjectFieldWithLookUpTable && this.SelectedObjectFieldWithLookUpTable.Id == this.SelectedObjectFieldPM.Id) {
            return this.SelectedObjectFieldWithLookUpTable.ObjectTable_LookUpTableName;
        }

        this.SelectedObjectFieldWithLookUpTable = window.ObjectFields.filter(f => this.SelectedObjectFieldPM.Id == f.Id)[0];
        if (this.SelectedObjectFieldWithLookUpTable) {
            return this.SelectedObjectFieldWithLookUpTable.ObjectTable_LookUpTableName
        }
        else if (!this.EntityResourcesLoaded) {
            this.LoadLookUpTableResources();
        }
    }

    EntityResourcesLoaded: boolean;
    LoadLookUpTableResources() {
        if (!this.MyParentClass) return;

        this.MyParentClass.CurrentSession.CurrentWindow.StartBusyIndicator("Loading ...");
        this.MyParentClass.EntityResourceService.getEntityResourceByTableName(this.MainEntityName).subscribe((response: any) => {
            this.MyParentClass.CurrentSession.StopBusyIndicator();
            this.EntityResourcesLoaded = true;
            this.SelectedObjectFieldWithLookUpTable = window.ObjectFields.filter(f => this.SelectedObjectFieldPM.Id == f.Id)[0];
            if (this.SelectedObjectFieldWithLookUpTable) {
                return this.SelectedObjectFieldWithLookUpTable.ObjectTable_LookUpTableName
            }
        });
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
        this.Operator = operator ? operator.Code : null;
        this.ValueChanged(null);
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
        this.IsRefreshField = !this.IsRefreshField;
        this.FieldChanged(null);
        if (AppTool.IsNullOrEmpty(mainEntityName)) {
            this.RefreshMainEntityFieldsFilterItems();
        }
    }

    public SecondaryEntityChanged(secondaryEntityName) {
        this.SecondaryEntityName = secondaryEntityName;
        this.IsRefreshFieldValue = !this.IsRefreshFieldValue;
        this.ValueChanged(null);
        if (AppTool.IsNullOrEmpty(secondaryEntityName)) {
            this.RefreshSecondaryEntityFieldsFilterItems();
        }
    }

    public FillMainEntityFields() {
        this.MainEntityFieldsFilterItems = new ApiQueryFilters();
        let mainObjectTable = window.ObjectTables.filter(t => t.Name == this.MainEntityName);
        if (!mainObjectTable || !mainObjectTable[0]) return;
        this.MainEntityFieldsFilterItems.addAdditionalFilter("ObjectTableId", mainObjectTable[0].Id, null, null, "Equals", false, false, false, "string");
        this.MainEntityFieldsFilterItems.addAdditionalFilter("CanFilter", true, null, null, "Equals", false, false, false, "boolean");
        this.MainEntityFieldsFilterItems.addAdditionalFilter("DataTypeCode", "Constant", null, null, "NotEqual", false, false, false, "string"); 
        this.MainEntityFieldsFilterItems.addAdditionalFilter("IsCustomFilter", false, null, null, "Equals", false, false, false, "boolean");
    }

    public FillSecondaryEntityFields() {
        this.SecondaryEntityFieldsFilterItems = new ApiQueryFilters();
        let SecondaryEntityName = window.ObjectTables.filter(t => t.Name == this.SecondaryEntityName);
        if (!SecondaryEntityName || !SecondaryEntityName[0]) return;
        this.SecondaryEntityFieldsFilterItems.addAdditionalFilter("ObjectTableId", SecondaryEntityName[0].Id, null, null, "Equals", false, false, false, "string");
        this.SecondaryEntityFieldsFilterItems.addAdditionalFilter("CanFilter", true, null, null, "Equals", false, false, false, "boolean");
        this.SecondaryEntityFieldsFilterItems.addAdditionalFilter("DataTypeCode", this.FieldDataType, null, null, "Equals", false, false, false, "string");
        this.SecondaryEntityFieldsFilterItems.addAdditionalFilter("IsCustomFilter", false, null, null, "Equals", false, false, false, "boolean");
        if (this.FieldDataType == 'LookUp') {
            this.SecondaryEntityFieldsFilterItems.addAdditionalFilter("LookUpTableId", this.SelectedObjectFieldPM?.LookUpTableId, null, null, "Equals", false, false, false, "string");
        }
    }

    FieldChanged(objectFieldPM: ObjectFieldPM) {
        if (!objectFieldPM) {
            this.FieldName = "";
            this.SelectedObjectFieldCode = "";
            this.Operator = null;
            this.SelectedObjectFieldPM = null;
            return;
        }
        if (this.SelectedObjectFieldPM && this.SelectedObjectFieldPM.Id == objectFieldPM.Id) {
            return;
        }

        this.FieldSelectedChanged(objectFieldPM);
    }

    FieldSelectedChanged(objectFieldPM) {
        this.SelectedObjectFieldPM = objectFieldPM;
        this.SelectedObjectFieldCode = this.SelectedObjectFieldPM.FieldCode;
        if (this.MyParentClass && this.MyParentClass.ParentObjectTableName == this.MainEntityName) {
            this.FieldName = this.MainEntityName + '.' + objectFieldPM.FieldName;
        }
        else {
            this.FieldName = objectFieldPM.FieldName;
        }
        this.IsCustom = objectFieldPM.IsCustom;
        this.FieldDataType = objectFieldPM.DataTypeCode;
        this.FillOperators(objectFieldPM.DataTypeCode);
        this.Operator = null;
        this.SelectedOperator = null;
        this.SecondaryEntityChanged("");
    }

    ValueChanged(objectFieldPM: ObjectFieldPM, changeValue = true) {
        if (!objectFieldPM) {
            this.FieldValue = changeValue ? "" : this.FieldValue;
            this.ObjectValueFieldCode = "";
            return;
        }
        this.ObjectValueFieldCode = objectFieldPM.FieldCode;
        if (!changeValue) return;
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
