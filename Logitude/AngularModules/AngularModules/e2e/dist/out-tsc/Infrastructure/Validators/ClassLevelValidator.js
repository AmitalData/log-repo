"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TextCodeTranslator_1 = require("../Utilities/TextCodeTranslator");
var Tools_1 = require("../Tools");
var RulesValidator_1 = require("./RulesValidator");
var ClassLevelValidator = /** @class */ (function () {
    function ClassLevelValidator() {
        this.ErrorsArray = [];
        if (this._RulesValidator == null) {
            this._RulesValidator = new RulesValidator_1.RulesValidator();
        }
    }
    ClassLevelValidator.prototype.Validate = function (objectTableName, entityPM) {
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
        }
        var objectFields = window.ObjectFields.filter(function (x) { return x.ObjectTableId === objectTable.Id; });
        if (objectTableName == "Shipment" && entityPM.ShipmentLevelCode == "C") {
            var masterTable = window.ObjectTables.filter(function (x) { return x.Name === "Master"; })[0];
            objectFields = objectFields.filter(function (x) { return x.IsCustom === false || (x.IsCustom === true && x.ObjectTableId === masterTable.Id); });
        }
        var isNewEntity = (entityPM.OldEntityPM === null || entityPM.OldEntityPM === undefined);
        if (this._RulesValidator.IsNewEntity != isNewEntity) {
            this._RulesValidator.IsNewEntity = isNewEntity;
            this._RulesValidator.Initizialize();
        }
        this._RulesValidator.ValidateAllTableRules(entityPM, objectTable.Id, errorsArray);
        objectFields.forEach(function (objectfield, key) {
            if (objectfield.ObjectTableId === objectTableId) {
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
                                if (Tools_1.AppTool.IsNullOrEmpty(entityPM[objectfield.FieldName])) {
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
                                if (Tools_1.AppTool.IsNullOrEmpty(entityPM[objectfield.FieldName])) {
                                    var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
                                    var fieldError = translatedRequiredError.replace("%FieldName", fieldName);
                                    errorsArray.push(fieldError);
                                }
                                break;
                            }
                    }
                }
                if (entityPM[objectfield.FieldName]) {
                    if (objectfield.DataTypeCode === "Text" || objectfield.DataTypeCode === "nText") {
                        if (objectfield.MaxLength !== 0 || objectfield.MinLength !== 0) {
                            if (entityPM[objectfield.FieldName]) {
                                var fieldvalue = "";
                                if (objectfield.IsCustom === true) {
                                    fieldvalue = entityPM[objectfield.FieldName].Value;
                                }
                                else
                                    fieldvalue = entityPM[objectfield.FieldName];
                                if (fieldvalue == null || fieldvalue == undefined) {
                                    fieldvalue = "";
                                }
                                if (!objectfield.IsMaxLength &&
                                    (fieldvalue.length > objectfield.MaxLength || fieldvalue.length < objectfield.MinLength)) {
                                    var errorMsg = "";
                                    if (objectfield.MinLength == 0) {
                                        // display only max length error
                                        var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
                                        var error = translatedMaxError.replace("%Maxlength", objectfield.MaxLength);
                                        error = error.replace("%FieldName", fieldName);
                                        errorMsg = error;
                                    }
                                    else {
                                        //display min max errors
                                        var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(objectfield.FullNameTextCodeCode);
                                        var minFieldError = translatedMinMaxError.replace("%Minlength", objectfield.MinLength);
                                        minFieldError = minFieldError.replace("%Maxlength", objectfield.MaxLength);
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
        });
        return errorsArray;
    };
    ClassLevelValidator.prototype.IsValid = function (entityPM) {
        var isValid;
        return isValid;
    };
    return ClassLevelValidator;
}());
exports.ClassLevelValidator = ClassLevelValidator;
//# sourceMappingURL=ClassLevelValidator.js.map