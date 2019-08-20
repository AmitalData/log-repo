"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TotangoService_1 = require("../Services/WebServices/TotangoService");
var RulesValidator_1 = require("../Validators/RulesValidator");
var ServiceLocator = /** @class */ (function () {
    function ServiceLocator() {
    }
    ServiceLocator.SendTotangoUserActivity = function (module, activity) {
        if (this.TotangoSenderService == null) {
            this.TotangoSenderService = new TotangoService_1.TotangoService();
        }
        this.TotangoSenderService.SendTotangoUserActivity(module, activity);
    };
    Object.defineProperty(ServiceLocator, "RulesValidator", {
        get: function () {
            if (!this.rulesValidator) {
                this.rulesValidator = new RulesValidator_1.RulesValidator();
            }
            return this.rulesValidator;
        },
        set: function (newValue) { this.rulesValidator = newValue; },
        enumerable: true,
        configurable: true
    });
    return ServiceLocator;
}());
exports.ServiceLocator = ServiceLocator;
//# sourceMappingURL=ServiceLocator.js.map