
declare var System: any;
declare var window: any;
import {AutomationPM} from '../../../../../Common/EntityPMs/AutomationPMExtended';
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools';

import {Component, OnInit, ChangeDetectorRef }  from '@angular/core';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {AutomationCondition} from '../../../../../Infrastructure/DataContracts/AutomationCondition';
import {ObjectFieldPM} from '../../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {FieldValueResolver} from '../../../../../Infrastructure/Utilities/FieldValueResolver';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AutomationHelper} from '../../../../../Infrastructure/Helpers/AutomationHelper';
import {TextCodeTranslationPipe} from '../../../../../Controls/Pipes/TextCodeTranslationPipe';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';


export class AutomationConditionViewModel extends BaseComponent implements OnInit {
    public CurrentEntityPM: AutomationCondition;
    AllowedinAutomationConditionsFieldLists: ObjectFieldPM[];
    SelectedCustomField: ObjectFieldPM;
    AddEditAutomationsViewModel: any;
    OperatorList: Operator[];
    BooleanList:boolean[] = [true, false];
    SelectedOperator: Operator;
    isChangeOperator: boolean;
    IsSetValue: boolean;
    FieldValue: any;
    IsLoadOperatorList: boolean = false;
    DelayAutomationconditionsViewModel: any;
    IsNewEntityCall: boolean = false;
    IsMultiline: boolean = false;
    IsRefrachCustomField: boolean = false;
    IsRefreshObjectFieldLov: boolean = false;
    DateTypeList: Operator[];
    SelectedDateType: Operator;
    IsModeDate: boolean = false;
    ObjectFieldPM: ObjectFieldPM;
    IsCustomCombox: boolean = false;
    AutomationCondationFieldListFilterItems: ApiQueryFilters;
    CustomAutomationCondationFieldListFilterItems: ApiQueryFilters;
    SystemVariableOperatorLists: Operator[];
    SelectedSystemVariableOperator: Operator;

    AutomationEntityLists: AutomationEntityList[];
    SelectedAutomationEntity: AutomationEntityList;
   PartnerObjectFieldId: string = null;
   CurrentEntityType: string;
   ObjectFieldId: string = "";
   IsRefreshAutomationCondationField: boolean;

   //IsSystemVariables: boolean = false;
    CustomObjectFieldId: string = "";
    AutomationHelper: AutomationHelper;
    constructor(entityPM: AutomationCondition, addEditAutomationsViewModel: any, delayAutomationconditionsViewModel = null) {
        super();
        this.CurrentEntityPM = entityPM;
        this.AddEditAutomationsViewModel = addEditAutomationsViewModel;
        this.DelayAutomationconditionsViewModel = delayAutomationconditionsViewModel;

        this.AutomationHelper = new AutomationHelper(this.CurrentEntityPM, this.AddEditAutomationsViewModel, this, "Condation");

        this.AllowedinAutomationConditionsFieldLists = addEditAutomationsViewModel.AllowedinAutomationConditionsFieldLists;
        this.ObjectFieldPM = this.AllowedinAutomationConditionsFieldLists.filter(d => d.Id == this.CurrentEntityPM.ObjectFieldId)[0];
        this.PartnerObjectFieldId = this.CurrentEntityPM.PartnerObjectFieldId ? this.CurrentEntityPM.PartnerObjectFieldId:null;

        this.FieldValue = this.CurrentEntityPM.Value;
        this.DateTypeList = [];

        this.FillAutomationEntityObjectField();

        this.DateTypeList.push(new Operator("@Today-", "-"));
        this.DateTypeList.push(new Operator("@Today+", "+"));
        if (this.SelectedAutomationEntity != null && this.SelectedAutomationEntity.ObjectTableId == this.AddEditAutomationsViewModel.ObjectTableId) {
            this.DateTypeList.push(new Operator("@Old Value-", "-"));
            this.DateTypeList.push(new Operator("@Old Value+", "+"));
        }
        this.DateTypeList.push(new Operator("Date", "Date"));


        this.SelectedDateType = this.DateTypeList[0];
        this.CurrentEntityType = this.AddEditAutomationsViewModel.CurrentEntityPM.Type;
        this.SystemVariableOperatorLists = [];
        this.SystemVariableOperatorLists.push(new Operator("System User", "SystemUser"));


        if (this.ObjectFieldPM) {
            this.ChosenOperatorList(this.ObjectFieldPM.DataTypeCode, false, this.ObjectFieldPM);
            this.ObjectFieldId = this.ObjectFieldPM.Id;
 

            this.SelectedCustomField = this.ObjectFieldPM;


            this.UIProperties.SetEnabled(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, true);
            this.UIProperties.SetRequired(this.SelectedCustomField.FieldName, this.AddEditAutomationsViewModel.ObjectTableName, false);


            if (this.CurrentEntityPM) this.CurrentEntityPM.ObjectFieldType = this.SelectedCustomField.DataTypeCode;

            if (this.ObjectFieldPM.DataTypeCode == "DateTime" || this.ObjectFieldPM.DataTypeCode == "Date") {
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
                        this.FieldValue = new Date();
                    }
                }


            }

            if (this.CurrentEntityPM.Value && this.CurrentEntityPM.Value.indexOf("@StatusName:") > -1) {
                this.FieldValue = this.CurrentEntityPM.Value.split("@StatusName:")[0];
                this.AddEditAutomationsViewModel.IsChangeCondition = true;
            }  

           
        }
        else {
            this.ChosenOperatorList("Text", false);
        }



        if (this.CurrentEntityPM != null) {
            this.SelectedOperator = this.OperatorList.filter(d => d.Code == this.CurrentEntityPM.OperatorCode)[0];

        }
        if (!this.SelectedOperator) {
            this.SelectedOperator = this.OperatorList.filter(d => d.Code == "=")[0];

            if (this.SelectedOperator) {
                this.CurrentEntityPM.OperatorCode = this.SelectedOperator.Code;
            }


        }

        if (this.SelectedOperator) {
            this.BuildCustomFromFieldbjectFieldLists();
            if (this.HideGeneralControl(this.SelectedOperator.Code)) {
                this.IsHideGeneralControl = true;
                this.FieldValue = "";
                this.CurrentEntityPM.Value = "";
            }

            if (this.SelectedOperator.Code == "EqualSystemVariable") {
                this.SelectedSystemVariableOperator = this.SystemVariableOperatorLists.filter(d => d.Code == this.FieldValue)[0];
                if (!this.SelectedSystemVariableOperator) {
                    this.SelectedSystemVariableOperator = this.SystemVariableOperatorLists[0];
                    this.AutomationHelper.ConditionValueChange(this.SelectedSystemVariableOperator.Code);
                }
            }
        }

        this.IsLoadOperatorList = true;


    }


    ngOnInit() {


    }

    FillAutomationEntityObjectField() {
        var translation: TextCodeTranslationPipe = new TextCodeTranslationPipe();
        var objectTableId = this.AddEditAutomationsViewModel.ObjectTableId;
        var objectTableName = this.AddEditAutomationsViewModel.IsMasterShipment ? "Master" : this.AddEditAutomationsViewModel.ObjectTableName;
        this.AutomationEntityLists = [];
        this.AutomationEntityLists.push(new AutomationEntityList((objectTableName == "Master" ? "Shipment" : objectTableName), objectTableId, null));
        window.ObjectFields.filter(f => f.DisplayInAutomationAsEnitity == true && f.ObjectTableId ==objectTableId  && (!f.RecordType || (f.RecordType && f.RecordType.split(',').filter(d => d == objectTableName)[0]))).forEach((objectField) => {
            this.AutomationEntityLists.push(new AutomationEntityList(objectField.FullNameTextCodeDefaultText, objectField.LookUpTableId, objectField.Id));
        });

        this.SelectedAutomationEntity = this.AutomationEntityLists.filter(d => d.ObjectFieldId == this.PartnerObjectFieldId)[0];
        if (!this.SelectedAutomationEntity) {
            this.SelectedAutomationEntity =   this.AutomationEntityLists.filter(d => d.ObjectTableId == this.AddEditAutomationsViewModel.ObjectTableId)[0];
        }

        if (this.SelectedAutomationEntity) {
            this.InitLOVFilters(this.SelectedAutomationEntity.ObjectTableId);
        }

    }

    get IsSystemVariables() {
        var result = false;
        if (this.SelectedOperator) {
            if (this.SelectedOperator.Code == "EqualSystemVariable") result = true;
        }

        return result;


    }



    InitLOVFilters(objectTableId:string) {

        this.AutomationCondationFieldListFilterItems = new ApiQueryFilters();
        this.AutomationCondationFieldListFilterItems.addAdditionalFilter("ObjectTableId", objectTableId, null, null, "Equals", false, false, false, "string");
        this.AutomationCondationFieldListFilterItems.addAdditionalFilter("AllowedinAutomationConditions", true, null, null, "Equals", true, false, false, "boolean");
        this.AutomationCondationFieldListFilterItems.Tenant = 0;

    }

    InitCustomLOVFilters(objectFieldPM: ObjectFieldPM) {
        var objectTableId: string = this.AddEditAutomationsViewModel.ObjectTableId;
        if (this.SelectedAutomationEntity) {
            objectTableId = this.SelectedAutomationEntity.ObjectTableId;
        }

        this.CustomAutomationCondationFieldListFilterItems = new ApiQueryFilters();
        this.CustomAutomationCondationFieldListFilterItems.addAdditionalFilter("ObjectTableId", objectTableId, null, null, "Equals", false, false, false, "string");
        this.CustomAutomationCondationFieldListFilterItems.addAdditionalFilter("AllowedinAutomationConditions", true, null, null, "Equals", true, false, false, "boolean");
        if (this.SelectedCustomField) {
            if (this.SelectedCustomField.DataTypeCode == "LookUp") {
                this.CustomAutomationCondationFieldListFilterItems.addAdditionalFilter("LookUpTableId", this.SelectedCustomField.LookUpTableId, null, null, "Equals", false, false, false, "string");
            }
            this.CustomAutomationCondationFieldListFilterItems.addAdditionalFilter("DataTypeCode", this.SelectedCustomField.DataTypeCode, null, null, "Equals", false, false, false, "string");
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
    BooleanListValueChanged(newValue: boolean) {
        this.isChecked = newValue;
        this.CurrentEntityPM.Value = this.FieldValue = this.isChecked ? "true" : "false";
        this.AddEditAutomationsViewModel.IsChangeCondition = true;
    }
    //set IsChecked(newValue: boolean) {
    //    this.isChecked = newValue;
    //    this.CurrentEntityPM.Value = this.FieldValue = this.isChecked ? "true" : "false";
    //    this.AddEditAutomationsViewModel.IsChangeCondition = true;
    //}
   
    ChosenOperatorList(dataTypeCode: string, isChangeOperator: boolean, objectFieldPM: ObjectFieldPM = null) {
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

        else if (dataTypeCode == "Boolean") this.OperatorList.push(new Operator("Equals", "="));
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
            if (this.SelectedAutomationEntity!=null && this.SelectedAutomationEntity.ObjectTableId == this.AddEditAutomationsViewModel.ObjectTableId) {
                this.OperatorList.push(new Operator("Changed to", "CHANGEDTO"));
                this.OperatorList.push(new Operator("Changed", "CHANGED"));
            }
        }


        if (dataTypeCode == "DateTime" || dataTypeCode == "Date" ) {

            if (!this.OperatorList.filter(d => d.Code == "CHANGED")[0]) {
                if (this.SelectedAutomationEntity != null && this.SelectedAutomationEntity.ObjectTableId == this.AddEditAutomationsViewModel.ObjectTableId) {
                    this.OperatorList.push(new Operator("Changed", "CHANGED"));
                }
            }
            this.OperatorList.push(new Operator("Is Empty", "ISEMPTY"));
            this.OperatorList.push(new Operator("Is not Empty", "ISNOTEMPTY"));
        }


        if (isChangeOperator) {
            this.CurrentEntityPM.OperatorCode = this.OperatorList[0] ? this.OperatorList[0].Code: "=";
        }


    }


    CustomFieldValueChanged(item) {
        {
            if (item) {
        
             
                var oldObjectFieldId: string = this.SelectedCustomField ? this.SelectedCustomField.Id : "";
                if (item.Id != oldObjectFieldId) {

                    this.IsHideGeneralControl = false;
                    this.FieldValue = item.DataTypeCode == "Date" || item.DataTypeCode == "DateTime" ? 0 : "";


                    if (item.DataTypeCode == "Date" || item.DataTypeCode == "DateTime") {
                        var todayDate = DateTool.GetCurrentDateTimeAsUtc();
                        this.CurrentEntityPM.Value = this.SelectedDateType.Name + "*" + this.FieldValue + "*" + FieldValueResolver.ConvertUTCDateToString(todayDate, "Automation");
                    } else this.CurrentEntityPM.Value = "";

                   
                    this.AddEditAutomationsViewModel.IsChangeCondition = true;
                    var ischange = false;

                    if (this.SelectedCustomField && this.SelectedCustomField.DataTypeCode == item.DataTypeCode) {
                        ischange = true;
                    }

                    this.SelectedCustomField = this.AllowedinAutomationConditionsFieldLists.filter(d => d.Id == item.Id)[0];
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

    }

    DeleteAutomationConditionMethod(item: AutomationConditionViewModel) {
        if (this.CurrentEntityPM.ConditionType == "And") {

            if (this.DelayAutomationconditionsViewModel) {

                var index = this.DelayAutomationconditionsViewModel.AutomationCondationAndList.indexOf(item);
                if (index != -1) this.DelayAutomationconditionsViewModel.AutomationCondationAndList.splice(index, 1);

            }
            else {
                var index = this.AddEditAutomationsViewModel.AutomationCondationAndList.indexOf(item);
                if (index != -1) this.AddEditAutomationsViewModel.AutomationCondationAndList.splice(index, 1);
            }
        }

        else {

            if (this.DelayAutomationconditionsViewModel) {

                var index = this.DelayAutomationconditionsViewModel.AutomationCondationOrList.indexOf(item);
                if (index != -1) this.DelayAutomationconditionsViewModel.AutomationCondationOrList.splice(index, 1)
            }
            else {

                var index = this.AddEditAutomationsViewModel.AutomationCondationOrList.indexOf(item);
                if (index != -1) this.AddEditAutomationsViewModel.AutomationCondationOrList.splice(index, 1);
            }
        }

        this.AddEditAutomationsViewModel.IsChangeCondition = true;
    }


    HideGeneralControl(operatorCode: string) {
        var result: boolean = false;
        if (operatorCode == "CHANGED" || operatorCode == "ISEMPTY" || operatorCode == "ISNOTEMPTY") {
            result = true;
        }

        return result;
    }


    AutomationEntityListValueChanged(value) {
        if (value) {
            this.ObjectFieldId = null;
            this.PartnerObjectFieldId = this.CurrentEntityPM.PartnerObjectFieldId = value.ObjectFieldId ? value.ObjectFieldId:null;
            this.SelectedAutomationEntity = this.AutomationEntityLists.filter(d => d.ObjectFieldId == this.PartnerObjectFieldId)[0];

            if (!this.SelectedAutomationEntity) {
                this.SelectedAutomationEntity =   this.AutomationEntityLists.filter(d => d.ObjectTableId == this.AddEditAutomationsViewModel.ObjectTableId)[0];
            }


            this.FieldValue = "";
            this.CurrentEntityPM.Value = "";

            this.IsRefreshAutomationCondationField = !this.IsRefreshAutomationCondationField;
            this.IsRefrachCustomField = !this.IsRefrachCustomField;
            this.IsHideGeneralControl = true;

            if (this.SelectedAutomationEntity) {
                this.InitLOVFilters(this.SelectedAutomationEntity.ObjectTableId);
            }


        }
    }



    IsHideGeneralControl: boolean = false;
    OperatorListValueChanged(item) {
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

            } else {
                this.IsHideGeneralControl = false;
                if (item.Code.indexOf("F") != -1) {
                    if (this.CurrentEntityPM.OperatorCode.indexOf("F") == -1) {
                        IsReloadGenerateControl = true;
                    }
                }
                else {
                    if (this.CurrentEntityPM.OperatorCode.indexOf("F") != -1 ) {
                        IsReloadGenerateControl = true;
                    }
                }

                if (item.Code.indexOf("F") != -1) {
                    this.IsCustomCombox = true;

                } else this.IsCustomCombox = false;



                this.IsSetValue = false;

                this.BuildCustomFromFieldbjectFieldLists();
            }
        }
    }

    BuildCustomFromFieldbjectFieldLists() {

        if (this.SelectedOperator != null && this.SelectedCustomField) {

            if (this.SelectedOperator.Code.indexOf("F") != -1) {
                this.IsCustomCombox = true;
                if (this.SelectedCustomField) {
                    this.CustomObjectFieldId = this.FieldValue;
                    this.InitCustomLOVFilters(this.SelectedCustomField);
                }
          
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


    SystemVariableCondationValueChanged(value) {
        this.AutomationHelper.SystemVariableCondationValueChanged(value);
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



class AutomationEntityList {
    Name: string;
    ObjectTableId: string;
    ObjectFieldId: string;
    constructor(name: string, objectTableId: string, objectFieldId:string) {
        this.Name = name;
        this.ObjectTableId = objectTableId;
        this.ObjectFieldId = objectFieldId;

    }


}
