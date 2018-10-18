"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Tools_1 = require('../../../../../Infrastructure/Tools');
var BaseComponent_1 = require('../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent');
var FieldValueResolver_1 = require('../../../../../Infrastructure/Utilities/FieldValueResolver');
var AutomationSetValueViewModel = (function (_super) {
    __extends(AutomationSetValueViewModel, _super);
    function AutomationSetValueViewModel(entityPM, addEditAutomationsViewModel) {
        var _this = this;
        _super.call(this);
        this.IsLoadOperatorList = false;
        this.IsNewEntityCall = false;
        this.IsMultiline = false;
        this.IsRefrachCustomField = false;
        this.IsCustomCombox = false;
        this.IsModeDate = false;
        this.isChecked = false;
        this.CurrentEntityPM = entityPM;
        this.AddEditAutomationsViewModel = addEditAutomationsViewModel;
        this.ValueFromFieldbjectFieldLists = [];
        this.AutomationSetValuebjectFieldLists = addEditAutomationsViewModel.AutomationSetValuebjectFieldLists;
        var objectField = this.AutomationSetValuebjectFieldLists.filter(function (d) { return d.Id == _this.CurrentEntityPM.ObjectFieldId; })[0];
        this.FieldValue = this.CurrentEntityPM.Value;
        this.DateTypeList = [];
        this.DateTypeList.push(new Operator("@Today-", "-"));
        this.DateTypeList.push(new Operator("@Today+", "+"));
        this.DateTypeList.push(new Operator("Date", "Date"));
        this.SelectedDateType = this.DateTypeList[0];
        this.OperatorList = [];
        this.OperatorList.push(new Operator("Set Constant Value", "SV"));
        this.OperatorList.push(new Operator("Set Value From [Field]", "SF"));
        if (objectField) {
            this.SelectedCustomField = objectField;
            this.UIProperties.SetEnabled(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, true);
            this.UIProperties.SetRequired(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, false);
            if (objectField.DataTypeCode == "DateTime" || objectField.DataTypeCode == "Date") {
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
                    this.FieldValue = Tools_1.DateTool.GetCurrentDateAsUtc();
                }
            }
        }
        if (this.CurrentEntityPM != null) {
            this.SelectedOperator = this.OperatorList.filter(function (d) { return d.Code == _this.CurrentEntityPM.OperatorCode; })[0];
        }
        if (!this.SelectedOperator) {
            this.SelectedOperator = this.OperatorList.filter(function (d) { return d.Code == "SV"; })[0];
        }
        if (this.SelectedOperator) {
            if (this.SelectedOperator.Code == "SF") {
                this.BuildCustomFromFieldbjectFieldLists();
            }
        }
        this.IsLoadOperatorList = true;
    }
    AutomationSetValueViewModel.prototype.ngOnInit = function () {
    };
    AutomationSetValueViewModel.prototype.BuildCustomFromFieldbjectFieldLists = function () {
        var _this = this;
        this.SelectedValueFromField = null;
        this.ValueFromFieldbjectFieldLists = [];
        if (this.SelectedOperator != null && this.SelectedCustomField) {
            if (this.SelectedOperator.Code.indexOf("F") != -1) {
                this.IsCustomCombox = true;
                if (this.SelectedCustomField) {
                    if (this.SelectedCustomField.DataTypeCode == "LookUp") {
                        this.ValueFromFieldbjectFieldLists = this.AutomationSetValuebjectFieldLists.filter(function (d) { return d.DataTypeCode == _this.SelectedCustomField.DataTypeCode && d.LookUpTableId == _this.SelectedCustomField.LookUpTableId; });
                    }
                    else {
                        this.ValueFromFieldbjectFieldLists = this.AutomationSetValuebjectFieldLists.filter(function (d) { return d.DataTypeCode == _this.SelectedCustomField.DataTypeCode; });
                    }
                    this.SelectedValueFromField = this.AutomationSetValuebjectFieldLists.filter(function (d) { return d.Id == _this.FieldValue; })[0];
                }
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
    AutomationSetValueViewModel.prototype.ValueFromFieldValueChanged = function (item) {
        this.CurrentEntityPM.Value = item.Id;
        this.SelectedValueFromField = item;
        this.AddEditAutomationsViewModel.IsChangeSetValue = true;
    };
    AutomationSetValueViewModel.prototype.CustomFieldValueChanged = function (item) {
        this.FieldValue = "";
        this.AddEditAutomationsViewModel.IsChangeSetValue = true;
        var ischange = false;
        if (this.SelectedCustomField && this.SelectedCustomField.DataTypeCode == item.DataTypeCode) {
            ischange = true;
        }
        this.SelectedCustomField = null;
        this.SelectedCustomField = item;
        this.UIProperties.SetEnabled(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, true);
        this.UIProperties.SetRequired(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, false);
        if (this.SelectedCustomField) {
            this.CurrentEntityPM.ObjectFieldId = this.SelectedCustomField.Id;
            this.CurrentEntityPM.FieldName = this.SelectedCustomField.FieldName;
            this.CurrentEntityPM.DataTypeCode = this.SelectedCustomField.DataTypeCode;
        }
        this.IsSetValue = false;
        if (ischange) {
            this.IsRefrachCustomField = !this.IsRefrachCustomField;
        }
        this.BuildCustomFromFieldbjectFieldLists();
        //}
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
        this.SelectedOperator = item;
        this.AddEditAutomationsViewModel.IsChangeSetValue = true;
        this.CurrentEntityPM.OperatorCode = this.SelectedOperator.Code;
        if (this.CurrentEntityPM.OperatorCode == "SF") {
            this.BuildCustomFromFieldbjectFieldLists();
            this.SelectedValueFromField = null;
        }
        else {
            this.IsCustomCombox = false;
        }
    };
    AutomationSetValueViewModel.prototype.OnTextChange = function (value) {
        this.CurrentEntityPM.Value = value;
        this.AddEditAutomationsViewModel.IsChangeSetValue = true;
    };
    AutomationSetValueViewModel.prototype.LogLovValueChange = function (value) {
        var valuecondition = value ? !Tools_1.AppTool.IsNullOrEmpty(value.Id) ? value.Id : value.Code : "";
        if (value && valuecondition != this.CurrentEntityPM.Value) {
            this.CurrentEntityPM.Value = valuecondition;
            this.AddEditAutomationsViewModel.IsChangeSetValue = true;
        }
    };
    AutomationSetValueViewModel.prototype.DateTypeListValueChanged = function (value) {
        if (value) {
            this.SelectedDateType = value;
            this.AddEditAutomationsViewModel.IsChangeSetValue = true;
            this.FieldValue = "";
            var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            if (value.Code == "Date") {
                this.IsModeDate = true;
                this.CurrentEntityPM.Value = this.SelectedDateType.Name + "*" + FieldValueResolver_1.FieldValueResolver.ConvertUTCDateToString(todayDate, "Automation");
            }
            else {
                this.FieldValue = 0;
                this.IsModeDate = false;
                this.CurrentEntityPM.Value = this.SelectedDateType.Name + "*" + this.FieldValue + "*" + FieldValueResolver_1.FieldValueResolver.ConvertUTCDateToString(todayDate, "Automation");
            }
        }
    };
    AutomationSetValueViewModel.prototype.OnLogDatePickerChange = function (value) {
        if (value) {
            var newValue = this.SelectedDateType.Name + "*" + FieldValueResolver_1.FieldValueResolver.ConvertUTCDateToString(value, "Automation");
            if (newValue != this.CurrentEntityPM.Value) {
                this.CurrentEntityPM.Value = newValue;
                this.AddEditAutomationsViewModel.IsChangeSetValue = true;
            }
        }
        else {
            var newValue = this.SelectedDateType.Name + "*";
            if (newValue != this.CurrentEntityPM.Value) {
                this.CurrentEntityPM.Value = newValue;
                this.AddEditAutomationsViewModel.IsChangeSetValue = true;
            }
        }
    };
    AutomationSetValueViewModel.prototype.NumericUpDownValueChanged = function () {
        if (this.FieldValue) {
            var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            var value = null;
            if (this.SelectedDateType.Code == "-") {
                value = Tools_1.DateTool.AddDays(todayDate, -this.FieldValue);
            }
            else {
                value = Tools_1.DateTool.AddDays(todayDate, this.FieldValue);
            }
            this.CurrentEntityPM.Value = this.SelectedDateType.Name + "*" + this.FieldValue + "*" + FieldValueResolver_1.FieldValueResolver.ConvertUTCDateToString(value, "Automation");
            this.AddEditAutomationsViewModel.IsChangeSetValue = true;
        }
    };
    return AutomationSetValueViewModel;
}(BaseComponent_1.BaseComponent));
exports.AutomationSetValueViewModel = AutomationSetValueViewModel;
var Operator = (function () {
    function Operator(name, code) {
        this.Code = code;
        this.Name = name;
    }
    return Operator;
}());
//# sourceMappingURL=AutomationSetValueViewModel.js.map