declare var window: any;
import {QueryPM} from '../../../../../Infrastructure/EntityPMs/QueryPM';
import {LogEvents} from '../../../../../Infrastructure/Utilities/LogEvents';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {PubSubService} from '../../../../../Infrastructure/Utilities/events/ApiFiltersEvent';
import {ComboBox} from '../../../../../Controls/ComboBox';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionInfo} from '../../../../../Infrastructure/Utilities/SessionInfo';
import {ObjectFieldPM} from '../../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools';
import {RuleConditionFieldPM} from '../../../../../Infrastructure/EntityPMs/RuleConditionFieldPM';
import {FieldValueResolver} from '../../../../../Infrastructure/Utilities/FieldValueResolver';

export class ConditionFilterField extends BaseComponent {

    //public textchangeevent: EventEmitter<any>;
    RuleConditionFieldPMs: RuleConditionFieldPM[];
    ParentClass: any;
    filters: ApiQueryFilters;
    iswidnowMode: boolean = false;
    RuleId: string;
    public ControlId: string = null;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(objectField: ObjectFieldPM, ruleId: string, iswidnowMode: boolean, ruleConditionFieldPMs: RuleConditionFieldPM[], parentClass: any = null, filterchangeevent: PubSubService = null) {
        super();
        this.ControlId = "CheckBox_" + this.CurrentSession.GetNewId("CheckBox");
        this.RuleId = ruleId;
        this.Filterchangeevent = filterchangeevent;
        this.ParentClass = parentClass;
        this.RuleConditionFieldPMs = ruleConditionFieldPMs;
        this.ObjectField = objectField;
        this.ObjectTable = window.ObjectTables.filter(a => a.Id == this.ObjectField.ObjectTableId)[0];
        var translation = TextCodeTranslator.Translate(this.ObjectField.FullNameTextCodeCode);
        this.iswidnowMode = iswidnowMode;
        //if (translation != null && translation != undefined && translation != "") {
        //    this.FieldName = translation;
        //}
        //else {
        this.FieldName = this.ObjectField.FieldName;

        if (this.RuleConditionFieldPMs) {
            var conditionField = this.RuleConditionFieldPMs.filter(f => f.ObjectFieldId === objectField.Id)[0];
            if (conditionField) {
                this.RuleConditionFieldPM = conditionField;
                if (!AppTool.IsNullOrEmpty(conditionField.Value)) {
                    this.EnableDelete = false;
                    this.IsPreDefined = true;
                    //if (conditionField.Value.toLowerCase() == "false") {
                    //    this.TextValue = "false";
                    //}
                    //else if (conditionField.Value.toLowerCase() == "true") {
                    //    this.TextValue = "true";
                    //}
                    //else {
                    //    this.TextValue = conditionField.Value;
                    //}

                    this.TextValue = FieldValueResolver.GetFieldDataValue(this.ObjectField, conditionField.Value);
                }

                this.Operation = this.Operators.filter(a => a.Code == conditionField.Operator)[0];
                this.EnableDelete = true;
            }

        }
 
    
    }


    private filterchangeevent: PubSubService;
    public get Filterchangeevent() { return this.filterchangeevent; }
    public set Filterchangeevent(newValue: PubSubService) { this.filterchangeevent = newValue; }

    private objectField: any;
    public get ObjectField() { return this.objectField; }
    public set ObjectField(newValue: any) { this.objectField = newValue; }

    private objectTable: any;
    public get ObjectTable() { return this.objectTable; }
    public set ObjectTable(newValue: any) { this.objectTable = newValue; }

    private ruleConditionFieldPM: RuleConditionFieldPM;
    public get RuleConditionFieldPM() { return this.ruleConditionFieldPM; }
    public set RuleConditionFieldPM(newValue: RuleConditionFieldPM) { this.ruleConditionFieldPM = newValue; }

    private fieldName: string;
    public get FieldName() { return this.fieldName; }
    public set FieldName(newValue: string) { this.fieldName = newValue; }

    private enableDelete: boolean = false;
    public get EnableDelete() { return this.enableDelete; }
    public set EnableDelete(newValue: boolean) { this.enableDelete = newValue; }

    private newField: boolean = true;
    public get NewField() { return this.newField; }
    public set NewField(newValue: boolean) { this.newField = newValue; }

    private isPreDefined: boolean = false;
    public get IsPreDefined() { return this.isPreDefined; }
    public set IsPreDefined(newValue: boolean) { this.isPreDefined = newValue; }

    private exists: boolean;
    public get Exists() { return this.exists; }
    public set Exists(newValue: boolean) {
        //if (this.ParentClass.SelectedObjectFields && (this.ParentClass.SelectedObjectFields.length) > 10 && newValue == true) {
        //    this.ParentClass.ValidationErrorsList = [];
        //    this.ParentClass.ValidationErrorsList.push("The max. number of filters you can use is 10");
        //    this.exists = false;
        //}
        //else {
        var temp = this.exists;
        this.exists = newValue;
        if (this.ParentClass != null && newValue != temp && temp != null) {
            if (newValue == true) {
                this.ParentClass.AddFilterField(this.ParentClass.MapJsonToEntityPM(this.ObjectField))
                if (!this.iswidnowMode) {
                    this.ParentClass.RunSave(this);
                }
            }
            else {
                //this.onDeleteFilterClick();
                var tempField = this;
                //if (this.exists == false) {
                tempField.IsDeleted = true;
                //if (!this.IsDeleted){
                if (this.RuleConditionFieldPMs.filter(a => a.ObjectFieldId == this.ObjectField.Id).length > 0) {
                    this.RuleConditionFieldPMs = this.RuleConditionFieldPMs.filter(a => a.ObjectFieldId != this.ObjectField.Id);
                }
                this.ParentClass.DeteteFilter(this);
                //if (this.TextValue != null && this.TextValue != '') {
                //this.Filterchangeevent.Stream.emit(tempField);
                //console.log(this.TextValue);
                //}
                //}

            }
        }
        else if (temp == null && this.NewField == true) {
            this.ParentClass.AddFilterField(this.ParentClass.MapJsonToEntityPM(this.ObjectField))
            if (!this.iswidnowMode && !temp) {
                this.ParentClass.RunSave(this);
            }
        }
        //else if (temp == null && !this.IsDeleted) {
        //    this.ParentClass.DeteteFilter(this);
        //    if (!this.iswidnowMode && this.TextValue != null) {
        //        this.Filterchangeevent.Stream.emit(tempField);
        //    }
        //}
        //}

    }

    private isDeleted: boolean = false;
    public get IsDeleted() { return this.isDeleted; }
    public set IsDeleted(newValue: boolean) {
        this.isDeleted = newValue;
    }

    private operation: ObjectFieldOperator;
    public get Operation() {
        if (!this.operation) {
            if ((this.ObjectField.DataTypeCode == "Text" || this.ObjectField.DataTypeCode == "nText") && AppTool.IsNullOrEmpty(this.operation)) {
                this.operation = new ObjectFieldOperator("StartsWith", "Starts With");
                return this.operation;
            }
            else {
                if (AppTool.IsNullOrEmpty(this.operation)) {
                    this.operation = new ObjectFieldOperator("Equals", "Equals to");
                }
                return this.operation;
            }

            //if (this.ObjectField.DataTypeCode == "Integer" || this.ObjectField.DataTypeCode == "UnsInteger"
            //    || this.ObjectField.DataTypeCode == "Double" || this.ObjectField.DataTypeCode == "SigDouble"
            //    || this.ObjectField.DataTypeCode == "Decimal" || this.ObjectField.DataTypeCode == "UnsDecimal"
            //    || this.ObjectField.DataTypeCode == "DateTime" || this.ObjectField.DataTypeCode == "Date") {
            //    return this.operation;
            //}

            //if (this.ObjectField.DataTypeCode == "LookUp" || this.ObjectField.DataTypeCode == "PickList" || this.ObjectField.DataTypeCode == "Boolean") {
            //    return this.operation;
            //}
        }
        else {
            return this.operation;
        }
    }
    public set Operation(newValue: ObjectFieldOperator) {
        this.operation = newValue;
        //this.Filterchangeevent.Stream.emit(this);
        //alert(this.operation + this.FieldName);
    }

    private operators: ObjectFieldOperator[];
    public get Operators() { return this.GetFieldOperators(this.ObjectField); }
    public set Operators(newValue: ObjectFieldOperator[]) {
        this.operators = newValue;
    }

    private textValue: any;
    public get TextValue() { return this.textValue; }
    public set TextValue(newValue: any) {
        this.textValue = newValue;
    }

    private textValue1: any;
    public get TextValue1() { return this.textValue1; }
    public set TextValue1(newValue: any) {
        this.textValue1 = newValue;
    }

    private name: any;
    public get Name() { return this.name; }
    public set Name(newValue: any) {
        this.name = newValue;
    }

    private deleteButtonVisibility: boolean = false;
    public get DeleteButtonVisibility() { return this.deleteButtonVisibility; }
    public set DeleteButtonVisibility(newValue: boolean) {
        this.deleteButtonVisibility = newValue;
    }


    OperationValueChanged(operation) {
        //alert(operation + this.FieldName);
        this.Operation = operation;
        if (this.TextValue != null) {
            //this.Filterchangeevent.Stream.emit(this);
        }
    }
    timerToken: any;
    onTextChange(value) {
        if (value != "true" && value != "false") {
            if (this.timerToken) {
                clearTimeout(this.timerToken);
            }
            if (value != null && value.Name != null) {
                this.TextValue = value.Name;
                this.Name = value.Name;
                this.Operation = new ObjectFieldOperator(value.Operation, value.Operation);
            }
            else if (value != null && value.Date != null) {
                this.TextValue = value.Date;
                this.Operation = new ObjectFieldOperator(value.Operation, value.Operation);
            }
            else if (value && value.FromDate && value.ToDate) {
                this.TextValue = value.FromDate;
                this.TextValue1 = value.ToDate;
                if (this.RuleConditionFieldPM) {
                    if (!AppTool.IsNullOrEmpty(value.Name)) {
                        this.RuleConditionFieldPM.Value = value.Name;
                    }
                    else {
                        this.RuleConditionFieldPM.Value = value.FromDate;
                        //this.AdvancedQueryFilterPM.PredefinedValue2 = value.ToDate;
                    }
                }
                //this.AdvancedQueryFilterPM.PredefinedValue2 = value.ToDate;
                this.Operation = new ObjectFieldOperator(value.Operation, value.Operation);
            }
            else if (value == "NoDate") {
                this.TextValue = "NoDate";
                this.TextValue1 = null;
            }
            else if ((value == null && this.ObjectField.DataTypeCode != "Boolean")) {
                this.TextValue = "";
            }
            else if ((value == null && this.ObjectField.DataTypeCode == "Boolean")) {
                this.TextValue = false;
            }
            else if (this.TextValue == "" || this.TextValue == null) {
                this.TextValue = value;
            }
            else if (value != this.TextValue) {
                this.TextValue = value;
            }
            this.timerToken = setTimeout((event) => this.RunSearch(this.TextValue), 300);
        }
    }

    RunSearch(event) {
        if (event != null) {
            var xx = event.toString();
            if (event.toString().toLowerCase() == "true") {
                this.TextValue = "True";
            }
            else if (event.toString().toLowerCase() == "false") {
                this.TextValue = "False";
            }
            else if (event.toString().toLowerCase() == "no filter") {
                this.TextValue = "No Filter";
            }
            else if (this.TextValue != event) {
                this.TextValue = event;
            }
            if (this.TextValue != null && this.Filterchangeevent) {
                //this.Filterchangeevent.Stream.emit(this);
            }
        }
        else {
            this.TextValue = "";
        }
        // this.textchangeevent.emit(this);
    }

    onDeleteFilterClick() {
        //var filter = this.ParentClass.AdvancedQueryFilterPMs.filter(d => ((d.Tenant == SessionInfo.LoggedUserTenant && d.UserId == SessionInfo.LoggedUserId))).filter(d => d.ObjectFieldId == this.ObjectField.Id && d.QueryId == this.QueryId)[0];
        //this.AdvancedQueryFilterPM = filter;
        var temp = this;
        temp.IsDeleted = true;
        if (this.RuleConditionFieldPMs.filter(a => a.ObjectFieldId == this.ObjectField.Id).length > 0) {
            this.RuleConditionFieldPMs = this.RuleConditionFieldPMs.filter(a => a.ObjectFieldId != this.ObjectField.Id);
        }
        this.ParentClass.DeteteFilter(this);
        if (this.TextValue != null) {
            //this.Filterchangeevent.Stream.emit(temp);
        }
        //this.Exists = false;

    }

    list: ObjectFieldOperator[];
    private GetFieldOperators(field: ObjectFieldPM) {


        this.list = [];

        if (field.DataTypeCode == "Text" || field.DataTypeCode == "nText") {


            this.list.push(this.equalsOp);
            //if (ruleMode) {
            //    list.push(notEqualsOp);
            //}
            //else {
            this.list.push(this.startsWithOp);
            //}
        }

        if (field.DataTypeCode == "Integer" || field.DataTypeCode == "UnsInteger"
            || field.DataTypeCode == "Double" || field.DataTypeCode == "SigDouble"
            || field.DataTypeCode == "Decimal" || field.DataTypeCode == "UnsDecimal"
            || field.DataTypeCode == "DateTime" || field.DataTypeCode == "Date") {
            this.list.push(this.largerThanOp);
            this.list.push(this.lessThanOp);
            this.list.push(this.equalsOp);
            this.list.push(this.greaterThanOrEqualOp);
            this.list.push(this.lessThanOrEqualOp);
            //if (ruleMode) {
            //    list.push(notEqualsOp);
            //}
        }


        if (field.DataTypeCode == "LookUp" || field.DataTypeCode == "PickList" || field.DataTypeCode == "Boolean") {
            this.list.push(this.equalsOp);
            this.list.push(this.notEqualsOp);


        }
        return this.list;
    }




    startsWithOp: ObjectFieldOperator = new ObjectFieldOperator("StartsWith", "Starts With");
    equalsOp: ObjectFieldOperator = new ObjectFieldOperator("Equals", "Equals to");
    notEqualsOp: ObjectFieldOperator = new ObjectFieldOperator("NotEqual", "Not Equal to");
    largerThanOp: ObjectFieldOperator = new ObjectFieldOperator("LargerThan", "Greater Than");
    lessThanOp: ObjectFieldOperator = new ObjectFieldOperator("LessThan", "Less Than");
    greaterThanOrEqualOp: ObjectFieldOperator = new ObjectFieldOperator("GreaterThanOrEqual", "Greater Than Or Equal");
    lessThanOrEqualOp: ObjectFieldOperator = new ObjectFieldOperator("LessThanOrEqual", "Less Than Or Equal");
    BetweenOp: ObjectFieldOperator = new ObjectFieldOperator("Between", "Between");

}


export class ConditionFilterFieldsClass {
    public IsWindowMode: boolean;
    filterfield: ConditionFilterField;
    ParentClass: any;
    event: PubSubService = null
    constructor(isWindowMode: boolean, parentClass: any, filterchangeevent: PubSubService = null) {
        this.ParentClass = parentClass;
        this.IsWindowMode = isWindowMode;
        this.event = filterchangeevent;
        this.FilterFields = [];
    }

    AddFiltersList(objectsList: any, ruleId: string, RuleConditionFieldPMs: RuleConditionFieldPM[]) {
        //ObservableCollection
        if (this.ParentClass == null) {
            var ss = "";
        }
        var newList = [];
        //.sort((a, b) => { return (a.FieldName === b.FieldName) ? 0 : a ? -1 : 1 })
        objectsList.sort((a, b) => { return (a.FieldName === b.FieldName) ? 0 : (a.FieldName < b.FieldName) ? -1 : 1 }).forEach((item, key) => {
            newList.push(new ConditionFilterField(item, ruleId, this.IsWindowMode, RuleConditionFieldPMs, this.ParentClass, this.event));
        });

        this.FilterFields = newList.sort((a, b) => { return (TextCodeTranslator.Translate(a.ObjectField.FullNameTextCodeCode).toLowerCase() === TextCodeTranslator.Translate(b.ObjectField.FullNameTextCodeCode).toLowerCase()) ? 0 : (TextCodeTranslator.Translate(a.ObjectField.FullNameTextCodeCode).toLowerCase() < TextCodeTranslator.Translate(b.ObjectField.FullNameTextCodeCode).toLowerCase()) ? -1 : 1 });
    }

    SetExists(field: any, value: boolean) {
        this.filterfield = this.FilterFields.filter(f => f.ObjectField.FieldName == field.FieldName)[0];
        if (this.filterfield != null) {
            this.filterfield.NewField = false;
            this.filterfield.Exists = value;
        }

    }
    SetDeleted(field: any, value: boolean) {
        this.filterfield = this.FilterFields.filter(f => f.ObjectField.FieldName == field.FieldName)[0];
        if (this.filterfield != null) {
            this.filterfield.NewField = false;
            this.filterfield.IsDeleted = value;
        }
    }

    filterFields: any[];
    public get FilterFields() { return this.filterFields; }
    public set FilterFields(newValue: any[]) { this.filterFields = newValue; }


}

export class FieldsValues {

    FieldsDictionary: { [key: string]: any; } = {};

    GetFieldValue(key: string) {
        return this.FieldsDictionary[key];
    }

    SetFieldValue(key: string, value: any) {
        this.FieldsDictionary[key] = value;
    }





}

export class ObjectFieldOperator {

    constructor(code: string, name: string) {
        this.Code = code;
        this.Name = name;
    }

    private code: string;
    public get Code() { return this.code; }
    public set Code(newValue: string) { this.code = newValue; }

    private name: string;
    public get Name() { return this.name; }
    public set Name(newValue: string) { this.name = newValue; }

}
