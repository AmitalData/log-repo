declare var window: any;
declare var System: any;
import {TextCodeTranslator} from '../Utilities/TextCodeTranslator';
import {AppTool} from '../Tools';
import {ObjectFieldPM} from '../EntityPMs/ObjectFieldPM';
import {CustomFieldClass} from '../DataContracts/CustomFieldClass';

export class FieldValidator {
    public ErrorsArray: any[];
    constructor() {
        this.ErrorsArray = [];
    }

    public Validate(objectFieldName, objectTableName, entityPM) {

        var errorsArray = [];
        var objectTableId;
        var requiredErrorCode: string = "General.M.FieldIsRequired";
        var minmaxErrorCode: string = "General.M.MinMax";
        var translatedRequiredError: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var translatedMinMaxError: string = TextCodeTranslator.Translate("General.M.MinMax");
        var translatedMaxError: string = TextCodeTranslator.Translate("General.M.Max");

        var objectTable = window.ObjectTables.filter(x => x.Name === objectTableName)[0];
        if (objectTable) {

            objectTableId = objectTable.Id;
            var objectfield: ObjectFieldPM = window.ObjectFields.filter(x => x.FieldName === objectFieldName && x.ObjectTableId === objectTableId)[0];

            if (objectfield) {
                var value = entityPM[objectfield.FieldName];
                if (objectfield.IsCustom) {
                    var customfieldClass: CustomFieldClass = entityPM[objectfield.FieldName];
                    value = customfieldClass.Value;
                }
                if (objectfield.IsRequiered === true) {

                    //Boolean
                    //Constant
                    //Date
                    //DateTime
                    //Decimal
                    //Double
                    //Emails
                    //Integer
                    //List
                    //LookUp
                    //nText
                    //PickList
                    //SigDouble
                    //Text
                    //UnsDecimal
                    //UnsInteger

                    switch (objectfield.DataTypeCode) {

                        case "Integer":
                        case "Double":
                        case "Decimal":
                        case "SigDouble":
                        case "UnsDecimal":
                        case "UnsInteger":
                            {
                                if (AppTool.IsNullOrEmpty(value)) {
                                    var fieldName: string = TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
                                    var fieldError: string = translatedRequiredError.replace("%FieldName", fieldName);
                                    errorsArray.push(fieldError);
                                }

                                break;
                            }

                        case "Text":
                        case "nText":
                        case "LookUp":
                        case "DateTime":
                            {
                                if (AppTool.IsNullOrEmpty(value)) {
                                    var fieldName: string = TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
                                    var fieldError: string = translatedRequiredError.replace("%FieldName", fieldName);
                                    errorsArray.push(fieldError);
                                }

                                break;
                            }
                    }
                }

                if (value) {
                    if (objectfield.DataTypeCode === "Text" || objectfield.DataTypeCode === "nText") {
                        if (objectfield.MaxLength !== 0 || objectfield.MinLength !== 0) {
                            if (!objectfield.IsMaxLength &&
                                (value.toString().length > objectfield.MaxLength || value.toString().length < objectfield.MinLength)) {

                                var errorMsg = "";
                                if (objectfield.MinLength == 0) {
                                    // display only max length error
                                    var fieldName: string = TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
                                    var error: string = translatedMaxError.replace("%Maxlength", objectfield.MaxLength + "");
                                    error = error.replace("%FieldName", fieldName);
                                    errorMsg = error;
                                }
                                else {
                                    //display min max errors
                                    var fieldName: string = TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
                                    var minFieldError: string = translatedMinMaxError.replace("%Minlength", objectfield.MinLength + "");
                                    minFieldError = minFieldError.replace("%Maxlength", objectfield.MaxLength + "");
                                    minFieldError = minFieldError.replace("%FieldName", fieldName);
                                    errorMsg = minFieldError;
                                }

                                errorsArray.push(errorMsg);
                            }
                        }
                    }
                }
            }

        }
        return errorsArray;
    }

    public IsValid(entityPM) {
        var isValid: boolean;

        return isValid;
    }
}