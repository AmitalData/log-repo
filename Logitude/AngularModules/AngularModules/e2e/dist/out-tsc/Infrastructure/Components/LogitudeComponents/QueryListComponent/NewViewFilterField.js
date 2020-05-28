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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var NewViewFilterField = /** @class */ (function (_super) {
    __extends(NewViewFilterField, _super);
    function NewViewFilterField(objectField, queryId, iswidnowMode, AdvancedQFPMs, parentClass, filterchangeevent) {
        if (parentClass === void 0) { parentClass = null; }
        if (filterchangeevent === void 0) { filterchangeevent = null; }
        var _this = _super.call(this) || this;
        _this.iswidnowMode = false;
        _this.enableDelete = false;
        _this.newField = true;
        _this.isPreDefined = false;
        _this.isDeleted = false;
        _this.deleteButtonVisibility = false;
        _this.startsWithOp = new NewViewObjectFieldOperator("StartsWith", "Starts With");
        _this.equalsOp = new NewViewObjectFieldOperator("Equals", "Equals to");
        _this.notEqualsOp = new NewViewObjectFieldOperator("NotEqual", "Not Equal to");
        _this.largerThanOp = new NewViewObjectFieldOperator("LargerThan", "Greater Than");
        _this.lessThanOp = new NewViewObjectFieldOperator("lessThanOp", "Less Than");
        _this.greaterThanOrEqualOp = new NewViewObjectFieldOperator("GreaterThanOrEqual", "Greater Than Or Equal");
        _this.lessThanOrEqualOp = new NewViewObjectFieldOperator("LessThanOrEqual", "Less Than Or Equal");
        _this.BetweenOp = new NewViewObjectFieldOperator("Between", "Between");
        _this.QueryId = queryId;
        _this.Filterchangeevent = filterchangeevent;
        _this.ParentClass = parentClass;
        _this.AdvancedQueryFilterPMs = AdvancedQFPMs;
        _this.ObjectField = objectField;
        var translation = TextCodeTranslator_1.TextCodeTranslator.Translate(_this.ObjectField.FullNameTextCodeCode);
        _this.iswidnowMode = iswidnowMode;
        //if (translation != null && translation != undefined && translation != "") {
        //    this.FieldName = translation;
        //}
        //else {
        _this.FieldName = _this.ObjectField.FieldName;
        //}
        //SessionInfo.LoggedUserTenant, SessionInfo.LoggedUserId
        if (queryId != null && queryId != undefined && queryId != "") {
            var preDefinedFilter = _this.AdvancedQueryFilterPMs.filter(function (d) { return ((d.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant && d.UserId == SessionInfo_1.SessionInfo.LoggedUserId) || d.Tenant == 0) && d.IsPredefined == true; }).filter(function (d) { return d.ObjectFieldId == objectField.Id && d.QueryId == queryId; })[0];
            if (preDefinedFilter != null) {
                _this.AdvancedQueryFilterPM = preDefinedFilter;
                if (preDefinedFilter.PredefinedValue != null) {
                    _this.EnableDelete = false;
                    _this.IsPreDefined = true;
                    if (preDefinedFilter.PredefinedValue.toLowerCase() == "false") {
                        _this.TextValue = false;
                    }
                    else if (preDefinedFilter.PredefinedValue.toLowerCase() == "true") {
                        _this.TextValue = true;
                    }
                    else {
                        _this.TextValue = preDefinedFilter.PredefinedValue;
                    }
                }
                else {
                    _this.EnableDelete = true;
                }
            }
            else {
                _this.EnableDelete = true;
            }
        }
        return _this;
    }
    Object.defineProperty(NewViewFilterField.prototype, "ObjectField", {
        get: function () { return this.objectField; },
        set: function (newValue) { this.objectField = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewFilterField.prototype, "AdvancedQueryFilterPM", {
        get: function () { return this.advancedQueryFilterPM; },
        set: function (newValue) { this.advancedQueryFilterPM = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewFilterField.prototype, "FieldName", {
        get: function () { return this.fieldName; },
        set: function (newValue) { this.fieldName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewFilterField.prototype, "EnableDelete", {
        get: function () { return this.enableDelete; },
        set: function (newValue) { this.enableDelete = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewFilterField.prototype, "NewField", {
        get: function () { return this.newField; },
        set: function (newValue) { this.newField = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewFilterField.prototype, "IsPreDefined", {
        get: function () { return this.isPreDefined; },
        set: function (newValue) { this.isPreDefined = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewFilterField.prototype, "Exists", {
        get: function () { return this.exists; },
        set: function (newValue) {
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewFilterField.prototype, "IsDeleted", {
        get: function () { return this.isDeleted; },
        set: function (newValue) {
            this.isDeleted = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewFilterField.prototype, "Operation", {
        get: function () {
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
        },
        set: function (newValue) {
            this.operation = newValue;
            //this.Filterchangeevent.Stream.emit(this);
            //alert(this.operation + this.FieldName);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewFilterField.prototype, "Operators", {
        get: function () { return this.GetFieldOperators(this.ObjectField); },
        set: function (newValue) {
            this.operators = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewFilterField.prototype, "TextValue", {
        get: function () { return this.textValue; },
        set: function (newValue) {
            this.textValue = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewFilterField.prototype, "TextValue1", {
        get: function () { return this.textValue1; },
        set: function (newValue) {
            this.textValue1 = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewFilterField.prototype, "DeleteButtonVisibility", {
        get: function () { return this.deleteButtonVisibility; },
        set: function (newValue) {
            this.deleteButtonVisibility = newValue;
        },
        enumerable: true,
        configurable: true
    });
    NewViewFilterField.prototype.OperationValueChanged = function (operation) {
        //alert(operation + this.FieldName);
        this.Operation = operation;
        if (this.TextValue != null) {
            this.Filterchangeevent.Stream.emit(this);
        }
    };
    NewViewFilterField.prototype.onTextChange = function (value) {
        var _this = this;
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
        this.timerToken = setTimeout(function (event) { return _this.RunSearch(_this.TextValue); }, 300);
    };
    NewViewFilterField.prototype.RunSearch = function (event) {
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
    };
    NewViewFilterField.prototype.onDeleteFilterClick = function () {
        //var filter = this.ParentClass.AdvancedQueryFilterPMs.filter(d => ((d.Tenant == SessionInfo.LoggedUserTenant && d.UserId == SessionInfo.LoggedUserId))).filter(d => d.ObjectFieldId == this.ObjectField.Id && d.QueryId == this.QueryId)[0];
        //this.AdvancedQueryFilterPM = filter;
        var temp = this;
        temp.IsDeleted = true;
        this.ParentClass.DeteteFilter(this);
        if (!this.iswidnowMode && this.TextValue != null) {
            this.Filterchangeevent.Stream.emit(temp);
        }
        //this.Exists = false;
    };
    NewViewFilterField.prototype.GetFieldOperators = function (field) {
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
    return NewViewFilterField;
}(BaseComponent_1.BaseComponent));
exports.NewViewFilterField = NewViewFilterField;
var NewViewFilterFieldsClass = /** @class */ (function () {
    function NewViewFilterFieldsClass(isWindowMode, parentClass) {
        this.ParentClass = parentClass;
        this.IsWindowMode = isWindowMode;
        this.FilterFields = [];
    }
    NewViewFilterFieldsClass.prototype.AddNewViewFiltersList = function (objectsList, queryId, AdvancedQueryFilterPMs) {
        var _this = this;
        //ObservableCollection
        if (this.ParentClass == null) {
            var ss = "";
        }
        var newList = [];
        //.sort((a, b) => { return (a.FieldName === b.FieldName) ? 0 : a ? -1 : 1 })
        objectsList.sort(function (a, b) { return (a.FieldName === b.FieldName) ? 0 : (a.FieldName < b.FieldName) ? -1 : 1; }).forEach(function (item, key) {
            newList.push(new NewViewFilterField(item, queryId, _this.IsWindowMode, AdvancedQueryFilterPMs, _this.ParentClass));
        });
        this.FilterFields = newList;
    };
    NewViewFilterFieldsClass.prototype.SetExists = function (field, value) {
        this.filterfield = this.FilterFields.filter(function (f) { return f.ObjectField.FieldName == field.FieldName; })[0];
        if (this.filterfield != null) {
            this.filterfield.NewField = false;
            this.filterfield.Exists = value;
        }
    };
    Object.defineProperty(NewViewFilterFieldsClass.prototype, "FilterFields", {
        get: function () { return this.filterFields; },
        set: function (newValue) { this.filterFields = newValue; },
        enumerable: true,
        configurable: true
    });
    return NewViewFilterFieldsClass;
}());
exports.NewViewFilterFieldsClass = NewViewFilterFieldsClass;
var NewViewFieldsValues = /** @class */ (function () {
    function NewViewFieldsValues() {
        this.FieldsDictionary = {};
    }
    NewViewFieldsValues.prototype.GetFieldValue = function (key) {
        return this.FieldsDictionary[key];
    };
    NewViewFieldsValues.prototype.SetFieldValue = function (key, value) {
        this.FieldsDictionary[key] = value;
    };
    return NewViewFieldsValues;
}());
exports.NewViewFieldsValues = NewViewFieldsValues;
var NewViewObjectFieldOperator = /** @class */ (function () {
    function NewViewObjectFieldOperator(code, name) {
        this.Code = code;
        this.Name = name;
    }
    Object.defineProperty(NewViewObjectFieldOperator.prototype, "Code", {
        get: function () { return this.code; },
        set: function (newValue) { this.code = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewObjectFieldOperator.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { this.name = newValue; },
        enumerable: true,
        configurable: true
    });
    return NewViewObjectFieldOperator;
}());
exports.NewViewObjectFieldOperator = NewViewObjectFieldOperator;
//# sourceMappingURL=NewViewFilterField.js.map