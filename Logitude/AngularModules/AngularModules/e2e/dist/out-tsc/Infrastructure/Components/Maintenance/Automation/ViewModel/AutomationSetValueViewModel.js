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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var FieldValueResolver_1 = require("../../../../../Infrastructure/Utilities/FieldValueResolver");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var AutomationHelper_1 = require("../../../../../Infrastructure/Helpers/AutomationHelper");
var AutomationSetValueViewModel = /** @class */ (function (_super) {
    __extends(AutomationSetValueViewModel, _super);
    function AutomationSetValueViewModel(entityPM, addEditAutomationsViewModel) {
        var _this = _super.call(this) || this;
        _this.IsLoadOperatorList = false;
        _this.IsNewEntityCall = false;
        _this.IsMultiline = false;
        _this.IsRefrachCustomField = false;
        _this.IsHideGeneralControl = false;
        _this.IsCustomCombox = false;
        _this.IsModeDate = false;
        _this.ObjectFieldId = "";
        _this.CustomObjectFieldId = "";
        _this.IsRefreshObjectFieldLov = false;
        _this.isChecked = false;
        _this.CurrentEntityPM = entityPM;
        _this.AddEditAutomationsViewModel = addEditAutomationsViewModel;
        _this.AutomationHelper = new AutomationHelper_1.AutomationHelper(_this.CurrentEntityPM, _this.AddEditAutomationsViewModel, _this, "SetValue");
        _this.InitLOVFilters();
        _this.AutomationSetValuebjectFieldLists = addEditAutomationsViewModel.AutomationSetValuebjectFieldLists;
        var objectField = _this.AutomationSetValuebjectFieldLists.filter(function (d) { return d.Id == _this.CurrentEntityPM.ObjectFieldId; })[0];
        _this.FieldValue = _this.CurrentEntityPM.Value;
        _this.DateTypeList = [];
        _this.DateTypeList.push(new Operator("@Today-", "-"));
        _this.DateTypeList.push(new Operator("@Today+", "+"));
        _this.DateTypeList.push(new Operator("Date", "Date"));
        _this.SelectedDateType = _this.DateTypeList[0];
        _this.OperatorList = [];
        _this.OperatorList.push(new Operator("Set Constant Value", "SV"));
        _this.OperatorList.push(new Operator("Set Value From [Field]", "SF"));
        if (objectField) {
            _this.ObjectFieldId = objectField.Id;
            _this.SelectedCustomField = objectField;
            _this.UIProperties.SetEnabled(_this.SelectedCustomField.FieldName, _this.AddEditAutomationsViewModel.ObjectTableName, true);
            _this.UIProperties.SetRequired(_this.SelectedCustomField.FieldName, _this.AddEditAutomationsViewModel.ObjectTableName, false);
            if (objectField.DataTypeCode == "DateTime" || objectField.DataTypeCode == "Date") {
                if (_this.CurrentEntityPM.OperatorCode && _this.CurrentEntityPM.OperatorCode.indexOf("F") == -1) {
                    if (_this.FieldValue) {
                        var values = _this.FieldValue.split("*");
                        _this.SelectedDateType = _this.DateTypeList.filter(function (d) { return d.Name == values[0]; })[0];
                        if (_this.FieldValue.indexOf("Date") != -1) {
                            if (values.length > 1)
                                _this.FieldValue = FieldValueResolver_1.FieldValueResolver.ConvertToDate(values[1], "Automation");
                            else {
                                _this.FieldValue = null;
                            }
                            _this.IsModeDate = true;
                        }
                        else {
                            _this.FieldValue = 0;
                            if (values.length > 1)
                                _this.FieldValue = Number(values[1]);
                            _this.IsModeDate = false;
                        }
                    }
                    else {
                        _this.FieldValue = Tools_1.DateTool.GetCurrentDateAsUtc();
                    }
                }
            }
        }
        if (_this.CurrentEntityPM != null) {
            _this.SelectedOperator = _this.OperatorList.filter(function (d) { return d.Code == _this.CurrentEntityPM.OperatorCode; })[0];
        }
        if (!_this.SelectedOperator) {
            _this.SelectedOperator = _this.OperatorList.filter(function (d) { return d.Code == "SV"; })[0];
        }
        if (_this.SelectedOperator) {
            if (_this.SelectedOperator.Code == "SF") {
                _this.BuildCustomFromFieldbjectFieldLists();
            }
        }
        _this.IsLoadOperatorList = true;
        return _this;
    }
    AutomationSetValueViewModel.prototype.ngOnInit = function () {
    };
    AutomationSetValueViewModel.prototype.InitLOVFilters = function () {
        this.AutomationCondationFieldListFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        this.AutomationCondationFieldListFilterItems.addAdditionalFilter("ObjectTableId", this.AddEditAutomationsViewModel.ObjectTableId, null, null, "Equals", false, false, false, "string");
        this.AutomationCondationFieldListFilterItems.addAdditionalFilter("CanAutomateSetValue", true, null, null, "Equals", false, false, false, "boolean");
    };
    AutomationSetValueViewModel.prototype.InitCustomLOVFilters = function (objectFieldPM) {
        this.CustomAutomationCondationFieldListFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        this.CustomAutomationCondationFieldListFilterItems.addAdditionalFilter("ObjectTableId", this.AddEditAutomationsViewModel.ObjectTableId, null, null, "Equals", false, false, false, "string");
        this.CustomAutomationCondationFieldListFilterItems.addAdditionalFilter("CanAutomateSetValue", true, null, null, "Equals", false, false, false, "boolean");
        if (this.SelectedCustomField) {
            if (this.SelectedCustomField.DataTypeCode == "LookUp") {
                this.CustomAutomationCondationFieldListFilterItems.addAdditionalFilter("LookUpTableId", this.SelectedCustomField.LookUpTableId, null, null, "Equals", false, false, false, "string");
            }
            this.CustomAutomationCondationFieldListFilterItems.addAdditionalFilter("DataTypeCode", this.SelectedCustomField.DataTypeCode, null, null, "Equals", false, false, false, "string");
        }
    };
    AutomationSetValueViewModel.prototype.BuildCustomFromFieldbjectFieldLists = function () {
        if (this.SelectedOperator != null && this.SelectedCustomField) {
            if (this.SelectedOperator.Code.indexOf("F") != -1) {
                this.IsCustomCombox = true;
                if (this.SelectedCustomField) {
                    this.CustomObjectFieldId = this.FieldValue;
                    this.InitCustomLOVFilters(this.SelectedCustomField);
                }
                this.IsRefreshObjectFieldLov = !this.IsRefreshObjectFieldLov;
            }
        }
    };
    Object.defineProperty(AutomationSetValueViewModel.prototype, "IsChecked", {
        get: function () {
            if (this.SelectedCustomField && this.CurrentEntityPM) {
                if (this.SelectedCustomField.DataTypeCode == "Boolean") {
                    this.isChecked = this.CurrentEntityPM.Value == "true" ? true : false;
                }
            }
            return this.isChecked;
        },
        set: function (newValue) {
            this.isChecked = newValue;
            this.CurrentEntityPM.Value = this.FieldValue = this.isChecked ? "true" : "false";
            this.AddEditAutomationsViewModel.IsChangeSetValue = true;
        },
        enumerable: true,
        configurable: true
    });
    AutomationSetValueViewModel.prototype.CustomFieldValueChanged = function (item) {
        if (item) {
            var oldObjectFieldId = this.SelectedCustomField ? this.SelectedCustomField.Id : "";
            if (item.Id != oldObjectFieldId) {
                this.FieldValue = "";
                this.CurrentEntityPM.Value = "";
                this.CustomObjectFieldId = "";
                this.AddEditAutomationsViewModel.IsChangeSetValue = true;
                var ischange = false;
                if (this.SelectedCustomField && this.SelectedCustomField.DataTypeCode == item.DataTypeCode) {
                    ischange = true;
                }
                this.SelectedCustomField = null;
                this.SelectedCustomField = this.AutomationSetValuebjectFieldLists.filter(function (d) { return d.Id == item.Id; })[0];
                this.IsCustomCombox = false;
                if (this.SelectedCustomField) {
                    this.UIProperties.SetEnabled(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, true);
                    this.UIProperties.SetRequired(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, false);
                    this.CurrentEntityPM.ObjectFieldId = this.SelectedCustomField.Id;
                    this.CurrentEntityPM.FieldName = this.SelectedCustomField.FieldName;
                    this.CurrentEntityPM.DataTypeCode = this.SelectedCustomField.DataTypeCode;
                }
                this.IsSetValue = false;
                if (ischange) {
                    this.IsRefrachCustomField = !this.IsRefrachCustomField;
                }
                this.BuildCustomFromFieldbjectFieldLists();
            }
        }
    };
    AutomationSetValueViewModel.prototype.DeleteAutomationConditionMethod = function (item) {
        if (this.AddEditAutomationsViewModel.AutomationSetValueLists) {
            var index = this.AddEditAutomationsViewModel.AutomationSetValueLists.indexOf(item);
            if (index != -1)
                this.AddEditAutomationsViewModel.AutomationSetValueLists.splice(index, 1);
        }
        this.AddEditAutomationsViewModel.IsChangeSetValue = true;
    };
    AutomationSetValueViewModel.prototype.OperatorListValueChanged = function (item) {
        if (item) {
            if ((item.Code.indexOf("F") != -1 && this.SelectedOperator.Code.indexOf("F") == -1) || (item.Code.indexOf("F") == -1 && this.SelectedOperator.Code.indexOf("F") != -1)) {
                this.FieldValue = "";
                this.CurrentEntityPM.Value = "";
            }
            this.SelectedOperator = item;
            this.AddEditAutomationsViewModel.IsChangeSetValue = true;
            this.CurrentEntityPM.OperatorCode = this.SelectedOperator.Code;
            if (this.CurrentEntityPM.OperatorCode == "SF") {
                this.BuildCustomFromFieldbjectFieldLists();
            }
            else {
                this.IsCustomCombox = false;
            }
        }
    };
    AutomationSetValueViewModel.prototype.ObjectFieldCondationValueChange = function (value) {
        this.AutomationHelper.ObjectFieldCondationValueChange(value);
    };
    AutomationSetValueViewModel.prototype.TextBoxCondationValueChange = function (value) {
        this.AutomationHelper.TextBoxCondationValueChange(value);
    };
    AutomationSetValueViewModel.prototype.LogLovCondationValueChange = function (value) {
        this.AutomationHelper.LogLovCondationValueChange(value);
    };
    AutomationSetValueViewModel.prototype.DateTypeListCondationValueChanged = function (value) {
        this.AutomationHelper.DateTypeListCondationValueChanged(value);
    };
    AutomationSetValueViewModel.prototype.DatePickerCondationValueChange = function (value) {
        this.AutomationHelper.DatePickerCondationValueChange(value);
    };
    AutomationSetValueViewModel.prototype.NumericUpDownCondationValueChanged = function () {
        this.AutomationHelper.NumericUpDownCondationValueChanged();
    };
    return AutomationSetValueViewModel;
}(BaseComponent_1.BaseComponent));
exports.AutomationSetValueViewModel = AutomationSetValueViewModel;
var Operator = /** @class */ (function () {
    function Operator(name, code) {
        this.Code = code;
        this.Name = name;
    }
    return Operator;
}());
//# sourceMappingURL=AutomationSetValueViewModel.js.map