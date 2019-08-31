"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
/// <reference path="../entitypms/airlinepm.ts" />
var FeatureLocator_1 = require("../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var AirlinePMInitService = /** @class */ (function () {
    function AirlinePMInitService() {
    }
    AirlinePMInitService.InitValues = function (entityPM, isNew) {
    };
    AirlinePMInitService.ApplyUIPoperties = function (entityPM, isNew) {
        //entityPM.UIProperties = new UIProperties;
        entityPM.UIProperties.SetVisibility("EnableConsolidationInvoices", "Airline", FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "Consolidation.Constituent"));
        if (SessionLocator_1.SessionLocator.TenantPM.Id != 0) {
            entityPM.UIProperties.SetEnabled("Prefix", "Airline", false);
        }
    };
    return AirlinePMInitService;
}());
exports.AirlinePMInitService = AirlinePMInitService;
//# sourceMappingURL=AirlinePMInitService.js.map