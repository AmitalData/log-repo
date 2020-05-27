"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var TariffPMInitService = /** @class */ (function () {
    function TariffPMInitService() {
    }
    TariffPMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.NewConcurrencyGUID = Tools_1.AppTool.GetNewGuid();
            entityPM.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
        }
    };
    TariffPMInitService.ApplyUIPoperties = function (entityPM, isNew) {
        if (entityPM.InActive) {
            entityPM.UIProperties.SetEnabled("Name", "Tariff", false);
            entityPM.UIProperties.SetEnabled("Contract", "Tariff", false);
            entityPM.UIProperties.SetEnabled("SellerId", "Tariff", false);
            entityPM.UIProperties.SetEnabled("CurrencyId", "Tariff", false);
            entityPM.UIProperties.SetEnabled("StartDate", "Tariff", false);
            entityPM.UIProperties.SetEnabled("ExpirationDate", "Tariff", false);
            entityPM.UIProperties.SetEnabled("Description", "Tariff", false);
        }
        else {
            entityPM.UIProperties.SetEnabled("Name", "Tariff", true);
            entityPM.UIProperties.SetEnabled("Contract", "Tariff", true);
            entityPM.UIProperties.SetEnabled("SellerId", "Tariff", true);
            entityPM.UIProperties.SetEnabled("CurrencyId", "Tariff", true);
            entityPM.UIProperties.SetEnabled("StartDate", "Tariff", true);
            entityPM.UIProperties.SetEnabled("ExpirationDate", "Tariff", true);
            entityPM.UIProperties.SetEnabled("Description", "Tariff", true);
        }
    };
    return TariffPMInitService;
}());
exports.TariffPMInitService = TariffPMInitService;
//# sourceMappingURL=TariffPMInitService.js.map