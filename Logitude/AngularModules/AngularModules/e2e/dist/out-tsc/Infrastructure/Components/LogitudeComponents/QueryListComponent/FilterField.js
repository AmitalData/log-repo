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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var FeatureLocator_1 = require("../../../Utilities/FeatureLocator");
var FilterField = /** @class */ (function (_super) {
    __extends(FilterField, _super);
    function FilterField(objectField, queryId, iswidnowMode, AdvancedQFPMs, parentClass, filterchangeevent) {
        if (parentClass === void 0) { parentClass = null; }
        if (filterchangeevent === void 0) { filterchangeevent = null; }
        var _this = _super.call(this) || this;
        _this.iswidnowMode = false;
        _this.ControlId = null;
        _this.SessionIdx = 0;
        _this.IsCustomFilter = false;
        _this.IsFilterDeleteButtonVisible = false;
        _this.BooleanFiltersEnabled = false;
        _this.TextFiltersEnabled = false;
        _this.LOVFiltersEnabled = false;
        _this.DateFiltersEnabled = false;
        _this.PickFiltersEnabled = false;
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
        _this.SessionIdx = _this.CurrentSession.SessionIndex;
        _this.ControlId = "CheckBox_" + _this.CurrentSession.GetNewId("CheckBox");
        _this.QueryId = queryId;
        _this.Filterchangeevent = filterchangeevent;
        _this.ParentClass = parentClass;
        _this.AdvancedQueryFilterPMs = AdvancedQFPMs;
        _this.ObjectField = objectField;
        _this.ObjectTable = window.ObjectTables.filter(function (a) { return a.Id == _this.ObjectField.ObjectTableId; })[0];
        var translation = TextCodeTranslator_1.TextCodeTranslator.Translate(_this.ObjectField.FullNameTextCodeCode);
        _this.iswidnowMode = iswidnowMode;
        _this.FieldName = _this.ObjectField.FieldName;
        _this.IsCustomFilter = _this.ObjectField.IsCustomFilter;
        if (queryId != null && queryId != undefined && queryId != "") {
            var preDefinedFilter = _this.AdvancedQueryFilterPMs.filter(function (d) { return d.IsPredefined == true && d.ObjectFieldId == objectField.Id; })[0];
            if (preDefinedFilter != null) {
                _this.AdvancedQueryFilterPM = preDefinedFilter;
                if (preDefinedFilter.PredefinedValue != null) {
                    _this.EnableDelete = false;
                    _this.IsPreDefined = true;
                    if (preDefinedFilter.PredefinedValue.toLowerCase() == "false") {
                        _this.TextValue = "false";
                    }
                    else if (preDefinedFilter.PredefinedValue.toLowerCase() == "true") {
                        _this.TextValue = "true";
                    }
                    else {
                        _this.TextValue = preDefinedFilter.PredefinedValue;
                        _this.TextValue1 = preDefinedFilter.PredefinedValue2;
                    }
                }
                else {
                    _this.EnableDelete = true;
                }
                _this.Operation = _this.Operators.filter(function (a) { return a.Code == preDefinedFilter.Operator; })[0];
            }
            else {
                _this.EnableDelete = true;
            }
        }
        else {
            var preDefinedFilter = _this.AdvancedQueryFilterPMs.filter(function (d) { return ((d.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant && d.UserId == SessionInfo_1.SessionInfo.LoggedUserId) || d.Tenant == 0) && d.IsPredefined == true; }).filter(function (d) { return d.ObjectFieldId == objectField.Id; })[0];
            if (preDefinedFilter != null) {
                _this.AdvancedQueryFilterPM = preDefinedFilter;
                if (preDefinedFilter.PredefinedValue != null) {
                    _this.EnableDelete = false;
                    _this.IsPreDefined = true;
                    if (preDefinedFilter.PredefinedValue.toLowerCase() == "false") {
                        _this.TextValue = "false";
                    }
                    else if (preDefinedFilter.PredefinedValue.toLowerCase() == "true") {
                        _this.TextValue = "true";
                    }
                    else {
                        _this.TextValue = preDefinedFilter.PredefinedValue;
                        _this.TextValue1 = preDefinedFilter.PredefinedValue2;
                    }
                }
                else {
                    _this.EnableDelete = true;
                }
                _this.Operation = _this.Operators.filter(function (a) { return a.Code == preDefinedFilter.Operator; })[0];
            }
            else {
                _this.EnableDelete = true;
            }
        }
        var currentQuery = window.Queries.filter(function (d) { return d.Id == _this.QueryId; })[0];
        if (currentQuery != null) {
            if (!Tools_1.AppTool.IsNullOrEmpty(currentQuery.SharedByUserId) && currentQuery.SharedByUserId != SessionLocator_1.SessionLocator.LoggedUserId) {
                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "User.Feature.EditSharedViews")) {
                    if (_this.ObjectField) {
                        _this.TextFiltersEnabled = true;
                        _this.LOVFiltersEnabled = true;
                        _this.DateFiltersEnabled = true;
                        _this.PickFiltersEnabled = true;
                        if (_this.ObjectField.DataTypeCode != "Constant") {
                            _this.IsFilterDeleteButtonVisible = true;
                        }
                        if (!_this.IsCustomFilter) {
                            _this.BooleanFiltersEnabled = true;
                        }
                    }
                }
            }
            else {
                _this.TextFiltersEnabled = true;
                _this.IsFilterDeleteButtonVisible = true;
                _this.BooleanFiltersEnabled = true;
                _this.LOVFiltersEnabled = true;
                _this.DateFiltersEnabled = true;
                _this.PickFiltersEnabled = true;
            }
        }
        else {
            _this.TextFiltersEnabled = true;
            _this.IsFilterDeleteButtonVisible = true;
            _this.BooleanFiltersEnabled = true;
            _this.LOVFiltersEnabled = true;
            _this.DateFiltersEnabled = true;
            _this.PickFiltersEnabled = true;
        }
        if (_this.ObjectField.DataTypeCode == "Text" || _this.ObjectField.DataTypeCode == "nText" || _this.ObjectField.DataTypeCode == "Integer" || _this.ObjectField.DataTypeCode == "Double" || _this.ObjectField.DataTypeCode == "Decimal") {
            _this.UIProperties.SetEnabled("TextValue", null, _this.TextFiltersEnabled);
        }
        else if (_this.ObjectField.DataTypeCode == "LookUp") {
            _this.UIProperties.SetEnabled("TextValue", _this.ObjectTable.Name, _this.LOVFiltersEnabled);
        }
        else if (_this.ObjectField.DataTypeCode == "PickList") {
            _this.UIProperties.SetEnabled(_this.ObjectField.FieldName, _this.ObjectTable.Name, _this.PickFiltersEnabled);
        }
        return _this;
    }
    Object.defineProperty(FilterField.prototype, "Filterchangeevent", {
        get: function () { return this.filterchangeevent; },
        set: function (newValue) { this.filterchangeevent = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilterField.prototype, "ObjectField", {
        get: function () { return this.objectField; },
        set: function (newValue) { this.objectField = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilterField.prototype, "ObjectTable", {
        get: function () { return this.objectTable; },
        set: function (newValue) { this.objectTable = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilterField.prototype, "AdvancedQueryFilterPM", {
        get: function () { return this.advancedQueryFilterPM; },
        set: function (newValue) { this.advancedQueryFilterPM = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilterField.prototype, "FieldName", {
        get: function () { return this.fieldName; },
        set: function (newValue) { this.fieldName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilterField.prototype, "EnableDelete", {
        get: function () { return this.enableDelete; },
        set: function (newValue) { this.enableDelete = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilterField.prototype, "NewField", {
        get: function () { return this.newField; },
        set: function (newValue) { this.newField = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilterField.prototype, "IsPreDefined", {
        get: function () { return this.isPreDefined; },
        set: function (newValue) { this.isPreDefined = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilterField.prototype, "Exists", {
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
                    if (this.AdvancedQueryFilterPMs.filter(function (a) { return a.ObjectFieldId == _this.ObjectField.Id; }).length > 0) {
                        this.AdvancedQueryFilterPMs = this.AdvancedQueryFilterPMs.filter(function (a) { return a.ObjectFieldId != _this.ObjectField.Id; });
                    }
                    this.ParentClass.DeteteFilter(this);
                    //if (this.TextValue != null && this.TextValue != '') {
                    this.Filterchangeevent.Stream.emit(tempField);
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
    Object.defineProperty(FilterField.prototype, "IsDeleted", {
        get: function () { return this.isDeleted; },
        set: function (newValue) {
            this.isDeleted = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilterField.prototype, "Operation", {
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
    Object.defineProperty(FilterField.prototype, "Operators", {
        get: function () { return this.GetFieldOperators(this.ObjectField); },
        set: function (newValue) {
            this.operators = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilterField.prototype, "TextValue", {
        get: function () { return this.textValue; },
        set: function (newValue) {
            this.textValue = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilterField.prototype, "TextValue1", {
        get: function () { return this.textValue1; },
        set: function (newValue) {
            this.textValue1 = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilterField.prototype, "MyName", {
        get: function () { return this.name; },
        set: function (newValue) {
            this.name = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FilterField.prototype, "DeleteButtonVisibility", {
        get: function () { return this.deleteButtonVisibility; },
        set: function (newValue) {
            this.deleteButtonVisibility = newValue;
        },
        enumerable: true,
        configurable: true
    });
    FilterField.prototype.OperationValueChanged = function (operation) {
        //alert(operation + this.FieldName);
        this.Operation = operation;
        if (this.TextValue != null) {
            this.Filterchangeevent.Stream.emit(this);
        }
    };
    FilterField.prototype.onTextChange = function (value) {
        var _this = this;
        if (value != "true" && value != "false") {
            if (this.timerToken) {
                clearTimeout(this.timerToken);
            }
            if (value != null && value.MyName != null) {
                this.TextValue = value.MyName;
                this.MyName = value.MyName;
                this.Operation = new ObjectFieldOperator(value.Operation, value.Operation);
            }
            else if (value != null && value.Date != null) {
                this.TextValue = value.Date;
                this.MyName = null;
                this.Operation = new ObjectFieldOperator(value.Operation, value.Operation);
            }
            else if (value && value.FromDate && value.ToDate) {
                this.TextValue = value.FromDate;
                this.TextValue1 = value.ToDate;
                if (this.AdvancedQueryFilterPM) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(value.MyName)) {
                        this.AdvancedQueryFilterPM.PredefinedValue = value.MyName;
                    }
                    else {
                        this.AdvancedQueryFilterPM.PredefinedValue = value.FromDate;
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
    FilterField.prototype.RunSearch = function (event) {
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
                this.Filterchangeevent.Stream.emit(this);
            }
        }
        else {
            this.TextValue = "";
        }
        // this.textchangeevent.emit(this);
    };
    FilterField.prototype.onDeleteFilterClick = function () {
        var _this = this;
        //var filter = this.ParentClass.AdvancedQueryFilterPMs.filter(d => ((d.Tenant == SessionInfo.LoggedUserTenant && d.UserId == SessionInfo.LoggedUserId))).filter(d => d.ObjectFieldId == this.ObjectField.Id && d.QueryId == this.QueryId)[0];
        //this.AdvancedQueryFilterPM = filter;
        var temp = this;
        temp.IsDeleted = true;
        if (this.AdvancedQueryFilterPMs.filter(function (a) { return a.ObjectFieldId == _this.ObjectField.Id; }).length > 0) {
            this.AdvancedQueryFilterPMs = this.AdvancedQueryFilterPMs.filter(function (a) { return a.ObjectFieldId != _this.ObjectField.Id; });
        }
        this.ParentClass.DeteteFilter(this);
        if (this.TextValue != null) {
            this.Filterchangeevent.Stream.emit(temp);
        }
        //this.Exists = false;
    };
    FilterField.prototype.GetFieldOperators = function (field) {
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
            if (field.DataTypeCode == "DateTime" || field.DataTypeCode == "Date") {
                this.list.push(this.BetweenOp);
            }
        }
        if (field.DataTypeCode == "LookUp" || field.DataTypeCode == "PickList" || field.DataTypeCode == "Boolean") {
            this.list.push(this.equalsOp);
            this.list.push(this.notEqualsOp);
        }
        return this.list;
    };
    return FilterField;
}(BaseComponent_1.BaseComponent));
exports.FilterField = FilterField;
var FilterFieldsClass = /** @class */ (function () {
    function FilterFieldsClass(isWindowMode, parentClass, filterchangeevent) {
        if (filterchangeevent === void 0) { filterchangeevent = null; }
        this.event = null;
        this.ParentClass = parentClass;
        this.IsWindowMode = isWindowMode;
        this.event = filterchangeevent;
        this.FilterFields = [];
    }
    FilterFieldsClass.prototype.AddFiltersList = function (objectsList, queryId, AdvancedQueryFilterPMs) {
        var _this = this;
        //ObservableCollection
        if (this.ParentClass == null) {
            var ss = "";
        }
        var newList = [];
        //.sort((a, b) => { return (a.FieldName === b.FieldName) ? 0 : a ? -1 : 1 })
        objectsList.sort(function (a, b) { return (a.FieldName === b.FieldName) ? 0 : (a.FieldName < b.FieldName) ? -1 : 1; }).forEach(function (item, key) {
            newList.push(new FilterField(item, queryId, _this.IsWindowMode, AdvancedQueryFilterPMs, _this.ParentClass, _this.event));
        });
        this.FilterFields = newList.sort(function (a, b) { return (TextCodeTranslator_1.TextCodeTranslator.Translate(a.ObjectField.FullNameTextCodeCode).toLowerCase() === TextCodeTranslator_1.TextCodeTranslator.Translate(b.ObjectField.FullNameTextCodeCode).toLowerCase()) ? 0 : (TextCodeTranslator_1.TextCodeTranslator.Translate(a.ObjectField.FullNameTextCodeCode).toLowerCase() < TextCodeTranslator_1.TextCodeTranslator.Translate(b.ObjectField.FullNameTextCodeCode).toLowerCase()) ? -1 : 1; });
    };
    FilterFieldsClass.prototype.SetExists = function (field, value) {
        this.filterfield = this.FilterFields.filter(function (f) { return f.ObjectField.FieldName == field.FieldName; })[0];
        if (this.filterfield != null) {
            this.filterfield.NewField = false;
            this.filterfield.Exists = value;
        }
    };
    FilterFieldsClass.prototype.SetDeleted = function (field, value) {
        this.filterfield = this.FilterFields.filter(function (f) { return f.ObjectField.FieldName == field.FieldName; })[0];
        if (this.filterfield != null) {
            this.filterfield.NewField = false;
            this.filterfield.IsDeleted = value;
        }
    };
    Object.defineProperty(FilterFieldsClass.prototype, "FilterFields", {
        get: function () { return this.filterFields; },
        set: function (newValue) { this.filterFields = newValue; },
        enumerable: true,
        configurable: true
    });
    return FilterFieldsClass;
}());
exports.FilterFieldsClass = FilterFieldsClass;
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
//# sourceMappingURL=FilterField.js.map