"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../Infrastructure/Tools");
var Validator_1 = require("../../Infrastructure/Validators/Validator");
var TicketValidator = /** @class */ (function () {
    function TicketValidator() {
        this.Errors = [];
        this.Errors = [];
        this.message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }
    TicketValidator.prototype.ValidateCurrenctEntity = function (entityPM) {
        this.Errors = [];
        this.EntityPM = entityPM;
        Validator_1.Validator.TryValidateObject(entityPM, "Ticket", this.Errors);
        if (Tools_1.AppTool.IsNullOrEmpty(entityPM.OwnerId)) {
            this.Errors.push("Owner field is required");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(entityPM.CompanyId)) {
            this.Errors.push("Company field is required");
        }
        return this.Errors;
    };
    return TicketValidator;
}());
exports.TicketValidator = TicketValidator;
//# sourceMappingURL=TicketValidator.js.map