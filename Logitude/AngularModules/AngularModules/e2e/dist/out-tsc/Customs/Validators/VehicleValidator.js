"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var VehicleValidator = /** @class */ (function () {
    function VehicleValidator() {
        this.ValidationErrorMessageCodes = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }
    VehicleValidator.prototype.SetEntityPM = function (VehiclePM) {
        this._VehiclePM = VehiclePM;
    };
    VehicleValidator.prototype.SubmitDateTimeCheck = function () {
        var errorMessage = "";
        if (this._VehiclePM != null) {
            if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                this.ValidationErrorMessageCodes.push(errorMessage);
            }
        }
    };
    VehicleValidator.prototype.Validate = function (entityPM) {
        var result = [];
        this._VehiclePM = entityPM;
        this.MandatoryFieldsCheck();
        return this.ValidationErrorMessageCodes;
    };
    //Check if there is an empty Importer Declarations
    VehicleValidator.prototype.MandatoryFieldsCheck = function () {
        var errorMessage = "";
        if (this._VehiclePM != null) {
            if (Tools_1.AppTool.IsNullOrEmpty(this._VehiclePM.ImporterIdentityId) && (Tools_1.AppTool.IsNullOrEmpty(this._VehiclePM.ImporterPassportNumber) || Tools_1.AppTool.IsNullOrEmpty(this._VehiclePM.ImporterPassCountryCode) || Tools_1.AppTool.IsNullOrEmpty(this._VehiclePM.ImporterPassportTypeCode) || Tools_1.AppTool.IsNullOrEmpty(this._VehiclePM.PassportName))) {
                errorMessage = "חובה להזין מס' יבואן או נתוני דרכון(מס' דרכון; מדינת הדרכון; סוג תעודת מסע ושם דרכון) לרכב";
                if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                    this.ValidationErrorMessageCodes.push(errorMessage);
                }
            }
            var newValue = this._VehiclePM.VehiclePowerKW;
            if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                var strValue = newValue.toString();
                if (strValue.indexOf(".") > -1)
                    strValue = newValue.toString().substring(0, newValue.toString().indexOf("."));
                if (!Tools_1.AppTool.IsNullOrEmpty(strValue)) {
                    if (strValue.length > 5) {
                        errorMessage = "הספק מנוע לא יכול להיות ארוך מחמישה תווים";
                        if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                            this.ValidationErrorMessageCodes.push(errorMessage);
                        }
                    }
                }
            }
            var newValue = this._VehiclePM.VehicleMaxPowerKW;
            if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                var strValue = newValue.toString();
                if (strValue.indexOf(".") > -1)
                    strValue = newValue.toString().substring(0, newValue.toString().indexOf("."));
                if (!Tools_1.AppTool.IsNullOrEmpty(strValue)) {
                    if (strValue.length > 5) {
                        errorMessage = "הספק מנוע מירבי לא יכול להיות ארוך מחמישה תווים";
                        if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                            this.ValidationErrorMessageCodes.push(errorMessage);
                        }
                    }
                }
            }
            if (this._VehiclePM.VehicleOwners != null && this._VehiclePM.VehicleOwners.length > 0) {
                for (var _i = 0, _a = this._VehiclePM.VehicleOwners; _i < _a.length; _i++) {
                    var item = _a[_i];
                    if (Tools_1.AppTool.IsNullOrEmpty(item.ClientId) && (Tools_1.AppTool.IsNullOrEmpty(item.PassportNumber) || Tools_1.AppTool.IsNullOrEmpty(item.PassCountryCode) || Tools_1.AppTool.IsNullOrEmpty(item.ImporterPassportTypeCode))) {
                        errorMessage = "חובה להזין זיהוי בעל רכב או נתוני דרכון(מס' דרכון, מדינת דרכון יבואן וסוג תעודת מסע) לבעל רכב";
                        if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                            this.ValidationErrorMessageCodes.push(errorMessage);
                        }
                    }
                }
            }
        }
    };
    VehicleValidator.prototype.GetRequierdFieldErrorText = function (fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(fieldName));
    };
    return VehicleValidator;
}());
exports.VehicleValidator = VehicleValidator;
//# sourceMappingURL=VehicleValidator.js.map