"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Validator_1 = require("../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../Infrastructure/Tools");
var VatTypeValidator = /** @class */ (function () {
    function VatTypeValidator() {
        this.Errors = [];
        this.Errors = [];
        this.message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }
    VatTypeValidator.prototype.Validate = function (entityPM) {
        this.Errors = [];
        this.EntityPM = entityPM;
        Validator_1.Validator.TryValidateObject(this.EntityPM, "VatType", this.Errors);
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            if (!this.EntityPM.IsMultiPercentage) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.NewEntityPercentage)) {
                    var field = TextCodeTranslator_1.TextCodeTranslator.Translate("VatType.F.NewEntityPercentage");
                    this.Errors.push(this.message.replace("%FieldName", field));
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.NewEntityPercentageDate)) {
                    var field = TextCodeTranslator_1.TextCodeTranslator.Translate("VatType.F.NewEntityPercentageDate");
                    this.Errors.push(this.message.replace("%FieldName", field));
                }
            }
        }
        return this.Errors;
    };
    return VatTypeValidator;
}());
exports.VatTypeValidator = VatTypeValidator;
//# sourceMappingURL=VatTypeValidator.js.map