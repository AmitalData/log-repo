"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var ClaimValidator = /** @class */ (function () {
    function ClaimValidator() {
        this.ValidationErrorMessageCodes = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }
    ClaimValidator.prototype.SetEntityPM = function (claimPM) {
        this._ClaimPM = claimPM;
    };
    //Check if there is an empty Importer Declarations
    ClaimValidator.prototype.ClaimImporterDeclarsPage3Check = function () {
        var errorMessage = "";
        if (this._ClaimPM != null) {
            for (var _i = 0, _a = this._ClaimPM.ClaimImporterDeclarsPage3; _i < _a.length; _i++) {
                var item = _a[_i];
                if (Tools_1.AppTool.IsNullOrEmpty(item.ImporterLoiDeclarationTypeCode) || item.ClaimImporterDeclarsP3Loi == null
                    || (item.ClaimImporterDeclarsP3Loi != null && item.ClaimImporterDeclarsP3Loi.length == 0)
                    || (item.ClaimImporterDeclarsP3Loi != null && item.ClaimImporterDeclarsP3Loi.length > 0 && Tools_1.AppTool.IsNullOrEmpty(item.ClaimImporterDeclarsP3Loi[0].DeclarationNumber))) {
                    errorMessage = "לא ניתן להוסיף הצהרת יבואן בלי תצהיר ו/או הצהרה";
                    if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                        this.ValidationErrorMessageCodes.push(errorMessage);
                    }
                }
            }
        }
    };
    //Check if there is an empty Importer Declarations Page 3A
    ClaimValidator.prototype.ClaimImporterDeclarsPage3ACheck = function () {
        var errorMessage = "";
        if (this._ClaimPM != null) {
            if (this._ClaimPM.ClaimImporterDeclarsPage3A != null && this._ClaimPM.ClaimImporterDeclarsPage3A.length > 0) {
                for (var _i = 0, _a = this._ClaimPM.ClaimImporterDeclarsPage3A; _i < _a.length; _i++) {
                    var item = _a[_i];
                    if (Tools_1.AppTool.IsNullOrEmpty(item.CommercialSaleTypeCode)) {
                        errorMessage = "לא ניתן להוסיף פרטי מישור מסחרי ריק";
                        if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                            this.ValidationErrorMessageCodes.push(errorMessage);
                        }
                    }
                }
            }
        }
    };
    //Check if there is an empty Importer Declarations Page 3B
    ClaimValidator.prototype.ClaimImporterDeclarsPage3BCheck = function () {
        var errorMessage = "";
        if (this._ClaimPM != null) {
            if (this._ClaimPM.ClaimImporterDeclarsPage3B != null && this._ClaimPM.ClaimImporterDeclarsPage3B.length > 0) {
                for (var _i = 0, _a = this._ClaimPM.ClaimImporterDeclarsPage3B; _i < _a.length; _i++) {
                    var item = _a[_i];
                    if (Tools_1.AppTool.IsNullOrEmpty(item.DescriptionOfGoods)) {
                        errorMessage = "לא ניתן להוסיף הצהרת מכירה ריקה";
                        if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                            this.ValidationErrorMessageCodes.push(errorMessage);
                        }
                    }
                }
            }
        }
    };
    ClaimValidator.prototype.SubmitDateTimeCheck = function () {
        var errorMessage = "";
        if (this._ClaimPM != null) {
            if (this._ClaimPM.SubmitDate == null) {
                errorMessage = errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
            }
            else {
                //if ((this._ClaimPM.SubmitDate.getFullYear != Date.) ||
                //    (this._ClaimPM.SubmitDate.Value.Date.Month != DateTime.Now.Date.Month) ||
                //    (this._ClaimPM.SubmitDate.Value.Date.Day != DateTime.Now.Date.Day)) {
                //    errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
                //}
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                this.ValidationErrorMessageCodes.push(errorMessage);
            }
        }
    };
    ClaimValidator.prototype.Validate = function (entityPM) {
        var result = [];
        this._ClaimPM = entityPM;
        this.ClaimImporterDeclarsPage3Check(); //Page3
        this.ClaimImporterDeclarsPage3ACheck(); //Page3-A
        this.ClaimImporterDeclarsPage3BCheck(); //Page3-B
        return this.ValidationErrorMessageCodes;
    };
    ClaimValidator.prototype.GetRequierdFieldErrorText = function (fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(fieldName));
    };
    return ClaimValidator;
}());
exports.ClaimValidator = ClaimValidator;
//# sourceMappingURL=ClaimValidator.js.map