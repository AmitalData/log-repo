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

                if (value && !this.IsValidTextValue(objectfield, value)) {
                    errorsArray.push(this.GetMinMaxErrorMessage(objectfield, value));
                }
            }

        }
        return errorsArray;
    }

    public GetMinMaxErrorMessage(objectfield: ObjectFieldPM, value: any) {
        if (objectfield.MaxLength == objectfield.MinLength) {
            return this.GetEqualLengthErrorMessage(objectfield);
        }

        if (!this.IsValidTextMaxValue(objectfield, value) && !this.IsValidTextMinValue(objectfield, value)) {
            return this.GetMinMaxLengthErrorMessage(objectfield);
        }

        if (!this.IsValidTextMaxValue(objectfield, value)) {
            return this.GetMaxLengthErrorMessage(objectfield);
        }

        return this.GetMinLengthErrorMessage(objectfield);
    }

    private GetEqualLengthErrorMessage(objectfield: ObjectFieldPM) {
        let translatedEqualError: string = TextCodeTranslator.Translate("General.M.EqualLength");
        let fieldName: string = TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
        let equalFieldError: string = translatedEqualError.replace("%Equallength", objectfield.MinLength + "");
        return equalFieldError.replace("%FieldName", fieldName);
    }

    private GetMinMaxLengthErrorMessage(objectfield: ObjectFieldPM) {
        let translatedMinMaxError: string = TextCodeTranslator.Translate("General.M.MinMax");
        let fieldName: string = TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
        let minFieldError: string = translatedMinMaxError.replace("%Minlength", objectfield.MinLength + "");
        minFieldError = minFieldError.replace("%Maxlength", objectfield.MaxLength + "");
        return minFieldError.replace("%FieldName", fieldName);
    }

    private GetMaxLengthErrorMessage(objectfield: ObjectFieldPM) {
        let translatedMaxError: string = TextCodeTranslator.Translate("General.M.Max");
        let fieldName: string = TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
        let error: string = translatedMaxError.replace("%Maxlength", objectfield.MaxLength + "");
        return error.replace("%FieldName", fieldName);
    }

    private GetMinLengthErrorMessage(objectfield: ObjectFieldPM) {
        let translatedMinError: string = TextCodeTranslator.Translate("General.M.Min");
        let fieldName: string = TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
        let error: string = translatedMinError.replace("%Minlength", objectfield.MinLength + "");

        return error.replace("%FieldName", fieldName);
    }

    public IsValidTextValue(objectfield: ObjectFieldPM, value: any) {
        if (objectfield.DataTypeCode !== "Text" && objectfield.DataTypeCode !== "nText") return true;
        if (objectfield.MaxLength === 0 && objectfield.MinLength === 0) return true;
        if (objectfield.IsMaxLength) return true;
        if (objectfield.MaxLength == 0) return this.IsValidTextMinValue(objectfield, value);

        return this.IsValidTextMinValue(objectfield, value) && this.IsValidTextMaxValue(objectfield, value);
    }

    private IsValidTextMinValue(objectfield: ObjectFieldPM, value: any) {
        return value.toString().length >= objectfield.MinLength;
    }

    private IsValidTextMaxValue(objectfield: ObjectFieldPM, value: any) {
        return objectfield.MaxLength == 0 || value.toString().length <= objectfield.MaxLength;
    }

    public IsValid(entityPM) {
        var isValid: boolean;

        return isValid;
    }
}
