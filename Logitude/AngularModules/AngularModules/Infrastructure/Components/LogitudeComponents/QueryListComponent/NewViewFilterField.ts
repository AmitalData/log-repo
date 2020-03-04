import {QueryPM} from '../../../../Infrastructure/EntityPMs/QueryPM'; 
import {LogEvents} from '../../../../Infrastructure/Utilities/LogEvents';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {PubSubService} from '../../../../Infrastructure/Utilities/events/ApiFiltersEvent';
import {ComboBox} from '../../../../Controls/ComboBox';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AdvancedQueryFilterPM} from '../../../../Infrastructure/EntityPMs/AdvancedQueryFilterPM';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {ObjectFieldPM} from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {NewViewComponent} from '../../../../Infrastructure/Components/NewViewComponent/NewViewComponent';




export class NewViewFilterField extends BaseComponent {
    //public textchangeevent: EventEmitter<any>;
    AdvancedQueryFilterPMs: AdvancedQueryFilterPM[]
    ParentClass: any;
    Filterchangeevent: PubSubService;
    filters: ApiQueryFilters;
    iswidnowMode: boolean = false;
    QueryId: string;
    QueryCode: string;

    constructor(objectField: any, queryCode: string, iswidnowMode: boolean, AdvancedQFPMs: AdvancedQueryFilterPM[], parentClass: any = null, filterchangeevent: PubSubService = null) {
        super();
        this.QueryCode = queryCode;
        this.Filterchangeevent = filterchangeevent;
        this.ParentClass = parentClass;
        this.AdvancedQueryFilterPMs = AdvancedQFPMs;
        this.ObjectField = objectField;
        var translation = TextCodeTranslator.Translate(this.ObjectField.FullNameTextCodeCode);
        this.iswidnowMode = iswidnowMode;
        //if (translation != null && translation != undefined && translation != "") {
        //    this.FieldName = translation;
        //}
        //else {
        this.FieldName = this.ObjectField.FieldName;


        //}
        //SessionInfo.LoggedUserTenant, SessionInfo.LoggedUserId
        if (queryCode != null && queryCode != undefined && queryCode != "") {
            var preDefinedFilter = this.AdvancedQueryFilterPMs.filter(d => ((d.Tenant == SessionInfo.LoggedUserTenant && d.UserId == SessionInfo.LoggedUserId) || d.Tenant == 0) && d.IsPredefined == true).filter(d => d.ObjectFieldCode == objectField.FieldCode && d.QueryCode == queryCode)[0];
            if (preDefinedFilter != null) {
                this.AdvancedQueryFilterPM = preDefinedFilter;
                if (preDefinedFilter.PredefinedValue != null) {
                    this.EnableDelete = false;
                    this.IsPreDefined = true;
                    if (preDefinedFilter.PredefinedValue.toLowerCase() == "false") {
                        this.TextValue = false;
                    }
                    else if (preDefinedFilter.PredefinedValue.toLowerCase() == "true") {
                        this.TextValue = true;
                    }
                    else {
                        this.TextValue = preDefinedFilter.PredefinedValue;
                    }
                }
                else {
                    this.EnableDelete = true;
                }
            }
            else {
                this.EnableDelete = true;
            }
        }
    }

    private objectField: any;
    public get ObjectField() { return this.objectField; }
    public set ObjectField(newValue: any) { this.objectField = newValue; }

    private advancedQueryFilterPM: AdvancedQueryFilterPM;
    public get AdvancedQueryFilterPM() { return this.advancedQueryFilterPM; }
    public set AdvancedQueryFilterPM(newValue: AdvancedQueryFilterPM) { this.advancedQueryFilterPM = newValue; }

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

                var tempField = this;
                //if (this.exists == false) {
                tempField.IsDeleted = true;
                //if (!this.IsDeleted){
                this.ParentClass.DeteteFilter(this);
                if (!this.iswidnowMode && this.TextValue != null && this.TextValue != '') {
                    this.Filterchangeevent.Stream.emit(tempField);
                    console.log(this.TextValue);
                }
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


    }

    private isDeleted: boolean = false;
    public get IsDeleted() { return this.isDeleted; }
    public set IsDeleted(newValue: boolean) {
        this.isDeleted = newValue;
    }

    private operation: NewViewObjectFieldOperator;
    public get Operation() {
        if (!this.operation) {
            if (this.ObjectField.DataTypeCode == "Text" || this.ObjectField.DataTypeCode == "nText") {
                this.operation = new NewViewObjectFieldOperator("StartsWith", "Starts With");
                return this.operation;
            }
            else {
                this.operation = new NewViewObjectFieldOperator("Equals", "Equals to");
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
    public set Operation(newValue: NewViewObjectFieldOperator) {
        this.operation = newValue;
        //this.Filterchangeevent.Stream.emit(this);
        //alert(this.operation + this.FieldName);
    }

    private operators: NewViewObjectFieldOperator[];
    public get Operators() { return this.GetFieldOperators(this.ObjectField); }
    public set Operators(newValue: NewViewObjectFieldOperator[]) {
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

    private deleteButtonVisibility: boolean = false;
    public get DeleteButtonVisibility() { return this.deleteButtonVisibility; }
    public set DeleteButtonVisibility(newValue: boolean) {
        this.deleteButtonVisibility = newValue;
    }


    OperationValueChanged(operation) {
        //alert(operation + this.FieldName);
        this.Operation = operation;
        if (this.TextValue != null) {
            this.Filterchangeevent.Stream.emit(this);
        }
    }
    timerToken: any;
    onTextChange(value) {
        //if (this.ObjectField.DataTypeCode == "Boolean" && (value == null || value == undefined)) {
        //    this.textValue = "false";
        //}
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (value != null && value.Date != null) {
            this.TextValue = value.Date;
            this.Operation = new NewViewObjectFieldOperator(value.Operation, value.Operation);
        }
        else if (value && value.FromDate && value.ToDate) {
            this.TextValue = value.FromDate;
            this.TextValue1 = value.ToDate;
            this.Operation = new NewViewObjectFieldOperator(value.Operation, value.Operation);
        }
        else if (value == "NoDate") {
            this.TextValue = "";
        }
        this.timerToken = setTimeout((event) => this.RunSearch(this.TextValue), 300);
    }

    RunSearch(event) {
        if (event != null) {
            if (event == true) {
                this.TextValue = "true";
            }
            else if (event == false && event != "") {
                this.TextValue = "false";
            }
            else {
                this.TextValue = event;
            }
            if (!this.iswidnowMode && this.TextValue != null) {
                this.Filterchangeevent.Stream.emit(this);
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
        this.ParentClass.DeteteFilter(this);
        if (!this.iswidnowMode && this.TextValue != null) {
            this.Filterchangeevent.Stream.emit(temp);
        }
        //this.Exists = false;

    }

    list: NewViewObjectFieldOperator[];
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




    startsWithOp: NewViewObjectFieldOperator = new NewViewObjectFieldOperator("StartsWith", "Starts With");
    equalsOp: NewViewObjectFieldOperator = new NewViewObjectFieldOperator("Equals", "Equals to");
    notEqualsOp: NewViewObjectFieldOperator = new NewViewObjectFieldOperator("NotEqual", "Not Equal to");
    largerThanOp: NewViewObjectFieldOperator = new NewViewObjectFieldOperator("LargerThan", "Greater Than");
    lessThanOp: NewViewObjectFieldOperator = new NewViewObjectFieldOperator("lessThanOp", "Less Than");
    greaterThanOrEqualOp: NewViewObjectFieldOperator = new NewViewObjectFieldOperator("GreaterThanOrEqual", "Greater Than Or Equal");
    lessThanOrEqualOp: NewViewObjectFieldOperator = new NewViewObjectFieldOperator("LessThanOrEqual", "Less Than Or Equal");
    BetweenOp: NewViewObjectFieldOperator = new NewViewObjectFieldOperator("Between", "Between");

}


export class NewViewFilterFieldsClass {
    public IsWindowMode: boolean;
    filterfield: NewViewFilterField;
    ParentClass: NewViewComponent;
   
    constructor(isWindowMode: boolean, parentClass: NewViewComponent) {
        this.ParentClass = parentClass;
        this.IsWindowMode = isWindowMode; 
        this.FilterFields = [];
    }

    AddNewViewFiltersList(objectsList: any, queryCode: string, AdvancedQueryFilterPMs: AdvancedQueryFilterPM[]) {
        //ObservableCollection
        if (this.ParentClass == null) {
            var ss = "";
        }
        var newList = [];
        //.sort((a, b) => { return (a.FieldName === b.FieldName) ? 0 : a ? -1 : 1 })
        objectsList.sort((a, b) => { return (a.FieldName === b.FieldName) ? 0 : (a.FieldName < b.FieldName) ? -1 : 1 }).forEach((item, key) => {
            newList.push(new NewViewFilterField(item, queryCode, this.IsWindowMode, AdvancedQueryFilterPMs, this.ParentClass));
        });

        this.FilterFields = newList;
    }

    SetExists(field: any, value: boolean) {
        this.filterfield = this.FilterFields.filter(f => f.ObjectField.FieldName == field.FieldName)[0];
        if (this.filterfield != null) {
            this.filterfield.NewField = false;
            this.filterfield.Exists = value;
        }
    }

    filterFields: any[];
    public get FilterFields() { return this.filterFields; }
    public set FilterFields(newValue: any[]) { this.filterFields = newValue; }


}

export class NewViewFieldsValues {

    FieldsDictionary: { [key: string]: any; } = {};

    GetFieldValue(key: string) {
        return this.FieldsDictionary[key];
    }

    SetFieldValue(key: string, value: any) {
        this.FieldsDictionary[key] = value;
    }





}

export class NewViewObjectFieldOperator {

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
