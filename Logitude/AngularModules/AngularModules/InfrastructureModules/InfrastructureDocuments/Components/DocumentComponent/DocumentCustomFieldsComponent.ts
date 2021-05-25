declare var System: any;
declare var window: any;
import {Component, OnInit}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DateAgeHelper} from '../../../../Infrastructure/Utilities/DateAgeHelper';
import {DocsOutDataViewModel} from './DocsOut/ViewModel/DocsOutDataViewModel';
import {DocumentTypeCustomFieldPM} from '../../../../Common/EntityPMs/DocumentTypeCustomFieldPM';
import {DocumentTypeCustomFieldPMViewModel} from './ViewModel/DocumentTypeCustomFieldPMViewModel';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {FormCustomFieldPM} from '../../../../Common/EntityPMs/FormCustomFieldPM';
import {DocumentTypeCustomFieldService} from '../../../../Common/Services/ExtendedPMs/DocumentTypeCustomFieldService';
import {DocumentCustomFieldsArgs} from './DocsOut/Filters/DocumentCustomFieldsArgs';
declare var jQuery: any;
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {FormBuilder, FormGroup} from '@angular/forms';
import {AppTool} from '../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    
    selector: 'DocumentCustomFields',
    templateUrl: './DocumentCustomFieldsComponent.html',
    inputs: ['DocumentCustomArgs'],
    providers: [DocumentTypeCustomFieldService],
})

export class DocumentCustomFieldsComponent extends BaseComponent implements OnInit {
    DateAgeHelper: DateAgeHelper = new DateAgeHelper(null);
    public DocumentTypeCustomFieldLists: DocumentTypeCustomFieldPMViewModel[];
    public FormCustomFieldPMLists: FormCustomFieldPM[];
    public DocumentCustomArgs: DocumentCustomFieldsArgs;
    public  SelectedDocumentTypeCustomFieldPMViewModel: DocumentTypeCustomFieldPMViewModel;

    dateitem: Date;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _documentTypeCustomFieldService: DocumentTypeCustomFieldService) {
        super();
      

    }


    ngOnInit() {

        this.DocumentCustomArgs.DocumentCustomFields = this;
        this.LoadFormCustomFieldsByDocument();

    }
 
    public LoadFormCustomFieldsByDocument() {

        this.DocumentTypeCustomFieldLists = [];

        this.DocumentCustomArgs.DocumentTypeCustomFieldLists.forEach((item) => {
            this.DocumentTypeCustomFieldLists.push(new DocumentTypeCustomFieldPMViewModel(item,this));
        });

        this._documentTypeCustomFieldService.getFormCustomFieldsByDocument(SessionInfo.LoggedUserTenant, this.DocumentCustomArgs.DocumentTypeId, this.DocumentCustomArgs.EntityId, this.DocumentCustomArgs.ObjectTableId).subscribe((res:any) => {
         

            var pmResponse: ServiceResponse = res;

            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.FormCustomFieldPMLists = myResult;

                    this.DocumentTypeCustomFieldLists.forEach((item) => {

                        var value = this.FormCustomFieldPMLists.filter(d=> d.FieldCode == item.FieldCode)[0].Value;
                        if (!value) {
                            if ((item.FieldDataTypeCode == "Date" || item.FieldDataTypeCode == "DateTime") && this.DocumentCustomArgs.EditCustomField) {
                                item.IsLoad = true;
                            }
                            else value = " ";


                        }


                        else if (item.FieldDataTypeCode == "Boolean") {
                            if (value.toLocaleLowerCase() == "false") {
                                if (this.DocumentCustomArgs.EditCustomField) {
                                    item.FieldValue = false;
                                }
                                else {
                                    item.FieldValue = "No";
                                }


                            }

                            else if (value.toLocaleLowerCase() == "true") {

                                if (this.DocumentCustomArgs.EditCustomField) {
                                    item.FieldValue = true;
                                }
                                else {
                                    item.FieldValue = "Yes";
                                }

                            }

                        }
                        else if ((item.FieldDataTypeCode == "Date" || item.FieldDataTypeCode == "DateTime") && this.DocumentCustomArgs.EditCustomField) {
                         
                        
                            if (value.indexOf('GMT') > -1) {

                                item.FieldValue = this.GetDateFromString(value.split('GMT')[0], 1);
                            }
                            else if (value.indexOf('T') > -1) {

                                item.FieldValue = this.DateAgeHelper.GetDateFromString(value);
                            }
                            else {
                                item.FieldValue = this.GetDateFromString(value , 2);
                            }
                            
                            item.IsLoad = true;
                      


                        }

                        else {
                            item.FieldValue = value;
                        }




                    });



                }
            }

           



        });


    }

    EditCustomField(item: DocumentTypeCustomFieldPMViewModel) {

       
        this.DocumentCustomArgs.IsEditCustomField = true;
        this.DocumentCustomArgs.IsChangeCustomField = true;
        var fromCustomFieldPM = this.FormCustomFieldPMLists.filter(d => d.FieldCode == item.FieldCode)[0];

        if (item.FieldDataTypeCode == "Boolean") {
            item.FieldValue = !item.FieldValue;
            if (item.FieldValue) fromCustomFieldPM.Value = "True";
            else fromCustomFieldPM.Value = "False";
            this.SaveAndRefresh(fromCustomFieldPM);
        }

        else {
            if (fromCustomFieldPM.Value != item.FieldValue) {
                fromCustomFieldPM.Value = item.FieldValue;
                this.SaveAndRefresh(fromCustomFieldPM);
            }
        }
       
    }

    ValueDateChange(date, item: DocumentTypeCustomFieldPMViewModel) {

        if (item.FieldValue != date) {
            item.FieldValue = date;
            this.DocumentCustomArgs.IsEditCustomField = true;
            this.DocumentCustomArgs.IsChangeCustomField = true;
            var fromCustomFieldPM = this.FormCustomFieldPMLists.filter(d => d.FieldCode == item.FieldCode)[0];
            if (item.FieldDataTypeCode == "DateTime" || item.FieldDataTypeCode == "Date") {

                fromCustomFieldPM.Value = item.FieldValue.toString();
                this.SaveAndRefresh(fromCustomFieldPM);

            }
        }
    }

       


    SetCustomFieldItem(item: DocumentTypeCustomFieldPMViewModel) {
        this.SelectedDocumentTypeCustomFieldPMViewModel = item;

    }

    public SaveAndRefresh(item: FormCustomFieldPM) {

        if (this.ValidateCustomFields().length > 0) {
            return;
        }
        this.CurrentSession.StartBusyIndicator("Saving...");
        if (this.DocumentCustomArgs.editDocumentComponent != null) {
            this.DocumentCustomArgs.editDocumentComponent.ValidationErrorsList = [];
        }

        item.ObjectTableId = this.DocumentCustomArgs.ObjectTableId;
        this._documentTypeCustomFieldService.UpdateFormCustomField(item).subscribe((res:any) => {
            this.CurrentSession.StopBusyIndicator();
            if (res.HasError) {
                if (this.DocumentCustomArgs.editDocumentComponent != null) {
                    this.DocumentCustomArgs.editDocumentComponent.ValidationErrorsList = res.ErrorsArray;
                }
            }
            if (this.DocumentCustomArgs.editDocumentComponent != null) {
                this.DocumentCustomArgs.editDocumentComponent.LoadstimulData(null, true, true, 1, "GenerateReport", "", "Refreshing Document...");
                this.DocumentCustomArgs.IsChangeCustomField = false;
                
            }
           
        });



    }
    
    public ValidateCustomFields() {

        var ValidationErrorsList: string[] = [];
        if (this.DocumentCustomArgs.editDocumentComponent != null) {
            this.DocumentCustomArgs.editDocumentComponent.ValidationErrorsList = [];
        }
        //this.ValidationErrorsList = [];
        this.FormCustomFieldPMLists.forEach(cf => {
            if (!AppTool.IsNullOrEmpty(cf.Value) && cf.Value.length > 250) {
                var translation: string = cf.FieldCode;
                var field: DocumentTypeCustomFieldPMViewModel = this.DocumentTypeCustomFieldLists.filter(f => f.FieldCode == cf.FieldCode)[0];
                if (field) {
                    var ft = TextCodeTranslator.TranslateCached(field.Name);
                    if (!AppTool.IsNullOrEmpty(ft)) {
                        translation = ft;
                    }
                    //var trans = TextCodeTranslator.
                }


                var error: string = translation + " Maximum length should be less than 1000";
                

                if (this.DocumentCustomArgs.editDocumentComponent != null) {
                    ValidationErrorsList.push(error);
                }
    //            DocumentTypeCustomFieldPM field = DocumentCustomFieldsList.FirstOrDefault(f => f.FieldCode == cf.FieldCode);
    //            if (field != null) {
    //                FieldsTranslations ft = FieldsTranslationsCachedDataProvider.GetFieldsTranslations(field.Name);

    //                if (ft != null) {
    //                    translation = ft.TranslatedText;
    //                }
    //                else {
    //                    translation = field.Name;

    //                }
    //            }

    //            ValidationResult error = new ValidationResult(translation + " Maximum length should be less than 1000");
    //            errors.Add(error);
            }
        });

        this.DocumentCustomArgs.editDocumentComponent.ValidationErrorsList = ValidationErrorsList;
        return ValidationErrorsList;

        
        //for (FormCustomFieldPM cf in this.FormCustomFieldPMLists) {
        //    if (!AppTool.IsNullOrEmpty(cf.v) && cf.Value.Length > 250) {
        //    }
        //}
    }

    //public List<ValidationResult> ValidateCustomFields() {
    //    List < ValidationResult > errors = new List<ValidationResult>();
    //    foreach(FormCustomFieldPM cf in fromCustomFieldsList)
    //    {
    //        if (!string.IsNullOrEmpty(cf.Value) && cf.Value.Length > 250) {
    //            string translation = cf.FieldCode;

    //            DocumentTypeCustomFieldPM field = DocumentCustomFieldsList.FirstOrDefault(f => f.FieldCode == cf.FieldCode);
    //            if (field != null) {
    //                FieldsTranslations ft = FieldsTranslationsCachedDataProvider.GetFieldsTranslations(field.Name);

    //                if (ft != null) {
    //                    translation = ft.TranslatedText;
    //                }
    //                else {
    //                    translation = field.Name;

    //                }
    //            }

    //            ValidationResult error = new ValidationResult(translation + " Maximum length should be less than 1000");
    //            errors.Add(error);
    //        }
    //    }

    //    if (errors.Count > 0) {
    //        this.previewControl.FillErrors(errors);

    //    }

    //    return errors;
    //}


    public GetDateFromString(datestring: string , fromat) {

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
        var x = new Date(d).getMonth() ; 
        return x;
    }
    return -1;
}

}
