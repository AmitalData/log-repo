"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Tools_1 = require('../../../../../Infrastructure/Tools');
var BaseComponent_1 = require('../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent');
var FieldValueResolver_1 = require('../../../../../Infrastructure/Utilities/FieldValueResolver');
var AutomationConditionViewModel = (function (_super) {
    __extends(AutomationConditionViewModel, _super);
    function AutomationConditionViewModel(entityPM, addEditAutomationsViewModel, delayAutomationconditionsViewModel) {
        var _this = this;
        if (delayAutomationconditionsViewModel === void 0) { delayAutomationconditionsViewModel = null; }
        _super.call(this);
        this.IsLoadOperatorList = false;
        this.IsNewEntityCall = false;
        this.IsMultiline = false;
        this.IsRefrachCustomField = false;
        this.IsModeDate = false;
        this.IsCustomCombox = false;
        this.isChecked = false;
        this.IsHideGeneralControl = false;
        this.CurrentEntityPM = entityPM;
        this.AddEditAutomationsViewModel = addEditAutomationsViewModel;
        this.DelayAutomationconditionsViewModel = delayAutomationconditionsViewModel;
        this.AllowedinAutomationConditionsFieldLists = addEditAutomationsViewModel.AllowedinAutomationConditionsFieldLists;
        this.ObjectFieldPM = this.AllowedinAutomationConditionsFieldLists.filter(function (d) { return d.Id == _this.CurrentEntityPM.ObjectFieldId; })[0];
        this.Id = entityPM.Id;
        this.FieldValue = this.CurrentEntityPM.Value;
        this.DateTypeList = [];
        this.DateTypeList.push(new Operator("@Today-", "-"));
        this.DateTypeList.push(new Operator("@Today+", "+"));
        this.DateTypeList.push(new Operator("Date", "Date"));
        this.SelectedDateType = this.DateTypeList[0];
        if (this.ObjectFieldPM) {
            this.ChosenOperatorList(this.ObjectFieldPM.DataTypeCode, false);
            this.SelectedCustomField = this.ObjectFieldPM;
            this.UIProperties.SetEnabled(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, true);
            this.UIProperties.SetRequired(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, false);
            if (this.CurrentEntityPM)
                this.CurrentEntityPM.ObjectFieldType = this.SelectedCustomField.DataTypeCode;
            if (this.ObjectFieldPM.DataTypeCode == "DateTime" || this.ObjectFieldPM.DataTypeCode == "Date") {
                if (this.FieldValue) {
                    var values = this.FieldValue.split("*");
                    this.SelectedDateType = this.DateTypeList.filter(function (d) { return d.Name == values[0]; })[0];
                    if (this.FieldValue.indexOf("Date") != -1) {
                        if (values.length > 1)
                            this.FieldValue = FieldValueResolver_1.FieldValueResolver.ConvertToDate(values[1], "Automation");
                        else {
                            this.FieldValue = null;
                        }
                        this.IsModeDate = true;
                    }
                    else {
                        this.FieldValue = 0;
                        if (values.length > 1)
                            this.FieldValue = Number(values[1]);
                        this.IsModeDate = false;
                    }
                }
                else {
                    this.FieldValue = new Date();
                }
            }
        }
        else {
            this.ChosenOperatorList("Text", false);
        }
        if (this.CurrentEntityPM != null) {
            this.SelectedOperator = this.OperatorList.filter(function (d) { return d.Code == _this.CurrentEntityPM.OperatorCode; })[0];
        }
        if (!this.SelectedOperator) {
            this.SelectedOperator = this.OperatorList.filter(function (d) { return d.Code == "="; })[0];
            if (this.SelectedOperator) {
                this.CurrentEntityPM.OperatorCode = this.SelectedOperator.Code;
            }
        }
        if (this.SelectedOperator) {
            if (this.SelectedOperator.Code.indexOf("F") != -1) {
                this.IsCustomCombox = true;
                if (this.ObjectFieldPM) {
                    if (this.ObjectFieldPM.DataTypeCode == "LookUp") {
                        this.CustomAutomationConditionsFieldLists = this.AllowedinAutomationConditionsFieldLists.filter(function (d) { return d.DataTypeCode == _this.ObjectFieldPM.DataTypeCode && d.LookUpTableId == _this.ObjectFieldPM.LookUpTableId; });
                    }
                    else {
                        this.CustomAutomationConditionsFieldLists = this.AllowedinAutomationConditionsFieldLists.filter(function (d) { return d.DataTypeCode == _this.ObjectFieldPM.DataTypeCode; });
                    }
                }
                this.SelectedCustomAutomationConditionsField = this.AllowedinAutomationConditionsFieldLists.filter(function (d) { return d.Id == _this.FieldValue; })[0];
            }
            if (this.SelectedOperator.Code == "CHANGED") {
                this.IsHideGeneralControl = true;
                this.FieldValue = "";
                this.CurrentEntityPM.Value = "";
            }
        }
        this.IsLoadOperatorList = true;
    }
    AutomationConditionViewModel.prototype.ngOnInit = function () {
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
    AutomationConditionViewModel.prototype.EditCustomField = function (item) {
    };
    AutomationConditionViewModel.prototype.ChosenOperatorList = function (dataTypeCode, isChangeOperator) {
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
        if (this.AddEditAutomationsViewModel && this.AddEditAutomationsViewModel.CurrentEntityPM && this.AddEditAutomationsViewModel.CurrentEntityPM.Type != "OnCreate") {
            this.OperatorList.push(new Operator("Changed to", "CHANGEDTO"));
            this.OperatorList.push(new Operator("Changed", "CHANGED"));
        }
        if (isChangeOperator) {
            this.CurrentEntityPM.OperatorCode = this.OperatorList[0] ? this.OperatorList[0].Code : "=";
        }
    };
    AutomationConditionViewModel.prototype.CustomFieldValueChanged = function (item) {
        {
            this.IsHideGeneralControl = false;
            this.FieldValue = "";
            this.CurrentEntityPM.Value = "";
            this.AddEditAutomationsViewModel.IsChangeCondition = true;
            var ischange = false;
            if (this.SelectedCustomField && this.SelectedCustomField.DataTypeCode == item.DataTypeCode) {
                ischange = true;
            }
            this.SelectedCustomField = null;
            this.SelectedCustomField = item;
            this.IsCustomCombox = false;
            this.UIProperties.SetEnabled(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, true);
            this.UIProperties.SetRequired(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, false);
            if (this.SelectedCustomField) {
                this.CurrentEntityPM.ObjectFieldId = this.SelectedCustomField.Id;
                this.ChosenOperatorList(this.SelectedCustomField.DataTypeCode, true);
                this.SelectedOperator = this.OperatorList[0];
            }
            this.IsSetValue = false;
            this.CurrentEntityPM.ObjectFieldType = this.SelectedCustomField.DataTypeCode;
            if (ischange) {
                this.IsRefrachCustomField = !this.IsRefrachCustomField;
            }
            this.ReBluidComboxCustomField();
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
    AutomationConditionViewModel.prototype.OperatorListValueChanged = function (item) {
        this.SelectedOperator = item;
        this.AddEditAutomationsViewModel.IsChangeCondition = true;
        var IsReloadGenerateControl = false;
        this.CurrentEntityPM.OperatorCode = this.SelectedOperator.Code;
        if (item.Code == "CHANGED") {
            this.IsHideGeneralControl = true;
            this.FieldValue = "";
            this.CurrentEntityPM.Value = "";
        }
        else {
            this.IsHideGeneralControl = false;
            if (item.Code.indexOf("F") != -1 || item.Code == "CHANGED") {
                if (this.CurrentEntityPM.OperatorCode.indexOf("F") == -1 || item.Code != "CHANGED") {
                    IsReloadGenerateControl = true;
                }
            }
            else {
                if (this.CurrentEntityPM.OperatorCode.indexOf("F") != -1 || this.CurrentEntityPM.OperatorCode == "CHANGED") {
                    IsReloadGenerateControl = true;
                }
            }
            if (item.Code.indexOf("F") != -1) {
                this.IsCustomCombox = true;
            }
            else
                this.IsCustomCombox = false;
            this.IsSetValue = false;
            this.ReBluidComboxCustomField();
        }
    };
    AutomationConditionViewModel.prototype.ReBluidComboxCustomField = function () {
        var _this = this;
        this.SelectedCustomAutomationConditionsField = null;
        this.CustomAutomationConditionsFieldLists = [];
        if (this.SelectedOperator != null && this.SelectedCustomField) {
            if (this.SelectedOperator.Code.indexOf("F") != -1) {
                this.IsCustomCombox = true;
                if (this.SelectedCustomField) {
                    if (this.SelectedCustomField.DataTypeCode == "LookUp") {
                        this.CustomAutomationConditionsFieldLists = this.AllowedinAutomationConditionsFieldLists.filter(function (d) { return d.DataTypeCode == _this.SelectedCustomField.DataTypeCode && d.LookUpTableId == _this.SelectedCustomField.LookUpTableId; });
                    }
                    else {
                        this.CustomAutomationConditionsFieldLists = this.AllowedinAutomationConditionsFieldLists.filter(function (d) { return d.DataTypeCode == _this.SelectedCustomField.DataTypeCode; });
                    }
                }
                this.SelectedCustomAutomationConditionsField = this.AllowedinAutomationConditionsFieldLists.filter(function (d) { return d.Id == _this.FieldValue; })[0];
            }
        }
    };
    AutomationConditionViewModel.prototype.ValueChange = function (value) {
        this.CurrentEntityPM.Value = value.Id;
        this.AddEditAutomationsViewModel.IsChangeCondition = true;
    };
    AutomationConditionViewModel.prototype.OnTextChange = function (value) {
        this.CurrentEntityPM.Value = value;
        this.AddEditAutomationsViewModel.IsChangeCondition = true;
    };
    AutomationConditionViewModel.prototype.LogLovValueChange = function (value) {
        var valuecondition = value ? !Tools_1.AppTool.IsNullOrEmpty(value.Id) ? value.Id : value.Code : "";
        if (value && valuecondition != this.CurrentEntityPM.Value) {
            this.CurrentEntityPM.Value = valuecondition;
            this.AddEditAutomationsViewModel.IsChangeCondition = true;
        }
    };
    AutomationConditionViewModel.prototype.DateTypeListValueChanged = function (value) {
        if (value) {
            this.SelectedDateType = value;
            this.AddEditAutomationsViewModel.IsChangeCondition = true;
            this.FieldValue = "";
            var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            if (value.Code == "Date") {
                this.IsModeDate = true;
                this.CurrentEntityPM.Value = this.SelectedDateType.Name + "*";
            }
            else {
                this.FieldValue = 0;
                this.IsModeDate = false;
                this.CurrentEntityPM.Value = this.SelectedDateType.Name + "*" + this.FieldValue + "*" + FieldValueResolver_1.FieldValueResolver.ConvertUTCDateToString(todayDate, "Automation");
            }
        }
    };
    AutomationConditionViewModel.prototype.OnLogDatePickerChange = function (value) {
        if (value) {
            var newValue = this.SelectedDateType.Name + "*" + FieldValueResolver_1.FieldValueResolver.ConvertUTCDateToString(value, "Automation");
            if (newValue != this.CurrentEntityPM.Value) {
                this.CurrentEntityPM.Value = newValue;
                this.AddEditAutomationsViewModel.IsChangeCondition = true;
            }
        }
        else {
            var newValue = this.SelectedDateType.Name + "*";
            if (newValue != this.CurrentEntityPM.Value) {
                this.CurrentEntityPM.Value = newValue;
                this.AddEditAutomationsViewModel.IsChangeCondition = true;
            }
        }
    };
    AutomationConditionViewModel.prototype.NumericUpDownValueChanged = function () {
        if (!this.FieldValue) {
            this.FieldValue = 0;
        }
        var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        var value = null;
        if (this.SelectedDateType.Code == "-") {
            value = Tools_1.DateTool.AddDays(todayDate, -this.FieldValue);
        }
        else {
            value = Tools_1.DateTool.AddDays(todayDate, this.FieldValue);
        }
        this.CurrentEntityPM.Value = this.SelectedDateType.Name + "*" + this.FieldValue + "*" + FieldValueResolver_1.FieldValueResolver.ConvertUTCDateToString(value, "Automation");
        this.AddEditAutomationsViewModel.IsChangeCondition = true;
    };
    return AutomationConditionViewModel;
}(BaseComponent_1.BaseComponent));
exports.AutomationConditionViewModel = AutomationConditionViewModel;
var Operator = (function () {
    function Operator(name, code) {
        this.Code = code;
        this.Name = name;
    }
    return Operator;
}());
//# sourceMappingURL=AutomationConditionViewModel.js.map