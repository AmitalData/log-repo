"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BusinessProcessQueuePMCustomCode = /** @class */ (function () {
    function BusinessProcessQueuePMCustomCode() {
    }
    BusinessProcessQueuePMCustomCode.ApplyEntityChanged = function (propertyName, entityPM) {
        entityPM.UIProperties.SetEnabled("BusinessRoleId", "BusinessProcessQueue", false);
        entityPM.UIProperties.SetEnabled("ObjectTableId", "BusinessProcessQueue", false);
    };
    return BusinessProcessQueuePMCustomCode;
}());
exports.BusinessProcessQueuePMCustomCode = BusinessProcessQueuePMCustomCode;
//# sourceMappingURL=BusinessProcessQueuePMCustomCode.js.map