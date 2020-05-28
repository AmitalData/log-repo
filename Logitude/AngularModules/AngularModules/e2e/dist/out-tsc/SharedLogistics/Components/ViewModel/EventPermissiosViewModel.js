"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var EventPermissiosViewModel = /** @class */ (function () {
    function EventPermissiosViewModel(zeroEvent, currentEvent) {
        this.entityPM_TenantZero = zeroEvent;
        this.entityPM = currentEvent;
        this.CustomerChooseCheckedBoxId = Guid_1.Guid.newGuid();
        this.CustomerSuggestedCheckedBoxId = Guid_1.Guid.newGuid();
    }
    Object.defineProperty(EventPermissiosViewModel.prototype, "EventTypeName", {
        get: function () { return this.entityPM_TenantZero ? this.entityPM_TenantZero.EnglishName : this.entityPM.EnglishName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EventPermissiosViewModel.prototype, "CustomerSuggestedIsChecked", {
        get: function () { return this.entityPM_TenantZero ? this.entityPM_TenantZero.IsCustomerView : false; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EventPermissiosViewModel.prototype, "AgentSuggestedIsChecked", {
        get: function () { return this.entityPM_TenantZero ? this.entityPM_TenantZero.IsAgentView : false; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EventPermissiosViewModel.prototype, "CustomerChooseIsChecked", {
        get: function () {
            if (this.entityPM) {
                return this.entityPM.IsCustomerView;
            }
            else
                return false;
        },
        set: function (value) {
            if (this.entityPM != null) {
                this.entityPM.IsCustomerView = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EventPermissiosViewModel.prototype, "AgentChooseIsChecked", {
        get: function () {
            if (this.entityPM) {
                return this.entityPM.IsAgentView;
            }
            else
                return false;
        },
        set: function (value) {
            if (this.entityPM != null) {
                this.entityPM.IsAgentView = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return EventPermissiosViewModel;
}());
exports.EventPermissiosViewModel = EventPermissiosViewModel;
//# sourceMappingURL=EventPermissiosViewModel.js.map