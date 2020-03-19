declare var System: any;
declare var window: any;
import {Component, OnInit}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityPMService} from '../../../../Infrastructure/Services/EntityPMService';
import {DateAgeHelper} from '../../../../Infrastructure/Utilities/DateAgeHelper';
import {DocsOutDataViewModel} from './DocsOut/ViewModel/DocsOutDataViewModel';
import {ObjectFieldPM} from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ScreenColumn} from '../../../../Infrastructure/GenericComponents/GeneratedComponent';
import {CustomFieldClass} from '../../../../Infrastructure/DataContracts/CustomFieldClass';
import {CachedDataManager} from '../../../../Infrastructure/Utilities/CachedDataManager';
import {DocumentCustomFieldsArgs} from './DocsOut/Filters/DocumentCustomFieldsArgs';
declare var jQuery: any;
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {FormBuilder, FormGroup} from '@angular/forms';
import {AppTool} from '../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';

@Component({
    moduleId: module.id,
    selector: 'GeneratedDocumentCustomFieldComponent',
    templateUrl: './GeneratedDocumentCustomFieldComponent.html',
    inputs: ['DocumentCustomArgs'],    
})

export class GeneratedDocumentCustomFieldComponent extends BaseComponent implements OnInit {
    DateAgeHelper: DateAgeHelper = new DateAgeHelper(null);
    public CustomFieldLists: CustomFieldViewModel[];
    SelectedCustomFieldViewModel: CustomFieldViewModel;
    public DocumentCustomArgs: DocumentCustomFieldsArgs;
    ObjectTableName: string;
    dateitem: Date;
    ObjectTableId: string;
    ScreenCode: string;
    ShowNoFieldsText: boolean;
    private entityPMService: EntityPMService;
    HasError: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor( ) {
        super();
        this.entityPMService = new EntityPMService();

    }


    ngOnInit() {

        this.ObjectTableId = this.DocumentCustomArgs.ObjectTableId;
        this.ObjectTableName = this.DocumentCustomArgs.ObjectTableName;
        this.ScreenCode = this.DocumentCustomArgs.ScreenCode;
        this.DocumentCustomArgs.GeneratedDocumentCustomFieldComponent = this;
        this.LoadData();

    }

    LoadData() {
        this.CustomFieldLists = [];
        var myScreenColumns: ScreenColumn[] = [];
        var myScreen = window.Screens.filter((x: any) => x.ObjectTableId === this.ObjectTableId && x.Code.toLowerCase() == this.ScreenCode.toLowerCase())[0];

        if (myScreen != null) {

            var myScreenFields = window.ScreenFields.filter((x: any) => x.ScreenId === myScreen.Id && x.Tenant === SessionInfo.LoggedUserTenant);

            if (myScreenFields.length == 0) {
                myScreenFields = window.ScreenFields.filter((x: any) => x.ScreenId === myScreen.Id);
            }

            if (myScreenFields.length == 0) {
                this.ShowNoFieldsText = true;
            }

            else {

                var myObjectFields = window.ObjectFields.filter((x: any) => x.ObjectTableId === this.ObjectTableId);
                //this.OnCreateAutomationList = this.OnCreateAutomationList.sort((a, b) => { return a.Order - b.Order });
                myScreenFields.sort((a, b) => { return a.Row - b.Row }).forEach(screenFields => {
                    var myObjectField = myObjectFields.filter((f: any) => f.FieldCode == screenFields.ObjectFieldCode)[0];
                    if (myObjectField) {

                        this.CustomFieldLists.push(new CustomFieldViewModel(myObjectField, this.DocumentCustomArgs.EntityPM, this.DocumentCustomArgs.EditCustomField, this.ObjectTableName));
                    }

                });

            }
        }

    }
    ValueChange(value, item: CustomFieldViewModel) {
        var newValue: any;
        if (item.ObjectField.IsCustom) {
                newValue = value.FieldName ? value.ResolvedValue : value;
        }
        else newValue = value;  
        if (item.FieldValue != newValue) {
            item.FieldValue = newValue;
            this.EditCustomField(item,true);
        }
    }
    EditCustomField(item: CustomFieldViewModel, ischange: boolean = false) {
        if (item.EntityPM ) {
        
            if (item.FieldDataTypeCode == "Boolean") {
                item.FieldValue = !item.FieldValue;
            }


            if (item.ObjectField.IsCustom) {
                var oldvalue: any = item.EntityPM[item.FieldName]; 

                var newValue: CustomFieldClass = new CustomFieldClass(item.FieldValue, item.FieldName, this.ObjectTableName);

                var oldValueText: any = oldvalue ? oldvalue.Value : "";

                var newValueText: any = newValue ? newValue.Value : "";
                if (oldValueText != newValueText) {
               
                    item.EntityPM[item.FieldName] = newValue;
                    ischange = true;
                }



            }
            else if (item.EntityPM[item.FieldName] != item.FieldValue){
                item.EntityPM[item.FieldName] = item.FieldValue;
                ischange = true;
            }

            if (ischange && item.EntityPM.IsDirty) {
                //Save
                this.DocumentCustomArgs.IsEditCustomField = true;
                this.DocumentCustomArgs.IsChangeCustomField = true;
            
                if (this.DocumentCustomArgs.editDocumentComponent != null) {
                    this.DocumentCustomArgs.editDocumentComponent.ValidationErrorsList = [];
                }


                this.CurrentSession.StartBusyIndicatorSaving();
                this.entityPMService.update(this.ObjectTableName, item.EntityPM).then((res: any) => {
                    res.subscribe((myResponse: ServiceResponse) => {

                        this.CurrentSession.StopBusyIndicator();
                        if (myResponse.HasError) {
                            this.HasError = true;
                            if (this.DocumentCustomArgs.editDocumentComponent != null) {
                                this.DocumentCustomArgs.editDocumentComponent.ValidationErrorsList = myResponse.ErrorsArray;
                            }
                        }
                        else {
                            this.HasError = false;
                            item.EntityPM = myResponse.Result;
                            var objectTable:any = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
                            if (objectTable && objectTable.CacheOnClient) {
                                CachedDataManager.RefreshTableData(this.ObjectTableName, true);
                            }


                            if (this.DocumentCustomArgs.editDocumentComponent != null) {
                                this.DocumentCustomArgs.editDocumentComponent.LoadstimulData(null, true, true, 1, "GenerateReport", "", "Refreshing Document...");
                                this.DocumentCustomArgs.IsChangeCustomField = false;
                            }
                        }
                    });
                });
            }


        }

    }

    SetCustomFieldItem(item: CustomFieldViewModel) {
        this.SelectedCustomFieldViewModel= item;

    }
    public GetDateFromString(datestring: string, fromat) {

        if (datestring) {
            var dateAndTime: string[];
            var timeArray: string[];
            var dateArray: string[];
            var suffix: string;
            dateAndTime = datestring.split(' ');

            if (fromat == 2) {
                dateArray = dateAndTime[0].split('/');
                var day: number = Number(dateArray[0]);
                var month: number = Number(dateArray[1]) - 1;
                var year: number = Number(dateArray[2]);



                timeArray = dateAndTime[1].split(':');
                var hour: number = this.DateAgeHelper.GetTimeFor24Mode(Number(timeArray[0]), suffix);
                var minute: number = Number(timeArray[1]);
                var second: number = Number(timeArray[2]);
            }
            else {

                var month: number = Number(this.getMonthFromString(dateAndTime[1]));
                var day: number = Number(dateAndTime[2]);
                var year: number = Number(dateAndTime[3]);

                timeArray = dateAndTime[4].split(':');
                var hour: number = this.DateAgeHelper.GetTimeFor24Mode(Number(timeArray[0]), suffix);
                var minute: number = Number(timeArray[1]);
                var second: number = Number(timeArray[2]);

            }
            var date: Date = this.DateAgeHelper.GetDate(year, month, day, hour, minute, second);
            return date;
        }
    }

    getMonthFromString(mon) {

        var d = Date.parse(mon + "1, 2012");
        if (!isNaN(d)) {
            var x = new Date(d).getMonth();
            return x;
        }
        return -1;
    }



}

export class CustomFieldViewModel {
    FieldValue: any;
    FieldName: string;
    FieldDataTypeCode: string;
    MultiLine: boolean;
    Name: string;
    keyYes: string;
    keyNo: string;
    IsEnableEditCustomField: boolean;
    ObjectField: ObjectFieldPM;
    EntityPM: any;
    ObjectTableName: string;
    public constructor(objectField: ObjectFieldPM, entityPM: any, isEnableEditCustomField: boolean, objectTableName:string) {
        this.ObjectField = objectField;
        this.FieldName = objectField.FieldName;
        this.MultiLine = objectField.MultiLine;
        this.FieldDataTypeCode = objectField.DataTypeCode;
        this.IsEnableEditCustomField = isEnableEditCustomField;
        this.keyNo = Guid.newGuid();
        this.keyYes = Guid.newGuid(); 
        this.ObjectTableName = objectTableName;

        this.EntityPM = entityPM; 
         this.Name = TextCodeTranslator.Translate(objectField.FullNameTextCodeCode);
        var value: any;
        if (entityPM) {

            if (objectField.IsCustom) {
                if (entityPM[this.FieldName]) {
                    value = entityPM[this.FieldName].ResolvedValue;
                }
            }
            else value = entityPM[this.FieldName];            
        }

        if (value) {

            if (this.FieldDataTypeCode == "Boolean") {
                if (value.toString().toLowerCase() == "false") {
                    this.FieldValue = isEnableEditCustomField ? false : "No";
                } else this.FieldValue = isEnableEditCustomField ? true : "Yes";

            }
            else {
                this.FieldValue = value;
            }

        }
        else {
            if (this.FieldDataTypeCode == "Boolean") {
                this.FieldValue = isEnableEditCustomField ? false : "No";
            }
        }
        if (this.FieldDataTypeCode == "LookUp" ) {
            //this.EntityPM.UIProperties.SetEnabled(this.ObjectField.FieldName, this.ObjectTableName, isEnableEditCustomField);

        }

    }
         
}
