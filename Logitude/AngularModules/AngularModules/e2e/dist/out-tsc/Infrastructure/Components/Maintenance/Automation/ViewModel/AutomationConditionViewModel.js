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
var AutomationConditionViewModel = /** @class */ (function (_super) {
    __extends(AutomationConditionViewModel, _super);
    function AutomationConditionViewModel(entityPM, addEditAutomationsViewModel, delayAutomationconditionsViewModel) {
        if (delayAutomationconditionsViewModel === void 0) { delayAutomationconditionsViewModel = null; }
        var _this = _super.call(this) || this;
        _this.IsLoadOperatorList = false;
        _this.IsNewEntityCall = false;
        _this.IsMultiline = false;
        _this.IsRefrachCustomField = false;
        _this.IsRefreshObjectFieldLov = false;
        _this.IsModeDate = false;
        _this.IsCustomCombox = false;
        _this.ObjectFieldId = "";
        //IsSystemVariables: boolean = false;
        _this.CustomObjectFieldId = "";
        _this.isChecked = false;
        _this.IsHideGeneralControl = false;
        _this.CurrentEntityPM = entityPM;
        _this.AddEditAutomationsViewModel = addEditAutomationsViewModel;
        _this.DelayAutomationconditionsViewModel = delayAutomationconditionsViewModel;
        _this.AutomationHelper = new AutomationHelper_1.AutomationHelper(_this.CurrentEntityPM, _this.AddEditAutomationsViewModel, _this, "Condation");
        _this.InitLOVFilters();
        _this.AllowedinAutomationConditionsFieldLists = addEditAutomationsViewModel.AllowedinAutomationConditionsFieldLists;
        _this.ObjectFieldPM = _this.AllowedinAutomationConditionsFieldLists.filter(function (d) { return d.Id == _this.CurrentEntityPM.ObjectFieldId; })[0];
        _this.FieldValue = _this.CurrentEntityPM.Value;
        _this.DateTypeList = [];
        _this.DateTypeList.push(new Operator("@Today-", "-"));
        _this.DateTypeList.push(new Operator("@Today+", "+"));
        _this.DateTypeList.push(new Operator("Date", "Date"));
        _this.SelectedDateType = _this.DateTypeList[0];
        _this.CurrentEntityType = _this.AddEditAutomationsViewModel.CurrentEntityPM.Type;
        _this.SystemVariableOperatorLists = [];
        _this.SystemVariableOperatorLists.push(new Operator("System User", "SystemUser"));
        if (_this.ObjectFieldPM) {
            _this.ChosenOperatorList(_this.ObjectFieldPM.DataTypeCode, false, _this.ObjectFieldPM);
            _this.ObjectFieldId = _this.ObjectFieldPM.Id;
            _this.SelectedCustomField = _this.ObjectFieldPM;
            _this.UIProperties.SetEnabled(_this.SelectedCustomField.FieldName, _this.AddEditAutomationsViewModel.ObjectTableName, true);
            _this.UIProperties.SetRequired(_this.SelectedCustomField.FieldName, _this.AddEditAutomationsViewModel.ObjectTableName, false);
            if (_this.CurrentEntityPM)
                _this.CurrentEntityPM.ObjectFieldType = _this.SelectedCustomField.DataTypeCode;
            if (_this.ObjectFieldPM.DataTypeCode == "DateTime" || _this.ObjectFieldPM.DataTypeCode == "Date") {
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
                        _this.FieldValue = new Date();
                    }
                }
            }
            if (_this.CurrentEntityPM.Value && _this.CurrentEntityPM.Value.indexOf("@StatusName:") > -1) {
                _this.FieldValue = _this.CurrentEntityPM.Value.split("@StatusName:")[0];
                _this.AddEditAutomationsViewModel.IsChangeCondition = true;
            }
        }
        else {
            _this.ChosenOperatorList("Text", false);
        }
        if (_this.CurrentEntityPM != null) {
            _this.SelectedOperator = _this.OperatorList.filter(function (d) { return d.Code == _this.CurrentEntityPM.OperatorCode; })[0];
        }
        if (!_this.SelectedOperator) {
            _this.SelectedOperator = _this.OperatorList.filter(function (d) { return d.Code == "="; })[0];
            if (_this.SelectedOperator) {
                _this.CurrentEntityPM.OperatorCode = _this.SelectedOperator.Code;
            }
        }
        if (_this.SelectedOperator) {
            _this.BuildCustomFromFieldbjectFieldLists();
            if (_this.HideGeneralControl(_this.SelectedOperator.Code)) {
                _this.IsHideGeneralControl = true;
                _this.FieldValue = "";
                _this.CurrentEntityPM.Value = "";
            }
            if (_this.SelectedOperator.Code == "EqualSystemVariable") {
                _this.SelectedSystemVariableOperator = _this.SystemVariableOperatorLists.filter(function (d) { return d.Code == _this.FieldValue; })[0];
                if (!_this.SelectedSystemVariableOperator) {
                    _this.SelectedSystemVariableOperator = _this.SystemVariableOperatorLists[0];
                    _this.AutomationHelper.ConditionValueChange(_this.SelectedSystemVariableOperator.Code);
                }
            }
        }
        _this.IsLoadOperatorList = true;
        return _this;
    }
    AutomationConditionViewModel.prototype.ngOnInit = function () {
    };
    Object.defineProperty(AutomationConditionViewModel.prototype, "IsSystemVariables", {
        get: function () {
            var result = false;
            if (this.SelectedOperator) {
                if (this.SelectedOperator.Code == "EqualSystemVariable")
                    result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    AutomationConditionViewModel.prototype.InitLOVFilters = function () {
        this.AutomationCondationFieldListFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        this.AutomationCondationFieldListFilterItems.addAdditionalFilter("ObjectTableId", this.AddEditAutomationsViewModel.ObjectTableId, null, null, "Equals", false, false, false, "string");
        this.AutomationCondationFieldListFilterItems.addAdditionalFilter("AllowedinAutomationConditions", true, null, null, "Equals", true, false, false, "boolean");
        this.AutomationCondationFieldListFilterItems.Tenant = 0;
    };
    AutomationConditionViewModel.prototype.InitCustomLOVFilters = function (objectFieldPM) {
        this.CustomAutomationCondationFieldListFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        this.CustomAutomationCondationFieldListFilterItems.addAdditionalFilter("ObjectTableId", this.AddEditAutomationsViewModel.ObjectTableId, null, null, "Equals", false, false, false, "string");
        this.CustomAutomationCondationFieldListFilterItems.addAdditionalFilter("AllowedinAutomationConditions", true, null, null, "Equals", true, false, false, "boolean");
        if (this.SelectedCustomField) {
            if (this.SelectedCustomField.DataTypeCode == "LookUp") {
                this.CustomAutomationCondationFieldListFilterItems.addAdditionalFilter("LookUpTableId", this.SelectedCustomField.LookUpTableId, null, null, "Equals", false, false, false, "string");
            }
            this.CustomAutomationCondationFieldListFilterItems.addAdditionalFilter("DataTypeCode", this.SelectedCustomField.DataTypeCode, null, null, "Equals", false, false, false, "string");
        }
    };
    Object.defineProperty(AutomationConditionViewModel.prototype, "IsChecked", {
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
            this.AddEditAutomationsViewModel.IsChangeCondition = true;
        },
        enumerable: true,
        configurable: true
    });
    AutomationConditionViewModel.prototype.ChosenOperatorList = function (dataTypeCode, isChangeOperator, objectFieldPM) {
        if (objectFieldPM === void 0) { objectFieldPM = null; }
        this.OperatorList = [];
        if (dataTypeCode == "DateTime" || dataTypeCode == "Date" || dataTypeCode == "Integer" || dataTypeCode == "Decimal" || dataTypeCode == "Double") {
            this.OperatorList.push(new Operator("=", "="));
            this.OperatorList.push(new Operator("<>", "<>"));
            this.OperatorList.push(new Operator(">", ">"));
            this.OperatorList.push(new Operator("<", "<"));
            this.OperatorList.push(new Operator(">=", ">="));
            this.OperatorList.push(new Operator("<=", "<="));
            this.OperatorList.push(new Operator("= [Field]", "=F"));
            this.OperatorList.push(new Operator("> [Field]", ">F"));
            this.OperatorList.push(new Operator("< [Field]", "<F"));
            this.OperatorList.push(new Operator("<> [Field]", "<>F"));
            this.OperatorList.push(new Operator("<= [Field]", "<=F"));
            this.OperatorList.push(new Operator(">= [Field]", ">=F"));
        }
        else if (dataTypeCode == "Boolean")
            this.OperatorList.push(new Operator("Equals", "="));
        else if (dataTypeCode == "LookUp") {
            this.OperatorList.push(new Operator("Equals", "="));
            this.OperatorList.push(new Operator("Does Not Equal", "<>"));
            this.OperatorList.push(new Operator("Equal [Field]", "=F"));
            this.OperatorList.push(new Operator("Does Not Equal [Field]", "<>F"));
            if (objectFieldPM != null) {
                if (objectFieldPM.ObjectTable_LookUpTableName == "User" || objectFieldPM.ObjectTable_LookUpTableName == "Contact") {
                    this.OperatorList.push(new Operator("Equal [System Variable]", "EqualSystemVariable"));
                }
            }
        }
        else {
            this.OperatorList.push(new Operator("Equals", "="));
            this.OperatorList.push(new Operator("Does Not Equal", "<>"));
            this.OperatorList.push(new Operator("Contains", "CONTAINS"));
            this.OperatorList.push(new Operator("Does Not Contain", "!CONTAINS"));
            this.OperatorList.push(new Operator("Equal [Field]", "=F"));
            this.OperatorList.push(new Operator("Does Not Equal [Field]", "<>F"));
            this.OperatorList.push(new Operator("Contains [Field]", "CONTAINSF"));
            this.OperatorList.push(new Operator("Does Not Contain [Field]", "!CONTAINSF"));
        }
        if (this.CurrentEntityType != "OnCreate") {
            this.OperatorList.push(new Operator("Changed to", "CHANGEDTO"));
            this.OperatorList.push(new Operator("Changed", "CHANGED"));
        }
        if (dataTypeCode == "DateTime" || dataTypeCode == "Date") {
            if (!this.OperatorList.filter(function (d) { return d.Code == "CHANGED"; })[0]) {
                this.OperatorList.push(new Operator("Changed", "CHANGED"));
            }
            this.OperatorList.push(new Operator("Is Empty", "ISEMPTY"));
            this.OperatorList.push(new Operator("Is not Empty", "ISNOTEMPTY"));
        }
        if (isChangeOperator) {
            this.CurrentEntityPM.OperatorCode = this.OperatorList[0] ? this.OperatorList[0].Code : "=";
        }
    };
    AutomationConditionViewModel.prototype.CustomFieldValueChanged = function (item) {
        {
            if (item) {
                var oldObjectFieldId = this.SelectedCustomField ? this.SelectedCustomField.Id : "";
                if (item.Id != oldObjectFieldId) {
                    this.IsHideGeneralControl = false;
                    this.FieldValue = item.DataTypeCode == "Date" || item.DataTypeCode == "DateTime" ? 0 : "";
                    if (item.DataTypeCode == "Date" || item.DataTypeCode == "DateTime") {
                        var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                        this.CurrentEntityPM.Value = this.SelectedDateType.Name + "*" + this.FieldValue + "*" + FieldValueResolver_1.FieldValueResolver.ConvertUTCDateToString(todayDate, "Automation");
                    }
                    else
                        this.CurrentEntityPM.Value = "";
                    this.AddEditAutomationsViewModel.IsChangeCondition = true;
                    var ischange = false;
                    if (this.SelectedCustomField && this.SelectedCustomField.DataTypeCode == item.DataTypeCode) {
                        ischange = true;
                    }
                    this.SelectedCustomField = this.AllowedinAutomationConditionsFieldLists.filter(function (d) { return d.Id == item.Id; })[0];
                    this.IsCustomCombox = false;
                    if (this.SelectedCustomField) {
                        this.UIProperties.SetEnabled(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, true);
                        this.UIProperties.SetRequired(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, false);
                        this.CurrentEntityPM.ObjectFieldId = this.SelectedCustomField.Id;
                        this.ChosenOperatorList(this.SelectedCustomField.DataTypeCode, true, this.SelectedCustomField);
                        this.SelectedOperator = this.OperatorList[0];
                    }
                    this.IsSetValue = false;
                    this.CurrentEntityPM.ObjectFieldType = this.SelectedCustomField.DataTypeCode;
                    if (ischange) {
                        this.IsRefrachCustomField = !this.IsRefrachCustomField;
                    }
                    this.BuildCustomFromFieldbjectFieldLists();
                }
            }
        }
    };
    AutomationConditionViewModel.prototype.DeleteAutomationConditionMethod = function (item) {
        if (this.CurrentEntityPM.ConditionType == "And") {
            if (this.DelayAutomationconditionsViewModel) {
                var index = this.DelayAutomationconditionsViewModel.AutomationCondationAndList.indexOf(item);
                if (index != -1)
                    this.DelayAutomationconditionsViewModel.AutomationCondationAndList.splice(index, 1);
            }
            else {
                var index = this.AddEditAutomationsViewModel.AutomationCondationAndList.indexOf(item);
                if (index != -1)
                    this.AddEditAutomationsViewModel.AutomationCondationAndList.splice(index, 1);
            }
        }
        else {
            if (this.DelayAutomationconditionsViewModel) {
                var index = this.DelayAutomationconditionsViewModel.AutomationCondationOrList.indexOf(item);
                if (index != -1)
                    this.DelayAutomationconditionsViewModel.AutomationCondationOrList.splice(index, 1);
            }
            else {
                var index = this.AddEditAutomationsViewModel.AutomationCondationOrList.indexOf(item);
                if (index != -1)
                    this.AddEditAutomationsViewModel.AutomationCondationOrList.splice(index, 1);
            }
        }
        this.AddEditAutomationsViewModel.IsChangeCondition = true;
    };
    AutomationConditionViewModel.prototype.HideGeneralControl = function (operatorCode) {
        var result = false;
        if (operatorCode == "CHANGED" || operatorCode == "ISEMPTY" || operatorCode == "ISNOTEMPTY") {
            result = true;
        }
        return result;
    };
    AutomationConditionViewModel.prototype.OperatorListValueChanged = function (item) {
        if (item) {
            if ((item.Code.indexOf("F") != -1 && this.SelectedOperator.Code.indexOf("F") == -1) || (item.Code.indexOf("F") == -1 && this.SelectedOperator.Code.indexOf("F") != -1)) {
                this.FieldValue = "";
                this.CurrentEntityPM.Value = "";
                this.SelectedSystemVariableOperator = null;
            }
            this.SelectedOperator = item;
            this.AddEditAutomationsViewModel.IsChangeCondition = true;
            var IsReloadGenerateControl = false;
            this.CurrentEntityPM.OperatorCode = this.SelectedOperator.Code;
            if (this.HideGeneralControl(item.Code)) {
                this.IsHideGeneralControl = true;
                this.FieldValue = "";
                this.CurrentEntityPM.Value = "";
                this.SelectedSystemVariableOperator = null;
            }
            else {
                this.IsHideGeneralControl = false;
                if (item.Code.indexOf("F") != -1) {
                    if (this.CurrentEntityPM.OperatorCode.indexOf("F") == -1) {
                        IsReloadGenerateControl = true;
                    }
                }
                else {
                    if (this.CurrentEntityPM.OperatorCode.indexOf("F") != -1) {
                        IsReloadGenerateControl = true;
                    }
                }
                if (item.Code.indexOf("F") != -1) {
                    this.IsCustomCombox = true;
                }
                else
                    this.IsCustomCombox = false;
                this.IsSetValue = false;
                this.BuildCustomFromFieldbjectFieldLists();
            }
        }
    };
    AutomationConditionViewModel.prototype.BuildCustomFromFieldbjectFieldLists = function () {
        if (this.SelectedOperator != null && this.SelectedCustomField) {
            if (this.SelectedOperator.Code.indexOf("F") != -1) {
                this.IsCustomCombox = true;
                if (this.SelectedCustomField) {
                    this.CustomObjectFieldId = this.FieldValue;
                    this.InitCustomLOVFilters(this.SelectedCustomField);
                }
            }
        }
    };
    AutomationConditionViewModel.prototype.ObjectFieldCondationValueChange = function (value) {
        this.AutomationHelper.ObjectFieldCondationValueChange(value);
    };
    AutomationConditionViewModel.prototype.TextBoxCondationValueChange = function (value) {
        this.AutomationHelper.TextBoxCondationValueChange(value);
    };
    AutomationConditionViewModel.prototype.LogLovCondationValueChange = function (value) {
        this.AutomationHelper.LogLovCondationValueChange(value);
    };
    AutomationConditionViewModel.prototype.SystemVariableCondationValueChanged = function (value) {
        this.AutomationHelper.SystemVariableCondationValueChanged(value);
    };
    AutomationConditionViewModel.prototype.DateTypeListCondationValueChanged = function (value) {
        this.AutomationHelper.DateTypeListCondationValueChanged(value);
    };
    AutomationConditionViewModel.prototype.DatePickerCondationValueChange = function (value) {
        this.AutomationHelper.DatePickerCondationValueChange(value);
    };
    AutomationConditionViewModel.prototype.NumericUpDownCondationValueChanged = function () {
        this.AutomationHelper.NumericUpDownCondationValueChanged();
    };
    return AutomationConditionViewModel;
}(BaseComponent_1.BaseComponent));
exports.AutomationConditionViewModel = AutomationConditionViewModel;
var Operator = /** @class */ (function () {
    function Operator(name, code) {
        this.Code = code;
        this.Name = name;
    }
    return Operator;
}());
//# sourceMappingURL=AutomationConditionViewModel.js.map