"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TextCodeTranslator_1 = require("../Utilities/TextCodeTranslator");
var Tools_1 = require("../Tools");
var FieldValidator = /** @class */ (function () {
    function FieldValidator() {
        this.ErrorsArray = [];
    }
    FieldValidator.prototype.Validate = function (objectFieldName, objectTableName, entityPM) {
        var errorsArray = [];
        var objectTableId;
        var requiredErrorCode = "General.M.FieldIsRequired";
        var minmaxErrorCode = "General.M.MinMax";
        var translatedRequiredError = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var translatedMinMaxError = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.MinMax");
        var translatedMaxError = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Max");
        var objectTable = window.ObjectTables.filter(function (x) { return x.Name === objectTableName; })[0];
        if (objectTable) {
            objectTableId = objectTable.Id;
            var objectfield = window.ObjectFields.filter(function (x) { return x.FieldName === objectFieldName && x.ObjectTableId === objectTableId; })[0];
            if (objectfield) {
                var value = entityPM[objectfield.FieldName];
                if (objectfield.IsCustom) {
                    var customfieldClass = entityPM[objectfield.FieldName];
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
                                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                                    var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
                                    var fieldError = translatedRequiredError.replace("%FieldName", fieldName);
                                    errorsArray.push(fieldError);
                                }
                                break;
                            }
                        case "Text":
                        case "nText":
                        case "LookUp":
                        case "DateTime":
                            {
                                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                                    var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
                                    var fieldError = translatedRequiredError.replace("%FieldName", fieldName);
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
                                    var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
                                    var error = translatedMaxError.replace("%Maxlength", objectfield.MaxLength + "");
                                    error = error.replace("%FieldName", fieldName);
                                    errorMsg = error;
                                }
                                else {
                                    //display min max errors
                                    var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
                                    var minFieldError = translatedMinMaxError.replace("%Minlength", objectfield.MinLength + "");
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
    };
    FieldValidator.prototype.IsValid = function (entityPM) {
        var isValid;
        return isValid;
    };
    return FieldValidator;
}());
exports.FieldValidator = FieldValidator;
//# sourceMappingURL=FieldValidator.js.map