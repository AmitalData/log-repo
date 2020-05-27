"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var ARInvoiceStockPMInitService = /** @class */ (function () {
    function ARInvoiceStockPMInitService() {
    }
    ARInvoiceStockPMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            var todayDateTime = Tools_1.DateTool.GetCurrentDateAsUtc();
            entityPM.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
            entityPM.StatusCode = "N";
            entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.CreateDate = todayDateTime;
            entityPM.UpdateDate = todayDateTime;
        }
    };
    ARInvoiceStockPMInitService.ApplyUIPoperties = function (entityPM, isNew) {
    };
    return ARInvoiceStockPMInitService;
}());
exports.ARInvoiceStockPMInitService = ARInvoiceStockPMInitService;
//# sourceMappingURL=ARInvoiceStockPMInitService.js.map