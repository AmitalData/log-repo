"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Rx_1 = require("rxjs/Rx");
require("rxjs/add/operator/map");
var SessionLocator_1 = require("../Utilities/SessionLocator");
var MessageWindow_1 = require("../../Controls/Windows/MessageWindow");
var DetectUserInActivity = /** @class */ (function () {
    function DetectUserInActivity(isTokenExpiration) {
        if (isTokenExpiration === void 0) { isTokenExpiration = false; }
        this.IsSignout = false;
        this.IsTokenExpiration = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsShowMessageWindow = false;
        this.WarningTimesub = null;
        this.IsStopTimer = false;
        this.IsTokenExpiration = isTokenExpiration;
    }
    //timeUnit // H:Hour , M:Minutes 
    DetectUserInActivity.prototype.Start = function (lifeTime, warningTime, timeUnit) {
        if (warningTime === void 0) { warningTime = 1; }
        if (timeUnit === void 0) { timeUnit = "H"; }
        if (lifeTime && lifeTime > 0) {
            if (timeUnit && timeUnit.toUpperCase() == "H") {
                lifeTime = lifeTime * 60;
            }
            if (!this.IsTokenExpiration) {
                warningTime = lifeTime / 4;
                warningTime = Math.round(warningTime);
            }
            this.LifeTimeInMiliseconds = ((lifeTime - warningTime) * 60000);
            this.WarningTimeInMiliseconds = (warningTime * 60000);
            if (this.LifeTimeInMiliseconds > 2147483647)
                this.LifeTimeInMiliseconds = 2147483647; // 24.8 Days
            if (this.WarningTimeInMiliseconds > 2147483647)
                this.WarningTimeInMiliseconds = 2147483647;
            if (this.WarningTimeInMiliseconds == 0) {
                this.ShowMessage(this);
                this.IsSignout = true;
            }
            this.SetupTimers(this);
        }
    };
    DetectUserInActivity.prototype.SetupTimers = function (viewModeil) {
        document.addEventListener("click", function () { viewModeil.ResetTimer(viewModeil); }, false);
        document.addEventListener("mousedown", function () { viewModeil.ResetTimer(viewModeil); }, false);
        document.addEventListener("keypress", function () { viewModeil.ResetTimer(viewModeil); }, false);
        this.StartTimer(viewModeil);
    };
    DetectUserInActivity.prototype.StartTimer = function (viewModeil, lifeTimeInMiliseconds) {
        if (lifeTimeInMiliseconds === void 0) { lifeTimeInMiliseconds = null; }
        if (!lifeTimeInMiliseconds)
            lifeTimeInMiliseconds = viewModeil.LifeTimeInMiliseconds;
        viewModeil.timeoutId = window.setTimeout(function () { viewModeil.DoInactive(viewModeil); }, lifeTimeInMiliseconds);
    };
    DetectUserInActivity.prototype.DoInactive = function (viewModeil) {
        if (this.CurrentSession == null)
            this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (viewModeil.IsSignout) {
            if (!SessionLocator_1.SessionLocator.IsSiguOut) {
                SessionLocator_1.SessionLocator.IsSiguOut = true;
                var messageWindow = new MessageWindow_1.MessageWindow();
                // messageWindow.Title = "Logitude Message";
                messageWindow.ShowWarningIcon = true;
                messageWindow.IsOverAll = true;
                var message = this.IsTokenExpiration ? "Your session has expired, Please login again" : "Logged out due to inactivity, you can login again to enter the system";
                messageWindow.Show(message);
                SessionLocator_1.SessionLocator.StopApplicationTimers();
                messageWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        SessionLocator_1.SessionLocator.HomeComponent.SignoutClicked();
                        viewModeil.IsSignout = false;
                    }
                });
            }
        }
        else if (this.CurrentSession != null && this.CurrentSession.SessionLocation != null) {
            viewModeil.ShowMessage(viewModeil);
            window.clearTimeout(viewModeil.timeoutId);
            viewModeil.StartTimer(viewModeil, viewModeil.WarningTimeInMiliseconds);
            viewModeil.IsSignout = true;
        }
    };
    DetectUserInActivity.prototype.ShowMessage = function (viewModeil) {
        viewModeil.messageWindow = new MessageWindow_1.MessageWindow();
        // viewModeil.messageWindow.Title = "Logitude Message";
        viewModeil.messageWindow.ShowWarningIcon = true;
        var minutes = viewModeil.WarningTimeInMiliseconds / 60000;
        viewModeil.WarningTimeInMinute = minutes;
        var displayWarningTime = "";
        if ((minutes % 60) == 0) {
            displayWarningTime = (minutes / 60) + " hour";
        }
        else {
            displayWarningTime = minutes + " minute";
        }
        //var displayWarningTime = "";
        //if ((minutes % 60) == 0) {
        //    displayWarningTime = (minutes / 60) + " hour";
        //} else {
        //    var hour: any = minutes / 60;
        //    var minute: any = minutes % 60;
        //    //4 Hour 40 Minute
        //    displayWarningTime = minutes + " minute";
        //}
        var warningMessage = !viewModeil.IsTokenExpiration ? "You will be logged out in " + displayWarningTime + " due to inactivity, unless you continue using the system" : "You will be logged out in " + displayWarningTime + " due to session expiration";
        viewModeil.messageWindow.Show(warningMessage);
        viewModeil.messageWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                viewModeil.messageWindow = null;
                viewModeil.IsStopTimer = true;
                viewModeil.IsShowMessageWindow = false;
                if (!viewModeil.IsTokenExpiration) {
                    viewModeil.IsSignout = false;
                    window.clearTimeout(viewModeil.timeoutId);
                    viewModeil.StartTimer(viewModeil);
                }
            }
        });
        viewModeil.IsShowMessageWindow = true;
        viewModeil.IsStopTimer = false;
        viewModeil.StartWarningTimeTimer(viewModeil);
    };
    DetectUserInActivity.prototype.ResetTimer = function (viewModeil) {
        if (!viewModeil.IsTokenExpiration && !viewModeil.IsShowMessageWindow) {
            viewModeil.IsSignout = false;
            window.clearTimeout(viewModeil.timeoutId);
            viewModeil.StartTimer(viewModeil);
        }
    };
    DetectUserInActivity.prototype.WarningTimeTimer = function () {
        return Rx_1.Observable.interval(60000).timeInterval();
    };
    DetectUserInActivity.prototype.StartWarningTimeTimer = function (viewModeil) {
        var _this = this;
        this.WarningTimesub = this.WarningTimeTimer().subscribe(function (res) {
            if (!_this.IsStopTimer) {
                if (viewModeil.messageWindow) {
                    viewModeil.WarningTimeInMinute = viewModeil.WarningTimeInMinute - 1;
                    if (viewModeil.WarningTimeInMinute > 0) {
                        var warningMessage = !viewModeil.IsTokenExpiration ? "You will be logged out in " + (viewModeil.WarningTimeInMinute).toString() + " minute due to inactivity, unless you continue using the system" : "You will be logged out in " + (viewModeil.WarningTimeInMinute).toString() + " minute due to session expiration";
                        viewModeil.messageWindow.Message = warningMessage;
                    }
                    else
                        viewModeil.IsStopTimer = true;
                }
            }
        });
    };
    return DetectUserInActivity;
}());
exports.DetectUserInActivity = DetectUserInActivity;
//# sourceMappingURL=DetectUserInActivity.js.map