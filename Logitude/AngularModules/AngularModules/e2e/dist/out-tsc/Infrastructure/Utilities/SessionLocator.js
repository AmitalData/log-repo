"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var LocalStorageManager_1 = require("./LocalStorageManager");
var SessionLocator = /** @class */ (function () {
    function SessionLocator() {
    }
    SessionLocator.AddSession = function (mySession) {
        if (SessionLocator.AllSessions == null) {
            SessionLocator.AllSessions = new Array();
        }
        if (SessionLocator.AllSessions.indexOf(mySession) == -1) {
            SessionLocator.AllSessions.push(mySession);
        }
    };
    SessionLocator.StopApplicationTimers = function () {
        for (var key in SessionLocator.TimersSubscribtions) {
            var timerSubscribion = SessionLocator.TimersSubscribtions[key];
            timerSubscribion.unsubscribe();
            //if (subscription != null && !subscription.isUnsubscribed()) {
            //    //subscription.unsubscribe();
            //    console.log("timer stopped");
            //    subscription.Dispose();
            //}
        }
        SessionLocator.TimersSubscribtions = [];
    };
    SessionLocator.ClearExternalParams = function () {
        this.IsExternalParams = false;
    };
    SessionLocator.ClearLocalStorage = function () {
        if (LocalStorageManager_1.LocalStorageManager.GetItem("Token")) {
            window.localStorage.setItem("Token_" + this.Tenant, "");
            window.localStorage.setItem("CardId_" + this.Tenant, "");
            window.localStorage.setItem("Token", "");
            window.localStorage.setItem("CardId", "");
        }
    };
    SessionLocator.GetComputerIdFromStorage = function () {
        return LocalStorageManager_1.LocalStorageManager.GetItem("UserLastLoginComputerID");
    };
    SessionLocator.StoreLogedComputerId = function (computerId) {
        LocalStorageManager_1.LocalStorageManager.SetItem("UserLastLoginComputerID", computerId);
    };
    SessionLocator.SustainFocusOnCell = false;
    SessionLocator.SustainLostFocusOnCell = false;
    SessionLocator.DisableEntityValidation = false;
    SessionLocator.UseCachedData = true;
    SessionLocator.IsExternalParams = false;
    SessionLocator.IsSiguOut = false;
    SessionLocator.DynamicLoader = null;
    SessionLocator.IsProduction = false;
    SessionLocator.ExternalParams = null;
    SessionLocator.Index = 0;
    SessionLocator.Tenant = null;
    SessionLocator.LoggedUserId = null;
    SessionLocator.LocalCurrencyId = null;
    SessionLocator.LocalCurrencyCode = null;
    SessionLocator.AccountingCurrencyId = null;
    SessionLocator.IsNewSignupTenant = false;
    SessionLocator.UserIcons = {};
    SessionLocator.BlockType = null;
    SessionLocator.AllVatTypesGroups = [];
    SessionLocator.FeatureToggles = [];
    SessionLocator.ShowUserNewReleaseToolTip = true;
    SessionLocator.TimersSubscribtions = [];
    SessionLocator.IsMainSidebarCollapsed = false;
    return SessionLocator;
}());
exports.SessionLocator = SessionLocator;
//# sourceMappingURL=SessionLocator.js.map