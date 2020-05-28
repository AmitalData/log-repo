"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CreditLimitSettingPM_1 = require("../../Common/EntityPMs/CreditLimitSettingPM");
var CustomsInterfaceSettingPM_1 = require("../../Common/EntityPMs/CustomsInterfaceSettingPM");
var SharedLogisticsSettingPM_1 = require("../EntityPMs/SharedLogisticsSettingPM");
var Settings_1 = require("../Settings");
var ObjectsLocator = /** @class */ (function () {
    function ObjectsLocator() {
    }
    Object.defineProperty(ObjectsLocator, "CreditLimitSettingPM", {
        get: function () {
            if (this.creditLimitSettingPM == null) {
                this.creditLimitSettingPM = new CreditLimitSettingPM_1.CreditLimitSettingPM();
            }
            return this.creditLimitSettingPM;
        },
        set: function (value) {
            if (value) {
                this.creditLimitSettingPM = value;
            }
            else {
                this.creditLimitSettingPM = new CreditLimitSettingPM_1.CreditLimitSettingPM();
            }
        },
        enumerable: true,
        configurable: true
    });
    ObjectsLocator.UpdateTenantPM = function (value) {
        this.TenantPM = value;
    };
    ObjectsLocator.UpdateLoggedUserPM = function (value) {
        this.LoggedUserPM = value;
    };
    ObjectsLocator.UpdateCreditLimitSettingPM = function (value) {
        this.CreditLimitSettingPM = value;
    };
    ObjectsLocator.UpdateGlobalSetting = function (value) {
        this.GlobalSetting = value;
        if (value) {
            Settings_1.Settings.LayoutDirection = value.LayoutDirection;
        }
    };
    ObjectsLocator.UpdatePrivateLableSettings = function (value) {
        this.PrivateLableSettings = value;
    };
    ObjectsLocator.CustomsInterfaceSettingPM = new CustomsInterfaceSettingPM_1.CustomsInterfaceSettingPM();
    ObjectsLocator.SharedLogisticsSettingPM = new SharedLogisticsSettingPM_1.SharedLogisticsSettingPM();
    return ObjectsLocator;
}());
exports.ObjectsLocator = ObjectsLocator;
//# sourceMappingURL=ObjectsLocator.js.map