"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../Tools");
var FieldValueResolver_1 = require("../Utilities/FieldValueResolver");
var AutomationHelper = /** @class */ (function () {
    function AutomationHelper(currentEntityPM, addEditAutomationsViewModel, viewModel, type) {
        this.CurrentEntityPM = currentEntityPM;
        this.AddEditAutomationsViewModel = addEditAutomationsViewModel;
        this.ViewModel = viewModel;
        this.Type = type;
    }
    AutomationHelper.prototype.LogLovCondationValueChange = function (value, otherValue) {
        if (otherValue === void 0) { otherValue = null; }
        var newValue = value ? !Tools_1.AppTool.IsNullOrEmpty(value.Id) ? value.Id : value.Code : "";
        this.ConditionValueChange(newValue);
    };
    AutomationHelper.prototype.ObjectFieldCondationValueChange = function (value) {
        var newValue = value ? !Tools_1.AppTool.IsNullOrEmpty(value.Id) ? value.Id : "" : "";
        this.ConditionValueChange(newValue);
    };
    AutomationHelper.prototype.ConditionValueChange = function (newValue) {
        if (newValue != this.CurrentEntityPM.Value) {
            this.CurrentEntityPM.Value = newValue;
            if (this.Type == "Condation") {
                this.AddEditAutomationsViewModel.IsChangeCondition = true;
            }
            else {
                this.AddEditAutomationsViewModel.IsChangeSetValue = true;
            }
        }
    };
    AutomationHelper.prototype.SystemVariableCondationValueChanged = function (value) {
        var newValue = value ? value.Code : "";
        this.ConditionValueChange(newValue);
    };
    AutomationHelper.prototype.TextBoxCondationValueChange = function (value) {
        this.CurrentEntityPM.Value = value;
        if (this.Type == "Condation")
            this.AddEditAutomationsViewModel.IsChangeCondition = true;
        else
            this.AddEditAutomationsViewModel.IsChangeSetValue = true;
    };
    AutomationHelper.prototype.DatePickerCondationValueChange = function (value) {
        if (value) {
            var newValue = this.ViewModel.SelectedDateType.Name + "*" + FieldValueResolver_1.FieldValueResolver.ConvertUTCDateToString(value, "Automation");
            this.ConditionValueChange(newValue);
        }
        else {
            var newValue = this.ViewModel.SelectedDateType.Name + "*";
            this.ConditionValueChange(newValue);
        }
    };
    AutomationHelper.prototype.NumericUpDownCondationValueChanged = function () {
        if (!this.ViewModel.FieldValue) {
            this.ViewModel.FieldValue = 0;
        }
        var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        var value = null;
        if (this.ViewModel.SelectedDateType.Code == "-") {
            value = Tools_1.DateTool.AddDays(todayDate, -this.ViewModel.FieldValue);
        }
        else {
            value = Tools_1.DateTool.AddDays(todayDate, this.ViewModel.FieldValue);
        }
        this.CurrentEntityPM.Value = this.ViewModel.SelectedDateType.Name + "*" + this.ViewModel.FieldValue + "*" + FieldValueResolver_1.FieldValueResolver.ConvertUTCDateToString(value, "Automation");
        if (this.Type == "Condation")
            this.AddEditAutomationsViewModel.IsChangeCondition = true;
        else
            this.AddEditAutomationsViewModel.IsChangeSetValue = true;
    };
    AutomationHelper.prototype.DateTypeListCondationValueChanged = function (value) {
        if (value) {
            this.ViewModel.SelectedDateType = value;
            this.ViewModel.FieldValue = "";
            var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            if (value.Code == "Date") {
                this.ViewModel.IsModeDate = true;
                this.CurrentEntityPM.Value = this.ViewModel.SelectedDateType.Name + "*";
            }
            else {
                this.ViewModel.FieldValue = 0;
                this.ViewModel.IsModeDate = false;
                this.CurrentEntityPM.Value = this.ViewModel.SelectedDateType.Name + "*" + this.ViewModel.FieldValue + "*" + FieldValueResolver_1.FieldValueResolver.ConvertUTCDateToString(todayDate, "Automation");
            }
            if (this.Type == "Condation")
                this.AddEditAutomationsViewModel.IsChangeCondition = true;
            else
                this.AddEditAutomationsViewModel.IsChangeSetValue = true;
        }
    };
    return AutomationHelper;
}());
exports.AutomationHelper = AutomationHelper;
//# sourceMappingURL=AutomationHelper.js.map