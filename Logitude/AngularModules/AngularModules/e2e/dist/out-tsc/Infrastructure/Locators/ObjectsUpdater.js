"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ObjectsLocator_1 = require("./ObjectsLocator");
var SessionLocator_1 = require("../Utilities/SessionLocator");
var Tools_1 = require("../Tools");
var TenantManagementJS_1 = require("../DataContracts/TenantManagementJS");
var AccountingSettingPM_1 = require("../../Common/EntityPMs/AccountingSettingPM");
var CustomsInterfaceSettingPM_1 = require("../../Common/EntityPMs/CustomsInterfaceSettingPM");
var SharedLogisticsSettingPM_1 = require("../EntityPMs/SharedLogisticsSettingPM");
var ObjectsUpdater = /** @class */ (function () {
    function ObjectsUpdater() {
    }
    ObjectsUpdater.UpdateTenantPM = function (value) {
        this.TenantPM = value;
        Tools_1.AppTool.TenantPM = value;
        Tools_1.DateTool.TenantPM = value;
        ObjectsLocator_1.ObjectsLocator.TenantPM = value;
        SessionLocator_1.SessionLocator.TenantPM = value;
        if (value) {
            SessionLocator_1.SessionLocator.Tenant = value.Id;
            SessionLocator_1.SessionLocator.LocalCurrencyId = value.CurrencyId;
            SessionLocator_1.SessionLocator.LocalCurrencyCode = value.CurrencyCode;
            SessionLocator_1.SessionLocator.AccountingCurrencyId = value.CurrencyId;
        }
    };
    ObjectsUpdater.UpdateLoggedUserPM = function (value) {
        ObjectsLocator_1.ObjectsLocator.LoggedUserPM = value;
    };
    ObjectsUpdater.UpdateTenantManagementJS = function (value) {
        if (!value) {
            value = new TenantManagementJS_1.TenantManagementJS();
        }
        ObjectsLocator_1.ObjectsLocator.TenantManagementJS = value;
        SessionLocator_1.SessionLocator.TenantManagementJS = value;
    };
    ObjectsUpdater.UpdateAccountingSettingPM = function (value) {
        if (!value) {
            value = new AccountingSettingPM_1.AccountingSettingPM();
        }
        ObjectsLocator_1.ObjectsLocator.AccountingSettingPM = value;
        SessionLocator_1.SessionLocator.AccountingSettingPM = value;
    };
    ObjectsUpdater.UpdateCustomsInterfaceSettingPM = function (value) {
        if (!value) {
            value = new CustomsInterfaceSettingPM_1.CustomsInterfaceSettingPM();
        }
        ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM = value;
    };
    ObjectsUpdater.UpdateSharedLogisticsSettingPM = function (value) {
        if (!value) {
            value = new SharedLogisticsSettingPM_1.SharedLogisticsSettingPM();
        }
        ObjectsLocator_1.ObjectsLocator.SharedLogisticsSettingPM = value;
    };
    return ObjectsUpdater;
}());
exports.ObjectsUpdater = ObjectsUpdater;
//# sourceMappingURL=ObjectsUpdater.js.map