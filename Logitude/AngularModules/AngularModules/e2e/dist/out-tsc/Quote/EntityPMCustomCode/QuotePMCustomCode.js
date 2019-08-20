"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../Tools");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var QuotePMCustomCode = /** @class */ (function () {
    function QuotePMCustomCode() {
    }
    QuotePMCustomCode.ApplyEntityChanged = function (propertyName, entityPM) {
        if (this.CurrentSession.CurrentEditComponent) {
            if (this.CurrentSession.CurrentEditComponent.IsEntityLoaded) {
                switch (propertyName) {
                    case "GrossWeight":
                    case "ChargeableWeight":
                    case "Volume":
                    case "TEU":
                    case "ValueOfGoods":
                        {
                            Tools_1.QuoteTool.OnQuoteQuantitiesChanged(entityPM);
                            break;
                        }
                }
            }
        }
    };
    // Ayman:
    // this Class is not applied (not working)
    // need to applied in the lxml file
    QuotePMCustomCode.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    return QuotePMCustomCode;
}());
exports.QuotePMCustomCode = QuotePMCustomCode;
//# sourceMappingURL=QuotePMCustomCode.js.map