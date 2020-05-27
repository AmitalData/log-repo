"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var FeatureLocator_1 = require("../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var Tools_2 = require("../Tools");
var Tools_3 = require("../../Infrastructure/Tools");
var ARInvoicePMInitService = /** @class */ (function () {
    function ARInvoicePMInitService() {
    }
    ARInvoicePMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            entityPM.StatusCode = "DR";
            entityPM.StatusName = "Draft";
            entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            entityPM.IssuedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.CreateDate = todayDate;
            entityPM.UpdateDate = todayDate;
            entityPM.InvoiceDate = todayDate;
            entityPM.BranchId = SessionLocator_1.SessionLocator.LoggedUserPM.BranchId;
            entityPM.LocalCurrencyId = SessionLocator_1.SessionLocator.LocalCurrencyId;
            entityPM.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
            entityPM.ProfitCurrencyId = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyId;
            entityPM.ProfitCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyCode;
            entityPM.SATTransferStatusCode = "NT";
            entityPM.SATTransferStatusName = "Not Transfered";
            entityPM.NewConcurrencyGUID = Tools_3.AppTool.GetNewGuid();
        }
    };
    ARInvoicePMInitService.ApplyUIPoperties = function (entityPM, isNew) {
        entityPM.UIProperties.SetEnabled("UpdateDate", "ARInvoice", false);
        entityPM.UIProperties.SetEnabled("UpdatedByUserId", "ARInvoice", false);
        var isAllowedEdit = Tools_2.InvoiceTool.IsEditingARInvoiceEnabled(entityPM);
        entityPM.UIProperties.SetEnabled("Sent", "ARInvoice", isAllowedEdit);
        entityPM.UIProperties.SetEnabled("HouseNumber", "ARInvoice", isAllowedEdit);
        entityPM.UIProperties.SetEnabled("MasterNumber", "ARInvoice", isAllowedEdit);
        entityPM.UIProperties.SetEnabled("BranchId", "ARInvoice", isAllowedEdit);
        entityPM.UIProperties.SetEnabled("CustomerRef", "ARInvoice", isAllowedEdit);
        entityPM.UIProperties.SetEnabled("SATPaymentMethodCode", "ARInvoice", isAllowedEdit);
        //entityPM.UIProperties.SetEnabled("SalesmanUserId", "ARInvoice", isAllowedEdit);
        entityPM.UIProperties.SetEnabled("BankAccountLiteId", "ARInvoice", isAllowedEdit);
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "Intercompany")) {
            entityPM.UIProperties.SetVisibility("Intercompany", "ARInvoice", true);
        }
        else {
            entityPM.UIProperties.SetVisibility("Intercompany", "ARInvoice", false);
        }
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (Tools_3.AppTool.IsNullOrEmpty(entityPM.SATPaymentMethodCode)) {
                entityPM.UIProperties.SetRequired("SATPaymentMethodCode", "ARInvoice", true);
            }
            if (Tools_3.AppTool.IsNullOrEmpty(entityPM.MetodoPagoCode)) {
                entityPM.UIProperties.SetRequired("MetodoPagoCode", "ARInvoice", true);
            }
        }
    };
    return ARInvoicePMInitService;
}());
exports.ARInvoicePMInitService = ARInvoicePMInitService;
//# sourceMappingURL=ARInvoicePMInitService.js.map