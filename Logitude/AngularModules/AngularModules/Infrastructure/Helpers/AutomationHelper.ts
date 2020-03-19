import {Component, OnInit}  from '@angular/core';

import {AppTool, DateTool} from '../Tools';
import {SessionLocator} from '../Utilities/SessionLocator';
import {FieldValueResolver} from '../Utilities/FieldValueResolver';
export class AutomationHelper   {

    CurrentEntityPM: any;
    AddEditAutomationsViewModel: any;
    Type: string;
    ViewModel: any;
    constructor(currentEntityPM: any, addEditAutomationsViewModel:any ,viewModel:any, type:string) {

        this.CurrentEntityPM = currentEntityPM;
        this.AddEditAutomationsViewModel = addEditAutomationsViewModel;
        this.ViewModel = viewModel;
        this.Type = type;
    }

    LogLovCondationValueChange(value, otherValue:string = null) {
        var newValue: string = value ? !AppTool.IsNullOrEmpty(value.Id) ? value.Id : value.Code : "";
        this.ConditionValueChange(newValue);

    }

    ObjectFieldCondationValueChange(value) {
        var newValue: string = value ? !AppTool.IsNullOrEmpty(value.FieldCode) ? value.FieldCode : "" : "";
        this.ConditionValueChange(newValue);
    }

    ConditionValueChange(newValue: string) {
        if (newValue != this.CurrentEntityPM.Value) {

            this.CurrentEntityPM.Value = newValue;
            if (this.Type == "Condation") {
                this.AddEditAutomationsViewModel.IsChangeCondition = true;
            } else {
                this.AddEditAutomationsViewModel.IsChangeSetValue = true;
            }
        }

    }

    SystemVariableCondationValueChanged(value) {
        var newValue: string = value ? value.Code : "";
        this.ConditionValueChange(newValue);
    }



    TextBoxCondationValueChange(value) {

        this.CurrentEntityPM.Value = value;
        if (this.Type == "Condation") this.AddEditAutomationsViewModel.IsChangeCondition = true;
        else this.AddEditAutomationsViewModel.IsChangeSetValue = true;

    }


    DatePickerCondationValueChange(value) {
        if (value) {

            var newValue = this.ViewModel.SelectedDateType.Name + "*" + FieldValueResolver.ConvertUTCDateToString(value, "Automation");
            this.ConditionValueChange(newValue);
        }
        else {
            var newValue = this.ViewModel.SelectedDateType.Name + "*";
            this.ConditionValueChange(newValue);
        }
    }


    NumericUpDownCondationValueChanged() {

        if (!this.ViewModel.FieldValue) {
            this.ViewModel.FieldValue = 0;
        }

        var todayDate = DateTool.GetCurrentDateTimeAsUtc();
        var value = null;
        if (this.ViewModel.SelectedDateType.Code == "-") {

            value = DateTool.AddDays(todayDate, - this.ViewModel.FieldValue);
        }
        else {
            value = DateTool.AddDays(todayDate, this.ViewModel.FieldValue);
        }

        this.CurrentEntityPM.Value = this.ViewModel.SelectedDateType.Name + "*" + this.ViewModel.FieldValue + "*" + FieldValueResolver.ConvertUTCDateToString(value, "Automation");
        if (this.Type == "Condation") this.AddEditAutomationsViewModel.IsChangeCondition = true;
        else this.AddEditAutomationsViewModel.IsChangeSetValue = true;

    }


    DateTypeListCondationValueChanged(value) {

        if (value) {
            this.ViewModel.SelectedDateType = value;
            this.ViewModel.FieldValue = "";
            var todayDate = DateTool.GetCurrentDateTimeAsUtc();
            if (value.Code == "Date") {
                this.ViewModel.IsModeDate = true;
                this.CurrentEntityPM.Value = this.ViewModel.SelectedDateType.Name + "*";
            }
            else {

                this.ViewModel.FieldValue = 0;
                this.ViewModel.IsModeDate = false;
                this.CurrentEntityPM.Value = this.ViewModel.SelectedDateType.Name + "*" + this.ViewModel.FieldValue + "*" + FieldValueResolver.ConvertUTCDateToString(todayDate, "Automation");
            }

            if (this.Type == "Condation") this.AddEditAutomationsViewModel.IsChangeCondition = true;
            else this.AddEditAutomationsViewModel.IsChangeSetValue = true;

        }
    }



}