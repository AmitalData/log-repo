import {ObjectFieldPM} from '../../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
declare var System: any;
declare var window: any;
import {AppTool} from '../../../../../Infrastructure/Tools';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';

export class DocumentObjectFieldsRowViewModel   {

    CurrentObjectField: ObjectFieldPM;
    ResultFieldName: string; 
    ObjectFieldType: string;
    FieldName: string;
    IsViewTree: boolean;
    HasTree: boolean;
    DivSelectBackgroud: string;
   Items: DocumentObjectFieldsRowViewModel[];
   Id: string;
    TranslatedText: string;
    FullNameTextCodeCode: string;
    Order: number;
    ObjectTableId: string;
    ObjectTableName: string;

    DisplayListOnly: boolean;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public constructor(objectField: ObjectFieldPM, resultFieldName: string, objectFieldType: string) {

        if (objectField) {
            this.CurrentObjectField = objectField;

     

            this.FieldName = objectField.FieldName;
            this.ObjectTableId = objectField.ObjectTableId;
            this.ObjectTableName = objectField.ObjectTableName;
            this.ResultFieldName = resultFieldName;
            this.ObjectFieldType = objectFieldType;

            this.Id = objectField.Id;

            if (!this.Id) {

                this.Id = Guid.newGuid();
            }
            var result = "";
            if ((objectField.FieldName == "Date" || objectField.FieldName == "Time") && objectField.DataTypeCode == "None") {
                result = objectField.FieldName;
            }
            else {
                //if (this.CurrentObjectField.ShortNameTextCodeCode) {
                //    result = TextCodeTranslator.Translate(objectField.ShortNameTextCodeCode)

                //}
                //else {
                    result = TextCodeTranslator.Translate(objectField.FullNameTextCodeCode)
                //}
            }

            if (result) {
                this.TranslatedText = result;
            }

            else {
                this.TranslatedText = this.FieldName;
            }


            if (!AppTool.IsNullOrEmpty(this.TranslatedText)) {
                if (this.TranslatedText.indexOf("(%") > -1 && this.TranslatedText.indexOf("Code)") > -1 ) {
                    this.TranslatedText = this.TranslatedText.split('(%')[0];
                }
            }
            this.FullNameTextCodeCode = objectField.FullNameTextCodeCode;

            if (objectField.DataTypeCode == "LookUp" || objectField.IsMulti || objectField.DataTypeCode == "DateTime") {
                this.HasTree = true;

            }

            else {
                this.HasTree = false;
            }

        }

    }

    Load() {

        this.Items = new Array<DocumentObjectFieldsRowViewModel>();
        var list = new Array<ObjectFieldPM>();
        var views = new Array<DocumentObjectFieldsRowViewModel>();
        if (this.CurrentObjectField.IsMulti || this.CurrentObjectField.DataTypeCode == "LookUp") {

            var table = null;
            if (this.CurrentObjectField.IsMulti) {
                table = window.ObjectTables.filter(d=> d.Id == this.CurrentObjectField.MultiTableId)[0];
            }
            else {
                table = window.ObjectTables.filter(d=> d.Id == this.CurrentObjectField.LookUpTableId)[0];
            }
            this._entityResourceService.getEntityResourceByTableName(table.Name).subscribe(response=> {
                if (this.CurrentObjectField.IsMulti) {
                    table = window.ObjectTables.filter(d=> d.Id == this.CurrentObjectField.MultiTableId)[0];
                    list = window.ObjectFields.filter(f=> f.ObjectTableId == table.Id && (f.PMPropertyPath != null || f.ListPropertyPath != null) && f.FieldName != "TimeFrameFilter" && !f.DisplayOnly && f.DisplayInEntityVariables == true);
                } else if (this.CurrentObjectField.DataTypeCode == "LookUp") {
                    table = window.ObjectTables.filter(d=> d.Id == this.CurrentObjectField.LookUpTableId)[0];
                    if (!this.DisplayListOnly) {

                        if (this.ObjectFieldType == "LookUpContactOrUser" || this.ObjectFieldType == "Emails") {
                            list = window.ObjectFields.filter(f=> f.ObjectTableId == this.CurrentObjectField.LookUpTableId && (f.PMPropertyPath == "Email" || f.ListPropertyPath == " Email") && f.FieldName != "TimeFrameFilter" && !f.DisplayOnly && f.DisplayInEntityVariables == true);

                        }
                        else {

                            list = window.ObjectFields.filter(f=> f.ObjectTableId == this.CurrentObjectField.LookUpTableId && (f.PMPropertyPath != null || f.ListPropertyPath != null) && f.FieldName != "TimeFrameFilter" && !f.DisplayOnly && f.DisplayInEntityVariables == true);

                        }

                    }
                    else {

                        list = window.ObjectFields.filter(f=> f.ObjectTableId == this.CurrentObjectField.LookUpTableId && f.ListPropertyPath != null && !f.IsMulti && f.FieldName != "TimeFrameFilter" && !f.DisplayOnly && f.DisplayInEntityVariables == true);
                    }
                }



                if (this.ResultFieldName.indexOf(".") > -1) {
                    list.forEach((objectField) => {
                        var documentObjectFieldsRowViewModel = new DocumentObjectFieldsRowViewModel(objectField, this.ResultFieldName + "." + objectField.FieldName, this.ObjectFieldType);
                        views.push(documentObjectFieldsRowViewModel);
                    });

                }
                else {
                    list.forEach((objectField) => {
                        var documentObjectFieldsRowViewModel = new DocumentObjectFieldsRowViewModel(objectField, this.CurrentObjectField.FieldName + "." + objectField.FieldName, this.ObjectFieldType);
                        views.push(documentObjectFieldsRowViewModel);
                    });


                }

                this.Items = views;
                this.IsViewTree = true;
            });


        } else if (this.CurrentObjectField.DataTypeCode == "DateTime") {
            list = new Array<ObjectFieldPM>();
            var item = new ObjectFieldPM();
            item.FieldName = "Date";
            item.DataTypeCode = "None";
            list.push(item);

            item = new ObjectFieldPM();
            item.FieldName = "Time";
            item.DataTypeCode = "None";
            list.push(item);


            if (this.ResultFieldName.indexOf(".") > -1) {
                list.forEach((objectField) => {
                    var documentObjectFieldsRowViewModel = new DocumentObjectFieldsRowViewModel(objectField, this.ResultFieldName + "." + objectField.FieldName, this.ObjectFieldType);
                    views.push(documentObjectFieldsRowViewModel);
                });

            }
            else {
                list.forEach((objectField) => {
                    var documentObjectFieldsRowViewModel = new DocumentObjectFieldsRowViewModel(objectField, this.CurrentObjectField.FieldName + "." + objectField.FieldName, this.ObjectFieldType);
                    views.push(documentObjectFieldsRowViewModel);
                });


            }

            this.Items = views;

            this.IsViewTree = true;

        }

        //if (this.ResultFieldName.indexOf(".") > -1) {
        //    list.forEach((objectField) => {
        //        var documentObjectFieldsRowViewModel = new DocumentObjectFieldsRowViewModel(objectField, this.ResultFieldName + "." + objectField.FieldName, this.ObjectFieldType);
        //        views.push(documentObjectFieldsRowViewModel);
        //    });

        //}
        //else {
        //    list.forEach((objectField) => {
        //        var documentObjectFieldsRowViewModel = new DocumentObjectFieldsRowViewModel(objectField, this.CurrentObjectField.FieldName + "." + objectField.FieldName, this.ObjectFieldType);
        //        views.push(documentObjectFieldsRowViewModel);
        //    });


        //}

      

    }


    LoadItems() {

        if (this.IsViewTree) {
            this.IsViewTree = false;
        }
        else {

            if (!this.Items) {

                this.Load();
            }
            else this.IsViewTree = true;
        }

    }




}

class Guid {
    static newGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
}
