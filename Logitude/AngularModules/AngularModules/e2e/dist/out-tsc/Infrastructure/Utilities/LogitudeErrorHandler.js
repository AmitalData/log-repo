"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var MessageWindow_1 = require("../../Controls/Windows/MessageWindow");
var Guid_1 = require("../Utilities/Guid");
var SessionInfo_1 = require("../Utilities/SessionInfo");
var SessionLocator_1 = require("../Utilities/SessionLocator");
var LogitudeErrorHandler = /** @class */ (function () {
    function LogitudeErrorHandler() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._console = console;
    }
    //private rethrowError = true;
    //constructor(rethrowError?: boolean) {
    //    this._console = console;
    //}
    LogitudeErrorHandler.prototype.handleError = function (error) {
        this.LogError(error);
    };
    LogitudeErrorHandler.prototype.LogError = function (error) {
        this.DisplayErrorMessage(error);
        this.StoreErrorLog(error);
        this._console.error(error);
        this.consoleError(error);
        this.consolePromiseError(error);
    };
    LogitudeErrorHandler.prototype.DisplayErrorMessage = function (error) {
        try {
            if (this.CurrentSession) {
                this.CurrentSession.StopBusyIndicator();
                if (error.message) {
                    var mywindow = new MessageWindow_1.MessageWindow();
                    mywindow.Show(error.message);
                }
            }
        }
        catch (e) { }
    };
    LogitudeErrorHandler.prototype.StoreErrorLog = function (error) {
        try {
            if (error.message && error.stack) {
                var errorLog = new ErrorLogPM();
                errorLog.Id = Guid_1.Guid.newGuid();
                errorLog.ClientDate = new Date();
                errorLog.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                errorLog.Tier = "Client";
                errorLog.UserName = SessionInfo_1.SessionInfo.LoggedUserEmail;
                errorLog.Exception = error.message;
                errorLog.StackTrace = error.stack;
                var rejection = error && error.rejection;
                if (rejection) {
                    if (rejection instanceof Error) {
                        errorLog.StackTrace = rejection.stack;
                    }
                }
                window.sessionStorage.setItem(["ErrorLogs", errorLog.Id], JSON.stringify(errorLog));
            }
        }
        catch (e) {
            console.error(e);
        }
    };
    LogitudeErrorHandler.prototype.consolePromiseError = function (e) {
        try {
            var rejection = e && e.rejection;
            if (rejection) {
                console.error('Unhandled Promise rejection:', rejection instanceof Error ? rejection.message : rejection, '; Zone:', e.zone.name, '; Task:', e.task && e.task.source, '; Value:', rejection, rejection instanceof Error ? rejection.stack : undefined);
            }
        }
        catch (e) { }
    };
    LogitudeErrorHandler.prototype.consoleError = function (error) {
        try {
            var originalError = this._findOriginalError(error);
            var originalStack = this._findOriginalStack(error);
            var context = this._findContext(error);
            this._console.error("EXCEPTION: " + this._extractMessage(error));
            if (originalError) {
                this._console.error("ORIGINAL EXCEPTION: " + this._extractMessage(originalError));
            }
            if (originalStack) {
                this._console.error('ORIGINAL STACKTRACE:');
                this._console.error(originalStack);
            }
            if (context) {
                this._console.error('ERROR CONTEXT:');
                this._console.error(context);
            }
            // We rethrow exceptions, so operations like 'bootstrap' will result in an error
            // when an error happens. If we do not rethrow, bootstrap will always succeed.
            //if (this.rethrowError)
            //   throw error;
        }
        catch (e) { }
    };
    /** @internal */
    LogitudeErrorHandler.prototype._extractMessage = function (error) {
        return error instanceof Error ? error.message : error.toString();
    };
    ;
    /** @internal */
    LogitudeErrorHandler.prototype._findContext = function (error) {
        if (error) {
            return error.context ? Contexting(error) :
                this._findContext(OrginalError(error));
        }
        else {
            return null;
        }
    };
    ;
    /** @internal */
    LogitudeErrorHandler.prototype._findOriginalError = function (error) {
        //while (error && error.originalError) {
        //    error = error.originalError;
        //}
        //return error;
        var e = OrginalError(error);
        while (e && OrginalError(e)) {
            e = OrginalError(e);
        }
        return e;
    };
    ;
    /** @internal */
    LogitudeErrorHandler.prototype._findOriginalStack = function (error) {
        if (!(error instanceof Error))
            return null;
        var e = error;
        var stack = e.stack;
        while (e instanceof Error && OrginalError(e)) {
            e = OrginalError(e);
            if (e instanceof Error && e.stack) {
                stack = e.stack;
            }
        }
        return stack;
    };
    return LogitudeErrorHandler;
}());
exports.LogitudeErrorHandler = LogitudeErrorHandler;
var ErrorLogPM = /** @class */ (function () {
    function ErrorLogPM() {
    }
    return ErrorLogPM;
}());
// https://www.bennadel.com/blog/3138-creating-a-custom-errorhandler-in-angular-2-rc-6.htm
//# sourceMappingURL=LogitudeErrorHandler.js.map