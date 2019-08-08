"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("./SessionLocator");
var SessionInfo = /** @class */ (function () {
    function SessionInfo() {
    }
    Object.defineProperty(SessionInfo, "LoggedUserId", {
        get: function () { return this.loggedUserId; },
        set: function (newValue) { this.loggedUserId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "LoggedUserTenant", {
        get: function () { return this.loggedUserTenant; },
        set: function (newValue) { this.loggedUserTenant = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "Token", {
        get: function () { return this.token; },
        set: function (newValue) { this.token = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "DocumentDownloadToken", {
        get: function () { return this.documentDownloadToken; },
        set: function (newValue) { this.documentDownloadToken = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "LoggedSessionToken", {
        get: function () { return this.loggedSessionToken; },
        set: function (newValue) { this.loggedSessionToken = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "LoggedUserEmail", {
        get: function () { return this.loggedUserEmail; },
        set: function (newValue) { this.loggedUserEmail = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "SessionTimeout", {
        get: function () { return this.sessionTimeout; },
        set: function (newValue) { this.sessionTimeout = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "WebTokenLifeTimeInMinutes", {
        get: function () { return this.webTokenLifeTimeInMinutes; },
        set: function (newValue) { this.webTokenLifeTimeInMinutes = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "WebTokenExpirationWarningInMinutes", {
        get: function () { return this.webTokenExpirationWarningInMinutes; },
        set: function (newValue) { this.webTokenExpirationWarningInMinutes = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "KeepUserLoggedIn", {
        get: function () { return this.keepUserLoggedIn; },
        set: function (newValue) { this.keepUserLoggedIn = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "LastLoginDateTime", {
        get: function () { return this.lastLoginDateTime; },
        set: function (newValue) { this.lastLoginDateTime = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "LoggedUserPM", {
        get: function () { return this.loggedUserPM; },
        set: function (newValue) {
            if (this.loggedUserPM != newValue) {
                this.loggedUserPM = newValue;
                SessionLocator_1.SessionLocator.LoggedUserPM = newValue;
                if (newValue) {
                    SessionLocator_1.SessionLocator.LoggedUserId = newValue.Id;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "LoggedUserCardId", {
        get: function () { return this.loggedUserCardId; },
        set: function (newValue) { this.loggedUserCardId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SessionInfo, "LoggedUserCardType", {
        get: function () { return this.loggedUserCardType; },
        set: function (newValue) { this.loggedUserCardType = newValue; },
        enumerable: true,
        configurable: true
    });
    return SessionInfo;
}());
exports.SessionInfo = SessionInfo;
//# sourceMappingURL=SessionInfo.js.map