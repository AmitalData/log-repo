"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("./SessionLocator");
var ObjectsLocator_1 = require("../Locators/ObjectsLocator");
var ObjectsUpdater_1 = require("../Locators/ObjectsUpdater");
var InfraSettings = /** @class */ (function () {
    function InfraSettings() {
    }
    Object.defineProperty(InfraSettings, "TenantPM", {
        get: function () { return this.tenantPM; },
        set: function (newValue) {
            if (this.tenantPM != newValue) {
                this.tenantPM = newValue;
                SessionLocator_1.SessionLocator.TenantPM = newValue;
                ObjectsLocator_1.ObjectsLocator.UpdateTenantPM(newValue);
                ObjectsUpdater_1.ObjectsUpdater.UpdateTenantPM(newValue);
                if (newValue) {
                    SessionLocator_1.SessionLocator.Tenant = newValue.Id;
                    SessionLocator_1.SessionLocator.LocalCurrencyId = newValue.CurrencyId;
                    SessionLocator_1.SessionLocator.LocalCurrencyCode = newValue.CurrencyCode;
                    SessionLocator_1.SessionLocator.AccountingCurrencyId = newValue.CurrencyId;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    return InfraSettings;
}());
exports.InfraSettings = InfraSettings;
//# sourceMappingURL=InfraSettings.js.map