declare var window: any;
declare var System: any;
import {TextCodeTranslator} from '../Utilities/TextCodeTranslator';
import {AppTool} from '../Tools';
import {ObjectFieldPM} from '../EntityPMs/ObjectFieldPM';
import {CustomFieldClass} from '../DataContracts/CustomFieldClass';
import { SessionLocator } from '../Utilities/SessionLocator';

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
                let isNotValid = false;
                if (objectfield.IsCustom) {
                    var customfieldClass: CustomFieldClass = entityPM[objectfield.FieldName];
                    value = customfieldClass.Value;
                    isNotValid = customfieldClass.IsNotValid;
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
                        case "Date":
                        case "PickList":
                        case "Time":
                            {
                                if (objectfield.IsCustom && AppTool.IsNullOrEmpty(value)) {
                                    var fieldName: string = TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
                                    var fieldError: string = translatedRequiredError.replace("%FieldName", fieldName);
                                    errorsArray.push(fieldError);
                                }
                                break;
                            }
                    }
                }
                if (value && !this.IsValidValue(objectfield, value, isNotValid)) {
                    errorsArray.push(this.GetValidationErrorMessage(objectfield, value));
                }
            }

        }
        return errorsArray;
    }

    public GetValidationErrorMessage(objectfield: ObjectFieldPM, value: any) {
        if (objectfield.DataTypeCode == "Text" || objectfield.DataTypeCode == "nText") {
            return this.GetMinMaxErrorMessage(objectfield, value);
        }

        if (objectfield.DataTypeCode == "Decimal") {
            return this.GetNumberValidationErrorMessage(objectfield, value)
        }

        return "Error!";
    }

    private GetNumberValidationErrorMessage(objectfield: ObjectFieldPM, value: any) {

        let { numberBeforePoint, numberAfterPoint }: { numberBeforePoint: string; numberAfterPoint: string; } = this.GetNumbersBeforeAndAfterPoint(value);
        if (!this.IsValidNumberOfDigitsValue(objectfield, numberBeforePoint) && !this.IsValidDecimalDigitsValue(objectfield, numberAfterPoint)) {
            return this.GetNotValidNumberOfDigitsAndDecimalDigitsInNumberCustomFieldErrorMessage(objectfield);
        }
        if (!this.IsValidNumberOfDigitsValue(objectfield, numberBeforePoint)) {
            return this.GetNotValidNumberOfDigitsInNumberCustomFieldErrorMessage(objectfield);
        }
        if (!this.IsValidDecimalDigitsValue(objectfield, numberAfterPoint)) {
            return this.GetNotValidDecimalDigitsInNumberCustomFieldErrorMessage(objectfield);
        }

        return "error from custom decimal";
    }

    private GetNotValidDecimalDigitsInNumberCustomFieldErrorMessage(objectfield: ObjectFieldPM) {
        return "Length of Decimal digits must be " + (objectfield.DigitsAfterPoint == 0 ? "0" : ("less than or equal " + objectfield.DigitsAfterPoint));
    }

    private GetNotValidNumberOfDigitsInNumberCustomFieldErrorMessage(objectfield: ObjectFieldPM) {
        return "Length of the Number must be " + (objectfield.NumberOfDigits != 1 ? "between 1 and " : "") + objectfield.NumberOfDigits;
    }

    private GetNotValidNumberOfDigitsAndDecimalDigitsInNumberCustomFieldErrorMessage(objectfield: ObjectFieldPM) {
        return "Length of the Number must be " + (objectfield.NumberOfDigits != 1 ? "between 1 and " : "") + objectfield.NumberOfDigits + ", Length of Decimal digits must be " + (objectfield.DigitsAfterPoint == 0 ? "0" : ("less than or equal " + objectfield.DigitsAfterPoint));
    }

    private GetNumbersBeforeAndAfterPoint(value: any) {
        let formatedValue = this.GetFormatedValue(value);
        let indexOfPoint = formatedValue.indexOf(".");
        let numberBeforePoint: string = "";
        let numberAfterPoint: string = "";
        switch (indexOfPoint) {
            case -1: {
                numberBeforePoint = formatedValue;
                numberAfterPoint = "";
                break;
            }
            case 0: {
                numberBeforePoint = "";
                numberAfterPoint = formatedValue.substring(1, formatedValue.length);
                break;
            }
            default: {
                numberBeforePoint = formatedValue.substring(0, indexOfPoint);
                numberAfterPoint = formatedValue.substring(indexOfPoint + 1, formatedValue.length);
                break;
            }
        }
        return { numberBeforePoint, numberAfterPoint };
    }

    private GetFormatedValue(value: any) {
        let decimalSeparator: string = ".";
        if (SessionLocator.TenantPM.NumberFormatCode == "DC") {
            decimalSeparator = ",";
        }
        let formatedvalue: string;
        if (decimalSeparator == ",") {
            formatedvalue = value.replace("+", "").replace("-", "").split(".").join("").split("'").join("").replace(",", ".");
        }
        else { // "."
            formatedvalue = value.replace("+", "").replace("-", "").split(",").join("").split("'").join("");
        }
        return formatedvalue;
    }

    private GetMinMaxErrorMessage(objectfield: ObjectFieldPM, value: any) {
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

    public IsValidValue(objectfield: ObjectFieldPM, value: any, isNotValid: any) {
        if (objectfield.DataTypeCode == "Text" || objectfield.DataTypeCode == "nText") {
            return this.IsValidTextValue(objectfield, value);
        }

        if (isNotValid && objectfield.DataTypeCode == "Decimal") {
            return false;
        }

        return true;
    }

    private IsValidTextValue(objectfield: ObjectFieldPM, value: any) {
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

    public IsValidCustomNumberValue(objectfield: ObjectFieldPM, value: any) {
        if (!objectfield.IsCustom) return true;
        if (objectfield.DataTypeCode != "Decimal") return true;
        let { numberBeforePoint, numberAfterPoint }: { numberBeforePoint: string; numberAfterPoint: string; } = this.GetNumbersBeforeAndAfterPoint(value);
        return this.IsValidNumberOfDigitsValue(objectfield, numberBeforePoint) && this.IsValidDecimalDigitsValue(objectfield, numberAfterPoint);
    }

    private IsValidNumberOfDigitsValue(objectfield: ObjectFieldPM, value: any) {
        return (objectfield.NumberOfDigits == 0 && value.length <= 12) || (value.length <= objectfield.NumberOfDigits && value.length > 0);
    }

    private IsValidDecimalDigitsValue(objectfield: ObjectFieldPM, value: any) {
        return value.length <= objectfield.DigitsAfterPoint;
    }

    public IsValid(entityPM) {
        var isValid: boolean;

        return isValid;
    }

}
