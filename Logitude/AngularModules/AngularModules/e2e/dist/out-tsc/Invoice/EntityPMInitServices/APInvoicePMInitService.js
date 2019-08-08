"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var FeatureLocator_1 = require("../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var Tools_2 = require("../Tools");
var APInvoicePMInitService = /** @class */ (function () {
    function APInvoicePMInitService() {
    }
    APInvoicePMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            var todayDateTime = Tools_1.DateTool.GetCurrentDateAsUtc();
            entityPM.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
            entityPM.StatusCode = "WA";
            entityPM.StatusName = "Waiting for Approval";
            entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.CreateDate = todayDateTime;
            entityPM.UpdateDate = todayDateTime;
            entityPM.BranchId = SessionLocator_1.SessionLocator.LoggedUserPM.BranchId;
            entityPM.LocalCurrencyId = SessionLocator_1.SessionLocator.LocalCurrencyId;
            entityPM.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
            entityPM.PaymentTermId = SessionLocator_1.SessionLocator.TenantPM.PaymentTermId;
            entityPM.SubTotalInLocalCurrency = 0;
            entityPM.SubTotalInInvoiceCurrency = 0;
        }
    };
    APInvoicePMInitService.ApplyUIPoperties = function (entityPM, isNew) {
        //entityPM.UIProperties = new UIProperties;
        entityPM.UIProperties.SetVisibility("EnableConsolidationInvoices", "APInvoice", FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "Consolidation.Constituent"));
        entityPM.UIProperties.SetEnabled("UpdateDate", "APInvoice", false);
        entityPM.UIProperties.SetEnabled("UpdatedByUserId", "APInvoice", false);
        var isAllowedEdit = Tools_2.InvoiceTool.IsEditingAPInvoiceEnabled(entityPM);
        entityPM.UIProperties.SetEnabled("HouseNumber", "APInvoice", isAllowedEdit);
        entityPM.UIProperties.SetEnabled("MasterNumber", "APInvoice", isAllowedEdit);
        entityPM.UIProperties.SetEnabled("BranchId", "APInvoice", isAllowedEdit);
        if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated) {
            entityPM.UIProperties.SetEnabled("AccountingDate", "APInvoice", isAllowedEdit);
        }
        if (entityPM.IsMultipleEntities) {
            entityPM.UIProperties.SetVisibility("HouseNumber", "APInvoice", false);
            entityPM.UIProperties.SetVisibility("MasterNumber", "APInvoice", false);
        }
    };
    return APInvoicePMInitService;
}());
exports.APInvoicePMInitService = APInvoicePMInitService;
//# sourceMappingURL=APInvoicePMInitService.js.map