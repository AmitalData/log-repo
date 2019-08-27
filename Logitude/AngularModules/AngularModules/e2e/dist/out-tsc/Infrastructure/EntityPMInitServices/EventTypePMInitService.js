"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var EventTypePMInitService = /** @class */ (function () {
    function EventTypePMInitService() {
    }
    EventTypePMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            entityPM.AddedManually = true;
            entityPM.IsManualEntry = true;
        }
    };
    EventTypePMInitService.ApplyUIPoperties = function (entityPM, isNew) {
        if (isNew) {
            entityPM.UIProperties.SetVisibility("InActive", "EventType", false);
        }
        else {
            entityPM.UIProperties.SetEnabled("IsFollowUp", "EventType", entityPM.AddedManually);
            entityPM.UIProperties.SetEnabled("ManualActivatedFollowUp", "EventType", entityPM.AddedManually);
            entityPM.UIProperties.SetEnabled("EntityStatusId", "EventType", entityPM.AddedManually);
        }
    };
    return EventTypePMInitService;
}());
exports.EventTypePMInitService = EventTypePMInitService;
//# sourceMappingURL=EventTypePMInitService.js.map