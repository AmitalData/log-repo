"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var UserLoginLogPM = /** @class */ (function () {
    function UserLoginLogPM() {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(UserLoginLogPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserLoginLogPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserLoginLogPM.prototype, "UserId", {
        get: function () { return this.userId; },
        set: function (newValue) { this.userId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserLoginLogPM.prototype, "IP", {
        get: function () { return this.iP; },
        set: function (newValue) { this.iP = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserLoginLogPM.prototype, "Browser", {
        get: function () { return this.browser; },
        set: function (newValue) { this.browser = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserLoginLogPM.prototype, "GMTDateTime", {
        get: function () { return this.gMTDateTime; },
        set: function (newValue) { this.gMTDateTime = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserLoginLogPM.prototype, "LocalDateTime", {
        get: function () { return this.localDateTime; },
        set: function (newValue) { this.localDateTime = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserLoginLogPM.prototype, "ComputerId", {
        get: function () { return this.computerId; },
        set: function (newValue) { this.computerId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserLoginLogPM.prototype, "UserAgent", {
        get: function () { return this.userAgent; },
        set: function (newValue) { this.userAgent = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserLoginLogPM.prototype, "DivSelectBackgroud", {
        get: function () { return this.divSelectBackgroud; },
        set: function (newValue) { this.divSelectBackgroud = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserLoginLogPM.prototype, "IPSiteUri", {
        get: function () { return this.iPSiteUri; },
        set: function (newValue) { this.iPSiteUri = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    UserLoginLogPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
    };
    return UserLoginLogPM;
}());
exports.UserLoginLogPM = UserLoginLogPM;
//# sourceMappingURL=UserLoginLogPM.js.map