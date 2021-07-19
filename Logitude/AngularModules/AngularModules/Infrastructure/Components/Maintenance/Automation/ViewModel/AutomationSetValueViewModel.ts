
import {AutomationPM} from '../../../../../Common/EntityPMs/AutomationPMExtended';
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools';

import {Component, OnInit, ChangeDetectorRef }  from '@angular/core';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {AutomationSetValue} from '../../../../../Infrastructure/DataContracts/AutomationSetValue';
import {ObjectFieldPM} from '../../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {FieldValueResolver} from '../../../../../Infrastructure/Utilities/FieldValueResolver';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AutomationHelper} from '../../../../../Infrastructure/Helpers/AutomationHelper';
export class AutomationSetValueViewModel extends BaseComponent implements OnInit {
    public CurrentEntityPM: AutomationSetValue;

    public AutomationCondationFieldListFilterItems: ApiQueryFilters;
    CustomAutomationCondationFieldListFilterItems: ApiQueryFilters;
    AutomationSetValuebjectFieldLists: ObjectFieldPM[];

    BooleanList: boolean[] = [true, false];

    SelectedCustomField: ObjectFieldPM;
    AddEditAutomationsViewModel: any;
    OperatorList: Operator[];
    SelectedOperator: Operator;
    isChangeOperator: boolean;
    IsSetValue: boolean;
    FieldValue: any;
    IsLoadOperatorList: boolean = false;
    DelayAutomationconditionsViewModel: any;
    IsNewEntityCall: boolean = false;
    IsMultiline: boolean = false;
    IsRefrachCustomField: boolean = false;
    IsHideGeneralControl: boolean = false;

    IsCustomCombox: boolean = false;
    DateTypeList: Operator[];
    SelectedDateType: Operator;
    IsModeDate: boolean = false;
    ObjectFieldCode: string = "";
    CustomObjectFieldCode: string = "";
    IsRefreshObjectFieldLov: boolean = false;
    AutomationHelper: AutomationHelper;
    constructor(entityPM: AutomationSetValue, addEditAutomationsViewModel: any) {
        super();
        this.CurrentEntityPM = entityPM;
        this.AddEditAutomationsViewModel = addEditAutomationsViewModel;
        this.AutomationHelper = new AutomationHelper(this.CurrentEntityPM, this.AddEditAutomationsViewModel, this, "SetValue");
        this.InitLOVFilters();
        this.AutomationSetValuebjectFieldLists = addEditAutomationsViewModel.AutomationSetValuebjectFieldLists;
        var objectField: ObjectFieldPM = this.AutomationSetValuebjectFieldLists.filter(d => d.FieldCode == this.CurrentEntityPM.ObjectFieldCode)[0];

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
            this.ObjectFieldCode = objectField.FieldCode;
            this.SelectedCustomField = objectField;
            this.UIProperties.SetEnabled(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, true);
            this.UIProperties.SetRequired(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, false);
            if (objectField.DataTypeCode == "DateTime" || objectField.DataTypeCode == "Date") {
                if (this.CurrentEntityPM.OperatorCode && this.CurrentEntityPM.OperatorCode.indexOf("F") == -1) {
                    if (this.FieldValue) {
                        var values = this.FieldValue.split("*");
                        this.SelectedDateType = this.DateTypeList.filter(d => d.Name == values[0])[0];

                        if (this.FieldValue.indexOf("Date") != -1) {
                            if (values.length > 1) this.FieldValue = FieldValueResolver.ConvertToDate(values[1], "Automation");
                            else {
                                this.FieldValue = null;
                            }

                            this.IsModeDate = true;
                        }
                        else {

                            this.FieldValue = 0;
                            if (values.length > 1) this.FieldValue = Number(values[1]);
                            this.IsModeDate = false;
                        }
                    }
                    else {
                        this.FieldValue = DateTool.GetCurrentDateAsUtc();
                    }
                }
            }

        }



        if (this.CurrentEntityPM != null) {
            this.SelectedOperator = this.OperatorList.filter(d => d.Code == this.CurrentEntityPM.OperatorCode)[0];

        }
        if (!this.SelectedOperator) {
            this.SelectedOperator = this.OperatorList.filter(d => d.Code == "SV")[0];
        }

        if (this.SelectedOperator) {
            if (this.SelectedOperator.Code == "SF") {
                this.BuildCustomFromFieldbjectFieldLists();
            }

        }





        this.IsLoadOperatorList = true;


    }


    ngOnInit() {



    }

    InitLOVFilters() {

        this.AutomationCondationFieldListFilterItems = new ApiQueryFilters();
        this.AutomationCondationFieldListFilterItems.addAdditionalFilter("ObjectTableId", this.AddEditAutomationsViewModel.ObjectTableId, null, null, "Equals", false, false, false, "string");
        this.AutomationCondationFieldListFilterItems.addAdditionalFilter("CanAutomateSetValue", true, null, null, "Equals", true, false, false, "boolean");


    }

    InitCustomLOVFilters(objectFieldPM: ObjectFieldPM) {
        this.CustomAutomationCondationFieldListFilterItems = new ApiQueryFilters();
        this.CustomAutomationCondationFieldListFilterItems.addAdditionalFilter("ObjectTableId", this.AddEditAutomationsViewModel.ObjectTableId, null, null, "Equals", false, false, false, "string");
        this.CustomAutomationCondationFieldListFilterItems.addAdditionalFilter("AllowedinAutomationConditions", true, null, null, "Equals", false, false, false, "boolean");

        if (this.SelectedCustomField) {
            if (this.SelectedCustomField.DataTypeCode == "LookUp") {
                this.CustomAutomationCondationFieldListFilterItems.addAdditionalFilter("LookUpTableId", this.SelectedCustomField.LookUpTableId, null, null, "Equals", false, false, false, "string");
            }
            this.CustomAutomationCondationFieldListFilterItems.addAdditionalFilter("DataTypeCode", this.SelectedCustomField.DataTypeCode, null, null, "Equals", false, false, false, "string");
        }
    }



    BooleanListValueChanged(newValue: boolean) {
        this.isChecked = newValue;
        this.CurrentEntityPM.Value = this.FieldValue = this.isChecked ? "true" : "false";
        this.AddEditAutomationsViewModel.IsChangeCondition = true;
    }








    BuildCustomFromFieldbjectFieldLists() {

        if (this.SelectedOperator != null && this.SelectedCustomField) {

            if (this.SelectedOperator.Code.indexOf("F") != -1) {
                this.IsCustomCombox = true;

                if (this.SelectedCustomField) {
                    this.CustomObjectFieldCode = this.FieldValue;
                    this.InitCustomLOVFilters(this.SelectedCustomField);

                }
                this.IsRefreshObjectFieldLov = !this.IsRefreshObjectFieldLov;
            }
        }


    }

    private isChecked: boolean = false;
    get IsChecked() {
        if (this.SelectedCustomField && this.CurrentEntityPM) {
            if (this.SelectedCustomField.DataTypeCode == "Boolean") {
                this.isChecked = this.CurrentEntityPM.Value == "true" ? true : false;
            }
        }

        return this.isChecked;
    }
    set IsChecked(newValue: boolean) {
        this.isChecked = newValue;
        this.CurrentEntityPM.Value = this.FieldValue = this.isChecked ? "true" : "false";
        this.AddEditAutomationsViewModel.IsChangeSetValue = true;
    }

    CustomFieldValueChanged(item) {

        if (item) {
            var oldObjectFieldCode: string = this.SelectedCustomField ? this.SelectedCustomField.FieldCode : "";
            if (item.FieldCode != oldObjectFieldCode) {
                this.CurrentEntityPM.IsCustomField = item.IsCustom;
                this.FieldValue = "";
                this.CurrentEntityPM.Value = "";
                this.CustomObjectFieldCode = "";
                this.AddEditAutomationsViewModel.IsChangeSetValue = true;


                if (item.DataTypeCode == "Boolean") this.IsChecked = false;
        



                var ischange = false;

                if (this.SelectedCustomField && this.SelectedCustomField.DataTypeCode == item.DataTypeCode) {

                    ischange = true;
                }

                this.SelectedCustomField = null;
                this.SelectedCustomField = this.AutomationSetValuebjectFieldLists.filter(d => d.FieldCode == item.FieldCode)[0];

                this.IsCustomCombox = false;

                if (this.SelectedCustomField) {
                    this.UIProperties.SetEnabled(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, true);
                    this.UIProperties.SetRequired(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, false);
                    this.CurrentEntityPM.ObjectFieldCode = this.SelectedCustomField.FieldCode;
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

    }

    DeleteAutomationConditionMethod(item: AutomationSetValueViewModel) {

        if (this.AddEditAutomationsViewModel.AutomationSetValueLists) {
            var index = this.AddEditAutomationsViewModel.AutomationSetValueLists.indexOf(item);
            if (index != -1) this.AddEditAutomationsViewModel.AutomationSetValueLists.splice(index, 1);
        }
        this.AddEditAutomationsViewModel.IsChangeSetValue = true;

    }

    OperatorListValueChanged(item) {
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

    }

    ObjectFieldCondationValueChange(value) {

        this.AutomationHelper.ObjectFieldCondationValueChange(value);
    }

    TextBoxCondationValueChange(value) {

        this.AutomationHelper.TextBoxCondationValueChange(value);

    }

    LogLovCondationValueChange(value) {
        this.AutomationHelper.LogLovCondationValueChange(value);
    }


    DateTypeListCondationValueChanged(value) {

        this.AutomationHelper.DateTypeListCondationValueChanged(value);
    }

    DatePickerCondationValueChange(value) {
        this.AutomationHelper.DatePickerCondationValueChange(value);
    }

    NumericUpDownCondationValueChanged() {
        this.AutomationHelper.NumericUpDownCondationValueChanged();
    }


}

class Operator {
    Code: string;
    Name: string;

    constructor(name: string, code: string) {
        this.Code = code;
        this.Name = name;

    }


}
