"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var FieldValueResolver_1 = require("../../../../../Infrastructure/Utilities/FieldValueResolver");
var ConditionFilterField = /** @class */ (function (_super) {
    __extends(ConditionFilterField, _super);
    function ConditionFilterField(objectField, ruleId, iswidnowMode, ruleConditionFieldPMs, parentClass, filterchangeevent) {
        if (parentClass === void 0) { parentClass = null; }
        if (filterchangeevent === void 0) { filterchangeevent = null; }
        var _this = _super.call(this) || this;
        _this.iswidnowMode = false;
        _this.ControlId = null;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.enableDelete = false;
        _this.newField = true;
        _this.isPreDefined = false;
        _this.isDeleted = false;
        _this.deleteButtonVisibility = false;
        _this.startsWithOp = new ObjectFieldOperator("StartsWith", "Starts With");
        _this.equalsOp = new ObjectFieldOperator("Equals", "Equals to");
        _this.notEqualsOp = new ObjectFieldOperator("NotEqual", "Not Equal to");
        _this.largerThanOp = new ObjectFieldOperator("LargerThan", "Greater Than");
        _this.lessThanOp = new ObjectFieldOperator("LessThan", "Less Than");
        _this.greaterThanOrEqualOp = new ObjectFieldOperator("GreaterThanOrEqual", "Greater Than Or Equal");
        _this.lessThanOrEqualOp = new ObjectFieldOperator("LessThanOrEqual", "Less Than Or Equal");
        _this.BetweenOp = new ObjectFieldOperator("Between", "Between");
        _this.ControlId = "CheckBox_" + _this.CurrentSession.GetNewId("CheckBox");
        _this.RuleId = ruleId;
        _this.Filterchangeevent = filterchangeevent;
        _this.ParentClass = parentClass;
        _this.RuleConditionFieldPMs = ruleConditionFieldPMs;
        _this.ObjectField = objectField;
        _this.ObjectTable = window.ObjectTables.filter(function (a) { return a.Id == _this.ObjectField.ObjectTableId; })[0];
        var translation = TextCodeTranslator_1.TextCodeTranslator.Translate(_this.ObjectField.FullNameTextCodeCode);
        _this.iswidnowMode = iswidnowMode;
        //if (translation != null && translation != undefined && translation != "") {
        //    this.FieldName = translation;
        //}
        //else {
        _this.FieldName = _this.ObjectField.FieldName;
        if (_this.RuleConditionFieldPMs) {
            var conditionField = _this.RuleConditionFieldPMs.filter(function (f) { return f.ObjectFieldId === objectField.Id; })[0];
            if (conditionField) {
                _this.RuleConditionFieldPM = conditionField;
                if (!Tools_1.AppTool.IsNullOrEmpty(conditionField.Value)) {
                    _this.EnableDelete = false;
                    _this.IsPreDefined = true;
                    //if (conditionField.Value.toLowerCase() == "false") {
                    //    this.TextValue = "false";
                    //}
                    //else if (conditionField.Value.toLowerCase() == "true") {
                    //    this.TextValue = "true";
                    //}
                    //else {
                    //    this.TextValue = conditionField.Value;
                    //}
                    _this.TextValue = FieldValueResolver_1.FieldValueResolver.GetFieldDataValue(_this.ObjectField, conditionField.Value);
                }
                _this.Operation = _this.Operators.filter(function (a) { return a.Code == conditionField.Operator; })[0];
                _this.EnableDelete = true;
            }
        }
        return _this;
    }
    Object.defineProperty(ConditionFilterField.prototype, "Filterchangeevent", {
        get: function () { return this.filterchangeevent; },
        set: function (newValue) { this.filterchangeevent = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConditionFilterField.prototype, "ObjectField", {
        get: function () { return this.objectField; },
        set: function (newValue) { this.objectField = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConditionFilterField.prototype, "ObjectTable", {
        get: function () { return this.objectTable; },
        set: function (newValue) { this.objectTable = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConditionFilterField.prototype, "RuleConditionFieldPM", {
        get: function () { return this.ruleConditionFieldPM; },
        set: function (newValue) { this.ruleConditionFieldPM = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConditionFilterField.prototype, "FieldName", {
        get: function () { return this.fieldName; },
        set: function (newValue) { this.fieldName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConditionFilterField.prototype, "EnableDelete", {
        get: function () { return this.enableDelete; },
        set: function (newValue) { this.enableDelete = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConditionFilterField.prototype, "NewField", {
        get: function () { return this.newField; },
        set: function (newValue) { this.newField = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConditionFilterField.prototype, "IsPreDefined", {
        get: function () { return this.isPreDefined; },
        set: function (newValue) { this.isPreDefined = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConditionFilterField.prototype, "Exists", {
        get: function () { return this.exists; },
        set: function (newValue) {
            var _this = this;
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
                    this.ParentClass.AddFilterField(this.ParentClass.MapJsonToEntityPM(this.ObjectField));
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
                    if (this.RuleConditionFieldPMs.filter(function (a) { return a.ObjectFieldId == _this.ObjectField.Id; }).length > 0) {
                        this.RuleConditionFieldPMs = this.RuleConditionFieldPMs.filter(function (a) { return a.ObjectFieldId != _this.ObjectField.Id; });
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
                this.ParentClass.AddFilterField(this.ParentClass.MapJsonToEntityPM(this.ObjectField));
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConditionFilterField.prototype, "IsDeleted", {
        get: function () { return this.isDeleted; },
        set: function (newValue) {
            this.isDeleted = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConditionFilterField.prototype, "Operation", {
        get: function () {
            if (!this.operation) {
                if ((this.ObjectField.DataTypeCode == "Text" || this.ObjectField.DataTypeCode == "nText") && Tools_1.AppTool.IsNullOrEmpty(this.operation)) {
                    this.operation = new ObjectFieldOperator("StartsWith", "Starts With");
                    return this.operation;
                }
                else {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.operation)) {
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
        },
        set: function (newValue) {
            this.operation = newValue;
            //this.Filterchangeevent.Stream.emit(this);
            //alert(this.operation + this.FieldName);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConditionFilterField.prototype, "Operators", {
        get: function () { return this.GetFieldOperators(this.ObjectField); },
        set: function (newValue) {
            this.operators = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConditionFilterField.prototype, "TextValue", {
        get: function () { return this.textValue; },
        set: function (newValue) {
            this.textValue = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConditionFilterField.prototype, "TextValue1", {
        get: function () { return this.textValue1; },
        set: function (newValue) {
            this.textValue1 = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConditionFilterField.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) {
            this.name = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConditionFilterField.prototype, "DeleteButtonVisibility", {
        get: function () { return this.deleteButtonVisibility; },
        set: function (newValue) {
            this.deleteButtonVisibility = newValue;
        },
        enumerable: true,
        configurable: true
    });
    ConditionFilterField.prototype.OperationValueChanged = function (operation) {
        //alert(operation + this.FieldName);
        this.Operation = operation;
        if (this.TextValue != null) {
            //this.Filterchangeevent.Stream.emit(this);
        }
    };
    ConditionFilterField.prototype.onTextChange = function (value) {
        var _this = this;
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
                    if (!Tools_1.AppTool.IsNullOrEmpty(value.Name)) {
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
            this.timerToken = setTimeout(function (event) { return _this.RunSearch(_this.TextValue); }, 300);
        }
    };
    ConditionFilterField.prototype.RunSearch = function (event) {
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
    };
    ConditionFilterField.prototype.onDeleteFilterClick = function () {
        var _this = this;
        //var filter = this.ParentClass.AdvancedQueryFilterPMs.filter(d => ((d.Tenant == SessionInfo.LoggedUserTenant && d.UserId == SessionInfo.LoggedUserId))).filter(d => d.ObjectFieldId == this.ObjectField.Id && d.QueryId == this.QueryId)[0];
        //this.AdvancedQueryFilterPM = filter;
        var temp = this;
        temp.IsDeleted = true;
        if (this.RuleConditionFieldPMs.filter(function (a) { return a.ObjectFieldId == _this.ObjectField.Id; }).length > 0) {
            this.RuleConditionFieldPMs = this.RuleConditionFieldPMs.filter(function (a) { return a.ObjectFieldId != _this.ObjectField.Id; });
        }
        this.ParentClass.DeteteFilter(this);
        if (this.TextValue != null) {
            //this.Filterchangeevent.Stream.emit(temp);
        }
        //this.Exists = false;
    };
    ConditionFilterField.prototype.GetFieldOperators = function (field) {
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
    };
    return ConditionFilterField;
}(BaseComponent_1.BaseComponent));
exports.ConditionFilterField = ConditionFilterField;
var ConditionFilterFieldsClass = /** @class */ (function () {
    function ConditionFilterFieldsClass(isWindowMode, parentClass, filterchangeevent) {
        if (filterchangeevent === void 0) { filterchangeevent = null; }
        this.event = null;
        this.ParentClass = parentClass;
        this.IsWindowMode = isWindowMode;
        this.event = filterchangeevent;
        this.FilterFields = [];
    }
    ConditionFilterFieldsClass.prototype.AddFiltersList = function (objectsList, ruleId, RuleConditionFieldPMs) {
        var _this = this;
        //ObservableCollection
        if (this.ParentClass == null) {
            var ss = "";
        }
        var newList = [];
        //.sort((a, b) => { return (a.FieldName === b.FieldName) ? 0 : a ? -1 : 1 })
        objectsList.sort(function (a, b) { return (a.FieldName === b.FieldName) ? 0 : (a.FieldName < b.FieldName) ? -1 : 1; }).forEach(function (item, key) {
            newList.push(new ConditionFilterField(item, ruleId, _this.IsWindowMode, RuleConditionFieldPMs, _this.ParentClass, _this.event));
        });
        this.FilterFields = newList.sort(function (a, b) { return (TextCodeTranslator_1.TextCodeTranslator.Translate(a.ObjectField.FullNameTextCodeCode).toLowerCase() === TextCodeTranslator_1.TextCodeTranslator.Translate(b.ObjectField.FullNameTextCodeCode).toLowerCase()) ? 0 : (TextCodeTranslator_1.TextCodeTranslator.Translate(a.ObjectField.FullNameTextCodeCode).toLowerCase() < TextCodeTranslator_1.TextCodeTranslator.Translate(b.ObjectField.FullNameTextCodeCode).toLowerCase()) ? -1 : 1; });
    };
    ConditionFilterFieldsClass.prototype.SetExists = function (field, value) {
        this.filterfield = this.FilterFields.filter(function (f) { return f.ObjectField.FieldName == field.FieldName; })[0];
        if (this.filterfield != null) {
            this.filterfield.NewField = false;
            this.filterfield.Exists = value;
        }
    };
    ConditionFilterFieldsClass.prototype.SetDeleted = function (field, value) {
        this.filterfield = this.FilterFields.filter(function (f) { return f.ObjectField.FieldName == field.FieldName; })[0];
        if (this.filterfield != null) {
            this.filterfield.NewField = false;
            this.filterfield.IsDeleted = value;
        }
    };
    Object.defineProperty(ConditionFilterFieldsClass.prototype, "FilterFields", {
        get: function () { return this.filterFields; },
        set: function (newValue) { this.filterFields = newValue; },
        enumerable: true,
        configurable: true
    });
    return ConditionFilterFieldsClass;
}());
exports.ConditionFilterFieldsClass = ConditionFilterFieldsClass;
var FieldsValues = /** @class */ (function () {
    function FieldsValues() {
        this.FieldsDictionary = {};
    }
    FieldsValues.prototype.GetFieldValue = function (key) {
        return this.FieldsDictionary[key];
    };
    FieldsValues.prototype.SetFieldValue = function (key, value) {
        this.FieldsDictionary[key] = value;
    };
    return FieldsValues;
}());
exports.FieldsValues = FieldsValues;
var ObjectFieldOperator = /** @class */ (function () {
    function ObjectFieldOperator(code, name) {
        this.Code = code;
        this.Name = name;
    }
    Object.defineProperty(ObjectFieldOperator.prototype, "Code", {
        get: function () { return this.code; },
        set: function (newValue) { this.code = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ObjectFieldOperator.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { this.name = newValue; },
        enumerable: true,
        configurable: true
    });
    return ObjectFieldOperator;
}());
exports.ObjectFieldOperator = ObjectFieldOperator;
//# sourceMappingURL=ConditionFilterField.js.map