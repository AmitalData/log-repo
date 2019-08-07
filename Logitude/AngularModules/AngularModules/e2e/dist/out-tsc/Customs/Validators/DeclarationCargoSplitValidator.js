"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var DeclarationCargoSplitValidator = /** @class */ (function () {
    function DeclarationCargoSplitValidator() {
        this.ValidationErrorMessageCodes = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }
    DeclarationCargoSplitValidator.prototype.SetEntityPM = function (DeclarationCargoSplitPM) {
        this._DeclarationCargoSplitPM = DeclarationCargoSplitPM;
    };
    DeclarationCargoSplitValidator.prototype.SubmitDateTimeCheck = function () {
        var errorMessage = "";
        if (this._DeclarationCargoSplitPM != null) {
            if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                this.ValidationErrorMessageCodes.push(errorMessage);
            }
        }
    };
    DeclarationCargoSplitValidator.prototype.Validate = function (entityPM) {
        var result = [];
        this._DeclarationCargoSplitPM = entityPM;
        return this.OriginalValidationErrorMessageCodes;
    };
    DeclarationCargoSplitValidator.prototype.GetRequierdFieldErrorText = function (fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(fieldName));
    };
    return DeclarationCargoSplitValidator;
}());
exports.DeclarationCargoSplitValidator = DeclarationCargoSplitValidator;
//# sourceMappingURL=DeclarationCargoSplitValidator.js.map